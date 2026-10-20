package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetEmptyBagsFromGrn' */
public interface PurchaseInvoiceEmptyBagsFromGrnRow {
    Integer getPurchaseOrderId();
    Integer getInvGrnId();
    Integer getTypeId();
    Integer getItemId();
    String getItemName();
    BigDecimal getPurchaseQty();
    BigDecimal getRate();
    String getRemarks();
    String getEmptyBagsType();
    Integer getItemConditionId();
    String getItemCondition();
}
