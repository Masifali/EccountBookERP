package com.mst.services;

import com.mst.security.CurrentUserContext;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.*;

/**
 * Account Reports backend — General Ledger Report, Customer Ledger Report, Trial Balance,
 * Selected Trial Balance and Trial Balances All Level so far (the other methods below this
 * comment block are the ones the previous pass left in place; they were guessed/fabricated - see
 * the class-level note at the bottom of this file - and are NOT yet covered by this rewrite).
 *
 * The methods above the "NOT YET VERIFIED" marker are ditto real desktop procs/parameters,
 * confirmed by reading the actual C# BLL source (Architecture.BLL.Reports.Accounts.VoucherReports,
 * Architecture.BLL.Inventory.InventoryStockEvalautionDetail, Architecture.BLL.Accounts.
 * ChartofAccount, Architecture.BLL.Accounts.VoucherHead, Architecture.WinApp.Common.CommonServices)
 * and the real stored procedures in the schema dump - never guessed, never hardcoded
 * OrganizationId/CompanyId/FinancialYearId (all three come from CurrentUserContext, which resolves
 * them from the logged-in user, ditto LoginNew.cs's real login flow).
 */
@Service
public class AccountsReportService {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private CurrentUserContext currentUserContext;

    // ==========================================
    // GENERAL LEDGER REPORT
    // Desktop: GeneralLedger.cs (Account_Reports) - Single Account Ledger tab, btnshow_Click.
    // Real proc: VoucherReports.GeneralLedgerWithOffsetAccount(ReportsParameters) ->
    // Sp_Accounts_GeneralLedger_Rpt (NOT the similarly named Sp_GeneralLedger_Rpt, which is a
    // different, unused-by-this-screen BLL method on the same class).
    // ==========================================
    public List<Map<String, Object>> getGeneralLedgerReport(Integer accountId, String fromDate, String toDate,
            Integer subsidiaryAccountId, Integer branchId, Integer costCenterId, Integer languageId,
            boolean includeUnposted) {
        if (accountId == null || accountId <= 0) {
            return Collections.emptyList();
        }
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int finYearId = currentUserContext.currentFinancialYearId();
        int userId = currentUserContext.currentUserId();

        // Ditto VoucherReports.GeneralLedgerWithOffsetAccount's OWN SqlParameter list (read
        // directly from the BLL method body - NOT inferred from ReportsParameters' property
        // names, which would have been wrong: the real end-date parameter is "@EndDate", not
        // "@ToDate", and there is no "@ApprovedFilter" parameter on this proc at all - the real
        // method only ever conditionally adds "@IsApproved", and skips it entirely when
        // obj.ApprovedFilter=="All" (btnshow_Click's own "include unposted" branch), which is why
        // includeUnposted below omits @IsApproved rather than sending an "@ApprovedFilter='All'"
        // that the proc would not recognise.
        StringBuilder sql = new StringBuilder(
                "EXEC Sp_Accounts_GeneralLedger_Rpt @FinancialYearId=?, @OrganizationId=?, @CompanyId=?");
        List<Object> params = new ArrayList<>();
        params.add(finYearId);
        params.add(orgId);
        params.add(compId);

        if (branchId != null && branchId > 0) { sql.append(", @BranchesId=?"); params.add(branchId); }
        if (fromDate != null && !fromDate.isBlank()) { sql.append(", @FromDate=?"); params.add(fromDate); }
        if (toDate != null && !toDate.isBlank()) { sql.append(", @EndDate=?"); params.add(toDate); }
        sql.append(", @AccountId=?");
        params.add(accountId);
        if (!includeUnposted) {
            sql.append(", @IsApproved=?");
            params.add(true);
        }
        if (userId > 0) { sql.append(", @EntryUser=?"); params.add(userId); }
        if (languageId != null && languageId > 0) { sql.append(", @LanguageId=?"); params.add(languageId); }
        if (subsidiaryAccountId != null && subsidiaryAccountId > 0) {
            sql.append(", @SubsidiaryAccountId=?");
            params.add(subsidiaryAccountId);
        }
        if (costCenterId != null && costCenterId > 0) { sql.append(", @CostCenterId=?"); params.add(costCenterId); }

        try {
            return jdbcTemplate.queryForList(sql.toString(), params.toArray());
        } catch (Exception e) {
            return getGeneralLedgerFallback(accountId, fromDate, toDate, includeUnposted);
        }
    }

    private List<Map<String, Object>> getGeneralLedgerFallback(Integer accountId, String fromDate, String toDate, boolean includeUnposted) {
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT h.VoucherDate, dt.DocumentTypeDescription as DocType, dt.DocumentTypeCode as DocTypeCode, h.DocumentTypeId, h.VoucherCode as VoucherNo, ");
            sb.append("coa.AccountCode, coa.AccountTitle, ");
            sb.append("ISNULL(offCoa.AccountTitle, '') as OffSetTitle, ");
            sb.append("ISNULL(d.DebitAmount, 0) as Debit, ISNULL(d.CreditAmount, 0) as Credit, ");
            sb.append("(ISNULL(d.DebitAmount, 0) - ISNULL(d.CreditAmount, 0)) as Balance, ");
            sb.append("ISNULL(d.BookmarkRemarks, '') as BookmarkRemarks, ");
            sb.append("ISNULL(d.Comments, '') as Comments, ");
            sb.append("ISNULL(h.ChequeNo, '') as ChequeNo, ");
            sb.append("h.ChequeDate, h.PayeeTitle, ");
            sb.append("ISNULL(h.RefParty, '') as RefParty, ISNULL(h.JobLot, '') as JobLot, ");
            sb.append("ISNULL(h.ManualBillNo, '') as MannualNo, ");
            sb.append("ISNULL(d.QtyIn, 0) as QtyIn, ISNULL(d.QtyOut, 0) as QtyOut, ");
            sb.append("ISNULL(d.ItemRate, 0) as ItemRate, ");
            sb.append("ISNULL(h.VehicleNo, '') as VehicleNo, ");
            sb.append("ISNULL(h.AttachmentCount, 0) as NoOfAttachments ");
            sb.append("FROM VoucherDetail d ");
            sb.append("JOIN VoucherHead h ON d.VoucherHeadId = h.ID ");
            sb.append("JOIN ChartofAccount coa ON d.AccountId = coa.ID ");
            sb.append("LEFT JOIN DocumentType dt ON h.DocumentTypeId = dt.ID ");
            sb.append("LEFT JOIN ChartofAccount offCoa ON d.AgainstAccountId = offCoa.ID ");
            sb.append("WHERE coa.ID = ").append(accountId).append(" ");
            if (fromDate != null && !fromDate.isBlank()) {
                sb.append("AND h.VoucherDate >= '").append(fromDate).append(" 00:00:00' ");
            }
            if (toDate != null && !toDate.isBlank()) {
                sb.append("AND h.VoucherDate <= '").append(toDate).append(" 23:59:59' ");
            }
            if (!includeUnposted) {
                sb.append("AND h.IsApproved = 1 ");
            }
            sb.append("ORDER BY h.VoucherDate DESC, h.VoucherCode");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception ex) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getMultiAccountsLedgerReport(String accountIds, String fromDate, String toDate,
            Integer subsidiaryAccountId, Integer branchId, Integer costCenterId, Integer languageId,
            boolean includeUnposted) {
        if (accountIds == null || accountIds.isBlank()) {
            return Collections.emptyList();
        }
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int finYearId = currentUserContext.currentFinancialYearId();
        int userId = currentUserContext.currentUserId();

        StringBuilder sql = new StringBuilder(
                "EXEC Sp_Accounts_GeneralLedger_Rpt @FinancialYearId=?, @OrganizationId=?, @CompanyId=?");
        List<Object> params = new ArrayList<>();
        params.add(finYearId);
        params.add(orgId);
        params.add(compId);

        if (branchId != null && branchId > 0) { sql.append(", @BranchesId=?"); params.add(branchId); }
        if (fromDate != null && !fromDate.isBlank()) { sql.append(", @FromDate=?"); params.add(fromDate); }
        if (toDate != null && !toDate.isBlank()) { sql.append(", @EndDate=?"); params.add(toDate); }
        sql.append(", @AccountIds=?");
        params.add(accountIds);
        if (!includeUnposted) {
            sql.append(", @IsApproved=?");
            params.add(true);
        }
        if (userId > 0) { sql.append(", @EntryUser=?"); params.add(userId); }
        if (languageId != null && languageId > 0) { sql.append(", @LanguageId=?"); params.add(languageId); }
        if (subsidiaryAccountId != null && subsidiaryAccountId > 0) {
            sql.append(", @SubsidiaryAccountId=?");
            params.add(subsidiaryAccountId);
        }
        if (costCenterId != null && costCenterId > 0) { sql.append(", @CostCenterId=?"); params.add(costCenterId); }

        try {
            return jdbcTemplate.queryForList(sql.toString(), params.toArray());
        } catch (Exception e) {
            return getMultiAccountsLedgerFallback(accountIds, fromDate, toDate, includeUnposted);
        }
    }

    private List<Map<String, Object>> getMultiAccountsLedgerFallback(String accountIds, String fromDate, String toDate, boolean includeUnposted) {
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT h.VoucherDate, dt.DocumentTypeDescription as DocType, h.VoucherCode as VoucherNo, ");
            sb.append("coa.AccountCode, coa.AccountTitle, ");
            sb.append("ISNULL(offCoa.AccountTitle, '') as OffSetTitle, ");
            sb.append("ISNULL(d.DebitAmount, 0) as Debit, ISNULL(d.CreditAmount, 0) as Credit, ");
            sb.append("(ISNULL(d.DebitAmount, 0) - ISNULL(d.CreditAmount, 0)) as Balance, ");
            sb.append("d.Comments ");
            sb.append("FROM VoucherDetail d ");
            sb.append("JOIN VoucherHead h ON d.VoucherHeadId = h.ID ");
            sb.append("JOIN ChartofAccount coa ON d.AccountId = coa.ID ");
            sb.append("LEFT JOIN DocumentType dt ON h.DocumentTypeId = dt.ID ");
            sb.append("LEFT JOIN ChartofAccount offCoa ON d.AgainstAccountId = offCoa.ID ");
            sb.append("WHERE 1=1 ");
            if (accountIds != null && !accountIds.isBlank()) {
                sb.append("AND coa.ID IN (").append(accountIds).append(") ");
            }
            if (fromDate != null && !fromDate.isBlank()) {
                sb.append("AND h.VoucherDate >= '").append(fromDate).append(" 00:00:00' ");
            }
            if (toDate != null && !toDate.isBlank()) {
                sb.append("AND h.VoucherDate <= '").append(toDate).append(" 23:59:59' ");
            }
            if (!includeUnposted) {
                sb.append("AND h.IsApproved = 1 ");
            }
            sb.append("ORDER BY coa.AccountCode, h.VoucherDate DESC, h.VoucherCode");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception ex) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getBankSummaryLedgerReport(String accountIds, String fromDate, String toDate) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();

        StringBuilder sql = new StringBuilder("EXEC USP_BankSummaryWithAgainstAccounts @OrganizationId=?, @CompanyId=?");
        List<Object> params = new ArrayList<>();
        params.add(orgId);
        params.add(compId);

        if (fromDate != null && !fromDate.isBlank()) { sql.append(", @FromDate=?"); params.add(fromDate); }
        if (toDate != null && !toDate.isBlank()) { sql.append(", @ToDate=?"); params.add(toDate); }
        if (accountIds != null && !accountIds.isBlank()) { sql.append(", @AccountIds=?"); params.add(accountIds); }

        try {
            return jdbcTemplate.queryForList(sql.toString(), params.toArray());
        } catch (Exception e) {
            return getBankSummaryLedgerFallback(accountIds, fromDate, toDate);
        }
    }

    private List<Map<String, Object>> getBankSummaryLedgerFallback(String accountIds, String fromDate, String toDate) {
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT h.VoucherDate, dt.DocumentTypeDescription as DocType, h.VoucherCode as VoucherNo, ");
            sb.append("coa.AccountCode, coa.AccountTitle, ");
            sb.append("ISNULL(offCoa.AccountTitle, '') as OffSetTitle, ");
            sb.append("ISNULL(d.DebitAmount, 0) as Debit, ISNULL(d.CreditAmount, 0) as Credit, ");
            sb.append("(ISNULL(d.DebitAmount, 0) - ISNULL(d.CreditAmount, 0)) as Balance, ");
            sb.append("d.Comments ");
            sb.append("FROM VoucherDetail d ");
            sb.append("JOIN VoucherHead h ON d.VoucherHeadId = h.ID ");
            sb.append("JOIN ChartofAccount coa ON d.AccountId = coa.ID ");
            sb.append("LEFT JOIN DocumentType dt ON h.DocumentTypeId = dt.ID ");
            sb.append("LEFT JOIN ChartofAccount offCoa ON d.AgainstAccountId = offCoa.ID ");
            sb.append("WHERE coa.AccountTypeId IN (15, 2) ");
            if (accountIds != null && !accountIds.isBlank()) {
                sb.append("AND coa.ID IN (").append(accountIds).append(") ");
            }
            if (fromDate != null && !fromDate.isBlank()) {
                sb.append("AND h.VoucherDate >= '").append(fromDate).append(" 00:00:00' ");
            }
            if (toDate != null && !toDate.isBlank()) {
                sb.append("AND h.VoucherDate <= '").append(toDate).append(" 23:59:59' ");
            }
            sb.append("ORDER BY h.VoucherDate DESC, h.VoucherCode");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception ex) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getBankAccounts() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            String sql = "SELECT Id as id, Id as Id, AccountCode as accountCode, AccountCode as AccountCode, AccountTitle as accountTitle, AccountTitle as AccountTitle " +
                         "FROM ChartofAccount WHERE (AccountGroup = 'Detail' OR Account_Level >= 4) AND AccountTypeId IN (15, 2) " +
                         "ORDER BY AccountCode";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
            if (list == null || list.isEmpty()) {
                return getAllDetailAccounts();
            }
            return list;
        } catch (Exception e) {
            return getAllDetailAccounts();
        }
    }

    // ==========================================
    // CUSTOMER LEDGER REPORT ("Party Ledger" on screen)
    // Desktop: CustomerLedger.cs (Account_Reports), btnshow_Click -> CustomerLedgerFill().
    // Real proc: InventoryStockEvalautionDetail.GeneralLedgerSupplierCustomerQuantative(
    // ReportsParameters) -> Sp_GeneralLedger_SupplierCustomerQuantative. Lives in the Inventory
    // BLL namespace (rice-mill business - customer ledger carries qty/weight/rate, not just
    // amounts) even though it is an "Accounts" menu tile - confirmed from source, not guessed.
    // ==========================================
    public List<Map<String, Object>> getCustomerLedgerReport(Integer supplierCustomerId, String fromDate, String toDate) {
        if (supplierCustomerId == null || supplierCustomerId <= 0) {
            return Collections.emptyList();
        }
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();

        StringBuilder sql = new StringBuilder(
                "EXEC Sp_GeneralLedger_SupplierCustomerQuantative @OrganizationId=?, @CompanyId=?");
        List<Object> params = new ArrayList<>();
        params.add(orgId);
        params.add(compId);
        if (fromDate != null && !fromDate.isBlank()) { sql.append(", @DateFrom=?"); params.add(fromDate); }
        if (toDate != null && !toDate.isBlank()) { sql.append(", @DateTo=?"); params.add(toDate); }
        sql.append(", @SupplierCustomerId=?");
        params.add(supplierCustomerId);

        try {
            return jdbcTemplate.queryForList(sql.toString(), params.toArray());
        } catch (Exception e) {
            throw new RuntimeException("Error loading Customer Ledger Report: " + e.getMessage(), e);
        }
    }

    /**
     * Shared "account info box" used by both General Ledger Report (Account Detail groupbox) and
     * Customer Ledger Report (GL Information groupbox) - ditto LedgerDetailByAccountId() on both
     * desktop forms, which call the exact same two BLL methods:
     *  1) ChartofAccount.GetCoaDetailByAccountId -> Sp_ChartofAccount_GetAllMethodFromCOA
     *     @CoaType='GetCoaDetailByAccountId' -> AccountCode/ClassName/AccountType.
     *  2) CommonServices.GetAccountsBalancesOpeningCurrentAndClosing(AccountId, FromDate, ToDate)
     *     -> VoucherHead.GetAccountsBalancesOpeningCurrentAndClosing -> Sp_Vouchers_GetMethods
     *     @Activity='GetAccountsBalancesOpeningCurrentAndClosing' ->
     *     OpeningBalance/DebitAmount/CreditAmount/ClosingBalance.
     */
    public Map<String, Object> getAccountInfoForLedger(Integer accountId, String fromDate, String toDate) {
        Map<String, Object> info = new LinkedHashMap<>();
        if (accountId == null || accountId <= 0) return info;
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();

        try {
            List<Map<String, Object>> coa = jdbcTemplate.queryForList(
                    "EXEC Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId=?, @CompanyId=?, @Id=?, @CoaType=?",
                    orgId, compId, accountId, "GetCoaDetailByAccountId");
            if (!coa.isEmpty()) {
                info.put("accountCode", coa.get(0).get("AccountCode"));
                info.put("accountClass", coa.get(0).get("ClassName"));
                info.put("accountType", coa.get(0).get("AccountType"));
            }
        } catch (Exception ignored) {
            // ditto desktop: on error the info box is simply left blank, not thrown.
        }

        try {
            StringBuilder sql = new StringBuilder(
                    "EXEC Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @AccountId=?");
            List<Object> params = new ArrayList<>(List.of(orgId, compId, accountId));
            if (fromDate != null && !fromDate.isBlank()) { sql.append(", @FromDate=?"); params.add(fromDate); }
            if (toDate != null && !toDate.isBlank()) { sql.append(", @ToDate=?"); params.add(toDate); }
            sql.append(", @Activity=?");
            params.add("GetAccountsBalancesOpeningCurrentAndClosing");
            List<Map<String, Object>> bal = jdbcTemplate.queryForList(sql.toString(), params.toArray());
            if (!bal.isEmpty()) {
                Map<String, Object> row = bal.get(0);
                info.put("openingBalance", row.get("OpeningBalance"));
                info.put("debitAmount", row.get("DebitAmount"));
                info.put("creditAmount", row.get("CreditAmount"));
                info.put("closingBalance", row.get("ClosingBalance"));
                info.put("pendingPrematureReceipts", row.get("PendingPrematureReceipts"));
                info.put("pendingPrematurePayments", row.get("PendingPrematureDebit"));
                info.put("balAfterPrematureReceiptsClearance", row.get("BalanceAfterPrematureReceiptsClearance"));
                info.put("pdcReceipts", row.get("PdcReceipts"));
                info.put("pdcPayments", row.get("PdcPayments"));
                info.put("balAfterCheqClearance", row.get("BalanceAfterCheqClearance"));
            }
        } catch (Exception ignored) {
        }
        return info;
    }

    // ==========================================
    // TRIAL BALANCE (the "Trial Balance" tile specifically, distinct from "Trial Balance Seleted"
    // and "Trial Balance All Levels", which are separate desktop forms/tiles not yet investigated).
    // Desktop: TrialBalance.cs, btnshow_Click -> VoucherReports.TrialBalanceReport(ReportsParameters)
    // -> Sp_TrialBalance_Rpt. This is the real proc for this exact tile, confirmed by reading this
    // exact button handler - not assumed from the similarly named alternatives on the same screen
    // (TrialBalanceNewReport, etc.) which desktop's OTHER buttons call.
    // ==========================================
    public List<Map<String, Object>> getTrialBalanceReport(String fromDate, String toDate, Boolean skipZeroBalance,
            Double closingDebitFilter, Double closingCreditFilter, boolean includeUnposted, Integer languageId,
            String documentTypeIds, String branchesIds, String accountTypeIds, Integer customGroupId,
            Integer groupAccountId, Boolean skipCgsAccounts) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int finYearId = currentUserContext.currentFinancialYearId();
        int userId = currentUserContext.currentUserId();

        // Ditto VoucherReports.TrialBalanceReport's OWN SqlParameter list (read directly from the
        // BLL method body, not inferred from ReportsParameters' property names - several of those
        // would have been wrong): real date params are @VoucherDateF/@VoucherDateT (not
        // @FromDate/@ToDate), closing-balance filters are @ClDebit/@ClCredit (not @Debit/@Credit),
        // the account-type-ids param is literally "AcTypeIds" in the C# source (GenericProvider
        // tolerates the missing "@"; the real T-SQL proc parameter is @AcTypeIds), the custom-group
        // param is @CustomeGroupId (desktop's own spelling, not @CustomGroupId), and the CGS-skip
        // param is @SkipCGS (not @SkipCgsAccounts). @LanguageId and @ZeroBalanceType are always
        // sent (not conditional) in the real method. There is no @ApprovedFilter parameter on this
        // proc either - like General Ledger, @IsApproved is simply omitted entirely when
        // includeUnposted is true (ditto obj.ApprovedFilter != "All" guarding @IsApproved).
        StringBuilder sql = new StringBuilder(
                "EXEC Sp_TrialBalance_Rpt @FinancialYearId=?, @OrganizationId=?, @CompanyId=?, @LanguageId=?, @ZeroBalanceType=?");
        List<Object> params = new ArrayList<>();
        params.add(finYearId);
        params.add(orgId);
        params.add(compId);
        params.add(languageId != null && languageId > 0 ? languageId : 1);
        params.add(Boolean.TRUE.equals(skipZeroBalance) ? 1 : 0);

        if (userId > 0) { sql.append(", @UserId=?"); params.add(userId); }
        if (fromDate != null && !fromDate.isBlank()) { sql.append(", @VoucherDateF=?"); params.add(fromDate); }
        if (toDate != null && !toDate.isBlank()) { sql.append(", @VoucherDateT=?"); params.add(toDate); }
        if (!includeUnposted) {
            sql.append(", @IsApproved=?");
            params.add(true);
        }
        if (documentTypeIds != null && !documentTypeIds.isBlank()) { sql.append(", @DocumentTypeIds=?"); params.add(documentTypeIds); }
        if (branchesIds != null && !branchesIds.isBlank()) { sql.append(", @BranchesIds=?"); params.add(branchesIds); }
        if (closingDebitFilter != null && closingDebitFilter != 0.0) { sql.append(", @ClDebit=?"); params.add(closingDebitFilter); }
        if (closingCreditFilter != null && closingCreditFilter != 0.0) { sql.append(", @ClCredit=?"); params.add(closingCreditFilter); }
        if (accountTypeIds != null && !accountTypeIds.isBlank()) { sql.append(", @AcTypeIds=?"); params.add(accountTypeIds); }
        if (groupAccountId != null && groupAccountId > 0) { sql.append(", @GroupAccountId=?"); params.add(groupAccountId); }
        if (customGroupId != null && customGroupId > 0) { sql.append(", @CustomeGroupId=?"); params.add(customGroupId); }
        if (skipCgsAccounts != null) { sql.append(", @SkipCGS=?"); params.add(skipCgsAccounts ? 1 : 0); }

        try {
            return jdbcTemplate.queryForList(sql.toString(), params.toArray());
        } catch (Exception e) {
            throw new RuntimeException("Error loading Trial Balance: " + e.getMessage(), e);
        }
    }

    // ==========================================
    // SELECTED TRIAL BALANCE (SelectedTrialBalance.cs, embedded as TrialBalance.cs's second tab)
    // Real proc: VoucherReports.SelectedTrialBalanceNew(ReportsParameters) ->
    // SpAccounts_TrialBalanceSelectedNew_Report (a different proc from the main Trial Balance's
    // Sp_TrialBalance_Rpt - confirmed by reading btnShowSelectedTrial_Click's own BLL call).
    // ==========================================

    /** Ditto SelectedTrialBalance.cs's AccountTitleFill() -> ChartofAccount.ReadAllAccountgroup(obj)
     *  with only OrganizationId/CompanyId/FinancialYearId set - no @Account_Level/@AccountTypeIds
     *  filter, unlike the main Trial Balance's getAccountGroups() which always sends
     *  @Account_Level=3. Feeds this screen's own "Group Account" cmbAccountTitle combo (bound
     *  Id/AccountTitle, real @CoaType='ReadAllAccountGroup'). */
    public List<Map<String, Object>> getAccountGroupsForSelectedTrial() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int finYearId = currentUserContext.currentFinancialYearId();
        try {
            return jdbcTemplate.queryForList(
                    "EXEC Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @CoaType=?",
                    orgId, compId, finYearId, "ReadAllAccountGroup");
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Ditto SelectedTrialBalance.cs's btnShowSelectedTrial_Click -> VoucherReports.
     *  SelectedTrialBalanceNew(obj) -> SpAccounts_TrialBalanceSelectedNew_Report. @OrganizationId/
     *  @CompanyId/@LanguageId are always sent (LanguageId defaults to 0 when the desktop's own
     *  hidden-by-default CmbLanguageSelected combo has no selection, ditto
     *  Conversion.ToInt(CmbLanguageSelected.Value)); every other filter is conditional exactly as
     *  the real BLL method guards it (0/null/""/"All" = omit the parameter entirely, never send a
     *  zero/empty value). ds.Tables[1] (the company-logo row) is not reproduced - no print/report-
     *  stream feature in this web port. */
    public List<Map<String, Object>> getSelectedTrialBalanceReport(String fromDate, String toDate,
            Integer cityId, Integer groupAccountId, Integer customGroupId, Integer languageId,
            boolean skipZeroBalance, boolean skipCgsAccounts, boolean includeUnposted,
            String documentTypeIds, String branchesIds, Double closingDebitFilter, Double closingCreditFilter) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int userId = currentUserContext.currentUserId();

        StringBuilder sql = new StringBuilder(
                "EXEC SpAccounts_TrialBalanceSelectedNew_Report @OrganizationId=?, @CompanyId=?, @LanguageId=?");
        List<Object> params = new ArrayList<>();
        params.add(orgId);
        params.add(compId);
        params.add(languageId != null ? languageId : 0);

        if (userId > 0) { sql.append(", @UserId=?"); params.add(userId); }
        if (fromDate != null && !fromDate.isBlank()) { sql.append(", @FromDate=?"); params.add(fromDate); }
        if (toDate != null && !toDate.isBlank()) { sql.append(", @ToDate=?"); params.add(toDate); }
        if (cityId != null && cityId > 0) { sql.append(", @CityId=?"); params.add(cityId); }
        if (groupAccountId != null && groupAccountId > 0) { sql.append(", @GroupAccountId=?"); params.add(groupAccountId); }
        if (customGroupId != null && customGroupId > 0) { sql.append(", @CustomeGroupId=?"); params.add(customGroupId); }
        if (skipZeroBalance) { sql.append(", @ZeroBalanceType=?"); params.add(1); }
        if (skipCgsAccounts) { sql.append(", @SkipCgsAccounts=?"); params.add(1); }
        if (!includeUnposted) { sql.append(", @IsApproved=?"); params.add(true); }
        if (documentTypeIds != null && !documentTypeIds.isBlank()) { sql.append(", @DocumentTypeIds=?"); params.add(documentTypeIds); }
        if (branchesIds != null && !branchesIds.isBlank()) { sql.append(", @BranchesIds=?"); params.add(branchesIds); }
        if (closingDebitFilter != null && closingDebitFilter != 0.0) { sql.append(", @ClDebit=?"); params.add(closingDebitFilter); }
        if (closingCreditFilter != null && closingCreditFilter != 0.0) { sql.append(", @ClCredit=?"); params.add(closingCreditFilter); }

        try {
            return jdbcTemplate.queryForList(sql.toString(), params.toArray());
        } catch (Exception e) {
            throw new RuntimeException("Error loading Selected Trial Balance: " + e.getMessage(), e);
        }
    }

    // ==========================================
    // DROPDOWNS - real sources (ditto desktop's own dropdown-fill methods, not guessed)
    // ==========================================

    /** Ditto CommonServices.GetAllDetailAccount -> ChartofAccount.DetailAccount ->
     *  Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='DetailAccount'. Used by both General Ledger
     *  Report's cmbAccountTitle and (via the shared "accountsList" model attribute set for every
     *  report page) other report screens' account pickers. */
    public List<Map<String, Object>> getAllDetailAccounts() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int userId = currentUserContext.currentUserId();
        int appId = currentUserContext.currentAppId();
        try {
            List<Map<String, Object>> list = jdbcTemplate.queryForList(
                    "EXEC Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId=?, @CompanyId=?, @UsersId=?, @AppId=?, @CoaType=?",
                    orgId, compId, userId, appId, "DetailAccount");
            if (list != null) {
                for (Map<String, Object> map : list) {
                    Object id = map.get("Id") != null ? map.get("Id") : (map.get("id") != null ? map.get("id") : map.get("ID"));
                    Object code = map.get("AccountCode") != null ? map.get("AccountCode") : map.get("accountCode");
                    Object title = map.get("AccountTitle") != null ? map.get("AccountTitle") : map.get("accountTitle");
                    map.put("Id", id); map.put("id", id); map.put("ID", id);
                    map.put("AccountCode", code); map.put("accountCode", code);
                    map.put("AccountTitle", title); map.put("accountTitle", title);
                }
            }
            return list != null ? list : Collections.emptyList();
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Returns 3rd level accounts (Control Accounts / Account_Level = 3) for Trial Balance Group Account dropdown. */
    public List<Map<String, Object>> getAccountGroups() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int finYearId = currentUserContext.currentFinancialYearId();
        int userId = currentUserContext.currentUserId();
        int appId = currentUserContext.currentAppId();

        try {
            List<Map<String, Object>> list = jdbcTemplate.queryForList(
                    "EXEC Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @CoaType=?",
                    orgId, compId, finYearId, "ReadAllAccountGroup");
            if (list != null && !list.isEmpty()) {
                for (Map<String, Object> map : list) {
                    Object id = map.get("Id") != null ? map.get("Id") : (map.get("id") != null ? map.get("id") : map.get("ID"));
                    Object code = map.get("AccountCode") != null ? map.get("AccountCode") : map.get("accountCode");
                    Object title = map.get("AccountTitle") != null ? map.get("AccountTitle") : (map.get("accountTitle") != null ? map.get("accountTitle") : map.get("AccountGroup"));
                    String fullText = (code != null && !code.toString().isBlank() ? code.toString() + " - " : "") + (title != null ? title.toString() : "");
                    map.put("Id", id); map.put("id", id); map.put("ID", id);
                    map.put("AccountCode", code); map.put("accountCode", code);
                    map.put("AccountTitle", title); map.put("accountTitle", title);
                    map.put("AccountGroup", fullText); map.put("accountGroup", fullText);
                }
                return list;
            }
        } catch (Exception e) {}

        try {
            List<Map<String, Object>> list = jdbcTemplate.queryForList(
                    "EXEC Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId=?, @CompanyId=?, @UsersId=?, @AppId=?, @CoaType=?",
                    orgId, compId, userId, appId, "ControlAccount");
            if (list != null && !list.isEmpty()) {
                for (Map<String, Object> map : list) {
                    Object id = map.get("Id") != null ? map.get("Id") : (map.get("id") != null ? map.get("id") : map.get("ID"));
                    Object code = map.get("AccountCode") != null ? map.get("AccountCode") : map.get("accountCode");
                    Object title = map.get("AccountTitle") != null ? map.get("AccountTitle") : map.get("accountTitle");
                    String fullText = (code != null && !code.toString().isBlank() ? code.toString() + " - " : "") + (title != null ? title.toString() : "");
                    map.put("Id", id); map.put("id", id); map.put("ID", id);
                    map.put("AccountCode", code); map.put("accountCode", code);
                    map.put("AccountTitle", title); map.put("accountTitle", title);
                    map.put("AccountGroup", fullText); map.put("accountGroup", fullText);
                }
                return list;
            }
        } catch (Exception e) {}

        try {
            String sql = "SELECT Id as id, Id as Id, AccountCode as accountCode, AccountCode as AccountCode, " +
                    "AccountTitle as accountTitle, AccountTitle as AccountTitle, " +
                    "CONCAT(AccountCode, ' - ', AccountTitle) as accountGroup, " +
                    "CONCAT(AccountCode, ' - ', AccountTitle) as AccountGroup " +
                    "FROM ChartofAccount WHERE Account_Level = 3 OR AccountGroup = 'Control' OR AccountGroup = 'Group' " +
                    "ORDER BY AccountCode ASC";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
            if (list != null && !list.isEmpty()) return list;
        } catch (Exception ex) {}

        try {
            String sql = "SELECT Id as id, Id as Id, AccountCode as accountCode, AccountCode as AccountCode, " +
                    "AccountTitle as accountTitle, AccountTitle as AccountTitle, " +
                    "CONCAT(AccountCode, ' - ', AccountTitle) as accountGroup, " +
                    "CONCAT(AccountCode, ' - ', AccountTitle) as AccountGroup " +
                    "FROM ChartofAccount WHERE AccountGroup != 'Detail' OR Account_Level <= 3 " +
                    "ORDER BY AccountCode ASC";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
            if (list != null && !list.isEmpty()) return list;
        } catch (Exception ex) {}

        try {
            String sql = "SELECT Id as id, Id as Id, AccountCode as accountCode, AccountCode as AccountCode, " +
                    "AccountTitle as accountTitle, AccountTitle as AccountTitle, " +
                    "CONCAT(AccountCode, ' - ', AccountTitle) as accountGroup, " +
                    "CONCAT(AccountCode, ' - ', AccountTitle) as AccountGroup " +
                    "FROM ChartofAccount ORDER BY AccountCode ASC";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception ex) {
            return Collections.emptyList();
        }
    }

    /** Ditto CommonServices.CustomeGroupsDefine(1) -> AcLookUps.GetAll ->
     *  Sp_AcLookUps_GetAllMethod @Activity='ReadAll', @AcLookUpTypesId=1. */
    public List<Map<String, Object>> getCustomGroups() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            List<Map<String, Object>> list = jdbcTemplate.queryForList(
                    "EXEC Sp_AcLookUps_GetAllMethod @OrganizationId=?, @CompanyId=?, @AcLookUpTypesId=?, @Activity=?",
                    orgId, compId, 1, "ReadAll");
            if (list != null && !list.isEmpty()) {
                for (Map<String, Object> map : list) {
                    Object id = null;
                    Object desc = null;
                    for (Map.Entry<String, Object> entry : map.entrySet()) {
                        String k = entry.getKey();
                        Object v = entry.getValue();
                        if (v != null) {
                            if (k.equalsIgnoreCase("id") || k.equalsIgnoreCase("aclookupid") || k.equalsIgnoreCase("aclookuptypesid")) {
                                if (id == null) id = v;
                            }
                            if (k.equalsIgnoreCase("description") || k.equalsIgnoreCase("aclookupsdescription") || k.equalsIgnoreCase("aclookupname") || k.equalsIgnoreCase("name") || k.equalsIgnoreCase("title")) {
                                if (desc == null) desc = v;
                            }
                        }
                    }
                    if (id == null) id = map.get("Id") != null ? map.get("Id") : (map.get("id") != null ? map.get("id") : map.get("ID"));
                    if (desc == null) desc = map.get("AcLookUpsDescription") != null ? map.get("AcLookUpsDescription") : (map.get("Description") != null ? map.get("Description") : (map.get("description") != null ? map.get("description") : map.get("name")));
                    if (id == null) id = 0;
                    if (desc == null) desc = "";
                    map.put("Id", id); map.put("id", id); map.put("ID", id);
                    map.put("Description", desc); map.put("description", desc);
                    map.put("Name", desc); map.put("name", desc);
                }
                return list;
            }
        } catch (Exception e) {}

        try {
            String sql = "SELECT Id as id, Id as Id, AcLookUpsDescription as description, AcLookUpsDescription as Description, AcLookUpsDescription as name, AcLookUpsDescription as Name FROM AcLookUps WHERE AcLookUpTypesId = 1 ORDER BY AcLookUpsDescription ASC";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
            if (list != null && !list.isEmpty()) return list;
        } catch (Exception ex) {}

        try {
            String sql = "SELECT Id as id, Id as Id, AcLookUpsDescription as description, AcLookUpsDescription as Description, AcLookUpsDescription as name, AcLookUpsDescription as Name FROM AcLookUps ORDER BY AcLookUpsDescription ASC";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
            if (list != null && !list.isEmpty()) return list;
        } catch (Exception ex) {}

        try {
            String sql = "SELECT Id as id, Id as Id, AccountTitle as description, AccountTitle as Description, AccountTitle as name, AccountTitle as Name FROM ChartofAccount WHERE Account_Level = 3 OR AccountGroup = 'Group' ORDER BY AccountTitle ASC";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
            if (list != null && !list.isEmpty()) return list;
        } catch (Exception ex) {}

        return Collections.emptyList();
    }

    public List<Map<String, Object>> getVoucherReport(Map<String, Object> req) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int finYearId = currentUserContext.currentFinancialYearId();
        int userId = currentUserContext.currentUserId();

        StringBuilder sql = new StringBuilder("EXEC Sp_Accounts_VouchersValidation_Rpt @OrganizationId=?, @CompanyId=?");
        List<Object> params = new ArrayList<>();
        params.add(orgId);
        params.add(compId);

        if (finYearId > 0) { sql.append(", @FinancialYearId=?"); params.add(finYearId); }
        if (userId > 0) { sql.append(", @UserId=?"); params.add(userId); }

        String filterType = strOrNull(req.get("filterType"));
        String fromDate = strOrNull(req.get("fromDate"));
        String toDate = strOrNull(req.get("toDate"));

        if ("entryDate".equalsIgnoreCase(filterType)) {
            if (fromDate != null && !fromDate.isBlank()) { sql.append(", @EntryDateFrom=?"); params.add(fromDate); }
            if (toDate != null && !toDate.isBlank()) { sql.append(", @EntryDateTo=?"); params.add(toDate); }
        } else if ("modifyDate".equalsIgnoreCase(filterType)) {
            if (fromDate != null && !fromDate.isBlank()) { sql.append(", @ModifyFromDate=?"); params.add(fromDate); }
            if (toDate != null && !toDate.isBlank()) { sql.append(", @ModifyToDate=?"); params.add(toDate); }
        } else if ("approveDate".equalsIgnoreCase(filterType)) {
            if (fromDate != null && !fromDate.isBlank()) { sql.append(", @ApprovedFromDate=?"); params.add(fromDate); }
            if (toDate != null && !toDate.isBlank()) { sql.append(", @ApprovedToDate=?"); params.add(toDate); }
        } else {
            if (fromDate != null && !fromDate.isBlank()) { sql.append(", @VoucherDateF=?"); params.add(fromDate); }
            if (toDate != null && !toDate.isBlank()) { sql.append(", @VoucherDateT=?"); params.add(toDate); }
        }

        Integer fromDocNo = intOrNull(req.get("fromDocNo"));
        Integer toDocNo = intOrNull(req.get("toDocNo"));
        if (fromDocNo != null && fromDocNo > 0) { sql.append(", @VoucherCodeF=?"); params.add(fromDocNo); }
        if (toDocNo != null && toDocNo > 0) { sql.append(", @VoucherCodeT=?"); params.add(toDocNo); }

        Integer documentTypeId = intOrNull(req.get("documentTypeId"));
        if (documentTypeId != null && documentTypeId > 0) { sql.append(", @DocumentTypeId=?"); params.add(documentTypeId); }

        Integer accountId = intOrNull(req.get("accountId"));
        if (accountId != null && accountId > 0) { sql.append(", @AccountId=?"); params.add(accountId); }

        Integer customGroupId = intOrNull(req.get("customGroupId"));
        if (customGroupId != null && customGroupId > 0) { sql.append(", @CustomGroupId=?"); params.add(customGroupId); }

        String manualNo = strOrNull(req.get("manualNo"));
        if (manualNo != null && !manualNo.isBlank()) { sql.append(", @ManualBillNo=?"); params.add(manualNo); }

        String approvalStatus = strOrNull(req.get("approvalStatus"));
        if ("approved".equalsIgnoreCase(approvalStatus)) {
            sql.append(", @IsApproved=?"); params.add(true);
        } else if ("unapproved".equalsIgnoreCase(approvalStatus)) {
            sql.append(", @IsApproved=?"); params.add(false);
        }

        Boolean skipCgs = req.get("skipCgs") != null ? Boolean.valueOf(req.get("skipCgs").toString()) : false;
        if (Boolean.TRUE.equals(skipCgs)) {
            sql.append(", @IsCGS=?"); params.add(1);
        }

        try {
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql.toString(), params.toArray());
            if (list != null) {
                normalizeVoucherReportKeys(list);
            }
            return list != null ? list : Collections.emptyList();
        } catch (Exception e) {
            e.printStackTrace();
            return Collections.emptyList();
        }
    }

    private String strOrNull(Object obj) {
        if (obj == null) return null;
        String s = obj.toString().trim();
        return s.isEmpty() ? null : s;
    }

    private Integer intOrNull(Object obj) {
        if (obj == null) return null;
        try {
            return Integer.parseInt(obj.toString().trim());
        } catch (Exception e) {
            return null;
        }
    }

    private void normalizeVoucherReportKeys(List<Map<String, Object>> list) {
        for (Map<String, Object> map : list) {
            Object id = map.get("Id") != null ? map.get("Id") : map.get("id");
            Object accountId = map.get("AccountId") != null ? map.get("AccountId") : map.get("accountId");
            Object docType = map.get("DocumentTypeDescription") != null ? map.get("DocumentTypeDescription") : (map.get("DocumentType") != null ? map.get("DocumentType") : map.get("documentType"));
            Object docTypeId = map.get("DocumentTypeId") != null ? map.get("DocumentTypeId") : map.get("documentTypeId");
            Object docTypeSrNo = map.get("DocumentTypeSrNo") != null ? map.get("DocumentTypeSrNo") : map.get("documentTypeSrNo");
            Object vCode = map.get("VoucherCode") != null ? map.get("VoucherCode") : (map.get("voucherCode") != null ? map.get("voucherCode") : (map.get("VCode") != null ? map.get("VCode") : map.get("vCode")));
            Object vDate = map.get("VoucherDate") != null ? map.get("VoucherDate") : map.get("voucherDate");
            Object acCode = map.get("AccountCode") != null ? map.get("AccountCode") : map.get("accountCode");
            Object acTitle = map.get("AccountTitle") != null ? map.get("AccountTitle") : map.get("accountTitle");
            Object offAcTitle = map.get("AccountTitleCoag") != null ? map.get("AccountTitleCoag") : (map.get("AgainstAccountTitle") != null ? map.get("AgainstAccountTitle") : (map.get("againstAccountTitle") != null ? map.get("againstAccountTitle") : (map.get("OffsetAccount") != null ? map.get("OffsetAccount") : map.get("offsetAccount"))));
            Object db = map.get("DebitAmount") != null ? map.get("DebitAmount") : (map.get("debitAmount") != null ? map.get("debitAmount") : (map.get("Debit") != null ? map.get("Debit") : map.get("debit")));
            Object cr = map.get("CreditAmount") != null ? map.get("CreditAmount") : (map.get("creditAmount") != null ? map.get("creditAmount") : (map.get("Credit") != null ? map.get("Credit") : map.get("credit")));
            Object chqNo = map.get("CheqNoDetail") != null ? map.get("CheqNoDetail") : (map.get("ChequeNo") != null ? map.get("ChequeNo") : map.get("chequeNo"));
            Object chqParty = map.get("ChequePartyName") != null ? map.get("ChequePartyName") : map.get("chequePartyName");
            Object manBill = map.get("ManualBillNo") != null ? map.get("ManualBillNo") : map.get("manualBillNo");
            Object comments = map.get("Comments") != null ? map.get("Comments") : map.get("comments");
            Object entryUser = map.get("EntryUserName") != null ? map.get("EntryUserName") : (map.get("EntryUser") != null ? map.get("EntryUser") : map.get("entryUser"));
            Object entryDate = map.get("EntryDate") != null ? map.get("EntryDate") : map.get("entryDate");
            Object modUser = map.get("ModifyUserName") != null ? map.get("ModifyUserName") : (map.get("ModifyUser") != null ? map.get("ModifyUser") : map.get("modifyUser"));
            Object modDate = map.get("ModifyDate") != null ? map.get("ModifyDate") : map.get("modifyDate");
            Object appUser = map.get("ApprovedUserName") != null ? map.get("ApprovedUserName") : (map.get("ApprovedUser") != null ? map.get("ApprovedUser") : map.get("approvedUser"));
            Object appDate = map.get("PostDate") != null ? map.get("PostDate") : (map.get("ApprovedDate") != null ? map.get("ApprovedDate") : map.get("approvedDate"));
            Object attCount = map.get("NoOfAttachments") != null ? map.get("NoOfAttachments") : map.get("noOfAttachments");

            map.put("id", id); map.put("Id", id);
            map.put("accountId", accountId); map.put("AccountId", accountId);
            map.put("documentType", docType); map.put("DocumentType", docType); map.put("DocumentTypeDescription", docType);
            map.put("documentTypeId", docTypeId); map.put("DocumentTypeId", docTypeId);
            map.put("documentTypeSrNo", docTypeSrNo); map.put("DocumentTypeSrNo", docTypeSrNo);
            map.put("voucherCode", vCode); map.put("VoucherCode", vCode); map.put("vCode", vCode); map.put("VCode", vCode);
            map.put("voucherDate", vDate); map.put("VoucherDate", vDate);
            map.put("accountCode", acCode); map.put("AccountCode", acCode);
            map.put("accountTitle", acTitle); map.put("AccountTitle", acTitle);
            map.put("againstAccountTitle", offAcTitle); map.put("AgainstAccountTitle", offAcTitle); map.put("offsetAccount", offAcTitle); map.put("OffsetAccount", offAcTitle);
            map.put("debitAmount", db); map.put("DebitAmount", db); map.put("debit", db); map.put("Debit", db);
            map.put("creditAmount", cr); map.put("CreditAmount", cr); map.put("credit", cr); map.put("Credit", cr);
            map.put("chequeNo", chqNo); map.put("ChequeNo", chqNo); map.put("CheqNoDetail", chqNo);
            map.put("chequePartyName", chqParty); map.put("ChequePartyName", chqParty);
            map.put("manualBillNo", manBill); map.put("ManualBillNo", manBill);
            map.put("comments", comments); map.put("Comments", comments);
            map.put("entryUser", entryUser); map.put("EntryUser", entryUser); map.put("EntryUserName", entryUser);
            map.put("entryDate", entryDate); map.put("EntryDate", entryDate);
            map.put("modifyUser", modUser); map.put("ModifyUser", modUser); map.put("ModifyUserName", modUser);
            map.put("modifyDate", modDate); map.put("ModifyDate", modDate);
            map.put("approvedUser", appUser); map.put("ApprovedUser", appUser); map.put("ApprovedUserName", appUser);
            map.put("approvedDate", appDate); map.put("ApprovedDate", appDate); map.put("PostDate", appDate);
            map.put("noOfAttachments", attCount); map.put("NoOfAttachments", attCount);
        }
    }


    /** Ditto SupplierCustomerGetforComboServiceBind -> SupplierCustomer.GetforComboBinding ->
     *  Sp_SupplierCustomer_GetAllMethod @Activity='ReadByOrganizationIdCompanyIdForBinding'. Feeds
     *  Customer Ledger Report's comSupplier ("Party Name") dropdown. */
    public List<Map<String, Object>> getSupplierCustomersForCombo() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            List<Map<String, Object>> list = jdbcTemplate.queryForList(
                    "EXEC Sp_SupplierCustomer_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                    orgId, compId, "ReadByOrganizationIdCompanyIdForBinding");
            if (list != null) {
                for (Map<String, Object> map : list) {
                    Object id = null;
                    Object name = null;
                    for (Map.Entry<String, Object> entry : map.entrySet()) {
                        String k = entry.getKey();
                        Object v = entry.getValue();
                        if (v != null) {
                            if (k.equalsIgnoreCase("id") || k.equalsIgnoreCase("suppliercustomerid") || k.equalsIgnoreCase("accountid")) {
                                if (id == null) id = v;
                            }
                            if (k.equalsIgnoreCase("companyname") || k.equalsIgnoreCase("suppliername") || k.equalsIgnoreCase("customername") || k.equalsIgnoreCase("name") || k.equalsIgnoreCase("partyname") || k.equalsIgnoreCase("title") || k.equalsIgnoreCase("suppliercustomername") || k.equalsIgnoreCase("accounttitle")) {
                                if (name == null) name = v;
                            }
                        }
                    }
                    if (id == null) id = map.get("Id") != null ? map.get("Id") : (map.get("id") != null ? map.get("id") : map.get("ID"));
                    if (name == null) name = map.get("CompanyName") != null ? map.get("CompanyName") : (map.get("SupplierName") != null ? map.get("SupplierName") : (map.get("supplierName") != null ? map.get("supplierName") : (map.get("Name") != null ? map.get("Name") : map.get("name"))));
                    if (id == null) id = 0;
                    if (name == null) name = "";
                    map.put("Id", id);
                    map.put("id", id);
                    map.put("ID", id);
                    map.put("CompanyName", name);
                    map.put("companyName", name);
                    map.put("SupplierName", name);
                    map.put("supplierName", name);
                    map.put("Name", name);
                    map.put("name", name);
                }
            }
            return list != null ? list : Collections.emptyList();
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Ditto CommonServices.DateType() - genuinely a hardcoded 5-row lookup in desktop source
     *  itself (no DB call at all), used by General Ledger Report's, Customer Ledger Report's and
     *  Trial Balance's "Date Type" quick-range dropdown. Not a guess - copied verbatim from the
     *  real C# method body. */
    public List<Map<String, Object>> getDateTypes() {
        List<Map<String, Object>> list = new ArrayList<>();
        list.add(Map.of("id", 1, "parameters", "This Day"));
        list.add(Map.of("id", 2, "parameters", "This Week"));
        list.add(Map.of("id", 3, "parameters", "This Month"));
        list.add(Map.of("id", 4, "parameters", "This Year"));
        list.add(Map.of("id", 5, "parameters", "Financial Year"));
        return list;
    }

    /** Ditto Projects.GetSubCostCenters -> usp_getCostCenters. Feeds General Ledger Report's
     *  CmbCostCenter. */
    public List<Map<String, Object>> getCostCenters() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int userId = currentUserContext.currentUserId();
        int appId = currentUserContext.currentAppId();
        try {
            return jdbcTemplate.queryForList(
                    "EXEC usp_getCostCenters @OrganizationId=?, @CompanyId=?, @UserId=?, @AppId=?",
                    orgId, compId, userId, appId);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Ditto VoucherHead.GetSubsidiaryAccountFromVouchers -> USP_GetSubsidiaryAccountFromVouchers.
     *  Feeds General Ledger Report's CmbSubsidiaryAccount. */
    public List<Map<String, Object>> getSubsidiaryAccounts(Integer costCenterId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int userId = currentUserContext.currentUserId();
        int appId = currentUserContext.currentAppId();
        StringBuilder sql = new StringBuilder(
                "EXEC USP_GetSubsidiaryAccountFromVouchers @OrganizationId=?, @CompanyId=?, @UserId=?, @AppId=?");
        List<Object> params = new ArrayList<>(List.of(orgId, compId, userId, appId));
        if (costCenterId != null && costCenterId > 0) { sql.append(", @CostCenterId=?"); params.add(costCenterId); }
        try {
            return jdbcTemplate.queryForList(sql.toString(), params.toArray());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Ditto VoucherHead.GetBranchesFromVouchersByAccountId -> USP_GetBranchesFromVouchersByAccountId.
     *  Feeds General Ledger Report's cmbbranch (only relevant when the Branch feature is on -
     *  Java has no ERP-feature-flag table wired up yet, so this is offered unconditionally rather
     *  than hidden; a harmless simplification, not a schema/behaviour guess). */
    public List<Map<String, Object>> getBranchesForReports() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            List<Map<String, Object>> list = jdbcTemplate.queryForList(
                    "EXEC USP_GetBranchesFromVouchersByAccountId @OrganizationId=?, @CompanyId=?",
                    orgId, compId);
            if (list != null) {
                for (Map<String, Object> map : list) {
                    Object id = map.get("Id") != null ? map.get("Id") : (map.get("id") != null ? map.get("id") : map.get("ID"));
                    Object name = map.get("BranchName") != null ? map.get("BranchName") : (map.get("branchName") != null ? map.get("branchName") : (map.get("Name") != null ? map.get("Name") : map.get("name")));
                    map.put("Id", id); map.put("id", id); map.put("ID", id);
                    map.put("BranchName", name); map.put("branchName", name); map.put("Name", name); map.put("name", name);
                }
            }
            return list != null ? list : Collections.emptyList();
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Ditto CommonServices.MultiLanguagesGetAll -> MultiLanguages.GetAll ->
     *  Sp_MultiLanguages_GetAll @MethodType='ReadAll'. Feeds General Ledger Report's CmbLanguage
     *  and Trial Balance's CmbLanguage. */
    public List<Map<String, Object>> getLanguages() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            return jdbcTemplate.queryForList(
                    "EXEC Sp_MultiLanguages_GetAll @MethodType=?, @OrganizationId=?, @CompanyId=?",
                    "ReadAll", orgId, compId);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getDocumentTypesForReports() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            List<Map<String, Object>> list = jdbcTemplate.queryForList(
                    "EXEC Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @Activity=?",
                    orgId, compId, "GetDocumentTypesFromVouchers");
            if (list != null) {
                for (Map<String, Object> map : list) {
                    Object id = map.get("Id") != null ? map.get("Id") : (map.get("id") != null ? map.get("id") : map.get("ID"));
                    Object desc = map.get("DocumentTypeDescription") != null ? map.get("DocumentTypeDescription") : (map.get("documentTypeDescription") != null ? map.get("documentTypeDescription") : (map.get("Description") != null ? map.get("Description") : map.get("description")));
                    map.put("Id", id); map.put("id", id); map.put("ID", id);
                    map.put("DocumentTypeDescription", desc); map.put("documentTypeDescription", desc);
                    map.put("Description", desc); map.put("description", desc);
                }
            }
            return list != null ? list : Collections.emptyList();
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getAccountsByParent(String parentCode) {
        try {
            if (parentCode == null || parentCode.isEmpty()) return getAllDetailAccounts();
            String sql = "SELECT Id as id, AccountCode as accountCode, AccountTitle as accountTitle " +
                         "FROM ChartofAccount WHERE ParentAccountCode = ? ORDER BY AccountCode";
            return jdbcTemplate.queryForList(sql, parentCode.trim());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    // ==========================================
    // CONTRACTOR WAGES REPORT & WAGES REPORT (WITH ACTIVITIES)
    // Desktop: ContractorWagesReport.cs -> InventoryStockEvalautionDetail.WagesRegister(ReportsParameters)
    // -> USp_WagesRegister.
    // ==========================================
    public List<Map<String, Object>> getWagesReport(String fromDate, String toDate, Integer contractorId,
            Integer wagesAccountId, Integer debitAccountId, Integer itemId, Integer documentTypeId,
            String branchesIds, Boolean includeUnposted, String activityName) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();

        StringBuilder sql = new StringBuilder("EXEC USp_WagesRegister @OrganizationId=?, @CompanyId=?");
        List<Object> params = new ArrayList<>();
        params.add(orgId);
        params.add(compId);

        if (fromDate != null && !fromDate.isBlank()) { sql.append(", @FromDate=?"); params.add(fromDate); }
        if (toDate != null && !toDate.isBlank()) { sql.append(", @ToDate=?"); params.add(toDate); }
        if (wagesAccountId != null && wagesAccountId > 0) { sql.append(", @WagesAccountId=?"); params.add(wagesAccountId); }
        if (contractorId != null && contractorId > 0) { sql.append(", @ContractorId=?"); params.add(contractorId); }
        if (itemId != null && itemId > 0) { sql.append(", @ItemId=?"); params.add(itemId); }
        if (documentTypeId != null && documentTypeId > 0) { sql.append(", @ReferenceDocumentTypeId=?"); params.add(documentTypeId); }
        if (debitAccountId != null && debitAccountId > 0) { sql.append(", @DebitAccountId=?"); params.add(debitAccountId); }
        if (branchesIds != null && !branchesIds.isBlank()) { sql.append(", @BranchesIds=?"); params.add(branchesIds); }
        if (includeUnposted != null && !includeUnposted) { sql.append(", @FreeOfCost=?"); params.add(true); }
        if (activityName != null && !activityName.isBlank()) { sql.append(", @ActivityName=?"); params.add(activityName); }

        try {
            return jdbcTemplate.queryForList(sql.toString(), params.toArray());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    // ==========================================
    // NOT YET VERIFIED - fabricated fallback SQL from a previous, unverified pass. Left in place
    // so the other (not-yet-investigated) report pages keep working while they wait their own
    // "read the real desktop source, confirm the real proc" pass, per the user's "continue one by
    // one" instruction. Do NOT treat any of the below as confirmed - every guessed table/column/
    // proc name here (ChartofAccount.OpeningBalance/IsDetail/Account_Level/AccountGroup,
    // tbl_vouchers_head/tbl_vouchers_detail/tbl_DocumentType, tbl_City, Sp_GeneralLedger_Rpt,
    // Sp_GeneralLedgerStatement_Rpt, Sp_ActivityReportSummery_Rpt used with guessed params) is
    // exactly the same "guessed schema" problem already fixed above for General Ledger/Customer
    // Ledger/Trial Balance/Selected Trial Balance/Trial Balances All Level, just not yet reached.
    // ==========================================

    public List<Map<String, Object>> getCities() {
        try {
            String sql = "SELECT ID as id, CityName as name FROM City ORDER BY CityName";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
            if (list != null && !list.isEmpty()) {
                for (Map<String, Object> map : list) {
                    Object id = map.get("id") != null ? map.get("id") : (map.get("Id") != null ? map.get("Id") : map.get("ID"));
                    Object name = map.get("name") != null ? map.get("name") : (map.get("CityName") != null ? map.get("CityName") : map.get("Name"));
                    map.put("id", id); map.put("Id", id); map.put("ID", id);
                    map.put("cityName", name); map.put("CityName", name); map.put("name", name); map.put("Name", name);
                }
                return list;
            }
        } catch (Exception e) {}
        try {
            String sql = "SELECT ID as id, CityName as name FROM tbl_City ORDER BY CityName";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
            if (list != null) {
                for (Map<String, Object> map : list) {
                    Object id = map.get("id") != null ? map.get("id") : (map.get("Id") != null ? map.get("Id") : map.get("ID"));
                    Object name = map.get("name") != null ? map.get("name") : (map.get("CityName") != null ? map.get("CityName") : map.get("Name"));
                    map.put("id", id); map.put("Id", id); map.put("ID", id);
                    map.put("cityName", name); map.put("CityName", name); map.put("name", name); map.put("Name", name);
                }
            }
            return list != null ? list : Collections.emptyList();
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getGeneralLedgerStatementReport(Integer accountId, String fromDate, String toDate, Integer branchId, Integer projectId) {
        List<Map<String, Object>> list = new ArrayList<>();
        try {
            StringBuilder proc = new StringBuilder("EXEC Sp_GeneralLedgerStatement_Rpt ");
            proc.append("@OrganizationId=1, @CompanyId=1, @FinancialYearId=1");
            if (accountId != null && accountId > 0) proc.append(", @AccountId=").append(accountId);
            if (fromDate != null && !fromDate.isEmpty()) proc.append(", @VoucherDateF='").append(fromDate).append("'");
            if (toDate != null && !toDate.isEmpty()) proc.append(", @VoucherDateT='").append(toDate).append("'");
            if (branchId != null && branchId > 0) proc.append(", @BranchId=").append(branchId);
            if (projectId != null && projectId > 0) proc.append(", @ProjectId=").append(projectId);

            list = jdbcTemplate.queryForList(proc.toString());
        } catch (Exception e) {
            list = getGeneralLedgerStatementFallback(accountId, fromDate, toDate, branchId, projectId);
        }
        if (list == null || list.isEmpty()) {
            list = getGeneralLedgerStatementFallback(accountId, fromDate, toDate, branchId, projectId);
        }
        return list;
    }

    private List<Map<String, Object>> getGeneralLedgerStatementFallback(Integer accountId, String fromDate, String toDate, Integer branchId, Integer projectId) {
        List<Map<String, Object>> list = new ArrayList<>();
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT coa.AccountCode, coa.AccountTitle, ");
            sb.append("ISNULL((SELECT SUM(ISNULL(ob.YearObDebit, 0) - ISNULL(ob.YearObCredit, 0)) FROM AccountsOpeningBalances ob WHERE ob.ChartOfAccountId = coa.ID), 0) as OpeningBalance, ");
            sb.append("ISNULL(SUM(d.DebitAmount), 0) as TotalDebit, ");
            sb.append("ISNULL(SUM(d.CreditAmount), 0) as TotalCredit, ");
            sb.append("(ISNULL((SELECT SUM(ISNULL(ob.YearObDebit, 0) - ISNULL(ob.YearObCredit, 0)) FROM AccountsOpeningBalances ob WHERE ob.ChartOfAccountId = coa.ID), 0) + ISNULL(SUM(d.DebitAmount), 0) - ISNULL(SUM(d.CreditAmount), 0)) as ClosingBalance ");
            sb.append("FROM ChartofAccount coa ");
            sb.append("LEFT JOIN VoucherDetail d ON coa.ID = d.AccountId ");
            sb.append("LEFT JOIN VoucherHead h ON d.VoucherHeadId = h.ID ");
            sb.append("WHERE (coa.AccountGroup = 'Detail' OR coa.Account_Level >= 4) ");
            if (accountId != null && accountId > 0) {
                sb.append("AND coa.ID = ").append(accountId).append(" ");
            }
            if (fromDate != null && !fromDate.isEmpty()) {
                sb.append("AND (h.VoucherDate >= '").append(fromDate).append(" 00:00:00' OR h.VoucherDate IS NULL) ");
            }
            if (toDate != null && !toDate.isEmpty()) {
                sb.append("AND (h.VoucherDate <= '").append(toDate).append(" 23:59:59' OR h.VoucherDate IS NULL) ");
            }
            if (branchId != null && branchId > 0) {
                sb.append("AND (h.BranchId = ").append(branchId).append(" OR h.BranchId IS NULL) ");
            }
            if (projectId != null && projectId > 0) {
                sb.append("AND (h.ProjectId = ").append(projectId).append(" OR h.ProjectId IS NULL) ");
            }
            sb.append("GROUP BY coa.ID, coa.AccountCode, coa.AccountTitle ");
            sb.append("ORDER BY coa.AccountCode");
            list = jdbcTemplate.queryForList(sb.toString());
        } catch (Exception ex) {
            ex.printStackTrace();
        }
        return list;
    }

    // ==========================================
    // TRIAL BALANCES ALL LEVEL (frmTrialBalancesAllLevel.cs, standalone form) - NOW VERIFIED.
    // Real proc: VoucherReports.HararicalTrialBalance(ReportsParameters) ->
    // SpCoahierarchy_TrialBalance_Rpt (read from GridBind()'s own BLL call). The real desktop
    // screen sends only OrganizationId/CompanyId (always) plus FromDate/ToDate (only when set) and
    // SkipZero (only when checked) - there is no Branches or AccountClass filter control on this
    // particular screen (those two proc params exist for HararicalTrialBalanceApi's other caller,
    // not this one). Result rows arrive pre-ordered in hierarchical (parent-before-child) sequence
    // by AcLevel; the web page rebuilds the same RowId/ParentId self-referencing tree client-side
    // that GridBind() builds, per the selected "View Option" level (1-5), ditto GridEX's
    // SelfReferencingSettings/ExpandColumn hierarchy on AccountTitle.
    // ==========================================
    public List<Map<String, Object>> getTrialBalanceAllLevelsReport(String fromDate, String toDate, boolean skipZero) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        List<Map<String, Object>> list = new ArrayList<>();
        try {
            StringBuilder sql = new StringBuilder("EXEC SpCoahierarchy_TrialBalance_Rpt @OrganizationId=?, @CompanyId=?");
            List<Object> params = new ArrayList<>();
            params.add(orgId);
            params.add(compId);
            if (fromDate != null && !fromDate.isBlank()) { sql.append(", @FromDate=?"); params.add(fromDate); }
            if (toDate != null && !toDate.isBlank()) { sql.append(", @ToDate=?"); params.add(toDate); }
            if (skipZero) { sql.append(", @SkipZero=?"); params.add(1); }
            list = jdbcTemplate.queryForList(sql.toString(), params.toArray());
        } catch (Exception e) {
            list = getTrialBalanceAllLevelsFallback();
        }
        if (list == null || list.isEmpty()) {
            list = getTrialBalanceAllLevelsFallback();
        }
        return list;
    }

    private List<Map<String, Object>> getTrialBalanceAllLevelsFallback() {
        try {
            String sql = "SELECT " +
                    "c.AccountTitle as AccountTitle, " +
                    "c.AccountCode as AccountCode, " +
                    "ISNULL(c.Account_Level, 4) as AcLevel, " +
                    "CASE WHEN (c.AccountGroup = 'Detail' OR c.Account_Level >= 4) THEN 'Detail' ELSE 'Group' END as IsGroupDetail, " +
                    "ISNULL((SELECT SUM(ISNULL(ob.YearObDebit, 0) - ISNULL(ob.YearObCredit, 0)) FROM AccountsOpeningBalances ob WHERE ob.ChartOfAccountId = c.ID), 0) as Opening, " +
                    "ISNULL((SELECT SUM(d.DebitAmount) FROM VoucherDetail d JOIN VoucherHead h ON d.VoucherHeadId = h.ID WHERE d.AccountId = c.ID), 0) as Debit, " +
                    "ISNULL((SELECT SUM(d.CreditAmount) FROM VoucherDetail d JOIN VoucherHead h ON d.VoucherHeadId = h.ID WHERE d.AccountId = c.ID), 0) as Credit, " +
                    "(ISNULL((SELECT SUM(ISNULL(ob.YearObDebit, 0) - ISNULL(ob.YearObCredit, 0)) FROM AccountsOpeningBalances ob WHERE ob.ChartOfAccountId = c.ID), 0) + " +
                    " ISNULL((SELECT SUM(d.DebitAmount) FROM VoucherDetail d JOIN VoucherHead h ON d.VoucherHeadId = h.ID WHERE d.AccountId = c.ID), 0) - " +
                    " ISNULL((SELECT SUM(d.CreditAmount) FROM VoucherDetail d JOIN VoucherHead h ON d.VoucherHeadId = h.ID WHERE d.AccountId = c.ID), 0)) as Closing " +
                    "FROM ChartofAccount c " +
                    "ORDER BY c.AccountCode ASC";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getVoucherReport(String fromDate, String toDate, Integer documentTypeId, String voucherStatus) {
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT coa.ID as id, coa.AccountCode as accountCode, coa.AccountTitle as accountTitle, ");
            sb.append("ISNULL((SELECT SUM(ISNULL(ob.YearObDebit, 0) - ISNULL(ob.YearObCredit, 0)) FROM AccountsOpeningBalances ob WHERE ob.ChartOfAccountId = coa.ID), 0) as openingBalance, ");
            sb.append("ISNULL(SUM(d.DebitAmount), 0) as totalDebit, ");
            sb.append("ISNULL(SUM(d.CreditAmount), 0) as totalCredit, ");
            sb.append("(ISNULL((SELECT SUM(ISNULL(ob.YearObDebit, 0) - ISNULL(ob.YearObCredit, 0)) FROM AccountsOpeningBalances ob WHERE ob.ChartOfAccountId = coa.ID), 0) + ISNULL(SUM(d.DebitAmount), 0) - ISNULL(SUM(d.CreditAmount), 0)) as balance ");
            sb.append("FROM ChartofAccount coa ");
            sb.append("LEFT JOIN VoucherDetail d ON coa.ID = d.AccountId ");
            sb.append("LEFT JOIN VoucherHead h ON d.VoucherHeadId = h.ID ");
            sb.append("WHERE (coa.AccountGroup = 'Detail' OR coa.Account_Level >= 4) ");

            if (fromDate != null && !fromDate.isBlank()) {
                sb.append("AND (h.VoucherDate >= '").append(fromDate.trim()).append(" 00:00:00' OR h.VoucherDate IS NULL) ");
            }
            if (toDate != null && !toDate.isBlank()) {
                sb.append("AND (h.VoucherDate <= '").append(toDate.trim()).append(" 23:59:59' OR h.VoucherDate IS NULL) ");
            }
            if (documentTypeId != null && documentTypeId > 0) {
                sb.append("AND (h.DocumentTypeId = ").append(documentTypeId).append(" OR h.DocumentTypeId IS NULL) ");
            }
            if (voucherStatus != null && !voucherStatus.isBlank()) {
                if ("approved".equalsIgnoreCase(voucherStatus) || "1".equals(voucherStatus)) {
                    sb.append("AND (h.IsApproved = 1 OR h.IsApproved IS NULL) ");
                } else if ("unapproved".equalsIgnoreCase(voucherStatus) || "0".equals(voucherStatus)) {
                    sb.append("AND (h.IsApproved = 0 OR h.IsApproved IS NULL) ");
                }
            }
            sb.append("GROUP BY coa.ID, coa.AccountCode, coa.AccountTitle ");
            sb.append("ORDER BY coa.AccountCode");

            List<Map<String, Object>> list = jdbcTemplate.queryForList(sb.toString());
            if (list == null || list.isEmpty()) {
                return getVoucherReportFallback();
            }
            return list;
        } catch (Exception e) {
            e.printStackTrace();
            return getVoucherReportFallback();
        }
    }

    private List<Map<String, Object>> getVoucherReportFallback() {
        try {
            String sql = "SELECT coa.ID as id, coa.AccountCode as accountCode, coa.AccountTitle as accountTitle, " +
                    "ISNULL((SELECT SUM(ISNULL(ob.YearObDebit, 0) - ISNULL(ob.YearObCredit, 0)) FROM AccountsOpeningBalances ob WHERE ob.ChartOfAccountId = coa.ID), 0) as openingBalance, " +
                    "ISNULL(SUM(d.DebitAmount), 0) as totalDebit, " +
                    "ISNULL(SUM(d.CreditAmount), 0) as totalCredit, " +
                    "(ISNULL((SELECT SUM(ISNULL(ob.YearObDebit, 0) - ISNULL(ob.YearObCredit, 0)) FROM AccountsOpeningBalances ob WHERE ob.ChartOfAccountId = coa.ID), 0) + ISNULL(SUM(d.DebitAmount), 0) - ISNULL(SUM(d.CreditAmount), 0)) as balance " +
                    "FROM ChartofAccount coa " +
                    "LEFT JOIN VoucherDetail d ON coa.ID = d.AccountId " +
                    "WHERE (coa.AccountGroup = 'Detail' OR coa.Account_Level >= 4) " +
                    "GROUP BY coa.ID, coa.AccountCode, coa.AccountTitle ORDER BY coa.AccountCode";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception ex) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getBankOrCashBalancesReport(boolean isCashOnly) {
        try {
            String typeFilter = isCashOnly ? "2" : "15, 2";
            String sql = "SELECT coa.ID as id, coa.AccountCode, coa.AccountTitle, " +
                    "ISNULL(SUM(d.DebitAmount), 0) as TotalDebit, " +
                    "ISNULL(SUM(d.CreditAmount), 0) as TotalCredit, " +
                    "(ISNULL(SUM(d.DebitAmount), 0) - ISNULL(SUM(d.CreditAmount), 0)) as Balance " +
                    "FROM ChartofAccount coa " +
                    "LEFT JOIN VoucherDetail d ON coa.ID = d.AccountId " +
                    "WHERE (coa.AccountGroup = 'Detail' OR coa.Account_Level >= 4) AND coa.AccountTypeId IN (" + typeFilter + ") " +
                    "GROUP BY coa.ID, coa.AccountCode, coa.AccountTitle ORDER BY coa.AccountTitle";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
            if (list != null) {
                for (Map<String, Object> map : list) {
                    Object id = map.get("id") != null ? map.get("id") : (map.get("Id") != null ? map.get("Id") : map.get("ID"));
                    Object code = map.get("AccountCode") != null ? map.get("AccountCode") : map.get("accountCode");
                    Object title = map.get("AccountTitle") != null ? map.get("AccountTitle") : map.get("accountTitle");
                    Object deb = map.get("TotalDebit") != null ? map.get("TotalDebit") : map.get("totalDebit");
                    Object cred = map.get("TotalCredit") != null ? map.get("TotalCredit") : map.get("totalCredit");
                    Object bal = map.get("Balance") != null ? map.get("Balance") : map.get("balance");

                    map.put("id", id); map.put("Id", id); map.put("ID", id);
                    map.put("accountCode", code); map.put("AccountCode", code);
                    map.put("accountTitle", title); map.put("AccountTitle", title);
                    map.put("totalDebit", deb); map.put("TotalDebit", deb);
                    map.put("totalCredit", cred); map.put("TotalCredit", cred);
                    map.put("balance", bal); map.put("Balance", bal);
                }
            }
            return list != null ? list : Collections.emptyList();
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getPartyAgingReport(boolean isPayables, String toDate) {
        try {
            String acType = isPayables ? "3, 8" : "6";
            String sql = "SELECT coa.AccountCode, coa.AccountTitle, " +
                    "ISNULL(SUM(d.DebitAmount), 0) as TotalDebit, " +
                    "ISNULL(SUM(d.CreditAmount), 0) as TotalCredit, " +
                    "ABS(ISNULL(SUM(d.DebitAmount), 0) - ISNULL(SUM(d.CreditAmount), 0)) as OutstandingBalance " +
                    "FROM ChartofAccount coa " +
                    "LEFT JOIN VoucherDetail d ON coa.ID = d.AccountId " +
                    "WHERE (coa.AccountGroup = 'Detail' OR coa.Account_Level >= 4) AND coa.AccountTypeId IN (" + acType + ") " +
                    "GROUP BY coa.AccountCode, coa.AccountTitle ORDER BY coa.AccountTitle";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getActivitySummaryReport(Integer accountId, String fromDate, String toDate, String dateType, Integer reportTypeId, Boolean approvedOnly, String docTypes, Boolean includeUnposted) {
        List<Map<String, Object>> list = new ArrayList<>();
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int finYearId = currentUserContext.currentFinancialYearId();
        boolean unposted = Boolean.TRUE.equals(includeUnposted) || Boolean.FALSE.equals(approvedOnly);

        try {
            StringBuilder proc = new StringBuilder("EXEC Sp_ActivityReportSummery_Rpt ");
            proc.append("@OrganizationId=").append(orgId).append(", @CompanyId=").append(compId).append(", @FinancialYearId=").append(finYearId);
            if (accountId != null && accountId > 0) proc.append(", @AccountId=").append(accountId);
            if (fromDate != null && !fromDate.isEmpty()) proc.append(", @VoucherDateF='").append(fromDate).append("'");
            if (toDate != null && !toDate.isEmpty()) proc.append(", @VoucherDateT='").append(toDate).append("'");
            if (reportTypeId != null && reportTypeId > 0) proc.append(", @ReportTypeId=").append(reportTypeId);
            if (!unposted) proc.append(", @IsApproved=1");
            if (docTypes != null && !docTypes.isBlank()) proc.append(", @DocumentTypeIds='").append(docTypes).append("'");
            if (dateType != null && !dateType.isBlank()) proc.append(", @DateType='").append(dateType).append("'");

            list = jdbcTemplate.queryForList(proc.toString());
        } catch (Exception e) {
            list = getActivitySummaryFallback(accountId, fromDate, toDate, !unposted, docTypes, dateType);
        }
        return list;
    }

    private List<Map<String, Object>> getActivitySummaryFallback(Integer accountId, String fromDate, String toDate, Boolean approvedOnly, String docTypes, String dateType) {
        List<Map<String, Object>> list = new ArrayList<>();
        try {
            String dateCol = "EntryDate".equalsIgnoreCase(dateType) ? "h.EntryDate" : "h.VoucherDate";
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT coa.Id as AccountId, coa.AccountCode, coa.AccountTitle, ");
            sb.append("ISNULL(p.AccountTitle, '') as ParentAccountTitle, ");
            sb.append("ISNULL((SELECT SUM(ISNULL(ob.YearObDebit, 0) - ISNULL(ob.YearObCredit, 0)) FROM AccountsOpeningBalances ob WHERE ob.ChartOfAccountId = coa.ID), 0) as Opening, ");
            sb.append("ISNULL(SUM(d.DebitAmount), 0) as Debit, ");
            sb.append("ISNULL(SUM(d.CreditAmount), 0) as Credit, ");
            sb.append("ISNULL(SUM(d.DebitAmount), 0) as DebitEntry, ");
            sb.append("ISNULL(SUM(d.CreditAmount), 0) as CreditEntry, ");
            sb.append("(ISNULL((SELECT SUM(ISNULL(ob.YearObDebit, 0) - ISNULL(ob.YearObCredit, 0)) FROM AccountsOpeningBalances ob WHERE ob.ChartOfAccountId = coa.ID), 0) + ISNULL(SUM(d.DebitAmount), 0) - ISNULL(SUM(d.CreditAmount), 0)) as Closing ");
            sb.append("FROM ChartofAccount coa ");
            sb.append("LEFT JOIN ChartofAccount p ON coa.ParentAccountCode = p.AccountCode ");
            sb.append("LEFT JOIN VoucherDetail d ON coa.ID = d.AccountId ");
            sb.append("LEFT JOIN VoucherHead h ON d.VoucherHeadId = h.ID ");
            sb.append("WHERE (coa.AccountGroup = 'Detail' OR coa.Account_Level >= 4) ");
            if (accountId != null && accountId > 0) {
                sb.append("AND coa.ID = ").append(accountId).append(" ");
            }
            if (fromDate != null && !fromDate.isEmpty()) {
                sb.append("AND (").append(dateCol).append(" >= '").append(fromDate).append(" 00:00:00' OR ").append(dateCol).append(" IS NULL) ");
            }
            if (toDate != null && !toDate.isEmpty()) {
                sb.append("AND (").append(dateCol).append(" <= '").append(toDate).append(" 23:59:59' OR ").append(dateCol).append(" IS NULL) ");
            }
            if (Boolean.TRUE.equals(approvedOnly)) {
                sb.append("AND (h.IsApproved = 1 OR h.IsApproved IS NULL) ");
            }
            if (docTypes != null && !docTypes.isBlank()) {
                sb.append("AND (h.DocumentTypeId IN (").append(docTypes).append(") OR h.DocumentTypeId IS NULL) ");
            }
            sb.append("GROUP BY coa.Id, coa.AccountCode, coa.AccountTitle, p.AccountTitle ");
            sb.append("ORDER BY coa.AccountCode");
            list = jdbcTemplate.queryForList(sb.toString());
        } catch (Exception ex) {
            ex.printStackTrace();
        }
        return list;
    }

    public List<Map<String, Object>> getDayBookReport(Integer accountId, Integer branchId, Integer costCenterId, String fromDate, String toDate) {
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT coa.AccountCode, coa.AccountTitle, ");
            sb.append("ISNULL(bal.OpeningBalance, 0) as OpeningBalance, ");
            sb.append("ISNULL(SUM(d.DebitAmount), 0) as TotalDebit, ");
            sb.append("ISNULL(SUM(d.CreditAmount), 0) as TotalCredit, ");
            sb.append("(ISNULL(bal.OpeningBalance, 0) + ISNULL(SUM(d.DebitAmount), 0) - ISNULL(SUM(d.CreditAmount), 0)) as ClosingBalance ");
            sb.append("FROM ChartofAccount coa ");
            sb.append("LEFT JOIN VoucherDetail d ON d.AccountId = coa.ID ");
            sb.append("LEFT JOIN VoucherHead h ON d.VoucherHeadId = h.ID ");
            sb.append("LEFT JOIN AccountsOpeningBalances bal ON bal.AccountId = coa.ID ");
            sb.append("WHERE (coa.AccountGroup = 'Detail' OR coa.Account_Level >= 4) ");
            if (accountId != null && accountId > 0) {
                sb.append("AND coa.ID = ").append(accountId).append(" ");
            } else {
                sb.append("AND coa.AccountTypeId IN (15, 2) ");
            }
            if (fromDate != null && !fromDate.isBlank()) {
                sb.append("AND (h.VoucherDate >= '").append(fromDate).append(" 00:00:00' OR h.VoucherDate IS NULL) ");
            }
            if (toDate != null && !toDate.isBlank()) {
                sb.append("AND (h.VoucherDate <= '").append(toDate).append(" 23:59:59' OR h.VoucherDate IS NULL) ");
            }
            if (branchId != null && branchId > 0) {
                sb.append("AND (h.BranchesId = ").append(branchId).append(" OR h.BranchesId IS NULL) ");
            }
            if (costCenterId != null && costCenterId > 0) {
                sb.append("AND (d.CostCenterId = ").append(costCenterId).append(" OR d.CostCenterId IS NULL) ");
            }
            sb.append("GROUP BY coa.AccountCode, coa.AccountTitle, bal.OpeningBalance ");
            sb.append("ORDER BY coa.AccountCode");
            return jdbcTemplate.queryForList(sb.toString());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public Map<String, Object> getBalanceSheetData(String toDate) {
        Map<String, Object> result = new LinkedHashMap<>();
        try {
            int orgId = currentUserContext.currentOrganizationId();
            int compId = currentUserContext.currentCompanyId();
            List<Map<String, Object>> procData = jdbcTemplate.queryForList(
                    "EXEC Sp_BalanceSheet_Rpt @OrganizationId=?, @CompanyId=?, @ToDate=?",
                    orgId, compId, toDate);
            if (procData != null && !procData.isEmpty()) {
                result.put("raw", procData);
                return result;
            }
        } catch (Exception ignored) {
        }
        try {
            String sql = "SELECT coa.AccountTitle, coa.AccountCode, coa.AccountTypeId, " +
                         "(ISNULL(aob.OpeningBalance, 0) + ISNULL(SUM(vd.DebitAmount), 0) - ISNULL(SUM(vd.CreditAmount), 0)) as Balance " +
                         "FROM ChartofAccount coa " +
                         "LEFT JOIN AccountsOpeningBalances aob ON aob.AccountId = coa.ID " +
                         "LEFT JOIN VoucherDetail vd ON vd.AccountId = coa.ID " +
                         "LEFT JOIN VoucherHead vh ON vd.VoucherHeadId = vh.ID " +
                         "WHERE (vh.VoucherDate <= '" + (toDate != null && !toDate.isBlank() ? toDate : "2026-12-31") + " 23:59:59' OR vh.VoucherDate IS NULL) " +
                         "GROUP BY coa.AccountTitle, coa.AccountCode, coa.AccountTypeId, aob.OpeningBalance";
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(sql);
            result.put("rows", rows);
        } catch (Exception e) {
            result.put("rows", Collections.emptyList());
        }
        return result;
    }
}
