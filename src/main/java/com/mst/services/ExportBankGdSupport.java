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
 * Conversion helpers shared by the five "Bank Export & GD Management" services (951, 952, 953,
 * 197, 198): the desktop's Conversion.ToInt / ToDouble / ToDecimal / ToDateTime semantics
 * (unparseable text is 0, "#,##0.###" text with thousands separators parses), case-insensitive
 * DataRow reads, FormHelper.ValidateField, and the yyyy-MM-dd <-> DATETIME bridge for the page.
 */
final class ExportBankGdSupport {

    private ExportBankGdSupport() { }

    /** Case-insensitive column read (DataRow["name"]). */
    static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Boolean) return (Boolean) v ? 1 : 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) {
            try { return (int) Double.parseDouble(String.valueOf(v).trim().replace(",", "")); }
            catch (NumberFormatException e2) { return 0; }
        }
    }

    static double asDouble(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return Double.parseDouble(String.valueOf(v).trim().replace(",", "")); }
        catch (NumberFormatException e) { return 0; }
    }

    static BigDecimal dec(Object v) {
        if (v instanceof BigDecimal) return (BigDecimal) v;
        return BigDecimal.valueOf(asDouble(v));
    }

    static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        String s = String.valueOf(v).trim().toLowerCase();
        return "true".equals(s) || "1".equals(s) || "on".equals(s);
    }

    static LocalDate asDate(Object v) {
        if (v == null) return null;
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate();
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate();
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        if (s.length() > 10) s = s.substring(0, 10);
        try { return LocalDate.parse(s); }
        catch (DateTimeParseException e) { return null; }
    }

    static java.sql.Date sqlDate(Object v) { LocalDate d = asDate(v); return d == null ? null : java.sql.Date.valueOf(d); }

    /** A DATETIME for the procedure; the page sends yyyy-MM-dd (or yyyy-MM-ddTHH:mm), the desktop the picker's value; blank = now. */
    static Timestamp ts(Object v) {
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

    /** yyyy-MM-dd for the page. */
    static String iso(Object v) {
        if (v == null) return "";
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.time.LocalDateTime) return ((java.time.LocalDateTime) v).toLocalDate().toString();
        if (v instanceof java.time.LocalDate) return v.toString();
        String s = String.valueOf(v).trim();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    /** yyyy-MM-ddTHH:mm:ss for the page (EntryDate / ModifyDate columns). */
    static String isoDateTime(Object v) {
        if (v == null) return "";
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().atStartOfDay().toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.time.LocalDateTime) return ((java.time.LocalDateTime) v).withNano(0).toString();
        return String.valueOf(v).trim();
    }

    /** FormHelper.ValidateField: null, 0, <= 0 decimals, blank strings -> "{field} is required in Detail Grid at row No: {n}". */
    static void validateField(Object value, String field, int rowIndex) {
        validateField(value, field, rowIndex, "Detail Grid");
    }

    static void validateField(Object value, String field, int rowIndex, String gridName) {
        boolean bad = value == null
                || (value instanceof Integer && (Integer) value == 0)
                || (value instanceof BigDecimal && ((BigDecimal) value).signum() <= 0)
                || (value instanceof Double && (Double) value <= 0)
                || (value instanceof String && ((String) value).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(field + " is required in " + gridName + " at row No: " + (rowIndex + 1));
    }

    @SuppressWarnings("unchecked")
    static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    @SuppressWarnings("unchecked")
    static Map<String, Object> map(Object v) { return v instanceof Map ? (Map<String, Object>) v : new LinkedHashMap<>(); }

    static String msg(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    interface Loader { Object load() throws Exception; }

    /** InitializeComponentMethod runs its reads together; one failing read is reported beside the others. */
    static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }

    static Map<String, Object> ok(int id, String message) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("message", message);
        return out;
    }

    /** GetOrdinal(number) of GdBankRequest. */
    static String ordinal(int number) {
        if (number <= 0) return String.valueOf(number);
        int num = number % 100;
        if (num - 11 >= 0 && num - 11 <= 2) return number + "th";
        switch (number % 10) {
            case 1: return number + "st";
            case 2: return number + "nd";
            case 3: return number + "rd";
            default: return number + "th";
        }
    }
}
