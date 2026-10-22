package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='InvPurchaseInvoiceJournal_ReadByPurchaseInvoiceID' */
public interface PurchaseInvoiceJournalRow {
    Integer getId();
    Integer getSupplierCustomerId();
    String getSupplierName();
    Integer getInvPurchaseInvoiceId();
    Integer getChartofAccountId();
    String getAccountTitle();
    String getJvRemarks();
    BigDecimal getJvDebit();
    BigDecimal getJvCredit();
    Integer getFreightId();
    BigDecimal getJvPrcnt();
    BigDecimal getJvQty();
    BigDecimal getJvRate();
    Integer getInvGrnId();
    Integer getTransporterSupCustId();
    String getRowType();
}
