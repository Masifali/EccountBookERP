package com.mst.projections;

/** Row of Sp_PurchaseOrder_GetAllMethod @Activity='MarketPurchaseOrderFormHistory' (native). */
public interface MarketPurchaseOrderFormHistoryRow {
    Long getRecordNo();
    Integer getId();
    Integer getDocNo();
    java.sql.Timestamp getDocDate();
    String getSupplierName();
    String getCommissionType();
    Double getCommRate();
    Double getCommAmount();
    Boolean getIsAproved();
    String getCommissionRemarks();
    Integer getDueDays();
    java.sql.Timestamp getDueDate();
    java.sql.Date getOrderExpiryDate();
    String getDeliveryTerm();
    Integer getDeliveryDays();
    java.sql.Timestamp getDeliveryStartDate();
    String getRemarksHeader();
    String getOrderStatus();
    Integer getItemCount();
    Double getItemQuantity();
    Double getItemWeight();
    String getTermsDescription();
    String getUserName();
    Integer getCityId();
    String getCityName();
    Integer getOrderCatagoryId();
    String getOrderCategoryDescription();
    Integer getBrokerAgentSupCustId();
    String getPurchaserName();
    Integer getNoOfAttachments();
}
