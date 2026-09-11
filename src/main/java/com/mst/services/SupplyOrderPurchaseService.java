package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

@Service
public class SupplyOrderPurchaseService {

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
            String sql = "SELECT p.Id as poHeaderId, p.DocNo as orderNo, s.CompanyName as supplierName " +
                    "FROM PurchaseOrder p LEFT JOIN SupplierCustomer s ON p.OrderSupCustId = s.Id " +
                    "WHERE (p.OrganizationId = ? OR p.OrganizationId IS NULL) AND (p.CompanyId = ? OR p.CompanyId IS NULL) " +
                    "ORDER BY p.DocNo DESC";
            result.put("purchaseOrders", jdbcTemplate.queryForList(sql, orgId, compId));
        } catch (Exception e) {
            result.put("purchaseOrders", Collections.emptyList());
        }

        return result;
    }

    public int generateNextDocNo(int orgId, int compId, int branchId, int yearId) {
        try {
            String sql = "SELECT ISNULL(MAX(DocNo), 0) + 1 FROM InvSupplyOrder WHERE DocumentTypeId = 104 AND (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL)";
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
            sb.append("SELECT s.Id as id, s.DocNo as docNo, CONVERT(VARCHAR(10), s.DocDate, 120) as docDate, ")
              .append("s.ManualRefNo as manualRefNo, s.VehicleNos as vehicleNo, s.BiltyNos as biltyNo, ")
              .append("s.RemarksHeader as remarks, CONVERT(VARCHAR(10), s.EntryDate, 120) as entryDate ")
              .append("FROM InvSupplyOrder s ")
              .append("WHERE s.DocumentTypeId = 104 ")
              .append("AND (s.OrganizationId = ").append(orgId).append(" OR s.OrganizationId IS NULL) ")
              .append("AND (s.CompanyId = ").append(compId).append(" OR s.CompanyId IS NULL) ");

            if (fromDocNo != null && fromDocNo > 0) {
                sb.append("AND s.DocNo >= ").append(fromDocNo).append(" ");
            }
            if (toDocNo != null && toDocNo > 0) {
                sb.append("AND s.DocNo <= ").append(toDocNo).append(" ");
            }
            if (fromDate != null && !fromDate.trim().isEmpty()) {
                String col = "entrydate".equalsIgnoreCase(dateType) ? "s.EntryDate" : "s.DocDate";
                sb.append("AND ").append(col).append(" >= '").append(fromDate).append("' ");
            }
            if (toDate != null && !toDate.trim().isEmpty()) {
                String col = "entrydate".equalsIgnoreCase(dateType) ? "s.EntryDate" : "s.DocDate";
                sb.append("AND ").append(col).append(" <= '").append(toDate).append(" 23:59:59' ");
            }

            sb.append("ORDER BY s.DocNo DESC");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public Map<String, Object> getById(int id) {
        try {
            String sqlHead = "SELECT s.* FROM InvSupplyOrder s WHERE s.Id = ?";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sqlHead, id);
            if (list == null || list.isEmpty()) return null;

            Map<String, Object> result = new HashMap<>(list.get(0));

            String sqlDetails = "SELECT d.*, it.ItemName, it.ItemCode, sc.CompanyName as supplierName " +
                    "FROM InvSupplyOrderDetail d " +
                    "LEFT JOIN Item it ON d.ItemId = it.Id " +
                    "LEFT JOIN SupplierCustomer sc ON d.SupplierCustomerId = sc.Id " +
                    "WHERE d.InvSupplyOrderId = ?";
            result.put("details", jdbcTemplate.queryForList(sqlDetails, id));

            return result;
        } catch (Exception e) {
            return null;
        }
    }

    @Transactional
    public Map<String, Object> saveSupplyOrder(Map<String, Object> payload) {
        Map<String, Object> response = new HashMap<>();
        try {
            Integer id = payload.get("id") != null ? ((Number) payload.get("id")).intValue() : 0;
            Integer orgId = payload.get("organizationId") != null ? ((Number) payload.get("organizationId")).intValue() : 1;
            Integer compId = payload.get("companyId") != null ? ((Number) payload.get("companyId")).intValue() : 1;
            Integer branchId = payload.get("branchesId") != null ? ((Number) payload.get("branchesId")).intValue() : 1;
            Integer yearId = payload.get("financialYearId") != null ? ((Number) payload.get("financialYearId")).intValue() : 1;

            Integer docNo = payload.get("docNo") != null ? ((Number) payload.get("docNo")).intValue() : generateNextDocNo(orgId, compId, branchId, yearId);
            String docDate = payload.get("docDate") != null ? payload.get("docDate").toString() : new java.text.SimpleDateFormat("yyyy-MM-dd").format(new Date());
            String manualRefNo = payload.get("manualRefNo") != null ? payload.get("manualRefNo").toString() : "";
            String vehicleNo = payload.get("vehicleNo") != null ? payload.get("vehicleNo").toString() : "";
            String biltyNo = payload.get("biltyNo") != null ? payload.get("biltyNo").toString() : "";
            String destinationLocation = payload.get("destinationLocation") != null ? payload.get("destinationLocation").toString() : "";
            String remarks = payload.get("remarksHeader") != null ? payload.get("remarksHeader").toString() : "";

            if (id > 0) {
                String updateHead = "UPDATE InvSupplyOrder SET DocNo=?, DocDate=?, ManualRefNo=?, VehicleNos=?, BiltyNos=?, DestinationsLocation=?, RemarksHeader=?, ModifyDate=GETDATE() WHERE Id=?";
                jdbcTemplate.update(updateHead, docNo, docDate, manualRefNo, vehicleNo, biltyNo, destinationLocation, remarks, id);
                jdbcTemplate.update("DELETE FROM InvSupplyOrderDetail WHERE InvSupplyOrderId=?", id);
            } else {
                String insertHead = "INSERT INTO InvSupplyOrder (DocumentTypeId, DocNo, DocDate, ManualRefNo, VehicleNos, BiltyNos, DestinationsLocation, RemarksHeader, OrganizationId, CompanyId, BranchesId, FinancialYearId, EntryDate, ModifyDate) " +
                        "VALUES (104, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, GETDATE(), GETDATE())";
                jdbcTemplate.update(insertHead, docNo, docDate, manualRefNo, vehicleNo, biltyNo, destinationLocation, remarks, orgId, compId, branchId, yearId);
                id = jdbcTemplate.queryForObject("SELECT @@IDENTITY", Integer.class);
            }

            @SuppressWarnings("unchecked")
            List<Map<String, Object>> details = (List<Map<String, Object>>) payload.get("details");
            if (details != null) {
                String insertDetail = "INSERT INTO InvSupplyOrderDetail (InvSupplyOrderId, PoHeaderId, PoDetailId, OrderNo, SupplierCustomerId, ItemId, Qty, NetWeight, GrossWeight, EbWtUnit, EbWtTotal, CropYear, RemarksDetail) " +
                        "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                for (Map<String, Object> d : details) {
                    jdbcTemplate.update(insertDetail,
                            id,
                            d.get("poHeaderId") != null ? ((Number) d.get("poHeaderId")).intValue() : 0,
                            d.get("poDetailId") != null ? ((Number) d.get("poDetailId")).intValue() : 0,
                            d.get("orderNo") != null ? ((Number) d.get("orderNo")).intValue() : 0,
                            d.get("supplierCustomerId") != null ? ((Number) d.get("supplierCustomerId")).intValue() : 0,
                            d.get("itemId") != null ? ((Number) d.get("itemId")).intValue() : 0,
                            d.get("qty") != null ? ((Number) d.get("qty")).doubleValue() : 0.0,
                            d.get("netWeight") != null ? ((Number) d.get("netWeight")).doubleValue() : 0.0,
                            d.get("grossWeight") != null ? ((Number) d.get("grossWeight")).doubleValue() : 0.0,
                            d.get("ebWtUnit") != null ? ((Number) d.get("ebWtUnit")).doubleValue() : 0.0,
                            d.get("ebWtTotal") != null ? ((Number) d.get("ebWtTotal")).doubleValue() : 0.0,
                            d.get("cropYear") != null ? d.get("cropYear").toString() : "",
                            d.get("remarksDetail") != null ? d.get("remarksDetail").toString() : ""
                    );
                }
            }

            response.put("success", true);
            response.put("id", id);
            response.put("docNo", docNo);
            response.put("message", "Supply Order Purchase saved successfully [" + docNo + "]");
        } catch (Exception e) {
            response.put("success", false);
            response.put("message", "Error saving Supply Order Purchase: " + e.getMessage());
        }
        return response;
    }

    @Transactional
    public boolean deleteSupplyOrder(int id) {
        try {
            jdbcTemplate.update("DELETE FROM InvSupplyOrderDetail WHERE InvSupplyOrderId=?", id);
            jdbcTemplate.update("DELETE FROM InvSupplyOrder WHERE Id=? AND DocumentTypeId=104", id);
            return true;
        } catch (Exception e) {
            return false;
        }
    }
}
