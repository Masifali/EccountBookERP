package com.mst.projections;

import java.sql.Timestamp;
import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='GetGpSrNoForPurchaseAnalysis' (native, #TmpGpData final select). */
public interface GatePassInwardForPurchaseAnalysisRow {
    Integer getId();
    Integer getGpSrNo();
    Date getGpDate();
    String getVehicleType();
    String getVehicleNo();
    Integer getRefDocumentTypeId();
    String getSupplierName();
    String getBiltyNo();
    Timestamp getBiltyDate();
    String getCity();
    Timestamp getInDateTimeStamp();
    String getEntryUserName();
    Integer getEntryUser();
    Timestamp getEntryDate();
    String getOrderType();
    String getLabType();
}
