package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class SaleReportService {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private CurrentUserContext currentUserContext;

    /**
     * Customer Receivables Aging Report
     */
    public List<Map<String, Object>> getCustomerReceivablesAgingReport(String asOnDate, Integer customerId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT coa.ID as customerId, coa.AccountCode as accountCode, coa.AccountTitle as partyName, ");
            sb.append("ISNULL(SUM(d.DebitAmount - d.CreditAmount), 0) as dueAmount ");
            sb.append("FROM VoucherDetail d ");
            sb.append("JOIN VoucherHead h ON d.VoucherHeadId = h.ID ");
            sb.append("JOIN ChartofAccount coa ON d.AccountId = coa.ID ");
            sb.append("WHERE coa.AccountTypeId IN (2, 7) ");
            sb.append("AND h.OrganizationId = ").append(orgId).append(" AND h.CompanyId = ").append(compId).append(" ");
            if (customerId != null && customerId > 0) {
                sb.append("AND coa.ID = ").append(customerId).append(" ");
            }
            if (asOnDate != null && !asOnDate.isBlank()) {
                sb.append("AND h.VoucherDate <= '").append(asOnDate).append(" 23:59:59' ");
            }
            sb.append("GROUP BY coa.ID, coa.AccountCode, coa.AccountTitle ");
            sb.append("HAVING SUM(d.DebitAmount - d.CreditAmount) <> 0 ");
            sb.append("ORDER BY coa.AccountTitle ASC");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /**
     * Sale Invoice Register Report
     */
    public List<Map<String, Object>> getSaleInvoiceRegisterReport(String fromDate, String toDate, Integer customerId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT s.ID as id, s.DocNo as docNo, s.DocDate as docDate, ");
            sb.append("c.AccountTitle as customerName, s.TotalAmount as totalAmount, ");
            sb.append("s.IsApproved as isApproved ");
            sb.append("FROM SaleInvoiceHead s ");
            sb.append("LEFT JOIN ChartofAccount c ON s.CustomerId = c.ID ");
            sb.append("WHERE s.OrganizationId = ").append(orgId).append(" AND s.CompanyId = ").append(compId).append(" ");
            if (customerId != null && customerId > 0) {
                sb.append("AND s.CustomerId = ").append(customerId).append(" ");
            }
            if (fromDate != null && !fromDate.isBlank()) {
                sb.append("AND s.DocDate >= '").append(fromDate).append(" 00:00:00' ");
            }
            if (toDate != null && !toDate.isBlank()) {
                sb.append("AND s.DocDate <= '").append(toDate).append(" 23:59:59' ");
            }
            sb.append("ORDER BY s.DocDate DESC, s.DocNo DESC");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }
}
