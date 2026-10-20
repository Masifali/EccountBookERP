package com.mst.projections;

/** Row of Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='DetailAccount' (native EXEC). */
public interface CoaDetailAccountRow {
    Integer getId();
    String getAccountCode();
    String getClassName();
    String getAccountType();
    String getAccountTitle();
    Integer getAccountTypeId();
    String getParentAccountTitle();
}
