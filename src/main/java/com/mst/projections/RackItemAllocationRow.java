package com.mst.projections;


/** Row of usp_GetItemsByRackAllocation (native). */
public interface RackItemAllocationRow {
    Integer getId();
    String getItemName();
    Boolean getIsActive();
    String getParentCategory();
}
