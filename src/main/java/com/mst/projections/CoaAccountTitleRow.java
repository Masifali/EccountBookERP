package com.mst.projections;

/** Row (Id, AccountTitle, AccountCode) of Sp_COAAllocation_GetAllMethod @Activity='GetAccountTitleByAccountTypeIds' (native EXEC) and 'GetAccountTitleByAccountClassIds' (native). */
public interface CoaAccountTitleRow {
    Integer getId();
    String getAccountTitle();
    String getAccountCode();
}
