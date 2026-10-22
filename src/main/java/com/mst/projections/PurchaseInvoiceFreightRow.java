package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='InvPurchaseInvoiceFreight_ReadByPurchaseInvoiceID' */
public interface PurchaseInvoiceFreightRow {
    Integer getId();
    Integer getSupplierCustomerId();
    String getSupplierName();
    Integer getInvPurchaseInvoiceId();
    Integer getInvGrnId();
    BigDecimal getFrQty();
    BigDecimal getFrWeight();
    BigDecimal getFrRate();
    BigDecimal getFreightAmount();
    BigDecimal getDebit();
    BigDecimal getPercentage();
    Integer getFreightId();
    Integer getPurchaseOrderId();
    String getRemarks();
    Integer getTansporterId();
    String getAccountTitle();
}
