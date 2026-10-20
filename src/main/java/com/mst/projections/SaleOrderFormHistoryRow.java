package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.Date;

/** Row of Sp_SaleOrder_GetAllMethod (SaleOrderFormHistory) - native projection. */
public interface SaleOrderFormHistoryRow {
    Long getRecordNo();
    Integer getId();
    Integer getDocNo();
    Integer getBranchSrNo();
    String getBranchName();
    Date getDocDate();
    String getCustomerName();
    String getCommissionType();
    BigDecimal getCommRate();
    BigDecimal getCommAmount();
    BigDecimal getItemAmountHeader();
    BigDecimal getDiscountAmountHeader();
    BigDecimal getTaxAmountHeader();
    BigDecimal getOrderAmount();
    BigDecimal getOrderQty();
    BigDecimal getOrderWeight();
    String getCommissionRemarks();
    Integer getDueDays();
    Date getDueDate();
    Date getOrderExpiryDate();
    Boolean getIsAproved();
    String getDeliveryTerm();
    Integer getDeliveryDays();
    String getRemarksHeader();
    String getOrderStatus();
    String getSupplierRefNo();
    String getTermsDescription();
    String getUserName();
    Timestamp getEntryDate();
    String getModifyUserName();
    Timestamp getModifyDate();
    String getApprovedUser();
    Timestamp getPostDate();
    Integer getOtherCommissionAgentId();
    String getOtherCommissionAgent();
    String getOtherCommissionType();
    String getOtherCommissionUom();
    BigDecimal getOtherCommissionRate();
    BigDecimal getOtherCommissionAmount();
    String getOtherCommissionRemarks();
    Integer getNoOfAttachments();
    String getOrderType();
    String getOtherCategory();
    String getBookingPerson();
}
