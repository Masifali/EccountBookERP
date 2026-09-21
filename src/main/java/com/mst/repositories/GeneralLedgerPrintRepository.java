package com.mst.repositories;

import java.sql.Date;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** Single-account print contracts from Architecture.BLL.Reports.Accounts.VoucherReports. */
@Repository
public class GeneralLedgerPrintRepository {
    public enum Format {
        STANDARD("105", "105-AcRptGeneralLedger.rpt", "Sp_Accounts_GeneralLedger_Rpt"),
        OFFSET("106", "106-AcRptGeneralLedgerB.rpt", "Sp_Accounts_GeneralLedger_Rpt"),
        QUANTITATIVE("107", "107-AcRptQuantativeLedger.rpt", "Sp_Accounts_GeneralLedger_Rpt"),
        SUMMARY_I("108", "108-AcRptGeneralLedgerSummery.rpt", "Sp_VouchersAccountsGeneralLedgerSummery"),
        SUMMARY_II("105A", "105A-GeneralLedgerSummary2.rpt", "SpAccounts_GeneralLedger2Format_Rpt");

        public final String code;
        public final String crystalFile;
        private final String procedure;
        Format(String code, String crystalFile, String procedure) {
            this.code = code; this.crystalFile = crystalFile; this.procedure = procedure;
        }
        public static Format fromCode(String code) {
            for (Format format : values()) if (format.code.equals(code)) return format;
            throw new IllegalArgumentException("Unknown General Ledger print format");
        }
    }

    private final JdbcTemplate jdbc;
    public GeneralLedgerPrintRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String, Object>> load(Format format, int organizationId, int companyId,
            int financialYearId, int userId, int accountId, LocalDate from, LocalDate to,
            boolean includeUnposted, Integer languageId) {
        if (organizationId <= 0 || companyId <= 0 || userId <= 0 || accountId <= 0
                || (format != Format.SUMMARY_II && financialYearId <= 0)
                || from == null || to == null || from.isAfter(to)) {
            throw new IllegalArgumentException("Valid accounting context, account and date range are required");
        }
        StringBuilder sql = new StringBuilder("EXEC dbo.").append(format.procedure);
        List<Object> values = new ArrayList<>();
        if (format != Format.SUMMARY_II) add(sql, values, "FinancialYearId", financialYearId);
        add(sql, values, "OrganizationId", organizationId);
        add(sql, values, "CompanyId", companyId);
        add(sql, values, format == Format.SUMMARY_I ? "VoucherDateF" : "FromDate", Date.valueOf(from));
        add(sql, values, format == Format.SUMMARY_I ? "VoucherDateT"
                : format == Format.SUMMARY_II ? "ToDate" : "EndDate", Date.valueOf(to));
        add(sql, values, "AccountId", accountId);
        if (!includeUnposted) add(sql, values, "IsApproved", true);
        if (format == Format.STANDARD || format == Format.OFFSET || format == Format.QUANTITATIVE) {
            add(sql, values, "EntryUser", userId);
        }
        if (languageId != null && languageId > 0) add(sql, values, "LanguageId", languageId);
        // Absent branch/subsidiary/cost-center selectors mean Desktop's zero/omitted parameters.
        // Keep returned decimals, row order, opening rows and summaries intact. Never use fallback SQL.
        return jdbc.queryForList(sql.toString(), values.toArray());
    }

    public int activeFinancialYear(int organizationId, int companyId) {
        List<Map<String, Object>> years = jdbc.queryForList(
                "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                organizationId, companyId);
        if (years.size() != 1 || !(years.get(0).get("Id") instanceof Number)) {
            throw new IllegalStateException("Select one active financial year before printing the ledger");
        }
        return ((Number) years.get(0).get("Id")).intValue();
    }

    private static void add(StringBuilder sql, List<Object> values, String name, Object value) {
        sql.append(values.isEmpty() ? " @" : ", @").append(name).append("=?");
        values.add(value);
    }
}
