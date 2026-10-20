package com.mst.projections;

/** Row (result set 2 = details) of Sp_PurchaseOrder_GetAllMethod @Activity='PurchaseOrderModernHistory' (native). */
public interface PurchaseOrderModernHistoryDetailRow {
    Integer getId();
    Integer getOrderId();
    String getItemName();
    String getPackUom();
    Double getOrderItemQty();
    Double getNetWeight();
    Double getOrderItemRate();
    String getRateUom();
    Double getAmount();
    String getJobLotDescription();
    String getCityArea();
    String getCrop();
    Double getBagWeight();
    Double getBagPrice();
    String getMoisture();
    Integer getLabSampleId();
    Integer getSampleAnalysisNo();
    String getOrderRemarks();
}
