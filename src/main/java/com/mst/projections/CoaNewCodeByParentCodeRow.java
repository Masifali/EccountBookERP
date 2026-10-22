package com.mst.projections;

/** Row of Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='NewCodeByParentCode' (native EXEC). */
public interface CoaNewCodeByParentCodeRow {
    String getAccount_Level();
    String getAccountCode();
    Integer getAccountClass();
    String getParentAccountCode();
    String getParentCodeTitle();
    String getAccountType();
    Integer getCustomerGroupId();
}
