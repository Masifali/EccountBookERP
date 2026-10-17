package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Component;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Batch inv42 — Inventory hub, module 42 "Returnable Inventory Management" (screens 923, 927, 928)
 * and module 137 "Stock in Trust" (screen 963). The reads the three Returnable transaction forms
 * share, each as the desktop helper builds it:
 *
 * <pre>
 *   CommonServices.StaticColumnsService(activity)          SpStaticColumnNames @Activity
 *   VehicleType.GetAll()                                    Sp_VehicleType_GetAllMethod (no parameters)
 *   ReferenceParties.GetAll(org, comp)                      Sp_ReferenceParties_GetAllMethod 'ReadAll'
 *   CommonServices.GetLookupsByTypeIdDt(13)                 Sp_InvLookup_GetAllMethod @InvLookupTypeId=13 'ReadByInvlookTypeId'
 *   clsGlobalVariables.globalAllSupplierCustomer            USP_GetVendorsAndCustomersWithCityName
 *   clsGlobalVariables.globalWarehousesWithBranches         USP_GetWarehousesAllocatedToBranch @BranchId = user's branch
 *   clsGlobalVariables.getGlobalAllItems                    USP_Item_AllItemsWithModal
 *   getGlobalVendorsAndCustomersForTransporters (feature 4) USP_GetVendorsAndCustomersForTransporter
 *   AllAccountsWithCustomGroupId (no feature 4)             USP_GETAllAccountsFromCustomGroups
 *   CommonServices.GetERPFeatureById(4)                     USP_GetERPFeaturesByCompanyId
 *   GlobalVariables_Helper.GetConfigValueFromGlobal(key)    Sp_ConfigrationsAllocation_GetAllMethod 'GetConfigurationByOrgCompandConfigDescription'
 * </pre>
 */
@Component
public class Inv42Support {

    private final JdbcTemplate jdbc;

    public Inv42Support(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public JdbcTemplate jdbc() { return jdbc; }

    // ------------------------------------------------------------------------------ procedure calls

    public List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }
    public int setProc(String proc, Map<String, Object> p) { return DesktopProc.setProc(jdbc, proc, p); }
    public Integer scalar(String proc, Map<String, Object> p) { return DesktopProc.scalar(jdbc, proc, p); }

    // ------------------------------------------------------------------------------ shared lookups

    /** CommonServices.StaticColumnsService(activity) → SpStaticColumnNames (Id, type). */
    public List<Map<String, Object>> staticColumns(String activity) {
        return project(rows("SpStaticColumnNames", params("Activity", activity)), "Id", "type");
    }

    /** VehicleType.GetAll() → Sp_VehicleType_GetAllMethod with no parameters (Id, VehicleDescription). */
    public List<Map<String, Object>> vehicleTypes() {
        return project(rows("Sp_VehicleType_GetAllMethod", params()), "Id", "VehicleDescription");
    }

    /** ReferenceParties.GetAll(org, comp) → Sp_ReferenceParties_GetAllMethod 'ReadAll' (Id, ReferencePartyName). */
    public List<Map<String, Object>> referenceParties(UserAccount u) {
        return project(rows("Sp_ReferenceParties_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll")),
                "Id", "ReferencePartyName");
    }

    /** CommonServices.GetLookupsByTypeIdDt(13) → Sp_InvLookup_GetAllMethod (Id, LookupName) — Requested By / Approved By. */
    public List<Map<String, Object>> lookups13(UserAccount u) {
        return project(rows("Sp_InvLookup_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "InvLookupTypeId", 13, "Activity", "ReadByInvlookTypeId")), "Id", "LookupName");
    }

    /** SupplierDtFillFromGlobal — dtSupplier (Id, CompanyName, PartyCode, GlAccountId, CityId, CityName, MobileNo). */
    public List<Map<String, Object>> customers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows("USP_GetVendorsAndCustomersWithCityName", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("CompanyName", str(r.get("CompanyName")));
            o.put("PartyCode", str(r.get("PartyCode")));
            o.put("GlAccountId", toInt(r.get("GlAccountId")));
            o.put("CityId", toInt(r.get("CityId")));
            o.put("CityName", str(r.get("CityName")));
            o.put("MobileNo", str(r.get("MobilePersonal")));
            out.add(o);
        }
        return out;
    }

    /** WarehousesDtFillFromGlobal — globalWarehousesWithBranches.Where(IsActive) → (Id, WareHouseName). */
    public List<Map<String, Object>> warehouses(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows("USP_GetWarehousesAllocatedToBranch", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchId", u.getBranchesId()))) {
            if (r.containsKey("IsActive") && !truthy(r.get("IsActive"))) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            Object id = r.containsKey("WarehouseId") ? r.get("WarehouseId") : r.get("Id");
            Object name = r.containsKey("WarehouseName") ? r.get("WarehouseName") : r.get("WareHouseName");
            o.put("Id", toInt(id));
            o.put("WareHouseName", str(name));
            out.add(o);
        }
        return out;
    }

    /** clsGlobalVariables.getGlobalAllItems → USP_Item_AllItemsWithModal (all columns kept). */
    public List<Map<String, Object>> allItems(UserAccount u) {
        return rows("USP_Item_AllItemsWithModal", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** CommonServices.GetERPFeatureById(id) → USP_GetERPFeaturesByCompanyId contains the id. */
    public boolean feature(UserAccount u, int id) {
        for (Map<String, Object> r : rows("USP_GetERPFeaturesByCompanyId", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (toInt(r.get("Id")) == id) return true;
        }
        return false;
    }

    /** GlobalVariables_Helper.GetConfigValueFromGlobal(description) → ConfigKey ("" when absent). */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = rows("Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        return r.isEmpty() ? "" : str(r.get(0).get("ConfigKey")).trim();
    }

    /**
     * TransporterBindFromglobal (GdnReturnable.cs:566 / OutwardGatePassReturnable.cs:656):
     * feature 4 → getGlobalVendorsAndCustomersForTransporters (value GlAccountId, text CompanyName, PartyCode);
     * otherwise AllAccountsWithCustomGroupId without AccountTypeId 2,4,10,11,12,13,14,15,20,21,22,
     * first row per ChartOfAccountId (value ChartOfAccountId, text AccountTitle, AccountCode).
     * Rows: Id, AccountTitle, AccountCode, SupplierCustomerId (the party behind a feature-4 row, else 0).
     */
    public List<Map<String, Object>> transporters(UserAccount u, boolean feature4) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (feature4) {
            for (Map<String, Object> r : rows("USP_GetVendorsAndCustomersForTransporter", params(
                    "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", toInt(r.get("GlAccountId")));
                o.put("AccountTitle", str(r.get("CompanyName")));
                o.put("AccountCode", str(r.get("PartyCode")));
                o.put("SupplierCustomerId", toInt(r.get("Id")));
                out.add(o);
            }
            return out;
        }
        Set<Integer> excluded = new HashSet<>(List.of(2, 4, 10, 11, 12, 13, 14, 15, 20, 21, 22));
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> r : rows("USP_GETAllAccountsFromCustomGroups", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (excluded.contains(toInt(r.get("AccountTypeId")))) continue;
            int id = toInt(r.get("ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("AccountTitle", str(r.get("AccountTitle")));
            o.put("AccountCode", str(r.get("AccountCode")));
            o.put("SupplierCustomerId", 0);
            out.add(o);
        }
        return out;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period of the signed-in financial year, "yyyy-MM-dd" (null when unknown). */
    public String financialYearStart(UserAccount u, int yearId) {
        try {
            for (Map<String, Object> r : jdbc.queryForList(
                    "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                    u.getOrganizationId(), u.getCompanyId())) {
                if (toInt(ci(r, "Id")) == yearId) return day(ci(r, "Start_Period"));
            }
        } catch (RuntimeException ignored) {
            // the page falls back to today
        }
        return null;
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
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
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
        return s.equals("true") || s.equals("1");
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

    /** A DATETIME cell as "yyyy-MM-dd HH:mm:ss" (null when empty). */
    public static String dateTime(Object v) {
        if (v == null) return null;
        LocalDateTime t = null;
        if (v instanceof java.sql.Timestamp) t = ((java.sql.Timestamp) v).toLocalDateTime();
        else if (v instanceof LocalDateTime) t = (LocalDateTime) v;
        else if (v instanceof java.util.Date) t = new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime();
        if (t == null) { String s = String.valueOf(v); return s.length() > 19 ? s.substring(0, 19).replace('T', ' ') : s.replace('T', ' '); }
        return t.toLocalDate() + " " + String.format("%02d:%02d:%02d", t.getHour(), t.getMinute(), t.getSecond());
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

    /** DateTime.Now as the desktop stores it for a NEW document; an opened one keeps the picker's day. */
    public static Timestamp docDate(Object iso) {
        Timestamp t = ts(iso);
        if (t == null) return Timestamp.valueOf(LocalDateTime.now());
        LocalDateTime d = t.toLocalDateTime();
        if (d.getHour() == 0 && d.getMinute() == 0 && d.getSecond() == 0) {
            LocalDateTime now = LocalDateTime.now();
            if (d.toLocalDate().equals(now.toLocalDate())) return Timestamp.valueOf(now);
        }
        return t;
    }

    public static Timestamp now() { return Timestamp.valueOf(LocalDateTime.now()); }

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
}
