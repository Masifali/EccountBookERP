package com.mst.projections;

/** Row of Sp_COAAllocation_GetAllMethod @Activity='ChartOfAccountPendingForAllocation' (JPQL) and 'AllocatedAndUnAllocatedAccountsToCompany' (native) - same columns. */
public interface CoaAllocationStatusRow {
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getId();
    String getAccountCode();
    String getAccountTitle();
    Integer getChartofAccountId();
    Integer getAccountTypeId();
}
