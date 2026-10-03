package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.StockInTransitDto;
import com.mst.repositories.StockInTransitRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Supplier Purchases (ModuleId 5) — screen 868 "Stock In Transit".
 *
 * <pre>
 * Screen id        868 (dbo.ScreenDefinition; TargetUrl Architecture.WinApp.Purchase.frmSupplierDispatchPreBill)
 * ScreenName       frmSupplierDispatchPreBill   (base.Name; rights key — frmSupplierDispatchPreBill.cs:536/:553/:635)
 * DocumentTypeId   251                          (:870, :898, :2074, :3340, :3646)
 * Route            /purchase/stock-in-transit ; API /api/purchase/stock-in-transit
 * Desktop          Architecture.WinApp.Purchase/frmSupplierDispatchPreBill.cs (8,560 lines, designer :4695-8560)
 * BLL / DAL        BLL 0517 / DAL 0383 Inventory.SupplierDispatch; BLL 0595 PurchaseOrder
 *                  (GetPurchaseOrderForPurchaseInvoice, via LoadPurchaseOrder DocumentTypeId 41);
 *                  BLL 0581 InvPurchaseInvoice.PurchaseOrderSupplierExpenseByOrderIds
 * Models           0927 SupplierDispatch, 0926 SupplierDispatchDetail, 0940 SupplierDispatchCommDetail,
 *                  0941 SupplierDispatchExpense
 * Print            CommonServices.SupplierDispatchPreBillSlip251 -> 251_SupplierDispatchPreBillSlip.rpt
 *                  (USP_SupplierDispatch_SlipAndRegister) — the page calls /api/print/by-template/...
 * </pre>
 *
 * No voucher and no stock posting: SetData (DAL 0383) writes the header, the detail rows, the
 * commission/brokery rows and the expense rows, nothing else. The records feed the Inward Gate Pass
 * and GRN "Transit Vehicle" / pre-bill lookup (USP_SupplierDispatch_GetNoForGpandGrn reads
 * SupplierDispatch rows with Status 'Open' for the supplier, not yet used by a gate pass or GRN).
 * Numbering, the financial-year check and "DocDate cannot be greater than ServerDate" are inside
 * USP_SupplierDispatch_InsertAndUpdate; the form has no date-lock check.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED, NOT CORRECTED (each is the user's call to change)
 * ---------------------------------------------------------------------------------------------
 *  D1  FormHistory sends @EntryUser (BLL 0517 :236) but the procedure filters on @EntryUserId: a user
 *      without "CanView AllRecord" gets NO history rows at all.
 *  D2  Header ProjectId is saved as the user's BranchesId (:2072); PendingForView is sent as 0.
 *  D3  Brokery UOM is saved as the combo's VALUE — the list Id 1..4 — not the 40/50/60/100 shown
 *      (:2223 Conversion.ToDouble(CmbBrokeryRateUom.Value)); Commission UOM is saved as the TEXT
 *      (:2219). Reopened, the brokery UOM therefore shows "1".."4".
 *  D4  Commission / brokery type is saved as the list Id ("1".."3", :2219/:2223), not the text.
 *  D5  Expense Qty is saved through Conversion.ToInt (:2260) — a whole number (banker's rounding).
 *  D6  The "Fcy Amount Rate Field is Required" check compares the FORMATTED text with "0" (:1367), so
 *      a zero amount shown as "0.00" passes.
 *  D7  Total gross weight vs Supplier WB weight is an exact double comparison (:2205).
 *  D8  "Data Save Successfully....  n" shows the Doc No displayed before the save; the procedure
 *      recomputes it (MAX+1) on insert.
 *  D9  Update does not check "approved" (only Delete does, :1961, and the DeleteById branch).
 *  D10 TotalBrokeryAmount (:3136) keeps the previous brokery amount when a rate is typed but the type
 *      is not Flat / Percent / Comm Weight; BillAmount (:2917) resets it to 0 when no broker is chosen.
 *  D11 An expense row with no Other Item still counts in Expense Amount and the bill total
 *      (GetColumnSum over the whole grid, :2910) but is not saved (:2249).
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web only)
 * ---------------------------------------------------------------------------------------------
 *  W1  Doc No / Branch Sr No: on a new save the server asks the generator, on update it keeps the
 *      stored numbers; never taken from the request.
 *  W2  Open / update / delete / print / status-update only when the record is DocumentTypeId 251 and
 *      belongs to the user's organization and company (ReadById and DeleteById filter by Id alone).
 *  W3  Every posted reference must be in the list the form offers: supplier / commission agent /
 *      broker (supplier list), reference party, city, currency, item (item types 14/17 excluded),
 *      crop year, packing type, pack/rate UOM (the item's UOM schedule), receiver location (the
 *      user's allocated branches), other item. A row with Id &gt; 0 must be a live detail of THIS
 *      record carrying the same order line; a new row with an order line must be a line the
 *      Load-Order loader offers now for that order; an expense row's order expense must belong to
 *      its order.
 *  W4  The header totals, commission/brokery amounts and row FcyAmount are recomputed exactly as the
 *      desktop does at the start of Insert() (:2034-2038) — the disabled boxes are not trusted.
 *  W5  Grid rows that were saved cannot be removed on the desktop (DeleteDetailRow :1556 reads a
 *      "CommissionAgentId" cell the table does not have and fails); the page reproduces that, so the
 *      removed-rows list (lstRemoveRecord) is never posted and the server never soft-deletes rows.
 *
 * Attachments (Attachment button, attachment count link): PurchaseDocAttachmentService, saved with the
 * document. The Reference Party button opens the DefineReferenceParties page. Not ported: saved grid
 * layouts (CtrlGrdBar).
 */
@Service
public class StockInTransitService {

    private static final Logger LOG = LoggerFactory.getLogger(StockInTransitService.class);

    public static final String SCREEN_NAME = "frmSupplierDispatchPreBill";
    public static final int SCREEN_ID = 868;
    public static final int DOCUMENT_TYPE_ID = 251;
    public static final int ORDER_DOCUMENT_TYPE_ID = 41;          // btnPurchaseOrderLoader_Click :4555
    private static final int FEATURE_MULTI_CURRENCY = 6;          // :637
    private static final int FEATURE_BRANCH = 9;                  // :638

    private final StockInTransitRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;
    /* DMS attachments (btnattachment, AT) - written inside the save transaction (DAL 0383 SetData :74-106). */
    @org.springframework.beans.factory.annotation.Autowired private PurchaseDocAttachmentService attachments;

    public StockInTransitService(StockInTransitRepository repo, StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
    }

    /** The attachment list / download of a record (PurchaseDocAttachmentController): View + the ownership check of load(). */
    public void requireReadable(int id) {
        UserAccount u = requireView();
        if (ownedHeader(u, id) == null) throw new IllegalArgumentException("Record Not Found");
    }

    // ======================================================================== form load

    /** InitializeComponentMethod (:631) — everything the form binds on open. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (!Boolean.TRUE.equals(r.get("view"))) throw new AccessDeniedException("You do not have the View right for Stock In Transit.");
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", r);
        out.put("screenId", SCREEN_ID);
        out.put("documentTypeId", DOCUMENT_TYPE_ID);
        out.put("multiCurrency", repo.feature(u, FEATURE_MULTI_CURRENCY));
        out.put("branchFeature", repo.feature(u, FEATURE_BRANCH));
        out.put("docNo", repo.generateCode(u, fy, DOCUMENT_TYPE_ID));                   // DocumentNoFill
        out.put("branchSrNo", repo.generateBranchCode(u, fy, DOCUMENT_TYPE_ID));        // BranchDocumentNoFill
        out.put("decimals", decimals(u));
        putConfigs(u, out);
        putGlobals(u, out);
        putHistoryBranches(u, out);
        return out;
    }

    /** btnRefresh_Click (:2450) — the globals rebound, configurations re-read. */
    public Map<String, Object> formRefresh() {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        putConfigs(u, out);
        putGlobals(u, out);
        return out;
    }

    /** Reset (:2561-2564) — the two generators and the configurations, asked again. */
    public Map<String, Object> numbers() {
        UserAccount u = requireView();
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", repo.generateCode(u, fy, DOCUMENT_TYPE_ID));
        out.put("branchSrNo", repo.generateBranchCode(u, fy, DOCUMENT_TYPE_ID));
        putConfigs(u, out);
        return out;
    }

    /** btnRefreshHistory_Click (:3305) — HistoryBranchComboDbCall + HistoryBranchComboFill. */
    public Map<String, Object> historyRefresh() {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        putHistoryBranches(u, out);
        return out;
    }

    /**
     * GetConfigurationsFromGlobal (:841) + GetConfigurationsFromGlobalAndBindValuesInColumns (:815) +
     * GetBaseCurrencyAndRate (:2626).
     */
    private void putConfigs(UserAccount u, Map<String, Object> out) {
        out.put("branchImplemented", toBoolText(repo.config(u, "SupplierDispatchBranchWise")));
        out.put("itemSearchByCode", toBoolText(repo.config(u, "ItemSearchByCode")));
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("defaultCropYear", toInt(repo.config(u, "Default Crop Year")));
        out.put("defaultPackingType", toInt(repo.config(u, "Paking Type")));
        out.put("cityArea", toInt(repo.config(u, "City Area")));
        out.put("baseCurrency", toInt(repo.config(u, "Base Currency")));
        out.put("baseCurrencyRate", toDouble(repo.config(u, "BaseCurrencyRate")));
    }

    private void putGlobals(UserAccount u, Map<String, Object> out) {
        out.put("suppliers", suppliers(u));                  // SupplierDtFillFromGlobal (:958): supplier, comm agent, broker
        out.put("referenceParties", referenceParties(u));    // ReferancePartyBind (:998)
        out.put("deliveryTerms", deliveryTerms());           // DeliveryTerm (:1027)
        out.put("currencies", currencies(u));                // CurrencyBind (:1073)
        out.put("items", items(u));                          // ItemDtFillFromGlobal (:1086)
        out.put("cropYears", cropYears(u));                  // CropDtFillFromGlobalAndBind (:1122)
        out.put("packingTypes", packingTypes());             // PackingTypeDtFillFromGlobalAndBind (:1137)
        out.put("cities", cities(u));                        // CityDtFillFromGlobalAndBind (:1012)
        out.put("receiverLocations", receiverLocations(u));  // BranchDetailBind (:1182)
        out.put("otherItems", otherItems(u));                // OtherItemdtDbCall (:1195)
        out.put("statuses", statuses());                     // dtStatus (:1205)
    }

    /**
     * HistoryBranchComboDbCall (:762): SupplierDispatchBranchWise on -> only the user's own branch;
     * otherwise USP_GetBranchsAllocatedToUserFromSupplierDispatch. HistoryBranchComboFill (:806) then
     * sets the combo text to the user's branch name — the page ticks that branch.
     */
    private void putHistoryBranches(UserAccount u, Map<String, Object> out) {
        boolean branchImplemented = toBoolText(repo.config(u, "SupplierDispatchBranchWise"));
        int userBranch = u.getBranchesId() == null ? 0 : u.getBranchesId();
        String userBranchName = "";
        for (Map<String, Object> b : repo.branchesAllocatedToUser(u)) {
            if (toInt(ci(b, "BranchId")) == userBranch) { userBranchName = str(ci(b, "BranchName")); break; }
        }
        List<Map<String, Object>> list = new ArrayList<>();
        if (branchImplemented) {
            list.add(idName(userBranch, userBranchName));
        } else {
            for (Map<String, Object> b : repo.historyBranches(u)) list.add(idName(toInt(ci(b, "BranchId")), str(ci(b, "BranchName"))));
        }
        out.put("historyBranches", list);
        out.put("userBranchId", userBranch);
        out.put("userBranchName", userBranchName);
    }

    // ============================================================================ lists

    /**
     * SupplierDtFillFromGlobal (:958) — clsGlobalVariables.globalSupplierCustomer. The helper that fills
     * that global (DatatableHelper) is not in this workspace; the rule below is the one the other ported
     * Purchase/Store pages document for the same global (PurchaseInvoiceStoreMgmtService,
     * PurchasePreBillService): USP_GetVendorsAndCustomersWithCityName, customer groups 7/9/10 always out,
     * 8 and 13 also out when ShowBothVendorAndCustomerOnSalesPurchase = 1.
     * Columns (:554-560): Id, CompanyName, PartyCode, GlAccountId, CityId, CityName, MobileNo — combo
     * columns 3 and 4 (GlAccountId, CityId) hidden (:983-990).
     */
    private List<Map<String, Object>> suppliers(UserAccount u) {
        int showBoth = toInt(repo.config(u, "ShowBothVendorAndCustomerOnSalesPurchase"));
        Set<Integer> excluded = new HashSet<>(Arrays.asList(7, 8, 9, 10, 13));
        Set<Integer> mustExclude = new HashSet<>(Arrays.asList(7, 9, 10));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.vendorsAndCustomers(u)) {
            int group = toInt(ci(r, "CustomerGroupId"));
            if (mustExclude.contains(group)) continue;
            if (showBoth == 1 && excluded.contains(group)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("CompanyName", str(ci(r, "CompanyName")));
            o.put("PartyCode", str(ci(r, "PartyCode")));
            o.put("GlAccountId", toInt(ci(r, "GlAccountId")));
            o.put("CityId", toInt(ci(r, "CityId")));
            o.put("CityName", str(ci(r, "CityName")));
            o.put("MobileNo", str(ci(r, "MobilePersonal")));
            out.add(o);
        }
        return out;
    }

    private List<Map<String, Object>> referenceParties(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.referenceParties(u)) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "ReferencePartyName"))));
        return out;
    }

    /** DeliveryTerm (:1035): Id / Description (column 2 hidden). */
    private List<Map<String, Object>> deliveryTerms() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.deliveryTerms()) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "Description"))));
        return out;
    }

    /** CurrencyBind (:1078): Id / CurrencyCode (AllColumns). */
    private List<Map<String, Object>> currencies(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.currencies(u)) {
            Map<String, Object> o = idName(toInt(ci(r, "Id")), str(ci(r, "CurrencyCode")));
            o.put("CurrencyName", str(ci(r, "CurrencyName")));
            out.add(o);
        }
        return out;
    }

    /** ItemDtFillFromGlobal (:1091): getGlobalAllItems without ItemTypeOfTypeId 14 and 17; Id, ItemName, ItemCode. */
    private List<Map<String, Object>> items(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.allItems(u)) {
            int t = toInt(ci(r, "ItemTypeOfTypeId"));
            if (t == 14 || t == 17) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemCode", str(ci(r, "ItemCode")));
            out.add(o);
        }
        return out;
    }

    private List<Map<String, Object>> cropYears(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.cropYears(u)) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "CropYear"))));
        return out;
    }

    private List<Map<String, Object>> packingTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.packingTypes()) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "PackTypeDesc"))));
        return out;
    }

    private List<Map<String, Object>> cities(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.cities(u)) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "CityName"))));
        return out;
    }

    /** BranchDetailBind (:1187): BranchId / BranchName — also the grid's ReceiverLocationId value list (:1799). */
    private List<Map<String, Object>> receiverLocations(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.branchesAllocatedToUser(u)) out.add(idName(toInt(ci(r, "BranchId")), str(ci(r, "BranchName"))));
        return out;
    }

    private List<Map<String, Object>> otherItems(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.otherItems(u)) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "OtherItemName"))));
        return out;
    }

    /** SpStaticColumnNames 'SupplierDispatchStatus': Open 1, Complete 2, Cancle 3 (value member Id, display "status"). */
    private List<Map<String, Object>> statuses() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.statuses()) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "status"))));
        return out;
    }

    /**
     * CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId) (:1159) — the item's rows of the UOM
     * schedule: Id, UOMCode, Equivalent (SelectedRow.Cells[2] is the equivalent, :1482/:2726/:2776).
     */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = requireView();
        return uomsFor(u, itemId);
    }

    private List<Map<String, Object>> uomsFor(UserAccount u, int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (itemId <= 0) return out;
        for (Map<String, Object> r : repo.uomSchedule(u)) {
            if (toInt(ci(r, "ItemId")) != itemId) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("UOMCode", str(ci(r, "UOMCode")));
            o.put("Equivalent", toDouble(ci(r, "Equivalent")));
            out.add(o);
        }
        return out;
    }

    /** CmbSupplierDetail_Leave (:1266) — the first row of LastSavedRecord, or null. */
    public Map<String, Object> lastSaved(int supplierId) {
        UserAccount u = requireView();
        if (supplierId <= 0) return new LinkedHashMap<>();
        List<Map<String, Object>> r = repo.lastSavedRecord(u, ctx.currentFinancialYearId(), supplierId);
        Map<String, Object> out = new LinkedHashMap<>();
        if (r.isEmpty()) return out;
        Map<String, Object> f = r.get(0);
        out.put("CityId", toInt(ci(f, "CityId")));
        out.put("DeliveryTerm", str(ci(f, "DeliveryTerm")));
        out.put("CommType", str(ci(f, "CommType")));
        out.put("CommRate", toDouble(ci(f, "CommRate")));
        out.put("CommUom", toDouble(ci(f, "CommUom")));
        out.put("BrokeryType", str(ci(f, "BrokeryType")));
        out.put("BrokeryRate", toDouble(ci(f, "BrokeryRate")));
        out.put("BrokeryUom", toDouble(ci(f, "BrokeryUom")));
        return out;
    }

    // ========================================================================== loaders

    /**
     * btnPurchaseOrderLoader_Click (:4544) opens LoadPurchaseOrder with DocumentTypeId 41 and
     * ReturnFulldt; LoadInGridDetail (:4568) reads the loader's rows (Id, DocNo, PODetailId, OrderItemId,
     * ItemCode, ItemName, CropYearId, PackUomId, PackUom, PackEquivalent, ItemQty, GrossWeight, EmptyBags,
     * EmptyBagsTotal, RateUomId, UOMCode, Equivalent, OrderItemRate, SupplierCustomerId, DeliveryTerm,
     * Brokery*, BrokerAgentSupCustId, CommissionType, CommRate, UomScheduleIdCmRate, CommAmount,
     * RemarksHeader) — exactly the column set of Sp_PurchaseOrder_GetAllMethod
     * 'GetPurchaseOrderForPurchaseInvoice' (BLL 0595 :891). The dialog's own source is not in this
     * workspace; its filters here are that BLL's optional parameters.
     */
    public List<Map<String, Object>> pendingOrders(int supplierId, String fromDate, String toDate, String fromDocNo, String toDocNo) {
        UserAccount u = requireView();
        return plainRows(repo.purchaseOrdersForLoader(u, ORDER_DOCUMENT_TYPE_ID, supplierId, 0,
                dateOnly(fromDate), dateOnly(toDate), cvIntText(fromDocNo), cvIntText(toDocNo)));
    }

    /** LoadExpData (:4650) — the loaded orders' supplier expenses. */
    public List<Map<String, Object>> orderExpenses(String orderIds) {
        requireView();
        String ids = cleanIds(orderIds);
        if (ids.isEmpty()) return new ArrayList<>();
        return plainRows(repo.purchaseOrderSupplierExpenses(ids));
    }

    // ========================================================================== history

    /**
     * gridhistoryfill (:3330). dateType: doc / entry / modify / approved (the four radio buttons);
     * branchIds is the ticked branches. No branch ticked is "Select branch first" (:3460).
     */
    public List<Map<String, Object>> history(String branchIds, String dateType, boolean fromChecked, String from,
                                             boolean toChecked, String to, String fromDocNo, String toDocNo) {
        UserAccount u = requireView();
        String ids = cleanIds(branchIds);
        if (ids.isEmpty()) throw new IllegalArgumentException("Select branch first");
        boolean viewAll = rights.has(SCREEN_NAME, "viewAll");
        Timestamp f = fromChecked && !isBlank(from) ? pickerDate(from) : null;
        Timestamp t = toChecked && !isBlank(to) ? pickerDate(to) : null;
        Timestamp df = null, dt = null, ef = null, et = null, mf = null, mt = null, af = null, at = null;
        switch (dateType == null ? "doc" : dateType) {
            case "entry":    ef = f; et = t; break;
            case "modify":   mf = f; mt = t; break;
            case "approved": af = f; at = t; break;
            default:         df = f; dt = t; break;
        }
        /* :3402 BranchIds = BranchIds + "," + id, starting from "" — a leading comma. */
        StringBuilder sb = new StringBuilder();
        for (String id : ids.split(",")) sb.append(',').append(id);
        List<Map<String, Object>> rows = repo.formHistory(u, ctx.currentFinancialYearId(), DOCUMENT_TYPE_ID, viewAll,
                df, dt, cvIntText(fromDocNo), cvIntText(toDocNo), ef, et, mf, mt, af, at, sb.toString());
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {                                                     // dtcol, :3409-3447
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("DocumentTypeId", toInt(ci(r, "DocumentTypeId")));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("BranchSrNo", toInt(ci(r, "BranchSrNo")));
            o.put("DocDate", plain(ci(r, "DocDate")));
            o.put("SupplierId", toInt(ci(r, "SupplierId")));
            o.put("Supplier", ci(r, "SupplierName"));
            o.put("ReferenceParty", ci(r, "ReferencePartyName"));
            o.put("ManualBillNo", ci(r, "ManualBillNo"));
            o.put("VehicleNo", ci(r, "VehicleNo"));
            o.put("BiltyNo", ci(r, "BiltyNo"));
            o.put("DepartureDate", plain(ci(r, "DepartureDate")));
            o.put("TransitDays", toInt(ci(r, "TransitDays")));
            o.put("ETADestination", plain(ci(r, "ETAdestination")));
            o.put("SupplierWbWeight", toDouble(ci(r, "SupplierWbWeight")));
            o.put("TotalQty", toDouble(ci(r, "TotalQty")));
            o.put("TotalWeight", toDouble(ci(r, "TotalWeight")));
            o.put("TotalAmount", toDouble(ci(r, "TotalAmount")));
            o.put("FcyAmount", toDouble(ci(r, "FcyAmount")));
            o.put("Freight", toDouble(ci(r, "Freight")));
            o.put("OtherExpense", toDouble(ci(r, "OtherExpense")));
            o.put("CityName", ci(r, "CityName"));
            o.put("RemarksHeader", ci(r, "RemarksHeader"));
            o.put("DeliveryTerm", ci(r, "DeliveryTerm"));
            o.put("Status", ci(r, "Status"));
            o.put("EntryDate", plain(ci(r, "EntryDate")));
            o.put("EntryUser", ci(r, "EntryUser"));
            o.put("ModifyDate", plain(ci(r, "ModifyDate")));
            o.put("ModifyUser", ci(r, "ModifyUser"));
            o.put("IsApproved", toBool(ci(r, "IsApproved")) ? "Approved" : "Not Approved");
            o.put("ApprovedDate", plain(ci(r, "ApprovedDate")));
            o.put("ApprovedUser", ci(r, "ApprovedUser"));
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    /**
     * btnUpdateStatusHistory_Click (:3789) — one SupplierDispatchApprove per ticked row, ReqType
     * "UpdateStatus", Status = the row's Status cell text, all in one transaction (BLL 0517 :358).
     */
    @Transactional
    public Map<String, Object> updateStatus(List<Map<String, Object>> rows) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "update")) throw new IllegalArgumentException("You Don't have right for Update");
        if (rows == null || rows.isEmpty()) throw new IllegalArgumentException("Please select check box first");
        Set<String> allowed = new HashSet<>();
        for (Map<String, Object> s : statuses()) allowed.add(str(s.get("Name")));
        for (Map<String, Object> r : rows) {
            int id = toInt(r.get("Id"));
            String status = str(r.get("Status"));
            if (ownedHeader(u, id) == null) throw new IllegalArgumentException("Record Not Found");         // W2
            if (!allowed.contains(status)) throw new IllegalArgumentException("Status Not Found");          // W3 (LimitToList :3476)
            repo.supplierDispatchApprove("UpdateStatus", status, u.getId(), id);
        }
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", "Record Approved Successfully");
        return m;
    }

    // =============================================================================== read

    /** ReadById (:2328) / GridDetailBind (:3690) — header, detail rows, commission rows, expense rows. */
    public Map<String, Object> load(int id) {
        UserAccount u = requireView();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) throw new IllegalArgumentException("Record Not Found");
        List<Map<String, Object>> details = repo.details(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", id);
        out.put("DocDate", plain(ci(h, "DocDate")));
        out.put("DocNo", toInt(ci(h, "DocNo")));
        out.put("BranchSrNo", toInt(ci(h, "BranchSrNo")));
        out.put("ManualBillNo", str(ci(h, "ManualBillNo")));
        out.put("VehicleNo", str(ci(h, "VehicleNo")));
        out.put("BiltyNo", str(ci(h, "BiltyNo")));
        out.put("DepartureDate", plain(ci(h, "DepartureDate")));
        out.put("TransitDays", toInt(ci(h, "TransitDays")));
        out.put("ETAdestination", plain(ci(h, "ETAdestination")));
        out.put("Freight", toDouble(ci(h, "Freight")));
        out.put("OtherExpense", toDouble(ci(h, "OtherExpense")));
        out.put("AdvanceFreight", toDouble(ci(h, "AdvanceFreight")));
        out.put("CityId", toInt(ci(h, "CityId")));
        out.put("SupplierWbWeight", toDouble(ci(h, "SupplierWbWeight")));
        out.put("DeliveryTerm", str(ci(h, "DeliveryTerm")));
        out.put("TotalQty", toDouble(ci(h, "TotalQty")));
        out.put("TotalWeight", toDouble(ci(h, "TotalWeight")));
        out.put("TotalAmount", toDouble(ci(h, "TotalAmount")));
        out.put("CurrencyId", toInt(ci(h, "CurrencyId")));
        out.put("ExchangeRate", toDouble(ci(h, "ExchangeRate")));
        out.put("FcyAmount", toDouble(ci(h, "FcyAmount")));
        out.put("RemarksHeader", str(ci(h, "RemarksHeader")));
        out.put("IsApproved", toBool(ci(h, "IsApproved")));
        out.put("Status", str(ci(h, "Status")));
        if (!details.isEmpty()) {
            out.put("SupplierId", toInt(ci(details.get(0), "SupplierId")));                          // :2368
            out.put("ReferencePartyId", toInt(ci(details.get(0), "ReferencePartyId")));              // :2369
        }
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : details) {                                                     // :2373 / :3727
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("OrderId", toInt(ci(d, "OrderId")));
            o.put("OrderNo", toInt(ci(d, "OrderNo")));
            o.put("OrderDetailId", toInt(ci(d, "OrderDetailId")));
            o.put("ItemId", toInt(ci(d, "ItemId")));
            o.put("ItemCode", str(ci(d, "ItemCode")));
            o.put("ItemName", str(ci(d, "ItemName")));
            o.put("CropYearId", toInt(ci(d, "CropYearId")));
            o.put("CropYear", str(ci(d, "CropYear")));
            o.put("PackingTypeId", toInt(ci(d, "PackingTypeId")));
            o.put("PackingType", str(ci(d, "PackingType")));
            o.put("PackUomId", toInt(ci(d, "PackSizeId")));
            o.put("PackUom", str(ci(d, "PackUomCode")));
            o.put("PackUomEquivalent", toDouble(ci(d, "PackUomEquivalent")));
            o.put("Qty", toDouble(ci(d, "Qty")));
            o.put("GrossWeight", toDouble(ci(d, "GrossWeight")));
            o.put("EBUnit", toDouble(ci(d, "EBWeight")));
            o.put("EBTotal", toDouble(ci(d, "EBTotalWt")));
            o.put("AddLss", toDouble(ci(d, "AdLsWeight")));
            o.put("Weight", toDouble(ci(d, "WeightKgs")));
            o.put("RateUomId", toInt(ci(d, "RateUomId")));
            o.put("RateUom", str(ci(d, "RateUomCode")));
            o.put("RateUomEquivalent", toDouble(ci(d, "RateUomEquivalent")));
            o.put("Rate", toDouble(ci(d, "Rate")));
            o.put("Amount", toDouble(ci(d, "Amount")));
            o.put("FcyAmount", toDouble(ci(d, "FcyAmount")));
            o.put("Remarks", str(ci(d, "Remarks")));
            o.put("ReceiverLocationId", toInt(ci(d, "BranchId")));
            o.put("ReceiverLocation", str(ci(d, "BranchName")));
            rows.add(o);
        }
        out.put("rows", rows);
        List<Map<String, Object>> comm = new ArrayList<>();
        for (Map<String, Object> c : repo.commDetails(id)) {                                        // :2382-2403
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("TypeId", toInt(ci(c, "TypeId")));
            o.put("CommissionAgentId", toInt(ci(c, "CommissionAgentId")));
            o.put("CommType", str(ci(c, "CommType")));
            o.put("CommRate", toDouble(ci(c, "CommRate")));
            o.put("RateUom", toDouble(ci(c, "RateUom")));
            o.put("CommAmount", toDouble(ci(c, "CommAmount")));
            comm.add(o);
        }
        out.put("comm", comm);
        List<Map<String, Object>> exps = new ArrayList<>();
        for (Map<String, Object> e : repo.expenses(id)) {                                           // :2413-2416
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(e, "Id")));
            o.put("OrderId", toInt(ci(e, "OrderId")));
            o.put("OrderExpId", toInt(ci(e, "OrderExpId")));
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

    // ============================================================================= delete

    /** btnDelete_Click (:1957) → BLL 0517 DeleteByID (its own transaction). */
    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "delete")) throw new AccessDeniedException("You do not have the Delete right for this screen.");
        if (id <= 0) throw new IllegalArgumentException("Record Not Found");
        Map<String, Object> h = ownedHeader(u, id);                                                  // W2
        if (h == null) throw new IllegalArgumentException("Record Not Found");
        if (toBool(ci(h, "IsApproved"))) throw new IllegalArgumentException("Record Not Delete because Record has approved");
        repo.deleteById(u.getId(), id);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", "Delete Record Successfully");
        m.put("id", id);
        return m;
    }

    /** The print buttons: Print right (btnprint.Enabled, :665) and the record's ownership (W2). */
    public Map<String, Object> checkPrint(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "print")) throw new AccessDeniedException("You do not have the Print right for this screen.");
        if (id <= 0 || ownedHeader(u, id) == null) throw new IllegalArgumentException("Record Not Found");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("id", id);
        m.put("template", "251_SupplierDispatchPreBillSlip.rpt");
        return m;
    }

    // =============================================================================== save

    /** btnsave_Click / btnupdate_Click → Insert() (:1999). */
    @Transactional
    public Map<String, Object> save(StockInTransitDto dto) {
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
        boolean multiCurrency = repo.feature(u, FEATURE_MULTI_CURRENCY);
        boolean branchFeature = repo.feature(u, FEATURE_BRANCH);
        boolean branchImplemented = toBoolText(repo.config(u, "SupplierDispatchBranchWise"));
        Map<String, Object> dec = decimals(u);
        int amountDec = (Integer) dec.get("amount");
        int fcyDec = (Integer) dec.get("fcy");
        List<StockInTransitDto.Row> rows = dto.rows == null ? new ArrayList<>() : dto.rows;
        List<StockInTransitDto.Expense> expenses = dto.expenses == null ? new ArrayList<>() : dto.expenses;

        // --------------------------------------------------------------- :2011 + FormValidation (:1321)
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        int docNo = existing != null ? toInt(ci(existing, "DocNo")) : repo.generateCode(u, fy, DOCUMENT_TYPE_ID);            // W1
        int branchSrNo = existing != null ? toInt(ci(existing, "BranchSrNo")) : repo.generateBranchCode(u, fy, DOCUMENT_TYPE_ID);
        if (docNo == 0) throw new IllegalArgumentException("Doc No Field is Required");
        List<Map<String, Object>> suppliers = suppliers(u);
        Map<Integer, Map<String, Object>> supplierById = byId(suppliers);
        int supplierId = i(dto.SupplierId);
        if (supplierId == 0 || !supplierById.containsKey(supplierId)) throw new IllegalArgumentException("Supplier Name Field is Required");
        String vehicleNo = nz(dto.VehicleNo);
        if (vehicleNo.trim().isEmpty() || vehicleNo.trim().equals("0")) throw new IllegalArgumentException("Vehicle No Field is Required");
        String transitText = nz(dto.TransitDays).trim();
        if (transitText.isEmpty() || cvIntText(transitText) == 0) throw new IllegalArgumentException("TransitDays Field is Required");
        int cityId = i(dto.CityId);
        if (cityId == 0 || !contains(cities(u), cityId)) throw new IllegalArgumentException("CityName Field is Required");
        int currencyId = i(dto.CurrencyId);
        boolean currencyOk = currencyId != 0 && contains(currencies(u), currencyId);
        String exText = nz(dto.ExchangeRate).trim();
        double exRate = cvDouble(exText);

        // Row FcyAmount and the FcyAmount box as txtExchangeRate_TextChanged + CalculateTotalInformation leave them (:2674/:2648)
        List<BigDecimal> rowFcy = new ArrayList<>();
        BigDecimal fcySum = BigDecimal.ZERO;
        for (StockInTransitDto.Row r : rows) {
            BigDecimal f = BigDecimal.ZERO;
            BigDecimal ex = BigDecimal.valueOf(exRate);
            if (ex.compareTo(BigDecimal.ZERO) > 0) {
                f = dec(d(r.Amount)).divide(ex, MathContext.DECIMAL128).setScale(fcyDec, RoundingMode.HALF_EVEN);
            }
            rowFcy.add(f);
            fcySum = fcySum.add(f);
        }
        String fcyText = formatFixed(fcySum, fcyDec);
        if (multiCurrency) {
            if (!currencyOk) throw new IllegalArgumentException("Fcy Code Field is Required");
            if (exText.isEmpty() || exText.equals("0")) throw new IllegalArgumentException("Exchange Rate Field is Required");
            if (fcyText.isEmpty() || fcyText.equals("0")) throw new IllegalArgumentException("Fcy Amount Rate Field is Required");     // D6
        } else {
            if (!currencyOk) throw new IllegalArgumentException("Please Configure Your Base Currency In configurations");
            if (exText.isEmpty() || exText.equals("0")) throw new IllegalArgumentException("Please Configure Your Base Currency Rate In configurations");
        }

        // -------------------------------------- :2034-2038 TotalCommissionAmount, TotalBrokeryAmount, ..., BillAmount
        double sumAmount = 0d, sumWeight = 0d, sumQty = 0d;
        for (StockInTransitDto.Row r : rows) { sumAmount += d(r.Amount); sumWeight += d(r.Weight); sumQty += d(r.Qty); }
        double expSum = 0d;
        for (StockInTransitDto.Expense e : expenses) expSum += d(e.Amount);                          // D11
        String commRateText = nz(dto.CommRate);
        String commTypeText = nz(dto.CommTypeText);
        double commAmount = commissionAmount(commRateText, commTypeText, nz(dto.CommUomText), sumAmount, sumWeight, amountDec);
        String brokeryRateText = nz(dto.BrokeryRate);
        int commAgentId = i(dto.CommissionAgentId);
        int brokeryAcId = i(dto.BrokeryAcId);
        if (commAgentId != 0 && !supplierById.containsKey(commAgentId)) throw new IllegalArgumentException("Commission Agent Not Found");   // W3
        if (brokeryAcId != 0 && !supplierById.containsKey(brokeryAcId)) throw new IllegalArgumentException("Broker Not Found");              // W3
        double brokeryAmount = brokeryAmount(brokeryRateText, nz(dto.BrokeryTypeText), nz(dto.BrokeryUomText),
                cvDouble(dto.BrokeryAmountText), sumAmount, sumWeight, amountDec);
        /* BillAmount (:2903) */
        double advanceFreight = cvDouble(dto.AdvanceFreight);
        double bill = sumAmount + expSum;
        if (supplierId == commAgentId) bill += commAmount;
        if (brokeryAcId != 0) {
            if (brokeryAmount > 0d) bill -= brokeryAmount; else brokeryAmount = 0d;
        } else {
            brokeryAmount = 0d;
        }
        bill += advanceFreight;
        double totalAmount = formatSingle(bill, amountDec);
        BigDecimal totalQty = dec(sumQty).setScale(2, RoundingMode.HALF_EVEN);                      // Math.Round(decimal, 2)
        BigDecimal totalWeight = dec(sumWeight).setScale(2, RoundingMode.HALF_EVEN);

        // ---------------------------------------------------------------------- :2039-2068
        if (commAmount > 0d && commAgentId == 0) throw new IllegalArgumentException("Please Select Commission Agent Account First");
        if (commAgentId > 0 && commAmount == 0d) throw new IllegalArgumentException("Commission Amount Required when Commission agent is Selected");
        if (brokeryAmount > 0d && brokeryAcId == 0) throw new IllegalArgumentException("Please Select Broker Account First");
        if (brokeryAcId > 0 && brokeryAmount == 0d) throw new IllegalArgumentException("Brokery Amount Required when Broker is Selected");
        if (brokeryAcId == supplierId) throw new IllegalArgumentException("Broker And Supplier Can't be Same...");

        int refPartyId = i(dto.ReferencePartyId);
        if (refPartyId != 0 && !contains(referenceParties(u), refPartyId)) throw new IllegalArgumentException("Reference Party Not Found");   // W3

        // ------------------------------------------------------------------ the detail rows (:2110-2199)
        List<Map<String, Object>> items = items(u);
        Map<Integer, Map<String, Object>> itemById = byId(items);
        List<Map<String, Object>> crops = cropYears(u);
        List<Map<String, Object>> packs = packingTypes();
        List<Map<String, Object>> locations = receiverLocations(u);
        List<Map<String, Object>> uomRows = repo.uomSchedule(u);
        Map<Integer, Map<String, Object>> stored = new HashMap<>();
        if (existing != null) for (Map<String, Object> d : repo.details(recId)) stored.put(toInt(ci(d, "Id")), d);
        Map<Integer, List<Map<String, Object>>> orderLines = new HashMap<>();
        Set<Integer> postedIds = new HashSet<>();
        double totalGrossWt = 0d;
        List<Map<String, Object>> details = new ArrayList<>();
        for (int idx = 0; idx < rows.size(); idx++) {
            StockInTransitDto.Row r = rows.get(idx);
            int rowNo = idx + 1;
            Map<String, Object> vd = detailModel();
            int rowId = recId == 0 ? 0 : i(r.Id);
            vd.put("ActionTypeId", rowId > 0 ? 2 : 1);
            vd.put("Id", rowId);
            if (i(r.ItemId) == 0) throw new IllegalArgumentException("Item Field Required in Grid Row no : " + rowNo);
            if (i(r.CropYearId) == 0) throw new IllegalArgumentException("CropYear Field Required in Grid Row no : " + rowNo);
            if (i(r.PackingTypeId) == 0) throw new IllegalArgumentException("PackingType Field Required in Grid Row no : " + rowNo);
            if (i(r.PackUomId) == 0) throw new IllegalArgumentException("PackUom Field Required in Grid Row no : " + rowNo);
            if (d(r.Qty) == 0d) throw new IllegalArgumentException("Qty Field Required in Grid Row no : " + rowNo);
            if (d(r.GrossWeight) == 0d) throw new IllegalArgumentException("GrossWeight Field Required in Grid Row no : " + rowNo);
            if (d(r.Weight) == 0d) throw new IllegalArgumentException("Weight Field Required in Grid Row no : " + rowNo);
            if (i(r.RateUomId) == 0) throw new IllegalArgumentException("RateUom Field Required in Grid Row no : " + rowNo);
            if (d(r.Rate) == 0d) throw new IllegalArgumentException("Rate Field Required in Grid Row no : " + rowNo);
            if (d(r.Amount) == 0d) throw new IllegalArgumentException("Amount Field Required in Grid Row no : " + rowNo);
            if (branchFeature && !branchImplemented && i(r.ReceiverLocationId) == 0) {
                throw new IllegalArgumentException("ReceiverLocation Field Required in Grid Row no : " + rowNo);
            }
            /* W3 — the posted references must be the form's own lists. */
            if (!itemById.containsKey(i(r.ItemId))) throw new IllegalArgumentException("Item Not Found in Grid Row no : " + rowNo);
            if (!contains(crops, i(r.CropYearId))) throw new IllegalArgumentException("CropYear Not Found in Grid Row no : " + rowNo);
            if (!contains(packs, i(r.PackingTypeId))) throw new IllegalArgumentException("PackingType Not Found in Grid Row no : " + rowNo);
            if (!uomOfItem(uomRows, i(r.ItemId), i(r.PackUomId))) throw new IllegalArgumentException("PackUom Not Found in Grid Row no : " + rowNo);
            if (!uomOfItem(uomRows, i(r.ItemId), i(r.RateUomId))) throw new IllegalArgumentException("RateUom Not Found in Grid Row no : " + rowNo);
            if (i(r.ReceiverLocationId) != 0 && !contains(locations, i(r.ReceiverLocationId))) {
                throw new IllegalArgumentException("ReceiverLocation Not Found in Grid Row no : " + rowNo);
            }
            if (rowId > 0) {
                Map<String, Object> s = stored.get(rowId);
                if (s == null || !postedIds.add(rowId)
                        || toInt(ci(s, "OrderId")) != i(r.OrderId) || toInt(ci(s, "OrderDetailId")) != i(r.OrderDetailId)) {
                    throw new IllegalArgumentException("Record Not Found in Grid Row no : " + rowNo);
                }
            } else if (i(r.OrderId) != 0 || i(r.OrderDetailId) != 0) {
                List<Map<String, Object>> lines = orderLines.computeIfAbsent(i(r.OrderId), oid -> oid <= 0 ? new ArrayList<>()
                        : repo.purchaseOrdersForLoader(u, ORDER_DOCUMENT_TYPE_ID, 0, oid, null, null, 0, 0));
                boolean found = false;
                for (Map<String, Object> l : lines) {
                    if (toInt(ci(l, "PODetailId")) == i(r.OrderDetailId) && toInt(ci(l, "OrderItemId")) == i(r.ItemId)) { found = true; break; }
                }
                if (!found) throw new IllegalArgumentException("Purchase Order line is not pending in Grid Row no : " + rowNo);
            }
            vd.put("SupplierId", supplierId);
            vd.put("OrderId", i(r.OrderId));
            vd.put("CommissionAgentId", 0);                                                           // :2176
            vd.put("ReferencePartyId", refPartyId);
            vd.put("OrderDetailId", i(r.OrderDetailId));
            vd.put("ItemId", i(r.ItemId));
            vd.put("CropYearId", i(r.CropYearId));
            vd.put("PackingTypeId", i(r.PackingTypeId));
            vd.put("PackSizeId", i(r.PackUomId));
            vd.put("Qty", d(r.Qty));
            vd.put("GrossWeight", d(r.GrossWeight));
            totalGrossWt += d(r.GrossWeight);
            vd.put("EBWeight", d(r.EBUnit));
            vd.put("EBTotalWt", d(r.EBTotal));
            vd.put("AdLsWeight", d(r.AddLss));
            vd.put("WeightKgs", d(r.Weight));
            vd.put("RateUomId", i(r.RateUomId));
            vd.put("Rate", d(r.Rate));
            vd.put("Amount", d(r.Amount));
            vd.put("CurrencyId", currencyId);
            vd.put("ExchangeRate", BigDecimal.valueOf(exRate));
            vd.put("FcyAmount", rowFcy.get(idx));
            vd.put("Remarks", nz(r.Remarks));
            vd.put("BranchId", i(r.ReceiverLocationId));
            details.add(vd);
        }
        if (details.isEmpty()) throw new IllegalArgumentException("At least enter value/quantity in one of the rows");
        double supplierWb = cvDouble(dto.SupplierWbWeight);
        if (totalGrossWt != supplierWb) {                                                            // D7
            throw new IllegalArgumentException("Total Gross Weight " + netStr(totalGrossWt) + " in Detail Grid is not Equal to SupplierWb "
                    + netStr(supplierWb) + " Weight!");
        }

        // ---------------------------------------------------------- commission / brokery rows (:2216-2245)
        List<Map<String, Object>> comms = new ArrayList<>();
        if (commAgentId != 0 && cvDouble(commRateText) != 0d) {
            comms.add(commModel(1, commAgentId, toInt(supplierById.get(commAgentId).get("GlAccountId")),
                    String.valueOf(cvIntText(dto.CommTypeValue)), cvDouble(dto.CommUomText), cvDouble(commRateText), commAmount));
        }
        if (brokeryAcId != 0 && cvDouble(brokeryRateText) != 0d) {
            comms.add(commModel(2, brokeryAcId, toInt(supplierById.get(brokeryAcId).get("GlAccountId")),
                    String.valueOf(cvIntText(dto.BrokeryTypeValue)), cvDouble(dto.BrokeryUomValue), cvDouble(brokeryRateText), brokeryAmount));   // D3, D4
        }

        // ---------------------------------------------------------------------- expense rows (:2246-2266)
        List<Map<String, Object>> others = otherItems(u);
        Map<Integer, List<Map<String, Object>>> orderExps = new HashMap<>();
        List<Map<String, Object>> exps = new ArrayList<>();
        for (int idx = 0; idx < expenses.size(); idx++) {
            StockInTransitDto.Expense e = expenses.get(idx);
            if (i(e.ItemId) == 0) continue;
            if (d(e.Amount) == 0d) throw new IllegalArgumentException("Amount Field Required in Expense Grid Row no : " + (idx + 1));
            if (!contains(others, i(e.ItemId))) throw new IllegalArgumentException("Other Item Not Found in Expense Grid Row no : " + (idx + 1));   // W3
            if (i(e.OrderExpId) != 0) {
                List<Map<String, Object>> oe = orderExps.computeIfAbsent(i(e.OrderId), oid -> oid <= 0 ? new ArrayList<>()
                        : repo.purchaseOrderSupplierExpenses(String.valueOf(oid)));
                boolean found = false;
                for (Map<String, Object> x : oe) if (toInt(ci(x, "Id")) == i(e.OrderExpId)) { found = true; break; }
                if (!found) throw new IllegalArgumentException("Order Expense Not Found in Expense Grid Row no : " + (idx + 1));
            }
            Map<String, Object> pe = new LinkedHashMap<>();                                           // model 0941, declaration order
            pe.put("Amount", d(e.Amount));
            pe.put("Qty", (double) cvInt(d(e.Qty)));                                                  // D5
            pe.put("Rate", d(e.Rate));
            pe.put("Id", i(e.Id));
            pe.put("OrderId", i(e.OrderId));
            pe.put("OrderExpId", i(e.OrderExpId));
            pe.put("OtherItemId", i(e.ItemId));
            pe.put("SupplierDispatchId", 0);
            pe.put("Remarks", nz(e.Remarks));
            exps.add(pe);
        }

        // ------------------------------------------------------------------------- the header (:2069-2103)
        Timestamp now = Timestamp.valueOf(LocalDateTime.now().withNano(0));
        Timestamp docDate = pickerKeepTime(dto.DocDate, existing == null ? null : asTimestamp(ci(existing, "DocDate")));
        LocalTime docTime = docDate.toLocalDateTime().toLocalTime();
        Timestamp departure = isBlank(dto.DepartureDate) ? Timestamp.valueOf(docDate.toLocalDateTime().toLocalDate().atStartOfDay())
                : Timestamp.valueOf(LocalDate.parse(dto.DepartureDate.trim().substring(0, 10)).atStartOfDay());
        Timestamp eta = isBlank(dto.ETAdestination) ? docDate
                : Timestamp.valueOf(LocalDateTime.of(LocalDate.parse(dto.ETAdestination.trim().substring(0, 10)), docTime));
        Map<String, Object> head = new LinkedHashMap<>();                                            // model 0927, declaration order
        head.put("IsApproved", false);
        head.put("ApprovedDate", now);
        head.put("DepartureDate", departure);
        head.put("DocDate", docDate);
        head.put("EntryDate", now);                                                                   // BLL Save :19
        head.put("ETAdestination", eta);
        head.put("ModifyDate", now);
        head.put("ExchangeRate", BigDecimal.valueOf(exRate));
        head.put("FcyAmount", new BigDecimal(fcyText.replace(",", "")));
        head.put("Freight", BigDecimal.valueOf(cvDouble(dto.Freight)));
        head.put("OtherExpense", BigDecimal.valueOf(cvDouble(dto.OtherExpense)));
        head.put("SupplierWbWeight", BigDecimal.valueOf(supplierWb));
        head.put("TotalAmount", BigDecimal.valueOf(totalAmount));
        head.put("TotalQty", totalQty);
        head.put("TotalWeight", totalWeight);
        head.put("BranchSrNo", branchSrNo);
        head.put("FinancialYearId", fy);
        head.put("ActionId", recId == 0 ? 1 : 2);                                                    // BLL Save :21-28
        head.put("ApprovedUserId", u.getId());
        head.put("BranchesId", branch(u));
        head.put("CityId", cityId);
        head.put("CompanyId", u.getCompanyId());
        head.put("CurrencyId", currencyId);
        head.put("DocNo", docNo);
        head.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        head.put("EntryUserId", u.getId());
        head.put("Id", recId);
        head.put("ModifyUserId", u.getId());
        head.put("OrganizationId", u.getOrganizationId());
        head.put("PendingForView", 0);                                                               // D2
        head.put("ProjectId", branch(u));                                                            // D2
        head.put("TransitDays", cvIntText(transitText));
        head.put("DeliveryTerm", nz(dto.DeliveryTerm));
        head.put("BiltyNo", nz(dto.BiltyNo));
        head.put("Driver1CellNo", "");                                                               // :2094
        head.put("Driver2CellNo", null);
        head.put("DriverName", null);
        head.put("DriverPic", null);
        head.put("RemarksHeader", nz(dto.RemarksHeader));
        head.put("VehicleImage", null);
        head.put("VehicleNo", vehicleNo);
        head.put("ManualBillNo", nz(dto.ManualBillNo));
        head.put("AdvanceFreight", BigDecimal.valueOf(advanceFreight));
        head.put("AttachmentsValues", existing == null ? "" : str(ci(existing, "AttachmentsValues")));
        head.put("CustomAttachmentsValues", existing == null ? "" : str(ci(existing, "CustomAttachmentsValues")));
        PurchaseDocAttachmentService.Prepared attached = attachments.prepare(recId, DOCUMENT_TYPE_ID, dto.attachments);
        if (attached != null && attached.changed()) {                                                // DAL 0383 :47-48
            head.put("AttachmentsValues", attached.names());
            head.put("CustomAttachmentsValues", attached.storedNames());
        }

        int id = setData(head, details, comms, exps);
        attachments.persist(id, DOCUMENT_TYPE_ID, id, attached);                                     // :95 RefAccountId = the record id (desktop)
        LOG.debug("Stock In Transit {} saved (DocNo {})", id, docNo);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", recId > 0 ? "Data Update Successfully....  " + docNo : "Data Save Successfully....  " + docNo);
        m.put("id", id);
        m.put("docNo", docNo);
        return m;
    }

    /** DAL 0383 SetData — header → details → commission rows → expense rows, one transaction. */
    private int setData(Map<String, Object> head, List<Map<String, Object>> details,
                        List<Map<String, Object>> comms, List<Map<String, Object>> exps) {
        if (details.isEmpty()) throw new IllegalArgumentException("Detail list not found");
        int recId = toInt(head.get("Id"));
        int num = repo.setProc("[dbo].[USP_SupplierDispatch_InsertAndUpdate]", head);
        int id = num > 0 ? num : recId;
        if (id <= 0) throw new IllegalStateException("Save returned no document id.");
        for (Map<String, Object> d : details) {
            d.put("SupplierDispatchId", id);
            repo.setProc("[dbo].[USP_SupplierDispatchDetail_Insert]", d);
        }
        for (Map<String, Object> c : comms) {
            c.put("SupplierDispatchId", id);
            repo.setProc("USP_SupplierDispatchCommDetail_Insert", c);
        }
        for (Map<String, Object> e : exps) {
            e.put("SupplierDispatchId", id);
            repo.setProc("USP_SupplierDispatchExpense_Insert", e);
        }
        return id;
    }

    /** Model 0926 SupplierDispatchDetail — non-virtual properties in declaration order. */
    private static Map<String, Object> detailModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("ExchangeRate", BigDecimal.ZERO);
        p.put("FcyAmount", BigDecimal.ZERO);
        for (String k : new String[] { "AdLsWeight", "Amount", "EBTotalWt", "EBWeight", "GrossWeight", "Qty", "Rate", "WeightKgs" }) p.put(k, 0d);
        for (String k : new String[] { "ActionTypeId", "BranchId", "CommissionAgentId", "CropYearId", "CurrencyId", "Id", "ItemId",
                "OrderId", "OrderDetailId", "PackingTypeId", "PackSizeId", "RateUomId", "ReferencePartyId", "SortNo",
                "SupplierDispatchId", "SupplierId" }) p.put(k, 0);
        p.put("Remarks", null);
        return p;
    }

    /** Model 0940 SupplierDispatchCommDetail — non-virtual properties in declaration order (Remarks never set). */
    private static Map<String, Object> commModel(int typeId, int agentId, int glId, String commType, double uom, double rate, double amount) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("CommAmount", amount);
        p.put("CommRate", rate);
        p.put("CommType", commType);
        p.put("RateUom", uom);
        p.put("CommissionAgentGLId", glId);
        p.put("CommissionAgentId", agentId);
        p.put("TypeId", typeId);
        p.put("Id", 0);
        p.put("SupplierDispatchId", 0);
        p.put("Remarks", null);
        return p;
    }

    // ======================================================================== arithmetic

    /**
     * TotalCommissionAmount (:3042) — the txtcommamount text, parsed back as Insert does.
     * Empty rate: 0. Flat: the rate. Percent/Percentage: sum(Amount) * rate / 100. Comm Weight:
     * sum(Weight) / UOM * rate. Any other type: 0 (the first block's else).
     */
    private static double commissionAmount(String rateText, String typeText, String uomText, double sumAmount, double sumWeight, int dec) {
        if (rateText.isEmpty()) return 0d;
        double rate = cvDouble(rateText);
        switch (typeText) {
            case "Flat": return formatSingle(rate, dec);
            case "Percent":
            case "Percentage": return formatSingle(sumAmount * rate / 100.0, dec);
            case "Comm Weight": {
                double uom = cvDouble(uomText);
                double v = sumWeight / uom * rate;
                return Double.isFinite(v) ? formatSingle(v, dec) : 0d;
            }
            default: return 0d;
        }
    }

    /** TotalBrokeryAmount (:3136) — like the commission, except an unknown type keeps the previous text (D10). */
    private static double brokeryAmount(String rateText, String typeText, String uomText, double previous,
                                        double sumAmount, double sumWeight, int dec) {
        if (rateText.isEmpty()) return 0d;
        double rate = cvDouble(rateText);
        switch (typeText) {
            case "Flat": return formatSingle(rate, dec);
            case "Percent": return formatSingle(sumAmount * rate / 100.0, dec);
            case "Comm Weight": {
                double uom = cvDouble(uomText);
                double v = sumWeight / uom * rate;
                return Double.isFinite(v) ? formatSingle(v, dec) : 0d;
            }
            default: return previous;
        }
    }

    /**
     * CommonServices.GetDecimalConfiguration: stringFormatsingle ("Default NoofDecimal Points For
     * Amount", 1-4 else none), DecimalRateFormate ("Default NoofDecimal Points For Rate", 1-4, 0 gives
     * two), DefaultNoofDecimalPointsForFcyAmount (read as DesktopVoucherSupport.decimals() reads it;
     * stringFormatsingleForFcy is taken to use the same count).
     */
    private Map<String, Object> decimals(UserAccount u) {
        Map<String, Object> m = new LinkedHashMap<>();
        int a = toInt(repo.config(u, "Default NoofDecimal Points For Amount"));
        m.put("amount", a >= 1 && a <= 4 ? a : 0);
        int r = toInt(repo.config(u, "Default NoofDecimal Points For Rate"));
        m.put("rate", r >= 1 && r <= 4 ? r : (r == 0 ? 2 : 0));
        int f = toInt(repo.config(u, "DefaultNoOfDecimalPointsForFcyAmount"));
        m.put("fcy", Math.max(0, Math.min(f, 10)));
        return m;
    }

    /** x.ToString("#,##0." + N zeros) parsed back — custom-format rounding is away from zero. */
    private static double formatSingle(double x, int decimals) {
        if (!Double.isFinite(x)) return 0d;
        return new BigDecimal(com.mst.repositories.StoreIssuanceRepository.clr(x)).setScale(decimals, RoundingMode.HALF_UP).doubleValue();
    }

    /** The text a "#,##0.00…" format writes for a decimal (grouping dropped) — used for the "0" test. */
    private static String formatFixed(BigDecimal v, int decimals) {
        return v.setScale(decimals, RoundingMode.HALF_UP).toPlainString();
    }

    // ============================================================================ helpers

    private UserAccount requireView() {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "view")) throw new AccessDeniedException("You do not have the View right for Stock In Transit.");
        return u;
    }

    /** The header must exist (ActionId &lt;&gt; 3), be DocumentTypeId 251 and belong to the user's organization and company. */
    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) return null;
        if (toInt(ci(h, "CompanyId")) != i(u.getCompanyId())) return null;
        if (toInt(ci(h, "OrganizationId")) != i(u.getOrganizationId())) return null;
        return h;
    }

    private static boolean uomOfItem(List<Map<String, Object>> uomRows, int itemId, int uomId) {
        for (Map<String, Object> r : uomRows) {
            if (toInt(ci(r, "ItemId")) == itemId && toInt(ci(r, "Id")) == uomId) return true;
        }
        return false;
    }

    /** A picker's Value: the chosen day at the picker's time of day (now for a new form, the stored time when reopened). */
    private static Timestamp pickerKeepTime(String day, Timestamp stored) {
        LocalDate d = isBlank(day) ? (stored != null ? stored.toLocalDateTime().toLocalDate() : LocalDate.now())
                : LocalDate.parse(day.trim().substring(0, 10));
        LocalTime t = stored != null ? stored.toLocalDateTime().toLocalTime() : LocalTime.now().withNano(0);
        return Timestamp.valueOf(LocalDateTime.of(d, t));
    }

    private static Timestamp pickerDate(String day) {
        return Timestamp.valueOf(LocalDateTime.of(LocalDate.parse(day.trim().substring(0, 10)), LocalTime.now().withNano(0)));
    }

    private static Timestamp dateOnly(String day) {
        if (isBlank(day)) return null;
        return Timestamp.valueOf(LocalDate.parse(day.trim().substring(0, 10)).atStartOfDay());
    }

    private static Timestamp asTimestamp(Object v) {
        if (v instanceof Timestamp) return (Timestamp) v;
        if (v instanceof LocalDateTime) return Timestamp.valueOf((LocalDateTime) v);
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime());
        return null;
    }

    private static Map<String, Object> idName(int id, String name) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("Name", name);
        return o;
    }

    private static Map<Integer, Map<String, Object>> byId(List<Map<String, Object>> rows) {
        Map<Integer, Map<String, Object>> m = new HashMap<>();
        for (Map<String, Object> r : rows) m.put(toInt(r.get("Id")), r);
        return m;
    }

    private static boolean contains(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (toInt(r.get("Id")) == id) return true;
        return false;
    }

    /** "1,2,x,3" -> "1,2,3" (digits only, no empties). */
    private static String cleanIds(String ids) {
        if (ids == null) return "";
        StringBuilder sb = new StringBuilder();
        for (String s : ids.split(",")) {
            String t = s.trim();
            if (!t.matches("\\d+") || Integer.parseInt(t) <= 0) continue;
            if (sb.length() > 0) sb.append(',');
            sb.append(Integer.parseInt(t));
        }
        return sb.toString();
    }

    /** Date values leave as local "yyyy-MM-dd HH:mm:ss" text, never as a JSON timestamp. */
    static Object plain(Object v) {
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString() + " 00:00:00";
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString().replace('T', ' ');
        if (v instanceof LocalDate) return v.toString() + " 00:00:00";
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

    /** double.ToString() — up to 15 significant digits, no exponent for ordinary weights. */
    private static String netStr(double v) {
        if (v == 0d) return "0";
        return new BigDecimal(v).round(new MathContext(15)).stripTrailingZeros().toPlainString();
    }

    private static BigDecimal dec(double v) { return new BigDecimal(com.mst.repositories.StoreIssuanceRepository.clr(v)); }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String t = v == null ? "" : String.valueOf(v).trim();
        return "true".equalsIgnoreCase(t) || "1".equals(t);
    }

    /** Conversion.ToBool on a configuration value ("1"/"True"). */
    private static boolean toBoolText(String v) { return toBool(v); }

    /** Conversion.ToInt on a double — Convert.ToInt32(double) rounds half to even. */
    private static int cvInt(double v) { return (int) Math.rint(v); }

    /** Conversion.ToInt on text — an integer (thousands separators allowed); anything else 0. */
    private static int cvIntText(String s) {
        if (s == null) return 0;
        try { return Integer.parseInt(s.trim().replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDouble on text — thousands separators allowed; anything unparseable is 0. */
    private static double cvDouble(String s) {
        if (s == null || s.trim().isEmpty()) return 0d;
        try { return Double.parseDouble(s.trim().replace(",", "")); } catch (NumberFormatException e) { return 0d; }
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static int i(Integer v) { return v == null ? 0 : v; }
    private static double d(Double v) { return v == null ? 0d : v; }
    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }
    private static String nz(String s) { return s == null ? "" : s; }
}
