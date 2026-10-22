package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Date;

/**
 * First result set of UPS_ReceivablesAging_New @Activity='Summary' (TOP 1 row of totals).
 * Dynamically named interval / percent columns are not exposed here (see
 * ReceivablesAgingQueryRepository.receivablesAgingSummaryRaw for a javax.persistence.Tuple view).
 */
public interface ReceivablesAgingSummaryRow {
    Date getStartDate();
    Date getAsOnDate();
    /** SUM(Opening) OVER() -> FLOAT */
    Double getOpening();
    /** MAX(TotalReceivables) OVER() -> DECIMAL(18,0) */
    BigDecimal getCurrentReceivables();
    BigDecimal getOverDue();
    BigDecimal getNotYetDue();
    BigDecimal getOverDuePercent();
    BigDecimal getNotYetDuePercent();
    BigDecimal getTotalReceivablesPercent();
    Integer getIstIntervalAccountsCount();
    Integer getSecondIntervalAccountsCount();
    Integer getThirdIntervalAccountsCount();
    Integer getAboveIntervalAccountsCount();
    BigDecimal getOverDueAccountsCount();
    BigDecimal getNotYetDueAccountsCount();
    BigDecimal getCurrentBalanceAccountsCount();
    String getFirstIntervalCaption();
    String getSecondIntervalCaption();
    String getThirdIntervalCaption();
    String getAboveIntervalCaption();
}
