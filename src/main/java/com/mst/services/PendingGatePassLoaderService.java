package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class PendingGatePassLoaderService {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    public List<Map<String, Object>> getPendingGatePasses(int orgId, int compId, int branchId, int yearId) {
        try {
            String sql = "EXEC USP_PendingGatePassInwardForGRN @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @BranchesIds=?";
            return jdbcTemplate.queryForList(sql, orgId, compId, yearId, String.valueOf(branchId));
        } catch (Exception e) {
            try {
                String sqlFallback = "SELECT g.Id as id, g.GpSrNo as gpSrNo, g.Status as status, g.GpDate as gpDate, " +
                        "g.OrderType as orderType, s.CompanyName as supplierName, g.VehicleNo as vehicleNo, " +
                        "g.BiltyNo as biltyNo, g.Qty as qty, g.SupplierWeight as supplierWeight, g.FactoryWeight as factoryWeight, " +
                        "g.ReceivedWeight as receivedWeight, g.StockWeight as stockWeight, g.AccessWeight as accessWeight, " +
                        "g.VehicleType as vehicleType, g.LabId as lastLabId " +
                        "FROM GatepassInward g " +
                        "LEFT JOIN SupplierCustomer s ON g.SupplierCustomerId = s.Id " +
                        "WHERE (g.OrganizationId = ? OR g.OrganizationId IS NULL) " +
                        "AND (g.CompanyId = ? OR g.CompanyId IS NULL) " +
                        "AND (g.Status = 'Accepted' OR g.Status = 'Open') " +
                        "ORDER BY g.GpSrNo DESC";
                return jdbcTemplate.queryForList(sqlFallback, orgId, compId);
            } catch (Exception e2) {
                return Collections.emptyList();
            }
        }
    }
}
