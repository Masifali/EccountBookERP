package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_WbTransation_GetAllMethod @Activity='FormHistory' (native) */
public interface WbTransactionFormHistoryRow {
    Long getRecordNo();
    Integer getId();
    Timestamp getDocDate();
    String getDocumentTypeDescription();
    String getWeighBridgeType();
    Integer getReferenceDocNoId();
    Integer getGpSrNo();
    Integer getTicketNo();
    String getVehicleNo();
    String getBiltyNo();
    String getItemDescription();
    Integer getReferenceDocTypeId();
    BigDecimal getItemQty();
    BigDecimal getWbCharges();
    String getWorkingReportNo();
    String getWbRemarks();
    Timestamp getEntryDate();
    String getPackingType();
    String getUserName();
    BigDecimal getFirstWeight();
    String getFirstWtWbStatus();
    BigDecimal getSecondWeight();
    String getSecondWtWbStatus();
    BigDecimal getNetWbWeight();
    String getPartyName();
    BigDecimal getPackingUnitWeight();
    BigDecimal getPackingTotalWeight();
    BigDecimal getWeightAfterPackingWeight();
    BigDecimal getLoadWeight();
    BigDecimal getTearWeight();
    BigDecimal getSupplierWbNetWeight();
}
