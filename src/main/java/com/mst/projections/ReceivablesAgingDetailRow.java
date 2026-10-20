package com.mst.projections;

import java.sql.Date;

/**
 * First result set of UPS_ReceivablesAging_New @Activity='Detail' (one row per account/branch, paged).
 * Dynamic interval aliases ([1_&lt;n&gt;] ...) are not bindable; the fixed aliases IstIntervale / ScnInterval /
 * TrdIntarval / Above carry the same values. Use ReceivablesAgingQueryRepository.receivablesAgingDetailRaw
 * (javax.persistence.Tuple) for the dynamic aliases.
 * FLOAT -> Double, DATE -> java.sql.Date, VARBINARY -> byte[].
 */
public interface ReceivablesAgingDetailRow {
    Long getRNo();
    Date getStartDate();
    Date getAsOnDate();
    Integer getAccountId();
    String getAccountCode();
    String getAccountTitle();
    String getParentAccount();
    String getAccountType();
    Double getOpening();
    Double getClosing();
    Double getIstIntervale();
    Double getScnInterval();
    Double getTrdIntarval();
    Double getAbove();
    Double getCurrentBalance();
    Double getNotYetDue();
    Double getOverDue();
    Integer getBranchesId();
    String getBranchName();
    String getFirstIntervalCaption();
    String getSecondIntervalCaption();
    String getThirdIntervalCaption();
    String getAboveIntervalCaption();
    String getCompCountry();
    String getCompContactPerson();
    String getCompMobileA();
    String getCompEmailA();
    byte[] getCompLogoImage();
    String getOrgReportingRemarks();
    String getReportCriteria();
    Double getTotalOpening();
    Double getTotalParentOpening();
    Double getTotalClosing();
    Double getTotalParentClosing();
    Double getIstIntervalTotal();
    Double getParentIstIntervalTotal();
    Double getSecondIntervalTotal();
    Double getParentSecondIntervalTotal();
    Double getThirdIntervalTotal();
    Double getParentThirdIntervalTotal();
    Double getAboveTotal();
    Double getParentAboveTotal();
    Double getIstIntervalPercent();
    Double getSecondIntervalPercent();
    Double getThirdIntervalPercent();
    Double getAbovePercent();
    Double getCurrentReceivablesTotal();
    Double getParentCurrentBalanceTotal();
    Double getOverDueTotal();
    Double getParentOverDueTotal();
    Double getNotYetDueTotal();
    Double getParentNotYetDueTotal();
    Double getOverDuePercent();
    Double getNotYetDuePercent();
    Double getTotalReceivablesPercent();
    Double getPercentOfTotal();
    Double getOverDuePercentByAccount();
    Double getNotYetDuePercentByAccount();
    Double getParentPercentOfTotal();
    /** alias row_count */
    Integer getRow_count();
    /** alias Parentrow_count */
    Integer getParentrow_count();
}
