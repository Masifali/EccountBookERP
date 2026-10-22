package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetPendingGrnForPurchaseInvoice' */
public interface PurchaseInvoicePendingGrnRow {
    Integer getId();
    Timestamp getDocDate();
    Integer getDocNo();
    Integer getGpNo();
    Timestamp getGpDate();
    String getVehicleNo();
    String getBiltyNo();
    String getDeliveryTerm();
    Integer getGrnStatusId();
    String getGrnStatus();
    String getSupplierCustomer();
    Integer getPurchaseOrderId();
    Integer getSupplierCustomerId();
    BigDecimal getItemQty();
    Integer getPurchaseOrder();
    Integer getRefDocumentTypeId();
    String getPurchaseAgainst();
    Integer getBranchesId();
    String getBranchName();
}
