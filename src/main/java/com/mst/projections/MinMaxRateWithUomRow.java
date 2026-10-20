package com.mst.projections;

/** Helper row used by fed.usp_ItemPricingSchedule_Insert pre-insert validation (TOP 1 min/max rate with its UOM equivalent) — native projection. */
public interface MinMaxRateWithUomRow {
    Double getMinRate();
    Double getMaxRate();
    Double getMinMaxRateUom();
}
