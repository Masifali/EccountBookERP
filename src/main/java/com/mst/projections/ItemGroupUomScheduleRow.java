package com.mst.projections;

/** Row of Sp_ItemGroup_UOMSchedule_GetAllMethod @Activity='ReadById' (JPQL) */
public interface ItemGroupUomScheduleRow {
    Integer getId();
    Integer getItemGroupId();
    Integer getUomId();
    String getEquivalent();
    Integer getOrganizationId();
    Integer getCompanyId();
}
