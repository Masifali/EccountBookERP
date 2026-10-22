package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetTransporterAndFreightFromGrn' */
public interface PurchaseInvoiceTransporterFreightRow {
    Integer getMainId();
    Integer getDocNo();
    Timestamp getDocDate();
    Integer getGpNo();
    Integer getTransporterSupCustId();
    Integer getTransporter();
    BigDecimal getCarriageAmount();
    String getAccountTitle();
    String getBiltyNo();
    String getVehicleNo();
    Integer getFreightOnCash();
    Integer getFreightId();
}
