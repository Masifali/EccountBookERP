package com.mst.services.lab;

import com.mst.models.UserAccount;
import com.mst.repositories.lab.InProcessAnalysisRepository;
import com.mst.security.CurrentUserContext;
import com.mst.services.DesktopAttachmentStore;
import com.mst.services.StoreScreenRights;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Base64;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.lab.InProcessAnalysisRepository.str;
import static com.mst.repositories.lab.InProcessAnalysisRepository.toDouble;
import static com.mst.repositories.lab.InProcessAnalysisRepository.toInt;

/**
 * Lab (ModuleId 7) — screen 162 "In-Process Analysis".
 *
 * <pre>
 * Screen id        162 (dbo.ScreenDefinition; TargetUrl Architecture.WinApp.Lab.InvLabAnalysisInProcess)
 * ScreenName       InvLabAnalysisInProcess  (base.Name; rights key — InvLabAnalysisInProcess.cs:275)
 * DocumentTypeId   306                      (:369, :919, :1441, :1588)
 * Route            /quality/inprocess-analysis ; API /api/lab/inprocess-analysis
 * Desktop          Architecture.WinApp.Lab/InvLabAnalysisInProcess.cs (3,812 lines, designer :1934-3812)
 * BLL / DAL        BLL 0397 / DAL 0353 Lab.LabInProcessAnalysisRawHeader (the form never calls BLL 0401
 *                  InvLabAnalysisInProcessHeader, nor 0395 ProcessStep / 0396 InvProcessAnalysisStepSchedule:
 *                  the step schedule arrives through USp_InvLabAnalysisInProcessHeader_GetAllMethod 'GetByPlantId')
 * Models           0614 LabInProcessAnalysisRawHeader, LabInProcessAnalysisRawDetail, InvLabAnalysisInProcessDetail
 * Print            CommonServices.LabAnalysisReport659 (CommonServices.cs:10959) -> 659-RptInvLabInProcessAnalysisSlip.rpt
 *                  (USp_InvLabAnalysisInProcessHeader_Slip + two sub-reports) — the page calls
 *                  /api/print/by-template/659-RptInvLabInProcessAnalysisSlip.rpt/pdf?id=
 * </pre>
 *
 * The desktop save (Insert() :872 -> BLL Save :15 -> DAL SetData :16) in one transaction:
 *   1. USP_LabInProcessAnalysisRawHeader_Insert (Id = 0) or ..._Update (which also deletes both detail sets)
 *   2. Sp_InvLabAnalysisInProcessDetail_Insert   — one per Analysis Group grid row, grid order
 *   3. USP_LabInProcessAnalysisRawDetail_Insert  — one per Step Analysis grid row, grid order
 * The form has NO Delete button and the BLL has no delete method, so this service has none either.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED, NOT CORRECTED (each is the user's call to change)
 * ---------------------------------------------------------------------------------------------
 *  D1  Group detail InvLabGroupAnalysisStandardsId is saved with the ANALYSIS GROUP id (grid column
 *      "AnalysisGroupId" = InvLabGroupAnalysisStandards.InvLabAnalysisGroup, :596 / :942), not the standards
 *      row id; 'BindAnalysisGroupHistory' joins on exactly that, so it is kept.
 *  D2  Every update sends IsApproved = false, ApprovedUser = 0, ApprovedDate = now (:927, BLL :21).
 *  D3  Header AnalysisTime keeps the picker's DATE part (the day the form was opened, or the stored day of
 *      an edited record) — the picker only shows the time (:922); the detail rows' Analysistime is parsed
 *      from the picker TEXT, i.e. today's date + the shown time (:946 / :957).
 *  D4  The "Row No" in the Step Analysis message is GridEXRow.RowIndex + 1, and the grid is grouped by
 *      StepName, so the number counts the group header rows as well (:906).
 *  D5  The Group Analysis message reads a cell "AnalysisParameterCode" the grid does not have (:915; the
 *      column is "AnalysisParameter") — on the desktop that raises an exception whose text is shown instead.
 *      Here the intended text is returned, with the parameter name.
 *  D6  USP_LabInProcessAnalysisRawHeader_Insert recomputes DocNo (MAX+1 per organization/company) and writes
 *      @ModifyUser/@ModifyDate into EntryUser/EntryDate; the Update procedure writes whatever DocNo it is sent.
 *  D7  History with no "CanView AllRecord" right is filtered to the user's own entries (@EntryUser).
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web only)
 * ---------------------------------------------------------------------------------------------
 *  W1  Doc No: new save asks the generator; update keeps the stored number. Never taken from the request.
 *  W2  Open / update / print / pictures / history detail only when the record belongs to the user's
 *      organization and company ('ReadById' and 'BindAnalysisGroupHistory' filter by Id alone).
 *  W3  Posted references must be in the lists the form offers (plant, job order, item of that job order,
 *      crop year, analysis group); every Step row must be a (step, parameter) of the plant's schedule or
 *      of the stored record, every Group row a parameter of the group's standards or of the stored record.
 *  W4  Pictures go through DesktopAttachmentStore (the same configured "Attachment Folder Path" / VPS
 *      storage), inside the transaction; the stored name is "Inv_&lt;uuid&gt;.ext" instead of "&lt;guid:N&gt;.ext".
 *      The desktop copies the file AFTER the database commit (:1013-1038) and deletes a cleared picture's
 *      file (:979-1007); here a storage failure refuses the whole save and old files are not deleted.
 *  W5  An insert that returns no identity is refused (the desktop would go on with header id 0).
 *
 * Not ported: the Attachment dialog (DMS attachments — stored attachments are left untouched), "Add
 * Attachment" and the attachment-count link in History, saved grid layouts / field chooser / grid export
 * (CtrlGrdBar).
 */
@Service
public class InProcessAnalysisService {

    public static final String SCREEN_NAME = "InvLabAnalysisInProcess";
    public static final int SCREEN_ID = 162;
    public static final int DOCUMENT_TYPE_ID = 306;
    public static final String PRINT_RPT = "659-RptInvLabInProcessAnalysisSlip.rpt";

    private static final DateTimeFormatter ISO = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss");
    /** btnimg1_Click :1737 — "(*.jpg; *.jpeg; *.gif; *.PNG; *.png)". */
    private static final Set<String> IMAGE_EXTENSIONS = Set.of("jpg", "jpeg", "gif", "png");

    private final InProcessAnalysisRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;
    private final DesktopAttachmentStore store;

    public InProcessAnalysisService(InProcessAnalysisRepository repo, StoreScreenRights rights,
                                    CurrentUserContext ctx, DesktopAttachmentStore store) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
        this.store = store;
    }

    /** Desktop Yes/No prompt (:889 / :895) — answered by the page and re-posted with confirm = true. */
    public static class ConfirmationRequiredException extends RuntimeException {
        public ConfirmationRequiredException(String message) { super(message); }
    }

    // ======================================================================== form load

    /** InvLabPurchaseAnalysis_Load (:268). */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (!Boolean.TRUE.equals(r.get("view"))) throw new AccessDeniedException("You do not have the View right for In-Process Analysis.");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", r);                                         // :275-278
        out.put("screenId", SCREEN_ID);
        out.put("documentTypeId", DOCUMENT_TYPE_ID);
        out.put("printTemplate", PRINT_RPT);
        putFormLists(u, out, 0);
        out.put("docNo", repo.generateCode(u));                       // GenerateCode :404
        out.put("defaultCropYear", toInt(repo.config(u, "Default Crop Year")));   // :336
        out.put("history", historyCombos(u));                         // HistoryComboFill :348
        out.put("serverNow", LocalDateTime.now().withNano(0).format(ISO));
        return out;
    }

    /** refresh() (:1210) — GenerateCode, the four lists again (RecId is 0 by then), Default Crop Year. */
    public Map<String, Object> formNew() {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", repo.generateCode(u));
        putFormLists(u, out, 0);
        out.put("defaultCropYear", toInt(repo.config(u, "Default Crop Year")));
        return out;
    }

    /** Refresh1() (:1255) — JobOrderNoFill (with the RecId being edited), AnalysisGroup, CropYearFill, PlantFill. */
    public Map<String, Object> formRefresh(int recId) {
        UserAccount u = requireView();
        if (recId != 0) owned(u, recId);
        Map<String, Object> out = new LinkedHashMap<>();
        putFormLists(u, out, recId);
        return out;
    }

    private void putFormLists(UserAccount u, Map<String, Object> out, int recId) {
        out.put("plants", pick(repo.plants(u), "Id", "Description"));                                    // :436
        out.put("jobOrders", pick(repo.jobOrders(u, ctx.currentFinancialYearId(), recId), "Id", "PlanCode"));  // :475
        out.put("analysisGroups", pick(repo.analysisGroups(u), "Id", "AnalysisGroupDescription"));       // :554
        out.put("cropYears", pick(repo.cropYears(u), "Id", "CropYear"));                                 // :528
    }

    /** btnRefreshHistory_Click (:1350). */
    public Map<String, Object> historyCombos() {
        return historyCombos(requireView());
    }

    /** HistoryComboFill (:348) — one procedure call, split by its "Activity" column into four (Id, name) lists. */
    private Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> items = new ArrayList<>(), analysts = new ArrayList<>(),
                jobOrders = new ArrayList<>(), plants = new ArrayList<>();
        for (Map<String, Object> r : repo.historyDropDownData(u, ctx.currentFinancialYearId())) {
            String activity = str(r.get("Activity"));
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("name", str(r.get("ReferenceName")));
            if ("Item".equals(activity)) items.add(o);
            else if ("AnalystPerson".equals(activity)) analysts.add(o);
            else if ("JobOrder".equals(activity)) jobOrders.add(o);
            else if ("Plant".equals(activity)) plants.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", items);
        out.put("analysts", analysts);
        out.put("jobOrders", jobOrders);
        out.put("plants", plants);
        return out;
    }

    // ======================================================================== dependent loads

    /** cmbPlantName_Leave (:660) — the plant's step / parameter schedule, then the last results. */
    public Map<String, Object> plantLeave(int plantId, int jobOrderId) {
        UserAccount u = requireView();
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> s : repo.stepScheduleByPlant(u, plantId)) {          // :680
            rows.add(stepRow(s.get("Id"), s.get("PlantId"), s.get("PlantName"), s.get("SortNo"),
                    s.get("LabInProcessAnalysisStepId"), s.get("ProcessStepName"), s.get("AnalysisParameterId"),
                    s.get("AnalysisParameterCode"), toDouble(s.get("ResultValue")), s.get("RemarkDetail")));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        out.put("top", top(u, plantId, jobOrderId));                                  // :686
        return out;
    }

    /** cmbproductionNo_Leave (:563) — the job order's items, then the last results. */
    public Map<String, Object> jobOrderLeave(int jobOrderId, int plantId) {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", items(u, jobOrderId));
        out.put("top", top(u, plantId, jobOrderId));
        return out;
    }

    /** cmbanalysisGroup_Leave (:576) — the group's analysis standards. */
    public List<Map<String, Object>> analysisGroupLeave(int analysisGroupId) {
        UserAccount u = requireView();
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> g : repo.standardsByAnalysisGroup(u, analysisGroupId)) {   // :596
            rows.add(groupRow(g.get("Id"), g.get("InvLabAnalysisGroup"), g.get("AnalysisGroupDescription"),
                    g.get("InvLabAnalysisItems"), g.get("AnalysisParameterCode"), g.get("MinValue"), g.get("MaxValue"),
                    toDouble(g.get("InAnalysisResult")), g.get("RemarksDetail")));
        }
        return rows;
    }

    /** ItemNameBind (:509) — AllColumns: Id, ItemName, ItemCode; value Id, display ItemName. */
    private List<Map<String, Object>> items(UserAccount u, int jobOrderId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.itemsByJobOrder(u, jobOrderId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("ItemName", str(r.get("ItemName")));
            o.put("ItemCode", str(r.get("ItemCode")));
            out.add(o);
        }
        return out;
    }

    /**
     * TopFiveStepAnalysisResultsAgainstPlantJobOrderAndItem (:755) — the rows the form matches on
     * (StepId, AnalysisParameterId), with Value_01..05, Average and dtm_01..05; the page does the matching
     * and the column captions exactly as the form does.
     */
    private List<Map<String, Object>> top(UserAccount u, int plantId, int jobOrderId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.topResults(u, plantId, jobOrderId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("StepId", toInt(r.get("StepId")));
            o.put("AnalysisParameterId", toInt(r.get("AnalysisParameterId")));
            for (int i = 1; i <= 5; i++) {
                o.put("Value_0" + i, toDouble(r.get("Value_0" + i)));
                o.put("dtm_0" + i, iso(r.get("dtm_0" + i)));
            }
            o.put("Average", toDouble(r.get("Average")));
            out.add(o);
        }
        return out;
    }

    /** GetByPlantdt columns (:305-320), in the form's order. */
    private static Map<String, Object> stepRow(Object id, Object plantId, Object plantName, Object sortNo, Object stepId,
                                               Object stepName, Object parameterId, Object parameter, double result,
                                               Object remarks) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("PlantId", plantId);
        o.put("PlantName", str(plantName));
        o.put("SortNo", sortNo);
        o.put("StepId", toInt(stepId));
        o.put("StepName", str(stepName));
        o.put("AnalysisParameterId", toInt(parameterId));
        o.put("AnalysisParameter", str(parameter));
        o.put("ResultValue", result);
        o.put("Caption1", 0);
        o.put("Caption2", 0);
        o.put("Caption3", 0);
        o.put("Caption4", 0);
        o.put("Caption5", 0);
        o.put("Average", 0);
        o.put("RemarksDetail", str(remarks));
        return o;
    }

    /** GetByAnalysisGroupdt columns (:293-301), in the form's order. */
    private static Map<String, Object> groupRow(Object id, Object groupId, Object groupName, Object parameterId,
                                                Object parameter, Object min, Object max, double result, Object remarks) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("AnalysisGroupId", toInt(groupId));
        o.put("AnalysisGroupName", str(groupName));
        o.put("AnalysisParameterId", toInt(parameterId));
        o.put("AnalysisParameter", str(parameter));
        o.put("MinValue", min);
        o.put("MaxValue", max);
        o.put("ResultValue", result);
        o.put("RemarksDetail", str(remarks));
        return o;
    }

    // ======================================================================== read

    /** ReadById (:1081). */
    public Map<String, Object> load(int id) {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rows = repo.readById(id);
        if (rows.isEmpty()) {                                           // :1092 — the form just returns
            out.put("found", false);
            return out;
        }
        Map<String, Object> h = rows.get(0);
        ownedRow(u, h);
        int jobOrderId = toInt(h.get("JobOrderId"));
        int plantId = toInt(h.get("PlantId"));
        out.put("found", true);
        out.put("Id", id);
        out.put("DocNo", str(h.get("DocNo")));
        out.put("DocDate", iso(h.get("DocDate")));
        out.put("CropYearId", toInt(h.get("CropYearId")));
        out.put("JobOrderId", jobOrderId);
        out.put("AnalysisGroupId", toInt(h.get("AnalysisGroupId")));
        out.put("PlantId", plantId);
        out.put("ItemId", toInt(h.get("ItemId")));
        out.put("AnalysisTime", iso(h.get("AnalysisTime")));
        out.put("RemarksHeader", str(h.get("RemarksHeader")));
        out.put("AnalystPerson", str(h.get("AnalystPerson")));
        out.put("hasAnalysisPic", !str(h.get("AnalysisPic")).isEmpty());   // :1111
        out.put("hasCookingPic", !str(h.get("CookingPic")).isEmpty());     // :1112
        out.put("items", items(u, jobOrderId));                            // :1101

        List<Map<String, Object>> steps = new ArrayList<>();
        for (Map<String, Object> s : repo.stepRowsOfRecord(u, id)) {       // :1168
            steps.add(stepRow(s.get("Id"), s.get("PlantId"), s.get("PlantName"), s.get("SortNo"), s.get("StepId"),
                    s.get("ProcessStepName"), s.get("ParameterId"), s.get("AnalysisParameterCode"),
                    toDouble(s.get("ResultValue")), s.get("RemarkDetail")));
        }
        out.put("stepRows", steps);

        List<Map<String, Object>> groups = new ArrayList<>();
        for (Map<String, Object> g : repo.groupRowsOfRecord(id)) {         // :1187
            groups.add(groupRow(g.get("Id"), g.get("InvLabGroupAnalysisStandardsId"), g.get("AnalysisGroupDescription"),
                    g.get("AnalysisParameterID"), g.get("AnalysisParameterDescription"), g.get("MinValue"),
                    g.get("MaxValue"), toDouble(g.get("InAnalysisResult")), g.get("RemarksDetail")));
        }
        out.put("groupRows", groups);
        out.put("jobOrders", pick(repo.jobOrders(u, ctx.currentFinancialYearId(), id), "Id", "PlanCode"));   // :1201
        out.put("top", top(u, plantId, jobOrderId));                       // :1202
        return out;
    }

    /** sampleanalysispicture / cookingpic of a saved record (:1115-1156) — which = "analysis" | "cooking". */
    public byte[] picture(int id, String which, String[] nameOut) {
        UserAccount u = requireView();
        Map<String, Object> h = owned(u, id);
        String name = str(h.get("cooking".equals(which) ? "CookingPic" : "AnalysisPic"));
        if (name.isEmpty()) throw new IllegalArgumentException("No picture is saved for this record.");
        nameOut[0] = name;
        return store.read(u, name);
    }

    // ======================================================================== history

    /** historygridfill (:1432) — the 22 columns of the form's table, in its order. */
    public List<Map<String, Object>> history(boolean fromChecked, String fromDate, boolean toChecked, String toDate,
                                             String fromDocNo, String toDocNo, int jobOrderId, int plantId,
                                             int itemId, String analyst) {
        UserAccount u = requireView();
        boolean viewAll = rights.has(SCREEN_NAME, "viewAll");                       // :1443
        Timestamp f = fromChecked && !blank(fromDate) ? pickerDate(fromDate) : null;   // :1448
        Timestamp t = toChecked && !blank(toDate) ? pickerDate(toDate) : null;         // :1452
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(u, ctx.currentFinancialYearId(), DOCUMENT_TYPE_ID, viewAll, f, t,
                toInt(fromDocNo), toInt(toDocNo), itemId, plantId, jobOrderId, analyst == null ? "" : analyst)) {
            Map<String, Object> o = new LinkedHashMap<>();                           // :1491
            o.put("RecordNo", r.get("RecordNo"));
            o.put("Id", toInt(r.get("Id")));
            o.put("DocNo", r.get("DocNo"));
            o.put("DocDate", iso(r.get("DocDate")));
            o.put("DocumentTypeId", r.get("DocumentTypeId"));
            o.put("PlantName", str(r.get("PlantName")));
            o.put("JobOrder", str(r.get("JobOrder")));
            o.put("ItemName", str(r.get("ItemName")));
            o.put("CropYear", str(r.get("CropYear")));
            o.put("AnalysisGroup", str(r.get("AnalysisGroupDescription")));
            o.put("AnalystPerson", str(r.get("AnalystPerson")));
            o.put("AnalysisTime", iso(r.get("AnalysisTime")));
            o.put("EntryDate", iso(r.get("EntryDate")));
            o.put("EntryUser", str(r.get("UserName")));
            o.put("ModifyDate", iso(r.get("ModifyDate")));
            o.put("ModifyUser", str(r.get("ModifyUserName")));
            o.put("AnalysisPic", str(r.get("AnalysisPic")).isEmpty() ? 0 : 1);
            o.put("CookingPic", str(r.get("CookingPic")).isEmpty() ? 0 : 1);
            o.put("NoOfAttachments", toInt(r.get("NoOfAttachments")));
            o.put("RemarksHeader", str(r.get("RemarksHeader")));
            out.add(o);
        }
        return out;
    }

    /** grdhistory_SelectionChanged (:1374) — HistoryStepAnalysisGridFill (:1600) + HistoryGroupAnalysisGridFill (:1639). */
    public Map<String, Object> historyDetail(int id) {
        UserAccount u = requireView();
        owned(u, id);
        List<Map<String, Object>> steps = new ArrayList<>();
        for (Map<String, Object> s : repo.stepRowsOfRecord(u, id)) {
            Map<String, Object> o = new LinkedHashMap<>();        // procedure column order; Id, PlantId, StepId, ParameterId, SortNo hidden
            o.put("PlantName", str(s.get("PlantName")));
            o.put("ProcessStepName", str(s.get("ProcessStepName")));
            o.put("AnalysisParameterCode", str(s.get("AnalysisParameterCode")));
            o.put("ResultValue", s.get("ResultValue"));
            o.put("RemarkDetail", str(s.get("RemarkDetail")));
            steps.add(o);
        }
        List<Map<String, Object>> groups = new ArrayList<>();
        for (Map<String, Object> g : repo.groupRowsOfRecord(id)) {
            Map<String, Object> o = new LinkedHashMap<>();        // Id, InvLabGroupAnalysisStandardsId, AnalysisParameterID hidden
            o.put("AnalysisGroupDescription", str(g.get("AnalysisGroupDescription")));
            o.put("AnalysisParameterDescription", str(g.get("AnalysisParameterDescription")));
            o.put("MinValue", g.get("MinValue"));
            o.put("MaxValue", g.get("MaxValue"));
            o.put("InAnalysisResult", g.get("InAnalysisResult"));
            o.put("RemarksDetail", str(g.get("RemarksDetail")));
            groups.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("steps", steps);
        out.put("groups", groups);
        return out;
    }

    /**
     * grdhistory_LinkClicked "NoOfAttachments" (:1399) -> CommonServices.GetNoofAttachmentsByRefDocumentTypeID
     * (CommonServices.cs:4552): AttachmentName, CustomName, EntryDate of the record's DMS attachments
     * (RefDocumentTypeId 306). An empty list opens nothing on the desktop.
     */
    public List<Map<String, Object>> attachments(int id) {
        UserAccount u = requireView();
        owned(u, id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.attachments(id, DOCUMENT_TYPE_ID)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("AttachmentName", str(r.get("Attachment")));
            o.put("CustomName", str(r.get("UploadedFileCustomName")));
            o.put("EntryDate", iso(r.get("EntryDate")));
            out.add(o);
        }
        return out;
    }

    // ======================================================================== print

    /** CommonServices.LabAnalysisReport659 (:10959) — "No Record Found For Display" when there is no id. */
    public Map<String, Object> checkPrint(int id) {
        UserAccount u = requireView();
        if (!rights.has(SCREEN_NAME, "print")) throw new AccessDeniedException("You do not have the Print right for this screen.");
        if (id == 0) throw new IllegalArgumentException("No Record Found For Display");
        List<Map<String, Object>> rows = repo.readById(id);
        if (rows.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
        ownedRow(u, rows.get(0));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("ok", true);
        out.put("template", PRINT_RPT);
        return out;
    }

    // ======================================================================== save

    /**
     * Insert() (:872) behind btnSave_Click (RecId = 0, :1060) and btnUpdate_Click (:1069); BLL Save (:15);
     * DAL SetData (:16) — one transaction.
     */
    @Transactional
    @SuppressWarnings("unchecked")
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = ctx.requireAccountingUser();
        if (body == null) throw new IllegalArgumentException("Grid Record Not Found");
        int recId = toInt(body.get("id"));
        if (!rights.has(SCREEN_NAME, "view")) throw new AccessDeniedException("You do not have the View right for In-Process Analysis.");
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) throw new AccessDeniedException("You do not have the Save right for this screen.");      // :276
        if (recId != 0 && !rights.has(SCREEN_NAME, "update")) throw new AccessDeniedException("You do not have the Update right for this screen.");  // :278

        int plantId = toInt(body.get("plantId"));
        int jobOrderId = toInt(body.get("jobOrderId"));
        int itemId = toInt(body.get("itemId"));
        int cropYearId = toInt(body.get("cropYearId"));
        int analysisGroupId = toInt(body.get("analysisGroupId"));
        String analyst = str(body.get("analystPerson"));
        String remarks = str(body.get("remarksHeader"));

        // ---- formvalidation (:831), desktop order and text
        if (plantId == 0) throw new IllegalArgumentException("Please Select Plant Name");
        if (jobOrderId == 0) throw new IllegalArgumentException("Please Select Job Order No");
        if (itemId == 0) throw new IllegalArgumentException("Please Select Item");
        if (cropYearId == 0) throw new IllegalArgumentException("Please Select Crop Year");
        if (analysisGroupId == 0) throw new IllegalArgumentException("Please Select Analysis Group");
        if (analyst.trim().isEmpty()) throw new IllegalArgumentException("Please Enter Analyst Person");

        // ---- Yes/No (:889 / :895)
        if (!truthy(body.get("confirm"))) {
            throw new ConfirmationRequiredException(recId > 0 ? "Are you sure to Update?" : "Are you sure to Save?");
        }

        List<Map<String, Object>> groupRows = rowsOf(body.get("groupRows"));
        List<Map<String, Object>> stepRows = rowsOf(body.get("stepRows"));

        // ---- :899 — both grids must have rows; :901-918 — every ResultValue filled
        if (groupRows.isEmpty() || stepRows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");   // :1047
        int rowIndex = 0;                                   // GridEXRow.RowIndex: the StepName group headers count (D4)
        String previousStep = null;                         // grdStepAnalysis.RootTable.Groups.Add("StepName") :718
        for (Map<String, Object> r : stepRows) {
            String stepName = str(r.get("StepName"));
            if (!stepName.equals(previousStep)) { rowIndex++; previousStep = stepName; }
            if (str(r.get("ResultValue")).trim().isEmpty()) {
                throw new IllegalArgumentException("PLease Enter Result Value In Step Analysis Grid...    In Row No " + (rowIndex + 1));
            }
            rowIndex++;
        }
        for (int i = 0; i < groupRows.size(); i++) {
            Map<String, Object> r = groupRows.get(i);
            if (str(r.get("ResultValue")).trim().isEmpty()) {
                throw new IllegalArgumentException("PLease Enter Result Value In Group Analysis Grid...    In Row No "
                        + (i + 1) + " " + str(r.get("AnalysisParameter")));       // D5
            }
        }

        // ---- W2 / W3: the record and every reference belong to this company and to the form's lists
        Map<String, Object> existing = recId == 0 ? null : owned(u, recId);
        int fy = ctx.currentFinancialYearId();
        requireIn(repo.plants(u), plantId, "Plant Name");
        requireIn(repo.jobOrders(u, fy, recId), jobOrderId, "Job Order No");
        requireIn(repo.itemsByJobOrder(u, jobOrderId), itemId, "Item");
        requireIn(repo.cropYears(u), cropYearId, "Crop Year");
        requireIn(repo.analysisGroups(u), analysisGroupId, "Analysis Group");

        Set<String> allowedSteps = new HashSet<>();
        for (Map<String, Object> s : repo.stepScheduleByPlant(u, plantId)) {
            allowedSteps.add(toInt(s.get("LabInProcessAnalysisStepId")) + ":" + toInt(s.get("AnalysisParameterId")));
        }
        Set<Integer> allowedParameters = new HashSet<>();
        for (Map<String, Object> g : repo.standardsByAnalysisGroup(u, analysisGroupId)) {
            allowedParameters.add(toInt(g.get("InvLabAnalysisItems")));
        }
        if (recId != 0) {
            for (Map<String, Object> s : repo.stepRowsOfRecord(u, recId)) {
                allowedSteps.add(toInt(s.get("StepId")) + ":" + toInt(s.get("ParameterId")));
            }
            for (Map<String, Object> g : repo.groupRowsOfRecord(recId)) {
                if (toInt(g.get("InvLabGroupAnalysisStandardsId")) == analysisGroupId) allowedParameters.add(toInt(g.get("AnalysisParameterID")));
            }
        }
        for (Map<String, Object> r : stepRows) {
            if (!allowedSteps.contains(toInt(r.get("StepId")) + ":" + toInt(r.get("AnalysisParameterId")))) {
                throw new IllegalArgumentException("A Step Analysis row does not belong to the selected plant's step schedule. Select the plant again.");
            }
        }
        for (Map<String, Object> r : groupRows) {
            if (toInt(r.get("AnalysisGroupId")) != analysisGroupId || !allowedParameters.contains(toInt(r.get("AnalysisParameterId")))) {
                throw new IllegalArgumentException("An Analysis Group row does not belong to the selected analysis group. Select the analysis group again.");
            }
        }

        // ---- header (:919-935) — Model 0614 property order, as SetProc sends it
        LocalDateTime nowLdt = LocalDateTime.now().withNano(0);
        Timestamp now = Timestamp.valueOf(nowLdt);                            // BLL :19-21
        LocalDateTime analysisLdt = pickerDateTime(str(body.get("analysisTime")), nowLdt);   // txtanalysistime.Value (:922)
        Timestamp analysisTime = Timestamp.valueOf(analysisLdt);
        // Conversion.ToDateTime(txtanalysistime.Text) (:946 / :957) — the text is the time only: today + time (D3)
        Timestamp detailTime = Timestamp.valueOf(LocalDateTime.of(LocalDate.now(), analysisLdt.toLocalTime()));
        Timestamp docDate = pickerDate(blank(str(body.get("docDate"))) ? LocalDate.now().toString() : str(body.get("docDate")));   // :921

        int docNo = recId == 0 ? repo.generateCode(u) : toInt(existing.get("DocNo"));      // W1
        String analysisPic = pictureName(u, body.get("analysisPic"), existing, "AnalysisPic");   // :963
        String cookingPic = pictureName(u, body.get("cookingPic"), existing, "CookingPic");      // :964

        Map<String, Object> h = new LinkedHashMap<>();
        h.put("IsApproved", false);                       // :927
        h.put("AnalysisTime", analysisTime);              // :922
        h.put("ApprovedDate", now);                       // BLL :21
        h.put("DocDate", docDate);                        // :921
        h.put("EntryDate", now);                          // BLL :19
        h.put("ModifyDate", now);                         // BLL :20
        h.put("ApprovedUser", 0);                         // never set
        h.put("CompanyId", u.getCompanyId());             // :932
        h.put("DocumentTypeId", DOCUMENT_TYPE_ID);        // :919
        h.put("AnalysisGroupId", analysisGroupId);        // :929
        h.put("CropYearId", cropYearId);                  // :926
        h.put("DocNo", docNo);                            // :920
        h.put("EntryUser", u.getId());                    // :933
        h.put("Id", recId);                               // :893
        h.put("FinancialYearId", fy);                     // :935
        h.put("ItemId", itemId);                          // :928
        h.put("JobOrderId", jobOrderId);                  // :924
        h.put("ModifyUser", u.getId());                   // :934
        h.put("OrganizationId", u.getOrganizationId());   // :931
        h.put("PlantId", plantId);                        // :923
        h.put("AnalysisPic", analysisPic);                // :963
        h.put("AnalystPerson", analyst);                  // :925
        h.put("CookingPic", cookingPic);                  // :964
        h.put("RemarksHeader", remarks);                  // :930

        // ---- DAL SetData (:46-63)
        int returned = repo.saveHeader(recId == 0, h);
        int headerId = returned > 0 ? returned : recId;                       // DAL :47-54
        if (headerId <= 0) throw new IllegalStateException("The In-Process Analysis header was not saved (no id returned).");   // W5

        for (Map<String, Object> r : groupRows) {                             // :939-948, DAL :55-59
            repo.insertGroupDetail(headerId, detailTime, toDouble(r.get("ResultValue")),
                    toInt(r.get("AnalysisParameterId")), toInt(r.get("AnalysisGroupId")), str(r.get("RemarksDetail")));
        }
        for (Map<String, Object> r : stepRows) {                              // :950-959, DAL :60-64
            repo.insertStepDetail(headerId, detailTime, toDecimal(r.get("ResultValue")),
                    toInt(r.get("AnalysisParameterId")), toInt(r.get("StepId")), str(r.get("RemarksDetail")));
        }

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", headerId);
        out.put("message", recId > 0 ? "Update Successfully" : "Save Successfully");   // :977 / :1011
        return out;
    }

    /**
     * labheader.AnalysisPic = fileSavePath (:963): the stored name when the picture was left alone, a new
     * stored name when one was browsed, "" when it was cleared or never chosen.
     * Request shape: {state: "keep" | "new" | "none", fileName, data (base64)}.
     */
    @SuppressWarnings("unchecked")
    private String pictureName(UserAccount u, Object posted, Map<String, Object> existing, String column) {
        if (!(posted instanceof Map)) return "";
        Map<String, Object> p = (Map<String, Object>) posted;
        String state = str(p.get("state"));
        if ("keep".equals(state)) return existing == null ? "" : str(existing.get(column));
        if (!"new".equals(state)) return "";
        String fileName = str(p.get("fileName")).trim();
        int dot = fileName.lastIndexOf('.');
        String ext = dot < 0 ? "" : fileName.substring(dot + 1).toLowerCase(Locale.ROOT);
        if (!IMAGE_EXTENSIONS.contains(ext)) {
            throw new IllegalArgumentException("Select an image (*.jpg; *.jpeg; *.gif; *.png)");
        }
        byte[] bytes;
        try {
            String data = str(p.get("data"));
            int comma = data.indexOf(',');
            if (data.startsWith("data:") && comma > 0) data = data.substring(comma + 1);
            bytes = Base64.getDecoder().decode(data.trim());
        } catch (IllegalArgumentException e) {
            throw new IllegalArgumentException("The selected image could not be read.");
        }
        return store.store(u, fileName, bytes);             // W4
    }

    // ======================================================================== helpers

    private UserAccount requireView() {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "view")) throw new AccessDeniedException("You do not have the View right for In-Process Analysis.");
        return u;
    }

    /** W2 — 'ReadById' filters by Id alone. */
    private Map<String, Object> owned(UserAccount u, int id) {
        List<Map<String, Object>> rows = repo.readById(id);
        if (rows.isEmpty()) throw new IllegalArgumentException("Record not found.");
        ownedRow(u, rows.get(0));
        return rows.get(0);
    }

    private static void ownedRow(UserAccount u, Map<String, Object> h) {
        if (toInt(h.get("OrganizationId")) != toInt(u.getOrganizationId()) || toInt(h.get("CompanyId")) != toInt(u.getCompanyId())) {
            throw new AccessDeniedException("This record does not belong to your company.");
        }
    }

    private static void requireIn(List<Map<String, Object>> list, int id, String caption) {
        for (Map<String, Object> r : list) if (toInt(r.get("Id")) == id) return;
        throw new IllegalArgumentException("The selected " + caption + " is not in the " + caption + " list. Refresh and select it again.");
    }

    /** (ValueMember, DisplayMember) rows of a DropDownBind.BindDDL / BindDDLNew combo. */
    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String valueMember, String displayMember) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put(valueMember, r.get(valueMember));
            o.put(displayMember, str(r.get(displayMember)));
            out.add(o);
        }
        return out;
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> rowsOf(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) {
            for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        }
        return out;
    }

    private static boolean truthy(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        String s = str(v).trim();
        return "true".equalsIgnoreCase(s) || "1".equals(s);
    }

    private static boolean blank(String s) { return s == null || s.trim().isEmpty(); }

    /** Conversion.ToDecimal — 0 for anything unreadable. */
    private static BigDecimal toDecimal(Object v) {
        try {
            String s = str(v).trim().replace(",", "");
            return s.isEmpty() ? BigDecimal.ZERO : new BigDecimal(s);
        } catch (NumberFormatException e) {
            return BigDecimal.ZERO;
        }
    }

    /** A DateTimePicker date: the chosen day with the current time of day (picker.Value). */
    private static Timestamp pickerDate(String day) {
        try {
            return Timestamp.valueOf(LocalDateTime.of(LocalDate.parse(day.trim().substring(0, 10)), LocalTime.now().withNano(0)));
        } catch (RuntimeException e) {
            throw new IllegalArgumentException("Invalid date: " + day);
        }
    }

    /** "yyyy-MM-ddTHH:mm[:ss]" from the page; the fallback when nothing usable was posted. */
    private static LocalDateTime pickerDateTime(String text, LocalDateTime fallback) {
        if (blank(text)) return fallback;
        try {
            String s = text.trim().replace(' ', 'T');
            if (s.length() == 16) s += ":00";
            return LocalDateTime.parse(s.substring(0, 19)).withNano(0);
        } catch (RuntimeException e) {
            throw new IllegalArgumentException("Invalid Analysis Time: " + text);
        }
    }

    private static String iso(Object v) {
        if (v == null) return "";
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).format(ISO);
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().atStartOfDay().format(ISO);
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).format(ISO);
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).format(ISO);
        if (v instanceof LocalDate) return ((LocalDate) v).atStartOfDay().format(ISO);
        return String.valueOf(v);
    }
}
