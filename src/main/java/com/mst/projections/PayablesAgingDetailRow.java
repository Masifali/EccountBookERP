package com.mst.projections;

import java.sql.Date;

/**
 * First result set of UPS_PayablesAging_New @Activity='Detail' (one row per account/branch, paged).
 * The four dynamically named interval columns ([1_&lt;AgingDays&gt;], [&lt;n+1&gt;_&lt;2n&gt;], [&lt;2n+1&gt;_&lt;3n&gt;],
 * [&lt;3n&gt;_Above]) cannot be bound to fixed getters; the same values are also returned under the
 * fixed aliases IstIntervale / ScnInterval / TrdIntarval / Above, and the caption columns carry the
 * dynamic names. Use PayablesAgingQueryRepository.payablesAgingDetailRaw (javax.persistence.Tuple) when
 * the dynamic aliases themselves are needed.
 * FLOAT columns arrive as Double, DECIMAL(18,0) as BigDecimal, DATE as java.sql.Date, VARBINARY as byte[].
 */
public interface PayablesAgingDetailRow {
    /** ROW_NUMBER() OVER(ORDER BY AccountId) -> bigint */
    Long getRNo();
    Date getStartDate();
    Date getAsOnDate();
    Integer getAccountId();
    String getAccountCode();
    /** multi-lingual title when @LanguageId matches ChartOfAccountMultiLingo, else C.AccountTitle */
    String getAccountTitle();
    String getEnglishAccountTitle();
    String getParentAccount();
    /** AccountTypes.Abbreviation */
    String getAccountType();
    Integer getBranchesId();
    String getBranchName();
    Double getOpening();
    Double getClosing();
    Double getIstIntervale();
    Double getScnInterval();
    Double getTrdIntarval();
    Double getAbove();
    Double getCurrentBalance();
    Double getOverDue();
    Double getNotYetDue();
    String getCompCountry();
    String getCompContactPerson();
    String getCompMobileA();
    String getCompEmailA();
    byte[] getCompLogoImage();
    String getOrgReportingRemarks();
    String getFirstIntervalCaption();
    String getSecondIntervalCaption();
    String getThirdIntervalCaption();
    String getAboveIntervalCaption();
    /** "EndDate: <AsOnDate> AND AgingDays <n>" */
    String getReportCriteria();
    Double getTotalOpening();
    Double getTotalClosing();
    Double getIstIntervalTotal();
    Double getSecondIntervalTotal();
    Double getThirdIntervalTotal();
    Double getAboveTotal();
    Double getTotalParentOpening();
    Double getTotalParentClosing();
    Double getParentIstIntervalTotal();
    Double getParentSecondIntervalTotal();
    Double getParentThirdIntervalTotal();
    Double getParentAboveTotal();
    Double getIstIntervalPercent();
    Double getSecondIntervalPercent();
    Double getThirdIntervalPercent();
    Double getAbovePercent();
    Double getCurrentPayablesTotal();
    Double getOverDueTotal();
    Double getNotYetDueTotal();
    Double getParentCurrentBalanceTotal();
    Double getParentOverDueTotal();
    Double getParentNotYetDueTotal();
    Double getOverDuePercent();
    Double getNotYetDuePercent();
    Double getTotalPayablesPercent();
    Double getPercentOfTotal();
    Double getOverDuePercentByAccount();
    Double getNotYetDuePercentByAccount();
    Double getParentPercentOfTotal();
    /** alias row_count (COUNT(..) OVER()) */
    Integer getRow_count();
    /** alias Parentrow_count */
    Integer getParentrow_count();
}
