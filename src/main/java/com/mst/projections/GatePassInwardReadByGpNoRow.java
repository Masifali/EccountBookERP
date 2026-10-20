package com.mst.projections;

import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='ReadByGpNo' (native-EXEC). */
public interface GatePassInwardReadByGpNoRow {
    Integer getId();
    Integer getGpSrNo();
    Date getGpDate();
    String getGatepassType();
    String getVehicleType();
    String getVehicleNo();
    Date getBiltyDate();
    String getBiltyNo();
    String getOtherRemarks();
    Double getNetPaid();
    Integer getNoOfPackages();
    Double getSupplierFirstWeight();
    Double getSupplierSecondWeight();
    Double getSupplierWeight();
    Double getFactoryWeight();
    Double getDifferenceWeight();
    String getSupplierContractCode();
    String getWareHouseName();
    String getCompanyName();
    String getVarietyName();
    Integer getPurchaseOrderId();
    Integer getRefDocumentTypeId();
    Integer getPackingTypeId();
    Integer getEntryUser();
    Integer getItemId();
    String getItemName();
    Double getItemQty();
}
