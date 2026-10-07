package com.mst.services;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.*;

/**
 * Screen 6 "Update COA Allocation" = Architecture.WinApp.Reconciliation.COAAllocation (the "COA Allocation" form: GL page number and
 * Is Active per chart-of-account row; not the cost-centre allocation page).
 *
 *   DayBookVoucher_Load: ParentAccountTitleFill -> ChartofAccount.ReadAllAccountgroup (Sp_ChartofAccount_GetAllMethodFromCOA
 *       @CoaType='ReadAllAccountGroup', @Account_Level=3; FinancialYearId is sent by the BLL but the procedure does not use it);
 *       AccountTitleFill -> CommonServices.GetAllDetailAccount (@CoaType='DetailAccount').
 *   btnSearch_Click -> COAAllocation.ReadAllForAllocation (Sp_COAAllocation_GetAllMethod @Activity='ReadAllForAllocation',
 *       @ParentAccountCode).
 *   CmbAccountTitle_Leave -> COAAllocation.GetPageNoandStatusbyAccountId (@Activity='GetPageNoandStatusbyAccountId', @Id).
 *   btnAccountUpdate / btnParentUpdate -> COAAllocation.UpdatePageNo (Sp_COAAllocation_UpdatePageNoandIsActive
 *       @ChartofAccountId, @CompanyId, @GLPageNo, @IsActive, @EntryUser = UserAccount.ID).
 * Tenancy (organization, company, user) always comes from CurrentUserContext. The procedure's UPDATE is keyed by
 * ChartofAccountId + CompanyId only, so every id posted by the browser is first re-checked against the organization / company.
 */
@Service
public class CoaAllocationUpdateService {
    private static final String ALLOC = "Sp_COAAllocation_GetAllMethod";

    @Autowired private JdbcTemplate jdbc;
    @Autowired private CurrentUserContext ctx;
    @Autowired private AccountReportsDesktopService reports;

    private static Object ci(Map<String, Object> r, String key) {
        if (r.containsKey(key)) return r.get(key);
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    private static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        try { return (int) Double.parseDouble(o.toString().trim()); } catch (Exception e) { return 0; }
    }

    /** DayBookVoucher_Load. */
    public Map<String, Object> load() {
        Map<String, Object> m = new LinkedHashMap<>();
        List<Map<String, Object>> parents = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_ChartofAccount_GetAllMethodFromCOA", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "Account_Level", 3, "FinancialYearId", ctx.currentFinancialYearId(), "CoaType", "ReadAllAccountGroup"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("AccountCode", ci(r, "AccountCode"));
            o.put("AccountTitle", ci(r, "AccountTitle"));
            parents.add(o);
        }
        List<Map<String, Object>> accounts = new ArrayList<>();
        for (Map<String, Object> r : reports.detailAccounts(null)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("AccountTitle", ci(r, "AccountTitle"));
            accounts.add(o);
        }
        m.put("parentAccounts", parents);
        m.put("accountTitles", accounts);
        return m;
    }

    /** DetailGridFill. Columns of the procedure, in its order; the form shows AccountCode, AccountTitle, GLPageNo. */
    public List<Map<String, Object>> rows(String parentAccountCode) {
        if (parentAccountCode == null || parentAccountCode.isEmpty())
            throw new IllegalArgumentException("Object reference not set to an instance of an object.");   // CmbParentAccount.Value.ToString() on null
        List<Map<String, Object>> raw = DesktopProc.rows(jdbc, ALLOC, DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(), "ParentAccountCode", parentAccountCode, "Activity", "ReadAllForAllocation"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String k : new String[]{"OrganizationId", "CompanyId", "Id", "AccountCode", "AccountTitle", "ChartofAccountId", "IsActive", "GLPageNo", "ParentAccountCode"})
                o.put(k, ci(r, k));
            out.add(o);
        }
        return out;
    }

    /** CmbAccountTitle_Leave: the first row, or an empty map when the procedure returns none. */
    public Map<String, Object> pageNoAndStatus(int accountId) {
        List<Map<String, Object>> raw = DesktopProc.rows(jdbc, ALLOC, DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(), "Id", accountId, "Activity", "GetPageNoandStatusbyAccountId"));
        Map<String, Object> o = new LinkedHashMap<>();
        if (raw.isEmpty()) return o;
        Map<String, Object> r = raw.get(0);
        o.put("GLPageNo", ci(r, "GLPageNo") == null ? "" : ci(r, "GLPageNo").toString());
        o.put("IsActive", toInt(ci(r, "IsActive")) != 0);
        o.put("CompanyId", ci(r, "CompanyId"));
        o.put("ChartofAccountId", ci(r, "ChartofAccountId"));   // null (no COAAllocation row): Convert.ToInt32(DBNull) fails on the desktop -> "Record Not Found"
        return o;
    }

    private void updatePageNo(int chartofAccountId, String glPageNo, boolean isActive) {
        DesktopProc.scalar(jdbc, "Sp_COAAllocation_UpdatePageNoandIsActive", DesktopProc.params("ChartofAccountId", chartofAccountId,
                "CompanyId", ctx.currentCompanyId(), "GLPageNo", glPageNo == null ? "" : glPageNo, "IsActive", isActive,
                "EntryUser", ctx.currentUserId()));
    }

    /** btnAccountUpdate_Click. chartofAccountId is the id the last Leave returned (0 when none: the procedure then changes nothing). */
    public void updateAccount(int chartofAccountId, String glPageNo, boolean isActive) {
        if (chartofAccountId != 0 && pageNoAndStatus(chartofAccountId).isEmpty())
            throw new IllegalArgumentException("Record does not belong to the current company");
        updatePageNo(chartofAccountId, glPageNo, isActive);
    }

    /** btnParentUpdate_Click: every row of the grid, IsActive = true. */
    public void updateRows(String parentAccountCode, List<Map<String, Object>> rows) {
        Set<Integer> allowed = new HashSet<>();
        for (Map<String, Object> r : rows(parentAccountCode)) allowed.add(toInt(r.get("Id")));
        for (Map<String, Object> r : rows) {
            int id = toInt(r.get("id"));
            if (!allowed.contains(id)) throw new IllegalArgumentException("Record does not belong to the current company");
        }
        for (Map<String, Object> r : rows) {
            Object g = r.get("glPageNo");
            updatePageNo(toInt(r.get("id")), g == null ? "" : g.toString(), true);
        }
    }
}
