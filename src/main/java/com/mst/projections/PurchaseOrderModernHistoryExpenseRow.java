package com.mst.projections;

import java.math.BigDecimal;

/** Row (result set 4 = expenses charge to product) of Sp_PurchaseOrder_GetAllMethod @Activity='PurchaseOrderModernHistory' (native). */
public interface PurchaseOrderModernHistoryExpenseRow {
    Integer getId();
    Integer getOrderId();
    String getAccountTitle();
    BigDecimal getPercentage();
    BigDecimal getQty();
    BigDecimal getRate();
    Integer getSupplierCustomerId();
    String getSupplierCustomerName();
    BigDecimal getAmount();
    String getRemarks();
}
