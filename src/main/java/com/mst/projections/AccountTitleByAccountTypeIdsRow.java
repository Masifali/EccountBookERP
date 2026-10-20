package com.mst.projections;

/** Row of USP_Accounts_GetAccountTitleByAccountTypeIds (native EXEC). */
public interface AccountTitleByAccountTypeIdsRow {
    Integer getId();
    String getAccountTitle();
    String getAccountCode();
    Integer getAccountTypeId();
    String getAccountType();
    Integer getNoteId();
    String getNoteTitle();
    String getParentAccountTitle();
    String getParentAccountCode();
    String getAccountClass();
    Integer getCurrencyId();
    String getCurrencyCode();
}
