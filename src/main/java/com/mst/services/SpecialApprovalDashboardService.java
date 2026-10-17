package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

/** SpecialApprovalDashboard.cs: branch feature 11, one procedure, three card groups. */
@Service
public class SpecialApprovalDashboardService {
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;

    public SpecialApprovalDashboardService(JdbcTemplate jdbc, CurrentUserContext context) {
        this.jdbc = jdbc;
        this.context = context;
    }

    public Map<String, Object> setup() {
        UserAccount user = context.requireAccountingUser();
        return Map.of("branchFeature", branchFeature(user), "branches", branches(user),
                "currentBranchId", context.currentBranchId());
    }

    public Map<String, Object> cards(int requestedBranch) {
        UserAccount user = context.requireAccountingUser();
        boolean multiBranch = branchFeature(user);
        int branch = multiBranch ? requestedBranch : context.currentBranchId();
        List<Map<String, Object>> allocated = branches(user);
        // A zero branch would omit the procedure's filter and expose other branches.
        if (branch <= 0) throw new IllegalArgumentException("Please select a branch first.");
        if (allocated.stream().noneMatch(row -> number(row, "BranchId") == branch)) {
            throw new AccessDeniedException("You do not have access to the selected branch.");
        }
        Map<String, Object> parameters = DesktopProc.params("OrganizationId", user.getOrganizationId(),
                "CompanyId", user.getCompanyId(), "UserId", user.getId(), "BranchesId", branch);
        int year = context.currentFinancialYearId();
        if (year != 0) parameters.put("FinancialYearId", year);
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc,
                "usp_ReadDashboardPendingForSpecialApprovals", parameters);
        return groupCards(rows, branch);
    }

    private boolean branchFeature(UserAccount user) {
        return DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                DesktopProc.params("OrganizationId", user.getOrganizationId(), "CompanyId", user.getCompanyId()))
                .stream().anyMatch(row -> number(row, "Id") == 11);
    }

    private List<Map<String, Object>> branches(UserAccount user) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> row : DesktopProc.rows(jdbc, "USP_GetBranchsAllocatedToUser",
                DesktopProc.params("OrganizationId", user.getOrganizationId(), "CompanyId", user.getCompanyId(),
                        "UserId", user.getId()))) {
            out.add(Map.of("BranchId", number(row, "BranchId"), "BranchName", string(row, "BranchName")));
        }
        return out;
    }

    static Map<String, Object> groupCards(List<Map<String, Object>> rows, int branch) {
        Map<String, Object> result = new LinkedHashMap<>();
        Map<String, List<Map<String, Object>>> groups = new LinkedHashMap<>();
        groups.put("accounts", new ArrayList<>());
        groups.put("inventory", new ArrayList<>());
        groups.put("commission", new ArrayList<>());
        for (Map<String, Object> row : rows) {
            String group = switch (number(row, "ActionId")) {
                case 1 -> "accounts";
                case 2 -> "inventory";
                case 3 -> "commission";
                default -> null;
            };
            if (group == null) continue;
            int type = number(row, "TypeID"), reasonId = number(row, "ReasonId");
            Map<String, Object> card = new LinkedHashMap<>();
            card.put("id", type);
            card.put("title", string(row, "Description"));
            card.put("count", number(row, "Count"));
            card.put("reasonId", reasonId);
            String reason = string(row, "ReasonForSpecialApproval");
            card.put("reason", reason.isEmpty() ? reasonName(reasonId) : reason);
            card.put("desktopScreen", desktopScreen(type));
            groups.get(group).add(card);
        }
        result.putAll(groups);
        result.put("branchId", branch);
        return result;
    }

    // Stable captions from the recovered desktop definitions; no master-data writes.
    static String reasonName(int reasonId) {
        return switch (reasonId) {
            case 1 -> "Purchase order excess weight";
            case 2 -> "Bill weight exceeds stock weight";
            case 3 -> "Empty bag weight total is zero";
            case 4 -> "Supplier and factory weight difference exceeds tolerance";
            case 5 -> "Rate cut with excess weight";
            case 6 -> "Sale weight is less than purchase weight";
            case 7 -> "Sale weight is greater than purchase weight";
            default -> "";
        };
    }

    private static String desktopScreen(int type) {
        return switch (type) {
            case 1 -> "Stock Transfer Special Approval";
            case 2 -> "Pending Inward Gate Pass for Special Approval";
            case 3 -> "Pending GRN for Special Approval";
            case 4 -> "Pending Job Order for Special Approval";
            case 19 -> "Commercial Invoice Special Approval";
            case 27 -> "Export Invoice Voucher Special Approval";
            case 1056 -> "Commission Agent Trade Bill Special Approval";
            default -> "Special Approval";
        };
    }

    private static Object value(Map<String, Object> row, String key) {
        for (Map.Entry<String, Object> entry : row.entrySet()) {
            if (entry.getKey().equalsIgnoreCase(key)) return entry.getValue();
        }
        return null;
    }

    private static int number(Map<String, Object> row, String key) {
        Object value = value(row, key);
        if (value instanceof Number) return ((Number) value).intValue();
        try { return Integer.parseInt(String.valueOf(value)); }
        catch (NumberFormatException ignored) { return 0; }
    }

    private static String string(Map<String, Object> row, String key) {
        Object value = value(row, key);
        return value == null ? "" : String.valueOf(value).trim();
    }
}
