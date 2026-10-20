package com.mst.projections;

import java.time.LocalDateTime;

/** Row of Sp_SaleOrder_GetAllMethod (GetSupplierBySaleOrderNo) - JPQL projection. */
public interface SaleOrderSupplierRow {
    Integer getId();
    String getCompanyName();
    String getSupCustCode();
    Integer getSoHId();
    Integer getDocNo();
    LocalDateTime getSoHDocDate();
    Integer getDocumentTypeId();
}
