package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.DeliveryOrderTransferDto;
import com.mst.repositories.DeliveryOrderTransferRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.DeliveryOrderTransferRepository.DOCUMENT_TYPE_ID;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Store Management (ModuleId 24) — screen 331 "Delivery Order (Stock Transfer)".
 *
 * <ul>
 *   <li>ScreenDefinition 331: ScreenName "DeliveryOrderTransfer", TargetUrl Architecture.WinApp.Sale.DeliveryOrder.
 *       The desktop opens DeliveryOrder.cs; its constructor takes ScreenName from UserAccountAllocationsList[0]
 *       (DeliveryOrder.cs:409-419) and FormHelper.OpenFormInPanel sets Tag = "DeliveryOrderTransfer" (FormHelper.cs:132).
 *       Every branch the form takes for ScreenName / Tag "DeliveryOrderTransfer" and DO Type 2 "StockTransfer" is
 *       ported; the Local-only branches (sale-order loaders, order balance, payment-term warning, customer schedule)
 *       are reproduced only where they are reachable in this mode. base.Name stays "DeliveryOrder" (designer :719;
 *       used only by attachments).</li>
 *   <li>DocumentTypeId 84 (:408 — never changed for the transfer mode), rights by ScreenName "DeliveryOrderTransfer"
 *       (SetRightsValueInRightsObject(ScreenName):525).</li>
 *   <li>Route /store/delivery-order-transfer, API /api/store/delivery-order-transfer.</li>
 *   <li>BLL 0558 / DAL 0411 InvDeliveryOrder, Model 1006 InvDeliveryOrder, 1000 InvDeliveryOrderdetail,
 *       0968 InvDeliveryOrderExpense; BLL 0574 GetCurrentStockByItemId; BLL 0131 InvDeliveryOrderSlip; BLL 0058
 *       Branches; 0611 VehicleType; 0074 ReferenceParties; 0573 InventoryItemsOther; 0379 GlobalServicesMethods;
 *       0269 CommonRepository; DAL 0205 CommonServices; DAL 0207 GenericProvider (DesktopProc).</li>
 * </ul>
 *
 * SAVE (btnsave / btnSaveAs / btnupdate → Insert():1768 → BLL Save:16 → DAL SetData:16), one transaction:
 * Sp_InvDeliveryOrder_Insert | _Update (every non-virtual Model 1006 property) → Sp_InvDeliveryOrderDetail_Insert per
 * detail (removed rows first with ActionTypeId 3, then grid rows 1 new / 2 existing) → Sp_InvDeliveryOrderExpense_Insert
 * per Expense Grid row → [DAW].[USp_DocumentApprovalDetail_Insert] (LimitAmount 0). No stock posting and no voucher.
 *
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected):
 *  1. The Expense Grid always holds at least one blank row (AddRowInvExpGrid:1643 — Id 0, ItemId 0, Qty 0) and Insert()
 *     sends every row (:1896), so every save inserts a blank InvDeliveryOrderExpense row with ItemId 0. ReadById never
 *     reads it back (the read joins InventoryItemsOther), and Sp_InvDeliveryOrder_Update deletes and re-inserts the rows.
 *  2. DeliveryOrderStackWarningMessage (BLL:1879) checks only the first detail of the list — on Update that is the first
 *     REMOVED row when a row was removed — because dbo.USP_DeliveryOrderStackWarningMessage always returns one row.
 *  3. The DocNo shown in the success message is the number in the Doc No box (generator value), not the one
 *     Sp_InvDeliveryOrder_Insert assigns (it renumbers MAX+1 itself).
 *  4. Removed rows travel with ActionTypeId 3 and TotalPackingWeight 0 (DeleteDetailRow:1587 never sets it).
 *  5. The grid's AvailableStock column (UpdateAvailableStockInGridDetailRow:3477) sends @CropYear = the CropYearId
 *     cell's text, i.e. the crop year's Id as text, not the year; the entry-bar balance (AvailableStockGetByItem:3608)
 *     sends the crop year's text. Both are reproduced as the page asks for them.
 *  6. History: GpDate / ApprovedDate / ModifyDate of a row without one is Conversion.ToDateTime(DBNull) = 01-Jan-1900;
 *     OrderStatus is ISNULL(SaleOrder.OrderStatus,'Open') — always "Open" for a transfer, so Edit is never refused.
 *  7. Delete refuses only on the IsApproved flag; RemoveByID(84, Id) is USP_RecoredRemoveByOrgCompDocAndByID.
 *  8. SaveAs (grdhistory_Saveas:2626) keeps the source document's Branch From (ReadById sets cmbBranchFrom) and its
 *     AttachmentsValues / CustomAttachmentsValues (ReadById:2053) — both are sent with the new document.
 *  9. The history customer combo is filled with BranchesIds = the user's branch as text ("0" when the user has none),
 *     which the procedure applies as a filter.
 * 10. On Update the header goes with IsApproved as read (the procedure resets it from configuration 94),
 *     ApprovedUser 0 and no ApprovedDate (never set by Insert()).
 *
 * DEVIATIONS (web-only, with reason):
 *  D1. DocNo is never taken from the request: the generator value on insert, the stored header's on update.
 *  D2. A document is opened, updated, deleted, printed or used as a SaveAs source only when it belongs to the signed-in
 *      user's organization and company, DocumentTypeId 84 and DeliveryOrderType "StockTransfer" (GetByID has no company
 *      filter; this screen must not rewrite a Local / Export delivery order).
 *  D3. Every posted id is re-checked against the same company-scoped list the combo is filled from (branches, customers,
 *      reference parties, items, the item's UOM schedule, crop years, packing types, warehouses, job lots, other items,
 *      vehicle types, sale types). An existing row id must be a live detail of this document; a removed id must be a live
 *      detail that is not also in the grid; on an existing row a value equal to the stored one is not re-checked (the
 *      lists hold active rows only). Branch From must be the user's branch, the stored header's (Update) or the SaveAs
 *      source's (the combo is disabled on the desktop). Removed rows are rebuilt from the stored detail (the procedure
 *      uses only their Id; the stack warning may read the first one). New rows carry RefDocumentTypeId/RefDocIdNo/
 *      RefDocSubIdNo 0 (a transfer row has no reference document; the loaders that set them are Local-only).
 *      Expense rows: SaleOrderId / SaleOrderCustomerExpId come from the stored expense row whose Id is posted, else 0.
 *  D4. The stock warning is a two-step request (the page re-posts with ConfirmWarning = true when the operator answers
 *      Yes). The "Are you sure to Save?" / packing-weight confirmations run on the page before the request.
 *  D5. On Update the stored AttachmentsValues / CustomAttachmentsValues are sent back (ReadById keeps them in the form).
 *  D6. Delete reads IsApproved from the stored header, not from the value read when the document was opened.
 *
 * NOT PORTED: attachments (btnattachment, AddAttachment / NoOfAttachments history columns, DMS copy), saved grid
 * layouts (ctrlGrdBar), keyboard shortcuts and the ShortCut Keys popup, the Crystal layouts 262-DeliveryOrderSlip.rpt /
 * 264-DeliveryChallanByDeliveryOrder.rpt (the slip procedure's rows are returned), DefineReferenceParties (another
 * definition screen), and the Local-only loaders (Load_SO_MainDetail / Load_SO_Main are hidden in this mode;
 * Load_Customer_Schedule only answers "Please select 'Local' as Delivery Order Type.").
 */
@Service
public class DeliveryOrderTransferService {

    public static final String SCREEN_NAME = "DeliveryOrderTransfer";
    public static final String ORDER_TYPE = "StockTransfer";
    public static final int ORDER_TYPE_ID = 2;
    private static final int FEATURE_SUBSIDIARY_ACCOUNTS = 4;

    private final DeliveryOrderTransferRepository repo;
    private final StoreIssuanceRepository common;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public DeliveryOrderTransferService(DeliveryOrderTransferRepository repo, StoreIssuanceRepository common,
                                        StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.common = common;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ====================================================================================== load

    /** InitializeComponentMethod:520 (+ GetConfigurationsFromGlobal:705) and btnRefresh_Click:2168 — every combo source. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));                                                 // :525
        out.put("userBranchId", branch(u));
        out.put("docNo", repo.generateCode(u, fy, branch(u)));                                    // :526
        /* GetConfigurationsFromGlobal:705-737 */
        Map<String, Object> cfg = new LinkedHashMap<>();
        cfg.put("WarningMsgForPaymentTermOtherThanCreditOnDO", toBool(common.config(u, "WarningMsgForPaymentTermOtherThanCreditOnDO")));
        cfg.put("IsStockReservedPerParty", toBool(common.config(u, "IsStockReservedPerParty")));
        cfg.put("ShowInfoGrid", toBool(common.config(u, "Order Rate Show On Delivery Order")));
        cfg.put("ItemSearchByCode", toBool(common.config(u, "ItemSearchByCode")));
        cfg.put("DefaultDaysToLessFromHistoryFromDate", toInt(common.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        cfg.put("WarningMessageOnDOForPackingWeight", toBool(common.config(u, "WarningMessageOnDOForPackingWeight")));
        /* GetConfigurationsFromGlobalAndBindValuesInColumns:740 */
        cfg.put("JobLot", toInt(common.config(u, "Job/Lot")));
        cfg.put("CropYear", toInt(common.config(u, "Default Crop Year")));
        cfg.put("PackingType", toInt(common.config(u, "Paking Type")));
        cfg.put("Warehouse", toInt(common.config(u, "Warehouse")));
        out.put("config", cfg);
        out.put("isCustomerPortal", repo.customerPortal(u));                                      // :712
        out.put("branches", project(repo.branches(u), "Id", "BranchName"));                       // BranchNameBind:794
        out.put("vehicleTypes", project(repo.vehicleTypes(), "Id", "VehicleDescription"));        // VehicleTypesBind:819
        out.put("suppliers", suppliers(u));                                                        // SupplierDtFillFromGlobal:848
        out.put("warehouses", repo.warehousesWithBranches(u, branch(u)));                         // :911
        out.put("items", items(u));                                                                // ItemDtFillFromGlobal:926
        out.put("uoms", repo.uomSchedule(u));                                                      // PackUomFromGlobalBind:1037
        out.put("cropYears", repo.cropYears(u));                                                   // :991
        out.put("jobLots", repo.jobLots(u));                                                       // :1006
        out.put("packingTypes", repo.packingTypes());                                              // :1022
        out.put("refParties", repo.referenceParties(u));                                           // ReferencePartiesDBCall:894
        out.put("otherItems", repo.otherItems(u));                                                 // OtherItemdtDbCall:1051
        out.put("historyCustomers", historyCustomers());                                           // HistoryComboDBCall:665
        return out;
    }

    /** Reset():2257 — UpdateDocumentNoUI(CommonServices.DeliveryOrderGenerateCode(84)). */
    public Map<String, Object> docNo() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", repo.generateCode(u, ctx.currentFinancialYearId(), branch(u)));
        return out;
    }

    /** HistoryComboDBCall:665 (btnRefreshHistory_Click:2364). */
    public List<Map<String, Object>> historyCustomers() {
        UserAccount u = ctx.requireAccountingUser();
        return repo.historyCustomers(u, ctx.currentFinancialYearId(), String.valueOf(branch(u)));
    }

    /** AvailableStockGetByItem:3608 — lblBalance (the page shows "0" when the value is not > 0). */
    public Map<String, Object> availableStock(int itemId, int warehouseId, int jobLotId, String cropYear, String docDate,
                                              int packingTypeId, int uomId) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("AvailableStock", repo.availableStockForDeliveryOrder(u, itemId, StoreIssuanceService.pickerDate(docDate),
                warehouseId, jobLotId, cropYear == null ? "" : cropYear.trim(), packingTypeId, uomId));
        return out;
    }

    /** UpdateAvailableStockInGridDetailRow:3477 — the grid row's AvailableStock (CropYear = the CropYearId cell's text). */
    public Map<String, Object> currentStock(int itemId, int warehouseId, int jobLotId, String cropYear, String docDate,
                                            int packingTypeId, int uomId) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("AvailableStock", repo.currentStockByItemId(u, itemId, StoreIssuanceService.pickerDate(docDate),
                warehouseId, jobLotId, cropYear == null ? "" : cropYear, packingTypeId, uomId));
        return out;
    }

    // =================================================================================== history

    /** gridhistoryfill:2388 — the raw procedure rows; the page de-duplicates by Id and builds the grid as the form does. */
    public List<Map<String, Object>> history(String dateMode, String fromDate, String toDate, int docNoFrom, int docNoTo,
                                             int supplierCustomerId) {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        boolean viewAll = rights.has(SCREEN_NAME, "viewAll");                                     // :2408
        Timestamp from = isBlank(fromDate) ? null : StoreIssuanceService.pickerDate(fromDate);   // FromDateHistory.Checked
        Timestamp to = isBlank(toDate) ? null : StoreIssuanceService.pickerDate(toDate);
        String m = dateMode == null ? "doc" : dateMode;
        Timestamp df = null, dt = null, ef = null, et = null, mf = null, mt = null, af = null, at = null;
        switch (m) {                                                                                 // :2413-2456
            case "entry":    ef = from; et = to; break;
            case "modify":   mf = from; mt = to; break;
            case "approved": af = from; at = to; break;
            default:         df = from; dt = to; break;
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, fy, viewAll, i(u.getId()), df, dt, ef, et, mf, mt, af, at,
                docNoFrom, docNoTo, supplierCustomerId)) {
            out.add(plain(r));
        }
        return out;
    }

    // ====================================================================================== read

    /** ReadById:2020 / DetailGridBind:2674 — header, ReadByIdDetailId rows and expense rows; null when not this company's transfer DO. */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) return null;
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", plain(h));
        List<Map<String, Object>> d = new ArrayList<>();
        for (Map<String, Object> r : repo.details(id)) d.add(plain(r));
        out.put("details", d);
        List<Map<String, Object>> e = new ArrayList<>();
        for (Map<String, Object> r : repo.expenses(id)) e.add(plain(r));
        out.put("expenses", e);
        return out;
    }

    /** btnprint_Click:2898 / history Print (262) and Print264 — CommonServices.InvDeliveryOrderSlip / DeliveryOrderSlip264. */
    public List<Map<String, Object>> slip(int id, boolean challan264) {
        UserAccount u = ctx.requireAccountingUser();
        if (challan264) {
            if (id <= 0) throw new IllegalArgumentException("No Record Found For Display");       // :9353
        } else if (id == 0) {
            throw new IllegalArgumentException("PrintId not found...");                           // :9395
        }
        if (ownedHeader(u, id) == null) return new ArrayList<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.slip(u, id)) out.add(plain(r));
        return out;
    }

    // ==================================================================================== delete

    /** btnDelete_Click:1992. */
    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "delete")) throw new RightsException("You do not have the Delete right for this screen.");
        if (id == 0) throw new IllegalArgumentException("Record Id Not Found");
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) throw new IllegalArgumentException("Record Id Not Found");
        if (bool(h.get("IsApproved"))) throw new IllegalArgumentException("Record has been approved");   // D6
        repo.removeById(u, id);
        return ok("Record Deleted Successfully", id, toInt(h.get("DocNo")));
    }

    // ====================================================================================== save

    /** Insert():1768 — Save (Id 0), SaveAs (Id 0) and Update (Id > 0). */
    @Transactional
    public Map<String, Object> save(DeliveryOrderTransferDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        int recId = i(dto.Id);
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) throw new RightsException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN_NAME, "update")) throw new RightsException("You do not have the Update right for this screen.");

        List<DeliveryOrderTransferDto.Row> rows = dto.rows == null ? new ArrayList<>() : dto.rows;
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");                 // :1780

        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = ownedHeader(u, recId);                                                             // D2
            if (existing == null) throw new IllegalArgumentException("Record Not Update because RecId Not Found");
        }
        Map<String, Object> saveAsSource = null;
        if (recId == 0 && i(dto.SaveAsFromId) > 0) {
            saveAsSource = ownedHeader(u, i(dto.SaveAsFromId));                                           // D2
            if (saveAsSource == null) throw new IllegalArgumentException("Record Not Found");
        }

        /* FormValidation():1107 — Branch, DocNo, DO Type, Sale Type (same messages, same order). */
        List<Map<String, Object>> branchList = repo.branches(u);
        Set<Integer> branchIds = ids(branchList, "Id");
        int branchFrom = i(dto.BranchFromId);
        Set<Integer> allowedFrom = new HashSet<>();                                                       // D3
        allowedFrom.add(branch(u));
        if (existing != null) allowedFrom.add(toInt(existing.get("BranchesId")));
        if (saveAsSource != null) allowedFrom.add(toInt(saveAsSource.get("BranchesId")));
        if (branchFrom == 0 || !branchIds.contains(branchFrom) || !allowedFrom.contains(branchFrom)) {
            throw new IllegalArgumentException("branch field is required");
        }
        int docNo = existing != null ? toInt(existing.get("DocNo")) : repo.generateCode(u, fy, branch(u));   // D1
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");
        if (i(dto.DoTypeId) != ORDER_TYPE_ID) throw new IllegalArgumentException("DeliveryOrderType Field is Required");
        int saleTypeId = i(dto.SaleTypeId);
        if (saleTypeId != 1 && saleTypeId != 2) throw new IllegalArgumentException("Sale Type Field is Required");   // SaleTypeBind:831
        /* :1786 — DO type 2 */
        int branchTo = i(dto.BranchToId);
        if (branchTo == 0 || !branchIds.contains(branchTo)) throw new IllegalArgumentException("'Branch To' Field is Required...");
        if (branchFrom == branchTo) throw new IllegalArgumentException(" 'Branch From' Cannot Be Equal To 'Branch To' ...");

        /* CmbVehicleType is LimitToList: its Text is one of the vehicle types or empty. */
        String vehicleType = dto.VehicleType == null ? "" : dto.VehicleType;
        if (!vehicleType.isEmpty()) {
            boolean known = existing != null && vehicleType.equals(str(existing.get("VehicleType")));
            if (!known) for (Map<String, Object> v : repo.vehicleTypes()) if (vehicleType.equals(str(v.get("VehicleDescription")))) { known = true; break; }
            if (!known) throw new IllegalArgumentException("Vehicle Type is not valid.");
        }

        /* D3 — the lists the combos are filled from. */
        Set<Integer> suppliers = new HashSet<>();
        for (Map<String, Object> s : suppliers(u)) suppliers.add(toInt(s.get("Id")));
        Set<Integer> refParties = ids(repo.referenceParties(u), "Id");
        Set<Integer> items = new HashSet<>();
        for (Map<String, Object> it : items(u)) items.add(toInt(it.get("Id")));
        Map<Integer, Set<Integer>> uomsByItem = new HashMap<>();
        for (Map<String, Object> m : repo.uomSchedule(u)) uomsByItem.computeIfAbsent(toInt(m.get("ItemId")), k -> new HashSet<>()).add(toInt(m.get("Id")));
        Map<Integer, String> crops = new HashMap<>();
        for (Map<String, Object> c : repo.cropYears(u)) crops.put(toInt(c.get("Id")), str(c.get("Name")));
        Set<Integer> packs = ids(repo.packingTypes(), "Id");
        Set<Integer> warehouses = ids(repo.warehousesWithBranches(u, branch(u)), "Id");
        Set<Integer> jobLots = ids(repo.jobLots(u), "Id");

        Map<Integer, Map<String, Object>> stored = new LinkedHashMap<>();
        if (existing != null) for (Map<String, Object> d : repo.details(recId)) stored.put(toInt(d.get("Id")), d);

        Timestamp docDate = StoreIssuanceService.formDate(dto.DocDate, recId);
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());

        List<Map<String, Object>> detailModels = new ArrayList<>();
        List<String> detailCropYears = new ArrayList<>();                 // vd.CropYear (virtual) — the stack warning's @CropYear
        /* :1855 — on Update the removed rows go first. */
        Set<Integer> postedIds = new HashSet<>();
        if (recId != 0) for (DeliveryOrderTransferDto.Row r : rows) if (i(r.Id) > 0) postedIds.add(i(r.Id));
        if (recId > 0 && dto.removedIds != null) {
            Set<Integer> seen = new HashSet<>();
            for (Integer rid : dto.removedIds) {
                int id = rid == null ? 0 : rid;
                if (id <= 0 || !seen.add(id)) continue;
                Map<String, Object> s = stored.get(id);
                if (s == null || postedIds.contains(id)) {
                    throw new IllegalArgumentException("A removed detail row does not belong to this document. Open the document again.");
                }
                detailModels.add(removedModel(s));
                detailCropYears.add(str(s.get("CropYear")));
            }
        }

        BigDecimal doTotalQty = BigDecimal.ZERO;
        double netWeight = 0d, packingWeight = 0d, grossWeight = 0d;
        Set<Integer> usedIds = new HashSet<>();
        for (int idx = 0; idx < rows.size(); idx++) {
            DeliveryOrderTransferDto.Row r = rows.get(idx);
            int n = idx + 1;
            int detailId = recId != 0 ? i(r.Id) : 0;                                                    // :1866
            int actionTypeId = detailId <= 0 ? 1 : 2;                                                     // :1867
            if (detailId > 0 && (!stored.containsKey(detailId) || !usedIds.add(detailId))) {              // D3
                throw new IllegalArgumentException("Detail row " + n + " does not belong to this document. Open the document again.");
            }
            /* :1879-1888 — FormHelper.ValidateField, same fields, same order. */
            required(i(r.SupplierCustomerId) != 0, "Customer", n);
            required(i(r.WareHouseId) != 0, "Warehouse", n);
            required(i(r.ItemId) != 0, "Item", n);
            required(i(r.CropYearId) != 0, "CropYear", n);
            required(i(r.JobLotId) != 0, "JobLot", n);
            required(i(r.PackingTypeId) != 0, "PackingType", n);
            required(i(r.ItemUOMId) != 0, "PackUom", n);
            required(dbl(r.LoadQty) > 0d, "Qty", n);
            required(dbl(r.LoadWeight) > 0d, "DoWeight", n);
            required(dbl(r.GrossWeight) > 0d, "GrossWeight", n);

            /* D3 — company-scoped re-validation (a value equal to the stored detail's is not re-checked). */
            Map<String, Object> old = detailId > 0 ? stored.get(detailId) : null;
            if (!same(old, "SupplierCustomerId", r.SupplierCustomerId) && !suppliers.contains(i(r.SupplierCustomerId))) invalid("Customer", n);
            if (i(r.RefPartyId) != 0 && !same(old, "RefPartyId", r.RefPartyId) && !refParties.contains(i(r.RefPartyId))) invalid("Reference Party", n);
            if (!same(old, "WarehouseId", r.WareHouseId) && !warehouses.contains(i(r.WareHouseId))) invalid("Warehouse", n);
            if (!same(old, "ItemId", r.ItemId) && !items.contains(i(r.ItemId))) invalid("Item", n);
            if (!same(old, "CropYearId", r.CropYearId) && !crops.containsKey(i(r.CropYearId))) invalid("CropYear", n);
            if (!same(old, "JobLotId", r.JobLotId) && !jobLots.contains(i(r.JobLotId))) invalid("JobLot", n);
            if (!same(old, "InvPackingTypeId", r.PackingTypeId) && !packs.contains(i(r.PackingTypeId))) invalid("PackingType", n);
            if (!(same(old, "ItemId", r.ItemId) && same(old, "PackUomId", r.ItemUOMId))) {
                Set<Integer> uoms = uomsByItem.get(i(r.ItemId));
                if (uoms == null || !uoms.contains(i(r.ItemUOMId))) invalid("PackUom", n);
            }

            /* FillDetailListCommonForInsertAndDelete:2085 — the StockTransfer branch (no order / schedule fields). */
            Map<String, Object> vd = detailModel();
            vd.put("DoQty", dbl(r.LoadQty));
            vd.put("DoWeight", dbl(r.LoadWeight));
            vd.put("LoadingQty", dbl(r.LoadQty));
            vd.put("LoadingWeight", dbl(r.LoadWeight));
            vd.put("PackingWeight", dbl(r.PackingUnit));
            vd.put("TotalPackingWeight", dbl(r.PackingWeight));                                          // :1892
            vd.put("GrossWeight", dbl(r.GrossWeight));
            vd.put("OuterEbTotal", dbl(r.PackingWeight));
            vd.put("Id", detailId);
            vd.put("InvPackingTypeId", i(r.PackingTypeId));
            vd.put("ItemId", i(r.ItemId));
            vd.put("PackUomId", i(r.ItemUOMId));
            vd.put("SupplierCustomerId", i(r.SupplierCustomerId));
            vd.put("WarehouseId", i(r.WareHouseId));
            vd.put("JobLotId", i(r.JobLotId));
            vd.put("RefPartyId", i(r.RefPartyId));
            vd.put("RefDocumentTypeId", old == null ? 0 : toInt(old.get("RefDocumentTypeId")));
            vd.put("RefDocIdNo", old == null ? 0 : toInt(old.get("RefDocIdNo")));
            vd.put("RefDocSubIdNo", old == null ? 0 : toInt(old.get("RefDocSubIdNo")));
            vd.put("CropYearId", i(r.CropYearId));
            vd.put("ActionTypeId", actionTypeId);
            vd.put("LoadingRemarks", r.Remarks == null ? "" : r.Remarks);                                // Conversion.ToString(cell)
            String cropText = crops.containsKey(i(r.CropYearId)) ? crops.get(i(r.CropYearId)) : (old == null ? "" : str(old.get("CropYear")));
            doTotalQty = doTotalQty.add(BigDecimal.valueOf(dbl(r.LoadQty)));                              // :1889
            netWeight += dbl(r.LoadWeight);
            packingWeight += dbl(r.PackingWeight);
            grossWeight += dbl(r.GrossWeight);
            detailModels.add(vd);
            detailCropYears.add(cropText);
        }

        /* :1896-1911 — Expense Grid rows, every row (the blank one too). */
        Map<Integer, Map<String, Object>> storedExp = new HashMap<>();
        int expSource = existing != null ? recId : (saveAsSource != null ? i(dto.SaveAsFromId) : 0);
        if (expSource > 0) for (Map<String, Object> e : repo.expenses(expSource)) storedExp.put(toInt(e.get("Id")), e);
        Set<Integer> otherItems = ids(repo.otherItems(u), "Id");
        List<Map<String, Object>> expenseModels = new ArrayList<>();
        List<DeliveryOrderTransferDto.Expense> exps = dto.expenses == null ? new ArrayList<>() : dto.expenses;
        for (int k = 0; k < exps.size(); k++) {
            DeliveryOrderTransferDto.Expense x = exps.get(k);
            Map<String, Object> se = i(x.Id) > 0 ? storedExp.get(i(x.Id)) : null;
            if (i(x.Id) > 0 && se == null) throw new IllegalArgumentException("Expense row " + (k + 1) + " does not belong to this document. Open the document again.");
            int saleOrderId = se == null ? 0 : toInt(se.get("SaleOrderId"));
            if (saleOrderId > 0) {                                                                        // :1902 — grid OrderIds are all 0 here
                throw new IllegalArgumentException("Order Id in 'row No " + (k + 1) + " in Expense Grid' Does not Exists in 'Detail Grid'\n"
                        + "Please Remove that row or And Revelent SaleOrder row In Detail");
            }
            int itemId = i(x.ItemId);
            if (itemId != 0 && !(se != null && toInt(se.get("ItemId")) == itemId) && !otherItems.contains(itemId)) {
                throw new IllegalArgumentException("Item is not valid in row No " + (k + 1) + " in Expense Grid");
            }
            Map<String, Object> ob = new LinkedHashMap<>();                                             // Model 0968
            ob.put("Id", i(x.Id));
            ob.put("SaleOrderId", saleOrderId);
            ob.put("SaleOrderCustomerExpId", se == null ? 0 : toInt(se.get("SaleOrderCustomerExpId")));
            ob.put("ExImInvoiceId", 0);
            ob.put("InvoiceOtherItemDetailId", 0);
            ob.put("InvDeliveryOrderId", 0);
            ob.put("ItemId", itemId);
            ob.put("Qty", dbl(x.Qty));
            ob.put("WeightPerQty", 0d);
            ob.put("NetWeight", 0d);
            ob.put("Remarks", x.Remarks == null ? "" : x.Remarks);
            expenseModels.add(ob);
        }

        /* :1913 — DeliveryOrderStackWarningMessage, answered on the page (D4). */
        if (!Boolean.TRUE.equals(dto.ConfirmWarning)) {
            String warning = "";
            for (int k = 0; k < detailModels.size(); k++) {
                Map<String, Object> d = detailModels.get(k);
                String w = repo.stackWarning(u, toInt(d.get("ItemId")), toInt(d.get("WarehouseId")), toInt(d.get("JobLotId")),
                        toInt(d.get("InvPackingTypeId")), toInt(d.get("PackUomId")), docDate, detailCropYears.get(k),
                        toDouble(d.get("DoWeight")));
                if (w != null) { warning = w; break; }
            }
            if (!warning.isEmpty()) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("success", false);
                m.put("confirm", true);
                m.put("message", warning);
                return m;
            }
        }

        /* Model 1006 InvDeliveryOrder — every non-virtual property, in declaration order. */
        Map<String, Object> attachSrc = existing != null ? existing : saveAsSource;                      // D5 / note 8
        boolean stockReservedVisible = toBool(common.config(u, "IsStockReservedPerParty"));
        Map<String, Object> po = new LinkedHashMap<>();
        po.put("IsApproved", existing != null && bool(existing.get("IsApproved")));                     // :1809
        po.put("IsStockReserved", stockReservedVisible && Boolean.TRUE.equals(dto.IsStockReserved));   // :1847
        po.put("ApprovedDate", null);
        po.put("ExpiryDate", null);
        po.put("ReturnableDate", null);
        po.put("DocDate", docDate);
        po.put("EntryDate", now);                                   // BLL Save:20
        po.put("ModifyDate", now);
        po.put("DoTotalQty", doTotalQty);
        po.put("ApprovedUser", 0);
        po.put("TransporterId", 0);
        po.put("BranchesId", branchFrom);
        po.put("ToBranchId", branchTo);
        po.put("FromBranchId", 0);
        po.put("CompanyId", u.getCompanyId());
        po.put("DocNo", docNo);
        po.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        po.put("EntryUser", i(u.getId()));
        po.put("Id", recId);
        po.put("ModifyUser", i(u.getId()));
        po.put("OrganizationId", u.getOrganizationId());
        po.put("ProjectsId", 0);
        po.put("EximInvoiceId", 0);
        po.put("FinancialYearId", fy);
        po.put("ActionId", recId == 0 ? 1 : 2);                    // BLL Save:28 / :32
        po.put("LoadingPortId", 0);
        po.put("SaleTypeId", saleTypeId);
        po.put("DepartmentFromId", 0);
        po.put("DepartmentToId", 0);
        po.put("RequestedByLookUpId", 0);
        po.put("ApprovedByLookUpId", 0);
        po.put("LoadingInstructions", dto.Remarks == null ? "" : dto.Remarks);
        po.put("DeliveryOrderType", ORDER_TYPE);                    // CmbDeliveryOrderType.Text
        po.put("OtherWeightRemarks", null);
        po.put("AccountRemarks", null);
        po.put("VehicleNo", dto.VehicleNo == null ? "" : dto.VehicleNo.trim().toUpperCase());   // txtVehicleNo_TextChanged upper-cases
        po.put("ScreenName", SCREEN_NAME);
        po.put("VehicleType", vehicleType);
        po.put("GrossWeight", grossWeight);
        po.put("NetWeight", netWeight);
        po.put("PackingWeight", packingWeight);
        po.put("OtherWeight", 0d);
        po.put("AttachmentsValues", attachSrc == null ? "" : attachSrc.get("AttachmentsValues"));
        po.put("CustomAttachmentsValues", attachSrc == null ? "" : attachSrc.get("CustomAttachmentsValues"));

        /* BLL Save:23 — an insert may carry ActionTypeId 1 only (always true here: removed rows exist only on Update). */
        /* DAL 0411 SetData:16 — one transaction (this method's). */
        if (detailModels.isEmpty()) throw new IllegalArgumentException("Detail List not found");
        int num = repo.setProc(recId == 0 ? "Sp_InvDeliveryOrder_Insert" : "Sp_InvDeliveryOrder_Update", po);
        int headerId;
        if (num > 0) headerId = num; else { num = recId; headerId = recId; }
        if (headerId <= 0) throw new IllegalStateException("Save returned no document id.");
        for (Map<String, Object> d : detailModels) {
            d.put("InvDeliveryOrderId", headerId);
            repo.setProc("Sp_InvDeliveryOrderDetail_Insert", d);
        }
        for (Map<String, Object> e : expenseModels) {
            e.put("InvDeliveryOrderId", headerId);
            repo.setProc("Sp_InvDeliveryOrderExpense_Insert", e);
        }
        repo.setProc("[DAW].[USp_DocumentApprovalDetail_Insert]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID,
                "Id", headerId,
                "LimitAmount", BigDecimal.ZERO));

        return ok((recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + docNo, num, docNo);
    }

    // =================================================================================== helpers

    /** SupplierDtFillFromGlobal:848 — with ERP feature 4 (subsidiary accounts on vouchers) only PartyTypeId 2. */
    private List<Map<String, Object>> suppliers(UserAccount u) {
        boolean subsidiary = repo.erpFeature(u, FEATURE_SUBSIDIARY_ACCOUNTS);                            // :524
        int showBoth = toInt(common.config(u, "ShowBothVendorAndCustomerOnSalesPurchase"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> s : repo.suppliers(u, showBoth)) {
            if (subsidiary && toInt(s.get("PartyTypeId")) != 2) continue;
            out.add(s);
        }
        return out;
    }

    /** ItemDtFillFromGlobal:926 — getGlobalAllItems without ItemTypeOfTypeId 14 and 17. */
    private List<Map<String, Object>> items(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.allItems(u)) {
            int t = toInt(r.get("ItemTypeOfTypeId"));
            if (t == 14 || t == 17) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("ItemName", r.get("ItemName"));
            o.put("ItemCode", r.get("ItemCode"));
            out.add(o);
        }
        return out;
    }

    /** D2 — this company's DocumentTypeId-84 "StockTransfer" header, or null. */
    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(h.get("OrganizationId")) != i(u.getOrganizationId())) return null;
        if (toInt(h.get("CompanyId")) != i(u.getCompanyId())) return null;
        if (toInt(h.get("DocumentTypeId")) != DOCUMENT_TYPE_ID) return null;
        if (!ORDER_TYPE.equals(str(h.get("DeliveryOrderType")))) return null;
        return h;
    }

    /** Model 1000 InvDeliveryOrderdetail — every non-virtual property, in declaration order, at its default. */
    private static Map<String, Object> detailModel() {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("DoQty", 0d);
        d.put("DoWeight", 0d);
        d.put("LoadingQty", 0d);
        d.put("LoadingWeight", 0d);
        d.put("PackingWeight", 0d);
        d.put("TotalPackingWeight", 0d);
        d.put("GrossWeight", 0d);
        d.put("StockWeight", 0d);
        d.put("OtherWeight", 0d);
        d.put("InnerQty", 0d);
        d.put("InnerUomId", 0d);
        d.put("InnerEbUnit", 0d);
        d.put("InnerEbTotal", 0d);
        d.put("AccessWtSet", 0d);
        d.put("OuterEbTotal", 0d);
        d.put("Id", 0);
        d.put("InvDeliveryOrderId", 0);
        d.put("InvPackingTypeId", 0);
        d.put("ItemId", 0);
        d.put("PackUomId", 0);
        d.put("CastingTypeId", 0);
        d.put("ItemVariantId", 0);
        d.put("SaleOrderId", 0);
        d.put("SaleOrderDetailId", 0);
        d.put("InvoiceDetailId", 0);
        d.put("SupplierCustomerId", 0);
        d.put("WarehouseId", 0);
        d.put("WareHouseToId", 0);
        d.put("JobLotId", 0);
        d.put("ToJobLotId", 0);
        d.put("RefPartyId", 0);
        d.put("RefDocumentTypeId", 0);
        d.put("RefDocIdNo", 0);
        d.put("RefDocSubIdNo", 0);
        d.put("CropYearId", 0);
        d.put("ExImInvoiceId", 0);
        d.put("ActionTypeId", 0);
        d.put("BagTypeId", 0);
        d.put("ContainerId", 0);
        d.put("DeliveryTypeId", 0);
        d.put("AssetId", 0);
        d.put("LoadingRemarks", null);
        d.put("ContainerRemarks", null);
        d.put("InspectionRemarks", null);
        d.put("ItemDiscription", null);
        d.put("DeliveryScheduleId", 0);
        d.put("DeliveryScheduleDetailId", 0);
        d.put("ThirdPartyAnalysisSubId", 0);
        d.put("ThirdPartyAnalysisId", 0);
        d.put("IsAssetItem", false);
        return d;
    }

    /**
     * DeleteDetailRow:1587 — the removed row as FillDetailListCommonForInsertAndDelete builds it from the grid, whose
     * values are ReadById's (FilldtDetailFromListCommonForReadById:2129): LoadQty = LoadingQty, LoadWeight =
     * LoadingWeight, PackingUnit = PackingWeight, PackingWeight = OuterEbTotal; ActionTypeId 3, TotalPackingWeight
     * not set. Rebuilt from the stored detail (D3).
     */
    private static Map<String, Object> removedModel(Map<String, Object> s) {
        Map<String, Object> vd = detailModel();
        vd.put("DoQty", toDouble(s.get("LoadingQty")));
        vd.put("DoWeight", toDouble(s.get("LoadingWeight")));
        vd.put("LoadingQty", toDouble(s.get("LoadingQty")));
        vd.put("LoadingWeight", toDouble(s.get("LoadingWeight")));
        vd.put("PackingWeight", toDouble(s.get("PackingWeight")));
        vd.put("GrossWeight", toDouble(s.get("GrossWeight")));
        vd.put("OuterEbTotal", toDouble(s.get("OuterEbTotal")));
        vd.put("Id", toInt(s.get("Id")));
        vd.put("InvPackingTypeId", toInt(s.get("InvPackingTypeId")));
        vd.put("ItemId", toInt(s.get("ItemId")));
        vd.put("PackUomId", toInt(s.get("PackUomId")));
        vd.put("SupplierCustomerId", toInt(s.get("SupplierCustomerId")));
        vd.put("WarehouseId", toInt(s.get("WarehouseId")));
        vd.put("JobLotId", toInt(s.get("JobLotId")));
        vd.put("RefPartyId", toInt(s.get("RefPartyId")));
        vd.put("RefDocumentTypeId", toInt(s.get("RefDocumentTypeId")));
        vd.put("RefDocIdNo", toInt(s.get("RefDocIdNo")));
        vd.put("RefDocSubIdNo", toInt(s.get("RefDocSubIdNo")));
        vd.put("CropYearId", toInt(s.get("CropYearId")));
        vd.put("ActionTypeId", 3);
        vd.put("LoadingRemarks", str(s.get("LoadingRemarks")));
        return vd;
    }

    /** FormHelper.ValidateField:503 — "{field} is required in Detail Grid at row No: {n}". */
    private static void required(boolean ok, String field, int n) {
        if (!ok) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + n);
    }

    private static void invalid(String field, int n) {
        throw new IllegalArgumentException(field + " is not valid in Detail Grid at row No: " + n);
    }

    /** The posted id equals the stored detail's column (false when there is no stored row). */
    private static boolean same(Map<String, Object> stored, String column, Integer posted) {
        return stored != null && toInt(stored.get(column)) == i(posted);
    }

    /** A missing Save / Update / Delete right — the controller's only 403. */
    public static class RightsException extends RuntimeException {
        public RightsException(String message) { super(message); }
    }

    private static Set<Integer> ids(List<Map<String, Object>> rows, String key) {
        Set<Integer> s = new HashSet<>();
        for (Map<String, Object> r : rows) {
            int v = toInt(r.get(key));
            if (v > 0) s.add(v);
        }
        return s;
    }

    private static List<Map<String, Object>> project(List<Map<String, Object>> rows, String id, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get(id)));
            o.put("Name", str(r.get(name)));
            out.add(o);
        }
        return out;
    }

    /** A row with its dates as local "yyyy-MM-ddTHH:mm:ss" text (no time-zone shift in JSON). */
    private static Map<String, Object> plain(Map<String, Object> r) {
        Map<String, Object> o = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : r.entrySet()) {
            Object v = e.getValue();
            if (v instanceof Timestamp) v = ((Timestamp) v).toLocalDateTime().withNano(0).toString();
            else if (v instanceof java.sql.Date) v = ((java.sql.Date) v).toLocalDate().toString();
            else if (v instanceof java.util.Date) v = new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString();
            else if (v instanceof LocalDateTime) v = ((LocalDateTime) v).withNano(0).toString();
            else if (v instanceof LocalDate) v = v.toString();
            else if (v instanceof java.time.OffsetDateTime) v = ((java.time.OffsetDateTime) v).toLocalDateTime().withNano(0).toString();
            o.put(e.getKey(), v);
        }
        return o;
    }

    private static boolean bool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        return v != null && ("true".equalsIgnoreCase(String.valueOf(v).trim()) || "1".equals(String.valueOf(v).trim()));
    }

    /** Conversion.ToBool(string) — Convert.ToBoolean, else Convert.ToInt32 != 0, else false. */
    private static boolean toBool(String s) {
        if (s == null) return false;
        String t = s.trim();
        if (t.equalsIgnoreCase("true")) return true;
        if (t.equalsIgnoreCase("false") || t.isEmpty()) return false;
        try { return Integer.parseInt(t) != 0; } catch (NumberFormatException e) { return false; }
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static Integer i(Integer v) { return v == null ? 0 : v; }
    private static double dbl(Double v) { return v == null ? 0d : v; }
    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }

    private static Map<String, Object> ok(String message, int id, int docNo) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        m.put("id", id);
        m.put("docNo", docNo);
        return m;
    }
}
