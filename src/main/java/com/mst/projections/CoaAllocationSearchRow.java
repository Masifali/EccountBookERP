package com.mst.projections;

/** Row of Sp_COAAllocation_GetAllMethod @Activity='COAAllocationSearch' (native). */
public interface CoaAllocationSearchRow {
    Integer getId();
    Integer getChartofAccountId();
    String getAccountCode();
    String getAccountTitle();
    Integer getCompanyId();
    Integer getBranchId();
    Integer getOrganizationId();
    Boolean getIsActive();
    String getGLPageNo();
    Integer getAccountTypeId();
    String getParentAccountTitle();
    Integer getAccountClassId();
    String getAccountClass();
    Integer getPLNoteId();
    Integer getBSNoteId();
    Integer getCurrencyId();
    String getCurrencyCode();
}
