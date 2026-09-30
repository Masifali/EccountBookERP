package com.mst.services.hrm;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Component;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.Date;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Objects;

/**
 * Shared BLL plumbing for every HRM service (dbo.App 12 "HRM").
 *
 * Tenancy and audit (Organization, Company, Branch, user id, dates) always come from the signed-in
 * user here - never from the request. Every page needs View on its own dbo.ScreenDefinition row, as
 * the menu tile that opens it does; Save / Update / Delete / Print are checked where the desktop form
 * checks them (SetRightsValueInRightsObject), and {@link #rights} hands the page the same flags so it
 * can enable or hide the buttons exactly as the form does.
 *
 * The static helpers reproduce the desktop's Conversion.* semantics (ToInt / ToDecimal / ToString /
 * CheckDateTimeNull) so a value the desktop would read as 0 is read as 0 here too.
 */
@Component
public class HrmSupport {

    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    /** Signed-in user with View on the screen. */
    public UserAccount user(int screenId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, screenId, "View");
        return u;
    }

    /** Signed-in user, no screen check (sub-forms that are opened from another screen). */
    public UserAccount user() {
        return currentUserContext.requireAccountingUser();
    }

    public void require(UserAccount u, int screenId, String action) {
        rights.require(u, screenId, action);
    }

    public boolean can(UserAccount u, int screenId, String action) {
        try { rights.require(u, screenId, action); return true; } catch (AccessDeniedException e) { return false; }
    }

    /** { save, update, delete, print } for the page (desktop RightsObject). */
    public Map<String, Object> rights(UserAccount u, int screenId) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("save", can(u, screenId, "Save"));
        r.put("update", can(u, screenId, "Update"));
        r.put("delete", can(u, screenId, "Delete"));
        r.put("print", can(u, screenId, "Print"));
        return r;
    }

    public int financialYearId() {
        try { return currentUserContext.currentFinancialYearId(); } catch (RuntimeException e) { return 0; }
    }

    // ------------------------------------------------------------------ Conversion.*

    /** Conversion.ToInt: 0 for null / blank / not a whole number (decimals are truncated as Convert.ToInt32 of a decimal string fails -> 0). */
    public static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        if (v instanceof Number) return ((Number) v).intValue();
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); } catch (NumberFormatException e) {
            try { return new BigDecimal(s.replace(",", "")).intValue(); } catch (NumberFormatException e2) { return 0; }
        }
    }

    /** Conversion.ToDecimal: 0 for null / blank / not numeric. */
    public static BigDecimal toDec(Object v) {
        if (v == null) return BigDecimal.ZERO;
        if (v instanceof BigDecimal) return (BigDecimal) v;
        if (v instanceof Number) return BigDecimal.valueOf(((Number) v).doubleValue());
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return BigDecimal.ZERO;
        try { return new BigDecimal(s); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    public static double toDouble(Object v) { return toDec(v).doubleValue(); }

    /** Conversion.ToBoolean: true for true / 1 / "true" / "1" / "yes" (case-insensitive). */
    public static boolean toBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = String.valueOf(v).trim().toLowerCase();
        return s.equals("true") || s.equals("1") || s.equals("yes") || s.equals("y");
    }

    /** Conversion.ToString: "" for null. */
    public static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    /** Text as typed, trimmed (TextBox.Text.Trim()). */
    public static String trim(Object v) { return str(v).trim(); }

    /** Null when blank - for C# string properties the form leaves unset. */
    public static String nz(Object v) { String s = str(v); return s.isEmpty() ? null : s; }

    /** Parses yyyy-MM-dd, yyyy-MM-ddTHH:mm[:ss], dd-MMM-yyyy, dd/MM/yyyy; null when blank or unparseable. */
    public static LocalDateTime toDate(Object v) {
        if (v == null) return null;
        if (v instanceof LocalDateTime) return (LocalDateTime) v;
        if (v instanceof LocalDate) return ((LocalDate) v).atStartOfDay();
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime();
        if (v instanceof Date) return new Timestamp(((Date) v).getTime()).toLocalDateTime();
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        try {
            if (s.length() >= 16 && s.charAt(10) == 'T') {
                String t = s.length() > 19 ? s.substring(0, 19) : s;
                if (t.length() == 16) t = t + ":00";
                return LocalDateTime.parse(t);
            }
            if (s.length() >= 19 && s.charAt(10) == ' ') return LocalDateTime.parse(s.substring(0, 19).replace(' ', 'T'));
            if (s.matches("\\d{4}-\\d{2}-\\d{2}.*")) return LocalDate.parse(s.substring(0, 10)).atStartOfDay();
            if (s.matches("\\d{1,2}-[A-Za-z]{3}-\\d{4}")) return LocalDate.parse(s, DateTimeFormatter.ofPattern("d-MMM-yyyy", java.util.Locale.ENGLISH)).atStartOfDay();
            if (s.matches("\\d{1,2}/\\d{1,2}/\\d{4}")) return LocalDate.parse(s, DateTimeFormatter.ofPattern("d/M/yyyy")).atStartOfDay();
        } catch (RuntimeException e) { return null; }
        return null;
    }

    /** Date part only (DateTimePicker.Value.Date). */
    public static LocalDateTime toDay(Object v) {
        LocalDateTime d = toDate(v);
        return d == null ? null : d.toLocalDate().atStartOfDay();
    }

    public static Timestamp ts(LocalDateTime d) { return d == null ? null : Timestamp.valueOf(d); }

    public static BigDecimal round(BigDecimal v, int places) { return v == null ? null : v.setScale(places, RoundingMode.HALF_EVEN); }

    // ------------------------------------------------------------------ result helpers

    /** First row or an exception - BLL GetByID's [0] (the desktop throws "Index was out of range"). */
    public static Map<String, Object> one(List<Map<String, Object>> rows) {
        if (rows == null || rows.isEmpty()) throw new IllegalArgumentException("Record not found.");
        return rows.get(0);
    }

    /** true when rows contain a row whose idKey equals id (tenancy guard for an id sent by the page). */
    public static boolean owns(List<Map<String, Object>> rows, String idKey, int id) {
        if (rows == null) return false;
        for (Map<String, Object> r : rows) if (toInt(r.get(idKey)) == id) return true;
        return false;
    }

    public static Map<String, Object> saved(int id, String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", true);
        r.put("id", id);
        if (message != null) r.put("message", message);
        return r;
    }

    public static Map<String, Object> map(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    public static boolean same(Object a, Object b) { return Objects.equals(str(a), str(b)); }

    /** IllegalArgumentException carrying the desktop MessageBox text - the controller returns it as a 400. */
    public static IllegalArgumentException invalid(String desktopMessage) { return new IllegalArgumentException(desktopMessage); }
}
