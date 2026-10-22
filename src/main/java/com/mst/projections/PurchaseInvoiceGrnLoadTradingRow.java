package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GrnLoadForPurchaseInvoiceTrading' */
public interface PurchaseInvoiceGrnLoadTradingRow {
    Integer getId();
    Integer getInvGrnId();
    Timestamp getDocDate();
    Integer getDocNo();
    Integer getSupplierCustomerId();
    String getSupplierCustomer();
    BigDecimal getCarriageAmount();
    Integer getItemId();
    String getItemCode();
    String getItemName();
    String getCropYear();
    Integer getItemUomId();
    String getUOMCodeItem();
    BigDecimal getItemQty();
    Integer getWarehouseId();
    Integer getPackingTypeId();
    String getLabAnalisysNo();
    Integer getPurchaseOrderId();
    Integer getPurchaseOrder();
    BigDecimal getGrossWeight();
    BigDecimal getEBWeight();
    BigDecimal getEBTotalWt();
    BigDecimal getWtCut();
    BigDecimal getWeightCutTotal();
    BigDecimal getNetBillWeight();
    BigDecimal getNetStockWeight();
    String getAreaCity();
    BigDecimal getOrderItemRate();
    Integer getOrderItemRateUOMId();
    BigDecimal getEquivalentPoRate();
    BigDecimal getItemAmount();
    BigDecimal getTaxAmount();
    Integer getJobLotId();
    Integer getItemTaxSchId();
    BigDecimal getTaxPercent();
    String getDeliveryTerm();
    Integer getPurchaseGLAC();
    Integer getGlAccountId();
    String getRemarksHeader();
    String getAccountTitle();
    Integer getGpNo();
    String getVehicleNo();
    Integer getInwardGatePassId();
}
