package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='FormHistory' */
public interface PurchaseInvoiceFormHistoryRow {
    Long getRecordNo();
    Integer getId();
    Integer getVoucherHeadId();
    Integer getDocumentTypeId();
    Integer getDocNo();
    Integer getOrderNo();
    Timestamp getDocDate();
    String getPurchaseAgainst();
    Integer getDueDays();
    Timestamp getDueDate();
    String getManualBillNo();
    Integer getSupplierCustomerId();
    String getSupplierName();
    String getReferencePartyName();
    String getCommissionAgent();
    String getCommissionType();
    BigDecimal getCommRate();
    BigDecimal getCommAmount();
    String getCommissionRemarks();
    String getRemarksHeader();
    BigDecimal getBillAmount();
    Integer getCurrencyId();
    String getCurrencyCode();
    String getCurrencyName();
    BigDecimal getExchangeRate();
    BigDecimal getFcyAmount();
    Timestamp getEntryDate();
    String getEntryUser();
    Timestamp getModifyDate();
    String getModifyUser();
    Timestamp getPostDate();
    String getApprovedUser();
    String getBillType();
    String getScreenName();
    String getApprovedStatus();
    Integer getNoOfAttachments();
    String getBranchName();
    Integer getBranchSrNo();
    Integer getCommissionCreditAccountId();
    String getVehicleNos();
}
