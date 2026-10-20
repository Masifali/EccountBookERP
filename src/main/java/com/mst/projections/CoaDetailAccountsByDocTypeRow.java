package com.mst.projections;

/** Row of Sp_COAAllocation_GetAllMethod @Activity='GetDetailAccountsByDocumentTypeId' (both result shapes) (native). */
public interface CoaDetailAccountsByDocTypeRow {
    Integer getId();
    String getAccountCode();
    String getAccountTitle();
    Integer getAccountClass();
    Integer getAccountTypeId();
    String getAccountGroup();
    Integer getAccount_Level();
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getCurrencyId();
    String getCurrencyCode();
}
