package com.mst.repositories;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.*;

@Repository
public class GrnRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    // Document Code Generator
    public Integer generateDocNo(int orgId, int compId, int yearId, int docTypeId) {
        try {
            String sql = "EXEC Sp_InvGrn_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @FinancialYearId=?, @Activity='GenerategpCode'";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, orgId, compId, docTypeId, yearId);
            if (list != null && !list.isEmpty() && list.get(0).get("GpSrNo") != null) {
                return Integer.parseInt(list.get(0).get("GpSrNo").toString());
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        return 1;
    }

    // Pending GatePasses for GRN
    public List<Map<String, Object>> getPendingGatePasses(int orgId, int compId, int docTypeId, String fromDate, String toDate) {
        try {
            StringBuilder sb = new StringBuilder("EXEC Sp_InvGrn_GetAllMethod ");
            sb.append("@OrganizationId=").append(orgId);
            sb.append(", @CompanyId=").append(compId);
            sb.append(", @DocumentTypeId=").append(docTypeId);
            if (fromDate != null && !fromDate.isEmpty()) sb.append(", @FromDate='").append(fromDate).append("'");
            if (toDate != null && !toDate.isEmpty()) sb.append(", @ToDate='").append(toDate).append("'");
            sb.append(", @Activity='GetPendingGatePassForGrn'");

            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    // Read by ID
    public Map<String, Object> getById(int id) {
        try {
            String sql = "EXEC Sp_InvGrn_GetAllMethod @Id=?, @Activity='ReadById'";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, id);
            return (list != null && !list.isEmpty()) ? list.get(0) : null;
        } catch (Exception e) {
            return null;
        }
    }

    public List<Map<String, Object>> getDetailsByHeaderId(int id) {
        try {
            String sql = "EXEC Sp_InvGrnDetail_GetAllMethod @InvGrnId=?, @Activity='ReadByInvGrnId'";
            return jdbcTemplate.queryForList(sql, id);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    // History Search
    public List<Map<String, Object>> getHistory(int orgId, int compId, int branchId, int yearId,
                                                int docTypeId, String fromDate, String toDate,
                                                Double fromDocNo, Double toDocNo, Integer supplierId) {
        try {
            StringBuilder sb = new StringBuilder("EXEC Sp_GatePassInward_GetAllMethod ");
            sb.append("@OrganizationId=").append(orgId);
            sb.append(", @CompanyId=").append(compId);
            if (branchId > 0) sb.append(", @BranchesId=").append(branchId);
            if (yearId > 0) sb.append(", @FinancialYearId=").append(yearId);
            sb.append(", @DocumentTypeId=").append(docTypeId);
            if (fromDate != null && !fromDate.isEmpty()) sb.append(", @DateFrom='").append(fromDate).append("'");
            if (toDate != null && !toDate.isEmpty()) sb.append(", @DateTo='").append(toDate).append("'");
            if (fromDocNo != null && fromDocNo > 0) sb.append(", @DocNoFrom=").append(fromDocNo);
            if (toDocNo != null && toDocNo > 0) sb.append(", @DocNoTo=").append(toDocNo);
            if (supplierId != null && supplierId > 0) sb.append(", @SupplierCustomerId=").append(supplierId);
            sb.append(", @Activity='GatepassHistory'");

            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            e.printStackTrace();
            return Collections.emptyList();
        }
    }

    // Delete Record
    public void deleteRecord(int id, int orgId, int compId) {
        try {
            String sql = "EXEC Sp_InvGrn_Delete @Id=?, @OrganizationId=?, @CompanyId=?";
            jdbcTemplate.update(sql, id, orgId, compId);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
