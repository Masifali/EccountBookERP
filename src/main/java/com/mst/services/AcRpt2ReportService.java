package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import java.math.BigDecimal;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Service;

/**
 * R2 account reports (new, additive - nothing existing is touched). Tenancy always comes from
 * CurrentUserContext; every stored procedure and parameter set is the desktop form's own call.
 *
 *  72  FCYPayablesAndReceivablesRpt  - VoucherReports.GetMultiCurrencyAndLastRate
 *        (Sp_Vouchers_GetMethods @Activity='GetMultiCurrencyAndLastRate'), Accounts_getReceivablesAndPayables_Export
 *        (uspAccounts_getReceivablesAndPayables_Export), FCYPayablesAndReceivables_Rpt
 *        (SPU_Accounts_FCYPayablesAndReceivables_Rpt).
 *  75  DueDateAnalysisPayablesAndReceivablesForcast - GeneralReprots.DueDateAnalysisPayablesAndReceivablesForcast
 *        (usp_shorttermDueDateAnalysisPayablesAndReceivablesForcast, two result sets).
 *  77  PayablesandReceivablesAging - VoucherHead.PayablesandReceivablesMenagment (SpAccounts_PayablesReceivablesAging_Rpt),
 *        ChartofAccount.ReadAll3rdLevelAccountsForPayablesandReceeivablesAging, CommonServices.CustomeGroupsDefine(1),
 *        AccountsCustomGroups.GetAll.
 *  870 frmPayablesAndReceivablesWithPaymentAndReceipts - GeneralReprots.PayablesAndReceivablesWithPaymentAndReceipts
 *        (usp_getPayablesAndReceivablesWithPaymentAndReceipts) and the form's own lookups.
 */
@Service
public class AcRpt2ReportService {
    private static final Logger LOG = LoggerFactory.getLogger(AcRpt2ReportService.class);

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;

    public AcRpt2ReportService(JdbcTemplate jdbc, CurrentUserContext ctx) {
        this.jdbc = jdbc;
        this.ctx = ctx;
    }

    // ===================================================================================== shared helpers

    private UserAccount user() { return ctx.requireAccountingUser(); }

    private static LocalDate date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        return LocalDate.parse(s.trim().substring(0, 10));
    }

    private static java.sql.Date sqlDate(LocalDate d) { return d == null ? null : java.sql.Date.valueOf(d); }

    /** DECIMAL as exact text, dates as local yyyy-MM-dd (a UTC instant would shift a day). */
    private static List<Map<String, Object>> clean(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) o.put(e.getKey(), cell(e.getValue()));
            out.add(o);
        }
        return out;
    }

    private static Object cell(Object v) {
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        if (v instanceof Timestamp) {
            Timestamp t = (Timestamp) v;
            java.time.LocalDateTime l = t.toLocalDateTime();
            return l.toLocalTime().toSecondOfDay() == 0 && l.getNano() == 0 ? l.toLocalDate().toString() : l.toString().replace('T', ' ');
        }
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        return v;
    }

    private static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return (int) Math.round(Double.parseDouble(o.toString().trim())); } catch (NumberFormatException e) { return 0; }
    }

    /** CommonServices.GetConfigurationByOrgCompandConfigDescription(description) -> ConfigKey, or null. */
    private String config(UserAccount u, String description) {
        try {
            List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod",
                    DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                            "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
            if (rows.isEmpty() || rows.get(0).get("ConfigKey") == null) return null;
            return String.valueOf(rows.get(0).get("ConfigKey")).trim();
        } catch (Exception e) {
            LOG.warn("Configuration '{}' could not be read", description, e);
            return null;
        }
    }

    /** GetDecimalConfiguration(): 1..4 -> that many zeros in the format, anything else -> no decimals. */
    private int decimalPlaces(UserAccount u, String description) {
        int p = toInt(config(u, description));
        return p >= 1 && p <= 4 ? p : 0;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period = the session's financial year. */
    private String yearStart() {
        try {
            List<Map<String, Object>> rows = jdbc.queryForList("SELECT Start_Period FROM FinancialYear WHERE Id = ?", ctx.currentFinancialYearId());
            if (!rows.isEmpty()) {
                Object v = cell(rows.get(0).get("Start_Period"));
                return v == null ? null : String.valueOf(v).substring(0, 10);
            }
        } catch (Exception e) {
            LOG.warn("Financial year start could not be read", e);
        }
        return null;
    }

    private boolean feature(UserAccount u, int id) {
        try {
            for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                    DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
                if (toInt(r.get("Id")) == id) return true;
            }
        } catch (Exception e) {
            LOG.warn("ERP feature {} could not be read; treating as off", id, e);
        }
        return false;
    }

    /** CommonServices.CustomeGroupsDefine(1): Sp_AcLookUps_GetAllMethod @AcLookUpTypesId=1, @Activity='ReadAll'. */
    private List<Map<String, Object>> customGroups(UserAccount u) {
        return clean(DesktopProc.rows(jdbc, "Sp_AcLookUps_GetAllMethod", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "AcLookUpTypesId", 1, "Activity", "ReadAll")));
    }

    // ===================================================================================== 72 FCY

    /** VoucherReports.GetMultiCurrencyAndLastRate(obj{OrganizationId, CompanyId}). */
    public Map<String, Object> fcyLookups() {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> cur = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetMultiCurrencyAndLastRate"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("DMultiCurrencyId", toInt(r.get("DMultiCurrencyId")));
            o.put("CurrencyCode", r.get("CurrencyCode"));
            o.put("CurrencyName", r.get("CurrencyName"));
            o.put("LastExchRate", cell(r.get("LastExchRate")));
            cur.add(o);
        }
        out.put("currencies", cur);
        out.put("fcyDecimals", decimalPlaces(u, "DefaultNoOfDecimalPointsForFcyAmount"));
        out.put("yearStart", yearStart());
        return out;
    }

    /**
     * Accounts_getReceivablesAndPayables_Export: @OrganizationId, @CompanyId, @ToDate always; @FromDate only when the
     * From picker is checked; @BalanceCurrencyId = CmbCurrencySelection.Value (0 = not sent, the procedure then uses the
     * dollar currency).
     */
    public List<Map<String, Object>> fcyData(String from, String to, int currencyId) {
        UserAccount u = user();
        LocalDate t = date(to);
        if (t == null) throw new IllegalArgumentException("To date is required");
        return clean(DesktopProc.rows(jdbc, "uspAccounts_getReceivablesAndPayables_Export", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FromDate", sqlDate(date(from)), "ToDate", sqlDate(t),
                "BalanceCurrencyId", currencyId == 0 ? null : currencyId)));
    }

    /** VoucherReports.FCYPayablesAndReceivables_Rpt: FromDate, ToDate and ReportTypeId (1 receivables, 2 payables). */
    public List<Map<String, Object>> fcyPrintRows(String from, String to, int reportTypeId) {
        UserAccount u = user();
        return clean(DesktopProc.rows(jdbc, "SPU_Accounts_FCYPayablesAndReceivables_Rpt", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FromDate", sqlDate(date(from)), "ToDate", sqlDate(date(to)), "ReportTypeId", reportTypeId)));
    }

    // ===================================================================================== 75 forecast

    public Map<String, Object> forecastLookups() {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("amountDecimals", decimalPlaces(u, "Default NoofDecimal Points For Amount"));
        return out;
    }

    /**
     * GeneralReprots.DueDateAnalysisPayablesAndReceivablesForcast: @FromDate always, @ToDate only when the To picker is
     * checked, @SortNo and @DaysInterval (IntervalDays) when non-zero. ds.Tables[0] = per due date, Tables[1] = due-wise.
     */
    public Map<String, Object> forecast(String from, String to, int intervalDays, int sortNo) {
        UserAccount u = user();
        LocalDate f = date(from);
        if (f == null) throw new IllegalArgumentException("From date is required");
        StringBuilder sql = new StringBuilder("EXEC dbo.usp_shorttermDueDateAnalysisPayablesAndReceivablesForcast @OrganizationId=?, @CompanyId=?, @FromDate=?");
        List<Object> args = new ArrayList<>();
        args.add(u.getOrganizationId());
        args.add(u.getCompanyId());
        args.add(sqlDate(f));
        LocalDate t = date(to);
        if (t != null) { sql.append(", @ToDate=?"); args.add(sqlDate(t)); }
        if (sortNo != 0) { sql.append(", @SortNo=?"); args.add(sortNo); }
        if (intervalDays != 0) { sql.append(", @DaysInterval=?"); args.add(intervalDays); }
        List<List<Map<String, Object>>> sets = jdbc.execute(sql.toString(), (PreparedStatementCallback<List<List<Map<String, Object>>>>) ps -> {
            for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
            List<List<Map<String, Object>>> all = new ArrayList<>();
            boolean rs = ps.execute();
            while (true) {
                if (rs) {
                    try (ResultSet r = ps.getResultSet()) {
                        ResultSetMetaData md = r.getMetaData();
                        int n = md.getColumnCount();
                        List<Map<String, Object>> rows = new ArrayList<>();
                        while (r.next()) {
                            Map<String, Object> row = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
                            for (int c = 1; c <= n; c++) {
                                String label = md.getColumnLabel(c);
                                if (label == null || label.isEmpty()) label = "Column" + c;
                                if (!row.containsKey(label)) row.put(label, r.getObject(c));
                            }
                            rows.add(row);
                        }
                        all.add(rows);
                    }
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                rs = ps.getMoreResults();
            }
            return all;
        });
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("table0", sets != null && sets.size() > 0 ? clean(sets.get(0)) : new ArrayList<>());
        out.put("table1", sets != null && sets.size() > 1 ? clean(sets.get(1)) : new ArrayList<>());
        return out;
    }

    // ===================================================================================== 77 aging

    /**
     * Receivables_Load lists: AccountFill3rdLevel (ReadAll3rdLevelAccountsForPayablesandReceeivablesAging =
     * Sp_AccountsOpeningBalances_GetMethod), CustomeGroupsDefine(1), CustomerGroupsData (AccountsCustomGroups.GetAll =
     * Sp_AccountsCustomGroups_ReadAll).
     */
    public Map<String, Object> agingLookups() {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("accounts", clean(DesktopProc.rows(jdbc, "Sp_AccountsOpeningBalances_GetMethod", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "ReadAll3rdLevelAccountsForPayablesandReceeivablesAging"))));
        out.put("customGroups", customGroups(u));
        out.put("customGroupAll", clean(DesktopProc.rows(jdbc, "Sp_AccountsCustomGroups_ReadAll", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))));
        out.put("yearStart", yearStart());
        return out;
    }

    /**
     * DataGridBindForShow: OrganizationId, CompanyId, UserId, FinancialYearId (ActiveYr.Id), ToDate (EndDate),
     * IntervalDays, DocNo (CBalNotEqualTo); ItemClassId (ReportClassification) and ItemTypeId (ReportType) only when a
     * radio is checked (0 = not sent, the procedure's own default 1).
     */
    public List<Map<String, Object>> aging(String to, int intervalDays, int notEqualTo, int classId, int typeId) {
        UserAccount u = user();
        LocalDate t = date(to);
        if (t == null) throw new IllegalArgumentException("To date is required");
        return clean(DesktopProc.rows(jdbc, "SpAccounts_PayablesReceivablesAging_Rpt", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(), "UserId", u.getId(),
                "EndDate", sqlDate(t),
                "IntervalDays", intervalDays == 0 ? null : intervalDays,
                "CBalNotEqualTo", notEqualTo == 0 ? null : notEqualTo,
                "ReportClassification", classId == 0 ? null : classId,
                "ReportType", typeId == 0 ? null : typeId)));
    }

    // ===================================================================================== 870

    /**
     * The form's InitializeComponentMethod / btnRefresh: features 17 / 18, branches
     * (VoucherHead.GetBranchesFromVouchersByAccountId), custom groups, the 3rd-level account groups of class 2,3
     * (ChartofAccount.ReadAllAccountgroup), the cities, DefaultDaysToLessFromHistoryFromDate.
     */
    public Map<String, Object> prLookups() {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        boolean bf = feature(u, 17), cons = feature(u, 18);
        out.put("branchFeature", bf);
        out.put("branchFeatureConsolidated", cons);
        out.put("glBranchFeature", feature(u, 11));
        List<Map<String, Object>> branches = new ArrayList<>();
        try {
            for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetBranchesFromVouchersByAccountId", DesktopProc.params(
                    "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", toInt(r.get("Id")));
                o.put("BranchName", r.get("BranchName"));
                branches.add(o);
            }
        } catch (Exception e) {
            LOG.warn("Branches could not be read", e);
        }
        out.put("branches", branches);
        out.put("userBranchId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        out.put("customGroups", customGroups(u));
        List<Map<String, Object>> accounts = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_ChartofAccount_GetAllMethodFromCOA", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(), "Account_Level", 3,
                "AccountClassIds", "2,3", "CoaType", "ReadAllAccountGroup"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("AccountTitle", r.get("AccountTitle"));
            o.put("AccountCode", r.get("AccountCode"));
            accounts.add(o);
        }
        out.put("accounts", accounts);
        List<Map<String, Object>> cities = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "SP_City_GetAllMethod", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("CityName", r.get("CityName"));
            cities.add(o);
        }
        out.put("cities", cities);
        out.put("defaultDaysBack", toInt(config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("amountDecimals", decimalPlaces(u, "Default NoofDecimal Points For Amount"));
        out.put("yearStart", yearStart());
        return out;
    }

    /**
     * GeneralReprots.PayablesAndReceivablesWithPaymentAndReceipts: BranchesIds (InfragisticsHelper.GetBranchesIdsByFeature,
     * sent when not empty), FromDate, ToDate, AccouuntClassId (3 purchase / 2 sale / 0 both -> @ClassId), CustomGroupId,
     * ParentId (-> @ParentAccountId), AreaCity (-> @CityName); the optional ones only when set.
     */
    public List<Map<String, Object>> prData(String from, String to, String branchIds, int classId, int customGroupId,
                                            int parentId, String city) {
        UserAccount u = user();
        LocalDate f = date(from), t = date(to);
        if (f == null || t == null) throw new IllegalArgumentException("From and To dates are required");
        String branches = branchIdsByFeature(u, branchIds);
        String c = city == null ? "" : city.trim();
        return clean(DesktopProc.rows(jdbc, "usp_getPayablesAndReceivablesWithPaymentAndReceipts", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesIds", branches.isEmpty() ? null : branches,
                "FromDate", sqlDate(f), "ToDate", sqlDate(t),
                "CustomGroupId", customGroupId == 0 ? null : customGroupId,
                "ClassId", classId == 0 ? null : classId,
                "ParentAccountId", parentId == 0 ? null : parentId,
                "CityName", c.isEmpty() ? null : c)));
    }

    /**
     * InfragisticsHelper.GetBranchesIdsByFeature: consolidated (17 and 18) -> ",id,id" of the checked branches (or "");
     * branch feature only -> the one selected id, "Please Select Branch first!" when none; no branch feature -> "".
     */
    private String branchIdsByFeature(UserAccount u, String requested) {
        boolean bf = feature(u, 17), cons = feature(u, 18);
        List<Integer> ids = new ArrayList<>();
        for (String s : (requested == null ? "" : requested).split(",")) {
            s = s.trim();
            if (!s.isEmpty()) ids.add(toInt(s));
        }
        if (bf && cons) {
            StringBuilder sb = new StringBuilder();
            for (Integer id : ids) sb.append(',').append(id);
            return sb.toString();
        }
        if (bf) {
            if (ids.isEmpty() || ids.get(0) == 0) throw new IllegalArgumentException("Please Select Branch first!");
            return String.valueOf(ids.get(0));
        }
        return "";
    }
}
