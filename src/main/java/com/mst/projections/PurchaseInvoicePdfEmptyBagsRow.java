package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetHistoryForPdfReport' */
public interface PurchaseInvoicePdfEmptyBagsRow {
    Integer getInvoiceId();
    Integer getItemId();
    Integer getTypeId();
    String getItemName();
    BigDecimal getAmount();
    String getRemarks();
}
