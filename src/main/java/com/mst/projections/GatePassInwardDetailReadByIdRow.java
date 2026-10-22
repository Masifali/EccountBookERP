package com.mst.projections;

/** Row of Sp_GatePassInwardDetail_GetAllMethod @Activity='ReadById' (native, d.* + lookups). */
public interface GatePassInwardDetailReadByIdRow {
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
    String getItemName();
    String getPackingType();
    Double getEquivalent();
    String getUOMCode();
    String getJobLotDescription();
    String getWareHouseName();
    Integer getDocNo();
    String getCityName();
    Integer getOrderNo();
}
