package com.mst.projections;

import java.sql.Timestamp;
import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='GatepassHistoryForRefdocumentTypeWise' (native, #HistoryRef). */
public interface GatePassInwardHistoryRefDocTypeRow {
    Long getRecordNo();
    Integer getId();
    Date getGpDate();
    Integer getGpSrNo();
    String getCompanyName();
    String getVehicleType();
    String getVehicleNo();
    String getBiltyNo();
    Integer getItemQty();
    String getOtherRemarks();
    Timestamp getInTime();
    Timestamp getOutTime();
    String getUserName();
    String getDescription();
    Double getFreight();
    Double getSupplierFirstWeight();
    Double getSupplierSecondWeight();
    Double getSupplierWeight();
    String getVarietyName();
    Integer getNoOfAttachments();
}
