package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_InvGRN_GetAllMethod @Activity='GrnLoadForStoreInvoiceDemondBase' (native). */
public interface InvGrnLoadForStoreInvoiceDemandRow {
    Integer getId();
    Timestamp getDocDate();
    Integer getDocNo();
    Integer getGpNo();
    String getVehicleNo();
    String getBiltyNo();
    String getSupplierName();
    Integer getDemandNo();
    Integer getSupplierCustomerId();
    Double getItemQty();
    String getItemName();
    String getItemCode();
    String getUOMCode();
    String getWareHouseName();
    String getBranchName();
}
