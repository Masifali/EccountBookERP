package com.mst.services.sale.steel;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import com.mst.services.sale.engr.SaleEngrSupport;
import org.springframework.stereotype.Service;

import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;

/**
 * Module 87 "Sales Steel Reports" - the four registers that had no web page.
 *   557 SaleInvoiceRegisterSteel              Architecture.WinApp.Steel.Reports.SalesReports.SaleInvoiceRegisterSteel
 *   558 SaleInvoiceRegisterWithActivities     Architecture.WinApp.Steel.Reports.SalesReports.SaleInvoiceRegisterWithActivities
 *   559 frmGPOutwardRegister                  Architecture.WinApp.Steel.Reports.SalesReports.frmGPOutwardRegister
 *   560 frmSaleOrderSlipAndRegister           Architecture.WinApp.Inventory_Reports.frmSaleOrderSlipAndRegister (uses the Steel BLL)
 * Tenancy always comes from the session (SaleEngrSupport / CurrentUserContext). The rights are the desktop ScreenId "View" right
 * (DesktopReportRights). Every guarded parameter of the BLL ("if (obj.X != 0)") is left out of the EXEC when unset.
 * Nothing here was run against a database.
 */
@Service
public class SaleSteelReportService {
    public static final Map<String, Integer> SCREENS;
    static {
        Map<String, Integer> m = new LinkedHashMap<>();
        m.put("sale-invoice-register", 557);
        m.put("sale-invoice-register-with-activities", 558);
        m.put("gp-outward-register", 559);
        m.put("sale-order-slip-register", 560);
        SCREENS = Collections.unmodifiableMap(m);
    }

    private final SaleEngrSupport sup;
    private final SaleSteelSupport steel;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public SaleSteelReportService(SaleEngrSupport sup, SaleSteelSupport steel, CurrentUserContext context, DesktopReportRights rights) {
        this.sup = sup; this.steel = steel; this.context = context; this.rights = rights;
    }

    public UserAccount user(String key) {
        Integer screen = SCREENS.get(key);
        if (screen == null) throw new IllegalArgumentException("Unknown report " + key);
        UserAccount u = context.requireAccountingUser();
        rights.require(u, screen, "View");
        return u;
    }

    // ------------------------------------------------------------------ helpers
    private static int i(Object v) { return SaleEngrSupport.toInt(v); }
    private static double dbl(Object v) { return SaleEngrSupport.toDouble(v); }
    private static String s(Object v) { return v == null ? "" : String.valueOf(v); }
    private static String q(Map<String, String> m, String k) { String v = m.get(k); return v == null ? "" : v.trim(); }
    private static Object ci(Map<String, Object> r, String key) { return SaleEngrSupport.ci(r, key); }

    private static LocalDate day(String v) {
        if (v == null || v.trim().length() < 10) return null;
        try { return LocalDate.parse(v.trim().substring(0, 10)); } catch (Exception e) { return null; }
    }

    /** A DateTimePicker value reaches the BLL as a DateTime (CheckDateTimeNull is false), so the dates are always sent. */
    private static void dates(Map<String, Object> p, String fromKey, String toKey, Map<String, String> q) {
        LocalDate f = day(q.get("fromDate")), t = day(q.get("toDate"));
        if (f != null) p.put(fromKey, Date.valueOf(f));
        if (t != null) p.put(toKey, Date.valueOf(t));
    }

    private static void nz(Map<String, Object> p, String key, int v) { if (v != 0) p.put(key, v); }

    /** GetDecimalConfiguration: stringFormatsingle / DecimalRateFormate zeros. */
    private int amountDecimals() {
        int n = i(sup.config("Default NoofDecimal Points For Amount"));
        return n >= 1 && n <= 4 ? n : 0;
    }

    private int rateDecimals() {
        int n = i(sup.config("Default NoofDecimal Points For Rate"));
        if (n >= 1 && n <= 4) return n;
        return n == 0 ? 2 : 0;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period of the session's financial year. */
    private String yearStart(UserAccount u) {
        int yearId = sup.fy();
        List<Map<String, Object>> years = sup.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        Map<String, Object> pick = null;
        for (Map<String, Object> y : years) if (i(ci(y, "Id")) == yearId) { pick = y; break; }
        if (pick == null && !years.isEmpty() && yearId <= 0) pick = years.get(0);
        if (pick == null) return null;
        Object start = ci(pick, "Start_Period");
        if (start == null) return null;
        if (start instanceof Timestamp) return ((Timestamp) start).toLocalDateTime().toLocalDate().toString();
        if (start instanceof java.util.Date) return new Date(((java.util.Date) start).getTime()).toLocalDate().toString();
        String t = String.valueOf(start);
        return t.substring(0, Math.min(10, t.length()));
    }

    private Map<String, Object> basics(UserAccount u) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("amountDecimals", amountDecimals());
        d.put("rateDecimals", rateDecimals());
        d.put("yearStart", yearStart(u));
        return d;
    }

    /** DropDownBind source rows {Id, name} of one Activity of a lookup procedure (the desktop copies the rows of that Activity into a two column table). */
    private static List<Map<String, Object>> activity(List<Map<String, Object>> all, String act, String idKey, String nameKey) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : all) {
            if (!act.equals(s(ci(r, "Activity")))) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, idKey)); o.put("name", ci(r, nameKey));
            out.add(o);
        }
        return out;
    }

    private static final String P_DROP = "[ST].[USP_GetDataForDropDownFromSaleInvoice]";

    /** InvSaleInvoice.GetDataForDropDownFromSaleInvoiceForSteel: Org, Company (Activity is null and is not sent). */
    private List<Map<String, Object>> dropDownRows(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId());
        return steel.table(P_DROP, p).rows;
    }

    // ================================================================== lookups (the forms' Load)
    public Map<String, Object> lookups(String key) {
        UserAccount u = user(key);
        Map<String, Object> d = basics(u);
        switch (key) {
            case "sale-invoice-register": {                     // GetDataForSaleInvoiceRegisterDropdownBind
                List<Map<String, Object>> all = dropDownRows(u);
                d.put("customers", activity(all, "Customer", "Id", "ReferenceName"));
                d.put("warehouses", activity(all, "Warehouse", "Id", "ReferenceName"));
                d.put("jobLots", activity(all, "JobLot", "Id", "ReferenceName"));
                d.put("items", activity(all, "Item", "Id", "ReferenceName"));
                return d;
            }
            case "sale-order-slip-register": {                  // combobind
                List<Map<String, Object>> all = dropDownRows(u);
                d.put("parties", activity(all, "Customer", "Id", "ReferenceName"));
                d.put("items", activity(all, "Item", "Id", "ReferenceName"));
                d.put("parentCategories", activity(all, "ParentCategory", "Id", "ReferenceName"));
                d.put("itemCategories", activity(all, "ItemCategory", "Id", "ReferenceName"));
                d.put("itemTypes", activity(all, "ItemType", "Id", "ReferenceName"));
                return d;
            }
            case "sale-invoice-register-with-activities":
                d.putAll(activitiesLookups(u));
                return d;
            case "gp-outward-register":                         // CommonServices.SupplierCustomerGetforComboServiceBind
                d.put("customers", sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                        "Activity", "ReadByOrganizationIdCompanyIdForBinding"));
                return d;
            default: return d;
        }
    }

    /** 558 AllComboBind: USP_GetDataFromInventoryStocksEvaluationsForSales (Org, Company, UserId, AppId, DocType 'Sale'); Activity is not sent. */
    private Map<String, Object> activitiesLookups(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId());
        p.put("UserId", u.getId());
        p.put("AppId", u.getAppId() != null && u.getAppId() > 0 ? u.getAppId() : context.currentAppId());
        p.put("DocType", "Sale");
        List<Map<String, Object>> all = steel.table("USP_GetDataFromInventoryStocksEvaluationsForSales", p).rows;
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("parentCategories", activity(all, "GetParentCategory", "Id", "RefName"));
        d.put("itemTypes", activity(all, "GetItemType", "Id", "RefName"));
        d.put("itemClasses", activity(all, "GetItemType", "Id", "RefName"));           // dtClass is filled from rowsType in the desktop code
        d.put("itemCategories", activity(all, "GetItemCategory", "Id", "RefName"));
        d.put("parties", activity(all, "GetSupplierCustomer", "Id", "RefName"));
        d.put("items", activity(all, "GetItems", "Id", "RefName"));
        d.put("cities", activity(all, "GetCity", "Id", "RefName"));
        d.put("jobLots", activity(all, "GetJobLot", "Id", "RefName"));
        d.put("warehouses", activity(all, "GetWarehouse", "Id", "RefName"));
        d.put("districts", activity(all, "GetDistrict", "Id", "RefName"));
        return d;
    }

    /** 558 btnNew_Click: AllComboBind, ActivityFill, ItemUOMFill - the lists are re-read. */
    public Map<String, Object> activitiesCombos() { return activitiesLookups(user("sale-invoice-register-with-activities")); }

    // ================================================================== 557 SaleInvoiceRegisterSteel
    private static final String P_INV = "[ST].[USP_SaleInvoiceRegister]";
    private static final String P_INV_DIRECT = "[ST].[USP_SaleInvoiceRegisterDirect]";

    /** The ReportsParameters of gridHisory / DirectRegisterHistory (BillNoFrom/To are set by the desktop but are not parameters of the BLL call). */
    private Map<String, Object> invoiceParams(UserAccount u, Map<String, String> q) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId());
        dates(p, "FromDate", "ToDate", q);
        nz(p, "FromDocNo", i(q.get("fromDocNo"))); nz(p, "ToDocNo", i(q.get("toDocNo")));
        nz(p, "SupplierCustomerId", i(q.get("supplierCustomerId")));
        nz(p, "ItemId", i(q.get("itemId")));
        nz(p, "JobLotId", i(q.get("jobLotId")));
        nz(p, "WarehouseId", i(q.get("warehouseId")));
        double rf = dbl(q.get("rateFrom")), rt = dbl(q.get("rateTo"));
        if (rf != 0.0) p.put("RateFrom", rf);
        if (rt != 0.0) p.put("RateTo", rt);
        return p;
    }

    /** dtInvoice / dtInvoiceDirect: the procedure's own rows (this is what the Print drop-downs hand to the .rpt). */
    public List<Map<String, Object>> invoiceRaw(Map<String, String> q) {
        UserAccount u = user("sale-invoice-register");
        return steel.table("direct".equals(q.get("mode")) ? P_INV_DIRECT : P_INV, invoiceParams(u, q)).rows;
    }

    private static Object put(Map<String, Object> o, String k, Object v) { o.put(k, v); return v; }

    /** The grid table of gridHisory (mode flow, 52 columns) / DirectRegisterHistory (mode direct, 44 columns). */
    public List<Map<String, Object>> invoiceRegister(Map<String, String> q) {
        boolean direct = "direct".equals(q.get("mode"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : invoiceRaw(q)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id")); o.put("InvoiceDetailId", ci(r, "InvSaleDetailId")); o.put("DocumentTypeId", ci(r, "DocumentTypeId")); o.put("VoucherHeadId", ci(r, "VoucherHeadId"));
            o.put("DocDate", ci(r, "DocDate")); o.put("DocNo", ci(r, "DocNo")); o.put("InvoiceType", ci(r, "OrderType"));
            if (!direct) {
                o.put("OrderId", ci(r, "OrderId")); o.put("OrderDate", ci(r, "DocDatepo")); o.put("OrderNo", ci(r, "DocNopo"));
                o.put("GpId", ci(r, "GpId"));
            }
            o.put("GpDate", ci(r, "GpDate")); o.put("GpSrNo", ci(r, "GpSrNo"));
            if (!direct) { o.put("GrnId", ci(r, "GrnId")); o.put("GrnDate", ci(r, "DocDateGrn")); o.put("GrnNo", ci(r, "DocNoGrn")); }
            o.put("VehicleNo", ci(r, "VehicleNo"));
            if (!direct) o.put("BiltyNo", ci(r, "BiltyNo"));
            o.put("SupplierCustomerId", ci(r, "SupplierCustomerId")); o.put("CustomerName", ci(r, "CustomerName")); o.put("CommAgentName", ci(r, "CommAgentName"));
            o.put("CommType", ci(r, "CommType")); o.put("CommRate", ci(r, "CommRate")); o.put("CommAmount", ci(r, "CommAmount")); o.put("PartyBillAmount", ci(r, "BillAmountH"));
            o.put("DueDate", ci(r, "DueDate")); o.put("ManualBillNo", ci(r, "ManualBillNo")); o.put("SaleGLAC", ci(r, "SaleGLAC"));
            o.put("ItemId", ci(r, "ItemId")); o.put("ItemName", ci(r, "ItemName")); o.put("WareHouseCode", ci(r, "WareHouseCode")); o.put("JobLotCode", ci(r, "JobLotCode"));
            o.put("PackTypeCode", ci(r, "PackTypeCode")); o.put("UOMCode", ci(r, "UOMCode")); o.put("ItemQty", ci(r, "ItemQty")); o.put("GrossWeight", ci(r, "GrossWeight"));
            o.put("WtCutPerUnit", ci(r, "WtCutPerUnit")); o.put("WtCutTotal", ci(r, "WtCutTotal")); o.put("AdLsWeight", ci(r, "AdLsWeight")); o.put("NetBillWeight", ci(r, "NetBillWeight"));
            o.put("NetStockWeight", ci(r, "NetStockWeight")); o.put("ItemRate", ci(r, "ItemRate")); o.put("NetRate", ci(r, "NetRate")); o.put("RateUom", ci(r, "RateUom"));
            o.put("RateCut", ci(r, "RateCut")); o.put("RateCutAmount", ci(r, "RateCutAmount")); o.put("ItemAmount", ci(r, "ItemAmount")); o.put("FreightAmount", ci(r, "FreightAmount"));
            o.put("ExpenseAmount", ci(r, "ExpenseAmount")); o.put("CommissionAmount", ci(r, "CommissionAmount")); o.put("ItemNetAmount", ci(r, "ItemNetAmount"));
            o.put("EntryDate", ci(r, "EntryDate")); o.put("EntryUser", ci(r, "EntryUserName"));
            out.add(o);
        }
        return out;
    }

    /** CommonServices.SaleInvoiceSteelSlip_1514: USP_InvSaleInvoice_CustomerBill (Org, Company, Id). */
    public List<Map<String, Object>> invoiceSlip(int id, boolean direct) {
        UserAccount u = user("sale-invoice-register");
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId());
        nz(p, "Id", id);
        return steel.table(direct ? "[ST].[USP_InvSaleInvoice_CustomerDirectBill]" : "[ST].[USP_InvSaleInvoice_CustomerBill]", p).rows;
    }

    /** InvSaleInvoice.SaleInvoiceSlipReport1514SupReprt: [ST].[USP_SaleInvoiceCustomerBill_OthersExp_SubReport] @PihId. */
    public List<Map<String, Object>> invoiceSlipSub(int id) {
        user("sale-invoice-register");
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("PihId", id);
        return steel.table("[ST].[USP_SaleInvoiceCustomerBill_OthersExp_SubReport]", p).rows;
    }

    // ================================================================== 558 SaleInvoiceRegisterWithActivities
    private static final String P_ACT = "[ST].[USP-SaleInvoiceRegisterWithActivities]";

    /** ActivityFill: the 14 activity names exactly as the desktop (the combo's Text is what the procedure receives and what the grid branches on). */
    public static final String[] ACTIVITIES = {
        "Sales Register", "Sales Summary By Item", "Sales Summary By Item & City", "Sales Summary By Item & Pack Size", "Sales Summary By Item & Warehouse",
        "Sales Summary By Item,Pack Size & City", "Sales Summary By Customer", "Sales Summary By Customer & Item", "Sales Summary By Customer & City",
        "Sales Summary By Customer,Item & City", "Sales Summary By Customer & Pack Size", "Sales Summary By Parent Category",
        "Sales Summary By Parent Category & Item", "Sales Summary By Parent Category & Customer" };

    /** GridFill's ReportsParameters. PackSizeFrom (the UOM combo) is set by the desktop but the BLL reads RateUOM, which is never set, so @PackUomId is never sent. */
    private Map<String, Object> activityParams(UserAccount u, Map<String, String> q) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId());
        dates(p, "FromDate", "ToDate", q);
        nz(p, "DocNoFrom", i(q.get("fromDocNo"))); nz(p, "DocNoTo", i(q.get("toDocNo")));
        nz(p, "WarehouseId", i(q.get("warehouseId"))); nz(p, "ItemId", i(q.get("itemId")));
        nz(p, "InventoryParentCategories", i(q.get("parentCategoryId"))); nz(p, "ItemCategoryId", i(q.get("itemCategoryId")));
        nz(p, "ItemClassGroupId", i(q.get("itemClassId"))); nz(p, "ItemTypeId", i(q.get("itemTypeId")));
        nz(p, "SupplierCustomerId", i(q.get("supplierCustomerId")));
        nz(p, "JobLotId", i(q.get("jobLotId"))); nz(p, "DistrictId", i(q.get("districtId"))); nz(p, "CityId", i(q.get("cityId")));
        String act = q(q, "activity");
        if (!act.isEmpty()) p.put("ActivityName", act);
        return p;
    }

    /** dtGrid: the procedure's own rows (the .rpt gets these). */
    public List<Map<String, Object>> activitiesRaw(Map<String, String> q) {
        UserAccount u = user("sale-invoice-register-with-activities");
        return steel.table(P_ACT, activityParams(u, q)).rows;
    }

    /** target column <- source column of the dtSales... table of every activity (the order of dtXxx.Rows.Add). */
    private static final String[][] M_REGISTER = { {"DocDate","DocDate"}, {"DocNo","DocCodeNo"}, {"Id","RefDocIdNo"}, {"DocumentTypeId","RefDocumentTypeId"}, {"DocType","DocumentTypeCode"},
        {"PartyName","Customer"}, {"ItemCode","ItemCode"}, {"ItemName","ItemName"}, {"PackUom","UOMCode"}, {"JobLot","JobLotCode"}, {"PackingType","PackTypeCode"}, {"VehicleNo","VehicleNo"},
        {"ItemQty","QtyOut"}, {"BillWeight","BillWeightOut"}, {"Amount","AmountOut"}, {"AvgRate","AvgRate"}, {"PrctByAmount","PrcntOfTotal"}, {"PrctByWeight","PrcntOfTotalWeight"}, {"CityName","CityName"} };
    private static final String[][] TAIL = { {"ItemQty","QtyOut"}, {"BillWeight","BillWeightOut"}, {"Amount","AmountOut"}, {"AvgRate","AvgRate"}, {"PrctByAmount","PrcntOfTotal"}, {"PrctByWeight","PrcntOfTotalWeight"} };
    private static final String[] CITY = { "CityName", "CityName" };

    private static String[][] map(String[][] head, boolean city) {
        List<String[]> l = new ArrayList<>(Arrays.asList(head));
        l.addAll(Arrays.asList(TAIL));
        if (city) l.add(CITY);
        return l.toArray(new String[0][]);
    }

    private static String[][] mapFor(String activity) {
        switch (activity) {
            case "Sales Register": return M_REGISTER;
            case "Sales Summary By Item": case "Sales Summary By Item & City":
                return map(new String[][] { {"ItemCode","ItemCode"}, {"ItemName","ItemName"} }, true);
            case "Sales Summary By Item & Pack Size": case "Sales Summary By Item,Pack Size & City":
                return map(new String[][] { {"ItemCode","ItemCode"}, {"ItemName","ItemName"}, {"PackUom","UOMCode"} }, true);
            case "Sales Summary By Item & Warehouse":
                return map(new String[][] { {"WarehouseName","WarehouseName"}, {"ItemCode","ItemCode"}, {"ItemName","ItemName"} }, true);
            case "Sales Summary By Customer": case "Sales Summary By Customer & City":
                return map(new String[][] { {"PartyName","Customer"}, {"PackUom","UOMCode"} }, true);
            case "Sales Summary By Customer & Item": case "Sales Summary By Customer,Item & City":
                return map(new String[][] { {"PartyName","Customer"}, {"ItemCode","ItemCode"}, {"ItemName","ItemName"}, {"PackUom","UOMCode"} }, true);
            case "Sales Summary By Customer & Pack Size":
                return map(new String[][] { {"PartyName","Customer"}, {"PackUom","UOMCode"} }, false);
            case "Sales Summary By Parent Category":
                return map(new String[][] { {"ParentCategory","ParentCategory"} }, false);
            case "Sales Summary By Parent Category & Item":
                return map(new String[][] { {"ItemName","ItemName"}, {"ParentCategory","ParentCategory"} }, false);
            case "Sales Summary By Parent Category & Customer":
                return map(new String[][] { {"CustomerName","Customer"}, {"ParentCategory","ParentCategory"} }, false);
            default: return null;
        }
    }

    /** GridFill: {columns:[names in table order], rows:[...]}; an unknown activity text or no rows leaves the grid cleared (ClearStructure). */
    public Map<String, Object> activitiesGrid(Map<String, String> q) {
        String act = q(q, "activity");
        List<Map<String, Object>> raw = activitiesRaw(q);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("activity", act);
        String[][] m = raw.isEmpty() ? null : mapFor(act);
        if (m == null) { out.put("columns", new ArrayList<>()); out.put("rows", new ArrayList<>()); return out; }
        List<String> cols = new ArrayList<>();
        for (String[] c : m) cols.add(c[0]);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String[] c : m) o.put(c[0], ci(r, c[1]));
            rows.add(o);
        }
        out.put("columns", cols); out.put("rows", rows);
        return out;
    }

    // ================================================================== 559 frmGPOutwardRegister
    private static final String P_GP = "[ST].[USp_GatePassOutward_SlipAndRegisterSteel_Rpt]";

    private Map<String, Object> gpParams(UserAccount u, Map<String, String> q) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId());
        nz(p, "SupplierCustomerId", i(q.get("supplierCustomerId")));
        dates(p, "FromDate", "ToDate", q);
        nz(p, "FromDocNo", i(q.get("fromDocNo"))); nz(p, "ToDocNo", i(q.get("toDocNo")));
        nz(p, "Id", i(q.get("id")));
        String st = q(q, "status");
        if (!st.isEmpty()) p.put("Status", st);
        return p;
    }

    /** dtReg: the procedure's rows (1520 register and 1512 slip print these). */
    public List<Map<String, Object>> gpRaw(Map<String, String> q) {
        UserAccount u = user("gp-outward-register");
        return steel.table(P_GP, gpParams(u, q)).rows;
    }

    /** gridHistory's 40 column table. */
    public List<Map<String, Object>> gpRegister(Map<String, String> q) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : gpRaw(q)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", i(ci(r, "Id"))); o.put("GpSrNo", i(ci(r, "GpSrNo"))); o.put("GpDate", ci(r, "GpDate")); o.put("GatepassType", s(ci(r, "GatepassType")));
            o.put("CustomerName", s(ci(r, "CompanyName"))); o.put("OrderType", s(ci(r, "OtherSupCust"))); o.put("OrderNo", i(ci(r, "SupplierContractCode")));
            o.put("OrderQty", i(dbl(ci(r, "DoQty")))); o.put("OrderGrossWeight", dbl(ci(r, "GrossWeight"))); o.put("PackUOM", s(ci(r, "PackUOM")));
            o.put("GpQty", dbl(ci(r, "NoOfPackages"))); o.put("PackingType", s(ci(r, "PackingType"))); o.put("VehicleType", s(ci(r, "VehicleType")));
            o.put("VehicleNo", s(ci(r, "VehicleNo"))); o.put("BiltyNo", s(ci(r, "BiltyNo"))); o.put("InDateTime", ci(r, "InDateTimeStamp")); o.put("OutDateTime", ci(r, "OutDateTimeStamp"));
            o.put("CityName", s(ci(r, "CityName"))); o.put("Freight", dbl(ci(r, "NetPaid"))); o.put("SupplierWeight", dbl(ci(r, "SupplierWeight")));
            o.put("FactoryWeight", dbl(ci(r, "FactoryWeight"))); o.put("DifferenceWeight", dbl(ci(r, "DifferenceWeight"))); o.put("NetWeightWb", dbl(ci(r, "NetWeightWb")));
            o.put("ContainerNo", String.valueOf(i(ci(r, "Container")))); o.put("WareHouseName", s(ci(r, "WareHouseName"))); o.put("VarietyName", s(ci(r, "VarietyName")));
            o.put("ItemName", s(ci(r, "ItemName"))); o.put("JobLot", s(ci(r, "JobLotDescription"))); o.put("EntryDate", ci(r, "EntryDate")); o.put("EntryUser", s(ci(r, "UserNameEuser")));
            o.put("ModifyDate", ci(r, "ModifyDate")); o.put("ModifyUser", s(ci(r, "UsernameMUser"))); o.put("ApprovedUser", s(ci(r, "UserNameAUser")));
            o.put("ApprovedDate", ci(r, "PostDate")); o.put("IsApproved", s(ci(r, "IsApproved"))); o.put("GPStatus", s(ci(r, "Status"))); o.put("OtherRemarks", s(ci(r, "OtherRemarks")));
            o.put("GPRemarks", i(ci(r, "GpRemarks"))); o.put("WtDiffRemarks", s(ci(r, "WtDiffRemarks"))); o.put("NoOfAttachtment", String.valueOf(i(ci(r, "DocAttachment"))));
            out.add(o);
        }
        return out;
    }

    // ================================================================== 560 frmSaleOrderSlipAndRegister
    private static final String P_SO = "[ST].[USP_SaleOrderSlipAndRegister]";

    private Map<String, Object> soParams(UserAccount u, Map<String, String> q) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId());
        dates(p, "FromDate", "Todate", q);
        nz(p, "FromDocNo", i(q.get("fromDocNo"))); nz(p, "ToDocNo", i(q.get("toDocNo")));
        String st = q(q, "status");
        if (!st.isEmpty()) p.put("Status", st);
        nz(p, "Id", i(q.get("id")));
        nz(p, "SupplierCustomerId", i(q.get("supplierCustomerId"))); nz(p, "ItemId", i(q.get("itemId")));
        // ApprovedFilter = the Approved combo text; the BLL sends @IsApproved unless it is "All"
        String filter = q(q, "approvedFilter");
        int approvedId = i(q.get("approvedId"));
        if (!"All".equals(filter)) p.put("IsApproved", approvedId == 2);
        nz(p, "ItemCategoryId", i(q.get("itemCategoryId"))); nz(p, "InventoryParentCategories", i(q.get("parentCategoryId"))); nz(p, "ItemTypeId", i(q.get("itemTypeId")));
        return p;
    }

    /** dt: the procedure's rows (the 1509 register prints these). */
    public List<Map<String, Object>> orderRaw(Map<String, String> q) {
        UserAccount u = user("sale-order-slip-register");
        return steel.table(P_SO, soParams(u, q)).rows;
    }

    /** CommonServices.SaleOrderSlip_1511: Id + ApprovedFilter "All". */
    public List<Map<String, Object>> orderSlip(int id) {
        UserAccount u = user("sale-order-slip-register");
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId());
        nz(p, "Id", id);
        return steel.table(P_SO, p).rows;
    }

    private static String boolText(Object v) {
        if (v instanceof Boolean) return ((Boolean) v) ? "True" : "False";
        return s(v);
    }

    /** gridHisory's 55 column table. */
    public List<Map<String, Object>> orderRegister(Map<String, String> q) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : orderRaw(q)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id")); o.put("PoDId", ci(r, "PoDId")); o.put("DocumentTypeId", ci(r, "DocumentTypeId")); o.put("DocumentTypeDescription", ci(r, "DocumentTypeDescription"));
            o.put("DocNo", ci(r, "DocNo")); o.put("DocDate", ci(r, "DocDate")); o.put("OrderCategory", ci(r, "OrderCategoryDescription")); o.put("CatagorySrNo", ci(r, "CatagorySrNo"));
            o.put("SupplierName", ci(r, "SupplierName")); o.put("OrderDueDate", ci(r, "OrderDueDate")); o.put("OrderDueDays", ci(r, "OrderDueDays")); o.put("OrderStatus", ci(r, "OrderStatus"));
            o.put("CommissionAgent", ci(r, "CompanyNameSpCAgent")); o.put("CommissionType", ci(r, "CommissionType")); o.put("CommRate", ci(r, "CommRate")); o.put("CommAmount", ci(r, "CommAmount"));
            o.put("CommRemarks", ci(r, "CommRemarks")); o.put("TermsDescription", ci(r, "TermsDescription")); o.put("OrderExpiryDate", ci(r, "OrderExpiryDate"));
            o.put("DeliveryTerm", ci(r, "DeliveryTerm")); o.put("DeliveryStartDate", ci(r, "DeliveryStartDate")); o.put("DeliveryDays", ci(r, "DeliveryDays"));
            o.put("DeliveryRemarks", ci(r, "DeliveryRemarks")); o.put("IsApproved", boolText(ci(r, "IsApproved"))); o.put("EntryUser", ci(r, "UserNameEusr")); o.put("EntryDate", ci(r, "EntryDate"));
            o.put("ModifyUser", ci(r, "UserNameMusr")); o.put("ModifyDate", ci(r, "ModifyDate")); o.put("ApprovedUser", ci(r, "UserNameAusr")); o.put("ApprovedDate", ci(r, "ApprovedDate"));
            o.put("ItemCode", ci(r, "ItemCode")); o.put("ItemName", ci(r, "ItemName")); o.put("PackUom", ci(r, "PackUom")); o.put("OrderItemQty", ci(r, "OrderItemQty"));
            o.put("DispatchQty", ci(r, "DispatchQty")); o.put("BalQty", ci(r, "BalQty")); o.put("NetWeight", ci(r, "NetWeight")); o.put("DispatchWeight", ci(r, "DispatchWeight"));
            o.put("BalWeight", ci(r, "BalWeight")); o.put("OrderItemRate", ci(r, "OrderItemRate")); o.put("RateUom", ci(r, "RateUom")); o.put("EquivalentRate", ci(r, "EquivalentRate"));
            o.put("ItemAmount", ci(r, "ItemAmount")); o.put("CityName", ci(r, "CityName")); o.put("JobLotCode", ci(r, "JobLotCode")); o.put("OrderItemId", ci(r, "OrderItemId"));
            o.put("SaleGLAC", ci(r, "SaleGLAC")); o.put("OrderSupCustId", ci(r, "OrderSupCustId")); o.put("PackTypeDesc", ci(r, "PackTypeDesc")); o.put("CurrencyCode", ci(r, "CurrencyCode"));
            o.put("ExchangeRate", ci(r, "ExchangeRate")); o.put("FcyAmount", ci(r, "FcyAmount")); o.put("ItemNetAmount", ci(r, "ItemNetAmount")); o.put("RemarksHeader", ci(r, "RemarksHeader"));
            o.put("NoOfAttachments", ci(r, "NoOfAttachments"));
            out.add(o);
        }
        return out;
    }

    // ================================================================== attachments (CommonServices.GetNoofAttachmentsByRefDocumentTypeID)
    /** DMSAttachments.ReadAttachmentsbyRefDocumentTypeId: Sp_DMSAttachments_GetAllMethod @RefDocumentTypeId, @Id, @Activity. AttachmentView shows AttachmentName, CustomName, EntryDate. */
    public List<Map<String, Object>> attachments(String key, int id, int documentTypeId) {
        user(key);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_DMSAttachments_GetAllMethod", "RefDocumentTypeId", documentTypeId, "Id", id, "Activity", "ReadAttachmentsbyRefDocumentTypeId")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id")); o.put("AttachmentName", ci(r, "Attachment")); o.put("CustomName", ci(r, "UploadedFileCustomName")); o.put("EntryDate", ci(r, "EntryDate"));
            out.add(o);
        }
        return out;
    }

    // ================================================================== print folders (CommonServices.DynamicReportsLoad)
    public static final Set<String> FOLDERS = new LinkedHashSet<>(Arrays.asList("Sales_Steel", "SalesDirect_Steel"));

    /** Name and address of the company for the @CompanyName / @CompanyAddress parameters of the .rpt files. */
    public UserAccount currentUser(String key) { return user(key); }
}
