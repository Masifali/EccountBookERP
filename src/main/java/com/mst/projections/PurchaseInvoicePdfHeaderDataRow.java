package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetHistoryForPdfReport_HeaderData' */
public interface PurchaseInvoicePdfHeaderDataRow {
    Long getRecordNo();
    Integer getInvoiceId();
    Integer getInvoiceNumber();
    Timestamp getInvoiceDate();
    Integer getSale_Purcahse_OrderNumber();
    Integer getSale_Purchase_OrderId();
    Integer getInvoiceTypeId();
    Integer getSupplierCustomerId();
    String getCustomer_Supplier_Name();
    Boolean getIsApproved();
    BigDecimal getGrandBillAmount();
}
