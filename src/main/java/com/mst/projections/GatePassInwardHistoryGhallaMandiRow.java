package com.mst.projections;

import java.sql.Timestamp;
import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='GatepassHistoryForGhallaMandi' (native, #TmpHisGM). */
public interface GatePassInwardHistoryGhallaMandiRow {
    Long getRecordNo();
    Integer getId();
    Date getGpDate();
    Integer getGpSrNo();
    Integer getWorkingReportNo();
    String getGhallaMandi();
    String getLoadingContractor();
    String getVehicleType();
    String getVehicleNo();
    String getBiltyNo();
    Integer getItemQty();
    String getOtherRemarks();
    Timestamp getInTime();
    Timestamp getOutTime();
    String getUserName();
    String getCarriageContractro();
    Double getFreight();
    Integer getNoOfAttachments();
}
