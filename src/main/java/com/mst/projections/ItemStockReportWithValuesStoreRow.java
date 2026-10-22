package com.mst.projections;

import java.sql.Date;

/**
 * Row of Sp_ItemStockReportWithValues_Store for every named @Activity branch
 * (ItemStockSummary, ItemandCropYearStockSummary, ItemandCropYearandWarehouseStockSummary,
 * ItemandPackSizeStockSummary, ItemandPackSizeandPackingTypeStockSummary, ItemandPackingTypeStockSummary,
 * ItemandWarehouseStockSummary, JobLotandItemStockSummary, WarehouseandJoblotandItemStockSummary):
 * ReportCriteria + #TMPFinalData f.* + AvgRate + Company/Organization columns + FromDate/ToDate.
 * Numeric columns are declared float in #TMPFinalData, hence Double.
 */
public interface ItemStockReportWithValuesStoreRow {
    String getReportCriteria();
    String getCategoryDescription();
    String getTypeDescription();
    Integer getAccountId();
    String getAccountTitle();
    Integer getItemId();
    String getItemName();
    String getItemCode();
    Integer getItemConditionId();
    String getItemCondition();
    Integer getRackId();
    String getRackName();
    String getCropYear();
    String getPackUom();
    String getPackingType();
    String getWareHouseName();
    String getJobLot();
    Double getOpQty();
    Double getQtyIn();
    Double getQtyOut();
    Double getBalQty();
    Double getBalQtyByWeight();
    Double getOpWeight();
    Double getWeightIn();
    Double getWeightOut();
    Double getBalWeight();
    Double getOpAmount();
    Double getAmountIn();
    Double getAmountOut();
    Double getBalAmount();
    Integer getPackUomId();
    Integer getPackingTypeId();
    Double getManualRate();
    Double getManualAmount();
    /** Round(BalAmount / BalWeight, 0) when both > 0 else 0 */
    Double getAvgRate();
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
    Date getFromDate();
    Date getToDate();
}
