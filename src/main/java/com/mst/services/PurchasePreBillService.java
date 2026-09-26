package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.PurchasePreBillDto;
import com.mst.repositories.PurchasePreBillRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
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

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.clr;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Store Management (ModuleId 24) — screen 961 "Store Purchase Pre Bill".
 *
 * <pre>
 * Screen id        961
 * ScreenName       frmPurchasePreBill   (base.Name; rights key — frmPurchasePreBill.cs:339)
 * DocumentTypeId   147                  (frmPurchasePreBill.cs:311)
 * Loader           frmPendingPurchaseDemand.cs with IsForPreBill = true — Purchase Demand DocumentTypeId 141 (:94)
 * Route            /store/purchase-pre-bill
 * API              /api/store/purchase-pre-bill
 * Desktop          frmPurchasePreBill.cs (4,256 lines) + frmPendingPurchaseDemand.cs
 *                  + Architecture.WinApp.StoreManagement.FromWise_Helper_Methods.frmPurchasePreBill_Helper
 *                  + CrystalReportPrint_Helper.StorePurchasePreBillSlip147 (print)
 * BLL / DAL        BLL 0314 + DAL 0254 StoreManagement.PurchasePreBillHeader (codes, read, history, save,
 *                  delete, combos, demand loader, slip + expense sub-report)
 *                  BLL 0557 InvPurchaseDemandHeader.GetDataForDropDownFromPurchaseDemand (loader Item combo)
 *                  BLL 0074 ReferenceParties, BLL 0512 Inventory.DeliveryTerm, BLL 0573 InventoryItemsOther,
 *                  BLL 0379 GlobalServicesMethods (suppliers, cities), BLL 0010 ItemCondition (V_ItemCondition)
 * Models           0440 PurchasePreBillHeader, 0437 PurchasePreBillDetail, 0441 PurchasePreBillExpenseDetail
 * </pre>
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED, NOT CORRECTED (each is the user's call to change)
 * ---------------------------------------------------------------------------------------------
 *  D1  The Doc No shown is USP_PurchasePreBillHeader_GetAllMethod 'GenerateCode' (ActionId &lt;&gt; 3), but
 *      the Insert branch of USP_PurchasePreBillHeader_InsertAndUpdate recomputes it as MAX+1 over ALL
 *      rows (deleted ones included). After a deletion of the last bill the saved number can differ
 *      from the one shown, and the "Data Save Successfully....  n" message shows the SHOWN number
 *      (Or.DocNo, :1122), not the saved one.
 *  D2  Header ProjectsId is saved as the user's BranchesId (:1034).
 *  D3  Detail ExpenseAmount is never copied into the saved row (FillDetailListCommonForInsertAndDelete,
 *      :1191-1221, does not set it), so the procedure receives the model default 0; ItemNetAmount does
 *      include the share. On reopen the grid shows ExpenseAmount 0 and ItemNetAmount = ItemAmount
 *      (ReadById runs CalculateFooterAmounts, not ExpProportion) until an edit redistributes it.
 *  D4  Expense Qty is saved through Conversion.ToInt (:1101) — rounded to a whole number (banker's).
 *  D5  The save-time balance check (:1080) is Qty &gt; DemandQty - PurchasedQty, where PurchasedQty is
 *      Pre-Bill + direct-GRN usage only; the loader's BalanceQty (which the grid's own cap uses,
 *      :707) also subtracts Purchase-Order usage. The two limits can differ.
 *  D6  The City combo is set from the "City Area" configuration only by New / after save / after
 *      delete / Refresh (GetConfigurationsFromGlobalAndBindValuesInColumns, :1261 / :1282) — never on
 *      form load. New does not clear Delivery Term, City (when the config is 0), Vendor Bill Date or
 *      Doc Date.
 *  D7  On first load the Reference Party combo selects its first real row and Delivery Term its
 *      SECOND real row (BindAndRetainSelection ActivateRow index 1 / 2 over a list with a
 *      "-- Select --" row at 0, :555 / :567); New clears Reference Party to blank.
 *  D8  Typing in the Item Discount footer distributes it over ThisQty (:1380) and CalculateFooterAmounts
 *      then writes Math.Round(sum of row discounts) back into the same box (:1314) — the typed value
 *      is rounded to a whole number (banker's).
 *  D9  Update does not check "approved" (only the DeleteById procedure branch does).
 *  D10 The history "Reset" puts From Date back to Now-3, not the DefaultDaysToLessFromHistoryFromDate
 *      value Form_Load uses (:1539 vs :421).
 *  D11 An expense row with no Other Item still counts in the Expense Amount footer and the bill
 *      amount (GetColumnSum over the whole grid, :1318), but is not saved (:1089).
 *  D12 New / after save empties the footer boxes one by one BEFORE clearing the grid (:1250-1256); the
 *      two boxes with a TextChanged handler recompute every footer from the rows still in the grid,
 *      so after New the footers can keep the previous document's totals (Net Bill Amount excepted)
 *      until the next edit. The page reproduces the same sequence.
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web only)
 * ---------------------------------------------------------------------------------------------
 *  W1  Doc No is disabled on the desktop; the server takes it from the generator on a new save and
 *      from the stored header on update, never from the request.
 *  W2  Open / update / delete / print only when the bill belongs to the user's organization and
 *      company AND is DocumentTypeId 147 (ReadById and DeleteById filter by Id alone).
 *  W3  Posted detail rows are re-validated (USP_PurchasePreBillDetail_Insert updates / soft-deletes by
 *      PurchasePreBillDetailId alone):
 *        - a row with Id 0 must be a (PurchaseDemandHeaderId, PurchaseDemandDetailId) pair the
 *          company-scoped pending loader (USP_InvPurchaseDemand_PendingDataLoader, this org / company /
 *          branch / financial year, DocumentTypeId 141, no other filter) offers now;
 *        - a row with Id &gt; 0 (update only) must be a live detail of THIS bill, with the same demand
 *          pair as stored;
 *        - a removed row (lstRemoveRecord) must be a live detail of THIS bill and not also posted as a row;
 *        - the same demand detail, or the same detail Id, may not appear twice;
 *        - ItemId, UomId, DemandQty and PurchasedQty (the :1080 check) come from the loader row /
 *          stored row, not from the request; a removed row is sent with its stored values.
 *  W4  Bill To Party / Vendor Supplier must be in the screen's supplier list, Reference Party / Delivery
 *      Term / City / Other Item in their lists, a row's Item Condition in the grid combo's list (Id != 4)
 *      or equal to the condition its pending-loader / stored row carries (a loaded condition 4 is kept,
 *      as the desktop keeps it), and an expense row Id must be 0 or one
 *      of this bill's stored expense rows.
 *  W5  The header Bill Amount and each row's ItemNetAmount are recomputed exactly as
 *      CalculateFooterAmounts / BillProportion do at the start of Insert (:1021), from the posted
 *      rows — the disabled footer box is not trusted.
 *  W6  The "Are you sure to Save/Update" prompt is a two-step request: the checks the desktop runs
 *      before its prompt (and the W3/W4 checks) are answered first, the row checks after the Yes.
 *  W7  A header procedure that returns no id stops the save.
 *  W8  Opening a bill for editing requires the Update right server-side (the desktop checks it in the
 *      history grid handlers); a history row's detail grid (SelectionChanged) needs no right.
 *
 * Not ported: attachments (Attachment button, attachment count link — stored AttachmentsValues kept
 * as they are), the Reference Party definition form (BtnRefPartyForm), saved grid layouts
 * (CtrlGrdBar), the shortcut-key popup and keyboard shortcuts, Crystal layouts (the print endpoints
 * return the report procedures' rows: 147_PurchasePreBillSlip + PurchasePreBillHeader_ExpenseSubReport).
 */
@Service
public class PurchasePreBillService {

    private static final Logger LOG = LoggerFactory.getLogger(PurchasePreBillService.class);

    public static final String SCREEN_NAME = "frmPurchasePreBill";
    public static final int DOCUMENT_TYPE_ID = 147;
    public static final int DEMAND_DOCUMENT_TYPE_ID = 141;     // frmPendingPurchaseDemand.cs:94
    private static final int REFERENCE_PARTY_TYPE_ID = 4;      // :541

    private final PurchasePreBillRepository repo;
    private final StoreIssuanceRepository common;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;
    private final JdbcTemplate jdbc;

    public PurchasePreBillService(PurchasePreBillRepository repo, StoreIssuanceRepository common,
                                  StoreScreenRights rights, CurrentUserContext ctx, JdbcTemplate jdbc) {
        this.repo = repo;
        this.common = common;
        this.rights = rights;
        this.ctx = ctx;
        this.jdbc = jdbc;
    }

    // ======================================================================== form load

    /** InitializeComponentMethod (:363) — everything the form binds on open. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));
        out.put("documentTypeId", DOCUMENT_TYPE_ID);
        out.put("docNo", repo.nextDocNo(u, fy, DOCUMENT_TYPE_ID));                                // DocumentNoDbCall
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(common.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("financialYearStart", financialYearStart(u, fy));                                  // date type 5
        out.put("amountDecimals", amountDecimals(u));                                               // stringFormatsingle
        putGlobals(u, out);
        putHistoryCombos(u, out);
        return out;
    }

    /** btnRefresh_Click (:1270) — the globals and lists rebound (selections kept by the page). */
    public Map<String, Object> formRefresh() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(common.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        putGlobals(u, out);
        return out;
    }

    /** btnRefreshHistory_Click (:1556). */
    public Map<String, Object> historyRefresh() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        putHistoryCombos(u, out);
        return out;
    }

    /** FromReset (:1259-1261) — the generator and the City Area configuration, asked again. */
    public Map<String, Object> numbers() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", repo.nextDocNo(u, ctx.currentFinancialYearId(), DOCUMENT_TYPE_ID));
        out.put("cityArea", toInt(common.config(u, "City Area")));
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(common.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        return out;
    }

    private void putGlobals(UserAccount u, Map<String, Object> out) {
        out.put("suppliers", suppliers(u));                // SupplierDtFillFromGlobal + BindSupplierName
        out.put("referenceParties", referenceParties(u));  // ReferancePartyBind
        out.put("deliveryTerms", deliveryTerms());         // DeliveryTerm(dtTerm)
        out.put("cities", cities(u));                      // CityDtFillFromGlobalAndBind
        out.put("itemConditions", common.itemConditions()); // GridDtFill — Id != 4, ConditionStatus as Description
        out.put("otherItems", otherItems(u));              // OtherItemdtDbCall
        out.put("cityArea", toInt(common.config(u, "City Area")));
    }

    /**
     * SupplierDtFillFromGlobal (:487) — clsGlobalVariables.globalSupplierCustomer, which
     * DatatableHelper.GlobalSupplierCustomerListsFillDbCall(org, comp, partyTypeId 0) builds from
     * USP_GetVendorsAndCustomersWithCityName: groups 7/9/10 always out; 8 and 13 also out when
     * ShowBothVendorAndCustomerOnSalesPurchase == 1; otherwise everything else in.
     */
    private List<Map<String, Object>> suppliers(UserAccount u) {
        int showBoth = toInt(common.config(u, "ShowBothVendorAndCustomerOnSalesPurchase"));
        Set<Integer> excluded = new HashSet<>(java.util.Arrays.asList(7, 8, 9, 10, 13));
        Set<Integer> mustExclude = new HashSet<>(java.util.Arrays.asList(7, 9, 10));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.vendorsAndCustomers(u)) {
            int group = toInt(ci(r, "CustomerGroupId"));
            if (mustExclude.contains(group)) continue;
            if (showBoth == 1 && excluded.contains(group)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("CompanyName", str(ci(r, "CompanyName")));
            o.put("PartyCode", str(ci(r, "PartyCode")));
            o.put("CityName", str(ci(r, "CityName")));
            o.put("MobileNo", str(ci(r, "MobilePersonal")));
            out.add(o);
        }
        return out;
    }

    /** ReferancePartyDbCall + ReferancePartyBind (AllColumns false: Id / ReferencePartyName). */
    private List<Map<String, Object>> referenceParties(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.referenceParties(u, REFERENCE_PARTY_TYPE_ID)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("ReferencePartyName", str(ci(r, "ReferencePartyName")));
            out.add(o);
        }
        return out;
    }

    /** DeliveryTerm (:563) — AllColumns: Description visible, column 2 (ValueDescription) hidden. */
    private List<Map<String, Object>> deliveryTerms() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.deliveryTerms()) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Description", str(ci(r, "Description")));
            out.add(o);
        }
        return out;
    }

    /** CityDtFillFromGlobalAndBind (:579) — PopulateDataTableAndReturn(Id, CityName) as Id / Description. */
    private List<Map<String, Object>> cities(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.cities(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Description", str(ci(r, "CityName")));
            out.add(o);
        }
        return out;
    }

    /** ExpenseGridCombBind (:829) — Id / OtherItemName. */
    private List<Map<String, Object>> otherItems(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.otherItems(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("OtherItemName", str(ci(r, "OtherItemName")));
            out.add(o);
        }
        return out;
    }

    /** HistoryComboBind (:1453) — "BillToPartyName" and "Item" rows; nothing bound when the proc returns none. */
    private void putHistoryCombos(UserAccount u, Map<String, Object> out) {
        List<Map<String, Object>> parties = new ArrayList<>(), items = new ArrayList<>();
        for (Map<String, Object> r : repo.historyCombos(u)) {
            String a = str(ci(r, "Activity"));
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "ReferenceName")));
            if ("BillToPartyName".equals(a)) parties.add(o);
            else if ("Item".equals(a)) items.add(o);
        }
        out.put("historyBillToParties", parties);
        out.put("historyItems", items);
    }

    // ============================================================================= loader

    /** frmPendingPurchaseDemand ComboDbCall + CombosFill (:140-180) — the Item combo; FY start for New. */
    public Map<String, Object> loaderLookups() {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : repo.demandDropDowns(u, String.valueOf(DEMAND_DOCUMENT_TYPE_ID), "Item")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "ReferenceName")));
            items.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", items);
        out.put("financialYearStart", financialYearStart(u, ctx.currentFinancialYearId()));
        return out;
    }

    /**
     * PendingDataDbCall (:195). FromDate / ToDate are the pickers' values (the procedure's parameters
     * are DATE); doc numbers are Conversion.ToInt of the text boxes; the item is the combo's value.
     */
    public List<Map<String, Object>> pendingDemands(String from, String to, String fromDocNo, String toDocNo, int itemId) {
        UserAccount u = ctx.requireAccountingUser();
        return plainRows(repo.pendingDemands(u, ctx.currentFinancialYearId(), DEMAND_DOCUMENT_TYPE_ID,
                StoreIssuanceService.dateOnly(from), StoreIssuanceService.dateOnly(to),
                cvIntText(fromDocNo), cvIntText(toDocNo), itemId));
    }

    // ============================================================================ history

    /** HistoryGridFill (:1574). dateType: doc / entry / modify / approved (the four radio buttons). */
    public List<Map<String, Object>> history(String dateType, boolean fromChecked, String from, boolean toChecked, String to,
                                             String fromDocNo, String toDocNo, int billToPartyId, int itemId) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN_NAME, "viewAll");
        Timestamp f = fromChecked && !isBlank(from) ? StoreIssuanceService.pickerDate(from) : null;
        Timestamp t = toChecked && !isBlank(to) ? StoreIssuanceService.pickerDate(to) : null;
        Timestamp df = null, dt = null, ef = null, et = null, mf = null, mt = null, af = null, at = null;
        switch (dateType == null ? "doc" : dateType) {
            case "entry":    ef = f; et = t; break;
            case "modify":   mf = f; mt = t; break;
            case "approved": af = f; at = t; break;
            default:         df = f; dt = t; break;
        }
        List<Map<String, Object>> rows = repo.formHistory(u, ctx.currentFinancialYearId(), DOCUMENT_TYPE_ID, viewAll,
                df, dt, ef, et, mf, mt, af, at, cvIntText(fromDocNo), cvIntText(toDocNo), billToPartyId, itemId, 0);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {                                                   // :1679-1705
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "PurchasePreBillHeaderId")));
            o.put("DocumentTypeId", toInt(ci(r, "DocumentTypeId")));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("DocDate", plain(ci(r, "DocDate")));
            o.put("BillToPartyId", toInt(ci(r, "BillToPartyId")));
            o.put("BillToPartyName", ci(r, "BillToPartyName"));
            o.put("ReferenceNo", ci(r, "ReferenceNo"));
            o.put("DiscountAmount", ci(r, "DiscountAmount"));
            o.put("BillAmount", ci(r, "BillAmount"));
            o.put("DeliveryTermId", toInt(ci(r, "DeliveryTermId")));
            o.put("DeliveryTerm", ci(r, "DeliveryTerm"));
            o.put("CityId", toInt(ci(r, "CityId")));
            o.put("CityName", ci(r, "CityName"));
            o.put("RemarksHeader", ci(r, "RemarksHeader"));
            o.put("EntryDate", plain(ci(r, "EntryDate")));
            o.put("EntryUserName", ci(r, "EntryUserName"));
            o.put("ModifyDate", plain(ci(r, "ModifyDate")));
            o.put("ModifyUserName", ci(r, "ModifyUserName"));
            o.put("IsApproved", toBool(ci(r, "IsApproved")));
            o.put("ApprovedDate", plain(ci(r, "ApprovedDate")));
            o.put("ApprovalUserName", ci(r, "ApprovalUserName"));
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    // =============================================================================== read

    /**
     * ReadById (:1136) — header, the first detail row's vendor fields, the grid and the expense rows.
     * {@code edit}: opened for editing from History (Edit / double-click need the Update right, W8);
     * false: the history detail grid (GetDetailGrdByHeadId, :1838).
     */
    public Map<String, Object> load(int id, boolean edit) {
        UserAccount u = ctx.requireAccountingUser();
        if (edit && !rights.has(SCREEN_NAME, "update")) throw new AccessDeniedException("you don't have update rights...");
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) return null;
        List<Map<String, Object>> details = repo.details(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", id);
        out.put("DocDate", plain(ci(h, "DocDate")));
        out.put("DocNo", toInt(ci(h, "DocNo")));
        out.put("BillToPartyId", toInt(ci(h, "BillToPartyId")));
        out.put("DeliveryTermId", toInt(ci(h, "DeliveryTermId")));
        out.put("CityId", toInt(ci(h, "CityId")));
        out.put("DiscountAmount", toDouble(ci(h, "DiscountAmount")));
        out.put("RemarksHeader", str(ci(h, "RemarksHeader")));
        if (!details.isEmpty()) {                                                               // :1157-1166
            Map<String, Object> dd = details.get(0);
            out.put("VendorSupplierId", toInt(ci(dd, "VendorSupplierId")));
            out.put("ReferencePartyId", toInt(ci(dd, "ReferencePartyId")));
            out.put("VendorBillDate", plain(ci(dd, "PartyBillDate")));
            out.put("VendorBillNo", str(ci(dd, "PartyBillNo")));
            out.put("VehicleNo", str(ci(dd, "VehicleNo")));
            out.put("BiltyNo", str(ci(dd, "BiltyNo")));
        }
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : details) rows.add(detailRow(d));                           // helper FillDetailFromListCommonForReadById
        out.put("rows", rows);
        List<Map<String, Object>> exps = new ArrayList<>();
        for (Map<String, Object> e : repo.expenses(id)) {                                        // :1176-1179
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(e, "PurchasePreBillExpenseDetailId")));
            o.put("ItemId", toInt(ci(e, "OtherItemId")));
            o.put("Qty", toDouble(ci(e, "Qty")));
            o.put("Rate", toDouble(ci(e, "Rate")));
            o.put("Amount", toDouble(ci(e, "Amount")));
            o.put("Remarks", str(ci(e, "Remarks")));
            exps.add(o);
        }
        out.put("expenses", exps);
        return out;
    }

    /** frmPurchasePreBill_Helper.FillDetailFromListCommonForReadById (ExtraColumns adds ItemCondition). */
    private static Map<String, Object> detailRow(Map<String, Object> d) {
        Map<String, Object> o = new LinkedHashMap<>();
        double demand = toDouble(ci(d, "PurchaseDemandQty"));
        double purchased = toDouble(ci(d, "TotalPurchasedForDemand"));
        o.put("Id", toInt(ci(d, "PurchasePreBillDetailId")));
        o.put("PurchaseDemandHeaderId", toInt(ci(d, "PurchaseDemandHeaderId")));
        o.put("PurchaseDemandDetailId", toInt(ci(d, "PurchaseDemandDetailId")));
        o.put("PurchaseDemandNo", toInt(ci(d, "PurchaseDemandDocNo")));
        o.put("ItemId", toInt(ci(d, "ItemId")));
        o.put("ItemCode", str(ci(d, "ItemCode")));
        o.put("ItemName", str(ci(d, "ItemName")));
        o.put("UomId", toInt(ci(d, "UomId")));
        o.put("Uom", str(ci(d, "Uom")));
        o.put("UomEquivalent", toDouble(ci(d, "UomEquivalent")));
        o.put("ItemConditionId", toInt(ci(d, "ItemConditionId")));
        o.put("ItemCondition", str(ci(d, "ItemCondition")));
        o.put("DemandQty", demand);
        o.put("PurchasedQty", purchased);
        o.put("BalanceQty", demand - purchased);
        o.put("ThisQty", toDouble(ci(d, "Qty")));
        o.put("Rate", toDouble(ci(d, "Rate")));
        o.put("ItemAmountWithoutDiscount", toDouble(ci(d, "ItemAmountWithoutDiscount")));
        o.put("DiscountAmount", toDouble(ci(d, "DiscountAmount")));
        o.put("ItemAmount", toDouble(ci(d, "ItemAmount")));
        o.put("ExpenseAmount", toDouble(ci(d, "ExpenseAmount")));
        o.put("ItemNetAmount", toDouble(ci(d, "ItemNetAmount")));
        o.put("Remarks", str(ci(d, "RemarksDetail")));
        return o;
    }

    // ============================================================================== print

    /** ShowPrint → CrystalReportPrint_Helper.StorePurchasePreBillSlip147 — the report procedure's rows. */
    public List<Map<String, Object>> slip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "print")) throw new AccessDeniedException("you don't have print rights...");
        if (id == 0) throw new IllegalArgumentException("PrintId not found...");
        if (ownedHeader(u, id) == null) throw new IllegalArgumentException("No Record Found For Display");
        List<Map<String, Object>> rows = repo.slip(u, ctx.currentFinancialYearId(), id);
        if (rows.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
        return plainRows(rows);
    }

    /** The slip's sub-report (PurchasePreBillHeader_ExpenseSubReport.rpt). */
    public List<Map<String, Object>> slipSubReport(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "print")) throw new AccessDeniedException("you don't have print rights...");
        if (ownedHeader(u, id) == null) throw new IllegalArgumentException("No Record Found For Display");
        return plainRows(repo.expenseSubReport(id));
    }

    // ============================================================================= delete

    /** btnDelete_Click (:983) → BLL 0314 DeleteByID (its own transaction). */
    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "delete")) throw new AccessDeniedException("You do not have the Delete right for this screen.");
        if (id <= 0) throw new IllegalArgumentException("No record found to Delete");
        if (ownedHeader(u, id) == null) throw new IllegalArgumentException("No record found to Delete");   // W2
        repo.deleteById(u.getId(), id);
        return ok("Delete Record Successfully", id, 0);
    }

    // =============================================================================== save

    /** btnsave_Click / btnupdate_Click → Insert() (:1007). */
    @Transactional
    public Map<String, Object> save(PurchasePreBillDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = i(dto.Id);
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) throw new AccessDeniedException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN_NAME, "update")) throw new AccessDeniedException("You do not have the Update right for this screen.");
        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = ownedHeader(u, recId);                                                        // W2
            if (existing == null) throw new IllegalArgumentException("Record Not Update because RecId Not Found");
        }
        int fy = ctx.currentFinancialYearId();
        List<PurchasePreBillDto.Row> rows = dto.rows == null ? new ArrayList<>() : dto.rows;
        List<PurchasePreBillDto.Row> removed = dto.removed == null ? new ArrayList<>() : dto.removed;
        List<PurchasePreBillDto.Expense> expenses = dto.expenses == null ? new ArrayList<>() : dto.expenses;

        // ----------------------------------------------------- checks before the prompt (:1017-1029)
        if (rows.isEmpty()) throw new IllegalArgumentException("Detail Record Not Found");
        int docNo = existing != null ? toInt(ci(existing, "DocNo")) : repo.nextDocNo(u, fy, DOCUMENT_TYPE_ID);   // W1
        if (docNo == 0) throw new IllegalArgumentException("Doc No must be a non-zero number");
        List<Map<String, Object>> suppliers = suppliers(u);
        int billToPartyId = i(dto.BillToPartyId);
        if (billToPartyId == 0 || !contains(suppliers, billToPartyId)) throw new IllegalArgumentException("Bill To Party Name field is required");

        /* W4 — every other posted reference must be one the screen's own lists offer. */
        int vendorId = i(dto.VendorSupplierId);
        if (vendorId != 0 && !contains(suppliers, vendorId)) throw new IllegalArgumentException("Vendor / Supplier Not Found");
        int refPartyId = i(dto.ReferencePartyId);
        if (refPartyId != 0 && !contains(referenceParties(u), refPartyId)) throw new IllegalArgumentException("Reference Party Not Found");
        int deliveryTermId = i(dto.DeliveryTermId);
        if (deliveryTermId != 0 && !contains(deliveryTerms(), deliveryTermId)) throw new IllegalArgumentException("Delivery Term Not Found");
        int cityId = i(dto.CityId);
        if (cityId != 0 && !contains(cities(u), cityId)) throw new IllegalArgumentException("City Not Found");
        List<Map<String, Object>> conditions = common.itemConditions();
        List<Map<String, Object>> otherItems = otherItems(u);
        Set<Integer> storedExpenseIds = new HashSet<>();
        if (existing != null) for (Map<String, Object> e : repo.expenses(recId)) storedExpenseIds.add(toInt(ci(e, "PurchasePreBillExpenseDetailId")));
        for (int idx = 0; idx < expenses.size(); idx++) {
            PurchasePreBillDto.Expense e = expenses.get(idx);
            if (i(e.ItemId) != 0 && !contains(otherItems, i(e.ItemId))) throw new IllegalArgumentException("Other Item Not Found in Expense Grid Row no : " + (idx + 1));
            if (i(e.Id) != 0 && !storedExpenseIds.contains(i(e.Id))) throw new IllegalArgumentException("Record Not Found in Expense Grid Row no : " + (idx + 1));
        }

        /* W3 — the demand rows. */
        Map<Integer, Map<String, Object>> stored = new HashMap<>();
        List<Map<String, Object>> storedList = existing != null ? repo.details(recId) : new ArrayList<>();
        for (Map<String, Object> d : storedList) stored.put(toInt(ci(d, "PurchasePreBillDetailId")), d);
        Map<String, Map<String, Object>> pending = new HashMap<>();
        boolean anyNew = false;
        for (PurchasePreBillDto.Row r : rows) if (recId == 0 || i(r.Id) == 0) anyNew = true;
        if (anyNew) {
            for (Map<String, Object> p : repo.pendingDemands(u, fy, DEMAND_DOCUMENT_TYPE_ID, null, null, 0, 0, 0)) {
                pending.putIfAbsent(toInt(ci(p, "Id")) + ":" + toInt(ci(p, "DetailId")), p);
            }
        }
        Set<String> pairs = new HashSet<>();
        Set<Integer> postedIds = new HashSet<>();
        List<Source> sources = new ArrayList<>();
        for (int idx = 0; idx < rows.size(); idx++) {
            PurchasePreBillDto.Row r = rows.get(idx);
            String pair = i(r.PurchaseDemandHeaderId) + ":" + i(r.PurchaseDemandDetailId);
            if (!pairs.add(pair)) throw new IllegalArgumentException("Demand row repeated in Detail Grid at row No: " + (idx + 1));
            int rowId = recId != 0 ? i(r.Id) : 0;                                                   // :1067
            Source s = new Source();
            if (rowId > 0) {
                Map<String, Object> d = stored.get(rowId);
                if (d == null || !postedIds.add(rowId)
                        || toInt(ci(d, "PurchaseDemandHeaderId")) != i(r.PurchaseDemandHeaderId)
                        || toInt(ci(d, "PurchaseDemandDetailId")) != i(r.PurchaseDemandDetailId)) {
                    throw new IllegalArgumentException("Record Not Found in Detail Grid at row No: " + (idx + 1));
                }
                s.itemId = toInt(ci(d, "ItemId"));
                s.uomId = toInt(ci(d, "UomId"));
                s.demandQty = toDouble(ci(d, "PurchaseDemandQty"));
                s.purchasedQty = toDouble(ci(d, "TotalPurchasedForDemand"));
                s.conditionId = toInt(ci(d, "ItemConditionId"));
            } else {
                Map<String, Object> p = pending.get(pair);
                if (p == null) throw new IllegalArgumentException("Demand is not pending for Pre-Bill in Detail Grid at row No: " + (idx + 1));
                s.itemId = toInt(ci(p, "ItemId"));
                s.uomId = toInt(ci(p, "ItemSchuomId"));
                s.demandQty = toDouble(ci(p, "ApprovedQty"));                                          // :2293
                s.purchasedQty = toDouble(ci(p, "QtyUsedInPreBill")) + toDouble(ci(p, "QtyUsedInGrnDirectly"));  // :2294
                s.conditionId = toInt(ci(p, "ItemConditionId"));                                     // :2313
            }
            /* W4 — the condition must be one the grid combo offers (Id != 4) OR the one the loader /
               stored row carried in (LoadInGridDetail copies it unfiltered, :2313; the desktop saves it). */
            int c = i(r.ItemConditionId);
            if (c != 0 && c != s.conditionId && !contains(conditions, c)) {
                throw new IllegalArgumentException("Item Condition Not Found in Detail Grid at row No: " + (idx + 1));
            }
            sources.add(s);
        }
        List<Map<String, Object>> removedStored = new ArrayList<>();
        if (recId > 0) {                                                                              // :1056
            Set<Integer> seen = new HashSet<>();
            for (PurchasePreBillDto.Row r : removed) {
                int rid = i(r.Id);
                Map<String, Object> d = stored.get(rid);
                if (rid <= 0 || d == null || postedIds.contains(rid) || !seen.add(rid)) throw new IllegalArgumentException("Record Not Found");
                removedStored.add(d);
            }
        }
        if (!Boolean.TRUE.equals(dto.confirmed)) {                                                    // W6
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("confirm", recId == 0 ? "Are you sure to Save" : "Are you sure to Update");
            return m;
        }

        // -------------------------------------------------- CalculateFooterAmounts at :1021 (W5)
        double itemAmountSum = 0d, expAmountSum = 0d;
        for (PurchasePreBillDto.Row r : rows) itemAmountSum += d(r.ItemAmount);
        for (PurchasePreBillDto.Expense e : expenses) expAmountSum += d(e.Amount);                 // D11
        double footerDiscount = cvDouble(dto.DiscountAmount);
        double billAmount = formatSingle(itemAmountSum + expAmountSum - footerDiscount, amountDecimals(u));

        // ----------------------------------------------------------------- the object (:1030-1112)
        Timestamp now = Timestamp.valueOf(LocalDateTime.now().withNano(0));
        Map<String, Object> head = headerModel();
        head.put("IsApproved", false);
        head.put("ApprovedDate", now);
        head.put("DocDate", StoreIssuanceService.formDate(dto.DocDate, recId));
        head.put("EntryDate", now);
        head.put("ModifyDate", now);
        head.put("BillAmount", billAmount);
        head.put("DiscountAmount", footerDiscount);
        head.put("ActionId", recId == 0 ? 1 : 2);                                                   // BLL 0314 Save
        head.put("ApprovedUserId", u.getId());
        head.put("BillToPartyId", billToPartyId);
        head.put("BranchesId", branch(u));
        head.put("CityId", cityId);
        head.put("CompanyId", u.getCompanyId());
        head.put("DeliveryTermId", deliveryTermId);
        head.put("DocNo", docNo);
        head.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        head.put("EntryUserId", u.getId());
        head.put("FinancialYearId", fy);
        head.put("ModifyUserId", u.getId());
        head.put("OrganizationId", u.getOrganizationId());
        head.put("ProjectsId", branch(u));                                                            // D2
        head.put("PurchasePreBillHeaderId", recId);
        head.put("AttachmentsValues", existing == null ? "" : ci(existing, "AttachmentsValues"));
        head.put("CustomAttachmentsValues", existing == null ? "" : ci(existing, "CustomAttachmentsValues"));
        head.put("RemarksHeader", nz(dto.RemarksHeader));

        Timestamp partyBillDate = vendorBillDate(dto.VendorBillDate,
                storedList.isEmpty() ? null : asTimestamp(ci(storedList.get(0), "PartyBillDate")));     // ReadById :1162
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> d : removedStored) {                                                 // lstRemoveRecord, :1058
            Map<String, Object> vd = detailModel();
            vd.put("PurchasePreBillDetailId", toInt(ci(d, "PurchasePreBillDetailId")));
            vd.put("PurchaseDemandHeaderId", toInt(ci(d, "PurchaseDemandHeaderId")));
            vd.put("PurchaseDemandDetailId", toInt(ci(d, "PurchaseDemandDetailId")));
            vd.put("ItemId", toInt(ci(d, "ItemId")));
            vd.put("UomId", toInt(ci(d, "UomId")));
            vd.put("ItemConditionId", toInt(ci(d, "ItemConditionId")));
            vd.put("Qty", toDouble(ci(d, "Qty")));
            vd.put("Rate", toDouble(ci(d, "Rate")));
            vd.put("ItemAmountWithoutDiscount", toDouble(ci(d, "ItemAmountWithoutDiscount")));
            vd.put("DiscountAmount", toDouble(ci(d, "DiscountAmount")));
            vd.put("ItemAmount", toDouble(ci(d, "ItemAmount")));
            vd.put("ItemNetAmount", toDouble(ci(d, "ItemNetAmount")));
            vd.put("RemarksDetail", str(ci(d, "RemarksDetail")));
            fillFromHeaderControls(vd, dto, vendorId, refPartyId, partyBillDate);
            vd.put("ActionTypeId", 3);
            details.add(vd);
        }
        for (int idx = 0; idx < rows.size(); idx++) {                                                 // :1063-1085
            PurchasePreBillDto.Row r = rows.get(idx);
            Source s = sources.get(idx);
            Map<String, Object> vd = detailModel();
            int rowId = recId != 0 ? i(r.Id) : 0;
            vd.put("PurchasePreBillDetailId", rowId);
            vd.put("ActionTypeId", rowId <= 0 ? 1 : 2);
            vd.put("PurchaseDemandHeaderId", i(r.PurchaseDemandHeaderId));
            vd.put("PurchaseDemandDetailId", i(r.PurchaseDemandDetailId));
            vd.put("ItemId", s.itemId);
            vd.put("UomId", s.uomId);
            vd.put("ItemConditionId", i(r.ItemConditionId));
            double qty = d(r.ThisQty);
            vd.put("Qty", qty);
            vd.put("Rate", d(r.Rate));
            vd.put("ItemAmountWithoutDiscount", d(r.ItemAmountWithoutDiscount));
            vd.put("DiscountAmount", d(r.DiscountAmount));
            vd.put("ItemAmount", d(r.ItemAmount));
            vd.put("ItemNetAmount", d(r.ItemAmount) + d(r.ExpenseAmount));                            // BillProportion (:1341), W5
            vd.put("RemarksDetail", nz(r.Remarks));
            fillFromHeaderControls(vd, dto, vendorId, refPartyId, partyBillDate);
            validate(vd.get("PurchaseDemandHeaderId"), "Demand No", idx);
            validate(vd.get("PurchaseDemandDetailId"), "Demand No", idx);
            validate(vd.get("ItemId"), "Item", idx);
            validate(vd.get("UomId"), "Uom", idx);
            validate(vd.get("ItemConditionId"), "Item Condition", idx);
            validate(vd.get("Qty"), "This Qty", idx);
            validate(vd.get("Rate"), "Rate", idx);
            validate(vd.get("ItemAmountWithoutDiscount"), "Gross amount", idx);
            validate(vd.get("ItemAmount"), "Item amount", idx);
            validate(vd.get("ItemNetAmount"), "Item Net amount", idx);
            if (qty > s.demandQty - s.purchasedQty) {                                                // :1080, D5
                throw new IllegalArgumentException("You can't add Qty more than Balance Qty in Detail Grid at row No: " + (idx + 1));
            }
            details.add(vd);
        }
        List<Map<String, Object>> exps = new ArrayList<>();
        for (int idx = 0; idx < expenses.size(); idx++) {                                             // :1086-1107
            PurchasePreBillDto.Expense e = expenses.get(idx);
            if (i(e.ItemId) == 0) continue;
            if (d(e.Amount) == 0d) throw new IllegalArgumentException("Amount Field Required in Expense Grid Row no : " + (idx + 1));
            Map<String, Object> pe = expenseModel();
            pe.put("Amount", d(e.Amount));
            pe.put("Qty", (double) cvInt(d(e.Qty)));                                                    // D4
            pe.put("Rate", d(e.Rate));
            pe.put("PurchasePreBillExpenseDetailId", i(e.Id));
            pe.put("PurchaseOrderId", 0);
            pe.put("PurchaseOrderExpId", 0);
            pe.put("OtherItemId", i(e.ItemId));
            pe.put("Remarks", nz(e.Remarks));
            exps.add(pe);
        }

        int id = setData(head, details, exps);
        LOG.debug("Store purchase pre-bill {} saved (DocNo {})", id, docNo);
        return ok(recId > 0 ? "Data Update Successfully....  " + docNo : "Data Save Successfully....  " + docNo, id, docNo);
    }

    /** The detail fields FillDetailListCommonForInsertAndDelete takes from the header controls (:1213-1220). */
    private static void fillFromHeaderControls(Map<String, Object> vd, PurchasePreBillDto dto, int vendorId, int refPartyId,
                                               Timestamp partyBillDate) {
        vd.put("VendorSupplierId", vendorId);
        vd.put("ReferencePartyId", refPartyId);
        vd.put("VehicleNo", nz(dto.VehicleNo));
        vd.put("BiltyNo", nz(dto.BiltyNo));
        vd.put("PartyBillNo", nz(dto.VendorBillNo));
        vd.put("PartyBillDate", partyBillDate);
    }

    /**
     * DAL 0254 SetData, in one transaction: header InsertAndUpdate → every detail (removed rows first,
     * then the grid) → every expense row. (Attachments are not ported.)
     */
    private int setData(Map<String, Object> head, List<Map<String, Object>> details, List<Map<String, Object>> exps) {
        if (details.isEmpty()) throw new IllegalArgumentException("Detail list not found");
        int recId = toInt(head.get("PurchasePreBillHeaderId"));
        int num = repo.setProc("[dbo].[USP_PurchasePreBillHeader_InsertAndUpdate]", head);
        int id = num > 0 ? num : recId;
        if (id <= 0) throw new IllegalArgumentException("Save returned no document id.");               // W7
        for (Map<String, Object> d : details) {
            d.put("PurchasePreBillHeaderId", id);
            repo.setProc("[dbo].[USP_PurchasePreBillDetail_Insert]", d);
        }
        for (Map<String, Object> e : exps) {
            e.put("PurchasePreBillHeaderId", id);
            repo.setProc("[dbo].[USP_PurchasePreBillExpenseDetail_Insert]", e);
        }
        return id;
    }

    private static final class Source {
        int itemId, uomId, conditionId;
        double demandQty, purchasedQty;
    }

    // ============================================================================= models

    /** Model 0440 PurchasePreBillHeader — non-virtual properties in declaration order. */
    private static Map<String, Object> headerModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("IsApproved", false);
        p.put("ApprovedDate", null);
        p.put("DocDate", null);
        p.put("EntryDate", null);
        p.put("ModifyDate", null);
        p.put("BillAmount", 0d);
        p.put("DiscountAmount", 0d);
        for (String k : new String[] { "ActionId", "ApprovedUserId", "BillToPartyId", "BranchesId", "CityId", "CompanyId",
                "DeliveryTermId", "DocNo", "DocumentTypeId", "EntryUserId", "FinancialYearId", "ModifyUserId",
                "OrganizationId", "ProjectsId", "PurchasePreBillHeaderId" }) p.put(k, 0);
        for (String k : new String[] { "ApprovalRemarks", "AttachmentsValues", "CustomAttachmentsValues", "ReferenceNo",
                "RemarksHeader" }) p.put(k, null);
        return p;
    }

    /** Model 0437 PurchasePreBillDetail — non-virtual properties in declaration order. */
    private static Map<String, Object> detailModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("PartyBillDate", null);
        for (String k : new String[] { "Qty", "Rate", "ItemAmountWithoutDiscount", "DiscountAmount", "ItemAmount",
                "ExpenseAmount", "ItemNetAmount" }) p.put(k, 0d);                                       // ExpenseAmount never set (D3)
        for (String k : new String[] { "ActionTypeId", "ItemId", "PurchaseDemandDetailId", "PurchaseDemandHeaderId",
                "PurchaseOrderDetailId", "PurchaseOrderHeaderId", "PurchasePreBillDetailId", "PurchasePreBillHeaderId",
                "ReferencePartyId", "UomId", "ItemConditionId", "VendorSupplierId", "WareHouseId" }) p.put(k, 0);
        for (String k : new String[] { "BiltyNo", "PartyBillNo", "VehicleNo", "RemarksDetail" }) p.put(k, null);
        return p;
    }

    /** Model 0441 PurchasePreBillExpenseDetail — non-virtual properties in declaration order. */
    private static Map<String, Object> expenseModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Amount", 0d);
        p.put("Qty", 0d);
        p.put("Rate", 0d);
        p.put("PurchasePreBillExpenseDetailId", 0);
        p.put("PurchaseOrderId", 0);
        p.put("PurchaseOrderExpId", 0);
        p.put("OtherItemId", 0);
        p.put("PurchasePreBillHeaderId", 0);
        p.put("Remarks", null);
        return p;
    }

    // ============================================================================ helpers

    /** The header must exist, be DocumentTypeId 147, and belong to the user's organization and company. */
    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) return null;
        if (toInt(ci(h, "CompanyId")) != i(u.getCompanyId())) return null;
        if (toInt(ci(h, "OrganizationId")) != i(u.getOrganizationId())) return null;
        return h;
    }

    /**
     * txtVendorBillDate.Value — a picker carrying a time of day (designer default Now); ReadById
     * assigns the stored PartyBillDate, whose time the picker keeps when only the day is changed.
     */
    private static Timestamp vendorBillDate(String day, Timestamp stored) {
        if (stored != null) {
            LocalDate d = isBlank(day) ? stored.toLocalDateTime().toLocalDate() : LocalDate.parse(day.trim().substring(0, 10));
            return Timestamp.valueOf(LocalDateTime.of(d, stored.toLocalDateTime().toLocalTime()));
        }
        return StoreIssuanceService.pickerDate(day);
    }

    private static Timestamp asTimestamp(Object v) {
        if (v instanceof Timestamp) return (Timestamp) v;
        if (v instanceof LocalDateTime) return Timestamp.valueOf((LocalDateTime) v);
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime());
        return null;
    }

    /** clsGlobalVariables.stringFormatsingle = "#,##0." + N zeros (CommonServices.GetDecimalConfiguration:5376). */
    private int amountDecimals(UserAccount u) {
        int n = toInt(common.config(u, "Default NoofDecimal Points For Amount"));
        return n >= 1 && n <= 4 ? n : 0;
    }

    /** Conversion.ToDouble(x.ToString(stringFormatsingle)) — custom format rounding is away from zero. */
    private static double formatSingle(double x, int decimals) {
        return new BigDecimal(clr(x)).setScale(decimals, RoundingMode.HALF_UP).doubleValue();
    }

    private String financialYearStart(UserAccount u, int yearId) {
        try {
            for (Map<String, Object> r : jdbc.queryForList(
                    "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                    u.getOrganizationId(), u.getCompanyId())) {
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

    /** FormHelper.ValidateField. */
    private static void validate(Object v, String field, int rowIndex) {
        boolean bad = v == null
                || (v instanceof Integer && (Integer) v == 0)
                || (v instanceof Double && (Double) v <= 0d)
                || (v instanceof String && ((String) v).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    /** Conversion.ToInt on a double — Convert.ToInt32(double) rounds half to even. */
    private static int cvInt(double v) { return (int) Math.rint(v); }

    /** Conversion.ToInt on text — Convert.ToInt32(string) only accepts an integer; anything else is 0. */
    private static int cvIntText(String s) {
        if (s == null) return 0;
        try { return Integer.parseInt(s.trim()); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDouble on text — thousands separators allowed; anything unparseable is 0. */
    private static double cvDouble(String s) {
        if (s == null || s.trim().isEmpty()) return 0d;
        try { return Double.parseDouble(s.trim().replace(",", "")); } catch (NumberFormatException e) { return 0d; }
    }

    private static boolean contains(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (toInt(r.get("Id")) == id) return true;
        return false;
    }

    /** Date values leave as local "yyyy-MM-dd HH:mm:ss" text, never as a JSON timestamp. */
    static Object plain(Object v) {
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString() + " 00:00:00";
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString().replace('T', ' ');
        if (v instanceof LocalDate) return v.toString() + " 00:00:00";
        if (v instanceof java.time.OffsetDateTime) return ((java.time.OffsetDateTime) v).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        return v;
    }

    static List<Map<String, Object>> plainRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) o.put(e.getKey(), plain(e.getValue()));
            out.add(o);
        }
        return out;
    }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String t = v == null ? "" : String.valueOf(v).trim();
        return "true".equalsIgnoreCase(t) || "1".equals(t);
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static int i(Integer v) { return v == null ? 0 : v; }
    private static double d(Double v) { return v == null ? 0d : v; }
    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }
    private static String nz(String s) { return s == null ? "" : s; }

    private static Map<String, Object> ok(String message, int id, int docNo) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        m.put("id", id);
        m.put("docNo", docNo);
        return m;
    }
}
