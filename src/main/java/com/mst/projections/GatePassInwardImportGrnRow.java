package com.mst.projections;

import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='GpNoAllforInvGrnForImport' (native). */
public interface GatePassInwardImportGrnRow {
    Integer getId();
    String getLcOrderNo();
    Integer getGatePassOutwardId();
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
    String getCompanyName();
    String getVehicleType();
    Double getFreight();
    String getContainer();
    String getContainer1();
    Integer getExImLcOrderId();
    Double getWbGrossWeight();
}
