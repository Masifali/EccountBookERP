package com.mst.projections;

import java.sql.Timestamp;

/** Row of usp_invWarehouseRack_GetAllMethod @Activity='ReadAll' (native). */
public interface RackReadAllRow {
    Integer getId();
    Integer getInvWarehouseId();
    String getWareHouseCode();
    String getWareHouseName();
    String getRackName();
    Integer getSortNo();
    Boolean getIsActive();
    Timestamp getEntryDate();
    Integer getEntryUserId();
    String getEntryUser();
    Timestamp getModifyDate();
    Integer getModifyUserId();
    String getModifyUser();
}
