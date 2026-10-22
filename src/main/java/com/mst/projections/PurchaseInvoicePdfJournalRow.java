package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetHistoryForPdfReport' */
public interface PurchaseInvoicePdfJournalRow {
    Integer getInvoiceId();
    Integer getAccountId();
    String getAccountName();
    BigDecimal getDebitAmount();
    BigDecimal getCreditAmount();
    String getRemarks();
}
