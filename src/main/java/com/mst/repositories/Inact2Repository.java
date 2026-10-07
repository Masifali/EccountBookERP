package com.mst.repositories;

import com.mst.models.UserAccount;
import java.math.BigDecimal;
import java.sql.Date;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.ColumnMapRowMapper;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;

/**
 * Inactive Account_Reports group K (screens 54 / 55 / 57 / 58 / 59). Each method is the desktop BLL call (VoucherReports.cs) with the
 * same procedure and the same guarded parameter set: a parameter the BLL only adds "if (x != 0)" is OMITTED when unset.
 *   54 GeneralLedgerStatment            GeneralLedgerStatement -> Sp_GeneralLedgerStatement_Rpt (VoucherReports :1237)
 *   55 AccountsBalanceSheetStandardRpt  AccountsBalanceSheetStandardRpt -> Sp_Accounts_BalanceSheetStandard_Rpt (VoucherReports :2419)
 *                                       AccountsProfitLoassStandard     -> Sp_Accounts_ProfitLoassStandard_Rpt  (:2701)
 *                                       AccountsBalanceSheetStandardFormatII -> SpAccounts_BalanceSheetFormatA_Report (:2456)
 *                                       ProfitandLoss                   -> SpAccounts_ProfitLoassFormatA_Report (:2746)
 *   57 GeneralJournalSummeryRegister    GeneralJournalReport -> Sp_Accounts_GeneralJournal_SummeryRegister_Rpt (:2380)
 *   58 PayablesAging                    LedgerAging          -> SpAccounts_LedgerAging (:1933)
 *   59 ReceivableAging                  ReceivableAging      -> Sp_Accounts_ReceivablesAging_Rpt (:1456)
 * The result is the FIRST result set, as SqlDataAdapter.Fill(DataTable) takes it.
 */
@Repository
public class Inact2Repository {
    private final JdbcTemplate jdbc;

    public Inact2Repository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ------------------------------------------------------------------ shared
    public int amountDecimals(UserAccount u) { return ReportValueSupport.amountDecimals(jdbc, u); }

    /** clsGlobalVariables.ActiveYr.Start_Period of the session's financial year, as a local yyyy-MM-dd (null when unknown). */
    public String yearStart(UserAccount u, int yearId) {
        List<Map<String, Object>> years = jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId());
        Map<String, Object> pick = null;
        for (Map<String, Object> y : years) {
            Object id = ci(y, "Id");
            if (id instanceof Number && ((Number) id).intValue() == yearId) { pick = y; break; }
        }
        if (pick == null && !years.isEmpty() && yearId <= 0) pick = years.get(0);
        if (pick == null) return null;
        Object start = ci(pick, "Start_Period");
        if (start instanceof Timestamp) return ((Timestamp) start).toLocalDateTime().toLocalDate().toString();
        if (start instanceof java.util.Date) return new Date(((java.util.Date) start).getTime()).toLocalDate().toString();
        return start == null ? null : String.valueOf(start).substring(0, Math.min(10, String.valueOf(start).length()));
    }

    /** 59 CompanyFill: Company.GetAll(OrgCompanyTypeId = organization) = Sp_Company_GetAllMethod ReadByOrganizationId; only Id + CompName are bound. */
    public List<Map<String, Object>> companies(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : norm(jdbc.queryForList("EXEC dbo.Sp_Company_GetAllMethod @OrgCompanyTypeId=?, @Activity='ReadByOrganizationId'", u.getOrganizationId()))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("CompName", ci(r, "CompName"));
            out.add(m);
        }
        return out;
    }


    // ------------------------------------------------------------------ 54
    /** GeneralLedgerStatment.BranchFill / ProjectFill: CommonServices.BrancheServiceBind = Sp_Branches_GetAllMethod GetAll (Id, BranchCode, BranchName, ProjectName kept). */
    public List<Map<String, Object>> branches(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : norm(jdbc.queryForList("EXEC dbo.Sp_Branches_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='GetAll'",
                u.getOrganizationId(), u.getCompanyId()))) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String k : new String[] { "Id", "BranchCode", "BranchName", "ProjectName" }) if (r.containsKey(k) || ci(r, k) != null) m.put(k, ci(r, k));
            out.add(m);
        }
        return out;
    }

    /** GeneralLedgerStatment.AccountTitleFill: CommonServices.CoaAllocationGetForComboServiceBind = Sp_COAAllocation_GetAllMethod COAForCombobindig (Id, AccountTitle kept). */
    public List<Map<String, Object>> coaForCombo(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : norm(jdbc.queryForList("EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @UserId=?, @Activity='COAForCombobindig'",
                u.getOrganizationId(), u.getCompanyId(), u.getId()))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("AccountTitle", ci(r, "AccountTitle"));
            out.add(m);
        }
        return out;
    }

    /** VoucherReports.GeneralLedgerStatement (:1237): FinancialYearId, Organization, Company, AccountId, ReportType always; VoucherDateF/T when set; ProjectsId / BranchesId only when non-zero. */
    public List<Map<String, Object>> glStatement(UserAccount u, int yearId, int accountId, LocalDate from, LocalDate to, int branchId, int projectId) {
        Sql s = new Sql("Sp_GeneralLedgerStatement_Rpt");
        s.p("FinancialYearId", yearId); s.p("OrganizationId", u.getOrganizationId()); s.p("CompanyId", u.getCompanyId());
        if (from != null) s.p("VoucherDateF", Date.valueOf(from));
        if (to != null) s.p("VoucherDateT", Date.valueOf(to));
        s.p("AccountId", accountId);
        s.opt("ProjectsId", projectId);
        s.opt("BranchesId", branchId);
        s.p("ReportType", "Account Statment By Date");
        return norm(first(s));
    }

    // ------------------------------------------------------------------ 55
    /** VoucherReports.AccountsBalanceSheetStandardRpt: FinancialYearId, OrganizationId, CompanyId always, ToDate when set. */
    public List<Map<String, Object>> balanceSheet(UserAccount u, int yearId, LocalDate toDate) {
        Sql s = new Sql("Sp_Accounts_BalanceSheetStandard_Rpt");
        s.p("OrganizationId", u.getOrganizationId()); s.p("CompanyId", u.getCompanyId()); s.p("FinancialYearId", yearId);
        if (toDate != null) s.p("ToDate", Date.valueOf(toDate));
        return norm(first(s));
    }

    /** VoucherReports.AccountsProfitLoassStandard: Organization, Company, FinancialYear always; FromDate / ToDate when set. */
    public List<Map<String, Object>> profitLoss(UserAccount u, int yearId, LocalDate from, LocalDate to) {
        Sql s = new Sql("Sp_Accounts_ProfitLoassStandard_Rpt");
        s.p("OrganizationId", u.getOrganizationId()); s.p("CompanyId", u.getCompanyId()); s.p("FinancialYearId", yearId);
        if (from != null) s.p("FromDate", Date.valueOf(from));
        if (to != null) s.p("ToDate", Date.valueOf(to));
        return norm(first(s));
    }

    /** VoucherReports.AccountsBalanceSheetStandardFormatII: Organization, Company, UserId always; FromDate / ToDate when set (AccountNoteId 0, BranchesIds "" are never sent). */
    public List<Map<String, Object>> balanceSheetFormat2(UserAccount u, LocalDate from, LocalDate to) {
        Sql s = new Sql("SpAccounts_BalanceSheetFormatA_Report");
        s.p("OrganizationId", u.getOrganizationId()); s.p("CompanyId", u.getCompanyId()); s.p("UserId", u.getId());
        if (from != null) s.p("FromDate", Date.valueOf(from));
        if (to != null) s.p("ToDate", Date.valueOf(to));
        return norm(first(s));
    }

    /** VoucherReports.ProfitandLoss: Organization, Company always; ToDate when set (the form leaves FromDate unset, so it is not sent). */
    public List<Map<String, Object>> profitAndLoss(UserAccount u, LocalDate to) {
        Sql s = new Sql("SpAccounts_ProfitLoassFormatA_Report");
        s.p("OrganizationId", u.getOrganizationId()); s.p("CompanyId", u.getCompanyId());
        if (to != null) s.p("ToDate", Date.valueOf(to));
        return norm(first(s));
    }

    // ------------------------------------------------------------------ 57
    private static final String[] JOURNAL_COLUMNS = { "RegisterType", "V_Type", "V_No", "VoucherDate", "RefAccountTitle", "VoucherAmount", "Remarks",
            "ChequeNo", "AgainstAccountTitle", "EntryUser", "EntryDate", "Is_Approved", "PostDate" };

    /**
     * VoucherReports.GeneralJournalReport: OrganizationId and CompanyId only - the BLL adds @DateFrom / @DateTo under
     * "if (Conversion.CheckDateTimeNull(date))", i.e. only for a null date, so a real From / To is never sent (the procedure then returns everything).
     * GridBind reads dtGrd.Rows[i]["RegisterType"], ["V_Type"], ...: when the procedure's result lacks one of those columns the .NET
     * DataRow indexer throws "Column 'X' does not belong to table ." for the first row, and that text is what the desktop shows.
     */
    public List<Map<String, Object>> generalJournal(UserAccount u) {
        Sql s = new Sql("Sp_Accounts_GeneralJournal_SummeryRegister_Rpt");
        s.p("OrganizationId", u.getOrganizationId()); s.p("CompanyId", u.getCompanyId());
        return jdbc.execute(s.text.toString(), (PreparedStatementCallback<List<Map<String, Object>>>) st -> {
            for (int i = 0; i < s.args.size(); i++) st.setObject(i + 1, s.args.get(i));
            boolean result = st.execute();
            while (true) {
                if (result) {
                    try (ResultSet rs = st.getResultSet()) {
                        List<Map<String, Object>> rows = new ArrayList<>();
                        if (!rs.next()) return rows;
                        ResultSetMetaData md = rs.getMetaData();
                        Set<String> have = new HashSet<>();
                        for (int c = 1; c <= md.getColumnCount(); c++) have.add(md.getColumnLabel(c).toLowerCase());
                        for (String need : JOURNAL_COLUMNS) {
                            if (!have.contains(need.toLowerCase())) throw new IllegalStateException("Column '" + need + "' does not belong to table .");
                        }
                        ColumnMapRowMapper mapper = new ColumnMapRowMapper();
                        do { rows.add(mapper.mapRow(rs, rows.size())); } while (rs.next());
                        return norm(rows);
                    }
                } else if (st.getUpdateCount() == -1) return new ArrayList<>();
                result = st.getMoreResults();
            }
        });
    }

    // ------------------------------------------------------------------ 58
    /** VoucherReports.LedgerAging: Organization, Company, UserId always; AsOnDate, AgingDays, NoOfInternal, ActionId, AccouuntClassId, SkipZero only when set / non-zero (IsApproved never: ApprovedFilter = "All"). */
    public List<Map<String, Object>> ledgerAging(UserAccount u, LocalDate endDate, int agingDays, int intervalDays, int actionId, int classId, int skipZero) {
        Sql s = new Sql("SpAccounts_LedgerAging");
        s.p("OrganizationId", u.getOrganizationId()); s.p("CompanyId", u.getCompanyId()); s.p("UserId", u.getId());
        if (endDate != null) s.p("AsOnDate", Date.valueOf(endDate));
        s.opt("AgingDays", agingDays);
        s.opt("NoOfInternal", intervalDays);
        s.opt("ActionId", actionId);
        s.opt("AccouuntClassId", classId);
        s.opt("SkipZero", skipZero);
        return norm(first(s));
    }

    // ------------------------------------------------------------------ 59
    /** VoucherReports.ReceivableAging: FinancialYearId, Organization, Company, UserId always; IntervalDays when non-zero; EndDate when set. */
    public List<Map<String, Object>> receivableAging(UserAccount u, int yearId, LocalDate endDate, int intervalDays) {
        Sql s = new Sql("Sp_Accounts_ReceivablesAging_Rpt");
        s.p("FinancialYearId", yearId); s.p("OrganizationId", u.getOrganizationId()); s.p("CompanyId", u.getCompanyId()); s.p("UserId", u.getId());
        s.opt("IntervalDays", intervalDays);
        if (endDate != null) s.p("EndDate", Date.valueOf(endDate));
        return norm(first(s));
    }

    // ------------------------------------------------------------------ plumbing
    private static final class Sql {
        final StringBuilder text;
        final List<Object> args = new ArrayList<>();
        boolean any;
        Sql(String proc) { text = new StringBuilder("EXEC dbo.").append(proc).append(' '); }
        void p(String name, Object v) { text.append(any ? ", " : "").append('@').append(name).append("=?"); any = true; args.add(v); }
        void opt(String name, int v) { if (v != 0) p(name, v); }
    }

    private List<Map<String, Object>> first(Sql s) {
        return jdbc.execute(s.text.toString(), (PreparedStatementCallback<List<Map<String, Object>>>) st -> {
            for (int i = 0; i < s.args.size(); i++) st.setObject(i + 1, s.args.get(i));
            boolean result = st.execute();
            while (true) {
                if (result) {
                    try (ResultSet rs = st.getResultSet()) {
                        List<Map<String, Object>> rows = new ArrayList<>();
                        ColumnMapRowMapper mapper = new ColumnMapRowMapper();
                        while (rs.next()) rows.add(mapper.mapRow(rs, rows.size()));
                        return rows;
                    }
                } else if (st.getUpdateCount() == -1) return new ArrayList<>();
                result = st.getMoreResults();
            }
        });
    }

    /** Numbers as doubles (Conversion.ToDouble), dates as local ISO text (a Timestamp would serialize as a UTC instant and can shift a day). */
    private static List<Map<String, Object>> norm(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof BigDecimal) v = ((BigDecimal) v).doubleValue();
                else if (v instanceof Timestamp) v = ((Timestamp) v).toLocalDateTime().toString();
                else if (v instanceof Date) v = ((Date) v).toLocalDate().toString();
                else if (v instanceof java.util.Date) v = new Date(((java.util.Date) v).getTime()).toLocalDate().toString();
                m.put(e.getKey(), v);
            }
            out.add(m);
        }
        return out;
    }

    private static Object ci(Map<String, Object> row, String key) {
        if (row == null) return null;
        if (row.containsKey(key)) return row.get(key);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }
}
