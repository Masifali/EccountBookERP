package com.mst.models.dto;

import java.math.BigDecimal;

/** InvDeffrmItemUomSchedule fields; tenant and audit values come from the session. */
public class InventoryUomScheduleRequest {
    public int id, itemId, scheduleUnitId;
    public BigDecimal equivalent, qtyEquivalent;
    public boolean baseRateUom, basePackUom, baseSecondaryUom, active;
}
