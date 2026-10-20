package com.mst.projections;

/** Row of Sp_PurchaseOrder_GetAllMethod @Activity='PurchaseOrderModernHistory_HeaderData' (native). */
public interface PurchaseOrderModernHistoryHeaderDataRow {
    Long getRecordNo();
    Integer getOrderId();
    Integer getDocNo();
    java.sql.Timestamp getDocDate();
    String getSupplier_Customer_Name();
    Boolean getIsAproved();
    String getOrderStatus();
    String getExpiryStatus();
    Double getOrderAmount();
}
