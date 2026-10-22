package com.mst.projections;

/** Row of Sp_Item_GetAllMethod @Activity='ReadItemAllocationByItemId' (native) */
public interface ItemAllocationRow {
    Integer getId();
    Integer getItemId();
    Integer getBranchId();
    Integer getCompanyId();
    Integer getOrganizationId();
    Boolean getIsActive();
    String getBranchName();
    String getItemAllocationlist();
}
