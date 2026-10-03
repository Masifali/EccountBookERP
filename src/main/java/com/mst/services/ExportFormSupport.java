package com.mst.services;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Shared conversions for the Export module forms ported in batch F (206, 213, 214, 205, 188, 907, 915):
 * the desktop's Conversion.ToInt / ToDouble / ToDecimal / ToBool / ToDateTime semantics, case-insensitive
 * DataRow column reads, and the JSON body helpers every service uses. No business logic lives here.
 */
public final class ExportFormSupport {

    private ExportFormSupport() { }

    /** Case-insensitive column read (DataRow["Name"]). */
    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    /** Conversion.ToString: null / DBNull -> "". */
    public static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    /** Conversion.ToInt: unparseable -> 0 (thousands separators tolerated). */
    public static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); }
        catch (NumberFormatException e) {
            try { return (int) Double.parseDouble(s); } catch (NumberFormatException e2) { return 0; }
        }
    }

    /** Conversion.ToDouble: unparseable -> 0. */
    public static double asDouble(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDecimal. */
    public static BigDecimal dec(Object v) {
        if (v instanceof BigDecimal) return (BigDecimal) v;
        return BigDecimal.valueOf(asDouble(v));
    }

    /**
     * Conversion.ToBool: Convert.ToBoolean(value), falling back to Convert.ToBoolean(Convert.ToInt32(value)),
     * and false for null / "" / anything neither "true"/"false" nor a number - so a CheckBox whose Text is
     * " " or "Active" converts to FALSE (the desktop reads the caption, not Checked, on several forms).
     */
    public static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return false;
        if (s.equalsIgnoreCase("true")) return true;
        if (s.equalsIgnoreCase("false")) return false;
        try { return Integer.parseInt(s) != 0; } catch (NumberFormatException e) { return false; }
    }

    /** Checkbox / JSON boolean from the page (true/"true"/1/"on"). */
    public static boolean flag(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v == null) return false;
        String s = String.valueOf(v).trim().toLowerCase();
        return "true".equals(s) || "1".equals(s) || "on".equals(s);
    }

    /** yyyy-MM-dd (or ISO date-time) -> LocalDate; null when blank / unparseable. */
    public static LocalDate asDate(Object v) {
        if (v == null) return null;
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate();
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate();
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        if (s.length() > 10) s = s.substring(0, 10);
        try { return LocalDate.parse(s); } catch (DateTimeParseException e) { return null; }
    }

    /** A DATETIME parameter from the page's yyyy-MM-dd[THH:mm[:ss]]; blank -> now (DateTimePicker.Value is never empty). */
    public static Timestamp ts(Object v) {
        if (v instanceof Timestamp) return (Timestamp) v;
        if (v == null) return new Timestamp(System.currentTimeMillis());
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return new Timestamp(System.currentTimeMillis());
        try { return Timestamp.valueOf(LocalDateTime.parse(s.length() == 10 ? s + "T00:00:00" : s.length() == 16 ? s + ":00" : s)); }
        catch (DateTimeParseException e) {
            try { return Timestamp.valueOf(s.replace('T', ' ')); }
            catch (IllegalArgumentException e2) { return new Timestamp(System.currentTimeMillis()); }
        }
    }

    /** Like {@link #ts} but null when blank (for the BLL's CheckDateTimeNull guards). */
    public static Timestamp tsOrNull(Object v) {
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        return ts(s);
    }

    public static java.sql.Date sqlDate(LocalDate d) { return d == null ? null : java.sql.Date.valueOf(d); }

    /** yyyy-MM-dd for the page. */
    public static String iso(Object v) {
        if (v == null) return "";
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate().toString();
        if (v instanceof LocalDate) return v.toString();
        String s = String.valueOf(v).trim();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    /** yyyy-MM-ddTHH:mm:ss for the page (EntryDate / ModifyDate columns). */
    public static String isoDateTime(Object v) {
        if (v == null) return "";
        LocalDateTime t = null;
        if (v instanceof java.sql.Timestamp) t = ((java.sql.Timestamp) v).toLocalDateTime();
        else if (v instanceof java.sql.Date) t = ((java.sql.Date) v).toLocalDate().atStartOfDay();
        else if (v instanceof java.util.Date) t = new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime();
        else if (v instanceof LocalDateTime) t = (LocalDateTime) v;
        else if (v instanceof LocalDate) t = ((LocalDate) v).atStartOfDay();
        if (t != null) return t.withNano(0).toString();
        return String.valueOf(v).trim();
    }

    @SuppressWarnings("unchecked")
    public static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    public static Map<String, Object> saved(int id, String message) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("message", message);
        return out;
    }

    /** Innermost exception text, as MessageBox.Show(ex.Message) shows it. */
    public static String msg(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    /** A copy of every row as a mutable LinkedHashMap (for projections). */
    public static List<Map<String, Object>> copy(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(new LinkedHashMap<>(r));
        return out;
    }

    /** Form text that must not be blank: "{label} Field is Required!!!"-style callers pass their own text. */
    public static void required(boolean bad, String message) {
        if (bad) throw new IllegalArgumentException(message);
    }
}
