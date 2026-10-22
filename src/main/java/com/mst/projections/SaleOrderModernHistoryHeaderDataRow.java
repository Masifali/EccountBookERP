package com.mst.projections;

import java.math.BigDecimal;
import java.util.Date;

/** Row of Sp_SaleOrder_GetAllMethod (SaleOrderModernHistory_HeaderData) - native projection. */
public interface SaleOrderModernHistoryHeaderDataRow {
    Long getRecordNo();
    Integer getOrderId();
    Integer getDocNo();
    Date getDocDate();
    String getSupplier_Customer_Name();
    Boolean getIsAproved();
    String getOrderStatus();
    String getExpiryStatus();
    BigDecimal getOrderAmount();
}
