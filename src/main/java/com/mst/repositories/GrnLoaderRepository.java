package com.mst.repositories;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.*;

@Repository
public class GrnLoaderRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    // Load Pending Regular GRNs
    public List<Map<String, Object>> getPendingGrns(int orgId, int compId, int docTypeId, int yearId,
                                                    String fromDate, String toDate, String branchIds,
                                                    Integer supplierCustomerId, Integer orderId) {
        try {
            StringBuilder sb = new StringBuilder("EXEC usp_getGrnLoaderDataForPurchaseInvoice ");
            sb.append("@OrganizationId=").append(orgId);
            sb.append(", @CompanyId=").append(compId);
            sb.append(", @DocumentTypeId=").append(docTypeId);
            if (financialYearCheck(yearId)) sb.append(", @FinancialYearId=").append(yearId);
            if (supplierCustomerId != null && supplierCustomerId > 0) sb.append(", @SupplierCustomerId=").append(supplierCustomerId);
            if (orderId != null && orderId > 0) sb.append(", @OrderId=").append(orderId);
            if (fromDate != null && !fromDate.isEmpty()) sb.append(", @FromDate='").append(fromDate).append("'");
            if (toDate != null && !toDate.isEmpty()) sb.append(", @ToDate='").append(toDate).append("'");
            if (branchIds != null && !branchIds.trim().isEmpty()) sb.append(", @BranchesIds='").append(branchIds).append("'");

            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            e.printStackTrace();
            return Collections.emptyList();
        }
    }

    // Load Pending Market GRNs
    public List<Map<String, Object>> getPendingMarketGrns(int orgId, int compId, int docTypeId,
                                                          String fromDate, String toDate,
                                                          Integer supplierCustomerId) {
        try {
            StringBuilder sb = new StringBuilder("EXEC Sp_InvPurchaseInvoice_GetAllMethod ");
            sb.append("@OrganizationId=").append(orgId);
            sb.append(", @CompanyId=").append(compId);
            sb.append(", @DocumentTypeId=").append(docTypeId);
            if (supplierCustomerId != null && supplierCustomerId > 0) sb.append(", @SupplierCustomerId=").append(supplierCustomerId);
            if (fromDate != null && !fromDate.isEmpty()) sb.append(", @FromDate='").append(fromDate).append("'");
            if (toDate != null && !toDate.isEmpty()) sb.append(", @ToDate='").append(toDate).append("'");
            sb.append(", @Activity='GetPendingMarketGrnForPurchaseInvoice'");

            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            e.printStackTrace();
            return Collections.emptyList();
        }
    }

    // Load GRN Detail lines by GRN Header ID
    public List<Map<String, Object>> getGrnDetails(int grnId) {
        try {
            String sql = "EXEC Sp_InvGrnDetail_GetAllMethod @InvGrnId=?, @Activity='ReadByInvGrnId'";
            return jdbcTemplate.queryForList(sql, grnId);
        } catch (Exception e1) {
            try {
                String sql = "SELECT * FROM InvGrnDetail WHERE InvGrnId = ?";
                return jdbcTemplate.queryForList(sql, grnId);
            } catch (Exception e2) {
                e2.printStackTrace();
                return Collections.emptyList();
            }
        }
    }

    // Get GRN Header by ID
    public Map<String, Object> getGrnHeader(int grnId) {
        try {
            String sql = "EXEC Sp_InvGrn_GetAllMethod @Id=?, @Activity='ReadById'";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, grnId);
            return (list != null && !list.isEmpty()) ? list.get(0) : null;
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }

    // Get User Allocated Branches for GRN
    public List<Map<String, Object>> getUserBranches(int orgId, int compId, int userId, int docTypeId) {
        try {
            String sql = "EXEC USP_GetBranchesAllocatedToUserFromGrn @OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeId=?";
            return jdbcTemplate.queryForList(sql, orgId, compId, userId, docTypeId);
        } catch (Exception e) {
            try {
                String sql = "SELECT Id as BranchId, BranchName FROM Branch WHERE (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL)";
                return jdbcTemplate.queryForList(sql, orgId, compId);
            } catch (Exception e2) {
                return Collections.emptyList();
            }
        }
    }

    private boolean financialYearCheck(int yearId) {
        return yearId > 0;
    }
}
