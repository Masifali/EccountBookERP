package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_PurchaseOrder_GetAllMethod @Activity='ReadPurchaseOrderExpensesChargeToProductDetailByHeaderId' (native). */
public interface PurchaseOrderExpensesChargeToProductRow {
    Integer getId();
    Integer getAccountId();
    String getAccountTitle();
    BigDecimal getPercentage();
    Integer getPurchaseOrderId();
    BigDecimal getQty();
    BigDecimal getRate();
    Integer getSupplierCustomerId();
    String getSupplierCustomerName();
    BigDecimal getAmount();
    String getRemarks();
}
