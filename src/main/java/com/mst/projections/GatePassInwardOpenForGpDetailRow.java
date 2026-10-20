package com.mst.projections;

import java.sql.Timestamp;
import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='GetOpenGatePassForGpDetail' (native). */
public interface GatePassInwardOpenForGpDetailRow {
    Integer getId();
    String getGatepassType();
    Integer getRefDocumentTypeId();
    Date getGpDate();
    Integer getGpSrNo();
    String getVehicleType();
    String getVehicleNo();
    String getBiltyNo();
    Integer getItemQty();
    String getSupplierName();
    String getSupplierCNICNO();
    String getSupplierMobileNo();
    String getDriverName();
    String getDriverCNICNO();
    String getDriverMobileNo();
    String getDistrict();
    String getTehsil();
    String getCityName();
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
