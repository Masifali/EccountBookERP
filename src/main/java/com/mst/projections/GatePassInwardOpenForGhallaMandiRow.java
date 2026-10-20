package com.mst.projections;

import java.sql.Timestamp;
import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='GetOpenGatePassForGhallaMandiGpDetail' (native). */
public interface GatePassInwardOpenForGhallaMandiRow {
    Integer getId();
    String getGatepassType();
    Integer getRefDocumentTypeId();
    Date getGpDate();
    Integer getGpSrNo();
    String getVehicleType();
    String getVehicleNo();
    String getBiltyNo();
    Integer getItemQty();
    String getLoadingContractor();
    String getDriverName();
    String getDriverMobileNo();
    String getGhallaMandi();
    String getCarriageContractor();
    String getStatus();
    String getWbStatus();
    Double getSupplierFirstWeight();
    Double getSupplierSecondWeight();
    Double getSupplierWeight();
    Double getFactoryWeight();
    String getOtherRemarks();
    Timestamp getInTime();
    Timestamp getEntryDate();
    String getEntryUser();
    Timestamp getModifyDate();
    String getModifyUser();
}
