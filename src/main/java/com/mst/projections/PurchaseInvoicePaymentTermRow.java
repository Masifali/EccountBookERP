package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='InvPurchaseInvoicePaymentTerm_ReadByPurchaseInvoiceID' */
public interface PurchaseInvoicePaymentTermRow {
    Integer getId();
    Integer getInvPurchaseInvoiceId();
    Integer getPaymentTermId();
    String getTermsDescription();
    BigDecimal getPrcntOfTotal();
    BigDecimal getAmount();
    String getPaymentRemarks();
    Integer getDueDays();
    Timestamp getDueDate();
    Integer getSortNo();
    Integer getPurchaseOrderId();
    Integer getPurchaseOrderNo();
    Boolean getSystemGeneratedRow();
}
