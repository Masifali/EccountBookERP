package com.mst.projections;


/** Row of USP_WarehousesAllocatedOrUnAllocatedToBranch (JPQL). */
public interface WarehouseBranchAllocationRow {
    Integer getBranchId();
    String getBranchName();
    Integer getWarehouseId();
    String getWareHouseName();
}
