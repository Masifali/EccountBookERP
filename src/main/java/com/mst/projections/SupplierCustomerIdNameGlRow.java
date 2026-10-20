package com.mst.projections;

/** Row (Id, CompanyName, GlAccountId) of Sp_SupplierCustomer_GetAllMethod 'GetSupplierustomerByCustomerGroupId' (native) and 'GetSameGLAccountPartiesByPartyId' (JPQL). */
public interface SupplierCustomerIdNameGlRow {
    Integer getId();
    String getCompanyName();
    Integer getGlAccountId();
}
