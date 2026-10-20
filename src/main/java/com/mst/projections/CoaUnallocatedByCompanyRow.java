package com.mst.projections;

/** Row of Sp_COAAllocation_GetAllMethod @Activity='GetUnallocatedAccountsByCompanyId' (native). The proc's second "CompanyId" column (COAAllocation.CompanyId) is exposed as AllocatedCompanyId. */
public interface CoaUnallocatedByCompanyRow {
    Integer getId();
    String getAccountTitle();
    Integer getAccount_Level();
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getAllocatedCompanyId();
}
