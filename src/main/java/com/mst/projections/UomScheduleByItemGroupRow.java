package com.mst.projections;

/** Row of Sp_UOMSchedule_GetAllMethod @Activity='ReadByItemGroupId' (JPQL) */
public interface UomScheduleByItemGroupRow {
    Integer getId();
    Integer getItemGroupId();
    Integer getUomId();
    String getEquivalent();
    String getItemGroupName();
    Double getQtyEquivalent();
}
