package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='InvPurchaseInvoiceEmptyBags_ReadByPurchaseInvoiceID' */
public interface PurchaseInvoiceEmptyBagsRow {
    Integer getId();
    Integer getTypeId();
    Integer getPurchaseOrderId();
    Integer getInvGrnId();
    Integer getInvPurchaseInvoiceId();
    Integer getItemId();
    String getItemName();
    BigDecimal getPurchaseQty();
    BigDecimal getRate();
    BigDecimal getAmount();
    String getRemarks();
    BigDecimal getReceivedQty();
    String getEmptyBagsType();
    Integer getCreditAccountId();
    String getAccountTitle();
    Integer getWarehouseId();
    Integer getRateUomId();
    String getCustomRemarks();
    Integer getItemConditionId();
    String getItemCondition();
    Integer getRackId();
    String getRackName();
}
