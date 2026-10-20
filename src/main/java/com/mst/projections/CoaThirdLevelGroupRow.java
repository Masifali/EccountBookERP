package com.mst.projections;

/** Row of Sp_COAAllocation_GetAllMethod @Activity='Get3rdLevelGroupAccounts' (JPQL). */
public interface CoaThirdLevelGroupRow {
    Integer getId();
    String getAccountCode();
    String getParentAccountCode();
    String getAccountTitle();
}
