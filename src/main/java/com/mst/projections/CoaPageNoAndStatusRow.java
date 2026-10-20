package com.mst.projections;

/** Row of Sp_COAAllocation_GetAllMethod @Activity='GetPageNoandStatusbyAccountId' (JPQL). */
public interface CoaPageNoAndStatusRow {
    Integer getCompanyId();
    Integer getChartofAccountId();
    Boolean getIsActive();
    String getGlPageNo();
}
