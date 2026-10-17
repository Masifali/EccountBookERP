package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.Timestamp;
import java.text.SimpleDateFormat;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Date;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

/**
 * Shared plumbing for the two Steel production screens (module 107 "Production Steel"):
 * 600 ProductionPreCostingAndJobOrder_St and 601 FoodProductionWithValuesNew_St.
 *
 * Every call reproduces the desktop DAL shape: {@code EXEC [ST].[Proc] @A=?, @B=?} where a CLR null
 * is OMITTED (ADO.NET AddWithValue(null) leaves the parameter out so the procedure default applies),
 * the first result set is read (GetDataTableProc / ExecuteScalar), and the remaining results are
 * drained so a RAISERROR after the first SELECT still surfaces.
 *
 * Rights follow CommonServices.SetRightsValueInRightsObject (:17565): the grant rows of
 * Sp_tblUserRights_GetAllMethod @Activity=GetByUserId for the desktop screen name, with the Admin
 * short-circuit for Save / Update / Print / CanView AllRecord / Delete only. "Rate"
 * (DoHaveCanRateandAmount) and "ProductionSettlement" (DoHaveCanSettlementtab) have no Admin
 * short-circuit on the desktop and have none here.
 */
@Service
public class SteelProductionSupport {

    private static final Logger LOG = LoggerFactory.getLogger(SteelProductionSupport.class);

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;

    public SteelProductionSupport(JdbcTemplate jdbc, CurrentUserContext ctx) {
        this.jdbc = jdbc;
        this.ctx = ctx;
    }

    public CurrentUserContext ctx() { return ctx; }

    public UserAccount user() { return ctx.requireAccountingUser(); }

    public int branchId(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }

    public static LinkedHashMap<String, Object> params() { return new LinkedHashMap<>(); }

    // ================================================================================ procedures

    /** GenericProvider.GetDataTableProc - every row of the first result set; nulls omitted. */
    public List<Map<String, Object>> query(String proc, Map<String, Object> p) {
        return jdbc.execute((ConnectionCallback<List<Map<String, Object>>>) con -> run(con, proc, p, false));
    }

    /** ExecuteScalar - first column of the first row of the first result set, or null. */
    public Object scalar(String proc, Map<String, Object> p) {
        List<Map<String, Object>> rows = jdbc.execute(
                (ConnectionCallback<List<Map<String, Object>>>) con -> run(con, proc, p, true));
        if (rows == null || rows.isEmpty()) return null;
        Map<String, Object> r = rows.get(0);
        return r.isEmpty() ? null : r.values().iterator().next();
    }

    /** GenericProvider.SetProc(tx, model, proc): "@Property" for every declared property, Convert.ToInt32(ExecuteScalar). */
    public int setProc(String proc, Map<String, Object> model, Set<String> declared) {
        LinkedHashMap<String, Object> p = params();
        for (Map.Entry<String, Object> e : model.entrySet()) {
            if (declared.contains(e.getKey())) p.put("@" + e.getKey(), e.getValue());
        }
        return toInt(scalar(proc, p));
    }

    private static List<Map<String, Object>> run(Connection con, String proc, Map<String, Object> p,
                                                 boolean firstRowOnly) throws java.sql.SQLException {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc);
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            if (e.getValue() == null) continue;
            sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
            values.add(e.getValue());
            first = false;
        }
        List<Map<String, Object>> out = new ArrayList<>();
        try (PreparedStatement ps = con.prepareStatement(sql.toString())) {
            for (int i = 0; i < values.size(); i++) {
                Object v = values.get(i);
                if (v instanceof LocalDateTime) v = Timestamp.valueOf((LocalDateTime) v);
                if (v instanceof java.util.Date && !(v instanceof Timestamp)) v = new Timestamp(((java.util.Date) v).getTime());
                ps.setObject(i + 1, v);
            }
            boolean isRs = ps.execute();
            while (true) {
                if (isRs) {
                    try (ResultSet rs = ps.getResultSet()) {
                        ResultSetMetaData md = rs.getMetaData();
                        int n = md.getColumnCount();
                        while (rs.next()) {
                            Map<String, Object> row = new LinkedHashMap<>();
                            for (int c = 1; c <= n; c++) row.put(md.getColumnLabel(c), plain(rs.getObject(c)));
                            out.add(row);
                            if (firstRowOnly) break;
                        }
                    }
                    drain(ps);
                    return out;
                }
                if (ps.getUpdateCount() == -1) break;
                isRs = ps.getMoreResults();
            }
        }
        return out;
    }

    private static void drain(PreparedStatement ps) throws java.sql.SQLException {
        while (true) {
            boolean more = ps.getMoreResults();
            if (!more && ps.getUpdateCount() == -1) return;
        }
    }

    /** Dates leave as local text: a Timestamp handed to Jackson is written in UTC and a midnight
     *  document date moves to the previous day east of Greenwich. */
    private static Object plain(Object o) {
        if (o instanceof Timestamp) return ((Timestamp) o).toLocalDateTime().withNano(0).toString();
        if (o instanceof java.sql.Date) return ((java.sql.Date) o).toLocalDate().toString();
        if (o instanceof java.util.Date) return new Timestamp(((java.util.Date) o).getTime()).toLocalDateTime().withNano(0).toString();
        return o;
    }

    // ===================================================================================== rights

    /** CommonServices.SetRightsValueInRightsObject(:17565) - the grant row for the screen. */
    public boolean hasRight(String screenName, String rightName) {
        String role = ctx.currentRoleName();
        if ("Admin".equals(role) && Arrays.asList("Save", "Update", "Print", "CanView AllRecord", "Delete").contains(rightName)) {
            return true;
        }
        try {
            List<Map<String, Object>> rights = jdbc.queryForList(
                    "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, @CompanyId=?, @Activity=?",
                    ctx.currentUserId(), screenName, role == null ? "" : role, ctx.currentCompanyId(), "GetByUserId");
            for (Map<String, Object> r : rights) {
                Object name = ci(r, "RightName");
                if (name != null && rightName.equals(String.valueOf(name).trim())) return toBool(ci(r, "Value"));
            }
        } catch (Exception e) {
            LOG.warn("Could not read the '{}' right for {}; denying", rightName, screenName, e);
        }
        return false;
    }

    /** clsGlobalVariables.DecimalRateFormate - "Default NoofDecimal Points For Rate" (1-4; 0 or none -> 2). */
    public int rateDecimals(UserAccount u) {
        try {
            List<Map<String, Object>> rows = jdbc.queryForList(
                    "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",
                    u.getOrganizationId(), u.getCompanyId(), "Default NoofDecimal Points For Rate",
                    "GetConfigurationByOrgCompandConfigDescription");
            int n = rows.isEmpty() ? 0 : toInt(ci(rows.get(0), "ConfigKey"));
            if (n >= 1 && n <= 4) return n;
            return n == 0 ? 2 : 0;
        } catch (Exception e) {
            LOG.warn("Could not read 'Default NoofDecimal Points For Rate'; using 2", e);
            return 2;
        }
    }

    /** clsGlobalVariables.DefaultNoofDecimalPointsForAmount - "Default NoofDecimal Points For Amount" (none -> 0). */
    public int amountDecimals(UserAccount u) {
        try {
            List<Map<String, Object>> rows = jdbc.queryForList(
                    "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",
                    u.getOrganizationId(), u.getCompanyId(), "Default NoofDecimal Points For Amount",
                    "GetConfigurationByOrgCompandConfigDescription");
            return rows.isEmpty() ? 0 : toInt(ci(rows.get(0), "ConfigKey"));
        } catch (Exception e) {
            return 0;
        }
    }

    // ============================================================================ shared lookups

    /** CommonServices.getActiveWareHouse -> Sp_InvWareHouse_GetAllMethod @Activity=GetActiveWareHouse. */
    public List<Map<String, Object>> activeWarehouses(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.Sp_InvWareHouse_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "GetActiveWareHouse");
    }

    /** CommonServices.WareHouseGetAllService -> InvWareHouse.Getall @Activity=ReadByOrganizationCompanyId. */
    public List<Map<String, Object>> allWarehouses(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.Sp_InvWareHouse_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadByOrganizationCompanyId");
    }

    /** CommonServices.JobLotGetAllService -> SP_JobLot_ReadMethod @Activity=GetAll. */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.SP_JobLot_ReadMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "GetAll");
    }

    /** InvPackingType.Getall() - @Activity only, no tenancy (as the desktop sends it). */
    public List<Map<String, Object>> packingTypes() {
        return jdbc.queryForList("EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity=?", "ReadAll");
    }

    /** CommonServices.GetUomScheduleByItemId -> Sp_UOMSchedule_GetAllMethod @Activity=ReadByItemID (Id, UOMCode, Equivalent). */
    public List<Map<String, Object>> uomSchedule(UserAccount u, int itemId) {
        return jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), itemId, "ReadByItemID");
    }

    /** [ST].[USP_ProductionJobOrder_GetAllMethod] @Activity=GetGlAccountsByJobOrderId. */
    public List<Map<String, Object>> glAccountsByJobOrder(UserAccount u, int jobOrderId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Id", jobOrderId);
        p.put("@Activity", "GetGlAccountsByJobOrderId");
        return query("[ST].[USP_ProductionJobOrder_GetAllMethod]", p);
    }

    /** InvProductionJobOrder.ReadPlantDetailByJobOrderId (Steel BLL 0236). */
    public List<Map<String, Object>> plantsByJobOrder(int jobOrderId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@Id", jobOrderId);
        p.put("@Activity", "ReadPlantDetailByJobOrderId");
        return query("[ST].[USP_ProductionJobOrder_GetAllMethod]", p);
    }

    /** CommonServices.VoucherHeadIdGet -> Sp_Vouchers_GetMethods GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId. */
    public int voucherHeadId(UserAccount u, int documentTypeId, int documentId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId");
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@DocumentTypeId", documentTypeId);
        p.put("@DocumentTypeSrNo", documentId);
        List<Map<String, Object>> rows = query("dbo.Sp_Vouchers_GetMethods", p);
        if (rows.isEmpty()) return 0;
        Object v = ci(rows.get(0), "Id");
        if (v == null && !rows.get(0).isEmpty()) v = rows.get(0).values().iterator().next();
        return toInt(v);
    }

    /** DMSAttachments.GetByID(id, screenName) - Sp_DMSAttachments_GetAllMethod @Activity=ReadById. */
    public List<Map<String, Object>> attachments(String screenName, int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbc.queryForList(
                "EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?, @Id=?, @Activity=?", screenName, id, "ReadById")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Attachment", ci(r, "Attachment"));
            out.add(o);
        }
        return out;
    }

    // ==================================================================================== helpers

    public static List<Map<String, Object>> idName(List<Map<String, Object>> rows, String idCol, String nameCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, idCol));
            o.put("Name", ci(r, nameCol));
            out.add(o);
        }
        return out;
    }

    public static List<Map<String, Object>> literal(String[][] pairs) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (String[] p : pairs) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", Integer.valueOf(p[0]));
            o.put("Name", p[1]);
            out.add(o);
        }
        return out;
    }

    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    /** Conversion.ToInt - Convert.ToInt32: numbers round half-to-even, non-integral strings give 0. */
    public static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        if (v instanceof Number) {
            double d = ((Number) v).doubleValue();
            if (Double.isNaN(d) || Double.isInfinite(d)) return 0;
            return (int) Math.rint(d);
        }
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s.startsWith("+") ? s.substring(1) : s); }
        catch (NumberFormatException e) {
            try { return (int) Math.rint(Double.parseDouble(s.replace(",", ""))); }
            catch (NumberFormatException e2) { return 0; }
        }
    }

    public static double toDouble(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) {
            double d = ((Number) v).doubleValue();
            return Double.isInfinite(d) || Double.isNaN(d) ? 0 : d;
        }
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0; }
    }

    public static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        if (v == null) return false;
        String s = String.valueOf(v).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s) || "yes".equalsIgnoreCase(s);
    }

    public static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    public static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }

    /** "yyyy-MM-dd" from whatever the page or the driver handed back. */
    public static String day(Object v) {
        Date d = parseDate(v);
        if (d == null) return v == null ? "" : String.valueOf(v);
        return new SimpleDateFormat("yyyy-MM-dd", Locale.ENGLISH).format(d);
    }

    /** A Timestamp at midnight for @DocDate style parameters; null when unparseable. */
    public static Timestamp ts(Object v) {
        Date d = parseDate(v);
        return d == null ? null : new Timestamp(d.getTime());
    }

    public static Timestamp now() { return new Timestamp(System.currentTimeMillis()); }

    public static Date parseDate(Object v) {
        if (v == null) return null;
        if (v instanceof Date) return (Date) v;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        for (String pattern : Arrays.asList("yyyy-MM-dd'T'HH:mm:ss", "yyyy-MM-dd HH:mm:ss.S", "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-dd", "dd-MMM-yyyy", "dd/MM/yyyy")) {
            try {
                SimpleDateFormat f = new SimpleDateFormat(pattern, Locale.ENGLISH);
                f.setLenient(false);
                return f.parse(s);
            } catch (java.text.ParseException ignored) { /* next */ }
        }
        return null;
    }

    public static Map<String, Object> ok(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        if (message != null) m.put("message", message);
        return m;
    }

    @SuppressWarnings("unchecked")
    public static List<Map<String, Object>> list(Object o) {
        return o instanceof List ? (List<Map<String, Object>>) o : new ArrayList<>();
    }

    @SuppressWarnings("unchecked")
    public static Map<String, Object> map(Object o) {
        return o instanceof Map ? (Map<String, Object>) o : new LinkedHashMap<>();
    }
}
