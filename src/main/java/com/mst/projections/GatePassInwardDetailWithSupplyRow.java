package com.mst.projections;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='ReadDetailByGatePassInward' (native, d.* + lookups). */
public interface GatePassInwardDetailWithSupplyRow {
    Integer getId();
    Integer getGatePassInwardId();
    Integer getSupplyScheduleId();
    Integer getItemId();
    String getCropYear();
    Integer getJobLotId();
    Integer getItemUOMId();
    Double getItemQty();
    Integer getWareHouseId();
    Double getPackUnit();
    Double getWeight();
    Integer getPackingTypeId();
    Integer getPurchaseOrderId();
    Integer getPurchaseOrderDetailId();
    Integer getCityId();
    Integer getSupplierCustomerId();
    Integer getRefDocumentTypeId();
    String getRemarksDetail();
    String getPackingType();
    Integer getDocNo();
    String getItemName();
    String getJobLotDescription();
    Double getEquivalent();
    String getUOMCode();
    String getWareHouseName();
    String getCityName();
    Integer getOrderNo();
}
