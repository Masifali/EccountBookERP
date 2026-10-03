package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
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
 * Shared helpers of the seven "Export document Tracking" (module 130, Architecture.WinApp.SDT) screens:
 * the Rightsobjects lookups the desktop forms make through CommonServices.SetRightsValueInRightsObject,
 * and the Conversion.* semantics (ToInt / ToDouble / ToBool / ToDateTime with the 1900-01-01 null date)
 * every form applies to its grid cells before a value reaches a procedure.
 *
 * Tenancy (OrganizationId, CompanyId, user id) is always the session's, never the request's.
 */
final class ExportSdtSupport {

    /** Conversion.ToDateTime(null / "" / unparseable) - the desktop's "no date". */
    static final Timestamp NULL_DATE = Timestamp.valueOf("1900-01-01 00:00:00");

    private ExportSdtSupport() { }

    // ------------------------------------------------------------------ rights

    /** CommonServices.SetRightsValueInRightsObject + the action check; throws 403 when the right is missing. */
    static UserAccount user(CurrentUserContext ctx, DesktopReportRights rights, int screenId, String action) {
        UserAccount u = ctx.requireAccountingUser();
        rights.require(u, screenId, action);
        return u;
    }

    static boolean allowed(DesktopReportRights rights, UserAccount u, int screenId, String action) {
        try { rights.require(u, screenId, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    /** Rightsobjects.DoHaveCanViewAllRecordRights: Admin always, else the "CanView AllRecord" right. */
    static boolean canViewAll(CurrentUserContext ctx, DesktopReportRights rights, UserAccount u, int screenId) {
        String role = ctx.currentRoleName();
        if ("Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role)) return true;
        return allowed(rights, u, screenId, "CanView AllRecord");
    }

    // ------------------------------------------------------------------ Conversion.*

    static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    /** Conversion.ToInt - unparseable text is 0. */
    static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        if (v instanceof Number) return ((Number) v).intValue();
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); }
        catch (NumberFormatException e) {
            try { return (int) Double.parseDouble(s); }
            catch (NumberFormatException e2) { return 0; }
        }
    }

    static long asLong(Object v) {
        if (v == null) return 0L;
        if (v instanceof Number) return ((Number) v).longValue();
        try { return Long.parseLong(String.valueOf(v).trim().replace(",", "")); }
        catch (NumberFormatException e) { return asInt(v); }
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

    /** Conversion.ToBool: true / 1 / "true" / "on". */
    static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = String.valueOf(v).trim().toLowerCase();
        return "true".equals(s) || "1".equals(s) || "on".equals(s) || "yes".equals(s);
    }

    /** yyyy-MM-dd of an ISO string / driver value; null when blank. */
    static LocalDate asDate(Object v) {
        if (v == null) return null;
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate();
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().toLocalDate();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate();
        if (v instanceof LocalDate) return (LocalDate) v;
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate();
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        if (s.length() > 10) s = s.substring(0, 10);
        try { return LocalDate.parse(s); }
        catch (DateTimeParseException e) {
            /* M/d/yyyy (ToShortDateString under the desktop's culture) */
            String[] p = s.split("/");
            if (p.length == 3) {
                try { return LocalDate.of(Integer.parseInt(p[2].trim()), Integer.parseInt(p[0].trim()), Integer.parseInt(p[1].trim())); }
                catch (RuntimeException e2) { return null; }
            }
            return null;
        }
    }

    static java.sql.Date sqlDate(Object v) {
        LocalDate d = asDate(v);
        return d == null ? null : java.sql.Date.valueOf(d);
    }

    /** Conversion.ToDateTime: blank / unparseable -> 1900-01-01 (what the desktop then binds). */
    static Timestamp tsOrNullDate(Object v) {
        LocalDate d = asDate(v);
        return d == null ? NULL_DATE : Timestamp.valueOf(d.atStartOfDay());
    }

    /** Conversion.CheckDateTimeNull. */
    static boolean isNullDate(Object v) {
        LocalDate d = asDate(v);
        return d == null || d.getYear() == 1900 && d.getMonthValue() == 1 && d.getDayOfMonth() == 1 || d.getYear() == 1;
    }

    static Timestamp now() { return new Timestamp(System.currentTimeMillis()); }

    /** yyyy-MM-dd for the page. */
    static String iso(Object v) {
        LocalDate d = asDate(v);
        return d == null ? "" : d.toString();
    }

    /** yyyy-MM-ddTHH:mm:ss for the page (the JS formats "dd-MMM-yyyy hh:mm tt"). */
    static String isoDateTime(Object v) {
        if (v == null) return "";
        LocalDateTime dt = null;
        if (v instanceof Timestamp) dt = ((Timestamp) v).toLocalDateTime();
        else if (v instanceof java.sql.Date) dt = ((java.sql.Date) v).toLocalDate().atStartOfDay();
        else if (v instanceof java.util.Date) dt = new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime();
        else if (v instanceof LocalDateTime) dt = (LocalDateTime) v;
        else if (v instanceof LocalDate) dt = ((LocalDate) v).atStartOfDay();
        if (dt != null) return dt.withNano(0).toString();
        String s = String.valueOf(v).trim();
        return s.length() >= 19 ? s.substring(0, 19).replace(' ', 'T') : s;
    }

    @SuppressWarnings("unchecked")
    static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    /** Case-insensitive column read (DataRow["name"]). */
    static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    static Map<String, Object> ok(String message, Object id) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", message);
        if (id != null) out.put("id", id);
        return out;
    }

    static String rootMessage(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    /** The "Id / name" DataTable the forms build from a DropDownFill row set for one Activity. */
    static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String activity) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (!activity.equals(text(ci(r, "Activity")))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("name", text(ci(r, "ReferenceName")));
            out.add(m);
        }
        return out;
    }

    /** CommonServices.DateType() - the fixed DataTable of the date-type combo. */
    static List<Map<String, Object>> dateTypes() {
        String[] names = { "This Day", "This Week", "This Month", "This Year", "Financial Year" };
        List<Map<String, Object>> out = new ArrayList<>();
        for (int i = 0; i < names.length; i++) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", i + 1);
            m.put("Parameters", names[i]);
            out.add(m);
        }
        return out;
    }

    // ------------------------------------------------------------------ CommonServices.ColorNameToHexCode

    private static final Map<String, String> KNOWN_COLORS = new LinkedHashMap<>();
    static {
        String[] k = {
            "AliceBlue", "F0F8FF", "AntiqueWhite", "FAEBD7", "Aqua", "00FFFF", "Aquamarine", "7FFFD4", "Azure", "F0FFFF",
            "Beige", "F5F5DC", "Bisque", "FFE4C4", "Black", "000000", "BlanchedAlmond", "FFEBCD", "Blue", "0000FF",
            "BlueViolet", "8A2BE2", "Brown", "A52A2A", "BurlyWood", "DEB887", "CadetBlue", "5F9EA0", "Chartreuse", "7FFF00",
            "Chocolate", "D2691E", "Coral", "FF7F50", "CornflowerBlue", "6495ED", "Cornsilk", "FFF8DC", "Crimson", "DC143C",
            "Cyan", "00FFFF", "DarkBlue", "00008B", "DarkCyan", "008B8B", "DarkGoldenrod", "B8860B", "DarkGray", "A9A9A9",
            "DarkGreen", "006400", "DarkKhaki", "BDB76B", "DarkMagenta", "8B008B", "DarkOliveGreen", "556B2F", "DarkOrange", "FF8C00",
            "DarkOrchid", "9932CC", "DarkRed", "8B0000", "DarkSalmon", "E9967A", "DarkSeaGreen", "8FBC8B", "DarkSlateBlue", "483D8B",
            "DarkSlateGray", "2F4F4F", "DarkTurquoise", "00CED1", "DarkViolet", "9400D3", "DeepPink", "FF1493", "DeepSkyBlue", "00BFFF",
            "DimGray", "696969", "DodgerBlue", "1E90FF", "Firebrick", "B22222", "FloralWhite", "FFFAF0", "ForestGreen", "228B22",
            "Fuchsia", "FF00FF", "Gainsboro", "DCDCDC", "GhostWhite", "F8F8FF", "Gold", "FFD700", "Goldenrod", "DAA520",
            "Gray", "808080", "Green", "008000", "GreenYellow", "ADFF2F", "Honeydew", "F0FFF0", "HotPink", "FF69B4",
            "IndianRed", "CD5C5C", "Indigo", "4B0082", "Ivory", "FFFFF0", "Khaki", "F0E68C", "Lavender", "E6E6FA",
            "LavenderBlush", "FFF0F5", "LawnGreen", "7CFC00", "LemonChiffon", "FFFACD", "LightBlue", "ADD8E6", "LightCoral", "F08080",
            "LightCyan", "E0FFFF", "LightGoldenrodYellow", "FAFAD2", "LightGray", "D3D3D3", "LightGreen", "90EE90", "LightPink", "FFB6C1",
            "LightSalmon", "FFA07A", "LightSeaGreen", "20B2AA", "LightSkyBlue", "87CEFA", "LightSlateGray", "778899", "LightSteelBlue", "B0C4DE",
            "LightYellow", "FFFFE0", "Lime", "00FF00", "LimeGreen", "32CD32", "Linen", "FAF0E6", "Magenta", "FF00FF",
            "Maroon", "800000", "MediumAquamarine", "66CDAA", "MediumBlue", "0000CD", "MediumOrchid", "BA55D3", "MediumPurple", "9370DB",
            "MediumSeaGreen", "3CB371", "MediumSlateBlue", "7B68EE", "MediumSpringGreen", "00FA9A", "MediumTurquoise", "48D1CC", "MediumVioletRed", "C71585",
            "MidnightBlue", "191970", "MintCream", "F5FFFA", "MistyRose", "FFE4E1", "Moccasin", "FFE4B5", "NavajoWhite", "FFDEAD",
            "Navy", "000080", "OldLace", "FDF5E6", "Olive", "808000", "OliveDrab", "6B8E23", "Orange", "FFA500",
            "OrangeRed", "FF4500", "Orchid", "DA70D6", "PaleGoldenrod", "EEE8AA", "PaleGreen", "98FB98", "PaleTurquoise", "AFEEEE",
            "PaleVioletRed", "DB7093", "PapayaWhip", "FFEFD5", "PeachPuff", "FFDAB9", "Peru", "CD853F", "Pink", "FFC0CB",
            "Plum", "DDA0DD", "PowderBlue", "B0E0E6", "Purple", "800080", "Red", "FF0000", "RosyBrown", "BC8F8F",
            "RoyalBlue", "4169E1", "SaddleBrown", "8B4513", "Salmon", "FA8072", "SandyBrown", "F4A460", "SeaGreen", "2E8B57",
            "SeaShell", "FFF5EE", "Sienna", "A0522D", "Silver", "C0C0C0", "SkyBlue", "87CEEB", "SlateBlue", "6A5ACD",
            "SlateGray", "708090", "Snow", "FFFAFA", "SpringGreen", "00FF7F", "SteelBlue", "4682B4", "Tan", "D2B48C",
            "Teal", "008080", "Thistle", "D8BFD8", "Tomato", "FF6347", "Turquoise", "40E0D0", "Violet", "EE82EE",
            "Wheat", "F5DEB3", "White", "FFFFFF", "WhiteSmoke", "F5F5F5", "Yellow", "FFFF00", "YellowGreen", "9ACD32"
        };
        for (int i = 0; i + 1 < k.length; i += 2) KNOWN_COLORS.put(k[i].toLowerCase(), k[i + 1]);
    }

    /**
     * CommonServices.ColorNameToHexCode: Color.FromName(name); a system colour or an unknown name
     * (A == 0, which is also what a "#RRGGBB" text gives) -> "#000000", else "#RRGGBB".
     */
    static String colorNameToHex(String colorName) {
        String hex = colorName == null ? null : KNOWN_COLORS.get(colorName.trim().toLowerCase());
        return hex == null ? "#000000" : "#" + hex;
    }
}
