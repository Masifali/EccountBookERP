package com.mst.projections;

/** Row of Sp_Item_GetAllMethod @Activity='GetItemGlIdsandItemName' / 'GetPartyProcessingItemGlIdsandItemName' / 'GetItemsByItemCategoryAndItemType' (native) */
public interface ItemGlIdsRow {
    Integer getId();
    String getItemName();
    Integer getPurchaseGLAC();
    Integer getSaleGLAC();
    Integer getCOGSGLAC();
}
