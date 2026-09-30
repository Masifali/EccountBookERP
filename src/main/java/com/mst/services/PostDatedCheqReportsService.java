package com.mst.services;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.*;

/**
 * Account Reports (module 3) - the post dated cheque report forms (R2, 2026-09-30).
 *
 * PostDatedCheqInformation.cs (no ScreenDefinition row; the desktop opens it as a dialog from
 * GeneralLedger.ValuePostDatedNoOfCheqs_Click with AccountId = cmbAccountTitle.Value and
 * ToDate = txtToDate.Value): GetData -> VoucherHead.getPostDatedCheqByAccountId
 * (0654_Architecture.BLL.Accounts.VoucherHead.cs:2739) -> usp_getPostDatedCheqByAccountId with
 * @OrganizationId, @CompanyId, @AccountId, @ToDate - all four always bound.
 *
 * Screen 88 PostDatedCheqRegister.cs has no data call of its own (btnshow_Click is empty; the only
 * output is the Crystal print RptPdcInventoryRegisterCheqParty.rpt), so it needs nothing here beyond
 * the COAAllocation lookup that /accounts/api/reports-desktop/lookups/coa-allocation already serves.
 *
 * Tenancy always comes from CurrentUserContext.
 */
@Service
public class PostDatedCheqReportsService {

    @Autowired
    private JdbcTemplate jdbc;

    @Autowired
    private CurrentUserContext ctx;

    /**
     * PostDatedCheqInformation.GetData: the rows copied into dt2 (AccountId, AccountCode, AccountTitle,
     * ChequeNo, CheqDate, CheqAmount); AccountId is hidden on the grid, CheqAmount summed.
     * AccountId is Conversion.ToInt(cmbAccountTitle.Value) on the desktop, so an empty account is 0.
     * ToDate defaults to DateTime.Now in the form's constructor, which is what a missing date means here.
     */
    public List<Map<String, Object>> postDatedCheqByAccountId(Integer accountId, java.sql.Date toDate) {
        java.sql.Date to = toDate != null ? toDate : new java.sql.Date(System.currentTimeMillis());
        List<Map<String, Object>> raw = DesktopProc.rows(jdbc, "usp_getPostDatedCheqByAccountId", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "AccountId", accountId == null ? 0 : accountId,
                "ToDate", to));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("AccountId", get(r, "AccountId"));
            m.put("AccountCode", get(r, "AccountCode"));
            m.put("AccountTitle", get(r, "AccountTitle"));
            m.put("ChequeNo", get(r, "ChequeNo"));
            m.put("CheqDate", shortDate(get(r, "CheqDate")));
            m.put("CheqAmount", dbl(get(r, "CheqAmount")));
            out.add(m);
        }
        return out;
    }

    /** Column lookup that does not depend on the driver's key casing. */
    private static Object get(Map<String, Object> r, String key) {
        if (r.containsKey(key)) return r.get(key);
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey() != null && e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    private static double dbl(Object o) {
        if (o == null) return 0.0;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(o.toString().trim()); } catch (Exception e) { return 0.0; }
    }

    private static String shortDate(Object o) {
        if (o == null) return null;
        if (o instanceof java.util.Date) return new java.text.SimpleDateFormat("yyyy-MM-dd").format((java.util.Date) o);
        String s = o.toString();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }
}
