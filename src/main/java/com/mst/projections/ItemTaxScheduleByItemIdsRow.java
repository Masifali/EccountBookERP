package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_ItemTaxSchedule_GetAllMethod (GetTaxScheduleDetailbyItemIds) — native projection. */
public interface ItemTaxScheduleByItemIdsRow {
    Long getRecordNo();
    Integer getItemId();
    Integer getTaxNameId();
    String getTaxName();
    Integer getTaxScheduleId();
    Timestamp getEffectedDate();
    BigDecimal getTaxPercent();
    Integer getTaxGLAccountId();
}
