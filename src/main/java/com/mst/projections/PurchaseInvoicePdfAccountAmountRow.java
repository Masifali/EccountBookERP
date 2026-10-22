package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetHistoryForPdfReport' */
public interface PurchaseInvoicePdfAccountAmountRow {
    Integer getInvoiceId();
    Integer getAccountId();
    String getAccountName();
    BigDecimal getAmount();
    String getRemarks();
}
