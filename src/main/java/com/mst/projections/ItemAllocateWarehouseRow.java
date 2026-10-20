package com.mst.projections;

/** Row of Sp_Item_GetAllMethod @Activity='GetItemsForAllocatetoWarehouse' (native) */
public interface ItemAllocateWarehouseRow {
    Integer getId();
    String getItemName();
    Integer getItemCategoryId();
    Integer getItemTypeId();
    Integer getPurchaseGLAC();
    Integer getSaleGLAC();
    Integer getCOGSGLAC();
}
