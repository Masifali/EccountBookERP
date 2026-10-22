package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='DirectGrnLoadForPurchaseInvoiceDirect' */
public interface PurchaseInvoiceDirectGrnLoadRow {
    Integer getInvGrnDetailId();
    Integer getInvGrnId();
    Timestamp getDocDate();
    Integer getDocNo();
    Integer getSupplierCustomerId();
    String getSupplierCustomer();
    Integer getTransporterId();
    String getAccountTitle();
    BigDecimal getCarriageAmount();
    String getDeliveryTerm();
    Integer getGpNo();
    String getVehicleNo();
    Integer getWarehouseId();
    String getWareHouseName();
    Integer getItemId();
    String getItemCode();
    String getItemName();
    String getCropYear();
    Integer getJobLotId();
    String getJobLotDescription();
    Integer getPackingTypeId();
    String getPackTypeDesc();
    Integer getItemUOMId();
    String getUOMCodeItem();
    BigDecimal getEquivalent();
    BigDecimal getItemQty();
    BigDecimal getGrossWeight();
    BigDecimal getEBWPerUnit();
    BigDecimal getEBTotalWt();
    String getLabAnalisysNo();
    BigDecimal getWtCut();
    BigDecimal getWeightCutTotal();
    BigDecimal getAdLsWeight();
    BigDecimal getNetBillWeight();
    BigDecimal getNetStockWeight();
    Integer getCityId();
    String getAreaCity();
    String getCommentsDetail();
    Integer getPurchaseGLAC();
    BigDecimal getScaleKart();
    String getBranchName();
    Integer getPurchaseOrderId();
    Integer getPurchaseOrderDetailId();
    Integer getOrderNo();
    Timestamp getOrderDate();
    BigDecimal getOrderItemRate();
    Integer getOrderItemRateUOMId();
    BigDecimal getOrderRateEquivalent();
}
