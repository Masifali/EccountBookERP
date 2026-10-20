package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_SaleOrder_GetAllMethod (SaleOrderModernHistory, second result set = detail lines) - native projection. */
public interface SaleOrderModernHistoryDetailRow {
    Integer getId();
    Integer getOrderId();
    String getItemName();
    String getPackUom();
    BigDecimal getOrderItemQty();
    BigDecimal getNetWeight();
    BigDecimal getOrderItemRate();
    String getRateUom();
    BigDecimal getAmount();
    String getJobLotDescription();
    String getCityArea();
    String getCrop();
    BigDecimal getBagWeight();
    BigDecimal getBagPrice();
    String getOrderRemarks();
}
