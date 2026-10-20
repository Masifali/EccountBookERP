package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_ItemTaxSchedule_GetAllMethod (GetAllTaxSchedule) — native projection over TaxScheduleMain. */
public interface TaxScheduleMainRow {
    Integer getTaxNameId();
    String getTaxName();
    BigDecimal getTaxPercent();
}
