package com.mst.services.sale.salt;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels.Commission;
import com.mst.models.saleinvoice.SaleInvoiceModels.Detail;
import com.mst.models.saleinvoice.SaleInvoiceModels.Expense;
import com.mst.models.saleinvoice.SaleInvoiceModels.Head;
import com.mst.models.saleinvoice.SaleInvoiceModels.Journal;
import com.mst.repositories.SaleInvoiceDirect139Repository;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.repositories.support.DesktopProc;
import com.mst.services.SaleInvoiceFinancialDirect;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleEngrSupport;
import com.mst.services.sale.salt.SaleSaltDirectCalc.Ex;
import com.mst.services.sale.salt.SaleSaltDirectCalc.Inv;
import com.mst.services.sale.salt.SaleSaltDirectCalc.Jv;
import com.mst.services.sale.salt.SaleSaltDirectCalc.Line;
import com.mst.services.sale.steel.SaleStInvoiceCalc.Cfg;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;
import static com.mst.services.sale.steel.SaleStInvoiceCalc.dec;
import static com.mst.services.sale.steel.SaleStInvoiceCalc.decText;
import static com.mst.services.sale.steel.SaleStInvoiceCalc.fmt;
import static com.mst.services.sale.steel.SaleStInvoiceCalc.hash;
import static com.mst.services.sale.steel.SaleStInvoiceCalc.num;

/**
 * Screen 585 "frmSaleDirectInvoice" = Architecture.WinApp.SaleForSalt.frmSaleDirectInvoice (Sale Salt, module 94, document type 1809).
 *
 * Desktop map (frmSaleDirectInvoice.cs): InvfrmPurchasedirectInvoice_Load :478, bindWareHouse :634, BinCity :663, suppliercustomer :692, item :736,
 * PaymentTerms :817, DeliveryTerm :847, CommissionTypeFill :865, CommissionUOMFill :884, PackingType :904, JobLotBind :933, AccountsFill :962, OtherItemsBind :974,
 * PackUOM :991, DocumentNo :1039, TransporterAcFill :1079, CurrencyFill :1107, MultiCurrencyFeature :1145, ConfigurationDefault :1175, AvailableStockGetByItem :1323,
 * FormValidation :2274, FormValidationDetila :2366, Reset :2483, Insert :2642, ReadById_Update :2964, FormHistory/GetAll :3170, HistoryCombosFill :3347,
 * GetDetailGrdByHeadId, prints :3588, LoadInGridDetail :4544; LoadSaleOrder.cs (loader dialog).
 *
 * Procedures: Sp_InvSaleInvoice_GetAllMethod (GetByID, SaleDetailReadByInvSaleInvoiceId, child reads, FormHistory), Usp_AllComboAgainstSaleInvoice,
 * USP_LoadSaleOrderOnInvoiceForSalt, Sp_SupplierCustomer_GetAllMethod, Sp_ItemAllocateToWareHouse_GetAllMethod, Sp_COAAllocation_GetAllMethod (COAAllocationSearch),
 * Sp_SaleOrder_GetAllMethod (GetCurrentStockByItemId), SP_City_GetAllMethod, and the save chain InvSaleInvoice.Save -> SaleInvoiceFinancialDirect -> SaleSaltPersist.
 */
@Service
public class SaleSaltDirectService {
    public static final String SCREEN = "frmSaleDirectInvoice";
    public static final int DOC_TYPE = 1809;
    private static final String P_INV = "Sp_InvSaleInvoice_GetAllMethod";

    /** MessageBox.Show(..); return; of the desktop: nothing is saved, the text is shown. */
    static class Stop extends RuntimeException {
        final String focus;
        Stop(String m, String focus) { super(m); this.focus = focus; }
    }

    private final SaleEngrSupport sup;
    private final SaleInvoiceRepository repo;
    private final SaleInvoiceDirect139Repository own;
    private final SaleEngrAttachments attachments;
    private final SaleSaltPersist persist;

    public SaleSaltDirectService(SaleEngrSupport sup, SaleInvoiceRepository repo, SaleInvoiceDirect139Repository own, SaleEngrAttachments attachments, SaleSaltPersist persist) {
        this.sup = sup; this.repo = repo; this.own = own; this.attachments = attachments; this.persist = persist;
    }

    // ------------------------------------------------------------------ configuration / lookups

    /** CommonServices.GetDecimalConfiguration + CreditAmountInItemSaleGL. */
    public Cfg cfg() {
        Cfg c = new Cfg();
        int a = toInt(sup.config("Default NoofDecimal Points For Amount"));
        c.amtRound = a; c.amtDec = a >= 1 && a <= 4 ? a : 0;
        int f = toInt(sup.config("DefaultNoOfDecimalPointsForFcyAmount"));
        c.fcyRound = f; c.fcyDec = f >= 1 && f <= 4 ? f : 0;
        int r = toInt(sup.config("Default NoofDecimal Points For Rate"));
        c.rateDec = r >= 1 && r <= 4 ? r : (r == 0 ? 2 : 0);
        String cr = sup.config("CreditAmountInItemSaleGL");
        c.creditRaw = cr.isEmpty() ? null : cr;
        return c;
    }

    public Map<String, Object> formats() {
        Cfg c = cfg();
        return row("amtDec", c.amtDec, "fcyDec", c.fcyDec, "rateDec", c.rateDec);
    }

    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String... cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String c : cols) o.put(c, ci(r, c));
            out.add(o);
        }
        return out;
    }

    /** bindWareHouse :634 - CommonServices.getActiveWareHouse. */
    public List<Map<String, Object>> warehouses() { return pick(own.warehouses(sup.user()), "Id", "WareHouseName"); }

    /** BinCity :663 - CityGetAllService -> City.GetAll. */
    public List<Map<String, Object>> cities() {
        return pick(sup.rows("SP_City_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll"), "Id", "CityName");
    }

    /** suppliercustomer :692 - SupplierCustomerGetAllServiceBind (Id, CompanyName; GlAccountId for txtSupplierGLId). */
    public List<Map<String, Object>> customers() { return pick(own.customers(sup.user()), "Id", "CompanyName", "GlAccountId"); }

    /** item :736 - ReadAllItems, or GetItemsFromWarehouseAllocationByWareHouseId when the configuration GetItemsagainstWarehouse is true (Id, ItemName, ItemCodeNew as ItemCode). */
    public List<Map<String, Object>> items(int warehouseId) {
        List<Map<String, Object>> rows = itemsAgainstWarehouse()
                ? sup.rows("Sp_ItemAllocateToWareHouse_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "WarehouseId", warehouseId, "Activity", "GetItemsFromWarehouseAllocationByWareHouseId")
                : own.items(sup.user());
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(row("Id", ci(r, "Id"), "ItemName", str(ci(r, "ItemName")), "ItemCode", str(ci(r, "ItemCodeNew"))));
        return out;
    }

    private boolean itemsAgainstWarehouse() {
        String s = sup.config("GetItemsagainstWarehouse");
        return !s.isEmpty() && toBool(s);
    }

    /** PaymentTerms :817 - GetDueTermServiceBind. */
    public List<Map<String, Object>> paymentTerms() { return pick(own.dueTerms(sup.user()), "Id", "TermsDescription"); }

    /** DeliveryTerm :847 */
    public List<Map<String, Object>> deliveryTerms() { return List.of(row("Id", 1, "DeliveryTerm", "Load"), row("Id", 2, "DeliveryTerm", "Ponch")); }

    /** CommissionTypeFill :865 */
    public List<Map<String, Object>> commissionTypes() {
        return List.of(row("Id", 1, "CommissionType", "Flat"), row("Id", 2, "CommissionType", "Percent"), row("Id", 3, "CommissionType", "Comm Weight"));
    }

    /** CommissionUOMFill :884 */
    public List<Map<String, Object>> commissionUoms() {
        return List.of(row("Id", 1, "Uom", "40"), row("Id", 2, "Uom", "50"), row("Id", 3, "Uom", "60"), row("Id", 4, "Uom", "100"));
    }

    /** PackingType :904 - InvPackingType.Getall. */
    public List<Map<String, Object>> packingTypes() { return pick(own.packingTypes(), "Id", "PackTypeDesc"); }

    /** JobLotBind :933 - JobLotGetAllService. */
    public List<Map<String, Object>> jobLots() { return pick(own.jobLots(sup.user()), "Id", "JobLotDescription"); }

    /** AccountsFill :962 - CoaAllocationGetAllServiceBind = COAAllocation.GetAll (OrganizationId, CompanyId, UserId when not 0; Activity COAAllocationSearch). */
    public List<Map<String, Object>> accounts() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company());
        if (sup.userId() != 0) p.put("UserId", sup.userId());
        p.put("Activity", "COAAllocationSearch");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(sup.jdbc(), "Sp_COAAllocation_GetAllMethod", p)) out.add(row("Id", r.get("Id"), "AccountTitle", r.get("AccountTitle")));
        return out;
    }

    /** OtherItemsBind :974 - InventoryItemsOther.GetAll. */
    public List<Map<String, Object>> otherItems() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.otherItems(sup.org(), sup.company())) out.add(row("Id", toInt(ci(r, "Id")), "OtherItemName", str(ci(r, "OtherItemName"))));
        return out;
    }

    /** CurrencyFill :1107 - MultiCurrency.GetAll. */
    public List<Map<String, Object>> currencies() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_MultiCurrency_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadAll"))
            out.add(row("Id", toInt(ci(r, "Id")), "CurrencyCode", str(ci(r, "CurrencyCode"))));
        return out;
    }

    /** PackUOM :991 - CommonServices.GetUomScheduleByItemId (Id, UOMCode, Equivalent). */
    public List<Map<String, Object>> uoms(int itemId) { return pick(own.uomsOfItem(sup.user(), itemId), "Id", "UOMCode", "Equivalent"); }

    /** AvailableStockGetByItem :1323 */
    public Map<String, Object> stock(int warehouseId, int itemId, int jobLotId, int packingTypeId, int uomId, String docDate) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("ItemId", itemId);
        Timestamp d = ts(docDate);
        if (d != null) p.put("DocDateTo", d);
        p.put("WareHouseId", warehouseId); p.put("JobLotId", jobLotId); p.put("InvPackingTypeId", packingTypeId); p.put("ItemUomId", uomId);
        p.put("Activity", "GetCurrentStockByItemId");
        List<Map<String, Object>> r = DesktopProc.rows(sup.jdbc(), "Sp_SaleOrder_GetAllMethod", p);
        double cur = r.isEmpty() ? 0.0 : toDouble(ci(r.get(0), "AvailableStock"));
        return row("balance", cur > 0.0 ? num(cur) : "0");
    }

    /** MultiCurrencyFeature :1145 - GetERPFeatureById(6). */
    private boolean multiCurrency() { return sup.erpFeature(6); }

    private int outwardAcId() {
        String s = sup.config("FreightOutwardAc");
        return s.isEmpty() ? 0 : Math.max(toInt(s), 0);
    }

    private int baseCurrency() { return toInt(sup.config("Base Currency")); }
    private double baseRate() { return toDouble(sup.config("BaseCurrencyRate")); }

    /** DocumentNo :1039 - InvSaleInvoice.GenerateInvSaleInvoiceCode (document type 1809). */
    public int nextNo() { return repo.nextDocNo(sup.user(), sup.fy(), DOC_TYPE); }

    /** HistoryCombosFill :3347 - AllComboBindAgainstSaleInvoice, the 'Supplier' rows as {Id, Customer}. */
    public Map<String, Object> historyCombos() {
        UserAccount u = sup.user();
        List<Map<String, Object>> cust = new ArrayList<>();
        for (Map<String, Object> r : repo.q("EXEC dbo.Usp_AllComboAgainstSaleInvoice @OrganizationId=?, @CompanyId=?, @AppId=?, @UserId=?, @DocumentTypeIds=?",
                u.getOrganizationId(), u.getCompanyId(), u.getAppId(), u.getId(), String.valueOf(DOC_TYPE))) {
            if ("Supplier".equals(str(ci(r, "Activity")))) cust.add(row("Id", ci(r, "Id"), "Customer", ci(r, "ReferenceName")));
        }
        return row("customers", cust);
    }

    /** ConfigurationDefault :1175 as the configuration values (the combos / text boxes are set from them by Reset and the browser). */
    public Map<String, Object> defaults() {
        Map<String, Object> m = new LinkedHashMap<>();
        String s = sup.config("Paking Type"); if (!s.isEmpty()) m.put("packingTypeId", toInt(s));
        s = sup.config("Warehouse"); if (!s.isEmpty()) m.put("warehouseId", toInt(s));
        s = sup.config("City Area"); if (!s.isEmpty()) m.put("cityId", toInt(s));
        s = sup.config("Job/Lot"); if (!s.isEmpty()) m.put("jobLotId", toInt(s));
        s = sup.config("Base Currency"); if (!s.isEmpty()) m.put("currencyId", toInt(s));
        s = sup.config("BaseCurrencyRate"); if (!s.isEmpty()) m.put("baseRate", toDouble(s));
        return m;
    }

    /** InvfrmPurchasedirectInvoice_Load :478 */
    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("multiCurrency", multiCurrency());
        m.put("outwardAcId", outwardAcId());
        m.put("itemsAgainstWarehouse", itemsAgainstWarehouse());
        m.put("wages", toBool(sup.config("WagesCompulsoryOnSaleInvoiceDirect")));
        m.put("currencies", currencies());
        m.put("accounts", accounts());
        m.put("otherItems", otherItems());
        m.put("warehouses", warehouses());
        m.put("cities", cities());
        m.put("customers", customers());
        m.put("paymentTerms", paymentTerms());
        m.put("deliveryTerms", deliveryTerms());
        m.put("commTypes", commissionTypes());
        m.put("commUoms", commissionUoms());
        m.put("items", items(0));
        m.put("packingTypes", packingTypes());
        m.put("jobLots", jobLots());
        m.put("defaults", defaults());
        m.put("formats", formats());
        m.put("history", historyCombos());
        int days = toInt(sup.config("DefaultDaysToLessFromHistoryFromDate"));
        m.put("historyDays", days > 0 ? days : 3);
        return m;
    }

    /** btnRefresh_Click :2599 - OtherItemsBind, AccountsFill, GridComboBind, bindWareHouse, BinCity, suppliercustomer, PaymentTerms, DeliveryTerm, CommissionTypeFill, CommissionUOMFill, item, PackingType, JobLotBind, CurrencyFill, ConfigurationDefault. */
    public Map<String, Object> refresh() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("otherItems", otherItems());
        m.put("accounts", accounts());
        m.put("warehouses", warehouses());
        m.put("cities", cities());
        m.put("customers", customers());
        m.put("paymentTerms", paymentTerms());
        m.put("deliveryTerms", deliveryTerms());
        m.put("commTypes", commissionTypes());
        m.put("commUoms", commissionUoms());
        m.put("items", items(0));
        m.put("packingTypes", packingTypes());
        m.put("jobLots", jobLots());
        m.put("currencies", currencies());
        m.put("defaults", defaults());
        return m;
    }

    private int glOf(int supplierId) {
        if (supplierId <= 0) return 0;
        List<Map<String, Object>> r = sup.jdbc().queryForList("SELECT GlAccountId FROM SupplierCustomer WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?", supplierId, sup.org(), sup.company());
        return r.isEmpty() ? 0 : toInt(r.get(0).get("GlAccountId"));
    }

    private SaleSaltDirectCalc.Lookups lookups() {
        Map<Integer, List<Map<String, Object>>> cache = new HashMap<>();
        return new SaleSaltDirectCalc.Lookups() {
            private List<Map<String, Object>> of(int item) { return cache.computeIfAbsent(item, k -> own.uomsOfItem(sup.user(), k)); }
            public Double equivalent(int itemId, int uomId) {
                for (Map<String, Object> r : of(itemId)) if (toInt(ci(r, "Id")) == uomId) return toDouble(ci(r, "Equivalent"));
                return null;
            }
            public String code(int itemId, int uomId) {
                for (Map<String, Object> r : of(itemId)) if (toInt(ci(r, "Id")) == uomId) return str(ci(r, "UOMCode"));
                return null;
            }
            public Integer idByCode(int itemId, String code) {
                for (Map<String, Object> r : of(itemId)) if (str(ci(r, "UOMCode")).equals(code)) return toInt(ci(r, "Id"));
                return null;
            }
            public Double lastExchangeRate(int currencyId) { return repo.lastExchangeRate(sup.user(), currencyId, 1608); }
        };
    }

    private SaleSaltDirectCalc calcFor(Inv v) {
        return new SaleSaltDirectCalc(cfg(), v, lookups(), outwardAcId(), baseCurrency(), baseRate());
    }

    // ------------------------------------------------------------------ calculations (the grid / text box events)

    public static class CalcRequest {
        public String event, key, col;
        public int row = -1;
        public boolean yes;
        public Inv state;
    }

    /** One desktop event on the form state; answers the new state and the MessageBox texts raised on the way. */
    public Inv calc(CalcRequest rq) {
        Inv v = rq.state == null ? new Inv() : rq.state;
        SaleSaltDirectCalc k = calcFor(v);
        String e = rq.event == null ? "" : rq.event;
        switch (e) {
            case "qty": case "gross": case "addless": case "rate": case "ratecut": case "rateuom": case "packuom": k.entryEvent(e); break;
            case "stock": break;
            case "agent": k.agentLeave(); break;
            case "comm": k.commissionChanged(); break;
            case "supplier": k.supplierChanged(); v.supplierGlId = glOf(v.supplierId); break;
            case "freight": k.freightTextChanged(); break;
            case "exch": k.exchangeChanged(); break;
            case "exchLeave": k.exchangeLeave(); break;
            case "curLeave": k.currencyLeave(); break;
            case "due": case "docDate": k.dueDateGenerate(); break;
            case "delTerm": k.deliveryTermChanged(); break;
            case "pterm": k.paymentTermLeave(); break;
            case "grid": k.gridButton(rq.key, rq.row); break;
            case "gridKey": k.gridKey(rq.key, rq.row, rq.yes); break;
            case "gridCell": k.gridCell(rq.row, rq.col); break;
            case "expBtn": k.expButton(rq.key, rq.row); break;
            case "expCell": k.expCell(rq.row, rq.col); break;
            case "expKey": k.expKey(rq.key, rq.row); break;
            case "glBtn": k.glButton(rq.key, rq.row); break;
            case "glCell": k.glCell(rq.row, rq.col); break;
            case "glKey": k.glKey(rq.key, rq.row, rq.yes); break;
            case "add": k.addDetail(); break;
            case "update": k.updateDetail(); break;
            case "edit": k.editRow(rq.row); break;
            case "cancelUpdate": k.cancelUpdate(); break;
            case "reset": reset(k, v); break;
            default: throw new IllegalArgumentException("Unknown event " + e);
        }
        return v;
    }

    /** Reset :2483 followed by DocumentNo, ConfigurationDefault, txtduedays and EnableDiableCommissionFields. */
    private void reset(SaleSaltDirectCalc k, Inv v) {
        k.resetForm();
        v.orderExist = v.orderExist;                                       // OrderExist is NOT reset by Reset (desktop)
        int n = nextNo();
        if (n > 0) v.docNo = String.valueOf(n);
        Map<String, Object> d = defaults();
        if (d.get("packingTypeId") != null) { v.e.packingTypeId = toInt(d.get("packingTypeId")); v.e.packingText = nameOf(packingTypes(), v.e.packingTypeId, "PackTypeDesc"); }
        if (d.get("warehouseId") != null) { v.e.warehouseId = toInt(d.get("warehouseId")); v.e.warehouseText = nameOf(warehouses(), v.e.warehouseId, "WareHouseName"); }
        if (d.get("cityId") != null && v.e.cityId == 0) { v.e.cityId = toInt(d.get("cityId")); v.e.cityText = nameOf(cities(), v.e.cityId, "CityName"); }
        if (d.get("jobLotId") != null && v.e.jobLotId == 0) { v.e.jobLotId = toInt(d.get("jobLotId")); v.e.jobLotText = nameOf(jobLots(), v.e.jobLotId, "JobLotDescription"); }
        k.configurationExchange(d.get("currencyId") != null, d.get("baseRate") != null);
        k.afterConfigurationDefault();
    }

    private static String nameOf(List<Map<String, Object>> rows, int id, String col) {
        for (Map<String, Object> r : rows) if (toInt(r.get("Id")) == id) return str(r.get(col));
        return "";
    }

    // ------------------------------------------------------------------ Load Sale Order (LoadSaleOrder.cs)

    /** LoadSaleOrder: ComboBinds - Sp_SupplierCustomer_GetAllMethod GetSupplierCustomerFromOrderByInvoice (OrderSupCustId, CompanyName); FromDate = ActiveYr.Start_Period. */
    public Map<String, Object> loaderCombos() {
        List<Map<String, Object>> cust = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetSupplierCustomerFromOrderByInvoice"))
            cust.add(row("Id", ci(r, "OrderSupCustId"), "Name", ci(r, "CompanyName")));
        return row("customers", cust, "fromDate", fyStart());
    }

    public String fyStart() {
        for (Map<String, Object> r : sup.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            if (toInt(r.get("Id")) == sup.fy()) return date10(r.get("Start_Period"));
        return "";
    }

    private List<Map<String, Object>> pendingRows(String from, String to, int fromNo, int toNo, int supplierId) {
        UserAccount u = sup.user();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId()); p.put("BranchesId", u.getBranchesId()); p.put("FinancialYearId", sup.fy());
        p.put("DocumentTypeId", 1806);
        p.put("DocDateFrom", ts(from)); p.put("DocDateTo", ts(to));
        if (fromNo != 0) p.put("FromDocNo", fromNo);
        if (toNo != 0) p.put("ToDocNo", toNo);
        if (supplierId != 0) p.put("OrderSupCustId", supplierId);
        return DesktopProc.rows(sup.jdbc(), "dbo.USP_LoadSaleOrderOnInvoiceForSalt", p);
    }

    /** Search of the dialog: the rows of the procedure with the dialog's column names. */
    public List<Map<String, Object>> loaderPending(String from, String to, int fromNo, int toNo, int supplierId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : pendingRows(from, to, fromNo, toNo, supplierId)) out.add(loaderRow(r));
        return out;
    }

    private static Map<String, Object> loaderRow(Map<String, Object> r) {
        return row("DocumentTypeId", ci(r, "DocumentTypeId"), "DetailId", ci(r, "DetailId"), "Id", ci(r, "Id"), "DocDate", date10(ci(r, "DocDate")), "DocNo", ci(r, "DocNo"),
                "SupplierCustomerId", ci(r, "SupplierCustomerId"), "SupplierName", ci(r, "SupplierName"), "CommissionAgentId", ci(r, "CommissionAgentId"), "CommissionAgent", ci(r, "CommissionAgent"),
                "CommissionType", ci(r, "CommissionType"), "CommRate", ci(r, "CommRate"), "CommUom", ci(r, "CommUom"), "CommAmount", ci(r, "CommAmount"), "CommissionRemarks", ci(r, "CommissionRemarks"),
                "DeliveryTerm", ci(r, "DeliveryTerm"), "PaymentTermsId", ci(r, "PaymentTermsId"), "PaymentTerm", ci(r, "PaymentTerm"), "DueDate", date10(ci(r, "OrderDueDate")), "DueDays", ci(r, "OrderDueDays"),
                "JobLotId", ci(r, "JobLotId"), "JobLot", ci(r, "JobLotDescription"), "ItemId", ci(r, "OrderItemId"), "ItemName", ci(r, "ItemName"), "ItemCode", ci(r, "ItemCode"),
                "ItemUOMId", ci(r, "OrderItemUOMId"), "PackUomCode", ci(r, "PackUomCode"), "PackUomEquivalent", ci(r, "PackUomEquivalent"), "ItemRate", ci(r, "OrderItemRate"),
                "RateUOMId", ci(r, "OrderItemRateUOMId"), "RateUomCode", ci(r, "RateUomCode"), "RateUomEquivalent", ci(r, "RateUomEquivalent"), "ItemQty", ci(r, "OrderItemQty"),
                "NetWeight", ci(r, "NetWeight"), "TotalAmount", ci(r, "TotalAmount"), "RecQty", ci(r, "RecQty"), "RecWeight", ci(r, "RecWeight"), "RecAmount", ci(r, "RecAmount"),
                "BalQty", ci(r, "BalQty"), "BalNetWeight", ci(r, "BalNetWeight"), "BalAmount", ci(r, "BalAmount"), "EntryDate", date10(ci(r, "EntryDate")), "EntryUserName", ci(r, "EntryUserName"),
                "ModifyDate", date10(ci(r, "ModifyDate")), "ModifyUserName", ci(r, "ModifyUserName"), "NoOfAttachments", ci(r, "NoOfAttachments"));
    }

    public static class LoadRequest {
        public List<Integer> detailIds = new ArrayList<>();
        public String from, to;
        public int fromNo, toNo, supplierId;
        /** the rows of grdDetail (the history detail grid) - the desktop's duplicate check reads that grid, not grd. */
        public List<Map<String, Object>> histDetail = new ArrayList<>();
        public Inv state;
    }

    /**
     * Desktop defect reproduced on purpose: LoadSaleOrder.btnLoad adds the dtLoader row with RateUOMId before ItemRate, the columns are ItemRate (decimal) then
     * RateUOMId (int), so a loaded line gets Rate = the rate UOM id and RateUOMId = the rate rounded to an integer (banker's rounding).
     */
    public static final boolean DESKTOP_LOADER_SWAPS_RATE_AND_RATE_UOM = true;

    /** btnLoadGdn_Click :4520 + LoadSaleOrder.btnLoad + LoadInGridDetail :4544 */
    public Inv load(LoadRequest rq) {
        Inv v = rq.state == null ? new Inv() : rq.state;
        SaleSaltDirectCalc k = calcFor(v);
        Cfg c = cfg();
        if (!v.lines.isEmpty() && v.lines.get(0).OrderId == 0) throw new IllegalArgumentException("You cannot Load Order because you already add record without Order");
        if (rq.detailIds == null || rq.detailIds.isEmpty()) return v;
        Map<Integer, Map<String, Object>> byDetail = new LinkedHashMap<>();
        for (Map<String, Object> r : pendingRows(rq.from, rq.to, rq.fromNo, rq.toNo, rq.supplierId)) byDetail.put(toInt(ci(r, "DetailId")), r);
        List<Map<String, Object>> rows = new ArrayList<>();
        int orderId = 0;
        for (Integer did : rq.detailIds) {
            Map<String, Object> r = byDetail.get(did);
            if (r == null) throw new IllegalArgumentException("Sale order row not found or no longer pending");
            int id = toInt(ci(r, "Id"));
            if (orderId == 0) orderId = id;
            if (orderId != id) throw new Warning("You Can Only Select Rows Of Same Order");
            rows.add(r);
        }
        Map<String, Object> f = rows.get(0);
        v.orderExist = true;
        v.commType = ""; v.commUom = "";
        v.supplierId = toInt(ci(f, "SupplierCustomerId")); v.supplierGlId = glOf(v.supplierId);
        v.commAgentId = toInt(ci(f, "CommissionAgentId"));
        v.commType = str(ci(f, "CommissionType"));
        v.commRate = dts(ci(f, "CommRate"));
        v.commUom = dts(ci(f, "CommUom"));
        v.commAmount = dts(ci(f, "CommAmount"));
        v.commRemarks = str(ci(f, "CommissionRemarks"));
        v.deliveryTerm = str(ci(f, "DeliveryTerm"));
        v.paymentTermId = toInt(ci(f, "PaymentTermsId"));
        v.dueDate = date10(ci(f, "OrderDueDate"));
        v.dueDays = dts(ci(f, "OrderDueDays"));
        v.supplierEnabled = false; v.deliveryTermEnabled = false; v.paymentTermEnabled = false; v.itemEnabled = false;
        k.dueDateGenerate();
        for (Map<String, Object> r : rows) {
            boolean dup = false;
            for (Map<String, Object> g : rq.histDetail) {                                     // iterates grdDetail (desktop defect)
                if (toInt(g.get("OrderId")) != toInt(ci(r, "Id"))) throw new Warning("Data Already Exists Against Different Order");
                if (toInt(g.get("OrderDetailId")) == toInt(ci(r, "DetailId"))) { dup = true; break; }
            }
            if (dup) continue;
            Line l = new Line();
            l.OrderId = toInt(ci(r, "Id")); l.OrderDetailId = toInt(ci(r, "DetailId")); l.OrderNo = toInt(ci(r, "DocNo"));
            l.ItemId = toInt(ci(r, "OrderItemId")); l.Item = str(ci(r, "ItemName")); l.ItemCode = str(ci(r, "ItemCode"));
            l.JobLotId = toInt(ci(r, "JobLotId")); l.JobLot = str(ci(r, "JobLotDescription"));
            l.PackingTypeId = 0; l.PackingType = "";
            l.PackUOMId = toInt(ci(r, "OrderItemUOMId")); l.PackUOM = toDouble(ci(r, "PackUomEquivalent"));
            l.ItemQty = toDouble(ci(r, "BalQty")); l.BalQty = l.ItemQty; l.GrossWeight = toDouble(ci(r, "BalNetWeight")); l.BalWeight = l.GrossWeight;
            l.NetBillWeight = l.GrossWeight; l.StockWeight = l.GrossWeight;
            double itemRate = toDouble(ci(r, "OrderItemRate"));
            int rateUomId = toInt(ci(r, "OrderItemRateUOMId"));
            if (DESKTOP_LOADER_SWAPS_RATE_AND_RATE_UOM) { l.Rate = rateUomId; l.RateUOMId = (int) Math.rint(itemRate); }
            else { l.Rate = itemRate; l.RateUOMId = rateUomId; }
            l.RateUOM = toDouble(ci(r, "RateUomEquivalent"));
            l.ItemAmount = toDouble(ci(r, "BalAmount"));
            v.lines.add(l);
        }
        k.afterLoad();
        return v;
    }

    // ------------------------------------------------------------------ record (ReadById_Update :2964)

    private Map<String, Object> header(int id) {
        List<Map<String, Object>> r = sup.rows(P_INV, "Id", id, "Activity", "ReadById");
        UserAccount u = sup.user();
        if (r.isEmpty() || toInt(ci(r.get(0), "OrganizationId")) != u.getOrganizationId() || toInt(ci(r.get(0), "CompanyId")) != u.getCompanyId()
                || toInt(ci(r.get(0), "DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Sale invoice not found in this company");
        return r.get(0);
    }

    /** Conversion.ToString(decimal): the scale of the column is kept. */
    private static String dts(Object o) {
        if (o == null) return "";
        if (o instanceof BigDecimal) return ((BigDecimal) o).toPlainString();
        return String.valueOf(o);
    }

    static String date10(Object o) {
        if (o == null) return "";
        String s = str(o);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    public Map<String, Object> record(int id) {
        Map<String, Object> h = header(id);
        Cfg c = cfg();
        Inv v = new Inv();
        v.id = id;
        v.saveMode = false;
        v.docNo = dts(ci(h, "DocNo")); v.docDate = date10(ci(h, "DocDate"));
        v.supplierId = toInt(ci(h, "SupplierCustomerId")); v.supplierGlId = glOf(v.supplierId);
        v.transporterId = toInt(ci(h, "TransporterId"));
        v.freightText = num(toDouble(ci(h, "FreightAmount")));
        v.refNo = dts(ci(h, "SupplierReferenceNo")); v.billNo = str(ci(h, "ManualBillNo"));
        v.commAgentId = toInt(ci(h, "CommissionAgentId"));
        v.commType = str(ci(h, "CommissionType"));
        v.commRate = fmt(toDouble(ci(h, "CommRate")), c.rateDec);
        v.commUom = str(ci(h, "UomScheduleIdCmRate"));
        v.commAmount = fmt(toDouble(ci(h, "CommAmount")), c.amtDec);
        v.commRemarks = str(ci(h, "CommissionRemarks"));
        v.remarks = str(ci(h, "OtherRemarks"));
        v.billAmount = fmt(toDouble(ci(h, "BillAmount")), c.amtDec);
        v.paymentTermId = toInt(ci(h, "PaymentTermId"));
        v.deliveryTerm = str(ci(h, "DeliveryTerm"));
        v.dueDays = dts(ci(h, "SupplierInvoiceNo")); v.dueDate = date10(ci(h, "SupplierInvoiceDate"));
        v.approved = toBool(ci(h, "IsApproved"));
        v.currencyId = toInt(ci(h, "CurrencyId"));
        v.invoiceQty = hash(decText(dts(ci(h, "InvoiceQty"))), 2); v.invoiceWeight = hash(decText(dts(ci(h, "InvoiceWeight"))), 2);
        v.fcyAmount = fmt(decText(dts(ci(h, "FcyAmount"))), c.fcyDec);
        if (v.currencyId > 0) v.exchangeRate = dts(ci(h, "ExchangeRate"));
        for (Map<String, Object> d : sup.rows(P_INV, "Id", id, "Activity", "SaleDetailReadByInvSaleInvoiceId")) {
            Line l = new Line();
            l.Id = toInt(ci(d, "Id")); l.OrderId = toInt(ci(d, "SaleOrderId")); l.OrderDetailId = toInt(ci(d, "SaleOrderDetailId")); l.OrderNo = toInt(ci(d, "SaleOrder"));
            l.WarehouseId = toInt(ci(d, "WarehouseId")); l.ItemId = toInt(ci(d, "ItemId")); l.Item = str(ci(d, "ItemName")); l.ItemCode = str(ci(d, "ItemCode"));
            l.JobLotId = toInt(ci(d, "JobLotId")); l.JobLot = str(ci(d, "JobLotDescription")); l.PackingTypeId = toInt(ci(d, "PackingTypeId")); l.PackingType = str(ci(d, "PackTypeDesc"));
            l.PackUOMId = toInt(ci(d, "ItemUOMId")); l.PackUOM = toDouble(ci(d, "PackEquivalent"));
            l.ItemQty = toDouble(ci(d, "ItemQty")); l.BalQty = l.ItemQty; l.GrossWeight = toDouble(ci(d, "GrossWeight")); l.BalWeight = l.GrossWeight;
            l.AddLss = toDouble(ci(d, "AdLsWeight")); l.NetBillWeight = toDouble(ci(d, "NetBillWeight")); l.StockWeight = toDouble(ci(d, "NetStockWeight"));
            l.Rate = toDouble(ci(d, "ItemRate")); l.RateUOMId = toInt(ci(d, "UomScheduleIdRate")); l.RateUOM = toDouble(ci(d, "RateUOM"));
            l.RateCutAmount = toDouble(ci(d, "RateCutAmount")); l.ItemAmount = toDouble(ci(d, "ItemAmount"));
            l.GpDate = date10(ci(d, "GpDate")); l.GpNo = toInt(ci(d, "GpNo")); l.VehicleNo = str(ci(d, "VehicleNo"));
            l.FcyAmount = toDouble(ci(d, "FcyAmount")); l.BillAmount = toDouble(ci(d, "BillAmount")); l.Expense = toDouble(ci(d, "ExpenseAmount"));
            l.Freights = toDouble(ci(d, "FreightAmount")); l.Commission = toDouble(ci(d, "CommissionAmount"));
            v.lines.add(l);
            if (l.OrderId > 0) {
                v.orderExist = true; v.supplierEnabled = false; v.deliveryTermEnabled = false; v.paymentTermEnabled = false;
            }
        }
        for (Map<String, Object> e : sup.rows(P_INV, "Id", id, "Activity", "InvSaleInvoiceExpense_ReadBySaleInvoiceID")) {
            Ex x = new Ex(); x.ItemId = toInt(ci(e, "InvRevExpItemId")); x.Qty = toDouble(ci(e, "Qty")); x.Rate = toDouble(ci(e, "Rate")); x.Amount = toDouble(ci(e, "Amount")); x.Remarks = str(ci(e, "Remarks"));
            v.exp.add(x);
        }
        for (Map<String, Object> e : sup.rows(P_INV, "Id", id, "Activity", "InvSaleInvoiceJournal_ReadBySaleInvoiceID")) {
            Jv x = new Jv(); x.AccountId = toInt(ci(e, "ChartofAccountId")); x.Remarks = str(ci(e, "JvRemarks")); x.Percentage = toDouble(ci(e, "JvPrcnt"));
            x.Qty = toDouble(ci(e, "JvQty")); x.Rate = toDouble(ci(e, "JvRate")); x.Debit = toDouble(ci(e, "JvDebit")); x.Credit = toDouble(ci(e, "JvCredit"));
            v.gl.add(x);
        }
        SaleSaltDirectCalc k = calcFor(v);
        if (v.orderExist) k.totalGrossInCaseOfOrderEntry();
        if (v.exp.isEmpty()) k.addExpRow();
        if (v.gl.isEmpty()) k.addGlRow();
        k.afterRead();
        k.enableDisableCommissionFields();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("state", v);
        out.put("currencyZero", v.currencyId == 0);
        out.put("voucherHeadId", repo.voucherHeadId(sup.org(), sup.company(), DOC_TYPE, id));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    // ------------------------------------------------------------------ History (GetAll :3170)

    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate, int fromNo, int toNo, int customerId) {
        UserAccount u = sup.user();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.historyByUser(u, DOC_TYPE, sup.fy(), true, dateType, day(fromDate), day(toDate), fromNo, toNo, customerId)) {
            out.add(row("Id", ci(r, "Id"), "VoucherHeadId", ci(r, "VoucherHeadId"), "GdnId", ci(r, "GdnId"), "DocNo", ci(r, "DocNo"), "DocDate", date10(ci(r, "DocDate")),
                    "SupplierCustomerId", ci(r, "SupplierCustomerId"), "CustomerName", ci(r, "CustomerName"), "ManualBillNo", ci(r, "ManualBillNo"), "DueDate", date10(ci(r, "DueDate")),
                    "CommAgent", ci(r, "CommissionAgent"), "CommType", ci(r, "CommissionType"), "CommRate", toDouble(ci(r, "CommRate")), "CommAmount", toDouble(ci(r, "CommAmount")),
                    "CommRemarks", ci(r, "CommissionRemarks"), "BillAmount", toDouble(ci(r, "BillAmount")), "EntryDate", date10(ci(r, "EntryDate")), "EntryUser", ci(r, "UserName"),
                    "IsReserve", toBool(ci(r, "IsReserve")), "NoOfAttachments", toInt(ci(r, "NoOfAttachments")), "Remarks", ci(r, "RemarksHeader")));
        }
        return out;
    }

    /** GetDetailGrdByHeadId - the 32 columns of grdDetail. */
    public List<Map<String, Object>> historyDetail(int id) {
        header(id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : sup.rows(P_INV, "Id", id, "Activity", "SaleDetailReadByInvSaleInvoiceId")) {
            out.add(row("Id", ci(d, "Id"), "OrderId", ci(d, "SaleOrderId"), "OrderDetailId", ci(d, "SaleOrderDetailId"), "OrderNo", ci(d, "SaleOrder"), "WarehouseId", ci(d, "WarehouseId"),
                    "Warehouse", ci(d, "WareHouseName"), "ItemId", ci(d, "ItemId"), "Item", ci(d, "ItemName"), "ItemCode", ci(d, "ItemCode"), "JobLotId", ci(d, "JobLotId"),
                    "JobLot", ci(d, "JobLotDescription"), "PackingTypeId", ci(d, "PackingTypeId"), "PackingType", ci(d, "PackTypeDesc"), "PackUOMId", ci(d, "ItemUOMId"),
                    "PackUOM", ci(d, "PackEquivalent"), "ItemQty", ci(d, "ItemQty"), "GrossWeight", ci(d, "GrossWeight"), "AddLss", ci(d, "AdLsWeight"), "NetBillWeight", ci(d, "NetBillWeight"),
                    "Rate", ci(d, "ItemRate"), "RateUOMId", ci(d, "UomScheduleIdRate"), "RateUOM", ci(d, "RateUOM"), "RateCutAmount", ci(d, "RateCutAmount"), "ItemAmount", ci(d, "ItemAmount"),
                    "GpDate", date10(ci(d, "GpDate")), "GpNo", ci(d, "GpNo"), "VehicleNo", ci(d, "VehicleNo"), "FcyAmount", ci(d, "FcyAmount"), "BillAmount", ci(d, "BillAmount"),
                    "Expense", ci(d, "ExpenseAmount"), "Freights", ci(d, "FreightAmount"), "Commission", ci(d, "CommissionAmount")));
        }
        return out;
    }

    /** CommonServices.VoucherHeadIdGet(id, 1809) for the history grid's Voucher button. */
    public int voucherHeadId(int id) { header(id); return repo.voucherHeadId(sup.org(), sup.company(), DOC_TYPE, id); }

    // ------------------------------------------------------------------ Save (Insert :2642)

    public static class SaveRequest {
        public Inv state;
        public SaleEngrAttachments.Change attachments;
    }

    private static LocalDateTime day(String s) {
        if (s == null || s.isBlank()) return null;
        try { return LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))).atStartOfDay(); } catch (RuntimeException e) { return null; }
    }

    private static Timestamp ts(String s) { LocalDateTime d = day(s); return d == null ? null : Timestamp.valueOf(d); }

    /** FormValidation :2274 - the first failing message (and the control to focus), word for word; null when valid. */
    private String[] formValidation(Inv v) {
        String no = text(v.docNo);
        if (no.isEmpty() || "0".equals(no)) return new String[]{"DocNo Field is Required", "txtdocno"};
        if (v.supplierId <= 0) return new String[]{"CustomerName Field is Required", "comsupplier"};
        if (v.paymentTermId <= 0) return new String[]{"Payment Term Field is Required", "combpttrm"};
        if (multiCurrency()) {
            if (v.currencyId <= 0) return new String[]{"Fcy Code Field is Required", "cmbCurrency"};
            if (text(v.exchangeRate).isEmpty() || "0".equals(text(v.exchangeRate))) return new String[]{"Exchange Rate Field is Required", "txtExchangeRate"};
            if (text(v.fcyAmount).isEmpty() || "0".equals(text(v.fcyAmount))) return new String[]{"Fcy Amount Rate Field is Required", "txtFcyAmount"};
        } else {
            if (v.currencyId <= 0) return new String[]{"Please Configure Your Base Currency In configurations", "cmbCurrency"};
            if (text(v.exchangeRate).isEmpty() || "0".equals(text(v.exchangeRate))) return new String[]{"Please Configure Your Base Currency Rate In configurations", "txtExchangeRate"};
        }
        if (v.commAgentId > 0 && toDouble(text(v.commAmount)) > 0.0) {
            if (toDouble(v.commRate) == 0.0) return new String[]{"Commission Rate Field is Required", "txtcommrate"};
            if (v.commRemarks == null || v.commRemarks.isEmpty()) return new String[]{"Commission Remarks Field is Required", "txtCommissionRemarks"};
        }
        if (v.paymentTermId <= 0) return new String[]{"Payment Term Field is Required", "combpttrm"};
        if (toInt(v.dueDays) == 0 && v.paymentTermId == 2) return new String[]{"Due Days Field is Required", "txtduedays"};
        String dt = text(v.deliveryTerm);
        if (!"Load".equals(dt) && !"Ponch".equals(dt)) return new String[]{"Delivery Term Field is Required", "combdeliverytrm"};
        return null;
    }

    /**
     * The part of Insert() that runs before the "Are you sure to Save/Update?" question: "Grid Record Not Found", FormValidation, "Transporter Account field Required".
     * Answers {ok:true} or {stopped:true, message, focus}.
     */
    public Map<String, Object> precheck(Inv v) {
        if (v == null || v.lines == null || v.lines.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        String[] m = formValidation(v);
        if (m != null) return row("stopped", true, "message", m[0], "focus", m[1]);
        if (toDouble(text(v.freightText)) > 0.0 && v.transporterId == 0) throw new IllegalArgumentException("Transporter Account field Required");
        return row("ok", true);
    }

    private boolean owned(String sql, int id) {
        Integer n = sup.jdbc().queryForObject(sql, Integer.class, id, sup.org(), sup.company());
        return n != null && n > 0;
    }

    private static Set<Integer> ids(List<Map<String, Object>> rows) {
        Set<Integer> s = new HashSet<>();
        for (Map<String, Object> r : rows) s.add(toInt(ci(r, "Id")));
        return s;
    }

    @Transactional
    public Map<String, Object> save(SaveRequest rq) {
        try {
            return doSave(rq);
        } catch (Stop s) {
            return row("stopped", true, "message", s.getMessage(), "focus", s.focus);
        }
    }

    private Map<String, Object> doSave(SaveRequest rq) {
        UserAccount u = sup.user();
        Inv v = rq.state;
        if (v == null || v.lines == null || v.lines.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        Map<String, Boolean> rights = sup.rights(SCREEN);
        if (v.id > 0 ? !Boolean.TRUE.equals(rights.get("update")) : !Boolean.TRUE.equals(rights.get("save"))) throw new Warning("You do not have the right to " + (v.id > 0 ? "update" : "save") + " this screen");
        String[] fv = formValidation(v);
        if (fv != null) throw new Stop(fv[0], fv[1]);
        if (toDouble(text(v.freightText)) > 0.0 && v.transporterId == 0) throw new IllegalArgumentException("Transporter Account field Required");

        Map<String, Object> existing = null;
        if (v.id > 0) {
            existing = header(v.id);
            if (toBool(ci(existing, "IsApproved"))) throw new IllegalArgumentException("Record Not Update beacause Record has approved");
        }
        for (Jv r : v.gl) if ((r.Credit > 0.0 || toInt(r.Debit) > 0) && r.AccountId == 0) throw new Stop("Please Select an Account Against JL First", "");
        for (Ex r : v.exp) if (r.Amount > 0.0 && r.ItemId == 0) throw new Stop("Please Select an Item Against Expense First", "");
        if (toDouble(v.commAmount) > 0.0 && v.commAgentId == 0) throw new Stop("Please Select Commission Agent Account First", "combcommAgent");

        // tenant checks the desktop gets from its own company-scoped combos
        Set<Integer> savedIds = new HashSet<>();
        if (v.id > 0) for (Map<String, Object> d : sup.rows(P_INV, "Id", v.id, "Activity", "SaleDetailReadByInvSaleInvoiceId")) savedIds.add(toInt(ci(d, "Id")));
        if (!ids(paymentTerms()).contains(v.paymentTermId)) throw new IllegalArgumentException("Payment term not found in this company");
        if (!ids(currencies()).contains(v.currencyId)) throw new IllegalArgumentException("Currency not found in this company");
        if (!owned("SELECT COUNT(*) FROM SupplierCustomer WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?", v.supplierId)) throw new IllegalArgumentException("Party not found in this company");
        if (v.commAgentId > 0 && !owned("SELECT COUNT(*) FROM SupplierCustomer WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?", v.commAgentId)) throw new IllegalArgumentException("Commission agent not found in this company");
        Set<Integer> accountIds = null;
        if (v.transporterId > 0 || !v.gl.isEmpty()) accountIds = ids(accounts());
        if (v.transporterId > 0 && !accountIds.contains(v.transporterId)) throw new IllegalArgumentException("Transporter account not found in this company");
        for (Jv r : v.gl) if (r.AccountId != 0 && !accountIds.contains(r.AccountId)) throw new IllegalArgumentException("Account not found in this company");
        Set<Integer> otherItemIds = ids(otherItems());
        for (Ex r : v.exp) if (r.ItemId != 0 && !otherItemIds.contains(r.ItemId)) throw new IllegalArgumentException("Expense item not found in this company");
        Set<Integer> whIds = ids(warehouses()), jobLotIds = ids(jobLots()), packIds = ids(packingTypes());
        Map<Integer, Boolean> itemIds = new HashMap<>();
        for (Map<String, Object> r : own.items(u)) itemIds.put(toInt(ci(r, "Id")), true);
        if (itemsAgainstWarehouse()) for (Line l : v.lines) if (l.WarehouseId > 0) for (Map<String, Object> r : items(l.WarehouseId)) itemIds.put(toInt(r.get("Id")), true);

        LocalDateTime now = LocalDateTime.now();
        Head h = new Head();
        h.Id = v.id > 0 ? v.id : 0;
        h.CommAmount = toDouble(v.commAmount);
        h.CommissionAgentId = v.commAgentId;
        h.CommissionRemarks = text(v.commRemarks);
        h.CommissionType = text(v.commType);
        h.CommRate = toDouble(v.commRate);
        h.UomScheduleIdCmRate = v.commUom == null ? "" : v.commUom;
        h.DocNo = toInt(text(v.docNo));
        h.DocDate = day(v.docDate);
        if (h.DocDate == null) throw new IllegalArgumentException("Doc Date is required");
        h.PaymentTermId = v.paymentTermId;
        h.DueDate = day(v.dueDate);
        h.ScreenName = SCREEN;
        h.DueDays = toInt(v.dueDays);
        h.DeliveryTerm = v.deliveryTerm == null ? "" : v.deliveryTerm;
        h.ManualBillNo = text(v.billNo);
        h.RemarksHeader = text(v.remarks);
        h.OtherRemarks = h.RemarksHeader;
        h.SupplierCustomerId = v.supplierId;
        h.TransporterId = v.transporterId;
        h.FreightAmount = toDouble(text(v.freightText));
        h.SupplierInvoiceDate = h.DueDate != null ? h.DueDate : now;       // = duedate.Value
        h.SupplierInvoiceNo = toInt(v.dueDays);                              // desktop: Conversion.ToInt(txtduedays.Text) (not a supplier invoice number)
        h.SupplierReferenceNo = text(v.refNo);
        h.BillAmount = toDouble(text(v.billAmount));
        h.DocumentTypeId = DOC_TYPE;
        h.EntryDate = now; h.ModifyDate = now;
        h.EntryUser = u.getId(); h.ModifyUser = u.getId();
        h.OrganizationId = u.getOrganizationId(); h.CompanyId = u.getCompanyId(); h.FinancialYearId = sup.fy();
        h.IsReserve = false;
        h.ExchangeRate = decText(v.exchangeRate);
        h.CurrencyId = v.currencyId;
        h.InvoiceQty = decText(v.invoiceQty);
        h.InvoiceWeight = decText(v.invoiceWeight);
        h.FcyAmount = decText(v.fcyAmount);
        h.AttachmentsValues = v.id > 0 ? nullable(ci(existing, "AttachmentsValues")) : "";
        h.CustomAttachmentsValues = v.id > 0 ? nullable(ci(existing, "CustomAttachmentsValues")) : "";

        List<Detail> details = new ArrayList<>();
        double grossTotal = 0.0;
        for (Line r : v.lines) {
            Detail d = new Detail();
            d.Id = r.Id;
            if (v.id > 0 && d.Id > 0 && !savedIds.contains(d.Id)) throw new IllegalArgumentException("Invalid detail row for this sale invoice");
            if (v.id == 0 && d.Id > 0) throw new IllegalArgumentException("Invalid detail row for this sale invoice");
            d.SaleOrderId = r.OrderId; d.SaleOrderDetailId = r.OrderDetailId;
            if (d.SaleOrderId > 0 && sup.jdbc().queryForObject("SELECT COUNT(*) FROM SaleOrderDetail d INNER JOIN SaleOrder h ON h.Id = d.SaleOrderId WHERE d.Id = ? AND h.Id = ? AND h.OrganizationId = ? AND h.CompanyId = ?",
                    Integer.class, d.SaleOrderDetailId, d.SaleOrderId, sup.org(), sup.company()) == 0) throw new IllegalArgumentException("Sale order not found in this company");
            d.WarehouseId = r.WarehouseId;
            if (d.WarehouseId == 0) throw new Stop("Warehouse field required", "");
            d.ItemId = r.ItemId;
            d.JobLotId = r.JobLotId;
            if (d.JobLotId == 0) throw new Stop("JobLot field required", "");
            d.PackingTypeId = r.PackingTypeId;
            if (d.PackingTypeId == 0) throw new Stop("PackingType field required", "");
            d.ItemUOMId = r.PackUOMId;
            if (d.ItemUOMId == 0) throw new Stop("PackUOM field required", "");
            d.ItemQty = r.ItemQty;
            if (d.ItemQty == 0.0) throw new Stop("ItemQty field required", "");
            d.GrossWeight = r.GrossWeight;
            if (d.GrossWeight == 0.0) throw new Stop("GrossWeight field required", "");
            grossTotal += d.GrossWeight;
            d.AdLsWeight = toDouble("-" + num(r.AddLss));                      // desktop: Conversion.ToDouble("-" + AddLss) (a negative AddLss gives 0)
            d.NetBillWeight = r.NetBillWeight;
            d.NetStockWeight = r.StockWeight;
            if (d.NetBillWeight == 0.0) throw new Stop("NetBillWeight field required", "");
            d.ItemRate = r.Rate;
            if (d.ItemRate == 0.0) throw new Stop("ItemRate field required", "");
            d.UomScheduleIdRate = r.RateUOMId;
            if (d.UomScheduleIdRate == 0) throw new Stop("RateUom field required", "");
            d.RateCutAmount = r.RateCutAmount;
            d.ItemAmount = r.ItemAmount;
            if (d.ItemAmount == 0.0) throw new Stop("ItemAmount field required", "");
            LocalDateTime gp = day(r.GpDate);
            d.GpDate = gp != null ? gp : now;
            d.GpNo = r.GpNo;
            d.VehicleNo = r.VehicleNo == null ? "" : r.VehicleNo;
            d.BillAmount = r.BillAmount;
            d.ExpenseAmount = r.Expense;
            d.FreightAmount = r.Freights;
            d.CommissionAmount = r.Commission;
            d.CurrencyId = v.currencyId;
            d.ExchangeRate = decText(text(v.exchangeRate));
            d.FcyAmount = dec(r.FcyAmount);
            if (!itemIds.containsKey(d.ItemId)) throw new IllegalArgumentException("Item not found in this company");
            if (!whIds.contains(d.WarehouseId)) throw new IllegalArgumentException("Warehouse not found in this company");
            if (!jobLotIds.contains(d.JobLotId)) throw new IllegalArgumentException("Job lot not found in this company");
            if (!packIds.contains(d.PackingTypeId)) throw new IllegalArgumentException("Packing type not found");
            for (int uomId : new int[]{d.ItemUOMId, d.UomScheduleIdRate})
                if (sup.jdbc().queryForObject("SELECT COUNT(*) FROM V_UomScheduleAndUom WHERE Id = ? AND CompanyId = ?", Integer.class, uomId, sup.company()) == 0) throw new IllegalArgumentException("UOM not found in this company");
            details.add(d);
        }
        List<Expense> expenses = new ArrayList<>();
        for (Ex r : v.exp) if (r.ItemId != 0) {
            Expense pe = new Expense();
            pe.InvRevExpItemId = r.ItemId; pe.Qty = toInt(r.Qty); pe.Rate = r.Rate; pe.Amount = r.Amount; pe.Remarks = r.Remarks == null ? "" : r.Remarks;   // PE.Qty = Conversion.ToInt
            expenses.add(pe);
        }
        List<Journal> journals = new ArrayList<>();
        for (Jv r : v.gl) if (r.AccountId != 0) {
            Journal pj = new Journal();
            pj.ChartofAccountId = r.AccountId; pj.JvRemarks = r.Remarks == null ? "" : r.Remarks; pj.JvPrcnt = r.Percentage; pj.JvQty = r.Qty; pj.JvRate = r.Rate;
            pj.JvDebit = r.Debit; pj.JvCredit = r.Credit;
            journals.add(pj);
        }
        List<Commission> commissions = new ArrayList<>();
        if (v.commAgentId > 0 && toDouble(v.commAmount) > 0.0) {
            Commission cm = new Commission();
            cm.Id = 0; cm.SaleOrderId = 0; cm.CommDebitSupCustId = 0; cm.DebitAccountId = 0; cm.CommissionAgentId = v.commAgentId;
            String t = v.commType == null ? "" : v.commType;
            if ("Percent".equals(t)) cm.CommType = 2.0;
            if ("Flat".equals(t)) cm.CommType = 1.0;
            if ("Comm Weight".equals(t)) cm.CommType = 3.0;
            cm.Rate = toDouble(v.commRate); cm.RateUom = toDouble(v.commUom); cm.CommAmount = toDouble(v.commAmount); cm.Remarks = v.commRemarks == null ? "" : v.commRemarks;
            commissions.add(cm);
        }

        SaleInvoiceFinancialDirect.Invoice inv = new SaleInvoiceFinancialDirect.Invoice();
        inv.h = h;
        inv.details.addAll(details);
        inv.expenses.addAll(expenses);
        inv.journals.addAll(journals);
        inv.commissions.addAll(commissions);

        /* BLL 0580 InvSaleInvoice.Save */
        boolean update = h.Id > 0;
        if (h.Id == 0) h.ModifyUser = 0; else h.EntryUser = 0;
        SaleInvoiceFinancialDirect.Voucher voucher = new SaleInvoiceFinancialDirect(repo).makeVoucherForSaleInvoice(inv);
        int saved = persist.persist(inv, voucher, h.Id == 0 ? "Sp_InvSaleInvoice_Insert" : "Sp_InvSaleInvoice_Update", SCREEN, rq.attachments);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", saved);
        out.put("docNo", h.DocNo);
        out.put("updated", update);
        out.put("voucherHeadId", repo.voucherHeadId(h.OrganizationId, h.CompanyId, DOC_TYPE, saved));
        out.put("grossWeightTotal", grossTotal);
        out.put("message", (update ? "Record Update Successfully [" : "Record Saved Successfully [") + h.DocNo + "] ");
        return out;
    }

    private static String nullable(Object o) { return o == null ? null : String.valueOf(o); }

    // ------------------------------------------------------------------ attachments

    public List<Map<String, Object>> attachmentList(int id) { header(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { header(id); return attachments.download(SCREEN, id, attachmentId); }
}
