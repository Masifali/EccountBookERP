package com.mst.repositories;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.*;

@Repository
public class PaddyGrnLoaderRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    // Load Suppliers for Paddy Purchase (CustomerGroupId = 2)
    public List<Map<String, Object>> getSuppliers(int orgId, int compId) {
        try {
            String sql = "SELECT Id as id, CompanyName as name FROM SupplierCustomer " +
                    "WHERE (CustomerGroupId = 2 OR CustomerGroupId = '2') " +
                    "AND (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL) " +
                    "ORDER BY CompanyName";
            return jdbcTemplate.queryForList(sql, orgId, compId);
        } catch (Exception e) {
            try {
                String sql = "EXEC USP_GetSupplierustomerByCustomerGroupId @CustomerGroupId=2";
                return jdbcTemplate.queryForList(sql);
            } catch (Exception e2) {
                return Collections.emptyList();
            }
        }
    }

    // Load Purchase Orders by Supplier ID
    public List<Map<String, Object>> getOrdersBySupplierId(int orgId, int compId, int supplierId) {
        try {
            String sql = "EXEC Sp_PurchaseOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=41, @Status='Open', @SupplierCustomerId=?, @IsApproved=1, @Activity='GetOrdersBySupplierId'";
            return jdbcTemplate.queryForList(sql, orgId, compId, supplierId);
        } catch (Exception e) {
            try {
                String sql = "SELECT Id as id, DocNo as name FROM PurchaseOrder " +
                        "WHERE SupplierCustomerId = ? AND Status = 'Open' AND (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL) " +
                        "ORDER BY DocNo DESC";
                return jdbcTemplate.queryForList(sql, supplierId, orgId, compId);
            } catch (Exception e2) {
                return Collections.emptyList();
            }
        }
    }

    // Load Pending Paddy GRNs
    public List<Map<String, Object>> getPendingPaddyGrns(int orgId, int compId, int docTypeId, int yearId,
                                                         String fromDate, String toDate, Integer orderId) {
        try {
            StringBuilder sb = new StringBuilder("EXEC usp_getGrnLoaderDataForPurchaseInvoice ");
            sb.append("@OrganizationId=").append(orgId);
            sb.append(", @CompanyId=").append(compId);
            sb.append(", @DocumentTypeId=").append(docTypeId);
            if (yearId > 0) sb.append(", @FinancialYearId=").append(yearId);
            if (orderId != null && orderId > 0) sb.append(", @OrderId=").append(orderId);
            if (fromDate != null && !fromDate.isEmpty()) sb.append(", @FromDate='").append(fromDate).append("'");
            if (toDate != null && !toDate.isEmpty()) sb.append(", @ToDate='").append(toDate).append("'");

            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            e.printStackTrace();
            return Collections.emptyList();
        }
    }
}
