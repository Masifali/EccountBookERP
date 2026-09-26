package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.DeptRequestToConsumableDto;
import com.mst.repositories.DeptRequestToConsumableRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Store Management (ModuleId 24) — screen 349 "Department Request To Consumable Store".
 *
 * <ul>
 *   <li>Desktop form: Architecture.WinApp.StoreManagement.DepartmentRequestToConsumableStore
 *       (DepartmentRequestToConsumableStore.cs, staged from D:\CShapEccorErp\recovered_source\ECCOUNTBOOKERP;
 *       no newer copy exists in recovery\resolved-source)</li>
 *   <li>ScreenName (base.Name, the rights key): "DepartmentRequestToConsumableStore" (designer Form.cs:3452)
 *       — identical to the screen catalogue's ScreenName and to DocumentType 1615's ScreenName.</li>
 *   <li>DocumentTypeId: 1615 (GenerateCode :307, Insert :725, txtdocno_Leave :889, HistoryGridFill :937,
 *       CommonServices.DepartmentRequestSlip1615)</li>
 *   <li>Route: /store/department-request-to-consumable — API: /api/store/department-request-to-consumable</li>
 *   <li>BLL 0068 Architecture.BLL.DepartmentRequest; DAL 0062 Architecture.DAL.DepartmentRequest;
 *       Model 0073 DepartmentRequest, 0074 DepartmentRequestDetail; BLL 0379 GlobalServicesMethods
 *       (getGlobalActiveWarehouse); BLL 0078 Projects; BLL 0583 Item; BLL 0610 UOMSchedule;
 *       BLL 0223 FixedAssetsRegister; BLL 0368 Mfg.WorkOrder; BLL 0056 GetAvgRatesAndStockInHand.</li>
 * </ul>
 *
 * Same tables and procedures as screen 338 (DepartmentRequest / DepartmentRequestDetail), document type
 * 1615: the header procedures and the detail procedure treat 1615 like 450 (details are kept on
 * update and the detail ActionTypeId is honoured). "Departments" here are warehouses of type 4, and
 * the procedures resolve their names from InvWareHouse when DocTypeId = 1615.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 * ---------------------------------------------------------------------------------------------
 *  1. Sp_DepartmentRequest_Insert ignores the Doc No sent and re-numbers with MAX(DocNo)+1 over
 *     BranchId = 0 rows WITHOUT the "ActionId &lt;&gt; 3" filter GenerateDocNo uses; after the highest
 *     request was deleted the stored number is higher than the one shown, and the success message
 *     still shows the number on screen ("Record Save Successfully " + deprt.DocNo, :783).
 *  2. BranchId, PostUser, JobLotId and StatusId are never set (0), PostDate is never set (null → not
 *     sent); IsApproved / PostState go as false. Sp_DepartmentRequest_Update writes IsApproved = 0,
 *     StatusId = 0, JobLotId = 0 and BranchId = 0 back over the stored values.
 *  3. Sp_DepartmentRequest_Update checks "same financial year" against the SaleOrder table using the
 *     request's Id (copy-paste in the procedure): an update fails with "You Cannot Update This Record
 *     in Financial Year(...)" whenever a SaleOrder with that Id exists in another year.
 *  4. Every detail row is written with WorkOrderId = the header's Work Order combo at save time (:761);
 *     a removed row carries the combo's value at the moment it was removed (:1186).
 *  5. The entry bar's Update (btnUpdateDetail_Click :1275) rewrites ItemId, Item, ItemCode, the UOM TEXT,
 *     the AssetName TEXT and RequestedQty of the row — NOT ItemUOMId / AssetId. After changing the UOM
 *     or asset of a row, the grid shows the new names and the save writes the OLD ids.
 *  6. Cancel in the entry bar (btnCancelUpdateDetial_Click :1334) only clears the fields; Update/Cancel
 *     stay visible and "+" stays hidden until New / a save / delete (Reset :633).
 *  7. Delete does not look at IsApproved (:904); the procedure refuses only when a Store Issuance header
 *     has RefDocNoId = this Id — of ANY document type (its DocumentType filter is commented out).
 *  8. Print with no document open prints the slip of EVERY 1615 request of the company
 *     (DepartmentRequestSlip1615(RecId = 0) → @Id not sent, :1406).
 *  9. History: the tab (HistoryGridFill(50), :1028) and "Load All" (HistoryGridFill(), :1057) send the
 *     same call — BLL 0068 DepartmentRequestHistory has no NoOfRecords parameter — so both list every
 *     1615 request of the company, all years, all users, newest first. No rows → the grid keeps what
 *     it showed (:941). One row per request (the first detail row's Work Order), dates shown without
 *     time (ToShortDateString, :965); a request never modified shows Modify Date 01-Jan-1900
 *     (Conversion.ToDateTime(DBNull)).
 * 10. Doc Date on New / after save goes back to now (Reset :629); the Project combo, the barcode box and
 *     the stock label are not touched by Reset.
 * 11. txtdocno is ReadOnly but focusable; leaving it (txtdocno_Leave :873) looks the number up with
 *     GetIdByDocNo and, when a request has it, opens that request — on an opened request this re-reads
 *     it and discards unsaved grid edits.
 * 12. The duplicate check in the entry bar is by ItemId only (:1218). FormValidationDetail requires an
 *     asset on every row and rejects a quantity of 0 (:584).
 * 13. The Work Order list is not distinct (see the repository) and the Department lists are the
 *     warehouses of type 4 allocated to the user's login branch.
 * 14. The stock label (BalanceStock :1352) is SUM(StockWeightIn) - SUM(StockWeightOut) of the item up to
 *     the Doc Date, formatted #,##0.## — shown even for item 0.
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web only)
 * ---------------------------------------------------------------------------------------------
 *  1. Tenancy: a request is opened, updated, printed or deleted only when it belongs to the signed-in
 *     user's organization/company and has DocTypeId 1615 (GetByID on the desktop reads any Id).
 *  2. Doc No is never taken from the request: the generator on insert (what the desktop showed), the
 *     stored header on update.
 *  3. Rights are checked on the server (save / update / delete / print) as well as on the buttons.
 *  4. "Record not Update because record has approved" is checked against the STORED IsApproved, not only
 *     the flag read when the document was opened.
 *  5. Detail Ids sent for update / removal must belong to the request being saved (the detail procedure
 *     updates by Id alone).
 *  6. GetByID on a request deleted meanwhile throws "Index was out of range" on the desktop and leaves
 *     RecId set; the page answers "Record not Found" and keeps its previous state.
 *  7. The ItemSearchByCode configuration is read live instead of from the login-time cache; the
 *     Department list is read live instead of from globalWarehousesWithBranches (Refresh re-reads it on
 *     the desktop too).
 *  8. grdDetail_ColumnButtonClick deletes through both the DataTable and the GridEX row (:1190-1195); the
 *     page removes exactly the clicked row.
 *
 * NOT PORTED: attachments (btnAttachment, DMS, the NoOfAttachments link), saved grid layouts / field
 * chooser / grid print-export (ctrlGrdBar1/2), the shortcut-key popup and keyboard shortcuts, "Define
 * Department" (Define_Department) and "Define Asset" (frmLookUpDefineAsset) — no web pages — the hidden
 * Work Station From/To combos (Visible = false, never filled), and the Crystal layout
 * 1615-DepartmentRequestToConsumableStoreSlip.rpt (the slip returns the procedure's rows).
 */
@Service
public class DeptRequestToConsumableService {

    public static final int DOCUMENT_TYPE_ID = 1615;
    public static final String SCREEN_NAME = "DepartmentRequestToConsumableStore";

    private final DeptRequestToConsumableRepository repo;
    private final StoreIssuanceRepository shared;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public DeptRequestToConsumableService(DeptRequestToConsumableRepository repo, StoreIssuanceRepository shared,
                                          StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.shared = shared;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ==================================================================================== load

    /** EccountBook_Load (Form.cs:225): rights, ItemSearchByCode, GenerateCode and the five fills. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));                                 // :232
        out.put("itemSearchByCode", StoreIssuanceService.toBool(shared.config(u, "ItemSearchByCode")));   // :254
        out.put("docNo", docNo(u));                                                // :276 GenerateCode
        out.put("projects", repo.projects(u));                                     // :278 ProjectFill (Load only)
        out.putAll(lists(u));                                                      // :277, :279-281
        return out;
    }

    /** btnRefresh_Click (Form.cs:671): DeprtmentFill, FixedAssest, ItemNameFill, WorkOrderFill — no projects. */
    public Map<String, Object> lists() { return lists(ctx.requireAccountingUser()); }

    private Map<String, Object> lists(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("departments", repo.departments(u, branch(u)));
        out.put("assets", repo.assets(u));
        out.put("items", repo.items(u, "14,17"));
        out.put("workOrders", repo.workOrders(u));
        return out;
    }

    /** GenerateCode (Form.cs:293) — Reset() asks for it again (:639). */
    public int docNo() { return docNo(ctx.requireAccountingUser()); }

    private int docNo(UserAccount u) {
        return repo.generateDocNo(u, DOCUMENT_TYPE_ID, ctx.currentFinancialYearId());
    }

    /** txtdocno_Leave (Form.cs:873) — 0 when none. */
    public int idByDocNo(int docNo) {
        UserAccount u = ctx.requireAccountingUser();
        return repo.idByDocNo(u, DOCUMENT_TYPE_ID, docNo, ctx.currentFinancialYearId());
    }

    /** ItemUOMFill (Form.cs:397). */
    public List<Map<String, Object>> uoms(int itemId) {
        return repo.uoms(ctx.requireAccountingUser(), itemId);
    }

    /** BalanceStock (Form.cs:1352) — DocDate.Value (the picker carries the time of day). */
    public Map<String, Object> stock(int itemId, String docDate) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("balQty", repo.stockInHand(u, itemId, StoreIssuanceService.pickerDate(docDate)));
        return out;
    }

    /** txtBarcodeReader_KeyDown (Form.cs:1426) → GetItemIdByBarcode (:1444). */
    public int itemIdByBarcode(String barcode) {
        UserAccount u = ctx.requireAccountingUser();
        return repo.itemIdByBarcode(u, barcode == null ? "" : barcode.trim());
    }

    // ================================================================================= history

    /** HistoryGridFill (Form.cs:928) — de-duplicated by Id (:963), projected to the grid's columns. */
    public List<Map<String, Object>> history() {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> r : repo.history(u, DOCUMENT_TYPE_ID, 0)) {
            int id = toInt(ci(r, "Id"));
            if (!seen.add(id)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("DocTypeId", toInt(ci(r, "DocTypeId")));
            o.put("DocDate", localText(ci(r, "DocDate")));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("WorkOrderNo", str(ci(r, "WorkOrderNo")));
            o.put("DepartmentNameFrom", str(ci(r, "DepartmentNameFrom")));
            o.put("ToDepartmentName", str(ci(r, "ToDepartmentName")));
            o.put("RemarksHeader", str(ci(r, "RemarksHeader")));
            o.put("EntryUser", str(ci(r, "EntryUser")));
            o.put("EntryDate", shortDate(ci(r, "EntryDate")));                    // ToShortDateString()
            o.put("ModifyUser", str(ci(r, "ModifyUser")));
            o.put("ModifyDate", shortDate(ci(r, "ModifyDate")));
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    // ==================================================================================== read

    /** ReadById (Form.cs:830) / DataGridHistory_SelectionChanged (:1112) — GetByID (header + detail). */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = owned(u, id);
        if (h == null) return null;
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(ci(h, "Id")));
        out.put("DocNo", toInt(ci(h, "DocNo")));
        out.put("DocDate", localText(ci(h, "DocDate")));
        out.put("ProjectId", toInt(ci(h, "ProjectId")));
        out.put("FromDepartmentId", toInt(ci(h, "FromDepartmentId")));
        out.put("ToDepartmentId", toInt(ci(h, "ToDepartmentId")));
        out.put("RemarksHeader", str(ci(h, "RemarksHeader")));
        out.put("IsApproved", toInt(ci(h, "IsApproved")) != 0);
        List<Map<String, Object>> rows = new ArrayList<>();
        int workOrderId = 0;
        boolean first = true;
        for (Map<String, Object> d : repo.details(id)) {                           // :856 dtdetail.Rows.Add(...)
            if (first) { workOrderId = toInt(ci(d, "WorkOrderId")); first = false; }   // :852 detail[0].WorkOrderId
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("ItemId", toInt(ci(d, "ItemId")));
            o.put("Item", str(ci(d, "ItemName")));
            o.put("ItemCode", str(ci(d, "ItemCode")));
            o.put("ItemUOMId", toInt(ci(d, "ItemUOMId")));
            o.put("UOM", str(ci(d, "UOMCode")));
            o.put("AssetId", toInt(ci(d, "AssetId")));
            o.put("AssetName", str(ci(d, "AssetName")));
            o.put("RequestedQty", toDouble(ci(d, "RequestedQty")));
            rows.add(o);
        }
        out.put("WorkOrderId", workOrderId);
        out.put("rows", rows);
        return out;
    }

    /** Slip (Form.cs:1414) → CommonServices.DepartmentRequestSlip1615 — the report's rows (Crystal not ported). */
    public List<Map<String, Object>> slip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "print")) throw new IllegalStateException("You do not have the Print right for this screen.");
        if (id != 0 && owned(u, id) == null) return null;
        return repo.history(u, DOCUMENT_TYPE_ID, id);
    }

    // ================================================================================== delete

    /** btnDelete_Click (Form.cs:904) → BLL 0068 DeleteByID(UserAccount.ID, RecId). Not in a transaction. */
    public Map<String, Object> delete(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "delete")) throw new IllegalStateException("You do not have the Delete right for this screen.");
        if (id <= 0 || owned(u, id) == null) throw new IllegalArgumentException("No Record Found For Deletion");
        repo.delete(u.getId(), id);
        return ok("Delete Record Successfully", id, 0);                            // :913
    }

    // ==================================================================================== save

    /**
     * Insert() (Form.cs:700) → BLL 0068 Save → DAL 0062 SetData: header, then every detail (grid rows,
     * then the removed rows) through Sp_DepartmentRequestDetail_Insert, all in ONE transaction.
     */
    @Transactional
    public Map<String, Object> save(DeptRequestToConsumableDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id == null ? 0 : dto.Id;
        Map<String, Object> existing = null;
        if (recId == 0) {
            if (!rights.has(SCREEN_NAME, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        } else {
            if (!rights.has(SCREEN_NAME, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
            existing = owned(u, recId);
            if (existing == null) throw new IllegalArgumentException("RecId Not Found");
            /* btnUpdate_Click :814 — before FormValidation. */
            if (toInt(ci(existing, "IsApproved")) != 0) throw new IllegalArgumentException("Record not Update because record has approved");
        }

        /* FormValidation (:549) — same messages, same order. */
        int docNo = existing != null ? toInt(ci(existing, "DocNo")) : docNo(u);
        if (i(dto.ProjectId) == 0) throw new IllegalArgumentException("Project Field is Required");
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");
        if (i(dto.WorkOrderId) == 0) throw new IllegalArgumentException("Work No Field is Required");
        if (i(dto.FromDepartmentId) == 0) throw new IllegalArgumentException("Department From Field is Required");
        if (i(dto.ToDepartmentId) == 0) throw new IllegalArgumentException("Department To Field is Required");

        List<DeptRequestToConsumableDto.Row> rows = dto.rows == null ? new ArrayList<>() : dto.rows;
        List<DeptRequestToConsumableDto.Row> removed = (recId > 0 && dto.removed != null) ? dto.removed : new ArrayList<>();
        /* :741 */
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid record not found");

        /* Deviation 5 — a detail Id must be one of this request's stored rows. */
        if (existing != null) {
            Set<Integer> own = new HashSet<>();
            for (Map<String, Object> d : repo.details(recId)) own.add(toInt(ci(d, "Id")));
            for (DeptRequestToConsumableDto.Row r : rows)
                if (i(r.Id) > 0 && !own.contains(r.Id)) throw new IllegalArgumentException("Detail row " + r.Id + " does not belong to this request.");
            for (DeptRequestToConsumableDto.Row r : removed)
                if (i(r.Id) > 0 && !own.contains(r.Id)) throw new IllegalArgumentException("Detail row " + r.Id + " does not belong to this request.");
        }

        Timestamp today = Timestamp.valueOf(LocalDate.now().atStartOfDay());       // DateTime.Today
        /* :725-739 — Model 0073 property order; GenericProvider.SetProc sends every non-virtual property,
           PostDate (DateTime?, never set) is null and therefore not sent. */
        Map<String, Object> head = params(
                "IsApproved", false,
                "PostState", false,
                "DocDate", docDate(dto.DocDate, existing),                         // :726 DocDate.Value
                "EntryDate", today,                                                // :732
                "ModifyDate", today,                                               // :734
                "PostDate", null,
                "CompanyId", u.getCompanyId(),
                "DocTypeId", DOCUMENT_TYPE_ID,                                     // :725
                "DocNo", docNo,                                                    // :727
                "EntryUser", u.getId(),
                "FromDepartmentId", i(dto.FromDepartmentId),
                "Id", recId,
                "JobLotId", 0,
                "ModifyUser", u.getId(),
                "OrganizationId", u.getOrganizationId(),
                "PostUser", 0,
                "ProjectId", i(dto.ProjectId),                                     // :728
                "StatusId", 0,
                "ActionId", recId == 0 ? 1 : 2,                                    // BLL 0068 Save
                "BranchId", 0,
                "ToDepartmentId", i(dto.ToDepartmentId),
                "FinancialYearId", ctx.currentFinancialYearId(),
                "RemarksHeader", dto.RemarksHeader == null ? "" : dto.RemarksHeader,
                "ScreenName", SCREEN_NAME);                                        // :739 base.Name

        int num = repo.setProc(recId == 0 ? "Sp_DepartmentRequest_Insert" : "Sp_DepartmentRequest_Update", head);
        int id = num > 0 ? num : recId;                                           // DAL 0062 SetData
        if (id <= 0) throw new RuntimeException("The request could not be saved: no id was returned.");

        /* :745-764 grid rows (ActionTypeId 2 when the row has an Id, else 1), then :768-774 the removed ones. */
        int workOrderId = i(dto.WorkOrderId);
        for (DeptRequestToConsumableDto.Row r : rows) {
            int detailId = i(r.Id);
            repo.setProc("Sp_DepartmentRequestDetail_Insert", detail(id, detailId, detailId > 0 ? 2 : 1,
                    i(r.ItemId), i(r.ItemUOMId), i(r.AssetId), r.RequestedQty == null ? 0d : r.RequestedQty, workOrderId));
        }
        for (DeptRequestToConsumableDto.Row r : removed) {
            repo.setProc("Sp_DepartmentRequestDetail_Insert", detail(id, i(r.Id), 3,
                    i(r.ItemId), i(r.ItemUOMId), i(r.AssetId), r.RequestedQty == null ? 0d : r.RequestedQty, i(r.WorkOrderId)));
        }

        /* :778 / :783 — the Doc No shown on the form; then Reset (:785) and Slip(success) (:788). */
        String msg = recId > 0 ? "Record Update Successfully " + docNo : "Record Save Successfully " + docNo;
        return ok(msg, id, docNo(u));
    }

    /** Model 0074 property order. WorkStationFromId / WorkStationToId / ItemConditionId are never set (0). */
    private static Map<String, Object> detail(int deptReqId, int id, int actionTypeId, int itemId, int uomId,
                                              int assetId, double qty, int workOrderId) {
        return params(
                "RequestedQty", qty,
                "DeptReqId", deptReqId,
                "Id", id,
                "ItemId", itemId,
                "ItemUOMId", uomId,
                "AssetId", assetId,
                "ActionTypeId", actionTypeId,
                "WorkOrderId", workOrderId,
                "WorkStationFromId", 0,
                "WorkStationToId", 0,
                "ItemConditionId", 0);
    }

    // ================================================================================= helpers

    /**
     * DocDate.Value at save time. New document: the page's day with the current time (the picker holds
     * DateTime.Now from Load / Reset). Opened document: ReadById assigned the stored DocDate (a datetime
     * column), so its time of day is kept and only the day the operator picked is applied.
     */
    private static Timestamp docDate(String yyyyMMdd, Map<String, Object> existing) {
        if (existing == null) return StoreIssuanceService.pickerDate(yyyyMMdd);
        LocalDate day = (yyyyMMdd == null || yyyyMMdd.trim().isEmpty()) ? LocalDate.now()
                : LocalDate.parse(yyyyMMdd.trim().substring(0, 10));
        LocalTime time = LocalTime.MIDNIGHT;
        Object stored = ci(existing, "DocDate");
        if (stored instanceof Timestamp) time = ((Timestamp) stored).toLocalDateTime().toLocalTime();
        else if (stored instanceof LocalDateTime) time = ((LocalDateTime) stored).toLocalTime();
        return Timestamp.valueOf(LocalDateTime.of(day, time));
    }

    /**
     * A date cell as local "yyyy-MM-ddTHH:mm:ss" text — a java.sql.Timestamp would otherwise be written
     * by Jackson in UTC and could show the previous / next day on the page.
     */
    private static String localText(Object v) {
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString();
        return v == null ? null : String.valueOf(v);
    }

    /** "yyyy-MM-dd" of a date cell; a null cell is Conversion.ToDateTime(DBNull) = 1900-01-01. */
    private static String shortDate(Object v) {
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        String s = v == null ? "" : String.valueOf(v);
        return s.length() >= 10 ? s.substring(0, 10) : "1900-01-01";
    }

    /** The header must exist (ReadById filters ActionId &lt;&gt; 3), be DocTypeId 1615 and the user's company. */
    private Map<String, Object> owned(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocTypeId")) != DOCUMENT_TYPE_ID) return null;
        if (toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static int i(Integer v) { return v == null ? 0 : v; }

    private static Map<String, Object> ok(String message, int id, int nextDocNo) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        m.put("id", id);
        m.put("docNo", nextDocNo);
        return m;
    }
}
