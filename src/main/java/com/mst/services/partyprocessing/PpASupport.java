package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.repositories.partyprocessing.PpARepository;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * Shared plumbing of the PpA party-processing services: the signed-in user with View on the screen,
 * the desktop's rights flags, tenancy guards for ids the page sends back, and the desktop Conversion
 * semantics the forms rely on (Convert.ToInt32 of a string is strict: "12.5" or "1,000" is 0).
 */
@Component
public class PpASupport {

    @Autowired private HrmSupport hrm;
    @Autowired private PpARepository repo;

    public UserAccount user(int screenId) { return hrm.user(screenId); }

    public void require(UserAccount u, int screenId, String action) { hrm.require(u, screenId, action); }

    public boolean can(UserAccount u, int screenId, String action) { return hrm.can(u, screenId, action); }

    /** SetRightsValueInRightsObject: save / update / delete / print / canViewAll. */
    public Map<String, Object> rights(UserAccount u, int screenId) {
        Map<String, Object> r = hrm.rights(u, screenId);
        r.put("canViewAll", hrm.can(u, screenId, "CanView AllRecord"));
        return r;
    }

    public int financialYearId() { return hrm.financialYearId(); }

    /** clsGlobalVariables.ActiveYr.Start_Period of the session's year (date type 5 "Financial Year"). */
    public String financialYearStart(UserAccount u) {
        try {
            int yearId = hrm.financialYearId();
            for (Map<String, Object> r : repo.activeFinancialYears(u)) {
                if (toInt(r.get("Id")) == yearId) {
                    LocalDateTime d = toDate(r.get("Start_Period"));
                    return d == null ? null : d.toLocalDate().toString();
                }
            }
        } catch (RuntimeException e) {
            return null;
        }
        return null;
    }

    /** CommonServices.GetERPFeatureById(id). */
    public boolean feature(UserAccount u, int featureId) {
        for (Map<String, Object> r : repo.erpFeatures(u)) if (toInt(r.get("Id")) == featureId) return true;
        return false;
    }

    /** A record read by Id alone (ReadById procedures take no tenancy) must belong to the signed-in company. */
    public static boolean mine(UserAccount u, Map<String, Object> row) {
        if (row == null) return false;
        return toInt(row.get("OrganizationId")) == toInt(u.getOrganizationId()) && toInt(row.get("CompanyId")) == toInt(u.getCompanyId());
    }

    /** Conversion.ToInt(string): Convert.ToInt32 - whole numbers only (sign and spaces allowed), anything else 0. */
    public static int cint(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s.startsWith("+") ? s.substring(1) : s); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDouble(string): Convert.ToDouble (en-US: group separators allowed), else 0. */
    public static double cdbl(Object v) {
        if (v == null) return 0.0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0.0;
        try { double d = Double.parseDouble(s); return Double.isInfinite(d) || Double.isNaN(d) ? 0.0 : d; } catch (NumberFormatException e) { return 0.0; }
    }

    public static String s(Map<String, Object> b, String k) { return b == null ? "" : str(b.get(k)); }

    public static int i(Map<String, Object> b, String k) { return b == null ? 0 : toInt(b.get(k)); }

    public static boolean flag(Map<String, Object> b, String k) { return b != null && toBool(b.get(k)); }

    /** A picker value as the desktop sends it (date + the picker's time of day), or null when blank. */
    public static Timestamp when(Object v) { return ts(toDate(v)); }

    /** Keeps only the named columns (a DataTable the form builds column by column); missing ones are null. */
    public static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String... map) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (int x = 0; x + 1 < map.length; x += 2) m.put(map[x], r.get(map[x + 1]));
            out.add(m);
        }
        return out;
    }

    /** true when id is 0 or appears in rows under key (an id the page sends must be one the combo offered). */
    public static boolean offered(List<Map<String, Object>> rows, String key, int id) {
        return id == 0 || owns(rows, key, id);
    }
}
