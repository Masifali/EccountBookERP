package com.mst.services;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Shared conversions for the Inventory hub "Inventory Defination" (module 4) definition screens of batch
 * inv4b (94 CommissionItems, 95 CustomerCommissionPolicy, 96 CustomerDiscountPolicy, 98 DiscountPolicyForParty,
 * 114 InvDeffrmItemItemUom). Mirrors Architecture.Common.Conversion (ToInt / ToDouble / ToBool / ToString return
 * 0 / 0.0 / false / "" for null or unparsable input).
 */
final class Inv4bDefinitionSupport {

    private Inv4bDefinitionSupport() { }

    /** Conversion.ToInt — 0 for null, blank or non-numeric. */
    static int num(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null || String.valueOf(o).trim().isEmpty()) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDouble — 0.0 for null, blank or non-numeric (OnlytextdecimelFunction keeps the inputs numeric). */
    static double dbl(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        if (o == null || String.valueOf(o).trim().isEmpty()) return 0d;
        try { return Double.parseDouble(String.valueOf(o).trim().replace(",", "")); } catch (NumberFormatException e) { return 0d; }
    }

    /** Conversion.ToString — "" for null. */
    static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    /** Conversion.ToBool — true / 1 / "True". */
    static boolean bool(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = str(o);
        return "true".equalsIgnoreCase(s) || "1".equals(s);
    }

    /** Case-insensitive column read (SQL Server column names come back as declared). */
    static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    /** A DATE / DATETIME cell as "yyyy-MM-dd" (ToShortDateString in the desktop grids). */
    static String day(Object v) {
        if (v == null) return null;
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate().toString();
        if (v instanceof LocalDate) return v.toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        String s = String.valueOf(v);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    /** A DATETIME cell as "yyyy-MM-dd HH:mm:ss" (the grids that format "dd-MMM-yyyy hh:mm tt" do so in the page). */
    static String dateTime(Object v) {
        if (v == null) return null;
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString().replace('T', ' ');
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        return String.valueOf(v);
    }

    /**
     * An ISO "yyyy-MM-dd" (or longer) string as a DATETIME value at midnight; null / blank → today.
     * (dtpEffectedDate.Value on the desktop carries the time of day of the pick; the procedures compare
     * CAST(... AS DATE), so midnight is equivalent.) Passed to DesktopProc, which binds a Timestamp directly.
     */
    static Timestamp dateParam(String iso) {
        LocalDate d = (iso == null || iso.trim().length() < 10) ? LocalDate.now() : LocalDate.parse(iso.trim().substring(0, 10));
        return Timestamp.valueOf(d.atStartOfDay());
    }

    /** DateTime.Now. */
    static Timestamp now() { return Timestamp.valueOf(LocalDateTime.now().withNano(0)); }

    /** The single-column "SELECT @Id" a *_Insert procedure ends with; 0 when the procedure returned no row. */
    static int returnedId(List<Map<String, Object>> rows) {
        if (rows == null || rows.isEmpty() || rows.get(0).isEmpty()) return 0;
        return num(rows.get(0).values().iterator().next());
    }

    static Map<String, Object> ok(String message, int id) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        m.put("id", id);
        return m;
    }
}
