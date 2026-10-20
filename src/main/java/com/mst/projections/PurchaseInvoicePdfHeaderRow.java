package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetHistoryForPdfReport' */
public interface PurchaseInvoicePdfHeaderRow {
    Long getRecordNo();
    Integer getInvoiceId();
    Integer getInvoiceNumber();
    Timestamp getInvoiceDate();
    String getReference();
    String getManualBillNumber();
    Integer getDueDays();
    Timestamp getDueDate();
    String getInvoiceHeaderRemarks();
    Integer getSale_Purcahse_OrderNumber();
    Integer getSale_Purchase_OrderId();
    Integer getInvoiceTypeId();
    Integer getCustomer_Supplier_Id();
    String getCustomer_Supplier_Name();
    String getCustomer_Supplier_MobileNumber();
    String getCustomer_Supplier_Email();
    String getCustomer_Supplier_Address();
    Integer getCustomer_Supplier_GlAccountId();
    Integer getComissionAgentId();
    Integer getComissionAgentGlId();
    String getComissionAgentName();
    String getComissionType();
    BigDecimal getComissioniRate();
    Integer getComissionRateUom();
    BigDecimal getComissionAmount();
    String getComissionRemarks();
    Integer getBrokerAgentId();
    Integer getBrokerGlId();
    String getBrokerAgentName();
    String getBrokeryType();
    BigDecimal getBrokeryRate();
    String getBrokeryRateUom();
    BigDecimal getBrokeryAmount();
    String getBrokeryRemarks();
    Boolean getIsApproved();
    String getEntryUser();
    Timestamp getEntryDate();
    String getModifyUser();
    Timestamp getModifyDate();
    String getApprovedUser();
    Timestamp getApprovedDate();
    Integer getOrganizationId();
    Integer getCompanyId();
    BigDecimal getGrandBillAmount();
    Integer getAttachmentsCount();
    Integer getNoOfAttachments();
}
