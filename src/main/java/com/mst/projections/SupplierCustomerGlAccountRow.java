package com.mst.projections;

/** Row of Sp_SupplierCustomer_GetAllMethod @Activity='GetGlAccountIdandCompanyNameBySupplierCustomerId' (JPQL). */
public interface SupplierCustomerGlAccountRow {
    Integer getId();
    Integer getGlAccountId();
    String getCompanyName();
    Integer getAdvanceGlAcId();
}
