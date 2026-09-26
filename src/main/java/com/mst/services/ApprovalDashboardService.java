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
 * Approvals (Dashboard).
 *
 * Ported from Architecture.WinApp.ApprovalDashboard\ApprovalDashboard.cs (1,812 lines).
 *
 * Two procedures, and they do NOT take the same parameters - the accounts one has no branch
 * filter at all:
 *
 *   BLL.Dashboard.ReadAccountsDashboardPendingForApproval
 *       Sp_Voucher_ReadDashboard
 *           @OrganizationId, @CompanyId, @FinancialYearId, @UserId   always
 *           @FromDate / @ToDate   only when their tick box is set
 *           @AppId                only when non-zero
 *
 *   BLL.Dashboard.ReadInventoryDashboardPendingForApproval
 *       Sp_Inventory_ReadDashboardPendingForApproval
 *           @OrganizationId, @CompanyId, @UserId                     always
 *           @FinancialYearId, @BranchesId, @AppId                    only when non-zero
 *           @FromDate / @ToDate   only when their tick box is set
 *
 * The rows are split into four panels by ActionId (:197-224):
 *
 *   Accounts        accounts rows,  ActionId != 2            title = VoucherType
 *   Inventory       inventory rows, ActionId != 3 and != 5   title = Description
 *   Export          inventory ActionId == 3 (Description) PLUS accounts ActionId == 2
 *                   (VoucherType) - one panel fed from BOTH sources
 *   CommissionAgent inventory ActionId == 5                  title = Description
 *
 * The Export and Commission Agent group boxes are hidden entirely when their set is empty
 * (:216, :223).
 *
 * ERP feature 11 gates the branch picker, and when it is on the form refuses to build any cards
 * until a branch is chosen: "Please Select Branch First" (:176-180). With the feature off it
 * uses the signed-in user's own branch (:188).
 *
 * Read-only: this screen only opens other screens. The approval itself happens on the form a
 * card opens, and none of those are ported, so nothing here approves anything.
 */
@Service
public class ApprovalDashboardService {

    private static final Logger LOG = LoggerFactory.getLogger(ApprovalDashboardService.class);

    /** CommonServices.GetERPFeatureById(11) - multi-branch (:111). */
    private static final int FEATURE_MULTI_BRANCH = 11;

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    /** ApprovalDashboard_Load, :105-130 - the feature, the branches, From a week ago, To today. */
    public Map<String, Object> setup() {
        Map<String, Object> out = new LinkedHashMap<>();
        boolean branchFeature = featureOn(FEATURE_MULTI_BRANCH);
        out.put("branchFeature", branchFeature);
        out.put("fromDate", LocalDate.now().minusDays(7).toString());
        out.put("toDate", LocalDate.now().toString());
        int myBranch = 0;
        try { myBranch = currentUserContext.currentBranchId(); } catch (Exception ignored) { }
        out.put("currentBranchId", myBranch);

        List<Map<String, Object>> branches = new ArrayList<>();
        try {
            branches = jdbcTemplate.queryForList(
                    "EXEC [dbo].[USP_GetBranchsAllocatedToUser] @OrganizationId=?, @CompanyId=?, @UserId=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    currentUserContext.currentUserId());
        } catch (Exception e) {
            LOG.error("Branch list failed", e);
            out.put("branchError", e.getMessage());
        }
        out.put("branches", branches);
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

    /** DynamicallyGenerateCards(), :159-242. */
    public Map<String, Object> cards(String fromDate, String toDate, int branchId) {
        Map<String, Object> out = new LinkedHashMap<>();
        boolean branchFeature = featureOn(FEATURE_MULTI_BRANCH);

        /* :176-180 - the form's own refusal, before it reads anything. */
        if (branchFeature && branchId == 0) {
            out.put("error", "Please Select Branch First");
            return out;
        }
        int effectiveBranch = branchId;
        if (!branchFeature) {
            try { effectiveBranch = currentUserContext.currentBranchId(); } catch (Exception ignored) { }
        }

        List<Map<String, Object>> accountsRows = new ArrayList<>();
        List<Map<String, Object>> inventoryRows = new ArrayList<>();
        try {
            accountsRows = runAccounts(fromDate, toDate);
        } catch (Exception e) {
            LOG.error("Accounts approval dashboard failed", e);
            out.put("accountsError", e.getMessage());
        }
        try {
            inventoryRows = runInventory(fromDate, toDate, effectiveBranch);
        } catch (Exception e) {
            LOG.error("Inventory approval dashboard failed", e);
            out.put("inventoryError", e.getMessage());
        }

        List<Map<String, Object>> accounts = new ArrayList<>();
        List<Map<String, Object>> inventory = new ArrayList<>();
        List<Map<String, Object>> export = new ArrayList<>();
        List<Map<String, Object>> commission = new ArrayList<>();

        for (Map<String, Object> r : accountsRows) {
            int action = asInt(col(r, "ActionId"));
            /* :215 - accounts ActionId 2 belongs to the Export panel, not the Accounts one. */
            if (action == 2) export.add(card(r, "VoucherType"));
            else             accounts.add(card(r, "VoucherType"));
        }
        for (Map<String, Object> r : inventoryRows) {
            int action = asInt(col(r, "ActionId"));
            if (action == 3)      export.add(card(r, "Description"));
            else if (action == 5) commission.add(card(r, "Description"));
            else                  inventory.add(card(r, "Description"));
        }

        out.put("accounts", accounts);
        out.put("inventory", inventory);
        out.put("export", export);
        out.put("commission", commission);
        out.put("showExport", !export.isEmpty());          // :216
        out.put("showCommission", !commission.isEmpty());  // :223
        return out;
    }

    private Map<String, Object> card(Map<String, Object> r, String titleColumn) {
        Map<String, Object> c = new LinkedHashMap<>();
        c.put("id", asInt(col(r, "TypeID")));
        c.put("title", str(col(r, titleColumn)));
        c.put("count", asInt(col(r, "Count")));
        return c;
    }

    private List<Map<String, Object>> runAccounts(String fromDate, String toDate) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId");  args.add(currentUserContext.currentOrganizationId());
        names.add("@CompanyId");       args.add(currentUserContext.currentCompanyId());
        names.add("@FinancialYearId"); args.add(currentUserContext.currentFinancialYearId());
        names.add("@UserId");          args.add(currentUserContext.currentUserId());
        Object from = date(fromDate);
        if (from != null) { names.add("@FromDate"); args.add(from); }
        Object to = date(toDate);
        if (to != null)   { names.add("@ToDate");   args.add(to); }
        /* Sp_Voucher_ReadDashboard has NO branch parameter - only the inventory one does. */
        return jdbcTemplate.queryForList(exec("Sp_Voucher_ReadDashboard", names), args.toArray());
    }

    private List<Map<String, Object>> runInventory(String fromDate, String toDate, int branchId) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
        names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
        names.add("@UserId");         args.add(currentUserContext.currentUserId());
        int fy = 0;
        try { fy = currentUserContext.currentFinancialYearId(); } catch (Exception ignored) { }
        if (fy != 0)       { names.add("@FinancialYearId"); args.add(fy); }
        if (branchId != 0) { names.add("@BranchesId");      args.add(branchId); }
        Object from = date(fromDate);
        if (from != null) { names.add("@FromDate"); args.add(from); }
        Object to = date(toDate);
        if (to != null)   { names.add("@ToDate");   args.add(to); }
        return jdbcTemplate.queryForList(
                exec("Sp_Inventory_ReadDashboardPendingForApproval", names), args.toArray());
    }

    private static String exec(String proc, List<String> names) {
        StringBuilder b = new StringBuilder("EXEC ").append(proc).append(' ');
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) b.append(", ");
            b.append(names.get(i)).append("=?");
        }
        return b.toString();
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

    private static Object date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, 10))); }
        catch (Exception e) { return null; }
    }
}
