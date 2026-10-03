package com.mst.services;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Collection;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;

/**
 * Value helpers shared by the three Export screens of this pass:
 *   200 Export Performa Invoice        (ExImProformaInvoice)                         /export/performa-invoice
 *   201 Export Delivery Order (New)    (ExportDeliveryOrderNew)                      /export/delivery-order-new
 *   202 Commercial Invoice (Transfer)  (CommiercialInvoiceAgainstPreInvoiceTransfer) /export/commercial-invoice-transfer
 * They reproduce Architecture.Common.Conversion (ToInt / ToDouble / ToString / ToDateTime never throw; a value that
 * does not parse is 0 / "" / DateTime.MinValue) so request values convert exactly as the desktop's text boxes did.
 */
public final class ExportInvoiceTransferSupport {

    private ExportInvoiceTransferSupport() { }

    public static final DateTimeFormatter DD_MMM_YYYY = DateTimeFormatter.ofPattern("dd-MMM-yyyy", Locale.ENGLISH);

    /** Case-insensitive DataRow lookup (rows from DesktopProc already are; request maps are not). */
    public static Object ci(Map<String, Object> r, String key) {
        if (r == null) return null;
        if (r.containsKey(key)) return r.get(key);
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey() != null && e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    /** Conversion.ToInt: numbers truncate toward zero, text must parse as a number ("12.0" -> 12), anything else 0. */
    public static int asInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        if (o instanceof Number) return ((Number) o).intValue();
        String s = String.valueOf(o).replace(",", "").trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); } catch (NumberFormatException e) { /* fall through */ }
        try { return (int) Math.round(Double.parseDouble(s)); } catch (NumberFormatException e) { return 0; }
    }

    public static long asLong(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).longValue();
        String s = String.valueOf(o).replace(",", "").trim();
        try { return Long.parseLong(s); } catch (NumberFormatException e) { /* fall through */ }
        try { return Math.round(Double.parseDouble(s)); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDouble. */
    public static double asDouble(Object o) {
        if (o == null) return 0;
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        if (o instanceof Number) return ((Number) o).doubleValue();
        String s = String.valueOf(o).replace(",", "").trim();
        if (s.isEmpty()) return 0;
        try { double d = Double.parseDouble(s); return Double.isFinite(d) ? d : 0; } catch (NumberFormatException e) { return 0; }
    }

    public static boolean asBool(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = String.valueOf(o).trim();
        return "true".equalsIgnoreCase(s) || "1".equals(s) || "yes".equalsIgnoreCase(s) || "on".equalsIgnoreCase(s);
    }

    /** Conversion.ToString: null -> "". */
    public static String text(Object o) {
        if (o == null) return "";
        if (o instanceof Double || o instanceof Float || o instanceof BigDecimal) return clr(asDouble(o));
        return String.valueOf(o);
    }

    /** A double printed the way .NET's Double.ToString() prints it (no trailing ".0"). */
    public static String clr(double d) {
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return new BigDecimal(Double.toString(d)).stripTrailingZeros().toPlainString();
    }

    /** yyyy-MM-dd of a SQL date / timestamp / ISO string; "" when there is none. */
    public static String iso(Object o) {
        if (o == null) return "";
        if (o instanceof Timestamp) return ((Timestamp) o).toLocalDateTime().toLocalDate().toString();
        if (o instanceof java.sql.Date) return ((java.sql.Date) o).toLocalDate().toString();
        if (o instanceof java.util.Date) return new Timestamp(((java.util.Date) o).getTime()).toLocalDateTime().toLocalDate().toString();
        if (o instanceof LocalDateTime) return ((LocalDateTime) o).toLocalDate().toString();
        if (o instanceof LocalDate) return o.toString();
        String s = String.valueOf(o).trim();
        return s.length() >= 10 && s.charAt(4) == '-' ? s.substring(0, 10) : s;
    }

    /** dd-MMM-yyyy (the desktop history grids' ToString("dd-MMM-yyyy")). */
    public static String ddMMMyyyy(Object o) {
        String s = iso(o);
        if (s.isEmpty()) return "";
        try { return LocalDate.parse(s).format(DD_MMM_YYYY); } catch (Exception e) { return s; }
    }

    /**
     * A DateTimePicker value: the picked date with the time of "now" (a WinForms DateTimePicker keeps the time part of
     * DateTime.Now it was initialised with). Missing / unparsable -> now.
     */
    public static Timestamp pickerDate(Object o) {
        String s = o == null ? "" : String.valueOf(o).trim();
        LocalTime t = LocalTime.now().withNano(0);
        if (s.length() >= 10) {
            try { return Timestamp.valueOf(LocalDate.parse(s.substring(0, 10)).atTime(t)); } catch (Exception e) { /* fall through */ }
        }
        return Timestamp.valueOf(LocalDateTime.now().withNano(0));
    }

    /** A date-only value (00:00), or null when missing. */
    public static Timestamp dateOnly(Object o) {
        String s = o == null ? "" : String.valueOf(o).trim();
        if (s.length() < 10) return null;
        try { return Timestamp.valueOf(LocalDate.parse(s.substring(0, 10)).atStartOfDay()); } catch (Exception e) { return null; }
    }

    public static Timestamp now() { return Timestamp.valueOf(LocalDateTime.now().withNano(0)); }

    @SuppressWarnings("unchecked")
    public static Map<String, Object> map(Object o) {
        if (o instanceof Map) return (Map<String, Object>) o;
        return new LinkedHashMap<>();
    }

    @SuppressWarnings("unchecked")
    public static List<Map<String, Object>> list(Object o) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (o instanceof Collection) for (Object x : (Collection<Object>) o) if (x instanceof Map) out.add((Map<String, Object>) x);
        return out;
    }

    public static Map<String, Object> row(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    /** Projection of DataTable rows to the listed columns (display combos). */
    public static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String... cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String c : cols) {
                Object v = ci(r, c);
                if (v instanceof Timestamp || v instanceof java.sql.Date) v = iso(v);
                m.put(c, v);
            }
            out.add(m);
        }
        return out;
    }

    /** Whole row, dates as ISO, keys as the procedure returned them. */
    public static Map<String, Object> plain(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        if (r == null) return m;
        for (Map.Entry<String, Object> e : r.entrySet()) {
            Object v = e.getValue();
            if (v instanceof Timestamp || v instanceof java.sql.Date) v = iso(v);
            else if (v instanceof BigDecimal) v = ((BigDecimal) v).doubleValue();
            else if (v instanceof byte[]) v = null;
            m.put(e.getKey(), v);
        }
        return m;
    }

    public static List<Map<String, Object>> plain(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(plain(r));
        return out;
    }

    /** .NET Math.Round(x, d) (banker's rounding, MidpointRounding.ToEven). */
    public static double roundEven(double v, int d) {
        if (!Double.isFinite(v)) return v;
        return new BigDecimal(Double.toString(v)).setScale(d, java.math.RoundingMode.HALF_EVEN).doubleValue();
    }

    public static String innermost(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }
}
