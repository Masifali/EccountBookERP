package com.mst.security.desktop;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Architecture.DAL.UserAccount (DAL 0078), login surface, ported method for method.
 *
 * ---------------------------------------------------------------------------------------------
 * WHY A ROW IS A Map AND NOT AN ENTITY
 * ---------------------------------------------------------------------------------------------
 * The C# DAL hands its rows to GenericProvider.GetProc / GetAllDetail, which map result columns
 * onto the model's properties BY NAME and ignore anything that does not match. Several of these
 * procedures return virtual properties - RoleName, CompName, OrgName, CompAddress, LicenseKey,
 * SystemDate - that are not columns of dbo.UserAccount and so cannot live on a JPA entity mapped
 * to that table.
 *
 * Reproducing that faithfully means keeping the row as what it is: whatever columns the procedure
 * chose to return. {@link #column} reads one case-insensitively and yields null when the procedure
 * did not select it, which is exactly GetProc's behaviour for an unmatched property. Forcing these
 * rows into an entity would silently drop the columns the login flow actually needs.
 *
 * ---------------------------------------------------------------------------------------------
 * TWO LOGIN PROCEDURES, AND THEY ARE NOT THE SAME ONE
 * ---------------------------------------------------------------------------------------------
 *   {@link #login(String, String)}    -&gt; Proc_UserAccount_Login   (DAL 0078 :304, with FillDTO)
 *   {@link #getDataAll(String, Map)}  -&gt; Sp_UserAccount_Login     (what BLL 0087 Login calls)
 *
 * LoginNew.cs goes through the BLL, so Sp_UserAccount_Login is the one the desktop's login screen
 * actually uses. Proc_UserAccount_Login is ported because it is in the DAL, and is marked as the
 * path nothing in the login screen takes - not quietly merged into the other.
 */
@Repository
public class DesktopUserAccountDal {

    private static final Logger LOG = LoggerFactory.getLogger(DesktopUserAccountDal.class);

    private final JdbcTemplate jdbc;
    public DesktopUserAccountDal(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ============================================================================ helpers

    /** Builds `EXEC dbo.<proc> @A=?, @B=?` with the parameters bound, never concatenated. */
    public List<Map<String, Object>> exec(String procName, Map<String, Object> params) {
        StringBuilder sql = new StringBuilder("EXEC ").append(qualify(procName));
        List<Object> values = new ArrayList<>();
        boolean first = true;
        if (params != null) {
            for (Map.Entry<String, Object> e : params.entrySet()) {
                sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
                values.add(e.getValue());
                first = false;
            }
        }
        return jdbc.queryForList(sql.toString(), values.toArray());
    }

    /** The C# names some procedures "[dbo].[X]" and others plain "X"; both mean the same thing. */
    private static String qualify(String procName) {
        String p = procName.trim();
        if (p.startsWith("[") || p.toLowerCase().startsWith("dbo.")) return p;
        return "dbo." + p;
    }

    /** An ordered parameter map - order is kept so the generated EXEC reads like the C# list. */
    public static Map<String, Object> params() { return new LinkedHashMap<>(); }

    /** GetProc's behaviour for a property the result set does not carry: null, not an error. */
    public static Object column(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    public static String stringOf(Map<String, Object> row, String name) {
        Object v = column(row, name);
        return v == null ? null : String.valueOf(v).trim();
    }

    public static int intOf(Map<String, Object> row, String name) {
        Object v = column(row, name);
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToBool - SQL bit, 0/1, "True"/"False". */
    public static boolean boolOf(Map<String, Object> row, String name) {
        Object v = column(row, name);
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = String.valueOf(v).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }

    // ============================================================================ readers

    /**
     * DAL 0078 :289 GetDataAll - a flat read, no child collections.
     * This is the one BLL.Login uses, against Sp_UserAccount_Login.
     */
    public List<Map<String, Object>> getDataAll(String procName, Map<String, Object> params) {
        return exec(procName, params);
    }

    /**
     * DAL 0078 :211 GetData - the row PLUS its three child collections, each on its own
     * connection in the C# and each reproduced here:
     *
     *   UserAccountAllocationsList  Sp_UserAccount_GetAllMethod @Id, @Activity='GetUserAllocationByUserId'
     *   UserProfileList             SP_UserProfile_GetAllMethod @UserId, @Activity='GetUserProfileByUserId'
     *                               - first row only, and only for the FIRST account in the list,
     *                                 which is what the C# does (list[0]); not generalised here
     *   ApplicationsAllocateToUserList
     *                               [dbo].[USP_ApplicationsAllocateToUser_ForUserAccount]
     *                               @UserId, @CompanyIds = the comma-joined CompanyIds of the
     *                               allocations where CompanyId &gt; 0
     */
    public List<Map<String, Object>> getData(String procName, Map<String, Object> params) {
        List<Map<String, Object>> rows = exec(procName, params);
        if (rows.isEmpty()) return rows;

        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> row = new LinkedHashMap<>(rows.get(i));
            int id = intOf(row, "Id");

            Map<String, Object> allocParams = params();
            allocParams.put("@Id", id);
            allocParams.put("@Activity", "GetUserAllocationByUserId");
            List<Map<String, Object>> allocations = safe("UserAccountAllocationsList",
                    "Sp_UserAccount_GetAllMethod", allocParams);
            row.put("UserAccountAllocationsList", allocations);

            /* list[0] in the C# - the profile is attached to the first account only. */
            if (i == 0) {
                Map<String, Object> profileParams = params();
                profileParams.put("@UserId", id);
                profileParams.put("@Activity", "GetUserProfileByUserId");
                List<Map<String, Object>> profile = safe("UserProfileList",
                        "SP_UserProfile_GetAllMethod", profileParams);
                row.put("UserProfileList", profile.isEmpty() ? null : profile.get(0));
            }

            StringBuilder companyIds = new StringBuilder();
            for (Map<String, Object> a : allocations) {
                int companyId = intOf(a, "CompanyId");
                if (companyId > 0) {
                    if (companyIds.length() > 0) companyIds.append(',');
                    companyIds.append(companyId);
                }
            }
            Map<String, Object> appParams = params();
            appParams.put("@UserId", id);
            appParams.put("@CompanyIds", companyIds.toString());
            row.put("ApplicationsAllocateToUserList", safe("ApplicationsAllocateToUserList",
                    "[dbo].[USP_ApplicationsAllocateToUser_ForUserAccount]", appParams));

            out.add(row);
        }
        return out;
    }

    /** DAL 0078 :274 GetDataAdminUser - GetProc into the AdminUser model. */
    public List<Map<String, Object>> getDataAdminUser(String procName, Map<String, Object> params) {
        return exec(procName, params);
    }

    /** DAL 0078 :361 VerifyCode - GetProc, no child collections. */
    public List<Map<String, Object>> verifyCode(String procName, Map<String, Object> params) {
        return exec(procName, params);
    }

    /**
     * DAL 0078 :304 Login(userName, password) -&gt; Proc_UserAccount_Login, mapped by FillDTO.
     *
     * NOT the path LoginNew.cs takes - see the class note. FillDTO decrypts the Password column
     * before handing it back; that decryption is deliberately NOT done here, because nothing in
     * this port needs a decrypted password and producing one would put a clear-text credential in
     * memory for no purpose.
     */
    public Map<String, Object> login(String userName, String password) {
        Map<String, Object> p = params();
        p.put("@UserName", userName);
        p.put("@Password", password);
        List<Map<String, Object>> rows = exec("Proc_UserAccount_Login", p);
        return rows.isEmpty() ? null : rows.get(0);
    }

    /**
     * A child read whose failure must not take the parent down. The C# runs each on its own
     * connection inside the same try, so a failure there aborts GetData - but it also has a
     * desktop's stack trace in front of an operator. Here it is logged by name and yields an
     * empty list, which is the difference between "this user has no allocations" being visible
     * in the log and being silent.
     */
    private List<Map<String, Object>> safe(String label, String procName, Map<String, Object> p) {
        try {
            return exec(procName, p);
        } catch (Exception e) {
            LOG.warn("UserAccount child read '{}' via {} failed", label, procName, e);
            return new ArrayList<>();
        }
    }
}
