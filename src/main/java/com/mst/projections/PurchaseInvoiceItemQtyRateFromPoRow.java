package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetItemQtyAndItemRateFromPO' */
public interface PurchaseInvoiceItemQtyRateFromPoRow {
    BigDecimal getBagPrice();
    BigDecimal getItemQty();
    Integer getId();
}
