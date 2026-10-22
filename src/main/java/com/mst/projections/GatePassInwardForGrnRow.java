package com.mst.projections;

import java.util.Date;

/**
 * Row of Sp_GatePassInward_GetAllMethod @Activity='ReadByGpNoForGRN' (native-EXEC).
 * Superset of the four sub-branches (RefDocumentTypeId 41 / 1500 / 105 / 106,110):
 * CityId and ScaleKart are only returned by the 106/110 sub-branch (null otherwise).
 */
public interface GatePassInwardForGrnRow {
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
    Double getSupplierWeight();
    Double getFactoryWeight();
    Double getDifferenceWeight();
    String getSupplierContractCode();
    String getWareHouseName();
    String getCompanyName();
    Integer getPurchaseOrderId();
    String getStatus();
    Integer getSupplierCustomerId();
    Integer getOrderCatagoryId();
    Integer getRefDocumentTypeId();
    String getAnalystName();
    Double getDeductionWeight();
    Integer getCityId();
    Double getScaleKart();
    String getTicketNos();
    Integer getWeightCutOnId();
    Integer getLabItemId();
    Double getAccessWeight();
    Integer getSupplierDispatchId();
}
