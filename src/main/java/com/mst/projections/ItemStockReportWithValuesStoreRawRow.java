package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Date;

/**
 * Row of Sp_ItemStockReportWithValues_Store when ISNULL(@Activity,'') = '' (un-grouped #f f.* + AvgRate
 * + Company/Organization columns + FromDate/ToDate). Different column set from the named-activity branches.
 */
public interface ItemStockReportWithValuesStoreRawRow {
    Integer getItemId();
    Integer getItemClassGroupId();
    Integer getInventoryParentCategoriesId();
    String getTypeofTypes();
    String getWareHouseName();
    Integer getRackId();
    String getRackName();
    String getCategoryDescription();
    String getTypeDescription();
    String getItemName();
    String getItemCode();
    Integer getAccountId();
    String getAccountTitle();
    /** constant '****' in the procedure */
    String getCropBatch();
    /** constant '****' in the procedure */
    String getJobLot();
    /** constant '****' in the procedure */
    String getPackingType();
    /** constant 0 in the procedure */
    Integer getInvPackingTypeId();
    Integer getItemUomId();
    String getPackSize();
    Integer getItemConditionId();
    String getItemCondition();
    BigDecimal getOp_Qty();
    BigDecimal getQtyIn();
    BigDecimal getQtyOut();
    BigDecimal getBalQty();
    BigDecimal getOp_Weight();
    BigDecimal getWeightIn();
    BigDecimal getWeightOut();
    BigDecimal getBalWeight();
    BigDecimal getOp_Amount();
    BigDecimal getAmountIn();
    BigDecimal getAmountOut();
    BigDecimal getBalAmount();
    /** BalAmount / BalWeight when both > 0 else 0 (not rounded in this branch) */
    BigDecimal getAvgRate();
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
