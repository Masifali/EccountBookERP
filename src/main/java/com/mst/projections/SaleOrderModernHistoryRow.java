package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.Date;

/** Row of Sp_SaleOrder_GetAllMethod (SaleOrderModernHistory, first result set = #SaleOrder header) - native projection. */
public interface SaleOrderModernHistoryRow {
    Long getRecordNo();
    Integer getOrderId();
    Integer getDocNo();
    Date getDocDate();
    String getOrderCategoryName();
    Integer getCatagorySrNo();
    String getSupplierRefNo();
    Integer getRefrenenceParty();
    String getPaymentTerm();
    Integer getOrderDueDays();
    Date getOrderDueDate();
    String getSupplier_Customer_Name();
    String getSupplier_Customer_Mobile();
    String getSupplier_Customer_Email();
    String getSupplier_Customer_Address();
    Date getOrderExpiryDate();
    String getDeliveryTerm();
    Date getDeliveryStartDate();
    Integer getDeliveryDays();
    String getDeliveryRemarks();
    String getRemarksHeader();
    Boolean getIsAproved();
    String getOrderStatus();
    String getExpiryStatus();
    Integer getAttachmentsCount();
    Integer getCommissionAgentId();
    String getCommissionAgentName();
    String getCommissionType();
    BigDecimal getCommRate();
    Integer getUomScheduleIdCmRate();
    BigDecimal getCommAmount();
    String getEntryUserName();
    Integer getEntryUser();
    Timestamp getEntryDate();
    String getModifyUserName();
    Integer getModifyUser();
    Timestamp getModifyDate();
    String getApprovalUserName();
    Integer getPostUser();
    Timestamp getPostDate();
    String getEntryUserProfileImage();
    String getModifyUserProfileImage();
    String getApprovalUserProfileImage();
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getFinancialYearId();
}
