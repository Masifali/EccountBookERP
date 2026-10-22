package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_SaleOrder_GetAllMethod (SaleOrderCustomerExpensesByHeaderId) - native projection. */
public interface SaleOrderCustomerExpenseRow {
    Integer getId();
    Integer getSaleOrderId();
    Integer getInvRevExpItemId();
    BigDecimal getQty();
    BigDecimal getRate();
    BigDecimal getAmount();
    String getRemarks();
    String getOtherItemName();
}
