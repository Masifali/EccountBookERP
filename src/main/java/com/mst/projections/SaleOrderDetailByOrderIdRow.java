package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_SaleOrder_GetAllMethod (ReadBySaleOrderIDNOrderItemId) - native projection. */
public interface SaleOrderDetailByOrderIdRow {
    Integer getId();
    Integer getSaleOrderId();
    Integer getOrderItemId();
    Integer getOrderItemUOMId();
    String getItemName();
    String getItemCodeNew();
    String getEquivalent();
    String getCrop();
    String getJobLotDescription();
    BigDecimal getOrderItemQty();
    BigDecimal getNetWeight();
    BigDecimal getOrderItemRate();
    String getRateUOM();
    Integer getOrderItemRateUOMId();
    BigDecimal getAmount();
    Integer getJobLotId();
    String getCityArea();
    String getLabSampleNo();
    String getAmountCalcType();
    String getTaxableStatus();
    String getOrderRemarks();
    BigDecimal getBagWeight();
    BigDecimal getBagPrice();
    Integer getDocumentTypeId();
    Integer getDiscountTypeId();
    String getDiscountType();
    String getPaymentTerm();
    Integer getDocNo();
}
