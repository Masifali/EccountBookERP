package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_InvGRN_GetAllMethod @Activity='PendingGrnForContractorWages' (native). */
public interface InvGrnPendingForContractorWagesRow {
    Integer getId();
    Integer getDocumentTypeId();
    String getDocumentTypeDescription();
    Timestamp getDocDate();
    Integer getDocNo();
    String getSupplierCustomer();
    Integer getGpNo();
    String getVehicleNo();
    String getBiltyNo();
    Double getGrossWeight();
}
