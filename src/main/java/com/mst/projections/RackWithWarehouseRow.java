package com.mst.projections;


/** Row of usp_getRackswithWarehouse (JPQL). */
public interface RackWithWarehouseRow {
    Integer getId();
    String getRackName();
    Integer getInvWarehouseId();
    String getWareHouseName();
}
