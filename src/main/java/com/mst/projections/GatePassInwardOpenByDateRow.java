package com.mst.projections;

import java.sql.Timestamp;
import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='ReadByGPDate' (native, #TmpResultOpenGP final select). */
public interface GatePassInwardOpenByDateRow {
    Integer getId();
    Integer getDocumentTypeId();
    String getGatepassType();
    Date getGpDate();
    Integer getGpSrNo();
    String getOrderType();
    String getVehicleType();
    String getVehicleNo();
    String getBiltyNo();
    Double getFreight();
    Double getAdvanceByFactory();
    Double getAdvanceByParty();
    Double getTotalPayableFreight();
    Double getNetPaid();
    String getVarietyName();
    String getWareHouseName();
    Integer getItemQty();
    String getSupplierName();
    String getOtherRemarks();
    Timestamp getInTime();
    Integer getOrderNo();
    Double getSupplierFirstWeight();
    Double getSupplierSecondWeight();
    Double getSupplierWeight();
    Double getFactoryWeight();
    Double getDifferenceWeight();
    String getCityName();
    String getStatus();
    Integer getRefDocumentTypeId();
    Integer getNoOfAttachments();
    Double getRateCut();
    String getRateCutRemarks();
    Double getWeightComparedToPoWt();
    Double getPackUnit();
    Double getAccessWeight();
    String getApprovalStatus();
    Boolean getRefferedInWbOrLab();
}
