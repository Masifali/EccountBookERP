package com.mst.projections;

import java.math.BigDecimal;
import java.util.Date;

/** Row of Sp_SaleOrder_GetAllMethod (SaleOrderPaymentTermDetailByHeaderId) - native projection. */
public interface SaleOrderPaymentTermDetailRow {
    Integer getId();
    Integer getSaleOrderId();
    Integer getPaymentTermId();
    Integer getPaymentTermsId();
    String getTermsDescription();
    BigDecimal getPrcntOfTotal();
    Integer getDueDays();
    String getPaymentRemarks();
    BigDecimal getAmount();
    Integer getSortNo();
    Date getDueDate();
}
