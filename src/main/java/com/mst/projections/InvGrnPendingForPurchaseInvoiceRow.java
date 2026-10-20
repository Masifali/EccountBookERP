package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_InvGRN_GetAllMethod @Activity='GetPendingGrnForPurchaseInvoice' (native). */
public interface InvGrnPendingForPurchaseInvoiceRow {
    Integer getId();
    Timestamp getDocDate();
    Integer getDocNo();
    Integer getGpNo();
    String getVehicleNo();
    String getBiltyNo();
    String getSupplierCustomer();
    Integer getPurchaseOrderId();
    Integer getSupplierCustomerId();
    Double getItemQty();
    String getItemName();
    Double getOrderItemRate();
    Integer getPurchaseOrder();
    String getItemCode();
    String getUOMCode();
    Integer getWarehouseId();
}
