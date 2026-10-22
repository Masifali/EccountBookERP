package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_PurchaseOrder_GetAllMethod @Activity='GetPurchaseOrderEmptyBagsDetailByOrderId' (native, SELECT * FROM PurchaseOrderEmptyBags + EmptyBagsType; known columns listed). */
public interface PurchaseOrderEmptyBagsByOrderRow {
    Integer getId();
    Integer getPurchaseOrderId();
    Integer getItemId();
    Integer getPackingTypeId();
    BigDecimal getRate();
    BigDecimal getWeightCut();
    Integer getType();
    String getEmptyBagsType();
}
