package com.mst.projections;

import java.sql.Timestamp;

/** Helper row used by Sp_ItemPricingSchedule_Insert / fed.usp_ItemPricingSchedule_Insert pre-insert validation (last effected date + item name) — native projection. */
public interface LastPricingRow {
    Timestamp getLastEffectedDate();
    String getItemName();
    String getGroupName();
}
