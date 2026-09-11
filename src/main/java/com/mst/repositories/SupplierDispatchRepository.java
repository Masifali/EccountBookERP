package com.mst.repositories;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.*;

@Repository
public class SupplierDispatchRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    // Generate Document Code (DocNo)
    public Integer generateDocNo(int orgId, int compId, int yearId, int docTypeId) {
        try {
            String sql = "EXEC USP_SupplierDispatch_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @FinancialYearId=?, @Activity='GenerateCode'";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, orgId, compId, docTypeId, yearId);
            if (list != null && !list.isEmpty() && list.get(0).get("DocNo") != null) {
                return Integer.parseInt(list.get(0).get("DocNo").toString());
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        return 1;
    }

    // Generate Branch Code (BranchSrNo)
    public Integer generateBranchDocNo(int orgId, int compId, int branchId, int yearId, int docTypeId) {
        try {
            String sql = "EXEC USP_SupplierDispatch_GetAllMethod @OrganizationId=?, @CompanyId=?, @BranchesId=?, @DocumentTypeId=?, @FinancialYearId=?, @Activity='GenerateBranchCode'";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, orgId, compId, branchId, docTypeId, yearId);
            if (list != null && !list.isEmpty() && list.get(0).get("BranchSrNo") != null) {
                return Integer.parseInt(list.get(0).get("BranchSrNo").toString());
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        return 1;
    }

    // Cities
    public List<Map<String, Object>> getCities(int orgId, int compId) {
        try {
            String sql = "SELECT Id as id, Description as name FROM City WHERE (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL) ORDER BY Description";
            return jdbcTemplate.queryForList(sql, orgId, compId);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    // History Search
    public List<Map<String, Object>> getHistory(int orgId, int compId, int docTypeId, int yearId,
                                                String fromDate, String toDate, Double fromDocNo, Double toDocNo) {
        try {
            StringBuilder sb = new StringBuilder("EXEC USP_SupplierDispatch_GetAllMethod ");
            sb.append("@OrganizationId=").append(orgId);
            sb.append(", @CompanyId=").append(compId);
            sb.append(", @DocumentTypeId=").append(docTypeId);
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

    // Read by ID
    public Map<String, Object> getById(int id) {
        try {
            String sql = "EXEC USP_SupplierDispatch_GetAllMethod @Id=?, @Activity='ReadById'";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, id);
            return (list != null && !list.isEmpty()) ? list.get(0) : null;
        } catch (Exception e) {
            return null;
        }
    }

    public List<Map<String, Object>> getDetailsByHeaderId(int id) {
        try {
            String sql = "EXEC USP_SupplierDispatchDetail_GetAllMethod @SupplierDispatchId=?, @Activity='ReadByHeaderId'";
            return jdbcTemplate.queryForList(sql, id);
        } catch (Exception e) {
            try {
                String sql = "SELECT * FROM SupplierDispatchDetail WHERE SupplierDispatchId = ?";
                return jdbcTemplate.queryForList(sql, id);
            } catch (Exception e2) {
                return Collections.emptyList();
            }
        }
    }

    // Delete Record
    public void deleteRecord(int id, int userId) {
        try {
            String sql = "EXEC USP_SupplierDispatch_GetAllMethod @Id=?, @EntryUserId=?, @Activity='DeleteById'";
            jdbcTemplate.update(sql, id, userId);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
