package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

@Service
public class PurchaseInvoiceReturnService {

    @Autowired private com.mst.repositories.PurchaseInvoiceRecordRepository records;

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private com.mst.repositories.PurchaseInvoiceNumberingRepository numbering;

    public Map<String, Object> getDropdowns(int orgId, int compId) {
        Map<String, Object> result = new HashMap<>();

        // Suppliers
        try {
            String sql = "SELECT Id as id, CompanyName as name, ISNULL(PartyCode, ManualPartyCode) as code, GlAccountId as glAccountId " +
                    "FROM SupplierCustomer WHERE (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL) " +
                    "ORDER BY CompanyName";
            result.put("suppliers", jdbcTemplate.queryForList(sql, orgId, compId));
        } catch (Exception e) {
            result.put("suppliers", Collections.emptyList());
        }

        // Items
        try {
            String sql = "SELECT Id as id, ItemName as name, ItemCode as code FROM Item WHERE (ItemStatus IS NULL OR ItemStatus = 1) ORDER BY ItemName";
            result.put("items", jdbcTemplate.queryForList(sql));
        } catch (Exception e) {
            result.put("items", Collections.emptyList());
        }

        // Warehouses
        try {
            String sql = "SELECT Id as id, WareHouseName as name FROM InvWareHouse WHERE (IsActive IS NULL OR IsActive = 1) ORDER BY WareHouseName";
            result.put("warehouses", jdbcTemplate.queryForList(sql));
        } catch (Exception e) {
            result.put("warehouses", Collections.emptyList());
        }

        // Crop Years
        try {
            String sql = "SELECT Id as id, CropYear as name FROM CropYear ORDER BY CropYear DESC";
            result.put("cropYears", jdbcTemplate.queryForList(sql));
        } catch (Exception e) {
            result.put("cropYears", Collections.emptyList());
        }

        // Job Lots
        try {
            String sql = "SELECT Id as id, JobLotDescription as name FROM JobLot ORDER BY JobLotDescription";
            result.put("jobLots", jdbcTemplate.queryForList(sql));
        } catch (Exception e) {
            result.put("jobLots", Collections.emptyList());
        }

        // Packing Types
        try {
            String sql = "SELECT Id as id, PackTypeDesc as name FROM InvPackingType WHERE (IsActive IS NULL OR IsActive = 1) ORDER BY PackTypeDesc";
            result.put("packingTypes", jdbcTemplate.queryForList(sql));
        } catch (Exception e) {
            result.put("packingTypes", Collections.emptyList());
        }

        return result;
    }

    public int generateNextDocNo(int orgId, int compId, int branchId, int yearId) {
        return numbering.next(orgId, compId, yearId, 59);
    }

    public List<Map<String, Object>> getHistory(int orgId, int compId, int branchId, int yearId,
                                                String fromDate, String toDate, Integer supplierId,
                                                Integer fromDocNo, Integer toDocNo, String dateType) {
        return records.history(59,fromDate,toDate,supplierId,fromDocNo,toDocNo,dateType);
    }

    public Map<String, Object> getById(int id) {
        return records.load(id,59);
    }

    @Transactional
    public Map<String, Object> savePurchaseReturn(Map<String, Object> payload) {
        Map<String, Object> response = new HashMap<>();
        try {
            records.scopeWrite(payload,59);
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
                        "VALUES (59, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, GETDATE(), GETDATE())";
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
            response.put("message", "Purchase Invoice Return saved successfully [" + docNo + "]");
        } catch (Exception e) {
            org.springframework.transaction.interceptor.TransactionAspectSupport.currentTransactionStatus().setRollbackOnly();
            response.put("success", false);
            response.put("message", "Error saving Purchase Invoice Return: " + e.getMessage());
        }
        return response;
    }

    @Transactional
    public boolean deletePurchaseReturn(int id) {
        records.delete(id,59);
        return true;
    }
}
