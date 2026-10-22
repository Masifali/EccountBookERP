package com.mst.repositories;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.ChartofAccount;
import com.mst.projections.GeneralLedgerRptRow;
import com.mst.projections.TradeDebtorsCreditorsRow;

/**
 * Query-only repository (bound to the existing ChartofAccount entity) for the two ledger-style account
 * reports of batch M2: SpAccounts_TradeDebtorsAndCreditors_Report and Sp_Accounts_GeneralLedger_Rpt.
 * Both are kept as procedure calls (KEEP-EXEC) because their pipelines branch on runtime state
 * (feature flag function, configuration function, RAISERROR validation, per-user show/hide table) and
 * end in paged result sets; see the method Javadoc for the exact reasons.
 */
@Repository
public interface AccountsLedgerReportQueryRepository extends org.springframework.data.repository.Repository<ChartofAccount, Integer> {

    /**
     * SpAccounts_TradeDebtorsAndCreditors_Report (single branch) -- FIRST result set: one row per detail
     * account (or per subsidiary party when ERP feature 4 is on) with Opening / CurrDebit / CurrCredit /
     * Closing, last bill / last received(paid) date and amount, party data, window totals and paging.
     * KEEP-EXEC: RAISERROR validations (@UserId 0, @AppId 5 without @CostCenterId), @AppId 4/6 resolves
     * @CoaDetailAccountId from UserAccount, dbo.fnCheckERPFeaturesExistorNot decides between the subsidiary
     * (GetSubsidiaryVoucherData table function, DELETE/INSERT into #Final) and the account-wise pipeline,
     * RECEIVABLES/PAYABLES branches with different document-type sets, STRING_AGG bank names,
     * TmpShowHideStatusWithAccounts per-user state, OFFSET/FETCH paging and a second result set.
     * JPA returns the first result set only; the second one (Company.CompLogoImage) is exposed by
     * {@link #companyLogo(Integer)}.
     * Procedure defaults applied when NULL is passed: @LanguageId -> 0, @PageNumber <= 0 -> 1,
     * @PageSize <= 0 -> 1, both NULL -> no paging (1000000 rows); @ReprotTypeId 1 = balance wise
     * (AccountTypeIds 3,6,8,16,17,18, classes 2,3, @ReportType 'RECEIVABLES' -> @ActionId 2 /
     * 'PAYABLES' -> @ActionId 1), otherwise AccountTypeId 3 and class @AccouuntClassId.
     * @ActionId 1 = only credit balances, 2 = only debit balances; @TradeTypeId 1 purchase parties,
     * 2 sale parties; @IsApproved NULL = include unposted vouchers.
     */
    @Query(value = "EXEC dbo.SpAccounts_TradeDebtorsAndCreditors_Report "
            + "@OrganizationId = :organizationId, @CompanyId = :companyId, @CompanyIdCoa = :companyIdCoa, "
            + "@FinancialYearId = :financialYearId, @FromDate = :fromDate, @ToDate = :toDate, "
            + "@AccouuntClassId = :accouuntClassId, @BalanceFrom = :balanceFrom, @BalanceTo = :balanceTo, "
            + "@CityId = :cityId, @ParentAccountCode = :parentAccountCode, @LanguageId = :languageId, "
            + "@CustomGroupId = :customGroupId, @CustomerGroupIds = :customerGroupIds, @ActionId = :actionId, "
            + "@UserId = :userId, @IsApproved = :isApproved, @TradeTypeId = :tradeTypeId, "
            + "@CoaDetailAccountId = :coaDetailAccountId, @SubsidoryAccountId = :subsidoryAccountId, "
            + "@PageSize = :pageSize, @PageNumber = :pageNumber, @BranchesIds = :branchesIds, @AppId = :appId, "
            + "@CostCenterId = :costCenterId, @ReprotTypeId = :reprotTypeId, @ReportType = :reportType",
            nativeQuery = true)
    List<TradeDebtorsCreditorsRow> tradeDebtorsAndCreditorsReport(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("companyIdCoa") Integer companyIdCoa,
            @Param("financialYearId") Integer financialYearId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate,
            @Param("accouuntClassId") Integer accouuntClassId,
            @Param("balanceFrom") Double balanceFrom,
            @Param("balanceTo") Double balanceTo,
            @Param("cityId") Integer cityId,
            @Param("parentAccountCode") String parentAccountCode,
            @Param("languageId") Integer languageId,
            @Param("customGroupId") Integer customGroupId,
            @Param("customerGroupIds") String customerGroupIds,
            @Param("actionId") Integer actionId,
            @Param("userId") Integer userId,
            @Param("isApproved") Boolean isApproved,
            @Param("tradeTypeId") Integer tradeTypeId,
            @Param("coaDetailAccountId") Integer coaDetailAccountId,
            @Param("subsidoryAccountId") Integer subsidoryAccountId,
            @Param("pageSize") Integer pageSize,
            @Param("pageNumber") Integer pageNumber,
            @Param("branchesIds") String branchesIds,
            @Param("appId") Integer appId,
            @Param("costCenterId") Integer costCenterId,
            @Param("reprotTypeId") Integer reprotTypeId,
            @Param("reportType") String reportType);

    /**
     * SpAccounts_TradeDebtorsAndCreditors_Report -- SECOND result set
     * (SELECT C.CompLogoImage FROM Company C WHERE C.Id = @CompanyId), as its own statement.
     * Native because the existing Company entity does not map CompLogoImage. Take first.
     */
    @Query(value = "SELECT C.CompLogoImage FROM Company C WHERE C.Id = :companyId", nativeQuery = true)
    List<byte[]> companyLogo(@Param("companyId") Integer companyId);

    /**
     * Sp_Accounts_GeneralLedger_Rpt (single branch): general ledger of one account (or an @AccountIds list)
     * between @FromDate and @EndDate with an opening row ('OPB'), running Balance, subsidiary titles,
     * attachment counts, stock-reserved status and company / party header data, paged.
     * KEEP-EXEC: dbo.GetConfigurationByConfigId(.., 315) switches the stock-reserved UPDATE block on,
     * tables without entities (genEmployee, SubsidiaryAccountsDefine, ChartOfAccountMultiLingo,
     * AttachmentsHistory, InvSaleInvoice, InvDeliveryOrder*, InvGrnPartyProcessingDetail, MultiCurrency,
     * FinancialYear), TRY_CAST / fnSplitString account list, five scalar pre-queries (@OpeningBalance,
     * @ClosingBalance via TOP 1 ... ORDER BY RecNo, post-dated cheques), eight UPDATE passes over
     * #GeneralLedger and OFFSET/FETCH paging.
     * Procedure defaults when NULL is passed: @PageNumber -> 1, @PageSize -> 1000000; @IsApproved NULL
     * = include unposted vouchers; @LanguageId > 0 switches to ChartOfAccountMultiLingo titles.
     */
    @Query(value = "EXEC dbo.Sp_Accounts_GeneralLedger_Rpt "
            + "@OrganizationId = :organizationId, @CompanyID = :companyId, @BranchesId = :branchesId, "
            + "@ProjectsId = :projectsId, @FinancialYearId = :financialYearId, @FromDate = :fromDate, "
            + "@EndDate = :endDate, @AccountId = :accountId, @IsApproved = :isApproved, @EntryUser = :entryUser, "
            + "@LanguageId = :languageId, @SubsidiaryAccountId = :subsidiaryAccountId, "
            + "@SubsidiaryTypeId = :subsidiaryTypeId, @AccountIds = :accountIds, @PageSize = :pageSize, "
            + "@PageNumber = :pageNumber, @CostCenterId = :costCenterId",
            nativeQuery = true)
    List<GeneralLedgerRptRow> generalLedgerRpt(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("projectsId") Integer projectsId,
            @Param("financialYearId") Integer financialYearId,
            @Param("fromDate") LocalDate fromDate,
            @Param("endDate") LocalDate endDate,
            @Param("accountId") Integer accountId,
            @Param("isApproved") Boolean isApproved,
            @Param("entryUser") Integer entryUser,
            @Param("languageId") Integer languageId,
            @Param("subsidiaryAccountId") Integer subsidiaryAccountId,
            @Param("subsidiaryTypeId") Integer subsidiaryTypeId,
            @Param("accountIds") String accountIds,
            @Param("pageSize") Integer pageSize,
            @Param("pageNumber") Integer pageNumber,
            @Param("costCenterId") Integer costCenterId);
}
