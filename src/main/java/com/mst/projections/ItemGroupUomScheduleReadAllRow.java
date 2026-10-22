package com.mst.projections;

/** Row of Sp_ItemGroup_UOMSchedule_GetAllMethod @Activity='ReadAll' (JPQL) */
public interface ItemGroupUomScheduleReadAllRow {
    Integer getId();
    Integer getItemGroupId();
    Integer getUomId();
    String getEquivalent();
    String getItemGroupName();
    String getScheduleUnit();
}
