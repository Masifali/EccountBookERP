package com.mst.services;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.*;

/**
 * Screen 13 "Capital Owner Equity Report" = Architecture.WinApp.Account_Reports.CapitalOwnerEquityReport.
 *
 *   VoucherValidation_Load / btnRefreshSelectedTrial_Click -> AccountTitleFill:
 *       ChartofAccount.ReadAllAccountgroup (Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='ReadAllAccountGroup'), then
 *       dt.Select("AccountClass = 1 AND Account_Level = 3").CopyToDataTable() bound to cmbAccountTitle (Id / AccountTitle).
 *   btnShowSelectedTrial_Click -> VoucherReports.SelectedTrialBalanceNew -> SpAccounts_TrialBalanceSelectedNew_Report with
 *       OrganizationId, CompanyId, UserId, LanguageId (0), FromDate = ActiveYr.Start_Period, ToDate = datToDateST,
 *       GroupAccountId = cmbAccountTitle.Value, ApprovedFilter "All" (IsApproved not sent), ZeroBalanceType 1 when Skip Zero.
 *       The form sets no BranchesIds / DocumentTypeIds / City / Custom group / Cl Debit / Cl Credit.
 *       Tables[0] rows are copied into dtHeader (AccountId, AccountCode, AccountTitle, Closing, Prct, Profit).
 */
@Service
public class CapitalOwnerEquityService {

    @Autowired private JdbcTemplate jdbc;
    @Autowired private CurrentUserContext ctx;
    @Autowired private AccountReportsDesktopService reports;

    private static double dbl(Object o) {
        if (o == null) return 0.0;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(o.toString().trim().replace(",", "")); } catch (Exception e) { return 0.0; }
    }

    private static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(o.toString().trim()); } catch (Exception e) { return 0; }
    }

    private static Object ci(Map<String, Object> r, String key) {
        if (r.containsKey(key)) return r.get(key);
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    /** AccountTitleFill. An empty source after the filter is CopyToDataTable's own InvalidOperationException text. */
    public List<Map<String, Object>> accountTitles() {
        List<Map<String, Object>> all = reports.accountGroups();
        List<Map<String, Object>> out = new ArrayList<>();
        if (all.isEmpty()) return out;                          // if (dt.Rows.Count > 0) ... nothing bound
        for (Map<String, Object> r : all) {
            if (toInt(ci(r, "AccountClass")) == 1 && toInt(ci(r, "Account_Level")) == 3) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", ci(r, "Id"));
                o.put("AccountTitle", ci(r, "AccountTitle"));
                out.add(o);
            }
        }
        if (out.isEmpty()) throw new IllegalStateException("The source contains no DataRows.");
        return out;
    }

    public Map<String, Object> load() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("accountTitles", accountTitles());
        m.put("activeYear", reports.activeYear());
        return m;
    }

    /** btnShowSelectedTrial_Click. profit = txtProfit.Text; the Prct / Profit columns are computed exactly as the form does. */
    public List<Map<String, Object>> show(Integer accountId, String toDate, boolean skipZero, String profitText) {
        if (accountId == null || accountId == 0) throw new IllegalArgumentException("Account Title Required");
        Map<String, Object> year = reports.activeYear();
        Map<String, Object> p = DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "UserId", nz(ctx.currentUserId()), "LanguageId", 0,
                "FromDate", AccountReportsDesktopService.date((String) year.get("startPeriod")),
                "ToDate", AccountReportsDesktopService.date(toDate),
                "GroupAccountId", nz(accountId), "ZeroBalanceType", skipZero ? 1 : null);
        List<Map<String, Object>> raw = DesktopProc.rows(jdbc, "SpAccounts_TrialBalanceSelectedNew_Report", p);
        List<Map<String, Object>> out = new ArrayList<>();
        if (raw.isEmpty()) return out;                          // grdSelected.ClearStructure()
        double profit = dbl(profitText);
        double total = 0.0;                                     // dtTrialSelected.Compute("SUM(Closing)")
        for (Map<String, Object> r : raw) total += dbl(ci(r, "Closing"));
        for (Map<String, Object> r : raw) {
            double closing = dbl(ci(r, "Closing"));
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("AccountId", ci(r, "AccountId"));
            o.put("AccountCode", ci(r, "AccountCode"));
            o.put("AccountTitle", ci(r, "AccountTitle"));
            o.put("Closing", closing);
            o.put("Prct", closing != 0.0 ? closing / total * 100.0 : 0.0);
            o.put("Profit", profit > 0.0 ? (closing != 0.0 ? closing / total * profit : 0.0) : 0.0);
            out.add(o);
        }
        return out;
    }

    private static Integer nz(Integer v) { return v == null || v == 0 ? null : v; }
}
