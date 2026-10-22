package com.mst.projections;

/** Row of Sp_Item_GetAllMethod @Activity='GetItemByItemTypeId' (native) */
public interface ItemByTypeRow {
    Integer getId();
    String getItemName();
    String getItemCodeNew();
    Integer getInventoryParentCategoriesId();
    String getInvParentCateCode();
    String getParentCategory();
}
