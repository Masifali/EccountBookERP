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
 * Purchase Analytics Dashboard.
 *
 * Ported from Architecture.WinApp.AnalyticDashboard\PurchaseAnalytiicsDashBoard.cs (2,850 lines).
 * Its Load handler and its window caption are both still "frmAnalyticsDashboard" (:209, :2811) -
 * the class was renamed and those were not. That is why the DashBoard TargetUrl for this screen
 * reads frmAnalyticsDashboard while the file is called PurchaseAnalytiicsDashBoard.
 *
 * ONE form, FOUR DashBoard entries. The desktop's Form.Tag decides which (:230, :375-384):
 *   Tag "frmPaddyPurchaseAnalytics"  keeps parent category 1 only, and shows the
 *                                    "Calculate Short Weight" tick box
 *   Tag "frmRicePurchaseAnalytics"   keeps parent categories 2 and 4, and hides that tick box
 * Anything else keeps NO parent category at all, because the loop at :375-384 adds a row only
 * when one of those two Tags matches. Reproduced exactly: an unknown tag yields an empty Parent
 * Category list, and the screen then refuses to read, as the desktop does.
 *
 * Report contract, from BLL.Dashboard.PurchaseAnalyticsDashBoardReport
 * (0065_Architecture.BLL.Dashboard.cs):
 *
 *   [dbo].[USP-PurchaseAnalyticsDashBoard_Report]
 *       @OrganizationId, @CompanyId, @ReportType             always
 *       @FinancialYearId                                     only when non-zero
 *       @FromDate / @ToDate                                  only when set
 *       @ItemCategoryId, @ItemTypeId, @JobLotId,
 *       @SupplierCustomerId, @ItemId,
 *       @InventoryParentCategoriesId                         only when non-zero
 *       @CropYear, @BranchesIds                              only when non-empty
 *
 *   @ReportType is "Item Wise Sales Summary" for the item grid and "Customer Wise Sales Summary"
 *   for the party grid (:610, :672) - the desktop reads the SAME procedure twice, and the names
 *   say "Sales" on a purchase screen. Left as they are.
 *
 * Break-up contract, from BLL.Dashboard.PurchaseComaprisonsByCustomerItemReport:
 *
 *   [dbo].[USP-PurchaseComaprisonsByCustomerItem_Report]
 *       @OrganizationId, @CompanyId, @ReportType             always
 *       @FromDate / @ToDate                                  only when set
 *       @InventoryParentCategoriesId, @ItemCategoryId, @ItemTypeId,
 *       @SupplierCustomerId, @ItemId, @JobLotId              only when non-zero
 *       @CropYear, @BranchesIds                              only when non-empty
 *
 *   @ReportType is "Item And Customer Wise Comparison" (Party Wise break-up, :557) or
 *   "Customer And Item Wise Comparison" (Item Wise break-up, :562). NOTE that the break-up call
 *   never sends @FinancialYearId and never sends @SupplierCustomerId/@ItemId from the filter -
 *   only the id of the row that was clicked (:558, :563).
 *
 * Avg-rate pop-ups:
 *   Sp_InvPurchaseInvoice_AvgRatesComparisonsByItem_Rpt          @Activity='AvgRateByItem',
 *                                                                @GroupByDate always sent
 *   Sp_InvPurchaseInvoice_AvgRatesComparisonsByItemSupplier_Rpt  @Activity='AvgRateBySupplier'
 *
 * Read-only: no save, update or delete, and no History button on the desktop form.
 */
@Service
public class PurchaseAnalyticsDashboardService {

    private static final Logger LOG = LoggerFactory.getLogger(PurchaseAnalyticsDashboardService.class);

    /** CommonServices.GetERPFeatureById(11) - multi-branch (:234). */
    private static final int FEATURE_BRANCH = 11;

    public static final String TAG_PADDY = "frmPaddyPurchaseAnalytics";
    public static final String TAG_RICE  = "frmRicePurchaseAnalytics";

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    // ------------------------------------------------------------------ setup

    /** frmAnalyticsDashboard_Load, :209-252. */
    public Map<String, Object> setup(String tag) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("tag", tag);
        /* :226-233 - the Short Weight tick box exists only on the Paddy screen. */
        out.put("showShortWeight", TAG_PADDY.equals(tag));

        boolean branchFeature = featureOn(FEATURE_BRANCH);
        out.put("branchFeature", branchFeature);

        out.put("previousDays", 7);                       // :223

        /* :237-240 - From Date is the active financial year's start; To Date keeps today. */
        Map<String, Object> year = activeFinancialYear();
        out.put("fromDate", str(year.get("start")).isEmpty()
                ? LocalDate.now().withDayOfMonth(1).toString() : str(year.get("start")));
        out.put("toDate", LocalDate.now().toString());
        out.put("financialYearId", year.get("id"));

        out.put("parentCategories", readOrError(out, "parentCategoryError",
                () -> parentCategories(tag)));
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
     * ParentCategoryFill, :349-399.
     * InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales ->
     *   USP_GetDataFromInventoryStocksEvaluationsForSales
     *     @OrganizationId, @CompanyId, @UserId, @AppId, @Activity='GetParentCategory',
     *     @DocType='Purchase'
     * Then the form keeps only the ids its Tag allows (:375-384).
     */
    private List<Map<String, Object>> parentCategories(String tag) {
        List<Map<String, Object>> rows = evaluationLookup("GetParentCategory", null);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            int id = asInt(col(r, "Id"));
            boolean keep;
            if (TAG_RICE.equals(tag))       keep = (id == 2 || id == 4);
            else if (TAG_PADDY.equals(tag)) keep = (id == 1);
            else                            keep = false;   // the desktop adds nothing otherwise
            if (!keep) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", String.valueOf(id));
            m.put("name", str(col(r, "RefName")));
            out.add(m);
        }
        return out;
    }

    /**
     * AllComboBind, :402-466 - ONE call returns every remaining dropdown in a single result set,
     * split by its Activity column. @ParentIds carries the chosen parent category, and @Activity
     * is sent EMPTY here (the form never sets it on this call), which is not the same as leaving
     * it out - the BLL always adds it.
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
        out.put("itemTypes",         byActivity(rows, "GetItemType"));
        out.put("itemCategories",    byActivity(rows, "GetItemCategory"));
        out.put("parties",           byActivity(rows, "GetSupplierCustomer"));
        out.put("items",             byActivity(rows, "GetItems"));
        out.put("cropYears",         byActivity(rows, "GetCropYear"));
        out.put("jobLots",           byActivity(rows, "GetJobLot"));
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
        names.add("@DocType");  args.add("Purchase");
        if (parentIds != null && !parentIds.trim().isEmpty() && !"0".equals(parentIds.trim())) {
            names.add("@ParentIds"); args.add(parentIds.trim());
        }
        return jdbcTemplate.queryForList(
                exec("USP_GetDataFromInventoryStocksEvaluationsForSales", names), args.toArray());
    }

    /** BranchesFill, :255-280 - USP_GetBranchesFromVouchersByAccountId, a checked list. */
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

    /** rdSeason_Click -> GetSeasonScheduleDates, :320-347. */
    public Map<String, Object> seasonDates() {
        Map<String, Object> out = new LinkedHashMap<>();
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC USP_SeasonYearSchedule_GetAll @OrganizationId=?, @CompanyId=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId());
            if (rows.isEmpty()) {
                /* :342 - the desktop's own wording. */
                out.put("warning", "No Schedule found. Please Make a season year schedule first!");
            } else {
                out.put("fromDate", dateOnly(col(rows.get(0), "SeasonStartDate")));
                out.put("toDate",   dateOnly(col(rows.get(0), "SeasonEndDate")));
            }
        } catch (Exception e) {
            LOG.error("Season schedule read failed", e);
            out.put("error", e.getMessage());
        }
        return out;
    }

    // ----------------------------------------------------------------- report

    public static class Filters {
        public String fromDate, toDate, cropYear = "", branchIds = "";
        public int parentCategoryId, itemCategoryId, itemTypeId, jobLotId, partyId, itemId;
    }

    /** GetComparisonDataAgainstSummary, :575-727 - the same procedure, twice. */
    public Map<String, Object> report(Filters f) {
        Map<String, Object> out = new LinkedHashMap<>();
        try {
            out.put("itemWise", summary(f, "Item Wise Sales Summary"));
        } catch (Exception e) {
            LOG.error("Item Wise summary failed", e);
            out.put("itemWiseError", e.getMessage());
            out.put("itemWise", new ArrayList<>());
        }
        /* :519-522 - the desktop reads the party grid ONLY when the item grid came back with
           rows, and otherwise clears the party grid and the three totals. */
        List<?> itemRows = (List<?>) out.get("itemWise");
        if (itemRows != null && !itemRows.isEmpty()) {
            try {
                out.put("customerWise", summary(f, "Customer Wise Sales Summary"));
            } catch (Exception e) {
                LOG.error("Customer Wise summary failed", e);
                out.put("customerWiseError", e.getMessage());
                out.put("customerWise", new ArrayList<>());
            }
        } else {
            out.put("customerWise", new ArrayList<>());
            out.put("noRecord", "No Record Found!");       // :532
        }
        return out;
    }

    private List<Map<String, Object>> summary(Filters f, String reportType) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
        names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
        int fy = 0;
        try { fy = currentUserContext.currentFinancialYearId(); } catch (Exception ignored) { }
        if (fy != 0) { names.add("@FinancialYearId"); args.add(fy); }
        addDate(names, args, "@FromDate", f.fromDate);
        addDate(names, args, "@ToDate",   f.toDate);
        if (f.itemCategoryId != 0)   { names.add("@ItemCategoryId");     args.add(f.itemCategoryId); }
        if (f.itemTypeId != 0)       { names.add("@ItemTypeId");         args.add(f.itemTypeId); }
        if (f.jobLotId != 0)         { names.add("@JobLotId");           args.add(f.jobLotId); }
        if (f.partyId != 0)          { names.add("@SupplierCustomerId"); args.add(f.partyId); }
        if (f.itemId != 0)           { names.add("@ItemId");             args.add(f.itemId); }
        if (notBlank(f.cropYear))    { names.add("@CropYear");           args.add(f.cropYear.trim()); }
        if (f.parentCategoryId != 0) { names.add("@InventoryParentCategoriesId"); args.add(f.parentCategoryId); }
        if (notBlank(f.branchIds))   { names.add("@BranchesIds");        args.add(f.branchIds.trim()); }
        names.add("@ReportType"); args.add(reportType);

        List<Map<String, Object>> raw = jdbcTemplate.queryForList(
                exec("[dbo].[USP-PurchaseAnalyticsDashBoard_Report]", names), args.toArray());

        /* :612-646 and :688-712 - the desktop rebuilds both tables, renaming CropBatch to
           CropYear and "Empty Shell / Trash" to "EmptyShell/Trash". The party table stops at
           Amount; the item table carries the lab-analysis columns as well. */
        boolean itemWise = "Item Wise Sales Summary".equals(reportType);
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
            m.put("Expenses",          col(r, "Expenses"));
            m.put("Exp40Kg",           col(r, "Exp40Kg"));
            m.put("AvgRateExp",        col(r, "AvgRateExp"));
            m.put("Amount",            col(r, "Amount"));
            if (itemWise) {
                m.put("AnalysisQty",        col(r, "AnalysisQty"));
                m.put("Moisture",           col(r, "Moisture"));
                /* the procedure spells it with spaces; the grid column has none */
                m.put("EmptyShell/Trash",   col(r, "Empty Shell / Trash"));
                m.put("Dust/Stone",         col(r, "Dust/Stone"));
                m.put("Broken",             col(r, "Broken"));
                m.put("AGL",                col(r, "AGL"));
                m.put("Damage",             col(r, "Damage"));
                m.put("ShortWeight",        col(r, "ShortWeight"));
                m.put("NetWeight",          col(r, "NetWeight"));
                m.put("NetRate",            col(r, "NetRate"));
            }
            rows.add(m);
        }
        return rows;
    }

    /**
     * GetBreakupDataAgainstActivityBreakUpName, :540-573.
     *
     * reportType is "Item And Customer Wise Comparison" (the Party Wise break-up of one item) or
     * "Customer And Item Wise Comparison" (the Item Wise break-up of one party). Only the id of
     * the clicked row is sent - the filter's own Party and Item are NOT.
     */
    public Map<String, Object> breakup(Filters f, String reportType, int groupId, String rowBranchId) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
        names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
        addDate(names, args, "@FromDate", f.fromDate);
        addDate(names, args, "@ToDate",   f.toDate);
        if (f.parentCategoryId != 0) { names.add("@InventoryParentCategoriesId"); args.add(f.parentCategoryId); }
        if (f.itemCategoryId != 0)   { names.add("@ItemCategoryId"); args.add(f.itemCategoryId); }
        if (f.itemTypeId != 0)       { names.add("@ItemTypeId");     args.add(f.itemTypeId); }
        if ("Customer And Item Wise Comparison".equals(reportType)) {
            if (groupId != 0) { names.add("@SupplierCustomerId"); args.add(groupId); }
        } else {
            if (groupId != 0) { names.add("@ItemId"); args.add(groupId); }
        }
        if (f.jobLotId != 0)      { names.add("@JobLotId"); args.add(f.jobLotId); }
        if (notBlank(f.cropYear)) { names.add("@CropYear"); args.add(f.cropYear.trim()); }
        /* :557 - the break-up sends the CLICKED ROW's branch, not the filter's branch list, and
           sends nothing at all when the branch feature is off. */
        String branch = notBlank(f.branchIds) && notBlank(rowBranchId) && !"0".equals(rowBranchId.trim())
                ? rowBranchId.trim() : "";
        if (notBlank(branch)) { names.add("@BranchesIds"); args.add(branch); }
        names.add("@ReportType"); args.add(reportType);

        try {
            out.put("rows", jdbcTemplate.queryForList(
                    exec("[dbo].[USP-PurchaseComaprisonsByCustomerItem_Report]", names), args.toArray()));
        } catch (Exception e) {
            LOG.error("Break-up report failed", e);
            out.put("error", e.getMessage());
            out.put("rows", new ArrayList<>());
        }
        return out;
    }

    /**
     * grdItemWise_ColumnButtonClick "Date Wise", :888-910 ->
     *   InvPurchaseInvoiceReports.InvPurchaseInvoiceAvgRateByItem221 ->
     *     Sp_InvPurchaseInvoice_AvgRatesComparisonsByItem_Rpt
     *       @GroupByDate is ALWAYS sent, and this caller sets it to 1 (:896).
     */
    public Map<String, Object> avgRateByItem(Filters f, int itemId, String rowBranchId) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
        names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
        addDate(names, args, "@FromDate", f.fromDate);
        addDate(names, args, "@ToDate",   f.toDate);
        if (itemId != 0)             { names.add("@ItemId"); args.add(itemId); }
        if (notBlank(f.cropYear))    { names.add("@CropYear"); args.add(f.cropYear.trim()); }
        if (f.parentCategoryId != 0) { names.add("@InventoryParentCategoriesId"); args.add(f.parentCategoryId); }
        if (f.itemCategoryId != 0)   { names.add("@ItemCategoryId"); args.add(f.itemCategoryId); }
        if (f.itemTypeId != 0)       { names.add("@ItemTypeId");     args.add(f.itemTypeId); }
        if (f.jobLotId != 0)         { names.add("@JobLotId");       args.add(f.jobLotId); }
        String branch = notBlank(f.branchIds) && notBlank(rowBranchId) && !"0".equals(rowBranchId.trim())
                ? rowBranchId.trim() : "";
        if (notBlank(branch)) { names.add("@BranchesIds"); args.add(branch); }
        names.add("@GroupByDate"); args.add(1);
        names.add("@Activity");    args.add("AvgRateByItem");
        return runRows("Sp_InvPurchaseInvoice_AvgRatesComparisonsByItem_Rpt", names, args);
    }

    /**
     * grdItemWise_LinkClicked on GroupName, :930-957, and grdCustomerWise_LinkClicked, :1065-1098
     * -> InvPurchaseInvoiceAvgRateBySupplier222 ->
     *      Sp_InvPurchaseInvoice_AvgRatesComparisonsByItemSupplier_Rpt @Activity='AvgRateBySupplier'
     *
     * From the item grid the clicked row supplies the ITEM and the filter supplies the party;
     * from the party grid the clicked row supplies the PARTY and the filter supplies the item
     * (:1084-1086). Both cases are covered by passing both ids in.
     */
    public Map<String, Object> avgRateBySupplier(Filters f, int itemId, int partyId, String rowBranchId) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
        names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
        addDate(names, args, "@FromDate", f.fromDate);
        addDate(names, args, "@ToDate",   f.toDate);
        if (partyId != 0)            { names.add("@SupplierCustomerId"); args.add(partyId); }
        if (itemId != 0)             { names.add("@ItemId"); args.add(itemId); }
        if (notBlank(f.cropYear))    { names.add("@CropYear"); args.add(f.cropYear.trim()); }
        if (f.parentCategoryId != 0) { names.add("@InventoryParentCategoriesId"); args.add(f.parentCategoryId); }
        if (f.itemCategoryId != 0)   { names.add("@ItemCategoryId"); args.add(f.itemCategoryId); }
        if (f.itemTypeId != 0)       { names.add("@ItemTypeId");     args.add(f.itemTypeId); }
        if (f.jobLotId != 0)         { names.add("@JobLotId");       args.add(f.jobLotId); }
        String branch = notBlank(f.branchIds) && notBlank(rowBranchId) && !"0".equals(rowBranchId.trim())
                ? rowBranchId.trim() : "";
        if (notBlank(branch)) { names.add("@BranchesIds"); args.add(branch); }
        names.add("@Activity"); args.add("AvgRateBySupplier");
        return runRows("Sp_InvPurchaseInvoice_AvgRatesComparisonsByItemSupplier_Rpt", names, args);
    }

    private Map<String, Object> runRows(String proc, List<String> names, List<Object> args) {
        Map<String, Object> out = new LinkedHashMap<>();
        try {
            out.put("rows", jdbcTemplate.queryForList(exec(proc, names), args.toArray()));
        } catch (Exception e) {
            LOG.error("{} failed", proc, e);
            out.put("error", e.getMessage());
            out.put("rows", new ArrayList<>());
        }
        return out;
    }

    // ---------------------------------------------------------------- helpers

    private Map<String, Object> activeFinancialYear() {
        Map<String, Object> out = new LinkedHashMap<>();
        try {
            int id = currentUserContext.currentFinancialYearId();
            out.put("id", id);
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "SELECT TOP 1 Start_Period, End_Period FROM FinancialYear WHERE Id = ?", id);
            if (!rows.isEmpty()) {
                out.put("start", dateOnly(col(rows.get(0), "Start_Period")));
                out.put("end",   dateOnly(col(rows.get(0), "End_Period")));
            }
        } catch (Exception e) {
            LOG.error("Active financial year read failed", e);
        }
        return out;
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

    private static int asInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    /** Keeps only yyyy-MM-dd, so nothing shifts a day at UTC+5. */
    private static String dateOnly(Object o) {
        String s = str(o);
        return s.length() >= 10 ? s.substring(0, 10) : "";
    }

    private static Object date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, 10))); }
        catch (Exception e) { return null; }
    }
}
