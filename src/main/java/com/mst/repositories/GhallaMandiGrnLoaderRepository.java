package com.mst.repositories;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.*;

@Repository
public class GhallaMandiGrnLoaderRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    // Load Loading Contractors (CustomerGroupId = 6)
    public List<Map<String, Object>> getLoadingContractors(int orgId, int compId) {
        try {
            String sql = "SELECT Id as id, CompanyName as name FROM SupplierCustomer " +
                    "WHERE (CustomerGroupId = 6 OR CustomerGroupId = '6') " +
                    "AND (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL) " +
                    "ORDER BY CompanyName";
            return jdbcTemplate.queryForList(sql, orgId, compId);
        } catch (Exception e) {
            try {
                String sql = "EXEC USP_GetSupplierustomerByCustomerGroupId @CustomerGroupId=6";
                return jdbcTemplate.queryForList(sql);
            } catch (Exception e2) {
                return Collections.emptyList();
            }
        }
    }

    // Load Ghalla Mandi dropdown
    public List<Map<String, Object>> getGhallaMandiList(int orgId, int compId) {
        try {
            String sql = "SELECT Id as id, Description as name FROM GhallaMandi ORDER BY Description";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            try {
                String sql = "EXEC Sp_GhallaMandi_GetAllMethod @Activity='ReadAll'";
                return jdbcTemplate.queryForList(sql);
            } catch (Exception e2) {
                return Collections.emptyList();
            }
        }
    }

    // Load Pending Ghalla Mandi GRNs
    public List<Map<String, Object>> getPendingGhallaMandiGrns(int orgId, int compId, int docTypeId, int yearId,
                                                               String fromDate, String toDate,
                                                               Integer supplierCustomerId, Integer ghallaMandiId) {
        try {
            StringBuilder sb = new StringBuilder("EXEC Sp_InvPurchaseInvoice_GetAllMethod ");
            sb.append("@OrganizationId=").append(orgId);
            sb.append(", @CompanyId=").append(compId);
            sb.append(", @DocumentTypeId=").append(docTypeId);
            if (yearId > 0) sb.append(", @FinancialYearId=").append(yearId);
            if (supplierCustomerId != null && supplierCustomerId > 0) sb.append(", @SupplierCustomerId=").append(supplierCustomerId);
            if (ghallaMandiId != null && ghallaMandiId > 0) sb.append(", @GhallaMandiId=").append(ghallaMandiId);
            if (fromDate != null && !fromDate.isEmpty()) sb.append(", @FromDate='").append(fromDate).append("'");
            if (toDate != null && !toDate.isEmpty()) sb.append(", @ToDate='").append(toDate).append("'");
            sb.append(", @Activity='GetPendingGhallaMandiGrnForInvoice'");

            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            e.printStackTrace();
            return Collections.emptyList();
        }
    }
}
