package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of USP_InvGrn_GetTransporterAndFreight (native) */
public interface GrnTransporterFreightRow {
    Integer getMainId();
    Integer getDocNo();
    Timestamp getDocDate();
    Integer getGpNo();
    Integer getTransporterSupCustId();
    Integer getTransporter();
    String getAccountTitle();
    BigDecimal getCarriageAmount();
    String getBiltyNo();
    String getVehicleNo();
    Integer getFreightOnCash();
    Integer getFreightId();
    BigDecimal getTotalPaidAmount();
    BigDecimal getTotalChargeToPartyAmount();
    BigDecimal getTotalDeductionAmount();
    BigDecimal getTotalAdanceByPartyAmount();
    BigDecimal getTotalAdvanceByFactoryAmount();
}
