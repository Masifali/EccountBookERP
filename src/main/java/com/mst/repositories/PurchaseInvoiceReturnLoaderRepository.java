package com.mst.repositories;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.*;

@Repository
public class PurchaseInvoiceReturnLoaderRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    // Get User Allocated Branches for Purchase Invoice Return
    public List<Map<String, Object>> getUserBranches(int orgId, int compId, int userId) {
        try {
            String sql = "EXEC USP_GetBranchsAllocatedToUserFromPurchaseInvoice @OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeId=0";
            return jdbcTemplate.queryForList(sql, orgId, compId, userId);
        } catch (Exception e) {
            try {
                String sql = "SELECT Id as BranchId, BranchName FROM Branch WHERE (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL)";
                return jdbcTemplate.queryForList(sql, orgId, compId);
            } catch (Exception e2) {
                return Collections.emptyList();
            }
        }
    }

    // Load Pending Purchase Invoices for Return
    public List<Map<String, Object>> getPendingInvoicesForReturn(int orgId, int compId, int yearId,
                                                                 String fromDate, String toDate, String branchIds) {
        try {
            StringBuilder sb = new StringBuilder("EXEC USP_PendingPurchaseInvoiceForReturnInvoice ");
            sb.append("@OrganizationId=").append(orgId);
            sb.append(", @CompanyId=").append(compId);
            if (yearId > 0) sb.append(", @FinancialYearId=").append(yearId);
            if (fromDate != null && !fromDate.isEmpty()) sb.append(", @FromDate='").append(fromDate).append("'");
            if (toDate != null && !toDate.isEmpty()) sb.append(", @ToDate='").append(toDate).append("'");
            if (branchIds != null && !branchIds.trim().isEmpty()) sb.append(", @BranchesIds='").append(branchIds).append("'");

            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            e.printStackTrace();
            return Collections.emptyList();
        }
    }
}
