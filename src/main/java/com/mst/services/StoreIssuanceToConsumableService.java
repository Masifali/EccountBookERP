package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.StoreIssuanceToConsumableDto;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.repositories.StoreIssuanceToConsumableRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.security.CurrentUserContext;

import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.clr;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Store Management (AppModules 24) — screen 347 "Store Issuance To Consumable Store".
 *
 * <pre>
 *   Desktop form   Architecture.WinApp.StoreManagement.frmStoreIssuanceToCosumableStore (.cs)
 *                  base.Name = "frmStoreIssuanceToCosumableStore" (= ScreenName, the rights key)
 *   Loader         Architecture.WinApp.StoreManagement.LoadDepRequestToConsumableStore, opened with
 *                  DocumentTypeId = 1615 (Department Request To Consumable Store, screen 349)
 *   DocumentTypeId 1616
 *   Route          /store/issuance-to-consumable-store     API /api/store/issuance-to-consumable-store
 *   BLL / DAL      BLL 0252 / DAL 0221 Architecture.*.PurchaseTrading.InvGsStoreIssuanceHeader
 *                  BLL 0056 GetAvgRatesAndStockInHand (AvgRateOnlyForCGS)
 *                  BLL 0058 Branches.GetAll, BLL 0078 Projects.GetAlldt
 *                  BLL 0066 DateLock, BLL 0654 / DAL 0205 VoucherHead id (CommonServices.VoucherHeadIdGet)
 *                  Model 0200 InvGsStoreIssuanceHeader, 0199 InvGsStoreIssuanceDetail
 * </pre>
 *
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected):
 * <ol>
 *  <li>The validation messages concatenate the row index and "1" as text (C# {@code "..." + r.RowIndex + 1}):
 *      the first row is "Row No 01" (Form.cs:370-419).</li>
 *  <li>Qty is tested with {@code Conversion.ToInt(IssueQty) > 0} — Convert.ToInt32 rounds half to even,
 *      so 0.5 is refused with "Qty Required…" while 0.6 passes (Form.cs:352). Removed rows likewise
 *      carry {@code Conversion.ToInt(IssueQty)} (Form.cs:793).</li>
 *  <li>Save and Update both check {@code VoucherHeadIdGet(RECID, 1616)} first; for Save RECID is 0
 *      (Form.cs:302).</li>
 *  <li>lstRemoveRecord is filled for every "X" on a row with an item — including rows never saved (Id 0)
 *      — and is NEVER cleared (not by New, not by ReadById, not after a save). On an Update every
 *      entry is sent first with ActionTypeId 3, LineId 0 (Form.cs:342, 777-799); the page keeps the
 *      same list for its lifetime.</li>
 *  <li>USP_InventoryValidation runs for every detail sent, removed ones included, with
 *      {@code @WarehouseId = DepartmentId} for 1616 (DAL 0221:161) and ItemConditionId 0.</li>
 *  <li>LineId = grid row index + 1; the DAL does not renumber for 1616 (DAL 0221:62-69).</li>
 *  <li>The header's RefDocNoId / RefDocumentTypeId stay 0 on a new document — LoadInGridDetail never
 *      sets them (Form.cs:952); an opened document keeps the stored values (Form.cs:487).</li>
 *  <li>Warehouseid of a loaded row is always 0 (LoadInGridDetail puts 0 in LocationId, Form.cs:966).</li>
 *  <li>The success message is "Record Save SuccessFully" / "Record Update SuccessFully" + DocNo with no
 *      space (Form.cs:427/431).</li>
 *  <li>The grid's "Unit" cell of an opened document (and of the history detail grid) shows the UOM's
 *      Equivalent number, not its code (Form.cs:493, 1159).</li>
 *  <li>History (Form tab switch = 50 rows, Load All = all) sends no branch or date filters, and when the
 *      procedure returns no row the previous grid is left as it was (Form.cs:1017) — done in the page.</li>
 *  <li>FormValidation tests the combo TEXT for "" / "0" (Form.cs:607-624), so the default row
 *      "...Select Any Value..." (value 0) passes and a document can be saved with Branch/Project 0.</li>
 *  <li>With Preview checked, a NEW document prints PrintMethod(RECID) with RECID still 0 (btnsave_Click
 *      sets it to 0 and Insert never assigns the new id), so the desktop shows "Record Id Not Found"
 *      instead of the slip; only an Update prints (Form.cs:436, 450) — done in the page.</li>
 * </ol>
 *
 * DEVIATIONS (web-only, with reason):
 * <ol>
 *  <li>Doc No is read-only on the desktop: the server takes it from the generator on Save and from the
 *      stored header on Update, never from the request (the insert procedure recomputes it anyway).</li>
 *  <li>Tenancy: open/print/update only for a 1616 header of the signed-in company; a removed-row id
 *      must be a 1616 detail of the same company; the Branch / Project ids must be 0 or one of the
 *      company's rows (the page could otherwise post any id).</li>
 *  <li>AttachmentsValues / CustomAttachmentsValues on Update are the stored header's (attachments are
 *      not ported; sending "" would wipe the column while the DMS rows stay).</li>
 *  <li>A header procedure that returns no id aborts the save instead of writing orphan details.</li>
 *  <li>The reads the desktop does and discards are not made: BalanceStock() on row removal
 *      (Form.cs:791), the DAL's attachment configuration reads (no attachments are sent) and
 *      BLL Save's StoreIssuanceFinancialEffect lookup (ActionForVoucherId is 0, so no voucher).</li>
 *  <li><b>Loading a request cannot work on the desktop.</b> LoadInGridDetail (Form.cs:966) reads
 *      {@code row["UOM"]} from loadDepReq.dtLoader, which is a copy of the PROCEDURE rows
 *      (LoadDepRequestToConsumableStore.cs:455-458); USP_GetPendingDepartmentRequestForIssuance returns
 *      the column as {@code UOMCode} (checked in procs.sql and in the 05/09/2026 GoldenAceDb dump), so
 *      the first new row throws "Column 'UOM' does not belong to table" and nothing is ever added —
 *      every save then stops at "Grid record not found". The page fills the Unit cell from UOMCode
 *      instead (the value the loader's own grid shows as UOM, :353). Unit is display-only; UnitId
 *      (ItemUOMId) is what is saved. Revert in countx_store_store_issuance_to_consumable.js
 *      (loadInGridDetail) if the desktop failure must be reproduced.</li>
 *  <li>No Delete endpoint: btnDelete is Visible=false in the designer and in Form_Load and nothing
 *      ever shows it (Form.cs:224, ReadById does not), so the desktop cannot delete a 1616 document.</li>
 * </ol>
 */
@Service
public class StoreIssuanceToConsumableService {

    private static final Logger LOG = LoggerFactory.getLogger(StoreIssuanceToConsumableService.class);

    public static final String SCREEN = "frmStoreIssuanceToCosumableStore";
    public static final int DOC_TYPE = 1616;
    /** btnLoadRequest_Click: loadDepReq.DocumentTypeId = 1615 (Form.cs:941). */
    public static final int LOADER_DOC_TYPE = 1615;

    private final StoreIssuanceRepository shared;
    private final StoreIssuanceToConsumableRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;
    private final JdbcTemplate jdbc;

    public StoreIssuanceToConsumableService(StoreIssuanceRepository shared, StoreIssuanceToConsumableRepository repo,
                                            StoreScreenRights rights, CurrentUserContext ctx, JdbcTemplate jdbc) {
        this.shared = shared;
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
        this.jdbc = jdbc;
    }

    // ====================================================================================== load

    /** frmGSIssuance_Load:168 — rights, BranchFill, ProjectFill, GenerateDocNo. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));
        out.put("branches", project(repo.branches(u), "Id", "BranchName"));      // :239 Id / BranchName
        out.put("projects", project(repo.projects(u), "Id", "ProjectName"));     // :257 Id / ProjectName
        out.put("docNo", docNo(u));
        out.put("financialYearStart", financialYearStart(u, ctx.currentFinancialYearId()));
        return out;
    }

    /** GenerateDocNo:268 — StoreIssuanceGenerateCode(1616). */
    public int docNo() { return docNo(ctx.requireAccountingUser()); }

    private int docNo(UserAccount u) {
        return shared.nextDocNo(u, ctx.currentFinancialYearId(), DOC_TYPE);
    }

    /** btnRefresh_Click:547 — BranchFill, ProjectFill, GenerateDocNo again. */
    public Map<String, Object> refresh() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("branches", project(repo.branches(u), "Id", "BranchName"));
        out.put("projects", project(repo.projects(u), "Id", "ProjectName"));
        out.put("docNo", docNo(u));
        return out;
    }

    // ================================================================================== loader

    /** LoadDepRequestToConsumableStore.AllCombobind:153 — split by Activity; only 1615's six. */
    public Map<String, Object> loaderLookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, List<Map<String, Object>>> by = new LinkedHashMap<>();
        for (String a : new String[] { "Item", "DepartmentTo", "DepartmentFrom", "WorkStationTo", "WorkStationFrom", "WorkOrderNo" }) {
            by.put(a, new ArrayList<>());
        }
        for (Map<String, Object> r : shared.departmentRequestDropDowns(u)) {
            List<Map<String, Object>> t = by.get(str(r.get("Activity")));
            if (t == null) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("ReferenceName")));
            t.add(o);
        }
        return new LinkedHashMap<>(by);
    }

    /** PendingDepRequestLoad:301 with DocumentTypeId 1615. Both pickers go to DATE parameters. */
    public List<Map<String, Object>> pendingRequests(String from, String to, int docNoFrom, int docNoTo,
                                                     int departmentFromId, int departmentToId,
                                                     int workStationFromId, int workStationToId,
                                                     int workOrderId, int itemId) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> rows = repo.pendingDepartmentRequests(u, LOADER_DOC_TYPE, ctx.currentFinancialYearId(),
                StoreIssuanceService.dateOnly(from), StoreIssuanceService.dateOnly(to), docNoFrom, docNoTo,
                itemId, workStationFromId, workStationToId, workOrderId, departmentFromId, departmentToId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            /* dtGrid columns (:327-348) plus the dtGridFromDb fields LoadInGridDetail reads. */
            o.put("Id", toInt(r.get("Id")));
            o.put("DetailId", toInt(r.get("DetailId")));
            o.put("DocDate", day(r.get("DocDate")));
            o.put("DocumentTypeId", toInt(r.get("DocTypeId")));
            o.put("DocNo", toInt(r.get("DocNo")));
            o.put("FromDepartmentId", toInt(r.get("FromDepartmentId")));
            o.put("FromDepartment", str(r.get("FromDepartment")));
            o.put("ToDepartmentId", toInt(r.get("ToDepartmentId")));
            o.put("ToDepartment", str(r.get("ToDepartment")));
            o.put("WorkOrderId", toInt(r.get("WorkOrderId")));
            o.put("WorkOrderNo", str(r.get("WorkOrderNo")));
            o.put("ItemId", toInt(r.get("ItemId")));
            o.put("ItemName", str(r.get("ItemName")));
            o.put("ItemCode", str(r.get("ItemCode")));
            o.put("ItemUOMId", toInt(r.get("ItemUOMId")));
            o.put("UOM", str(r.get("UOMCode")));
            o.put("AssetId", toInt(r.get("AssetId")));
            o.put("AssetName", str(r.get("AssetName")));
            o.put("RequestedQty", toDouble(r.get("RequestedQty")));
            o.put("IssuedQty", toDouble(r.get("IssuedQty")));
            o.put("QtyBal", toDouble(r.get("BalQty")));
            o.put("QtyInStock", toDouble(r.get("StockQtyBal")));
            out.add(o);
        }
        return out;
    }

    // ================================================================================= history

    /** HistoryFill:1012 — CommonServices.StoreIssuanceHistory(1616, CanViewAllRecord, NoOfRecords). */
    public List<Map<String, Object>> history(int noOfRecords) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN, "viewAll");
        List<Map<String, Object>> rows = repo.formHistory(u, DOC_TYPE, ctx.currentFinancialYearId(), viewAll,
                noOfRecords, u.getId());
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> r : rows) {
            int id = toInt(r.get("Id"));
            if (!seen.add(id)) continue;                                     // :1032 first row per Id
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("RecordNo", toInt(r.get("RecordNo")));
            o.put("Id", id);
            o.put("DocNo", toInt(r.get("DocNo")));
            o.put("DocDate", day(r.get("DocDate")));                         // ToShortDateString → date only (page)
            o.put("Remarks", str(r.get("Remarks")));
            o.put("NoOfAttachments", toInt(r.get("NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    // ==================================================================================== read

    /** ReadById:471 / DataGridHistory_SelectionChanged:1128 — BLL GetByID(RECID). */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) return null;
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : shared.details(h)) {
            Map<String, Object> o = new LinkedHashMap<>();           // dtgrddetail.Rows.Add order (:493)
            o.put("Id", toInt(d.get("Id")));
            o.put("DepartmentRequestId", toInt(d.get("DepartRequestId")));
            o.put("DepartmentRequestDetailId", toInt(d.get("DepartRequestDetailId")));
            o.put("DepartmentRequestNo", toInt(d.get("DepartmentRequestNo")));
            o.put("LocationId", toInt(d.get("Warehouseid")));
            o.put("DepartmentFromId", toInt(d.get("DepartmentId")));
            o.put("DepartmentFrom", str(d.get("DepartmentName")));
            o.put("DepartmentToId", toInt(d.get("DepartmentToId")));
            o.put("DepartmentTo", str(d.get("ToDepartmentName")));
            o.put("WorkOrderId", toInt(d.get("WorkOrderIdId")));
            o.put("WorkOrder", str(d.get("WorkOrderNo")));
            o.put("ItemId", toInt(d.get("ItemId")));
            o.put("Item", str(d.get("ItemName")));
            o.put("ItemCode", str(d.get("ItemCode")));
            o.put("UnitId", toInt(d.get("ItemUomSchId")));
            o.put("Unit", clr(toDouble(d.get("Equivalent"))));       // desktop note 10
            o.put("AssetId", toInt(d.get("AssetsId")));
            o.put("Asset", str(d.get("AssetName")));
            o.put("IssueQty", toDouble(d.get("IssueQty")));
            o.put("Remarks", str(d.get("ReamarksDetail")));
            rows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(h.get("Id")));
        out.put("DocDate", day(h.get("DocDate")));
        out.put("DocNo", toInt(h.get("DocNo")));
        out.put("BranchesId", toInt(h.get("BranchesId")));
        out.put("ProjectsId", toInt(h.get("ProjectsId")));
        out.put("Remarks", str(h.get("Remarks")));
        out.put("RefDocNoId", toInt(h.get("RefDocNoId")));
        out.put("RefDocumentTypeId", toInt(h.get("RefDocumentTypeId")));
        out.put("IsApproved", h.get("IsApproved"));
        out.put("rows", rows);
        return out;
    }

    /** PrintMethod:633 → CommonServices.StoreIssuanceSlip1616 (the .rpt layout itself is not ported). */
    public List<Map<String, Object>> slip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN, "print")) throw new IllegalStateException("You do not have the Print right for this screen.");
        if (id == 0) throw new IllegalArgumentException("Record Id Not Found");
        if (ownedHeader(u, id) == null) return null;
        return repo.slip(u, id);
    }

    // ==================================================================================== save

    /**
     * Insert():296 up to the confirmation box — "Grid record not found", the voucher check, then
     * FormValidation. The page calls this BEFORE asking "Are you sure to Save/Update?", so the
     * messages come in the desktop's order; {@link #save} runs the same checks again.
     */
    public Map<String, Object> precheck(StoreIssuanceToConsumableDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id == null ? 0 : dto.Id;
        requireRight(recId);
        Map<String, Object> existing = recId != 0 ? ownedHeader(u, recId) : null;
        if (recId != 0 && existing == null) throw new IllegalArgumentException("Record Not Found");
        preConfirm(u, dto, recId, existing);
        return ok("", 0, 0);
    }

    private void requireRight(int recId) {
        if (recId == 0 && !rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
    }

    /** Returns the Doc No the save will use (generator on Save, stored header on Update). */
    private int preConfirm(UserAccount u, StoreIssuanceToConsumableDto dto, int recId, Map<String, Object> existing) {
        if (dto.rows == null || dto.rows.isEmpty()) throw new IllegalArgumentException("Grid record not found");      // :298
        if (shared.voucherHeadId(u, DOC_TYPE, recId) > 0) {                                                           // :302
            throw new IllegalArgumentException("Record Not Update because Record has exist in Voucher");
        }
        /* FormValidation:605 — the combos' TEXT. */
        if (blankOrZero(dto.BranchText)) throw new IllegalArgumentException("Branch Field Required");
        if (blankOrZero(dto.ProjectText)) throw new IllegalArgumentException("Project Field Required");
        int docNo = existing != null ? toInt(ci(existing, "DocNo")) : docNo(u);
        if (docNo == 0) throw new IllegalArgumentException("document Number Field Required");
        /* DEVIATION 2 — the ids behind the texts must be the company's own rows (or 0). */
        int b = i(dto.BranchesId), p = i(dto.ProjectsId);
        if (b != 0 && !containsId(repo.branches(u), b)) throw new IllegalArgumentException("Branch is not one of this company's branches.");
        if (p != 0 && !containsId(repo.projects(u), p)) throw new IllegalArgumentException("Project is not one of this company's projects.");
        return docNo;
    }

    @Transactional
    public Map<String, Object> save(StoreIssuanceToConsumableDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id == null ? 0 : dto.Id;
        requireRight(recId);
        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = ownedHeader(u, recId);
            if (existing == null) throw new IllegalArgumentException("Record Not Found");
        }
        int docNo = preConfirm(u, dto, recId, existing);

        /* An opened document's picker holds head.DocDate (midnight); a new one the time of day. */
        Timestamp docDate = StoreIssuanceService.formDate(dto.DocDate, recId);
        Map<String, Object> head = header(u, dto, recId, docNo, docDate, existing);

        List<Map<String, Object>> details = new ArrayList<>();
        /* :342 — lstRemoveRecord first, only when updating. */
        if (recId > 0 && dto.removed != null) {
            for (StoreIssuanceToConsumableDto.Row r : dto.removed) {
                int id = i(r.Id);
                if (id > 0 && !repo.detailOfCompanyDocType(u, id, DOC_TYPE)) {
                    throw new IllegalArgumentException("Record Not Found");                  // DEVIATION 2
                }
                details.add(removedDetail(r));
            }
        }
        /* :349-420 */
        for (int idx = 0; idx < dto.rows.size(); idx++) {
            StoreIssuanceToConsumableDto.Row r = dto.rows.get(idx);
            if (cInt(r.IssueQty) <= 0) {
                throw new IllegalArgumentException("Qty Required In grid Row No " + idx + "1");          // :419
            }
            Map<String, Object> d = detailModel();
            d.put("LineId", idx + 1);                                                                   // :355
            int id = i(r.Id);
            d.put("Id", id);
            d.put("ActionTypeId", id > 0 ? 2 : 1);                                                      // :357
            d.put("DepartRequestId", i(r.DepartmentRequestId));
            d.put("DepartRequestDetailId", i(r.DepartmentRequestDetailId));
            d.put("Warehouseid", i(r.LocationId));
            if (i(r.DepartmentFromId) == 0) throw new IllegalArgumentException("DepartmentFrom Required In grid Row No " + idx + "1");
            d.put("DepartmentId", i(r.DepartmentFromId));
            if (i(r.DepartmentToId) == 0) throw new IllegalArgumentException("DepartmentTo Required In grid Row No " + idx + "1");
            d.put("DepartmentToId", i(r.DepartmentToId));
            d.put("WorkStationFromId", i(r.DepartmentFromId));                                          // :378
            d.put("WorkStationToId", i(r.DepartmentToId));
            d.put("WorkOrderIdId", i(r.WorkOrderId));
            if (i(r.ItemId) == 0) throw new IllegalArgumentException("Item Required In grid Row No " + idx + "1");
            d.put("ItemId", i(r.ItemId));
            if (i(r.UnitId) == 0) throw new IllegalArgumentException("Unit Required In grid Row No " + idx + "1");
            d.put("ItemUomSchId", i(r.UnitId));
            d.put("AssetsId", i(r.AssetId));
            d.put("ReamarksDetail", r.Remarks == null ? "" : r.Remarks);
            double qty = r.IssueQty == null ? 0d : r.IssueQty;
            d.put("IssueQty", qty);
            if (i(r.ItemId) > 0) {                                                                       // :395
                double avg = repo.avgRateOnlyForCgs(u, i(r.ItemId), docDate);
                if (!(avg > 0d)) throw new IllegalArgumentException("AvgRate Not Found");
                d.put("ItemRate", avg);
                if (avg == 0d) throw new IllegalArgumentException("Item Rate Not Found");
                d.put("ItemAmount", qty * avg);
            }
            d.put("SupplierCustomerId", 0);
            details.add(d);
        }

        int id = persist(u, head, details);
        String msg = (recId == 0 ? "Record Save SuccessFully" : "Record Update SuccessFully") + docNo;
        return ok(msg, id, docNo);
    }

    /** grdDetail_ColumnButtonClick:777-798 — the entry lstRemoveRecord gets. */
    private Map<String, Object> removedDetail(StoreIssuanceToConsumableDto.Row r) {
        Map<String, Object> d = detailModel();
        d.put("Id", i(r.Id));
        d.put("DepartRequestId", i(r.DepartmentRequestId));
        d.put("DepartRequestDetailId", i(r.DepartmentRequestDetailId));
        d.put("Warehouseid", i(r.LocationId));
        d.put("DepartmentId", i(r.DepartmentFromId));
        d.put("DepartmentToId", i(r.DepartmentToId));
        d.put("WorkStationFromId", i(r.DepartmentFromId));
        d.put("WorkStationToId", i(r.DepartmentToId));
        d.put("WorkOrderIdId", i(r.WorkOrderId));
        d.put("ItemId", i(r.ItemId));
        d.put("ItemUomSchId", i(r.UnitId));
        d.put("AssetsId", i(r.AssetId));
        d.put("ReamarksDetail", r.Remarks == null ? "" : r.Remarks);
        d.put("IssueQty", (double) cInt(r.IssueQty));                                     // :793 ToInt
        d.put("ItemRate", 0d);
        d.put("ItemAmount", 0d);
        d.put("SupplierCustomerId", 0);
        d.put("ActionTypeId", 3);
        return d;
    }

    /** Insert():324-340 on Architecture.Model.PurchaseTrading.InvGsStoreIssuanceHeader, in property order. */
    private Map<String, Object> header(UserAccount u, StoreIssuanceToConsumableDto dto, int recId, int docNo,
                                       Timestamp docDate, Map<String, Object> existing) {
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("IsApproved", false);                                          // :338
        h.put("ApprovedDate", null);                                         // DateTime? unset → omitted
        h.put("DocDate", docDate);
        h.put("EntryDate", now);
        h.put("ModifyDate", now);
        h.put("ApprovedUserId", 0);
        h.put("BranchesId", i(dto.BranchesId));                              // cmbBranch.Value
        h.put("BranchSrNo", 0);                                              // never set by this form
        h.put("CompanyId", u.getCompanyId());
        h.put("DocNo", docNo);                                               // DEVIATION 1
        h.put("EntryUser", u.getId());                                       // set on Update too (:335)
        h.put("Id", recId);
        h.put("ModifyUser", u.getId());
        h.put("OrganizationId", u.getOrganizationId());
        h.put("ProjectsId", i(dto.ProjectsId));                              // cmbProject.Value
        h.put("RefDocNoId", existing == null ? 0 : toInt(ci(existing, "RefDocNoId")));               // note 7
        h.put("RefDocumentTypeId", existing == null ? 0 : toInt(ci(existing, "RefDocumentTypeId")));
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("FinancialYearId", ctx.currentFinancialYearId());
        h.put("ExImInvoiceId", 0);
        h.put("Remarks", dto.Remarks == null ? "" : dto.Remarks);            // txtRemarks.Text
        h.put("ManualNo", null);
        /* UpdateAttachmentsForObject sets AttachmentsValues ("" with no attachments); DEVIATION 3. */
        h.put("AttachmentsValues", existing == null ? "" : str(ci(existing, "AttachmentsValues")));
        h.put("CustomAttachmentsValues", existing == null ? null : ci(existing, "CustomAttachmentsValues"));
        h.put("ActionId", recId == 0 ? 1 : 2);                               // BLL Save
        h.put("TypeId", 0);
        h.put("BaseDocumentTypeId", 0);
        h.put("IsUploaded", false);
        return h;
    }

    /** Model 0199 InvGsStoreIssuanceDetail — non-virtual properties in declaration order, defaults. */
    private static Map<String, Object> detailModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("IssueQty", 0d);
        p.put("AssetsId", 0);
        p.put("IssuanceTypeId", 0);
        p.put("ExImInvoiceId", 0);
        p.put("DepartmentId", 0);
        p.put("Id", 0);
        p.put("LineId", 0);
        p.put("InvGsStoreIssuanceHeaderId", 0);
        p.put("ItemId", 0);
        p.put("ItemUomSchId", 0);
        p.put("ItemConditionId", 0);
        p.put("Warehouseid", 0);
        p.put("SupplierCustomerId", 0);
        p.put("EmployeeId", 0);
        p.put("DrAccountId", 0);
        p.put("BagTypeId", 0);
        p.put("ItemRate", 0d);
        p.put("ItemAmount", 0d);
        p.put("ReamarksDetail", null);
        p.put("RefDocumentTypeId", 0);
        p.put("RefDocId", 0);
        p.put("RefDocDetailId", 0);
        p.put("ActionTypeId", 0);
        p.put("DepartmentToId", 0);
        p.put("DepartRequestDetailId", 0);
        p.put("DepartRequestId", 0);
        p.put("WorkOrderIdId", 0);
        p.put("WorkStationFromId", 0);
        p.put("WorkStationToId", 0);
        p.put("ContractScheduleId", 0);
        p.put("RackId", 0);
        return p;
    }

    /**
     * BLL 0252 Save + DAL 0221 SetData, branch {@code DocumentTypeId == 1616 && ActionForVoucherId != 1},
     * inside one transaction:
     *   DateLock refusal (BLL) → header Insert|Update → each detail Sp_InvGsStoreIssuanceDetail_Insert
     *   → Sp_InventoryTransactions_GetALLMethod → Sp_InventoryStockEvalautionDetail_Update
     *   → USP_InventoryValidation per detail (WarehouseId = DepartmentId). No voucher.
     */
    private int persist(UserAccount u, Map<String, Object> head, List<Map<String, Object>> details) {
        Timestamp docDate = (Timestamp) head.get("DocDate");
        Timestamp lock = shared.dateLock(u);
        if (lock != null && !docDate.after(lock)) {
            throw new IllegalArgumentException("Not Insert or Update record please check lock date");
        }
        int recId = toInt(head.get("Id"));
        String proc = recId == 0 ? "Sp_InvGsStoreIssuanceHeader_Insert" : "Sp_InvGsStoreIssuanceHeader_Update";
        int num = shared.setProc(proc, head);
        if (num > 0) head.put("Id", num); else num = recId;
        /* DEVIATION 4 — not IllegalStateException: the controller maps that to 403 (missing right). */
        if (num <= 0) throw new RuntimeException("Save returned no document id.");

        for (Map<String, Object> d : details) {
            d.put("InvGsStoreIssuanceHeaderId", num);
            d.put("Id", shared.setProc("Sp_InvGsStoreIssuanceDetail_Insert", d));
        }
        shared.postStock(u, DOC_TYPE, num);
        for (Map<String, Object> d : details) {
            shared.inventoryValidation(u, DOC_TYPE, docDate, toInt(d.get("ItemId")),
                    toInt(d.get("ItemConditionId")), toInt(d.get("DepartmentId")), toDouble(d.get("IssueQty")));
        }
        return num;
    }

    // ================================================================================= helpers

    /** The header must exist (ActionId <> 3), be DocumentTypeId 1616 and belong to the user's company. */
    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = shared.header(id);
        if (h == null) return null;
        if (toInt(h.get("DocumentTypeId")) != DOC_TYPE) return null;
        if (toInt(h.get("CompanyId")) != u.getCompanyId()) return null;
        if (toInt(h.get("OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }

    /** Convert.ToInt32(double) — round half to even (Conversion.ToInt). */
    static int cInt(Double v) {
        if (v == null || v.isNaN() || v.isInfinite()) return 0;
        return (int) Math.rint(v);
    }

    private static boolean blankOrZero(String t) {
        String s = t == null ? "" : t.trim();
        return s.isEmpty() || "0".equals(s);
    }

    private static boolean containsId(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (toInt(ci(r, "Id")) == id) return true;
        return false;
    }

    private static List<Map<String, Object>> project(List<Map<String, Object>> rows, String id, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, id)));
            o.put("Name", str(ci(r, name)));
            out.add(o);
        }
        return out;
    }

    private String financialYearStart(UserAccount u, int yearId) {
        try {
            List<Map<String, Object>> years = jdbc.queryForList(
                    "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                    u.getOrganizationId(), u.getCompanyId());
            for (Map<String, Object> r : years) {
                if (toInt(ci(r, "Id")) == yearId) {
                    Object v = ci(r, "Start_Period");
                    return v == null ? null : String.valueOf(v).substring(0, 10);
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read the financial year start", e);
        }
        return null;
    }

    /** A DATE / DATETIME cell as "yyyy-MM-dd" (the day the server reads, never shifted by a JSON time zone). */
    static String day(Object v) {
        if (v == null) return null;
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.time.LocalDateTime) return ((java.time.LocalDateTime) v).toLocalDate().toString();
        if (v instanceof java.time.LocalDate) return v.toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        String s = String.valueOf(v);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    private static int i(Integer v) { return v == null ? 0 : v; }

    private static Map<String, Object> ok(String message, int id, int docNo) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        m.put("id", id);
        m.put("docNo", docNo);
        return m;
    }
}
