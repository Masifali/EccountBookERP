package com.mst.projections;

import java.math.BigDecimal;

/** Row of USP_GetWagesAmountByRefDocumentTypeIdandRefIds (native, single aggregate row) */
public interface GrnWagesAmountRow {
    BigDecimal getTotalQty();
    BigDecimal getTotalWeight();
    BigDecimal getWagesAmount();
}
