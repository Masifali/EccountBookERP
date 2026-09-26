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
 * Purchase &amp; Sales Pending Works (Dashboard) - the whole of the
 * "Followups &amp;&amp; Pending Works DashBoards" module.
 *
 * Ported from Architecture.WinApp.Dashboard\frmPendingWorksRpt.cs (753 lines) and its drill-down
 * frmPendingWorksDetail.cs.
 *
 * Procedure contract, from BLL.Dashboard.PendingWorkDashboard (:927-1011):
 *
 *   Sp_Dashboards_PendingWork
 *       @OrganizationId, @CompanyId                 always
 *       @FromDate, @ToDate                          only when set
 *       @FinancaialYearId                           only when non-zero  (misspelled at source)
 *       @ReportType                                 only when non-empty
 *       @PendingWorkDepartment                      only when non-empty
 *       @PendingWorkCaption                         only when non-empty
 *       @BranchesIds                                only when non-empty
 *       @TypeId                                     only when non-zero
 *
 * The cards pass @ReportType = 'FiguresOnly' (:170); the drill-down passes
 * @ReportType = 'DetailByPendingWorkCaption' (frmPendingWorksDetail :135) together with the
 * clicked card's department and caption. @TypeId is 1 for the "Paddy &amp; Rice" radio and 2 for
 * "Packing Material &amp; Others" (:171).
 *
 * Branches come from [dbo].[USP_GetBranchsAllocatedToUser] @OrganizationId, @CompanyId, @UserId
 * (BLL.BranchesAllocationToUser :85-111), and are only applied when ERP feature 11 (multi-branch)
 * is on - AllCardsBind :176 sets BranchesIds to "" when it is off.
 *
 * Read-only: the desktop form has no save, update or delete.
 */
@Service
public class PendingWorksDashboardService {

    private static final Logger LOG = LoggerFactory.getLogger(PendingWorksDashboardService.class);

    /** CommonServices.GetERPFeatureById(11) - the multi-branch feature (form :99). */
    private static final int FEATURE_MULTI_BRANCH = 11;

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    // ---------------------------------------------------------------- setup

    /**
     * BranchesFill(), form :111-136, plus the multi-branch feature flag the form reads at :99.
     * The combo is a checked list, so more than one branch can be chosen and their ids are sent
     * as a comma-separated string.
     */
    public Map<String, Object> setup() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("multiBranch", false);
        out.put("branches", Collections.emptyList());
        out.put("toDate", LocalDate.now().toString());
        out.put("fromDate", LocalDate.now().toString());

        try {
            out.put("multiBranch", featureOn(FEATURE_MULTI_BRANCH));
        } catch (Exception e) {
            LOG.error("ERP feature {} read failed", FEATURE_MULTI_BRANCH, e);
        }
        try {
            out.put("branches", jdbcTemplate.queryForList(
                    "EXEC [dbo].[USP_GetBranchsAllocatedToUser] @OrganizationId=?, @CompanyId=?, @UserId=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    currentUserContext.currentUserId()));
        } catch (Exception e) {
            LOG.error("Branches allocated to user read failed", e);
            out.put("branchError", e.getMessage());
        }
        return out;
    }

    /** CommonServices.GetERPFeatureById - the same ERPFeatures row the desktop reads. */
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

    // ---------------------------------------------------------------- the cards

    /**
     * AllCardsBind(), form :139-282.
     *
     * One row per card. The form then splits them into five panels:
     *
     *   Purchase Department, InSeparatePanel = 0   PurchaseDashboardFlowLayout
     *   Purchase Department, InSeparatePanel = 1   PurchaseSeparateFlowLayout  (DarkCyan heading)
     *   Sale Department,     InSeparatePanel = 0   SaleDashboardFlowLayout
     *   Sale Department,     InSeparatePanel = 1   SaleSeparateFlowLayout      (DarkCyan heading)
     *   Export Department    (no split)            ExportCardsFlowLayoutPanel
     *
     * The split is done here so the page renders the same five groups. A NULL InSeparatePanel
     * counts as 0, which is what GetValueOrDefault() does at :186.
     */
    public Map<String, Object> cards(String fromDate, String toDate, int typeId, String branchIds) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> purchase = new ArrayList<>();
        List<Map<String, Object>> purchaseSeparate = new ArrayList<>();
        List<Map<String, Object>> sale = new ArrayList<>();
        List<Map<String, Object>> saleSeparate = new ArrayList<>();
        List<Map<String, Object>> export = new ArrayList<>();
        try {
            for (Map<String, Object> r : run("FiguresOnly", fromDate, toDate, typeId, branchIds,
                                             null, null)) {
                Map<String, Object> c = new LinkedHashMap<>();
                c.put("department", str(col(r, "PendingWorkDepartment")));
                c.put("title", str(col(r, "PendingWorkCaption")));
                c.put("caption", str(col(r, "ActivityCaption")));
                c.put("count", asInt(col(r, "Count")));

                String dept = str(col(r, "PendingWorkDepartment"));
                int sep = asInt(col(r, "InSeparatePanel"));
                if ("Export Department".equals(dept))        export.add(c);
                else if ("Purchase Department".equals(dept)) (sep == 1 ? purchaseSeparate : purchase).add(c);
                else if ("Sale Department".equals(dept))     (sep == 1 ? saleSeparate : sale).add(c);
            }
        } catch (Exception e) {
            LOG.error("Pending work dashboard failed", e);
            out.put("error", e.getMessage());
        }
        out.put("purchase", purchase);
        out.put("purchaseSeparate", purchaseSeparate);
        out.put("sale", sale);
        out.put("saleSeparate", saleSeparate);
        out.put("export", export);
        return out;
    }

    /**
     * GetDetailPendingWorkForPurchase(), form :351-369, then frmPendingWorksDetail :135-141:
     * the same procedure with @ReportType = 'DetailByPendingWorkCaption' plus the clicked card's
     * department and caption.
     *
     * The columns differ per pending-work type, so whatever the procedure returns is passed
     * through rather than being forced into one fixed shape - which is what the desktop does
     * too, building a different DataTable per case.
     */
    public Map<String, Object> detail(String fromDate, String toDate, int typeId, String branchIds,
                                      String department, String pendingWorkName) {
        Map<String, Object> out = new LinkedHashMap<>();
        try {
            out.put("rows", run("DetailByPendingWorkCaption", fromDate, toDate, typeId, branchIds,
                                department, pendingWorkName));
        } catch (Exception e) {
            LOG.error("Pending work detail failed for {} / {}", department, pendingWorkName, e);
            out.put("rows", Collections.emptyList());
            out.put("error", e.getMessage());
        }
        return out;
    }

    /** One place that builds the parameter list, so the cards and the detail cannot drift apart. */
    private List<Map<String, Object>> run(String reportType, String fromDate, String toDate,
                                          int typeId, String branchIds,
                                          String department, String pendingWorkName) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
        names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());

        Object from = date(fromDate);
        if (from != null) { names.add("@FromDate"); args.add(from); }
        Object to = date(toDate);
        if (to != null)   { names.add("@ToDate");   args.add(to); }

        int fy = 0;
        try { fy = currentUserContext.currentFinancialYearId(); } catch (Exception ignored) { }
        /* Spelled @FinancaialYearId at the source - BLL :962. Corrected spelling would not bind. */
        if (fy != 0) { names.add("@FinancaialYearId"); args.add(fy); }

        if (nz(reportType))      { names.add("@ReportType");             args.add(reportType); }
        if (nz(department))      { names.add("@PendingWorkDepartment");  args.add(department); }
        if (nz(pendingWorkName)) { names.add("@PendingWorkCaption");     args.add(pendingWorkName); }
        if (nz(branchIds))       { names.add("@BranchesIds");            args.add(branchIds); }
        if (typeId != 0)         { names.add("@TypeId");                 args.add(typeId); }

        StringBuilder sql = new StringBuilder("EXEC Sp_Dashboards_PendingWork ");
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) sql.append(", ");
            sql.append(names.get(i)).append("=?");
        }
        return jdbcTemplate.queryForList(sql.toString(), args.toArray());
    }

    // ---------------------------------------------------------------- helpers

    private static boolean nz(String s) { return s != null && !s.trim().isEmpty(); }

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

    private static Object date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, 10))); }
        catch (Exception e) { return null; }
    }
}
