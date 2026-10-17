package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Two small approval dashboards that are not the big ApprovalDashboard.cs:
 *
 *   364 PendingDocumentForMultipleApprovals   Architecture.WinApp.ApprovalDashboard\PendingDocumentForMultipleApprovals.cs
 *       DynamicallyGenerateCards (:35-95)
 *         BLL.Dashboard.GetPendingDocumentApprovalsForDashboard
 *           USP_GetPendingDocumentApprovalsForDashboard @OrganizationId, @CompanyId, @UserId
 *         one card per ROW (not grouped): Id = RefDocumentTypeId, TargetUrl = TargetURL, Title = DocumentName,
 *         Count = NoofApprovals. TransType "Accounts" fills the Accounts panel, "Inventory" the Inventory panel.
 *
 *   368 ApprovalDashboard_St                  Architecture.WinApp.Steel.Approvals\ApprovalDashboard_St.cs
 *       DynamicallyGenerateCards (:41-92), ReportsParameters { OrganizationId, CompanyId, BranchesId = user's branch,
 *       FinancialYearId = active year, UserId }
 *         BLL.Dashboard.ReadAccountsDashboardPendingForApproval -> Sp_Voucher_ReadDashboard
 *             @OrganizationId, @CompanyId, @FinancialYearId, @UserId (no dates: FromDate/ToDate are not set)
 *             card: Id = TypeID, Title = VoucherType, Count = Count
 *         BLL.Steel.Inventory.PurchaseOrder.ReadInventoryDashboardPendingForApprovalforSteel
 *             [ST].[USP_Inventory_ReadDashboardPendingForApproval]
 *             @OrganizationId, @CompanyId, @BranchesId, @FinancialYearId (only when non-zero), @UserId
 *             card: Id = TypeID, Title = Description, Count = Count
 *
 * Read-only: both forms only open other screens; the approvals happen on those.
 */
@Service
public class W4ApprovalDashboardsService {

    private static final Logger LOG = LoggerFactory.getLogger(W4ApprovalDashboardsService.class);

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private ScreenRouteIndex screenRouteIndex;

    /** 364 - PendingDocumentForMultipleApprovals.DynamicallyGenerateCards. */
    public Map<String, Object> pendingDocuments() {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> accounts = new ArrayList<>();
        List<Map<String, Object>> inventory = new ArrayList<>();
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC USP_GetPendingDocumentApprovalsForDashboard @OrganizationId=?, @CompanyId=?, @UserId=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    currentUserContext.currentUserId());
            for (Map<String, Object> r : rows) {
                String transType = str(col(r, "TransType"));
                String target = str(col(r, "TargetURL"));
                Map<String, Object> c = new LinkedHashMap<>();
                c.put("id", asInt(col(r, "RefDocumentTypeId")));
                c.put("title", str(col(r, "DocumentName")));
                c.put("count", asInt(col(r, "NoofApprovals")));
                c.put("targetUrl", target);
                /* UserControl_Click :97-118 - the desktop opens Type.GetType(TargetUrl); the web opens a page only
                   when this application has one for that class (ScreenRouteIndex reads the registered routes). */
                c.put("route", target.isEmpty() ? null : screenRouteIndex.routeFor("", target));
                if ("Accounts".equals(transType)) accounts.add(c);
                else if ("Inventory".equals(transType)) inventory.add(c);
            }
        } catch (Exception e) {
            LOG.error("Pending document approvals dashboard failed", e);
            out.put("error", e.getMessage());
        }
        out.put("accounts", accounts);
        out.put("inventory", inventory);
        return out;
    }

    /** 368 - ApprovalDashboard_St.DynamicallyGenerateCards. */
    public Map<String, Object> steel() {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> accounts = new ArrayList<>();
        List<Map<String, Object>> inventory = new ArrayList<>();
        int org = currentUserContext.currentOrganizationId();
        int company = currentUserContext.currentCompanyId();
        int user = currentUserContext.currentUserId();
        int fy = 0;
        try { fy = currentUserContext.currentFinancialYearId(); } catch (Exception ignored) { }
        int branch = 0;
        try { branch = currentUserContext.currentBranchId(); } catch (Exception ignored) { }

        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC Sp_Voucher_ReadDashboard @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @UserId=?",
                    org, company, fy, user);
            for (Map<String, Object> r : rows) accounts.add(card(r, "VoucherType"));
        } catch (Exception e) {
            LOG.error("Steel approval dashboard (accounts) failed", e);
            out.put("accountsError", e.getMessage());
        }
        try {
            List<Map<String, Object>> rows;
            if (fy != 0) {
                rows = jdbcTemplate.queryForList(
                        "EXEC [ST].[USP_Inventory_ReadDashboardPendingForApproval] @OrganizationId=?, @CompanyId=?, @BranchesId=?, @FinancialYearId=?, @UserId=?",
                        org, company, branch, fy, user);
            } else {
                rows = jdbcTemplate.queryForList(
                        "EXEC [ST].[USP_Inventory_ReadDashboardPendingForApproval] @OrganizationId=?, @CompanyId=?, @BranchesId=?, @UserId=?",
                        org, company, branch, user);
            }
            for (Map<String, Object> r : rows) inventory.add(card(r, "Description"));
        } catch (Exception e) {
            LOG.error("Steel approval dashboard (inventory) failed", e);
            out.put("inventoryError", e.getMessage());
        }
        out.put("accounts", accounts);
        out.put("inventory", inventory);
        return out;
    }

    private static Map<String, Object> card(Map<String, Object> r, String titleColumn) {
        Map<String, Object> c = new LinkedHashMap<>();
        c.put("id", asInt(col(r, "TypeID")));
        c.put("title", str(col(r, titleColumn)));
        c.put("count", asInt(col(r, "Count")));
        return c;
    }

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
}
