package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='FormHistoryTradePro' */
public interface PurchaseInvoiceFormHistoryTradeProRow {
    Long getRecordNo();
    Integer getId();
    Integer getVoucherHeadId();
    Integer getGrnId();
    Integer getDocNo();
    Timestamp getDocDate();
    String getManualBillNo();
    String getSupplierName();
    String getCommissionAgent();
    Integer getGrnNo();
    String getCommissionType();
    BigDecimal getCommRate();
    BigDecimal getCommAmount();
    String getCommissionRemarks();
    String getRemarksHeader();
    String getStockPartyName();
    String getRefPartyName();
    BigDecimal getBillAmount();
    String getUserName();
    String getItemName();
    BigDecimal getEquivalent();
    BigDecimal getItemRate();
    BigDecimal getItemAmount();
    BigDecimal getItemQty();
    String getRemarksDetail();
    String getScreenName();
    Integer getNoOfAttachments();
}
