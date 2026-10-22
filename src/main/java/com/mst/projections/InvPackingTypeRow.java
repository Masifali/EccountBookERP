package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_InvPackingType_GetAllMethod @Activity='ReadById' (JPQL) */
public interface InvPackingTypeRow {
    Integer getId();
    String getPackTypeCode();
    String getPackTypeDesc();
    Boolean getIsActive();
    BigDecimal getMinEbWeight();
    BigDecimal getMaxEbWeight();
    BigDecimal getMinEbStockWeight();
    BigDecimal getMaxEbStockWeight();
}
