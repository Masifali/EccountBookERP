package com.mst.repositories;

import java.time.LocalDate;
import java.util.List;

import javax.persistence.Tuple;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.ChartofAccount;
import com.mst.projections.ReceivablesAgingDetailRow;
import com.mst.projections.ReceivablesAgingSummaryRow;

/**
 * UPS_ReceivablesAging_New (customer / assets aging, AccountTypeId = 3). Query-only repository bound to the
 * existing ChartofAccount entity. Both branches are kept as procedure calls (see KEEP-EXEC notes).
 * Procedure defaults applied inside the proc when NULL is passed: @PageNumber -> 1, @PageSize -> 1000000,
 * @AccouuntClassId -> 2, @AgingDays -> 30, @AsOnDate -> today, @TopCustomers -> 5; @UserId = 0/NULL raises
 * 'User not found'. When @AccountId is NULL and @AppId in (4,6) the proc resolves the account from
 * UserAccount.SupplierCustomerId -> SupplierCustomer.GlAccountId. @StartDate = @AsOnDate - 4 * @AgingDays.
 */
@Repository
public interface ReceivablesAgingQueryRepository extends org.springframework.data.repository.Repository<ChartofAccount, Integer> {

    /**
     * UPS_ReceivablesAging_New @Activity='Detail' -- first result set: one row per account/branch with the
     * aging buckets, totals and percentages, filtered by the grid filters and paged.
     * KEEP-EXEC: dynamic SQL (sp_executesql with per-call column aliases, grid filters and sort column
     * concatenated into the statement), ten+ temp tables with UPDATEs, STRING_SPLIT / fnSplitString,
     * TmpShowHideStatusWithAccounts per-user state and multiple result sets -- not expressible as one statement.
     * JPA returns only the FIRST result set of the procedure. The later sets of this branch
     * (Opening/Debit/Credit/Balance/Increase totals, Company.CompLogoImage, OverDue invoices, NotYetDue invoices) are not reachable through this method; the existing JdbcTemplate-based
     * ReceivablesAgingRepository keeps returning every set.
     * Parameters (all procedure parameters of this branch):
     *   - @OrganizationId (required)
     *   - @CompanyId (required)
     *   - @AsOnDate (default NULL -> today)
     *   - @AgingDays (default 0/NULL -> 30)
     *   - @UserId (required (0/NULL raises 'User not found'))
     *   - @AccouuntClassId (NULL -> 2 (assets))
     *   - @CustomGroupId (default NULL)
     *   - @ParentAccountId (default NULL)
     *   - @AccountId (default NULL (derived from UserAccount.SupplierCustomerId when @AppId in 4,6))
     *   - @ReportId (0 all | 1 overdue | 2 not-yet-due, default NULL)
     *   - @Interval (1..4, default NULL)
     *   - @PageSize (default NULL -> 1000000)
     *   - @PageNumber (default NULL -> 1)
     *   - @BranchesIds (comma list of branch ids, default NULL)
     *   - @TopCustomers (default NULL -> 5 (Summary only))
     *   - @AppId (4/6 customer portals, 5 cost-centre app, default NULL)
     *   - @CostCenterId (used only when @AppId = 5, default NULL)
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
           "EXEC dbo.UPS_ReceivablesAging_New @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@AsOnDate = :asOnDate, @AgingDays = :agingDays, @UserId = :userId, "
         + "@AccouuntClassId = :accouuntClassId, @Activity = 'Detail', @CustomGroupId = :customGroupId, "
         + "@ParentAccountId = :parentAccountId, @AccountId = :accountId, @ReportId = :reportId, "
         + "@Interval = :interval, @PageSize = :pageSize, @PageNumber = :pageNumber, "
         + "@BranchesIds = :branchesIds, @TopCustomers = :topCustomers, @AppId = :appId, "
         + "@CostCenterId = :costCenterId, @AccountCode = :accountCode, @AccountTitle = :accountTitle, "
         + "@ParentAccountTitle = :parentAccountTitle, @AccountType = :accountType, @Opening = :opening, "
         + "@IstInterval = :istInterval, @ScnInterval = :scnInterval, @TrdIntarval = :trdIntarval, "
         + "@Above = :above, @Closing = :closing, @CurrentBalance = :currentBalance, @OverDue = :overDue, "
         + "@NotYetDue = :notYetDue, @StartDateOperator = :startDateOperator, "
         + "@AsOnDateOperator = :asOnDateOperator, @AccountCodeOperator = :accountCodeOperator, "
         + "@AcTitleFT = :acTitleFT, @ParentAcTitleFT = :parentAcTitleFT, @AcTypeFT = :acTypeFT, "
         + "@OpeningOperator = :openingOperator, @IstIntervalOperator = :istIntervalOperator, "
         + "@ScnIntervalOperator = :scnIntervalOperator, @TrdIntarvalOperator = :trdIntarvalOperator, "
         + "@AboveOperator = :aboveOperator, @ClosingOperator = :closingOperator, "
         + "@CurrentBalanceOperator = :currentBalanceOperator, @OverDueOperator = :overDueOperator, "
         + "@NotYetDueOperator = :notYetDueOperator, @SortType = :sortType, @SortingCol = :sortingCol",
           nativeQuery = true)
    List<ReceivablesAgingDetailRow> receivablesAgingDetail(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("asOnDate") LocalDate asOnDate,
            @Param("agingDays") Integer agingDays,
            @Param("userId") Integer userId,
            @Param("accouuntClassId") Integer accouuntClassId,
            @Param("customGroupId") Integer customGroupId,
            @Param("parentAccountId") Integer parentAccountId,
            @Param("accountId") Integer accountId,
            @Param("reportId") Integer reportId,
            @Param("interval") Integer interval,
            @Param("pageSize") Integer pageSize,
            @Param("pageNumber") Integer pageNumber,
            @Param("branchesIds") String branchesIds,
            @Param("topCustomers") Integer topCustomers,
            @Param("appId") Integer appId,
            @Param("costCenterId") Integer costCenterId,
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
     * UPS_ReceivablesAging_New @Activity='Detail' -- same call as {@link #receivablesAgingDetail} returning the
     * raw tuple so the dynamically named interval columns can be read by the alias held in the caption columns.
     * // KEEP-EXEC: see receivablesAgingDetail.
     */
    @Query(value =
           "EXEC dbo.UPS_ReceivablesAging_New @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@AsOnDate = :asOnDate, @AgingDays = :agingDays, @UserId = :userId, "
         + "@AccouuntClassId = :accouuntClassId, @Activity = 'Detail', @CustomGroupId = :customGroupId, "
         + "@ParentAccountId = :parentAccountId, @AccountId = :accountId, @ReportId = :reportId, "
         + "@Interval = :interval, @PageSize = :pageSize, @PageNumber = :pageNumber, "
         + "@BranchesIds = :branchesIds, @TopCustomers = :topCustomers, @AppId = :appId, "
         + "@CostCenterId = :costCenterId, @AccountCode = :accountCode, @AccountTitle = :accountTitle, "
         + "@ParentAccountTitle = :parentAccountTitle, @AccountType = :accountType, @Opening = :opening, "
         + "@IstInterval = :istInterval, @ScnInterval = :scnInterval, @TrdIntarval = :trdIntarval, "
         + "@Above = :above, @Closing = :closing, @CurrentBalance = :currentBalance, @OverDue = :overDue, "
         + "@NotYetDue = :notYetDue, @StartDateOperator = :startDateOperator, "
         + "@AsOnDateOperator = :asOnDateOperator, @AccountCodeOperator = :accountCodeOperator, "
         + "@AcTitleFT = :acTitleFT, @ParentAcTitleFT = :parentAcTitleFT, @AcTypeFT = :acTypeFT, "
         + "@OpeningOperator = :openingOperator, @IstIntervalOperator = :istIntervalOperator, "
         + "@ScnIntervalOperator = :scnIntervalOperator, @TrdIntarvalOperator = :trdIntarvalOperator, "
         + "@AboveOperator = :aboveOperator, @ClosingOperator = :closingOperator, "
         + "@CurrentBalanceOperator = :currentBalanceOperator, @OverDueOperator = :overDueOperator, "
         + "@NotYetDueOperator = :notYetDueOperator, @SortType = :sortType, @SortingCol = :sortingCol",
           nativeQuery = true)
    List<Tuple> receivablesAgingDetailRaw(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("asOnDate") LocalDate asOnDate,
            @Param("agingDays") Integer agingDays,
            @Param("userId") Integer userId,
            @Param("accouuntClassId") Integer accouuntClassId,
            @Param("customGroupId") Integer customGroupId,
            @Param("parentAccountId") Integer parentAccountId,
            @Param("accountId") Integer accountId,
            @Param("reportId") Integer reportId,
            @Param("interval") Integer interval,
            @Param("pageSize") Integer pageSize,
            @Param("pageNumber") Integer pageNumber,
            @Param("branchesIds") String branchesIds,
            @Param("topCustomers") Integer topCustomers,
            @Param("appId") Integer appId,
            @Param("costCenterId") Integer costCenterId,
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
     * UPS_ReceivablesAging_New @Activity='Summary' -- first result set: TOP 1 totals row (interval sums, opening,
     * current receivables, overdue, not-yet-due, percentages, account counters, captions).
     * KEEP-EXEC: dynamic SQL (sp_executesql with per-call column aliases, grid filters and sort column
     * concatenated into the statement), ten+ temp tables with UPDATEs, STRING_SPLIT / fnSplitString,
     * TmpShowHideStatusWithAccounts per-user state and multiple result sets -- not expressible as one statement.
     * JPA returns only the FIRST result set of the procedure. The later sets of this branch
     * (top-N customers by CurrentBalance limited by @TopCustomers (RNo, StartDate, AsOnDate, AccountId, AccountCode, AccountTitle, ParentAccount, AccountType, Closing, CurrentBalance, NotYetDue, BalPercent)) are not reachable through this method; the existing JdbcTemplate-based
     * ReceivablesAgingRepository keeps returning every set.
     * Grid-filter / sorting / paging parameters are not used by this branch and are not sent.
     * Parameters:
     *   - @OrganizationId (required)
     *   - @CompanyId (required)
     *   - @AsOnDate (default NULL -> today)
     *   - @AgingDays (default 0/NULL -> 30)
     *   - @UserId (required (0/NULL raises 'User not found'))
     *   - @AccouuntClassId (NULL -> 2 (assets))
     *   - @CustomGroupId (default NULL)
     *   - @ParentAccountId (default NULL)
     *   - @AccountId (default NULL (derived from UserAccount.SupplierCustomerId when @AppId in 4,6))
     *   - @ReportId (0 all | 1 overdue | 2 not-yet-due, default NULL)
     *   - @Interval (1..4, default NULL)
     *   - @BranchesIds (comma list of branch ids, default NULL)
     *   - @TopCustomers (default NULL -> 5 (Summary only))
     *   - @AppId (4/6 customer portals, 5 cost-centre app, default NULL)
     *   - @CostCenterId (used only when @AppId = 5, default NULL)
     */
    @Query(value =
           "EXEC dbo.UPS_ReceivablesAging_New @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@AsOnDate = :asOnDate, @AgingDays = :agingDays, @UserId = :userId, "
         + "@AccouuntClassId = :accouuntClassId, @Activity = 'Summary', @CustomGroupId = :customGroupId, "
         + "@ParentAccountId = :parentAccountId, @AccountId = :accountId, @ReportId = :reportId, "
         + "@Interval = :interval, @BranchesIds = :branchesIds, @TopCustomers = :topCustomers, @AppId = :appId, "
         + "@CostCenterId = :costCenterId",
           nativeQuery = true)
    List<ReceivablesAgingSummaryRow> receivablesAgingSummary(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("asOnDate") LocalDate asOnDate,
            @Param("agingDays") Integer agingDays,
            @Param("userId") Integer userId,
            @Param("accouuntClassId") Integer accouuntClassId,
            @Param("customGroupId") Integer customGroupId,
            @Param("parentAccountId") Integer parentAccountId,
            @Param("accountId") Integer accountId,
            @Param("reportId") Integer reportId,
            @Param("interval") Integer interval,
            @Param("branchesIds") String branchesIds,
            @Param("topCustomers") Integer topCustomers,
            @Param("appId") Integer appId,
            @Param("costCenterId") Integer costCenterId);

    /**
     * UPS_ReceivablesAging_New @Activity='Summary' -- same call as {@link #receivablesAgingSummary} as a raw
     * tuple, exposing the dynamically named [1_n] .. [3n_Above] sums and the [0_n%] .. [3n_Above%] percentages.
     * // KEEP-EXEC: see receivablesAgingSummary.
     */
    @Query(value =
           "EXEC dbo.UPS_ReceivablesAging_New @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@AsOnDate = :asOnDate, @AgingDays = :agingDays, @UserId = :userId, "
         + "@AccouuntClassId = :accouuntClassId, @Activity = 'Summary', @CustomGroupId = :customGroupId, "
         + "@ParentAccountId = :parentAccountId, @AccountId = :accountId, @ReportId = :reportId, "
         + "@Interval = :interval, @BranchesIds = :branchesIds, @TopCustomers = :topCustomers, @AppId = :appId, "
         + "@CostCenterId = :costCenterId",
           nativeQuery = true)
    List<Tuple> receivablesAgingSummaryRaw(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("asOnDate") LocalDate asOnDate,
            @Param("agingDays") Integer agingDays,
            @Param("userId") Integer userId,
            @Param("accouuntClassId") Integer accouuntClassId,
            @Param("customGroupId") Integer customGroupId,
            @Param("parentAccountId") Integer parentAccountId,
            @Param("accountId") Integer accountId,
            @Param("reportId") Integer reportId,
            @Param("interval") Integer interval,
            @Param("branchesIds") String branchesIds,
            @Param("topCustomers") Integer topCustomers,
            @Param("appId") Integer appId,
            @Param("costCenterId") Integer costCenterId);
}
