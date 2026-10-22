package com.mst.projections;

/** Row of usp_getAccountsInfoByItemCategoryId (Stock / Sale) (JPQL). */
public interface AccountsInfoByItemCategoryRow {
    Integer getParentCodeId();
    String getParentAccountCode();
    Integer getAccountClass();
    Integer getAccountTypeId();
    Integer getBsNoteId();
    Integer getPlNoteId();
    Integer getAccount_Level();
}
