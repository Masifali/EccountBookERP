package com.mst.projections;

/** Row of Sp_ItemAllocation_GetAllMethod (ItemsAllocatedOrUnAllocatedToCompany) — JPQL projection. */
public interface ItemsAllocatedOrUnAllocatedRow {
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getId();
    String getItemName();
    Integer getItemId();
}
