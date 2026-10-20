package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetPurchaseInvoiceReturnForApproval' */
public interface PurchaseInvoiceReturnForApprovalRow {
    Integer getId();
    Timestamp getDocDate();
    Integer getDocNo();
    String getCustomerName();
    String getRemarksHeader();
    String getItemName();
    BigDecimal getItemQty();
    BigDecimal getItemRate();
    BigDecimal getItemAmount();
    String getWareHouseName();
}
