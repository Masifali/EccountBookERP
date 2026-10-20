package com.mst.repositories;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.List;

import javax.persistence.Tuple;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.ChartofAccount;
import com.mst.projections.PayablesAgingDetailRow;
import com.mst.projections.PayablesAgingSummaryRow;

/**
 * UPS_PayablesAging_New (supplier / liabilities aging, AccountTypeId = 3). Query-only repository bound to the
 * existing ChartofAccount entity. Both branches are kept as procedure calls (see KEEP-EXEC notes).
 * Procedure defaults applied inside the proc when NULL is passed: @PageNumber -> 1, @PageSize -> 1000000,
 * @LanguageId -> 0, @BalanceFrom -> 0, @AccouuntClassId -> 3, @AgingDays -> 30, @AsOnDate -> today,
 * @TopSuppliers -> 5. @StartDate = @AsOnDate - 4 * @AgingDays.
 */
@Repository
public interface PayablesAgingQueryRepository extends org.springframework.data.repository.Repository<ChartofAccount, Integer> {

    /**
     * UPS_PayablesAging_New @Activity='Detail' -- first result set: one row per account/branch with the
     * aging buckets, totals and percentages, filtered by the grid filters and paged
     * (OFFSET (@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize).
     * KEEP-EXEC: dynamic SQL (sp_executesql with per-call column aliases, grid filters and sort column
     * concatenated into the statement), ten+ temp tables with UPDATEs, STRING_SPLIT / fnSplitString,
     * TmpShowHideStatusWithAccounts per-user state and multiple result sets -- not expressible as one statement.
     * JPA returns only the FIRST result set of the procedure. The later sets of this branch
     * (Opening/Debit/Credit/Balance/Increase totals, Company.CompLogoImage, OverDue invoices, NotYetDue invoices) are not reachable through this method; the existing JdbcTemplate-based
     * PayablesAgingRepository.load(...) keeps returning every set.
     * Parameters (all procedure parameters of this branch):
     *   - @OrganizationId (required)
     *   - @CompanyId (required)
     *   - @AsOnDate (default NULL -> today)
     *   - @AgingDays (default 0/NULL -> 30)
     *   - @UserId (required)
     *   - @AccouuntClassId (default NULL -> 3 (liabilities))
     *   - @PageSize (default NULL -> 1000000)
     *   - @PageNumber (default NULL -> 1)
     *   - @CustomGroupId (default NULL)
     *   - @ControlAccountIds (comma list of ParentCodeId, default NULL)
     *   - @AccountId (default NULL)
     *   - @Interval (1..4, default NULL)
     *   - @BranchesIds (comma list of branch ids, default NULL)
     *   - @TopSuppliers (default NULL -> 5 (Summary only))
     *   - @ReportId (0 all | 1 overdue | 2 not-yet-due, default NULL)
     *   - @LanguageId (default NULL -> 0)
     *   - @BalanceFrom (default NULL -> 0)
     *   - @AccountCode (grid filter, default NULL)
     *   - @AccountTitle (grid filter, default NULL)
     *   - @ParentAccountTitle (grid filter, default NULL)
     *   - @AccountType (grid filter, default NULL)
     *   - @Opening (grid filter, default NULL)
     *   - @IstInterval (grid filter, default NULL)
     *   - @ScnInterval (grid filter, default NULL)
     *   - @TrdIntarval (grid filter, default NULL)
     *   - @Above (grid filter, default NULL)
     *   - @Closing (grid filter, default NULL)
     *   - @CurrentBalance (grid filter, default NULL)
     *   - @OverDue (grid filter, default NULL)
     *   - @NotYetDue (grid filter, default NULL)
     *   - @StartDateOperator (SQL operator for the StartDate filter, default NULL)
     *   - @AsOnDateOperator (SQL operator for the AsOnDate filter, default NULL)
     *   - @AccountCodeOperator (SQL operator, default NULL)
     *   - @AcTitleFT (Contains|BeginsWith|EndsWith|Equals|NotEquals, default NULL)
     *   - @ParentAcTitleFT (Contains|BeginsWith|EndsWith|Equals|NotEquals, default NULL)
     *   - @AcTypeFT (Contains|BeginsWith|EndsWith|Equals|NotEquals, default NULL)
     *   - @OpeningOperator (SQL operator, default NULL)
     *   - @IstIntervalOperator (SQL operator, default NULL)
     *   - @ScnIntervalOperator (SQL operator, default NULL)
     *   - @TrdIntarvalOperator (SQL operator, default NULL)
     *   - @AboveOperator (SQL operator, default NULL)
     *   - @ClosingOperator (SQL operator, default NULL)
     *   - @CurrentBalanceOperator (SQL operator, default NULL)
     *   - @OverDueOperator (SQL operator, default NULL)
     *   - @NotYetDueOperator (SQL operator, default NULL)
     *   - @SortType (ASC|DESC, default NULL)
     *   - @SortingCol (sort column name, default NULL (-> AccountId ASC))
     */
    @Query(value =
           "EXEC dbo.UPS_PayablesAging_New @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@AsOnDate = :asOnDate, @AgingDays = :agingDays, @UserId = :userId, "
         + "@AccouuntClassId = :accouuntClassId, @Activity = 'Detail', @PageSize = :pageSize, "
         + "@PageNumber = :pageNumber, @CustomGroupId = :customGroupId, "
         + "@ControlAccountIds = :controlAccountIds, @AccountId = :accountId, @Interval = :interval, "
         + "@BranchesIds = :branchesIds, @TopSuppliers = :topSuppliers, @ReportId = :reportId, "
         + "@LanguageId = :languageId, @BalanceFrom = :balanceFrom, @AccountCode = :accountCode, "
         + "@AccountTitle = :accountTitle, @ParentAccountTitle = :parentAccountTitle, "
         + "@AccountType = :accountType, @Opening = :opening, @IstInterval = :istInterval, "
         + "@ScnInterval = :scnInterval, @TrdIntarval = :trdIntarval, @Above = :above, @Closing = :closing, "
         + "@CurrentBalance = :currentBalance, @OverDue = :overDue, @NotYetDue = :notYetDue, "
         + "@StartDateOperator = :startDateOperator, @AsOnDateOperator = :asOnDateOperator, "
         + "@AccountCodeOperator = :accountCodeOperator, @AcTitleFT = :acTitleFT, "
         + "@ParentAcTitleFT = :parentAcTitleFT, @AcTypeFT = :acTypeFT, @OpeningOperator = :openingOperator, "
         + "@IstIntervalOperator = :istIntervalOperator, @ScnIntervalOperator = :scnIntervalOperator, "
         + "@TrdIntarvalOperator = :trdIntarvalOperator, @AboveOperator = :aboveOperator, "
         + "@ClosingOperator = :closingOperator, @CurrentBalanceOperator = :currentBalanceOperator, "
         + "@OverDueOperator = :overDueOperator, @NotYetDueOperator = :notYetDueOperator, "
         + "@SortType = :sortType, @SortingCol = :sortingCol",
           nativeQuery = true)
    List<PayablesAgingDetailRow> payablesAgingDetail(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("asOnDate") LocalDate asOnDate,
            @Param("agingDays") Integer agingDays,
            @Param("userId") Integer userId,
            @Param("accouuntClassId") Integer accouuntClassId,
            @Param("pageSize") Integer pageSize,
            @Param("pageNumber") Integer pageNumber,
            @Param("customGroupId") Integer customGroupId,
            @Param("controlAccountIds") String controlAccountIds,
            @Param("accountId") Integer accountId,
            @Param("interval") Integer interval,
            @Param("branchesIds") String branchesIds,
            @Param("topSuppliers") Integer topSuppliers,
            @Param("reportId") Integer reportId,
            @Param("languageId") Integer languageId,
            @Param("balanceFrom") BigDecimal balanceFrom,
            @Param("accountCode") Integer accountCode,
            @Param("accountTitle") String accountTitle,
            @Param("parentAccountTitle") String parentAccountTitle,
            @Param("accountType") String accountType,
            @Param("opening") Double opening,
            @Param("istInterval") Double istInterval,
            @Param("scnInterval") Double scnInterval,
            @Param("trdIntarval") Double trdIntarval,
            @Param("above") Double above,
            @Param("closing") Double closing,
            @Param("currentBalance") Double currentBalance,
            @Param("overDue") Double overDue,
            @Param("notYetDue") Double notYetDue,
            @Param("startDateOperator") String startDateOperator,
            @Param("asOnDateOperator") String asOnDateOperator,
            @Param("accountCodeOperator") String accountCodeOperator,
            @Param("acTitleFT") String acTitleFT,
            @Param("parentAcTitleFT") String parentAcTitleFT,
            @Param("acTypeFT") String acTypeFT,
            @Param("openingOperator") String openingOperator,
            @Param("istIntervalOperator") String istIntervalOperator,
            @Param("scnIntervalOperator") String scnIntervalOperator,
            @Param("trdIntarvalOperator") String trdIntarvalOperator,
            @Param("aboveOperator") String aboveOperator,
            @Param("closingOperator") String closingOperator,
            @Param("currentBalanceOperator") String currentBalanceOperator,
            @Param("overDueOperator") String overDueOperator,
            @Param("notYetDueOperator") String notYetDueOperator,
            @Param("sortType") String sortType,
            @Param("sortingCol") String sortingCol);

    /**
     * UPS_PayablesAging_New @Activity='Detail' -- same call as {@link #payablesAgingDetail} returning the raw
     * tuple so the dynamically named interval columns ([1_n], [n+1_2n], [2n+1_3n], [3n_Above]) can be read by
     * the alias held in FirstIntervalCaption .. AboveIntervalCaption. // KEEP-EXEC: see payablesAgingDetail.
     */
    @Query(value =
           "EXEC dbo.UPS_PayablesAging_New @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@AsOnDate = :asOnDate, @AgingDays = :agingDays, @UserId = :userId, "
         + "@AccouuntClassId = :accouuntClassId, @Activity = 'Detail', @PageSize = :pageSize, "
         + "@PageNumber = :pageNumber, @CustomGroupId = :customGroupId, "
         + "@ControlAccountIds = :controlAccountIds, @AccountId = :accountId, @Interval = :interval, "
         + "@BranchesIds = :branchesIds, @TopSuppliers = :topSuppliers, @ReportId = :reportId, "
         + "@LanguageId = :languageId, @BalanceFrom = :balanceFrom, @AccountCode = :accountCode, "
         + "@AccountTitle = :accountTitle, @ParentAccountTitle = :parentAccountTitle, "
         + "@AccountType = :accountType, @Opening = :opening, @IstInterval = :istInterval, "
         + "@ScnInterval = :scnInterval, @TrdIntarval = :trdIntarval, @Above = :above, @Closing = :closing, "
         + "@CurrentBalance = :currentBalance, @OverDue = :overDue, @NotYetDue = :notYetDue, "
         + "@StartDateOperator = :startDateOperator, @AsOnDateOperator = :asOnDateOperator, "
         + "@AccountCodeOperator = :accountCodeOperator, @AcTitleFT = :acTitleFT, "
         + "@ParentAcTitleFT = :parentAcTitleFT, @AcTypeFT = :acTypeFT, @OpeningOperator = :openingOperator, "
         + "@IstIntervalOperator = :istIntervalOperator, @ScnIntervalOperator = :scnIntervalOperator, "
         + "@TrdIntarvalOperator = :trdIntarvalOperator, @AboveOperator = :aboveOperator, "
         + "@ClosingOperator = :closingOperator, @CurrentBalanceOperator = :currentBalanceOperator, "
         + "@OverDueOperator = :overDueOperator, @NotYetDueOperator = :notYetDueOperator, "
         + "@SortType = :sortType, @SortingCol = :sortingCol",
           nativeQuery = true)
    List<Tuple> payablesAgingDetailRaw(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("asOnDate") LocalDate asOnDate,
            @Param("agingDays") Integer agingDays,
            @Param("userId") Integer userId,
            @Param("accouuntClassId") Integer accouuntClassId,
            @Param("pageSize") Integer pageSize,
            @Param("pageNumber") Integer pageNumber,
            @Param("customGroupId") Integer customGroupId,
            @Param("controlAccountIds") String controlAccountIds,
            @Param("accountId") Integer accountId,
            @Param("interval") Integer interval,
            @Param("branchesIds") String branchesIds,
            @Param("topSuppliers") Integer topSuppliers,
            @Param("reportId") Integer reportId,
            @Param("languageId") Integer languageId,
            @Param("balanceFrom") BigDecimal balanceFrom,
            @Param("accountCode") Integer accountCode,
            @Param("accountTitle") String accountTitle,
            @Param("parentAccountTitle") String parentAccountTitle,
            @Param("accountType") String accountType,
            @Param("opening") Double opening,
            @Param("istInterval") Double istInterval,
            @Param("scnInterval") Double scnInterval,
            @Param("trdIntarval") Double trdIntarval,
            @Param("above") Double above,
            @Param("closing") Double closing,
            @Param("currentBalance") Double currentBalance,
            @Param("overDue") Double overDue,
            @Param("notYetDue") Double notYetDue,
            @Param("startDateOperator") String startDateOperator,
            @Param("asOnDateOperator") String asOnDateOperator,
            @Param("accountCodeOperator") String accountCodeOperator,
            @Param("acTitleFT") String acTitleFT,
            @Param("parentAcTitleFT") String parentAcTitleFT,
            @Param("acTypeFT") String acTypeFT,
            @Param("openingOperator") String openingOperator,
            @Param("istIntervalOperator") String istIntervalOperator,
            @Param("scnIntervalOperator") String scnIntervalOperator,
            @Param("trdIntarvalOperator") String trdIntarvalOperator,
            @Param("aboveOperator") String aboveOperator,
            @Param("closingOperator") String closingOperator,
            @Param("currentBalanceOperator") String currentBalanceOperator,
            @Param("overDueOperator") String overDueOperator,
            @Param("notYetDueOperator") String notYetDueOperator,
            @Param("sortType") String sortType,
            @Param("sortingCol") String sortingCol);

    /**
     * UPS_PayablesAging_New @Activity='Summary' -- first result set: TOP 1 totals row (interval sums, closing,
     * current payables, overdue, not-yet-due, percentages, account counters, captions).
     * KEEP-EXEC: dynamic SQL (sp_executesql with per-call column aliases, grid filters and sort column
     * concatenated into the statement), ten+ temp tables with UPDATEs, STRING_SPLIT / fnSplitString,
     * TmpShowHideStatusWithAccounts per-user state and multiple result sets -- not expressible as one statement.
     * JPA returns only the FIRST result set of the procedure. The later sets of this branch
     * (top-N suppliers by |CurrentBalance| limited by @TopSuppliers (RNo, StartDate, AsOnDate, AccountId, AccountCode, AccountTitle, EnglishAccountTitle, ParentAccount, AccountType, Closing, NotYetDue, BalPercent)) are not reachable through this method; the existing JdbcTemplate-based
     * PayablesAgingRepository keeps returning every set.
     * Grid-filter / sorting / paging parameters are not used by this branch and are not sent.
     * Parameters:
     *   - @OrganizationId (required)
     *   - @CompanyId (required)
     *   - @AsOnDate (default NULL -> today)
     *   - @AgingDays (default 0/NULL -> 30)
     *   - @UserId (required)
     *   - @AccouuntClassId (default NULL -> 3 (liabilities))
     *   - @CustomGroupId (default NULL)
     *   - @ControlAccountIds (comma list of ParentCodeId, default NULL)
     *   - @AccountId (default NULL)
     *   - @Interval (1..4, default NULL)
     *   - @BranchesIds (comma list of branch ids, default NULL)
     *   - @TopSuppliers (default NULL -> 5 (Summary only))
     *   - @ReportId (0 all | 1 overdue | 2 not-yet-due, default NULL)
     *   - @LanguageId (default NULL -> 0)
     *   - @BalanceFrom (default NULL -> 0)
     */
    @Query(value =
           "EXEC dbo.UPS_PayablesAging_New @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@AsOnDate = :asOnDate, @AgingDays = :agingDays, @UserId = :userId, "
         + "@AccouuntClassId = :accouuntClassId, @Activity = 'Summary', @CustomGroupId = :customGroupId, "
         + "@ControlAccountIds = :controlAccountIds, @AccountId = :accountId, @Interval = :interval, "
         + "@BranchesIds = :branchesIds, @TopSuppliers = :topSuppliers, @ReportId = :reportId, "
         + "@LanguageId = :languageId, @BalanceFrom = :balanceFrom",
           nativeQuery = true)
    List<PayablesAgingSummaryRow> payablesAgingSummary(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("asOnDate") LocalDate asOnDate,
            @Param("agingDays") Integer agingDays,
            @Param("userId") Integer userId,
            @Param("accouuntClassId") Integer accouuntClassId,
            @Param("customGroupId") Integer customGroupId,
            @Param("controlAccountIds") String controlAccountIds,
            @Param("accountId") Integer accountId,
            @Param("interval") Integer interval,
            @Param("branchesIds") String branchesIds,
            @Param("topSuppliers") Integer topSuppliers,
            @Param("reportId") Integer reportId,
            @Param("languageId") Integer languageId,
            @Param("balanceFrom") BigDecimal balanceFrom);

    /**
     * UPS_PayablesAging_New @Activity='Summary' -- same call as {@link #payablesAgingSummary} as a raw tuple,
     * exposing the dynamically named [1_n] .. [3n_Above] sums and the [0_n%] .. [3n_Above%] percentages.
     * // KEEP-EXEC: see payablesAgingSummary.
     */
    @Query(value =
           "EXEC dbo.UPS_PayablesAging_New @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@AsOnDate = :asOnDate, @AgingDays = :agingDays, @UserId = :userId, "
         + "@AccouuntClassId = :accouuntClassId, @Activity = 'Summary', @CustomGroupId = :customGroupId, "
         + "@ControlAccountIds = :controlAccountIds, @AccountId = :accountId, @Interval = :interval, "
         + "@BranchesIds = :branchesIds, @TopSuppliers = :topSuppliers, @ReportId = :reportId, "
         + "@LanguageId = :languageId, @BalanceFrom = :balanceFrom",
           nativeQuery = true)
    List<Tuple> payablesAgingSummaryRaw(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("asOnDate") LocalDate asOnDate,
            @Param("agingDays") Integer agingDays,
            @Param("userId") Integer userId,
            @Param("accouuntClassId") Integer accouuntClassId,
            @Param("customGroupId") Integer customGroupId,
            @Param("controlAccountIds") String controlAccountIds,
            @Param("accountId") Integer accountId,
            @Param("interval") Integer interval,
            @Param("branchesIds") String branchesIds,
            @Param("topSuppliers") Integer topSuppliers,
            @Param("reportId") Integer reportId,
            @Param("languageId") Integer languageId,
            @Param("balanceFrom") BigDecimal balanceFrom);
}
