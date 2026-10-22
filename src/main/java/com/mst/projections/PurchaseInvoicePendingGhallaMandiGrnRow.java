package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetPendingGhallaMandiGrnForInvoice' */
public interface PurchaseInvoicePendingGhallaMandiGrnRow {
    Integer getId();
    Timestamp getDocDate();
    Integer getDocNo();
    Integer getGhallaMandiId();
    String getGhallaMandi();
    Integer getGpNo();
    String getVehicleNo();
    String getBiltyNo();
    Integer getSupplierCustomerId();
    String getLoadingContractor();
    BigDecimal getItemQty();
    String getItemCode();
    String getItemName();
    String getPackUom();
}
