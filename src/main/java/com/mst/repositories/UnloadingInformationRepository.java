package com.mst.repositories;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.*;

@Repository
public class UnloadingInformationRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    // Get Items for Unloading against GatePass & Lab
    public List<Map<String, Object>> getItemsForUnloading(int orgId, int compId, Integer gpId, Integer labId) {
        try {
            String sql = "EXEC USP_InvLabAnalysisPurchaseHeader_GetItemsForUnloading @OrganizationId=?, @CompanyId=?, @GpId=?, @LabId=?";
            return jdbcTemplate.queryForList(sql, orgId, compId, gpId != null ? gpId : 0, labId != null ? labId : 0);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    // Get Contractors for Wages
    public List<Map<String, Object>> getContractors(int orgId, int compId, Integer branchId) {
        try {
            String sql = "EXEC USP_GetContractorsAllocatedToBranch @OrganizationId=?, @CompanyId=?, @BranchesId=?";
            return jdbcTemplate.queryForList(sql, orgId, compId, branchId != null ? branchId : 0);
        } catch (Exception e) {
            try {
                String sql = "SELECT Id as id, CompanyName as name FROM SupplierCustomer WHERE CustomerGroupId = 6 AND (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL)";
                return jdbcTemplate.queryForList(sql, orgId, compId);
            } catch (Exception e2) {
                return Collections.emptyList();
            }
        }
    }

    // Read by ID
    public Map<String, Object> getById(int id) {
        try {
            String sql = "EXEC USP_UnloadingInformation_GetAllMethod @Id=?, @Activity='ReadById'";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, id);
            return (list != null && !list.isEmpty()) ? list.get(0) : null;
        } catch (Exception e) {
            return null;
        }
    }

    public List<Map<String, Object>> getDetailsByHeaderId(int id) {
        try {
            String sql = "EXEC USP_UnloadingInformationDetail_GetAllMethod @UnloadingInformationId=?, @Activity='ReadByHeaderId'";
            return jdbcTemplate.queryForList(sql, id);
        } catch (Exception e) {
            try {
                String sql = "SELECT * FROM UnloadingInformationDetail WHERE UnloadingInformationId = ?";
                return jdbcTemplate.queryForList(sql, id);
            } catch (Exception e2) {
                return Collections.emptyList();
            }
        }
    }

    // History Search
    public List<Map<String, Object>> getHistory(int orgId, int compId, int yearId, String fromDate, String toDate, Double fromDocNo, Double toDocNo) {
        try {
            StringBuilder sb = new StringBuilder("EXEC USP_UnloadingInformation_GetAllMethod ");
            sb.append("@OrganizationId=").append(orgId);
            sb.append(", @CompanyId=").append(compId);
            if (yearId > 0) sb.append(", @FinancialYearId=").append(yearId);
            if (fromDate != null && !fromDate.isEmpty()) sb.append(", @FromDate='").append(fromDate).append("'");
            if (toDate != null && !toDate.isEmpty()) sb.append(", @ToDate='").append(toDate).append("'");
            if (fromDocNo != null && fromDocNo > 0) sb.append(", @FromDocNo=").append(fromDocNo);
            if (toDocNo != null && toDocNo > 0) sb.append(", @ToDocNo=").append(toDocNo);
            sb.append(", @Activity='FormHistory'");

            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            e.printStackTrace();
            return Collections.emptyList();
        }
    }

    // Delete Record
    public void deleteRecord(int id, int userId) {
        try {
            String sql = "EXEC USP_UnloadingInformation_GetAllMethod @Id=?, @EntryUserId=?, @Activity='DeleteById'";
            jdbcTemplate.update(sql, id, userId);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
