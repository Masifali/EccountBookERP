package com.mst.projections;

import java.math.BigDecimal;

/** Row (result set 3 = empty bags) of Sp_PurchaseOrder_GetAllMethod @Activity='PurchaseOrderModernHistory' (native). */
public interface PurchaseOrderModernHistoryEmptyBagsRow {
    Integer getId();
    Integer getOrderId();
    String getEmptyBagsType();
    String getOtherItemName();
    String getPackTypeDesc();
    BigDecimal getRate();
    BigDecimal getWeightCut();
}
