package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_SaleOrder_GetAllMethod (LoadSaleOrderForGoodsDispatchNotes) - native projection. */
public interface SaleOrderForGoodsDispatchNotesRow {
    Integer getSupplierCustomerId();
    String getSupplierCustomer();
    Integer getRefPartyId();
    Integer getOrderId();
    Integer getOrderNo();
    Integer getSaleOrderDetailId();
    Integer getItemId();
    String getItem();
    String getItemUOM();
    Integer getItemUOMId();
    Integer getPackingTypeId();
    String getPackTypeDesc();
    BigDecimal getQTY();
    BigDecimal getWeight();
    BigDecimal getLoadQty();
    BigDecimal getLoadWeight();
    BigDecimal getPackingWeight();
    BigDecimal getGrossWeight();
    Integer getJobLotId();
    String getJobLotDescription();
    String getRemarks();
    String getCropYear();
    String getDeliveryTerm();
    Integer getCityId();
    String getCityArea();
    String getItemCode();
}
