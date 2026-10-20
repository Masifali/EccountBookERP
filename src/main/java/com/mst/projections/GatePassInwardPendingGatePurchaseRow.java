package com.mst.projections;

import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='GetPandingGatePassforInvGrnForGatePurchase' (native). */
public interface GatePassInwardPendingGatePurchaseRow {
    Integer getId();
    Integer getGpSrNo();
    Date getGpDate();
    String getSupplierName();
    String getVehicleType();
    String getVehicleNo();
    String getBiltyNo();
    String getItemName();
    String getCrop();
    Integer getQty();
    Double getSupplierFirstWeight();
    Double getSupplierSecondWeight();
    Double getSupplierWeight();
    Double getFactoryWeight();
    String getStatus();
    Double getNetPaid();
}
