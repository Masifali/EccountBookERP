package com.mst.services.sale.engr;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.services.SaleInvoiceEngrPersist;
import com.mst.services.SaleInvoiceFinancialDirect;
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
 * Screen 535 "SaleInvoiceTrading_Engr" = Architecture.WinApp.SaleTrading.SaleInvoiceTrading_Engr (Sale Engr, module 83, document type 1609).
 *
 * Desktop map (siT.cs = SaleInvoiceTrading_Engr.cs, 5458 lines; method : line):
 *   Load :246, DocumentNo :470, HistoryComboBind :493, TransPorterDebitFill :546, PaymentTerms :668, DeliveryTerm :702, CommissionUOMFill :719,
 *   CommissionTypeFill :743, SupplierNameFilll :756, CommissionDebitAccountFill :806, TaxAccountFill :845, DiscountAccountFill :877,
 *   VoucherHeadIdGet :909, GetallTaxType :920, OtherItemsBind :931, CurrencyFill :945, MultiCurrencyFeature :978, ConfigurationDefault :1007,
 *   cmbCurrency_Leave :1036, txtExchangeRate_TextChanged :1072, FormValidation :1115, Reset :1203, btnRefresh_Click :1282,
 *   grdPaymentTerm_* :1329-1590, PropotionateCommissiongrid :1653, CalculateCommissionGrid :1737, grdGLedger_* :2069-2398, grdInvExp_* :2482-2606,
 *   grd_CellUpdated :2620, grdSettings :2746, ReadById :2919, Insert :3121, btnDelete_Click :3631, GetAll (history) :3663,
 *   TotalCommissionAmount :4077, BillAmount :4189, ExpProportion :4269, FreightProportion :4301, LedgerProportion :4338, BillProportion :4396,
 *   toolStripButton3_Click_1 (Load Gdn) :4502, LoadDataDetailGridAgainstGP :4518, LoadExpensesFromGdn :4614, LoadPaymentDetailBySaleOrderIds :4636,
 *   LoadInGridDetail :4658, print buttons :4829-4887. Loader dialog: LoadGdnTrading_Engr.cs (PendingGdnLoad :72, btnLoadOnInvoice_Click_1 :202).
 * BLL 0580 InvSaleInvoice.Save (Id 0: ModifyUser 0, Sp_InvSaleInvoice_Insert; else EntryUser 0, Sp_InvSaleInvoice_Update) and DAL 0433 SetData
 * (SaleInvoiceEngrPersist), voucher by SaleInvoiceFinancialDirect.makeVoucherForSaleInvoice, all in one transaction.
 *
 * The page runs the desktop's grid arithmetic by calling {@link #calc}; the same code is run again by {@link #save} on the data the
 * database holds (the GDN / the stored invoice), so the saved numbers never depend on what the browser computed.
 */
@Service
public class SaleSaleInvoiceEngrService {
    public static final String SCREEN = "SaleInvoiceTrading_Engr";
    public static final int DOC_TYPE = 1609;
    public static final int GDN_TYPE = 1612;

    private final SaleEngrSupport sup;
    private final SaleEngrAttachments attachments;
    private final SaleInvoiceRepository repo;
    private final SaleInvoiceEngrPersist persist;

    public SaleSaleInvoiceEngrService(SaleEngrSupport sup, SaleEngrAttachments attachments, SaleInvoiceRepository repo, SaleInvoiceEngrPersist persist) {
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
        boolean sub, freightToExp, commToExp, commLock, taxEditable, multi;
        String creditInSaleGl = "";
        int amtRound, amtPlaces, ratePlaces, fcyPlaces;
    }

    private Cfg cfg() {
        return cached("cfg", () -> {
            Cfg c = new Cfg();
            c.sub = sup.erpFeature(4);                                                  // SubsidiaryAccountAllownOnVouchers
            c.multi = sup.erpFeature(6);                                                // HasMultiCurrencyFeature
            Map<String, String> m = new HashMap<>();
            for (Map<String, Object> r : sup.rows("Sp_ConfigrationsAllocation_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                    "ConfigDescriptions", "DebitAmountChargetoExpenseAcFreightGrid,DebitAmountChargetoExpenseAcOfCommission,CommissionEditableOnInvoice,TaxPercentEditable",
                    "Activity", "GetMultipleConfigurationsByConfigDescriptions"))
                m.put(str(ci(r, "ConfigDescription")), str(ci(r, "ConfigKey")));
            c.freightToExp = toBool(m.get("DebitAmountChargetoExpenseAcFreightGrid"));
            c.commToExp = toBool(m.get("DebitAmountChargetoExpenseAcOfCommission"));
            c.commLock = toBool(m.get("CommissionEditableOnInvoice"));
            c.taxEditable = toBool(m.get("TaxPercentEditable"));
            c.creditInSaleGl = sup.config("CreditAmountInItemSaleGL");
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
        Map<String, Object> m = new LinkedHashMap<>(lists());
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());                                         // DocumentNo
        m.put("lastDiscountAcId", lastDiscountAcId());                     // PreviousDiscountAc
        m.put("history", historyCombos());                                 // HistoryComboBind
        m.put("fyStart", fyStart());
        m.put("branchId", sup.branch());
        return m;
    }

    /** Refresh button: OtherItemsBind, SupplierNameFilll, TaxAccountFill, TransPorterDebitFill, configurations, ConfigurationDefault, DiscountAccountFill. */
    public Map<String, Object> refresh() {
        cache.clear();
        Map<String, Object> m = new LinkedHashMap<>(lists());
        m.put("lastDiscountAcId", 0);
        return m;
    }

    private Map<String, Object> lists() {
        Cfg c = cfg();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("customers", customers());
        m.put("terms", terms());
        m.put("deliveryTerms", List.of(row("Id", 1, "DeliveryTerm", "Load"), row("Id", 2, "DeliveryTerm", "Ponch")));
        m.put("currencies", sup.rows("Sp_MultiCurrency_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadAll"));
        m.put("otherItems", otherItems());
        m.put("freightAccounts", freightAccounts(c));
        m.put("taxAccounts", accountTitles("3,6,8,16,17,18,19", null));
        m.put("commissionDebitAccounts", c.commToExp ? accountTitles("11,12,13,14,20,21", null) : List.of());
        m.put("discountAccounts", discountAccounts());
        m.put("taxTypes", taxTypes());
        m.put("commissionTypes", List.of(row("Id", 1, "CommissionType", "Flat"), row("Id", 2, "CommissionType", "Percent")));
        m.put("commissionUoms", List.of("1", "5", "10", "25", "40", "50", "60", "65", "80", "100"));
        m.put("multiCurrency", c.multi);
        m.put("subsidiary", c.sub);
        m.put("freightDebitToExpenses", c.freightToExp);
        m.put("commissionDebitToExpenses", c.commToExp);
        m.put("commissionLocked", c.commLock);
        m.put("taxEditable", c.taxEditable);
        m.put("defaults", row("baseCurrency", sup.configInt("Base Currency"), "baseRate", toDouble(sup.config("BaseCurrencyRate"))));
        m.put("fmt", row("amountRound", c.amtRound, "amount", c.amtPlaces, "rate", c.ratePlaces, "fcy", c.fcyPlaces));
        return m;
    }

    /** SupplierCustomer.Getall -> Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyId'. */
    public List<Map<String, Object>> customers() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadByOrganizationCompanyId"))
            out.add(row("Id", toInt(ci(r, "Id")), "CompanyName", str(ci(r, "CompanyName")), "GlAccountId", toInt(ci(r, "GlAccountId")), "PartyTypeId", toInt(ci(r, "PartyTypeId"))));
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

    /** TransPorterDebitFill: dtAccountlst {Id (GL), SupplierCustomerId, AccountTitle}. */
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

    /** GetallTaxType: ItemTaxSchedule.GetAllTaxSchedule(org, company, DocDate, 2). */
    private List<Map<String, Object>> taxTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_ItemTaxSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "EffectedDate", LocalDate.now(),
                "TaxTypeId", 2, "Activity", "GetAllTaxSchedule"))
            out.add(row("TaxNameId", toInt(ci(r, "TaxNameId")), "TaxName", str(ci(r, "TaxName")), "TaxPercent", toDouble(ci(r, "TaxPercent"))));
        return out;
    }

    /** DocumentNo: InvSaleInvoice.GenerateInvSaleInvoiceCode (document type 1609). */
    public int nextNo() { return repo.nextDocNo(sup.user(), sup.fy(), DOC_TYPE); }

    /** PreviousDiscountAc: InvSaleInvoice.GetLastDiscountAcId. */
    public int lastDiscountAcId() {
        List<Map<String, Object>> r = sup.rows("Sp_InvSaleInvoice_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetLastDiscountAcId");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "DiscountAccountId"));
    }

    /** cmbCurrency_Leave: VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher (document type 1609). */
    public List<Map<String, Object>> lastRate(int currencyId) {
        return sup.rows("Sp_Vouchers_GetMethods", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeIds", String.valueOf(DOC_TYPE),
                "DMultiCurrencyIds", String.valueOf(currencyId), "Activity", "GetMultiCurrencyAndLastRate");
    }

    /** grdPaymentTerm_CellUpdated (PaymentTermId 3): SaleOrder.GetSaleOrderRemningAmountByOrderId. */
    public Map<String, Object> remaining(int orderId, int recId) {
        List<Map<String, Object>> r = sup.rows("USP_GetSaleOrderRemningAmountByOrderId", "OrganizationId", sup.org(), "CompanyId", sup.company(), "OrderId", orderId, "RecId", idOrNull(recId));
        return row("balanceAmount", r.isEmpty() ? 0 : toDouble(ci(r.get(0), "BalanceAmount")));
    }

    /** Start of the active financial year (clsGlobalVariables.ActiveYr.Start_Period). */
    public String fyStart() {
        for (Map<String, Object> r : sup.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            if (toInt(r.get("Id")) == sup.fy()) return str(r.get("Start_Period"));
        return "";
    }

    // ------------------------------------------------------------------ Load Gdn dialog (LoadGdnTrading_Engr)

    /** PendingGdnLoad: InvSaleInvoice.GetPendingGDNForSaleInvoice_Engr (document type 1612). */
    public List<Map<String, Object>> loaderRows(String fromDate, String toDate) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company());
        p.put("DocumentTypeId", GDN_TYPE); p.put("FinancialYearId", sup.fy());
        LocalDate f = parseDate(fromDate), t = parseDate(toDate);
        if (f != null) p.put("FromDate", f);
        if (t != null) p.put("ToDate", t);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "USP_GetPendingGDNForSaleInvoice_Engr", p))
            out.add(row("Id", toInt(ci(r, "Id")), "SaleOrderId", toInt(ci(r, "SaleOrderId")), "DocDate", ci(r, "DocDate"), "DocNo", toInt(ci(r, "DocNo")),
                    "SupplierCustomerId", toInt(ci(r, "SupplierCustomerId")), "CustomerName", ci(r, "SupplierCustomer"), "GpNO", toInt(ci(r, "GpNO")), "BiltyNo", ci(r, "BiltyNo"),
                    "VehicleNo", ci(r, "VehicleNo"), "ItemQty", toDouble(ci(r, "ItemQty")), "DeliveryTypeId", toInt(ci(r, "DeliveryTypeId")), "DeliveryType", ci(r, "DeliveryType")));
        return out;
    }

    /** grd_SelectionChanged: InvGdn.GetByID detail rows of the highlighted pending GDN. */
    public List<Map<String, Object>> loaderDetail(int gdnId) {
        gdnHeader(gdnId);
        return sup.rows("Sp_InvGdn_GetAllMethod", "InvGdnMainId", gdnId, "Activity", "GetGDNDetailByGdnId");
    }

    private Map<String, Object> gdnHeader(int id) {
        List<Map<String, Object>> r = sup.rows("Sp_InvGdn_GetAllMethod", "Id", id, "Activity", "GetById");
        UserAccount u = sup.user();
        if (r.isEmpty() || toInt(ci(r.get(0), "OrganizationId")) != u.getOrganizationId() || toInt(ci(r.get(0), "CompanyId")) != u.getCompanyId()
                || toInt(ci(r.get(0), "DocumentTypeId")) != GDN_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Goods dispatch note not found in this company");
        return r.get(0);
    }

    private static List<Integer> idList(String ids) {
        List<Integer> out = new ArrayList<>();
        if (ids != null) for (String s : ids.split(",")) { int n = toInt(s.trim()); if (n > 0 && !out.contains(n)) out.add(n); }
        return out;
    }

    private static String join(Collection<Integer> ids) {
        StringBuilder b = new StringBuilder();
        for (Integer i : ids) b.append(',').append(i);
        return b.toString();
    }

    /** USP_GetGdnLoad_Eng rows of GDNs of this company (the tenant is checked per GDN). */
    private List<Map<String, Object>> gdnRows(Collection<Integer> gdnIds) {
        for (Integer g : gdnIds) gdnHeader(g);
        return sup.rows("USP_GetGdnLoad_Eng", "GdnIds", join(gdnIds));
    }

    /**
     * LoadDataDetailGridAgainstGP + LoadExpensesFromGdn + LoadPaymentDetailBySaleOrderIds: everything the page needs to add the checked GDNs.
     * The grid rows are dtGrid rows (desktop column names). Errors keep the desktop's wording.
     */
    public Map<String, Object> loadGdn(String gdnIds, int deliveryTypeId, int customerId) {
        List<Integer> ids = idList(gdnIds);
        if (ids.isEmpty()) throw new Warning("Chek the row first");
        List<Map<String, Object>> g = gdnRows(ids);
        Map<String, Object> out = new LinkedHashMap<>();
        if (g.isEmpty()) { out.put("lines", List.of()); return out; }
        Map<String, Object> first = g.get(0);
        if (customerId > 0 && toInt(ci(first, "SupplierCustomerId")) != customerId) throw new Warning("You Can't Load GDN Of Different Customer At Same Time");
        out.put("head", row("SupplierCustomerId", toInt(ci(first, "SupplierCustomerId")), "DeliveryTerm", str(ci(first, "DeliveryTerm")), "PaymentTermsId", toInt(ci(first, "PaymentTermsId")),
                "OrderDueDays", ci(first, "OrderDueDays"), "AccountTitle", str(ci(first, "AccountTitle")), "CarriageAmount", toDouble(ci(first, "CarriageAmount")),
                "RemarksHeader", str(ci(first, "RemarksHeader")), "SaleOrderId", toInt(ci(first, "SaleOrderId"))));
        List<Map<String, Object>> comm = new ArrayList<>();
        Set<Integer> orders = new HashSet<>();
        for (Map<String, Object> r : g) {                                  // dtComm rows, one per sale order that has a broker
            int so = toInt(ci(r, "SaleOrderId"));
            if (!orders.contains(so) && toInt(ci(r, "BrokerAgentSupCustId")) != 0) {
                // the desktop adds 10 values to the 11 columns, so the remarks land in DebitAccountId
                comm.add(row("Id", 0, "OrderId", ci(r, "SaleOrderId"), "OrderNo", ci(r, "SaleOrder"), "InvoiceId", 0, "CommissionAgentId", ci(r, "BrokerAgentSupCustId"),
                        "CommType", str(ci(r, "CommissionType")), "CommRate", toDouble(ci(r, "CommRate")), "CommUom", str(ci(r, "UomScheduleIdCmRate")),
                        "CommissionAmount", toDouble(ci(r, "CommAmount")), "DebitAccountId", str(ci(r, "CommissionRemarks")), "Remarks", null));
            }
            orders.add(so);
        }
        out.put("comm", comm);
        List<Map<String, Object>> lines = new ArrayList<>();
        for (Map<String, Object> r : g) lines.add(gdnLine(r, deliveryTypeId));
        out.put("lines", lines);
        List<Map<String, Object>> exp = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_InvGdnExpensesByInvGdnIdIds", "GdnIds", join(ids)))
            exp.add(row("ItemId", ci(r, "ItemId"), "Qty", toDouble(ci(r, "Qty")), "Rate", 0, "Amount", 0, "Remarks", str(ci(r, "Remarks"))));
        out.put("expenses", exp);
        List<Map<String, Object>> pay = new ArrayList<>();
        if (deliveryTypeId == 1) {
            for (Map<String, Object> r : repo.saleOrderPaymentTerms(String.valueOf(toInt(ci(first, "SaleOrderId")))))
                pay.add(row("PaymentTermId", toInt(ci(r, "PaymentTermId")), "%ofTotal", toDouble(ci(r, "PrcntOfTotal")), "Amount", toDouble(ci(r, "Amount")),
                        "DueDays", toInt(ci(r, "DueDays")), "DueDate", ci(r, "DueDate"), "Remarks", str(ci(r, "PaymentRemarks"))));
        }
        out.put("payments", pay);
        return out;
    }

    /** dtGrid.Rows.Add(...) of LoadDataDetailGridAgainstGP. */
    private Map<String, Object> gdnLine(Map<String, Object> g, int typeId) {
        double itemAmount = round(toDouble(ci(g, "ItemAmount")), 4);
        double taxPct = toDouble(ci(g, "TaxPercent"));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", 0); m.put("GdnId", toInt(ci(g, "InvGdnId"))); m.put("GdnDetailId", toInt(ci(g, "Id")));
        m.put("SaleOrderId", toInt(ci(g, "SaleOrderId"))); m.put("SaleOrderDetailId", toInt(ci(g, "SaleOrderDetailId")));
        m.put("SaleOrder", toInt(ci(g, "SaleOrder"))); m.put("OrderDate", ci(g, "OrderDate")); m.put("WarehouseId", toInt(ci(g, "WarehouseId")));
        m.put("ItemCode", str(ci(g, "ItemCode"))); m.put("ItemId", toInt(ci(g, "ItemId"))); m.put("ItemName", str(ci(g, "ItemName")));
        m.put("PackUomId", toInt(ci(g, "ItemUomId"))); m.put("PackUom", str(ci(g, "PackUom"))); m.put("PackEquivalent", toDouble(ci(g, "PackEquivalent")));
        m.put("JobLotId", toInt(ci(g, "JobLotId"))); m.put("ItemQty", toDouble(ci(g, "ItemQty")));
        m.put("ItemRate", toDouble(ci(g, "ItemRate"))); m.put("RateUomId", toInt(ci(g, "OrderItemRateUOMId")));
        m.put("RateUOM", typeId != 1 ? "1" : str(ci(g, "RateUom"))); m.put("RateEquivalent", typeId != 1 ? 1.0 : toDouble(ci(g, "RateEquivalent")));
        m.put("AddLessRate", 0.0); m.put("NetRate", toDouble(ci(g, "ItemRate")));
        m.put("DiscPrcnt", 0.0); m.put("DiscAmount", 0.0); m.put("ItemAmount", itemAmount); m.put("FcyAmount", 0.0);
        m.put("TaxNameId", toInt(ci(g, "TaxNameId"))); m.put("TaxPrcnt", taxPct); m.put("TaxAmount", itemAmount * taxPct / 100.0);
        m.put("Freights", 0.0); m.put("Expense", 0.0); m.put("Journal", 0.0); m.put("Commission", 0.0); m.put("BillAmount", 0.0);
        m.put("VehicleNo", str(ci(g, "VehicleNo"))); m.put("GpNo", toInt(ci(g, "GpNo")));
        m.put("PaymentTermId", toInt(ci(g, "PaymentTermsId"))); m.put("DueDays", toInt(ci(g, "OrderDueDays"))); m.put("DueDate", dateOr1900(ci(g, "OrderDueDate")));
        int bt = toInt(ci(g, "BillCalculateTypeId"));
        m.put("BillTypeId", bt); m.put("BillType", bt == 2 ? "On Qty" : "On Weight");
        return m;
    }

    /** ReadById's dtGrid.Rows.Add(...) from an InvSaleInvoiceDetail row. */
    private Map<String, Object> storedLine(Map<String, Object> d) {
        double rate = toDouble(ci(d, "ItemRate")), cut = toDouble(ci(d, "RateCut"));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", toInt(ci(d, "Id"))); m.put("GdnId", toInt(ci(d, "InvGdnId"))); m.put("GdnDetailId", toInt(ci(d, "InvGdnDetailId")));
        m.put("SaleOrderId", toInt(ci(d, "SaleOrderId"))); m.put("SaleOrderDetailId", toInt(ci(d, "SaleOrderDetailId")));
        m.put("SaleOrder", toInt(ci(d, "SaleOrder"))); m.put("OrderDate", ci(d, "OrderDate")); m.put("WarehouseId", toInt(ci(d, "WarehouseId")));
        m.put("ItemCode", str(ci(d, "ItemCode"))); m.put("ItemId", toInt(ci(d, "ItemId"))); m.put("ItemName", str(ci(d, "ItemName")));
        m.put("PackUomId", toInt(ci(d, "ItemUOMId"))); m.put("PackUom", str(ci(d, "UOMCodeItem"))); m.put("PackEquivalent", toDouble(ci(d, "PackEquivalent")));
        m.put("JobLotId", toInt(ci(d, "JobLotId"))); m.put("ItemQty", toDouble(ci(d, "ItemQty")));
        m.put("ItemRate", rate - cut); m.put("RateUomId", toInt(ci(d, "UomScheduleIdRate"))); m.put("RateUOM", str(ci(d, "UOMCodeRate")));
        m.put("RateEquivalent", toDouble(ci(d, "RateUOM"))); m.put("AddLessRate", cut); m.put("NetRate", rate);
        m.put("DiscPrcnt", toDouble(ci(d, "ItemDiscount"))); m.put("DiscAmount", toDouble(ci(d, "ItemDiscountAmount")));
        m.put("ItemAmount", toDouble(ci(d, "ItemAmount"))); m.put("FcyAmount", toDouble(ci(d, "FcyAmount")));
        m.put("TaxNameId", toInt(ci(d, "TaxNameId"))); m.put("TaxPrcnt", toDouble(ci(d, "TaxPercent"))); m.put("TaxAmount", toDouble(ci(d, "TaxAmount")));
        m.put("Freights", toDouble(ci(d, "FreightAmount"))); m.put("Expense", toDouble(ci(d, "ExpenseAmount"))); m.put("Journal", toDouble(ci(d, "JournalAmount")));
        m.put("Commission", toDouble(ci(d, "CommissionAmount"))); m.put("BillAmount", toDouble(ci(d, "BillAmount")));
        m.put("VehicleNo", str(ci(d, "VehicleNo"))); m.put("GpNo", toInt(ci(d, "GpNo")));
        m.put("PaymentTermId", toInt(ci(d, "PaymentTermId"))); m.put("DueDays", toInt(ci(d, "DueDays"))); m.put("DueDate", dateOr1900(ci(d, "DueDate")));
        int bt = toInt(ci(d, "BillCalculateTypeId"));
        m.put("BillTypeId", bt != 2 ? 1 : 2); m.put("BillType", bt == 2 ? "On Qty" : "On Weight");
        return m;
    }

    private static Object dateOr1900(Object o) {
        if (o == null || String.valueOf(o).isBlank()) return "1900-01-01";
        return o;
    }

    // ------------------------------------------------------------------ History

    /** HistoryComboBind: Usp_AllComboAgainstSaleInvoice, the 'Customer' rows. */
    public Map<String, Object> historyCombos() {
        UserAccount u = sup.user();
        List<Map<String, Object>> cust = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Usp_AllComboAgainstSaleInvoice", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "AppId", sup.ctx().currentAppId(), "UserId", u.getId(), "DocumentTypeIds", String.valueOf(DOC_TYPE)))
            if ("Customer".equals(str(ci(r, "Activity")))) cust.add(row("Id", toInt(ci(r, "Id")), "CustomerName", str(ci(r, "ReferenceName"))));
        return row("customers", cust);
    }

    /** GetAll: InvSaleInvoice.FormHistory. */
    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate, int fromNo, int toNo, int customerId) {
        boolean all = Boolean.TRUE.equals(sup.rights(SCREEN).get("viewAll"));
        LocalDate f = parseDate(fromDate), t = parseDate(toDate);
        String mode = dateType == null ? "document" : dateType;
        return repo.historyByUser(sup.user(), DOC_TYPE, sup.fy(), all, mode, f == null ? null : f.atStartOfDay(), t == null ? null : t.atStartOfDay(), fromNo, toNo, customerId);
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
        for (String k : List.of("DocNo", "DocDate", "SupplierCustomerId", "CommissionDebitAcGLId", "SupplierReferenceNo", "ManualBillNo", "TaxAccountId", "TransporterId",
                "TransporterCreditPartyId", "TransporterDebitGLId", "TransporterDebitPartyId", "FreightAmount", "CommissionAgentId", "CommRate", "CommAmount", "CommissionType",
                "UomScheduleIdCmRate", "CommissionRemarks", "RemarksHeader", "DeliveryTerm", "PaymentTermId", "DueDays", "DueDate", "BillAmount", "CurrencyId", "ExchangeRate",
                "DiscountAmount", "DiscountAccountId", "FcyAmount", "IsApproved", "FreightRemark"))
            head.put(k, ci(inv, k));
        head.put("Id", id);
        List<Map<String, Object>> lines = new ArrayList<>();
        for (Map<String, Object> d : children(inv, "details")) lines.add(storedLine(d));
        List<Map<String, Object>> exp = new ArrayList<>();
        for (Map<String, Object> e : children(inv, "expenses"))
            exp.add(row("ItemId", toInt(ci(e, "InvRevExpItemId")), "Qty", toDouble(ci(e, "Qty")), "Rate", toDouble(ci(e, "Rate")), "Amount", toDouble(ci(e, "Amount")), "Remarks", str(ci(e, "Remarks"))));
        List<Map<String, Object>> jl = new ArrayList<>();
        for (Map<String, Object> j : children(inv, "journals"))
            jl.add(row("AccountId", c.sub ? toInt(ci(j, "TransporterSupCustId")) : toInt(ci(j, "ChartofAccountId")), "GlAccountId", toInt(ci(j, "ChartofAccountId")),
                    "Remarks", str(ci(j, "JvRemarks")), "Percentage", toDouble(ci(j, "JvPrcnt")), "Qty", toDouble(ci(j, "JvQty")), "Rate", toDouble(ci(j, "JvRate")),
                    "Debit", toDouble(ci(j, "JvDebit")), "Credit", toDouble(ci(j, "JvCredit"))));
        List<Map<String, Object>> comm = new ArrayList<>();
        for (Map<String, Object> k : children(inv, "commissions"))
            comm.add(row("Id", toInt(ci(k, "Id")), "OrderId", toInt(ci(k, "SaleOrderId")), "OrderNo", toInt(ci(k, "OrderNo")), "InvoiceId", toInt(ci(k, "InvSaleInvoiceId")),
                    "CommissionAgentId", toInt(ci(k, "CommissionAgentId")), "CommType", str(ci(k, "CommType")).replaceAll("\\.0+$", ""), "CommRate", toDouble(ci(k, "Rate")),
                    "CommUom", str(ci(k, "RateUom")).replaceAll("\\.0+$", ""), "CommissionAmount", toDouble(ci(k, "CommAmount")), "DebitAccountId", toInt(ci(k, "DebitAccountId")),
                    "Remarks", str(ci(k, "Remarks"))));
        List<Map<String, Object>> pay = new ArrayList<>();
        for (Map<String, Object> p : children(inv, "paymentTerms"))
            pay.add(row("PaymentTermId", toInt(ci(p, "PaymentTermId")), "%ofTotal", toDouble(ci(p, "PrcntOfTotal")), "Amount", toDouble(ci(p, "Amount")),
                    "DueDays", toInt(ci(p, "DueDays")), "DueDate", ci(p, "DueDate"), "Remarks", str(ci(p, "PaymentRemarks"))));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", head);
        out.put("lines", lines);
        out.put("expenses", exp);
        out.put("journals", jl);
        out.put("commissions", comm);
        out.put("payments", pay);
        out.put("deliveryTypeId", !lines.isEmpty() && toInt(lines.get(0).get("SaleOrderId")) > 0 ? 1 : 2);
        out.put("voucherHeadId", repo.voucherHeadId(sup.org(), sup.company(), DOC_TYPE, id));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    /** GetDetailGrdByHeadId: the rows of the highlighted history invoice. */
    public List<Map<String, Object>> historyDetail(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : children(invoice(id), "details")) {
            int bt = toInt(ci(d, "BillCalculateTypeId"));
            out.add(row("SaleOrder", ci(d, "SaleOrder"), "WareHouseName", ci(d, "WareHouseName"), "ItemName", ci(d, "ItemName"), "ItemUOM", ci(d, "UOMCodeItem"),
                    "JobLot", ci(d, "JobLotDescription"), "ItemQty", toDouble(ci(d, "ItemQty")), "ItemRate", toDouble(ci(d, "ItemRate")) - toDouble(ci(d, "RateCut")),
                    "RateUOM", ci(d, "UOMCodeRate"), "AddLessRate", toDouble(ci(d, "RateCut")), "NetRate", toDouble(ci(d, "ItemRate")),
                    "DiscPrcnt", toDouble(ci(d, "ItemDiscount")), "DiscAmount", toDouble(ci(d, "ItemDiscountAmount")), "ItemAmount", toDouble(ci(d, "ItemAmount")),
                    "FcyAmount", toDouble(ci(d, "FcyAmount")), "TaxName", ci(d, "TaxName"), "TaxPrcnt", toDouble(ci(d, "TaxPercent")), "TaxAmount", toDouble(ci(d, "TaxAmount")),
                    "ExpenseAmount", toDouble(ci(d, "ExpenseAmount")), "FreightAmount", toDouble(ci(d, "FreightAmount")), "BillAmount", toDouble(ci(d, "BillAmount")),
                    "GpNo", ci(d, "GpNo"), "VehicleNo", ci(d, "VehicleNo"), "PaymentTerm", ci(d, "PaymentTerm"), "DueDays", ci(d, "DueDays"), "DueDate", ci(d, "DueDate"),
                    "BillType", bt == 1 ? "On Weight" : "On Qty"));
        }
        return out;
    }

    // ------------------------------------------------------------------ the request (common to calc and save)

    public static class Req {
        public int id;
        public String docNo;
        public String docDate;
        public int customerId;
        public int customerGlId;                 // preview only; save derives it from the party list
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
        public int deliveryTypeId;
        public double headCommAmount;            // preview only; save reads the stored value
        public List<Map<String, Object>> lines = new ArrayList<>();
        public List<Map<String, Object>> expenses = new ArrayList<>();
        public List<Map<String, Object>> journals = new ArrayList<>();
        public List<Map<String, Object>> payments = new ArrayList<>();
        public List<Map<String, Object>> comm = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    private static final class Totals {
        double bill, fcy, itemAmount, tax, exp, qty;
    }

    private static double d(Map<String, Object> m, String k) { return toDouble(m.get(k)); }

    private static double round(double v, int dec) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return 0.0;
        return BigDecimal.valueOf(v).setScale(Math.max(0, dec), RoundingMode.HALF_UP).doubleValue();
    }

    /** Convert.ToInt32(double): banker's rounding. */
    private static int convInt(double v) { return (int) Math.rint(v); }

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
        List<Map<String, Object>> lines = copy(r.lines), comm = copy(r.comm);
        Totals t = compute(c, r, lines, comm, r.customerGlId, r.headCommAmount);
        return row("lines", lines, "comm", comm, "billAmount", t.bill, "fcyAmount", t.fcy, "itemAmount", t.itemAmount, "taxAmount", t.tax, "expAmount", t.exp);
    }

    /**
     * Runs, in the order Insert() leaves them: every row's cell formulas, txtExchangeRate_TextChanged, CalculateCommissionGrid,
     * PropotionateCommissiongrid, ExpProportion, FreightProportion, LedgerProportion, BillAmount (+ BillProportion).
     */
    private Totals compute(Cfg c, Req r, List<Map<String, Object>> lines, List<Map<String, Object>> comm, int customerGl, double headComm) {
        Totals t = new Totals();
        int type = r.deliveryTypeId;
        double rate = r.exchangeRate;
        for (Map<String, Object> m : lines) {
            String calc = str(m.get("calc"));
            double qty = d(m, "ItemQty");
            if (!calc.isEmpty()) {
                double base = d(m, "ItemRate"), addLess = d(m, "AddLessRate");
                double net = addLess != 0.0 ? base + addLess : base;
                m.put("NetRate", net);
                if ("discAmt".equals(calc)) {                               // DiscAmount edited
                    double eq = d(m, "RateEquivalent");
                    double amount = eq == 0.0 ? 0.0 : qty / eq * net;
                    double da = d(m, "DiscAmount");
                    m.put("DiscPrcnt", amount == 0.0 ? 0.0 : da * 100.0 / amount);
                    m.put("ItemAmount", round(amount - da, 4));
                } else if ("rate".equals(calc)) {                           // ItemRate / RateUOM / AddLessRate / DiscPrcnt edited
                    if (type != 1) m.put("RateEquivalent", toDouble(m.get("RateUOM")));
                    double eq = d(m, "RateEquivalent");
                    double amount = eq == 0.0 ? 0.0 : qty / eq * net;
                    double da = amount * d(m, "DiscPrcnt") / 100.0;
                    m.put("DiscAmount", da);
                    m.put("ItemAmount", round(amount - da, 4));
                }
                int taxType = toInt(m.get("TaxNameId"));
                double pct = d(m, "TaxPrcnt"), itemAmount = d(m, "ItemAmount");
                if (pct > 100.0) pct = 100.0;                               // "Tax % Can't be greater than 100..."
                if (itemAmount > 0.0 && pct > 0.0 && taxType > 0) { m.put("TaxPrcnt", pct); m.put("TaxAmount", round(itemAmount * pct / 100.0, 4)); }
                else { m.put("TaxPrcnt", 0.0); m.put("TaxAmount", 0.0); }
            }
            m.put("FcyAmount", rate > 0.0 ? d(m, "ItemAmount") / rate : 0.0);                    // txtExchangeRate_TextChanged
            t.qty += qty; t.itemAmount += d(m, "ItemAmount"); t.tax += d(m, "TaxAmount");
        }
        calculateCommissionGrid(c, comm, lines);
        propotionateCommission(c, comm, lines);
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
        // LedgerProportion (txtcommamount is the stored header value, 0 for a new invoice)
        double jCredit = 0, jDebit = 0, jlCredit = 0, jlDebit = 0;
        for (Map<String, Object> j : r.journals) {
            jlCredit += d(j, "Credit"); jlDebit += d(j, "Debit");
            if (toInt(j.get("AccountId")) > 0) { jCredit += d(j, "Credit"); jDebit += d(j, "Debit"); }
        }
        double credit = jlCredit + headComm;
        for (Map<String, Object> m : lines) m.put("Journal", jlDebit > credit && t.qty != 0.0 ? (jlDebit - credit) / t.qty * d(m, "ItemQty") : 0.0);
        // BillAmount
        double bill = 0.0;
        if (!lines.isEmpty()) {
            if (!c.sub) {
                if (r.freightCrId > 0 && customerGl == r.freightCrId) bill -= r.freightAmount;
                if (r.freightDrId > 0 && customerGl == r.freightDrId) bill += r.freightAmount;
            } else {
                if (r.freightCrId > 0 && r.customerId == r.freightCrId) bill -= r.freightAmount;
                if (r.freightDrId > 0 && r.customerId == r.freightDrId) bill += r.freightAmount;
            }
            bill = bill + t.itemAmount + t.exp + t.tax;
            double diff = round(jCredit - jDebit, c.amtRound);
            bill = diff < 0.0 ? bill - Math.abs(diff) : bill + diff;
            double partyComm = 0.0;
            for (Map<String, Object> k : comm) if (r.customerId == toInt(k.get("CommissionAgentId"))) partyComm += d(k, "CommissionAmount");
            bill = bill - partyComm - r.discountAmount;
            t.bill = round(bill, 4);                                       // txtBillAmount.Text = ToString("#,##0.####")
            t.fcy = rate > 0.0 ? round(t.bill / rate, 4) : 0.0;
        }
        // BillProportion
        boolean plain = !c.creditInSaleGl.isEmpty() && !toBool(c.creditInSaleGl);
        for (Map<String, Object> m : lines) {
            double ia = d(m, "ItemAmount");
            m.put("BillAmount", plain ? ia : round(ia + d(m, "Expense") + d(m, "TaxAmount") - d(m, "Freights") - d(m, "Commission"), 4));
        }
        return t;
    }

    /** CalculateCommissionGrid. */
    private void calculateCommissionGrid(Cfg c, List<Map<String, Object>> comm, List<Map<String, Object>> lines) {
        for (Map<String, Object> k : comm) {
            int orderId = toInt(k.get("OrderId"));
            double total = 0.0;
            for (Map<String, Object> m : lines) if (orderId <= 0 || toInt(m.get("SaleOrderId")) == orderId) total += d(m, "ItemAmount");
            String type = str(k.get("CommType"));
            if ("Percent".equals(type)) {
                double a = total * d(k, "CommRate") / 100.0;
                k.put("CommissionAmount", c.commToExp ? round(a, c.amtRound) : a);
            } else if ("Flat".equals(type)) {
                double a = d(k, "CommRate");
                k.put("CommissionAmount", c.commToExp ? round(a, c.amtRound) : a);
            }
        }
    }

    /** PropotionateCommissiongrid. */
    private void propotionateCommission(Cfg c, List<Map<String, Object>> comm, List<Map<String, Object>> lines) {
        for (Map<String, Object> m : lines) m.put("Commission", 0.0);
        if (c.commToExp) return;
        double totalItem = 0.0;
        for (Map<String, Object> m : lines) totalItem += d(m, "ItemAmount");
        for (Map<String, Object> k : comm) {
            String type = str(k.get("CommType"));
            int orderId = toInt(k.get("OrderId"));
            for (Map<String, Object> m : lines) {
                if (orderId > 0 && toInt(m.get("SaleOrderId")) != orderId) continue;
                double pre = d(m, "Commission"), ia = d(m, "ItemAmount");
                if ("Percent".equals(type)) m.put("Commission", ia * d(k, "CommRate") / 100.0 + pre);
                else if ("Flat".equals(type)) m.put("Commission", (totalItem == 0.0 ? 0.0 : ia / totalItem * 100.0 * d(k, "CommRate") / 100.0) + pre);
            }
        }
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
        List<Map<String, Object>> lines = copy(r.lines), expenses = copy(r.expenses), journals = copy(r.journals), payments = copy(r.payments), comm = copy(r.comm);
        if (lines.isEmpty()) throw new Warning("Grid Record Not Found");

        // ---- lookups that the combos of the desktop are filled from (tenant scope)
        List<Map<String, Object>> customers = customers(), terms = terms(), currencies = sup.rows("Sp_MultiCurrency_GetAllMethod", "OrganizationId", sup.org(),
                "CompanyId", sup.company(), "Activity", "ReadAll");
        List<Map<String, Object>> freightAcs = freightAccounts(c), taxAcs = accountTitles("3,6,8,16,17,18,19", null), discAcs = discountAccounts();
        List<Map<String, Object>> others = otherItems();
        Map<String, Object> customer = null;
        for (Map<String, Object> x : customers) if (toInt(x.get("Id")) == r.customerId) customer = x;
        int customerGl = customer == null ? 0 : toInt(customer.get("GlAccountId"));
        String fKey = c.sub ? "SupplierCustomerId" : "Id";
        if (!has(freightAcs, fKey, r.freightCrId)) r.freightCrId = 0;
        if (!has(freightAcs, fKey, r.freightDrId)) r.freightDrId = 0;
        if (!has(taxAcs, "Id", r.taxAccountId)) r.taxAccountId = 0;
        if (!has(discAcs, "Id", r.discountAccountId)) r.discountAccountId = 0;

        Map<String, Object> stored = null;
        double storedComm = 0.0;
        Map<Integer, Map<String, Object>> storedLines = new HashMap<>();
        if (r.id > 0) {
            stored = invoice(r.id);
            storedComm = toDouble(ci(stored, "CommAmount"));
            for (Map<String, Object> d : children(stored, "details")) storedLines.put(toInt(ci(d, "Id")), storedLine(d));
        }

        // ---- the source of every row is the database, not the page (GDN rows / the invoice's own rows)
        int type = r.deliveryTypeId;
        Set<Integer> needGdn = new LinkedHashSet<>();
        for (Map<String, Object> m : lines) {
            int id = toInt(m.get("Id"));
            if (id > 0) {
                Map<String, Object> s = storedLines.get(id);
                if (s == null || toInt(s.get("GdnDetailId")) != toInt(m.get("GdnDetailId"))) throw new Warning("InvGdnDetailId not found");
            } else if (toInt(m.get("GdnId")) > 0) needGdn.add(toInt(m.get("GdnId")));
        }
        Map<Integer, Map<String, Object>> gdnByDetail = new HashMap<>();
        if (!needGdn.isEmpty()) {
            for (Map<String, Object> g : gdnRows(needGdn)) gdnByDetail.putIfAbsent(toInt(ci(g, "Id")), g);
        }
        for (Map<String, Object> m : lines) {
            int id = toInt(m.get("Id"));
            Map<String, Object> src;
            if (id > 0) src = storedLines.get(id);
            else {
                Map<String, Object> g = gdnByDetail.get(toInt(m.get("GdnDetailId")));
                if (g == null || toInt(ci(g, "InvGdnId")) != toInt(m.get("GdnId"))) {
                    if (toInt(m.get("GdnDetailId")) == 0) throw new Warning("InvGdnDetailId not found");
                    if (toInt(m.get("GdnId")) == 0) throw new Warning("Gdn Id not found");
                    throw new Warning("InvGdnDetailId not found");
                }
                src = gdnLine(g, type);
            }
            mergeSource(c, m, src, type);
        }
        // the customer of the loaded rows must be the invoice customer (LoadDataDetailGridAgainstGP)
        for (Integer gid : needGdn) if (toInt(ci(gdnHeader(gid), "SupplierCustomerId")) != r.customerId) throw new Warning("You Can't Load GDN Of Different Customer At Same Time");

        r.customerGlId = customerGl;
        Totals t = compute(c, r, lines, comm, customerGl, storedComm);

        // ---- FormValidation(), in the desktop's order
        String docNoText = r.docNo == null ? "" : r.docNo.trim();
        if (docNoText.isEmpty() || docNoText.equals("0")) throw new Warning("DocNo  field is Required");
        boolean currencyOk = has(currencies, "Id", r.currencyId);
        if (c.multi) {
            if (!currencyOk) throw new Warning("Fcy Code Field is Required");
            if (!(r.exchangeRate > 0)) throw new Warning("Exchange Rate Field is Required");
            if (t.fcy == 0.0) throw new Warning("Fcy Amount Field is Required");
        } else {
            if (!currencyOk) throw new Warning("Please Configure Your Base Currency In configurations");
            if (!(r.exchangeRate > 0)) throw new Warning("Please Configure Your Base Currency Rate In configurations");
        }
        if (customer == null) throw new Warning("Customer Name field is Required");
        String deliveryTerm = r.deliveryTerm == null ? "" : r.deliveryTerm;
        if (!deliveryTerm.equals("Load") && !deliveryTerm.equals("Ponch")) throw new Warning("Delivery Term field is Required");
        if (!has(terms, "Id", r.paymentTermId)) throw new Warning("Payment Term field is Required");
        int dueDays = toInt(r.dueDays);
        if (r.paymentTermId == 2 && dueDays <= 0) throw new Warning("Due Days field is Required");
        if (!(r.freightAmount > 0.0) && (r.freightCrId > 0 || r.freightDrId > 0)) throw new Warning("Freight Amount Field is Required");
        if (r.freightAmount > 0.0 && r.freightCrId <= 0) throw new Warning("Freight/Ac Cr Field is Required");
        if (r.freightAmount > 0.0 && (r.freightRemarks == null || r.freightRemarks.trim().isEmpty())) throw new Warning("Freight Remarks Field is Required");

        // ---- Insert(): discount, grids, commission, tax
        if (r.discountAccountId > 0 && r.discountAmount == 0.0) throw new Warning("Discount Amount Required When Discount Account Is Present...");
        if (r.discountAmount != 0.0 && r.discountAccountId == 0) throw new Warning("Discount Ac Required When Discount Amount Is Present...");
        if (r.discountAmount >= t.bill) throw new Warning("Discount Amount Can not be Greater than or Equal to Bill Amount");
        for (Map<String, Object> j : journals)
            if ((d(j, "Credit") > 0.0 || toInt(j.get("Debit")) > 0) && toInt(j.get("AccountId")) == 0) throw new Warning("Please Select an Account Against JL First");
        for (Map<String, Object> e : expenses)
            if (d(e, "Amount") > 0.0 && toInt(e.get("ItemId")) == 0) throw new Warning("Please Select an Item Against Expense First");
        Map<String, Object> firstComm = comm.isEmpty() ? null : comm.get(0);
        int commAgent0 = firstComm == null ? 0 : toInt(firstComm.get("CommissionAgentId"));
        String commType0 = firstComm == null ? "" : str(firstComm.get("CommType"));
        boolean commSame = true;
        double commAmt = 0.0;
        for (Map<String, Object> k : comm) {
            commAmt += d(k, "CommissionAmount");
            if (d(k, "CommissionAmount") > 0.0) {
                if (toInt(k.get("CommissionAgentId")) == 0) throw new Warning("Please Select Commission Agent In Commission Grid");
                if (toInt(k.get("CommissionAgentId")) != commAgent0 || !str(k.get("CommType")).equals(commType0)) commSame = false;
                if (c.commToExp && toInt(k.get("DebitAccountId")) == 0) throw new Warning("Debit Account Feild Required In Commission Grid ...");
            }
        }
        double taxTotal = 0.0;
        for (Map<String, Object> m : lines) {
            if (d(m, "TaxAmount") > 0.0) {
                taxTotal += d(m, "TaxAmount");
                if (r.taxAccountId == 0) throw new Warning("Tax Account is Required");
            }
        }

        // ---- header
        SaleInvoiceModels.Head h = new SaleInvoiceModels.Head();
        LocalDateTime now = LocalDateTime.now();
        LocalDate docDate = parseDate(r.docDate);
        if (docDate == null) throw new Warning("DocNo  field is Required");
        h.Id = r.id;
        h.DocNo = r.id > 0 ? toInt(ci(stored, "DocNo")) : repo.nextDocNo(u, sup.fy(), DOC_TYPE);
        if (h.DocNo == 0) throw new Warning("DocNo  field is Required");
        h.BranchesId = u.getBranchesId();
        h.DocDate = docDate.atStartOfDay();
        h.DocumentTypeId = DOC_TYPE;
        h.ManualBillNo = text(r.manualBillNo);
        h.RemarksHeader = text(r.remarks);
        h.SupplierCustomerId = r.customerId;
        h.SupplierInvoiceDate = now;
        h.SupplierInvoiceNo = h.DocNo;
        h.SupplierReferenceNo = text(r.refNo);
        h.ScreenName = SCREEN;
        if (!c.sub) { h.TransporterId = r.freightCrId; h.TransporterCreditPartyId = 0; h.TransporterDebitGLId = r.freightDrId; h.TransporterDebitPartyId = 0; }
        else {
            for (Map<String, Object> a : freightAcs) {
                if (r.freightCrId > 0 && toInt(a.get("SupplierCustomerId")) == r.freightCrId) { h.TransporterId = toInt(a.get("Id")); h.TransporterCreditPartyId = toInt(a.get("SupplierCustomerId")); }
                if (r.freightDrId > 0 && toInt(a.get("SupplierCustomerId")) == r.freightDrId) { h.TransporterDebitGLId = toInt(a.get("Id")); h.TransporterDebitPartyId = toInt(a.get("SupplierCustomerId")); }
            }
        }
        h.FreightAmount = r.freightAmount;
        h.FreightRemark = text(r.freightRemarks);
        h.TaxAccountId = r.taxAccountId;
        h.IsTaxable = r.taxAccountId > 0 && taxTotal > 0.0;
        h.UomScheduleIdCmRate = "";
        h.BillAmount = t.bill;
        if (commSame && commAmt > 0.0) {                                                    // IsCommsame && CommissionAmt > 0
            h.CommissionAgentId = commAgent0;
            h.CommissionDebitAcGLId = firstComm == null ? 0 : toInt(firstComm.get("DebitAccountId"));
            h.CommissionType = commType0;
            double sumRate = 0.0;
            for (Map<String, Object> k : comm) sumRate += d(k, "CommRate");
            h.CommRate = comm.isEmpty() ? 0.0 : sumRate / comm.size();                       // Janus Average
            h.CommAmount = commAmt;
            h.CommissionRemarks = firstComm == null ? "" : str(firstComm.get("Remarks"));
        }
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.FinancialYearId = sup.fy();
        h.EntryUser = u.getId();
        h.EntryDate = now;
        h.ModifyDate = now;
        h.ModifyUser = u.getId();
        h.DiscountAmount = BigDecimal.valueOf(r.discountAmount);
        h.DiscountAccountId = r.discountAccountId;
        h.ExchangeRate = BigDecimal.valueOf(r.exchangeRate);
        h.CurrencyId = r.currencyId;
        h.DeliveryTerm = deliveryTerm;
        h.PaymentTermId = r.paymentTermId;
        h.DueDays = dueDays;
        LocalDate dd = parseDate(r.dueDate);
        h.DueDate = (dd != null ? dd : docDate.plusDays(dueDays)).atStartOfDay();
        h.FcyAmount = BigDecimal.valueOf(t.fcy);
        h.AttachmentsValues = r.id > 0 && stored != null && ci(stored, "AttachmentsValues") != null ? str(ci(stored, "AttachmentsValues")) : "";
        h.CustomAttachmentsValues = r.id > 0 && stored != null && ci(stored, "CustomAttachmentsValues") != null ? str(ci(stored, "CustomAttachmentsValues")) : "";

        // ---- detail rows (per row checks, in the desktop's order)
        SaleInvoiceFinancialDirect.Invoice inv = new SaleInvoiceFinancialDirect.Invoice();
        inv.h = h;
        BigDecimal exRate = BigDecimal.valueOf(r.exchangeRate);
        int rowNo = 0;
        for (Map<String, Object> m : lines) {
            rowNo++;
            SaleInvoiceModels.Detail pd = new SaleInvoiceModels.Detail();
            pd.LineId = rowNo;
            pd.Id = toInt(m.get("Id"));
            pd.SaleOrderId = toInt(m.get("SaleOrderId"));
            pd.SaleOrderDetailId = toInt(m.get("SaleOrderDetailId"));
            if (type == 1 && pd.SaleOrderId == 0) throw new Warning("SaleOrderId Field Required");
            pd.InvGdnDetailId = toInt(m.get("GdnDetailId"));
            if (pd.InvGdnDetailId == 0) throw new Warning("InvGdnDetailId not found");
            pd.InvGdnId = toInt(m.get("GdnId"));
            if (pd.InvGdnId == 0) throw new Warning("Gdn Id not found");
            pd.ItemId = toInt(m.get("ItemId"));
            if (pd.ItemId == 0) throw new Warning("Item Required");
            pd.ItemQty = d(m, "ItemQty");
            if (pd.ItemQty == 0.0) throw new Warning("ItemQty mustbe greater than zero");
            pd.ItemUOMId = toInt(m.get("PackUomId"));
            if (pd.ItemUOMId == 0) throw new Warning("ItemUOMId Field Required");
            pd.JobLotId = toInt(m.get("JobLotId"));
            if (pd.JobLotId == 0) throw new Warning("JobLot Required");
            pd.GrossWeight = pd.ItemQty; pd.AdLsWeight = 0.0; pd.NetBillWeight = pd.ItemQty; pd.NetStockWeight = pd.ItemQty;
            pd.ItemRate = d(m, "NetRate");
            if (pd.ItemRate == 0.0) throw new Warning("ItemRate Field Required");
            int rateUom = convInt(d(m, "RateEquivalent"));
            if (type != 1) {
                List<Map<String, Object>> sched = sup.rows("Sp_UOMSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", pd.ItemId, "Activity", "ReadByItemID");
                if (!sched.isEmpty()) {
                    Map<String, Object> hit = null;
                    for (Map<String, Object> s : sched) if (hit == null && toDouble(ci(s, "Equivalent")) == (double) rateUom) hit = s;
                    if (hit == null) throw new Warning("RateUom of the Item " + pd.ItemId + " in row No " + rowNo + " is not define please check ");
                    pd.UomScheduleIdRate = toInt(ci(hit, "Id"));
                }
            } else pd.UomScheduleIdRate = toInt(m.get("RateUomId"));
            if (pd.UomScheduleIdRate == 0) throw new Warning("Rate UOM Required");
            pd.RateCut = d(m, "AddLessRate");
            pd.ItemAmount = round(d(m, "ItemAmount"), c.amtRound);
            if (pd.ItemAmount == 0.0) throw new Warning("ItemAmount Field Required");
            pd.ExchangeRate = exRate;
            pd.CurrencyId = r.currencyId;
            pd.FcyAmount = BigDecimal.valueOf(d(m, "FcyAmount"));
            pd.WarehouseId = toInt(m.get("WarehouseId"));
            if (pd.WarehouseId == 0) throw new Warning("Warehouse Required");
            pd.ItemDiscount = d(m, "DiscPrcnt");
            pd.ItemDiscountAmount = d(m, "DiscAmount");
            pd.TaxNameId = toInt(m.get("TaxNameId"));
            pd.TaxPercent = d(m, "TaxPrcnt");
            pd.TaxAmount = d(m, "TaxAmount");
            pd.IsTaxable = pd.TaxNameId > 0 && pd.TaxPercent > 0.0 && pd.TaxAmount > 0.0 ? "true" : "false";
            pd.BillAmount = round(d(m, "BillAmount"), c.amtRound);
            if (!c.freightToExp) pd.FreightAmount = d(m, "Freights");
            pd.JournalAmount = d(m, "Journal");
            pd.ExpenseAmount = d(m, "Expense");
            if (!c.commToExp) pd.CommissionAmount = d(m, "Commission");
            pd.GpDate = now;
            pd.GpNo = toInt(m.get("GpNo"));
            pd.VehicleNo = str(m.get("VehicleNo"));
            pd.PaymentTermId = toInt(m.get("PaymentTermId"));
            pd.BillCalculateTypeId = toInt(m.get("BillTypeId"));
            pd.DueDays = toInt(m.get("DueDays"));
            LocalDate ld = parseDate(m.get("DueDate"));
            pd.DueDate = (ld == null ? LocalDate.of(1900, 1, 1) : ld).atStartOfDay();
            inv.details.add(pd);
        }

        // ---- expenses, journals
        Map<String, Integer> glOf = new HashMap<>();
        for (Map<String, Object> a : freightAcs) glOf.put("f" + a.get("SupplierCustomerId"), toInt(a.get("Id")));
        for (Map<String, Object> e : expenses) {
            if (toInt(e.get("ItemId")) != 0 && d(e, "Amount") != 0.0) {
                if (!has(others, "Id", toInt(e.get("ItemId")))) throw new Warning("Please Select an Item Against Expense First");
                SaleInvoiceModels.Expense pe = new SaleInvoiceModels.Expense();
                pe.InvRevExpItemId = toInt(e.get("ItemId"));
                pe.Qty = toInt(e.get("Qty") == null ? null : (Object) (double) convInt(d(e, "Qty")));        // Conversion.ToInt(Qty): a whole number
                pe.Rate = d(e, "Rate");
                pe.Amount = d(e, "Amount");
                pe.Remarks = str(e.get("Remarks"));
                inv.expenses.add(pe);
            }
        }
        for (Map<String, Object> j : journals) {
            int acc = toInt(j.get("AccountId"));
            if (acc == 0) continue;
            if (d(j, "Debit") > 0.0 && d(j, "Credit") > 0.0) throw new Warning(d(j, "Debit") > 0.0 ? "Debit Side is aleady added" : "Credit Side is aleady added");
            int gl;
            if (c.sub) {
                if (!glOf.containsKey("f" + acc)) throw new Warning("Please Select an Account Against JL First");
                gl = glOf.get("f" + acc);
                if (acc == r.customerId) throw new Warning("Selected Account Can not be Same As Supplier Account");
            } else {
                if (!has(freightAcs, "Id", acc)) throw new Warning("Please Select an Account Against JL First");
                gl = acc;
                if (gl == customerGl) throw new Warning("Selected Account Can not be Same As Supplier Account");
            }
            SaleInvoiceModels.Journal pj = new SaleInvoiceModels.Journal();
            pj.ChartofAccountId = gl;
            pj.TransporterSupCustId = c.sub ? acc : 0;
            pj.JvRemarks = str(j.get("Remarks"));
            pj.JvPrcnt = d(j, "Percentage"); pj.JvQty = d(j, "Qty"); pj.JvRate = d(j, "Rate"); pj.JvDebit = d(j, "Debit"); pj.JvCredit = d(j, "Credit");
            inv.journals.add(pj);
        }

        // ---- payment terms
        BigDecimal payAmount = BigDecimal.ZERO, payPct = BigDecimal.ZERO;
        double sumPay = 0.0;
        for (Map<String, Object> p : payments) sumPay += d(p, "Amount");
        if (!payments.isEmpty() && sumPay > 0.0) {
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
            payAmount = o.Amount;
            o.DueDays = dueDays;
            o.DueDate = h.DueDate;
            o.PaymentRemarks = "";
            inv.paymentTerms.add(o);
        }
        if (payPct.compareTo(new BigDecimal("100")) != 0) throw new Warning("Payment Detail Total% not equal to 100");
        if (payAmount.compareTo(BigDecimal.valueOf(h.BillAmount)) != 0)
            throw new Warning("Payment Detail Amount " + fmtMax(payAmount.doubleValue(), 4) + " Not Equal to Total Invoice Amount " + fmtMax(h.BillAmount, 4));

        // ---- commission rows
        Set<Integer> partyIds = new HashSet<>();
        for (Map<String, Object> x : customers) partyIds.add(toInt(x.get("Id")));
        for (Map<String, Object> k : comm) {
            if (toInt(k.get("CommissionAgentId")) != 0 && d(k, "CommissionAmount") > 0.0) {
                if (!partyIds.contains(toInt(k.get("CommissionAgentId")))) throw new Warning("Please Select Commission Agent In Commission Grid");
                SaleInvoiceModels.Commission pj = new SaleInvoiceModels.Commission();
                pj.Id = toInt(k.get("Id"));
                pj.SaleOrderId = toInt(k.get("OrderId"));
                pj.InvSaleInvoiceId = toInt(k.get("InvoiceId"));
                pj.DebitAccountId = toInt(k.get("DebitAccountId"));
                pj.CommissionAgentId = toInt(k.get("CommissionAgentId"));
                String ct = str(k.get("CommType"));
                if (ct.equals("Percent")) pj.CommType = 2.0;
                if (ct.equals("Flat")) pj.CommType = 1.0;
                if (ct.equals("Comm Weight")) pj.CommType = 3.0;
                if (toDouble(ct) > 0.0) pj.CommType = toDouble(ct);
                pj.Rate = d(k, "CommRate");
                pj.RateUom = d(k, "CommUom");
                pj.CommAmount = d(k, "CommissionAmount");
                pj.Remarks = str(k.get("Remarks"));
                inv.commissions.add(pj);
            }
        }

        // ---- BLL 0580 Save + DAL 0433 SetData (one transaction)
        if (h.Id == 0) h.ModifyUser = 0; else h.EntryUser = 0;
        SaleInvoiceFinancialDirect.Voucher voucher = new SaleInvoiceFinancialDirect(repo).makeVoucherForSaleInvoice(inv);
        int saved = persist.persist(inv, voucher, h.Id == 0 ? "Sp_InvSaleInvoice_Insert" : "Sp_InvSaleInvoice_Update");
        attachments.apply(SCREEN, DOC_TYPE, saved, r.customerId, r.attachments);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", saved);
        out.put("docNo", h.DocNo);
        out.put("voucherHeadId", repo.voucherHeadId(u.getOrganizationId(), u.getCompanyId(), DOC_TYPE, saved));
        out.put("message", (r.id == 0 ? "Record saved Successfully" : "Record Update Successfully") + h.DocNo);
        return out;
    }

    /**
     * Puts the database's version of the identity / source columns on the row the page sent. Rows the user did not edit (calc empty)
     * take the source's amounts too; edited rows keep the editable inputs (rate for a non order row, add/less, discount, tax, vehicle, gate pass).
     */
    private void mergeSource(Cfg c, Map<String, Object> m, Map<String, Object> src, int type) {
        String calc = str(m.get("calc"));
        Map<String, Object> mine = new LinkedHashMap<>(m);
        m.clear();
        m.putAll(src);
        m.put("calc", calc);
        m.put("VehicleNo", str(mine.get("VehicleNo")));
        m.put("GpNo", toInt(mine.get("GpNo")));
        if (calc.isEmpty()) return;
        m.put("AddLessRate", d(mine, "AddLessRate"));
        m.put("DiscPrcnt", d(mine, "DiscPrcnt"));
        m.put("DiscAmount", d(mine, "DiscAmount"));
        if (type != 1) {                                                   // ItemRate / RateUOM are editable only when there is no sale order
            m.put("ItemRate", d(mine, "ItemRate"));
            m.put("RateUOM", str(mine.get("RateUOM")));
            m.put("RateEquivalent", d(mine, "RateEquivalent"));
        }
        if (c.taxEditable) { m.put("TaxNameId", toInt(mine.get("TaxNameId"))); m.put("TaxPrcnt", d(mine, "TaxPrcnt")); }
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
