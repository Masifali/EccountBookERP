package com.mst.projections;

import java.sql.Timestamp;
import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='GpNoPandingforInvGrn' (native, #GpNoPandingforInvGrn final select). */
public interface GatePassInwardPendingForGrnRow {
    Integer getId();
    Integer getGpSrNo();
    Date getGpDate();
    String getSupplierName();
    String getVehicleNo();
    String getBiltyNo();
    String getVarietyName();
    Integer getQty();
    Double getSupplierFirstWeight();
    Double getSupplierSecondWeight();
    Double getSupplierWeight();
    Double getFactoryWeight();
    Double getReceivedWeight();
    Double getStockWeight();
    String getStatus();
    String getOrderType();
    String getVehicleType();
    Double getNetPaid();
    Integer getRefDocumentTypeId();
    Timestamp getEntryDate();
    String getEntryUser();
    Timestamp getModifyDate();
    String getModifyUser();
    Integer getDetailIdsCount();
    Integer getFreightOnCash();
    Integer getFreightId();
    Integer getLabId();
    Integer getLabNo();
    Double getAccessWeight();
    String getSupplierContractCode();
}
