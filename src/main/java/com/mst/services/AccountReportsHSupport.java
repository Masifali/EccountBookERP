package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Component;

import java.math.BigDecimal;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.text.SimpleDateFormat;
import java.util.*;

/**
 * Shared plumbing for the Account Reports ported in the 2026-09-30 recheck (group H):
 * 62 Balance Sheet (BalanceSheet.cs), 64 Profit &amp; Loss 01 (frmProfitLossHararical.cs),
 * 84 BS and PL Breakup (BSandPLBreakup.cs), 958 Monthly Profit Loss
 * (frmProfitLossMonthWiseComparison.cs), 886 Commission Agent Report (CommissionAgentLedger.cs).
 *
 * Every call here is one the desktop makes, with the desktop's own parameter rule: a parameter the
 * BLL only adds when non-zero / non-empty is left out of the EXEC (ADO.NET never sends it), and
 * tenancy always comes from {@link CurrentUserContext}, never from the request.
 */
@Component
public class AccountReportsHSupport {

    /** CommonServices.GetERPFeatureById(17) - branch feature; (18) - branch consolidated. */
    public static final int FEATURE_BRANCH = 17;
    public static final int FEATURE_BRANCH_CONSOLIDATED = 18;

    /** A refusal in the desktop's own words (MessageBox text), returned as 400. */
    public static class Refusal extends RuntimeException {
        public Refusal(String m) { super(m); }
    }

    @Autowired
    private JdbcTemplate jdbc;

    @Autowired
    private CurrentUserContext ctx;

    public int org()  { return ctx.currentOrganizationId(); }
    public int comp() { return ctx.currentCompanyId(); }
    public int user() { return ctx.currentUserId(); }

    // ================================================================================ execution

    /**
     * GenericProvider.GetDataTableProc: the FIRST result set, columns kept in the procedure's own
     * order (the month-wise P&amp;L reads Column1..N / Caption1..N by position). A null value is not
     * sent. Dates go out as ISO text, decimals as numbers, binary (logo) columns are dropped.
     */
    public List<Map<String, Object>> rows(String proc, Map<String, Object> params) {
        return rowsWithColumns(proc, params).rows;
    }

    public static final class Table {
        public final List<String> columns;
        public final List<Map<String, Object>> rows;
        Table(List<String> c, List<Map<String, Object>> r) { columns = c; rows = r; }
    }

    public Table rowsWithColumns(String proc, Map<String, Object> params) {
        List<Object> args = new ArrayList<>();
        StringBuilder sb = new StringBuilder("EXEC ").append(proc.startsWith("[") ? proc : "dbo." + proc);
        boolean first = true;
        if (params != null) {
            for (Map.Entry<String, Object> e : params.entrySet()) {
                if (e.getValue() == null) continue;
                sb.append(first ? " " : ", ").append('@').append(e.getKey()).append("=?");
                args.add(e.getValue());
                first = false;
            }
        }
        final String sql = sb.toString();
        return jdbc.execute(sql, (PreparedStatementCallback<Table>) ps -> {
            for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
            List<String> cols = new ArrayList<>();
            List<Map<String, Object>> out = new ArrayList<>();
            boolean isRs = ps.execute();
            boolean taken = false;
            while (true) {
                if (isRs) {
                    try (ResultSet rs = ps.getResultSet()) {
                        if (!taken) {
                            ResultSetMetaData md = rs.getMetaData();
                            int n = md.getColumnCount();
                            List<Integer> keep = new ArrayList<>();
                            for (int c = 1; c <= n; c++) {
                                String label = md.getColumnLabel(c);
                                if (label == null || label.isEmpty()) label = "Column" + c;
                                int t = md.getColumnType(c);
                                boolean binary = t == java.sql.Types.BINARY || t == java.sql.Types.VARBINARY
                                        || t == java.sql.Types.LONGVARBINARY || t == java.sql.Types.BLOB;
                                if (binary || cols.contains(label)) continue;
                                cols.add(label);
                                keep.add(c);
                            }
                            while (rs.next()) {
                                Map<String, Object> row = new LinkedHashMap<>();
                                for (int i = 0; i < keep.size(); i++) row.put(cols.get(i), plain(rs.getObject(keep.get(i))));
                                out.add(row);
                            }
                            taken = true;
                        } else {
                            while (rs.next()) { /* drain */ }
                        }
                    }
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                isRs = ps.getMoreResults();
            }
            return new Table(cols, out);
        });
    }

    private static Object plain(Object v) {
        if (v == null) return null;
        if (v instanceof BigDecimal) return ((BigDecimal) v).doubleValue();
        if (v instanceof java.sql.Timestamp) return new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss").format((java.sql.Timestamp) v);
        if (v instanceof java.sql.Date) return new SimpleDateFormat("yyyy-MM-dd").format((java.sql.Date) v);
        if (v instanceof java.time.temporal.TemporalAccessor) return v.toString();
        if (v instanceof byte[]) return null;
        return v;
    }

    public static Map<String, Object> params(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    // ================================================================================ lookups

    /** CommonServices.GetERPFeatureById - clsGlobalVariables.ErpFeaturesList (USP_GetERPFeaturesByCompanyId). */
    public boolean erpFeature(int featureId) {
        for (Map<String, Object> r : rows("USP_GetERPFeaturesByCompanyId", params("OrganizationId", org(), "CompanyId", comp()))) {
            if (toInt(col(r, "Id")) == featureId) return true;
        }
        return false;
    }

    /**
     * BranchFill (BalanceSheet.cs:589 / frmProfitLossHararical.cs BranchFill) -
     * VoucherHead.GetBranchesFromVouchersByAccountId(Org, Company, "", 0) (BLL 0654:2367):
     * @OrganizationId always, @CompanyId when non-zero, no @CompanyIds, no @AccountId.
     * Bound "Id" / "BranchName".
     */
    public List<Map<String, Object>> branchesFromVouchers() {
        List<Map<String, Object>> out = new ArrayList<>();
        int c = comp();
        for (Map<String, Object> r : rows("USP_GetBranchesFromVouchersByAccountId",
                params("OrganizationId", org(), "CompanyId", c != 0 ? c : null))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", col(r, "Id"));
            m.put("BranchName", col(r, "BranchName"));
            out.add(m);
        }
        return out;
    }

    /** UserAccount.BranchName - the signed-in branch (Sp_Branches_GetAllMethod GetById). */
    public String userBranchName() {
        try {
            List<Map<String, Object>> r = rows("Sp_Branches_GetAllMethod", params("Id", ctx.currentBranchId(), "Activity", "GetById"));
            if (r.isEmpty()) return "";
            Object v = col(r.get(0), "BranchName");
            return v == null ? "" : String.valueOf(v);
        } catch (Exception e) {
            return "";
        }
    }

    /** clsGlobalVariables.ActiveYr.Start_Period - the signed-in financial year. */
    public String financialYearStart() {
        int yearId = ctx.currentFinancialYearId();
        List<Map<String, Object>> years = rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                params("OrganizationId", org(), "CompanyId", comp()));
        Map<String, Object> row = null;
        for (Map<String, Object> r : years) if (toInt(col(r, "Id")) == yearId) { row = r; break; }
        if (row == null && !years.isEmpty()) row = years.get(0);
        Object v = row == null ? null : col(row, "Start_Period");
        return v == null ? null : String.valueOf(v).substring(0, Math.min(10, String.valueOf(v).length()));
    }

    public int financialYearId() { return ctx.currentFinancialYearId(); }

    /** CommonServices.GetConfigurationByOrgCompandConfigDescription. */
    public String configValue(String description) {
        List<Map<String, Object>> r = rows("Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", org(), "CompanyId", comp(), "ConfigDescription", description,
                "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = col(r.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** GetDecimalConfiguration: DefaultNoofDecimalPointsForAmount (1-4 give that many places, else none). */
    public int amountDecimals() {
        int n = intOf(configValue("Default NoofDecimal Points For Amount"));
        return (n >= 1 && n <= 4) ? n : 0;
    }

    /** GetDecimalConfiguration: DecimalRateFormate ("#,#0." + 1-4 places; 0 gives 2 places). */
    public int rateDecimals() {
        int n = intOf(configValue("Default NoofDecimal Points For Rate"));
        if (n == 0) return 2;
        return (n >= 1 && n <= 4) ? n : 0;
    }

    /** Load-time flags every branch-aware form in this group reads. */
    public Map<String, Object> branchContext() {
        Map<String, Object> out = new LinkedHashMap<>();
        boolean f17 = erpFeature(FEATURE_BRANCH);
        boolean f18 = erpFeature(FEATURE_BRANCH_CONSOLIDATED);
        out.put("branchFeature", f17);
        out.put("branchFeatureConsolidated", f18);
        out.put("branches", f17 ? branchesFromVouchers() : new ArrayList<>());
        out.put("userBranchName", f17 ? userBranchName() : "");
        return out;
    }

    /**
     * InfragisticsHelper.GetBranchesIdsByFeature (Architecture.WinApp.Common/InfragisticsHelper.cs:186).
     *  - feature 17 AND 18: the combo's Text (ticked names, "," separated) is split and each name is
     *    looked up by BranchName in the bound list; ids are appended as ",id" (so the result starts
     *    with a comma, exactly as the desktop builds it). Empty text gives "".
     *  - feature 17 only: the single selected branch is required - "Please Select Branch first!".
     *  - otherwise "".
     */
    public String branchIdsByFeature(String branchText, Integer branchId) {
        boolean f17 = erpFeature(FEATURE_BRANCH);
        boolean f18 = erpFeature(FEATURE_BRANCH_CONSOLIDATED);
        if (f17 && f18) {
            String text = branchText == null ? "" : branchText;
            StringBuilder ids = new StringBuilder();
            if (!text.isEmpty()) {
                List<Map<String, Object>> list = branchesFromVouchers();
                for (String author : (text + ",").split(",", -1)) {
                    for (Map<String, Object> b : list) {
                        if (author.equals(String.valueOf(col(b, "BranchName")))) { ids.append(',').append(toInt(col(b, "Id"))); break; }
                    }
                }
            }
            return ids.toString();
        } else if (f17) {
            if (branchId == null || branchId == 0) throw new Refusal("Please Select Branch first!");
            return String.valueOf(branchId);
        }
        return "";
    }

    // ================================================================================ helpers

    public static Object col(Map<String, Object> r, String key) {
        if (r == null) return null;
        if (r.containsKey(key)) return r.get(key);
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    public static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    public static double toDouble(Object o) {
        if (o == null) return 0d;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0d; }
    }

    private static int intOf(String s) {
        try { return s == null || s.isEmpty() ? 0 : (int) Math.floor(Double.parseDouble(s)); } catch (Exception e) { return 0; }
    }

    /** A DateTimePicker value from the page (yyyy-MM-dd); required, as the picker always has one. */
    public static java.sql.Date date(Object v, String caption) {
        String s = v == null ? "" : String.valueOf(v).trim();
        if (s.length() >= 10) s = s.substring(0, 10);
        try { return java.sql.Date.valueOf(s); } catch (Exception e) { throw new Refusal(caption + " is not a valid date."); }
    }
}
