package com.mst.projections;


/** Row of USP_GetWarehousesAllocatedToBranch (native). */
public interface WarehouseAllocatedToBranchRow {
    Integer getId();
    String getWareHouseName();
    Integer getBranchId();
    String getBranchName();
    String getWareHouseTypeId();
    String getWareHouseType();
    Boolean getIsActive();
    Integer getPlantId();
    String getPlantName();
}
