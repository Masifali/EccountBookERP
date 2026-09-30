package com.mst.services;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 10 "Account Opening Balance" - desktop form Architecture.WinApp.AcfrmOpeningBalance,
 * BLL/DAL Accounts.AccountsOpeningBalances.
 *
 * <ul>
 * <li>fillDataGrid / FindAccountTitle: Sp_AccountsOpeningBalances_GetMethod @OrganizationId,
 *     @CompanyId, @FinancialYearId (the login year), @UserId (EntryUser != 0), @Activity='GetAll'.
 *     The combo's value is the opening-balance row Id, its text ChartOfAccountTitle.</li>
 * <li>cmbAccountTitle_Leave / grid double-click: @Id, @OrganizationId, @CompanyId,
 *     @FinancialYearId, @Activity='GetById'.</li>
 * <li>Update Single (SingleRecordUpdate) and Update All (AllRecordUpdate): Sp_AccountsOpeningBalances_Update
 *     with every non-virtual model property (BranchesId is 0 - the model never sets it).
 *     Update All runs in one transaction.</li>
 * </ul>
 * Rights: both updates need the Update right (BtnEdit / button1 are disabled without it).
 */
@Service
public class OpeningBalanceDesktopService {

    public static final String SCREEN_NAME = "AcfrmOpeningBalance";

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;

    public OpeningBalanceDesktopService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights) {
        this.jdbc = jdbc;
        this.ctx = ctx;
        this.rights = rights;
    }

    public Map<String, Boolean> rights() {
        return rights.of(SCREEN_NAME);
    }

    public List<Map<String, Object>> getAll() {
        int user = ctx.currentUserId();
        return DesktopProc.rows(jdbc, "Sp_AccountsOpeningBalances_GetMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(),
                "UserId", user != 0 ? user : null,
                "Activity", "GetAll"));
    }

    public Map<String, Object> getById(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_AccountsOpeningBalances_GetMethod", DesktopProc.params(
                "Id", id,
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(),
                "Activity", "GetById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** SingleRecordUpdate: FormValidation, then "ChartofAccount Id Not Found" when RId == 0. */
    @Transactional
    public String updateSingle(int comboValue, int recId, int chartOfAccountId, String comboText,
                               String debitText, String creditText) {
        requireUpdateRight();
        double debit = toDouble(debitText == null ? "" : debitText.trim());
        double credit = toDouble(creditText == null ? "" : creditText.trim());
        if (comboValue == 0) throw new IllegalArgumentException("Account Title Field is Required");
        if (debit > 0.0 && credit > 0.0) throw new IllegalArgumentException("Debit Amount and Credit Amount Both cannot be greater than zero");
        if (debit == 0.0 && credit == 0.0) throw new IllegalArgumentException("Credit Or Debit Field is Required");
        if (recId == 0) throw new IllegalArgumentException("ChartofAccount Id Not Found");
        DesktopProc.setProc(jdbc, "Sp_AccountsOpeningBalances_Update",
                model(recId, chartOfAccountId, comboText, toDouble(debitText), toDouble(creditText)));
        return "Update recored Successfully";
    }

    /**
     * AllRecordUpdate: for each grid row whose Debit differs from the loaded value -> (debit, 0); whose
     * Credit differs -> (0, credit); both may be sent for one row, in that order. ChartOfAccountTitle is
     * the top combo's text for every row, as on the desktop.
     */
    @Transactional
    public String updateAll(List<Map<String, Object>> rows, String comboText) {
        requireUpdateRight();
        List<Map<String, Object>> list = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            int id = toInt(r.get("id"));
            int coaId = toInt(r.get("chartOfAccountId"));
            double origDebit = toDouble(r.get("origDebit")), newDebit = toDouble(r.get("debit"));
            double origCredit = toDouble(r.get("origCredit")), newCredit = toDouble(r.get("credit"));
            if (origDebit != newDebit) list.add(model(id, coaId, comboText, newDebit, 0.0));
            if (origCredit != newCredit) list.add(model(id, coaId, comboText, 0.0, newCredit));
        }
        for (Map<String, Object> p : list) DesktopProc.setProc(jdbc, "Sp_AccountsOpeningBalances_Update", p);
        return "Update recored Successfully";
    }

    public boolean hasPrintRight() {
        Boolean b = rights.of(SCREEN_NAME).get("print");
        return b != null && b;
    }

    private void requireUpdateRight() {
        Boolean b = rights.of(SCREEN_NAME).get("update");
        if (b == null || !b) throw new IllegalArgumentException("You do not have the Update right for this screen.");
    }

    private Map<String, Object> model(int id, int chartOfAccountId, String title, double debit, double credit) {
        LocalDateTime now = LocalDateTime.now();
        int user = ctx.currentUserId();
        return DesktopProc.params(
                "PostState", false, "EntryDate", now, "ModifyDate", now, "PostDate", now,
                "YearObCredit", credit, "YearObDebit", debit, "ChartOfAccountId", chartOfAccountId,
                "CompanyId", ctx.currentCompanyId(), "BranchesId", 0, "EntryUser", user,
                "FinancialYearId", ctx.currentFinancialYearId(), "Id", id, "ModifyUser", user,
                "OrganizationId", ctx.currentOrganizationId(), "PostUser", 0,
                "ChartOfAccountTitle", title == null ? "" : title);
    }

    static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDouble: anything unparsable (including thousands separators left by typing) is 0 after stripping commas. */
    static double toDouble(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        if (o == null) return 0.0;
        String s = String.valueOf(o).trim().replace(",", "");
        if (s.isEmpty()) return 0.0;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0.0; }
    }
}
