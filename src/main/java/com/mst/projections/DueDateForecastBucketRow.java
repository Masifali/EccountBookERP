package com.mst.projections;

import java.math.BigDecimal;

/**
 * Second result set of usp_shorttermDueDateAnalysisPayablesAndReceivablesForcast: the five aging buckets
 * (SortNo 1..5) with bucket totals and cumulative receivables / payables / balance.
 */
public interface DueDateForecastBucketRow {
    Integer getSortNo();
    String getDescription();
    BigDecimal getSaleReceivable();
    BigDecimal getExportReceivable();
    BigDecimal getTotalReceivables();
    BigDecimal getComulativeReceivables();
    BigDecimal getPurchasePayable();
    BigDecimal getImportPayable();
    BigDecimal getTotalPayables();
    BigDecimal getComulativePayables();
    BigDecimal getNetBalance();
    BigDecimal getComulativeBalance();
}
