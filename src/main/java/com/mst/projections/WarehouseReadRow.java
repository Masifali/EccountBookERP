package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_InvWareHouse_GetAllMethod @Activity='ReadByOrganizationCompanyId' (native). */
public interface WarehouseReadRow {
    Integer getId();
    String getWareHouseCode();
    String getWareHouseName();
    Timestamp getEntryDate();
    Integer getEntryUser();
    Timestamp getModifyDate();
    Integer getModifyUser();
    Integer getOrganizationId();
    Integer getCompanyId();
    Boolean getIsActive();
    Integer getBranchesId();
    String getBranchName();
    String getWarehouseType();
}
