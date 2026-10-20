package com.mst.projections;

import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='GetLabDataByGpIdForGrnGate' (native; empty unless config 101 = 1). */
public interface GatePassInwardLabDataGrnGateRow {
    Integer getId();
    Date getDocDate();
    Integer getDocNo();
    Integer getItemId();
    String getItemName();
    Double getDeductionWeight();
    Double getDeductionRate();
    Double getItemRate();
    String getAnalystName();
    String getRemarksHeader();
    String getCrop();
    Integer getPoDetailId();
    String getLabStatus();
    Boolean getGatePurchaseLabConfigStatus();
    String getEntryUserName();
    Double getNetWeight();
}
