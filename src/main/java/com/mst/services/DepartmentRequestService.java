package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.DepartmentRequestDto;
import com.mst.repositories.DepartmentRequestRepository;
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

import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Store Management (ModuleId 24) — screen 338 "Department Request".
 *
 * <ul>
 *   <li>Desktop form: Architecture.WinApp.StoreManagement.frmDepartmentRequest (frmDepartmentRequest.cs)</li>
 *   <li>ScreenName (base.Name, the rights key): "frmDepartmentRequest"</li>
 *   <li>DocumentTypeId: 450 (constructor :237, Insert :749, HistoryGridFill :927)</li>
 *   <li>Route: /store/department-request — API: /api/store/department-request</li>
 *   <li>BLL 0068 Architecture.BLL.DepartmentRequest; DAL 0062 Architecture.DAL.DepartmentRequest;
 *       Model 0073 DepartmentRequest, 0074 DepartmentRequestDetail; BLL 0067 Department,
 *       0223 FixedAssetsRegister, 0583 Item (barcode), 0056 GetAvgRatesAndStockInHand.</li>
 * </ul>
 *
 * Store Issuance 322 reads what this screen saves through USP_GetPendingDepartmentRequestForIssuance
 * (DocTypeId 450, ActionId &lt;&gt; 3, detail ActionTypeId &lt;&gt; 3, inner joins on Item / ItemCategory /
 * V_UomScheduleAndUom). Rows are written by the same two procedures with the same parameters as the
 * desktop, so the data stays compatible.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 * ---------------------------------------------------------------------------------------------
 *  1. Sp_DepartmentRequest_Insert ignores the Doc No sent and re-numbers with
 *     MAX(DocNo)+1 WITHOUT the "ActionId &lt;&gt; 3" filter GenerateDocNo uses, so when the highest
 *     request was deleted the stored Doc No is higher than the one shown; the success message still
 *     shows the number on screen ("Record Save Successfully " + deprt.DocNo, :808).
 *  2. Sp_DepartmentRequest_Update checks "same financial year" against the SaleOrder table using the
 *     Department Request's Id (a copy-paste in the procedure): an update fails with "You Cannot Update
 *     This Record in Financial Year(...)" whenever a SaleOrder with that Id exists in another year.
 *  3. Update sends IsApproved = 0, StatusId = 0, JobLotId = 0 (model defaults) and the procedure
 *     writes them; ProjectId is the user's BranchesId (:748).
 *  4. Delete does not look at IsApproved (:893); the procedure refuses only when a Store Issuance
 *     header has RefDocNoId = this Id — of ANY document type (its DocumentType filter is commented out).
 *  5. Print with no document open prints the slip of EVERY Department Request of the company
 *     (DepartmentSlip451(RecId = 0) → @Id not sent, :1563).
 *  6. A removed stored row is sent with only Id/ItemId/ItemUOMId/AssetId/RequestedQty and
 *     ActionTypeId 3 (ItemConditionId 0, :1272); rows added on an opened document go as
 *     ActionTypeId 1, stored rows as ActionTypeId 2 (:767-782).
 *  7. Duplicate check in the entry bar is by ItemId only, whatever the condition / asset (:1336).
 *  8. History uses @Activity 'ReadAll' with no entry-user / "CanView AllRecord" filter (:976).
 *  9. Doc Date on New / after save goes back to now (Reset :631) — unlike the issuance screens.
 * 10. Item Condition is not cleared by New or by the entry bar reset (Reset :614, ResetDetail :654).
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web only)
 * ---------------------------------------------------------------------------------------------
 *  1. Tenancy: a request is opened, updated, printed or deleted only when it belongs to the
 *     signed-in user's organization/company and has DocTypeId 450.
 *  2. Doc No is never taken from the request: the generator on insert (what the desktop showed),
 *     the stored header on update.
 *  3. "Record not Update because record has approved" is checked against the STORED IsApproved,
 *     not only the flag read when the document was opened (the desktop would otherwise update a
 *     request approved in between and, by note 3, reset it to unapproved).
 *  4. Detail Ids sent for update / removal must belong to the request being saved. The desktop
 *     never clears lstRemoveRecord on ReadById, so a row removed on one request and a different
 *     request opened afterwards soft-deletes the FIRST request's row on the next update (the detail
 *     procedure updates by Id alone). The page clears the list on open and the server refuses a
 *     foreign Id.
 *  5. A header procedure that returns no id on insert is refused (the desktop would write details
 *     with DeptReqId 0).
 *  6. The two configuration flags (ItemSearchByCode, PackingMaterialItemsNotIncludeOnStore) are read
 *     live from the allocation table instead of the login-time global cache.
 *  7. The entry bar's Update (btnUpdateDetail_Click:1391) tests only ActiveRow != null for Item, UOM
 *     and AssetName, so picking the blank default row passes on the desktop; a web select cannot tell
 *     "blank row picked" from "nothing picked", so 0 is treated as missing, as Add_Click does.
 *  8. ReadById:854 assigns RecId BEFORE GetByID; when the read fails the desktop keeps that RecId
 *     with the previous document's fields on screen. The page keeps its previous state untouched.
 *
 * NOT PORTED: attachments (btnAttachment, DMS), saved grid layouts (ctrlGrdBar), the shortcut-key
 * popup and keyboard shortcuts, "Define Department" (opens Define_Department, which has no web page),
 * the Crystal layout 451-RptDepartmentRequestSlip.rpt (the slip returns the procedure's rows).
 */
@Service
public class DepartmentRequestService {

    public static final int DOCUMENT_TYPE_ID = 450;
    public static final String SCREEN_NAME = "frmDepartmentRequest";

    private final DepartmentRequestRepository repo;
    private final StoreIssuanceRepository shared;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public DepartmentRequestService(DepartmentRequestRepository repo, StoreIssuanceRepository shared,
                                    StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.shared = shared;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ==================================================================================== load

    /** InitializeComponentMethod:300 (and btnRefresh_Click:679 — same reads). */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        boolean packingNotIncluded = StoreIssuanceService.toBool(shared.config(u, "PackingMaterialItemsNotIncludeOnStore"));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));
        out.put("docNo", docNo(u));
        out.put("itemSearchByCode", StoreIssuanceService.toBool(shared.config(u, "ItemSearchByCode")));
        out.put("packingMaterialItemsNotIncludeOnStore", packingNotIncluded);
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(shared.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("items", repo.items(u, packingNotIncluded));                      // ItemDtsFillFromGlobal:402
        out.put("uoms", repo.uoms(u));                                            // globalUomSchedule
        out.put("itemConditions", shared.itemConditions());                       // ItemConditionBindFromGlobal:534 (Id != 4)
        out.put("departments", repo.departments(u));                              // DepartmentDbCall:367
        out.put("assets", repo.assets(u));                                        // FixedAssestDbCall:498
        return out;
    }

    /** DocumentNoDbCall:565 — Reset() asks for it again (:640). */
    public int docNo() { return docNo(ctx.requireAccountingUser()); }

    private int docNo(UserAccount u) {
        return repo.generateDocNo(u, DOCUMENT_TYPE_ID, ctx.currentFinancialYearId(), branch(u));
    }

    /**
     * BalanceStock:1509 — GetAvgRateQtyAndStockInHand with org, company, item, DocDate.Value and the
     * condition when non-zero (no document type, no RecId, no warehouse). QtyInHand, Math.Round 2.
     * Returns null when the procedure gave no row (the label is then left at "0", still hidden if it was).
     */
    public Map<String, Object> stock(int itemId, int itemConditionId, String docDate) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        if (itemId == 0) { out.put("found", false); out.put("qtyInHand", 0d); return out; }
        Map<String, Object> r = shared.avgRateAndStock(u, itemId, StoreIssuanceService.pickerDate(docDate),
                itemConditionId, 0, 0, 0, 0);
        out.put("found", r != null);
        out.put("qtyInHand", r == null ? 0d : Math.rint(toDouble(r.get("QtyInHand")) * 100d) / 100d);   // :1535 Math.Round(double, 2) on the binary value
        return out;
    }

    /** txtBarcodeReader_KeyDown:1571 → GetItemIdByBarcode:1589. */
    public int itemIdByBarcode(String barcode) {
        UserAccount u = ctx.requireAccountingUser();
        return repo.itemIdByBarcode(u, barcode == null ? "" : barcode.trim());
    }

    // ================================================================================= history

    /**
     * HistoryGridFill:917. dateField is the checked radio: "doc" (drdocdate), "entry", "modify",
     * "approved"; a blank from/to is an unchecked picker. Rows are de-duplicated by Id (:998) and
     * projected to the desktop grid's columns.
     */
    public List<Map<String, Object>> history(String dateField, String from, String to, int docNoFrom, int docNoTo) {
        UserAccount u = ctx.requireAccountingUser();
        Timestamp f = StoreIssuanceService.dateOnly(from), t = StoreIssuanceService.dateOnly(to);
        String mode = dateField == null ? "doc" : dateField;
        List<Map<String, Object>> rows = repo.formHistory(u, DOCUMENT_TYPE_ID, ctx.currentFinancialYearId(), branch(u),
                "doc".equals(mode) ? f : null, "doc".equals(mode) ? t : null,
                "entry".equals(mode) ? f : null, "entry".equals(mode) ? t : null,
                "modify".equals(mode) ? f : null, "modify".equals(mode) ? t : null,
                "approved".equals(mode) ? f : null, "approved".equals(mode) ? t : null,
                docNoFrom, docNoTo);
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> r : rows) {
            int id = toInt(r.get("Id"));
            if (!seen.add(id)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("DocTypeId", toInt(r.get("DocTypeId")));
            o.put("DocDate", r.get("DocDate"));
            o.put("DocNo", str(r.get("DocNo")));
            o.put("DepartmentNameFrom", str(r.get("FromDepartmentName")));
            o.put("ToDepartmentName", str(r.get("ToDepartmentName")));
            o.put("RemarksHeader", str(r.get("RemarksHeader")));
            o.put("EntryUser", str(r.get("CreatedBy")));
            o.put("EntryDate", r.get("EntryDate"));
            o.put("ModifyUser", str(r.get("ModifiedBy")));
            o.put("ModifyDate", r.get("ModifyDate"));
            o.put("NoOfAttachments", toInt(r.get("NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    // ==================================================================================== read

    /** ReadById:850 / DataGridHistory_SelectionChanged:1143 — GetByID (header + detail). */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = owned(u, id);
        if (h == null) return null;
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(h.get("Id")));
        out.put("DocNo", toInt(h.get("DocNo")));
        out.put("DocDate", h.get("DocDate"));
        out.put("FromDepartmentId", toInt(h.get("FromDepartmentId")));
        out.put("ToDepartmentId", toInt(h.get("ToDepartmentId")));
        out.put("RemarksHeader", str(h.get("RemarksHeader")));
        out.put("IsApproved", toInt(h.get("IsApproved")) != 0);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.details(id)) {                         // :870 dtdetail.Rows.Add(...)
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(d.get("Id")));
            o.put("ItemId", toInt(d.get("ItemId")));
            o.put("Item", str(d.get("ItemName")));
            o.put("ItemCode", str(d.get("ItemCode")));
            o.put("ItemConditionId", toInt(d.get("ItemConditionId")));
            o.put("ItemCondition", str(d.get("ItemCondition")));
            o.put("ItemUOMId", toInt(d.get("ItemUOMId")));
            o.put("UOM", str(d.get("UOMCode")));
            o.put("AssetId", toInt(d.get("AssetId")));
            o.put("AssetName", str(d.get("AssetName")));
            o.put("RequestedQty", toDouble(d.get("RequestedQty")));
            rows.add(o);
        }
        out.put("rows", rows);
        return out;
    }

    /** CommonServices.DepartmentSlip451 — the report's rows (Crystal layout not ported). */
    public List<Map<String, Object>> slip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "print")) throw new IllegalStateException("You do not have the Print right for this screen.");
        if (id != 0 && owned(u, id) == null) return null;
        return repo.slip(u, DOCUMENT_TYPE_ID, id);
    }

    // ================================================================================== delete

    /** btnDelete_Click:893 → BLL 0068 DeleteByID(UserAccount.ID, RecId). Not in a transaction on the desktop. */
    public Map<String, Object> delete(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "delete")) throw new IllegalStateException("You do not have the Delete right for this screen.");
        if (id <= 0) throw new IllegalArgumentException("No Record Found For Deletion");
        if (owned(u, id) == null) throw new IllegalArgumentException("No Record Found For Deletion");
        repo.delete(u.getId(), id);
        return ok("Delete Record Seccessfully", id, 0);                           // :902 verbatim
    }

    // ==================================================================================== save

    /** Insert():711 → BLL 0068 Save → DAL 0062 SetData, header and every detail in ONE transaction. */
    @Transactional
    public Map<String, Object> save(DepartmentRequestDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id == null ? 0 : dto.Id;
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) {
            throw new IllegalStateException("You do not have the Save right for this screen.");
        }
        Map<String, Object> existing = null;
        if (recId != 0) {
            if (!rights.has(SCREEN_NAME, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
            existing = owned(u, recId);
            if (existing == null) throw new IllegalArgumentException("Record not Found");
        }
        List<DepartmentRequestDto.Row> rows = dto.rows == null ? new ArrayList<>() : dto.rows;

        /* :719 */
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid record not found");
        /* FormValidation:591 — same messages, same order. */
        int docNo = existing != null ? toInt(existing.get("DocNo")) : docNo(u);
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");
        if (i(dto.FromDepartmentId) == 0) throw new IllegalArgumentException("Department From Field is Required");
        if (i(dto.ToDepartmentId) == 0) throw new IllegalArgumentException("Department To Field is Required");
        /* :730 */
        if (existing != null && toInt(existing.get("IsApproved")) != 0) {
            throw new IllegalArgumentException("Record not Update because record has approved");
        }

        /* Deviation 4 — a detail Id must be one of this request's stored rows. */
        Set<Integer> ownIds = new HashSet<>();
        if (existing != null) for (Map<String, Object> d : repo.details(recId)) ownIds.add(toInt(d.get("Id")));
        if (existing != null) {
            for (DepartmentRequestDto.Row r : rows) {
                if (i(r.Id) > 0 && !ownIds.contains(r.Id)) throw new IllegalArgumentException("Detail row " + r.Id + " does not belong to this request.");
            }
            if (dto.removed != null) for (DepartmentRequestDto.Row r : dto.removed) {
                if (i(r.Id) > 0 && !ownIds.contains(r.Id)) throw new IllegalArgumentException("Detail row " + r.Id + " does not belong to this request.");
            }
        }

        LocalDate today = LocalDate.now();
        Timestamp todayTs = Timestamp.valueOf(today.atStartOfDay());
        int branchesId = branch(u);

        /* :744-761 — Model 0073 property order (GenericProvider.SetProc sends every non-virtual one). */
        Map<String, Object> head = params(
                "IsApproved", false,
                "PostState", false,
                "DocDate", docDate(dto.DocDate, existing),
                "EntryDate", todayTs,                                              // DateTime.Today
                "ModifyDate", todayTs,
                "PostDate", todayTs,
                "CompanyId", u.getCompanyId(),
                "DocTypeId", DOCUMENT_TYPE_ID,                                     // :749
                "DocNo", docNo,
                "EntryUser", u.getId(),
                "FromDepartmentId", i(dto.FromDepartmentId),
                "Id", recId,
                "JobLotId", 0,
                "ModifyUser", u.getId(),
                "OrganizationId", u.getOrganizationId(),
                "PostUser", u.getId(),
                "ProjectId", branchesId,                                           // :748 ProjectId = BranchesId
                "StatusId", 0,
                "ActionId", recId == 0 ? 1 : 2,                                    // BLL 0068 Save
                "BranchId", branchesId,
                "ToDepartmentId", i(dto.ToDepartmentId),
                "FinancialYearId", ctx.currentFinancialYearId(),
                "RemarksHeader", dto.RemarksHeader == null ? "" : dto.RemarksHeader,
                "ScreenName", SCREEN_NAME);                                        // :761 base.Name

        int num = repo.setProc(recId == 0 ? "Sp_DepartmentRequest_Insert" : "Sp_DepartmentRequest_Update", head);
        int id = num > 0 ? num : recId;                                           // DAL 0062 SetData
        if (id <= 0) throw new IllegalStateException("The request could not be saved: no id was returned.");

        /* :763-789 grid rows, then :790 the removed ones — each through Sp_DepartmentRequestDetail_Insert. */
        for (DepartmentRequestDto.Row r : rows) {
            int detailId = recId > 0 ? i(r.Id) : 0;                               // :767
            repo.setProc("Sp_DepartmentRequestDetail_Insert", detail(id, detailId, detailId > 0 ? 2 : 1,
                    i(r.ItemId), i(r.ItemUOMId), i(r.AssetId), r.RequestedQty == null ? 0d : r.RequestedQty,
                    i(r.ItemConditionId)));
        }
        if (recId > 0 && dto.removed != null) {
            for (DepartmentRequestDto.Row r : dto.removed) {
                repo.setProc("Sp_DepartmentRequestDetail_Insert", detail(id, i(r.Id), 3,
                        i(r.ItemId), i(r.ItemUOMId), i(r.AssetId), r.RequestedQty == null ? 0d : r.RequestedQty, 0));
            }
        }

        /* :803 / :808 — the Doc No shown on the form, then Reset; "success" is what is printed. */
        String msg = recId > 0 ? "Record Update Successfully " + docNo : "Record Save Successfully " + docNo;
        return ok(msg, id, docNo(u));
    }

    /** Model 0074 property order. WorkOrderId / WorkStationFromId / WorkStationToId are 0 (never set). */
    private static Map<String, Object> detail(int deptReqId, int id, int actionTypeId, int itemId, int uomId,
                                              int assetId, double qty, int conditionId) {
        return params(
                "RequestedQty", qty,
                "DeptReqId", deptReqId,
                "Id", id,
                "ItemId", itemId,
                "ItemUOMId", uomId,
                "AssetId", assetId,
                "ActionTypeId", actionTypeId,
                "WorkOrderId", 0,
                "WorkStationFromId", 0,
                "WorkStationToId", 0,
                "ItemConditionId", conditionId);
    }

    // ================================================================================= helpers

    /**
     * DocDate.Value at save time. New document: the page's day with the current time (the picker
     * holds DateTime.Now from Reset / Load). Opened document: ReadById assigned the stored DocDate,
     * so its time of day is kept and only the day the operator picked is applied.
     */
    private static Timestamp docDate(String yyyyMMdd, Map<String, Object> existing) {
        if (existing == null) return StoreIssuanceService.pickerDate(yyyyMMdd);
        LocalDate day = (yyyyMMdd == null || yyyyMMdd.trim().isEmpty()) ? LocalDate.now()
                : LocalDate.parse(yyyyMMdd.trim().substring(0, 10));
        LocalTime time = LocalTime.MIDNIGHT;
        Object stored = existing.get("DocDate");
        if (stored instanceof Timestamp) time = ((Timestamp) stored).toLocalDateTime().toLocalTime();
        else if (stored instanceof LocalDateTime) time = ((LocalDateTime) stored).toLocalTime();
        return Timestamp.valueOf(LocalDateTime.of(day, time));
    }

    /** The header must exist (ReadById filters ActionId &lt;&gt; 3), be DocTypeId 450 and the user's company. */
    private Map<String, Object> owned(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(h.get("DocTypeId")) != DOCUMENT_TYPE_ID) return null;
        if (toInt(h.get("CompanyId")) != u.getCompanyId()) return null;
        if (toInt(h.get("OrganizationId")) != u.getOrganizationId()) return null;
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
