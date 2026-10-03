package com.mst.services;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Shared conversions for the Export contract screens ported in batch G (209 FrmExportSalesContract,
 * 879 frmExportContractIII, 216 frmSaleContractSchedule, 240 frmExportSalesContractPmDetail): the
 * desktop's Conversion.ToInt / ToDouble / ToBool / ToDateTime semantics, case-insensitive DataRow
 * reads and the JSON-body helpers. No business logic lives here.
 */
public final class ExportGSupport {

    private ExportGSupport() { }

    /** DataRow["Name"] - case-insensitive. */
    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    /** Conversion.ToString: null / DBNull -> "". */
    public static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    /** Conversion.ToInt: unparseable -> 0. */
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

    /** Conversion.ToBool: true/false text, else a number != 0, else false. */
    public static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return false;
        if (s.equalsIgnoreCase("true") || s.equalsIgnoreCase("on")) return true;
        if (s.equalsIgnoreCase("false")) return false;
        try { return Integer.parseInt(s) != 0; } catch (NumberFormatException e) { return false; }
    }

    /** Math.Round(value, digits, MidpointRounding.AwayFromZero). */
    public static double roundAwayFromZero(double v, int digits) {
        return BigDecimal.valueOf(v).setScale(digits, RoundingMode.HALF_UP).doubleValue();
    }

    @SuppressWarnings("unchecked")
    public static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    @SuppressWarnings("unchecked")
    public static Map<String, Object> map(Object v) {
        return v instanceof Map ? (Map<String, Object>) v : new LinkedHashMap<>();
    }

    /** yyyy-MM-dd of any date-ish value, "" for null. */
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

    /** yyyy-MM-ddTHH:mm:ss for the "dd-MM-yyyy hh:mm tt" history columns. */
    public static String isoDateTime(Object v) {
        if (v == null) return "";
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString();
        return String.valueOf(v).trim();
    }

    /** A page date (yyyy-MM-dd or ISO date-time) as LocalDate, null when blank / unparseable. */
    public static LocalDate asDate(Object v) {
        if (v == null) return null;
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate();
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        if (s.length() > 10) s = s.substring(0, 10);
        try { return LocalDate.parse(s); } catch (DateTimeParseException e) { return null; }
    }

    /** Conversion.CheckDateTimeNull: null, MinValue or 1900-01-01. */
    public static boolean dateNull(LocalDate d) {
        return d == null || d.getYear() <= 1900;
    }

    public static java.sql.Date sqlDate(LocalDate d) { return d == null ? null : java.sql.Date.valueOf(d); }

    /** A DateTime parameter: a page date -> midnight timestamp; null stays null (omitted). */
    public static Timestamp ts(Object v) {
        if (v instanceof Timestamp) return (Timestamp) v;
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        try { return Timestamp.valueOf(LocalDateTime.parse(s.length() == 10 ? s + "T00:00:00" : s.length() == 16 ? s + ":00" : s)); }
        catch (DateTimeParseException e) {
            try { return Timestamp.valueOf(s.replace('T', ' ')); }
            catch (IllegalArgumentException e2) { return null; }
        }
    }

    public static Timestamp now() { return Timestamp.valueOf(LocalDateTime.now().withNano(0)); }

    /** double.ToString(): an integral value prints without a decimal part. */
    public static String num(double d) {
        return d == Math.rint(d) && Math.abs(d) < 1e15 ? String.valueOf((long) d) : String.valueOf(d);
    }

    /** The innermost message of an exception chain (MessageBox.Show(ex.Message)). */
    public static String msg(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    /** Id / name pairs of a drop-down source, in row order. */
    public static List<Map<String, Object>> pairs(List<Map<String, Object>> rows, String idCol, String nameCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, idCol)));
            m.put("name", text(ci(r, nameCol)));
            out.add(m);
        }
        return out;
    }
}
