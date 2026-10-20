package com.mst.projections;

import java.math.BigDecimal;

/**
 * Second result set of usp_PayablesAndPaymentSchedule: three fixed rows (Net / OverDue / NotYetDue Payables)
 * carrying the aging interval captions built from @AgingDays; Amount and Value_1..Value_4 are always 0.
 */
public interface PayablesAgingCaptionRow {
    String getDescription();
    BigDecimal getAmount();
    String getIstIntervale();
    String getScnInterval();
    String getTrdIntarval();
    String getAbove();
    Integer getValue_1();
    Integer getValue_2();
    Integer getValue_3();
    Integer getValue_4();
}
