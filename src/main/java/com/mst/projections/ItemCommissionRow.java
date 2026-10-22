package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_Item_GetAllMethod @Activity='ItemCommissionScheduleByItemId' (TOP 1, no ORDER BY) (native) */
public interface ItemCommissionRow {
    BigDecimal getOnSale();
    BigDecimal getOnPurchase();
}
