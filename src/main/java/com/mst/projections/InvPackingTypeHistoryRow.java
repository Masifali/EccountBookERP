package com.mst.projections;

/** Row of Sp_InvPackingType_GetAllMethod @Activity='FormHistory' (JPQL) */
public interface InvPackingTypeHistoryRow {
    Integer getId();
    String getPackTypeCode();
    String getPackTypeDesc();
    Boolean getIsActive();
}
