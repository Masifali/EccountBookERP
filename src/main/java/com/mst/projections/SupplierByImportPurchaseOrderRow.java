package com.mst.projections;

import java.sql.Date;

/** Row of Sp_SupplierCustomer_GetAllMethod @Activity='GetSupplierByImportPurchaseOrderNo' (native). */
public interface SupplierByImportPurchaseOrderRow {
    Integer getId();
    String getCompanyName();
    String getSupCustCode();
    Integer getPurchaseOrderId();
    String getDocNo();
    Date getPoHDocDate();
    Integer getDocumentTypeId();
}
