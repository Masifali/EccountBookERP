package com.mst.services.lab;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryPosItemRequest;
import com.mst.repositories.lab.SaleAnalysisRepository;
import com.mst.security.CurrentUserContext;
import com.mst.services.DesktopAttachmentStore;
import com.mst.services.StoreScreenRights;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.MathContext;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.Base64;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

/**
 * Lab (ModuleId 7) — screen 161 "Sale Analysis".
 *
 * <pre>
 * Screen id        161 (dbo.ScreenDefinition; Architecture.WinApp.Lab.InvLabSaleAnalysis)
 * ScreenName       InvLabSaleAnalysis              (base.Name; rights key — InvLabSaleAnalysis.cs:225)
 * DocumentTypeId   304                             (:615)
 * Route            /quality/sale-analysis ; API /api/lab/sale-analysis
 * Desktop          Architecture.WinApp.Lab/InvLabSaleAnalysis.cs (2,301 lines, designer :1224-2301)
 * BLL / DAL        BLL 0405 / DAL 0360 Lab.InvLabAnalysisSaleHeader; BLL 0568 GatePassOutward;
 *                  BLL 0583 Item; BLL 0400 InvLabAnalysisGroup; BLL 0407 InvLabSampleAnalysisHeader;
 *                  BLL 0571 InvCropYear; BLL 0069 DMSAttachments
 * Models           0623 InvLabAnalysisSaleHeader, 0622 InvLabAnalysisSaleDetail, DMSAttachments
 * Print            658-RptInvLabSaleAnalysisSlip.rpt (:1154) over SP_InvLabAnalysisSale_Slip_Rpt
 * </pre>
 *
 * The form has NO Delete and no approve action. Save chain (DAL 0360 SetData, one transaction):
 * Sp_InvLabAnalysisSaleHeader_Insert | _Update (the Update procedure deletes the detail rows),
 * Sp_InvLabAnalysisSaleDetail_Insert per grid row, Proc_DMSAttachments_Insert per attachment; before
 * it, Sp_DMSAttachments_GetAllMethod 'DeleteById' for the record id and this screen (:652).
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED, NOT CORRECTED (each is the user's call to change)
 * ---------------------------------------------------------------------------------------------
 *  D1  The form never sets EntryUser, SupplierCustomerId, BranchesId, ProjectsId, DeductionRate,
 *      DeductionWeight or ContainerNo2 (:612-634): they are saved as 0 / not sent, on insert and update.
 *      With EntryUser 0, a user WITHOUT the "Can View All Record" right sees no history rows (the
 *      procedure filters EntryUser = the user), and the history's EntryUser column is empty.
 *  D2  Update sends IsApproved 0 / ApprovedUserId 0 / ApprovedDate now — the Update procedure writes them.
 *  D3  The message after a save is "Record Update Successfully [id] " for a new record too (:663 —
 *      Save returns the id, which is always &gt; 0).
 *  D4  Crop is saved as the combo's TEXT (:623).
 *  D5  Neither the item nor the analysed bags are validated; an empty item is saved as ItemId 0.
 *  D6  A gate pass GetByGpNo(91, id) does not answer for (another document type / financial year, or a
 *      GDN already refers to it) leaves the hidden gate pass id empty: the record is saved with
 *      GatePassOutwardId 0 and an empty Customer Code (:324-329).
 *  D7  The slip procedure only returns parameters whose result is &gt; 0; a record with no positive
 *      result prints "Not Record Found For Display".
 *  D8  ReadById is header INNER JOIN detail (and standards / parameter / group): a record without
 *      detail rows cannot be opened.
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web only)
 * ---------------------------------------------------------------------------------------------
 *  W1  Document No: on a new save the server asks the generator again, on update it keeps the stored
 *      number; it is never taken from the request.
 *  W2  Open / update / print only when the record belongs to the user's organization and company
 *      (ReadById filters by Id alone).
 *  W3  Posted references must be ones the form offers: the gate pass (new: the open gate pass list),
 *      the item (the gate pass's item list, or the record's own), the analysis group (the group list)
 *      and every grid row's standard id (the group's parameters, or the record's stored rows). The
 *      hidden boxes (gate pass id, customer code, sale order id, sample id) are read by the server:
 *      from GetByGpNo on a new record, from the stored record on update (the gate pass box is
 *      disabled once a record is opened, :741).
 *  W4  Opening a record also shows its stored Doc Date (the desktop leaves the picker untouched, so an
 *      update rewrites the document date with whatever the picker shows — normally today).
 *  W5  Pictures / attachments are uploaded through DesktopAttachmentStore (the configured attachment
 *      folder / VPS) instead of File.Copy from the workstation; the stored name is the store's. A
 *      stored picture is kept on update without the desktop's File.Exists test. Pressing New drops a
 *      browsed / loaded picture (the desktop keeps the old path variables and would write the previous
 *      record's picture path into the next record).
 *  W6  Attachments: the desktop deletes all stored attachments of the record on every update and
 *      inserts only what its (application-wide, static) attachment list holds. Here the record's stored
 *      attachments are listed when it is opened and are re-inserted unless the user removes them.
 *  W7  "Preview" after a save prints the saved record (the desktop calls GenerateReport(RecId) with
 *      RecId 0 after a new save, which prints every slip of the company). Printing with no record
 *      opened is refused with "Not Record Found For Display" for the same reason.
 *  W8  "Grid Record Not Found" leaves the form as it is (the desktop still runs New() and clears it).
 *  W9  The history "Detail" button fills only the history detail grid (on the desktop both grids share
 *      one DataTable, so it also replaces the rows of the form's grid).
 */
@Service
public class SaleAnalysisService {

    private static final Logger LOG = LoggerFactory.getLogger(SaleAnalysisService.class);

    public static final String SCREEN_NAME = "InvLabSaleAnalysis";
    public static final int SCREEN_ID = 161;
    public static final int DOCUMENT_TYPE_ID = 304;
    public static final String RPT_658 = "658-RptInvLabSaleAnalysisSlip.rpt";
    /** CommonServices.GetByGpNo(91, …) (:312). */
    private static final int GP_DOCUMENT_TYPE_ID = 91;
    /** historygridfill (:869). */
    private static final int HISTORY_RECORDS = 50;

    private final SaleAnalysisRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;
    private final DesktopAttachmentStore store;

    public SaleAnalysisService(SaleAnalysisRepository repo, StoreScreenRights rights, CurrentUserContext ctx,
                               DesktopAttachmentStore store) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
        this.store = store;
    }

    /** A desktop Yes/No prompt the page has not answered yet (HTTP 409 {confirm:true,message}). */
    public static class ConfirmRequired extends RuntimeException {
        public ConfirmRequired(String message) { super(message); }
    }

    // ======================================================================== form load

    /** InvLabPurchaseAnalysis_Load (:221): ItemFill, GenerateCode, AnalysisGroup, historygridfill, GatepassNofill, CropYearFill. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (!Boolean.TRUE.equals(r.get("view"))) throw new AccessDeniedException("You do not have the View right for Sale Analysis.");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", r);
        out.put("screenId", SCREEN_ID);
        out.put("documentTypeId", DOCUMENT_TYPE_ID);
        out.put("attachmentPathSet", attachmentPathSet(u));
        out.put("items", items(0));
        out.put("docNo", repo.generateDocNo(u));
        out.put("analysisGroups", analysisGroups(u));
        out.put("history", historyRows(u));
        out.put("gatePasses", gatePasses(u));
        out.put("cropYears", cropYears(u));
        return out;
    }

    /** Refresh1 (:554): AnalysisGroup, historygridfill, GatepassNofill, CropYearFill (the page then runs the gate pass Leave). */
    public Map<String, Object> formRefresh() {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("analysisGroups", analysisGroups(u));
        out.put("history", historyRows(u));
        out.put("gatePasses", gatePasses(u));
        out.put("cropYears", cropYears(u));
        return out;
    }

    /** New (:518): GenerateCode, ItemFill (the gate pass box was just emptied), CropYearFill. */
    public Map<String, Object> formNew() {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", repo.generateDocNo(u));
        out.put("items", items(0));
        out.put("cropYears", cropYears(u));
        return out;
    }

    // ============================================================================ lists

    /** GatepassNofill (:270): Id, GpSrNo, VehicleNo. */
    private List<Map<String, Object>> gatePasses(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.gatePasses(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("GpSrNo", str(ci(r, "GpSrNo")));
            o.put("VehicleNo", str(ci(r, "VehicleNo")));
            out.add(o);
        }
        return out;
    }

    /** ItemFill (:451): Id, ItemName of USP_GetItemsFromDoAndSoByGpId. */
    private List<Map<String, Object>> items(int gpId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.itemsByGatePass(gpId)) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "ItemName"))));
        return out;
    }

    /** AnalysisGroup (:379): Id, AnalysisGroupDescription. */
    private List<Map<String, Object>> analysisGroups(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.analysisGroups(u)) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "AnalysisGroupDescription"))));
        return out;
    }

    /** CropYearFill (:470): Id, CropYear. */
    private List<Map<String, Object>> cropYears(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.cropYears(u)) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "CropYear"))));
        return out;
    }

    /** cmbanalysisgroup_Leave (:412): the rows whose InvLabAnalysisGroup is the chosen group (:430). */
    private List<Map<String, Object>> parameterRows(UserAccount u, int groupId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.parametersByGroup(u, groupId)) {
            if (toInt(ci(r, "InvLabAnalysisGroup")) != groupId) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("LabstandardanalysisId", toInt(ci(r, "labgroupstandardId")));
            o.put("AnalysisPerameter", str(ci(r, "AnalysisParameterDescription")));
            o.put("Min", dbText(ci(r, "MinValue")));
            o.put("MaxValue", dbText(ci(r, "MaxValue")));
            o.put("AnalysisResult", "");
            o.put("Remarks", "");
            out.add(o);
        }
        return out;
    }

    // ===================================================================== form events

    /** txtgatepassno_Leave (:305): ItemFill for the gate pass, then CommonServices.GetByGpNo(91, id). */
    public Map<String, Object> gatePass(int gatePassId) {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", items(gatePassId));
        List<Map<String, Object>> rows = gatePassId <= 0 ? new ArrayList<Map<String, Object>>()
                : repo.gatePassByGpNo(u, GP_DOCUMENT_TYPE_ID, gatePassId, ctx.currentFinancialYearId());
        out.put("found", !rows.isEmpty());
        if (!rows.isEmpty()) {
            Map<String, Object> g = rows.get(0);
            out.put("SupplierContractCode", str(ci(g, "SupplierContractCode")));
            out.put("BiltyNo", str(ci(g, "BiltyNo")));
            out.put("VehicleNo", str(ci(g, "VehicleNo")));
            out.put("Container", str(ci(g, "Container")));
        }
        return out;
    }

    public List<Map<String, Object>> parameters(int groupId) {
        UserAccount u = requireView();
        return parameterRows(u, groupId);
    }

    // ========================================================================== history

    /** historygridfill (:860): NoOfRecords 50, CanViewAllRecord from the screen rights. */
    public List<Map<String, Object>> history() {
        UserAccount u = requireView();
        return historyRows(u);
    }

    private List<Map<String, Object>> historyRows(UserAccount u) {
        boolean viewAll = rights.has(SCREEN_NAME, "viewAll");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(u, HISTORY_RECORDS, viewAll, nz(u.getId()))) {
            Map<String, Object> o = new LinkedHashMap<>();                                           // :886
            o.put("Id", toInt(ci(r, "Id")));
            o.put("DocNo", str(ci(r, "DocNo")));
            o.put("AnalysisItem", str(ci(r, "ItemName")));
            o.put("AnlysisGroup", str(ci(r, "AnalysisGroupDescription")));
            o.put("GatePass#", str(ci(r, "GpSrNo")));
            o.put("CustomerCode", str(ci(r, "SupCustCode")));
            o.put("BiltyNo", str(ci(r, "BiltyNo")));
            o.put("VehicleNo", str(ci(r, "VehicleNo")));
            o.put("CropYear", str(ci(r, "Crop")));
            o.put("EntryUser", str(ci(r, "UserNameEusr")));
            o.put("Remarks", str(ci(r, "RemarksHeader")));
            out.add(o);
        }
        return out;
    }

    // ============================================================================= read

    /** ReadById (:714) — also what the history "Detail" button reads (:820). */
    public Map<String, Object> load(int id) {
        UserAccount u = requireView();
        List<Map<String, Object>> rows = ownedRows(u, id);
        if (rows == null) throw new IllegalArgumentException("Record Not Found");
        Map<String, Object> h = rows.get(0);
        int gpId = toInt(ci(h, "GatePassOutwardId"));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", id);
        out.put("DocNo", str(ci(h, "SAHDocNo")));
        out.put("DocDate", day(ci(h, "SAHDocDate")));                                                // W4
        out.put("BiltyNo", str(ci(h, "BiltyNo")));
        out.put("VehicleNo", str(ci(h, "VehicleNo")));
        out.put("GpSrNo", str(ci(h, "GpSrNo")));
        out.put("GatePassOutwardId", gpId);
        out.put("SupCustCode", str(ci(h, "SAHSupCustCode")));
        out.put("IsAccepted", toBool(ci(h, "SAHIsAccepted")));
        out.put("ItemId", toInt(ci(h, "SAHItmId")));
        out.put("ItemName", str(ci(h, "ItemName")));
        out.put("CropYear", str(ci(h, "SAHCropYear")));
        out.put("ContainerNo1", str(ci(h, "ContainerNo1")));
        out.put("AnalysisGroupId", toInt(ci(h, "SAHAnalysisGroup")));
        out.put("AnalysisGroupDescription", str(ci(h, "AnalysisGroupDescription")));
        out.put("RemarksHeader", str(ci(h, "RemarksHeader")));
        out.put("AnalystName", str(ci(h, "AnaLystName")));
        out.put("AnalyzedBags", netText(ci(h, "AnalyzedBags")));
        out.put("HasAnalysisPic", !str(ci(h, "SAHAnalysisPic")).trim().isEmpty());
        out.put("HasCookingPic", !str(ci(h, "SAHCookingPic")).trim().isEmpty());
        out.put("rows", storedRows(rows));
        out.put("items", items(gpId));
        out.put("attachments", attachmentRows(id));
        return out;
    }

    /** The grid rows ReadById builds (:786-789). */
    private static List<Map<String, Object>> storedRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("LabstandardanalysisId", toInt(ci(r, "InvLabGroupAnalysisStandardsId")));
            o.put("AnalysisPerameter", str(ci(r, "AnalysisParameterDescription")));
            o.put("Min", dbText(ci(r, "MinValue")));
            o.put("MaxValue", dbText(ci(r, "MaxValue")));
            o.put("AnalysisResult", dbText(ci(r, "InAnalysisResult")));
            o.put("Remarks", str(ci(r, "RemarksDetail")));
            out.add(o);
        }
        return out;
    }

    public List<Map<String, Object>> attachments(int id) {
        UserAccount u = requireView();
        if (ownedRows(u, id) == null) throw new IllegalArgumentException("Record Not Found");
        return attachmentRows(id);
    }

    private List<Map<String, Object>> attachmentRows(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.attachments(id, SCREEN_NAME)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            String custom = str(ci(r, "UploadedFileCustomName"));
            o.put("Name", custom.isEmpty() ? fileName(str(ci(r, "Attachment"))) : custom);
            o.put("EntryDate", plain(ci(r, "EntryDate")));
            out.add(o);
        }
        return out;
    }

    /**
     * btnprint (:1097) / history Print (:815) / the "Preview" tick after a save (:683) -> GenerateReport (:1133).
     * Only the toolbar button is tied to the Print right on the desktop (btnprint.Enabled, :228).
     */
    public Map<String, Object> checkPrint(int id, boolean toolbar) {
        UserAccount u = requireView();
        if (toolbar && !rights.has(SCREEN_NAME, "print")) throw new AccessDeniedException("You do not have the Print right for this screen.");
        if (id <= 0 || ownedRows(u, id) == null) throw new IllegalArgumentException("Not Record Found For Display");   // W7 / W2
        if (repo.printRows(u, id).isEmpty()) throw new IllegalArgumentException("Not Record Found For Display");        // :1143
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("id", id);
        m.put("template", RPT_658);
        return m;
    }

    /** The stored Analysis Pic (which = "analysis") or Cooking Pic ("cooking") — {name, bytes}; null when there is none. */
    public Object[] picture(int id, String which) {
        UserAccount u = requireView();
        List<Map<String, Object>> rows = ownedRows(u, id);
        if (rows == null) throw new IllegalArgumentException("Record Not Found");
        String name = str(ci(rows.get(0), "cooking".equals(which) ? "SAHCookingPic" : "SAHAnalysisPic")).trim();
        if (name.isEmpty()) return null;
        return new Object[] { name, store.read(u, name) };
    }

    // ============================================================================= save

    /** btnSave_Click (:573) / btnUpdate_Click (:700) -> Insert() (:584). */
    @Transactional
    public Map<String, Object> save(SaleAnalysisRequest req) {
        UserAccount u = ctx.requireAccountingUser();
        if (req == null) throw new IllegalArgumentException("Grid Record Not Found");
        int recId = req.id;
        if (recId < 0) throw new IllegalArgumentException("Record Not Update beacause Record Id not found");
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) throw new AccessDeniedException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN_NAME, "update")) throw new AccessDeniedException("You do not have the Update right for this screen.");

        // ----------------------------------------------------------------- formvalidation (:495)
        if (nz(req.gatePassText).trim().isEmpty()) throw new IllegalArgumentException("Please Select GatePass No");
        if (nz(req.analystName).trim().isEmpty()) throw new IllegalArgumentException("Please Select Analyst Name");
        if (nz(req.analysisGroupText).trim().isEmpty()) throw new IllegalArgumentException("Please Select Analysis Group No");

        List<Map<String, Object>> existing = null;
        if (recId != 0) {
            existing = ownedRows(u, recId);                                                          // W2
            if (existing == null) throw new IllegalArgumentException("Record Not Found");
        }
        Map<String, Object> stored = existing == null ? null : existing.get(0);

        // ------------------------------------------------------- the gate pass and its hidden boxes (W3)
        int gatePassOutwardId, saleOrderId, sampleId, itemListGpId;
        String supCustCode;
        if (stored != null) {
            gatePassOutwardId = toInt(ci(stored, "GatePassOutwardId"));
            supCustCode = str(ci(stored, "SAHSupCustCode"));
            saleOrderId = toInt(ci(stored, "SaleOrderId"));
            sampleId = toInt(ci(stored, "SAHSampleLogRegisterId"));
            itemListGpId = gatePassOutwardId;
        } else {
            int gpId = req.gatePassId;
            if (gpId <= 0 || !contains(gatePasses(u), gpId)) throw new IllegalArgumentException("Please Select GatePass No");
            List<Map<String, Object>> gp = repo.gatePassByGpNo(u, GP_DOCUMENT_TYPE_ID, gpId, ctx.currentFinancialYearId());
            if (gp.isEmpty()) {                                                                      // D6 (:324-329)
                gatePassOutwardId = 0;
                supCustCode = "";
                saleOrderId = 0;
            } else {
                gatePassOutwardId = toInt(ci(gp.get(0), "Id"));
                supCustCode = str(ci(gp.get(0), "SupplierContractCode"));
                saleOrderId = toInt(ci(gp.get(0), "SaleOrderId"));
            }
            sampleId = 0;                                                                            // txtSampleId is never filled on a new record
            itemListGpId = gpId;
        }

        // ------------------------------------------------------------------------- item, group (W3)
        int itemId = req.itemId;
        if (itemId != 0 && !(stored != null && toInt(ci(stored, "SAHItmId")) == itemId) && !contains(items(itemListGpId), itemId)) {
            throw new IllegalArgumentException("Item Not Found");
        }
        int groupId = req.analysisGroupId;
        int storedGroup = stored == null ? 0 : toInt(ci(stored, "SAHAnalysisGroup"));
        if (groupId <= 0) throw new IllegalArgumentException("Please Select Analysis Group No");
        if (groupId != storedGroup && !contains(analysisGroups(u), groupId)) throw new IllegalArgumentException("Please Select Analysis Group No");

        // -------------------------------------------------------------- the Yes/No prompt (:600 / :607)
        if (!req.confirm) throw new ConfirmRequired(recId > 0 ? "Are you sure to Update?" : "Are you sure to Save?");

        // ------------------------------------------------------------------- grd.GetRows() (:610, :636)
        Set<Integer> allowed = new HashSet<>();
        for (Map<String, Object> p : parameterRows(u, groupId)) allowed.add(toInt(p.get("LabstandardanalysisId")));
        if (existing != null && groupId == storedGroup) {
            for (Map<String, Object> r : existing) allowed.add(toInt(ci(r, "InvLabGroupAnalysisStandardsId")));
        }
        List<SaleAnalysisRequest.Detail> details = new ArrayList<>();
        if (req.details != null) for (SaleAnalysisRequest.Detail d : req.details) if (d != null) details.add(d);
        if (details.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");         // :671
        Set<Integer> seen = new HashSet<>();
        for (SaleAnalysisRequest.Detail d : details) {
            if (!allowed.contains(d.standardsId) || !seen.add(d.standardsId)) throw new IllegalArgumentException("Analysis Parameter Not Found");   // W3
        }

        // ------------------------------------------------------------------------- pictures (W5)
        String analysisPic = picture(u, req.analysisPic, req.keepAnalysisPic, stored == null ? "" : str(ci(stored, "SAHAnalysisPic")));
        String cookingPic = picture(u, req.cookingPic, req.keepCookingPic, stored == null ? "" : str(ci(stored, "SAHCookingPic")));

        // ---------------------------------------------------------------------- attachments (:648-659, W6)
        List<Object[]> attach = new ArrayList<>();                                                   // {Attachment, custom name, size MB}
        if (recId != 0 && req.keepAttachmentIds != null && !req.keepAttachmentIds.isEmpty()) {
            Set<Integer> keep = new HashSet<>(req.keepAttachmentIds);
            for (Map<String, Object> a : repo.attachments(recId, SCREEN_NAME)) {
                if (!keep.contains(toInt(ci(a, "Id")))) continue;
                Object custom = ci(a, "UploadedFileCustomName");
                attach.add(new Object[] { str(ci(a, "Attachment")), custom == null ? null : String.valueOf(custom), toDouble(ci(a, "UploadedFileSizeMb")) });
            }
        }
        if (req.files != null) {
            for (InventoryPosItemRequest.Upload f : req.files) {
                if (f == null) continue;
                String name = fileName(nz(f.name).trim());
                if (name.isEmpty()) throw new IllegalArgumentException("The attachment could not be read");
                requireAttachmentPath(u);
                byte[] bytes = decode(f.base64, "The attachment could not be read");
                String storedName = store.store(u, name, bytes);
                attach.add(new Object[] { storedName, name, Math.round(bytes.length / 1048576d * 100d) / 100d });
            }
        }

        // -------------------------------------------------------------------- the header (:612-634)
        Timestamp now = Timestamp.valueOf(LocalDateTime.now().withNano(0));
        int docNo = stored != null ? toInt(ci(stored, "SAHDocNo")) : repo.generateDocNo(u);          // W1
        Map<String, Object> head = new LinkedHashMap<>();                                            // model 0623, declaration order
        head.put("IsAccepted", req.accepted);
        head.put("IsApproved", false);                                                               // D2
        head.put("ApprovedDate", now);                                                               // BLL 0405 Save
        head.put("DocDate", pickerDate(req.docDate));
        head.put("EntryDate", now);
        head.put("ModifyDate", now);
        head.put("DeductionRate", 0d);                                                               // D1
        head.put("DeductionWeight", 0d);
        head.put("ApprovedUserId", 0);
        head.put("BranchesId", 0);
        head.put("CompanyId", u.getCompanyId());
        head.put("DocNo", docNo);
        head.put("EntryUser", 0);                                                                    // D1
        head.put("GatePassOutwardId", gatePassOutwardId);
        head.put("Id", recId);
        head.put("InvLabAnalysisGroup", groupId);
        head.put("InvLabSampleLogRegisterId", sampleId);
        head.put("ItemId", itemId);
        head.put("ModifyUser", u.getId());
        head.put("OrganizationId", u.getOrganizationId());
        head.put("ProjectsId", 0);
        head.put("SaleOrderId", saleOrderId);
        head.put("SupplierCustomerId", 0);
        head.put("AnalysisPic", analysisPic);
        head.put("ContainerNo1", nz(req.containerNo));
        /* ContainerNo2 is a CLR null on the desktop: the parameter is not sent. */
        head.put("CookingPic", cookingPic);
        head.put("analystName", nz(req.analystName));
        head.put("AnalyzedBags", cvDouble(req.analyzedBags));
        head.put("Crop", nz(req.cropText));                                                          // D4
        head.put("RemarksHeader", nz(req.remarks));
        head.put("SupCustCode", supCustCode);
        head.put("DocumentTypeId", DOCUMENT_TYPE_ID);

        // ------------------------------------------- DMSAttachments.RemoveById (:652), then DAL 0360 SetData
        repo.removeAttachments(recId, SCREEN_NAME);
        int num = repo.setHeader(recId == 0 ? "Sp_InvLabAnalysisSaleHeader_Insert" : "Sp_InvLabAnalysisSaleHeader_Update", head);
        int id = num > 0 ? num : recId;
        if (id <= 0) throw new IllegalStateException("Save returned no document id.");
        for (SaleAnalysisRequest.Detail d : details) {
            repo.insertDetail(id, d.standardsId, cvDouble(d.result), nz(d.remarks));                 // :639-641
        }
        for (Object[] a : attach) {
            repo.insertAttachment(id, 0, DOCUMENT_TYPE_ID, (String) a[0], now, 0, now, nz(u.getId()),
                    nz(u.getOrganizationId()), nz(u.getCompanyId()), SCREEN_NAME, (String) a[1], (Double) a[2]);
        }
        LOG.debug("Sale Analysis {} saved (gate pass {})", id, gatePassOutwardId);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", "Record Update Successfully [" + id + "] ");                               // D3 (:663)
        m.put("id", id);
        return m;
    }

    /**
     * fileSavePath / fileSavePathcookingpic (:627-628): a newly browsed .jpg / .jpeg / .png (btnimg1_Click
     * :1043, btnimage2_Click :1070), the stored name when the loaded picture is still shown, else "".
     */
    private String picture(UserAccount u, InventoryPosItemRequest.Upload p, boolean keep, String stored) {
        if (p == null || nz(p.base64).isEmpty()) return keep && stored != null ? stored : "";
        String name = fileName(nz(p.name).trim());
        String lower = name.toLowerCase(Locale.ROOT);
        if (!(lower.endsWith(".jpg") || lower.endsWith(".jpeg") || lower.endsWith(".png"))) {       // the dialog's filter (:1051)
            throw new IllegalArgumentException("Only .jpg, .jpeg and .png pictures can be attached");
        }
        requireAttachmentPath(u);
        return store.store(u, name, decode(p.base64, "The picture could not be read"));
    }

    /** definition() (:1018). */
    private void requireAttachmentPath(UserAccount u) {
        if (!attachmentPathSet(u)) throw new IllegalArgumentException("Please Map the Path in Configration");
    }

    private boolean attachmentPathSet(UserAccount u) {
        return !nz(store.configuration(u, "Attachment Folder Path")).trim().isEmpty()
                || toBool(store.configuration(u, "IsVpsAttachmentsServiceOn"));
    }

    private static byte[] decode(String base64, String refusal) {
        String s = nz(base64).trim();
        int comma = s.startsWith("data:") ? s.indexOf(',') : -1;
        if (comma >= 0) s = s.substring(comma + 1);
        if (s.isEmpty()) throw new IllegalArgumentException(refusal);
        try { return Base64.getDecoder().decode(s); }
        catch (IllegalArgumentException e) { throw new IllegalArgumentException(refusal); }
    }

    // =========================================================================== helpers

    private UserAccount requireView() {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "view")) throw new AccessDeniedException("You do not have the View right for Sale Analysis.");
        return u;
    }

    /** The ReadById rows of a record of the user's organization and company; null otherwise (W2). */
    private List<Map<String, Object>> ownedRows(UserAccount u, int id) {
        if (id <= 0) return null;
        List<Map<String, Object>> rows = repo.readById(id);
        if (rows.isEmpty()) return null;                                                             // D8
        Map<String, Object> h = rows.get(0);
        if (toInt(ci(h, "SAHCompanyId")) != nz(u.getCompanyId())) return null;
        if (toInt(ci(h, "SAHOraganizationId")) != nz(u.getOrganizationId())) return null;
        return rows;
    }

    private static Map<String, Object> idName(int id, String name) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("Name", name);
        return o;
    }

    private static boolean contains(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (toInt(r.get("Id")) == id) return true;
        return false;
    }

    private static String fileName(String path) {
        int i = Math.max(path.lastIndexOf('/'), path.lastIndexOf('\\'));
        return i >= 0 ? path.substring(i + 1) : path;
    }

    /** A picker's Value — the chosen day at the current time of day (the procedure's parameter is a DATE). */
    private static Timestamp pickerDate(String day) {
        LocalDate d;
        try { d = day == null || day.trim().length() < 10 ? LocalDate.now() : LocalDate.parse(day.trim().substring(0, 10)); }
        catch (RuntimeException e) { throw new IllegalArgumentException("Doc Date is not a valid date"); }
        return Timestamp.valueOf(LocalDateTime.of(d, LocalTime.now().withNano(0)));
    }

    /** yyyy-MM-dd of a database date. */
    private static String day(Object v) {
        Object p = plain(v);
        String s = p == null ? "" : String.valueOf(p);
        return s.length() >= 10 ? s.substring(0, 10) : "";
    }

    /** Date values leave as local "yyyy-MM-dd HH:mm:ss" text, never as a JSON timestamp. */
    private static Object plain(Object v) {
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString() + " 00:00:00";
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString().replace('T', ' ');
        if (v instanceof LocalDate) return v.toString() + " 00:00:00";
        return v;
    }

    /** A float column as the text an untyped DataTable column shows it (double.ToString()); NULL is "". */
    private static String dbText(Object v) {
        if (v == null) return "";
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        if (v instanceof Double || v instanceof Float) return clr(((Number) v).doubleValue());
        return String.valueOf(v);
    }

    /** Conversion.ToString of a database value. */
    private static String netText(Object v) { return dbText(v); }

    /** .NET double.ToString() — 15 significant digits, no trailing zeros. */
    private static String clr(double v) {
        if (v == 0d) return "0";
        if (Double.isNaN(v) || Double.isInfinite(v)) return String.valueOf(v);
        return new BigDecimal(v).round(new MathContext(15)).stripTrailingZeros().toPlainString();
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    /** Conversion.ToInt on a database value — anything that is not a number is 0. */
    private static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) {
            try { return (int) Math.rint(Double.parseDouble(String.valueOf(v).trim())); }
            catch (NumberFormatException e2) { return 0; }
        }
    }

    private static double toDouble(Object v) {
        if (v == null) return 0d;
        if (v instanceof Number) { double d = ((Number) v).doubleValue(); return Double.isFinite(d) ? d : 0d; }
        return cvDouble(String.valueOf(v));
    }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String t = v == null ? "" : String.valueOf(v).trim();
        return "true".equalsIgnoreCase(t) || "1".equals(t);
    }

    /** Conversion.ToDouble(text) — thousands separators allowed; anything unparseable (or infinite) is 0. */
    private static double cvDouble(String s) {
        if (s == null || s.trim().isEmpty()) return 0d;
        try { double v = Double.parseDouble(s.trim().replace(",", "")); return Double.isFinite(v) ? v : 0d; }
        catch (NumberFormatException e) { return 0d; }
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }
    private static String nz(String s) { return s == null ? "" : s; }
    private static int nz(Integer v) { return v == null ? 0 : v; }
}
