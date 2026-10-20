package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_InvGRN_GetAllMethod @Activity='GrnLoadForStoreInvoicePurchaseOrderBase' (native). */
public interface InvGrnLoadForStoreInvoiceOrderRow {
    Integer getId();
    Timestamp getDocDate();
    Integer getDocNo();
    Integer getGpNo();
    Integer getRefDocumentTypeId();
    String getOrderType();
    String getVehicleNo();
    String getBiltyNo();
    String getSupplierName();
    Integer getOrderNo();
    Integer getSupplierCustomerId();
    Double getItemQty();
    String getItemName();
    String getItemCode();
    String getUOMCode();
    String getWareHouseName();
    Integer getPurchaseOrderId();
    Integer getBillCalculateTypeId();
    String getBillCalculateType();
    String getBranchName();
    Integer getItemConditionId();
    String getConditionStatus();
}
