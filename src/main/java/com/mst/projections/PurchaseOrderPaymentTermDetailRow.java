package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_PurchaseOrder_GetAllMethod @Activity='PurchaseOrderPaymentTermDetailByHeaderId' (native). */
public interface PurchaseOrderPaymentTermDetailRow {
    Integer getId();
    Integer getPurchaseOrderId();
    Integer getPaymentTermId();
    String getPaymentTerm();
    BigDecimal getPrcntOfTotal();
    Integer getDueDays();
    String getPaymentRemarks();
    BigDecimal getAmount();
    Integer getSortNo();
    java.sql.Timestamp getDueDate();
}
