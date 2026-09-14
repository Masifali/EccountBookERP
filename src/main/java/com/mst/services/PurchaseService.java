package com.mst.services;

import com.mst.models.dto.PurchaseLineItemDto;
import com.mst.models.dto.PurchaseTransactionDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.*;

@Service
public class PurchaseService {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    public int generateNextDocNo(int documentTypeId) {
        try {
            String sql = "SELECT ISNULL(MAX(VoucherCode), 0) + 1 FROM VoucherHead WHERE DocumentTypeId = ?";
            Integer code = jdbcTemplate.queryForObject(sql, Integer.class, documentTypeId);
            return (code != null && code > 0) ? code : 1;
        } catch (Exception e) {
            return 1;
        }
    }

    public List<Map<String, Object>> getSuppliers(String query) {
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT Id as id, ISNULL(SupCustCode, ManualPartyCode) as supplierCode, CompanyName as supplierName, GlAccountId as glAccountId ");
            sb.append("FROM SupplierCustomer ");
            sb.append("WHERE (Status IS NULL OR Status = 1) ");
            if (query != null && !query.trim().isEmpty()) {
                String q = query.trim().replace("'", "''");
                sb.append("AND (CompanyName LIKE '%").append(q).append("%' OR SupCustCode LIKE '%").append(q).append("%' OR ManualPartyCode LIKE '%").append(q).append("%') ");
            }
            sb.append("ORDER BY CompanyName");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getItems(String query) {
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT Id as id, ItemCode as itemCode, ItemName as itemName, ISNULL(PurchasePrice, 0) as purchasePrice, ")
              .append("BaseUnitId as baseUnitId, PurchaseGLAC as purchaseGlAc ")
              .append("FROM Item ")
              .append("WHERE (ItemStatus IS NULL OR ItemStatus = 1) ");
            if (query != null && !query.trim().isEmpty()) {
                String q = query.trim().replace("'", "''");
                sb.append("AND (ItemName LIKE '%").append(q).append("%' OR ItemCode LIKE '%").append(q).append("%') ");
            }
            sb.append("ORDER BY ItemName");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getWarehouses() {
        try {
            String sql = "SELECT Id as id, WareHouseName as warehouseName FROM InvWareHouse WHERE (IsActive IS NULL OR IsActive = 1) ORDER BY WareHouseName";
            return jdbcTemplate.queryForList(sql);
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

    public List<Map<String, Object>> getPurchaseTransactionsByDocType(int documentTypeId) {
        try {
            String sql = "SELECT h.Id as id, h.VoucherCode as docNo, CONVERT(VARCHAR(10), h.VoucherDate, 120) as docDate, " +
                    "s.CompanyName as supplierName, h.VoucherAmount as netTotal, h.Remarks as remarks " +
                    "FROM VoucherHead h " +
                    "LEFT JOIN SupplierCustomer s ON h.RefAccountId = s.GlAccountId " +
                    "WHERE h.DocumentTypeId = ? " +
                    "ORDER BY h.VoucherCode DESC";
            return jdbcTemplate.queryForList(sql, documentTypeId);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public Map<String, Object> savePurchaseTransaction(PurchaseTransactionDto dto) {
        Map<String, Object> response = new HashMap<>();
        try {
            if (dto.getDocumentTypeId() == null || dto.getDocumentTypeId() <= 0) {
                throw new IllegalArgumentException("Document Type ID is required.");
            }
            if (dto.getLineItems() == null || dto.getLineItems().isEmpty()) {
                throw new IllegalArgumentException("At least one transaction item is required.");
            }

            int docNo = dto.getDocNo() != null && dto.getDocNo() > 0 ? dto.getDocNo() : generateNextDocNo(dto.getDocumentTypeId());
            String vDate = dto.getDocDate() != null && !dto.getDocDate().isEmpty() ? dto.getDocDate() : LocalDate.now().toString();

            BigDecimal netTotal = dto.getNetTotal() != null ? dto.getNetTotal() : BigDecimal.ZERO;
            if (netTotal.compareTo(BigDecimal.ZERO) <= 0) {
                netTotal = BigDecimal.ZERO;
                for (PurchaseLineItemDto item : dto.getLineItems()) {
                    if (item.getAmount() != null) {
                        netTotal = netTotal.add(item.getAmount());
                    }
                }
            }

            Integer refAccId = dto.getSupplierId();
            if (refAccId != null && refAccId > 0) {
                try {
                    String glSql = "SELECT GlAccountId FROM SupplierCustomer WHERE Id = ?";
                    Integer glId = jdbcTemplate.queryForObject(glSql, Integer.class, refAccId);
                    if (glId != null && glId > 0) {
                        refAccId = glId;
                    }
                } catch (Exception ignored) {}
            }

            if (dto.getId() != null && dto.getId() > 0) {
                // Update header
                String updateHead = "UPDATE VoucherHead SET " +
                        "VoucherCode = ?, VoucherDate = ?, RefAccountId = ?, Remarks = ?, VoucherAmount = ? " +
                        "WHERE Id = ?";
                jdbcTemplate.update(updateHead, docNo, vDate, refAccId != null ? refAccId : 0, dto.getRemarks() != null ? dto.getRemarks() : "", netTotal.doubleValue(), dto.getId());
                
                // Delete previous details
                jdbcTemplate.update("DELETE FROM VoucherDetail WHERE VoucherHeadId = ?", dto.getId());
            } else {
                // Insert header
                String insertHead = "INSERT INTO VoucherHead (" +
                        "DocumentTypeId, VoucherCode, VoucherDate, RefAccountId, Remarks, VoucherAmount, " +
                        "OrganizationId, CompanyId, FinancialYearId, EntryUser, EntryDate, IsApproved" +
                        ") VALUES (?, ?, ?, ?, ?, ?, 1, 1, 1, 1, GETDATE(), 1)";

                jdbcTemplate.update(insertHead,
                        dto.getDocumentTypeId(),
                        docNo,
                        vDate,
                        refAccId != null ? refAccId : 0,
                        dto.getRemarks() != null ? dto.getRemarks() : "",
                        netTotal.doubleValue()
                );
            }

            Integer voucherHeadId = dto.getId() != null && dto.getId() > 0 ? dto.getId() : jdbcTemplate.queryForObject("SELECT @@IDENTITY", Integer.class);

            int lineNo = 1;
            String detailSql = "INSERT INTO VoucherDetail (" +
                    "VoucherHeadId, LineId, ItemId, AccountId, AgainstAccountId, DebitAmount, CreditAmount, " +
                    "Comments, JobLotId" +
                    ") VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

            for (PurchaseLineItemDto item : dto.getLineItems()) {
                double amt = item.getAmount() != null ? item.getAmount().doubleValue() : 0.0;
                String remarks = item.getRemarks() != null ? item.getRemarks() : "";
                Integer jobLot = item.getJobLotId() != null ? item.getJobLotId() : 0;
                Integer itemId = item.getItemId() != null ? item.getItemId() : 0;

                Integer lineAccountId = null;
                if (itemId != null && itemId > 0) {
                    try {
                        String itemGlSql = "SELECT PurchaseGLAC FROM Item WHERE Id = ?";
                        lineAccountId = jdbcTemplate.queryForObject(itemGlSql, Integer.class, itemId);
                    } catch (Exception ignored) {}
                }
                if (lineAccountId == null || lineAccountId <= 0) {
                    lineAccountId = (refAccId != null && refAccId > 0) ? refAccId : 1;
                }

                jdbcTemplate.update(detailSql,
                        voucherHeadId,
                        lineNo++,
                        itemId,
                        lineAccountId,
                        refAccId != null ? refAccId : 0,
                        amt,
                        0.0,
                        remarks,
                        jobLot
                );
            }

            response.put("success", true);
            response.put("id", voucherHeadId);
            response.put("docNo", docNo);
            response.put("docNoDisplay", String.format("DOC-%04d", docNo));
            response.put("message", "Purchase Transaction saved successfully (Doc No: " + docNo + ")");
        } catch (Exception e) {
            response.put("success", false);
            response.put("message", "Error saving purchase transaction: " + e.getMessage());
        }
        return response;
    }

    public Map<String, Object> getPurchaseTransactionById(Integer id) {
        try {
            String headSql = "SELECT h.Id as id, h.DocumentTypeId as documentTypeId, h.VoucherCode as docNo, " +
                    "CONVERT(VARCHAR(10), h.VoucherDate, 120) as docDate, h.RefAccountId as supplierId, h.Remarks as remarks, " +
                    "h.VoucherAmount as netTotal, s.CompanyName as supplierName, ISNULL(s.SupCustCode, s.ManualPartyCode) as supplierCode " +
                    "FROM VoucherHead h " +
                    "LEFT JOIN SupplierCustomer s ON h.RefAccountId = s.GlAccountId " +
                    "WHERE h.Id = ?";
            List<Map<String, Object>> headList = jdbcTemplate.queryForList(headSql, id);
            if (headList == null || headList.isEmpty()) {
                return null;
            }
            Map<String, Object> head = new HashMap<>(headList.get(0));

            String detailSql = "SELECT d.Id as lineId, d.ItemId as itemId, i.ItemCode as itemCode, i.ItemName as itemName, " +
                    "d.DebitAmount as amount, d.Comments as remarks, d.JobLotId as jobLotId, jl.JobLotDescription as jobLotCode " +
                    "FROM VoucherDetail d " +
                    "LEFT JOIN Item i ON d.ItemId = i.Id " +
                    "LEFT JOIN JobLot jl ON d.JobLotId = jl.Id " +
                    "WHERE d.VoucherHeadId = ? AND d.DebitAmount > 0 " +
                    "ORDER BY d.LineId";
            List<Map<String, Object>> lineItems = jdbcTemplate.queryForList(detailSql, id);
            head.put("lineItems", lineItems);
            return head;
        } catch (Exception e) {
            return null;
        }
    }

    public boolean deletePurchaseTransaction(Integer id) {
        try {
            jdbcTemplate.update("DELETE FROM VoucherDetail WHERE VoucherHeadId = ?", id);
            jdbcTemplate.update("DELETE FROM VoucherHead WHERE Id = ?", id);
            return true;
        } catch (Exception e) {
            return false;
        }
    }

    public List<Map<String, Object>> getTaxAccounts() {
        try {
            String sql = "SELECT Id as id, AccountCode as accountCode, AccountTitle as accountTitle FROM ChartofAccount WHERE AccountTitle LIKE '%TAX%' OR AccountCode LIKE '2%' ORDER BY AccountCode";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
            if (list != null && !list.isEmpty()) return list;
        } catch (Exception ignored) {}
        return getAccountsList();
    }

    public List<Map<String, Object>> getDiscountAccounts() {
        try {
            String sql = "SELECT Id as id, AccountCode as accountCode, AccountTitle as accountTitle FROM ChartofAccount WHERE AccountTitle LIKE '%DISCOUNT%' OR AccountTitle LIKE '%MAINTENANCE%' OR AccountTitle LIKE '%EXPENSE%' ORDER BY AccountCode";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
            if (list != null && !list.isEmpty()) return list;
        } catch (Exception ignored) {}
        return getAccountsList();
    }

    public List<Map<String, Object>> getAccountsList() {
        try {
            String sql = "SELECT Id as id, AccountCode as accountCode, AccountTitle as accountTitle FROM ChartofAccount ORDER BY AccountCode";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getRacks() {
        try {
            String sql = "SELECT Id as id, RackName as rackName FROM InvRack ORDER BY RackName";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public Map<String, Object> saveStorePurchaseInvoice(Map<String, Object> req) {
        Map<String, Object> res = new HashMap<>();
        try {
            Integer docNo = req.get("docNo") != null ? Integer.parseInt(req.get("docNo").toString()) : generateNextDocNo(61);
            Integer branchSrNo = req.get("branchSrNo") != null ? Integer.parseInt(req.get("branchSrNo").toString()) : 1;
            Integer salesTaxNo = req.get("salesTaxNo") != null ? Integer.parseInt(req.get("salesTaxNo").toString()) : 1;
            Integer supplierCustomerId = req.get("supplierCustomerId") != null ? Integer.parseInt(req.get("supplierCustomerId").toString()) : 0;
            String docDate = req.get("docDate") != null ? req.get("docDate").toString() : LocalDate.now().toString();
            String manualBillNo = req.get("manualBillNo") != null ? req.get("manualBillNo").toString() : "";
            String remarksHeader = req.get("remarksHeader") != null ? req.get("remarksHeader").toString() : "";
            Double billAmount = req.get("billAmount") != null ? Double.parseDouble(req.get("billAmount").toString()) : 0.0;

            String insertHead = "INSERT INTO VoucherHead (" +
                    "DocumentTypeId, VoucherCode, VoucherDate, RefAccountId, Remarks, VoucherAmount, " +
                    "OrganizationId, CompanyId, FinancialYearId, EntryUser, EntryDate, IsApproved, ManualBillNo" +
                    ") VALUES (61, ?, ?, ?, ?, ?, 1, 1, 1, 1, GETDATE(), 1, ?)";

            jdbcTemplate.update(insertHead, docNo, docDate, supplierCustomerId, remarksHeader, billAmount, manualBillNo);
            Integer headId = jdbcTemplate.queryForObject("SELECT @@IDENTITY", Integer.class);

            res.put("success", true);
            res.put("id", headId);
            res.put("docNo", docNo);
            res.put("message", "Purchase Invoice Store Management saved successfully (Doc No: " + docNo + ")");
        } catch (Exception e) {
            res.put("success", false);
            res.put("message", "Error saving Purchase Invoice Store Management: " + e.getMessage());
        }
        return res;
    }

    public List<Map<String, Object>> getStorePurchaseInvoiceHistory(Map<String, Object> req) {
        try {
            String fromDate = req.get("fromDate") != null ? req.get("fromDate").toString() : null;
            String toDate = req.get("toDate") != null ? req.get("toDate").toString() : null;
            Integer suppId = req.get("supplierCustomerId") != null && !req.get("supplierCustomerId").toString().equals("0") ? Integer.parseInt(req.get("supplierCustomerId").toString()) : null;

            StringBuilder sb = new StringBuilder();
            sb.append("SELECT h.Id as id, h.VoucherCode as DocNo, 1 as BranchSrNo, CONVERT(VARCHAR(10), h.VoucherDate, 120) as DocDate, ");
            sb.append("s.CompanyName as CompanyName, h.ManualBillNo as ManualBillNo, h.VoucherAmount as BillAmount, h.Remarks as RemarksHeader ");
            sb.append("FROM VoucherHead h ");
            sb.append("LEFT JOIN SupplierCustomer s ON h.RefAccountId = s.Id OR h.RefAccountId = s.GlAccountId ");
            sb.append("WHERE h.DocumentTypeId = 61 ");
            if (fromDate != null && !fromDate.isBlank()) {
                sb.append("AND h.VoucherDate >= '").append(fromDate).append(" 00:00:00' ");
            }
            if (toDate != null && !toDate.isBlank()) {
                sb.append("AND h.VoucherDate <= '").append(toDate).append(" 23:59:59' ");
            }
            if (suppId != null && suppId > 0) {
                sb.append("AND (s.Id = ").append(suppId).append(" OR s.GlAccountId = ").append(suppId).append(") ");
            }
            sb.append("ORDER BY h.VoucherCode DESC");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }
}

