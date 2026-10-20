package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_WbTransation_GetAllMethod @Activity='PendingTicketsforContratorWages' (native UNION) */
public interface WbPendingTicketsForContractorWagesRow {
    String getTranType();
    Integer getReferenceDocTypeId();
    Integer getReferenceDocNoId();
    Integer getId();
    Integer getGpSrNo();
    Timestamp getGpDate();
    String getVehicleNo();
    BigDecimal getNoOfPackages();
    BigDecimal getFirstWeight();
    BigDecimal getSecondWeight();
    Integer getTicketNo();
    Integer getScaleSlipNo();
    Integer getSupplierCustomerId();
    String getCompanyName();
    String getOtherRemarks();
    String getVarietyName();
}
