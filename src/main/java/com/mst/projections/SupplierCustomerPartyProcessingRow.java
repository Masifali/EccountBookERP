package com.mst.projections;

/** Row of Sp_SupplierCustomer_GetAllMethod @Activity='FormHistoryForPartyProcessing' (JPQL). */
public interface SupplierCustomerPartyProcessingRow {
    Integer getId();
    String getCompanyName();
    Integer getGlAccountId();
    String getGlAccount();
    String getMobilePersonal();
    Integer getCountryId();
    String getCountryName();
    String getCityName();
    String getCnic();
    String getAddress1();
    String getPictureURL();
}
