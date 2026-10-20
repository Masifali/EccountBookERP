package com.mst.projections;

import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='GetPandingGatePassforInvGrnForGhallaMandiPurchase' (native). */
public interface GatePassInwardPendingGhallaMandiRow {
    Integer getId();
    Integer getGpSrNo();
    Date getGpDate();
    Integer getLoadingContractorId();
    String getLoadingContractor();
    Integer getGhallaMandiId();
    String getGhallaMandi();
    String getVehicleType();
    String getVehicleNo();
    String getBiltyNo();
    String getOtherRemarks();
    Integer getNoOfPackages();
    String getItemName();
    Double getItemQty();
    Double getFactoryWeight();
    Double getNetPaid();
    Integer getCarriageContractorId();
    String getCarriageContractor();
}
