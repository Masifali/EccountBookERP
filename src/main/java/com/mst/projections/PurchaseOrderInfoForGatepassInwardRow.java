package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of usp_GetPurchaseOrderInformationForGatepassInward (native) */
public interface PurchaseOrderInfoForGatepassInwardRow {
    Integer getDocumentTypeId();
    String getDocumentTypeDescription();
    Integer getOrderNo();
    String getSupplierName();
    Integer getItemId();
    String getItemName();
    String getPackUom();
    BigDecimal getOrderQty();
    BigDecimal getOrderWeight();
    BigDecimal getReceivedQty();
    BigDecimal getReceivedWeight();
    BigDecimal getBalQty();
    BigDecimal getBalWeight();
    BigDecimal getGrnReceivedQty();
    BigDecimal getGrnReceivedWeight();
    BigDecimal getBalQtyByGrn();
    BigDecimal getBalWeightByGrn();
    String getRemarksHeader();
    String getApprovedStatus();
    String getBranchName();
    String getOrderStatus();
    Timestamp getOrderExpiryDate();
}
