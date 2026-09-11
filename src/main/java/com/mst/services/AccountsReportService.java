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
            throw new RuntimeException("Error loading General Ledger Report: " + e.getMessage(), e);
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
                info.put("openingBalance", bal.get(0).get("OpeningBalance"));
                info.put("debitAmount", bal.get(0).get("DebitAmount"));
                info.put("creditAmount", bal.get(0).get("CreditAmount"));
                info.put("closingBalance", bal.get(0).get("ClosingBalance"));
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
            return jdbcTemplate.queryForList(
                    "EXEC Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId=?, @CompanyId=?, @UsersId=?, @AppId=?, @CoaType=?",
                    orgId, compId, userId, appId, "DetailAccount");
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Ditto CommonServices.CustomeGroupsDefine(1) -> AcLookUps.GetAll ->
     *  Sp_AcLookUps_GetAllMethod @Activity='ReadAll', @AcLookUpTypesId=1. */
    public List<Map<String, Object>> getCustomGroups() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            return jdbcTemplate.queryForList(
                    "EXEC Sp_AcLookUps_GetAllMethod @OrganizationId=?, @CompanyId=?, @AcLookUpTypesId=?, @Activity=?",
                    orgId, compId, 1, "ReadAll");
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Ditto SupplierCustomerGetforComboServiceBind -> SupplierCustomer.GetforComboBinding ->
     *  Sp_SupplierCustomer_GetAllMethod @Activity='ReadByOrganizationIdCompanyIdForBinding'. Feeds
     *  Customer Ledger Report's comSupplier ("Party Name") dropdown. */
    public List<Map<String, Object>> getSupplierCustomersForCombo() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            return jdbcTemplate.queryForList(
                    "EXEC Sp_SupplierCustomer_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                    orgId, compId, "ReadByOrganizationIdCompanyIdForBinding");
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
            return jdbcTemplate.queryForList(
                    "EXEC USP_GetBranchesFromVouchersByAccountId @OrganizationId=?, @CompanyId=?",
                    orgId, compId);
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

    /** Ditto ChartofAccount.ReadAllAccountgroup -> Sp_ChartofAccount_GetAllMethodFromCOA
     *  @CoaType='ReadAllAccountGroup'. Feeds Trial Balance's CmbGroupAccount. */
    public List<Map<String, Object>> getAccountGroups() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int finYearId = currentUserContext.currentFinancialYearId();
        try {
            return jdbcTemplate.queryForList(
                    "EXEC Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @Account_Level=?, @CoaType=?",
                    orgId, compId, finYearId, 3, "ReadAllAccountGroup");
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Ditto VoucherHead.GetDocumentTypesFromVouchers -> Sp_Vouchers_GetMethods
     *  @Activity='GetDocumentTypesFromVouchers'. Feeds Trial Balance's "Documents Not To Include"
     *  cmbDocumentTypeIds multi-select combo (real columns: Id, DocumentTypeDescription). */
    public List<Map<String, Object>> getDocumentTypesForReports() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            return jdbcTemplate.queryForList(
                    "EXEC Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @Activity=?",
                    orgId, compId, "GetDocumentTypesFromVouchers");
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
            String sql = "SELECT ID as id, CityName as name FROM tbl_City ORDER BY CityName";
            return jdbcTemplate.queryForList(sql);
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
            list = getGeneralLedgerStatementFallback(accountId, fromDate, toDate);
        }
        return list;
    }

    private List<Map<String, Object>> getGeneralLedgerStatementFallback(Integer accountId, String fromDate, String toDate) {
        List<Map<String, Object>> list = new ArrayList<>();
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT coa.AccountCode, coa.AccountTitle, ");
            sb.append("ISNULL(coa.OpeningBalance, 0) as OpeningBalance, ");
            sb.append("ISNULL(SUM(d.DebitAmount), 0) as TotalDebit, ");
            sb.append("ISNULL(SUM(d.CreditAmount), 0) as TotalCredit, ");
            sb.append("(ISNULL(coa.OpeningBalance, 0) + ISNULL(SUM(d.DebitAmount), 0) - ISNULL(SUM(d.CreditAmount), 0)) as ClosingBalance ");
            sb.append("FROM ChartofAccount coa ");
            sb.append("LEFT JOIN tbl_vouchers_detail d ON coa.ID = d.AccountId ");
            sb.append("LEFT JOIN tbl_vouchers_head h ON d.VoucherHeadId = h.ID ");
            sb.append("WHERE 1=1 ");
            if (accountId != null && accountId > 0) {
                sb.append("AND coa.ID = ").append(accountId).append(" ");
            }
            if (fromDate != null && !fromDate.isEmpty()) {
                sb.append("AND (h.VoucherDate >= '").append(fromDate).append(" 00:00:00' OR h.VoucherDate IS NULL) ");
            }
            if (toDate != null && !toDate.isEmpty()) {
                sb.append("AND (h.VoucherDate <= '").append(toDate).append(" 23:59:59' OR h.VoucherDate IS NULL) ");
            }
            sb.append("GROUP BY coa.ID, coa.AccountCode, coa.AccountTitle, coa.OpeningBalance ");
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
        StringBuilder sql = new StringBuilder("EXEC SpCoahierarchy_TrialBalance_Rpt @OrganizationId=?, @CompanyId=?");
        List<Object> params = new ArrayList<>();
        params.add(orgId);
        params.add(compId);
        if (fromDate != null && !fromDate.isBlank()) { sql.append(", @FromDate=?"); params.add(fromDate); }
        if (toDate != null && !toDate.isBlank()) { sql.append(", @ToDate=?"); params.add(toDate); }
        if (skipZero) { sql.append(", @SkipZero=?"); params.add(1); }
        try {
            return jdbcTemplate.queryForList(sql.toString(), params.toArray());
        } catch (Exception e) {
            throw new RuntimeException("Error loading Trial Balances All Level: " + e.getMessage(), e);
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
                    "LEFT JOIN tbl_vouchers_detail d ON coa.ID = d.AccountId " +
                    "WHERE coa.IsDetail = 1 AND coa.AccountTypeId IN (" + typeFilter + ") " +
                    "GROUP BY coa.ID, coa.AccountCode, coa.AccountTitle ORDER BY coa.AccountTitle";
            return jdbcTemplate.queryForList(sql);
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
                    "LEFT JOIN tbl_vouchers_detail d ON coa.ID = d.AccountId " +
                    "WHERE coa.IsDetail = 1 AND coa.AccountTypeId IN (" + acType + ") " +
                    "GROUP BY coa.AccountCode, coa.AccountTitle ORDER BY coa.AccountTitle";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getActivitySummaryReport(Integer accountId, String fromDate, String toDate, String dateType, Integer reportTypeId, Boolean approvedOnly) {
        List<Map<String, Object>> list = new ArrayList<>();
        try {
            StringBuilder proc = new StringBuilder("EXEC Sp_ActivityReportSummery_Rpt ");
            proc.append("@OrganizationId=1, @CompanyId=1, @FinancialYearId=1");
            if (accountId != null && accountId > 0) proc.append(", @AccountId=").append(accountId);
            if (fromDate != null && !fromDate.isEmpty()) proc.append(", @VoucherDateF='").append(fromDate).append("'");
            if (toDate != null && !toDate.isEmpty()) proc.append(", @VoucherDateT='").append(toDate).append("'");
            if (reportTypeId != null && reportTypeId > 0) proc.append(", @ReportTypeId=").append(reportTypeId);
            if (approvedOnly != null && approvedOnly) proc.append(", @IsApproved=1");

            list = jdbcTemplate.queryForList(proc.toString());
        } catch (Exception e) {
            list = getActivitySummaryFallback(accountId, fromDate, toDate, approvedOnly);
        }
        return list;
    }

    private List<Map<String, Object>> getActivitySummaryFallback(Integer accountId, String fromDate, String toDate, Boolean approvedOnly) {
        List<Map<String, Object>> list = new ArrayList<>();
        try {
            StringBuilder sb = new StringBuilder();
            sb.append("SELECT coa.Id as AccountId, coa.AccountCode, coa.AccountTitle, ");
            sb.append("ISNULL(p.AccountTitle, '') as ParentAccountTitle, ");
            sb.append("ISNULL(coa.OpeningBalance, 0) as Opening, ");
            sb.append("ISNULL(SUM(d.DebitAmount), 0) as Debit, ");
            sb.append("ISNULL(SUM(d.CreditAmount), 0) as Credit, ");
            sb.append("(ISNULL(coa.OpeningBalance, 0) + ISNULL(SUM(d.DebitAmount), 0) - ISNULL(SUM(d.CreditAmount), 0)) as Closing ");
            sb.append("FROM ChartofAccount coa ");
            sb.append("LEFT JOIN ChartofAccount p ON coa.ParentAccountCode = p.AccountCode ");
            sb.append("LEFT JOIN tbl_vouchers_detail d ON coa.ID = d.AccountId ");
            sb.append("LEFT JOIN tbl_vouchers_head h ON d.VoucherHeadId = h.ID ");
            sb.append("WHERE 1=1 ");
            if (accountId != null && accountId > 0) {
                sb.append("AND coa.ID = ").append(accountId).append(" ");
            }
            if (fromDate != null && !fromDate.isEmpty()) {
                sb.append("AND (h.VoucherDate >= '").append(fromDate).append(" 00:00:00' OR h.VoucherDate IS NULL) ");
            }
            if (toDate != null && !toDate.isEmpty()) {
                sb.append("AND (h.VoucherDate <= '").append(toDate).append(" 23:59:59' OR h.VoucherDate IS NULL) ");
            }
            if (Boolean.TRUE.equals(approvedOnly)) {
                sb.append("AND (h.IsApproved = 1 OR h.IsApproved IS NULL) ");
            }
            sb.append("GROUP BY coa.Id, coa.AccountCode, coa.AccountTitle, p.AccountTitle, coa.OpeningBalance ");
            sb.append("ORDER BY coa.AccountCode");
            list = jdbcTemplate.queryForList(sb.toString());
        } catch (Exception ex) {
            ex.printStackTrace();
        }
        return list;
    }
}
