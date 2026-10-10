package com.mst.services.sale.bk;

import com.mst.models.UserAccount;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalTime;
import java.util.*;

/**
 * Screen 586 SaleInvoice_Register = Architecture.WinApp.Salt.Reports.SaleInvoice_Register ("Sale Invoice Register"), module 95 (Sale Salt).
 * Tabs: "Detail Register" (Sale Invoice History) and "Summary Register" (Activity Wise Summary).
 * BLL: InvSaleInvoice.AllComboBindAgainstSaleInvoice (0580), InvSaleInvoiceReports.InvSaleInvoice_RegisterSalt (0128),
 *      InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales / SalesFromEvaulation_RegisterSalt (0574).
 *
 *   frmSaleInvoiceRegister_Load :274   ReportsLoad, Datetypefill, AllComboBind, gridHisory, the empty summary tables, AllComboBindSummary, ActivityFill, ItemUOMFill, activity row 0, PrintButtonManage
 *   AllComboBind :524                  Usp_AllComboAgainstSaleInvoice (Org, Company, AppId, UserId, DocumentTypeIds "1809"); Activity Supplier / WareHouse / ItemName / JobLot, Id + ReferenceName
 *   AllComboBindSummary :589           USP_GetDataFromInventoryStocksEvaluationsForSales (Org, Company, UserId, AppId, DocType "Sale"); Activity lists, Id + RefName; the item class combo
 *                                      is filled from the GetItemType rows (dtClass uses rowsType) and the history tab's cmbWareHouse is rebound with the GetWarehouse rows
 *   ItemUOMFill :676 / ActivityFill :703   fixed lists
 *   gridHisory :732                    USP_InvSaleInvoice_RegisterSalt; 41 column grid
 *   SummaryGridFill :1416              USP_SalesFromEvaulation_RegisterSalt (@ActivityName = the activity text); 14 table layouts
 */
@Service
public class SaleBkSaleRegisterService {
    private final SaleBkSupport bk;

    public SaleBkSaleRegisterService(SaleBkSupport bk) { this.bk = bk; }

    private static Object ci(Map<String, Object> r, String k) { return SaleBkSupport.ci(r, k); }
    private static int i(Object o) { return SaleBkSupport.i(o); }
    private static double dbl(Object o) { return SaleBkSupport.dbl(o); }
    private static String s(Object o) { return SaleBkSupport.s(o); }

    public Map<String, Object> lookups() {
        Map<String, Object> d = new LinkedHashMap<>(bk.basics());
        d.put("uom", Arrays.asList(1, 5, 10, 20, 25, 40, 50, 60, 65, 80, 100));
        return d;
    }

    private static Map<String, Object> idName(Object id, Object name) { return SaleBkSupport.p("Id", id, "name", name); }

    /** AllComboBind: null when the procedure returns nothing (the combos are left as they are). */
    public Map<String, Object> combos() {
        UserAccount u = bk.user();
        Map<String, Object> p = SaleBkSupport.p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "AppId", bk.appId(), "UserId", u.getId(), "DocumentTypeIds", "1809");
        List<Map<String, Object>> rows = bk.steel().table("[dbo].[Usp_AllComboAgainstSaleInvoice]", p).rows;
        if (rows.isEmpty()) return null;
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> sup = new ArrayList<>(), wh = new ArrayList<>(), item = new ArrayList<>(), job = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            String a = s(ci(r, "Activity"));
            Map<String, Object> o = idName(ci(r, "Id"), ci(r, "ReferenceName"));
            if ("Supplier".equals(a)) sup.add(o);
            else if ("WareHouse".equals(a)) wh.add(o);
            else if ("ItemName".equals(a)) item.add(o);
            else if ("JobLot".equals(a)) job.add(o);
        }
        out.put("supplier", sup); out.put("warehouse", wh); out.put("item", item); out.put("jobLot", job);
        return out;
    }

    /** AllComboBindSummary. */
    public Map<String, Object> summaryCombos() {
        UserAccount u = bk.user();
        Map<String, Object> p = SaleBkSupport.p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "UserId", u.getId(), "AppId", bk.appId(), "DocType", "Sale");
        List<Map<String, Object>> rows = bk.steel().table("USP_GetDataFromInventoryStocksEvaluationsForSales", p).rows;
        if (rows.isEmpty()) return null;
        Map<String, List<Map<String, Object>>> by = new LinkedHashMap<>();
        for (Map<String, Object> r : rows) by.computeIfAbsent(s(ci(r, "Activity")), k -> new ArrayList<>()).add(idName(ci(r, "Id"), ci(r, "RefName")));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("parentCategory", by.getOrDefault("GetParentCategory", new ArrayList<>()));
        out.put("itemType", by.getOrDefault("GetItemType", new ArrayList<>()));
        out.put("itemClass", by.getOrDefault("GetItemType", new ArrayList<>()));            // dtClass = rowsType copy
        out.put("itemCategory", by.getOrDefault("GetItemCategory", new ArrayList<>()));
        out.put("party", by.getOrDefault("GetSupplierCustomer", new ArrayList<>()));
        out.put("item", by.getOrDefault("GetItems", new ArrayList<>()));
        out.put("city", by.getOrDefault("GetCity", new ArrayList<>()));
        out.put("jobLot", by.getOrDefault("GetJobLot", new ArrayList<>()));
        out.put("warehouse", by.getOrDefault("GetWarehouse", new ArrayList<>()));         // bound to the HISTORY tab's cmbWareHouse
        out.put("district", by.getOrDefault("GetDistrict", new ArrayList<>()));
        return out;
    }

    private static Timestamp stamp(String day, String time) {
        if (day == null || day.trim().length() < 10) return null;
        LocalTime t = LocalTime.MIDNIGHT;
        if (time != null && time.length() >= 8) {
            try { t = LocalTime.parse(time.substring(0, 8)); } catch (RuntimeException ignored) { /* midnight */ }
        }
        return Timestamp.valueOf(LocalDate.parse(day.trim().substring(0, 10)).atTime(t));
    }

    private static void nzI(Map<String, Object> p, String k, int v) { if (v != 0) p.put(k, v); }
    private static void nzD(Map<String, Object> p, String k, double v) { if (v != 0.0) p.put(k, v); }

    /** InvSaleInvoice_RegisterSalt: the procedure rows as they come (also what the Print drop-down hands to the template). */
    public List<Map<String, Object>> raw(Map<String, String> q) {
        UserAccount u = bk.user();
        Map<String, Object> p = SaleBkSupport.p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        Timestamp from = stamp(q.get("fromDate"), q.get("fromTime")), to = stamp(q.get("toDate"), q.get("toTime"));
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        nzI(p, "DocNoFrom", i(q.get("fromDocNo"))); nzI(p, "DocNoTo", i(q.get("toDocNo")));
        nzI(p, "ItemId", i(q.get("itemId"))); nzI(p, "SupplierCustomerId", i(q.get("supplierCustomerId")));
        nzI(p, "SaleOrderFrom", i(q.get("saleOrderFrom"))); nzI(p, "SaleOrderTo", i(q.get("saleOrderTo")));
        nzD(p, "RateFrom", dbl(q.get("rateFrom"))); nzD(p, "RateTo", dbl(q.get("rateTo")));
        nzI(p, "WarehouseId", i(q.get("warehouseId"))); nzI(p, "JobLotId", i(q.get("jobLotId")));
        return bk.steel().table("[dbo].[USP_InvSaleInvoice_RegisterSalt]", p).rows;
    }

    private static final String[] INT_COLS = {"Id", "DocumentTypeId", "VoucherHeadId", "SuppCustId", "DueDays", "ItemId", "SaleGLAC"};

    /** gridHisory: the 41 column DataTable. */
    public List<Map<String, Object>> history(Map<String, String> q) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : raw(q)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", i(ci(r, "Id"))); o.put("DocumentTypeId", i(ci(r, "DocumentTypeId"))); o.put("VoucherHeadId", i(ci(r, "VoucherHeadId")));
            o.put("InvoiceNo", s(ci(r, "InvoiceNo"))); o.put("InvoiceDate", ci(r, "InvoiceDate")); o.put("SuppCustId", i(ci(r, "SupplierCustomerId")));
            o.put("CustomerName", s(ci(r, "CustomerName"))); o.put("PaymentTerm", s(ci(r, "PaymentTerm"))); o.put("DueDays", i(ci(r, "DueDays")));
            o.put("DueDate", ci(r, "DueDate")); o.put("ManualBillNo", s(ci(r, "ManualBillNo"))); o.put("CommissionAgent", s(ci(r, "CompanyNamecmagnt")));
            o.put("OrderNo", s(ci(r, "OrderNo"))); o.put("OrderDate", ci(r, "OrderDate")); o.put("WareHouse", s(ci(r, "WareHouseName")));
            o.put("ItemId", i(ci(r, "ItemId"))); o.put("SaleGLAC", i(ci(r, "SaleGLAC"))); o.put("ItemName", s(ci(r, "ItemName")));
            o.put("JobLot", s(ci(r, "JobLotDescription"))); o.put("PackingType", s(ci(r, "PackTypeDesc"))); o.put("PackUom", s(ci(r, "PackUom")));
            for (String k : new String[]{"ItemQty", "GrossWeight", "AdLsWeight", "StockWeight", "NetBillWeight", "ItemRate"}) o.put(k, dbl(ci(r, k)));
            o.put("RateUOM", s(ci(r, "RateUOM")));
            for (String k : new String[]{"RateCut", "RateCutAmount", "NetRate", "ItemAmount", "FreightAmount", "ExpenseAmount", "JournalAmount", "CommissionAmount"}) o.put(k, dbl(ci(r, k)));
            o.put("GpNo", s(ci(r, "GpNo"))); o.put("GpDate", ci(r, "GpDate")); o.put("VehicleNo", s(ci(r, "VehicleNo")));
            o.put("ItemNetAmount", dbl(ci(r, "ItemNetAmount"))); o.put("PartyBillAmount", dbl(ci(r, "PartyBillAmount")));
            out.add(o);
        }
        return out;
    }

    /** SalesFromEvaulation_RegisterSalt with the Show's filters: dtSummary rows as they come. */
    public List<Map<String, Object>> summaryRaw(Map<String, String> q) {
        UserAccount u = bk.user();
        Map<String, Object> p = SaleBkSupport.p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        Timestamp from = stamp(q.get("fromDate"), q.get("fromTime")), to = stamp(q.get("toDate"), q.get("toTime"));
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        nzI(p, "DocNoFrom", i(q.get("fromDocNo"))); nzI(p, "DocNoTo", i(q.get("toDocNo")));
        nzI(p, "InventoryParentCategories", i(q.get("parentCategoryId"))); nzI(p, "ItemCategoryId", i(q.get("itemCategoryId")));
        nzI(p, "ItemTypeId", i(q.get("itemTypeId"))); nzI(p, "ItemClassGroupId", i(q.get("itemClassGroupId")));
        nzI(p, "ItemId", i(q.get("itemId"))); nzI(p, "JobLotId", i(q.get("jobLotId")));
        nzI(p, "SupplierCustomerId", i(q.get("supplierCustomerId"))); nzI(p, "WarehouseId", i(q.get("warehouseId")));
        nzI(p, "PackUom", i(q.get("packUom"))); nzI(p, "CityId", i(q.get("cityId"))); nzI(p, "DistrictId", i(q.get("districtId")));
        String act = s(q.get("activity"));
        if (!act.isEmpty()) p.put("ActivityName", act);
        return bk.steel().table("[dbo].[USP_SalesFromEvaulation_RegisterSalt]", p).rows;
    }

    private static final String[][] TAIL = {{"ItemQty", "QtyOut"}, {"BillWeight", "BillWeightOut"}, {"Amount", "AmountOut"}, {"AvgRate", "AvgRate"},
            {"PrctByAmount", "PrcntOfTotal"}, {"PrctByWeight", "PrcntOfTotalWeight"}};

    /** The DataTable column list (target, source) of each activity, in the order of the desktop's Rows.Add. */
    private static List<String[]> layout(String activity) {
        List<String[]> l = new ArrayList<>();
        switch (activity) {
            case "Sales Register":
                for (String[] x : new String[][]{{"DocDate", "DocDate"}, {"DocNo", "DocCodeNo"}, {"Id", "RefDocIdNo"}, {"DocumentTypeId", "RefDocumentTypeId"}, {"DocType", "DocumentTypeCode"},
                        {"PartyName", "Customer"}, {"ItemCode", "ItemCode"}, {"ItemName", "ItemName"}, {"PackUom", "UOMCode"}, {"JobLot", "JobLotCode"}, {"PackingType", "PackTypeCode"}, {"VehicleNo", "VehicleNo"}}) l.add(x);
                tail(l); l.add(new String[]{"CityName", "CityName"}); return l;
            case "Sales Summary By Item": case "Sales Summary By Item & City":
                l.add(new String[]{"ItemCode", "ItemCode"}); l.add(new String[]{"ItemName", "ItemName"}); tail(l); l.add(new String[]{"CityName", "CityName"}); return l;
            case "Sales Summary By Item & Pack Size": case "Sales Summary By Item,Pack Size & City":
                l.add(new String[]{"ItemCode", "ItemCode"}); l.add(new String[]{"ItemName", "ItemName"}); l.add(new String[]{"PackUom", "UOMCode"}); tail(l); l.add(new String[]{"CityName", "CityName"}); return l;
            case "Sales Summary By Item & Warehouse":
                l.add(new String[]{"WarehouseName", "WarehouseName"}); l.add(new String[]{"ItemCode", "ItemCode"}); l.add(new String[]{"ItemName", "ItemName"}); tail(l); l.add(new String[]{"CityName", "CityName"}); return l;
            case "Sales Summary By Customer":
                l.add(new String[]{"PartyName", "Customer"}); tail(l); l.add(new String[]{"CityName", "CityName"}); return l;
            case "Sales Summary By Customer & Item": case "Sales Summary By Customer,Item & City":
                l.add(new String[]{"PartyName", "Customer"}); l.add(new String[]{"ItemCode", "ItemCode"}); l.add(new String[]{"ItemName", "ItemName"}); l.add(new String[]{"PackUom", "UOMCode"});
                tail(l); l.add(new String[]{"CityName", "CityName"}); return l;
            case "Sales Summary By Customer & City":
                l.add(new String[]{"PartyName", "Customer"}); l.add(new String[]{"PackUom", "UOMCode"}); tail(l); l.add(new String[]{"CityName", "CityName"}); return l;
            case "Sales Summary By Customer & Pack Size":
                l.add(new String[]{"PartyName", "Customer"}); l.add(new String[]{"PackUom", "UOMCode"}); tail(l); return l;
            case "Sales Summary By Parent Category":
                l.add(new String[]{"ParentCategory", "ParentCategory"}); tail(l); return l;
            case "Sales Summary By Parent Category & Item":
                l.add(new String[]{"ItemName", "ItemName"}); l.add(new String[]{"ParentCategory", "ParentCategory"}); tail(l); return l;
            case "Sales Summary By Parent Category & Customer":
                l.add(new String[]{"CustomerName", "Customer"}); l.add(new String[]{"ParentCategory", "ParentCategory"}); tail(l); return l;
            default:
                return null;
        }
    }

    private static void tail(List<String[]> l) { for (String[] t : TAIL) l.add(t); }

    /** btnShowSumamry_Click -> SummaryGridFill: {activity, columns:[...], rows:[...]}; no rows -> ClearStructure (empty rows); an unknown activity changes nothing (columns null). */
    public Map<String, Object> summary(Map<String, String> q) {
        String activity = s(q.get("activity"));
        List<Map<String, Object>> raw = summaryRaw(q);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("activity", activity);
        List<String[]> lay = layout(activity);
        List<Map<String, Object>> rows = new ArrayList<>();
        if (raw.isEmpty() || lay == null) {
            out.put("columns", lay == null ? null : new ArrayList<>());
            out.put("rows", rows);
            out.put("hasData", !raw.isEmpty());
            return out;
        }
        List<String> cols = new ArrayList<>();
        for (String[] c : lay) cols.add(c[0]);
        for (Map<String, Object> r : raw) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String[] c : lay) o.put(c[0], ci(r, c[1]));
            rows.add(o);
        }
        out.put("columns", cols);
        out.put("rows", rows);
        out.put("hasData", true);
        return out;
    }
}
