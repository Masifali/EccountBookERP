package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

@Service
public class PurchaseDirectInvoiceService {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    public Map<String, Object> getDropdowns(int orgId, int compId) {
        Map<String, Object> result = new HashMap<>();

        try {
            String sql = "SELECT Id as id, CompanyName as name, ISNULL(PartyCode, ManualPartyCode) as code, GlAccountId as glAccountId " +
                    "FROM SupplierCustomer WHERE (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL) " +
                    "ORDER BY CompanyName";
            result.put("suppliers", jdbcTemplate.queryForList(sql, orgId, compId));
        } catch (Exception e) {
            result.put("suppliers", Collections.emptyList());
        }

        try {
            String sql = "SELECT Id as id, ItemName as name, ItemCode as code FROM Item WHERE (ItemStatus IS NULL OR ItemStatus = 1) ORDER BY ItemName";
            result.put("items", jdbcTemplate.queryForList(sql));
        } catch (Exception e) {
            result.put("items", Collections.emptyList());
        }

        try {
            String sql = "SELECT Id as id, WareHouseName as name FROM InvWareHouse WHERE (IsActive IS NULL OR IsActive = 1) ORDER BY WareHouseName";
            result.put("warehouses", jdbcTemplate.queryForList(sql));
        } catch (Exception e) {
            result.put("warehouses", Collections.emptyList());
        }

        try {
            String sql = "SELECT Id as id, CropYear as name FROM CropYear ORDER BY CropYear DESC";
            result.put("cropYears", jdbcTemplate.queryForList(sql));
        } catch (Exception e) {
            result.put("cropYears", Collections.emptyList());
        }

        try {
            String sql = "SELECT Id as id, JobLotDescription as name FROM JobLot ORDER BY JobLotDescription";
            result.put("jobLots", jdbcTemplate.queryForList(sql));
        } catch (Exception e) {
            result.put("jobLots", Collections.emptyList());
        }

        try {
            String sql = "SELECT Id as id, PackTypeDesc as name FROM InvPackingType WHERE (IsActive IS NULL OR IsActive = 1) ORDER BY PackTypeDesc";
            result.put("packingTypes", jdbcTemplate.queryForList(sql));
        } catch (Exception e) {
            result.put("packingTypes", Collections.emptyList());
        }

        return result;
    }

    public int generateNextDocNo(int orgId, int compId, int branchId, int yearId) {
        try {
            String sql = "SELECT ISNULL(MAX(DocNo), 0) + 1 FROM InvPurchaseInvoice WHERE DocumentTypeId = 57 AND (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL)";
            Integer nextNo = jdbcTemplate.queryForObject(sql, Integer.class, orgId, compId);
            return (nextNo != null && nextNo > 0) ? nextNo : 1;
        } catch (Exception e) {
            return 1;
        }
    }

    public List<Map<String, Object>> getHistory(int orgId, int compId, int branchId, int yearId,
                                                String fromDate, String toDate, Integer supplierId,
                                                Integer fromDocNo, Integer toDocNo, String dateType) {
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT i.Id as id, i.DocNo as docNo, CONVERT(VARCHAR(10), i.DocDate, 120) as docDate, ")
              .append("s.CompanyName as supplierName, i.BillAmount as billAmount, i.ManualBillNo as manualBillNo, ")
              .append("i.RemarksHeader as remarks, CONVERT(VARCHAR(10), i.EntryDate, 120) as entryDate ")
              .append("FROM InvPurchaseInvoice i ")
              .append("LEFT JOIN SupplierCustomer s ON i.SupplierCustomerId = s.Id ")
              .append("WHERE i.DocumentTypeId = 57 ")
              .append("AND (i.OrganizationId = ").append(orgId).append(" OR i.OrganizationId IS NULL) ")
              .append("AND (i.CompanyId = ").append(compId).append(" OR i.CompanyId IS NULL) ");

            if (supplierId != null && supplierId > 0) {
                sb.append("AND i.SupplierCustomerId = ").append(supplierId).append(" ");
            }
            if (fromDocNo != null && fromDocNo > 0) {
                sb.append("AND i.DocNo >= ").append(fromDocNo).append(" ");
            }
            if (toDocNo != null && toDocNo > 0) {
                sb.append("AND i.DocNo <= ").append(toDocNo).append(" ");
            }
            if (fromDate != null && !fromDate.trim().isEmpty()) {
                String col = "entrydate".equalsIgnoreCase(dateType) ? "i.EntryDate" : "i.DocDate";
                sb.append("AND ").append(col).append(" >= '").append(fromDate).append("' ");
            }
            if (toDate != null && !toDate.trim().isEmpty()) {
                String col = "entrydate".equalsIgnoreCase(dateType) ? "i.EntryDate" : "i.DocDate";
                sb.append("AND ").append(col).append(" <= '").append(toDate).append(" 23:59:59' ");
            }

            sb.append("ORDER BY i.DocNo DESC");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public Map<String, Object> getById(int id) {
        try {
            String sqlHead = "SELECT i.*, s.CompanyName as supplierName FROM InvPurchaseInvoice i " +
                    "LEFT JOIN SupplierCustomer s ON i.SupplierCustomerId = s.Id WHERE i.Id = ?";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sqlHead, id);
            if (list == null || list.isEmpty()) return null;

            Map<String, Object> result = new HashMap<>(list.get(0));

            String sqlDetails = "SELECT d.*, it.ItemName, it.ItemCode, w.WareHouseName, jl.JobLotDescription " +
                    "FROM InvPurchaseInvoiceDetail d " +
                    "LEFT JOIN Item it ON d.ItemId = it.Id " +
                    "LEFT JOIN InvWareHouse w ON d.WarehouseId = w.Id " +
                    "LEFT JOIN JobLot jl ON d.JobLotId = jl.Id " +
                    "WHERE d.InvPurchaseInvoiceId = ?";
            result.put("details", jdbcTemplate.queryForList(sqlDetails, id));

            return result;
        } catch (Exception e) {
            return null;
        }
    }

    @Transactional
    public Map<String, Object> saveDirectInvoice(Map<String, Object> payload) {
        Map<String, Object> response = new HashMap<>();
        try {
            Integer id = payload.get("id") != null ? ((Number) payload.get("id")).intValue() : 0;
            Integer orgId = payload.get("organizationId") != null ? ((Number) payload.get("organizationId")).intValue() : 1;
            Integer compId = payload.get("companyId") != null ? ((Number) payload.get("companyId")).intValue() : 1;
            Integer branchId = payload.get("branchesId") != null ? ((Number) payload.get("branchesId")).intValue() : 1;
            Integer yearId = payload.get("financialYearId") != null ? ((Number) payload.get("financialYearId")).intValue() : 1;

            Integer docNo = payload.get("docNo") != null ? ((Number) payload.get("docNo")).intValue() : generateNextDocNo(orgId, compId, branchId, yearId);
            String docDate = payload.get("docDate") != null ? payload.get("docDate").toString() : new java.text.SimpleDateFormat("yyyy-MM-dd").format(new Date());
            Integer supplierId = payload.get("supplierCustomerId") != null ? ((Number) payload.get("supplierCustomerId")).intValue() : 0;
            Double billAmount = payload.get("billAmount") != null ? ((Number) payload.get("billAmount")).doubleValue() : 0.0;
            String remarks = payload.get("remarksHeader") != null ? payload.get("remarksHeader").toString() : "";
            String manualBillNo = payload.get("manualBillNo") != null ? payload.get("manualBillNo").toString() : "";

            if (id > 0) {
                String updateHead = "UPDATE InvPurchaseInvoice SET DocNo=?, DocDate=?, SupplierCustomerId=?, BillAmount=?, RemarksHeader=?, ManualBillNo=?, ModifyDate=GETDATE() WHERE Id=?";
                jdbcTemplate.update(updateHead, docNo, docDate, supplierId, billAmount, remarks, manualBillNo, id);
                jdbcTemplate.update("DELETE FROM InvPurchaseInvoiceDetail WHERE InvPurchaseInvoiceId=?", id);
            } else {
                String insertHead = "INSERT INTO InvPurchaseInvoice (DocumentTypeId, DocNo, DocDate, SupplierCustomerId, BillAmount, RemarksHeader, ManualBillNo, OrganizationId, CompanyId, BranchesId, FinancialYearId, EntryDate, ModifyDate) " +
                        "VALUES (57, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, GETDATE(), GETDATE())";
                jdbcTemplate.update(insertHead, docNo, docDate, supplierId, billAmount, remarks, manualBillNo, orgId, compId, branchId, yearId);
                id = jdbcTemplate.queryForObject("SELECT @@IDENTITY", Integer.class);
            }

            @SuppressWarnings("unchecked")
            List<Map<String, Object>> details = (List<Map<String, Object>>) payload.get("details");
            if (details != null) {
                String insertDetail = "INSERT INTO InvPurchaseInvoiceDetail (InvPurchaseInvoiceId, ItemId, ItemQty, GrossWeight, NetBillWeight, StockWeight, ItemRate, ItemAmount, BillAmount, WarehouseId, JobLotId, PackingTypeId, CropYear) " +
                        "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                for (Map<String, Object> d : details) {
                    jdbcTemplate.update(insertDetail,
                            id,
                            d.get("itemId") != null ? ((Number) d.get("itemId")).intValue() : 0,
                            d.get("itemQty") != null ? ((Number) d.get("itemQty")).doubleValue() : 0.0,
                            d.get("grossWeight") != null ? ((Number) d.get("grossWeight")).doubleValue() : 0.0,
                            d.get("netBillWeight") != null ? ((Number) d.get("netBillWeight")).doubleValue() : 0.0,
                            d.get("stockWeight") != null ? ((Number) d.get("stockWeight")).doubleValue() : 0.0,
                            d.get("itemRate") != null ? ((Number) d.get("itemRate")).doubleValue() : 0.0,
                            d.get("itemAmount") != null ? ((Number) d.get("itemAmount")).doubleValue() : 0.0,
                            d.get("billAmount") != null ? ((Number) d.get("billAmount")).doubleValue() : 0.0,
                            d.get("warehouseId") != null ? ((Number) d.get("warehouseId")).intValue() : 0,
                            d.get("jobLotId") != null ? ((Number) d.get("jobLotId")).intValue() : 0,
                            d.get("packingTypeId") != null ? ((Number) d.get("packingTypeId")).intValue() : 0,
                            d.get("cropYear") != null ? d.get("cropYear").toString() : ""
                    );
                }
            }

            response.put("success", true);
            response.put("id", id);
            response.put("docNo", docNo);
            response.put("message", "Purchase Direct Invoice saved successfully [" + docNo + "]");
        } catch (Exception e) {
            response.put("success", false);
            response.put("message", "Error saving Purchase Direct Invoice: " + e.getMessage());
        }
        return response;
    }

    @Transactional
    public boolean deleteDirectInvoice(int id) {
        try {
            jdbcTemplate.update("DELETE FROM InvPurchaseInvoiceDetail WHERE InvPurchaseInvoiceId=?", id);
            jdbcTemplate.update("DELETE FROM InvPurchaseInvoice WHERE Id=? AND DocumentTypeId=57", id);
            return true;
        } catch (Exception e) {
            return false;
        }
    }
}
