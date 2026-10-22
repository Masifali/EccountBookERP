package com.mst.projections;

/** Row (Id, CompanyName) of Sp_SupplierCustomer_GetAllMethod 'GetAllTaxableSupplierCustomer' / 'GetSalesManForSupplierustomer' / 'GetSupplierustomerForPartyProcessing' (JPQL) and 'GetCustomerIdByLcOrderAndFcReceipt' / 'GetCustomerExistInExportInvoices' (native). */
public interface SupplierCustomerIdNameRow {
    Integer getId();
    String getCompanyName();
}
