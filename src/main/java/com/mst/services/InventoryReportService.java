package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class InventoryReportService {
    private static final org.slf4j.Logger LOG = org.slf4j.LoggerFactory.getLogger(InventoryReportService.class);

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private CurrentUserContext currentUserContext;

    /**
     * Stock Evaluation / Stock Balance Report
     */
    public List<Map<String, Object>> getStockEvaluationReport(String asOnDate, Integer warehouseId, Integer itemCategoryId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT i.ID as itemId, i.ItemCode as itemCode, i.ItemName as itemName, ");
            sb.append("c.CategoryName as categoryName, w.WarehouseName as warehouseName, ");
            sb.append("ISNULL(s.OpeningStock, 0) as openingStock, ISNULL(s.QtyIn, 0) as qtyIn, ");
            sb.append("ISNULL(s.QtyOut, 0) as qtyOut, ISNULL(s.ClosingStock, 0) as closingStock, ");
            sb.append("ISNULL(s.AverageRate, 0) as averageRate, ISNULL(s.TotalValue, 0) as totalValue ");
            sb.append("FROM Item i ");
            sb.append("LEFT JOIN ItemCategory c ON i.ItemCategoryId = c.ID ");
            sb.append("LEFT JOIN ItemStockSummary s ON i.ID = s.ItemId ");
            sb.append("LEFT JOIN Warehouse w ON s.WarehouseId = w.ID ");
            sb.append("WHERE i.OrganizationId = ").append(orgId).append(" AND i.CompanyId = ").append(compId).append(" ");
            if (warehouseId != null && warehouseId > 0) {
                sb.append("AND s.WarehouseId = ").append(warehouseId).append(" ");
            }
            if (itemCategoryId != null && itemCategoryId > 0) {
                sb.append("AND i.ItemCategoryId = ").append(itemCategoryId).append(" ");
            }
            sb.append("ORDER BY i.ItemCode ASC");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) { LOG.warn("Item query failed", e); return Collections.emptyList(); }
    }

    /**
     * Inward Gate Pass Register Report
     */
    public List<Map<String, Object>> getInwardGatePassReport(String fromDate, String toDate, Integer supplierId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT gp.ID as id, gp.GPSrNo as docNo, gp.GPDate as docDate, ");
            sb.append("s.AccountTitle as supplierName, gp.VehicleNo as vehicleNo, ");
            sb.append("gp.DriverName as driverName, gp.Status as status ");
            sb.append("FROM InwardGatePass gp ");
            sb.append("LEFT JOIN ChartofAccount s ON gp.SupplierId = s.ID ");
            sb.append("WHERE gp.OrganizationId = ").append(orgId).append(" AND gp.CompanyId = ").append(compId).append(" ");
            if (supplierId != null && supplierId > 0) {
                sb.append("AND gp.SupplierId = ").append(supplierId).append(" ");
            }
            if (fromDate != null && !fromDate.isBlank()) {
                sb.append("AND gp.GPDate >= '").append(fromDate).append(" 00:00:00' ");
            }
            if (toDate != null && !toDate.isBlank()) {
                sb.append("AND gp.GPDate <= '").append(toDate).append(" 23:59:59' ");
            }
            sb.append("ORDER BY gp.GPDate DESC, gp.GPSrNo DESC");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) { LOG.warn("InwardGatePass query failed", e); return Collections.emptyList(); }
    }

    /**
     * Inventory Profitability Report
     */
    public List<Map<String, Object>> getInventoryProfitabilityReport(String fromDate, String toDate, Integer itemCategoryId, Integer itemId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT c.CategoryDescription as catName, i.ItemCode as itemCode, i.ItemName as item, ");
            sb.append("'KGs' as uom, ");
            sb.append("ISNULL(SUM(d.QtyOut), 0) as qty, ");
            sb.append("ISNULL(SUM(d.CreditAmount), 0) as saleAmount, ");
            sb.append("ISNULL(SUM(d.DebitAmount), 0) as costAmount, ");
            sb.append("ISNULL(AVG(i.CostPrice), 0) as realCost ");
            sb.append("FROM Item i ");
            sb.append("LEFT JOIN ItemCategory c ON i.ItemCategoryId = c.ID ");
            sb.append("LEFT JOIN VoucherDetail d ON d.AccountId = i.SaleGLAC ");
            sb.append("WHERE i.OrganizationId = ").append(orgId).append(" AND i.CompanyId = ").append(compId).append(" ");
            if (itemCategoryId != null && itemCategoryId > 0) {
                sb.append("AND i.ItemCategoryId = ").append(itemCategoryId).append(" ");
            }
            if (itemId != null && itemId > 0) {
                sb.append("AND i.ID = ").append(itemId).append(" ");
            }
            sb.append("GROUP BY c.CategoryDescription, i.ItemCode, i.ItemName ");
            sb.append("ORDER BY i.ItemName ASC");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) { LOG.warn("Item query failed", e); return Collections.emptyList(); }
    }
}
