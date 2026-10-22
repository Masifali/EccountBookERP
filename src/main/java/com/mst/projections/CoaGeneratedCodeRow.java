package com.mst.projections;

/** Row of dbo.GenerateNewAccountCodeByParentAccountCode (used by Proc_ChartofAccount_Insert and Sp_ChartofAccount_GetAllMethodFromCOA 'NewCodeByParentCode'). Native. */
public interface CoaGeneratedCodeRow {
    String getAccountCode();
    Integer getAccountClass();
    String getAccountType();
    String getAccount_Level();
    String getParentAccountCode();
    String getParentCodeTitle();
}
