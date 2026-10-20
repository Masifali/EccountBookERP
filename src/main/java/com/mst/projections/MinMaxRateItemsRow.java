package com.mst.projections;

import java.sql.Timestamp;

/**
 * Row of usp_GetItemsFromMinAndMaxRateSchedule — native projection.
 * MinRate/MaxRate are Integer on purpose: the proc creates #TmpResult with "0 as MinRate / 0 as MaxRate" (int columns)
 * and then UPDATEs them from the float schedule values, so the procedure has always returned truncated ints.
 */
public interface MinMaxRateItemsRow {
    Integer getId();
    String getItemName();
    String getItemCodeNew();
    Integer getRateUomId();
    String getRateUom();
    Timestamp getEffectedDate();
    Integer getMinRate();
    Integer getMaxRate();
    Integer getParentcategoryId();
    String getParentcategory();
    String getCategoryDescription();
    String getTypeDescription();
}
