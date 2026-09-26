package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.PurchaseInvoiceDirectStoreDto;
import com.mst.repositories.PurchaseInvoiceDirectStoreRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.lang.reflect.Field;
import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.clr;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Store Purchase (ModuleId 67) — screen 334 "Purchase Invoice Direct Store".
 *
 * <pre>
 * Desktop form   Architecture.WinApp.StoreManagement.frmPurchaseInvoiceDirectStore (5,936 lines)
 * ScreenName     frmPurchaseInvoiceDirectStore   (base.Name — the rights key)
 * DocumentType   61   (Load:376 — any Tag other than "PurchaseInvoiceDirectWithTax")
 * Route          /store/purchase-invoice-direct-store      API /api/store/purchase-invoice-direct-store
 * BLL / DAL      BLL 0581 + DAL 0434 Architecture.*.Inventory.InvPurchaseInvoice (Save / SetData,
 *                GetByID / GetDate, FormHistory, RemoveByID → AccountandInventoryRemoveById);
 *                BLL 0549 PurchaseInvoiceFinancial.MakeVoucherForPurchaseInvoice;
 *                BLL 0614 PurchaseInvoiceGeneralFinancialMethods (ItemAndSupplierTradingFinancial,
 *                FreightFinancial, SupplierAddLessFinancial — the three that produce lines for 61);
 *                BLL 0379 GlobalServicesMethods, 0019 JobLotsAllocationToBranch, 0594 jobLot,
 *                0056 GetAvgRatesAndStockInHand, 0267 CommonServies, 0141 VoucherReports.
 * Models         1036 InvPurchaseInvoice, 1037 InvPurchaseInvoiceDetail, 1035 InvPurchaseInvoiceFreight,
 *                1038 InvPurchaseInvoiceJournal, 1195 VoucherHead, 1191 VoucherDetail (via ContraVoucherDto).
 * </pre>
 *
 * DocumentTypeId 131 is the SAME form opened with Tag "PurchaseInvoiceDirectWithTax"
 * (CommonServices.OpenDynamicallyScreen sets form.Tag = ScreenDefinition.ScreenName): IsTaxable,
 * tax invoice no / tax account visible, items from the item tax schedule, tax per row, slip 237.
 * Screen 334 opens the 61 path; nothing of the 131 path is reachable from this route, so the
 * tax controls and columns stay hidden exactly as Load:342-345 hides them.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 * ---------------------------------------------------------------------------------------------
 *  1. Freight rows post only a CREDIT (Freight cell, caption "Credit") to the transporter
 *     (FreightFinancial, BLL 0614:340); the matching debit is the per-row charge-to-product line
 *     (BLL 0614:1430) of Math.Round(total)/ΣQty*Qty, while the credit is Conversion.ToInt of each
 *     row (Insert:1331). When the two differ by 1 or more USP_VoucherBalanceCheck refuses the save.
 *  2. A freight row whose transporter is the supplier's own GL account is added to the bill
 *     (BillAmount:1643) — the comparison is the Transporter VALUE against the supplier's
 *     GlAccountId as text, so with ERP feature 4 on (the value is a SupplierCustomer id) it only
 *     matches by coincidence.
 *  3. With ERP feature 4 OFF the transporter / journal account lists hold GL ids in both columns
 *     (AccountsFill:886), so InvPurchaseInvoiceFreight/Journal.SupplierCustomerId store the GL id
 *     and the voucher gets SupplierCustomerId / SubsidiaryAccountId = that GL id.
 *  4. The GL id behind a Transporter / Account cell is found by TITLE (dtAccountlst.Select
 *     "AccountTitle='…'", :1804/:1977) — two accounts with the same title resolve to the first.
 *     The "+" on a freight row copies Transporter and Remarks but not GlAccountId (:1756), so the
 *     copy posts no transporter voucher line until its Transporter cell is changed again.
 *  5. The first freight row is created with Remarks "0" (AddRowInFreightGrid:1690 Rows.Add(0,0,0)),
 *     so a transporter picked on it without touching Remarks gets the voucher comment "0".
 *  6. EntryUser is set to the signed-in user on UPDATE as well (Insert:1264).
 *  7. SupplierInvoiceDate, DueDate and every row's GpDate are DateTime.Now (Insert:1259/1268/1299).
 *  8. The detail row's saved BillAmount is recomputed by the DAL (DAL 0434:87-94) as
 *     ItemAmount + Freight (+ wages when ContractWagesChargetoProduct) — tax is not in it.
 *  9. Voucher RefDocNoId is the invoice id known at MakeVoucher time: 0 on a new invoice
 *     (BLL 0549:24, before the header is written).
 * 10. Voucher detail BranchesId is overwritten with the header's branch for every line (DAL 0434:505),
 *     whatever the row's warehouse branch.
 * 11. Deleting a saved grid row runs usp_StockInReferenceValidationReferredOrNot at once in its own
 *     transaction (DeleteDetailrow:2268); the Bill Amount / Freights are not recalculated until the
 *     next add, edit or save.
 * 12. History From/To carry the time of day of the picker (GetAll:2530 FromDateHistory.Value).
 * 13. BranchImplemented is never assigned on this form, so job lots load unscoped and the
 *     warehouse/job-lot branch check (btnAdd_Click:2017) fires whenever ERP feature 11 is on.
 * 14. The Doc No is regenerated on Reset only; a document saved while another user saved the
 *     same number is not re-numbered by the desktop. Here (deviation D1) the number is taken
 *     from the generator at save time.
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web-only)
 * ---------------------------------------------------------------------------------------------
 *  D1. Doc No / Branch Sr No are ReadOnly on the form: never taken from the request — generator on
 *      insert, the stored header on update.
 *  D2. Bill Amount, each row's Freights and BillAmount are recomputed on the server with the
 *      desktop formulas (BillAmount:1621, FreightProportion:2326, BillProportion:2360) — the
 *      posted values are ignored. The supplier's GL id is read from the supplier list server-side.
 *  D3. Open / update / delete / print only for an invoice of this company and DocumentTypeId 61.
 *  D4. Attachments are not ported; on update the stored AttachmentsValues / CustomAttachmentsValues
 *      are sent back unchanged so an update cannot wipe them.
 *  D5. History branch ids are intersected with the user's allowed branches (the desktop can only
 *      produce ids from that list, since it maps names back through dtBranch).
 *  D6. Crystal layouts (233 slip, 118 / 103 vouchers) are not rendered: the procedures' rows are returned.
 *  D7. Server-side integrity checks on the posted grid (the desktop builds these values itself, so it
 *      can never send anything else):
 *      a. on UPDATE every posted detail Id > 0 must be a detail of the stored invoice
 *         (USP_InvPurchaseInvoiceDetail_ReadById) and appear once — Sp_InvPurchaseInvoiceDetail_Insert
 *         updates by Id alone. On insert the BLL's own "detailId greater than zero" refusal applies.
 *      b. ItemId must be in the ItemTypeOfTypeId 14/17 item list (ItemDtsFillFromGlobal:744);
 *         JobLotId, when non-zero, in the job-lot list (Branchlot:647).
 *      c. a freight / journal row's GlAccountId must be 0 or the id the desktop's title lookup gives
 *         for its Transporter / AccountId value (SupCustIdUpdateforFrieghtGrid:1789 /
 *         SupCustIdUpdateforGLGrid:1962 over AccountsFill's dtAccountlst).
 *      d. TaxNameId / TaxName / TaxPercent / TaxAmount are forced to 0 / "" — DocumentTypeId 61
 *         never looks tax up (btnAdd_Click:2028 only for 131).
 *  D8. btnRefresh_Click:1580 → GridcomboBind:1660 rebinds the Transporter / Account columns keyed by
 *      "Id" (the GL id) while grdFreightSettings / gridGLSettings key them by "SuppliercustomerId";
 *      with ERP feature 4 on, after a Refresh the desktop stores GL ids as SupplierCustomerId. This is a
 *      desktop defect and is NOT reproduced: the page's Refresh keeps the SupplierCustomerId key.
 */
@Service
public class PurchaseInvoiceDirectStoreService {

    public static final int DOCUMENT_TYPE_ID = 61;
    public static final String SCREEN_NAME = "frmPurchaseInvoiceDirectStore";

    /** AccountsFill:874 — AccountTypeIds allowed when feature 4 is off. */
    private static final int[] ACCOUNT_TYPES = { 3, 4, 6, 7, 8, 11, 12, 13, 14, 20, 21 };

    private final PurchaseInvoiceDirectStoreRepository repo;
    private final StoreIssuanceRepository shared;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public PurchaseInvoiceDirectStoreService(PurchaseInvoiceDirectStoreRepository repo, StoreIssuanceRepository shared,
                                             StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.shared = shared;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ====================================================================================== load

    /** InvfrmPurchasedirectInvoice_Load:333 (and btnRefresh_Click:1580, which re-reads the same lists). */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        int branch = branch(u);
        int fy = ctx.currentFinancialYearId();
        boolean feature4 = repo.erpFeature(u, 4);                                  // :340
        boolean branchFeature = repo.erpFeature(u, 11);                            // :401
        boolean branchImplemented = false;                                         // never assigned on this form

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));
        out.put("subsidiaryAccountAllowed", feature4);
        out.put("branchFeature", branchFeature);
        out.put("branchImplemented", branchImplemented);
        out.put("docNo", repo.nextDocNo(u, fy, DOCUMENT_TYPE_ID));                 // DocumentNo:973
        out.put("branchSrNo", repo.nextBranchSrNo(u, fy, branch, DOCUMENT_TYPE_ID)); // BranchSrNoFill:574
        out.put("userBranchId", branch);

        /* SupplierDtFillFromGlobal:604 */
        out.put("suppliers", repo.suppliers(u, toInt(shared.config(u, "ShowBothVendorAndCustomerOnSalesPurchase"))));

        /* ItemDtsFillFromGlobal:744 — ItemTypeOfTypeId 14 / 17 only. */
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : repo.allItems(u)) {
            int t = toInt(ci(r, "ItemTypeOfTypeId"));
            if (t != 14 && t != 17) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemCode", str(ci(r, "ItemCode")));
            items.add(o);
        }
        out.put("items", items);

        out.put("accounts", accountList(u, feature4));                             // AccountsFill:857

        /* bindWareHouse:689 — IsActive only. */
        List<Map<String, Object>> whs = new ArrayList<>();
        for (Map<String, Object> w : repo.warehousesWithBranches(u, branch)) if (Boolean.TRUE.equals(w.get("IsActive"))) whs.add(w);
        out.put("warehouses", whs);
        out.put("racks", repo.racks(u, branch));
        out.put("uoms", repo.uoms(u));
        /* Branchlot:647 — BranchesId = (BranchFeature && BranchImplemented) ? user's : 0. */
        out.put("jobLots", repo.jobLots(u, (branchFeature && branchImplemented) ? branch : 0));
        out.put("itemConditions", shared.itemConditions());                        // ConditiontFillFromGlobalAndBind:1005
        out.put("defaultWarehouseForStoreFlow", toInt(shared.config(u, "DefaultWarehouseForStoreFlow")));

        /* HistoryBranchComboFill:2408 (BranchImplemented false → the allocation list). */
        out.put("historyBranches", historyBranchList());
        return out;
    }

    /** Reset():1532-1533 — DocumentNo() and BranchSrNoFill() only. */
    public Map<String, Object> numbers() {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", repo.nextDocNo(u, fy, DOCUMENT_TYPE_ID));
        out.put("branchSrNo", repo.nextBranchSrNo(u, fy, branch(u), DOCUMENT_TYPE_ID));
        return out;
    }

    /** btnRefreshHistory_Click:2950 → HistoryBranchComboFill:2408. */
    public List<Map<String, Object>> historyBranchList() {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> branches = new ArrayList<>();
        for (Map<String, Object> r : repo.historyBranches(u, DOCUMENT_TYPE_ID)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "BranchId")));
            o.put("BranchName", str(ci(r, "BranchName")));
            branches.add(o);
        }
        return branches;
    }

    /** AccountsFill:857 — dtAccountlst (Id = GL account, SupplierCustomerId = the value-list value). */
    private List<Map<String, Object>> accountList(UserAccount u, boolean feature4) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (feature4) {
            for (Map<String, Object> r : repo.vendorsForTransporter(u)) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", toInt(ci(r, "GlAccountId")));
                o.put("SupplierCustomerId", toInt(ci(r, "Id")));
                o.put("AccountTitle", str(ci(r, "CompanyName")));
                out.add(o);
            }
            return out;
        }
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> r : repo.accountsWithCustomGroup(u)) {
            int type = toInt(ci(r, "AccountTypeId"));
            boolean ok = false;
            for (int t : ACCOUNT_TYPES) if (t == type) { ok = true; break; }
            if (!ok) continue;
            int id = toInt(ci(r, "ChartOfAccountId"));
            if (seen.add(id)) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", id);
                o.put("SupplierCustomerId", id);
                o.put("AccountTitle", str(ci(r, "AccountTitle")));
                out.add(o);
            }
        }
        return out;
    }

    /** BalanceStock:1146 — QtyInHand rounded to 2 (JobLotId is set on the parameters but the BLL never sends it). */
    public Map<String, Object> stock(int itemId, String docDate, int itemConditionId, int warehouseId, int rackId) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("qtyInHand", 0d);
        if (itemId == 0) return out;
        Map<String, Object> r = shared.avgRateAndStock(u, itemId, StoreIssuanceService.pickerDate(docDate),
                itemConditionId, 0, 0, warehouseId, rackId);
        if (r != null) out.put("qtyInHand", round2(toDouble(ci(r, "QtyInHand"))));
        return out;
    }

    /**
     * HistoryComboFill:2452 — the supplier filter. branchIds is the ",id,id" string the page builds
     * from its checked branches; blank means none checked: with validate the desktop refuses,
     * without it (Form_Load) the user's own branch is used.
     */
    public List<Map<String, Object>> historySuppliers(String branchIds, boolean validate) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> allowed = repo.historyBranches(u, DOCUMENT_TYPE_ID);
        String ids = null;
        if (!allowed.isEmpty()) {
            ids = allowedIds(allowed, branchIds);
            if (ids.isEmpty()) {
                if (validate) throw new IllegalArgumentException("Select branch first");
                ids = String.valueOf(branch(u));
            }
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.historyCombos(u, String.valueOf(DOCUMENT_TYPE_ID), ids)) {
            if (!"Supplier".equals(str(ci(r, "Activity")))) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Supplier", str(ci(r, "ReferenceName")));
            out.add(o);
        }
        return out;
    }

    // ================================================================================= history

    /** GetAll:2510. dateType: doc | entry | modify | approved (the four radio buttons). */
    public List<Map<String, Object>> history(String dateType, String from, String to, int fromDocNo, int toDocNo,
                                             int supplierCustomerId, String branchIds) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN_NAME, "viewAll");
        String ids = allowedIds(repo.historyBranches(u, DOCUMENT_TYPE_ID), branchIds);
        if (ids.isEmpty()) throw new IllegalArgumentException("Select branch first");       // :2624
        Timestamp f = isBlank(from) ? null : StoreIssuanceService.pickerDate(from);
        Timestamp t = isBlank(to) ? null : StoreIssuanceService.pickerDate(to);
        String dt = dateType == null ? "doc" : dateType;
        List<Map<String, Object>> rows = repo.formHistory(u, DOCUMENT_TYPE_ID, ctx.currentFinancialYearId(), viewAll,
                "doc".equals(dt) ? f : null, "doc".equals(dt) ? t : null,
                "entry".equals(dt) ? f : null, "entry".equals(dt) ? t : null,
                "modify".equals(dt) ? f : null, "modify".equals(dt) ? t : null,
                "approved".equals(dt) ? f : null, "approved".equals(dt) ? t : null,
                fromDocNo, toDocNo, supplierCustomerId, ids);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {                                         // :2589-2612
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("VoucherHeadId", toInt(ci(r, "VoucherHeadId")));
            o.put("DocumentTypeId", toInt(ci(r, "DocumentTypeId")));
            o.put("DocDate", ci(r, "DocDate"));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("BranchSrNo", toInt(ci(r, "BranchSrNo")));
            o.put("BranchName", str(ci(r, "BranchName")));
            o.put("DueDays", toInt(ci(r, "DueDays")));
            o.put("DueDate", ci(r, "DueDate"));
            o.put("ManualBillNo", str(ci(r, "ManualBillNo")));
            o.put("SupplierName", str(ci(r, "SupplierName")));
            o.put("BillAmount", toDouble(ci(r, "BillAmount")));
            o.put("ApprovedStatus", str(ci(r, "ApprovedStatus")));
            o.put("EntryUser", str(ci(r, "EntryUser")));
            o.put("EntryDate", ci(r, "EntryDate"));
            o.put("ModifyUser", str(ci(r, "ModifyUser")));
            o.put("ModifyDate", ci(r, "ModifyDate"));
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            o.put("Remarks", str(ci(r, "RemarksHeader")));
            out.add(o);
        }
        return out;
    }

    /** ",id,id" built from the requested ids that are in the user's allocation list, in request order. */
    private static String allowedIds(List<Map<String, Object>> allowed, String requested) {
        Set<Integer> ok = new HashSet<>();
        for (Map<String, Object> r : allowed) ok.add(toInt(ci(r, "BranchId")));
        StringBuilder sb = new StringBuilder();
        if (requested == null) return "";
        Set<Integer> done = new LinkedHashSet<>();
        for (String p : requested.split(",")) {
            int id = toInt(p.trim());
            if (id != 0 && ok.contains(id) && done.add(id)) sb.append(',').append(id);
        }
        return sb.toString();
    }

    // ==================================================================================== read

    /** ReadById:1425 — refused (null) when the invoice is not this company's DocumentTypeId 61. */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) return null;
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(ci(h, "Id")));
        out.put("DocNo", toInt(ci(h, "DocNo")));
        out.put("BranchSrNo", toInt(ci(h, "BranchSrNo")));
        out.put("SalesTaxNo", toInt(ci(h, "SalesTaxNo")));
        out.put("DocDate", ci(h, "DocDate"));
        out.put("SupplierCustomerId", toInt(ci(h, "SupplierCustomerId")));
        out.put("ReferencePartyId", toInt(ci(h, "ReferencePartyId")));
        out.put("ManualBillNo", str(ci(h, "ManualBillNo")));
        out.put("RemarksHeader", str(ci(h, "RemarksHeader")));
        out.put("BillAmount", toDouble(ci(h, "BillAmount")));
        out.put("IsApproved", toBool(ci(h, "IsApproved")));
        out.put("voucherHeadId", shared.voucherHeadId(u, DOCUMENT_TYPE_ID, id));   // VoucherHeadIdGet:1446

        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.details(id)) {                             // :1450 column order
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("ItemId", toInt(ci(d, "ItemId")));
            o.put("ItemCode", str(ci(d, "ItemCode")));
            o.put("Item", str(ci(d, "ItemName")));
            o.put("WarehouseId", toInt(ci(d, "WarehouseId")));
            o.put("Warehouse", str(ci(d, "WareHouseName")));
            o.put("RackId", toInt(ci(d, "RackId")));
            o.put("RackName", str(ci(d, "rackName")));
            o.put("ItemConditionId", toInt(ci(d, "ItemConditionId")));
            o.put("ItemCondition", str(ci(d, "ItemCondition")));
            o.put("JobLotId", toInt(ci(d, "JobLotId")));
            o.put("JobLot", str(ci(d, "JobLotDescription")));
            o.put("UOMId", toInt(ci(d, "ItemUOMId")));
            o.put("UOM", str(ci(d, "UOMCodeItem")));
            o.put("ItemQty", toDouble(ci(d, "ItemQty")));
            o.put("Rate", toDouble(ci(d, "ItemRate")));
            o.put("RateUOMId", toInt(ci(d, "UomScheduleIdRate")));
            o.put("RateUOM", str(ci(d, "RateUom")));
            o.put("ItemAmount", toDouble(ci(d, "ItemAmount")));
            o.put("TaxNameId", toInt(ci(d, "TaxNameId")));
            o.put("TaxName", str(ci(d, "TaxDescriptions")));
            o.put("TaxPercent", toDouble(ci(d, "TaxPercent")));
            o.put("TaxAmount", toDouble(ci(d, "TaxAmount")));
            o.put("BillAmount", toDouble(ci(d, "BillAmount")));
            o.put("Freights", toDouble(ci(d, "FreightAmount")));
            o.put("RemarksDetail", str(ci(d, "RemarksDetail")));
            o.put("GpNo", str(toInt(ci(d, "GpNo"))));
            o.put("VehicleNo", str(ci(d, "VehicleNo")));
            o.put("BranchId", toInt(ci(d, "BranchId")));
            o.put("BranchName", str(ci(d, "BranchName")));
            rows.add(o);
        }
        out.put("rows", rows);

        List<Map<String, Object>> fr = new ArrayList<>();
        for (Map<String, Object> f : repo.freight(id)) {                             // :1460
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Transporter", str(toInt(ci(f, "SupplierCustomerId"))));
            o.put("Freight", toDouble(ci(f, "FreightAmount")));
            o.put("Remarks", str(ci(f, "Remarks")));
            o.put("GlAccountId", toInt(ci(f, "TansporterId")));
            fr.add(o);
        }
        out.put("freight", fr);

        List<Map<String, Object>> jl = new ArrayList<>();
        for (Map<String, Object> j : repo.journal(id)) {                             // :1455
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("AccountId", str(toInt(ci(j, "SupplierCustomerId"))));
            o.put("Remarks", str(ci(j, "JvRemarks")));
            o.put("Percentage", clr(toDouble(ci(j, "JvPrcnt"))));
            o.put("Qty", clr(toDouble(ci(j, "JvQty"))));
            o.put("Rate", clr(toDouble(ci(j, "JvRate"))));
            o.put("Debit", toDouble(ci(j, "JvDebit")));
            o.put("Credit", toDouble(ci(j, "JvCredit")));
            o.put("GlAccountId", toInt(ci(j, "ChartofAccountId")));
            jl.add(o);
        }
        out.put("journal", jl);
        return out;
    }

    // ================================================================================== delete

    /** btnDelete_Click:3531. */
    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "delete")) throw new IllegalStateException("You do not have the Delete right for this screen.");
        if (id <= 0) throw new IllegalArgumentException("Record Id Not Found.....");
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) throw new IllegalArgumentException("Record Id Not Found.....");
        if (toBool(ci(h, "IsApproved"))) throw new IllegalArgumentException("Record Not Update because Record has approved");
        shared.removeInvoiceVoucherAndStock(u, DOCUMENT_TYPE_ID, id, u.getId());     // BLL 0581 RemoveByID
        return ok("Delete Record Successfully", id);
    }

    /** DeleteDetailrow:2252 — only for a saved row (DetailId > 0) of an opened invoice. */
    public Map<String, Object> checkDetailDelete(int id, int detailId) {
        UserAccount u = ctx.requireAccountingUser();
        if (detailId <= 0) return ok("", id);
        if (ownedHeader(u, id) == null) throw new IllegalArgumentException("Record Id Not Found.....");
        repo.stockInReferenceValidation(u, DOCUMENT_TYPE_ID, id, detailId);
        return ok("", id);
    }

    // =================================================================================== print

    /** btnSlip_Click / history Slip → PurchaseInvoiceStoreDirectSlip_233. */
    public List<Map<String, Object>> slip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        requirePrint();
        if (id == 0) throw new IllegalArgumentException("Record Id Not Found");
        if (ownedHeader(u, id) == null) throw new IllegalArgumentException("Record Id Not Found");
        return repo.slip(u, id);
    }

    /** btnPrint_Click / Print Voucher after save → VoucherReport_118 (VoucherHeadId resolved from the invoice). */
    public List<Map<String, Object>> voucher118(int id) {
        UserAccount u = ctx.requireAccountingUser();
        requirePrint();
        int vh = id > 0 && ownedHeader(u, id) != null ? shared.voucherHeadId(u, DOCUMENT_TYPE_ID, id) : 0;
        if (vh == 0) throw new IllegalArgumentException("VoucherId Not Found");
        return repo.voucher118(u, vh, DOCUMENT_TYPE_ID);
    }

    /** History "Voucher" button → AcRptPurchaseSalesVoucherSlip_103 with the row's VoucherHeadId. */
    public List<Map<String, Object>> voucher103(int id) {
        UserAccount u = ctx.requireAccountingUser();
        requirePrint();
        int vh = id > 0 && ownedHeader(u, id) != null ? shared.voucherHeadId(u, DOCUMENT_TYPE_ID, id) : 0;
        if (vh == 0) throw new IllegalArgumentException("No Record Found For Display");
        return repo.voucher103(u, vh, DOCUMENT_TYPE_ID);
    }

    private void requirePrint() {
        if (!rights.has(SCREEN_NAME, "print")) throw new IllegalStateException("You don't Have Print Right");
    }

    // ==================================================================================== save

    /**
     * saveToolStripButton_Click / btnUpdate_Click → Insert():1201 → BLL 0581 Save → DAL 0434 SetData,
     * all in ONE transaction (the DAL's SqlTransaction).
     */
    @Transactional
    public Map<String, Object> save(PurchaseInvoiceDirectStoreDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id == null ? 0 : dto.Id;
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN_NAME, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = ownedHeader(u, recId);
            if (existing == null) throw new IllegalArgumentException("Record Id Not Found.....");
            if (toBool(ci(existing, "IsApproved"))) throw new IllegalArgumentException("Record Not Update because Record has approved"); // :1415
        }
        int branch = branch(u);
        int fy = ctx.currentFinancialYearId();

        /* FormValidation:477 */
        int supplierId = i(dto.SupplierCustomerId);
        List<Map<String, Object>> suppliers = repo.suppliers(u, toInt(shared.config(u, "ShowBothVendorAndCustomerOnSalesPurchase")));
        Map<String, Object> supplier = null;
        for (Map<String, Object> s : suppliers) if (toInt(s.get("Id")) == supplierId) { supplier = s; break; }
        /* ActiveRow == null || Value == 0: a supplier that is not in the bound list has no active row. */
        if (supplierId == 0 || supplier == null) throw new IllegalArgumentException("Supplier Field is Required");
        int docNo = existing != null ? toInt(ci(existing, "DocNo")) : repo.nextDocNo(u, fy, DOCUMENT_TYPE_ID);        // D1
        int branchSrNo = existing != null ? toInt(ci(existing, "BranchSrNo")) : repo.nextBranchSrNo(u, fy, branch, DOCUMENT_TYPE_ID);
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");

        List<PurchaseInvoiceDirectStoreDto.FreightRow> freight = dto.freight == null ? new ArrayList<>() : dto.freight;
        List<PurchaseInvoiceDirectStoreDto.JournalRow> journal = dto.journal == null ? new ArrayList<>() : dto.journal;
        List<PurchaseInvoiceDirectStoreDto.Row> rows = dto.rows == null ? new ArrayList<>() : dto.rows;

        /* :1230-1245 */
        for (PurchaseInvoiceDirectStoreDto.FreightRow r : freight) {
            if (d(r.Freight) > 0 && cInt(r.Transporter) == 0) throw new IllegalArgumentException("Please Select an Account Against Freight First");
        }
        for (PurchaseInvoiceDirectStoreDto.JournalRow r : journal) {
            if ((d(r.Credit) > 0 || netInt(d(r.Debit)) > 0) && cInt(r.AccountId) == 0) throw new IllegalArgumentException("Please Select an Account Against JL First");
        }

        /* D7d — DocumentTypeId 61 carries no tax (before BillAmount, which adds TaxAmount). */
        for (PurchaseInvoiceDirectStoreDto.Row r : rows) {
            r.TaxNameId = 0; r.TaxName = ""; r.TaxPercent = 0d; r.TaxAmount = 0d;
        }

        /* BillAmount():1621 then FreightProportion():2326 — D2. */
        String supplierGlText = supplier == null ? "" : String.valueOf(toInt(supplier.get("GlAccountId")));
        double billAmount = billAmount(rows, freight, journal, supplierGlText);
        freightProportion(rows, freight);

        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");                              // :1388
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Timestamp docDate = StoreIssuanceService.formDate(dto.DocDate, recId);

        Map<String, Object> head = headerModel();
        head.put("Id", recId);
        head.put("BranchesId", branch);
        head.put("DocDate", docDate);
        head.put("DocNo", docNo);
        head.put("BranchSrNo", branchSrNo);
        head.put("SalesTaxNo", existing != null ? toInt(ci(existing, "SalesTaxNo")) : 0);   // txtTaxInvoiceNo (hidden, "" on 61)
        head.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        head.put("ManualBillNo", dto.ManualBillNo == null ? "" : dto.ManualBillNo.trim());
        head.put("ProjectsId", branch);
        head.put("RemarksHeader", dto.RemarksHeader == null ? "" : dto.RemarksHeader.trim());
        head.put("SupplierCustomerId", supplierId);
        head.put("ReferencePartyId", 0);                                              // CmbTaxAccount (hidden, empty)
        head.put("SupplierInvoiceDate", now);
        head.put("BillAmount", billAmount);
        head.put("OrganizationId", u.getOrganizationId());
        head.put("CompanyId", u.getCompanyId());
        head.put("FinancialYearId", fy);
        head.put("EntryUser", u.getId());
        head.put("EntryDate", now);
        head.put("ModifyDate", now);
        head.put("ModifyUser", u.getId());
        head.put("DueDate", now);
        head.put("IsTaxable", false);
        head.put("ScreenName", SCREEN_NAME);
        if (existing != null) {                                                       // D4
            head.put("AttachmentsValues", nullIfEmpty(str(ci(existing, "AttachmentsValues"))));
            head.put("CustomAttachmentsValues", nullIfEmpty(str(ci(existing, "CustomAttachmentsValues"))));
        }

        List<Map<String, Object>> details = new ArrayList<>();
        List<String> jobLotText = new ArrayList<>(), uomText = new ArrayList<>();
        for (int idx = 0; idx < rows.size(); idx++) {                                 // :1275
            PurchaseInvoiceDirectStoreDto.Row r = rows.get(idx);
            Map<String, Object> pd = detailModel();
            double qty = d(r.ItemQty);
            pd.put("Id", i(r.Id));
            pd.put("ItemId", i(r.ItemId));
            pd.put("WarehouseId", i(r.WarehouseId));
            pd.put("RackId", i(r.RackId));
            pd.put("ItemConditionId", i(r.ItemConditionId));
            pd.put("JobLotId", i(r.JobLotId));
            pd.put("ItemUOMId", i(r.UOMId));
            pd.put("ItemQty", qty);
            pd.put("GrossWeight", qty);
            pd.put("NetBillWeight", qty);
            pd.put("NetStockWeight", qty);
            pd.put("UomScheduleIdRate", i(r.RateUOMId));
            pd.put("ItemRate", d(r.Rate));
            pd.put("ItemAmount", d(r.ItemAmount));
            pd.put("TaxNameId", i(r.TaxNameId));
            pd.put("TaxDescriptions", r.TaxName == null ? "" : r.TaxName);
            pd.put("TaxPercent", d(r.TaxPercent));
            pd.put("TaxAmount", d(r.TaxAmount));
            pd.put("GpDate", now);
            pd.put("GpNo", cInt(r.GpNo));
            pd.put("VehicleNo", r.VehicleNo == null ? "" : r.VehicleNo);
            pd.put("RemarksDetail", r.RemarksDetail == null ? "" : r.RemarksDetail);
            pd.put("BillAmount", d(r.BillAmount));
            pd.put("FreightAmount", d(r.Freights));
            pd.put("BranchId", i(r.BranchId));
            validate(pd.get("WarehouseId"), "Warehouse", idx);                       // :1307-1315
            validate(pd.get("RackId"), "Rack Name", idx);
            validate(pd.get("ItemConditionId"), "ItemCondition", idx);
            validate(pd.get("JobLotId"), "Job Lot", idx);
            validate(pd.get("ItemUOMId"), "Pack Uom", idx);
            validate(pd.get("ItemQty"), "Item Qty", idx);
            validate(pd.get("UomScheduleIdRate"), "Rate Uom", idx);
            validate(pd.get("ItemRate"), "Item Rate", idx);
            validate(pd.get("ItemAmount"), "Item Amount", idx);
            details.add(pd);
            jobLotText.add(r.JobLot == null ? "" : r.JobLot);                         // Pd.JobLotDescription (virtual)
            uomText.add(r.UOM == null ? "" : r.UOM);                                  // Pd.UOMCodeItem (virtual)
        }

        List<Map<String, Object>> freightModels = new ArrayList<>();
        for (PurchaseInvoiceDirectStoreDto.FreightRow r : freight) {                  // :1323
            if (cInt(r.Transporter) == 0) continue;
            Map<String, Object> pf = new LinkedHashMap<>();
            pf.put("InvPurchaseInvoiceId", 0);
            pf.put("Id", 0);
            pf.put("FrRate", 0d);
            pf.put("FrWeight", 0d);
            pf.put("FrQty", 0d);
            pf.put("FreightAmount", (double) netInt(d(r.Freight)));                   // Conversion.ToInt(Freight)
            pf.put("Debit", 0d);
            pf.put("Percentage", 0d);
            pf.put("InvGrnId", 0);
            pf.put("FreightId", 0);
            pf.put("TansporterId", i(r.GlAccountId));
            pf.put("PurchaseOrderId", 0);
            pf.put("SupplierCustomerId", cInt(r.Transporter));
            pf.put("Remarks", r.Remarks == null ? "" : r.Remarks);
            freightModels.add(pf);
        }
        List<Map<String, Object>> journalModels = new ArrayList<>();
        for (PurchaseInvoiceDirectStoreDto.JournalRow r : journal) {                  // :1337
            if (cInt(r.AccountId) == 0) continue;
            Map<String, Object> pj = new LinkedHashMap<>();
            pj.put("Id", 0);
            pj.put("InvPurchaseInvoiceId", 0);
            pj.put("InvGrnId", 0);
            pj.put("FreightId", 0);
            pj.put("ChartofAccountId", i(r.GlAccountId));
            pj.put("TransporterSupCustId", 0);
            pj.put("SupplierCustomerId", cInt(r.AccountId));
            pj.put("JvRemarks", r.Remarks == null ? "" : r.Remarks);
            pj.put("JvDebit", d(r.Debit));
            pj.put("JvCredit", d(r.Credit));
            pj.put("JvPrcnt", toDouble(r.Percentage));
            pj.put("JvQty", toDouble(r.Qty));
            pj.put("JvRate", toDouble(r.Rate));
            pj.put("RowType", 0);
            journalModels.add(pj);
        }

        integrityChecks(u, recId, details, freight, journal);                         // D7a-c

        /* BLL 0581 Save:18 — voucher first, then the detail-id guard, then SetData. */
        Voucher voucher = makeVoucher(u, head, details, jobLotText, uomText, freightModels, journalModels);
        if (recId == 0) {
            for (Map<String, Object> pd : details) {
                if (toInt(pd.get("Id")) > 0) throw new IllegalArgumentException("Record cannot be inserted because detailId greater than zero");
            }
        }
        head.put("ActionId", recId == 0 ? 1 : 2);
        int id = setData(u, head, details, freightModels, journalModels, voucher, recId == 0 ? "Sp_InvPurchaseInvoice_Insert" : "Sp_InvPurchaseInvoice_Update");

        Map<String, Object> res = ok(recId > 0 ? "Record Update Successfully" : "Record Saved Successfully", id);
        return res;
    }

    /** D7a-c — values the desktop derives itself are accepted only when they match what it would derive. */
    private void integrityChecks(UserAccount u, int recId, List<Map<String, Object>> details,
                                 List<PurchaseInvoiceDirectStoreDto.FreightRow> freight,
                                 List<PurchaseInvoiceDirectStoreDto.JournalRow> journal) {
        /* a — detail ids on update */
        if (recId != 0) {
            Set<Integer> allowed = new HashSet<>();
            for (Map<String, Object> d : repo.details(recId)) allowed.add(toInt(ci(d, "Id")));
            Set<Integer> seen = new HashSet<>();
            for (int idx = 0; idx < details.size(); idx++) {
                int id = toInt(details.get(idx).get("Id"));
                if (id <= 0) continue;
                if (!allowed.contains(id) || !seen.add(id)) {
                    throw new IllegalArgumentException("Detail row " + (idx + 1) + " does not belong to this invoice.");
                }
            }
        }
        /* b — items and job lots */
        Set<Integer> items = new HashSet<>();
        for (Map<String, Object> r : repo.allItems(u)) {
            int t = toInt(ci(r, "ItemTypeOfTypeId"));
            if (t == 14 || t == 17) items.add(toInt(ci(r, "Id")));
        }
        Set<Integer> jobLots = new HashSet<>();
        for (Map<String, Object> j : repo.jobLots(u, 0)) jobLots.add(toInt(j.get("Id")));   // BranchImplemented false → unscoped
        for (int idx = 0; idx < details.size(); idx++) {
            Map<String, Object> d = details.get(idx);
            if (!items.contains(toInt(d.get("ItemId")))) throw new IllegalArgumentException("Item not found in Detail Grid at row No: " + (idx + 1));
            int jl = toInt(d.get("JobLotId"));
            if (jl != 0 && !jobLots.contains(jl)) throw new IllegalArgumentException("Job Lot not found in Detail Grid at row No: " + (idx + 1));
        }
        /* c — GL id behind a Transporter / Account value = the first account with the same title */
        List<Map<String, Object>> accounts = accountList(u, repo.erpFeature(u, 4));
        for (PurchaseInvoiceDirectStoreDto.FreightRow r : freight) {
            if (cInt(r.Transporter) == 0) continue;
            int gl = i(r.GlAccountId);
            if (gl != 0 && gl != glByTitle(accounts, r.Transporter)) throw new IllegalArgumentException("Transporter account not found.");
        }
        for (PurchaseInvoiceDirectStoreDto.JournalRow r : journal) {
            if (cInt(r.AccountId) == 0) continue;
            int gl = i(r.GlAccountId);
            if (gl != 0 && gl != glByTitle(accounts, r.AccountId)) throw new IllegalArgumentException("Supplier Add/Less account not found.");
        }
    }

    /** dtAccountlst.Select("AccountTitle='<title of the value>'")[0].Id — 0 when the value is not in the list. */
    private static int glByTitle(List<Map<String, Object>> accounts, String value) {
        int v = cInt(value);
        String title = null;
        for (Map<String, Object> a : accounts) if (toInt(a.get("SupplierCustomerId")) == v) { title = str(a.get("AccountTitle")); break; }
        if (title == null || title.isEmpty() || "0".equals(title)) return 0;
        for (Map<String, Object> a : accounts) if (title.equals(str(a.get("AccountTitle")))) return toInt(a.get("Id"));
        return 0;
    }

    // ================================================================= DAL 0434 SetData

    private int setData(UserAccount u, Map<String, Object> head, List<Map<String, Object>> details,
                        List<Map<String, Object>> freight, List<Map<String, Object>> journal,
                        Voucher voucher, String proc) {
        if (details.isEmpty()) throw new IllegalArgumentException("Detail List not found");                  // :34
        double invQty = 0, invWeight = 0;
        for (Map<String, Object> d : details) { invQty += toDouble(d.get("ItemQty")); invWeight += toDouble(d.get("NetBillWeight")); }
        head.put("InvoiceQty", BigDecimal.valueOf(invQty));                           // :40
        head.put("InvoiceWeight", BigDecimal.valueOf(invWeight));

        int recId = toInt(head.get("Id"));
        int num3 = repo.setProc(proc, head);                                          // :60
        if (num3 > 0) head.put("Id", num3); else num3 = recId;
        /* DEVIATION (defensive): the DAL would continue with Id 0 and write orphan rows. */
        if (num3 <= 0) throw new IllegalStateException("Save returned no document id.");

        boolean wages = StoreIssuanceService.toBool(shared.config(u, "ContractWagesChargetoProduct"));  // :72
        StringBuilder detailIds = new StringBuilder();
        int line = 0;
        for (Map<String, Object> d : details) {                                       // :73
            d.put("LineId", ++line);
            double bill = toDouble(d.get("ItemAmount")) + toDouble(d.get("FreightAmount")) + toDouble(d.get("ExpenseAmount"))
                    + toDouble(d.get("CommissionAmount")) + toDouble(d.get("Brokery"))
                    + (wages ? toDouble(d.get("WagesAmount")) : 0d)
                    - toDouble(d.get("EbPurAgainstWeightAmount")) - toDouble(d.get("FreightDeduction"));
            d.put("BillAmount", bill);
            d.put("InvPurchaseInvoiceId", num3);
            d.put("Id", repo.setProc("Sp_InvPurchaseInvoiceDetail_Insert", d));
            detailIds.append(toInt(d.get("Id"))).append(',');
        }
        for (Map<String, Object> f : freight) {                                       // :100
            f.put("InvPurchaseInvoiceId", num3);
            repo.setProc("Sp_InvPurchaseInvoiceFreight_Insert", f);
        }
        for (Map<String, Object> j : journal) {                                       // :105
            j.put("InvPurchaseInvoiceId", num3);
            repo.setProc("Sp_InvPurchaseInvoiceJournal_Insert", j);
        }
        repo.removeDetailsNotIn(u, num3, detailIds.toString());                       // :140
        repo.stockInTransitDelete(num3);                                              // :150 (DocumentTypeId != 59)
        repo.stockEvaluationUpdate(u, DOCUMENT_TYPE_ID, num3);                        // :467 (not feature 5 + 59)

        ContraVoucherDto.Head vh = voucher.head;
        int existingVoucher = shared.voucherHeadId(u, DOCUMENT_TYPE_ID, num3);        // :476
        if (existingVoucher > 0) vh.Id = existingVoucher;
        vh.DocumentTypeSrNo = num3;
        int num = repo.setProc(existingVoucher == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", model(vh));
        if (num > 0) vh.Id = num; else num = vh.Id;
        if (voucher.lines.isEmpty()) throw new IllegalArgumentException("VoucherDetail list Not Found");       // :570
        for (ContraVoucherDto.Detail l : voucher.lines) {                             // :502
            l.VoucherHeadId = vh.Id;
            l.BranchesId = toInt(head.get("BranchesId"));
            repo.setProc("Sp_VoucherDetail_Insert", model(l));
        }
        repo.voucherBalanceCheck(u, vh.Id);                                           // :508
        int documentTypeIdRef = repo.setProc("Sp_VoucherHead_H_Insert", model(vh));  // :517
        for (ContraVoucherDto.Detail l : voucher.lines) {
            l.VoucherHeadId = vh.Id;
            l.DocumentTypeIdRef = documentTypeIdRef;
            repo.setProc("Sp_VoucherDetail_H_Insert", model(l));
        }
        repo.inventoryTransactions(u, DOCUMENT_TYPE_ID, num3);                        // :524 (61 is in the set)
        ContraVoucherDto.ApprovalDetail appr = new ContraVoucherDto.ApprovalDetail(); // :556
        appr.OrganizationId = u.getOrganizationId();
        appr.CompanyId = u.getCompanyId();
        appr.DocumentTypeId = DOCUMENT_TYPE_ID;
        appr.Id = num3;
        appr.LimitAmount = BigDecimal.valueOf(toDouble(head.get("BillAmount")));
        repo.setProc("[DAW].[USp_DocumentApprovalDetail_Insert]", model(appr));
        return num3;
    }

    // ================================================================= BLL 0549 / 0614 voucher

    private static final class Voucher {
        ContraVoucherDto.Head head;
        List<ContraVoucherDto.Detail> lines = new ArrayList<>();
    }

    /** PurchaseInvoiceFinancial.MakeVoucherForPurchaseInvoice (BLL 0549) for DocumentTypeId 61. */
    private Voucher makeVoucher(UserAccount u, Map<String, Object> head, List<Map<String, Object>> details,
                                List<String> jobLotText, List<String> uomText,
                                List<Map<String, Object>> freight, List<Map<String, Object>> journal) {
        Voucher v = new Voucher();
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        v.head = vh;
        int recId = toInt(head.get("Id"));
        vh.DocumentTypeId = DOCUMENT_TYPE_ID;
        vh.DocumentTypeSrNo = recId;
        vh.RefDocNoId = recId;                                                        // behaviour 9
        vh.VoucherCode = toInt(head.get("DocNo"));
        vh.VoucherDate = String.valueOf(head.get("DocDate"));
        vh.RemarksOtherLingo = "";
        vh.VoucherAmount = toDouble(head.get("BillAmount"));
        vh.MultiCurrencyId = 0;
        vh.ExchangeCurrencyRate = 0d;
        vh.FcAmount = 0d;

        String stockAgainst = shared.config(u, "StockAgainstAccount");
        if (stockAgainst.isEmpty()) throw new IllegalArgumentException("Stock AgainstAc Configuration Not Found");
        int offsetAccountId = toInt(stockAgainst);
        String companyName = null;
        List<Map<String, Object>> sup = repo.supplierGlAccounts(u);
        if (!sup.isEmpty()) {
            Map<String, Object> s = null;
            int supplierId = toInt(head.get("SupplierCustomerId"));
            for (Map<String, Object> x : sup) if (toInt(ci(x, "Id")) == supplierId) { s = x; break; }
            if (s == null) throw new IllegalArgumentException("Supplier GLAccountId not Found");
            vh.RefAccountId = toInt(ci(s, "GlAccountId"));
            companyName = str(ci(s, "CompanyName"));
        }
        List<Map<String, Object>> itemGl = shared.itemGlAccounts(u);
        vh.ChequeDate = Timestamp.valueOf(LocalDate.now().atStartOfDay()).toString();
        vh.IncludeWHT = false;
        vh.BranchId = toInt(head.get("BranchesId"));
        vh.ProjectId = toInt(head.get("ProjectsId"));
        vh.BillAmount = toDouble(head.get("BillAmount"));
        vh.ManualBillNo = (String) head.get("ManualBillNo");
        vh.DueDate = String.valueOf(head.get("DueDate"));
        vh.DueDays = 0;
        vh.OrganizationId = u.getOrganizationId();
        vh.CompanyId = u.getCompanyId();
        vh.FinancialYearId = toInt(head.get("FinancialYearId"));
        vh.EntryUser = u.getId();
        String now = Timestamp.valueOf(LocalDateTime.now()).toString();
        vh.EntryDate = now;
        vh.ModifyDate = now;
        vh.ModifyUser = u.getId();
        int refAc = vh.RefAccountId;
        int supplierId = toInt(head.get("SupplierCustomerId"));

        /* ItemAndSupplierTradingFinancial (BLL 0614:1282), case 61. */
        Map<Integer, Integer> jobLotAccount = new HashMap<>();
        double num = 0, num2 = 0;
        for (int k = 0; k < details.size(); k++) {
            Map<String, Object> it = details.get(k);
            Map<String, Object> gl = find(itemGl, toInt(it.get("ItemId")));
            if (gl == null) throw new IllegalArgumentException("Item Purchase GL Account Not found");
            int jobLotId = toInt(it.get("JobLotId"));
            int jobAc = jobLotId > 0 ? jobLotAccount.computeIfAbsent(jobLotId, repo::jobLotAccountId) : 0;
            int account = jobAc > 0 ? jobAc : toInt(ci(gl, "PurchaseGLAC"));
            double qty = toDouble(it.get("ItemQty")), rate = toDouble(it.get("ItemRate")), weight = toDouble(it.get("NetBillWeight"));
            double amount = toDouble(it.get("ItemAmount")), frt = toDouble(it.get("FreightAmount"));
            int gp = toInt(it.get("GpNo"));
            String vehicle = (String) it.get("VehicleNo");
            String remarks = (String) it.get("RemarksDetail");
            StringBuilder sb = new StringBuilder();
            sb.append(str(ci(gl, "ItemName"))).append(' ').append(jobLotText.get(k)).append(' ').append(uomText.get(k))
              .append(' ').append(clr(weight)).append(" @").append(clr(rate)).append("/-");
            if (gp > 0) sb.append(" GP# ").append(gp);
            if (vehicle != null && !vehicle.trim().isEmpty()) sb.append(" V# ").append(vehicle);
            if (remarks != null && !remarks.trim().isEmpty()) sb.append(' ').append(remarks);
            if (frt > 0) sb.append(" Freight Amount: ").append(clr(Math.rint(frt)));
            String comments = sb.toString();

            ContraVoucherDto.Detail dr = new ContraVoucherDto.Detail();
            dr.AccountId = account;
            dr.AgainstAccountId = refAc;
            dr.Comments = comments;
            dr.DebitAmount = amount;
            dr.CreditAmount = 0d;
            dr.ItemId = toInt(it.get("ItemId"));
            dr.JobLotId = jobLotId;
            dr.QtyIn = qty;
            dr.ItemRate = rate;
            dr.WeightIn = weight;
            dr.RateCut = 0d;
            dr.RateCutAmount = 0d;
            dr.ItemAmount = amount;
            dr.Freight = frt;
            dr.Commission = 0d;
            dr.OrderNo = 0;
            dr.GpNo = gp;
            dr.VehicleNo = vehicle;
            dr.SupplierCustomerId = supplierId;
            dr.BranchesId = toInt(it.get("BranchId"));
            v.lines.add(dr);

            ContraVoucherDto.Detail cr = new ContraVoucherDto.Detail();
            cr.AccountId = refAc;
            cr.AgainstAccountId = account;
            cr.Comments = comments;
            cr.CreditAmount = amount;
            cr.DebitAmount = 0d;
            cr.ItemId = toInt(it.get("ItemId"));
            cr.JobLotId = jobLotId;
            cr.QtyIn = qty;
            cr.ItemRate = rate;
            cr.RateCut = 0d;
            cr.RateCutAmount = 0d;
            cr.WeightIn = weight;
            cr.ItemAmount = amount;
            cr.Freight = frt;
            cr.Commission = 0d;
            cr.OrderNo = 0;
            cr.GpNo = gp;
            cr.VehicleNo = vehicle;
            cr.SupplierCustomerId = supplierId;
            cr.SubsidiaryAccountId = supplierId;
            cr.SubsidiaryTypeId = 1;
            cr.BranchesId = toInt(it.get("BranchId"));
            v.lines.add(cr);
            num += frt;
            num2 += weight;
        }
        if (num > 0) {                                                                // :1430 charge to product
            for (Map<String, Object> it : details) {
                Map<String, Object> gl = find(itemGl, toInt(it.get("ItemId")));
                if (gl == null) throw new IllegalArgumentException("Item Purchase GL Account Not found");
                int jobLotId = toInt(it.get("JobLotId"));
                int jobAc = jobLotId > 0 ? jobLotAccount.computeIfAbsent(jobLotId, repo::jobLotAccountId) : 0;
                double frt = toDouble(it.get("FreightAmount"));
                ContraVoucherDto.Detail c = new ContraVoucherDto.Detail();
                c.AccountId = jobAc > 0 ? jobAc : toInt(ci(gl, "PurchaseGLAC"));
                c.AgainstAccountId = offsetAccountId;
                c.DebitAmount = frt;
                c.Comments = "ChargeToProduct/TotalWeightInDetail * Qty = Proportionated Charges : " + clr(num) + " / " + clr(num2)
                        + " * " + clr(toDouble(it.get("ItemQty"))) + " = " + clr(Math.rint(frt));
                c.CreditAmount = 0d;
                c.ItemId = toInt(it.get("ItemId"));
                c.JobLotId = jobLotId;
                c.BranchesId = toInt(it.get("BranchId"));
                v.lines.add(c);
            }
        }

        /* FreightFinancial (BLL 0614:292). PurchaseGLAC is virtual and never set by the form, so the
           distinct count is 1 and Max is 0 — the against account is the transporter itself. */
        for (Map<String, Object> f : freight) {
            int trans = toInt(f.get("TansporterId"));
            double amt = toDouble(f.get("FreightAmount"));
            int sc = toInt(f.get("SupplierCustomerId"));
            String rem = (String) f.get("Remarks");
            if (trans > 0 && toDouble(f.get("Debit")) > 0) {
                ContraVoucherDto.Detail l = new ContraVoucherDto.Detail();
                l.AccountId = trans;
                l.AgainstAccountId = trans;
                l.Comments = (rem == null || rem.isEmpty()) ? "PartyName :  " + str(companyName) + "  Freight : " + clr(amt) : rem;
                l.DebitAmount = toDouble(f.get("Debit"));
                l.CreditAmount = 0d;
                if (sc > 0) { l.SupplierCustomerId = sc; l.SubsidiaryAccountId = sc; l.SubsidiaryTypeId = 1; }
                l.BranchesId = toInt(head.get("BranchesId"));
                v.lines.add(l);
            }
            if (trans > 0 && amt > 0) {
                ContraVoucherDto.Detail l = new ContraVoucherDto.Detail();
                l.AccountId = trans;
                l.AgainstAccountId = trans;
                l.Comments = (rem == null || rem.isEmpty()) ? "SupplierName :  " + str(companyName) + "  Freight : " + clr(amt) : rem;
                l.DebitAmount = 0d;
                l.CreditAmount = amt;
                if (sc > 0) { l.SupplierCustomerId = sc; l.SubsidiaryAccountId = sc; l.SubsidiaryTypeId = 1; }
                l.BranchesId = toInt(head.get("BranchesId"));
                v.lines.add(l);
            }
        }

        /* SupplierAddLessFinancial (BLL 0614:171). */
        for (Map<String, Object> j : journal) {
            int coa = toInt(j.get("ChartofAccountId"));
            double jd = toDouble(j.get("JvDebit")), jc = toDouble(j.get("JvCredit"));
            if (coa <= 0 || !(jc > 0 || jd > 0)) continue;
            int sc = toInt(j.get("SupplierCustomerId"));
            String rem = (String) j.get("JvRemarks");
            ContraVoucherDto.Detail a = new ContraVoucherDto.Detail();
            a.AccountId = coa;
            a.AgainstAccountId = refAc;
            a.Comments = rem;
            a.DebitAmount = jd;
            a.CreditAmount = jc;
            if (sc > 0) { a.SupplierCustomerId = sc; a.SubsidiaryAccountId = sc; a.SubsidiaryTypeId = 1; }
            a.BranchesId = toInt(head.get("BranchesId"));
            v.lines.add(a);
            ContraVoucherDto.Detail b = new ContraVoucherDto.Detail();
            b.AccountId = refAc;
            b.AgainstAccountId = coa;
            b.Comments = rem;
            b.DebitAmount = jc;
            b.CreditAmount = jd;
            b.SupplierCustomerId = supplierId;
            b.SubsidiaryAccountId = supplierId;
            b.SubsidiaryTypeId = 1;
            b.BranchesId = toInt(head.get("BranchesId"));
            v.lines.add(b);
        }
        /* BrokeryBeHalfOnPartyFinancial, EmptyBagsFinancial, SalesTaxFinancial, ItemAndSupplierByWeightFinancial
           and PurchaseReturnFinancial produce nothing for DocumentTypeId 61 (their DocumentTypeId sets exclude it). */

        String rh = (String) head.get("RemarksHeader");
        if (rh != null && !rh.isEmpty()) vh.Remarks = rh;                             // BLL 0549 tail
        return v;
    }

    // ================================================================= form calculations

    /** BillAmount():1621 — Math.Round (banker's) of items + tax + journal debit − credit + supplier-own freight. */
    static double billAmount(List<PurchaseInvoiceDirectStoreDto.Row> rows, List<PurchaseInvoiceDirectStoreDto.FreightRow> freight,
                             List<PurchaseInvoiceDirectStoreDto.JournalRow> journal, String supplierGlText) {
        double items = 0, tax = 0, jd = 0, jc = 0, tc = 0;
        for (PurchaseInvoiceDirectStoreDto.Row r : rows) { items += d(r.ItemAmount); tax += d(r.TaxAmount); }
        for (PurchaseInvoiceDirectStoreDto.JournalRow r : journal) {
            if (cInt(r.AccountId) > 0) { jd += d(r.Debit); jc += d(r.Credit); }
        }
        for (PurchaseInvoiceDirectStoreDto.FreightRow r : freight) {
            String t = r.Transporter == null ? "" : r.Transporter.trim();
            if (cInt(t) > 0 && supplierGlText.equals(t)) tc += d(r.Freight);
        }
        double bill = items + tax;
        bill = bill + jd - jc;
        bill += tc;
        return Math.rint(bill);
    }

    /** FreightProportion():2326 then BillProportion():2360. */
    static void freightProportion(List<PurchaseInvoiceDirectStoreDto.Row> rows, List<PurchaseInvoiceDirectStoreDto.FreightRow> freight) {
        double net = 0, credit = 0;
        for (PurchaseInvoiceDirectStoreDto.Row r : rows) net += d(r.ItemQty);
        for (PurchaseInvoiceDirectStoreDto.FreightRow f : freight) credit += d(f.Freight);
        for (PurchaseInvoiceDirectStoreDto.Row r : rows) {
            r.Freights = credit > 0 ? Math.rint(credit) / net * d(r.ItemQty) : 0d;
            r.BillAmount = d(r.ItemAmount) + d(r.Freights) + d(r.TaxAmount);
        }
    }

    // ================================================================= models

    /** Architecture.Model.Inventory.InvPurchaseInvoice (1036) — non-virtual properties, declaration order, CLR defaults. */
    private static Map<String, Object> headerModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("IsApproved", false);
        p.put("CustomAccounts", false);
        p.put("DocDate", null);
        p.put("EntryDate", null);
        p.put("ModifyDate", null);
        p.put("DueDate", null);
        p.put("PostDate", null);
        p.put("DeliveryStartDate", null);
        p.put("SupplierInvoiceDate", null);
        p.put("BillAmount", 0d);
        p.put("CashReceived", 0d);
        p.put("CommAmount", 0d);
        p.put("CommRate", 0d);
        p.put("FreightAmount", 0d);
        p.put("TransportAccountId", 0);
        p.put("BranchesId", 0);
        p.put("BranchSrNo", 0);
        p.put("TransporterCreditPartyId", 0);
        p.put("TransporterDebitGLId", 0);
        p.put("TransporterDebitPartyId", 0);
        p.put("FinancialYearId", 0);
        p.put("CommissionAgentId", 0);
        p.put("CommissionCreditAccountId", 0);
        p.put("CompanyId", 0);
        p.put("DocNo", 0);
        p.put("SalesTaxNo", 0);
        p.put("DocumentTypeId", 0);
        p.put("DocumentTypeSrNo", 0);
        p.put("EntryUser", 0);
        p.put("Id", 0);
        p.put("ModifyUser", 0);
        p.put("OrganizationId", 0);
        p.put("PostUser", 0);
        p.put("ProjectsId", 0);
        p.put("TaxAccountId", 0);
        p.put("ReferencePartyId", 0);
        p.put("StockPartyId", 0);
        p.put("SupplierCustomerId", 0);
        p.put("SupplierInvoiceNo", 0);
        p.put("SupplierReferenceNo", null);
        p.put("CurrencyId", 0);
        p.put("ReferencePartyName", null);
        p.put("DueDays", 0);
        p.put("ActionId", 0);
        p.put("CommissionRemarks", null);
        p.put("CommissionType", null);
        p.put("IsAttachments", null);
        p.put("IsTaxable", false);
        p.put("ManualBillNo", null);
        p.put("OtherRemarks", null);
        p.put("RemarksHeader", null);
        p.put("ScreenName", null);
        p.put("UomScheduleIdCmRate", null);
        p.put("DeliveryTerm", null);
        p.put("PaymentTermsId", 0);
        p.put("DiscountAmount", 0d);
        p.put("DiscountAccountId", 0);
        p.put("DeliveryDays", 0);
        p.put("BrokerAgentId", 0);
        p.put("InvoiceTypeId", 0);
        p.put("GhallaMandiId", 0);
        p.put("BillCalculateTypeId", 0);
        p.put("BrokeryType", null);
        p.put("BrokeryRate", 0d);
        p.put("BrokeryUom", 0d);
        p.put("BrokeryAmount", 0d);
        p.put("WagesAmount", 0d);
        p.put("InvoiceQty", BigDecimal.ZERO);
        p.put("InvoiceWeight", BigDecimal.ZERO);
        p.put("ExchangeRate", BigDecimal.ZERO);
        p.put("FcyAmount", BigDecimal.ZERO);
        p.put("AttachmentsValues", null);
        p.put("CustomAttachmentsValues", null);
        p.put("FreightRemark", null);
        p.put("IsUploaded", false);
        return p;
    }

    /** Architecture.Model.Inventory.InvPurchaseInvoiceDetail (1037) — non-virtual properties, declaration order. */
    private static Map<String, Object> detailModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("GpDate", null);
        for (String k : new String[] { "AdLsWeight", "AvgCgsRate", "BillAmount", "CommissionAmount", "EBTotalWt",
                "EbPurAgainstWeight", "EBWeight", "ExpenseAmount", "FreightAmount", "GrossWeight", "WagesAmount",
                "FreightDeduction", "EbPurAgainstWeightAmount", "ScaleKart", "ItemAmount", "ItemCgsRate", "ItemQty",
                "ItemRate", "JournalAmount", "NetBillWeight", "NetStockWeight", "RateCut", "RateCutAmount",
                "ExcessWeight", "RateCutForExcessWeight", "RateCutAmountForExcessWeight", "TaxAmount", "TaxPercent",
                "WeightCut", "WeightCutTotal", "Brokery" }) p.put(k, 0d);
        p.put("ExchangeRate", BigDecimal.ZERO);
        p.put("FcyAmount", BigDecimal.ZERO);
        p.put("ItemAmountWithoutDiscount", BigDecimal.ZERO);
        for (String k : new String[] { "GpNo", "BranchId", "Id", "ActionTypeId", "InvGrnDetailId", "InvGrnId",
                "InvPurchaseInvoiceId", "ItemId", "ItemUOMId", "JobLotId", "PackingTypeId", "PurchaseOrderId",
                "PurchaseOrderDetailId", "TaxNameId", "UomScheduleIdRate", "WarehouseId", "LineId", "CurrencyId",
                "AttributeVariantId", "ItemConditionId", "RackId" }) p.put(k, 0);
        for (String k : new String[] { "CropYear", "IsTaxable", "LabAnalisysNo", "RemarksDetail", "TaxDescriptions",
                "VehicleNo", "PackingDate", "ExpiryDate" }) p.put(k, null);
        p.put("CropYearId", 0);
        p.put("DiscountTypeId", 0);
        p.put("DiscountPercent", 0d);
        p.put("DiscountAmount", 0d);
        for (String k : new String[] { "RefDocumentTypeId", "RefDocId", "RefDocSubId", "RefDocInvoiceId", "CityId",
                "ProductionStageId", "CastingTypeId", "GdnDocumentTypeId", "GdnId", "GdnDetailId", "LocationTypeId" }) p.put(k, 0);
        return p;
    }

    /** Public fields in declaration order; date strings become Timestamps (the voucher DTO carries them as text). */
    private static Map<String, Object> model(Object o) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Field f : o.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            try {
                Object v = f.get(o);
                if (v instanceof String && ("VoucherDate".equals(f.getName()) || "ChequeDate".equals(f.getName())
                        || "DueDate".equals(f.getName()) || "EntryDate".equals(f.getName())
                        || "ModifyDate".equals(f.getName()) || "PostDate".equals(f.getName())
                        || "DCheqDate".equals(f.getName()) || "GpDate".equals(f.getName()))) {
                    String s = (String) v;
                    v = Timestamp.valueOf(s.length() == 10 ? s + " 00:00:00" : s);
                }
                m.put(f.getName(), v);
            } catch (IllegalAccessException ignored) { }
        }
        return m;
    }

    // ================================================================= helpers

    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) return null;
        if (toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }

    private static Map<String, Object> find(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (toInt(ci(r, "Id")) == id) return r;
        return null;
    }

    /** FormHelper.ValidateField:503. */
    private static void validate(Object v, String field, int rowIndex) {
        boolean bad = v == null
                || (v instanceof Integer && (Integer) v == 0)
                || (v instanceof Double && (Double) v <= 0d)
                || (v instanceof String && ((String) v).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    /** Conversion.ToInt of a text cell — Convert.ToInt32(string): anything not an integer is 0. */
    static int cInt(String s) {
        if (s == null) return 0;
        try { return Integer.parseInt(s.trim()); } catch (NumberFormatException e) { return 0; }
    }

    /** Convert.ToInt32(double) — round half to even. */
    static int netInt(double v) { return (int) Math.rint(v); }

    private static double round2(double v) {
        return BigDecimal.valueOf(v).setScale(2, java.math.RoundingMode.HALF_EVEN).doubleValue();
    }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = v == null ? "" : String.valueOf(v).trim();
        return "true".equalsIgnoreCase(s) || "1".equals(s);
    }

    private static String nullIfEmpty(String s) { return s == null || s.isEmpty() ? null : s; }
    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static int i(Integer v) { return v == null ? 0 : v; }
    private static double d(Double v) { return v == null ? 0d : v; }
    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }

    private static Map<String, Object> ok(String message, int id) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        m.put("id", id);
        return m;
    }
}
