package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_InvPackingType_GetAllMethod @Activity='ReadAll' (JPQL) */
public interface InvPackingTypeReadAllRow {
    Integer getId();
    String getPackTypeCode();
    String getPackTypeDesc();
    BigDecimal getMinEbWeight();
    BigDecimal getMaxEbWeight();
    BigDecimal getMinEbStockWeight();
    BigDecimal getMaxEbStockWeight();
}
