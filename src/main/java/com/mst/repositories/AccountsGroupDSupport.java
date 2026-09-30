package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * The small desktop helpers the Accounts Transaction screens 15 / 24 / 861 / 863 / 885 read on
 * load, each reproduced from the desktop source it names. Every call takes its tenancy from
 * {@link CurrentUserContext}; nothing here accepts an organisation, company, branch or user from
 * a request.
 *
 * <ul>
 *   <li>{@link #rights} — CommonServices.SetRightsValueInRightsObject (tblUserRights.GetByUserId,
 *       BLL 0084 → Sp_tblUserRights_GetAllMethod 'GetByUserId'), with the Admin short-circuit for
 *       Save / Update / Print / CanView AllRecord and the Delete right read from the grant row
 *       even for Admin (the desktop presets it true but lets the row override it).</li>
 *   <li>{@link #feature} — CommonServices.GetERPFeatureById (login-time cache of
 *       USP_GetERPFeaturesByCompanyId).</li>
 *   <li>{@link #config} — GlobalVariables_Helper.GetConfigValueFromGlobal: the ConfigKey of the
 *       company's configuration row with that description.</li>
 *   <li>{@link #accountsFromGlobal} — DatatableHelper.GetAccountsFromGlobalByTypeIds over
 *       clsGlobalVariables.AllAccountsWithCustomGroupId (GlobalServicesMethods
 *       .GetGlobalAllAccountsWithCustomGroup → [dbo].[USP_GETAllAccountsFromCustomGroups]
 *       @OrganizationId @CompanyId), distinct by ChartOfAccountId, first row wins, columns
 *       Id / AccountTitle / AccountCode / ParentAccountTitle / AccountClass in that order.</li>
 *   <li>{@link #glBalance} — CommonServices.GetCurrentGlBalance → VoucherHead
 *       .GetCurrentGLAccountBalance → Sp_Vouchers_GetMethods 'GetCurrentGLAccountBalance',
 *       column CurrentBalance.</li>
 * </ul>
 */
@Repository
public class AccountsGroupDSupport {

    private static final Logger LOG = LoggerFactory.getLogger(AccountsGroupDSupport.class);

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;

    public AccountsGroupDSupport(JdbcTemplate jdbc, CurrentUserContext ctx) {
        this.jdbc = jdbc;
        this.ctx = ctx;
    }

    public JdbcTemplate jdbc() { return jdbc; }

    public UserAccount user() { return ctx.requireAccountingUser(); }

    public int yearId() { return ctx.currentFinancialYearId(); }

    /**
     * clsGlobalVariables.ActiveYr.Start_Period — the active year's row of
     * Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId (the list LoginNew picks it from).
     */
    public String yearStart() {
        UserAccount u = user();
        int year = yearId();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (toInt(r.get("Id")) == year) return r.get("Start_Period") == null ? "" : String.valueOf(r.get("Start_Period"));
        }
        return "";
    }

    public int branchId() {
        Integer b = user().getBranchesId();
        return b == null ? 0 : b;
    }

    // ================================================================================= rights

    public Map<String, Boolean> rights(String screenName) {
        UserAccount u = user();
        String role = ctx.currentRoleName();
        boolean admin = "Admin".equals(role);
        Map<String, Boolean> r = new LinkedHashMap<>();
        r.put("canView", false);
        r.put("canSave", admin);
        r.put("canUpdate", admin);
        r.put("canDelete", admin);
        r.put("canPrint", admin);
        r.put("canViewAll", admin);
        try {
            for (Map<String, Object> row : DesktopProc.rows(jdbc, "Sp_tblUserRights_GetAllMethod", DesktopProc.params(
                    "UserId", u.getId(), "ScreenName", screenName, "RightName", role == null ? "" : role,
                    "CompanyId", u.getCompanyId(), "Activity", "GetByUserId"))) {
                String name = row.get("RightName") == null ? "" : String.valueOf(row.get("RightName")).trim();
                boolean v = toBool(row.get("Value"));
                switch (name) {
                    case "View": r.put("canView", v); break;
                    case "Save": r.put("canSave", admin || v); break;
                    case "Update": r.put("canUpdate", admin || v); break;
                    case "Print": r.put("canPrint", admin || v); break;
                    case "CanView AllRecord": r.put("canViewAll", admin || v); break;
                    case "Delete": r.put("canDelete", v); break;
                    default: break;
                }
            }
        } catch (RuntimeException e) {
            LOG.warn("Rights for {} could not be read; the non-admin answer is 'denied'", screenName, e);
        }
        return r;
    }

    public void requireRight(String screenName, String key, String message) {
        if (!Boolean.TRUE.equals(rights(screenName).get(key))) throw new IllegalStateException(message);
    }

    // ============================================================================ feature/config

    public boolean feature(int featureId) {
        UserAccount u = user();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (toInt(r.get("Id")) == featureId) return true;
        }
        return false;
    }

    public String config(String description) {
        UserAccount u = user();
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v);
    }

    // ================================================================================ accounts

    /**
     * DatatableHelper.GetAccountsFromGlobalByTypeIds(withTypeIds, withoutTypeIds, accountTitle,
     * withClassIds, withoutClassIds, withPlNoteIds, withOutPlNoteIds) — the three filters the
     * screens of this group use. An empty or null set means "not applied", as on the desktop.
     */
    public List<Map<String, Object>> accountsFromGlobal(Set<Integer> withTypeIds, Set<Integer> withoutTypeIds,
                                                        Set<Integer> withoutPlNoteIds) {
        UserAccount u = user();
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> a : DesktopProc.rows(jdbc, "[dbo].[USP_GETAllAccountsFromCustomGroups]",
                DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            int type = toInt(a.get("AccountTypeId"));
            if (withTypeIds != null && !withTypeIds.isEmpty() && !withTypeIds.contains(type)) continue;
            if (withoutTypeIds != null && !withoutTypeIds.isEmpty() && withoutTypeIds.contains(type)) continue;
            if (withoutPlNoteIds != null && !withoutPlNoteIds.isEmpty() && withoutPlNoteIds.contains(toInt(a.get("PLNoteId")))) continue;
            int id = toInt(a.get("ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("AccountTitle", str(a.get("AccountTitle")));
            o.put("AccountCode", str(a.get("AccountCode")));
            o.put("ParentAccountTitle", str(a.get("ParentAccountTitle")));
            o.put("AccountClass", str(a.get("AccountClassName")));
            out.add(o);
        }
        return out;
    }

    /** Sp_Vouchers_GetMethods 'GetCurrentGLAccountBalance' — the CurrentBalance column, 0 without a row. */
    public double glBalance(int accountId, String voucherDate) {
        UserAccount u = user();
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "RefAccountId", accountId, "VoucherDate", voucherDate, "Activity", "GetCurrentGLAccountBalance"));
        if (r.isEmpty()) return 0d;
        return toDouble(r.get(0).get("CurrentBalance"));
    }

    // ================================================================================ helpers

    public static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        if (o == null) return 0;
        String s = String.valueOf(o).trim();
        if (s.isEmpty()) return 0;
        try { return (int) Double.parseDouble(s.replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }

    public static double toDouble(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        if (o == null) return 0d;
        String s = String.valueOf(o).trim().replace(",", "");
        if (s.isEmpty()) return 0d;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0d; }
    }

    /** Conversion.ToBool: "1" and "true" are true. */
    public static boolean toBool(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        if (o == null) return false;
        String s = String.valueOf(o).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }

    public static String str(Object o) { return o == null ? "" : String.valueOf(o); }
}
