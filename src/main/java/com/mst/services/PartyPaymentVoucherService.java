package com.mst.services;

import com.mst.models.dto.PartyPaymentLineItemDto;
import com.mst.models.dto.PartyPaymentVoucherDto;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.*;

@Service
public class PartyPaymentVoucherService {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private CurrentUserContext currentUserContext;

    public int generateNextVoucherCode() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int finYearId = currentUserContext.currentFinancialYearId();
        try {
            List<Map<String, Object>> list = jdbcTemplate.queryForList(
                "EXEC Sp_Vouchers_GetMethods @Activity=?, @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @FinancialYearId=?, @BranchesId=?",
                "GenerateVoucherCodeByDocumentTypeId", orgId, compId, 35, finYearId, branchId
            );
            if (list != null && !list.isEmpty()) {
                Map<String, Object> row = list.get(0);
                Object val = row.get("VoucherCode");
                if (val == null) val = row.get("VoucherNo");
                if (val == null && !row.values().isEmpty()) val = row.values().iterator().next();
                if (val != null) {
                    return Integer.parseInt(val.toString());
                }
            }
        } catch (Exception e) {
            try {
                String sql = "SELECT ISNULL(MAX(CAST(VoucherCode AS INT)), 0) + 1 FROM VoucherHead WHERE DocumentTypeId = 35 AND OrganizationId = ? AND CompanyId = ?";
                Integer code = jdbcTemplate.queryForObject(sql, Integer.class, orgId, compId);
                return (code != null && code > 0) ? code : 1;
            } catch (Exception ex) {
                return 1;
            }
        }
        return 1;
    }

    public List<Map<String, Object>> getDrAccounts() {
        return getDrAccounts(null);
    }

    public List<Map<String, Object>> getDrAccounts(String query) {
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
                             "AND C.AccountTypeId IN (2, 15, 3, 6, 8) " +
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
                             "AND C.AccountTypeId IN (2, 15, 3, 6, 8) " +
                             "ORDER BY C.AccountTitle ASC";
                return jdbcTemplate.queryForList(sql, orgId, compId);
            }
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getCrAccounts(String query) {
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
                             "AND C.AccountTypeId IN (2, 15, 3, 6, 8) " +
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
                             "AND C.AccountTypeId IN (2, 15, 3, 6, 8) " +
                             "ORDER BY C.AccountTitle ASC";
                return jdbcTemplate.queryForList(sql, orgId, compId);
            }
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

    public Map<String, Object> saveVoucher(PartyPaymentVoucherDto dto) {
        Map<String, Object> response = new HashMap<>();
        try {
            if (dto.getRefAccountId() == null || dto.getRefAccountId() <= 0) {
                throw new IllegalArgumentException("Debit Account (Account Title Dr) is required.");
            }
            if (dto.getLineItems() == null || dto.getLineItems().isEmpty()) {
                throw new IllegalArgumentException("At least one transaction line item is required.");
            }

            int voucherCode = dto.getVoucherCode() != null && dto.getVoucherCode() > 0 ? dto.getVoucherCode() : generateNextVoucherCode();
            String vDate = dto.getVoucherDate() != null && !dto.getVoucherDate().isEmpty() ? dto.getVoucherDate() : LocalDate.now().toString();

            BigDecimal grandTotal = BigDecimal.ZERO;
            for (PartyPaymentLineItemDto item : dto.getLineItems()) {
                if (item.getAmount() != null) {
                    grandTotal = grandTotal.add(item.getAmount());
                }
            }

            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            int finYearId = currentUserContext.currentFinancialYearId();
            int userId = currentUserContext.currentUserId();

            // Insert into VoucherHead
            String headSql = "INSERT INTO VoucherHead (" +
                    "DocumentTypeId, VoucherCode, VoucherDate, RefAccountId, Remarks, VoucherAmount, " +
                    "OrganizationId, CompanyId, FinancialYearId, EntryUser, EntryDate, IsApproved, MultiCurrencyId, ExchangeCurrencyRate" +
                    ") VALUES (35, ?, ?, ?, ?, ?, ?, ?, ?, ?, GETDATE(), 1, ?, ?)";

            jdbcTemplate.update(headSql,
                    voucherCode,
                    vDate,
                    dto.getRefAccountId(),
                    dto.getRemarks() != null ? dto.getRemarks() : "",
                    grandTotal.doubleValue(),
                    orgId,
                    compId,
                    finYearId,
                    userId,
                    dto.getMultiCurrencyId() != null ? dto.getMultiCurrencyId() : 0,
                    dto.getExchangeRate() != null ? dto.getExchangeRate() : 1.0
            );

            // Get inserted VoucherHead ID
            Integer voucherHeadId = jdbcTemplate.queryForObject("SELECT @@IDENTITY", Integer.class);

            // Insert VoucherDetail records matching desktop frmPartyPaymentVoucher.cs two-line accounting logic
            int lineNo = 1;
            String detailSql = "INSERT INTO VoucherDetail (" +
                    "VoucherHeadId, LineId, AccountId, AgainstAccountId, DebitAmount, CreditAmount, " +
                    "Comments, JobLotId, CheqNoDetail, DCheqDate" +
                    ") VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            for (PartyPaymentLineItemDto item : dto.getLineItems()) {
                double amt = item.getAmount() != null ? item.getAmount().doubleValue() : 0.0;
                String remarks = item.getRemarks() != null ? item.getRemarks() : "";
                Integer jobLot = item.getJobLotId() != null ? item.getJobLotId() : 0;
                String chqNo = item.getChequeNo() != null ? item.getChequeNo() : "";
                String chqDate = item.getChequeDate() != null && !item.getChequeDate().isEmpty() ? item.getChequeDate() : null;

                // 1. Debit Row for RefAccountId (Dr Account)
                jdbcTemplate.update(detailSql,
                        voucherHeadId,
                        lineNo++,
                        dto.getRefAccountId(),
                        item.getAccountId(),
                        amt,
                        0.0,
                        remarks,
                        jobLot,
                        chqNo,
                        chqDate
                );

                // 2. Credit Row for item accountId (Cr Account)
                jdbcTemplate.update(detailSql,
                        voucherHeadId,
                        lineNo++,
                        item.getAccountId(),
                        dto.getRefAccountId(),
                        0.0,
                        amt,
                        remarks,
                        jobLot,
                        chqNo,
                        chqDate
                );
            }

            response.put("success", true);
            response.put("voucherHeadId", voucherHeadId);
            response.put("voucherCode", voucherCode);
            response.put("voucherCodeDisplay", String.format("PPV-%s-%04d", vDate.length() >= 4 ? vDate.substring(0, 4) : "2026", voucherCode));
            response.put("message", "Party Payment Voucher saved successfully (Code: " + voucherCode + ")");
        } catch (Exception e) {
            response.put("success", false);
            response.put("message", "Error saving voucher: " + e.getMessage());
        }
        return response;
    }

    public Map<String, Object> getVoucherById(Integer voucherHeadId) {
        try {
            String headSql = "SELECT h.Id as voucherHeadId, h.VoucherCode as voucherCode, h.VoucherDate as voucherDate, " +
                    "h.RefAccountId as refAccountId, h.Remarks as remarks, " +
                    "coa.AccountCode as refAccountCode, coa.AccountTitle as refAccountTitle, coa.AccountTypeId as refAccountTypeId " +
                    "FROM VoucherHead h " +
                    "LEFT JOIN ChartofAccount coa ON h.RefAccountId = coa.Id " +
                    "WHERE h.Id = ? AND h.DocumentTypeId = 35";
            List<Map<String, Object>> headList = jdbcTemplate.queryForList(headSql, voucherHeadId);
            if (headList == null || headList.isEmpty()) {
                return null;
            }
            Map<String, Object> head = new HashMap<>(headList.get(0));

            String detailSql = "SELECT d.Id as lineId, d.AccountId as accountId, d.CreditAmount as amount, " +
                    "d.Comments as remarks, d.JobLotId as jobLotId, jl.JobLotDescription as jobLotDescription, " +
                    "d.CheqNoDetail as chequeNo, CONVERT(VARCHAR(10), d.DCheqDate, 120) as chequeDate, " +
                    "coa.AccountCode as accountCode, coa.AccountTitle as accountTitle, coa.AccountTypeId as accountTypeId " +
                    "FROM VoucherDetail d " +
                    "LEFT JOIN ChartofAccount coa ON d.AccountId = coa.Id " +
                    "LEFT JOIN JobLot jl ON d.JobLotId = jl.Id " +
                    "WHERE d.VoucherHeadId = ? AND d.CreditAmount > 0 " +
                    "ORDER BY d.LineId";
            List<Map<String, Object>> lineItems = jdbcTemplate.queryForList(detailSql, voucherHeadId);
            head.put("lineItems", lineItems);
            return head;
        } catch (Exception e) {
            return null;
        }
    }
}

