package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.security.DesktopReportRights;
import org.springframework.security.access.AccessDeniedException;

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
 * Conversions shared by the Export batch-Q pages (207 ExImfrmDefineDocuments, DefineExportCharges,
 * DefineThirdPartyType, frmGenerateExportContractNos, GdContainerBreakUp,
 * frmStockReservedAgainstThirdPartyInspection, frmFIOpening): the desktop's Conversion.ToInt / ToDouble /
 * ToDecimal / ToBool semantics, case-insensitive DataRow reads, date helpers and the "any of these parent
 * screens" rights check used by the popup forms that have no ScreenDefinition row of their own.
 * No business logic lives here.
 */
public final class ExportQSupport {

    private ExportQSupport() { }

    /**
     * The popup forms carry no rights code on the desktop and most have no ScreenDefinition row; they open
     * from a parent screen. The web lets a user in when the parent screen grants the action (first match
     * wins); the last denial is re-thrown otherwise.
     */
    public static void requireAny(DesktopReportRights rights, UserAccount u, int[] screenIds, String action) {
        AccessDeniedException last = null;
        for (int id : screenIds) {
            try { rights.require(u, id, action); return; }
            catch (AccessDeniedException e) { last = e; }
        }
        if (last != null) throw last;
    }

    public static boolean allowedAny(DesktopReportRights rights, UserAccount u, int[] screenIds, String action) {
        try { requireAny(rights, u, screenIds, action); return true; }
        catch (AccessDeniedException e) { return false; }
    }

    /** DataRow["Name"] - case-insensitive. */
    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    public static boolean has(Map<String, Object> row, String name) {
        if (row == null) return false;
        if (row.containsKey(name)) return true;
        for (String k : row.keySet()) if (k.equalsIgnoreCase(name)) return true;
        return false;
    }

    /** Conversion.ToString. */
    public static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    /** TextBox.Text as typed (not trimmed). */
    public static String raw(Object v) { return v == null ? "" : String.valueOf(v); }

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

    /** Conversion.ToBool. */
    public static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = String.valueOf(v).trim();
        if (s.equalsIgnoreCase("true") || s.equalsIgnoreCase("on")) return true;
        try { return Integer.parseInt(s) != 0; } catch (NumberFormatException e) { return false; }
    }

    public static LocalDate asDate(Object v) {
        if (v == null) return null;
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate();
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().toLocalDate();
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        if (s.length() > 10) s = s.substring(0, 10);
        try { return LocalDate.parse(s); } catch (DateTimeParseException e) { return null; }
    }

    /** DateTimePicker.Value (never empty): yyyy-MM-dd[THH:mm[:ss]] -> Timestamp; blank -> now. */
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

    public static Timestamp now() { return new Timestamp(System.currentTimeMillis()); }

    /** yyyy-MM-dd. */
    public static String iso(Object v) {
        if (v == null) return "";
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate().toString();
        if (v instanceof LocalDate) return v.toString();
        String s = String.valueOf(v).trim();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    /** yyyy-MM-ddTHH:mm:ss. */
    public static String isoDateTime(Object v) {
        if (v == null) return "";
        LocalDateTime t = null;
        if (v instanceof Timestamp) t = ((Timestamp) v).toLocalDateTime();
        else if (v instanceof java.sql.Date) t = ((java.sql.Date) v).toLocalDate().atStartOfDay();
        else if (v instanceof java.util.Date) t = new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime();
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

    public static String msg(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    public interface Loader { Object load() throws Exception; }

    /** Initial loads run together; one failing read is reported beside the others (key + "Error"). */
    public static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }

    /** Projection: {target: sourceColumn} pairs; dates as ISO strings when asked. */
    public static List<Map<String, Object>> project(List<Map<String, Object>> rows, String... pairs) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (int i = 0; i + 1 < pairs.length; i += 2) {
                Object v = ci(r, pairs[i + 1]);
                if (v instanceof java.util.Date || v instanceof LocalDateTime) v = isoDateTime(v);
                if (v instanceof BigDecimal) v = ((BigDecimal) v).doubleValue();
                m.put(pairs[i], v);
            }
            out.add(m);
        }
        return out;
    }
}
