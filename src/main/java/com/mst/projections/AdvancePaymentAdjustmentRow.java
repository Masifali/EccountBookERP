package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_PurchaseOrder_GetAllMethod @Activity='ReadAdvancePaymentAdjustmentByHeaderId' (native). */
public interface AdvancePaymentAdjustmentRow {
    Integer getId();
    Integer getPurchaseOrderId();
    Integer getVoucherId();
    Integer getAccountId();
    String getAccountTitle();
    Integer getSupplierCustomerId();
    String getSupplierCustomerName();
    BigDecimal getAdjustmentAmount();
    Integer getDocumentStatusId();
}
