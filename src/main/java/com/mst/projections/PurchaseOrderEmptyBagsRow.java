package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_PurchaseOrder_GetAllMethod @Activity='ReadPurchaseOrderEmptyBagsDetailByHeaderId' (native). */
public interface PurchaseOrderEmptyBagsRow {
    Integer getId();
    Integer getItemId();
    String getItemName();
    Integer getPackingTypeId();
    String getPackTypeDesc();
    Integer getPurchaseOrderId();
    BigDecimal getRate();
    BigDecimal getWeightCut();
    Integer getType();
    String getEmptyBagsType();
}
