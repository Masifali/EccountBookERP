package com.mst.repositories;

import com.mst.models.UserAccount;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/**
 * R3 2026-09-30 - two desktop payables reports that had only fabricated web mock-ups:
 * <ul>
 *   <li>Screen 89 "Subsidiary Payable Receiveables Report" = Account_Reports.SubsidiaryPayableReceiveables
 *       (GetData -> VoucherHead.GetSubsidiaryPayablesReceivables -> USP_GetSubsidiaryPayablesReceivables);</li>
 *   <li>Account_Reports.PayablesWithBillAmount ("Payables Report" with the last bill / last paid amounts;
 *       GetData -> VoucherReports.PayablesWithLastBillAndPaidAmount -> Sp_Accounts_PayablesWithLastBillAndPaidAmount_Rpt).</li>
 * </ul>
 * Tenancy (organisation, company, branch, user, financial year) always comes from the signed-in user.
 */
@Repository
public class PayablesSubsidiaryBillsRepository {
    private final JdbcTemplate jdbc;

    public PayablesSubsidiaryBillsRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    private static int n(Integer v) { return v == null ? 0 : v; }

    /** VoucherHead.GetSubsidiaryPayablesReceivables: all eight parameters are always added (no conditions in the DAL);
     *  @BranchId = UserAccount.BranchesId, @ControlAccountId = the trade report row's AccountId, @PartyTypeId 1 payables / 2 receivables. */
    public List<Map<String, Object>> subsidiary(UserAccount u, LocalDate from, LocalDate to, int controlAccountId, int partyTypeId) {
        return ReportValueSupport.decimalStrings(jdbc.queryForList(
                "EXEC dbo.USP_GetSubsidiaryPayablesReceivables @OrganizationId=?, @CompanyId=?, @BranchId=?, @UserId=?, @DateFrom=?, @DateTo=?, @ControlAccountId=?, @PartyTypeId=?",
                n(u.getOrganizationId()), n(u.getCompanyId()), n(u.getBranchesId()), n(u.getId()),
                java.sql.Date.valueOf(from), java.sql.Date.valueOf(to), controlAccountId, partyTypeId));
    }

    /** VoucherReports.PayablesWithLastBillAndPaidAmount: FinancialYearId always; From/To when set; BalanceFrom/To when non-zero;
     *  @AccountType = obj.AccountId (the form sets 3); @ParentAccountCode = the checked group accounts' AccountCodes when not empty;
     *  @BranchesIds when not empty. No @LanguageId (the form leaves it 0). */
    public List<Map<String, Object>> bills(UserAccount u, int financialYearId, LocalDate from, LocalDate to, int balanceFrom, int balanceTo,
                                           String parentAccountCodes, String branchIds) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_Accounts_PayablesWithLastBillAndPaidAmount_Rpt @OrganizationId=?, @CompanyId=?, @FinancialYearId=?");
        List<Object> args = new ArrayList<>(Arrays.asList(n(u.getOrganizationId()), n(u.getCompanyId()), financialYearId));
        if (from != null) { sql.append(", @FromDate=?"); args.add(java.sql.Date.valueOf(from)); }
        if (to != null) { sql.append(", @ToDate=?"); args.add(java.sql.Date.valueOf(to)); }
        if (balanceFrom != 0) { sql.append(", @BalanceFrom=?"); args.add(balanceFrom); }
        if (balanceTo != 0) { sql.append(", @BalanceTo=?"); args.add(balanceTo); }
        sql.append(", @AccountType=?"); args.add(3);
        if (parentAccountCodes != null && !parentAccountCodes.isEmpty()) { sql.append(", @ParentAccountCode=?"); args.add(parentAccountCodes); }
        if (branchIds != null && !branchIds.isEmpty()) { sql.append(", @BranchesIds=?"); args.add(branchIds); }
        return ReportValueSupport.decimalStrings(jdbc.queryForList(sql.toString(), args.toArray()));
    }

    /** The lists both forms bind on load. */
    public Map<String, Object> lookups(UserAccount u, int financialYearId) {
        int org = n(u.getOrganizationId()), comp = n(u.getCompanyId());
        Map<String, Object> data = new LinkedHashMap<>();
        data.put("amountDecimals", ReportValueSupport.amountDecimals(jdbc, u));
        /* PayablesWithBillAmount.AccountFill3rdLevel: ChartofAccount.ReadAllAccountgroup{FinancialYearId, Account_Level 3,
           AccountTypeId 3, AccouuntClassId 3}; the combo's value column is AccountCode, its text AccountTitle. */
        data.put("controls", jdbc.queryForList("EXEC dbo.Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @Account_Level=3, @AccountTypeId=3, @AccountClassId=3, @CoaType='ReadAllAccountGroup'", org, comp, financialYearId));
        /* clsGlobalVariables.AllAccountsWithCustomGroupId - the payables party accounts (AccountTypeId 3, AccountClass 3)
           that the trade report's rows (and so its SubsidiaryBreakUp) carry. */
        data.put("accounts", jdbc.queryForList("EXEC dbo.USP_GETAllAccountsFromCustomGroups @OrganizationId=?, @CompanyId=?", org, comp));
        /* PayablesWithBillAmount.BranchesFill: GetBranchesFromVouchersByAccountId(OrganizationId, CompanyIdFromBreakUp, "", 0);
           opened from the menu CompanyIdFromBreakUp is 0, and the DAL then leaves @CompanyId out. */
        data.put("branches", jdbc.queryForList("EXEC dbo.USP_GetBranchesFromVouchersByAccountId @OrganizationId=?", org));
        List<Integer> features = new ArrayList<>();
        for (Map<String, Object> r : jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?", org, comp)) {
            Object id = r.get("Id");
            if (id instanceof Number) features.add(((Number) id).intValue());
        }
        data.put("features", features);
        data.put("branchId", n(u.getBranchesId()));
        Map<String, Object> year = null;
        for (Map<String, Object> y : jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?", org, comp)) {
            if (y.get("Id") instanceof Number && ((Number) y.get("Id")).intValue() == financialYearId) { year = y; break; }
        }
        data.put("year", year);
        return data;
    }

    public List<Integer> features(UserAccount u) {
        List<Integer> out = new ArrayList<>();
        for (Map<String, Object> r : jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?", n(u.getOrganizationId()), n(u.getCompanyId()))) {
            Object id = r.get("Id");
            if (id instanceof Number) out.add(((Number) id).intValue());
        }
        return out;
    }
}
