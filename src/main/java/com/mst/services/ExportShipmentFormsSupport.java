package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.security.CurrentUserContext;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Shared helpers of the five Export shipment forms ported together (agent P2):
 *
 *   /export/goods-receipts-at-port       ExImGoodsReceiptsAtPort (+ GetForwardingDataForGoodsReceiptsAsPort)
 *   /export/pm-issuance-for-shipment     PackingMaterialIssuanceForShipment
 *   /export/fi-utilized-against-shipment FinancialInstrumentsUtilizedAgainstShipment
 *   /export/invoice-against-forwarding   ExportInvoiceAgainstForwarding (+ LoadExportForwarding)
 *   /export/dhl-tracking                 frmDhlTracking
 *
 * Rights. Four of the forms read their rights with CommonServices.SetRightsValueInRightsObject(base.Name) and
 * dbo.ScreenDefinition has NO row for those names (ExImGoodsReceiptsAtPort, PackingMaterialIssuanceForShipment,
 * ExportInvoiceAgainstForwarding, frmDhlTracking). The desktop then gets an empty list from
 * Sp_tblUserRights_GetAllMethod 'GetByUserId' and only the "Admin" role keeps Save / Update / Print / CanViewAll.
 * {@link #rights} reproduces exactly that call (SaleInvoiceRepository.rights = the same procedure, the same
 * role test), so a row added to ScreenDefinition later takes effect without a code change.
 * FinancialInstrumentsUtilizedAgainstShipment reads "CommiercialInvoiceAgainstPreInvoiceTransfer" = ScreenDefinition
 * 202 and goes through DesktopReportRights (its own service).
 *
 * Conversion.* semantics: unparseable numbers are 0, blank strings stay "", dates come as yyyy-MM-dd.
 */
public final class ExportShipmentFormsSupport {

    private ExportShipmentFormsSupport() { }

    // ------------------------------------------------------------------ rights

    /** CommonServices.SetRightsValueInRightsObject(screenName): Save, Update, Delete, Print, CanViewAllRecord, View. */
    public static Map<String, Boolean> rights(SaleInvoiceRepository repo, CurrentUserContext ctx, UserAccount u, String screenName) {
        return repo.rights(u, ctx.currentRoleName(), screenName);
    }

    public static void require(Map<String, Boolean> rights, String right, String message) {
        if (!Boolean.TRUE.equals(rights.get(right))) throw new IllegalStateException(message);
    }

    // ------------------------------------------------------------------ Conversion.*

    public static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    /** Conversion.ToString: null -> "", otherwise the value untrimmed. */
    public static String raw(Object v) { return v == null ? "" : String.valueOf(v); }

    public static int asInt(Object v) {
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

    public static long asLong(Object v) {
        if (v == null) return 0L;
        if (v instanceof Number) return ((Number) v).longValue();
        try { return Long.parseLong(String.valueOf(v).trim().replace(",", "")); }
        catch (NumberFormatException e) { return asInt(v); }
    }

    public static double asDouble(Object v) {
        if (v == null) return 0d;
        if (v instanceof Number) return ((Number) v).doubleValue();
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0d;
        try { return Double.parseDouble(s); }
        catch (NumberFormatException e) { return 0d; }
    }

    public static BigDecimal dec(Object v) {
        if (v instanceof BigDecimal) return (BigDecimal) v;
        return BigDecimal.valueOf(asDouble(v));
    }

    public static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = String.valueOf(v).trim();
        return "true".equalsIgnoreCase(s) || "1".equals(s) || "yes".equalsIgnoreCase(s);
    }

    /** Math.Round(v, places) - .NET rounds the midpoint to even. */
    public static double round(double v, int places) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return v;
        return BigDecimal.valueOf(v).setScale(places, java.math.RoundingMode.HALF_EVEN).doubleValue();
    }

    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    @SuppressWarnings("unchecked")
    public static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    // ------------------------------------------------------------------ dates

    public static LocalDate asDate(Object v) {
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        if (s.length() > 10) s = s.substring(0, 10);
        try { return LocalDate.parse(s); }
        catch (DateTimeParseException e) { return null; }
    }

    /** A DateTimePicker's Value: the chosen day with the time of day the form runs at. */
    public static Timestamp pickerDate(Object yyyyMMdd) {
        LocalDate d = asDate(yyyyMMdd);
        if (d == null) d = LocalDate.now();
        return Timestamp.valueOf(LocalDateTime.of(d, LocalTime.now().withNano(0)));
    }

    /** A picker set from a DATE column keeps midnight. */
    public static Timestamp midnight(Object yyyyMMdd) {
        LocalDate d = asDate(yyyyMMdd);
        if (d == null) d = LocalDate.now();
        return Timestamp.valueOf(d.atStartOfDay());
    }

    public static Timestamp now() { return new Timestamp(System.currentTimeMillis()); }

    /** yyyy-MM-dd for the page. */
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

    /** yyyy-MM-ddTHH:mm:ss for the page (EntryDate / ModifyDate columns). */
    public static String isoDateTime(Object v) {
        if (v == null) return "";
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString();
        return String.valueOf(v);
    }

    /** A JSON-friendly copy of a procedure row: dates as yyyy-MM-dd(THH:mm:ss), decimals as double. */
    public static Map<String, Object> plain(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : r.entrySet()) {
            Object v = e.getValue();
            if (v instanceof Timestamp || v instanceof java.util.Date || v instanceof LocalDateTime) v = isoDateTime(v);
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

    public static Map<String, Object> ok(String message, Object id) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        m.put("id", id);
        return m;
    }

    public static String msg(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    public interface Loader { Object load() throws Exception; }

    /** Load events run their reads one after another; one failing read is shown beside the others (MessageBox each). */
    public static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }
}
