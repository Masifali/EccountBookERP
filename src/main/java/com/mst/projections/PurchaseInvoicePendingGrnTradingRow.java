package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetPendingGrnForPurchaseInvoiceTrading' */
public interface PurchaseInvoicePendingGrnTradingRow {
    Integer getId();
    Timestamp getDocDate();
    Integer getDocNo();
    Integer getGpNo();
    String getVehicleNo();
    String getBiltyNo();
    String getSupplierCustomer();
    Integer getPurchaseOrderId();
    Integer getSupplierCustomerId();
    BigDecimal getItemQty();
    String getItemName();
    BigDecimal getOrderItemRate();
    Integer getPurchaseOrder();
    String getItemCode();
    String getUOMCode();
    Integer getWarehouseId();
}
