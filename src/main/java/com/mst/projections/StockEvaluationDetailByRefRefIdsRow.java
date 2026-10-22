package com.mst.projections;

import java.sql.Timestamp;

/** Row of USP_GetStockEvalautionDetailByRefRefIds (native-EXEC; SELECT * FROM #TmpFinal + Company/Organization columns) */
public interface StockEvaluationDetailByRefRefIdsRow {
    Integer getId();
    Integer getRefDocumentTypeId();
    Integer getRefDocIdNo();
    Integer getRefDocSubIdNo();
    String getRefDocumentType();
    Timestamp getDocDate();
    Integer getDocNo();
    String getRefDocNo();
    Integer getManualNo();
    Integer getJobOrderNo();
    Integer getSupplierCustomerId();
    String getPartyName();
    String getVehicleNo();
    Integer getGpNo();
    Integer getWarehouseId();
    String getWareHouseCode();
    Integer getItemId();
    String getItemName();
    String getCropBatch();
    Integer getJobLotId();
    String getJobLotCode();
    Integer getInvPackingTypeId();
    String getPackingType();
    Integer getItemUomId();
    String getPackUom();
    Double getQtyIn();
    Double getQtyOut();
    Double getWeightIn();
    Double getWeightOut();
    Integer getRateUom();
    Integer getRateUomId();
    Double getAVgRate();
    Double getAmountIn();
    Double getAmountOut();
    String getCompCountry();
    String getCompContactPerson();
    String getCompMobileA();
    String getCompMobileB();
    String getCompMobileC();
    String getCompEmailA();
    String getCompEmailB();
    byte[] getCompLogoImage();
    String getCompanyWebsite();
    String getCompanyFaxNo();
    String getOrgReportingRemarks();
}
