package com.mst.services.sale.engr;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.services.SaleInvoiceFinancialDirect;
import com.mst.services.SaleInvoiceMfgEngrPersist;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Screen 780 "frmSaleInvoiceDirectEngr" = Architecture.WinApp.Mfg.Sale.frmSaleInvoiceDirectEngr (module 134 Sale Engr, "Sale Invoice Direct",
 * document type 1661; rows are typed in the item entry panel or loaded from pending sale orders, document type 1656, by frmLoadSaleOrderEngr).
 *
 * Desktop map (frmSaleInvoiceDirectEngr.cs, method : line):
 *   SupplierDtFillFromGlobal / ItemDtFillFromGlobal :1383, SampleModaldBCall :1310, ItemDetailBind :1403, ItemModalBind :1332, BindProductionStage :1627,
 *   CmbItem_Leave :1925, CmbCategory_Leave :1965, AvailableStockGetByItem :5005, btnAdd_Click :3343, grd_DoubleClick :3370, btnUpdateDetail_Click :3417,
 *   grdSettings :3646, Insert :3825, ReadById :4242, LoadInGridDetail :5083, LoadPaymentDetailBySaleOrderIds :5145, toolStripButton3_Click_1 :5184,
 *   GetAll :5355, BindDetailOfHeaderId :5636, GeneratePrint1661A :5944.
 * The header, expense, journal and payment logic is the same code as 849 / 847; the lines carry OrderId / OrderDetailId / OrderNo / OrderDate and a
 * production stage, and PaymentTermId 3 takes the remaining amount of the loaded sale order (grdPaymentTerm_CellUpdated).
 * BLL 0580 InvSaleInvoice.Save + DAL 0433 SetData (SaleInvoiceMfgEngrPersist) and the financial voucher (SaleInvoiceFinancialDirect) run in one transaction.
 *
 * The page runs the desktop's grid arithmetic by calling {@link #calc}; {@link #save} runs the same code again: loaded rows take their identity
 * from the database, typed rows are re-priced from qty, rate, rate equivalent (UOM schedule), add/less, discount and tax, so what is saved never
 * depends on what the browser computed.
 */
@Service
public class SaleInvoiceDirectMfgEngrService {
    public static final String SCREEN = "frmSaleInvoiceDirectEngr";
    public static final int DOC_TYPE = 1661;
    public static final String ORDER_TYPE = "1656";

    private final SaleEngrSupport sup;
    private final SaleEngrAttachments attachments;
    private final SaleInvoiceRepository repo;
    private final SaleInvoiceMfgEngrPersist persist;

    public SaleInvoiceDirectMfgEngrService(SaleEngrSupport sup, SaleEngrAttachments attachments, SaleInvoiceRepository repo, SaleInvoiceMfgEngrPersist persist) {
        this.sup = sup; this.attachments = attachments; this.repo = repo; this.persist = persist;
    }

    // ------------------------------------------------------------------ configuration (cached for a few seconds per company)

    private final Map<String, Object[]> cache = new ConcurrentHashMap<>();

    @SuppressWarnings("unchecked")
    private <T> T cached(String key, java.util.function.Supplier<T> s) {
        String k = sup.org() + "|" + sup.company() + "|" + key;
        Object[] e = cache.get(k);
        long now = System.currentTimeMillis();
        if (e != null && (long) e[0] > now) return (T) e[1];
        T v = s.get();
        cache.put(k, new Object[]{now + 15000L, v});
        if (cache.size() > 500) cache.clear();
        return v;
    }

    private static final class Cfg {
        boolean sub, multi, branchFeature, freightToExp, taxEditable, branchImplemented;
        int defaultDays, outwardFreightAc;
        int amtRound, amtPlaces, ratePlaces, fcyPlaces;
    }

    /** InitializeComponentMethod's feature / configuration reads. */
    private Cfg cfg() {
        return cached("cfg", () -> {
            Cfg c = new Cfg();
            c.sub = sup.erpFeature(4);                                                  // SubsidiaryAccountAllownOnVouchers
            c.branchFeature = sup.erpFeature(11);                                       // BranchFeature
            c.multi = sup.erpFeature(6);                                                // HasMultiCurrencyFeature
            Map<String, String> m = new HashMap<>();
            for (Map<String, Object> r : sup.rows("Sp_ConfigrationsAllocation_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                    "ConfigDescriptions", "TaxPercentEditable,DebitAmountChargetoExpenseAcFreightGrid,SaleInvoiceDirectBranchWise,DefaultDaysToLessFromHistoryFromDate,FreightOutwardAc",
                    "Activity", "GetMultipleConfigurationsByConfigDescriptions"))
                m.put(str(ci(r, "ConfigDescription")), str(ci(r, "ConfigKey")));
            c.taxEditable = toBool(m.get("TaxPercentEditable"));
            c.freightToExp = toBool(m.get("DebitAmountChargetoExpenseAcFreightGrid"));
            c.branchImplemented = toBool(m.get("SaleInvoiceDirectBranchWise"));
            c.defaultDays = toInt(m.get("DefaultDaysToLessFromHistoryFromDate"));
            c.outwardFreightAc = toInt(m.get("FreightOutwardAc"));
            int amt = toInt(sup.config("Default NoofDecimal Points For Amount"));
            int rate = toInt(sup.config("Default NoofDecimal Points For Rate"));
            int fcy = toInt(sup.config("DefaultNoOfDecimalPointsForFcyAmount"));
            c.amtRound = Math.max(0, Math.min(10, amt));
            c.amtPlaces = amt >= 1 && amt <= 4 ? amt : 0;
            c.ratePlaces = rate >= 1 && rate <= 4 ? rate : (rate == 0 ? 2 : 0);
            c.fcyPlaces = fcy >= 1 && fcy <= 4 ? fcy : 0;
            return c;
        });
    }

    // ------------------------------------------------------------------ Load

    public Map<String, Object> initial() {
        Cfg c = cfg();
        Map<String, Object> m = new LinkedHashMap<>(lists());
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());                                         // DocumentNoDBCall
        m.put("branchSrNo", nextBranchSrNo());                             // BranchSrNoDBCall
        m.put("lastDiscountAcId", lastDiscountAcId());                     // PreviousDiscountAcIdWithDbCall
        m.put("lastSalesTaxAcId", lastSalesTaxAcId());                     // PreviousSalesTaxAcWithDbCall
        m.put("branchId", sup.branch());
        m.put("branchName", repo.branchName(sup.user()));
        m.put("showBranchSrNo", c.branchFeature && c.branchImplemented);
        m.put("outwardFreightAc", c.outwardFreightAc);
        m.put("defaultDays", c.defaultDays);
        m.put("historyBranches", historyBranches());                       // HistoryCombosBranchFill
        m.put("fyStart", fyStart());
        return m;
    }

    /** btnFrmRefresh_Click: SupplierDBCall, FreightAccount, TaxAccount, PaymentTerm, DiscountAccount, Currency, LookUps, AllTaxType, City, OtherItems. */
    public Map<String, Object> refresh() {
        cache.clear();
        return new LinkedHashMap<>(lists());
    }

    private Map<String, Object> lists() {
        Cfg c = cfg();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("customers", customers(c));
        m.put("terms", terms());
        m.put("deliveryTerms", List.of(row("Id", 1, "DeliveryTerm", "Load"), row("Id", 2, "DeliveryTerm", "Ponch")));
        m.put("currencies", sup.rows("Sp_MultiCurrency_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadAll"));
        m.put("otherItems", otherItems());
        m.put("freightAccounts", freightAccounts(c));
        m.put("taxAccounts", accountTitles("3,6,8,16,17,18,19", null));
        m.put("glAccounts", accountTitles("3,6,8,16,17,18,19", null));      // AccountlstDBCall: the same tax-type accounts feed grdGLedger
        m.put("discountAccounts", discountAccounts());
        m.put("taxTypes", taxTypes());
        m.put("cities", cities());
        m.put("warehouses", warehouses());
        m.put("categories", categories());
        m.put("castingTypes", castingTypes());
        m.put("productionStages", productionStages());
        m.put("items", allItems());
        m.put("sampleModals", sampleModals());
        m.put("multiCurrency", c.multi);
        m.put("subsidiary", c.sub);
        m.put("freightDebitToExpenses", c.freightToExp);
        m.put("taxEditable", c.taxEditable);
        m.put("defaults", row("baseCurrency", sup.configInt("Base Currency"), "baseRate", toDouble(sup.config("BaseCurrencyRate"))));
        m.put("fmt", row("amountRound", c.amtRound, "amount", c.amtPlaces, "rate", c.ratePlaces, "fcy", c.fcyPlaces));
        return m;
    }

    /** SupplierDBCall: with feature 4 GetVendorsAndCustomersWithCityName(2), otherwise AllSupplierCustomerWithCityName (the whole party master). */
    private List<Map<String, Object>> customers(Cfg c) {
        List<Map<String, Object>> out = new ArrayList<>();
        List<Map<String, Object>> src = c.sub
                ? sup.rows("USP_GetVendorsAndCustomersWithCityName", "OrganizationId", sup.org(), "CompanyId", sup.company(), "PartyTypeId", 2)
                : sup.rows("USP_GetVendorsAndCustomersWithCityName", "OrganizationId", sup.org(), "CompanyId", sup.company());
        for (Map<String, Object> r : src) {
            Object mobile = ci(r, "MobileNo") != null ? ci(r, "MobileNo") : ci(r, "MobilePersonal");
            out.add(row("Id", toInt(ci(r, "Id")), "CompanyName", str(ci(r, "CompanyName")), "PartyCode", str(ci(r, "PartyCode")), "GlAccountId", toInt(ci(r, "GlAccountId")),
                    "CityId", toInt(ci(r, "CityId")), "CityName", str(ci(r, "CityName")), "MobileNo", str(mobile)));
        }
        return out;
    }

    /** CommonServices.GetDueTermServiceBind. */
    public List<Map<String, Object>> terms() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.paymentTerms(sup.user())) out.add(row("Id", toInt(ci(r, "Id")), "TermsDescription", str(ci(r, "TermsDescription"))));
        return out;
    }

    /** InventoryItemsOther.GetAll. */
    public List<Map<String, Object>> otherItems() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.otherItems(sup.user())) out.add(row("Id", toInt(ci(r, "Id")), "OtherItemName", str(ci(r, "OtherItemName"))));
        return out;
    }

    /** CommonServices.CoaAllocationAccountTitleByAccountTypeIds(ids, idsNot). */
    private List<Map<String, Object>> accountTitles(String ids, String not) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.accountTitlesByTypes(sup.user(), ids, not)) out.add(row("Id", toInt(ci(r, "Id")), "AccountTitle", str(ci(r, "AccountTitle"))));
        return out;
    }

    /** FreightAccountDebitAndCreditDBCall: dtTransporter {Id (GL), AccountTitle, AccountCode, SupplierCustomerId}. */
    private List<Map<String, Object>> freightAccounts(Cfg c) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (c.sub) {
            for (Map<String, Object> r : sup.rows("USP_GetVendorsAndCustomersForTransporter", "OrganizationId", sup.org(), "CompanyId", sup.company()))
                out.add(row("Id", toInt(ci(r, "GlAccountId")), "SupplierCustomerId", toInt(ci(r, "Id")), "AccountTitle", str(ci(r, "CompanyName"))));
        } else {
            // the desktop's string, double comma included
            for (Map<String, Object> r : accountTitles(null, c.freightToExp ? "2,15,22" : "2,11,12,,13,14,15,20,21,22"))
                out.add(row("Id", r.get("Id"), "SupplierCustomerId", 0, "AccountTitle", r.get("AccountTitle")));
        }
        return out;
    }

    /** CommonServices.GetAccountTitleByAccountClassIds("4"). */
    private List<Map<String, Object>> discountAccounts() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_COAAllocation_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "AccountClassIds", "4",
                "UserId", idOrNull(sup.userId()), "Activity", "GetAccountTitleByAccountClassIds"))
            out.add(row("Id", toInt(ci(r, "Id")), "AccountTitle", str(ci(r, "AccountTitle"))));
        return out;
    }

    /** AllTaxTypeDBCall: ItemTaxSchedule.GetAllTaxSchedule(org, company, DocDate, 2). */
    private List<Map<String, Object>> taxTypes() { return taxTypes(null); }

    /** AllTaxTypeDBCall: ItemTaxSchedule.GetAllTaxSchedule(.., DocDate.Value, 2) - the tax types effective on the document date. */
    public List<Map<String, Object>> taxTypes(String docDate) {
        LocalDate eff = parseDate(docDate);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_ItemTaxSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "EffectedDate", eff == null ? LocalDate.now() : eff,
                "TaxTypeId", 2, "Activity", "GetAllTaxSchedule"))
            out.add(row("TaxNameId", toInt(ci(r, "TaxNameId")), "TaxName", str(ci(r, "TaxName")), "TaxPercent", toDouble(ci(r, "TaxPercent"))));
        return out;
    }

    /** CityDBCall: CommonServices.CityGetAllService = SP_City_GetAllMethod 'GetAll' (Id, CityName). */
    private List<Map<String, Object>> cities() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("SP_City_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll")) {
            Object name = ci(r, "CityName") != null ? ci(r, "CityName") : (ci(r, "Description") != null ? ci(r, "Description") : ci(r, "City"));
            out.add(row("Id", toInt(ci(r, "Id")), "CityName", str(name)));
        }
        return out;
    }

    /** DocumentNoDBCall: InvSaleInvoice.GenerateInvSaleInvoiceCode (document type 1661). */
    public int nextNo() { return repo.nextDocNo(sup.user(), sup.fy(), DOC_TYPE); }

    /** BranchSrNoDBCall: InvSaleInvoice.GenerateInvSaleInvoiceCodeBranch ('GenerateBranchCode', reading BranchSrNo). */
    public int nextBranchSrNo() {
        List<Map<String, Object>> r = sup.rows("Sp_InvSaleInvoice_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "BranchesId", idOrNull(sup.branch()),
                "DocumentTypeId", DOC_TYPE, "FinancialYearId", sup.fy(), "Activity", "GenerateBranchCode");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "BranchSrNo"));
    }

    /** PreviousDiscountAcIdWithDbCall: InvSaleInvoice.GetLastDiscountAcId. */
    public int lastDiscountAcId() {
        List<Map<String, Object>> r = sup.rows("Sp_InvSaleInvoice_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetLastDiscountAcId");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "DiscountAccountId"));
    }

    /** PreviousSalesTaxAcWithDbCall: InvSaleInvoice.GetLastSaleTaxAcId. */
    public int lastSalesTaxAcId() {
        List<Map<String, Object>> r = sup.rows("Sp_InvSaleInvoice_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetLastSaleTaxAcId");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "SaleTaxAcId"));
    }

    /** cmbCurrency_Leave: VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher (document type 1661). */
    public List<Map<String, Object>> lastRate(int currencyId) {
        return sup.rows("Sp_Vouchers_GetMethods", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeIds", String.valueOf(DOC_TYPE),
                "DMultiCurrencyIds", String.valueOf(currencyId), "Activity", "GetMultiCurrencyAndLastRate");
    }

    /** grdPaymentTerm_CellUpdated (PaymentTermId 3): SaleOrder.GetSaleOrderRemningAmountByOrderId (SaleOrderId = the order the rows were loaded from). */
    public Map<String, Object> remaining(int orderId, int recId) {
        List<Map<String, Object>> r = sup.rows("USP_GetSaleOrderRemningAmountByOrderId", "OrganizationId", sup.org(), "CompanyId", sup.company(), "OrderId", orderId, "RecId", idOrNull(recId));
        return row("balanceAmount", r.isEmpty() ? 0 : toDouble(ci(r.get(0), "BalanceAmount")));
    }

    /** cmbsuppliername_ValueChanged: ledger balance as of the document date (feature 4: subsidiary ledger). */
    public Map<String, Object> ledgerBalance(int customerId, String docDate) {
        Cfg c = cfg();
        int gl = 0;
        for (Map<String, Object> x : customers(c)) if (toInt(x.get("Id")) == customerId) gl = toInt(x.get("GlAccountId"));
        LocalDate d = parseDate(docDate);
        if (customerId <= 0 || d == null) return row("balance", 0, "glAccountId", gl);
        LocalDate start = parseDate(fyStart());
        double bal = repo.ledgerBalance(sup.user(), c.sub, customerId, gl, (start == null ? d : start).atStartOfDay(), d.atStartOfDay());
        return row("balance", bal, "glAccountId", gl);
    }

    /** DocDate_Leave: CommonServices.GetTaxScheduleDetailbyItemIds(ItemIds, DocDate). */
    public List<Map<String, Object>> taxByDate(String itemIds, String docDate) {
        LocalDate d = parseDate(docDate);
        List<Map<String, Object>> out = new ArrayList<>();
        if (d == null || itemIds == null || itemIds.isBlank()) return out;
        for (Map<String, Object> r : sup.rows("Sp_ItemTaxSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemIds", itemIds,
                "EffectedDate", d, "Activity", "GetTaxScheduleDetailbyItemIds"))
            out.add(row("ItemId", toInt(ci(r, "ItemId")), "TaxNameId", toInt(ci(r, "TaxNameId")), "TaxPercent", toDouble(ci(r, "TaxPercent"))));
        return out;
    }

    /** Start of the active financial year (clsGlobalVariables.ActiveYr.Start_Period). */
    public String fyStart() {
        for (Map<String, Object> r : sup.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            if (toInt(r.get("Id")) == sup.fy()) return str(r.get("Start_Period"));
        return "";
    }

    // ------------------------------------------------------------------ Load Sale Order dialog (frmLoadSaleOrderEngr)

    /** BranchFill + CustomerFill: the user's own branch (BranchFeature && SaleOrderBranchWise), else GetBranchsAllocatedToUser; customers of USP_GetDataForDropDownFromSaleOrder. */
    public Map<String, Object> loaderInit() {
        Cfg c = cfg();
        boolean branchWise = sup.configBool("SaleOrderBranchWise");
        List<Map<String, Object>> branches = new ArrayList<>();
        if (c.branchFeature && branchWise) branches.add(row("Id", sup.branch(), "BranchName", repo.branchName(sup.user())));
        else for (Map<String, Object> r : sup.rows("USP_GetBranchsAllocatedToUser", "OrganizationId", sup.org(), "CompanyId", sup.company(), "UserId", sup.user().getId()))
            branches.add(row("Id", toInt(ci(r, "BranchId")), "BranchName", str(ci(r, "BranchName"))));
        String ids = joinIds(branches, "Id");
        List<Map<String, Object>> cust = loaderCustomers(c.branchFeature && branchWise ? String.valueOf(sup.branch()) : ids);
        return row("branches", branches, "customers", cust, "fyStart", fyStart(), "branchFeature", c.branchFeature, "branchImplemented", branchWise);
    }

    /** CustomerFill: SaleOrder.GetDataForDropDownFromSaleOrder (DocumentTypeIds 1656, Activity Customer). */
    public List<Map<String, Object>> loaderCustomers(String branchIds) {
        List<Map<String, Object>> cust = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_GetDataForDropDownFromSaleOrder", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeIds", ORDER_TYPE,
                "Activity", "Customer", "BranchesIds", branchIds == null || branchIds.isEmpty() ? null : branchIds, "AppId", sup.ctx().currentAppId(), "UserId", sup.user().getId()))
            cust.add(row("Id", toInt(ci(r, "Id")), "ReferenceName", str(ci(r, "ReferenceName"))));
        return cust;
    }

    private static String joinIds(List<Map<String, Object>> rows, String key) {
        StringBuilder b = new StringBuilder();
        for (Map<String, Object> r : rows) b.append(',').append(toInt(r.get(key)));
        return b.toString();
    }

    /** SaleOrder.SaleOrder_PendingForInvoice_Engr -> USP_SaleOrder_PendingForInvoice_Engr (DocumentTypeIds 1656, ZeroBalanceType 1 = @SkipZero, ApprovedFilter "All" = no @IsApproved). */
    private List<Map<String, Object>> pending(String branchIds, LocalDate from, LocalDate to, int fromNo, int toNo, int customerId, int orderId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("FinancialYearId", sup.fy());
        p.put("DocumentTypeIds", ORDER_TYPE);
        if (branchIds != null && !branchIds.isEmpty()) p.put("BranchesIds", branchIds);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (fromNo != 0) p.put("FromDocNo", fromNo);
        if (toNo != 0) p.put("ToDocNo", toNo);
        if (orderId > 0) p.put("Id", orderId);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        p.put("SkipZero", 1);
        return com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "USP_SaleOrder_PendingForInvoice_Engr", p);
    }

    /** PendingOrderLoad: the dialog's grid (desktop column names, one row per pending order line). */
    public List<Map<String, Object>> loaderRows(String branchIds, String fromDate, String toDate, int fromNo, int toNo, int customerId) {
        if (branchIds == null || branchIds.replace(",", "").trim().isEmpty()) throw new Warning("Select branch first");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : pending(branchIds, parseDate(fromDate), parseDate(toDate), fromNo, toNo, customerId, 0))
            out.add(row("Id", toInt(ci(r, "Id")), "OrderDetailId", toInt(ci(r, "PoDId")), "DocDate", ci(r, "DocDate"), "DocNo", toInt(ci(r, "DocNo")),
                    "SupplierCustomerId", toInt(ci(r, "OrderSupCustId")), "CustomerName", str(ci(r, "SupplierName")), "CustomerRefNo", str(ci(r, "SupplierRefNo")),
                    "PaymentTerm", str(ci(r, "TermsDescription")), "DueDays", toInt(ci(r, "OrderDueDays")), "OrderDueDate", ci(r, "OrderDueDate"),
                    "DeliveryTerm", str(ci(r, "DeliveryTerm")), "RemarksHeader", str(ci(r, "RemarksHeader")), "ItemId", toInt(ci(r, "OrderItemId")),
                    "ItemCode", str(ci(r, "ItemCode")), "ItemName", str(ci(r, "ItemName")), "VariantDescription", str(ci(r, "VarientDescription")),
                    "PackUom", str(ci(r, "PackUom")), "ItemQty", toDouble(ci(r, "OrderItemQty")), "DispatchQty", toDouble(ci(r, "DispatchQty")), "BalQty", toDouble(ci(r, "BalQty")),
                    "ItemRate", toDouble(ci(r, "OrderItemRate")), "RateUom", str(ci(r, "RateUom")), "DiscountPrcnt", toDouble(ci(r, "discountPercent")),
                    "DiscountAmount", toDouble(ci(r, "DiscountAmount")), "ItemAmount", toDouble(ci(r, "ItemAmount")), "TaxType", str(ci(r, "TaxName")),
                    "TaxPrcnt", toDouble(ci(r, "TaxPercent")), "TaxAmount", toDouble(ci(r, "TaxAmount")), "ItemNetAmount", toDouble(ci(r, "ItemNetAmount")),
                    "DetailRemarks", str(ci(r, "DetailRemarks")), "DeliveryCity", str(ci(r, "CityName"))));
        return out;
    }

    /** The pending rows of the checked orders (the proc is filtered by tenant and financial year). */
    private List<Map<String, Object>> pendingRowsOf(Collection<Integer> orderIds) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Integer o : orderIds) out.addAll(pending(null, null, null, 0, 0, 0, o));
        return out;
    }

    private static List<Integer> idList(String ids) {
        List<Integer> out = new ArrayList<>();
        if (ids != null) for (String s : ids.split(",")) { int n = toInt(s.trim()); if (n > 0 && !out.contains(n)) out.add(n); }
        return out;
    }

    /**
     * btnLoadOnInvoice_Click_1 + LoadInGridDetail + LoadPaymentDetailBySaleOrderIds: the header fields of the first checked row, one grid row per
     * CHECKED pending order line (dtLoader) and the order's payment terms. The "different order" rule keeps the desktop's wording.
     */
    public Map<String, Object> loadOrders(int orderId, String detailIds, int loadedOrderId) {
        List<Integer> ids = idList(detailIds);
        if (orderId <= 0 || ids.isEmpty()) throw new Warning("Chek the row first");
        List<Map<String, Object>> g = new ArrayList<>();
        for (Map<String, Object> r : pending(null, null, null, 0, 0, 0, orderId)) if (ids.contains(toInt(ci(r, "PoDId")))) g.add(r);
        Map<String, Object> out = new LinkedHashMap<>();
        if (g.isEmpty()) { out.put("lines", List.of()); return out; }
        Map<String, Object> first = g.get(0);
        if (loadedOrderId > 0 && loadedOrderId != toInt(ci(first, "Id")))
            throw new Warning("Already Loaded Row's Have Different Order. So you Can't Load Rows Of Different Order!");
        out.put("head", row("SupplierCustomerId", toInt(ci(first, "OrderSupCustId")), "SupplierReferenceNo", str(ci(first, "SupplierRefNo")),
                "TermsDescription", str(ci(first, "TermsDescription")), "DueDays", toInt(ci(first, "OrderDueDays")), "DeliveryTerm", str(ci(first, "DeliveryTerm")),
                "RemarksHeader", str(ci(first, "RemarksHeader")), "SaleOrderId", toInt(ci(first, "Id"))));
        List<Map<String, Object>> lines = new ArrayList<>();
        for (Map<String, Object> r : g) lines.add(orderLine(r));
        out.put("lines", lines);
        List<Map<String, Object>> pay = new ArrayList<>();
        for (Map<String, Object> r : repo.saleOrderPaymentTerms(String.valueOf(toInt(ci(first, "Id")))))
            pay.add(row("PaymentTermId", toInt(ci(r, "PaymentTermId")), "%ofTotal", toDouble(ci(r, "PrcntOfTotal")), "Amount", toDouble(ci(r, "Amount")),
                    "DueDays", toInt(ci(r, "DueDays")), "DueDate", ci(r, "DueDate"), "Remarks", str(ci(r, "PaymentRemarks"))));
        out.put("payments", pay);
        return out;
    }

    /** dtGrid.Rows.Add(...) of LoadInGridDetail from a USP_SaleOrder_PendingForInvoice_Engr row. */
    private Map<String, Object> orderLine(Map<String, Object> g) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", 0); m.put("OrderId", toInt(ci(g, "Id"))); m.put("OrderDetailId", toInt(ci(g, "PoDId"))); m.put("OrderNo", toInt(ci(g, "DocNo")));
        m.put("OrderDate", shortDate(ci(g, "DocDate")));
        m.put("WarehouseId", 0); m.put("WarehouseName", "");
        m.put("ItemId", toInt(ci(g, "OrderItemId"))); m.put("ItemCode", str(ci(g, "ItemCode"))); m.put("ItemName", str(ci(g, "ItemName")));
        m.put("PackUomId", toInt(ci(g, "PackUomId"))); m.put("PackUom", str(ci(g, "PackUom"))); m.put("PackEquivalent", toDouble(ci(g, "PackEquivalent")));
        m.put("VariantId", toInt(ci(g, "ItemVariantId"))); m.put("VariantDescription", str(ci(g, "VarientDescription")));
        m.put("CastingTypeId", toInt(ci(g, "CastingTypeId"))); m.put("CastingType", "");
        m.put("ProductionStageId", 0); m.put("ProductionStage", "");
        m.put("Remarks", str(ci(g, "DetailRemarks")));
        m.put("ItemQty", toDouble(ci(g, "BalQty"))); m.put("NetWeight", toDouble(ci(g, "WeightFinishGoods")));
        m.put("ItemRate", toDouble(ci(g, "OrderItemRate"))); m.put("RateUomId", toInt(ci(g, "RateUomId"))); m.put("RateUom", str(ci(g, "RateUom")));
        m.put("RateEquivalent", toDouble(ci(g, "EquivalentRate"))); m.put("AddLessRate", 0.0); m.put("NetRate", toDouble(ci(g, "OrderItemRate")));
        m.put("DiscountType", toInt(ci(g, "DiscountTypeId"))); m.put("DiscPct", toDouble(ci(g, "discountPercent"))); m.put("DiscountAmount", toDouble(ci(g, "DiscountAmount")));
        m.put("ItemAmount", toDouble(ci(g, "ItemAmount"))); m.put("FcyAmount", 0.0);
        m.put("TaxTypeId", toInt(ci(g, "TaxNameId"))); m.put("TaxPct", toDouble(ci(g, "TaxPercent"))); m.put("TaxAmount", toDouble(ci(g, "TaxAmount")));
        m.put("Expense", 0.0); m.put("Freights", 0.0); m.put("BillAmount", 0.0);
        m.put("GpDate", java.time.LocalDate.now().toString()); m.put("GpNo", "0"); m.put("VehicleNo", "0");
        m.put("CityId", toInt(ci(g, "CityId")));
        return m;
    }

    /** ReadById's dtGrid.Rows.Add(...) from an InvSaleInvoiceDetail row. */
    private Map<String, Object> storedLine(Map<String, Object> d) {
        double rate = toDouble(ci(d, "ItemRate")), cut = toDouble(ci(d, "RateCut"));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", toInt(ci(d, "Id"))); m.put("OrderId", toInt(ci(d, "SaleOrderId"))); m.put("OrderDetailId", toInt(ci(d, "SaleOrderDetailId")));
        m.put("OrderNo", toInt(ci(d, "SaleOrder"))); m.put("OrderDate", shortDate(ci(d, "OrderDate")));
        m.put("WarehouseId", toInt(ci(d, "WarehouseId"))); m.put("WarehouseName", str(ci(d, "WareHouseName")));
        m.put("ItemId", toInt(ci(d, "ItemId"))); m.put("ItemCode", str(ci(d, "ItemCode"))); m.put("ItemName", str(ci(d, "ItemName")));
        m.put("PackUomId", toInt(ci(d, "ItemUOMId"))); m.put("PackUom", str(ci(d, "UOMCodeItem"))); m.put("PackEquivalent", toDouble(ci(d, "PackEquivalent")));
        m.put("VariantId", toInt(ci(d, "ItemVariantId"))); m.put("VariantDescription", str(ci(d, "VariantDescription")));
        m.put("CastingTypeId", toInt(ci(d, "CastingTypeId"))); m.put("CastingType", str(ci(d, "CastingType")));
        m.put("ProductionStageId", toInt(ci(d, "ProductionStageId"))); m.put("ProductionStage", str(ci(d, "ProductionProcessStage")));
        m.put("Remarks", str(ci(d, "RemarksDetail")));
        m.put("ItemQty", toDouble(ci(d, "ItemQty"))); m.put("NetWeight", toDouble(ci(d, "WeightFinishGoods")));
        m.put("ItemRate", rate - cut); m.put("RateUomId", toInt(ci(d, "UomScheduleIdRate"))); m.put("RateUom", str(ci(d, "UOMCodeRate")));
        m.put("RateEquivalent", toDouble(ci(d, "RateUOM"))); m.put("AddLessRate", cut); m.put("NetRate", rate);
        m.put("DiscountType", toInt(ci(d, "DiscountTypeId"))); m.put("DiscPct", toDouble(ci(d, "ItemDiscount"))); m.put("DiscountAmount", toDouble(ci(d, "ItemDiscountAmount")));
        m.put("ItemAmount", toDouble(ci(d, "ItemAmount"))); m.put("FcyAmount", toDouble(ci(d, "FcyAmount")));
        m.put("TaxTypeId", toInt(ci(d, "TaxNameId"))); m.put("TaxPct", toDouble(ci(d, "TaxPercent"))); m.put("TaxAmount", toDouble(ci(d, "TaxAmount")));
        m.put("Expense", toDouble(ci(d, "ExpenseAmount"))); m.put("Freights", toDouble(ci(d, "FreightAmount"))); m.put("BillAmount", toDouble(ci(d, "BillAmount")));
        m.put("GpDate", shortDate(ci(d, "GpDate"))); m.put("GpNo", str(ci(d, "GpNo"))); m.put("VehicleNo", str(ci(d, "VehicleNo")));
        m.put("CityId", toInt(ci(d, "CityId")));
        return m;
    }

    // ------------------------------------------------------------------ item entry panel (DetailPanel)

    /** ItemDtFillFromGlobal: clsGlobalVariables.getGlobalAllItems = Sp_Item_GetAllMethod 'AllItemsBindForEngr' (Id, ItemName, ItemCode, ItemCategoryId, itemModalId, FinishWeight). */
    private List<Map<String, Object>> allItems() {
        return cached("items", () -> {
            List<Map<String, Object>> out = new ArrayList<>();
            for (Map<String, Object> r : sup.rows("Sp_Item_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "AllItemsBindForEngr")) {
                Object code = ci(r, "ItemCode") != null ? ci(r, "ItemCode") : ci(r, "ItemCodeNew");
                out.add(row("Id", toInt(ci(r, "Id")), "ItemName", str(ci(r, "ItemName")), "ItemCode", str(code), "ItemCategoryId", toInt(ci(r, "ItemCategoryId")),
                        "itemModalId", toInt(ci(r, "itemModalId")), "FinishWeight", toDouble(ci(r, "FinishWeight"))));
            }
            return out;
        });
    }

    /** SampleModaldBCall: ItemModel.ItemModel_WithItemId -> [item].[USP_ItemModel_WithItemId] (Id = itemModalId, SampleNo, ItemModal, ItemCategoryId, ItemId). */
    private List<Map<String, Object>> sampleModals() {
        return cached("modals", () -> {
            List<Map<String, Object>> out = new ArrayList<>();
            for (Map<String, Object> r : sup.rows("[item].[USP_ItemModel_WithItemId]", "OrganizationId", sup.org(), "CompanyId", sup.company()))
                out.add(row("Id", toInt(ci(r, "itemModalId")), "SampleNo", str(ci(r, "ItemSampleNo")), "ItemModal", str(ci(r, "ItemModal")),
                        "ItemCategoryId", toInt(ci(r, "ItemCategoryId")), "ItemId", toInt(ci(r, "ItemId"))));
            return out;
        });
    }

    /** AvailableStockGetByItem: InventoryStockEvalautionDetail.StockEvaluation_GetCurrStockByItem_Engr -> [Mfg].[USP_StockEvaluation_GetCurrStockByItem] (AvailableStock; lblBalance shows 0 when it is not positive). */
    public Map<String, Object> stock(String docDate, int warehouseId, int itemId, int variantId, int productionStageId, int uomId, int castingTypeId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("ItemId", itemId);
        LocalDate dd = parseDate(docDate);
        if (dd != null) p.put("DocDate", dd);
        p.put("WarehouseId", warehouseId); p.put("VariantId", variantId); p.put("ProductionStageId", productionStageId);
        p.put("ItemUomId", uomId); p.put("CastingTypeId", castingTypeId);
        double v = 0.0;
        List<Map<String, Object>> rows = com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "[Mfg].[USP_StockEvaluation_GetCurrStockByItem]", p);
        if (!rows.isEmpty()) v = toDouble(ci(rows.get(0), "AvailableStock"));
        return row("stock", v > 0.0 ? v : 0.0);
    }

    /** BindPackUomAndRateUom: CommonServices.dtUomFromGloablUomScheduleByItemId -> Sp_UOMSchedule_GetAllMethod 'ReadByItemID'. */
    public List<Map<String, Object>> uoms(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (itemId <= 0) return out;
        for (Map<String, Object> r : sup.rows("Sp_UOMSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "ReadByItemID"))
            out.add(row("Id", toInt(ci(r, "Id")), "UOMCode", str(ci(r, "UOMCode")), "Equivalent", toDouble(ci(r, "Equivalent"))));
        return out;
    }

    /** bindvarientunit: ItemAttributeVarient.GetAllForCombo -> USP_ItemAttributeVarient_GetAllMethod 'GetAllForCombo'. */
    public List<Map<String, Object>> variants(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (itemId <= 0) return out;
        for (Map<String, Object> r : sup.rows("USP_ItemAttributeVarient_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "GetAllForCombo"))
            out.add(row("VariantId", toInt(ci(r, "ItemAttributeVarientId")), "VariantDescription", str(ci(r, "VarientDescription"))));
        return out;
    }

    private List<Map<String, Object>> warehouses() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_InvWareHouse_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetActiveWareHouse"))
            out.add(row("Id", toInt(ci(r, "Id")), "WareHouseName", str(ci(r, "WareHouseName"))));
        return out;
    }

    private List<Map<String, Object>> categories() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_ItemCategory_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadByOrganizationCompanyId"))
            out.add(row("Id", toInt(ci(r, "Id")), "CategoryDescription", str(ci(r, "CategoryDescription"))));
        return out;
    }

    /** CastingTypeDtFillFromGlobal: the Engr lookups of type 1. */
    private List<Map<String, Object>> castingTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("[Mfg].[USP_Lookups_GetAllMethod]", "Activity", "GetDataByTypeId", "Id", 1))
            out.add(row("Id", toInt(ci(r, "Id")), "LookupName", str(ci(r, "LookupName"))));
        return out;
    }

    /** CastingTypeDtFillFromGlobal: the Engr lookups of type 3 (dtProductionStage). */
    private List<Map<String, Object>> productionStages() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("[Mfg].[USP_Lookups_GetAllMethod]", "Activity", "GetDataByTypeId", "Id", 3))
            out.add(row("Id", toInt(ci(r, "Id")), "ProductionStage", str(ci(r, "LookupName"))));
        return out;
    }

    private static String shortDate(Object o) {
        LocalDate d = parseDate(o);
        return d == null ? "" : d.toString();
    }

    // ------------------------------------------------------------------ History

    /** HistoryCombosBranchFill: only the user's branch when SaleInvoiceDirectBranchWise, else USP_GetBranchsAllocatedToUserFromSaleInvoice(…, 1660). */
    public List<Map<String, Object>> historyBranches() {
        Cfg c = cfg();
        UserAccount u = sup.user();
        List<Map<String, Object>> out = new ArrayList<>();
        if (c.branchImplemented) { out.add(row("Id", sup.branch(), "BranchName", repo.branchName(u))); return out; }
        for (Map<String, Object> r : sup.rows("USP_GetBranchsAllocatedToUserFromSaleInvoice", "OrganizationId", sup.org(), "CompanyId", sup.company(), "UserId", u.getId(), "DocumentTypeId", DOC_TYPE))
            out.add(row("Id", toInt(ci(r, "BranchId")), "BranchName", str(ci(r, "BranchName"))));
        return out;
    }

    /** HistoryComboFill: Usp_AllComboAgainstSaleInvoice, the 'Supplier' and 'PaymentTerm' rows. */
    public Map<String, Object> historyCombos(String branchIds) {
        UserAccount u = sup.user();
        String ids = branchIds == null ? "" : branchIds.trim();
        if (ids.isEmpty()) throw new Warning("Select branch first");
        List<Map<String, Object>> cust = new ArrayList<>(), pay = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Usp_AllComboAgainstSaleInvoice", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "AppId", sup.ctx().currentAppId(), "UserId", u.getId(), "DocumentTypeIds", String.valueOf(DOC_TYPE), "BranchesIds", ids)) {
            String a = str(ci(r, "Activity"));
            if ("Supplier".equals(a)) cust.add(row("Id", toInt(ci(r, "Id")), "Customer", str(ci(r, "ReferenceName"))));
            else if ("PaymentTerm".equals(a)) pay.add(row("Id", toInt(ci(r, "Id")), "PaymentTerm", str(ci(r, "ReferenceName"))));
        }
        return row("customers", cust, "paymentTerms", pay);
    }

    /** GetAll: InvSaleInvoice.FormHistory. */
    public List<Map<String, Object>> history(String branchIds, String dateType, String fromDate, String toDate, int fromNo, int toNo, int customerId, int paymentTermId) {
        if (branchIds == null || branchIds.trim().isEmpty()) throw new Warning("Select branch first");
        UserAccount u = sup.user();
        boolean all = Boolean.TRUE.equals(sup.rights(SCREEN).get("viewAll"));
        LocalDate f = parseDate(fromDate), t = parseDate(toDate);
        String mode = dateType == null ? "document" : dateType;
        String fk = "FromDate", tk = "ToDate";
        if ("entry".equals(mode)) { fk = "EntryFromDate"; tk = "EntryToDate"; }
        else if ("modify".equals(mode)) { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
        else if ("approved".equals(mode)) { fk = "ApprovedFromDate"; tk = "ApprovedToDate"; }
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("DocumentTypeId", DOC_TYPE); p.put("CanViewAllRecord", all);
        p.put("FinancialYearId", sup.fy()); p.put("EntryUser", u.getId());
        if (f != null) p.put(fk, f);
        if (t != null) p.put(tk, t);
        if (fromNo != 0) p.put("DocNoFrom", fromNo);
        if (toNo != 0) p.put("DocNoTo", toNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (paymentTermId != 0) p.put("PaymentTermId", paymentTermId);
        p.put("BranchesIds", branchIds);
        p.put("Activity", "FormHistory");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "Sp_InvSaleInvoice_GetAllMethod", p))
            out.add(row("Id", toInt(ci(r, "Id")), "VoucherHeadId", toInt(ci(r, "VoucherHeadId")), "DocNo", toInt(ci(r, "DocNo")), "DocDate", shortDate(ci(r, "DocDate")),
                    "DueDate", shortDate(ci(r, "DueDate")), "ManualBillNo", str(ci(r, "ManualBillNo")), "SupplierCustomerId", toInt(ci(r, "SupplierCustomerId")),
                    "CustomerName", str(ci(r, "CustomerName")), "BillAmount", toDouble(ci(r, "BillAmount")), "EntryUser", str(ci(r, "UserName")), "EntryDate", ci(r, "EntryDate"),
                    "ModifyUser", str(ci(r, "ModifyUserName")), "ModifyDate", ci(r, "ModifyDate"), "ApprovedUser", str(ci(r, "ApprovedUserName")), "ApprovedDate", ci(r, "ApprovedDate"),
                    "NoOfAttachments", toInt(ci(r, "NoOfAttachments")), "Remarks", str(ci(r, "RemarksHeader"))));
        return out;
    }

    // ------------------------------------------------------------------ ReadById (InvSaleInvoice.GetByID)

    private Map<String, Object> invoice(int id) {
        Map<String, Object> inv = repo.readById(id);
        UserAccount u = sup.user();
        if (inv == null || (inv.containsKey("OrganizationId") && toInt(ci(inv, "OrganizationId")) != u.getOrganizationId())
                || (inv.containsKey("CompanyId") && toInt(ci(inv, "CompanyId")) != u.getCompanyId())
                || toInt(ci(inv, "DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Sale invoice not found in this company");
        return inv;
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> children(Map<String, Object> inv, String key) {
        Object o = inv.get(key);
        return o instanceof List ? (List<Map<String, Object>>) o : List.of();
    }

    public Map<String, Object> record(int id) {
        Cfg c = cfg();
        Map<String, Object> inv = invoice(id);
        Map<String, Object> head = new LinkedHashMap<>();
        for (String k : List.of("DocNo", "BranchSrNo", "DocDate", "SupplierCustomerId", "SupplierReferenceNo", "ManualBillNo", "TaxAccountId", "TransporterId",
                "TransporterCreditPartyId", "TransporterDebitGLId", "TransporterDebitPartyId", "FreightAmount", "RemarksHeader", "DeliveryTerm", "PaymentTermId", "DueDays",
                "DueDate", "BillAmount", "CurrencyId", "ExchangeRate", "DiscountAmount", "DiscountAccountId", "FcyAmount", "IsApproved", "FreightRemark"))
            head.put(k, ci(inv, k));
        head.put("Id", id);
        List<Map<String, Object>> lines = new ArrayList<>();
        int saleOrderId = 0;
        for (Map<String, Object> d : children(inv, "details")) { if (lines.isEmpty()) saleOrderId = toInt(ci(d, "SaleOrderId")); lines.add(storedLine(d)); }
        List<Map<String, Object>> exp = new ArrayList<>();
        for (Map<String, Object> e : children(inv, "expenses"))
            exp.add(row("ItemId", toInt(ci(e, "InvRevExpItemId")), "Qty", toDouble(ci(e, "Qty")), "Rate", toDouble(ci(e, "Rate")), "Amount", toDouble(ci(e, "Amount")), "Remarks", str(ci(e, "Remarks"))));
        List<Map<String, Object>> jl = new ArrayList<>();
        for (Map<String, Object> j : children(inv, "journals"))
            jl.add(row("AccountId", toInt(ci(j, "ChartofAccountId")), "GlAccountId", toInt(ci(j, "ChartofAccountId")),
                    "Remarks", str(ci(j, "JvRemarks")), "Percentage", toDouble(ci(j, "JvPrcnt")), "Qty", toDouble(ci(j, "JvQty")), "Rate", toDouble(ci(j, "JvRate")),
                    "Debit", toDouble(ci(j, "JvDebit")), "Credit", toDouble(ci(j, "JvCredit"))));
        List<Map<String, Object>> pay = new ArrayList<>();
        for (Map<String, Object> p : children(inv, "paymentTerms"))
            pay.add(row("PaymentTermId", toInt(ci(p, "PaymentTermId")), "%ofTotal", toDouble(ci(p, "PrcntOfTotal")), "Amount", toDouble(ci(p, "Amount")),
                    "DueDays", toInt(ci(p, "DueDays")), "DueDate", ci(p, "DueDate"), "Remarks", str(ci(p, "PaymentRemarks"))));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", head);
        out.put("lines", lines);
        out.put("expenses", exp);
        out.put("journals", jl);
        out.put("payments", pay);
        out.put("saleOrderId", saleOrderId);
        out.put("voucherHeadId", repo.voucherHeadId(sup.org(), sup.company(), DOC_TYPE, id));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    /** BindDetailOfHeaderId: the rows of the highlighted history invoice. */
    public List<Map<String, Object>> historyDetail(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : children(invoice(id), "details")) {
            double cut = toDouble(ci(d, "RateCut"));
            out.add(row("OrderId", toInt(ci(d, "SaleOrderId")), "OrderDetailId", toInt(ci(d, "SaleOrderDetailId")), "OrderNo", toInt(ci(d, "SaleOrder")),
                    "OrderDate", shortDate(ci(d, "OrderDate")),
                    "Warehouse", str(ci(d, "WareHouseName")), "ItemCode", str(ci(d, "ItemCode")), "ItemName", str(ci(d, "ItemName")),
                    "PackUom", str(ci(d, "UOMCodeItem")), "VariantDescription", str(ci(d, "VariantDescription")), "CastingType", str(ci(d, "CastingType")),
                    "ProductionStage", str(ci(d, "ProductionProcessStage")),
                    "Remarks", str(ci(d, "RemarksDetail")), "ItemQty", toDouble(ci(d, "ItemQty")),
                    "NetWeight", toDouble(ci(d, "WeightFinishGoods")), "ItemRate", toDouble(ci(d, "ItemRate")) - cut, "RateUom", str(ci(d, "UOMCodeRate")),
                    "AddLessRate", cut, "NetRate", toDouble(ci(d, "ItemRate")), "DiscountType", str(ci(d, "DiscountType")), "DiscPct", toDouble(ci(d, "ItemDiscount")),
                    "DiscountAmount", toDouble(ci(d, "ItemDiscountAmount")), "ItemAmount", toDouble(ci(d, "ItemAmount")), "FcyAmount", toDouble(ci(d, "FcyAmount")),
                    "TaxType", str(ci(d, "TaxName")), "TaxPct", toDouble(ci(d, "TaxPercent")), "TaxAmount", toDouble(ci(d, "TaxAmount")), "Expense", toDouble(ci(d, "ExpenseAmount")),
                    "BillAmount", toDouble(ci(d, "BillAmount")), "GpDate", shortDate(ci(d, "GpDate")), "GpNo", str(ci(d, "GpNo")), "VehicleNo", str(ci(d, "VehicleNo")),
                    "CityName", str(ci(d, "CityName"))));
        }
        return out;
    }

    // ------------------------------------------------------------------ the request (common to calc and save)

    public static class Req {
        public int id;
        public String docNo;
        public String docDate;
        public int customerId;
        public String deliveryTerm;
        public int paymentTermId;
        public String dueDays;
        public String dueDate;
        public int currencyId;
        public double exchangeRate;
        public int taxAccountId;
        public int freightCrId;
        public int freightDrId;
        public double freightAmount;
        public String freightRemarks;
        public int discountAccountId;
        public double discountAmount;
        public String manualBillNo;
        public String refNo;
        public String remarks;
        public boolean taxRefresh;               // DocDate_Leave ran on this (new) invoice: re-read the tax schedule by item and date
        public List<Map<String, Object>> lines = new ArrayList<>();
        public List<Map<String, Object>> expenses = new ArrayList<>();
        public List<Map<String, Object>> journals = new ArrayList<>();
        public List<Map<String, Object>> payments = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    private static final class Totals {
        double bill, billText, fcy, itemAmount, tax, exp, qty;
    }

    private static double d(Map<String, Object> m, String k) { return toDouble(m.get(k)); }

    /** Math.Round(v, dec, MidpointRounding.AwayFromZero) */
    private static double round(double v, int dec) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return 0.0;
        return BigDecimal.valueOf(v).setScale(Math.max(0, dec), RoundingMode.HALF_UP).doubleValue();
    }

    /** Math.Round(v, dec): the default, to-even rounding. */
    private static double roundEven(double v, int dec) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return 0.0;
        return BigDecimal.valueOf(v).setScale(Math.max(0, dec), RoundingMode.HALF_EVEN).doubleValue();
    }

    private static String fmtMax(double v, int max) {
        java.text.DecimalFormat f = new java.text.DecimalFormat("#,##0." + "#".repeat(max));
        return f.format(BigDecimal.valueOf(v));
    }

    private static LocalDate parseDate(Object o) {
        if (o == null) return null;
        String s = String.valueOf(o).trim();
        if (s.length() < 10) return null;
        try { return LocalDate.parse(s.substring(0, 10)); } catch (RuntimeException e) { return null; }
    }

    private static List<Map<String, Object>> copy(List<Map<String, Object>> in) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (in != null) for (Map<String, Object> m : in) out.add(m == null ? new LinkedHashMap<>() : new LinkedHashMap<>(m));
        return out;
    }

    // ------------------------------------------------------------------ the grid arithmetic (grd_CellUpdated, BillAmount, *Proportion)

    /** The page's preview: the same numbers Insert() would carry, for the rows as they stand. */
    public Map<String, Object> calc(Req r) {
        Cfg c = cfg();
        List<Map<String, Object>> lines = copy(r.lines), pay = copy(r.payments);
        int customerGl = 0;
        for (Map<String, Object> x : customers(c)) if (toInt(x.get("Id")) == r.customerId) customerGl = toInt(x.get("GlAccountId"));
        Totals t = compute(c, r, lines, customerGl);
        paymentFromPercent(pay, t.billText);
        return row("lines", lines, "payments", pay, "billAmount", t.billText, "fcyAmount", t.fcy, "itemAmount", t.itemAmount, "taxAmount", t.tax, "expAmount", t.exp);
    }

    /** PaymentTermAmountCalculateFromPercent: Amount = round(bill * % / 100, 2, away from zero) when % > 0, else 0 (only when the bill is positive and rows exist). */
    private void paymentFromPercent(List<Map<String, Object>> pay, double bill) {
        if (!(bill > 0.0) || pay.isEmpty()) return;
        for (Map<String, Object> p : pay) {
            double pct = d(p, "%ofTotal");
            p.put("Amount", pct > 0.0 ? round(bill * pct / 100.0, 2) : 0.0);
        }
    }

    /**
     * Runs, in the order Insert() leaves them: every edited row's cell formulas (grd_CellUpdated), txtExchangeRate_TextChanged,
     * ExpProportion, FreightProportion, BillAmount (+ BillProportion).
     */
    private Totals compute(Cfg c, Req r, List<Map<String, Object>> lines, int customerGl) {
        Totals t = new Totals();
        double rate = r.exchangeRate;
        for (Map<String, Object> m : lines) {
            String calc = str(m.get("calc"));
            double qty = d(m, "ItemQty");
            if (!calc.isEmpty()) {
                if (calc.contains("rate")) {                                // ItemQty / AddLessRate / DiscountType / Discount% edited
                    double base = d(m, "ItemRate"), addLess = d(m, "AddLessRate");
                    double net = addLess != 0.0 ? base + addLess : base;
                    m.put("NetRate", net);
                    double eq = d(m, "RateEquivalent");
                    double amount = eq == 0.0 ? 0.0 : qty / eq * net;
                    int dt = toInt(m.get("DiscountType"));
                    double pct = d(m, "DiscPct"), da;
                    if (dt == 1) da = pct > 0.0 ? pct : 0.0;
                    else if (dt == 2) {
                        if (pct > 100.0) { pct = 100.0; m.put("DiscPct", 100.0); }
                        da = pct > 0.0 ? amount * pct / 100.0 : 0.0;
                    } else { da = 0.0; m.put("DiscPct", 0.0); }
                    m.put("DiscountAmount", da);
                    m.put("ItemAmount", amount - da);
                }
                int taxType = toInt(m.get("TaxTypeId"));
                double pct = d(m, "TaxPct"), itemAmount = d(m, "ItemAmount");
                if (pct > 100.0) pct = 100.0;                               // "Tax % Can't be greater than 100..."
                if (itemAmount > 0.0 && pct > 0.0 && taxType > 0) { m.put("TaxPct", pct); m.put("TaxAmount", round(itemAmount * pct / 100.0, c.amtRound)); }
                else { m.put("TaxPct", 0.0); m.put("TaxAmount", 0.0); }
            }
            m.put("FcyAmount", rate > 0.0 ? d(m, "ItemAmount") / rate : 0.0);                    // txtExchangeRate_TextChanged
            t.qty += qty; t.itemAmount += d(m, "ItemAmount"); t.tax += d(m, "TaxAmount");
        }
        for (Map<String, Object> e : r.expenses) t.exp += d(e, "Amount");
        // ExpProportion
        for (Map<String, Object> m : lines) m.put("Expense", t.exp > 0.0 && t.qty != 0.0 ? t.exp / t.qty * d(m, "ItemQty") : 0.0);
        // FreightProportion
        if (!lines.isEmpty()) {
            if (r.freightAmount > 0.0 && r.freightCrId > 0 && r.freightDrId <= 0) {
                for (Map<String, Object> m : lines) m.put("Freights", t.qty == 0.0 ? 0.0 : round(r.freightAmount / t.qty * d(m, "ItemQty"), 4));
            } else if (!(r.freightAmount > 0.0) || r.freightDrId > 0) {
                for (Map<String, Object> m : lines) m.put("Freights", 0.0);
            }
        }
        // BillAmount
        double jCredit = 0, jDebit = 0;
        for (Map<String, Object> j : r.journals) if (toInt(j.get("AccountId")) > 0) { jCredit += d(j, "Credit"); jDebit += d(j, "Debit"); }
        if (!lines.isEmpty()) {
            double bill = 0.0;
            if (!c.sub) {
                if (r.freightCrId > 0 && customerGl == r.freightCrId) bill -= r.freightAmount;
                if (r.freightDrId > 0 && customerGl == r.freightDrId) bill += r.freightAmount;
            } else {
                if (r.freightCrId > 0 && r.customerId == r.freightCrId) bill -= r.freightAmount;
                if (r.freightDrId > 0 && r.customerId == r.freightDrId) bill += r.freightAmount;
            }
            bill = bill + t.itemAmount + t.tax + t.exp;
            double diff = jCredit - jDebit;
            bill = diff < 0.0 ? bill - Math.abs(diff) : bill + diff;
            bill = bill - r.discountAmount;
            t.bill = bill;
            t.billText = round(bill, c.amtPlaces);                         // txtBillAmount.Text = BillAmount.ToString(stringFormatsingle)
            t.fcy = rate > 0.0 ? round(bill / rate, 4) : 0.0;              // ToString("#,##0.####")
        }
        // BillProportion
        for (Map<String, Object> m : lines) m.put("BillAmount", d(m, "ItemAmount") + d(m, "TaxAmount") + d(m, "Expense") + d(m, "Freights"));
        return t;
    }

    // ------------------------------------------------------------------ Save (Insert / Update)

    private static boolean has(Collection<Map<String, Object>> rows, String key, int id) {
        for (Map<String, Object> m : rows) if (toInt(ci(m, key)) == id) return true;
        return false;
    }

    @Transactional
    public Map<String, Object> save(Req r) {
        UserAccount u = sup.user();
        Cfg c = cfg();
        Map<String, Boolean> rights = sup.rights(SCREEN);
        if (!Boolean.TRUE.equals(rights.get(r.id > 0 ? "update" : "save")))
            throw new Warning("You do not have the " + (r.id > 0 ? "Update" : "Save") + " right for this screen.");
        List<Map<String, Object>> lines = copy(r.lines), expenses = copy(r.expenses), journals = copy(r.journals), payments = copy(r.payments);

        // ---- lookups that the combos of the desktop are filled from (tenant scope)
        List<Map<String, Object>> customers = customers(c), terms = terms(), currencies = sup.rows("Sp_MultiCurrency_GetAllMethod", "OrganizationId", sup.org(),
                "CompanyId", sup.company(), "Activity", "ReadAll");
        List<Map<String, Object>> freightAcs = freightAccounts(c), taxAcs = accountTitles("3,6,8,16,17,18,19", null), discAcs = discountAccounts();
        List<Map<String, Object>> others = otherItems(), cities = cities(), taxTypes = taxTypes(r.docDate);
        Map<String, Object> customer = null;
        for (Map<String, Object> x : customers) if (toInt(x.get("Id")) == r.customerId) customer = x;
        int customerGl = customer == null ? 0 : toInt(customer.get("GlAccountId"));
        String fKey = c.sub ? "SupplierCustomerId" : "Id";
        if (!has(freightAcs, fKey, r.freightCrId)) r.freightCrId = 0;
        if (!has(freightAcs, fKey, r.freightDrId)) r.freightDrId = 0;
        if (!has(taxAcs, "Id", r.taxAccountId)) r.taxAccountId = 0;
        if (!has(discAcs, "Id", r.discountAccountId)) r.discountAccountId = 0;

        Map<String, Object> stored = null;
        Map<Integer, Map<String, Object>> storedLines = new HashMap<>();
        if (r.id > 0) {
            stored = invoice(r.id);
            for (Map<String, Object> d : children(stored, "details")) storedLines.put(toInt(ci(d, "Id")), storedLine(d));
        }

        // ---- loaded rows take their identity from the database (pending order rows / the invoice's own rows); typed rows are re-priced here
        Set<Integer> needSrc = new LinkedHashSet<>();
        int firstOrder = 0;
        for (Map<String, Object> m : lines) {
            int id = toInt(m.get("Id"));
            if (id > 0 && !storedLines.containsKey(id)) throw new Warning("Detail row not found");
            int ord = toInt(m.get("OrderId"));
            if (ord > 0) {
                if (firstOrder == 0) firstOrder = ord;
                if (firstOrder != ord) throw new Warning("Already Loaded Row's Have Different Order. So you Can't Load Rows Of Different Order!");
            }
            if (id == 0 && ord > 0) needSrc.add(ord);
        }
        Map<Integer, Map<String, Object>> srcByDetail = new HashMap<>();
        if (!needSrc.isEmpty()) for (Map<String, Object> g : pendingRowsOf(needSrc)) srcByDetail.putIfAbsent(toInt(ci(g, "PoDId")), g);
        Map<Integer, Map<Integer, double[]>> uomCache = new HashMap<>();
        for (Map<String, Object> m : lines) {
            int id = toInt(m.get("Id"));
            if (!(toInt(m.get("OrderId")) > 0)) { typedLine(c, m, taxTypes, uomCache); continue; }
            Map<String, Object> src;
            if (id > 0) {
                src = storedLines.get(id);
                if (toInt(src.get("OrderDetailId")) != toInt(m.get("OrderDetailId"))) throw new Warning("Detail row not found");
            } else {
                Map<String, Object> g = srcByDetail.get(toInt(m.get("OrderDetailId")));
                if (g == null || toInt(ci(g, "Id")) != toInt(m.get("OrderId"))) throw new Warning("Sale order row not found, or it is already fully invoiced");
                if (toInt(ci(g, "OrderSupCustId")) != r.customerId) throw new Warning("The loaded sale order belongs to a different Customer.");
                src = orderLine(g);
            }
            mergeSource(c, m, src);
        }
        if (r.taxRefresh && r.id == 0 && !lines.isEmpty()) applyTaxRefresh(lines, r.docDate);

        Totals t = compute(c, r, lines, customerGl);

        // ---- FormValidation(), in the desktop's order
        String docNoText = r.docNo == null ? "" : r.docNo.trim();
        if (toInt(docNoText) == 0) throw new Warning("Doc No Name is Required");
        if (customer == null) throw new Warning("Customer Name is Required");
        if (!has(terms, "Id", r.paymentTermId)) throw new Warning("Payment Term is Required");
        int dueDays = toInt(r.dueDays);
        if (r.paymentTermId == 2 && dueDays == 0) throw new Warning("Due Days Field is Required");
        String deliveryTerm = r.deliveryTerm == null ? "" : r.deliveryTerm;
        if (!deliveryTerm.equals("Load") && !deliveryTerm.equals("Ponch")) throw new Warning("Delivery Term is Required");
        boolean currencyOk = has(currencies, "Id", r.currencyId);
        if (c.multi) {
            if (!currencyOk) throw new Warning("Fcy Code Field is Required");
            if (!(r.exchangeRate > 0)) throw new Warning("Exchange Rate Field is Required");
            if (t.fcy == 0.0) throw new Warning("Fcy Amount Rate Field is Required");
        } else {
            if (!currencyOk) throw new Warning("Please Configure Your Base Currency In configurations");
            if (!(r.exchangeRate > 0)) throw new Warning("Please Configure Your Base Currency Rate In configurations");
        }
        if (!(r.freightAmount > 0.0) && (r.freightCrId > 0 || r.freightDrId > 0)) throw new Warning("Freight Amount Field is Required");
        if (r.freightAmount > 0.0 && r.freightCrId <= 0) throw new Warning("Freight/Ac Cr Field is Required");
        if (r.freightAmount > 0.0 && r.freightDrId <= 0) throw new Warning("Freight/Ac Dr Field is Required");
        if (r.freightAmount > 0.0 && (r.freightRemarks == null || r.freightRemarks.trim().isEmpty())) throw new Warning("Freight Remarks Field is Required");

        // ---- Insert(): journal rows, tax account, discount
        for (Map<String, Object> j : journals)
            if ((d(j, "Credit") > 0.0 || toInt(j.get("Debit")) > 0) && toInt(j.get("AccountId")) == 0) throw new Warning("Please Select an Account Against JL First");
        for (Map<String, Object> m : lines)
            if (d(m, "TaxAmount") > 0.0 && r.taxAccountId == 0) throw new Warning("Tax Account is Required");
        if (r.discountAccountId > 0 && r.discountAmount == 0.0) throw new Warning("Discount Amount Required When Discount Account Is Present...");
        if (r.discountAmount != 0.0 && r.discountAccountId == 0) throw new Warning("Discount Ac Required When Discount Amount Is Present...");
        if (r.discountAmount >= t.billText) throw new Warning("Discount Amount Can not be Greater than or Equal to Bill Amount");
        paymentFromPercent(payments, t.billText);                           // PaymentTermAmountCalculateFromPercent
        if (t.billText == 0.0) throw new Warning("Bill Amount Required");

        // ---- header
        SaleInvoiceModels.Head h = new SaleInvoiceModels.Head();
        LocalDateTime now = LocalDateTime.now();
        LocalDate docDate = parseDate(r.docDate);
        if (docDate == null) throw new Warning("Doc No Name is Required");
        h.TaxAccountId = r.taxAccountId;
        h.BranchesId = u.getBranchesId();
        h.DocumentTypeId = DOC_TYPE;
        h.SupplierInvoiceDate = now;
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.EntryDate = now;
        h.ModifyDate = now;
        h.EntryUser = u.getId();
        h.ScreenName = SCREEN;
        h.ModifyUser = u.getId();
        h.FinancialYearId = sup.fy();
        h.Id = r.id;
        h.DocNo = r.id > 0 ? toInt(ci(stored, "DocNo")) : nextNo();
        if (h.DocNo == 0) throw new Warning("Doc No Name is Required");
        h.SupplierInvoiceNo = h.DocNo;
        h.BranchSrNo = r.id > 0 ? toInt(ci(stored, "BranchSrNo")) : nextBranchSrNo();
        h.DocDate = docDate.atStartOfDay();
        h.SupplierCustomerId = r.customerId;
        h.SupplierReferenceNo = str(r.refNo);
        h.ManualBillNo = text(r.manualBillNo);
        h.PaymentTermId = r.paymentTermId;
        h.DueDays = dueDays;
        LocalDate dd = parseDate(r.dueDate);
        h.DueDate = (dd != null ? dd : docDate.plusDays(dueDays)).atStartOfDay();
        h.DeliveryTerm = deliveryTerm;
        h.BillAmount = t.billText;
        h.RemarksHeader = text(r.remarks);
        h.DiscountAmount = BigDecimal.valueOf(r.discountAmount);
        h.DiscountAccountId = r.discountAccountId;
        h.ExchangeRate = BigDecimal.valueOf(r.exchangeRate);
        h.CurrencyId = r.currencyId;
        h.InvoiceQty = BigDecimal.valueOf(t.qty);
        h.InvoiceWeight = BigDecimal.valueOf(t.qty);
        h.FcyAmount = BigDecimal.valueOf(t.fcy);
        if (!c.sub) { h.TransporterId = r.freightCrId; h.TransporterCreditPartyId = 0; h.TransporterDebitGLId = r.freightDrId; h.TransporterDebitPartyId = 0; }
        else {
            for (Map<String, Object> a : freightAcs) {
                if (r.freightCrId > 0 && toInt(a.get("SupplierCustomerId")) == r.freightCrId) { h.TransporterId = toInt(a.get("Id")); h.TransporterCreditPartyId = toInt(a.get("SupplierCustomerId")); }
                if (r.freightDrId > 0 && toInt(a.get("SupplierCustomerId")) == r.freightDrId) { h.TransporterDebitGLId = toInt(a.get("Id")); h.TransporterDebitPartyId = toInt(a.get("SupplierCustomerId")); }
            }
        }
        h.FreightAmount = r.freightAmount;
        h.FreightRemark = text(r.freightRemarks);
        double taxTotal = 0.0;
        for (Map<String, Object> m : lines) if (d(m, "TaxAmount") > 0.0) taxTotal += d(m, "TaxAmount");
        h.IsTaxable = r.taxAccountId > 0 && taxTotal > 0.0;
        h.UomScheduleIdCmRate = "";
        h.AttachmentsValues = r.id > 0 && stored != null && ci(stored, "AttachmentsValues") != null ? str(ci(stored, "AttachmentsValues")) : "";
        h.CustomAttachmentsValues = r.id > 0 && stored != null && ci(stored, "CustomAttachmentsValues") != null ? str(ci(stored, "CustomAttachmentsValues")) : "";

        // ---- detail rows (per row checks, in the desktop's order)
        if (lines.isEmpty()) throw new Warning("Grid Record Not Found");
        SaleInvoiceFinancialDirect.Invoice inv = new SaleInvoiceFinancialDirect.Invoice();
        inv.h = h;
        BigDecimal exRate = BigDecimal.valueOf(r.exchangeRate);
        int rowNo = 0;
        for (Map<String, Object> m : lines) {
            rowNo++;
            String tail = rowNo + " In Detail Grid";
            SaleInvoiceModels.Detail pd = new SaleInvoiceModels.Detail();
            pd.Id = toInt(m.get("Id"));
            pd.SaleOrderId = toInt(m.get("OrderId"));
            pd.SaleOrderDetailId = toInt(m.get("OrderDetailId"));
            pd.SaleOrder = toInt(m.get("OrderNo"));
            LocalDate od = parseDate(m.get("OrderDate"));
            pd.OrderDate = od == null ? now : od.atStartOfDay();
            pd.WarehouseId = toInt(m.get("WarehouseId"));
            pd.ItemId = toInt(m.get("ItemId"));
            pd.ItemUOMId = toInt(m.get("PackUomId"));
            pd.ItemVariantId = toInt(m.get("VariantId"));
            pd.CastingTypeId = toInt(m.get("CastingTypeId"));
            pd.ProductionStageId = toInt(m.get("ProductionStageId"));
            pd.ItemQty = d(m, "ItemQty");
            pd.RemarksDetail = str(m.get("Remarks"));
            pd.GrossWeight = pd.ItemQty; pd.AdLsWeight = 0.0; pd.NetBillWeight = pd.ItemQty; pd.NetStockWeight = pd.ItemQty;
            pd.UomScheduleIdRate = toInt(m.get("RateUomId"));
            pd.RateCut = toInt(m.get("AddLessRate"));                       // Conversion.ToInt(AddLessRate): a whole number
            pd.ItemRate = d(m, "NetRate");
            pd.DiscountTypeId = toInt(m.get("DiscountType"));
            pd.ItemDiscount = d(m, "DiscPct");
            pd.ItemDiscountAmount = d(m, "DiscountAmount");
            pd.ItemAmount = d(m, "ItemAmount");
            pd.ExchangeRate = exRate;
            pd.CurrencyId = r.currencyId;
            pd.FcyAmount = BigDecimal.valueOf(d(m, "FcyAmount"));
            pd.TaxNameId = toInt(m.get("TaxTypeId"));
            pd.TaxPercent = d(m, "TaxPct");
            pd.TaxAmount = roundEven(d(m, "TaxAmount"), 2);                  // Math.Round(TaxAmount, 2)
            pd.IsTaxable = pd.TaxNameId > 0 && pd.TaxPercent > 0.0 && pd.TaxAmount > 0.0 ? "true" : "false";
            pd.FreightAmount = d(m, "Freights");
            pd.JournalAmount = 0.0;
            pd.ExpenseAmount = d(m, "Expense");
            pd.BillAmount = d(m, "BillAmount");
            LocalDate gp = parseDate(m.get("GpDate"));
            pd.GpDate = gp == null ? now : gp.atStartOfDay();
            pd.DueDate = h.DueDate;
            pd.GpNo = toInt(m.get("GpNo"));
            pd.VehicleNo = str(m.get("VehicleNo"));
            pd.CityId = toInt(m.get("CityId"));
            pd.LineId = rowNo;
            if (pd.WarehouseId == 0) throw new Warning("Warehouse Required In Row#" + tail);
            if (pd.ItemId == 0) throw new Warning("Item Required In Row #" + tail);
            if (pd.ItemUOMId == 0) throw new Warning("Pack Uom Required In Row#" + tail);
            if (pd.CastingTypeId == 0) throw new Warning("CastingType Required In Row#" + tail);
            if (pd.ProductionStageId == 0) throw new Warning("ProductionStage Required In Row#" + tail);
            if (pd.ItemQty == 0.0) throw new Warning("Item Qty Required In Row#" + tail);
            if (pd.ItemRate == 0.0) throw new Warning("Item Rate Required In Row#" + tail);
            if (pd.UomScheduleIdRate == 0) throw new Warning("Rate Uom Required In Row#" + tail);
            if (pd.DiscountTypeId > 0 && pd.ItemDiscount <= 0.0 && pd.ItemDiscountAmount <= 0.0) pd.DiscountTypeId = 0;
            if (pd.DiscountTypeId > 0 && pd.ItemDiscount > 0.0 && pd.ItemDiscountAmount <= 0.0) throw new Warning("Discount Amount Required In Row#" + tail);
            if (pd.ItemAmount == 0.0) throw new Warning("Item Amount Required In Row#" + tail);
            if (pd.BillAmount == 0.0) throw new Warning("Bill Amount Required In Row#" + tail);
            if (pd.CityId == 0) throw new Warning("City Name Required In Row#" + tail);
            if (!has(cities, "Id", pd.CityId)) throw new Warning("City Name Required In Row#" + tail);
            inv.details.add(pd);
        }

        // ---- journals (rows whose account is chosen)
        Map<Integer, String> glIds = new HashMap<>();
        for (Map<String, Object> a : taxAcs) glIds.put(toInt(a.get("Id")), str(a.get("AccountTitle")));
        for (Map<String, Object> j : journals) {
            int acc = toInt(j.get("AccountId"));
            if (acc == 0) continue;
            if (!glIds.containsKey(acc)) throw new Warning("Please Select an Account Against JL First");
            if (!c.sub && acc == customerGl) throw new Warning("Selected Account Can not be Same As Supplier Account");
            SaleInvoiceModels.Journal pj = new SaleInvoiceModels.Journal();
            pj.ChartofAccountId = acc;
            pj.TransporterSupCustId = 0;
            pj.JvRemarks = str(j.get("Remarks"));
            pj.JvPrcnt = d(j, "Percentage"); pj.JvQty = d(j, "Qty"); pj.JvRate = d(j, "Rate"); pj.JvDebit = d(j, "Debit"); pj.JvCredit = d(j, "Credit");
            inv.journals.add(pj);
        }

        // ---- payment terms
        if (payments.isEmpty()) throw new Warning("Payment Detail Record Not Found");
        BigDecimal payAmount = BigDecimal.ZERO, payPct = BigDecimal.ZERO;
        double sumPay = 0.0;
        for (Map<String, Object> p : payments) sumPay += d(p, "Amount");
        if (sumPay > 0.0) {
            int pr = 0;
            for (Map<String, Object> p : payments) {
                pr++;
                if (toDecimal(p.get("Amount")).compareTo(BigDecimal.ZERO) > 0) {
                    SaleInvoiceModels.PaymentTerm o = new SaleInvoiceModels.PaymentTerm();
                    o.PaymentTermId = toInt(p.get("PaymentTermId"));
                    o.PrcntOfTotal = toDecimal(p.get("%ofTotal"));
                    payPct = payPct.add(o.PrcntOfTotal);
                    o.Amount = toDecimal(p.get("Amount"));
                    payAmount = payAmount.add(o.Amount);
                    o.DueDays = toInt(p.get("DueDays"));
                    LocalDate pdd = parseDate(p.get("DueDate"));
                    o.DueDate = (pdd == null ? LocalDate.of(1900, 1, 1) : pdd).atStartOfDay();
                    o.PaymentRemarks = str(p.get("Remarks"));
                    if (o.PaymentTermId <= 0) throw new Warning("Payment Term Required in row#" + pr);
                    if (!has(terms, "Id", o.PaymentTermId)) throw new Warning("Payment Term Required in row#" + pr);
                    if (o.PaymentTermId == 2 && o.DueDays <= 0) throw new Warning("Due Days Required In case Of Credit row in row#" + pr);
                    inv.paymentTerms.add(o);
                }
            }
        } else {
            SaleInvoiceModels.PaymentTerm o = new SaleInvoiceModels.PaymentTerm();
            o.PaymentTermId = r.paymentTermId;
            o.PrcntOfTotal = new BigDecimal("100");
            payPct = new BigDecimal("100");
            o.Amount = BigDecimal.valueOf(100.0 * h.BillAmount / 100.0);
            payAmount = o.Amount.setScale(4, RoundingMode.HALF_EVEN);
            o.DueDays = dueDays;
            o.DueDate = h.DueDate;
            o.PaymentRemarks = "";
            inv.paymentTerms.add(o);
        }
        if (payPct.compareTo(new BigDecimal("100")) != 0) throw new Warning("Payment Detail Total% not equal to 100");
        if (payAmount.compareTo(BigDecimal.valueOf(h.BillAmount)) != 0)
            throw new Warning("Payment Detail Amount " + fmtMax(payAmount.doubleValue(), 4) + " Not Equal to Total Invoice Amount " + fmtMax(h.BillAmount, 4));

        // ---- expenses
        for (Map<String, Object> e : expenses) {
            if (toInt(e.get("ItemId")) != 0 && d(e, "Amount") != 0.0) {
                if (!has(others, "Id", toInt(e.get("ItemId")))) throw new Warning("Please Select an Item Against Expense First");
                SaleInvoiceModels.Expense pe = new SaleInvoiceModels.Expense();
                pe.InvRevExpItemId = toInt(e.get("ItemId"));
                pe.Qty = (int) Math.rint(d(e, "Qty"));                        // Conversion.ToInt(Qty): a whole number
                pe.Rate = d(e, "Rate");
                pe.Amount = d(e, "Amount");
                pe.Remarks = str(e.get("Remarks"));
                inv.expenses.add(pe);
            }
        }

        // ---- BLL 0580 Save + DAL 0433 SetData (one transaction); InvSaleInvoiceCommission is an empty list on this screen
        if (h.Id == 0) h.ModifyUser = 0; else h.EntryUser = 0;
        SaleInvoiceFinancialDirect.Voucher voucher = new SaleInvoiceFinancialDirect(repo).makeVoucherForSaleInvoice(inv);
        int saved = persist.persist(inv, voucher, h.Id == 0 ? "Sp_InvSaleInvoice_Insert" : "Sp_InvSaleInvoice_Update");
        attachments.apply(SCREEN, DOC_TYPE, saved, r.customerId, r.attachments);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", saved);
        out.put("docNo", h.DocNo);
        out.put("customerId", r.customerId);
        out.put("docDate", docDate.toString());
        out.put("voucherHeadId", repo.voucherHeadId(u.getOrganizationId(), u.getCompanyId(), DOC_TYPE, saved));
        out.put("message", (r.id == 0 ? "Record Save Successfully [" : "Record Update Successfully [") + h.DocNo + "]");
        return out;
    }

    /**
     * Puts the database's version of the identity / source columns on the row the page sent (loaded rows). Rows the user did not edit (calc empty)
     * take the source's amounts as they are; edited rows keep the editable inputs (grd: ItemQty, AddLessRate, DiscountType, Discount%, Remarks, GpDate,
     * GpNo, VehicleNo, CityId and, when TaxPercentEditable, the tax type / %).
     */
    private void mergeSource(Cfg c, Map<String, Object> m, Map<String, Object> src) {
        String calc = str(m.get("calc"));
        Map<String, Object> mine = new LinkedHashMap<>(m);
        m.clear();
        m.putAll(src);
        m.put("calc", calc);
        m.put("Remarks", str(mine.get("Remarks")));
        m.put("GpDate", mine.get("GpDate") != null && !str(mine.get("GpDate")).isEmpty() ? mine.get("GpDate") : src.get("GpDate"));
        m.put("GpNo", str(mine.get("GpNo")));
        m.put("VehicleNo", str(mine.get("VehicleNo")));
        m.put("CityId", toInt(mine.get("CityId")) > 0 ? toInt(mine.get("CityId")) : toInt(src.get("CityId")));
        int wh = toInt(mine.get("WarehouseId"));
        m.put("ProductionStageId", toInt(mine.get("ProductionStageId")));
        if (toInt(mine.get("CastingTypeId")) > 0) m.put("CastingTypeId", toInt(mine.get("CastingTypeId")));   // the grid cell is a drop-down
        if (wh > 0) m.put("WarehouseId", wh);                               // BtnUpdateInGridDetail_Click sets the warehouse of every row
        if (calc.isEmpty()) return;
        m.put("AddLessRate", d(mine, "AddLessRate"));
        if (calc.contains("rate")) {
            m.put("ItemQty", d(mine, "ItemQty"));
            m.put("DiscountType", toInt(mine.get("DiscountType")));
            m.put("DiscPct", d(mine, "DiscPct"));
        }
        if (c.taxEditable) { m.put("TaxTypeId", toInt(mine.get("TaxTypeId"))); m.put("TaxPct", d(mine, "TaxPct")); }
    }

    /**
     * A row typed in the item entry panel (no Ref*): ids come from the page, every amount is recomputed (btnAdd_Click / CalculateAmount /
     * CalculateTaxAmount) from the UOM schedule's equivalents and the tax schedule.
     */
    private void typedLine(Cfg c, Map<String, Object> m, List<Map<String, Object>> taxTypes, Map<Integer, Map<Integer, double[]>> uomCache) {
        Map<String, Object> mine = new LinkedHashMap<>(m);
        m.clear();
        int item = toInt(mine.get("ItemId"));
        Map<Integer, double[]> eq = uomCache.computeIfAbsent(item, k -> {
            Map<Integer, double[]> x = new HashMap<>();
            for (Map<String, Object> u : uoms(k)) x.put(toInt(u.get("Id")), new double[]{toDouble(u.get("Equivalent"))});
            return x;
        });
        int pack = toInt(mine.get("PackUomId")), rateUom = toInt(mine.get("RateUomId"));
        if (!eq.containsKey(pack)) pack = 0;
        if (!eq.containsKey(rateUom)) rateUom = 0;
        int taxType = toInt(mine.get("TaxTypeId"));
        double sched = 0.0;
        for (Map<String, Object> t : taxTypes) if (toInt(t.get("TaxNameId")) == taxType) sched = toDouble(t.get("TaxPercent"));
        double pct = d(mine, "TaxPct");
        pct = c.taxEditable && pct > 0.0 ? pct : sched;                      // txtTaxPercent is editable only when TaxPercentEditable
        m.put("Id", toInt(mine.get("Id"))); m.put("OrderId", 0); m.put("OrderDetailId", 0); m.put("OrderNo", 0); m.put("OrderDate", null);
        m.put("WarehouseId", toInt(mine.get("WarehouseId"))); m.put("ItemId", item);
        m.put("PackUomId", pack); m.put("PackEquivalent", pack > 0 ? eq.get(pack)[0] : 0.0);
        m.put("VariantId", toInt(mine.get("VariantId"))); m.put("CastingTypeId", toInt(mine.get("CastingTypeId"))); m.put("ProductionStageId", toInt(mine.get("ProductionStageId")));
        m.put("Remarks", str(mine.get("Remarks"))); m.put("ItemQty", d(mine, "ItemQty")); m.put("NetWeight", d(mine, "NetWeight"));
        m.put("ItemRate", d(mine, "ItemRate")); m.put("RateUomId", rateUom); m.put("RateEquivalent", rateUom > 0 ? eq.get(rateUom)[0] : 0.0);
        m.put("AddLessRate", d(mine, "AddLessRate")); m.put("DiscountType", toInt(mine.get("DiscountType"))); m.put("DiscPct", d(mine, "DiscPct"));
        m.put("TaxTypeId", taxType); m.put("TaxPct", pct);
        m.put("GpDate", mine.get("GpDate")); m.put("GpNo", str(mine.get("GpNo"))); m.put("VehicleNo", str(mine.get("VehicleNo")));
        m.put("CityId", toInt(mine.get("CityId")));
        String calc = str(mine.get("calc"));
        m.put("calc", calc);
        if (!calc.isEmpty()) return;                                         // edited in the grid afterwards: compute() runs grd_CellUpdated's formulas
        // btnAdd_Click / btnUpdateDetail_Click: the amounts are the entry panel's CalculateAmount / CalculateTaxAmount results
        double qty = d(m, "ItemQty"), rate = d(m, "ItemRate"), addLess = d(m, "AddLessRate"), eqv = d(m, "RateEquivalent");
        double net = 0.0, itemAmount = 0.0, da = 0.0, dpct = d(m, "DiscPct");
        if (qty > 0.0 && rate > 0.0 && eqv > 0.0) {
            net = addLess != 0.0 ? rate + addLess : rate;
            double amount = qty / eqv * net;
            int dt = toInt(m.get("DiscountType"));
            if (dt == 1) da = dpct > 0.0 ? dpct : 0.0;
            else if (dt == 2) { if (dpct > 100.0) dpct = 100.0; da = dpct > 0.0 ? amount * dpct / 100.0 : 0.0; }
            else { da = 0.0; dpct = 0.0; }
            da = round(da, 3);                                               // txtDiscountAmount.Text is "#,##0.###"
            itemAmount = round(amount - da, c.amtRound);                     // txtAmount: Math.Round(Amount, amountDecimals, AwayFromZero)
        } else { dpct = 0.0; }
        m.put("NetRate", net); m.put("DiscPct", dpct); m.put("DiscountAmount", da); m.put("ItemAmount", itemAmount);
        double tp = d(m, "TaxPct"), ta = 0.0;
        if (taxType > 0) { if (tp > 100.0) tp = 100.0; ta = round(itemAmount * tp / 100.0, c.amtRound); }
        else tp = 0.0;
        m.put("TaxPct", tp); m.put("TaxAmount", ta);
    }

    /** DocDate_Leave: the tax schedule of the items on the document date decides the tax type / % / amount of every row (new invoice only). */
    private void applyTaxRefresh(List<Map<String, Object>> lines, String docDate) {
        StringBuilder ids = new StringBuilder();
        for (Map<String, Object> m : lines) ids.append(',').append(toInt(m.get("ItemId")));
        List<Map<String, Object>> dt = taxByDate(ids.toString(), docDate);
        for (Map<String, Object> m : lines) {
            boolean flag = false;
            for (Map<String, Object> t : dt) {
                if (toInt(t.get("ItemId")) == toInt(m.get("ItemId"))) {
                    flag = true;
                    double pct = d(t, "TaxPercent"), ia = d(m, "ItemAmount");
                    m.put("TaxTypeId", toInt(t.get("TaxNameId"))); m.put("TaxPct", pct);
                    if (ia > 0.0 && pct > 0.0) m.put("TaxAmount", ia * pct / 100.0);
                }
            }
            if (!flag) { m.put("TaxTypeId", 0); m.put("TaxPct", 0.0); m.put("TaxAmount", 0.0); }
        }
    }

    // ------------------------------------------------------------------ Delete

    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = sup.user();
        if (!Boolean.TRUE.equals(sup.rights(SCREEN).get("delete"))) throw new Warning("You do not have the Delete right for this screen.");
        Map<String, Object> inv = invoice(id);
        if (toBool(ci(inv, "IsApproved"))) throw new Warning("Record cannot be  Delete beacause Record has approved");
        repo.delete(u, id, DOC_TYPE);                                      // InvPurchaseInvoice.RemoveByID -> Sp_InvoicesVouchersandStocksDelete
        return row("message", "Delete Record Successfully");
    }

    // ------------------------------------------------------------------ attachments

    public List<Map<String, Object>> attachmentList(int id) { invoice(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { invoice(id); return attachments.download(SCREEN, id, attachmentId); }
}
