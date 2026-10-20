package com.mst.projections;

import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='GetShipmentDataByGpNoForImport' (native). */
public interface GatePassInwardImportShipmentRow {
    Integer getId();
    Integer getExImLcOrderId();
    String getLcOrderNo();
    Integer getShipmentId();
    String getShipmentNo();
    String getOrderType();
    Integer getPurchaseOrderId();
    Date getGpDate();
    Integer getGpSrNo();
    String getVehicleNo();
    String getBiltyNo();
    String getVarietyName();
    Integer getNoOfPackages();
    Double getFactoryWeight();
    Double getDifferenceWeight();
    String getGatepassType();
    String getSupplierContractCode();
    Integer getSupplierCustomerId();
    String getCompanyName();
    String getVehicleType();
    Double getFreight();
    String getContainer();
    String getContainer1();
    Double getWbGrossWeight();
}
