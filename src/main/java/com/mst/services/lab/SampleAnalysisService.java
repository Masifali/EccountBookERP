package com.mst.services.lab;

import com.mst.models.UserAccount;
import com.mst.repositories.lab.SampleAnalysisRepository;
import com.mst.security.CurrentUserContext;
import com.mst.services.StoreScreenRights;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.transaction.support.TransactionSynchronization;
import org.springframework.transaction.support.TransactionSynchronizationManager;

import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardOpenOption;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Base64;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;
import java.util.UUID;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Lab (ModuleId 7) — screen 159 "Sample Analysis".
 *
 * <pre>
 * Screen id        159 (dbo.ScreenDefinition; TargetUrl Architecture.WinApp.Lab.InvLabSampleAnalysis)
 * ScreenName       InvLabSampleAnalysis   (base.Name; rights key — InvLabSampleAnalysis.cs:289)
 * DocumentTypeId   302                    (:345, :1081, :1131, :1612, :1654)
 * Route            /quality/sample-analysis ; API /api/lab/sample-analysis
 * Desktop          Architecture.WinApp.Lab/InvLabSampleAnalysis.cs (4,599 lines, designer :1986-4599)
 * BLL / DAL        BLL 0407 / DAL 0362 Lab.InvLabSampleAnalysisHeader; BLL 0400 InvLabAnalysisGroup;
 *                  BLL 0394 LabAnalysisStandardSchedule; BLL 0406 InvLabGroupAnalysisStandards;
 *                  BLL 0595 PurchaseOrder.GetForComboPoBySupplierandItemId
 * Models           0626 InvLabSampleAnalysisHeader, 0625 InvLabSampleAnalysisDetail,
 *                  0611 InvLabSampleAnalysiSubParamsDetail
 * Print            Print657 (:1892) -> 657-RptInvLabSampleAnalysisSlipA.rpt
 *                  (Sp_InvLabSampleAnalysisHeader_RiceSlipAndRegister_Rpt) — the page calls
 *                  /api/print/by-template/657-RptInvLabSampleAnalysisSlipA.rpt/pdf
 * </pre>
 *
 * The form has no Delete and no approval action: Insert() always sends IsApproved = false (:1106),
 * the Insert procedure stores it and the Update procedure does not touch IsApproved. No voucher, no
 * stock posting — DAL 0362 SetData writes the header, the detail rows and their sub-parameter rows.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED, NOT CORRECTED (each is the user's call to change)
 * ---------------------------------------------------------------------------------------------
 *  D1  History: "From &amp; To Doc No" are read by the form (:1495-1496) but BLL GetAll never sends
 *      @FromDocNo / @ToDocNo — the two boxes do not filter.
 *  D2  formvalidation (:817, :823) tests the combo TEXT; the "-- Select --" row has a text, so it
 *      passes and the save then stops at "No detail record found" (after the Yes/No question).
 *  D3  The Yes/No question is asked before "No detail record found", the sub-parameter total check
 *      and "ResultValue cannot be equal to zero" (:1048-1080).
 *  D4  Sub-parameter total vs parameter value is an exact double comparison (:1072).
 *  D5  Update does not check IsApproved; ApprovedDate is sent as "now" on every save but neither
 *      procedure stores it.
 *  D6  Update sends no Attachments / InvSampleLogNo / SupplierCode / SampleStatus (CLR nulls), so
 *      the Update procedure sets those four columns to NULL.
 *  D7  The Sample Log No is saved from whatever the combo holds; ReadById (:1229) never restores it.
 *  D8  AnalysisGroupdtFill (:556): every type other than 1 — including 0 "-- Select --" — reads the
 *      Standard Schedule list.
 *  D9  DetailGridFillByGroup (:905): a parameter id met again after a different one STOPS the loop
 *      ("break"), it does not just skip that row.
 *  D10 A picture replaced by browsing another one leaves the old file in the folder; only a cleared
 *      picture is deleted, and only on Update (:1119-1126, :1128-1160).
 *  D11 657-Print on a new form (RecId 0) sends no @Id — the procedure then returns every record of
 *      the company (:1906, BLL "if (obj.Id != 0)").
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web only)
 * ---------------------------------------------------------------------------------------------
 *  W1  Doc No: on a new save the server asks the generator again; on update it keeps the stored
 *      number; never taken from the request (the desktop sends the number shown in the box).
 *  W2  Open / update / print / pictures only when the record is DocumentTypeId 302 and belongs to the
 *      user's organization and company (ReadById filters by Id alone).
 *  W3  Supplier, commission agent, job lot and analysis type must be in the lists the form binds;
 *      every posted detail row must be a parameter of the posted group (for the posted type and
 *      date) or a stored row of THIS record, and every sub row a sub-parameter of its parameter.
 *      The parameter name in the sub-total message is the server's.
 *  W4  Pictures: the folder is checked BEFORE the save (the desktop saves the record and then fails
 *      on File.Copy with "Attachment Folder Path Not Configure Properly"); a new file is written
 *      inside the transaction and removed on rollback; 5 MB cap; jpg / jpeg / gif / png only (the
 *      desktop's dialog filter).
 *  W5  The pending picture names are reset by New / after a save. On the desktop refresh() (:1343)
 *      does not clear fileSavePath, so a record opened with a picture and then "New" would save the
 *      next record pointing at the previous record's picture file. Not reproduced.
 *  W6  Print is refused without the Print right on every path (the desktop only disables the
 *      toolbar button; the History grid's Print button is not checked).
 *
 * Not ported: DMS attachments (toolbar "Attachment", History "Add Attachment"; the attachment count
 * link is a read-only list), saved grid layouts (CtrlGrdBar), the 657 sub-report
 * (LabSampleAnalysisSubParameterReport.rpt) inside the Jasper template.
 */
@Service
public class SampleAnalysisService {

    private static final Logger LOG = LoggerFactory.getLogger(SampleAnalysisService.class);

    public static final String SCREEN_NAME = "InvLabSampleAnalysis";
    public static final int SCREEN_ID = 159;
    public static final int DOCUMENT_TYPE_ID = 302;
    public static final String PRINT_RPT = "657-RptInvLabSampleAnalysisSlipA.rpt";
    private static final int MAX_PICTURE_BYTES = 5 * 1024 * 1024;

    /** Desktop Yes/No prompt — answered by the controller as HTTP 409 {confirm:true,message}. */
    public static class ConfirmRequired extends RuntimeException {
        private static final long serialVersionUID = 1L;
        public ConfirmRequired(String message) { super(message); }
    }

    /** A stored picture: file name and bytes. */
    public static final class PictureFile {
        public final String name;
        public final byte[] bytes;
        PictureFile(String name, byte[] bytes) { this.name = name; this.bytes = bytes; }
    }

    private final SampleAnalysisRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public SampleAnalysisService(SampleAnalysisRepository repo, StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ======================================================================== form load

    /** InvLabSampleAnalysis_Load (:285) — everything the form binds on open. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (!Boolean.TRUE.equals(r.get("view"))) throw new AccessDeniedException("You do not have the View right for Sample Analysis.");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", r);
        out.put("screenId", SCREEN_ID);
        out.put("documentTypeId", DOCUMENT_TYPE_ID);
        out.put("printRpt", PRINT_RPT);
        out.put("docNo", repo.generateDocNo(u));                 // itemcode (:411)
        putLists(u, out);
        putConfigs(u, out);
        putHistoryCombos(u, out);                                // HistoryComboFill (:324)
        return out;
    }

    /** toolStripButton2_Click "Refresh" (:1385) — itemcode + the six fills again. */
    public Map<String, Object> formRefresh() {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", repo.generateDocNo(u));
        putLists(u, out);
        return out;
    }

    /** refresh() (:1343) — itemcode (:1369) and GetConfigurationsFromGlobalAndBindValuesInColumns (:1377). */
    public Map<String, Object> numbers() {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", repo.generateDocNo(u));
        putConfigs(u, out);
        return out;
    }

    /** btnRefreshHistory_Click (:1451) — HistoryComboFill. */
    public Map<String, Object> historyCombos() {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        putHistoryCombos(u, out);
        return out;
    }

    private void putLists(UserAccount u, Map<String, Object> out) {
        out.put("items", items(u));                      // ItemFill (:600)
        out.put("analysisTypes", analysisTypes());       // AnalysisTypeFill(InvLabAnalysisGroup.AnalysisType()) (:298)
        out.put("suppliers", suppliers(u));              // cmbsupplierfill (:467): Supplier + Commission Agent
        out.put("cropYears", cropYears(u));              // CropYearFill (:616)
        out.put("jobLots", jobLots(u));                  // combojoblotfill (:629)
        out.put("statuses", statuses());                 // LabstatusFill (:645)
    }

    /** GetConfigurationsFromGlobalAndBindValuesInColumns (:390): "Job/Lot" and "Default Crop Year", both through Conversion.ToInt. */
    private void putConfigs(UserAccount u, Map<String, Object> out) {
        out.put("defaultJobLot", toInt(repo.config(u, "Job/Lot")));
        out.put("defaultCropYear", toInt(repo.config(u, "Default Crop Year")));
    }

    /**
     * HistoryComboFill (:324): one procedure, rows split by their "Activity" column into six
     * Id / name tables (Item, Supplier, CropYear, JobLot, Status, AnalysisGroup); other activities
     * the procedure returns (ItemTypes, ItemCategories, ParentCategories) are ignored by the form.
     */
    private void putHistoryCombos(UserAccount u, Map<String, Object> out) {
        List<Map<String, Object>> supplier = new ArrayList<>(), item = new ArrayList<>(), crop = new ArrayList<>(),
                job = new ArrayList<>(), status = new ArrayList<>(), group = new ArrayList<>();
        for (Map<String, Object> r : repo.historyDropDowns(u, String.valueOf(DOCUMENT_TYPE_ID))) {
            String activity = str(ci(r, "Activity"));
            Map<String, Object> o = idName(toInt(ci(r, "Id")), str(ci(r, "ReferenceName")));
            if ("Item".equals(activity)) item.add(o);
            else if ("Supplier".equals(activity)) supplier.add(o);
            else if ("CropYear".equals(activity)) crop.add(o);
            else if ("JobLot".equals(activity)) job.add(o);
            else if ("Status".equals(activity)) status.add(o);
            else if ("AnalysisGroup".equals(activity)) group.add(o);
        }
        out.put("historySuppliers", supplier);
        out.put("historyItems", item);
        out.put("historyCropYears", crop);
        out.put("historyJobLots", job);
        out.put("historyStatuses", status);
        out.put("historyAnalysisGroups", group);
    }

    // ============================================================================ lists

    /**
     * ItemFill (:600) — DDL.BindDDL(dt, CmbItemName, "Id", "ItemName", "Item"): the whole table is
     * the combo's source, column 0 hidden. Columns of ReadAllForComboTwoColumns: Id, ItemName,
     * ItemCategory, ItemCode, InventoryParentCategoriesId.
     */
    private List<Map<String, Object>> items(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.items(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemCategory", str(ci(r, "ItemCategory")));
            o.put("ItemCode", str(ci(r, "ItemCode")));
            o.put("InventoryParentCategoriesId", str(ci(r, "InventoryParentCategoriesId")));
            out.add(o);
        }
        return out;
    }

    /** AnalysisTypeFill (:521) — vAnalysisType "Id" / "Type" (two columns, "-- Select --" row added by the page). */
    private List<Map<String, Object>> analysisTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.analysisTypes()) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "Type"))));
        return out;
    }

    /** cmbsupplierfill (:467) — BindDDL over Id, CompanyName, GlAccountId, PartyCode (column 0 hidden). */
    private List<Map<String, Object>> suppliers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.suppliers(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("CompanyName", str(ci(r, "CompanyName")));
            o.put("GlAccountId", str(ci(r, "GlAccountId")));
            o.put("PartyCode", str(ci(r, "PartyCode")));
            out.add(o);
        }
        return out;
    }

    /** CropYearFill (:616) — BindDDLNew "Id" / "CropYear". */
    private List<Map<String, Object>> cropYears(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.cropYears(u)) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "CropYear"))));
        return out;
    }

    /** combojoblotfill (:629) — BindDDLNew "Id" / "JobLotDescription". */
    private List<Map<String, Object>> jobLots(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.jobLots(u)) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "JobLotDescription"))));
        return out;
    }

    /** LabstatusFill (:645) — 1 Accepted, 2 Rejected. */
    private List<Map<String, Object>> statuses() {
        List<Map<String, Object>> out = new ArrayList<>();
        out.add(idName(1, "Accepted"));
        out.add(idName(2, "Rejected"));
        return out;
    }

    /**
     * AnalysisGroupdtFill (:533): type 1 -> InvLabAnalysisGroup.GetAllOrById rows as
     * (Id, AnalysisGroupDescription, GroupType -> GroupTypeId, InvParentCateDescription -> GroupType);
     * any other type (0 included, D8) -> LabAnalysisStandardSchedule.ComboFill rows as
     * (LabAnalysisGroupId, "name - EffectiveDateFrom.ToShortDateString()", GroupTypeId, GroupType).
     */
    public List<Map<String, Object>> analysisGroups(int typeId, String docDate) {
        UserAccount u = requireView();
        List<Map<String, Object>> out = new ArrayList<>();
        if (typeId == 1) {
            for (Map<String, Object> r : repo.analysisGroups(u)) {
                out.add(group(toInt(ci(r, "Id")), str(ci(r, "AnalysisGroupDescription")),
                        toInt(ci(r, "GroupType")), str(ci(r, "InvParentCateDescription"))));
            }
        } else {
            for (Map<String, Object> r : repo.scheduleGroups(u, dateOf(docDate))) {
                String name = str(ci(r, "LabAnalysisStandardScheduleName"));
                String date = shortDate(ci(r, "EffectiveDateFrom"));
                out.add(group(toInt(ci(r, "LabAnalysisGroupId")), name + " - " + date,
                        toInt(ci(r, "GroupTypeId")), str(ci(r, "GroupType"))));
            }
        }
        return out;
    }

    private static Map<String, Object> group(int id, String description, int groupTypeId, String groupType) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("AnalysisGroupDescription", description);
        o.put("GroupTypeId", groupTypeId);
        o.put("GroupType", groupType);
        return o;
    }

    /** GetDtForDetailGrid (:838) + DetailGridFillByGroup (:865) — the detail rows of a group, with their sub-parameter rows. */
    public List<Map<String, Object>> parameters(int typeId, int groupId, String docDate) {
        UserAccount u = requireView();
        return detailRowsOf(parameterRows(u, typeId, groupId, docDate));
    }

    /** GetDtForDetailGrid (:838): type 0 or group 0 -> null; type 1 -> group standards; else -> standard schedule for the Doc Date. */
    private List<Map<String, Object>> parameterRows(UserAccount u, int typeId, int groupId, String docDate) {
        if (typeId == 0 || groupId == 0) return new ArrayList<>();
        if (typeId == 1) return repo.parametersFromGroupStandards(u, groupId);
        return repo.parametersFromGroupStandardSchedule(u, groupId, dateOf(docDate));
    }

    /** DetailGridFillByGroup (:865), statement for statement — including the "break" on a repeated parameter (D9). */
    private static List<Map<String, Object>> detailRowsOf(List<Map<String, Object>> dt) {
        List<Map<String, Object>> detail = new ArrayList<>();
        if (dt == null || dt.isEmpty()) return detail;
        List<Map<String, Object>> lstsub = new ArrayList<>();
        for (Map<String, Object> r : dt) {
            if (toInt(ci(r, "SubParamsId")) > 0) {
                Map<String, Object> s = new LinkedHashMap<>();
                s.put("Id", 0);
                s.put("InvLabSampleAnalysisDetailId", 0);
                s.put("InvLabSampleAnalysisHeaderId", 0);
                s.put("InvParentParameterId", toInt(ci(r, "ParentParameterId")));
                s.put("SubParameterId", toInt(ci(r, "SubParamsId")));
                s.put("ParentParameterName", null);                     // not set by the form for new rows
                s.put("SubParameterName", str(ci(r, "SubParameterName")));
                s.put("ResultValue", 0d);
                lstsub.add(s);
            }
        }
        int paramsId = 0;
        for (Map<String, Object> r : dt) {
            int itemsId = toInt(ci(r, "InvLabAnalysisItemsId"));
            if (paramsId == itemsId) continue;
            boolean flag = true;
            for (Map<String, Object> d : detail) {
                if (toInt(d.get("InvLabAnalysisItemsId")) == itemsId) { flag = false; break; }
            }
            if (!flag) break;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", 0);
            o.put("InvLabAnalysisItemsId", itemsId);
            o.put("InvLabSampleAnalysisHeaderId", 0);
            o.put("AnalysisGroupDescription", str(ci(r, "AnalysisGroupDescription")));
            o.put("AnalysisParameterDescription", str(ci(r, "AnalysisParameterDescription")));
            o.put("MinValue", toDouble(ci(r, "MinValue")));
            o.put("MaxValue", toDouble(ci(r, "MaxValue")));
            o.put("ResultValue", 0d);
            o.put("RemarksDetail", null);
            List<Map<String, Object>> subs = new ArrayList<>();
            for (Map<String, Object> s : lstsub) if (toInt(s.get("InvParentParameterId")) == itemsId) subs.add(s);
            o.put("Subs", subs);
            detail.add(o);
            paramsId = itemsId;
        }
        return detail;
    }

    /**
     * ItemBind (:705): order and group chosen -> BindItemsByAnalysisGroupandOrderId (:736, Id / ItemName);
     * group only -> BindItemsByAnalysisGroup (:757, Id / ItemName); neither -> ItemFill (:600).
     * The page shows the desktop's "Items not found" message when mode is order/group and the list is empty.
     */
    public Map<String, Object> itemsFor(int groupId, int groupTypeId, int orderId) {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        if (orderId > 0 && groupId > 0) {
            out.put("mode", "order");
            out.put("items", twoColumnItems(repo.itemsByAnalysisGroupAndOrder(u, groupTypeId, orderId)));
        } else if (groupId > 0) {
            out.put("mode", "group");
            out.put("items", twoColumnItems(repo.itemsByAnalysisGroup(u, groupTypeId)));
        } else {
            out.put("mode", "all");
            out.put("items", items(u));
        }
        return out;
    }

    private static List<Map<String, Object>> twoColumnItems(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("ItemName", str(ci(r, "ItemName")));
            out.add(o);
        }
        return out;
    }

    /**
     * cmbsupplier_Leave / cmbitem_Leave (:778, :791): PurchaseOrderFill (:484, supplier != 0 and
     * item != 0) and SampleLogNumberFill (:435, supplier &gt; 0 and item &gt; 0). "applies" false = the
     * desktop calls neither procedure and leaves both combos as they are.
     */
    public Map<String, Object> byPartyItem(int supplierId, int itemId) {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        boolean po = supplierId != 0 && itemId != 0;
        boolean log = supplierId > 0 && itemId > 0;
        out.put("purchaseOrderApplies", po);
        out.put("sampleLogApplies", log);
        List<Map<String, Object>> orders = new ArrayList<>();
        if (po) for (Map<String, Object> r : repo.purchaseOrders(u, supplierId, itemId)) orders.add(idName(toInt(ci(r, "Id")), str(ci(r, "DocNo"))));
        List<Map<String, Object>> logs = new ArrayList<>();
        if (log) for (Map<String, Object> r : repo.sampleLogs(u, supplierId, itemId)) logs.add(idName(toInt(ci(r, "Id")), str(ci(r, "SampleNo"))));
        out.put("purchaseOrders", orders);
        out.put("sampleLogs", logs);
        return out;
    }

    // ========================================================================== history

    /**
     * gridhistoryfill (:1475) -> BLL GetAll. From / To date only when the picker's box is ticked
     * (:1487-1494); From / To Doc No are never sent (D1); Crop Year and Status are the combo TEXTS.
     * The rows are mapped exactly as tablehistory is filled (:1532).
     */
    public List<Map<String, Object>> history(boolean fromChecked, String fromDate, boolean toChecked, String toDate,
                                             int supplierId, int itemId, int jobLotId, int analysisGroupId,
                                             String cropYear, String status) {
        UserAccount u = requireView();
        boolean viewAll = Boolean.TRUE.equals(rights.of(SCREEN_NAME).get("viewAll"));
        List<Map<String, Object>> rows = repo.history(u, viewAll, ctx.currentUserId(),
                fromChecked ? dateOf(fromDate) : null, toChecked ? dateOf(toDate) : null,
                supplierId, itemId, jobLotId, analysisGroupId, cropYear, status);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("DocDate", iso(ci(r, "DocDate")));
            o.put("ReportNo", str(ci(r, "ReportNo")));
            o.put("PartyLotNo", str(ci(r, "PartyLotRefNo")));
            o.put("NoOfBags", toDouble(ci(r, "NoOfBagsInspected")));
            o.put("PartyName", str(ci(r, "CompanyName")));
            o.put("CommissionAgent", str(ci(r, "CommissionAgent")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("CropYear", str(ci(r, "Crop")));
            o.put("JobLot", str(ci(r, "JobLotDescription")));
            o.put("Status", str(ci(r, "Status")));
            o.put("AnalysisGroup", str(ci(r, "AnalysisGroupDescription")));
            o.put("EntryDate", iso(ci(r, "EntryDate")));
            o.put("EntryUser", str(ci(r, "UserName")));
            o.put("ModifyDate", iso(ci(r, "ModifyDate")));
            o.put("ModifyUser", str(ci(r, "ModifyUserName")));
            o.put("AnalysisPic", str(ci(r, "AnalysisPic")).isEmpty() ? 0 : 1);      // !string.IsNullOrEmpty(...) ? 1 : 0
            o.put("CookingPic", str(ci(r, "CookingPic")).isEmpty() ? 0 : 1);
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    /** grdhistory_LinkClicked "NoOfAttachments" (:1652) -> CommonServices.GetNoofAttachmentsByRefDocumentTypeID: AttachmentName, CustomName, EntryDate. */
    public List<Map<String, Object>> attachments(int id) {
        requireView();
        requireOwned(id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.attachments(id, DOCUMENT_TYPE_ID)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("AttachmentName", str(ci(r, "Attachment")));
            o.put("CustomName", str(ci(r, "UploadedFileCustomName")));
            o.put("EntryDate", iso(ci(r, "EntryDate")));
            out.add(o);
        }
        return out;
    }

    // ============================================================================= read

    /**
     * BLL GetByID (:222) + DAL GetData (:125): header, detail rows (ReadDetailByHeaderId) and the
     * sub rows (ReadLabSampleSubParamsDetailByHeaderId) attached where
     * InvParentParameterId == InvLabAnalysisItemsId. Used by ReadById (:1229) and by the History
     * detail grid (grdhistory_SelectionChanged :1674).
     */
    public Map<String, Object> load(int id) {
        UserAccount u = requireView();
        Map<String, Object> h = requireOwned(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(ci(h, "Id")));
        out.put("DocNo", toInt(ci(h, "DocNo")));
        out.put("DocDate", isoDay(ci(h, "DocDate")));
        out.put("ReportNo", str(ci(h, "ReportNo")));
        out.put("PartyLotRefNo", str(ci(h, "PartyLotRefNo")));
        out.put("NoOfBagsInspected", toDouble(ci(h, "NoOfBagsInspected")));
        out.put("SupplierCustomerId", toInt(ci(h, "SupplierCustomerId")));
        out.put("CommissionAgentId", toInt(ci(h, "CommissionAgentId")));
        out.put("Crop", str(ci(h, "Crop")));
        out.put("AnalysisTypeId", toInt(ci(h, "AnalysisTypeId")));
        out.put("InvLabAnalysisGroupId", toInt(ci(h, "InvLabAnalysisGroupId")));
        out.put("ItemId", toInt(ci(h, "ItemId")));
        out.put("OrderId", toInt(ci(h, "OrderId")));
        /* 'ReadById' returns h.InvLabSampleLogRegisterId. The desktop's ReadById never puts it back into
           CmbSampleLogNo, so an Update there silently saves the link as 0; the page preselects it instead. */
        out.put("InvLabSampleLogRegisterId", toInt(ci(h, "InvLabSampleLogRegisterId")));
        out.put("JobLotId", toInt(ci(h, "JobLotId")));
        out.put("IsAccepted", bool(ci(h, "IsAccepted")));
        out.put("Remarks", str(ci(h, "Remarks")));
        Path root = pictureRoot(u);
        out.put("AnalysisPicExists", existing(root, str(ci(h, "AnalysisPic"))) != null);   // File.Exists (:1290)
        out.put("CookingPicExists", existing(root, str(ci(h, "CookingPic"))) != null);     // File.Exists (:1312)
        out.put("Details", storedDetails(id));
        return out;
    }

    private List<Map<String, Object>> storedDetails(int headerId) {
        List<Map<String, Object>> subs = new ArrayList<>();
        for (Map<String, Object> r : repo.subDetails(headerId)) {
            Map<String, Object> s = new LinkedHashMap<>();
            s.put("Id", toInt(ci(r, "Id")));
            s.put("InvLabSampleAnalysisDetailId", toInt(ci(r, "InvLabSampleAnalysisDetailId")));
            s.put("InvLabSampleAnalysisHeaderId", toInt(ci(r, "InvLabSampleAnalysisHeaderId")));
            s.put("InvParentParameterId", toInt(ci(r, "InvParentParameterId")));
            s.put("SubParameterId", toInt(ci(r, "SubParameterId")));
            s.put("ParentParameterName", str(ci(r, "ParentParameterName")));
            s.put("SubParameterName", str(ci(r, "SubParameterName")));
            s.put("ResultValue", toDouble(ci(r, "ResultValue")));
            subs.add(s);
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.details(headerId)) {
            int itemsId = toInt(ci(r, "InvLabAnalysisItemsId"));
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("InvLabAnalysisItemsId", itemsId);
            o.put("InvLabSampleAnalysisHeaderId", toInt(ci(r, "InvLabSampleAnalysisHeaderId")));
            o.put("AnalysisGroupDescription", str(ci(r, "AnalysisGroupDescription")));
            o.put("AnalysisParameterDescription", str(ci(r, "AnalysisParameterDescription")));
            o.put("MinValue", toDouble(ci(r, "MinValue")));
            o.put("MaxValue", toDouble(ci(r, "MaxValue")));
            o.put("ResultValue", toDouble(ci(r, "ResultValue")));
            Object rem = ci(r, "RemarksDetail");
            o.put("RemarksDetail", rem == null ? null : String.valueOf(rem));
            List<Map<String, Object>> mine = new ArrayList<>();
            for (Map<String, Object> s : subs) if (toInt(s.get("InvParentParameterId")) == itemsId) mine.add(s);
            o.put("Subs", mine);
            out.add(o);
        }
        return out;
    }

    // ============================================================================= print

    /**
     * Print657 (:1892): GetPrintSlipAndReport for the record — no rows is "No Record Found For
     * Display" (:1911). Id 0 (657-Print on a new form) sends no @Id (D11).
     */
    public Map<String, Object> checkPrint(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "print")) throw new AccessDeniedException("You do not have the Print right for Sample Analysis.");
        if (id != 0) requireOwned(id);
        if (repo.printRows(u, id).isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("ok", true);
        out.put("rpt", PRINT_RPT);
        return out;
    }

    // ========================================================================== pictures

    /** The stored Analysis / Cooking picture of a record (ReadById :1285-1330, FormHelper.ShowLabImagesPreview); null when there is no file. */
    public PictureFile picture(int id, String which) {
        UserAccount u = requireView();
        Map<String, Object> h = requireOwned(id);
        String name = "cooking".equalsIgnoreCase(which) ? str(ci(h, "CookingPic")) : str(ci(h, "AnalysisPic"));
        Path file = existing(pictureRoot(u), name);
        if (file == null) return null;
        try {
            return new PictureFile(file.getFileName().toString(), Files.readAllBytes(file));
        } catch (Exception e) {
            LOG.warn("Sample Analysis picture {} could not be read", file, e);
            return null;
        }
    }

    /** clsGlobalVariables.AttachmentFolderPath — configuration "Attachment Folder Path" (DashboardNew.cs:1227); null when not a directory. */
    private Path pictureRoot(UserAccount u) {
        String configured = repo.config(u, "Attachment Folder Path");
        if (configured == null || configured.trim().isEmpty()) return null;
        try {
            Path root = Paths.get(configured.trim()).toAbsolutePath().normalize();
            return Files.isDirectory(root) ? root : null;
        } catch (Exception e) {
            return null;
        }
    }

    /** File.Exists(Path.Combine(AttachmentFolderPath, name)) — the name must be a plain file name inside the folder. */
    private static Path existing(Path root, String name) {
        if (root == null || name == null) return null;
        String n = name.trim();
        if (n.isEmpty() || n.contains("/") || n.contains("\\") || n.contains(":") || n.equals(".") || n.equals("..")) return null;
        try {
            Path file = root.resolve(n).normalize();
            if (!file.startsWith(root) || !Files.isRegularFile(file)) return null;
            return file;
        } catch (Exception e) {
            return null;
        }
    }

    private static final class PicturePlan {
        String name = "";            // header AnalysisPic / CookingPic
        byte[] bytes;                // a browsed picture to write
        Path deleteAfter;            // DeleteImgList (:1118-1126), Update only
    }

    /**
     * fileSavePath / ofd / PictureBox state at Insert() (:1107-1108, :1118-1126):
     *  "new"  — btnimg1_Click (:1742): "{Guid:N}{extension}", copied into the folder after the save (:1166-1178);
     *  "keep" — the stored name, as ReadById left it (:1285) when the file exists, otherwise "";
     *  other  — "" and, on Update, the stored file (Tag) goes into DeleteImgList.
     */
    private PicturePlan planPicture(SampleAnalysisRequest.Picture p, String storedName, boolean update, Path root) {
        PicturePlan plan = new PicturePlan();
        String mode = p == null || p.mode == null ? "" : p.mode;
        if ("new".equals(mode)) {
            String original = p.name == null ? "" : p.name;
            int dot = original.lastIndexOf('.');
            String ext = dot >= 0 ? original.substring(dot).toLowerCase(Locale.ROOT) : "";
            if (!(ext.equals(".jpg") || ext.equals(".jpeg") || ext.equals(".gif") || ext.equals(".png")))
                throw new IllegalArgumentException("Select an image (*.jpg; *.jpeg; *.gif; *.png)");
            byte[] bytes;
            try {
                String data = p.data == null ? "" : p.data;
                int comma = data.indexOf(',');
                if (data.startsWith("data:") && comma >= 0) data = data.substring(comma + 1);
                bytes = Base64.getMimeDecoder().decode(data);
            } catch (IllegalArgumentException e) {
                throw new IllegalArgumentException("The selected image could not be read.");
            }
            if (bytes.length == 0) throw new IllegalArgumentException("The selected image could not be read.");
            if (bytes.length > MAX_PICTURE_BYTES) throw new IllegalArgumentException("The selected image is larger than 5 MB.");
            if (root == null) throw new IllegalArgumentException("Attachment Folder Path Not Configure Properly");   // :1170 (W4: before the save)
            plan.name = UUID.randomUUID().toString().replace("-", "") + ext;                                         // $"{Guid.NewGuid():N}{ext}"
            plan.bytes = bytes;
        } else if ("keep".equals(mode)) {
            if (update && existing(root, storedName) != null) plan.name = storedName;
        } else if (update) {
            plan.deleteAfter = existing(root, storedName);
        }
        return plan;
    }

    private void writePicture(Path root, PicturePlan plan) {
        if (plan.bytes == null) return;
        final Path target = root.resolve(plan.name);
        try {
            Files.write(target, plan.bytes, StandardOpenOption.CREATE_NEW);
        } catch (Exception e) {
            throw new IllegalStateException("The picture could not be written to the Attachment Folder Path; nothing was saved.");
        }
        if (TransactionSynchronizationManager.isSynchronizationActive()) {
            TransactionSynchronizationManager.registerSynchronization(new TransactionSynchronization() {
                @Override public void afterCompletion(int status) {
                    if (status != STATUS_COMMITTED) {
                        try { Files.deleteIfExists(target); } catch (Exception ex) { LOG.error("Rollback cleanup failed for picture {}", target); }
                    }
                }
            });
        }
    }

    /** DeleteImgList loop (:1132-1160) — after the record is updated. */
    private void deleteAfterCommit(final Path file) {
        if (file == null) return;
        if (TransactionSynchronizationManager.isSynchronizationActive()) {
            TransactionSynchronizationManager.registerSynchronization(new TransactionSynchronization() {
                @Override public void afterCommit() {
                    try { Files.deleteIfExists(file); } catch (Exception ex) { LOG.warn("Cleared picture {} could not be deleted", file); }
                }
            });
        } else {
            try { Files.deleteIfExists(file); } catch (Exception ex) { LOG.warn("Cleared picture {} could not be deleted", file); }
        }
    }

    // ============================================================================= save

    /**
     * Insert() (:1035) + BLL Save (:15) + DAL SetData (:16), in the desktop's order and in one
     * transaction: header (Insert or Update procedure — Update deletes the old detail and sub rows),
     * then for every detail row Sp_InvLabSampleAnalysisDetail_Insert and its sub rows
     * Sp_InvLabSampleAnalysiSubParamsDetail_Insert. A procedure's RAISERROR ("The record cannot be
     * inserted because another sample is referred to against this order and item.", "Record cannot
     * be updated because record has referred in PurchaseOrder") rolls everything back.
     */
    @Transactional
    public Map<String, Object> save(SampleAnalysisRequest q) {
        UserAccount u = ctx.requireAccountingUser();
        if (q == null) throw new IllegalArgumentException("No detail record found");
        boolean update = q.id > 0;
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (update && !Boolean.TRUE.equals(r.get("update"))) throw new AccessDeniedException("You do not have the Update right for Sample Analysis.");   // BtnUpdate.Enabled (:292)
        if (!update && !Boolean.TRUE.equals(r.get("save"))) throw new AccessDeniedException("You do not have the Save right for Sample Analysis.");       // BtnSave.Enabled (:290)

        // ---- formvalidation (:809), desktop order and text
        if (trim(q.reportNo).isEmpty()) throw new IllegalArgumentException("Please Report No");
        if (trim(q.analysisTypeText).isEmpty()) throw new IllegalArgumentException("Please select Analysis Type");
        if (trim(q.analysisGroupText).isEmpty()) throw new IllegalArgumentException("Please select Analysis Standard Group");
        if (q.supplierId == 0 && q.commissionAgentId == 0) throw new IllegalArgumentException("Please select Supplier or Commission Agent");

        // ---- the Yes/No question (:1046-1057)
        if (!q.confirm) throw new ConfirmRequired(update ? "Are you sure to Update?" : "Are you sure to Save?");

        // ---- W2: the record being updated
        Map<String, Object> stored = update ? requireOwned(q.id) : null;

        // ---- (:1058)
        List<SampleAnalysisRequest.Detail> details = q.details == null ? new ArrayList<>() : q.details;
        if (details.isEmpty()) throw new IllegalArgumentException("No detail record found");

        // ---- W3: what the form can hold for this group / record
        Map<Integer, String> allowedNames = new HashMap<>();
        Map<Integer, Set<Integer>> allowedSubs = new HashMap<>();
        collect(detailRowsOf(parameterRows(u, q.analysisTypeId, q.analysisGroupId, q.docDate)), allowedNames, allowedSubs);
        if (update) collect(storedDetails(q.id), allowedNames, allowedSubs);

        // ---- sub-parameter totals and the result total (:1062-1080)
        double result = 0d;
        for (SampleAnalysisRequest.Detail d : details) {
            if (d == null || !allowedNames.containsKey(d.invLabAnalysisItemsId))
                throw new IllegalArgumentException("An analysis parameter does not belong to the selected Analysis Group.");
            double totalValueParent = d.resultValue;
            result += d.resultValue;
            double totalValueSub = 0d;
            List<SampleAnalysisRequest.Sub> subs = d.subs == null ? new ArrayList<>() : d.subs;
            Set<Integer> okSubs = allowedSubs.get(d.invLabAnalysisItemsId);
            for (SampleAnalysisRequest.Sub s : subs) {
                if (s == null || okSubs == null || !okSubs.contains(s.subParameterId))
                    throw new IllegalArgumentException("A sub parameter does not belong to its analysis parameter.");
                totalValueSub += s.resultValue;
            }
            if (totalValueParent != totalValueSub && subs.size() > 0) {
                String name = allowedNames.get(d.invLabAnalysisItemsId);
                throw new IllegalArgumentException("Total Value of all sub parameters for " + name + " must be equal to value of " + name);
            }
        }
        if (result == 0d) throw new IllegalArgumentException("ResultValue cannot be equal to zero");

        // ---- W3: header references
        if (q.supplierId != 0 || q.commissionAgentId != 0) {
            Set<Integer> ids = new HashSet<>();
            for (Map<String, Object> s : repo.suppliers(u)) ids.add(toInt(ci(s, "Id")));
            if (q.supplierId != 0 && !ids.contains(q.supplierId)) throw new IllegalArgumentException("The Supplier is not in the Supplier list.");
            if (q.commissionAgentId != 0 && !ids.contains(q.commissionAgentId)) throw new IllegalArgumentException("The Commission Agent is not in the Commission Agent list.");
        }
        if (q.jobLotId != 0 && !containsId(repo.jobLots(u), q.jobLotId)) throw new IllegalArgumentException("The Job Lot is not in the Job Lot list.");
        if (q.analysisTypeId != 0 && !containsId(repo.analysisTypes(), q.analysisTypeId)) throw new IllegalArgumentException("The Analysis Type is not in the Analysis Type list.");

        // ---- pictures (:1107-1108, :1118-1126)
        Path root = pictureRoot(u);
        PicturePlan analysis = planPicture(q.analysisPic, stored == null ? "" : str(ci(stored, "AnalysisPic")), update, root);
        PicturePlan cooking = planPicture(q.cookingPic, stored == null ? "" : str(ci(stored, "CookingPic")), update, root);

        // ---- header (:1081-1114), every non-virtual property of Model 0626 (GenericProvider.SetProc)
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        int userId = ctx.currentUserId();
        int docNo = update ? toInt(ci(stored, "DocNo")) : repo.generateDocNo(u);      // W1
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("IsAccepted", "Accepted".equals(q.statusText));                          // :1098
        p.put("IsApproved", false);                                                    // :1106
        p.put("ApprovedDate", now);                                                    // :1084
        p.put("DocDate", dateOf(q.docDate));                                           // :1083
        p.put("EntryDate", now);                                                       // BLL Save :19
        p.put("ModifyDate", now);                                                      // BLL Save :20
        p.put("ApprovedUserId", 0);
        p.put("BranchesId", 0);
        p.put("CompanyId", u.getCompanyId());                                          // :1110
        p.put("DocNo", docNo);                                                         // :1082
        p.put("EntryUser", userId);                                                    // :1111
        p.put("Id", update ? q.id : 0);                                                // :1052
        p.put("DocumentTypeId", DOCUMENT_TYPE_ID);                                     // :1081
        p.put("AnalysisTypeId", q.analysisTypeId);                                     // :1093
        p.put("InvLabAnalysisGroupId", q.analysisGroupId);                             // :1092
        p.put("InvLabSampleLogRegisterId", q.sampleLogId);                             // :1094
        p.put("ItemId", q.itemId);                                                     // :1086
        p.put("JobLotId", q.jobLotId);                                                 // :1096
        p.put("ModifyUser", userId);                                                   // :1112
        p.put("OrganizationId", u.getOrganizationId());                                // :1109
        p.put("ProjectsId", 0);
        p.put("CommissionAgentId", q.commissionAgentId);                               // :1088
        p.put("SupplierCustomerId", q.supplierId);                                     // :1087
        p.put("OrderId", q.orderId);                                                   // :1091
        p.put("NoOfBagsInspected", toDouble(q.qty));                                   // :1090
        p.put("AnalysisPic", analysis.name);                                           // :1107
        p.put("Attachments", null);                                                    // never set -> not sent
        p.put("CookingPic", cooking.name);                                             // :1108
        p.put("Crop", q.cropText == null ? "" : q.cropText);                           // :1095
        p.put("InvSampleLogNo", null);                                                 // never set -> not sent
        p.put("Remarks", q.remarks == null ? "" : q.remarks);                          // :1097
        p.put("ReportNo", q.reportNo == null ? "" : q.reportNo);                       // :1085
        p.put("PartyLotRefNo", trim(q.partyLotRefNo));                                 // :1089
        p.put("SupplierCode", null);                                                   // never set -> not sent

        // ---- DAL SetData (:44-51)
        int num = repo.setHeader(update ? "Sp_InvLabSampleAnalysisHeader_Update" : "Sp_InvLabSampleAnalysisHeader_Insert", p);
        int headerId = num > 0 ? num : q.id;
        if (headerId <= 0) throw new IllegalStateException("The Sample Analysis was not saved: the procedure returned no Id.");

        // ---- DAL SetData (:52-66)
        for (SampleAnalysisRequest.Detail d : details) {
            int detailId = repo.insertDetail(d.id, d.invLabAnalysisItemsId, headerId, d.resultValue, d.remarksDetail);
            if (d.subs == null || d.subs.isEmpty()) continue;
            for (SampleAnalysisRequest.Sub s : d.subs) {
                repo.insertSubDetail(s.id, detailId, headerId, d.invLabAnalysisItemsId, s.subParameterId, s.resultValue);
            }
        }

        // ---- pictures: File.Copy (:1166-1191) and, on Update, the DeleteImgList loop (:1128-1160)
        writePicture(root, analysis);
        writePicture(root, cooking);
        deleteAfterCommit(analysis.deleteAfter);
        deleteAfterCommit(cooking.deleteAfter);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", headerId);                                                       // "success" (:1127) -> Print657(success) (:1195)
        out.put("docNo", docNo);
        out.put("message", update ? "Update Successfully" : "Save Successfully");      // :1130 / :1164
        return out;
    }

    private static void collect(List<Map<String, Object>> rows, Map<Integer, String> names, Map<Integer, Set<Integer>> subs) {
        for (Map<String, Object> d : rows) {
            int itemsId = toInt(d.get("InvLabAnalysisItemsId"));
            if (!names.containsKey(itemsId)) names.put(itemsId, str(d.get("AnalysisParameterDescription")));
            Object list = d.get("Subs");
            if (list instanceof List) {
                for (Object o : (List<?>) list) {
                    if (!(o instanceof Map)) continue;
                    Set<Integer> set = subs.get(itemsId);
                    if (set == null) { set = new HashSet<>(); subs.put(itemsId, set); }
                    set.add(toInt(((Map<?, ?>) o).get("SubParameterId")));
                }
            }
        }
    }

    private static boolean containsId(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (toInt(ci(r, "Id")) == id) return true;
        return false;
    }

    // ========================================================================== plumbing

    private UserAccount requireView() {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "view")) throw new AccessDeniedException("You do not have the View right for Sample Analysis.");
        return u;
    }

    /**
     * W2 — the header row of a record of THIS organization / company and DocumentTypeId 302.
     * A missing record answers with the text the desktop shows (GetByID returns null and the form
     * dereferences it).
     */
    private Map<String, Object> requireOwned(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = id > 0 ? repo.header(id) : null;
        if (h == null) throw new IllegalArgumentException("Object reference not set to an instance of an object.");
        if (toInt(ci(h, "OrganizationId")) != toInt(u.getOrganizationId()) || toInt(ci(h, "CompanyId")) != toInt(u.getCompanyId())
                || toInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID)
            throw new AccessDeniedException("This Sample Analysis does not belong to the current company.");
        return h;
    }

    private static Map<String, Object> idName(int id, String name) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("Name", name);
        return o;
    }

    private static String trim(String s) { return s == null ? "" : s.trim(); }

    private static boolean bool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = String.valueOf(v).trim();
        return s.equals("1") || s.equalsIgnoreCase("true");
    }

    /** yyyy-MM-dd (or an ISO date-time) to a midnight Timestamp; anything else is today, as an untouched DateTimePicker. */
    private static Timestamp dateOf(String iso) {
        try {
            String s = iso == null ? "" : iso.trim();
            if (s.length() >= 10) return Timestamp.valueOf(LocalDate.parse(s.substring(0, 10)).atStartOfDay());
        } catch (Exception ignored) { /* falls through */ }
        return Timestamp.valueOf(LocalDate.now().atStartOfDay());
    }

    private static LocalDateTime dateTime(Object v) {
        if (v == null) return null;
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().atStartOfDay();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime();
        if (v instanceof LocalDateTime) return (LocalDateTime) v;
        if (v instanceof LocalDate) return ((LocalDate) v).atStartOfDay();
        try {
            String s = String.valueOf(v).trim().replace(' ', 'T');
            if (s.length() == 10) return LocalDate.parse(s).atStartOfDay();
            return LocalDateTime.parse(s.length() > 23 ? s.substring(0, 23) : s);
        } catch (Exception e) {
            return null;
        }
    }

    /** ISO date-time text for the page ("" when null). */
    private static String iso(Object v) {
        LocalDateTime d = dateTime(v);
        return d == null ? "" : d.withNano(0).toString();
    }

    private static String isoDay(Object v) {
        LocalDateTime d = dateTime(v);
        return d == null ? "" : d.toLocalDate().toString();
    }

    /**
     * DateTime.ToShortDateString() (AnalysisGroupdtFill :572). The desktop's text follows the PC's
     * regional short-date setting; the en-US pattern M/d/yyyy is used here.
     */
    private static String shortDate(Object v) {
        LocalDateTime d = dateTime(v);
        return d == null ? "" : d.getMonthValue() + "/" + d.getDayOfMonth() + "/" + d.getYear();
    }
}
