package com.mst.projections;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='GpNoBySupplyOrderPendingForGrn' (native). */
public interface GatePassInwardSoPendingRow {
    Integer getPoDetailId();
    Integer getOrderId();
    Integer getOrderNo();
    Integer getItemId();
    String getItemName();
    String getCropYear();
    Integer getJobId();
    Integer getPackingTypeId();
    String getUOM();
    Integer getUOMId();
    Double getQty();
    Double getGrossWeight();
    Double getEbUnit();
    Double getEbTotal();
    Integer getWtCut();
    Integer getWtCutTotal();
    Integer getAddLesswt();
    Double getNetWeight();
    Double getStockWeight();
    Integer getWareHouse();
    Integer getCity();
    Integer getGpId();
    Integer getGpSrNo();
    String getVehicleNo();
    String getBiltyNo();
    Double getSupplierFirstWeight();
    Double getSupplierSecondWeight();
    Double getSupplierWeight();
    Double getFactoryWeight();
    Double getNetPaid();
    Integer getSupplierCustomerId();
    String getSupplierName();
    String getDeliveryTerm();
}
