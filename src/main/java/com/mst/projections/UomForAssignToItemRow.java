package com.mst.projections;

/** Row of Sp_UOMSchedule_GetAllMethod @Activity='GetUomForAssigntoItem' (JPQL) */
public interface UomForAssignToItemRow {
    Integer getId();
    Integer getItemGroupId();
    Integer getUomId();
    String getEquivalent();
    Integer getOrganizationId();
    Integer getCompanyId();
    Double getQtyEquivalent();
}
