package com.mst.projections;

import java.sql.Timestamp;

/** Row of usp_GetLastItemMinAndMaxRateByItemIdAndUomId — native projection (TOP 1). */
public interface LastMinMaxRateRow {
    Double getMinRate();
    Double getMaxRate();
    Timestamp getPreviousDate();
}
