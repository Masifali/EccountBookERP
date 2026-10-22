package com.mst.projections;

import java.math.BigDecimal;
import java.util.Date;

/** Row of Sp_SaleOrder_GetAllMethod (GetSaleOrderdetailforDeliverySchedule) - native projection. */
public interface SaleOrderDetailForDeliveryScheduleRow {
    Integer getId();
    Date getDocDate();
    Integer getDocNo();
    Integer getOrderSupCustId();
    String getCompanyName();
    String getSupplierRefNo();
    Integer getItemId();
    Integer getOrderItemUOMId();
    String getProductNo();
    String getManufacturePartNo();
    String getItemName();
    String getItemCodeNew();
    BigDecimal getOrderItemQty();
    String getItemCode();
    BigDecimal getCommOnSale();
    String getUOMCode();
}
