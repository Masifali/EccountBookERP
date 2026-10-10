package com.mst.services.sale.bk;

import com.mst.models.UserAccount;
import com.mst.services.sale.engr.SaleEngrSupport;
import com.mst.services.sale.steel.SaleSteelSupport;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Component;

import java.sql.Date;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;

/**
 * Plumbing for the Booking Office / Customer Portal and Sale Salt screens (module 94 / 95 / 132 / 2042, key B of the Sale hub port).
 * Tenancy, rights and configuration come from SaleEngrSupport / CurrentUserContext; the ordered procedure call from SaleSteelSupport.
 */
@Component
public class SaleBkSupport {
    private final SaleEngrSupport sup;
    private final SaleSteelSupport steel;

    private final JdbcTemplate jdbc;

    public SaleBkSupport(SaleEngrSupport sup, SaleSteelSupport steel, JdbcTemplate jdbc) { this.sup = sup; this.steel = steel; this.jdbc = jdbc; }

    /**
     * GenericProvider.GetDataSetProc: EXEC proc @p=? ... (null values omitted) and EVERY result set returned in order (SaleSteelSupport.table keeps only the first).
     */
    public List<List<Map<String, Object>>> tables(String proc, Map<String, Object> params) {
        List<Object> args = new ArrayList<>();
        StringBuilder sb = new StringBuilder("EXEC ").append(proc.startsWith("[") ? proc : "dbo." + proc);
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            if (e.getValue() == null) continue;
            sb.append(first ? " " : ", ").append('@').append(e.getKey()).append("=?");
            args.add(e.getValue());
            first = false;
        }
        return jdbc.execute(sb.toString(), (PreparedStatementCallback<List<List<Map<String, Object>>>>) ps -> {
            for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
            List<List<Map<String, Object>>> sets = new ArrayList<>();
            boolean isRs = ps.execute();
            while (true) {
                if (isRs) {
                    try (ResultSet rs = ps.getResultSet()) {
                        ResultSetMetaData md = rs.getMetaData();
                        int n = md.getColumnCount();
                        List<String> labels = new ArrayList<>();
                        for (int c = 1; c <= n; c++) {
                            String label = md.getColumnLabel(c);
                            if (label == null || label.isEmpty()) label = "Column" + c;
                            String base = label;
                            int k = 1;
                            while (labels.contains(label)) label = base + (++k);
                            labels.add(label);
                        }
                        List<Map<String, Object>> rows = new ArrayList<>();
                        while (rs.next()) {
                            Map<String, Object> row = new LinkedHashMap<>();
                            for (int c = 1; c <= n; c++) row.put(labels.get(c - 1), SaleSteelSupport.json(rs.getObject(c)));
                            rows.add(row);
                        }
                        sets.add(rows);
                    }
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                isRs = ps.getMoreResults();
            }
            return sets;
        });
    }

    public SaleEngrSupport sup() { return sup; }
    public SaleSteelSupport steel() { return steel; }
    public UserAccount user() { return sup.user(); }

    public static int i(Object o) { return SaleEngrSupport.toInt(o); }
    public static double dbl(Object o) { return SaleEngrSupport.toDouble(o); }
    public static String s(Object o) { return o == null ? "" : String.valueOf(o); }
    public static Object ci(Map<String, Object> r, String k) { return SaleEngrSupport.ci(r, k); }

    /** A DateTimePicker value as the BLL receives it (always a DateTime). */
    public static Date day(String v) {
        if (v == null || v.trim().length() < 10) return null;
        try { return Date.valueOf(LocalDate.parse(v.trim().substring(0, 10))); } catch (Exception e) { return null; }
    }

    /** GetDecimalConfiguration: stringFormatsingle zeros. */
    public int amountDecimals() {
        int n = i(sup.config("Default NoofDecimal Points For Amount"));
        return n >= 1 && n <= 4 ? n : 0;
    }

    /** GetDecimalConfiguration: DecimalRateFormate zeros. */
    public int rateDecimals() {
        int n = i(sup.config("Default NoofDecimal Points For Rate"));
        if (n >= 1 && n <= 4) return n;
        return n == 0 ? 2 : 0;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period of the session's financial year (yyyy-MM-dd) or null. */
    public String yearStart() {
        UserAccount u = user();
        int yearId = sup.fy();
        List<Map<String, Object>> years = sup.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        Map<String, Object> pick = null;
        for (Map<String, Object> y : years) if (i(ci(y, "Id")) == yearId) { pick = y; break; }
        if (pick == null && !years.isEmpty() && yearId <= 0) pick = years.get(0);
        if (pick == null) return null;
        Object start = ci(pick, "Start_Period");
        if (start == null) return null;
        if (start instanceof Timestamp) return ((Timestamp) start).toLocalDateTime().toLocalDate().toString();
        if (start instanceof java.util.Date) return new Date(((java.util.Date) start).getTime()).toLocalDate().toString();
        String t = String.valueOf(start);
        return t.substring(0, Math.min(10, t.length()));
    }

    public Map<String, Object> basics() {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("amountDecimals", amountDecimals());
        d.put("rateDecimals", rateDecimals());
        d.put("yearStart", yearStart());
        return d;
    }

    /** The DropDownBind source: rows {Id, name} copied from the rows of one Activity (CommonServices.CreateDropDownTable). */
    public static List<Map<String, Object>> activity(List<Map<String, Object>> all, String act, String idKey, String nameKey) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : all) {
            if (!act.equals(s(ci(r, "Activity")))) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, idKey)); o.put("name", ci(r, nameKey));
            out.add(o);
        }
        return out;
    }

    public static void nz(Map<String, Object> p, String key, int v) { if (v != 0) p.put(key, v); }

    public static Map<String, Object> p(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int k = 0; k + 1 < kv.length; k += 2) m.put(String.valueOf(kv[k]), kv[k + 1]);
        return m;
    }

    /** UserAccount.AppId of the desktop session = the application the web session has chosen (0 when none). */
    public int appId() {
        try { return sup.ctx().currentAppId(); } catch (RuntimeException e) { return 0; }
    }

    /** Projects.GetSubCostCenters(Org, Company, UserId, AppId, ParentCostCenterId): usp_getCostCenters (AppId / ParentCostCenterId only when != 0). */
    public List<Map<String, Object>> subCostCenters(int parentCostCenterId) {
        UserAccount u = user();
        Map<String, Object> p = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "UserId", u.getId());
        nz(p, "AppId", appId());
        nz(p, "ParentCostCenterId", parentCostCenterId);
        return steel.table("usp_getCostCenters", p).rows;
    }

    /** CommonServices.DateType(): Id / Parameters. */
    public static List<Map<String, Object>> dateTypes(boolean withAll) {
        String[] n = {"This Day", "This Week", "This Month", "This Year", "Financial Year"};
        List<Map<String, Object>> out = new ArrayList<>();
        for (int k = 0; k < n.length; k++) out.add(p("Id", k + 1, "Parameters", n[k]));
        if (withAll) out.add(p("Id", 6, "Parameters", "All"));
        return out;
    }
}
