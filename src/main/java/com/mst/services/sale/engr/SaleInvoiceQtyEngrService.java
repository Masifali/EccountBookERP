package com.mst.services.sale.engr;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.repositories.support.DesktopProc;
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
 * Screen 139 "SaleInvoiceQtyWithTax" = Architecture.WinApp.SaleTrading.SaleInvoiceQtyWithTax (Sale Engr, module 83, document type 1608).
 *
 * Desktop map (siqT.cs = SaleInvoiceQtyWithTax.cs; method : line):
 *   Load :~330, FormValidation :1136, FormValidationDetila :~1230, Reset, btnRefresh_Click, btnAdd_Click :2216, grd_DoubleClick :2242,
 *   btnUpdateDetail_Click, grd_CellUpdated :~2440, grdSettings, grdPaymentTerm_* , grdGLedger_* , grdInvExp_* , CalculateAmount / CalculateTaxAmount,
 *   Insert :2667, ReadById :~3040, BillAmount :~3190, ExpProportion :~3275, FreightProportion :~3300, LedgerProportion :~3332, BillProportion :~3395,
 *   GetAll (history) :3951, HistoryGridSettings :4033, DocDate_Leave, toolStripButton3_Click (Load Order), LoadInGridDetail, print buttons.
 *   Loader dialog: LoadSaleOrderWithQty.cs (PendingOrderLoad :86, btnLoadOnInvoice_Click_1 :288).
 * BLL 0580 InvSaleInvoice.Save (Id 0: ModifyUser 0, Sp_InvSaleInvoice_Insert; else EntryUser 0, Sp_InvSaleInvoice_Update) + DAL SetData
 * (SaleInvoiceEngrPersist) + SaleInvoiceFinancialDirect.makeVoucherForSaleInvoice, one transaction.
 *
 * Grid arithmetic lives here: the page posts its rows to {@link #calc} and Save runs the same code on rows whose source (the stored invoice
 * line / the sale order line) is re-read from the database, so saved numbers never depend on what the browser computed.
 * A row carries a sticky "calc" flag: "entry" (row added from the entry panel), "amt" (ItemQty / AddLessRate / DiscountType / Discount% cell edited),
 * "tax" (any other cell edited), "" (untouched: the source values are kept).
 */
@Service
public class SaleInvoiceQtyEngrService {
    public static final String SCREEN = "SaleInvoiceQtyWithTax";
    public static final int DOC_TYPE = 1608;
    public static final int SO_TYPE = 1605;

    private final SaleEngrSupport sup;
    private final SaleEngrAttachments attachments;
    private final SaleInvoiceRepository repo;
    private final SaleInvoiceEngrPersist persist;

    public SaleInvoiceQtyEngrService(SaleEngrSupport sup, SaleEngrAttachments attachments, SaleInvoiceRepository repo, SaleInvoiceEngrPersist persist) {
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
        boolean sub, freightToExp, taxEditable, multi;
        int amtRound, amtPlaces, ratePlaces, fcyPlaces;
    }

    private Cfg cfg() {
        return cached("cfg", () -> {
            Cfg c = new Cfg();
            c.sub = sup.erpFeature(4);                                                  // SubsidiaryAccountAllownOnVouchers
            c.multi = sup.erpFeature(6);                                                // HasMultiCurrencyFeature
            Map<String, String> m = new HashMap<>();
            for (Map<String, Object> r : sup.rows("Sp_ConfigrationsAllocation_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                    "ConfigDescriptions", "DebitAmountChargetoExpenseAcFreightGrid,TaxPercentEditable", "Activity", "GetMultipleConfigurationsByConfigDescriptions"))
                m.put(str(ci(r, "ConfigDescription")), str(ci(r, "ConfigKey")));
            c.freightToExp = toBool(m.get("DebitAmountChargetoExpenseAcFreightGrid"));
            c.taxEditable = toBool(m.get("TaxPercentEditable"));
            int amt = toInt(sup.config("Default NoofDecimal Points For Amount"));
            int rate = toInt(sup.config("Default NoofDecimal Points For Rate"));
            int fcy = toInt(sup.config("DefaultNoOfDecimalPointsForFcyAmount"));
            c.amtRound = Math.max(0, Math.min(10, amt));
            c.amtPlaces = amt >= 1 && amt <= 4 ? amt : 0;                                  // CommonServices.GetDecimalConfiguration
            c.ratePlaces = rate >= 1 && rate <= 4 ? rate : (rate == 0 ? 2 : 0);
            c.fcyPlaces = fcy >= 1 && fcy <= 4 ? fcy : 0;
            return c;
        });
    }

    // ------------------------------------------------------------------ Load

    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>(lists(LocalDate.now().toString()));
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());                                         // DocumentNo
        m.put("lastDiscountAcId", lastDiscountAcId());                     // PreviousDiscountAc
        m.put("lastTaxAcId", lastTaxAcId());                               // PreviousSalesTaxAc
        m.put("history", historyCombos());                                 // HistoryComboFill
        m.put("fyStart", fyStart());
        m.put("branchId", sup.branch());
        return m;
    }

    /** btnRefresh_Click: the lists are loaded again (the desktop's AccountsFill overwrites the account list; the web reloads the right ones). */
    public Map<String, Object> refresh(String taxDate) {
        cache.clear();
        Map<String, Object> m = new LinkedHashMap<>(lists(taxDate));
        m.put("lastDiscountAcId", 0);
        return m;
    }

    private Map<String, Object> lists(String taxDate) {
        Cfg c = cfg();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("customers", customers());
        m.put("terms", terms());
        m.put("deliveryTerms", List.of(row("Id", 1, "DeliveryTerm", "Load"), row("Id", 2, "DeliveryTerm", "Ponch")));
        m.put("discountTypes", List.of(row("Id", 1, "Name", "Flat"), row("Id", 2, "Name", "Percent")));
        m.put("currencies", currencies());
        m.put("otherItems", otherItems());
        m.put("freightAccounts", freightAccounts(c));
        m.put("taxAccounts", accountTitles("3,6,8,16,17,18,19", null));
        m.put("discountAccounts", discountAccounts());
        m.put("taxTypes", taxTypes(taxDate));
        m.put("items", items());
        m.put("warehouses", warehouses());
        m.put("jobLots", jobLots());
        m.put("cities", cities());
        m.put("multiCurrency", c.multi);
        m.put("subsidiary", c.sub);
        m.put("freightDebitToExpenses", c.freightToExp);
        m.put("taxEditable", c.taxEditable);
        m.put("defaults", row("baseCurrency", sup.configInt("Base Currency"), "baseRate", toDouble(sup.config("BaseCurrencyRate")),
                "cityId", sup.configInt("City Area"), "jobLotId", sup.configInt("Job/Lot")));
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

    public List<Map<String, Object>> currencies() {
        return sup.rows("Sp_MultiCurrency_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadAll");
    }

    /** InventoryItemsOther.GetAll. */
    public List<Map<String, Object>> otherItems() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.otherItems(sup.user())) out.add(row("Id", toInt(ci(r, "Id")), "OtherItemName", str(ci(r, "OtherItemName"))));
        return out;
    }

    /** itemsFill: Sp_Item_GetAllMethod 'AllItemsBindForEngr'. */
    public List<Map<String, Object>> items() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_Item_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "AllItemsBindForEngr"))
            out.add(row("Id", toInt(ci(r, "Id")), "ItemName", str(ci(r, "ItemName")), "ItemCode", str(ci(r, "ItemCodeNew"))));
        return out;
    }

    /** bindWareHouse: Sp_InvWareHouse_GetAllMethod 'GetActiveWareHouse'. */
    public List<Map<String, Object>> warehouses() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_InvWareHouse_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetActiveWareHouse"))
            out.add(row("Id", toInt(ci(r, "Id")), "WareHouseName", str(ci(r, "WareHouseName"))));
        return out;
    }

    /** JobLotFill: SP_JobLot_ReadMethod 'GetAll'. */
    public List<Map<String, Object>> jobLots() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("SP_JobLot_ReadMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll"))
            out.add(row("Id", toInt(ci(r, "Id")), "JobLotDescription", str(ci(r, "JobLotDescription"))));
        return out;
    }

    /** CityFill: SP_City_GetAllMethod @MethodType 'GetAll'. */
    public List<Map<String, Object>> cities() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("SP_City_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll"))
            out.add(row("Id", toInt(ci(r, "Id")), "CityName", str(ci(r, "CityName"))));
        return out;
    }

    /** CommonServices.GetUomScheduleByItemId -> Sp_UOMSchedule_GetAllMethod 'ReadByItemID'. */
    public List<Map<String, Object>> uoms(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_UOMSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "ReadByItemID"))
            out.add(row("Id", toInt(ci(r, "Id")), "UOMCode", str(ci(r, "UOMCode")), "Equivalent", toDouble(ci(r, "Equivalent"))));
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
    public List<Map<String, Object>> taxTypes(String date) {
        LocalDate d = parseDate(date);
        if (d == null) d = LocalDate.now();
        final LocalDate fd = d;
        return cached("tax|" + fd, () -> {
            List<Map<String, Object>> out = new ArrayList<>();
            for (Map<String, Object> r : sup.rows("Sp_ItemTaxSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "EffectedDate", fd,
                    "TaxTypeId", 2, "Activity", "GetAllTaxSchedule"))
                out.add(row("TaxNameId", toInt(ci(r, "TaxNameId")), "TaxName", str(ci(r, "TaxName")), "TaxPercent", toDouble(ci(r, "TaxPercent"))));
            return out;
        });
    }

    private Map<Integer, Double> taxMap(String date) {
        Map<Integer, Double> m = new HashMap<>();
        for (Map<String, Object> t : taxTypes(date)) m.put(toInt(t.get("TaxNameId")), toDouble(t.get("TaxPercent")));
        return m;
    }

    /** CommonServices.GetTaxScheduleDetailbyItemIds (DocDate_Leave): Sp_ItemTaxSchedule_GetAllMethod 'GetTaxScheduleDetailbyItemIds'. */
    public List<Map<String, Object>> taxByItems(String itemIds, String date) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company());
        p.put("ItemIds", itemIds == null ? "" : itemIds);
        p.put("EffectedDate", parseDate(date));
        p.put("Activity", "GetTaxScheduleDetailbyItemIds");
        return DesktopProc.rows(sup.jdbc(), "Sp_ItemTaxSchedule_GetAllMethod", p);
    }

    /** DocumentNo: InvSaleInvoice.GenerateInvSaleInvoiceCode (document type 1608). */
    public int nextNo() { return repo.nextDocNo(sup.user(), sup.fy(), DOC_TYPE); }

    /** PreviousDiscountAc: InvSaleInvoice.GetLastDiscountAcId. */
    public int lastDiscountAcId() {
        List<Map<String, Object>> r = sup.rows("Sp_InvSaleInvoice_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetLastDiscountAcId");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "DiscountAccountId"));
    }

    /** PreviousSalesTaxAc: InvSaleInvoice.GetLastSaleTaxAcId. */
    public int lastTaxAcId() {
        List<Map<String, Object>> r = sup.rows("Sp_InvSaleInvoice_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetLastSaleTaxAcId");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "SaleTaxAcId"));
    }

    /** cmbCurrency_Leave: VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher (document type 1608). */
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

    // ------------------------------------------------------------------ Load Order dialog (LoadSaleOrderWithQty)

    /** CmbSupplier of the dialog: SaleOrder.GetDataForDropDownFromSaleOrder, Activity 'Customer'. */
    public Map<String, Object> loaderCombos() {
        UserAccount u = sup.user();
        List<Map<String, Object>> cust = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_GetDataForDropDownFromSaleOrder", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "AppId", sup.ctx().currentAppId(), "UserId", u.getId()))
            if ("Customer".equals(str(ci(r, "Activity")))) cust.add(row("Id", toInt(ci(r, "Id")), "Customer", str(ci(r, "ReferenceName"))));
        return row("customers", cust, "fromDate", fyStart());
    }

    /** PendingOrderLoad: SaleOrder.SaleOrderSlipAndRegister_Engr (USP_SaleOrderSlipAndRegister_Engr), document type 1605, ZeroBalanceType 1. */
    private List<Map<String, Object>> pendingOrders(int orderId, String fromDate, String toDate, int fromNo, int toNo, int customerId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("BranchesId", sup.branch()); p.put("FinancialYearId", sup.fy());
        LocalDate f = parseDate(fromDate), t = parseDate(toDate);
        if (f != null) p.put("FromDate", f);
        if (t != null) p.put("Todate", t);
        if (fromNo > 0) p.put("FromDocNo", fromNo);
        if (toNo > 0) p.put("ToDocNo", toNo);
        if (orderId > 0) p.put("Id", orderId);
        if (customerId > 0) p.put("SupplierCustomerId", customerId);
        p.put("DocumentTypeIds", String.valueOf(SO_TYPE));
        p.put("SkipZero", 1);
        return DesktopProc.rows(sup.jdbc(), "USP_SaleOrderSlipAndRegister_Engr", p);
    }

    /** The dialog's dtHistory columns (desktop names) plus the grid line LoadInGridDetail builds from the row ("line"). */
    private Map<String, Object> loaderRow(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", toInt(ci(r, "Id"))); m.put("OrderDetailId", toInt(ci(r, "PoDId")));
        m.put("DocDate", ci(r, "DocDate")); m.put("DocNo", toInt(ci(r, "DocNo"))); m.put("RemarksHeader", str(ci(r, "RemarksHeader")));
        m.put("SupplierCustomerId", toInt(ci(r, "OrderSupCustId"))); m.put("CustomerName", str(ci(r, "SupplierName"))); m.put("SupplierRefNo", str(ci(r, "SupplierRefNo")));
        m.put("BillCalculateTypeId", toInt(ci(r, "BillCalculateTypeId"))); m.put("PaymentTermsId", toInt(ci(r, "PaymentTermsId"))); m.put("PaymentTerm", str(ci(r, "TermsDescription")));
        m.put("DueDays", toInt(ci(r, "OrderDueDays"))); m.put("OrderDueDate", ci(r, "OrderDueDate")); m.put("DeliveryTerm", str(ci(r, "DeliveryTerm")));
        m.put("ItemId", toInt(ci(r, "OrderItemId"))); m.put("ItemCode", str(ci(r, "ItemCode"))); m.put("ItemName", str(ci(r, "ItemName")));
        m.put("JobLotId", toInt(ci(r, "JobLotId"))); m.put("JobLot", str(ci(r, "JobLotCode")));
        m.put("PackUomId", toInt(ci(r, "PackUomId"))); m.put("PackUom", str(ci(r, "PackUom"))); m.put("PackEquivalent", toDouble(ci(r, "PackEquivalent")));
        m.put("ItemQty", toDouble(ci(r, "OrderItemQty"))); m.put("DispatchQty", toDouble(ci(r, "DispatchQty"))); m.put("BalQty", toDouble(ci(r, "BalQty")));
        m.put("Weight", toDouble(ci(r, "NetWeight"))); m.put("DispatchWeight", toDouble(ci(r, "DispatchWeight"))); m.put("BalWeight", toDouble(ci(r, "BalWeight")));
        m.put("ItemRate", toDouble(ci(r, "OrderItemRate"))); m.put("RateUomId", toInt(ci(r, "RateUomId"))); m.put("RateUom", str(ci(r, "RateUom")));
        m.put("RateEquivalent", toDouble(ci(r, "EquivalentRate")));
        m.put("CityId", toInt(ci(r, "CityId"))); m.put("CityName", str(ci(r, "CityName")));
        m.put("TaxTypeId", toInt(ci(r, "TaxNameId"))); m.put("TaxType", str(ci(r, "TaxName"))); m.put("TaxPrcnt", toDouble(ci(r, "TaxPercent")));
        m.put("TaxAmount", toDouble(ci(r, "TaxAmount"))); m.put("ItemAmount", toDouble(ci(r, "ItemAmount"))); m.put("DetailRemarks", str(ci(r, "DetailRemarks")));
        m.put("line", orderLine(m));
        return m;
    }

    /** LoadInGridDetail's dtGrid.Rows.Add(...) for one checked order row. */
    private Map<String, Object> orderLine(Map<String, Object> o) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", 0); m.put("OrderId", toInt(o.get("Id"))); m.put("OrderDetailId", toInt(o.get("OrderDetailId")));
        m.put("OrderNo", toInt(o.get("DocNo"))); m.put("OrderDate", o.get("DocDate")); m.put("WarehouseId", 0);
        m.put("ItemId", toInt(o.get("ItemId"))); m.put("ItemCode", str(o.get("ItemCode"))); m.put("ItemName", str(o.get("ItemName")));
        m.put("PackUomId", toInt(o.get("PackUomId"))); m.put("PackUom", str(o.get("PackUom"))); m.put("PackEquivalent", toDouble(o.get("PackEquivalent")));
        m.put("JobLotId", toInt(o.get("JobLotId"))); m.put("JobLot", str(o.get("JobLot")));
        m.put("ItemQty", toDouble(o.get("BalQty"))); m.put("ItemRate", toDouble(o.get("ItemRate")));
        m.put("RateUomId", toInt(o.get("RateUomId"))); m.put("RateUom", str(o.get("RateUom"))); m.put("RateEquivalent", toDouble(o.get("RateEquivalent")));
        m.put("AddLessRate", 0.0); m.put("NetRate", toDouble(o.get("ItemRate")));
        m.put("DiscountType", 2); m.put("DiscPct", 0.0); m.put("DiscountAmount", 0.0);
        m.put("ItemAmount", toDouble(o.get("ItemAmount"))); m.put("FcyAmount", 0.0);
        m.put("CityId", toInt(o.get("CityId"))); m.put("GpDate", LocalDate.now().toString()); m.put("GpNo", 0); m.put("VehicleNo", "");
        m.put("TaxTypeId", toInt(o.get("TaxTypeId"))); m.put("TaxPct", toDouble(o.get("TaxPrcnt"))); m.put("TaxAmount", toDouble(o.get("TaxAmount")));
        m.put("Expense", 0.0); m.put("Freights", 0.0); m.put("Journal", 0.0); m.put("BillAmount", 0.0);
        return m;
    }

    public List<Map<String, Object>> loaderRows(String fromDate, String toDate, int fromNo, int toNo, int customerId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : pendingOrders(0, fromDate, toDate, fromNo, toNo, customerId)) out.add(loaderRow(r));
        return out;
    }

    /** LoadPaymentDetailBySaleOrderIds: InvSaleInvoice.SaleOrderPaymentTermDetailBySoIds. */
    public List<Map<String, Object>> orderPayments(String orderIds) {
        List<Map<String, Object>> pay = new ArrayList<>();
        for (Map<String, Object> r : repo.saleOrderPaymentTerms(orderIds == null ? "" : orderIds))
            pay.add(row("PaymentTermId", toInt(ci(r, "PaymentTermId")), "%ofTotal", toDouble(ci(r, "PrcntOfTotal")), "Amount", toDouble(ci(r, "Amount")),
                    "DueDays", toInt(ci(r, "DueDays")), "DueDate", ci(r, "DueDate"), "Remarks", str(ci(r, "PaymentRemarks"))));
        return pay;
    }

    // ------------------------------------------------------------------ History

    /** HistoryComboFill: Usp_AllComboAgainstSaleInvoice, DocumentTypeIds 1608: 'Supplier' = customers, 'PaymentTerm' = payment terms. */
    public Map<String, Object> historyCombos() {
        UserAccount u = sup.user();
        List<Map<String, Object>> cust = new ArrayList<>(), terms = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Usp_AllComboAgainstSaleInvoice", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "AppId", sup.ctx().currentAppId(), "UserId", u.getId(), "DocumentTypeIds", String.valueOf(DOC_TYPE))) {
            String a = str(ci(r, "Activity"));
            if (a.equals("Supplier") || a.equals("Customer")) cust.add(row("Id", toInt(ci(r, "Id")), "CustomerName", str(ci(r, "ReferenceName"))));
            else if (a.equals("PaymentTerm")) terms.add(row("Id", toInt(ci(r, "Id")), "TermsDescription", str(ci(r, "ReferenceName"))));
        }
        return row("customers", cust, "terms", terms);
    }

    /** GetAll: InvSaleInvoice.FormHistory (Sp_InvSaleInvoice_GetAllMethod 'FormHistory'), plus the payment term filter of this screen. */
    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate, int fromNo, int toNo, int customerId, int paymentTermId) {
        boolean all = Boolean.TRUE.equals(sup.rights(SCREEN).get("viewAll"));
        UserAccount u = sup.user();
        LocalDate f = parseDate(fromDate), t = parseDate(toDate);
        String fromKey, toKey;
        switch (dateType == null ? "" : dateType) {
            case "entry": fromKey = "EntryFromDate"; toKey = "EntryToDate"; break;
            case "modify": fromKey = "ModifyFromDate"; toKey = "ModifyToDate"; break;
            case "approved": fromKey = "ApprovedFromDate"; toKey = "ApprovedToDate"; break;
            default: fromKey = "FromDate"; toKey = "ToDate"; break;
        }
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId());
        p.put("DocumentTypeId", DOC_TYPE); p.put("CanViewAllRecord", all);
        if (sup.fy() != 0) p.put("FinancialYearId", sup.fy());
        if (f != null) p.put(fromKey, f.atStartOfDay());
        if (t != null) p.put(toKey, t.atStartOfDay());
        if (fromNo != 0) p.put("DocNoFrom", fromNo);
        if (toNo != 0) p.put("DocNoTo", toNo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (paymentTermId != 0) p.put("PaymentTermId", paymentTermId);
        if (!all) p.put("EntryUser", u.getId());
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(sup.jdbc(), "Sp_InvSaleInvoice_GetAllMethod", p);
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

    /** ReadById's dtGrid.Rows.Add(...) from an InvSaleInvoiceDetail row. */
    private Map<String, Object> storedLine(Map<String, Object> d) {
        double rate = toDouble(ci(d, "ItemRate")), cut = toDouble(ci(d, "RateCut"));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", toInt(ci(d, "Id"))); m.put("OrderId", toInt(ci(d, "SaleOrderId"))); m.put("OrderDetailId", toInt(ci(d, "SaleOrderDetailId")));
        m.put("OrderNo", toInt(ci(d, "SaleOrder"))); m.put("OrderDate", ci(d, "OrderDate")); m.put("WarehouseId", toInt(ci(d, "WarehouseId")));
        m.put("ItemId", toInt(ci(d, "ItemId"))); m.put("ItemCode", str(ci(d, "ItemCode"))); m.put("ItemName", str(ci(d, "ItemName")));
        m.put("PackUomId", toInt(ci(d, "ItemUOMId"))); m.put("PackUom", str(ci(d, "UOMCodeItem"))); m.put("PackEquivalent", toDouble(ci(d, "PackEquivalent")));
        m.put("JobLotId", toInt(ci(d, "JobLotId"))); m.put("JobLot", str(ci(d, "JobLotDescription")));
        m.put("ItemQty", toDouble(ci(d, "ItemQty")));
        m.put("ItemRate", rate - cut); m.put("RateUomId", toInt(ci(d, "UomScheduleIdRate"))); m.put("RateUom", str(ci(d, "UOMCodeRate")));
        m.put("RateEquivalent", toDouble(ci(d, "RateUOM"))); m.put("AddLessRate", cut); m.put("NetRate", rate);
        m.put("DiscountType", toInt(ci(d, "DiscountTypeId"))); m.put("DiscPct", toDouble(ci(d, "ItemDiscount"))); m.put("DiscountAmount", toDouble(ci(d, "ItemDiscountAmount")));
        m.put("ItemAmount", toDouble(ci(d, "ItemAmount"))); m.put("FcyAmount", toDouble(ci(d, "FcyAmount")));
        m.put("CityId", toInt(ci(d, "CityId"))); m.put("GpDate", ci(d, "GpDate")); m.put("GpNo", toInt(ci(d, "GpNo"))); m.put("VehicleNo", str(ci(d, "VehicleNo")));
        m.put("TaxTypeId", toInt(ci(d, "TaxNameId"))); m.put("TaxPct", toDouble(ci(d, "TaxPercent"))); m.put("TaxAmount", toDouble(ci(d, "TaxAmount")));
        m.put("Expense", toDouble(ci(d, "ExpenseAmount"))); m.put("Freights", toDouble(ci(d, "FreightAmount"))); m.put("Journal", toDouble(ci(d, "JournalAmount")));
        m.put("BillAmount", toDouble(ci(d, "BillAmount")));
        return m;
    }

    public Map<String, Object> record(int id) {
        Cfg c = cfg();
        Map<String, Object> inv = invoice(id);
        Map<String, Object> head = new LinkedHashMap<>();
        for (String k : List.of("DocNo", "DocDate", "SupplierCustomerId", "SupplierReferenceNo", "ManualBillNo", "TaxAccountId", "TransporterId",
                "TransporterCreditPartyId", "TransporterDebitGLId", "TransporterDebitPartyId", "FreightAmount", "RemarksHeader", "DeliveryTerm", "PaymentTermId", "DueDays",
                "DueDate", "BillAmount", "CurrencyId", "ExchangeRate", "DiscountAmount", "DiscountAccountId", "FcyAmount", "IsApproved", "FreightRemark"))
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
        out.put("saleOrderId", lines.isEmpty() ? 0 : toInt(lines.get(0).get("OrderId")));
        out.put("voucherHeadId", repo.voucherHeadId(sup.org(), sup.company(), DOC_TYPE, id));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    /** BindDetailOfHeaderId: the rows of the highlighted history invoice. */
    public List<Map<String, Object>> historyDetail(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        boolean multi = cfg().multi;
        for (Map<String, Object> d : children(invoice(id), "details")) {
            int dt = toInt(ci(d, "DiscountTypeId"));
            double cut = toDouble(ci(d, "RateCut"));
            out.add(row("OrderNo", ci(d, "SaleOrder"), "Warehouse", ci(d, "WareHouseName"), "ItemName", ci(d, "ItemName"), "Uom", ci(d, "UOMCodeItem"),
                    "JobLot", ci(d, "JobLotDescription"), "ItemQty", toDouble(ci(d, "ItemQty")), "ItemRate", toDouble(ci(d, "ItemRate")) - cut,
                    "RateUOM", ci(d, "UOMCodeRate"), "AddLessRate", cut, "NetRate", toDouble(ci(d, "ItemRate")),
                    "DiscountType", dt == 1 ? "Flat" : dt == 2 ? "Percent" : "", "DiscPct", toDouble(ci(d, "ItemDiscount")), "DiscountAmount", toDouble(ci(d, "ItemDiscountAmount")),
                    "ItemAmount", toDouble(ci(d, "ItemAmount")), "FcyAmount", toDouble(ci(d, "FcyAmount")), "CityName", ci(d, "CityName"), "GpDate", ci(d, "GpDate"),
                    "GpNo", ci(d, "GpNo"), "VehicleNo", ci(d, "VehicleNo"), "TaxName", ci(d, "TaxName"), "TaxPct", toDouble(ci(d, "TaxPercent")), "TaxAmount", toDouble(ci(d, "TaxAmount"))));
        }
        return out;
    }

    // ------------------------------------------------------------------ the request (common to calc, validate and save)

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
        public List<Map<String, Object>> lines = new ArrayList<>();
        public List<Map<String, Object>> expenses = new ArrayList<>();
        public List<Map<String, Object>> journals = new ArrayList<>();
        public List<Map<String, Object>> payments = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    private static final class Totals {
        double bill, fcy, itemAmount, tax, exp, qty;
    }

    private static double d(Map<String, Object> m, String k) { return toDouble(m.get(k)); }

    /** Math.Round(v, dec, MidpointRounding.AwayFromZero) and ToString("#,##0.00") rounding. */
    private static double round(double v, int dec) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return 0.0;
        return BigDecimal.valueOf(v).setScale(Math.max(0, dec), RoundingMode.HALF_UP).doubleValue();
    }

    /** Math.Round(v, 2) (banker's, exact binary value). */
    private static double bankers2(double v) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return 0.0;
        return new BigDecimal(v).setScale(2, RoundingMode.HALF_EVEN).doubleValue();
    }

    /** Convert.ToInt32(double): banker's rounding. */
    private static int convInt(double v) { return Double.isNaN(v) || Double.isInfinite(v) ? 0 : (int) Math.rint(v); }

    private static double fin(double v) { return Double.isNaN(v) || Double.isInfinite(v) ? 0.0 : v; }

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

    private static boolean has(Collection<Map<String, Object>> rows, String key, int id) {
        for (Map<String, Object> m : rows) if (toInt(ci(m, key)) == id) return true;
        return false;
    }

    // ------------------------------------------------------------------ the grid arithmetic

    /** grd_CellUpdated, ItemQty / AddLessRate / DiscountType / Discount%: ItemAmount stays unrounded. */
    private void cellAmount(Map<String, Object> m) {
        double qty = d(m, "ItemQty"), base = d(m, "ItemRate"), addLess = d(m, "AddLessRate"), eq = d(m, "RateEquivalent");
        double net = addLess != 0.0 ? base + addLess : base;
        m.put("NetRate", net);
        double amount = eq == 0.0 ? 0.0 : fin(qty / eq * net);
        int dt = toInt(m.get("DiscountType"));
        double pct = d(m, "DiscPct"), da;
        if (dt == 1) da = pct > 0.0 ? pct : 0.0;
        else if (dt == 2) {
            if (pct > 100.0) pct = 100.0;                                  // the page tells the user: Discount Percent Can't Greater Than 100.
            da = pct > 0.0 ? amount * pct / 100.0 : 0.0;
        } else { da = 0.0; pct = 0.0; }
        m.put("DiscPct", pct);
        m.put("DiscountAmount", da);
        m.put("ItemAmount", amount - da);
    }

    /** The always-run tax block of grd_CellUpdated (and the TaxTypeId / Tax% branches when the tax cell was edited). */
    private void cellTax(Cfg c, Map<String, Object> m, Map<Integer, Double> tax, boolean taxCell) {
        int type = toInt(m.get("TaxTypeId"));
        double pct = d(m, "TaxPct");
        if (taxCell) {
            if (type == 0) pct = 0.0;
            else if (!(pct > 0.0) && tax.containsKey(type)) pct = tax.get(type);
        }
        double ia = d(m, "ItemAmount");
        if (pct > 100.0) pct = 100.0;                                      // "Tax % Can't be greater than 100..."
        if (ia > 0.0 && pct > 0.0 && type > 0) { m.put("TaxPct", pct); m.put("TaxAmount", round(ia * pct / 100.0, c.amtRound)); }
        else { m.put("TaxPct", 0.0); m.put("TaxAmount", 0.0); }
    }

    /** CalculateAmount + CalculateTaxAmount of the entry panel, as the row is added with btnAdd. */
    private void entryCalc(Cfg c, Map<String, Object> m, Map<Integer, Double> tax) {
        double qty = d(m, "ItemQty"), eq = d(m, "RateEquivalent");
        double base = round(d(m, "ItemRate"), c.ratePlaces), addLess = d(m, "AddLessRate");
        m.put("ItemRate", base);
        double net = addLess != 0.0 ? base + addLess : base;
        int dt = toInt(m.get("DiscountType"));
        double pct = d(m, "DiscPct"), da = 0.0;
        if (qty > 0.0 && base > 0.0 && eq > 0.0) {
            m.put("NetRate", round(net, c.ratePlaces));
            double amount = fin(qty / eq * net);
            if (dt == 1) da = pct > 0.0 ? pct : 0.0;
            else if (dt == 2) {
                if (pct > 100.0) pct = 100.0;
                da = pct > 0.0 ? amount * pct / 100.0 : 0.0;
            } else { da = 0.0; pct = 0.0; }
            m.put("DiscPct", pct);
            m.put("DiscountAmount", round(da, 3));
            amount -= da;
            m.put("ItemAmount", round(round(amount, c.amtRound), c.amtPlaces));
        } else {
            m.put("NetRate", 0.0); m.put("ItemAmount", 0.0); m.put("DiscountAmount", 0.0);
            if (dt != 1 && dt != 2) m.put("DiscPct", 0.0);
        }
        int type = toInt(m.get("TaxTypeId"));
        double ia = d(m, "ItemAmount");
        if (type > 0 && tax.containsKey(type)) {
            double lp = tax.get(type);
            double p = c.taxEditable ? d(m, "TaxPct") : lp;
            if (!(p > 0.0)) p = lp;
            if (p > 100.0) p = 100.0;
            m.put("TaxPct", p); m.put("TaxAmount", round(ia * p / 100.0, c.amtPlaces));
        } else { m.put("TaxTypeId", 0); m.put("TaxPct", 0.0); m.put("TaxAmount", 0.0); }
    }

    /** The page's preview: the numbers Insert() would carry for the rows as they stand. */
    public Map<String, Object> calc(Req r) {
        Cfg c = cfg();
        List<Map<String, Object>> lines = copy(r.lines), exps = copy(r.expenses);
        Totals t = compute(c, r, lines, exps, taxMap(r.docDate), customerGl(r.customerId));
        return row("lines", lines, "expenses", exps, "billAmount", t.bill, "fcyAmount", t.fcy, "itemAmount", t.itemAmount, "taxAmount", t.tax, "expAmount", t.exp);
    }

    private int customerGl(int customerId) {
        List<Map<String, Object>> cs = cached("cust", this::customers);
        for (Map<String, Object> x : cs) if (toInt(x.get("Id")) == customerId) return toInt(x.get("GlAccountId"));
        return 0;
    }

    /**
     * Runs the formulas in the order the desktop leaves them: every row's cell / entry formula, txtExchangeRate_TextChanged (FcyAmount),
     * grdInvExp amounts, ExpProportion, FreightProportion, LedgerProportion, BillAmount, BillProportion.
     */
    private Totals compute(Cfg c, Req r, List<Map<String, Object>> lines, List<Map<String, Object>> exps, Map<Integer, Double> tax, int customerGl) {
        Totals t = new Totals();
        double rate = r.exchangeRate;
        for (Map<String, Object> m : lines) {
            String calc = str(m.get("calc"));
            if (calc.equals("entry")) entryCalc(c, m, tax);
            else if (calc.equals("amt")) { cellAmount(m); cellTax(c, m, tax, false); }
            else if (calc.equals("tax")) cellTax(c, m, tax, true);
            m.put("FcyAmount", rate > 0.0 ? fin(d(m, "ItemAmount") / rate) : 0.0);                    // txtExchangeRate_TextChanged
            t.qty += d(m, "ItemQty"); t.itemAmount += d(m, "ItemAmount"); t.tax += d(m, "TaxAmount");
        }
        for (Map<String, Object> e : exps) {                                                         // grdInvExp_CellUpdated: Amount = Round(Qty * Rate, amountRound)
            e.put("Amount", round(d(e, "Qty") * d(e, "Rate"), c.amtRound));
            t.exp += d(e, "Amount");
        }
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
        // LedgerProportion (all rows of grdGLedger) and the BillAmount sums (rows with an account)
        double allCredit = 0, allDebit = 0, jCredit = 0, jDebit = 0;
        for (Map<String, Object> j : r.journals) {
            allCredit += d(j, "Credit"); allDebit += d(j, "Debit");
            if (toInt(j.get("AccountId")) > 0) { jCredit += d(j, "Credit"); jDebit += d(j, "Debit"); }
        }
        for (Map<String, Object> m : lines)
            m.put("Journal", allDebit < allCredit && t.qty != 0.0 ? fin(Math.rint(allCredit - allDebit) / t.qty * d(m, "ItemQty")) : 0.0);
        // BillAmount
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
            t.bill = round(bill, c.amtPlaces);                                                       // txtBillAmount.Text = ToString(stringFormatsingle)
            t.fcy = rate > 0.0 ? round(fin(bill / rate), 4) : 0.0;                                   // txtFcyAmount.Text = ToString("#,##0.####")
        }
        // BillProportion
        for (Map<String, Object> m : lines) m.put("BillAmount", d(m, "ItemAmount") + d(m, "TaxAmount") + d(m, "Expense") + d(m, "Freights"));
        return t;
    }

    // ------------------------------------------------------------------ FormValidation

    private void formValidation(Cfg c, Req r, Totals t, List<Map<String, Object>> customers, List<Map<String, Object>> terms, List<Map<String, Object>> currencies) {
        String docNoText = r.docNo == null ? "" : r.docNo.trim();
        if (toInt(docNoText) == 0) throw new Warning("Doc No Name is Required");
        if (!has(customers, "Id", r.customerId)) throw new Warning("Customer Name is Required");
        if (!has(terms, "Id", r.paymentTermId)) throw new Warning("Payment Term is Required");
        if (r.paymentTermId == 2 && toInt(r.dueDays) == 0) throw new Warning("Due Days Field is Required");
        boolean currencyOk = r.currencyId != 0 && has(currencies, "Id", r.currencyId);
        if (c.multi) {
            if (!currencyOk) throw new Warning("Fcy Code Field is Required");
            if (!(r.exchangeRate != 0.0)) throw new Warning("Exchange Rate Field is Required");
            if (t.fcy == 0.0) throw new Warning("Fcy Amount Rate Field is Required");
        } else {
            if (!currencyOk) throw new Warning("Please Configure Your Base Currency In configurations");
            if (!(r.exchangeRate != 0.0)) throw new Warning("Please Configure Your Base Currency Rate In configurations");
        }
        String dtm = r.deliveryTerm == null ? "" : r.deliveryTerm;
        if (!dtm.equals("Load") && !dtm.equals("Ponch")) throw new Warning("Delivery Term is Required");
        if (!(r.freightAmount > 0.0) && (r.freightCrId > 0 || r.freightDrId > 0)) throw new Warning("Freight Amount Field is Required");
        if (r.freightAmount > 0.0 && r.freightCrId <= 0) throw new Warning("Freight/Ac Cr Field is Required");
        if (r.freightAmount > 0.0 && (r.freightRemarks == null || r.freightRemarks.trim().isEmpty())) throw new Warning("Freight Remarks Field is Required");
    }

    private void sanitizeHeader(Cfg c, Req r) {
        List<Map<String, Object>> freightAcs = freightAccounts(c);
        String fKey = c.sub ? "SupplierCustomerId" : "Id";
        if (!has(freightAcs, fKey, r.freightCrId)) r.freightCrId = 0;
        if (!has(freightAcs, fKey, r.freightDrId)) r.freightDrId = 0;
        if (!has(accountTitles("3,6,8,16,17,18,19", null), "Id", r.taxAccountId)) r.taxAccountId = 0;
        if (!has(discountAccounts(), "Id", r.discountAccountId)) r.discountAccountId = 0;
    }

    /** The page calls this before it asks "Are you sure to Save?": FormValidation() only. */
    public Map<String, Object> validate(Req r) {
        Cfg c = cfg();
        sanitizeHeader(c, r);
        List<Map<String, Object>> lines = copy(r.lines), exps = copy(r.expenses);
        Totals t = compute(c, r, lines, exps, taxMap(r.docDate), customerGl(r.customerId));
        formValidation(c, r, t, customers(), terms(), currencies());
        return row("ok", true);
    }

    // ------------------------------------------------------------------ Save (Insert / Update)

    @Transactional
    public Map<String, Object> save(Req r) {
        UserAccount u = sup.user();
        Cfg c = cfg();
        Map<String, Boolean> rights = sup.rights(SCREEN);
        if (!Boolean.TRUE.equals(rights.get(r.id > 0 ? "update" : "save")))
            throw new Warning("You do not have the " + (r.id > 0 ? "Update" : "Save") + " right for this screen.");
        List<Map<String, Object>> lines = copy(r.lines), expenses = copy(r.expenses), journals = copy(r.journals), payments = copy(r.payments);

        // ---- the lookups the desktop combos are filled from (tenant scope)
        List<Map<String, Object>> customers = customers(), terms = terms(), currencies = currencies();
        List<Map<String, Object>> freightAcs = freightAccounts(c), others = otherItems();
        sanitizeHeader(c, r);
        Map<String, Object> customer = null;
        for (Map<String, Object> x : customers) if (toInt(x.get("Id")) == r.customerId) customer = x;
        int customerGl = customer == null ? 0 : toInt(customer.get("GlAccountId"));
        Map<Integer, Double> tax = taxMap(r.docDate);

        Map<String, Object> stored = null;
        Map<Integer, Map<String, Object>> storedLines = new HashMap<>();
        if (r.id > 0) {
            stored = invoice(r.id);
            for (Map<String, Object> d : children(stored, "details")) storedLines.put(toInt(ci(d, "Id")), storedLine(d));
        }

        // ---- the source of every row is the database, not the page
        Set<Integer> whIds = new HashSet<>(), cityIds = new HashSet<>(), jobIds = new HashSet<>(), itemIds = new HashSet<>();
        for (Map<String, Object> x : warehouses()) whIds.add(toInt(x.get("Id")));
        for (Map<String, Object> x : cities()) cityIds.add(toInt(x.get("Id")));
        for (Map<String, Object> x : jobLots()) jobIds.add(toInt(x.get("Id")));
        for (Map<String, Object> x : items()) itemIds.add(toInt(x.get("Id")));
        Map<Integer, Map<Integer, Map<String, Object>>> orderRows = new HashMap<>();   // orderId -> OrderDetailId -> loader row
        Map<Integer, List<Map<String, Object>>> uomCache = new HashMap<>();
        int firstOrder = 0;
        for (Map<String, Object> m : lines) {
            int id = toInt(m.get("Id")), od = toInt(m.get("OrderDetailId")), oid = toInt(m.get("OrderId"));
            Map<String, Object> mine = new LinkedHashMap<>(m);
            Map<String, Object> src = null;
            if (id > 0) {
                src = storedLines.get(id);
                if (src == null) throw new Warning("Sale invoice detail record not found");
            } else if (od > 0) {
                if (oid <= 0) throw new Warning("Sale order Id is required for a row loaded from a sale order");
                Map<Integer, Map<String, Object>> byDetail = orderRows.get(oid);
                if (byDetail == null) {
                    byDetail = new HashMap<>();
                    for (Map<String, Object> p : pendingOrders(oid, null, null, 0, 0, 0)) {
                        Map<String, Object> lr = loaderRow(p);
                        byDetail.put(toInt(lr.get("OrderDetailId")), lr);
                    }
                    orderRows.put(oid, byDetail);
                }
                Map<String, Object> lr = byDetail.get(od);
                if (lr == null) throw new Warning("Sale order row not found or already fully delivered");
                if (toInt(lr.get("SupplierCustomerId")) != r.customerId) throw new Warning("Sale order row belongs to a different customer");
                @SuppressWarnings("unchecked") Map<String, Object> ln = (Map<String, Object>) lr.get("line");
                src = ln;
            }
            int rowOrder = id > 0 ? toInt(src.get("OrderId")) : oid;
            if (rowOrder > 0) {
                if (firstOrder == 0) firstOrder = rowOrder;
                else if (firstOrder != rowOrder) throw new Warning("Already Loaded Row's Have Different Order. So you Can't Load Rows Of Different Order!");
            }
            boolean entryUpdate = id > 0 && str(mine.get("calc")).equals("entry") && toInt(src.get("OrderId")) == 0;   // a stored direct row re-written from the entry panel
            if (src != null && !entryUpdate) mergeSource(c, m, new LinkedHashMap<>(src), mine, whIds, cityIds, tax);
            else mergeEntry(c, m, id, whIds, cityIds, jobIds, itemIds, tax, uomCache);
        }

        Totals t = compute(c, r, lines, expenses, tax, customerGl);

        // ---- FormValidation(), then Insert()'s own checks, in the desktop's order
        formValidation(c, r, t, customers, terms, currencies);
        for (Map<String, Object> j : journals)
            if ((d(j, "Credit") > 0.0 || toInt(j.get("Debit")) > 0) && toInt(j.get("AccountId")) == 0) throw new Warning("Please Select an Account Against JL First");
        for (Map<String, Object> m : lines)
            if (d(m, "TaxAmount") > 0.0 && r.taxAccountId == 0) throw new Warning("Tax Account is Required");
        if (r.discountAccountId > 0 && r.discountAmount == 0.0) throw new Warning("Discount Amount Required When Discount Account Is Present...");
        if (r.discountAmount != 0.0 && r.discountAccountId == 0) throw new Warning("Discount Ac Required When Discount Amount Is Present...");
        if (r.discountAmount >= t.bill) throw new Warning("Discount Amount Can not be Greater than or Equal to Bill Amount");

        // ---- header
        SaleInvoiceModels.Head h = new SaleInvoiceModels.Head();
        LocalDateTime now = LocalDateTime.now();
        LocalDate docDate = parseDate(r.docDate);
        if (docDate == null) throw new Warning("Doc No Name is Required");
        h.Id = r.id;
        h.DocNo = r.id > 0 ? toInt(ci(stored, "DocNo")) : repo.nextDocNo(u, sup.fy(), DOC_TYPE);
        if (h.DocNo == 0) throw new Warning("Doc No Name is Required");
        h.TaxAccountId = r.taxAccountId;
        h.BranchesId = u.getBranchesId();
        h.DocumentTypeId = DOC_TYPE;
        h.SupplierInvoiceDate = now;
        h.SupplierInvoiceNo = h.DocNo;
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.EntryDate = now;
        h.ModifyDate = now;
        h.EntryUser = u.getId();
        h.ModifyUser = u.getId();
        h.ScreenName = SCREEN;
        h.FinancialYearId = sup.fy();
        h.DocDate = docDate.atStartOfDay();
        h.SupplierCustomerId = r.customerId;
        h.SupplierReferenceNo = text(r.refNo);
        h.ManualBillNo = text(r.manualBillNo);
        h.PaymentTermId = r.paymentTermId;
        int dueDays = toInt(r.dueDays);
        h.DueDays = dueDays;
        LocalDate dd = parseDate(r.dueDate);
        h.DueDate = (dd != null ? dd : docDate.plusDays(dueDays)).atStartOfDay();
        h.DeliveryTerm = r.deliveryTerm;
        h.BillAmount = t.bill;
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
        h.UomScheduleIdCmRate = "";
        h.AttachmentsValues = r.id > 0 && stored != null && ci(stored, "AttachmentsValues") != null ? str(ci(stored, "AttachmentsValues")) : "";
        h.CustomAttachmentsValues = r.id > 0 && stored != null && ci(stored, "CustomAttachmentsValues") != null ? str(ci(stored, "CustomAttachmentsValues")) : "";

        if (lines.isEmpty()) throw new Warning("Grid Record Not Found");

        // ---- detail rows (per row checks, in the desktop's order)
        SaleInvoiceFinancialDirect.Invoice inv = new SaleInvoiceFinancialDirect.Invoice();
        inv.h = h;
        BigDecimal exRate = BigDecimal.valueOf(r.exchangeRate);
        int rowNo = 0;
        for (Map<String, Object> m : lines) {
            rowNo++;
            String where = " In Row#" + rowNo + " In Detail Grid";
            SaleInvoiceModels.Detail pd = new SaleInvoiceModels.Detail();
            pd.Id = toInt(m.get("Id"));
            pd.SaleOrderId = toInt(m.get("OrderId"));
            pd.SaleOrderDetailId = toInt(m.get("OrderDetailId"));
            pd.SaleOrder = toInt(m.get("OrderNo"));
            pd.WarehouseId = toInt(m.get("WarehouseId"));
            if (pd.WarehouseId == 0) throw new Warning("Warehouse Required" + where);
            pd.ItemId = toInt(m.get("ItemId"));
            if (pd.ItemId == 0) throw new Warning("Item Required" + where);
            pd.JobLotId = toInt(m.get("JobLotId"));
            if (pd.JobLotId == 0) throw new Warning("JobLot Required" + where);
            pd.ItemUOMId = toInt(m.get("PackUomId"));
            if (pd.ItemUOMId == 0) throw new Warning("Pack Uom Required" + where);
            pd.ItemQty = d(m, "ItemQty");
            if (pd.ItemQty == 0.0) throw new Warning("Item Qty Required" + where);
            pd.GrossWeight = pd.ItemQty; pd.AdLsWeight = 0.0; pd.NetBillWeight = pd.ItemQty; pd.NetStockWeight = pd.ItemQty;
            pd.UomScheduleIdRate = toInt(m.get("RateUomId"));
            if (pd.UomScheduleIdRate == 0) throw new Warning("Rate Uom Required" + where);
            pd.RateCut = convInt(d(m, "AddLessRate"));                    // Conversion.ToInt(AddLessRate)
            pd.ItemRate = d(m, "NetRate");
            if (pd.ItemRate == 0.0) throw new Warning("Item Rate Required" + where);
            pd.DiscountTypeId = toInt(m.get("DiscountType"));
            pd.ItemDiscount = d(m, "DiscPct");
            pd.ItemDiscountAmount = d(m, "DiscountAmount");
            if (pd.DiscountTypeId > 0 && pd.ItemDiscount <= 0.0 && pd.ItemDiscountAmount <= 0.0) pd.DiscountTypeId = 0;
            if (pd.DiscountTypeId > 0 && pd.ItemDiscount > 0.0 && pd.ItemDiscountAmount <= 0.0) throw new Warning("Discount Amount Required" + where);
            pd.ItemAmount = d(m, "ItemAmount");
            pd.ExchangeRate = exRate;
            pd.CurrencyId = r.currencyId;
            pd.FcyAmount = BigDecimal.valueOf(d(m, "FcyAmount"));
            if (pd.ItemAmount == 0.0) throw new Warning("Item Amount Required" + where);
            pd.CityId = toInt(m.get("CityId"));
            LocalDate gp = parseDate(m.get("GpDate"));
            pd.GpDate = (gp == null ? LocalDate.of(1900, 1, 1) : gp).atStartOfDay();
            pd.DueDate = h.DueDate;
            pd.GpNo = toInt(m.get("GpNo"));
            pd.VehicleNo = str(m.get("VehicleNo"));
            pd.TaxNameId = toInt(m.get("TaxTypeId"));
            pd.TaxPercent = d(m, "TaxPct");
            pd.TaxAmount = bankers2(d(m, "TaxAmount"));
            pd.FreightAmount = d(m, "Freights");
            pd.JournalAmount = d(m, "Journal");
            pd.ExpenseAmount = d(m, "Expense");
            pd.BillAmount = d(m, "BillAmount");
            if (pd.BillAmount == 0.0) throw new Warning("Bill Amount Required" + where);
            pd.IsTaxable = pd.TaxNameId > 0 && pd.TaxPercent > 0.0 && pd.TaxAmount > 0.0 ? "true" : "false";
            pd.LineId = rowNo;
            inv.details.add(pd);
        }

        // ---- journals (rows with an account), no both-sides check at Insert
        Map<String, Integer> glOf = new HashMap<>();
        for (Map<String, Object> a : freightAcs) glOf.put("f" + a.get("SupplierCustomerId"), toInt(a.get("Id")));
        for (Map<String, Object> j : journals) {
            int acc = toInt(j.get("AccountId"));
            if (acc == 0) continue;
            int gl;
            if (c.sub) {
                if (!glOf.containsKey("f" + acc)) throw new Warning("Please Select an Account Against JL First");
                gl = glOf.get("f" + acc);
                if (acc == r.customerId) throw new Warning("Selected Account Can not be Same As Supplier Account");
            } else {
                if (!has(freightAcs, "Id", acc)) throw new Warning("Please Select an Account Against JL First");
                gl = acc;
                if (gl == customerGl) throw new Warning("Customer Account Not select");
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
                    if (o.PaymentTermId <= 0 || !has(terms, "Id", o.PaymentTermId)) throw new Warning("Payment Term Required in row#" + pr);
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

        // ---- expenses (rows with an item and an amount; the others are dropped, as on the desktop)
        for (Map<String, Object> e : expenses) {
            int item = toInt(e.get("ItemId"));
            if (item != 0 && d(e, "Amount") != 0.0 && has(others, "Id", item)) {
                SaleInvoiceModels.Expense pe = new SaleInvoiceModels.Expense();
                pe.InvRevExpItemId = item;
                pe.Qty = convInt(d(e, "Qty"));                             // Conversion.ToInt(Qty)
                pe.Rate = d(e, "Rate");
                pe.Amount = d(e, "Amount");
                pe.Remarks = str(e.get("Remarks"));
                inv.expenses.add(pe);
            }
        }
        // the commission list is empty on this screen

        // ---- BLL 0580 Save + DAL SetData (one transaction)
        if (h.Id == 0) h.ModifyUser = 0; else h.EntryUser = 0;
        SaleInvoiceFinancialDirect.Voucher voucher = new SaleInvoiceFinancialDirect(repo).makeVoucherForSaleInvoice(inv);
        int saved = persist.persist(inv, voucher, h.Id == 0 ? "Sp_InvSaleInvoice_Insert" : "Sp_InvSaleInvoice_Update");
        attachments.apply(SCREEN, DOC_TYPE, saved, r.customerId, r.attachments);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", saved);
        out.put("docNo", h.DocNo);
        out.put("voucherHeadId", repo.voucherHeadId(u.getOrganizationId(), u.getCompanyId(), DOC_TYPE, saved));
        out.put("message", (r.id == 0 ? "Record Save Successfully [" : "Record Update Successfully [") + h.DocNo + "]");
        return out;
    }

    /**
     * Row whose source is the database (a stored invoice line, or a sale order line): identity columns, rate, UOMs and (when the tax cells are
     * not editable) the tax come from the source; the cells the grid lets the user edit come from the page.
     */
    private void mergeSource(Cfg c, Map<String, Object> m, Map<String, Object> src, Map<String, Object> mine, Set<Integer> whIds, Set<Integer> cityIds, Map<Integer, Double> tax) {
        String calc = str(mine.get("calc"));
        if (calc.equals("entry")) calc = "amt";
        m.clear();
        m.putAll(src);
        m.put("calc", calc);
        int wh = toInt(mine.get("WarehouseId"));
        m.put("WarehouseId", whIds.contains(wh) ? wh : 0);
        int city = toInt(mine.get("CityId"));
        m.put("CityId", cityIds.contains(city) ? city : toInt(src.get("CityId")));
        boolean gpEditable = toInt(src.get("OrderId")) > 0;               // GpDate / GpNo / VehicleNo are editable only when OrderId > 0
        if (gpEditable) {
            m.put("GpDate", mine.get("GpDate") == null ? src.get("GpDate") : mine.get("GpDate"));
            m.put("GpNo", toInt(mine.get("GpNo")));
            m.put("VehicleNo", str(mine.get("VehicleNo")));
        }
        if (calc.isEmpty()) return;
        m.put("ItemQty", d(mine, "ItemQty"));
        m.put("AddLessRate", d(mine, "AddLessRate"));
        int dt = toInt(mine.get("DiscountType"));
        m.put("DiscountType", dt == 1 || dt == 2 ? dt : 0);
        m.put("DiscPct", d(mine, "DiscPct"));
        int tt = toInt(mine.get("TaxTypeId"));
        double tp = d(mine, "TaxPct");
        if (c.taxEditable) {
            m.put("TaxTypeId", tax.containsKey(tt) ? tt : 0);
            m.put("TaxPct", tp);
        } else if (tt != toInt(src.get("TaxTypeId")) && tax.containsKey(tt) && Math.abs(tp - tax.get(tt)) < 1e-9) {
            m.put("TaxTypeId", tt);                                       // the DocDate_Leave refresh: a type with its own scheduled percent
            m.put("TaxPct", tp);
        }
    }

    /** Row added from the entry panel: nothing but the user's choices; every id is checked against the lists the combos hold. */
    private void mergeEntry(Cfg c, Map<String, Object> m, int keepId, Set<Integer> whIds, Set<Integer> cityIds, Set<Integer> jobIds, Set<Integer> itemIds,
                            Map<Integer, Double> tax, Map<Integer, List<Map<String, Object>>> uomCache) {
        m.put("Id", keepId); m.put("OrderId", 0); m.put("OrderDetailId", 0); m.put("OrderNo", 0);
        if (str(m.get("calc")).isEmpty()) m.put("calc", "entry");
        int wh = toInt(m.get("WarehouseId"));
        m.put("WarehouseId", whIds.contains(wh) ? wh : 0);
        int item = toInt(m.get("ItemId"));
        if (!itemIds.contains(item)) item = 0;
        m.put("ItemId", item);
        int job = toInt(m.get("JobLotId"));
        m.put("JobLotId", jobIds.contains(job) ? job : 0);
        int city = toInt(m.get("CityId"));
        m.put("CityId", cityIds.contains(city) ? city : 0);
        int tt = toInt(m.get("TaxTypeId"));
        m.put("TaxTypeId", tax.containsKey(tt) ? tt : 0);
        int dt = toInt(m.get("DiscountType"));
        m.put("DiscountType", dt == 1 || dt == 2 ? dt : 0);
        List<Map<String, Object>> uoms = item == 0 ? List.of() : uomCache.computeIfAbsent(item, this::uoms);
        int pu = toInt(m.get("PackUomId")), ru = toInt(m.get("RateUomId"));
        Map<String, Object> pm = null, rm = null;
        for (Map<String, Object> x : uoms) { if (toInt(x.get("Id")) == pu) pm = x; if (toInt(x.get("Id")) == ru) rm = x; }
        m.put("PackUomId", pm == null ? 0 : pu); m.put("PackEquivalent", pm == null ? 0.0 : toDouble(pm.get("Equivalent"))); if (pm != null) m.put("PackUom", pm.get("UOMCode"));
        m.put("RateUomId", rm == null ? 0 : ru); m.put("RateEquivalent", rm == null ? 0.0 : toDouble(rm.get("Equivalent"))); if (rm != null) m.put("RateUom", rm.get("UOMCode"));
        m.put("GpNo", toInt(m.get("GpNo")));
        m.put("VehicleNo", str(m.get("VehicleNo")));
        if (parseDate(m.get("GpDate")) == null) m.put("GpDate", LocalDate.now().toString());
    }

    // ------------------------------------------------------------------ Delete

    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = sup.user();
        if (!Boolean.TRUE.equals(sup.rights(SCREEN).get("delete"))) throw new Warning("You do not have the Delete right for this screen.");
        if (id <= 0) throw new Warning("RecordId Not Found.....");
        Map<String, Object> inv = invoice(id);
        if (toBool(ci(inv, "IsApproved"))) throw new Warning("Record cannot be  Delete beacause Record has approved");
        repo.delete(u, id, DOC_TYPE);                                      // InvPurchaseInvoice.RemoveByID -> Sp_InvoicesVouchersandStocksDelete
        return row("message", "Delete Record Successfully");
    }

    // ------------------------------------------------------------------ attachments

    public List<Map<String, Object>> attachmentList(int id) { invoice(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { invoice(id); return attachments.download(SCREEN, id, attachmentId); }
}
