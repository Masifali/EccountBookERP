package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='GetCompleteCoaDetailByAccountId' (native: ChartofAccount c.* + AccountType + ClassName). */
public interface CoaCompleteDetailRow {
    Integer getId();
    String getAccountCode();
    String getAccountTitle();
    String getAccountTitleOtherLingo();
    Integer getAccountClass();
    Integer getAccountTypeId();
    String getAccountGroup();
    Integer getAccount_Level();
    Integer getPLNoteId();
    Integer getBSNoteId();
    String getParentAccountCode();
    Integer getParentCodeId();
    String getOtherErpCode();
    String getQrCode();
    Integer getFinancialYearId();
    Boolean getIsActive();
    Timestamp getEntryDate();
    Integer getEntryUser();
    Timestamp getModifyDate();
    Integer getModifyUser();
    Timestamp getPostDate();
    Integer getPostUser();
    Boolean getPostState();
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getBranchId();
    Integer getCustomerGroupId();
    Integer getCityId();
    Integer getCurrencyId();
    String getContactNo();
    Integer getSubSidiaryAcStatus();
    Integer getLvl01Id();
    Integer getLvl02Id();
    Integer getLvl03Id();
    Integer getLvl04Id();
    Long getUserLogId();
    String getAccountType();
    String getClassName();
}
