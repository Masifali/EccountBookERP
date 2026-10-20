package com.mst.projections;

import java.sql.Timestamp;

/** Row of USP_GetItemsFromMinAndMaxRateSchedule_FormHistory — native projection. */
public interface MinMaxRateFormHistoryRow {
    Timestamp getEffectedDate();
    Integer getId();
    Integer getParentCategoryId();
    String getParentCategory();
    Integer getItemCategoryId();
    String getItemCategory();
    Integer getItemTypeId();
    String getItemType();
    Integer getItemId();
    String getItemName();
    String getItemCode();
    Integer getRateUomId();
    String getRateUom();
    Double getMinRate();
    Double getMaxRate();
}
