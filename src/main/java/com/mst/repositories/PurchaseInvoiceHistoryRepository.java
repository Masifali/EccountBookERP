package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/**
 * 476 Inventory_Reports.frmPurchaseInvoiceHistory "Purchase Report" - the desktop BLL calls, parameter for
 * parameter (BLL: InvPurchaseInvoice / InvPurchaseInvoiceReports). The BLL source is only available as
 * architecture.bll.dll; the procedure names below are the string literals found in it next to each method name.
 *
 *   GetDataForPurchaseInvoiceRegisterDropdownBind  Sp_InvPurchaseInvoice_GetAllMethod, Activity 'GetDataForPurchaseInvoiceRegisterDropdownBind'
 *   InvPurchaseInvoiceHistoryRpt                   Sp_InvPurchaseInvoiceHistoryNew_rpt
 *   InvPurchaseInvoiceAvgRateByItem221             Sp_InvPurchaseInvoice_AvgRatesComparisonsByItem_Rpt
 *   InvPurchaseInvoiceAvgRateBySupplier222         Sp_InvPurchaseInvoice_AvgRatesComparisonsByItemSupplier_Rpt
 *   RicePaddyPuchaseReport                         SpInventoryTransactions_PurchasesingRicePaddy_Rpt
 *   InvPurchaseInvoiceDetailRegister (297/224)     Sp_InvPurchaseInvoice_Rpt
 * Parameters the BLL guards with "!= 0" / "!= empty" are omitted when empty (DesktopProc drops nulls).
 */
@Repository
public class PurchaseInvoiceHistoryRepository {
    private final JdbcTemplate jdbc;

    public PurchaseInvoiceHistoryRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private static Map<String, Object> tenant(UserAccount u) {
        return DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    public List<Map<String, Object>> dropdown(UserAccount u, String branchIds) {
        var p = tenant(u);
        p.put("Activity", "GetDataForPurchaseInvoiceRegisterDropdownBind");
        text(p, "BranchesIds", branchIds);
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoice_GetAllMethod", p);
    }

    /** CommonServices.CustomeGroupsDefine(2) -> Sp_AcLookUps_GetAllMethod 'ReadAll', AcLookUpTypesId 2. */
    public List<Map<String, Object>> customGroups(UserAccount u) {
        var p = tenant(u);
        p.put("AcLookUpTypesId", 2);
        p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, "Sp_AcLookUps_GetAllMethod", p);
    }

    public List<Map<String, Object>> history(UserAccount u, Map<String, Object> f) {
        var p = tenant(u);
        date(p, "FromDate", f.get("fromDate"));
        date(p, "ToDate", f.get("toDate"));
        number(p, "FromDocNo", f.get("fromNo"));
        number(p, "ToDocNo", f.get("toNo"));
        number(p, "SupplierCustomerId", f.get("supplierId"));
        number(p, "ItemId", f.get("itemId"));
        number(p, "CommissionAgentId", f.get("commissionAgentId"));
        number(p, "JobLotId", f.get("jobLotId"));
        text(p, "CropYear", f.get("cropYear"));
        number(p, "WarehouseId", f.get("warehouseId"));
        number(p, "ItemCategoryId", f.get("itemCategoryId"));
        number(p, "ItemTypeId", f.get("itemTypeId"));
        number(p, "InventoryParentCategories", f.get("parentCategoryId"));
        date(p, "DueDateFrom", f.get("dueFrom"));
        date(p, "DueDateTo", f.get("dueTo"));
        number(p, "RateFrom", f.get("rateFrom"));
        number(p, "RateTo", f.get("rateTo"));
        text(p, "BranchesIds", f.get("branchesIds"));
        text(p, "CustomGroupIds", f.get("customGroupIds"));
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoiceHistoryNew_rpt", p);
    }

    public List<Map<String, Object>> avgRateByItem(UserAccount u, Map<String, Object> f) {
        var p = avgCommon(u, f);
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoice_AvgRatesComparisonsByItem_Rpt", p);
    }

    public List<Map<String, Object>> avgRateBySupplier(UserAccount u, Map<String, Object> f) {
        var p = avgCommon(u, f);
        number(p, "SupplierCustomerId", f.get("supplierId"));
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoice_AvgRatesComparisonsByItemSupplier_Rpt", p);
    }

    private Map<String, Object> avgCommon(UserAccount u, Map<String, Object> f) {
        var p = tenant(u);
        date(p, "FromDate", f.get("fromDate"));
        date(p, "ToDate", f.get("toDate"));
        number(p, "ItemId", f.get("itemId"));
        text(p, "CropYear", f.get("cropYear"));
        number(p, "InventoryParentCategoriesId", f.get("parentCategoryId"));
        number(p, "ItemCategoryId", f.get("itemCategoryId"));
        number(p, "ItemTypeId", f.get("itemTypeId"));
        number(p, "JobLotId", f.get("jobLotId"));
        text(p, "BranchesIds", f.get("branchesIds"));
        return p;
    }

    /** RicePaddyPuchaseReport: Activity 1 = GroupItem&CropYear, 2 = GroupByItem, none for GroupBySupplier. */
    public List<Map<String, Object>> ricePaddy(UserAccount u, Map<String, Object> f) {
        var p = tenant(u);
        date(p, "FromDate", f.get("fromDate"));
        date(p, "Todate", f.get("toDate"));
        number(p, "ItemId", f.get("itemId"));
        text(p, "CropYear", f.get("cropYear"));
        number(p, "InventoryParentCategoriesId", f.get("parentCategoryId"));
        number(p, "Activity", f.get("activityId"));
        text(p, "BranchesIds", f.get("branchesIds"));
        return DesktopProc.rows(jdbc, "SpInventoryTransactions_PurchasesingRicePaddy_Rpt", p);
    }

    /** InvPurchaseInvoiceReports.InvPurchaseInvoiceDetailRegister (PurchaseInvoiceRegisterDetail helper). */
    public List<Map<String, Object>> detailRegister(UserAccount u, Map<String, Object> f) {
        var p = tenant(u);
        date(p, "DocDateFrom", f.get("fromDate"));
        date(p, "DocDateTo", f.get("toDate"));
        number(p, "DocNoFrom", f.get("fromNo"));
        number(p, "DocNoTo", f.get("toNo"));
        number(p, "SupplierCustomerId", f.get("supplierId"));
        number(p, "ItemId", f.get("itemId"));
        number(p, "CommissionAgentId", f.get("commissionAgentId"));
        number(p, "JobLotId", f.get("jobLotId"));
        text(p, "CropYear", f.get("cropYear"));
        number(p, "WarehouseId", f.get("warehouseId"));
        number(p, "ItemCategoryId", f.get("itemCategoryId"));
        number(p, "ItemTypeId", f.get("itemTypeId"));
        number(p, "InventoryParentCategories", f.get("parentCategoryId"));
        number(p, "RateFrom", f.get("rateFrom"));
        number(p, "RateTo", f.get("rateTo"));
        number(p, "CustomeGroupId", f.get("customGroupId"));
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoice_Rpt", p);
    }

    private static void number(Map<String, Object> p, String key, Object value) {
        double n = value instanceof Number x ? x.doubleValue() : parse(value);
        if (n != 0) p.put(key, n == Math.rint(n) ? (Object) (long) n : (Object) n);
    }

    private static double parse(Object v) {
        if (v == null) return 0;
        try { return Double.parseDouble(v.toString().trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static void text(Map<String, Object> p, String key, Object value) {
        if (value == null) return;
        String v = value.toString();
        if (!v.isEmpty()) p.put(key, v);
    }

    private static void date(Map<String, Object> p, String key, Object value) {
        if (value instanceof LocalDateTime d) p.put(key, Timestamp.valueOf(d));
    }
}
