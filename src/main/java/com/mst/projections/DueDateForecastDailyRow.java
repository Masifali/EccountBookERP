package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Date;

/**
 * First result set of usp_shorttermDueDateAnalysisPayablesAndReceivablesForcast: one row per calendar day
 * between @FromDate and @ToDate (or the max due date) with daily and running receivables / payables.
 * All amount columns are DECIMAL(18,2) in the procedure's #ReceivablesPayablesSummary table.
 */
public interface DueDateForecastDailyRow {
    Date getDueDate();
    BigDecimal getSaleReceivable();
    BigDecimal getExportReceivable();
    BigDecimal getTotalReceivables();
    BigDecimal getRunningTotalReceivables();
    BigDecimal getPurchasePayable();
    BigDecimal getImportPayable();
    BigDecimal getTotalPayables();
    BigDecimal getRunningTotalPayables();
    BigDecimal getReceivablesMinusPayables();
    BigDecimal getRunningReceivables();
    BigDecimal getNetReceivables();
    BigDecimal getReceivablesOverDue();
    BigDecimal getReceivablesNotYetDue();
    BigDecimal getNetPayables();
    BigDecimal getPayablesOverDue();
    BigDecimal getPayablesNotYetDue();
    Date getMaxDueDate();
}
