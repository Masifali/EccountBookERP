package com.mst.reports.jasper;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.text.DecimalFormat;
import java.text.DecimalFormatSymbols;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Collection;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;

/**
 * Crystal formula runtime for converted .jrxml templates.
 *
 * rpt2jrxml.py translates Crystal formula syntax into Java expressions that call these static
 * methods. Every value travels as Object - BigDecimal for numbers, String, java.util.Date,
 * Boolean - so a translated formula never needs Java's static typing to match Crystal's dynamic
 * one. The semantics follow Crystal's documented behaviour: 1-based string positions, "&"
 * concatenation through ToText, NULL propagating through arithmetic, ToText's default of two
 * decimals with a thousands separator.
 *
 * Crystal variables (NumberVar x := ...) live in a Vars map that the renderer puts into the
 * report as $P{CR_VARS}, shared with sub-reports so Shared variables work as they do in Crystal.
 */
public final class CR {

    private CR() { }

    public static final Locale LOCALE = Locale.US;

    // ------------------------------------------------------------------ variables
    public static final class Vars extends HashMap<String, Object> {
        private static final long serialVersionUID = 1L;
    }

    public static Object get(Map<String, Object> vars, String key, Object dflt) {
        if (vars == null) return dflt;
        if (!vars.containsKey(key)) { vars.put(key, dflt); return dflt; }
        return vars.get(key);
    }

    /** Declaration without assignment keeps an existing value (Crystal semantics for globals). */
    public static Object declare(Map<String, Object> vars, String key, Object dflt) {
        return get(vars, key, dflt);
    }

    public static Object set(Map<String, Object> vars, String key, Object value) {
        if (vars != null) vars.put(key, value);
        return value;
    }

    /** A statement sequence: every argument has already been evaluated left to right. */
    public static Object seq(Object... values) {
        return values.length == 0 ? null : values[values.length - 1];
    }

    // ------------------------------------------------------------------ coercions
    public static BigDecimal num(Object o) {
        if (o == null) return null;
        if (o instanceof BigDecimal) return (BigDecimal) o;
        if (o instanceof Integer || o instanceof Long || o instanceof Short || o instanceof Byte)
            return BigDecimal.valueOf(((Number) o).longValue());
        if (o instanceof Number) {
            double d = ((Number) o).doubleValue();
            if (Double.isNaN(d) || Double.isInfinite(d)) return null;
            return new BigDecimal(Double.toString(d));
        }
        if (o instanceof Boolean) return ((Boolean) o) ? BigDecimal.ONE : BigDecimal.ZERO;
        String s = String.valueOf(o).trim().replace(",", "");
        if (s.isEmpty()) return null;
        try { return new BigDecimal(s); } catch (NumberFormatException e) { return null; }
    }

    /** Numeric value for arithmetic where Crystal would treat NULL as zero (converted nulls). */
    public static BigDecimal nz(Object o) {
        BigDecimal b = num(o);
        return b == null ? BigDecimal.ZERO : b;
    }

    public static String str(Object o) {
        if (o == null) return null;
        if (o instanceof String) return (String) o;
        return toText(o);
    }

    public static Date date(Object o) {
        if (o == null) return null;
        if (o instanceof Date) return (Date) o;
        if (o instanceof java.time.LocalDate) return java.sql.Date.valueOf((java.time.LocalDate) o);
        if (o instanceof java.time.LocalDateTime) return java.sql.Timestamp.valueOf((java.time.LocalDateTime) o);
        String s = String.valueOf(o).trim();
        if (s.isEmpty()) return null;
        String[] fmts = { "yyyy-MM-dd HH:mm:ss.S", "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd'T'HH:mm:ss", "yyyy-MM-dd",
                          "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy", "dd-MMM-yyyy", "dd-MM-yyyy", "MM/dd/yyyy" };
        for (String f : fmts) {
            try {
                SimpleDateFormat p = new SimpleDateFormat(f, LOCALE);
                p.setLenient(false);
                return p.parse(s);
            } catch (Exception ignored) { }
        }
        return null;
    }

    public static boolean bool(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).doubleValue() != 0;
        String s = String.valueOf(o).trim();
        return s.equalsIgnoreCase("true") || s.equals("1") || s.equalsIgnoreCase("yes");
    }

    /** Field value as Crystal sees it: numbers normalised to BigDecimal. */
    public static Object v(Object o) {
        if (o instanceof Number && !(o instanceof BigDecimal)) return num(o);
        if (o instanceof java.time.LocalDate || o instanceof java.time.LocalDateTime) return date(o);
        return o;
    }

    // ------------------------------------------------------------------ operators
    public static Object add(Object a, Object b) {
        a = v(a); b = v(b);
        if (a == null || b == null) return null;
        if (a instanceof Date && b instanceof BigDecimal) return addDays((Date) a, (BigDecimal) b);
        if (b instanceof Date && a instanceof BigDecimal) return addDays((Date) b, (BigDecimal) a);
        if (a instanceof String || b instanceof String) return str(a) + str(b);
        return nz(a).add(nz(b));
    }

    public static Object sub(Object a, Object b) {
        a = v(a); b = v(b);
        if (a == null || b == null) return null;
        if (a instanceof Date && b instanceof Date)
            return BigDecimal.valueOf((((Date) a).getTime() - ((Date) b).getTime()) / 86400000L);
        if (a instanceof Date) return addDays((Date) a, nz(b).negate());
        return nz(a).subtract(nz(b));
    }

    public static Object mul(Object a, Object b) {
        BigDecimal x = num(a), y = num(b);
        return x == null || y == null ? null : x.multiply(y);
    }

    public static Object div(Object a, Object b) {
        BigDecimal x = num(a), y = num(b);
        if (x == null || y == null || y.signum() == 0) return null;   // Crystal: "Division by zero"
        return x.divide(y, 12, RoundingMode.HALF_UP).stripTrailingZeros();
    }

    public static Object idiv(Object a, Object b) {
        BigDecimal x = num(a), y = num(b);
        if (x == null || y == null || y.signum() == 0) return null;
        return x.divideToIntegralValue(y);
    }

    public static Object mod(Object a, Object b) {
        BigDecimal x = num(a), y = num(b);
        if (x == null || y == null || y.signum() == 0) return null;
        return x.remainder(y);
    }

    public static Object pow(Object a, Object b) {
        BigDecimal x = num(a), y = num(b);
        if (x == null || y == null) return null;
        return new BigDecimal(Double.toString(Math.pow(x.doubleValue(), y.doubleValue())));
    }

    public static Object neg(Object a) { BigDecimal x = num(a); return x == null ? null : x.negate(); }

    public static Object pct(Object a, Object b) { return mul(div(a, b), BigDecimal.valueOf(100)); }

    /** Crystal "&": always a string; each side through ToText. NULL on either side yields NULL. */
    public static String cat(Object a, Object b) {
        if (a == null || b == null) return null;
        return str(v(a)) + str(v(b));
    }

    public static int cmp(Object a, Object b) {
        a = v(a); b = v(b);
        if (a instanceof BigDecimal && b instanceof BigDecimal) return ((BigDecimal) a).compareTo((BigDecimal) b);
        if (a instanceof Date && b instanceof Date) return ((Date) a).compareTo((Date) b);
        if (a instanceof Boolean && b instanceof Boolean) return ((Boolean) a).compareTo((Boolean) b);
        if (a instanceof BigDecimal || b instanceof BigDecimal) {
            BigDecimal x = num(a), y = num(b);
            if (x != null && y != null) return x.compareTo(y);
        }
        return String.valueOf(a).compareTo(String.valueOf(b));
    }

    public static Boolean eq(Object a, Object b) { return a == null || b == null ? Boolean.FALSE : cmp(a, b) == 0; }
    public static Boolean ne(Object a, Object b) { return a == null || b == null ? Boolean.FALSE : cmp(a, b) != 0; }
    public static Boolean lt(Object a, Object b) { return a == null || b == null ? Boolean.FALSE : cmp(a, b) < 0; }
    public static Boolean le(Object a, Object b) { return a == null || b == null ? Boolean.FALSE : cmp(a, b) <= 0; }
    public static Boolean gt(Object a, Object b) { return a == null || b == null ? Boolean.FALSE : cmp(a, b) > 0; }
    public static Boolean ge(Object a, Object b) { return a == null || b == null ? Boolean.FALSE : cmp(a, b) >= 0; }

    public static Boolean in(Object x, Object list) {
        if (x == null) return Boolean.FALSE;
        if (list instanceof Range) { Range r = (Range) list; return ge(x, r.lo) && le(x, r.hi); }
        if (list instanceof Object[]) { for (Object o : (Object[]) list) if (bool(in(x, o))) return Boolean.TRUE; return Boolean.FALSE; }
        if (list instanceof Collection) { for (Object o : (Collection<?>) list) if (bool(in(x, o))) return Boolean.TRUE; return Boolean.FALSE; }
        if (list instanceof String && x instanceof String) return ((String) list).contains((String) x);
        return eq(x, list);
    }

    public static final class Range { final Object lo, hi; Range(Object lo, Object hi) { this.lo = lo; this.hi = hi; } }
    public static Range range(Object lo, Object hi) { return new Range(lo, hi); }
    public static Object[] arr(Object... xs) { return xs; }

    /** s[i] - 1-based character, or array element. */
    public static Object idx(Object s, Object i) {
        BigDecimal n = num(i);
        if (s == null || n == null) return null;
        int k = n.intValue() - 1;
        if (s instanceof Object[]) { Object[] a = (Object[]) s; return k >= 0 && k < a.length ? a[k] : null; }
        String t = str(s);
        return k >= 0 && k < t.length() ? String.valueOf(t.charAt(k)) : "";
    }

    /** Crystal "like": ? one char, * any run; case-insensitive. */
    public static Boolean like(Object s, Object pattern) {
        if (s == null || pattern == null) return Boolean.FALSE;
        StringBuilder re = new StringBuilder("(?is)");
        for (char c : str(pattern).toCharArray()) {
            if (c == '*') re.append(".*"); else if (c == '?') re.append('.');
            else re.append(java.util.regex.Pattern.quote(String.valueOf(c)));
        }
        return str(s).matches(re.toString());
    }

    public static Boolean startsWith(Object s, Object p) { return s != null && p != null && str(s).startsWith(str(p)); }

    // ------------------------------------------------------------------ ToText family
    public static String toText(Object o) {
        o = v(o);
        if (o == null) return null;
        if (o instanceof BigDecimal) return fmtNum((BigDecimal) o, 2, ",", ".");
        if (o instanceof Date) return fmtDate((Date) o, hasTime((Date) o) ? "dd/MM/yyyy hh:mm:ss a" : "dd/MM/yyyy");
        if (o instanceof Boolean) return ((Boolean) o) ? "True" : "False";
        return String.valueOf(o);
    }

    public static String toText(Object o, Object a) {
        o = v(o);
        if (o == null) return null;
        if (o instanceof BigDecimal && a instanceof BigDecimal) return fmtNum((BigDecimal) o, ((BigDecimal) a).intValue(), ",", ".");
        if (o instanceof BigDecimal && a instanceof String) return fmtNumPattern((BigDecimal) o, (String) a);
        if (o instanceof Date) return fmtDate((Date) o, crystalDatePattern(str(a)));
        if (o instanceof Boolean) return toText(o);
        return toText(o);
    }

    public static String toText(Object o, Object dec, Object thousands) {
        o = v(o);
        if (o instanceof BigDecimal) return fmtNum((BigDecimal) o, nz(dec).intValue(), str(thousands), ".");
        return toText(o, dec);
    }

    public static String toText(Object o, Object dec, Object thousands, Object decSep) {
        o = v(o);
        if (o instanceof BigDecimal) return fmtNum((BigDecimal) o, nz(dec).intValue(), str(thousands), str(decSep));
        return toText(o, dec);
    }

    static String fmtNum(BigDecimal n, int dec, String thousands, String decSep) {
        BigDecimal r = n.setScale(Math.max(0, dec), RoundingMode.HALF_UP);
        String plain = r.abs().toPlainString();
        String ip = plain, fp = "";
        int dot = plain.indexOf('.');
        if (dot >= 0) { ip = plain.substring(0, dot); fp = plain.substring(dot + 1); }
        StringBuilder g = new StringBuilder();
        for (int i = 0; i < ip.length(); i++) {
            if (i > 0 && (ip.length() - i) % 3 == 0 && thousands != null) g.append(thousands);
            g.append(ip.charAt(i));
        }
        String s = g + (dec > 0 ? (decSep == null ? "." : decSep) + fp : "");
        return r.signum() < 0 ? "-" + s : s;
    }

    static String fmtNumPattern(BigDecimal n, String pattern) {
        try { return new DecimalFormat(pattern, DecimalFormatSymbols.getInstance(LOCALE)).format(n); }
        catch (Exception e) { return toText(n); }
    }

    static String fmtDate(Date d, String pattern) {
        try { return new SimpleDateFormat(pattern, LOCALE).format(d); }
        catch (Exception e) { return new SimpleDateFormat("dd/MM/yyyy", LOCALE).format(d); }
    }

    static boolean hasTime(Date d) {
        Calendar c = Calendar.getInstance(); c.setTime(d);
        return c.get(Calendar.HOUR_OF_DAY) != 0 || c.get(Calendar.MINUTE) != 0 || c.get(Calendar.SECOND) != 0;
    }

    /** Crystal date format letters to SimpleDateFormat: d/M/y/H/m/s match; "tt" is the AM/PM marker. */
    public static String crystalDatePattern(String f) {
        if (f == null || f.isEmpty()) return "dd/MM/yyyy";
        return f.replace("tt", "a").replace("t", "a");
    }

    // ------------------------------------------------------------------ number functions
    public static Object round(Object x) { return round(x, BigDecimal.ZERO); }
    public static Object round(Object x, Object n) {
        BigDecimal b = num(x); if (b == null) return null;
        return b.setScale(nz(n).intValue(), RoundingMode.HALF_UP);
    }
    public static Object truncate(Object x) { return truncate(x, BigDecimal.ZERO); }
    public static Object truncate(Object x, Object n) {
        BigDecimal b = num(x); if (b == null) return null;
        return b.setScale(nz(n).intValue(), RoundingMode.DOWN);
    }
    public static Object abs(Object x) { BigDecimal b = num(x); return b == null ? null : b.abs(); }
    public static Object sgn(Object x) { BigDecimal b = num(x); return b == null ? null : BigDecimal.valueOf(b.signum()); }
    public static Object sqr(Object x) { BigDecimal b = num(x); return b == null ? null : new BigDecimal(Math.sqrt(b.doubleValue())); }
    public static Object toNumber(Object x) { return num(x); }
    public static Object isNumeric(Object x) { return x != null && num(x) != null; }
    public static Object remainder(Object a, Object b) { return mod(a, b); }

    // ------------------------------------------------------------------ string functions
    public static Boolean isNull(Object o) { return o == null; }
    public static Object trim(Object s) { return s == null ? null : str(s).trim(); }
    public static Object ltrim(Object s) { return s == null ? null : str(s).replaceAll("^\\s+", ""); }
    public static Object rtrim(Object s) { return s == null ? null : str(s).replaceAll("\\s+$", ""); }
    public static Object upper(Object s) { return s == null ? null : str(s).toUpperCase(LOCALE); }
    public static Object lower(Object s) { return s == null ? null : str(s).toLowerCase(LOCALE); }
    public static Object proper(Object s) {
        if (s == null) return null;
        StringBuilder b = new StringBuilder(); boolean start = true;
        for (char c : str(s).toCharArray()) {
            b.append(start ? Character.toUpperCase(c) : Character.toLowerCase(c));
            start = !Character.isLetterOrDigit(c);
        }
        return b.toString();
    }
    public static Object len(Object s) { return s == null ? null : BigDecimal.valueOf(str(s).length()); }
    public static Object left(Object s, Object n) {
        if (s == null) return null; String t = str(s); int k = Math.max(0, nz(n).intValue());
        return t.substring(0, Math.min(k, t.length()));
    }
    public static Object right(Object s, Object n) {
        if (s == null) return null; String t = str(s); int k = Math.max(0, nz(n).intValue());
        return t.substring(Math.max(0, t.length() - k));
    }
    public static Object mid(Object s, Object start) {
        if (s == null) return null; String t = str(s); int a = Math.max(1, nz(start).intValue());
        return a > t.length() ? "" : t.substring(a - 1);
    }
    public static Object mid(Object s, Object start, Object n) {
        if (s == null) return null; String t = str(s);
        int a = Math.max(1, nz(start).intValue()), k = Math.max(0, nz(n).intValue());
        if (a > t.length()) return "";
        return t.substring(a - 1, Math.min(t.length(), a - 1 + k));
    }
    public static Object instr(Object s, Object find) { return instr(BigDecimal.ONE, s, find); }
    public static Object instr(Object start, Object s, Object find) {
        if (s == null || find == null) return null;
        if (!(start instanceof BigDecimal) && !(start instanceof Number)) return instr(BigDecimal.ONE, start, s); // InStr(s, find, compare)
        int from = Math.max(1, nz(start).intValue());
        return BigDecimal.valueOf(str(s).indexOf(str(find), from - 1) + 1);
    }
    public static Object replace(Object s, Object f, Object r) {
        if (s == null) return null;
        return str(s).replace(f == null ? "" : str(f), r == null ? "" : str(r));
    }
    public static Object space(Object n) { return replicate(" ", n); }
    public static Object replicate(Object s, Object n) {
        if (s == null) return null; StringBuilder b = new StringBuilder();
        for (int i = 0; i < nz(n).intValue(); i++) b.append(str(s));
        return b.toString();
    }
    public static Object chr(Object n) { return n == null ? null : String.valueOf((char) nz(n).intValue()); }
    public static Object asc(Object s) { return s == null || str(s).isEmpty() ? null : BigDecimal.valueOf(str(s).charAt(0)); }
    public static Object reverse(Object s) { return s == null ? null : new StringBuilder(str(s)).reverse().toString(); }
    public static Object split(Object s, Object sep) {
        if (s == null) return new Object[0];
        return str(s).split(java.util.regex.Pattern.quote(sep == null ? " " : str(sep)), -1);
    }
    public static Object join(Object arr, Object sep) {
        if (!(arr instanceof Object[])) return str(arr);
        List<String> parts = new ArrayList<>();
        for (Object o : (Object[]) arr) parts.add(o == null ? "" : str(o));
        return String.join(sep == null ? " " : str(sep), parts);
    }
    public static Object ubound(Object arr) { return arr instanceof Object[] ? BigDecimal.valueOf(((Object[]) arr).length) : BigDecimal.ONE; }
    public static Object iif(Object c, Object a, Object b) { return bool(c) ? a : b; }

    // ------------------------------------------------------------------ date functions
    public static Object currentDate() {
        Calendar c = Calendar.getInstance();
        c.set(Calendar.HOUR_OF_DAY, 0); c.set(Calendar.MINUTE, 0); c.set(Calendar.SECOND, 0); c.set(Calendar.MILLISECOND, 0);
        return c.getTime();
    }
    public static Object currentDateTime() { return new Date(); }
    public static Object dateOf(Object o) {
        Date d = date(o); if (d == null) return null;
        Calendar c = Calendar.getInstance(); c.setTime(d);
        c.set(Calendar.HOUR_OF_DAY, 0); c.set(Calendar.MINUTE, 0); c.set(Calendar.SECOND, 0); c.set(Calendar.MILLISECOND, 0);
        return c.getTime();
    }
    public static Object dateOf(Object y, Object m, Object d) {
        Calendar c = Calendar.getInstance(); c.clear();
        c.set(nz(y).intValue(), nz(m).intValue() - 1, nz(d).intValue());
        return c.getTime();
    }
    public static Object dateTimeOf(Object o) { return date(o); }
    public static Object dateTimeOf(Object d, Object t) {
        Date a = date(d), b = date(t); if (a == null) return null; if (b == null) return a;
        Calendar ca = Calendar.getInstance(), cb = Calendar.getInstance(); ca.setTime(a); cb.setTime(b);
        ca.set(Calendar.HOUR_OF_DAY, cb.get(Calendar.HOUR_OF_DAY)); ca.set(Calendar.MINUTE, cb.get(Calendar.MINUTE));
        ca.set(Calendar.SECOND, cb.get(Calendar.SECOND));
        return ca.getTime();
    }
    private static Object part(Object o, int field, int offset) {
        Date d = date(o); if (d == null) return null;
        Calendar c = Calendar.getInstance(); c.setTime(d);
        return BigDecimal.valueOf(c.get(field) + offset);
    }
    public static Object year(Object d) { return part(d, Calendar.YEAR, 0); }
    public static Object month(Object d) { return part(d, Calendar.MONTH, 1); }
    public static Object day(Object d) { return part(d, Calendar.DAY_OF_MONTH, 0); }
    public static Object hour(Object d) { return part(d, Calendar.HOUR_OF_DAY, 0); }
    public static Object minute(Object d) { return part(d, Calendar.MINUTE, 0); }
    public static Object second(Object d) { return part(d, Calendar.SECOND, 0); }
    public static Object dayOfWeek(Object d) { return part(d, Calendar.DAY_OF_WEEK, 0); }
    public static Object monthName(Object m) { return monthName(m, Boolean.FALSE); }
    public static Object monthName(Object m, Object abbr) {
        if (m == null) return null;
        String[] names = new java.text.DateFormatSymbols(LOCALE).getMonths();
        String n = names[Math.max(0, Math.min(11, nz(m).intValue() - 1))];
        return bool(abbr) ? n.substring(0, 3) : n;
    }
    public static Object weekdayName(Object d) { return weekdayName(d, Boolean.FALSE); }
    public static Object weekdayName(Object d, Object abbr) {
        if (d == null) return null;
        String n = new java.text.DateFormatSymbols(LOCALE).getWeekdays()[Math.max(1, Math.min(7, nz(d).intValue()))];
        return bool(abbr) ? n.substring(0, 3) : n;
    }
    static Date addDays(Date d, BigDecimal n) {
        Calendar c = Calendar.getInstance(); c.setTime(d); c.add(Calendar.DAY_OF_MONTH, n.intValue());
        return c.getTime();
    }
    private static int calField(String interval) {
        switch (interval.toLowerCase(LOCALE)) {
            case "yyyy": return Calendar.YEAR;
            case "q": return -1;
            case "m": return Calendar.MONTH;
            case "ww": case "w": return Calendar.WEEK_OF_YEAR;
            case "h": return Calendar.HOUR_OF_DAY;
            case "n": return Calendar.MINUTE;
            case "s": return Calendar.SECOND;
            default: return Calendar.DAY_OF_MONTH;
        }
    }
    public static Object dateAdd(Object interval, Object n, Object d) {
        Date x = date(d); if (x == null || n == null) return null;
        Calendar c = Calendar.getInstance(); c.setTime(x);
        int f = calField(str(interval)), k = nz(n).intValue();
        if (f == -1) c.add(Calendar.MONTH, 3 * k); else c.add(f, k);
        return c.getTime();
    }
    public static Object dateDiff(Object interval, Object a, Object b) {
        Date x = date(a), y = date(b); if (x == null || y == null) return null;
        long ms = y.getTime() - x.getTime();
        switch (str(interval).toLowerCase(LOCALE)) {
            case "yyyy": return BigDecimal.valueOf(nz(year(y)).intValue() - nz(year(x)).intValue());
            case "m": return BigDecimal.valueOf((nz(year(y)).intValue() - nz(year(x)).intValue()) * 12L
                                                + nz(month(y)).intValue() - nz(month(x)).intValue());
            case "ww": case "w": return BigDecimal.valueOf(ms / (7 * 86400000L));
            case "h": return BigDecimal.valueOf(ms / 3600000L);
            case "n": return BigDecimal.valueOf(ms / 60000L);
            case "s": return BigDecimal.valueOf(ms / 1000L);
            default: return BigDecimal.valueOf(Math.round(ms / 86400000d));
        }
    }

    // ------------------------------------------------------------------ ToWords
    private static final String[] ONES = { "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten",
        "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
    private static final String[] TENS = { "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

    private static String words999(int n) {
        StringBuilder b = new StringBuilder();
        if (n >= 100) { b.append(ONES[n / 100]).append(" hundred"); n %= 100; if (n > 0) b.append(' '); }
        if (n >= 20) { b.append(TENS[n / 10]); if (n % 10 > 0) b.append('-').append(ONES[n % 10]); }
        else if (n > 0) b.append(ONES[n]);
        return b.toString();
    }

    static String words(long n) {
        if (n == 0) return "zero";
        String[] scale = { "", " thousand", " million", " billion", " trillion" };
        StringBuilder out = new StringBuilder(); int i = 0;
        while (n > 0) {
            int chunk = (int) (n % 1000);
            if (chunk > 0) out.insert(0, words999(chunk) + scale[i] + (out.length() > 0 ? " " : ""));
            n /= 1000; i++;
        }
        return out.toString();
    }

    /** Crystal ToWords(123.45) = "one hundred twenty-three and 45 / 100". */
    public static Object toWords(Object x) { return toWords(x, BigDecimal.valueOf(2)); }
    public static Object toWords(Object x, Object decimals) {
        BigDecimal b = num(x); if (b == null) return null;
        int dec = nz(decimals).intValue();
        b = b.setScale(dec, RoundingMode.HALF_UP);
        long whole = b.abs().longValue();
        String s = (b.signum() < 0 ? "minus " : "") + words(whole);
        if (dec > 0) {
            BigDecimal frac = b.abs().subtract(BigDecimal.valueOf(whole)).movePointRight(dec);
            s += " and " + String.format("%0" + dec + "d", frac.intValue()) + " / " + BigDecimal.TEN.pow(dec).toPlainString();
        }
        return s;
    }

    // ------------------------------------------------------------------ plumbing for converted templates
    /** NULL to "" for string joins inside text objects (Crystal prints an empty embedded field). */
    public static Object nzs(Object o) { return o == null ? "" : o; }

    /** Rows of a sub-report, put in the parameters as SUBDATA_<key>; a fresh source on every use. */
    @SuppressWarnings("unchecked")
    public static net.sf.jasperreports.engine.JRDataSource ds(Map<String, Object> params, String key) {
        Object rows = params == null ? null : params.get(key);
        Collection<Map<String, ?>> list = rows instanceof Collection
                ? (Collection<Map<String, ?>>) rows : new ArrayList<Map<String, ?>>();
        return new net.sf.jasperreports.engine.data.JRMapCollectionDataSource(list);
    }

    /** Compiled sub-report, put in the parameters as SUBREPORT_<key>. */
    public static net.sf.jasperreports.engine.JasperReport sub(Map<String, Object> params, String key) {
        Object r = params == null ? null : params.get(key);
        if (!(r instanceof net.sf.jasperreports.engine.JasperReport))
            throw new IllegalStateException("Sub-report " + key + " was not supplied by the renderer.");
        return (net.sf.jasperreports.engine.JasperReport) r;
    }

    /** The main report's own parameters handed down (company name, sub-report data), minus Jasper built-ins. */
    public static Map<String, Object> subParams(Map<String, Object> params) {
        Map<String, Object> m = new HashMap<>();
        if (params == null) return m;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            String k = e.getKey();
            if (k.startsWith("REPORT_") || k.startsWith("JASPER_") || k.startsWith("IS_IGNORE")
                    || k.equals("SORT_FIELDS") || k.equals("FILTER") || k.equals("CR_VARS")) continue;
            m.put(k, e.getValue());
        }
        return m;
    }

    /** A blob field (logo, signature) as an image stream. */
    public static java.io.InputStream img(Object o) {
        if (o instanceof byte[] && ((byte[]) o).length > 0) return new java.io.ByteArrayInputStream((byte[]) o);
        if (o instanceof String && !((String) o).isEmpty()) {
            try { return new java.io.ByteArrayInputStream(java.util.Base64.getDecoder().decode((String) o)); }
            catch (IllegalArgumentException e) { return null; }
        }
        return null;
    }

    // ------------------------------------------------------------------ helpers for hand-built report templates
    /** Amount with the company's decimals (DecimalPointOfAmount); blank for zero when asked, like Crystal "suppress if zero". */
    public static String amt(Object x, Object dec, boolean blankZero) {
        BigDecimal b = num(x);
        if (b == null || (blankZero && b.signum() == 0)) return blankZero ? "" : fmtNum(BigDecimal.ZERO, decimals(dec), ",", ".");
        return fmtNum(b, decimals(dec), ",", ".");
    }

    /** Running balance as the desktop shows it: absolute amount with Dr / Cr. */
    public static String drcr(Object x, Object dec) {
        BigDecimal b = num(x);
        if (b == null) return "";
        String s = fmtNum(b.abs(), decimals(dec), ",", ".");
        return b.signum() > 0 ? s + " Dr" : b.signum() < 0 ? s + " Cr" : s;
    }

    static int decimals(Object dec) {
        BigDecimal d = num(dec);
        return d == null ? 2 : Math.max(0, Math.min(6, d.intValue()));
    }

    /** True when the value carries a time of day (so it prints with the time). */
    public static boolean hasTime(Object o) {
        Date d = date(o);
        return d != null && hasTime(d);
    }

    /** Date text, blank when null. */
    public static String dt(Object d, String pattern) {
        Date x = date(d);
        return x == null ? "" : fmtDate(x, pattern);
    }

    /** Null-safe text. */
    public static String s(Object o) { return o == null ? "" : str(v(o)).trim(); }

    /** Joins the non-blank parts with sep. */
    public static String joinNonEmpty(String sep, Object... parts) {
        StringBuilder b = new StringBuilder();
        for (Object p : parts) {
            String t = s(p);
            if (t.isEmpty()) continue;
            if (b.length() > 0) b.append(sep);
            b.append(t);
        }
        return b.toString();
    }

    /** A classpath image (company logo baked into the .rpt), or null. */
    public static java.io.InputStream resource(String path) {
        return CR.class.getResourceAsStream(path);
    }

    /** Anything the converter could not translate: prints nothing rather than failing the fill. */
    public static Object unsupported(String what) { return null; }
}
