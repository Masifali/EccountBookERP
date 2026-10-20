package com.mst.projections;

import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='GetGpDataForFreightVoucher' (native). */
public interface GatePassInwardFreightVoucherRow {
    Integer getId();
    Date getGpDate();
    Integer getGpSrNo();
    String getVehicleNo();
    String getBiltyNo();
    Double getBiltyFreight();
    Double getWbCharges();
    Double getSupplierFirstWeight();
    Double getSupplierSecondWeight();
    Double getSupplierWeight();
    Double getNetWbWeight();
    Double getToleranceWeight();
    Double getShortWeight();
    Double getShortageAmount();
    String getStatus();
    String getRemarks();
    Integer getDebitAccountId();
    String getAccountTitle();
    Integer getCityId();
    String getCityName();
    String getSupplierName();
    String getVarietyName();
    Integer getItemQty();
}
