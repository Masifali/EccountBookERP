package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Stock DashBoard.
 *
 * Ported from Architecture.WinApp.Dashboard\frmStockDashboard.cs (2,938 lines). Its Load handler
 * is still called frmPendingWorksRpt_Load - the form was copied from that one and never renamed.
 *
 * Report contract, from BLL.Reports.Inventory.StocksReport.stockReportWithValues
 * (0125_Architecture.BLL.Reports.Inventory.StocksReport.cs):
 *
 *   Sp_ItemStockReportWithValues_Rpt
 *       @OrganizationId, @CompanyId, @Activity          always  (@Activity is sent EMPTY here,
 *                                                                because the form never sets it)
 *       @DateFrom / @DateTo                             only when set
 *       @SaleValue                                      only when the "Sale Value" radio is on
 *       @BranchesIds                                    only when non-empty
 *
 *   The form sets stockUOM = 0 and PackingTypeId = 0 explicitly (:430-431), and the BLL drops a
 *   zero, so @IsPackSizeOn and @PackingTypeId are NOT sent. Every other parameter the procedure
 *   accepts - @ItemTypeId, @ItemCategoryId, @CropYear, @JobLotId, @WarehouseId, @ItemId,
 *   @ItemStockAc, @AccountGroupId, @IsPackTypeOn, @ItemClassGroupId,
 *   @InventoryParentCategoriesId, @InventoryParentCategoriesIds, @SkipZero, @AllowWipItem,
 *   @StockPartyId - is omitted, which is not the same as sending NULL.
 *
 * The screen is six totals panels, and the desktop builds them ON THE CLIENT by walking every
 * returned row and adding it into whichever bucket its ItemClassGroupId and
 * InventoryParentCategoriesId pair matches (:536-625). The pairs, in the desktop's own order:
 *
 *   Raw Material Rice    ItemClassGroupId 1, ParentCategory 2
 *   Raw Material Paddy   ItemClassGroupId 1, ParentCategory 1
 *   By Product           ItemClassGroupId 2, ParentCategory 4
 *   Finish Goods         ItemClassGroupId 3, ParentCategory 2
 *   Packing Material     ItemClassGroupId 4, ParentCategory 7
 *   Store                ItemClassGroupId 5, ParentCategory 8
 *
 * A row matching none of the six is counted nowhere, exactly as the desktop's if/else chain
 * leaves it out. That same grouping is done here so the page receives the six panels rather than
 * every stock row.
 *
 * Read-only: no save, update or delete, and no History button on the desktop form.
 */
@Service
public class StockDashboardService {
    private static final org.slf4j.Logger LOG = org.slf4j.LoggerFactory.getLogger(StockDashboardService.class);


    /** CommonServices.GetERPFeatureById(11) - multi-branch (:385). */
    private static final int FEATURE_BRANCH = 11;

    /** The six panels: key, caption, ItemClassGroupId, InventoryParentCategoriesId. */
    private static final Object[][] PANELS = {
        { "rawRice",  "Raw Material Rice",  1, 2 },
        { "rawPaddy", "Raw Material Paddy", 1, 1 },
        { "byProduct", "By Product",        2, 4 },
        { "finishGoods", "Finish Goods",    3, 2 },
        { "packMaterial", "Packing Material", 4, 7 },
        { "store",    "Store",              5, 8 }
    };

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    // ------------------------------------------------------------------ setup

    /** frmPendingWorksRpt_Load, :379-393. */
    public Map<String, Object> setup() {
        Map<String, Object> out = new LinkedHashMap<>();
        boolean branchFeature = featureOn(FEATURE_BRANCH);
        out.put("branchFeature", branchFeature);

        /* :382 - From Date is the active financial year's start; To Date keeps the designer's
           today. */
        out.put("fromDate", financialYearStart());
        out.put("toDate", LocalDate.now().toString());

        List<Map<String, Object>> branches = new ArrayList<>();
        try {
            /* BranchesFill, :395-420 - BranchesAllocationToUser.GetBranchsAllocatedToUser. */
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC [dbo].[USP_GetBranchsAllocatedToUser] @OrganizationId=?, @CompanyId=?, @UserId=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    currentUserContext.currentUserId());
            for (Map<String, Object> r : rows) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("id", str(col(r, "BranchId")));
                m.put("name", str(col(r, "BranchName")));
                branches.add(m);
            }
        } catch (Exception e) {
            LOG.error("Branch list failed", e);
            out.put("branchError", e.getMessage());
        }
        out.put("branches", branches);
        return out;
    }

    // ----------------------------------------------------------------- report

    /** btnsearch_Click -> GridBind(), :422-708. */
    public Map<String, Object> report(String fromDate, String toDate,
                                      boolean saleValue, String branchIds) {
        Map<String, Object> out = new LinkedHashMap<>();
        boolean branchFeature = featureOn(FEATURE_BRANCH);

        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
        names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
        Object from = date(fromDate);
        if (from != null) { names.add("@DateFrom"); args.add(from); }
        Object to = date(toDate);
        if (to != null)   { names.add("@DateTo");   args.add(to); }
        /* :435-441 - ActionId is 1 for Sale Value and 0 otherwise, and the BLL drops a zero. */
        if (saleValue) { names.add("@SaleValue"); args.add(1); }
        /* :443-457 - the branch list is sent only when the branch feature is on. */
        String branches = branchFeature && branchIds != null ? branchIds.trim() : "";
        if (!branches.isEmpty()) { names.add("@BranchesIds"); args.add(branches); }
        /* Always sent, and always empty - the form never sets Activity. */
        names.add("@Activity"); args.add("");

        List<Map<String, Object>> rows;
        try {
            rows = jdbcTemplate.queryForList(
                    exec("Sp_ItemStockReportWithValues_Rpt", names), args.toArray());
        } catch (Exception e) {
            LOG.error("Sp_ItemStockReportWithValues_Rpt failed", e);
            out.put("error", e.getMessage());
            out.put("panels", emptyPanels());
            return out;
        }

        /* :459-625 - the totals are added up here, bucket by bucket, exactly as the desktop's
           if/else chain does. A row matching no pair is counted nowhere. */
        Map<String, double[]> sums = new LinkedHashMap<>();
        for (Object[] p : PANELS) sums.put((String) p[0], new double[12]);

        for (Map<String, Object> r : rows) {
            int classGroup = asInt(col(r, "ItemClassGroupId"));
            int parentCat  = asInt(col(r, "InventoryParentCategoriesId"));
            String key = null;
            for (Object[] p : PANELS) {
                if (classGroup == (Integer) p[2] && parentCat == (Integer) p[3]) {
                    key = (String) p[0];
                    break;
                }
            }
            if (key == null) continue;
            double[] t = sums.get(key);
            t[0]  += dbl(col(r, "Op_Qty"));
            t[1]  += dbl(col(r, "QtyIn"));
            t[2]  += dbl(col(r, "QtyOut"));
            t[3]  += dbl(col(r, "BalQty"));
            t[4]  += dbl(col(r, "Op_Weight"));
            t[5]  += dbl(col(r, "WeightIn"));
            t[6]  += dbl(col(r, "WeightOut"));
            t[7]  += dbl(col(r, "BalWeight"));
            t[8]  += dbl(col(r, "Op_Amount"));
            t[9]  += dbl(col(r, "AmountIn"));
            t[10] += dbl(col(r, "AmountOut"));
            t[11] += dbl(col(r, "BalAmount"));
        }

        /* :627-707 - with no rows at all the desktop calls Reset(), which puts every figure back
           to zero. An all-zero panel set is the same thing. */
        List<Map<String, Object>> panels = new ArrayList<>();
        for (Object[] p : PANELS) {
            double[] t = sums.get((String) p[0]);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("key", p[0]);
            m.put("title", p[1]);
            m.put("classGroupId", p[2]);
            m.put("parentCategoryId", p[3]);
            m.put("opQty", t[0]);   m.put("inQty", t[1]);   m.put("outQty", t[2]);   m.put("balQty", t[3]);
            m.put("opWeight", t[4]); m.put("inWeight", t[5]); m.put("outWeight", t[6]); m.put("balWeight", t[7]);
            m.put("opAmount", t[8]); m.put("inAmount", t[9]); m.put("outAmount", t[10]); m.put("balAmount", t[11]);
            panels.add(m);
        }
        out.put("panels", panels);
        out.put("rowCount", rows.size());
        return out;
    }

    private static List<Map<String, Object>> emptyPanels() {
        List<Map<String, Object>> panels = new ArrayList<>();
        for (Object[] p : PANELS) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("key", p[0]);
            m.put("title", p[1]);
            m.put("classGroupId", p[2]);
            m.put("parentCategoryId", p[3]);
            for (String k : new String[] { "opQty", "inQty", "outQty", "balQty",
                                           "opWeight", "inWeight", "outWeight", "balWeight",
                                           "opAmount", "inAmount", "outAmount", "balAmount" }) {
                m.put(k, 0.0);
            }
            panels.add(m);
        }
        return panels;
    }

    // ---------------------------------------------------------------- helpers

    private String financialYearStart() {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "SELECT TOP 1 Start_Period FROM FinancialYear WHERE Id = ?",
                    currentUserContext.currentFinancialYearId());
            if (!rows.isEmpty()) {
                String s = str(col(rows.get(0), "Start_Period"));
                if (s.length() >= 10) return s.substring(0, 10);
            }
        } catch (Exception e) {
            LOG.error("Financial year start read failed", e);
        }
        return LocalDate.now().withDayOfMonth(1).toString();
    }

    private boolean featureOn(int featureId) {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "SELECT TOP 1 IsActive FROM ERPFeatures WHERE Id = ?", featureId);
            if (!rows.isEmpty()) {
                Object v = rows.get(0).values().iterator().next();
                if (v instanceof Boolean) return (Boolean) v;
                if (v instanceof Number) return ((Number) v).intValue() != 0;
                String s = String.valueOf(v).trim();
                return "1".equals(s) || "true".equalsIgnoreCase(s);
            }
        } catch (Exception e) {
            LOG.error("ERP feature lookup failed for {}", featureId, e);
        }
        return false;
    }

    private static String exec(String proc, List<String> names) {
        StringBuilder b = new StringBuilder("EXEC ").append(proc).append(' ');
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) b.append(", ");
            b.append(names.get(i)).append("=?");
        }
        return b.toString();
    }

    private static Object col(Map<String, Object> row, String name) {
        if (row == null) return null;
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

    private static double dbl(Object o) {
        if (o == null) return 0.0;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0.0; }
    }

    /** Plain yyyy-MM-dd only - nothing shifts a day at UTC+5. */
    private static Object date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, 10))); }
        catch (Exception e) { LOG.warn("lookup failed", e); return null; }
    }
}
