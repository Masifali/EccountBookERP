package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_SupplierCustomer_GetAllMethod @Activity='GetSupplierBySaleOrderNo' (native EXEC). */
public interface SupplierBySaleOrderRow {
    Integer getId();
    String getCompanyName();
    String getSupCustCode();
    Integer getSaleOrderId();
    Integer getDocNo();
    Timestamp getSoHDocDate();
    Integer getDocumentTypeId();
}
