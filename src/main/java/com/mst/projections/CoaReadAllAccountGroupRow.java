package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='ReadAllAccountGroup' (native). */
public interface CoaReadAllAccountGroupRow {
    Integer getId();
    String getAccountCode();
    String getAccountTitle();
    Integer getAccountClass();
    Integer getAccountTypeId();
    String getAccountGroup();
    Integer getAccount_Level();
    Integer getPLNoteId();
    Integer getBSNoteId();
    String getParentAccountCode();
    String getAccountTitleOtherLingo();
    String getOtherErpCode();
    String getQrCode();
    Boolean getIsActive();
    Integer getFinancialYearId();
    Timestamp getEntryDate();
    Integer getEntryUser();
    Timestamp getModifyDate();
    Integer getModifyUser();
    Timestamp getPostDate();
    Integer getPostUser();
    Boolean getPostState();
    Integer getOrganizationId();
    Integer getCompanyId();
    String getParentAccountTitle();
    String getClassName();
    String getAccountType();
}
