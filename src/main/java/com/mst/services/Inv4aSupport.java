package com.mst.services;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;

/**
 * Batch inv4a (Inventory hub — pricing / discount schedules): small conversions shared by the five
 * services, mirroring Architecture.Common.Conversion (ToInt / ToDouble / ToString never throw and
 * give 0 / 0.0 / "" for null, DBNull or unparsable text).
 */
final class Inv4aSupport {

    private Inv4aSupport() { }

    /** Conversion.ToInt — (int) of the parsed double, 0 when not numeric. */
    static int num(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        if (o == null || String.valueOf(o).trim().isEmpty()) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim().replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDouble. */
    static double dbl(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        if (o == null || String.valueOf(o).trim().isEmpty()) return 0d;
        try { return Double.parseDouble(String.valueOf(o).trim().replace(",", "")); } catch (NumberFormatException e) { return 0d; }
    }

    /** Conversion.ToString — "" for null. */
    static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    static boolean truthy(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = str(o).toLowerCase(Locale.ROOT);
        return s.equals("true") || s.equals("1");
    }

    /** Case-insensitive column read (DesktopProc rows are already case-insensitive; plain JdbcTemplate rows are not). */
    static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    /** "yyyy-MM-dd" (or any ISO date-time prefix) → midnight Timestamp; null when blank/unparsable. */
    static Timestamp day(String iso) {
        if (iso == null || iso.trim().length() < 10) return null;
        try { return Timestamp.valueOf(LocalDate.parse(iso.trim().substring(0, 10)).atStartOfDay()); } catch (RuntimeException e) { return null; }
    }

    /**
     * The desktop's EffectedDate = date part of the date picker + TimeOfDay of the time picker
     * (frmItemPricingSchedule.Insert :454-456). {@code time} is "HH:mm" or "HH:mm:ss"; blank → 00:00:00.
     */
    static Timestamp dateWithTime(String isoDate, String time) {
        Timestamp d = day(isoDate);
        if (d == null) return null;
        LocalTime t = LocalTime.MIDNIGHT;
        if (time != null && time.trim().length() >= 5) {
            try { t = LocalTime.parse(time.trim().length() == 5 ? time.trim() + ":00" : time.trim()); } catch (RuntimeException e) { t = LocalTime.MIDNIGHT; }
        }
        return Timestamp.valueOf(LocalDateTime.of(d.toLocalDateTime().toLocalDate(), t));
    }

    static Timestamp now() { return Timestamp.valueOf(LocalDateTime.now().withNano(0)); }

    /** A DATE / DATETIME cell as "yyyy-MM-dd" (the day the server read; never shifted by a JSON time zone). */
    static String dayText(Object v) {
        if (v == null) return null;
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate().toString();
        if (v instanceof LocalDate) return v.toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        String s = String.valueOf(v);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    /** A DATETIME cell as "yyyy-MM-dd HH:mm:ss" (for grids that show the time, e.g. "dd-MMM-yy hh:mm tt"). */
    static String dateTimeText(Object v) {
        if (v == null) return null;
        LocalDateTime t = null;
        if (v instanceof Timestamp) t = ((Timestamp) v).toLocalDateTime();
        else if (v instanceof LocalDateTime) t = (LocalDateTime) v;
        else if (v instanceof java.util.Date) t = new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime();
        if (t == null) { String s = String.valueOf(v); return s.length() >= 19 ? s.substring(0, 19).replace('T', ' ') : s; }
        return t.toLocalDate() + " " + String.format("%02d:%02d:%02d", t.getHour(), t.getMinute(), t.getSecond());
    }

    /** Copies rows into plain maps with dates rendered as text so Jackson never shifts them. */
    static List<Map<String, Object>> plain(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof Timestamp || v instanceof java.util.Date || v instanceof LocalDateTime) v = dateTimeText(v);
                else if (v instanceof byte[]) v = null;
                o.put(e.getKey(), v);
            }
            out.add(o);
        }
        return out;
    }

    /** {Id, Name} projection of a lookup result. */
    static List<Map<String, Object>> idName(List<Map<String, Object>> rows, String idCol, String nameCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, idCol)));
            o.put("Name", str(ci(r, nameCol)));
            out.add(o);
        }
        return out;
    }

    @SuppressWarnings("unchecked")
    static List<Map<String, Object>> rowsOf(Map<String, Object> body, String key) {
        Object v = body == null ? null : body.get(key);
        return v instanceof List ? (List<Map<String, Object>>) v : new ArrayList<>();
    }

    static Map<String, Object> ok(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        return m;
    }
}
