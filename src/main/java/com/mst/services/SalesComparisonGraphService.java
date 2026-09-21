package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Sales Comparison (Graph) Report.
 *
 * Ported from Architecture.WinApp.Graph\SalesComparisonReportWithGraph.cs (1,582 lines).
 *
 * Procedure contract, from
 * BLL.Reports.Inventory.InvSaleInvoiceReports.SaleComparisionsCustomerItemCityandPack_Wise:
 *
 *   USP_SaleComparisionsCustomerItemCityandPack_Wise
 *       @OrganizationId, @CompanyId, @FromDate, @ToDate, @TopRow, @UserId, @AppId   always
 *       @ParentCategoryId   only when non-zero
 *       @ItemTypeId         only when non-zero
 *       @CostCenterId       only when non-zero
 *       @OrderByFlag        only when non-empty - "Top" or "Bottom" from the two radios
 *
 * It returns ONE flat table which the form splits four ways on its DescriptionTitle column
 * (:316, :370, :424, :478) - note the fourth value has no space:
 *
 *   "Customer"  -> Top N Customers    grid column captioned CustomerName
 *   "Item"      -> Top N Items                              ItemName
 *   "City"      -> Top N Cities                              CityName
 *   "PackingSize" -> Top N Pack Size                         PackUom
 *
 * Every group carries GroupTitle, SaleQty, SaleWeight, SaleAmount and %OfTopBottom - which the
 * grids re-caption as "%OfTop" (:342). All four measures are summed (AggregateFunction 2).
 *
 * Combos:
 *   Parent Category / Item Type  USP_GetDataFromInventoryStocksEvaluationsForSales
 *       @OrganizationId, @CompanyId, @UserId, @AppId, @Activity, @DocType   always
 *       - the form sets only ReportType = "Sale", which the BLL binds to @DocType, leaving
 *         @Activity NULL. Both are always sent, so NULL is what @Activity gets - that is the
 *         contract, not an omission.
 *       One table split on its own Activity column into "GetParentCategory" and "GetItemType",
 *       taking Id and RefName (:260-272).
 *   Cost Center  usp_getCostCenters @OrganizationId, @CompanyId, @UserId [, @AppId]
 *       [, @ParentCostCenterId]  - the form passes ParentCostCenterId 0, so it is omitted.
 *
 * AppId 5 is a special case on the desktop: the cost centre is forced to the first row and
 * locked (:181-186), and ShowReport refuses with "Cost Center Not Found" when it is still 0
 * (:303-306). Both are reproduced.
 *
 * Read-only: no save, update or delete on the desktop form.
 */
@Service
public class SalesComparisonGraphService {

    private static final Logger LOG = LoggerFactory.getLogger(SalesComparisonGraphService.class);

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    // ---------------------------------------------------------------- setup

    /** Load(), :157-200 - the dates, the default Top rows of 10, and the three lists. */
    public Map<String, Object> setup() {
        Map<String, Object> out = new LinkedHashMap<>();
        int appId = 0;
        try { appId = currentUserContext.currentAppId(); } catch (Exception ignored) { }
        out.put("appId", appId);
        /* :181 - for AppId 5 the combo is pinned to its first row and disabled. */
        out.put("lockCostCenter", appId == 5);
        out.put("topRows", 10);                       // :180

        /* :163-166 - From is the financial year's start, To is today. */
        out.put("fromDate", financialYearStart());
        out.put("toDate", LocalDate.now().toString());

        List<Map<String, Object>> categories = new ArrayList<>();
        List<Map<String, Object>> itemTypes = new ArrayList<>();
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    "EXEC USP_GetDataFromInventoryStocksEvaluationsForSales @OrganizationId=?, "
                  + "@CompanyId=?, @UserId=?, @AppId=?, @Activity=?, @DocType=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    currentUserContext.currentUserId(),
                    appId,
                    null,                              // the form leaves Activity unset
                    "Sale")) {
                String activity = str(col(r, "Activity"));
                Map<String, Object> item = new LinkedHashMap<>();
                item.put("id", asInt(col(r, "Id")));
                item.put("name", str(col(r, "RefName")));
                if ("GetParentCategory".equals(activity))  categories.add(item);
                else if ("GetItemType".equals(activity))   itemTypes.add(item);
            }
        } catch (Exception e) {
            LOG.error("Parent category / item type lists failed", e);
            out.put("listError", e.getMessage());
        }

        List<Map<String, Object>> costCenters = new ArrayList<>();
        try {
            List<String> names = new ArrayList<>();
            List<Object> args = new ArrayList<>();
            names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
            names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
            names.add("@UserId");         args.add(currentUserContext.currentUserId());
            if (appId != 0) { names.add("@AppId"); args.add(appId); }
            /* ParentCostCenterId is passed as 0 by the form, so the BLL omits it. */
            StringBuilder sql = new StringBuilder("EXEC usp_getCostCenters ");
            for (int i = 0; i < names.size(); i++) {
                if (i > 0) sql.append(", ");
                sql.append(names.get(i)).append("=?");
            }
            for (Map<String, Object> r : jdbcTemplate.queryForList(sql.toString(), args.toArray())) {
                Map<String, Object> item = new LinkedHashMap<>();
                item.put("id", asInt(col(r, "Id")));
                Object name = col(r, "CostCenterName");
                if (name == null) name = col(r, "Description");
                if (name == null) name = col(r, "Name");
                item.put("name", str(name));
                costCenters.add(item);
            }
        } catch (Exception e) {
            LOG.error("Cost centre list failed", e);
            out.put("costCenterError", e.getMessage());
        }

        out.put("categories", categories);
        out.put("itemTypes", itemTypes);
        out.put("costCenters", costCenters);
        return out;
    }

    // ---------------------------------------------------------------- the report

    /** ShowReport(), :280-540. */
    public Map<String, Object> report(String fromDate, String toDate, int topRows,
                                      String orderByFlag, int categoryId, int itemTypeId,
                                      int costCenterId) {
        Map<String, Object> out = new LinkedHashMap<>();
        int appId = 0;
        try { appId = currentUserContext.currentAppId(); } catch (Exception ignored) { }

        /* :303-306 - the desktop's own message, and it does not run the report. */
        if (appId == 5 && costCenterId == 0) {
            out.put("error", "Cost Center Not Found");
            return out;
        }

        try {
            List<String> names = new ArrayList<>();
            List<Object> args = new ArrayList<>();
            names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
            names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
            names.add("@FromDate");       args.add(date(fromDate));
            names.add("@ToDate");         args.add(date(toDate));
            names.add("@TopRow");         args.add(topRows);
            names.add("@UserId");         args.add(currentUserContext.currentUserId());
            names.add("@AppId");          args.add(appId);
            if (categoryId != 0)   { names.add("@ParentCategoryId"); args.add(categoryId); }
            if (itemTypeId != 0)   { names.add("@ItemTypeId");       args.add(itemTypeId); }
            if (costCenterId != 0) { names.add("@CostCenterId");     args.add(costCenterId); }
            if (orderByFlag != null && !orderByFlag.trim().isEmpty()) {
                names.add("@OrderByFlag"); args.add(orderByFlag.trim());
            }

            StringBuilder sql = new StringBuilder(
                    "EXEC USP_SaleComparisionsCustomerItemCityandPack_Wise ");
            for (int i = 0; i < names.size(); i++) {
                if (i > 0) sql.append(", ");
                sql.append(names.get(i)).append("=?");
            }

            List<Map<String, Object>> customers = new ArrayList<>();
            List<Map<String, Object>> items = new ArrayList<>();
            List<Map<String, Object>> cities = new ArrayList<>();
            List<Map<String, Object>> packSizes = new ArrayList<>();

            for (Map<String, Object> r : jdbcTemplate.queryForList(sql.toString(), args.toArray())) {
                Map<String, Object> row = new LinkedHashMap<>();
                row.put("name", str(col(r, "GroupTitle")));
                row.put("saleQty", num(col(r, "SaleQty")));
                row.put("saleWeight", num(col(r, "SaleWeight")));
                row.put("saleAmount", num(col(r, "SaleAmount")));
                /* The column is %OfTopBottom; the grid captions it %OfTop (:342). */
                row.put("pctOfTop", num(col(r, "%OfTopBottom")));

                String title = str(col(r, "DescriptionTitle"));
                if ("Customer".equals(title))         customers.add(row);
                else if ("Item".equals(title))        items.add(row);
                else if ("City".equals(title))        cities.add(row);
                else if ("PackingSize".equals(title)) packSizes.add(row);
            }

            out.put("customers", customers);
            out.put("items", items);
            out.put("cities", cities);
            out.put("packSizes", packSizes);
        } catch (Exception e) {
            LOG.error("Sales comparison report failed", e);
            out.put("customers", Collections.emptyList());
            out.put("items", Collections.emptyList());
            out.put("cities", Collections.emptyList());
            out.put("packSizes", Collections.emptyList());
            out.put("error", e.getMessage());
        }
        return out;
    }

    private String financialYearStart() {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "SELECT Start_Period FROM FinancialYear WHERE Id = ?",
                    currentUserContext.currentFinancialYearId());
            if (!rows.isEmpty()) return ymd(col(rows.get(0), "Start_Period"));
        } catch (Exception e) {
            LOG.error("Financial year start read failed", e);
        }
        return LocalDate.now().toString();
    }

    // ---------------------------------------------------------------- helpers

    private static Object col(Map<String, Object> row, String name) {
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    private static int asInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    private static double num(Object o) {
        if (o == null) return 0d;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0d; }
    }

    private static Object date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, 10))); }
        catch (Exception e) { return null; }
    }

    private static String ymd(Object o) {
        if (o == null) return "";
        String s = String.valueOf(o);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }
}
