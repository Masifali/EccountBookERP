package com.mst.projections;

/** Row of Sp_ItemGroup_GetAllMethod @Activity='ReadById' (JPQL) */
public interface ItemGroupReadByIdRow {
    Integer getGroupId();
    String getItemGroupName();
    Integer getOrganizationId();
    Integer getCompanyId();
}
