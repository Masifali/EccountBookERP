package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class PurchaseReportService {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private CurrentUserContext currentUserContext;

    /**
     * Supplier Aging Document-Wise Report (ditto WinForms frmPayablesAgingDocumentWise.cs)
     * Real proc: VoucherReports.SupplierAgingDocumentWise -> Sp_Accounts_SupplierAgingDocumentWise_Rpt
     */
    public List<Map<String, Object>> getSupplierAgingDocumentWiseReport(String asOnDate, Integer agingDays,
            Integer accountId, Integer customGroupId, Integer customerGroupId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();

        StringBuilder sql = new StringBuilder("EXEC Sp_Accounts_SupplierAgingDocumentWise_Rpt @OrganizationId=?, @CompanyId=?");
        List<Object> params = new ArrayList<>();
        params.add(orgId);
        params.add(compId);

        if (asOnDate != null && !asOnDate.isBlank()) { sql.append(", @AsOnDate=?"); params.add(asOnDate); }
        if (agingDays != null && agingDays > 0) { sql.append(", @AgingDays=?"); params.add(agingDays); }
        if (accountId != null && accountId > 0) { sql.append(", @AccountId=?"); params.add(accountId); }
        if (customGroupId != null && customGroupId > 0) { sql.append(", @CustomGroupId=?"); params.add(customGroupId); }
        if (customerGroupId != null && customerGroupId > 0) { sql.append(", @CustomerGroupId=?"); params.add(customerGroupId); }

        try {
            return jdbcTemplate.queryForList(sql.toString(), params.toArray());
        } catch (Exception e) {
            return getSupplierAgingFallback(asOnDate, accountId);
        }
    }

    private List<Map<String, Object>> getSupplierAgingFallback(String asOnDate, Integer accountId) {
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT h.ID as Id, h.DocumentTypeId, dt.DocumentTypeDescription as DocumentType, ");
            sb.append("h.VoucherCode as InvoiceNo, h.VoucherDate as InvoiceDate, h.DueDate as InvoiceDueDate, ");
            sb.append("coa.AccountCode, coa.AccountTitle as PartyName, ");
            sb.append("ISNULL(d.CreditAmount, 0) - ISNULL(d.DebitAmount, 0) as DueAmount, ");
            sb.append("d.Comments as Remarks ");
            sb.append("FROM VoucherDetail d ");
            sb.append("JOIN VoucherHead h ON d.VoucherHeadId = h.ID ");
            sb.append("JOIN ChartofAccount coa ON d.AccountId = coa.ID ");
            sb.append("LEFT JOIN DocumentType dt ON h.DocumentTypeId = dt.ID ");
            sb.append("WHERE coa.AccountTypeId IN (3, 8) ");
            if (accountId != null && accountId > 0) {
                sb.append("AND coa.ID = ").append(accountId).append(" ");
            }
            if (asOnDate != null && !asOnDate.isBlank()) {
                sb.append("AND h.VoucherDate <= '").append(asOnDate).append(" 23:59:59' ");
            }
            sb.append("ORDER BY coa.AccountTitle, h.VoucherDate DESC");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception ex) {
            return Collections.emptyList();
        }
    }

    /**
     * Purchase Orders Register Report
     */
    public List<Map<String, Object>> getPurchaseOrderRegisterReport(String fromDate, String toDate, Integer supplierId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT po.ID as id, po.DocNo as docNo, po.DocDate as docDate, ");
            sb.append("s.AccountTitle as supplierName, po.TotalAmount as totalAmount, ");
            sb.append("po.IsApproved as isApproved ");
            sb.append("FROM PurchaseOrderHead po ");
            sb.append("LEFT JOIN ChartofAccount s ON po.SupplierId = s.ID ");
            sb.append("WHERE po.OrganizationId = ").append(orgId).append(" AND po.CompanyId = ").append(compId).append(" ");
            if (supplierId != null && supplierId > 0) {
                sb.append("AND po.SupplierId = ").append(supplierId).append(" ");
            }
            if (fromDate != null && !fromDate.isBlank()) {
                sb.append("AND po.DocDate >= '").append(fromDate).append(" 00:00:00' ");
            }
            if (toDate != null && !toDate.isBlank()) {
                sb.append("AND po.DocDate <= '").append(toDate).append(" 23:59:59' ");
            }
            sb.append("ORDER BY po.DocDate DESC, po.DocNo DESC");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /**
     * Goods Receipt Notes (GRN) Register Report
     */
    public List<Map<String, Object>> getGrnRegisterReport(String fromDate, String toDate, Integer supplierId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT g.ID as id, g.DocNo as docNo, g.DocDate as docDate, ");
            sb.append("s.AccountTitle as supplierName, g.TotalAmount as totalAmount, ");
            sb.append("g.IsApproved as isApproved ");
            sb.append("FROM GRNHead g ");
            sb.append("LEFT JOIN ChartofAccount s ON g.SupplierId = s.ID ");
            sb.append("WHERE g.OrganizationId = ").append(orgId).append(" AND g.CompanyId = ").append(compId).append(" ");
            if (supplierId != null && supplierId > 0) {
                sb.append("AND g.SupplierId = ").append(supplierId).append(" ");
            }
            if (fromDate != null && !fromDate.isBlank()) {
                sb.append("AND g.DocDate >= '").append(fromDate).append(" 00:00:00' ");
            }
            if (toDate != null && !toDate.isBlank()) {
                sb.append("AND g.DocDate <= '").append(toDate).append(" 23:59:59' ");
            }
            sb.append("ORDER BY g.DocDate DESC, g.DocNo DESC");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /**
     * Inward Gate Pass Register Report (ditto WinForms frmGatePassReport.cs)
     */
    public List<Map<String, Object>> getGatePassRegisterReport(String fromDate, String toDate, Integer supplierId, String status) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT g.ID as id, g.GpSrNo as gpSrNo, g.GpDate as gpDate, ");
            sb.append("s.AccountTitle as supplierName, g.VehicleNo as vehicleNo, ");
            sb.append("i.ItemName as itemName, g.TotalBags as itemQty, ");
            sb.append("g.SupplierWeight as supplierWeight, g.FactoryWeight as factoryWeight, ");
            sb.append("(ISNULL(g.FactoryWeight, 0) - ISNULL(g.SupplierWeight, 0)) as differenceWeight, ");
            sb.append("g.Status as status ");
            sb.append("FROM GatePassInward g ");
            sb.append("LEFT JOIN ChartofAccount s ON g.SupplierId = s.ID ");
            sb.append("LEFT JOIN InvItemItem i ON g.ItemId = i.ID ");
            sb.append("WHERE g.OrganizationId = ").append(orgId).append(" AND g.CompanyId = ").append(compId).append(" ");
            if (supplierId != null && supplierId > 0) {
                sb.append("AND g.SupplierId = ").append(supplierId).append(" ");
            }
            if (status != null && !status.isBlank()) {
                sb.append("AND g.Status = '").append(status).append("' ");
            }
            if (fromDate != null && !fromDate.isBlank()) {
                sb.append("AND g.GpDate >= '").append(fromDate).append(" 00:00:00' ");
            }
            if (toDate != null && !toDate.isBlank()) {
                sb.append("AND g.GpDate <= '").append(toDate).append(" 23:59:59' ");
            }
            sb.append("ORDER BY g.GpDate DESC, g.GpSrNo DESC");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }
}
