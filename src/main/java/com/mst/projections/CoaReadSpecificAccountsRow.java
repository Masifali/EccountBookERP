package com.mst.projections;

import java.time.LocalDateTime;

/** Row of Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='ReadSpecificAccounts' (JPQL). */
public interface CoaReadSpecificAccountsRow {
    Integer getId();
    String getAccountCode();
    String getAccountTitle();
    Integer getAccountClass();
    Integer getAccountTypeId();
    String getAccountGroup();
    Integer getAccount_Level();
    Integer getPlNoteId();
    Integer getBsNoteId();
    String getParentAccountCode();
    Boolean getIsActive();
    LocalDateTime getEntryDate();
    Integer getEntryUser();
    LocalDateTime getModifyDate();
    Integer getModifyUser();
    LocalDateTime getPostDate();
    Integer getPostUser();
    Boolean getPostState();
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getFinancialYearId();
}
