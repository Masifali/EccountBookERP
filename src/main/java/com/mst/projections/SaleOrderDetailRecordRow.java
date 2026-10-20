package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_SaleOrder_GetAllMethod (GetOrderDetailRecordByOrderId) - native projection. */
public interface SaleOrderDetailRecordRow {
    Integer getId();
    Integer getSaleOrderId();
    Integer getOrderItemId();
    Integer getOrderItemUOMId();
    String getItemName();
    String getEquivalent();
    String getCrop();
    Integer getJobLotId();
    String getJobLotCode();
    BigDecimal getOrderItemQty();
    BigDecimal getOrderItemRate();
    Integer getOrderItemRateUOMId();
    String getRateUOM();
}
