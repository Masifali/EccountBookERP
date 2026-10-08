package com.mst.services.sale.engr;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Component;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.*;

/**
 * Shared plumbing for the Sale Engr screens (Architecture.WinApp.SaleTrading.*).
 *
 * Tenancy (organization, company, user, branch, financial year) always comes from CurrentUserContext - never from the browser.
 * Procedure calls go through DesktopProc, which reproduces GenericProvider.GetDataTableProc / SetProc: a null parameter is
 * left out of the EXEC (ADO.NET AddWithValue(null) is not sent), exactly like the desktop.
 */
@Component
public class SaleEngrSupport {

    /** A desktop MessageBox the user must read; nothing is saved. Message text is word-for-word the desktop's. */
    public static class Warning extends IllegalArgumentException {
        public Warning(String m) { super(m); }
    }

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;

    public SaleEngrSupport(JdbcTemplate jdbc, CurrentUserContext ctx) { this.jdbc = jdbc; this.ctx = ctx; }

    public JdbcTemplate jdbc() { return jdbc; }
    public CurrentUserContext ctx() { return ctx; }
    public UserAccount user() { return ctx.requireAccountingUser(); }
    public int org() { return ctx.currentOrganizationId(); }
    public int company() { return ctx.currentCompanyId(); }
    public int branch() { return user().getBranchesId(); }
    public int userId() { return ctx.currentUserId(); }
    public int fy() { return ctx.currentFinancialYearId(); }
    public String role() { String r = ctx.currentRoleName(); return r == null ? "" : r; }

    // ------------------------------------------------------------------ procedures

    /** DataTable fill: first result set, case-insensitive columns. kv = name, value, name, value ... (null value => not sent). */
    public List<Map<String, Object>> rows(String proc, Object... kv) {
        return DesktopProc.rows(jdbc, proc, DesktopProc.params(kv));
    }

    /** GenericProvider.SetProc: Convert.ToInt32(ExecuteScalar()). */
    public int setProc(String proc, Object... kv) {
        return DesktopProc.setProc(jdbc, proc, DesktopProc.params(kv));
    }

    /** GenericProvider.SetProc with a prepared (ordered) parameter map; null values are not sent. */
    public int setProcMap(String proc, Map<String, Object> params) {
        return DesktopProc.setProc(jdbc, proc, params);
    }

    public Integer scalar(String proc, Object... kv) {
        return DesktopProc.scalar(jdbc, proc, DesktopProc.params(kv));
    }

    /** Desktop "DataTable -> first row, column" with Conversion.ToInt semantics, 0 when there is no row. */
    public int firstInt(List<Map<String, Object>> rows, String col) {
        return rows == null || rows.isEmpty() ? 0 : toInt(rows.get(0).get(col));
    }

    // ------------------------------------------------------------------ configuration (clsGlobalVariables.configrationsAllocation)

    public String config(String description) {
        List<Map<String, Object>> r = rows("Sp_ConfigrationsAllocation_GetAllMethod", "OrganizationId", org(), "CompanyId", company(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription");
        Object v = r.isEmpty() ? null : r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    public boolean configBool(String description) { return toBool(config(description)); }

    public int configInt(String description) { return toInt(config(description)); }

    /** CommonServices.GetERPFeatureById(id). */
    public boolean erpFeature(int id) {
        for (Map<String, Object> r : rows("USP_GetERPFeaturesByCompanyId", "OrganizationId", org(), "CompanyId", company()))
            if (toInt(r.get("Id")) == id) return true;
        return false;
    }

    // ------------------------------------------------------------------ rights (CommonServices.SetRightsValueInRightsObject)

    public Map<String, Boolean> rights(String screenName) {
        UserAccount u = user();
        String role = role();
        boolean admin = "Admin".equals(role);
        Map<String, Boolean> m = new LinkedHashMap<>();
        for (String k : List.of("view", "save", "update", "print", "viewAll", "delete", "approve", "rate", "rateDenied", "settle",
                "toComplete", "toOpen", "toCancel", "expiry", "gridPrint", "gridExport")) m.put(k, false);
        if (admin) {
            for (String k : List.of("save", "update", "print", "viewAll", "delete", "toComplete", "toOpen", "toCancel", "expiry")) m.put(k, true);
        }
        for (Map<String, Object> r : rows("Sp_tblUserRights_GetAllMethod", "UserId", u.getId(), "ScreenName", screenName,
                "RightName", role, "CompanyId", u.getCompanyId(), "Activity", "GetByUserId")) {
            String n = String.valueOf(r.get("RightName")).trim();
            boolean v = toBool(r.get("Value"));
            switch (n) {
                case "View" -> m.put("view", v);
                case "Save" -> m.put("save", admin || v);
                case "Update" -> m.put("update", admin || v);
                case "Print" -> m.put("print", admin || v);
                case "CanView AllRecord" -> m.put("viewAll", admin || v);
                case "Grid Print" -> m.put("gridPrint", v);
                case "Grid Export" -> m.put("gridExport", v);
                case "Delete" -> m.put("delete", v);
                case "Approve" -> m.put("approve", v);
                case "Rate" -> m.put("rate", v);
                case "RateandAmountFieldAccessDenied" -> m.put("rateDenied", !admin && v);
                case "ProductionSettlement" -> m.put("settle", v);
                case "CanChangeOrderStatusToComplete" -> { if (!admin) m.put("toComplete", v); }
                case "CanChangeOrderStatusToOpen" -> { if (!admin) m.put("toOpen", v); }
                case "CanChangeOrderStatusToCancel" -> { if (!admin) m.put("toCancel", v); }
                case "CanChangeOrderExpiryDate" -> { if (!admin) m.put("expiry", v); }
                default -> { }
            }
        }
        return m;
    }

    // ------------------------------------------------------------------ Architecture.Common.Conversion

    public static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number n) return n.intValue();
        String s = String.valueOf(o).trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s.startsWith("+") ? s.substring(1) : s); } catch (NumberFormatException e) { }
        try { return (int) Math.rint(Double.parseDouble(s.replace(",", ""))); } catch (NumberFormatException e) { return 0; }
    }

    public static double toDouble(Object o) {
        if (o == null) return 0.0;
        if (o instanceof Number n) { double d = n.doubleValue(); return Double.isNaN(d) || Double.isInfinite(d) ? 0.0 : d; }
        String s = String.valueOf(o).trim().replace(",", "");
        if (s.isEmpty()) return 0.0;
        try { double d = Double.parseDouble(s); return Double.isNaN(d) || Double.isInfinite(d) ? 0.0 : d; } catch (NumberFormatException e) { return 0.0; }
    }

    public static BigDecimal toDecimal(Object o) {
        if (o == null) return BigDecimal.ZERO;
        if (o instanceof BigDecimal b) return b;
        try { return new BigDecimal(String.valueOf(o).trim().replace(",", "")); } catch (RuntimeException e) { return BigDecimal.ZERO; }
    }

    public static boolean toBool(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean b) return b;
        if (o instanceof Number n) return n.intValue() != 0;
        String s = String.valueOf(o).trim();
        return "true".equalsIgnoreCase(s) || "1".equals(s) || "yes".equalsIgnoreCase(s);
    }

    public static String str(Object o) { return o == null ? "" : String.valueOf(o); }

    public static String text(String s) { return s == null ? "" : s.trim(); }

    /** Integer id or null (a 0 id is "not sent" to the procedure, the way the desktop's unset int properties arrive). */
    public static Integer idOrNull(int n) { return n > 0 ? n : null; }

    public static Timestamp now() { return new Timestamp(System.currentTimeMillis()); }

    /** Case-insensitive lookup in a row that is not already case-insensitive. */
    public static Object ci(Map<String, Object> r, String key) {
        if (r == null) return null;
        if (r.containsKey(key)) return r.get(key);
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    /** A new ordered row. */
    public static Map<String, Object> row(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }
}
