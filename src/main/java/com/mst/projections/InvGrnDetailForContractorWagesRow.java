package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_InvGRN_GetAllMethod @Activity='GetGrnDetialForContractorWages' (native). */
public interface InvGrnDetailForContractorWagesRow {
    Integer getId();
    Timestamp getDocDate();
    Integer getItemId();
    String getItemName();
    String getCropYear();
    Integer getJlId();
    String getJobLotDescription();
    Integer getPackingTypeId();
    String getPackTypeCode();
    Double getEquivalent();
    Double getGrossWeight();
    Double getNetBillWeight();
    Integer getWhFId();
    String getWarehouseFrom();
    Integer getWhTId();
    String getWareHouseTo();
}
