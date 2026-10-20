package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_SaleOrder_GetAllMethod (SaleOrderCustomerExpensesByOrderIds) - native projection. */
public interface SaleOrderCustomerExpenseByOrderIdsRow {
    Integer getId();
    Integer getSaleOrderId();
    Integer getSaleOrder();
    Integer getInvRevExpItemId();
    BigDecimal getQty();
    BigDecimal getRate();
    BigDecimal getAmount();
    String getRemarks();
    String getOtherItemName();
}
