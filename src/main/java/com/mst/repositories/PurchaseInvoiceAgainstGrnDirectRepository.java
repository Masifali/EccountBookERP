package com.mst.repositories;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.*;

@Repository
public class PurchaseInvoiceAgainstGrnDirectRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    // Accounts filling for dropdowns
    public List<Map<String, Object>> getBrokerageAccounts() {
        try {
            String sql = "EXEC USP_CoaAllocationAccountTitleByAccountTypeIds @AccountTypeIds='11'";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            try {
                String sql = "SELECT Id as id, AccountTitle as name FROM ChartofAccount WHERE AccountTypeId = 11 ORDER BY AccountTitle";
                return jdbcTemplate.queryForList(sql);
            } catch (Exception e2) {
                return Collections.emptyList();
            }
        }
    }

    public List<Map<String, Object>> getFreightAccounts() {
        try {
            String sql = "EXEC USP_CoaAllocationAccountTitleByAccountTypeIds @AccountTypeIds='12'";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            try {
                String sql = "SELECT Id as id, AccountTitle as name FROM ChartofAccount WHERE AccountTypeId = 12 ORDER BY AccountTitle";
                return jdbcTemplate.queryForList(sql);
            } catch (Exception e2) {
                return Collections.emptyList();
            }
        }
    }

    public List<Map<String, Object>> getCreditAccountsForEmptyBags() {
        try {
            String sql = "EXEC USP_CoaAllocationAccountTitleByAccountTypeIds @AccountTypeIds='10'";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            try {
                String sql = "SELECT Id as id, AccountTitle as name FROM ChartofAccount WHERE AccountTypeId = 10 ORDER BY AccountTitle";
                return jdbcTemplate.queryForList(sql);
            } catch (Exception e2) {
                return Collections.emptyList();
            }
        }
    }

    // Load GRN details into Invoice
    public List<Map<String, Object>> loadGrnDirectForInvoice(String gdnIds) {
        try {
            String sql = "EXEC Sp_InvPurchaseInvoice_GetAllMethod @GdnIds=?, @Activity='DirectGrnLoadForPurchaseInvoiceDirect'";
            return jdbcTemplate.queryForList(sql, gdnIds);
        } catch (Exception e) {
            e.printStackTrace();
            return Collections.emptyList();
        }
    }

    // Load Empty Bags from GRN
    public List<Map<String, Object>> getEmptyBagsFromGrn(String gdnIds, Integer orderId) {
        try {
            String sql = "EXEC Sp_InvPurchaseInvoice_GetAllMethod @GdnIds=?, @OrderId=?, @Activity='GetEmptyBagsFromGrn'";
            return jdbcTemplate.queryForList(sql, gdnIds, orderId != null ? orderId : 0);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    // History Search
    public List<Map<String, Object>> getHistory(int orgId, int compId, Integer docTypeId, String fromDate, String toDate,
                                                Double fromDocNo, Double toDocNo, Integer supplierId) {
        try {
            StringBuilder sb = new StringBuilder("EXEC Sp_InvPurchaseInvoice_GetAllMethod ");
            sb.append("@OrganizationId=").append(orgId);
            sb.append(", @CompanyId=").append(compId);
            if (docTypeId != null && docTypeId > 0) sb.append(", @DocumentTypeId=").append(docTypeId);
            if (fromDate != null && !fromDate.isEmpty()) sb.append(", @FromDate='").append(fromDate).append("'");
            if (toDate != null && !toDate.isEmpty()) sb.append(", @ToDate='").append(toDate).append("'");
            if (fromDocNo != null && fromDocNo > 0) sb.append(", @FromDocNo=").append(fromDocNo);
            if (toDocNo != null && toDocNo > 0) sb.append(", @ToDocNo=").append(toDocNo);
            if (supplierId != null && supplierId > 0) sb.append(", @SupplierCustomerId=").append(supplierId);
            sb.append(", @Activity='FormHistory'");

            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            e.printStackTrace();
            return Collections.emptyList();
        }
    }

    // Read by ID
    public Map<String, Object> getById(int id) {
        try {
            String sql = "EXEC Sp_InvPurchaseInvoice_GetAllMethod @Id=?, @Activity='ReadById'";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, id);
            return (list != null && !list.isEmpty()) ? list.get(0) : null;
        } catch (Exception e) {
            return null;
        }
    }

    public List<Map<String, Object>> getDetailsByHeaderId(int id) {
        try {
            String sql = "EXEC Sp_InvPurchaseInvoiceDetail_GetAllMethod @InvPurchaseInvoiceId=?, @Activity='ReadByHeaderId'";
            return jdbcTemplate.queryForList(sql, id);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    // Delete record
    public void deleteRecord(int id, int orgId, int compId) {
        try {
            String sql = "EXEC Sp_InvPurchaseInvoice_Delete @Id=?, @OrganizationId=?, @CompanyId=?";
            jdbcTemplate.update(sql, id, orgId, compId);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
