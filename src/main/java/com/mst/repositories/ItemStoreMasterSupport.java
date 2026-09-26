package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * What the two Store master screens 336 "Item Category Store" (InvDeffrmItemCatagory) and
 * 337 "Item Type Store" (InvDeffrmItemType) share: the parent-category lookup both forms bind,
 * the desktop's string-{@code Contains} id filter both forms use, the global configuration read
 * (GlobalVariables_Helper.GetConfigValueFromGlobal) and the Architecture.Common.Conversion
 * helpers the forms apply to control text.
 *
 * Only reads; nothing here writes.
 */
@Repository
public class ItemStoreMasterSupport {

    private final JdbcTemplate jdbc;
    private final StoreIssuanceRepository common;

    public ItemStoreMasterSupport(JdbcTemplate jdbc, StoreIssuanceRepository common) {
        this.jdbc = jdbc;
        this.common = common;
    }

    public JdbcTemplate jdbc() { return jdbc; }

    /**
     * BLL 0583 Item.InventoryParentCategories(ReportsParameters): only {@code @Ids} (when set) and
     * {@code @Activity='InventoryParentCategories'} are sent — the Organization/Company the forms
     * put on the ReportsParameters are ignored by the BLL. Both forms leave Ids empty.
     * Rows: Id, InvParentCateDescription.
     */
    public List<Map<String, Object>> inventoryParentCategories() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_InventoryItemsOther_GetAllMethod",
                params("Activity", "InventoryParentCategories"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", StoreIssuanceRepository.toInt(r.get("Id")));
            o.put("InvParentCateDescription", StoreIssuanceRepository.str(r.get("InvParentCateDescription")));
            out.add(o);
        }
        return out;
    }

    /**
     * GlobalVariables_Helper.GetConfigValueFromGlobal(name): ConfigKey of the allocation row whose
     * ConfigDescription is {@code name} ("" when absent). The desktop reads a login-time cache of
     * the same allocation rows; this reads them per request (StoreIssuanceRepository.config).
     */
    public String config(UserAccount u, String name) {
        return common.config(u, name);
    }

    /**
     * DAL 0205 CommonServices.GetERPFeaturesByCompanyId(org, company, id) and the WinApp
     * CommonServices.GetERPFeatureById(id): membership of {@code id} in USP_GetERPFeaturesByCompanyId.
     */
    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (StoreIssuanceRepository.toInt(r.get("Id")) == featureId) return true;
        }
        return false;
    }

    // ================================================================ desktop semantics helpers

    /**
     * {@code "1,2,4".Contains(Conversion.ToString(id))} — a SUBSTRING test, not a list test. The
     * forms filter every id list this way, so e.g. "15,16,18" also admits 1, 5, 6 and 8, and a
     * NULL id (Conversion.ToString(DBNull) = "") is always admitted. Reproduced as written.
     */
    public static boolean desktopContains(String csv, Object id) {
        String s = id == null ? "" : String.valueOf(id);
        if (id instanceof Number) s = String.valueOf(((Number) id).longValue());
        return csv.contains(s);
    }

    /**
     * Architecture.Common.Conversion.ToBool(object): Convert.ToBoolean, then
     * Convert.ToBoolean(Convert.ToInt32(value)), else false.
     */
    public static boolean convToBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).doubleValue() != 0;
        String s = String.valueOf(v);
        if (s.isEmpty()) return false;
        String t = s.trim();
        if (t.equalsIgnoreCase("true")) return true;
        if (t.equalsIgnoreCase("false")) return false;
        Integer i = parseInt32(s);
        return i != null && i != 0;
    }

    /**
     * Architecture.Common.Conversion.ToInt(object) on a string: Convert.ToInt32 (Int32.Parse with
     * NumberStyles.Integer — surrounding white space and a leading sign allowed), 0 on any failure.
     */
    public static int convToInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        Integer i = parseInt32(String.valueOf(v));
        return i == null ? 0 : i;
    }

    private static Integer parseInt32(String s) {
        if (s == null) return null;
        String t = s.trim();
        if (t.isEmpty()) return null;
        try {
            long l = Long.parseLong(t.startsWith("+") ? t.substring(1) : t);
            if (l < Integer.MIN_VALUE || l > Integer.MAX_VALUE) return null;
            return (int) l;
        } catch (NumberFormatException e) {
            return null;
        }
    }

    /** Conversion.ToString(object): "" for null. */
    public static String convToString(Object v) {
        return v == null ? "" : String.valueOf(v);
    }
}
