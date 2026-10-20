package com.mst.projections;

/** Row of Sp_COAAllocation_GetAllMethod @Activity='ReadAllForAllocation' (JPQL). */
public interface CoaReadAllForAllocationRow {
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getId();
    String getAccountCode();
    String getAccountTitle();
    Integer getChartofAccountId();
    Boolean getIsActive();
    String getGlPageNo();
    String getParentAccountCode();
}
