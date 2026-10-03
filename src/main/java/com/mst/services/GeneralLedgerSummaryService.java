package com.mst.services;

import com.mst.models.GeneralLedgerSummaryRequest;
import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.Date;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

@Service
public class GeneralLedgerSummaryService {
    private final JdbcTemplate jdbcTemplate;
    private final CurrentUserContext currentUserContext;

    public GeneralLedgerSummaryService(JdbcTemplate jdbcTemplate, CurrentUserContext currentUserContext) {
        this.jdbcTemplate = jdbcTemplate;
        this.currentUserContext = currentUserContext;
    }

    /** Summary-I / Print-108: keep the procedure's rows, decimals and running balances intact. */
    public List<Map<String, Object>> getGeneralLedgerSummary(GeneralLedgerSummaryRequest request) {
        StringBuilder sql = new StringBuilder(
                "EXEC dbo.Sp_VouchersAccountsGeneralLedgerSummery "
                + "@FinancialYearId=?, @OrganizationId=?, @CompanyId=?, "
                + "@AccountId=?, @VoucherDateF=?, @VoucherDateT=?");
        List<Object> parameters = new ArrayList<>();
        parameters.add(currentUserContext.currentFinancialYearId());
        parameters.add(currentUserContext.currentOrganizationId());
        parameters.add(currentUserContext.currentCompanyId());
        parameters.add(request.getAccountId());
        parameters.add(Date.valueOf(request.getFromDate()));
        parameters.add(Date.valueOf(request.getToDate()));

        // Leaving @IsApproved out includes both posted and unposted vouchers.
        if (!request.isIncludeUnposted()) {
            sql.append(", @IsApproved=?");
            parameters.add(true);
        }
        if (request.getBranchId() != null && request.getBranchId() > 0) {
            sql.append(", @BranchesId=?");
            parameters.add(request.getBranchId());
        }
        if (request.getSubsidiaryAccountId() != null && request.getSubsidiaryAccountId() > 0) {
            sql.append(", @SubsidiaryAccountId=?");
            parameters.add(request.getSubsidiaryAccountId());
            if (request.getSubsidiaryTypeId() != null && request.getSubsidiaryTypeId() > 0) {
                sql.append(", @SubsidiaryTypeId=?");
                parameters.add(request.getSubsidiaryTypeId());
            }
        }
        if (request.getCostCenterId() != null && request.getCostCenterId() > 0) {
            sql.append(", @CostCenterId=?");
            parameters.add(request.getCostCenterId());
        }
        if (request.getLanguageId() != null && request.getLanguageId() > 0) {
            sql.append(", @LanguageId=?");
            parameters.add(request.getLanguageId());
        }

        // Put a breakpoint here to inspect the SQL, parameter values and returned rows.
        return jdbcTemplate.queryForList(sql.toString(), parameters.toArray());
    }
}
