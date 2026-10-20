package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Date;

/**
 * First result set of UPS_PayablesAging_New @Activity='Summary' (TOP 1 row of totals).
 * The dynamically named columns ([1_&lt;n&gt;], [&lt;n+1&gt;_&lt;2n&gt;], [&lt;2n+1&gt;_&lt;3n&gt;], [&lt;3n&gt;_Above] and the
 * matching "...%" percent columns) have no fixed alias in the procedure and are therefore not exposed
 * here; use PayablesAgingQueryRepository.payablesAgingSummaryRaw (javax.persistence.Tuple) to read them
 * by the caption values (FirstIntervalCaption etc. hold the exact alias names).
 */
public interface PayablesAgingSummaryRow {
    Date getStartDate();
    Date getAsOnDate();
    /** SUM(Closing) OVER() -> FLOAT */
    Double getClosing();
    /** MAX(TotalPayables) OVER() -> DECIMAL(18,0) */
    BigDecimal getCurrentPayables();
    BigDecimal getOverDue();
    BigDecimal getNotYetDue();
    BigDecimal getOverDuePercent();
    BigDecimal getNotYetDuePercent();
    BigDecimal getTotalPayablesPercent();
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
