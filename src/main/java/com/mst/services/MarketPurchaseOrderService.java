package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

@Service
public class MarketPurchaseOrderService {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    public Map<String, Object> getDropdowns(int orgId, int compId) {
        Map<String, Object> result = new HashMap<>();

        try {
            String sql = "SELECT Id as id, CompanyName as name, ISNULL(PartyCode, ManualPartyCode) as code " +
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
            String sql = "SELECT Id as id, CropYear as name FROM CropYear ORDER BY CropYear DESC";
            result.put("cropYears", jdbcTemplate.queryForList(sql));
        } catch (Exception e) {
            result.put("cropYears", Collections.emptyList());
        }

        try {
            String sql = "SELECT Id as id, CityName as name FROM City WHERE (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL) ORDER BY CityName";
            result.put("cities", jdbcTemplate.queryForList(sql, orgId, compId));
        } catch (Exception e) {
            result.put("cities", Collections.emptyList());
        }

        return result;
    }

    public int generateNextDocNo(int orgId, int compId, int branchId, int yearId) {
        try {
            String sql = "SELECT ISNULL(MAX(DocNo), 0) + 1 FROM PurchaseOrder WHERE DocumentTypeId = 41 AND (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL)";
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
            sb.append("SELECT p.Id as id, p.DocNo as docNo, CONVERT(VARCHAR(10), p.DocDate, 120) as docDate, ")
              .append("s.CompanyName as supplierName, p.OrderStatus as orderStatus, ")
              .append("p.RemarksHeader as remarks, CONVERT(VARCHAR(10), p.EntryDate, 120) as entryDate ")
              .append("FROM PurchaseOrder p ")
              .append("LEFT JOIN SupplierCustomer s ON p.OrderSupCustId = s.Id ")
              .append("WHERE p.DocumentTypeId = 41 ")
              .append("AND (p.OrganizationId = ").append(orgId).append(" OR p.OrganizationId IS NULL) ")
              .append("AND (p.CompanyId = ").append(compId).append(" OR p.CompanyId IS NULL) ");

            if (supplierId != null && supplierId > 0) {
                sb.append("AND p.OrderSupCustId = ").append(supplierId).append(" ");
            }
            if (fromDocNo != null && fromDocNo > 0) {
                sb.append("AND p.DocNo >= ").append(fromDocNo).append(" ");
            }
            if (toDocNo != null && toDocNo > 0) {
                sb.append("AND p.DocNo <= ").append(toDocNo).append(" ");
            }
            if (fromDate != null && !fromDate.trim().isEmpty()) {
                String col = "entrydate".equalsIgnoreCase(dateType) ? "p.EntryDate" : "p.DocDate";
                sb.append("AND ").append(col).append(" >= '").append(fromDate).append("' ");
            }
            if (toDate != null && !toDate.trim().isEmpty()) {
                String col = "entrydate".equalsIgnoreCase(dateType) ? "p.EntryDate" : "p.DocDate";
                sb.append("AND ").append(col).append(" <= '").append(toDate).append(" 23:59:59' ");
            }

            sb.append("ORDER BY p.DocNo DESC");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public Map<String, Object> getById(int id) {
        try {
            String sqlHead = "SELECT p.*, s.CompanyName as supplierName FROM PurchaseOrder p " +
                    "LEFT JOIN SupplierCustomer s ON p.OrderSupCustId = s.Id WHERE p.Id = ?";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sqlHead, id);
            if (list == null || list.isEmpty()) return null;

            Map<String, Object> result = new HashMap<>(list.get(0));

            String sqlDetails = "SELECT d.*, it.ItemName, it.ItemCode " +
                    "FROM PurchaseOrderDetail d " +
                    "LEFT JOIN Item it ON d.OrderItemId = it.Id " +
                    "WHERE d.PurchaseOrderId = ?";
            result.put("details", jdbcTemplate.queryForList(sqlDetails, id));

            return result;
        } catch (Exception e) {
            return null;
        }
    }

    @Transactional
    public Map<String, Object> saveMarketPurchaseOrder(Map<String, Object> payload) {
        Map<String, Object> response = new HashMap<>();
        try {
            Integer id = payload.get("id") != null ? ((Number) payload.get("id")).intValue() : 0;
            Integer orgId = payload.get("organizationId") != null ? ((Number) payload.get("organizationId")).intValue() : 1;
            Integer compId = payload.get("companyId") != null ? ((Number) payload.get("companyId")).intValue() : 1;
            Integer branchId = payload.get("branchesId") != null ? ((Number) payload.get("branchesId")).intValue() : 1;
            Integer yearId = payload.get("financialYearId") != null ? ((Number) payload.get("financialYearId")).intValue() : 1;

            Integer docNo = payload.get("docNo") != null ? ((Number) payload.get("docNo")).intValue() : generateNextDocNo(orgId, compId, branchId, yearId);
            String docDate = payload.get("docDate") != null ? payload.get("docDate").toString() : new java.text.SimpleDateFormat("yyyy-MM-dd").format(new Date());
            Integer supplierId = payload.get("orderSupCustId") != null ? ((Number) payload.get("orderSupCustId")).intValue() : 0;
            String remarks = payload.get("remarksHeader") != null ? payload.get("remarksHeader").toString() : "";
            String status = payload.get("orderStatus") != null ? payload.get("orderStatus").toString() : "Open";

            if (id > 0) {
                String updateHead = "UPDATE PurchaseOrder SET DocNo=?, DocDate=?, OrderSupCustId=?, RemarksHeader=?, OrderStatus=?, ModifyDate=GETDATE() WHERE Id=?";
                jdbcTemplate.update(updateHead, docNo, docDate, supplierId, remarks, status, id);
                jdbcTemplate.update("DELETE FROM PurchaseOrderDetail WHERE PurchaseOrderId=?", id);
            } else {
                String insertHead = "INSERT INTO PurchaseOrder (DocumentTypeId, DocNo, DocDate, OrderSupCustId, RemarksHeader, OrderStatus, OrganizationId, CompanyId, BranchesId, FinancialYearId, EntryDate, ModifyDate) " +
                        "VALUES (41, ?, ?, ?, ?, ?, ?, ?, ?, ?, GETDATE(), GETDATE())";
                jdbcTemplate.update(insertHead, docNo, docDate, supplierId, remarks, status, orgId, compId, branchId, yearId);
                id = jdbcTemplate.queryForObject("SELECT @@IDENTITY", Integer.class);
            }

            @SuppressWarnings("unchecked")
            List<Map<String, Object>> details = (List<Map<String, Object>>) payload.get("details");
            if (details != null) {
                String insertDetail = "INSERT INTO PurchaseOrderDetail (PurchaseOrderId, OrderItemId, OrderItemQty, NetWeight, OrderItemRate, Crop, OrderRemarks) " +
                        "VALUES (?, ?, ?, ?, ?, ?, ?)";
                for (Map<String, Object> d : details) {
                    jdbcTemplate.update(insertDetail,
                            id,
                            d.get("orderItemId") != null ? ((Number) d.get("orderItemId")).intValue() : 0,
                            d.get("orderItemQty") != null ? ((Number) d.get("orderItemQty")).doubleValue() : 0.0,
                            d.get("netWeight") != null ? ((Number) d.get("netWeight")).doubleValue() : 0.0,
                            d.get("orderItemRate") != null ? ((Number) d.get("orderItemRate")).doubleValue() : 0.0,
                            d.get("crop") != null ? d.get("crop").toString() : "",
                            d.get("orderRemarks") != null ? d.get("orderRemarks").toString() : ""
                    );
                }
            }

            response.put("success", true);
            response.put("id", id);
            response.put("docNo", docNo);
            response.put("message", "Market Purchase Order saved successfully [" + docNo + "]");
        } catch (Exception e) {
            response.put("success", false);
            response.put("message", "Error saving Market Purchase Order: " + e.getMessage());
        }
        return response;
    }

    @Transactional
    public boolean deleteMarketPurchaseOrder(int id) {
        try {
            jdbcTemplate.update("DELETE FROM PurchaseOrderDetail WHERE PurchaseOrderId=?", id);
            jdbcTemplate.update("DELETE FROM PurchaseOrder WHERE Id=? AND DocumentTypeId=41", id);
            return true;
        } catch (Exception e) {
            return false;
        }
    }
}
