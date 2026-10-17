package com.mst.services;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Component;

import java.math.BigDecimal;
import java.math.MathContext;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;

/**
 * Shared plumbing of the W3C Production group (screens 887 Production Costing and 906 Production FOH):
 * the desktop rights object, the desktop number formats and the financial-year month list.
 * Tenancy, branch, financial year and user always come from {@link CurrentUserContext}.
 */
@Component
public class W3CSupport {

    private static final DateTimeFormatter MMM_YY = DateTimeFormatter.ofPattern("MMM-yy", Locale.ENGLISH);

    @Autowired private JdbcTemplate jdbc;
    @Autowired private CurrentUserContext ctx;

    /* ================================================================================== rights */

    /**
     * CommonServices.SetRightsValueInRightsObject(ScreenName) (CommonServices.cs:17565): only the role spelled
     * exactly "Admin" is special (Save, Update, Delete, Print, CanView AllRecord); View, Grid Print and
     * Grid Export come only from grant rows; FieldChooser / SaveLayout / GroupCollapse / GroupExpand start true.
     */
    public Map<String, Boolean> rights(String screenName) {
        String role = ctx.currentRoleName();
        boolean admin = "Admin".equals(role);
        Map<String, Boolean> r = new LinkedHashMap<>();
        r.put("view", false);
        r.put("save", admin);
        r.put("update", admin);
        r.put("delete", admin);
        r.put("print", admin);
        r.put("canViewAllRecord", admin);
        r.put("gridPrint", false);
        r.put("gridExport", false);
        r.put("fieldChooser", true);
        r.put("saveLayout", true);
        r.put("groupCollapse", true);
        r.put("groupExpand", true);
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_tblUserRights_GetAllMethod", DesktopProc.params(
                "UserId", ctx.currentUserId(), "ScreenName", screenName,
                "RightName", role == null ? "" : role,
                "CompanyId", ctx.currentCompanyId(), "Activity", "GetByUserId"));
        for (Map<String, Object> row : rows) {
            String name = str(row.get("RightName"));
            boolean v = toBool(row.get("Value"));
            switch (name) {
                case "View":              r.put("view", v); break;
                case "Save":              r.put("save", admin || v); break;
                case "Update":            r.put("update", admin || v); break;
                case "Print":             r.put("print", admin || v); break;
                case "CanView AllRecord": r.put("canViewAllRecord", admin || v); break;
                case "Grid Print":        r.put("gridPrint", v); break;
                case "Grid Export":       r.put("gridExport", v); break;
                case "Delete":            r.put("delete", admin || v); break;
                default: break;
            }
        }
        return r;
    }

    public void require(Map<String, Boolean> rights, String right, String what) {
        if (!Boolean.TRUE.equals(rights.get(right))) {
            throw new IllegalArgumentException("You do not have the " + what + " right for this screen.");
        }
    }

    /* ============================================================================ configuration */

    /** CommonServices.GetDecimalConfiguration: stringFormatsingle = "#,##0." + 1..4 zeros, else no decimals. */
    public int amountDecimals() {
        int n = configInt("Default NoofDecimal Points For Amount");
        return n >= 1 && n <= 4 ? n : 0;
    }

    /** clsGlobalVariables.DecimalRateFormate = "#,#0." + zeros: 0 (or missing) -> 2, 1..4 -> n, anything else -> 0. */
    public int rateDecimals() {
        int n = configInt("Default NoofDecimal Points For Rate");
        if (n == 0) return 2;
        return n >= 1 && n <= 4 ? n : 0;
    }

    /** GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"); 0 when unset. */
    public int defaultHistoryDays() {
        return configInt("DefaultDaysToLessFromHistoryFromDate");
    }

    private int configInt(String description) {
        try {
            List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", DesktopProc.params(
                    "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                    "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
            return r.isEmpty() ? 0 : toInt(str(r.get(0).get("ConfigKey")));
        } catch (RuntimeException e) {
            return 0;
        }
    }

    /* ======================================================================== financial year */

    /** The active financial year row (Start_Period / End_Period) of the signed-in session. */
    public Map<String, Object> activeYear() {
        int yearId = ctx.currentFinancialYearId();
        List<Map<String, Object>> years = DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId()));
        for (Map<String, Object> y : years) if (toInt(y.get("Id")) == yearId) return y;
        if (!years.isEmpty()) return years.get(0);
        throw new IllegalStateException("No active financial year");
    }

    /**
     * InfragisticsHelper.FinancialYearMonthsComboBind -> GlobalVariables_Helper.GetMonthListByFinancialYEarFromGlobal:
     * ActiveYr.Start_Period, then date.AddMonths(1) while date &lt;= End_Period; Id and Month are both "MMM-yy".
     */
    public List<String> financialYearMonths() {
        Map<String, Object> year = activeYear();
        LocalDateTime start = toDateTime(year.get("Start_Period"));
        LocalDateTime end = toDateTime(year.get("End_Period"));
        List<String> out = new ArrayList<>();
        if (start == null || end == null) return out;
        LocalDateTime date = start;
        for (int guard = 0; !date.isAfter(end) && guard < 1200; guard++) {
            out.add(date.format(MMM_YY));
            date = date.plusMonths(1);
        }
        return out;
    }

    public String activeYearStart() {
        LocalDateTime s = toDateTime(activeYear().get("Start_Period"));
        return s == null ? null : s.toLocalDate().toString();
    }

    /* ================================================================================ helpers */

    public static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    public static boolean toBool(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = str(o);
        return "true".equalsIgnoreCase(s) || "1".equals(s);
    }

    public static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return (int) Math.rint(((Number) o).doubleValue());
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        String s = str(o).replace(",", "");
        if (s.isEmpty()) return 0;
        try { return (int) Math.rint(Double.parseDouble(s)); } catch (NumberFormatException e) { return 0; }
    }

    public static double toDouble(Object o) {
        if (o == null) return 0d;
        if (o instanceof Number) return ((Number) o).doubleValue();
        String s = str(o).replace(",", "");
        if (s.isEmpty()) return 0d;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0d; }
    }

    /** Conversion.ToDecimal(double): Convert.ToDecimal rounds a double to 15 significant digits. */
    public static BigDecimal toDecimal(Object o) {
        if (o instanceof BigDecimal) return (BigDecimal) o;
        double d = toDouble(o);
        if (Double.isNaN(d) || Double.isInfinite(d)) return BigDecimal.ZERO;
        return new BigDecimal(d).round(new MathContext(15));
    }

    public static LocalDateTime toDateTime(Object o) {
        if (o instanceof Timestamp) return ((Timestamp) o).toLocalDateTime();
        if (o instanceof java.sql.Date) return ((java.sql.Date) o).toLocalDate().atStartOfDay();
        if (o instanceof LocalDateTime) return (LocalDateTime) o;
        String s = str(o);
        if (s.isEmpty()) return null;
        try { return LocalDateTime.parse(s.length() > 19 ? s.substring(0, 19) : s); } catch (RuntimeException e) { /* date only */ }
        try { return LocalDate.parse(s.length() > 10 ? s.substring(0, 10) : s).atStartOfDay(); } catch (RuntimeException e) { return null; }
    }

    /** A JSON-safe copy of a result row (dates as ISO text, decimals as doubles). */
    public static Map<String, Object> plainRow(Map<String, Object> row) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : row.entrySet()) m.put(e.getKey(), plain(e.getValue()));
        return m;
    }

    public static List<Map<String, Object>> plainRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(plainRow(r));
        return out;
    }

    public static Object plain(Object o) {
        if (o instanceof Timestamp) return ((Timestamp) o).toLocalDateTime().toString();
        if (o instanceof java.sql.Date) return ((java.sql.Date) o).toLocalDate().toString();
        if (o instanceof BigDecimal) return ((BigDecimal) o).doubleValue();
        return o;
    }

    @SuppressWarnings("unchecked")
    public static List<Map<String, Object>> rowList(Object o) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (o instanceof List) for (Object r : (List<?>) o) if (r instanceof Map) out.add((Map<String, Object>) r);
        return out;
    }

    @SuppressWarnings("unchecked")
    public static Map<String, Object> mapOf(Object o) {
        return o instanceof Map ? (Map<String, Object>) o : new LinkedHashMap<>();
    }

    /** Case-insensitive column read from a result row or a posted row. */
    public static Object col(Map<String, Object> row, String name) {
        if (row == null) return null;
        Object v = row.get(name);
        if (v != null || row.containsKey(name)) return v;
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }
}
