package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of USP_GetDataRateCutInCaseOfAccessWeightReceived (native) */
public interface RateCutAccessWeightRow {
    Integer getMainId();
    Integer getDocNo();
    Timestamp getDocDate();
    Integer getGpNo();
    Integer getGlAccountId();
    String getAccountTitle();
    BigDecimal getQty();
    BigDecimal getRateCut();
    BigDecimal getRateCutAmount();
    String getBiltyNo();
    String getVehicleNo();
}
