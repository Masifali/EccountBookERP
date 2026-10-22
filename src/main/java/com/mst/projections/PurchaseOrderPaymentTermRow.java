package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='PurchaseOrderPaymentTermDetailByPoIds' */
public interface PurchaseOrderPaymentTermRow {
    Integer getId();
    Integer getPurchaseOrderId();
    Integer getPurchaseOrderNo();
    Integer getPaymentTermId();
    Integer getPaymentTermsId();
    String getTermsDescription();
    BigDecimal getPrcntOfTotal();
    Integer getDueDays();
    String getPaymentRemarks();
    BigDecimal getAmount();
    Integer getSortNo();
    Timestamp getDueDate();
}
