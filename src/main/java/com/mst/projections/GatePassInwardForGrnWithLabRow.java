package com.mst.projections;

import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='ReadByGpNoForGRNWithLabAttach' (native). */
public interface GatePassInwardForGrnWithLabRow {
    Double getSupplierWeight();
    Double getNetWbWeight();
    Integer getId();
    Integer getGpSrNo();
    Date getGpDate();
    String getVehicleType();
    String getVehicleNo();
    String getBiltyNo();
    String getOtherRemarks();
    Double getNetPaid();
    Integer getNoOfPackages();
    Double getSupplierFirstWeight();
    Double getSupplierSecondWeight();
    Double getFactoryWeight();
    Double getDifferenceWeight();
    String getSupplierContractCode();
    String getWareHouseName();
    String getCompanyName();
    Integer getPurchaseOrderId();
    String getStatus();
    Integer getSupplierCustomerId();
    String getAnalystName();
    Double getDeductionWeight();
    Integer getRefDocumentTypeId();
}
