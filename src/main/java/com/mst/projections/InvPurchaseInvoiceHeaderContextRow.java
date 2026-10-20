package com.mst.projections;

import java.util.Date;

/** Row of InvPurchaseInvoiceRepository.readHeaderContext (JPQL): header values read by Sp_InvPurchaseInvoiceDetail_Insert / Sp_InvPurchaseInvoiceEmptyBags_Insert. */
public interface InvPurchaseInvoiceHeaderContextRow {
    Integer getOrganizationId();
    Integer getRevisionNo();
    Integer getModifyUser();
    Integer getCompanyId();
    Integer getDocumentTypeId();
    Date getDocDate();
    Integer getInvoiceTypeId();
    Integer getActionId();
    Integer getSupplierCustomerId();
    Integer getBillCalculateTypeId();
    Integer getFinancialYearId();
    Integer getBranchesId();
    Integer getEntryUser();
}
