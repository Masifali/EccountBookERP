package com.mst.projections;

/** Row of Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='GetAllAccountsByCustomGroupId' (JPQL). */
public interface CoaByCustomGroupIdRow {
    Integer getAcLookUpsId();
    Integer getChartOfAccountId();
    String getAccountCode();
    String getAccountTitle();
}
