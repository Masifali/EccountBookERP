package com.mst.services.sale;

import com.mst.security.CurrentUserContext;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.Collections;
import java.util.List;
import java.util.Map;

/**
 * Service providing common lookups, master dropdowns, and document code generators
 * for the Sale Module (Booking, Sale Order, Delivery Order, GDN, Invoice, Return, Gate Pass).
 */
@Service
public class SaleCommonService {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private CurrentUserContext currentUserContext;

    // ==========================================
    // 1. CUSTOMER & SUPPLIER LOOKUPS
    // ==========================================
    public List<Map<String, Object>> getCustomers(String query) {
        try {
            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            
            if (query != null && !query.trim().isEmpty()) {
                String searchPattern = "%" + query.trim() + "%";
                String sql = "SELECT C.Id as id, C.AccountTitle as accountTitle, C.AccountCode as accountCode, C.AccountTypeId as accountTypeId " +
                             "FROM ChartofAccount C " +
                             "INNER JOIN COAAllocation A ON C.Id = A.ChartofAccountId " +
                             "WHERE C.IsActive = 1 AND A.IsActive = 1 " +
                             "AND A.OrganizationId = ? AND A.CompanyId = ? " +
                             "AND C.AccountGroup = 'Detail' " +
                             "AND C.AccountTypeId IN (3, 6, 8, 22) " +
                             "AND (C.AccountTitle LIKE ? OR C.AccountCode LIKE ?) " +
                             "ORDER BY C.AccountTitle ASC";
                return jdbcTemplate.queryForList(sql, orgId, compId, searchPattern, searchPattern);
            } else {
                String sql = "SELECT C.Id as id, C.AccountTitle as accountTitle, C.AccountCode as accountCode, C.AccountTypeId as accountTypeId " +
                             "FROM ChartofAccount C " +
                             "INNER JOIN COAAllocation A ON C.Id = A.ChartofAccountId " +
                             "WHERE C.IsActive = 1 AND A.IsActive = 1 " +
                             "AND A.OrganizationId = ? AND A.CompanyId = ? " +
                             "AND C.AccountGroup = 'Detail' " +
                             "AND C.AccountTypeId IN (3, 6, 8, 22) " +
                             "ORDER BY C.AccountTitle ASC";
                return jdbcTemplate.queryForList(sql, orgId, compId);
            }
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    // ==========================================
    // 2. ITEM & INVENTORY LOOKUPS
    // ==========================================
    public List<Map<String, Object>> getSaleItems(String query) {
        try {
            if (query != null && !query.trim().isEmpty()) {
                String searchPattern = "%" + query.trim() + "%";
                String sql = "SELECT i.Id as id, i.ItemCode as itemCode, i.ItemName as itemName, " +
                             "i.ItemCatagoryId as categoryId, i.ItemTypeId as itemTypeId, " +
                             "ISNULL(i.RetailPrice, 0) as retailPrice, ISNULL(i.TradePrice, 0) as tradePrice " +
                             "FROM Item i WHERE i.IsActive = 1 " +
                             "AND (i.ItemName LIKE ? OR i.ItemCode LIKE ?) " +
                             "ORDER BY i.ItemName ASC";
                return jdbcTemplate.queryForList(sql, searchPattern, searchPattern);
            } else {
                String sql = "SELECT i.Id as id, i.ItemCode as itemCode, i.ItemName as itemName, " +
                             "i.ItemCatagoryId as categoryId, i.ItemTypeId as itemTypeId, " +
                             "ISNULL(i.RetailPrice, 0) as retailPrice, ISNULL(i.TradePrice, 0) as tradePrice " +
                             "FROM Item i WHERE i.IsActive = 1 ORDER BY i.ItemName ASC";
                return jdbcTemplate.queryForList(sql);
            }
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getWarehouses() {
        try {
            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            String sql = "SELECT Id as id, WarehouseName as warehouseName, WarehouseCode as warehouseCode " +
                         "FROM Warehouse WHERE OrganizationId = ? AND CompanyId = ? AND IsActive = 1 ORDER BY WarehouseName";
            return jdbcTemplate.queryForList(sql, orgId, compId);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getJobLots() {
        try {
            String sql = "SELECT Id as id, JobLotDescription as description FROM JobLot ORDER BY JobLotDescription";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getItemPackings() {
        try {
            String sql = "SELECT Id as id, PackingTitle as packingTitle, PackingSize as packingSize FROM ItemPacking ORDER BY PackingTitle";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getItemUoms() {
        try {
            String sql = "SELECT Id as id, UOMTitle as uomTitle FROM ItemUOM ORDER BY UOMTitle";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    // ==========================================
    // 3. DOCUMENT CODE GENERATORS
    // ==========================================
    public int generateDocumentCode(int documentTypeId) {
        try {
            int compId = currentUserContext.currentCompanyId();
            int finYearId = currentUserContext.currentFinancialYearId();

            String sql = "SELECT ISNULL(MAX(VoucherCode), 0) + 1 FROM VoucherHead " +
                         "WHERE DocumentTypeId = ? AND CompanyId = ? AND FinancialYearId = ?";
            Integer code = jdbcTemplate.queryForObject(sql, Integer.class, documentTypeId, compId, finYearId);
            return (code != null && code > 0) ? code : 1;
        } catch (Exception e) {
            return 1;
        }
    }
}
