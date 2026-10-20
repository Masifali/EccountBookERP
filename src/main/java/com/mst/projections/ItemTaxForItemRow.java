package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_ItemTaxSchedule_GetAllMethod (GetItemTaxScheduleForItemId) — native projection over V_TaxScheduleForTax. */
public interface ItemTaxForItemRow {
    Integer getTaxNameId();
    String getTaxName();
    Integer getTaxScheduleId();
    Timestamp getEffectedDate();
    BigDecimal getTaxPercent();
    Integer getTaxGLAccountId();
}
