package com.mst.projections;

/** Row of Sp_SupplierCustomer_GetAllMethod @Activity='AllSupplierCustomerWithCityName' (native). */
public interface SupplierCustomerWithCityRow {
    Integer getId();
    String getCompanyName();
    Integer getGlAccountId();
    String getPartyCode();
    Integer getCityId();
    String getCityName();
    String getMobilePersonal();
}
