package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='ReadPurchaseOrderSupplierExpenseByGdnIds' */
public interface PurchaseOrderSupplierExpenseRow {
    Integer getId();
    Integer getPurchaseOrderId();
    Integer getInvRevExpItemId();
    BigDecimal getQty();
    BigDecimal getRate();
    BigDecimal getAmount();
    String getRemarks();
    String getOtherItemName();
}
