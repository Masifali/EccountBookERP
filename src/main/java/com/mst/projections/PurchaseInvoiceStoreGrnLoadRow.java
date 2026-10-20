package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='LoadGrnForStorePurchaseInvoiceByGrnIds' */
public interface PurchaseInvoiceStoreGrnLoadRow {
    Integer getInvGrnId();
    Integer getSupplierCustomerId();
    String getRemarksHeader();
    Integer getId();
    Integer getItemId();
    String getItemName();
    BigDecimal getItemQty();
    Integer getWarehouseId();
    Integer getItemUomId();
    String getUOMCode();
    String getWareHouseName();
    BigDecimal getItemRate();
    Integer getInvPurchasedemondId();
    Integer getRefDocNo();
    BigDecimal getItemAmount();
    Integer getJobLotId();
    String getDeliveryTerm();
    Integer getPurchaseGLAC();
    Integer getGlAccountId();
    String getTaxNameId();
    String getTaxName();
    String getTaxPercent();
    BigDecimal getTaxAmount();
}
