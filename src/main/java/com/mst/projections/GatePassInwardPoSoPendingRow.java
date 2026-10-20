package com.mst.projections;

import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='GpNoByPurchaserOrderAndSupplyOrderPendingForGrn' (native UNION ALL; column names from the first SELECT). */
public interface GatePassInwardPoSoPendingRow {
    String getRefDocTypeDesc();
    Integer getGpId();
    Date getGpDate();
    Integer getGpSrNo();
    String getVehicleType();
    String getVehicleNo();
    String getBiltyNo();
    Double getSupplierFirstWeight();
    Double getSupplierSecondWeight();
    Double getSupplierWeight();
    Double getWbNetWeight();
    Integer getRefDocumentTypeId();
    Integer getRefDocumentEntryNo();
    Integer getPoSpohId();
    Integer getPoSpodId();
    Integer getPoSpoDocNo();
    Integer getPurchaseOrderId();
    Integer getPurchaserOrderNo();
    Integer getPurchaserOrderDetailId();
    Integer getSupplierCustomerId();
    String getSupplierName();
    Integer getItemId();
    String getItemName();
    String getCropYear();
    Integer getPackSizeId();
    String getUOMCode();
    Double getQty();
    Double getNetWeight();
}
