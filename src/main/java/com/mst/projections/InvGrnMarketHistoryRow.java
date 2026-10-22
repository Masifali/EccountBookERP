package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_InvGRN_GetAllMethod @Activity='HistoryForMarketGrn' (native). */
public interface InvGrnMarketHistoryRow {
    Long getRecordNo();
    Integer getId();
    Integer getOrderNo();
    Timestamp getDocDate();
    Integer getDocNo();
    Integer getDocumentTypeId();
    String getScreenName();
    String getSupplierName();
    Integer getGpNo();
    String getVehicleNo();
    String getBiltyNo();
    Double getFactoryWeight();
    Double getPartyWeight();
    String getAccountTitle();
    Double getFreight();
    Double getNetPaid();
    String getRemarksHeader();
    String getUserName();
    Integer getNoOfAttachments();
}
