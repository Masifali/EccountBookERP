package com.mst.projections;

/** Row of Sp_PurchaseOrder_GetAllMethod @Activity='ReadByHeaderId_PurchaseOrderSupplierDispatchDetail' (native). */
public interface PurchaseOrderSupplierDispatchDetailRow {
    Integer getId();
    Integer getPurchaseOrderId();
    Integer getSupplierDispatchId();
    Integer getSupplierDispatchNo();
    Integer getPackingTypeId();
    String getPackingType();
    Integer getLoadingDetailId();
    Integer getItemId();
    String getItemName();
    String getItemCode();
    Integer getCropYearId();
    String getCropYear();
    Integer getItemUOMId();
    String getItemUomCode();
    Double getItemUomEquivalent();
    Double getItemQty();
    Double getNetWeight();
    Double getItemRate();
    Integer getRateUOMId();
    String getRateUomCode();
    Double getRateUomEquivalent();
    Integer getAmount();
}
