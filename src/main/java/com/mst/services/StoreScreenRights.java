package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Component;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * CommonServices.SetRightsValueInRightsObject(screenName) (CommonServices.cs:17565) for the Store
 * Management screens — the grant rows for ONE screen, with the desktop's Admin short-circuit
 * (:17581: RoleName "Admin" gets Save, Update, Delete, CanView AllRecord and Print regardless of
 * the stored rows).
 *
 * The right names are the literal RightName values in dbo.ScreenRights, confirmed against the live
 * grant dump for these screens (322 frmGSIssuance, 321 StoreIssuanceDirect, 330 StoreReturn):
 * "View", "Save", "Update", "Delete", "Print", "CanView AllRecord".
 *
 * Read per call and never cached: a revoked right takes effect on the next request. Any failure
 * answers "no".
 */
@Component
public class StoreScreenRights {

    private static final Logger LOG = LoggerFactory.getLogger(StoreScreenRights.class);

    private static final String SQL =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, "
          + "@CompanyId=?, @Activity=?";

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;

    public StoreScreenRights(JdbcTemplate jdbc, CurrentUserContext ctx) {
        this.jdbc = jdbc;
        this.ctx = ctx;
    }

    /** All six flags the Store screens use, in one read. */
    public Map<String, Boolean> of(String screenName) {
        Map<String, Boolean> out = new LinkedHashMap<>();
        out.put("view", false);
        out.put("save", false);
        out.put("update", false);
        out.put("delete", false);
        out.put("print", false);
        out.put("viewAll", false);
        String role = ctx.currentRoleName();
        boolean admin = "Admin".equals(role);
        try {
            List<Map<String, Object>> rows = jdbc.queryForList(SQL,
                    ctx.currentUserId(), screenName, role == null ? "" : role,
                    ctx.currentCompanyId(), "GetByUserId");
            for (Map<String, Object> r : rows) {
                String name = String.valueOf(ci(r, "RightName")).trim();
                boolean v = toBool(ci(r, "Value"));
                switch (name) {
                    case "View":              out.put("view", v); break;
                    case "Save":              out.put("save", v); break;
                    case "Update":            out.put("update", v); break;
                    case "Delete":            out.put("delete", v); break;
                    case "Print":             out.put("print", v); break;
                    case "CanView AllRecord": out.put("viewAll", v); break;
                    default: break;
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read rights for {}; denying", screenName, e);
        }
        if (admin) {
            out.put("save", true);
            out.put("update", true);
            out.put("delete", true);
            out.put("viewAll", true);
            out.put("print", true);
        }
        return out;
    }

    public boolean has(String screenName, String key) {
        Boolean b = of(screenName).get(key);
        return b != null && b;
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        return v != null && ("true".equalsIgnoreCase(String.valueOf(v).trim()) || "1".equals(String.valueOf(v).trim()));
    }
}
