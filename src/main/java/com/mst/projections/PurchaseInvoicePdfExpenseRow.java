package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetHistoryForPdfReport' */
public interface PurchaseInvoicePdfExpenseRow {
    Integer getInvoiceId();
    Integer getItemId();
    String getItemName();
    BigDecimal getAmount();
    String getRemarks();
}
