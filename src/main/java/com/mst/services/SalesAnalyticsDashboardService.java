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
 * Sales Analytics Dashboard.
 *
 * Ported from Architecture.WinApp.AnalyticDashboard\SalesAnalyticsDashBoard.cs (2,375 lines).
 *
 * NOT the same screen as Architecture.WinApp.Dashboard\SalesAnalytics.cs (523 lines), which is the
 * organisation-level sale comparison already served at /dashboard/sales-analytics. These are two
 * different forms with two different procedures; this one is the sibling of
 * PurchaseAnalytiicsDashBoard.
 *
 * Both AnalyticDashboard forms carry the window caption "frmAnalyticsDashboard" (:2343 here,
 * :2811 in the purchase form) and a Load handler of that name. No type called
 * frmAnalyticsDashboard exists - confirmed against the shipped ECCOUNTBOOKERP.exe, where the only
 * ASCII occurrence is the method name frmAnalyticsDashboard_Load while SalesAnalyticsDashBoard and
 * PurchaseAnalytiicsDashBoard are both present as real type names. The DashBoard row's TargetUrl
 * is the type to instantiate and its ScreenName is what the desktop puts in Form.Tag
 * (DashboardNew.cs:1571-1572), so "frmAnalyticsDashboard" is a ScreenName, never a TargetUrl.
 *
 * Summary contract, from
 * BLL.Reports.Inventory.InvSaleInvoiceReports.InventoryStockEvaluation_LocalSalesComaprisons_Report:
 *
 *   spInventoryStockEvaluation_LocalSalesComaprisons_Report
 *       @OrganizationId, @CompanyId, @ReportType              always
 *       @FromDate / @ToDate                                   only when set
 *       @ItemCategoryId, @ItemTypeId, @JobLotId,
 *       @SupplierCustomerId, @ItemId,
 *       @InventoryParentCategoriesId                          only when non-zero
 *       @CropYear, @Ids, @BranchesIds                         only when non-empty
 *
 *   @ReportType is "Item Wise Sales Summary" or "Customer Wise Sales Summary" (:477, :519).
 *
 * Break-up contract, same parameters, different procedure:
 *
 *   spInventoryStockEvaluation_LocalSalesComaprisonsByCustomerItem_Report
 *       @ReportType "Item And Customer Wise Comparison" or "Customer And Item Wise Comparison"
 *
 * DIFFERENT FROM THE PURCHASE FORM, and reproduced as-is:
 *   - the break-up sets are read EAGERLY during GetComparisonData (:378-379) and the button click
 *     only FILTERS what is already in memory, on FirstGroupName == GroupName plus BranchesId when
 *     a branch is chosen (:699-717). The purchase form instead re-queries per clicked row.
 *   - the break-up call does NOT narrow by the clicked row's id, and it does not send
 *     @SupplierCustomerId or @ItemId at all (:417-424).
 *   - parent categories are NOT filtered by Form.Tag - this form has no Paddy/Rice split and no
 *     Short Weight tick box.
 *   - @DocType is "Sale" here, "Purchase" there.
 *
 * Read-only: no save, update or delete, and no History button on the desktop form.
 */
@Service
public class SalesAnalyticsDashboardService {

    private static final Logger LOG = LoggerFactory.getLogger(SalesAnalyticsDashboardService.class);

    /** CommonServices.GetERPFeatureById(11) - multi-branch (:168). */
    private static final int FEATURE_BRANCH = 11;

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    // ------------------------------------------------------------------ setup

    /** frmAnalyticsDashboard_Load, :160-192. */
    public Map<String, Object> setup() {
        Map<String, Object> out = new LinkedHashMap<>();
        boolean branchFeature = featureOn(FEATURE_BRANCH);
        out.put("branchFeature", branchFeature);

        /* :174 - From Date is the active financial year's start; To Date keeps today. */
        out.put("fromDate", financialYearStart());
        out.put("toDate", LocalDate.now().toString());

        out.put("parentCategories", readOrError(out, "parentCategoryError", this::parentCategories));
        out.put("branches", branchFeature
                ? readOrError(out, "branchError", this::branches)
                : new ArrayList<Map<String, Object>>());
        return out;
    }

    private interface Reader { List<Map<String, Object>> read(); }

    private List<Map<String, Object>> readOrError(Map<String, Object> out, String key, Reader r) {
        try {
            return r.read();
        } catch (Exception e) {
            LOG.error("Read failed for {}", key, e);
            out.put(key, e.getMessage());
            return new ArrayList<>();
        }
    }

    /**
     * ParameterCategoryFill, :222-254.
     * USP_GetDataFromInventoryStocksEvaluationsForSales
     *   @OrganizationId, @CompanyId, @UserId, @AppId, @Activity='GetParentCategory', @DocType='Sale'
     *
     * Unlike the purchase form there is NO Tag filter - every returned row is bound (:245).
     */
    private List<Map<String, Object>> parentCategories() {
        List<Map<String, Object>> rows = evaluationLookup("GetParentCategory", null);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", str(col(r, "Id")));
            m.put("name", str(col(r, "RefName")));
            out.add(m);
        }
        return out;
    }

    /**
     * AllComboBind, :256-320 - one call returns every remaining dropdown in a single result set,
     * split by its Activity column. @Activity is sent EMPTY on this call (the form never sets it),
     * which is not the same as leaving it out - the BLL always adds it.
     */
    public Map<String, Object> combos(String parentCategoryId) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rows;
        try {
            rows = evaluationLookup("", parentCategoryId);
        } catch (Exception e) {
            LOG.error("AllComboBind failed", e);
            out.put("error", e.getMessage());
            rows = new ArrayList<>();
        }
        out.put("itemTypes",      byActivity(rows, "GetItemType"));
        out.put("itemCategories", byActivity(rows, "GetItemCategory"));
        out.put("parties",        byActivity(rows, "GetSupplierCustomer"));
        out.put("items",          byActivity(rows, "GetItems"));
        out.put("jobLots",        byActivity(rows, "GetJobLot"));
        out.put("cropYears",      byActivity(rows, "GetCropYear"));
        return out;
    }

    private static List<Map<String, Object>> byActivity(List<Map<String, Object>> rows, String activity) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (!activity.equals(str(col(r, "Activity")))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", str(col(r, "Id")));
            m.put("name", str(col(r, "RefName")));
            out.add(m);
        }
        return out;
    }

    private List<Map<String, Object>> evaluationLookup(String activity, String parentIds) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
        names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
        names.add("@UserId");         args.add(currentUserContext.currentUserId());
        int appId = 0;
        try { appId = currentUserContext.currentAppId(); } catch (Exception ignored) { }
        names.add("@AppId");    args.add(appId);
        names.add("@Activity"); args.add(activity == null ? "" : activity);
        names.add("@DocType");  args.add("Sale");
        if (parentIds != null && !parentIds.trim().isEmpty() && !"0".equals(parentIds.trim())) {
            names.add("@ParentIds"); args.add(parentIds.trim());
        }
        return jdbcTemplate.queryForList(
                exec("USP_GetDataFromInventoryStocksEvaluationsForSales", names), args.toArray());
    }

    /** BranchesFill, :195-220 - USP_GetBranchesFromVouchersByAccountId, a checked list. */
    private List<Map<String, Object>> branches() {
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC USP_GetBranchesFromVouchersByAccountId @OrganizationId=?, @CompanyId=?",
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId());
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", str(col(r, "Id")));
            m.put("name", str(col(r, "BranchName")));
            out.add(m);
        }
        return out;
    }

    // ----------------------------------------------------------------- report

    public static class Filters {
        public String fromDate, toDate, cropYear = "", branchIds = "";
        public int parentCategoryId, itemCategoryId, itemTypeId, jobLotId, partyId, itemId;
    }

    /** GetComparisonData, :371-393 - both summaries AND both break-up sets, in one go. */
    public Map<String, Object> report(Filters f) {
        Map<String, Object> out = new LinkedHashMap<>();

        List<Map<String, Object>> itemWise;
        try {
            itemWise = summary(f, "Item Wise Sales Summary", true);
        } catch (Exception e) {
            LOG.error("Item Wise summary failed", e);
            out.put("itemWiseError", e.getMessage());
            itemWise = new ArrayList<>();
        }
        out.put("itemWise", itemWise);

        /* :375-381 - the party grid and BOTH break-up sets are read only when the item grid came
           back with rows; otherwise the totals are zeroed and the party grid cleared. */
        if (!itemWise.isEmpty()) {
            try {
                out.put("customerWise", summary(f, "Customer Wise Sales Summary", false));
            } catch (Exception e) {
                LOG.error("Customer Wise summary failed", e);
                out.put("customerWiseError", e.getMessage());
                out.put("customerWise", new ArrayList<>());
            }
            /* The break-ups are fetched EAGERLY here, not on the button click (:378-379). */
            out.put("breakupItemWise",
                    breakupOrEmpty(out, "breakupItemWiseError", f, "Item And Customer Wise Comparison"));
            out.put("breakupCustomerWise",
                    breakupOrEmpty(out, "breakupCustomerWiseError", f, "Customer And Item Wise Comparison"));
        } else {
            out.put("customerWise", new ArrayList<>());
            out.put("breakupItemWise", new ArrayList<>());
            out.put("breakupCustomerWise", new ArrayList<>());
            out.put("noRecord", "No Record Found!");       // :386
        }
        return out;
    }

    private List<Map<String, Object>> breakupOrEmpty(Map<String, Object> out, String key,
                                                     Filters f, String reportType) {
        try {
            return runReport("spInventoryStockEvaluation_LocalSalesComaprisonsByCustomerItem_Report",
                             f, reportType);
        } catch (Exception e) {
            LOG.error("Break-up read failed for {}", reportType, e);
            out.put(key, e.getMessage());
            return new ArrayList<>();
        }
    }

    /** GetComparisonDataAgainstSummary, :443-563. */
    private List<Map<String, Object>> summary(Filters f, String reportType, boolean itemWise) {
        List<Map<String, Object>> raw = runReport(
                "spInventoryStockEvaluation_LocalSalesComaprisons_Report", f, reportType);

        /* :481-508 and :527-549 - the desktop rebuilds both tables, renaming CropBatch to CropYear.
           The item table additionally carries the sales account; the party table stops at
           AvgRateExp. */
        List<Map<String, Object>> rows = new ArrayList<>(raw.size());
        for (Map<String, Object> r : raw) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("BranchesId",        col(r, "BranchesId"));
            m.put("BranchName",        col(r, "BranchName"));
            m.put("GroupCaption",      col(r, "GroupCaption"));
            m.put("GroupId",           col(r, "GroupId"));
            m.put("GroupName",         col(r, "GroupName"));
            m.put("CropYear",          col(r, "CropBatch"));          // CropBatch -> CropYear
            m.put("ParentCategory",    col(r, "ParentCategory"));
            m.put("TotalAmount",       col(r, "TotalAmount"));
            m.put("PrctOfTotalAmount", col(r, "PrctOfTotalAmount"));
            m.put("Qty",               col(r, "Qty"));
            m.put("WeightKg",          col(r, "WeightKg"));
            m.put("AvgRate",           col(r, "AvgRate"));
            m.put("ItemAmount",        col(r, "ItemAmount"));
            m.put("AddExpenses",       col(r, "AddExpenses"));
            m.put("LessExpenses",      col(r, "LessExpenses"));
            m.put("Amount",            col(r, "Amount"));
            m.put("AvgRateExp",        col(r, "AvgRateExp"));
            if (itemWise) {
                m.put("SalesAccountId", col(r, "SalesAccountId"));
                m.put("SalesAccount",   col(r, "SalesAccount"));
            }
            rows.add(m);
        }
        return rows;
    }

    /** Both procedures take exactly the same parameter set; only @ReportType differs. */
    private List<Map<String, Object>> runReport(String proc, Filters f, String reportType) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
        names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
        addDate(names, args, "@FromDate", f.fromDate);
        addDate(names, args, "@ToDate",   f.toDate);
        if (f.itemCategoryId != 0)   { names.add("@ItemCategoryId");     args.add(f.itemCategoryId); }
        if (f.itemTypeId != 0)       { names.add("@ItemTypeId");         args.add(f.itemTypeId); }
        if (f.jobLotId != 0)         { names.add("@JobLotId");           args.add(f.jobLotId); }
        /* The break-up call on the desktop never sets Party or Item (:417-424); this method is
           shared, and the caller passes a Filters with those left at 0 for a break-up. */
        if (f.partyId != 0)          { names.add("@SupplierCustomerId"); args.add(f.partyId); }
        if (f.itemId != 0)           { names.add("@ItemId");             args.add(f.itemId); }
        if (notBlank(f.cropYear))    { names.add("@CropYear");           args.add(f.cropYear.trim()); }
        if (f.parentCategoryId != 0) { names.add("@InventoryParentCategoriesId"); args.add(f.parentCategoryId); }
        if (notBlank(f.branchIds))   { names.add("@BranchesIds");        args.add(f.branchIds.trim()); }
        names.add("@ReportType"); args.add(reportType);
        return jdbcTemplate.queryForList(exec(proc, names), args.toArray());
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

    private static void addDate(List<String> names, List<Object> args, String name, String value) {
        Object v = date(value);
        if (v != null) { names.add(name); args.add(v); }
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

    private static boolean notBlank(String s) { return s != null && !s.trim().isEmpty(); }

    /** Plain yyyy-MM-dd only - nothing shifts a day at UTC+5. */
    private static Object date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, 10))); }
        catch (Exception e) { return null; }
    }
}
