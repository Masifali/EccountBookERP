package com.mst.services.banking;

import java.math.BigDecimal;
import java.sql.Time;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import com.mst.services.AccountReportsDesktopService;
import com.mst.services.StoreScreenRights;

/**
 * Screen 705 "Bank Balance Entry" - desktop form Architecture.WinApp.Account_Definition.BankBalanceManualEntry
 * (base.Name "BankBalanceManualEntry"), BLL/DAL/Model Architecture.*.BankBalanceManual.
 *
 * <ul>
 * <li>FrmBankBalancesRpt_Load: rights (btnSave.Enabled = DoHaveSaveRight), SourceByBindFromStaticColumns
 *     (CommonServices.StaticColumnsService("SourceBy") -> GeneralReprots.StaticColumnNames -> SpStaticColumnNames
 *     @Activity='SourceBy': Id/type rows), txtBalanceDate = today, SummeryGrd().</li>
 * <li>SummeryGrd (Load, Show, New, after Save): VoucherReports.CashandBankBalancesSummery ->
 *     Sp_Accounts_CashBankBalancesSummery_Rpt @FinancialYearId, @OrganizationId, @CompanyId, @UserId,
 *     @FromDate = @ToDate = txtBalanceDate, @AccountTypeId = 15 (ApprovedFilter "All" -> no @IsApproved;
 *     BranchesId / ProjectsId / PageNumber / PageSize / BranchesIds / LanguageId are 0/null -> left out).
 *     Tables[0] is re-shaped into the grid columns Time, AccountId, AccountCode, AccountTitle,
 *     ERPBalance (Closing), BankBalance (ManualBankBalance), Source (BySourceId), ConfirmBy (ConfirmedBy).</li>
 * <li>btnSave_Click: every grid row with BankBalance &gt; 0 -> one BankBalanceManual; BLL Save with Id 0 ->
 *     DAL SetData: USP_AcBankBalanceManualEntry_Insert once per row in ONE transaction (the proc itself updates
 *     the row of the same account + date when it exists). GenericProvider.SetProc binds every non-virtual
 *     model property with AddWithValue: Id, ChartOfAccountId, BalanceDate, BalanceTime, Balance, EntryUserId,
 *     EntryDate, BySrource, BySourceDescription, OrganizationId, CompanyId. No message on success; SummeryGrd().</li>
 * </ul>
 * The form has no Update, Delete, history or print button (btnPrintSummery_Click exists in the class but no
 * control is wired to it).
 */
@Service
public class BankBalanceManualEntryService {

    public static final String SCREEN_NAME = "BankBalanceManualEntry";

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;
    private final AccountReportsDesktopService reportsDesktop;

    public BankBalanceManualEntryService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights,
                                         AccountReportsDesktopService reportsDesktop) {
        this.jdbc = jdbc;
        this.ctx = ctx;
        this.rights = rights;
        this.reportsDesktop = reportsDesktop;
    }

    /** CommonServices.SetRightsValueInRightsObject(base.Name). */
    public Map<String, Boolean> rights() {
        return rights.of(SCREEN_NAME);
    }

    /** SourceByBindFromStaticColumns: SpStaticColumnNames @Activity='SourceBy' (columns Id, type). */
    public List<Map<String, Object>> sourceBy() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "SpStaticColumnNames", DesktopProc.params("Activity", "SourceBy"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", r.get("Id"));
            m.put("type", r.get("type") == null ? "" : String.valueOf(r.get("type")));
            out.add(m);
        }
        return out;
    }

    /** clsGlobalVariables.stringFormatboth decimals: config "Default NoofDecimal Points For Amount". */
    public int amountDecimals() {
        try {
            String v = reportsDesktop.config("Default NoofDecimal Points For Amount");
            return v == null || v.isEmpty() ? 0 : Integer.parseInt(v.trim());
        } catch (Exception e) {
            return 0;
        }
    }

    /** SummeryGrd(). */
    public List<Map<String, Object>> summary(LocalDate balanceDate) {
        LocalDate d = balanceDate != null ? balanceDate : LocalDate.now();
        List<Map<String, Object>> raw = DesktopProc.rows(jdbc, "Sp_Accounts_CashBankBalancesSummery_Rpt", DesktopProc.params(
                "FinancialYearId", ctx.currentFinancialYearId(),
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "UserId", ctx.currentUserId(),
                "FromDate", java.sql.Date.valueOf(d),
                "ToDate", java.sql.Date.valueOf(d),
                "AccountTypeId", 15));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> g = new LinkedHashMap<>();
            /* BalanceTime is CAST(mb.BalanceTime AS TIME); empty -> the page puts the current time ("hh:mm tt"). */
            g.put("Time", timeText(r.get("BalanceTime")));
            g.put("AccountId", r.get("AccountId"));
            g.put("AccountCode", r.get("AccountCode"));
            g.put("AccountTitle", r.get("AccountTitle"));
            g.put("ERPBalance", r.get("Closing"));
            g.put("BankBalance", r.get("ManualBankBalance"));
            g.put("Source", r.get("BySourceId"));
            g.put("ConfirmBy", r.get("ConfirmedBy"));
            out.add(g);
        }
        return out;
    }

    /** One grid row posted by the page. */
    public static class Row {
        public int accountId;
        public String time;          // HH:mm[:ss]
        public BigDecimal bankBalance;
        public int source;           // Conversion.ToInt(Source.Value)
        public String sourceText;    // Source.Text
    }

    /** btnSave_Click -> BankBalanceManual.Save (Id 0 -> USP_AcBankBalanceManualEntry_Insert per row, one transaction). */
    @Transactional
    public int save(LocalDate balanceDate, List<Row> rows) {
        if (!Boolean.TRUE.equals(rights.of(SCREEN_NAME).get("save"))) {
            throw new IllegalArgumentException("You do not have the Save right for this screen.");
        }
        if (rows == null || rows.isEmpty()) return 0;
        LocalDateTime now = LocalDateTime.now();
        /* txtBalanceDate.Value: the picked day with the time of day the picker was given at Load (DateTime.Now). */
        LocalDateTime balDate = (balanceDate != null ? balanceDate : now.toLocalDate()).atTime(now.toLocalTime());
        int org = ctx.currentOrganizationId(), comp = ctx.currentCompanyId(), user = ctx.currentUserId();
        int result = 0, n = 0;
        for (Row r : rows) {
            if (r == null || r.bankBalance == null || r.bankBalance.signum() <= 0) continue;   // ToDouble(BankBalance) > 0
            LocalTime t = parseTime(r.time, now.toLocalTime());
            result = DesktopProc.setProc(jdbc, "USP_AcBankBalanceManualEntry_Insert", DesktopProc.params(
                    "BalanceDate", java.sql.Timestamp.valueOf(balDate),
                    "BalanceTime", java.sql.Timestamp.valueOf(balDate.toLocalDate().atTime(t)),
                    "EntryDate", java.sql.Timestamp.valueOf(now),
                    "Balance", r.bankBalance,
                    "BySrource", r.source,
                    "ChartOfAccountId", r.accountId,
                    "CompanyId", comp,
                    "EntryUserId", user,
                    "Id", 0,
                    "OrganizationId", org,
                    "BySourceDescription", r.sourceText == null ? "" : r.sourceText));
            n++;
        }
        return n == 0 ? 0 : result;
    }

    private static LocalTime parseTime(String s, LocalTime fallback) {
        if (s == null || s.trim().isEmpty()) return fallback;
        String v = s.trim();
        try {
            if (v.length() > 8) v = v.substring(0, 8);
            return LocalTime.parse(v.length() == 5 ? v + ":00" : v);
        } catch (Exception e) {
            return fallback;
        }
    }

    private static String timeText(Object v) {
        if (v == null) return "";
        if (v instanceof Time) return ((Time) v).toLocalTime().toString();
        if (v instanceof LocalTime) return v.toString();
        String s = String.valueOf(v).trim();
        return s.length() > 8 ? s.substring(0, 8) : s;
    }
}
