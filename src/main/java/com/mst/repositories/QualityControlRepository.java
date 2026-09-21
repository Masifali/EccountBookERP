package com.mst.repositories;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.Collections;
import java.util.List;
import java.util.Map;

@Repository
public class QualityControlRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    public List<Map<String, Object>> getLabs(int orgId, int companyId, int branchId) {
        String sql = "SELECT InvLabAnalysisItemsId AS Id, LabTitle, LabCode, IsActive FROM InvLabAnalysisItems WHERE (OrganizationId = ? OR ? = 0) ORDER BY LabTitle";
        try {
            return jdbcTemplate.queryForList(sql, orgId, orgId);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getAnalysisParameters(int orgId, int companyId, int branchId) {
        String sql = "SELECT InvLabAnalysisItemsId AS Id, ParameterTitle, ParameterCode, StandardValue, MinValue, MaxValue FROM InvLabAnalysisItems WHERE (OrganizationId = ? OR ? = 0) ORDER BY ParameterTitle";
        try {
            return jdbcTemplate.queryForList(sql, orgId, orgId);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getAnalysisGroups(int orgId, int companyId, int branchId) {
        String sql = "SELECT InvLabAnalysisGroupId AS Id, GroupTitle, GroupCode FROM InvLabAnalysisGroup WHERE (OrganizationId = ? OR ? = 0) ORDER BY GroupTitle";
        try {
            return jdbcTemplate.queryForList(sql, orgId, orgId);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getSampleLogHistory(int orgId, int companyId, int branchId) {
        try {
            return jdbcTemplate.queryForList("EXEC Sp_InvLabSampleLogRegister_GetMethod @Activity='FormHistory', @OrganizationId=?, @CompanyId=?, @BranchId=?", orgId, companyId, branchId);
        } catch (Exception e) {
            String fallback = "SELECT TOP 100 InvLabSampleLogRegisterId AS Id, SampleLogNo AS VoucherCode, SampleLogDate AS VoucherDate, CustomerName, Remarks FROM InvLabSampleLogRegister ORDER BY InvLabSampleLogRegisterId DESC";
            try { return jdbcTemplate.queryForList(fallback); } catch (Exception ex) { return Collections.emptyList(); }
        }
    }

    public List<Map<String, Object>> getSampleAnalysisHistory(int orgId, int companyId, int branchId) {
        try {
            return jdbcTemplate.queryForList("EXEC Sp_InvLabSampleAnalysis_GetMethod @Activity='FormHistory', @OrganizationId=?, @CompanyId=?, @BranchId=?", orgId, companyId, branchId);
        } catch (Exception e) {
            String fallback = "SELECT TOP 100 InvLabSampleAnalysisHeaderId AS Id, AnalysisNo AS VoucherCode, AnalysisDate AS VoucherDate, SampleNo, CustomerName, Remarks FROM InvLabSampleAnalysisHeader ORDER BY InvLabSampleAnalysisHeaderId DESC";
            try { return jdbcTemplate.queryForList(fallback); } catch (Exception ex) { return Collections.emptyList(); }
        }
    }

    public List<Map<String, Object>> getPurchaseAnalysisHistory(int orgId, int companyId, int branchId) {
        try {
            return jdbcTemplate.queryForList("EXEC Sp_InvLabPurchaseAnalysis_GetMethod @Activity='FormHistory', @OrganizationId=?, @CompanyId=?, @BranchId=?", orgId, companyId, branchId);
        } catch (Exception e) {
            String fallback = "SELECT TOP 100 InvLabAnalysisPurchaseHeaderId AS Id, PurchaseAnalysisNo AS VoucherCode, AnalysisDate AS VoucherDate, VehicleNo, SupplierName, Remarks FROM InvLabAnalysisPurchaseHeader ORDER BY InvLabAnalysisPurchaseHeaderId DESC";
            try { return jdbcTemplate.queryForList(fallback); } catch (Exception ex) { return Collections.emptyList(); }
        }
    }

    public List<Map<String, Object>> getInProcessAnalysisHistory(int orgId, int companyId, int branchId) {
        try {
            return jdbcTemplate.queryForList("EXEC Sp_InvLabAnalysisInProcess_GetMethod @Activity='FormHistory', @OrganizationId=?, @CompanyId=?, @BranchId=?", orgId, companyId, branchId);
        } catch (Exception e) {
            String fallback = "SELECT TOP 100 InvLabAnalysisInProcessHeaderId AS Id, InProcessNo AS VoucherCode, AnalysisDate AS VoucherDate, StepName, OperatorName, Remarks FROM InvLabAnalysisInProcessHeader ORDER BY InvLabAnalysisInProcessHeaderId DESC";
            try { return jdbcTemplate.queryForList(fallback); } catch (Exception ex) { return Collections.emptyList(); }
        }
    }

    public Map<String, Object> getSampleLogById(int id) {
        String sql = "SELECT * FROM InvLabSampleLogRegister WHERE InvLabSampleLogRegisterId = ?";
        try {
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, id);
            return list.isEmpty() ? Collections.emptyMap() : list.get(0);
        } catch (Exception e) {
            return Collections.emptyMap();
        }
    }

    public Map<String, Object> getSampleAnalysisById(int id) {
        String sql = "SELECT * FROM InvLabSampleAnalysisHeader WHERE InvLabSampleAnalysisHeaderId = ?";
        try {
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, id);
            return list.isEmpty() ? Collections.emptyMap() : list.get(0);
        } catch (Exception e) {
            return Collections.emptyMap();
        }
    }

    public Map<String, Object> getPurchaseAnalysisById(int id) {
        String sql = "SELECT * FROM InvLabAnalysisPurchaseHeader WHERE InvLabAnalysisPurchaseHeaderId = ?";
        try {
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, id);
            return list.isEmpty() ? Collections.emptyMap() : list.get(0);
        } catch (Exception e) {
            return Collections.emptyMap();
        }
    }

    public Map<String, Object> getInProcessAnalysisById(int id) {
        String sql = "SELECT * FROM InvLabAnalysisInProcessHeader WHERE InvLabAnalysisInProcessHeaderId = ?";
        try {
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, id);
            return list.isEmpty() ? Collections.emptyMap() : list.get(0);
        } catch (Exception e) {
            return Collections.emptyMap();
        }
    }
}
