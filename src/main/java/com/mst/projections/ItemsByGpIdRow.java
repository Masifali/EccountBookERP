package com.mst.projections;

/** Row of usp_getItemsFromLabOrPurchaseOrderOrAllByGpId (native EXEC) */
public interface ItemsByGpIdRow {
    Integer getItemId();
    String getItemName();
    String getItemCode();
    Integer getPoDetailId();
    Double getItemWbWeight();
    Double getMoisture();
}
