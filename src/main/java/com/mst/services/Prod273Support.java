package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Component;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Batch prod273 — Production hub (module 18): screen 273 "Production Job Order (Pre Costing)"
 * (ProductionPreCostingAndJobOrder.cs, DocumentTypeId 401) and screen 285 "Receipe (Branding)"
 * (frmBillOfMaterial_Manufacturing.cs, DocumentTypeId 1120). The reads both forms share, each as
 * the desktop helper builds it:
 *
 * <pre>
 *   CommonServices.SetRightsValueInRightsObject(base.Name)     Sp_tblUserRights_GetAllMethod 'GetByUserId'
 *   GlobalVariables_Helper.GetConfigValueFromGlobal(key)        Sp_ConfigrationsAllocation_GetAllMethod
 *                                                               'GetConfigurationByOrgCompandConfigDescription'
 *   UOMSchedule.Getall(org, comp)                               Sp_UOMSchedule_GetAllMethod 'ReadByOrganizationCompanyId'
 *   CommonServices.CoaAllocationGetAllServiceBind               Sp_COAAllocation_GetAllMethod 'COAAllocationSearch'
 *   CommonServices.ItemGetAllServiceBind                        Sp_Item_GetAllMethod 'ReadByOrganizationCompanyId'
 *   Item.ReadAllItems                                           Sp_Item_GetAllMethod 'ReadAllItems'
 *   CommonServices.GetItemByItemTypeId("14" | "14,17")          Sp_Item_GetAllMethod 'GetItemByItemTypeId' @LookupTypeIds
 *   CommonServices.AvgRateOnlyForCGS(...)                       Sp_GetAvgRatesAndStockInHand_GetAllMethod 'GetOnlyAvgRateForCGS'
 *   CommonServices.DateType()                                   literal (This Day .. Financial Year)
 *   clsGlobalVariables.ActiveYr.Start_Period                    Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId
 *   clsGlobalVariables.DecimalRateFormate                       "Default NoofDecimal Points For Rate"
 * </pre>
 *
 * Every parameter was checked against procdure_index.csv; a null is never bound (DesktopProc omits it,
 * exactly as ADO.NET AddWithValue(null) does).
 */
@Component
public class Prod273Support {

    private static final Logger LOG = LoggerFactory.getLogger(Prod273Support.class);

    private static final String SQL_RIGHTS =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, @CompanyId=?, @Activity=?";

    /** ParameterFill / Datetypefill — CommonServices.DateType(), a literal table on the desktop. */
    public static final String[][] DATE_TYPES = {
            { "1", "This Day" }, { "2", "This Week" }, { "3", "This Month" }, { "4", "This Year" }, { "5", "Financial Year" }
    };

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;

    public Prod273Support(JdbcTemplate jdbc, CurrentUserContext ctx) {
        this.jdbc = jdbc;
        this.ctx = ctx;
    }

    public JdbcTemplate jdbc() { return jdbc; }
    public UserAccount user() { return ctx.requireAccountingUser(); }
    public int financialYearId() { return ctx.currentFinancialYearId(); }
    public int branchId(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }

    // ------------------------------------------------------------------------------ procedure calls

    public List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }
    public int setProc(String proc, Map<String, Object> p) { return DesktopProc.setProc(jdbc, proc, p); }

    /** A lookup that fails is reported and returns empty; the screen still opens and the log names the procedure. */
    public List<Map<String, Object>> safe(String label, String proc, Map<String, Object> p) {
        try {
            return DesktopProc.rows(jdbc, proc, p);
        } catch (RuntimeException e) {
            LOG.warn("prod273 lookup '{}' ({}) failed", label, proc, e);
            return new ArrayList<>();
        }
    }

    // ------------------------------------------------------------------------------ rights / config

    /**
     * CommonServices.SetRightsValueInRightsObject(base.Name) (CommonServices.cs:17565) — the grant
     * rows of ONE screen with the desktop's Admin short-circuit (RoleName "Admin" gets Save, Update,
     * Delete, Print and CanView AllRecord). Keys: view, save, update, delete, print, viewAll,
     * rateAndAmount (DoHaveCanRateandAmount, screen 273's PM grid).
     */
    public Map<String, Boolean> rights(String screenName) {
        Map<String, Boolean> out = new LinkedHashMap<>();
        for (String k : new String[] { "view", "save", "update", "delete", "print", "viewAll", "rateAndAmount" }) out.put(k, false);
        String role = ctx.currentRoleName();
        try {
            List<Map<String, Object>> rows = jdbc.queryForList(SQL_RIGHTS, ctx.currentUserId(), screenName,
                    role == null ? "" : role, ctx.currentCompanyId(), "GetByUserId");
            for (Map<String, Object> r : rows) {
                String name = str(ci(r, "RightName")).trim();
                boolean v = truthy(ci(r, "Value"));
                String key = name.replace(" ", "").toLowerCase();
                switch (key) {
                    case "view":             out.put("view", v); break;
                    case "save":             out.put("save", v); break;
                    case "update":           out.put("update", v); break;
                    case "delete":           out.put("delete", v); break;
                    case "print":            out.put("print", v); break;
                    case "canviewallrecord": out.put("viewAll", v); break;
                    case "canrateandamount": out.put("rateAndAmount", v); break;
                    default: break;
                }
            }
        } catch (RuntimeException e) {
            LOG.warn("Could not read the rights of {}; denying", screenName, e);
        }
        if ("Admin".equals(role)) {
            out.put("save", true); out.put("update", true); out.put("delete", true);
            out.put("print", true); out.put("viewAll", true); out.put("rateAndAmount", true);
        }
        return out;
    }

    public boolean has(String screenName, String key) {
        Boolean b = rights(screenName).get(key);
        return b != null && b;
    }

    /** GlobalVariables_Helper.GetConfigValueFromGlobal(description) → ConfigKey ("" when absent). */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = safe("config " + description, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        return r.isEmpty() ? "" : str(ci(r.get(0), "ConfigKey")).trim();
    }

    /** Conversion.ToBool(GetConfigValueFromGlobal(key)) — a missing row is false. */
    public boolean configFlag(UserAccount u, String description) { return truthy(config(u, description)); }

    /**
     * clsGlobalVariables.DecimalRateFormate: "Default NoofDecimal Points For Rate" 1-4 → that many
     * decimals, 0 / missing → 2, anything else → 0 (the desktop's "#,#0." has no decimals).
     */
    public int rateDecimals(UserAccount u) {
        int n = toInt(config(u, "Default NoofDecimal Points For Rate"));
        if (n >= 1 && n <= 4) return n;
        return n == 0 ? 2 : 0;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period of the signed-in year, "yyyy-MM-dd" (null when unknown). */
    public String financialYearStart(UserAccount u, int yearId) {
        try {
            Map<String, Object> first = null;
            for (Map<String, Object> r : jdbc.queryForList(
                    "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                    u.getOrganizationId(), u.getCompanyId())) {
                if (first == null) first = r;
                if (toInt(ci(r, "Id")) == yearId) return day(ci(r, "Start_Period"));
            }
            if (first != null) return day(ci(first, "Start_Period"));
        } catch (RuntimeException e) {
            LOG.warn("Could not read the active financial year start", e);
        }
        return null;
    }

    // ------------------------------------------------------------------------------ shared lookups

    /** UOMSchedule.Getall(org, comp) → every item's UOM schedule rows (ItemId, Id, UOMCode, UOMDescription, Equivalent). */
    public List<Map<String, Object>> allUoms(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : safe("allUoms", "Sp_UOMSchedule_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("UOMCode", str(ci(r, "UOMCode")));
            o.put("UOMDescription", str(ci(r, "UOMDescription")));
            o.put("Equivalent", toDouble(ci(r, "Equivalent")));
            out.add(o);
        }
        return out;
    }

    /** CommonServices.GetUomScheduleByItemId → UOMSchedule.SearchByObject 'ReadByItemID'. */
    public List<Map<String, Object>> uomsByItem(UserAccount u, int itemId) {
        return safe("uomsByItem", "Sp_UOMSchedule_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId, "Activity", "ReadByItemID"));
    }

    /** CommonServices.CoaAllocationGetAllServiceBind → COAAllocation.GetAll 'COAAllocationSearch' (@UserId when non-zero). */
    public List<Map<String, Object>> coaAllocation(UserAccount u) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        p.put("Activity", "COAAllocationSearch");
        return safe("coaAllocation", "Sp_COAAllocation_GetAllMethod", p);
    }

    /** CommonServices.ItemGetAllServiceBind → Item.GetAll 'ReadByOrganizationCompanyId'. */
    public List<Map<String, Object>> itemsGetAll(UserAccount u) {
        return safe("itemsGetAll", "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }

    /** Item.ReadAllItems → 'ReadAllItems'. */
    public List<Map<String, Object>> itemsReadAll(UserAccount u) {
        return safe("itemsReadAll", "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAllItems"));
    }

    /** CommonServices.GetItemByItemTypeId(ids) → BLL 0583 Item.GetItemByItemTypeId: @LookupTypeIds, 'GetItemByItemTypeId'. */
    public List<Map<String, Object>> itemsByType(UserAccount u, String lookupTypeIds) {
        return project(safe("itemsByType", "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "LookupTypeIds", lookupTypeIds, "Activity", "GetItemByItemTypeId")), "Id", "ItemName");
    }

    /**
     * CommonServices.AvgRateOnlyForCGS → BLL 0056 GetAvgRatesAndStockInHand.AvgRateOnlyForCGS:
     * @ItemId @DocDate always; @DocumentTypeId @RecId @JobLotId @CropYearId @WarehouseId @BranchesId when
     * non-zero; @CropYear when not empty; 'GetOnlyAvgRateForCGS' → AvgRate of row 0 (0 when none).
     */
    public double avgRateOnlyForCgs(UserAccount u, int itemId, Timestamp docDate, int documentTypeId, int recId,
                                    int jobLotId, int cropYearId, String cropYear, int warehouseId, int branchesId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "DocDate", docDate);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (recId != 0) p.put("RecId", recId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (cropYearId != 0) p.put("CropYearId", cropYearId);
        if (cropYear != null && !cropYear.trim().isEmpty()) p.put("CropYear", cropYear.trim());
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (branchesId != 0) p.put("BranchesId", branchesId);
        p.put("Activity", "GetOnlyAvgRateForCGS");
        List<Map<String, Object>> r = rows("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        return r.isEmpty() ? 0d : toDouble(ci(r.get(0), "AvgRate"));
    }

    /** DMSAttachments.GetByID(id, screenName) → Sp_DMSAttachments_GetAllMethod 'ReadById' (the Attachment column). */
    public List<Map<String, Object>> attachments(String screenName, int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : safe("attachments", "Sp_DMSAttachments_GetAllMethod", params(
                "ScreenName", screenName, "Id", id, "Activity", "ReadById"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Attachment", str(ci(r, "Attachment")));
            out.add(o);
        }
        return out;
    }

    public static List<Map<String, Object>> literal(String[][] pairs, String nameColumn) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (String[] p : pairs) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", Integer.valueOf(p[0]));
            o.put(nameColumn, p[1]);
            o.put("Name", p[1]);                 // the page's pickers read Id / Name
            out.add(o);
        }
        return out;
    }

    // ------------------------------------------------------------------------------ conversions

    public static List<Map<String, Object>> project(List<Map<String, Object>> rows, String idKey, String nameKey) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, idKey)));
            o.put("Name", str(ci(r, nameKey)));
            out.add(o);
        }
        return out;
    }

    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    public static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try { return (int) Double.parseDouble(s); } catch (NumberFormatException e) { return 0; }
    }

    public static double toDouble(Object v) {
        if (v == null) return 0d;
        if (v instanceof Number) return ((Number) v).doubleValue();
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0d;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0d; }
    }

    public static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    public static boolean truthy(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = String.valueOf(v).trim().toLowerCase();
        return s.equals("true") || s.equals("1") || s.equals("yes");
    }

    /** A DATE / DATETIME cell as "yyyy-MM-dd". */
    public static String day(Object v) {
        if (v == null) return null;
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate().toString();
        if (v instanceof LocalDate) return v.toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        String s = String.valueOf(v);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    /** A DATETIME cell as "yyyy-MM-ddTHH:mm:ss" (null when empty) — the history grids format "dd-MM-yyyy hh:mm tt". */
    public static String stamp(Object v) {
        if (v == null) return null;
        LocalDateTime t = null;
        if (v instanceof java.sql.Timestamp) t = ((java.sql.Timestamp) v).toLocalDateTime();
        else if (v instanceof LocalDateTime) t = (LocalDateTime) v;
        else if (v instanceof java.util.Date) t = new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime();
        if (t == null) { String s = String.valueOf(v); return s.length() > 19 ? s.substring(0, 19).replace(' ', 'T') : s.replace(' ', 'T'); }
        return t.toLocalDate() + "T" + String.format("%02d:%02d:%02d", t.getHour(), t.getMinute(), t.getSecond());
    }

    /** "yyyy-MM-dd" or "yyyy-MM-ddTHH:mm[:ss]" → Timestamp; null for blank. */
    public static Timestamp ts(Object iso) {
        String s = str(iso).trim();
        if (s.isEmpty()) return null;
        s = s.replace('T', ' ');
        if (s.length() == 10) s = s + " 00:00:00";
        else if (s.length() == 16) s = s + ":00";
        try { return Timestamp.valueOf(s.length() > 19 ? s.substring(0, 19) : s); } catch (RuntimeException e) { return null; }
    }

    /** DateTimePicker.Value: the day the page sends with the current time of day (DateTime.Now when blank). */
    public static Timestamp pickerValue(Object iso) {
        Timestamp t = ts(iso);
        if (t == null) return Timestamp.valueOf(LocalDateTime.now());
        LocalDateTime d = t.toLocalDateTime();
        if (d.getHour() == 0 && d.getMinute() == 0 && d.getSecond() == 0) {
            LocalDateTime now = LocalDateTime.now();
            return Timestamp.valueOf(d.toLocalDate().atTime(now.toLocalTime().withNano(0)));
        }
        return t;
    }

    public static Timestamp now() { return Timestamp.valueOf(LocalDateTime.now().withNano(0)); }

    public static Map<String, Object> ok(String message, int id, Object docNo) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        m.put("id", id);
        m.put("docNo", docNo);
        return m;
    }

    @SuppressWarnings("unchecked")
    public static List<Map<String, Object>> list(Object v) {
        if (v instanceof List) return (List<Map<String, Object>>) v;
        return new ArrayList<>();
    }

    @SuppressWarnings("unchecked")
    public static Map<String, Object> map(Object v) {
        if (v instanceof Map) return (Map<String, Object>) v;
        return new LinkedHashMap<>();
    }

    public static boolean blank(Object v) { return v == null || String.valueOf(v).trim().isEmpty(); }
}
