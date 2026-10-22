package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='ReadByFinancialYearIdNOrganizationIdNCompanyIdNParentAccountCode' (native). */
public interface CoaReadByFinYearRow {
    Integer getId();
    String getAccountTitle();
    String getAccountCode();
    String getAccountCodeBigInt();
    Integer getAccountClass();
    Integer getAccountTypeId();
    String getAccountGroup();
    Integer getAccount_Level();
    Integer getPLNoteId();
    Integer getBSNoteId();
    String getParentAccountCode();
    String getParentAccountTitle();
    String getAccountTitleOtherLingo();
    String getOtherErpCode();
    String getQrCode();
    /** CASE WHEN AccountGroup='Group' THEN 1 ELSE COAAllocation.IsActive END - int typed by SQL Server. */
    Integer getIsActive();
    Timestamp getEntryDate();
    Integer getEntryUser();
    Timestamp getModifyDate();
    Integer getModifyUser();
    Timestamp getPostDate();
    Integer getPostUser();
    Boolean getPostState();
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getFinancialYearId();
    String getAccountType();
    String getPLNote();
    String getBsNote();
    String getAccountClassName();
}
