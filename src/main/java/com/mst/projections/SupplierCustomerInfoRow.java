package com.mst.projections;

/** Row of Sp_SupplierCustomer_GetAllMethod @Activity='GetSupplierCustomerInfoByGLAccountId' (JPQL). */
public interface SupplierCustomerInfoRow {
    Integer getId();
    String getCompanyName();
    String getFirstName();
    String getLastName();
    String getReportingTitle();
    String getMobileOffice();
    String getMobilePersonal();
    String getEmail();
}
