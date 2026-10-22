package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_SupplierCustomer_GetAllMethod @Activity='SupplierByPurchaseOrderNo' (native EXEC). */
public interface SupplierByPurchaseOrderRow {
    Integer getPurchaseOrderId();
    String getOrderCategoryName();
    Integer getDocNo();
    String getDeliveryTerm();
    Timestamp getDocDate();
    Integer getId();
    String getCompanyName();
    String getSupCustCode();
    Integer getItemId();
    String getItemName();
    String getCityArea();
}
