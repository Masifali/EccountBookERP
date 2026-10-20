package com.mst.projections;

/** Row of Sp_Item_GetAllMethod @Activity='ReadAllForComboTwoColumns' / 'ReadAllForComboTwoColumnsWithParentId' (native) */
public interface ItemComboParentRow {
    Integer getId();
    String getItemName();
    String getItemCategory();
    String getItemCode();
    Integer getInventoryParentCategoriesId();
}
