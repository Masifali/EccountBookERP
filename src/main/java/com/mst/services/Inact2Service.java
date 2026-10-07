package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.Inact2Repository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.time.LocalDate;
import java.util.*;
import org.springframework.stereotype.Service;

/**
 * Inactive Account_Reports group K - screens 54 / 55 / 57 / 58 / 59. Tenancy (Organization / Company / User / Financial Year) always
 * comes from CurrentUserContext; rights are the desktop's ScreenId "View" right (DesktopReportRights). No shared service is edited.
 */
@Service
public class Inact2Service {
    /** dbo.ScreenDefinition ids: GeneralLedgerStatment, AccountsBalanceSheetStandardRpt, GeneralJournalSummeryRegister, PayablesAging, ReceivableAging. */
    public static final int S_LEDGER_STATEMENT = 54, S_BALANCE_SHEET = 55, S_JOURNAL = 57, S_PAYABLES_AGING = 58, S_RECEIVABLE_AGING = 59;

    private final Inact2Repository repository;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public Inact2Service(Inact2Repository repository, CurrentUserContext context, DesktopReportRights rights) {
        this.repository = repository;
        this.context = context;
        this.rights = rights;
    }

    private UserAccount user(int screen) {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, screen, "View");
        return u;
    }

    /** CompLogoImage (a hidden image column) is not sent to the browser; the print path keeps the raw rows. */
    public static List<Map<String, Object>> stripBinary(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) m.put(e.getKey(), e.getValue() instanceof byte[] ? null : e.getValue());
            out.add(m);
        }
        return out;
    }

    private Map<String, Object> baseLookups(UserAccount u) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("amountDecimals", repository.amountDecimals(u));
        d.put("yearStart", repository.yearStart(u, context.currentFinancialYearId()));
        return d;
    }

    // ---- 54
    /** VoucherValidation_Load: CompanyFill, AccountTitleFill, BranchFill, ProjectFill (the same branch table), fromdate = ActiveYr.Start_Period. */
    public Map<String, Object> ledgerStatementLookups() {
        UserAccount u = user(S_LEDGER_STATEMENT);
        Map<String, Object> d = baseLookups(u);
        d.put("companies", repository.companies(u));
        d.put("coa", repository.coaForCombo(u));
        d.put("branches", repository.branches(u));
        return d;
    }
    public List<Map<String, Object>> ledgerStatement(int accountId, LocalDate from, LocalDate to, int branchId, int projectId) {
        UserAccount u = user(S_LEDGER_STATEMENT);
        return repository.glStatement(u, context.currentFinancialYearId(), accountId, from, to, branchId, projectId);
    }

    // ---- 55
    public Map<String, Object> balanceSheetLookups() { return baseLookups(user(S_BALANCE_SHEET)); }
    public List<Map<String, Object>> balanceSheet(LocalDate to) { UserAccount u = user(S_BALANCE_SHEET); return repository.balanceSheet(u, context.currentFinancialYearId(), to); }
    public List<Map<String, Object>> profitLoss(LocalDate from, LocalDate to) { UserAccount u = user(S_BALANCE_SHEET); return repository.profitLoss(u, context.currentFinancialYearId(), from, to); }
    public List<Map<String, Object>> balanceSheetFormat2(LocalDate from, LocalDate to) { return repository.balanceSheetFormat2(user(S_BALANCE_SHEET), from, to); }
    public List<Map<String, Object>> profitAndLoss(LocalDate to) { return repository.profitAndLoss(user(S_BALANCE_SHEET), to); }

    // ---- 57
    public Map<String, Object> generalJournalLookups() { return baseLookups(user(S_JOURNAL)); }
    public List<Map<String, Object>> generalJournal() { return repository.generalJournal(user(S_JOURNAL)); }

    // ---- 58
    public Map<String, Object> payablesAgingLookups() { return baseLookups(user(S_PAYABLES_AGING)); }
    public List<Map<String, Object>> payablesAging(LocalDate endDate, int agingDays, int intervalDays, boolean skipZero) {
        UserAccount u = user(S_PAYABLES_AGING);
        /* VoucherValidation_Load: Tag "PayablesAging" -> ActionId 1, AccouuntClassId 3; chkSkipZero -> ZeroBalanceType 1 */
        return repository.ledgerAging(u, endDate, agingDays, intervalDays, 1, 3, skipZero ? 1 : 0);
    }

    // ---- 59
    public Map<String, Object> receivableAgingLookups() {
        UserAccount u = user(S_RECEIVABLE_AGING);
        Map<String, Object> d = baseLookups(u);
        d.put("companies", repository.companies(u));
        return d;
    }
    public List<Map<String, Object>> receivableAging(LocalDate endDate, int intervalDays) {
        UserAccount u = user(S_RECEIVABLE_AGING);
        return repository.receivableAging(u, context.currentFinancialYearId(), endDate, intervalDays);
    }
}
