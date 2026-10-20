package com.mst.repositories;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.ChartofAccount;
import com.mst.projections.PayablesRptRow;
import com.mst.projections.ReceivablesByDueDatesRow;
import com.mst.projections.ReceivablesRptRow;

/**
 * Query-only repository (bound to the existing ChartofAccount entity) for the payables / receivables balance
 * reports of batch M2: Sp_Accounts_Payables_Rpt, Sp_Accounts_Receivables_Rpt and Usp_ReceivablesByDueDates.
 * Each procedure is a linear temp-table pipeline (SELECT INTO ... UPDATE ... SELECT) and is rewritten as ONE
 * native statement with a WITH chain; every UPDATE became a LEFT JOIN + CASE/ISNULL computed column, and the
 * temp-table column types the procedures relied on (INT truncation of amounts, NVARCHAR(50) dates, FLOAT)
 * are reproduced with CASTs. A KEEP-EXEC fallback with the same projection is provided for each.
 * Shared helpers used verbatim: dbo.fnSplitString, dbo.GetConfigurationByConfigId, vAccountClasses,
 * V_AcReceiablesMaxDateAndTotalDebit / Credit, TmpShowHideStatusWithAccounts (per-user show/hide status:
 * 1 = hide listed accounts, 2 = show only listed accounts, 0/none = all).
 */
@Repository
public interface AccountsPayablesReceivablesQueryRepository extends org.springframework.data.repository.Repository<ChartofAccount, Integer> {

    // ------------------------------------------------------------------ Sp_Accounts_Payables_Rpt

    /**
     * Sp_Accounts_Payables_Rpt (single branch): payables / receivables control accounts (AccountClass 2,3,
     * AccountTypeId 3,6,8,16,17,18) with opening (ObDebit/ObCredit), current period and closing (ClDebit/
     * ClCredit) balances, keeping only accounts whose net balance is a credit (HAVING ... < 0), plus party /
     * city data, last paid and last bill date/amount.
     * Native CTE rewrite of #Tcoa, #BrancheIds, #TmpVouchersData, #TmpResult, #SupplierCustomerData,
     * #MyDebit, #MyCredit and the three UPDATEs (Final CTE). Output columns and order unchanged except that
     * the procedure emits CityName twice and this statement emits it once.
     * Defaults: @SkipZero NULL/0 = keep zero balances; @ShowOnlyTrade 1 = only accounts with a last bill;
     * @ShowAssetLiability = AccountClass filter; @FinancialYearId is accepted but unused by the procedure.
     */
    @Query(value = "WITH AccStatus AS ( "
            + "SELECT CASE WHEN EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts WHERE UserId = :userId) "
            + "THEN (SELECT TOP 1 AccountStatus FROM TmpShowHideStatusWithAccounts WHERE UserId = :userId) ELSE 0 END AS AccountStatus "
            + "), Tcoa AS ( "
            + "SELECT c.Id, c.AccountCode, c.AccountTitle, c.AccountClass, c.AccountTypeId, c.AccountGroup "
            + "FROM ChartofAccount c "
            + "WHERE c.CompanyId = :companyId AND c.AccountClass IN (2,3) AND c.AccountTypeId IN (3,6,8,16,17,18) "
            + "), BrancheIds AS ( "
            + "SELECT data AS BranchId FROM dbo.fnSplitString(:branchesIds, ',') "
            + "), TmpVouchersData AS ( "
            + "SELECT H.Id, H.DocumentTypeId, H.VoucherDate, D.AccountId, "
            + "CASE WHEN :branchesIds IS NULL THEN 0 ELSE D.BranchesId END AS BranchesId, "
            + "D.DebitAmount, D.CreditAmount, H.FinancialYearId, H.OrganizationId, H.CompanyId "
            + "FROM VoucherDetail D INNER JOIN VoucherHead H ON D.VoucherHeadId = H.Id "
            + "WHERE H.OrganizationId = :organizationId AND H.CompanyId = :companyId "
            + "AND H.VoucherDate >= :fromDate AND H.VoucherDate <= :toDate "
            + "), TmpResult AS ( "
            + "SELECT B.AccountId, B.AccountCode, B.AccountTitle, B.AccountClass, B.AccountTypeId, B.AccountGroup, "
            + "SUM(ObDebit) AS ObDebit, SUM(ObCredit) AS ObCredit, SUM(CurrDebit) AS CurrDebit, SUM(CurrCredit) AS CurrCredit, "
            + "CASE WHEN SUM(ObDebit) - SUM(ObCredit) + SUM(CurrDebit) - SUM(CurrCredit) > 0 THEN SUM(ObDebit) - SUM(ObCredit) + SUM(CurrDebit) - SUM(CurrCredit) ELSE 0 END AS ClDebit, "
            + "CASE WHEN SUM(ObDebit) - SUM(ObCredit) + SUM(CurrDebit) - SUM(CurrCredit) < 0 THEN ABS(SUM(ObDebit) - SUM(ObCredit) + SUM(CurrDebit) - SUM(CurrCredit)) ELSE 0 END AS ClCredit, "
            + "B.OrganizationId, B.CompanyId, c.ParentCodeId, B.BranchesId "
            + "FROM ( "
            + "SELECT AccountId, BranchesId, AccountCode, AccountTitle, AccountClass, AccountTypeId, AccountGroup, "
            + "CASE WHEN SUM(YearObDebit) - SUM(YearObCredit) > 0 THEN SUM(YearObDebit) - SUM(YearObCredit) ELSE 0 END AS ObDebit, "
            + "CASE WHEN SUM(YearObDebit) - SUM(YearObCredit) < 0 THEN ABS(SUM(YearObDebit) - SUM(YearObCredit)) ELSE 0 END AS ObCredit, "
            + "0 AS CurrDebit, 0 AS CurrCredit, 0 AS ClDebit, 0 AS ClCredit, OrganizationId, CompanyId "
            + "FROM ( "
            + "SELECT Ob.ChartOfAccountId AS AccountId, 0 AS BranchesId, Coa.AccountCode, Coa.AccountTitle, Coa.AccountClass, Coa.AccountTypeId, Coa.AccountGroup, "
            + "Ob.YearObDebit AS YearObDebit, Ob.YearObCredit AS YearObCredit, Ob.OrganizationId, Ob.CompanyId "
            + "FROM AccountsOpeningBalances AS Ob INNER JOIN Tcoa AS Coa ON Ob.ChartOfAccountId = Coa.Id "
            + "WHERE Ob.OrganizationId = :organizationId AND Ob.CompanyId = :companyId "
            + "GROUP BY Ob.Id, Ob.ChartOfAccountId, Ob.YearObDebit, Ob.YearObCredit, Ob.CompanyId, Ob.FinancialYearId, Ob.OrganizationId, "
            + "Coa.AccountCode, Coa.AccountTitle, Coa.AccountClass, Coa.AccountTypeId, Coa.AccountGroup "
            + "UNION ALL "
            + "SELECT Vd.AccountId, CASE WHEN :branchesIds IS NULL THEN 0 ELSE Vd.BranchesId END AS BranchesId, "
            + "Coa.AccountCode, Coa.AccountTitle, Coa.AccountClass, Coa.AccountTypeId, Coa.AccountGroup, "
            + "CASE WHEN SUM(Vd.DebitAmount) - SUM(Vd.CreditAmount) > 0 THEN SUM(Vd.DebitAmount) - SUM(Vd.CreditAmount) ELSE 0 END AS DebitAmount, "
            + "CASE WHEN SUM(Vd.DebitAmount) - SUM(Vd.CreditAmount) < 0 THEN ABS(SUM(Vd.DebitAmount) - SUM(Vd.CreditAmount)) ELSE 0 END AS CreditAmount, "
            + "VH.OrganizationId, VH.CompanyId "
            + "FROM VoucherHead AS VH INNER JOIN VoucherDetail AS Vd ON VH.Id = Vd.VoucherHeadId INNER JOIN Tcoa AS Coa ON Vd.AccountId = Coa.Id "
            + "WHERE VH.OrganizationId = :organizationId AND VH.CompanyId = :companyId AND VH.VoucherDate < :fromDate "
            + "AND (:branchesIds IS NULL OR EXISTS (SELECT 1 FROM BrancheIds WHERE BranchId = Vd.BranchesId)) "
            + "GROUP BY Vd.AccountId, VH.OrganizationId, VH.CompanyId, VH.FinancialYearId, Coa.AccountCode, Coa.AccountTitle, Coa.AccountClass, "
            + "Coa.AccountTypeId, Coa.AccountGroup, VH.VoucherDate, CASE WHEN :branchesIds IS NULL THEN 0 ELSE Vd.BranchesId END "
            + ") A GROUP BY AccountId, A.BranchesId, AccountCode, AccountTitle, AccountClass, AccountTypeId, AccountGroup, OrganizationId, CompanyId "
            + "UNION ALL "
            + "SELECT Vd.AccountId, Vd.BranchesId, Coa.AccountCode, Coa.AccountTitle, Coa.AccountClass, Coa.AccountTypeId, Coa.AccountGroup, "
            + "0, 0, SUM(Vd.DebitAmount) AS DebitAmount, SUM(Vd.CreditAmount) AS CreditAmount, 0, 0, Vd.OrganizationId, Vd.CompanyId "
            + "FROM TmpVouchersData Vd INNER JOIN Tcoa AS Coa ON Vd.AccountId = Coa.Id "
            + "WHERE (:branchesIds IS NULL OR EXISTS (SELECT 1 FROM BrancheIds WHERE BranchId = Vd.BranchesId)) "
            + "GROUP BY Vd.AccountId, Vd.OrganizationId, Vd.CompanyId, Coa.AccountCode, Coa.AccountTitle, Coa.AccountClass, Coa.AccountTypeId, Coa.AccountGroup, Vd.BranchesId "
            + ") B INNER JOIN ChartofAccount c ON c.Id = B.AccountId "
            + "GROUP BY B.AccountId, B.AccountCode, B.AccountTitle, B.AccountClass, B.AccountTypeId, B.AccountGroup, B.OrganizationId, B.CompanyId, c.ParentCodeId, B.BranchesId "
            + "HAVING (SUM(ObDebit) - SUM(ObCredit) + SUM(CurrDebit) - SUM(CurrCredit)) < 0 "
            + "), SupplierCustomerData AS ( "
            + "SELECT A.RNo, A.AccountId, A.CityId, A.CityName, A.CompanyId, A.MobilePersonal, A.Title, A.CustomerGroupId, A.SupplierCustomerId "
            + "FROM ( "
            + "SELECT ROW_NUMBER() OVER (PARTITION BY R.AccountId ORDER BY R.AccountId, sp.CityId) AS RNo, sp.CityId, R.AccountId, sp.CompanyId, "
            + "CASE WHEN City.Description IS NULL THEN 'Other City' ELSE City.Description END AS CityName, "
            + "sp.MobilePersonal, sp.Title, sp.CustomerGroupId, sp.Id AS SupplierCustomerId "
            + "FROM TmpResult R INNER JOIN SupplierCustomer sp ON R.AccountId = sp.GlAccountId AND sp.CompanyId = :companyId "
            + "LEFT JOIN City ON sp.CityId = City.Id "
            + ") A WHERE A.RNo = 1 "
            + "), MyDebit AS ( "
            + "SELECT a.row_num, a.AccountId, a.VoucherDate, a.DebitAmount FROM ( "
            + "SELECT ROW_NUMBER() OVER (PARTITION BY Vd.AccountId ORDER BY Maxdt.VoucherDate DESC) AS row_num, "
            + "Vd.AccountId AS AccountId, Maxdt.VoucherDate, ISNULL(SUM(Vd.DebitAmount), 0) AS DebitAmount "
            + "FROM TmpVouchersData Vd "
            + "INNER JOIN V_AcReceiablesMaxDateAndTotalDebit AS Maxdt ON Vd.AccountId = Maxdt.AccountId AND Vd.VoucherDate = Maxdt.VoucherDate "
            + "INNER JOIN ChartofAccount T ON Maxdt.AccountId = T.Id AND Vd.AccountId = Maxdt.AccountId "
            + "WHERE Vd.DocumentTypeId IN (1,2,7,11,14,21,22,23,24) "
            + "GROUP BY Vd.AccountId, Maxdt.VoucherDate "
            + ") a WHERE a.row_num = 1 "
            + "), MyCredit AS ( "
            + "SELECT a.row_num, a.AccountId, a.VoucherDate, a.CreditAmount FROM ( "
            + "SELECT ROW_NUMBER() OVER (PARTITION BY Maxdt.AccountId ORDER BY Maxdt.VoucherDate DESC) AS row_num, "
            + "Maxdt.AccountId AS AccountId, Maxdt.VoucherDate, ISNULL(SUM(Vd.CreditAmount), 0) AS CreditAmount "
            + "FROM TmpVouchersData Vd "
            + "INNER JOIN V_AcReceiablesMaxDateAndTotalCredit AS Maxdt ON Vd.AccountId = Maxdt.AccountId AND Vd.VoucherDate = Maxdt.VoucherDate "
            + "INNER JOIN ChartofAccount T ON Maxdt.AccountId = T.Id AND Vd.AccountId = Maxdt.AccountId "
            + "WHERE Vd.DocumentTypeId IN (56,57,702,61,64,1603,1604) "
            + "GROUP BY Maxdt.AccountId, Maxdt.VoucherDate "
            + ") a WHERE a.row_num = 1 "
            + "), Final AS ( "
            + "SELECT R.AccountId, R.AccountCode, R.AccountTitle, R.AccountClass, R.AccountTypeId, R.AccountGroup, "
            + "R.ObDebit, R.ObCredit, R.CurrDebit, R.CurrCredit, R.ClDebit, R.ClCredit, R.OrganizationId, R.CompanyId, R.ParentCodeId, R.BranchesId, "
            + "ISNULL(S.SupplierCustomerId, 0) AS SupplierCustomerId, "
            + "ISNULL(S.CityId, 0) AS CityId, "
            + "CASE WHEN S.AccountId IS NULL THEN CAST('' AS NVARCHAR(150)) ELSE CAST(S.CityName AS NVARCHAR(150)) END AS CityName, "
            + "CASE WHEN S.AccountId IS NULL THEN CAST('' AS NVARCHAR(50)) ELSE CAST(S.MobilePersonal AS NVARCHAR(50)) END AS MobilePersonal, "
            + "CASE WHEN S.AccountId IS NULL THEN CAST('' AS NVARCHAR(250)) ELSE CAST(S.Title AS NVARCHAR(250)) END AS Title, "
            + "CASE WHEN S.AccountId IS NULL THEN 0 ELSE S.CustomerGroupId END AS CustomerGroupId, "
            + "CASE WHEN MD.AccountId IS NULL THEN CONVERT(NVARCHAR(50), '') ELSE CONVERT(NVARCHAR(50), MD.VoucherDate) END AS LastPaidDate, "
            + "CASE WHEN MD.AccountId IS NULL THEN 0 ELSE CAST(MD.DebitAmount AS INT) END AS LastPidAmount, "
            + "CASE WHEN MC.AccountId IS NULL THEN CONVERT(NVARCHAR(50), '') ELSE CONVERT(NVARCHAR(50), MC.VoucherDate) END AS LastBillDate, "
            + "CASE WHEN MC.AccountId IS NULL THEN 0 ELSE CAST(MC.CreditAmount AS INT) END AS LastBillsAmount "
            + "FROM TmpResult R "
            + "LEFT JOIN SupplierCustomerData S ON R.AccountId = S.AccountId AND S.CompanyId = :companyId "
            + "LEFT JOIN MyDebit MD ON R.AccountId = MD.AccountId "
            + "LEFT JOIN MyCredit MC ON R.AccountId = MC.AccountId "
            + ") "
            + "SELECT R.AccountId, R.AccountCode, R.AccountTitle, R.AccountClass, R.AccountTypeId, R.AccountGroup, "
            + "R.ObDebit - R.ObCredit AS Opening, R.ObDebit, R.ObCredit, R.CurrDebit, R.CurrCredit, R.ClDebit, R.ClCredit, "
            + "R.ClDebit - R.ClCredit AS Closing, "
            + "R.ClDebit - R.ObDebit AS [Increase/Decrease_Debit], "
            + "R.ClCredit - R.ObCredit AS [Increase/Decrease_Credit], "
            + "(R.ClDebit - R.ClCredit) - (R.ObDebit - R.ObCredit) AS [Increase/Decrease], "
            + "R.OrganizationId, R.CompanyId, 0 AS FinancialYearId, R.CityName, "
            + "CONVERT(INT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 78)) AS DecimalPointsAmount, "
            + "(SELECT TOP 1 co.CompLogoImage FROM Company co WHERE co.Id = :companyId) AS CompLogoImage, "
            + "c.ParentCodeId, c.AccountTitle AS ParentAccountTitle, t.Abbreviation AS AccountType, CLS.ClassName, "
            + "R.BranchesId, BR.BranchName, R.SupplierCustomerId, R.CityId, R.MobilePersonal, R.Title, R.CustomerGroupId, "
            + "R.LastBillDate, R.LastBillsAmount, "
            + "CASE WHEN ISNULL(R.LastBillDate, '') <> '' THEN DATEDIFF(DAY, CONVERT(DATE, R.LastBillDate), CONVERT(DATE, GETDATE())) ELSE 0 END AS BillDays, "
            + "R.LastPaidDate, R.LastPidAmount, "
            + "CASE WHEN ISNULL(R.LastPaidDate, '') <> '' THEN DATEDIFF(DAY, CONVERT(DATE, R.LastPaidDate), CONVERT(DATE, GETDATE())) ELSE 0 END AS PaidDays "
            + "FROM Final R "
            + "CROSS JOIN AccStatus st "
            + "LEFT JOIN ChartofAccount c ON c.Id = R.ParentCodeId "
            + "LEFT JOIN AccountTypes t ON t.Id = R.AccountTypeId "
            + "LEFT JOIN vAccountClasses CLS ON R.AccountClass = CLS.ClassId "
            + "LEFT JOIN Branches BR ON R.BranchesId = BR.Id "
            + "LEFT JOIN AccountsCustomGroups cg ON R.AccountId = cg.ChartOfAccountId "
            + "WHERE ( "
            + "(st.AccountStatus = 1 AND NOT EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts t WHERE t.ChartofAccountId = R.AccountId AND UserId = :userId)) "
            + "OR (st.AccountStatus = 2 AND EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts t WHERE t.ChartofAccountId = R.AccountId AND UserId = :userId)) "
            + "OR (st.AccountStatus = 0) ) "
            + "AND ( (ISNULL(:skipZero, 0) = 1 AND (ABS(R.ClDebit - R.ClCredit) > 0)) OR (ISNULL(:skipZero, 0) = 0) ) "
            + "AND (:customGroupId IS NULL OR cg.AcLookUpsId = :customGroupId) "
            + "AND (:controlAccountIds IS NULL OR EXISTS (SELECT 1 FROM fnSplitString(:controlAccountIds, ',') AS PC WHERE PC.Data = R.ParentCodeId)) "
            + "AND (:customerGroupIds IS NULL OR R.CustomerGroupId IN (SELECT data FROM dbo.fnSplitString(:customerGroupIds, ','))) "
            + "AND (:cityId IS NULL OR R.CityId = :cityId) "
            + "AND (:balanceFrom IS NULL OR ABS(R.ClDebit - R.ClCredit) >= :balanceFrom) "
            + "AND (:balanceTo IS NULL OR ABS(R.ClDebit - R.ClCredit) <= :balanceTo) "
            + "AND ((:showOnlyTrade = 1 AND (R.LastBillsAmount > 0) OR :showOnlyTrade IS NULL)) "
            + "AND (:showAssetLiability IS NULL OR R.AccountClass = :showAssetLiability)",
            nativeQuery = true)
    List<PayablesRptRow> payablesRpt(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("userId") Integer userId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate,
            @Param("skipZero") Integer skipZero,
            @Param("customGroupId") Integer customGroupId,
            @Param("controlAccountIds") String controlAccountIds,
            @Param("branchesIds") String branchesIds,
            @Param("balanceFrom") Double balanceFrom,
            @Param("balanceTo") Double balanceTo,
            @Param("cityId") Integer cityId,
            @Param("customerGroupIds") String customerGroupIds,
            @Param("showOnlyTrade") Integer showOnlyTrade,
            @Param("showAssetLiability") Integer showAssetLiability);

    /**
     * Sp_Accounts_Payables_Rpt -- KEEP-EXEC fallback: the original procedure call with all 16 parameters
     * (@FinancialYearId is unused by the procedure). Same projection; the duplicate CityName column of the
     * procedure maps onto the single getCityName().
     */
    @Query(value = "EXEC dbo.Sp_Accounts_Payables_Rpt "
            + "@OrganizationId = :organizationId, @CompanyId = :companyId, @FinancialYearId = :financialYearId, "
            + "@UserId = :userId, @FromDate = :fromDate, @ToDate = :toDate, @SkipZero = :skipZero, "
            + "@CustomGroupId = :customGroupId, @ControlAccountIds = :controlAccountIds, @BranchesIds = :branchesIds, "
            + "@BalanceFrom = :balanceFrom, @BalanceTo = :balanceTo, @CityId = :cityId, "
            + "@CustomerGroupIds = :customerGroupIds, @ShowOnlyTrade = :showOnlyTrade, @ShowAssetLiability = :showAssetLiability",
            nativeQuery = true)
    List<PayablesRptRow> payablesRptExec(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("financialYearId") Integer financialYearId,
            @Param("userId") Integer userId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate,
            @Param("skipZero") Integer skipZero,
            @Param("customGroupId") Integer customGroupId,
            @Param("controlAccountIds") String controlAccountIds,
            @Param("branchesIds") String branchesIds,
            @Param("balanceFrom") Double balanceFrom,
            @Param("balanceTo") Double balanceTo,
            @Param("cityId") Integer cityId,
            @Param("customerGroupIds") String customerGroupIds,
            @Param("showOnlyTrade") Integer showOnlyTrade,
            @Param("showAssetLiability") Integer showAssetLiability);

    // ------------------------------------------------------------------ Sp_Accounts_Receivables_Rpt

    /**
     * Sp_Accounts_Receivables_Rpt (single branch): same control-account set as the payables report, keeping
     * accounts whose net balance is a debit (HAVING ... > 0), with last received / last bill date and amount
     * (V_AcReceiablesMaxDateAndTotal* views), latest follow-up / promise date, party and city data.
     * Native CTE rewrite of #Tcoa, #BrancheIds, #TmpVouchersData, #ReceivablesTemp, #MyCredits, #MyDebits,
     * #FollowuptHistory, #SupplierCustomerData (+ its UPDATE as RecvTemp2) and #TmpResult. The procedure's
     * `ORDER BY R.CityName` sits on the SELECT INTO and does not order the final result; not reproduced.
     * Defaults: @SkipZero NULL/0 keeps zero balances; @ShowOnlyTrade 1 = only accounts with a last bill;
     * @LanguageId > 0 switches to ChartOfAccountMultiLingo titles; @AccountType / @FinancialYearId unused.
     */
    @Query(value = "WITH AccStatus AS ( "
            + "SELECT CASE WHEN EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts WHERE UserId = :userId) "
            + "THEN (SELECT TOP 1 AccountStatus FROM TmpShowHideStatusWithAccounts WHERE UserId = :userId) ELSE 0 END AS AccountStatus "
            + "), Tcoa AS ( "
            + "SELECT c.Id, c.AccountCode, c.AccountTitle, c.AccountClass, c.AccountTypeId, c.AccountGroup "
            + "FROM ChartofAccount c "
            + "WHERE c.CompanyId = :companyId AND c.AccountClass IN (2,3) AND c.AccountTypeId IN (3,6,8,16,17,18) "
            + "), BrancheIds AS ( "
            + "SELECT data AS BranchId FROM dbo.fnSplitString(:branchesIds, ',') "
            + "), TmpVouchersData AS ( "
            + "SELECT H.Id, H.DocumentTypeId, H.VoucherDate, D.AccountId, "
            + "CASE WHEN :branchesIds IS NULL THEN 0 ELSE D.BranchesId END AS BranchesId, "
            + "D.DebitAmount, D.CreditAmount, H.OrganizationId, H.CompanyId "
            + "FROM VoucherDetail D INNER JOIN VoucherHead H ON D.VoucherHeadId = H.Id "
            + "WHERE H.OrganizationId = :organizationId AND H.CompanyId = :companyId "
            + "AND H.VoucherDate >= :fromDate AND H.VoucherDate <= :toDate "
            + "), ReceivablesTemp AS ( "
            + "SELECT AccountId, AccountCode, AccountTitle, AccountClass, AccountTypeId, AccountGroup, "
            + "SUM(ObDebit) AS ObDebit, SUM(ObCredit) AS ObCredit, SUM(CurrDebit) AS CurrDebit, SUM(CurrCredit) AS CurrCredit, "
            + "CASE WHEN SUM(ObDebit) - SUM(ObCredit) + SUM(CurrDebit) - SUM(CurrCredit) > 0 THEN SUM(ObDebit) - SUM(ObCredit) + SUM(CurrDebit) - SUM(CurrCredit) ELSE 0 END AS ClDebit, "
            + "CASE WHEN SUM(ObDebit) - SUM(ObCredit) + SUM(CurrDebit) - SUM(CurrCredit) < 0 THEN SUM(ObDebit) - SUM(ObCredit) + SUM(CurrDebit) - SUM(CurrCredit) ELSE 0 END AS Credit, "
            + "OrganizationId, CompanyId, B.BranchesId "
            + "FROM ( "
            + "SELECT AccountId, AccountCode, AccountTitle, AccountClass, AccountTypeId, AccountGroup, "
            + "CASE WHEN SUM(YearObDebit) - SUM(YearObCredit) > 0 THEN SUM(YearObDebit) - SUM(YearObCredit) ELSE 0 END AS ObDebit, "
            + "CASE WHEN SUM(YearObDebit) - SUM(YearObCredit) < 0 THEN ABS(SUM(YearObDebit) - SUM(YearObCredit)) ELSE 0 END AS ObCredit, "
            + "0 AS CurrDebit, 0 AS CurrCredit, 0 AS ClDebit, 0 AS ClCredit, OrganizationId, CompanyId, A.BranchesId "
            + "FROM ( "
            + "SELECT Ob.ChartOfAccountId AS AccountId, Coa.AccountCode, Coa.AccountTitle, Coa.AccountClass, Coa.AccountTypeId, Coa.AccountGroup, "
            + "Ob.YearObDebit AS YearObDebit, Ob.YearObCredit AS YearObCredit, Ob.OrganizationId, Ob.CompanyId, 0 AS BranchesId "
            + "FROM AccountsOpeningBalances AS Ob INNER JOIN Tcoa AS Coa ON Ob.ChartOfAccountId = Coa.Id "
            + "WHERE Ob.OrganizationId = :organizationId AND Ob.CompanyId = :companyId "
            + "GROUP BY Ob.Id, Ob.ChartOfAccountId, Ob.YearObDebit, Ob.YearObCredit, Ob.CompanyId, Ob.OrganizationId, "
            + "Coa.AccountCode, Coa.AccountTitle, Coa.AccountClass, Coa.AccountTypeId, Coa.AccountGroup "
            + "UNION ALL "
            + "SELECT Vd.AccountId, Coa.AccountCode, Coa.AccountTitle, Coa.AccountClass, Coa.AccountTypeId, Coa.AccountGroup, "
            + "CASE WHEN SUM(Vd.DebitAmount) - SUM(Vd.CreditAmount) > 0 THEN SUM(Vd.DebitAmount) - SUM(Vd.CreditAmount) ELSE 0 END AS DebitAmount, "
            + "CASE WHEN SUM(Vd.DebitAmount) - SUM(Vd.CreditAmount) < 0 THEN ABS(SUM(Vd.DebitAmount) - SUM(Vd.CreditAmount)) ELSE 0 END AS CreditAmount, "
            + "VH.OrganizationId, VH.CompanyId, CASE WHEN :branchesIds IS NULL THEN 0 ELSE Vd.BranchesId END AS BranchesId "
            + "FROM VoucherHead AS VH INNER JOIN VoucherDetail AS Vd ON VH.Id = Vd.VoucherHeadId INNER JOIN Tcoa AS Coa ON Vd.AccountId = Coa.Id "
            + "WHERE VH.OrganizationId = :organizationId AND VH.CompanyId = :companyId AND VH.VoucherDate < :fromDate "
            + "AND (:branchesIds IS NULL OR EXISTS (SELECT 1 FROM BrancheIds WHERE BranchId = Vd.BranchesId)) "
            + "GROUP BY Vd.AccountId, VH.OrganizationId, VH.CompanyId, Coa.AccountCode, Coa.AccountTitle, Coa.AccountClass, Coa.AccountTypeId, Coa.AccountGroup, "
            + "CASE WHEN :branchesIds IS NULL THEN 0 ELSE Vd.BranchesId END "
            + ") A GROUP BY AccountId, AccountCode, AccountTitle, AccountClass, AccountTypeId, AccountGroup, OrganizationId, CompanyId, A.BranchesId "
            + "UNION ALL "
            + "SELECT Vd.AccountId, Coa.AccountCode, Coa.AccountTitle, Coa.AccountClass, Coa.AccountTypeId, Coa.AccountGroup, "
            + "0, 0, SUM(Vd.DebitAmount) AS DebitAmount, SUM(Vd.CreditAmount) AS CreditAmount, 0, 0, Vd.OrganizationId, Vd.CompanyId, Vd.BranchesId "
            + "FROM TmpVouchersData Vd INNER JOIN Tcoa AS Coa ON Vd.AccountId = Coa.Id "
            + "WHERE (:branchesIds IS NULL OR EXISTS (SELECT 1 FROM BrancheIds WHERE BranchId = Vd.BranchesId)) "
            + "GROUP BY Vd.AccountId, Vd.OrganizationId, Vd.CompanyId, Coa.AccountCode, Coa.AccountTitle, Coa.AccountClass, Coa.AccountTypeId, Coa.AccountGroup, Vd.VoucherDate, Vd.BranchesId "
            + ") B GROUP BY AccountId, AccountCode, AccountTitle, AccountClass, AccountTypeId, AccountGroup, OrganizationId, CompanyId, B.BranchesId "
            + "HAVING (SUM(ObDebit) - SUM(ObCredit) + SUM(CurrDebit) - SUM(CurrCredit)) > 0 "
            + "), MyCredits AS ( "
            + "SELECT a.row_num, a.AccountId, a.VoucherDate, a.CreditAmount FROM ( "
            + "SELECT ROW_NUMBER() OVER (PARTITION BY Vd.AccountId ORDER BY Maxdt.VoucherDate DESC) AS row_num, "
            + "Vd.AccountId AS AccountId, Maxdt.VoucherDate, ISNULL(SUM(Vd.CreditAmount), 0) AS CreditAmount "
            + "FROM TmpVouchersData Vd "
            + "INNER JOIN V_AcReceiablesMaxDateAndTotalCredit AS Maxdt ON Vd.AccountId = Maxdt.AccountId AND Vd.VoucherDate = Maxdt.VoucherDate "
            + "INNER JOIN Tcoa T ON Maxdt.AccountId = T.Id "
            + "WHERE Vd.OrganizationId = :organizationId AND Vd.CompanyId = :companyId "
            + "AND Vd.DocumentTypeId IN (3,4,8,12,13,15,16,203) "
            + "GROUP BY Vd.AccountId, Maxdt.VoucherDate "
            + ") a WHERE a.row_num = 1 "
            + "), MyDebits AS ( "
            + "SELECT a.row_num, a.AccountId, a.VoucherDate, a.DebitAmount FROM ( "
            + "SELECT ROW_NUMBER() OVER (PARTITION BY Maxdt.AccountId ORDER BY Maxdt.VoucherDate DESC) AS row_num, "
            + "Maxdt.AccountId AS AccountId, Maxdt.VoucherDate, ISNULL(SUM(Vd.DebitAmount), 0) AS DebitAmount "
            + "FROM TmpVouchersData Vd "
            + "INNER JOIN V_AcReceiablesMaxDateAndTotalDebit AS Maxdt ON Vd.AccountId = Maxdt.AccountId AND Vd.VoucherDate = Maxdt.VoucherDate "
            + "INNER JOIN Tcoa T ON Maxdt.AccountId = T.Id "
            + "WHERE Vd.DocumentTypeId IN (95,99,100,103,106,126,128,133) "
            + "GROUP BY Maxdt.AccountId, Maxdt.VoucherDate "
            + ") a WHERE a.row_num = 1 "
            + "), FollowuptHistory AS ( "
            + "SELECT h.ChartOfAccountId, MAX(h.FollowupDate) AS FollowupDate, h.PromiseDate "
            + "FROM AccountsPayRecFollowUpHistory h "
            + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND FollowupDate <= :toDate "
            + "GROUP BY h.ChartOfAccountId, h.PromiseDate "
            + "), SupplierCustomerData AS ( "
            + "SELECT A.RNo, A.AccountId, A.CityId, A.CityName, A.CompanyId, A.MobilePersonal, A.Title, A.CustomerGroupId, A.SupplierCustomerId "
            + "FROM ( "
            + "SELECT ROW_NUMBER() OVER (PARTITION BY R.AccountId ORDER BY R.AccountId, sp.CityId) AS RNo, sp.CityId, R.AccountId, sp.CompanyId, "
            + "CASE WHEN City.Description IS NULL THEN 'Other City' ELSE City.Description END AS CityName, "
            + "sp.MobilePersonal, sp.Title, sp.CustomerGroupId, sp.Id AS SupplierCustomerId "
            + "FROM ReceivablesTemp R INNER JOIN SupplierCustomer sp ON R.AccountId = sp.GlAccountId AND sp.CompanyId = :companyId "
            + "LEFT JOIN City ON sp.CityId = City.Id "
            + ") A WHERE A.RNo = 1 "
            + "), RecvTemp2 AS ( "
            + "SELECT R.AccountId, R.AccountCode, R.AccountTitle, R.AccountClass, R.AccountTypeId, R.AccountGroup, "
            + "R.ObDebit, R.ObCredit, R.CurrDebit, R.CurrCredit, R.ClDebit, R.Credit, R.OrganizationId, R.CompanyId, R.BranchesId, "
            + "ISNULL(S.CityId, 0) AS CityId, "
            + "CASE WHEN S.AccountId IS NULL THEN CAST('' AS NVARCHAR(150)) ELSE CAST(S.CityName AS NVARCHAR(150)) END AS CityName, "
            + "CASE WHEN S.AccountId IS NULL THEN CAST('' AS NVARCHAR(50)) ELSE CAST(S.MobilePersonal AS NVARCHAR(50)) END AS MobilePersonal, "
            + "CASE WHEN S.AccountId IS NULL THEN CAST('' AS NVARCHAR(250)) ELSE CAST(S.Title AS NVARCHAR(250)) END AS Title, "
            + "CASE WHEN S.AccountId IS NULL THEN 0 ELSE S.CustomerGroupId END AS CustomerGroupId, "
            + "ISNULL(S.SupplierCustomerId, 0) AS SupplierCustomerId "
            + "FROM ReceivablesTemp R "
            + "LEFT JOIN SupplierCustomerData S ON R.AccountId = S.AccountId AND S.CompanyId = :companyId "
            + "), TmpResult AS ( "
            + "SELECT DISTINCT r.AccountId, r.SupplierCustomerId AS Id, r.AccountCode, "
            + "CASE WHEN L.MultiLanguagesId > 0 THEN L.AccountTitle ELSE r.AccountTitle END AS AccountTitle, "
            + "(r.ObDebit - r.ObCredit) AS Opening, r.ObDebit, r.ObCredit, r.CurrDebit, r.CurrCredit, r.ClDebit, "
            + "dr.VoucherDate AS LastBillDate, dr.DebitAmount AS LastBillsAmount, "
            + "CASE WHEN NOT (dr.VoucherDate IS NULL) THEN DATEDIFF(DAY, CONVERT(DATE, dr.VoucherDate), CONVERT(DATE, GETDATE())) ELSE 0 END AS BillDays, "
            + "cr.VoucherDate AS LastRcvdDate, ISNULL(cr.CreditAmount, 0) AS LastRcvdAmount, "
            + "CASE WHEN NOT (cr.VoucherDate IS NULL) THEN DATEDIFF(DAY, CONVERT(DATE, cr.VoucherDate), CONVERT(DATE, GETDATE())) ELSE 0 END AS RcvdDays, "
            + "r.CityName, r.MobilePersonal, r.Title, fh.FollowupDate, fh.PromiseDate, cmp.CompLogoImage, "
            + "CONVERT(INT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, 78)) AS DecimalPointsAmount, "
            + "r.BranchesId, br.BranchName, CL.ClassName, r.AccountClass, ATP.Abbreviation AS AccountType, PAC.AccountTitle AS ParentAccount "
            + "FROM RecvTemp2 r "
            + "INNER JOIN ChartofAccount c ON r.AccountId = c.Id "
            + "INNER JOIN ChartofAccount PAC ON c.ParentCodeId = PAC.Id "
            + "INNER JOIN AccountTypes ATP ON r.AccountTypeId = ATP.Id "
            + "INNER JOIN vAccountClasses CL ON c.AccountClass = CL.ClassId "
            + "LEFT OUTER JOIN MyCredits cr ON r.AccountId = cr.AccountId "
            + "LEFT OUTER JOIN MyDebits dr ON r.AccountId = dr.AccountId "
            + "LEFT OUTER JOIN FollowuptHistory fh ON r.AccountId = fh.ChartOfAccountId "
            + "LEFT JOIN ChartOfAccountMultiLingo L ON r.AccountId = L.ChartOfAccountId AND L.MultiLanguagesId = :languageId "
            + "LEFT JOIN AccountsCustomGroups cg ON r.AccountId = cg.ChartOfAccountId "
            + "INNER JOIN Company cmp ON cmp.Id = :companyId "
            + "LEFT JOIN Branches br ON r.BranchesId = br.Id "
            + "WHERE (:balanceFrom IS NULL OR r.ClDebit >= :balanceFrom) "
            + "AND (:balanceTo IS NULL OR r.ClDebit <= :balanceTo) "
            + "AND (:cityId IS NULL OR r.CityId = :cityId) "
            + "AND (:customGroupId IS NULL OR cg.AcLookUpsId = :customGroupId) "
            + "AND (:customerGroupIds IS NULL OR r.CustomerGroupId IN (SELECT data FROM dbo.fnSplitString(:customerGroupIds, ','))) "
            + "AND (:parentAccountCode IS NULL OR c.ParentCodeId IN (SELECT data FROM dbo.fnSplitString(:parentAccountCode, ','))) "
            + "AND ((:showOnlyTrade = 1 AND (dr.DebitAmount > 0) OR :showOnlyTrade IS NULL)) "
            + ") "
            + "SELECT r.AccountId, r.Id, r.AccountCode, r.AccountTitle, r.ClassName, r.AccountType, r.ParentAccount, "
            + "r.Opening, r.ObDebit, r.ObCredit, r.CurrDebit, r.CurrCredit, r.ClDebit, "
            + "r.LastBillDate, r.LastBillsAmount, r.BillDays, r.LastRcvdDate, r.LastRcvdAmount, r.RcvdDays, "
            + "r.CityName, r.MobilePersonal, r.Title, r.FollowupDate, r.PromiseDate, r.CompLogoImage, r.DecimalPointsAmount, "
            + "r.BranchesId, r.BranchName "
            + "FROM TmpResult r CROSS JOIN AccStatus st "
            + "WHERE ( "
            + "(st.AccountStatus = 1 AND NOT EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts t WHERE t.ChartofAccountId = r.AccountId AND UserId = :userId)) "
            + "OR (st.AccountStatus = 2 AND EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts t WHERE t.ChartofAccountId = r.AccountId AND UserId = :userId)) "
            + "OR (st.AccountStatus = 0) ) "
            + "AND ( (ISNULL(:skipZero, 0) = 1 AND (r.ClDebit > 0)) OR (ISNULL(:skipZero, 0) = 0) ) "
            + "AND (:showAssetLiability IS NULL OR r.AccountClass = :showAssetLiability)",
            nativeQuery = true)
    List<ReceivablesRptRow> receivablesRpt(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate,
            @Param("balanceFrom") Double balanceFrom,
            @Param("balanceTo") Double balanceTo,
            @Param("cityId") Integer cityId,
            @Param("userId") Integer userId,
            @Param("parentAccountCode") String parentAccountCode,
            @Param("languageId") Integer languageId,
            @Param("showOnlyTrade") Integer showOnlyTrade,
            @Param("customGroupId") Integer customGroupId,
            @Param("customerGroupIds") String customerGroupIds,
            @Param("skipZero") Integer skipZero,
            @Param("branchesIds") String branchesIds,
            @Param("showAssetLiability") Integer showAssetLiability);

    /**
     * Sp_Accounts_Receivables_Rpt -- KEEP-EXEC fallback: the original procedure call with all 18 parameters
     * (@FinancialYearId and @AccountType are accepted but unused by the procedure). Same projection.
     */
    @Query(value = "EXEC dbo.Sp_Accounts_Receivables_Rpt "
            + "@OrganizationId = :organizationId, @CompanyId = :companyId, @FinancialYearId = :financialYearId, "
            + "@FromDate = :fromDate, @ToDate = :toDate, @BalanceFrom = :balanceFrom, @BalanceTo = :balanceTo, "
            + "@AccountType = :accountType, @CityId = :cityId, @UserId = :userId, @ParentAccountCode = :parentAccountCode, "
            + "@LanguageId = :languageId, @ShowOnlyTrade = :showOnlyTrade, @CustomGroupId = :customGroupId, "
            + "@CustomerGroupIds = :customerGroupIds, @SkipZero = :skipZero, @BranchesIds = :branchesIds, "
            + "@ShowAssetLiability = :showAssetLiability",
            nativeQuery = true)
    List<ReceivablesRptRow> receivablesRptExec(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("financialYearId") Integer financialYearId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate,
            @Param("balanceFrom") Double balanceFrom,
            @Param("balanceTo") Double balanceTo,
            @Param("accountType") Integer accountType,
            @Param("cityId") Integer cityId,
            @Param("userId") Integer userId,
            @Param("parentAccountCode") String parentAccountCode,
            @Param("languageId") Integer languageId,
            @Param("showOnlyTrade") Integer showOnlyTrade,
            @Param("customGroupId") Integer customGroupId,
            @Param("customerGroupIds") String customerGroupIds,
            @Param("skipZero") Integer skipZero,
            @Param("branchesIds") String branchesIds,
            @Param("showAssetLiability") Integer showAssetLiability);

    // ------------------------------------------------------------------ Usp_ReceivablesByDueDates

    /**
     * Usp_ReceivablesByDueDates (single branch): per detail account (AccountTypeId 3, class @AccouuntClassId)
     * the opening + voucher totals up to @ToDate split into NotYetDue (DueDate > @ToDate) and
     * OverDueReceivables (DueDate <= @ToDate; opening rows carry DueDate 1900-01-01), capped by the closing
     * balance exactly like the procedure's four UPDATE passes (Step1..Step3 CTEs), with window totals and
     * company header data; only accounts with Closing > 0.99.
     * Native CTE rewrite of #AllData, #TMP_Result (TmpRows), #TmpResult (Net), #TmpNotYetDue,
     * #TmpOverDueReceivables and the UPDATEs. The temp tables are FLOAT typed, reproduced with CASTs;
     * the proc's time literal '12:00:00 AM' is written as CAST('1900-01-01' AS DATETIME) (same value).
     * @ActionId is accepted but unused by the procedure; @FromDate NULL = no lower voucher-date bound.
     */
    @Query(value = "WITH AccStatus AS ( "
            + "SELECT CASE WHEN EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts WHERE UserId = :userId) "
            + "THEN (SELECT TOP 1 AccountStatus FROM TmpShowHideStatusWithAccounts WHERE UserId = :userId) ELSE 0 END AS AccountStatus "
            + "), AllData AS ( "
            + "SELECT CAST('1900-01-01' AS DATE) AS VoucherDate, CAST('1900-01-01' AS DATETIME) AS VoucherTime, CAST('1900-01-01' AS DATE) AS DueDate, "
            + "CAST('0OBV' AS NVARCHAR(50)) AS VoucherType, 0 AS VoucherNo, o.ChartOfAccountId AS AccountId, o.ChartOfAccountId AS OfAccountId, "
            + "0 AS SubsidiaryAccountId, CONVERT(NVARCHAR(MAX), '') AS SubsidiaryAccountTitle, CAST(0 AS FLOAT) AS Opening, "
            + "CAST(ISNULL(o.YearObDebit, 0) AS FLOAT) AS DebitAmount, CAST(ISNULL(o.YearObCredit, 0) AS FLOAT) AS CreditAmount, CAST(0 AS FLOAT) AS Balance, "
            + "0 AS Id, 0 AS DocumentTypeId, 0 AS DocumentTypeSrNo, CAST('Opening Balance' AS NVARCHAR(MAX)) AS Remarks, CAST(1 AS BIT) AS IsApproved "
            + "FROM AccountsOpeningBalances o INNER JOIN ChartofAccount c ON o.ChartOfAccountId = c.Id "
            + "WHERE c.OrganizationId = :organizationId AND c.CompanyId = :companyId AND c.AccountTypeId IN (3) "
            + "AND (:accouuntClassId IS NULL OR c.AccountClass = :accouuntClassId) "
            + "UNION ALL "
            + "SELECT CAST(vh.VoucherDate AS DATE), CAST(CAST(vh.EntryDate AS TIME) AS DATETIME), CAST(vh.DueDate AS DATE), "
            + "CAST(dt.DocumentTypeCode AS NVARCHAR(50)), vh.VoucherCode, vd.AccountId, vd.AgainstAccountId, "
            + "0, CONVERT(NVARCHAR(MAX), ''), CAST(0 AS FLOAT), "
            + "CAST(ISNULL(vd.DebitAmount, 0) AS FLOAT), CAST(ISNULL(vd.CreditAmount, 0) AS FLOAT), CAST(0 AS FLOAT), "
            + "vh.Id, vh.DocumentTypeId, vh.DocumentTypeSrNo, CAST(vd.Comments AS NVARCHAR(MAX)), vh.IsApproved "
            + "FROM VoucherHead vh "
            + "INNER JOIN VoucherDetail vd ON vh.Id = vd.VoucherHeadId "
            + "INNER JOIN DocumentType dt ON vh.DocumentTypeId = dt.Id "
            + "INNER JOIN ChartofAccount c ON vd.AccountId = c.Id "
            + "WHERE vh.OrganizationId = :organizationId AND vh.CompanyId = :companyId "
            + "AND (:fromDate IS NULL OR CAST(vh.VoucherDate AS DATE) >= CAST(:fromDate AS DATE)) "
            + "AND CAST(vh.VoucherDate AS DATE) <= CAST(:toDate AS DATE) "
            + "AND (:accouuntClassId IS NULL OR c.AccountClass = :accouuntClassId) "
            + "AND c.AccountTypeId IN (3) "
            + "), TmpRows AS ( "
            + "SELECT ROW_NUMBER() OVER (PARTITION BY T.AccountId ORDER BY T.AccountId, T.VoucherDate, T.VoucherTime) AS Rno, "
            + "T.VoucherDate, T.VoucherTime, CASE WHEN T.DueDate IS NULL THEN T.VoucherDate ELSE T.DueDate END AS DueDate, "
            + "T.VoucherType, T.VoucherNo, T.AccountId, T.OfAccountId, T.DocumentTypeId, T.DocumentTypeSrNo, "
            + "CAST('' AS VARCHAR(MAX)) AS Remarks, CAST(1 AS BIT) AS IsApproved, "
            + "T.DebitAmount - T.CreditAmount AS Opening, T.DebitAmount AS DebitAmount, T.CreditAmount AS CreditAmount, "
            + "CAST(c.AccountTitle AS VARCHAR(MAX)) AS AccountTitle, CAST(c.AccountCode AS VARCHAR(150)) AS AccountCode, "
            + "T.SubsidiaryAccountId, T.SubsidiaryAccountTitle, c.ParentCodeId AS ParentAccountId, c.AccountTypeId "
            + "FROM AllData T INNER JOIN ChartofAccount c ON T.AccountId = c.Id "
            + "), Net AS ( "
            + "SELECT a.AccountId, CAST(a.AccountCode AS NVARCHAR(250)) AS AccountCode, CAST(a.AccountTitle AS NVARCHAR(MAX)) AS AccountTitle, "
            + "SUM(a.Opening) AS Opening, SUM(a.DebitAmount) AS Debit, SUM(a.CreditAmount) AS Credit, "
            + "SUM(a.DebitAmount) - SUM(a.CreditAmount) AS Closing, a.ParentAccountId, a.AccountTypeId "
            + "FROM TmpRows a GROUP BY a.AccountId, a.AccountCode, a.AccountTitle, a.ParentAccountId, a.AccountTypeId "
            + "), NotYetDueT AS ( "
            + "SELECT a.AccountId, SUM(a.DebitAmount) - SUM(a.CreditAmount) AS Closing "
            + "FROM TmpRows a WHERE a.DueDate > :toDate "
            + "GROUP BY a.AccountId, a.AccountCode, a.AccountTitle "
            + "), OverDueT AS ( "
            + "SELECT a.AccountId, SUM(a.DebitAmount) - SUM(a.CreditAmount) AS Closing "
            + "FROM TmpRows a WHERE CAST(a.DueDate AS DATE) <= CAST(:toDate AS DATE) "
            + "GROUP BY a.AccountId, a.AccountCode, a.AccountTitle "
            + "), Step1 AS ( "
            + "SELECT n.AccountId, n.AccountCode, n.AccountTitle, n.Opening, n.Debit, n.Credit, n.Closing, n.ParentAccountId, n.AccountTypeId, "
            + "CASE WHEN o.Closing > 0 THEN o.Closing ELSE CAST(0 AS FLOAT) END AS OverDueReceivables, "
            + "CASE WHEN y.Closing > 0 THEN y.Closing ELSE CAST(0 AS FLOAT) END AS NotYetDue "
            + "FROM Net n LEFT JOIN OverDueT o ON n.AccountId = o.AccountId LEFT JOIN NotYetDueT y ON n.AccountId = y.AccountId "
            + "), Step2 AS ( "
            + "SELECT s.AccountId, s.AccountCode, s.AccountTitle, s.Opening, s.Debit, s.Credit, s.Closing, s.ParentAccountId, s.AccountTypeId, s.NotYetDue, "
            + "CASE WHEN s.OverDueReceivables > 0 AND s.Closing >= s.OverDueReceivables THEN s.OverDueReceivables ELSE CAST(0 AS FLOAT) END AS OverDueReceivables "
            + "FROM Step1 s "
            + "), Step3 AS ( "
            + "SELECT s.AccountId, s.AccountCode, s.AccountTitle, s.Opening, s.Debit, s.Credit, s.Closing, s.ParentAccountId, s.AccountTypeId, s.OverDueReceivables, "
            + "CASE WHEN s.NotYetDue <> 0 THEN "
            + "CASE WHEN s.NotYetDue > 0 AND (s.Closing - s.OverDueReceivables) > s.NotYetDue THEN s.NotYetDue ELSE s.Closing - s.OverDueReceivables END "
            + "ELSE s.NotYetDue END AS NotYetDue "
            + "FROM Step2 s "
            + ") "
            + "SELECT R.AccountId, R.AccountCode, R.AccountTitle, PA.AccountTitle AS ParentAccount, ATP.Abbreviation AS AccountType, "
            + "R.Opening, R.Debit, R.Credit, R.Closing, R.NotYetDue, R.OverDueReceivables, "
            + "SUM(R.Closing) OVER() AS NetReceivablesTotal, "
            + "SUM(R.NotYetDue) OVER() AS NotYetDueReceivablesTotal, "
            + "SUM(R.OverDueReceivables) OVER() AS OverDueReceivablesTotal, "
            + "co.CompCountry, co.CompContactPerson, co.CompMobileA, co.CompMobileB, co.CompMobileC, co.CompEmailA, co.CompEmailB, co.CompLogoImage, "
            + "co.CompanyWebsite, co.CompanyFaxNo, org.ReportingRemarks AS OrgReportingRemarks "
            + "FROM Step3 R "
            + "CROSS JOIN AccStatus st "
            + "INNER JOIN Company co ON co.Id = :companyId "
            + "INNER JOIN Organization org ON org.Id = co.OrgCompanyTypeId "
            + "INNER JOIN ChartofAccount PA ON R.ParentAccountId = PA.Id "
            + "INNER JOIN AccountTypes ATP ON R.AccountTypeId = ATP.Id "
            + "LEFT JOIN AccountsCustomGroups CG ON CG.ChartOfAccountId = R.AccountId "
            + "WHERE ( "
            + "(st.AccountStatus = 1 AND NOT EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts t WHERE t.ChartofAccountId = R.AccountId AND UserId = :userId)) "
            + "OR (st.AccountStatus = 2 AND EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts t WHERE t.ChartofAccountId = R.AccountId AND UserId = :userId)) "
            + "OR (st.AccountStatus = 0) ) "
            + "AND R.Closing > 0.99 "
            + "AND (:parentAccountId IS NULL OR R.ParentAccountId = :parentAccountId) "
            + "AND (:customGroupId IS NULL OR CG.AcLookUpsId = :customGroupId)",
            nativeQuery = true)
    List<ReceivablesByDueDatesRow> receivablesByDueDates(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate,
            @Param("accouuntClassId") Integer accouuntClassId,
            @Param("customGroupId") Integer customGroupId,
            @Param("parentAccountId") Integer parentAccountId,
            @Param("userId") Integer userId);

    /**
     * Usp_ReceivablesByDueDates -- KEEP-EXEC fallback: the original procedure call with all 9 parameters
     * (@ActionId is accepted but unused by the procedure). Same projection.
     */
    @Query(value = "EXEC dbo.Usp_ReceivablesByDueDates "
            + "@OrganizationId = :organizationId, @CompanyId = :companyId, @FromDate = :fromDate, @ToDate = :toDate, "
            + "@AccouuntClassId = :accouuntClassId, @CustomGroupId = :customGroupId, @ParentAccountId = :parentAccountId, "
            + "@ActionId = :actionId, @UserId = :userId",
            nativeQuery = true)
    List<ReceivablesByDueDatesRow> receivablesByDueDatesExec(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate,
            @Param("accouuntClassId") Integer accouuntClassId,
            @Param("customGroupId") Integer customGroupId,
            @Param("parentAccountId") Integer parentAccountId,
            @Param("actionId") Integer actionId,
            @Param("userId") Integer userId);
}
