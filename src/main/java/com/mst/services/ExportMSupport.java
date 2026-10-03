package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.security.access.AccessDeniedException;

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
 * Logic-free helpers of the three Export screens of batch M:
 *   189 ExportOpening       "Export Opening Balance"  (/export/export-opening)
 *   190 ExportReturn_Grn    "Export Return GRN"       (/export/return-grn)
 *   191 ExportReturnInvoice "Export Return Invoice"   (/export/return-invoice)
 * The desktop's Conversion.ToInt / ToDouble / ToDecimal / ToBool semantics, case-insensitive DataRow reads,
 * page-date parsing and the CommonServices.SetRightsValueInRightsObject role rule. No business logic.
 */
public final class ExportMSupport {

    private ExportMSupport() { }

    /** DataRow["Name"], case-insensitive. */
    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    /** Conversion.ToString: null -> "". */
    public static String text(Object v) { return v == null ? "" : String.valueOf(v); }
    public static String trim(Object v) { return text(v).trim(); }

    /** Conversion.ToInt: unparseable -> 0 (thousand separators tolerated). */
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

    /** Conversion.ToDecimal: unparseable -> 0. */
    public static BigDecimal dec(Object v) {
        if (v == null) return BigDecimal.ZERO;
        if (v instanceof BigDecimal) return (BigDecimal) v;
        if (v instanceof Number) return BigDecimal.valueOf(((Number) v).doubleValue());
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return BigDecimal.ZERO;
        try { return new BigDecimal(s); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    /** Conversion.ToBool. */
    public static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = String.valueOf(v).trim();
        if (s.equalsIgnoreCase("true")) return true;
        if (s.equalsIgnoreCase("false") || s.isEmpty()) return false;
        try { return Double.parseDouble(s) != 0; } catch (NumberFormatException e) { return false; }
    }

    /** double.TryParse(text) && != 0 - FormHelper.ValidType.Double. */
    public static boolean nonZeroNumber(Object v) {
        String s = text(v).trim().replace(",", "");
        if (s.isEmpty()) return false;
        try { return Double.parseDouble(s) != 0; } catch (NumberFormatException e) { return false; }
    }

    /** Math.Round(x) / Math.Round(x, 0): banker's rounding (MidpointRounding.ToEven). */
    public static double roundEven(double v) { return Math.rint(v); }

    /** decimal.ToString("#,##0.<n>") value (AwayFromZero) - the number a formatted textbox holds. */
    public static BigDecimal roundTo(BigDecimal v, int digits) { return v.setScale(Math.max(digits, 0), RoundingMode.HALF_UP); }

    /** C# double/decimal ToString() for message texts: no exponent, no trailing zeros. */
    public static String num(Object v) {
        BigDecimal d = v instanceof BigDecimal ? (BigDecimal) v : BigDecimal.valueOf(asDouble(v));
        d = d.stripTrailingZeros();
        if (d.scale() < 0) d = d.setScale(0);
        return d.toPlainString();
    }

    @SuppressWarnings("unchecked")
    public static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    @SuppressWarnings("unchecked")
    public static Map<String, Object> map(Object v) { return v instanceof Map ? (Map<String, Object>) v : new LinkedHashMap<>(); }

    /** yyyy-MM-dd of a date-ish value, "" for null. */
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

    /** yyyy-MM-ddTHH:mm:ss, for the "dd-MM-yyyy hh:mm tt" columns. */
    public static String isoDateTime(Object v) {
        if (v == null) return "";
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString();
        return String.valueOf(v).trim();
    }

    /** A page date (yyyy-MM-dd or ISO date-time) as LocalDate; null when blank or unparseable. */
    public static LocalDate asDate(Object v) {
        if (v == null) return null;
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().toLocalDate();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate();
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        if (s.length() > 10) s = s.substring(0, 10);
        try { return LocalDate.parse(s); } catch (DateTimeParseException e) { return null; }
    }

    /** DateTimePicker.Value: the picked date with the current time of day (Conversion.ToDateTime(picker.Value)). */
    public static Timestamp pickerValue(Object v) {
        LocalDate d = asDate(v);
        if (d == null) d = LocalDate.now();
        return Timestamp.valueOf(d.atTime(LocalDateTime.now().toLocalTime().withNano(0)));
    }

    /** A filter date for a "date" parameter; null when not ticked / blank (Conversion.CheckDateTimeNull -> not sent). */
    public static java.sql.Date sqlDate(Object v) { LocalDate d = asDate(v); return d == null ? null : java.sql.Date.valueOf(d); }

    public static Timestamp now() { return Timestamp.valueOf(LocalDateTime.now().withNano(0)); }

    /** Innermost message (MessageBox.Show(ex.Message)). */
    public static String msg(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    /** FormHelper.ValidateField - "{field} is required in {grid} at row No: {n}". */
    public static void validateField(Object value, String fieldName, int rowIndex, String gridName) {
        boolean bad = value == null
                || (value instanceof Integer && (Integer) value == 0)
                || (value instanceof Double && (Double) value <= 0)
                || (value instanceof BigDecimal && ((BigDecimal) value).signum() <= 0)
                || (value instanceof String && ((String) value).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(fieldName + " is required in " + gridName + " at row No: " + (rowIndex + 1));
    }

    public static Map<String, Object> ok(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        return m;
    }

    // ------------------------------------------------------------------ rights

    /** CommonServices.SetRightsValueInRightsObject: RoleName "Admin" turns Save/Update/Print/Delete/CanViewAll on. */
    public static boolean isAdmin(CurrentUserContext ctx) {
        try { return "Admin".equalsIgnoreCase(ctx.currentRoleName()); } catch (RuntimeException e) { return false; }
    }

    public static boolean allowed(DesktopReportRights rights, UserAccount u, int screenId, String action) {
        try { rights.require(u, screenId, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    /** Admin, else the ScreenRights / tblUserRights chain for the action. */
    public static boolean right(CurrentUserContext ctx, DesktopReportRights rights, UserAccount u, int screenId, String action) {
        return isAdmin(ctx) || allowed(rights, u, screenId, action);
    }

    /** The configured "Default NoofDecimal Points For Amount" / "DefaultNoOfDecimalPointsForFcyAmount" as digits (1-4, else 0). */
    public static int digits(String configValue) {
        int n = asInt(configValue);
        return n >= 1 && n <= 4 ? n : 0;
    }
}
