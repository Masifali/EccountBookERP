package com.mst.projections;

/** Row of Sp_Item_GetAllMethod @Activity='ReadAllItemsByParentId' / 'ReadAllItemsTaxable' (native) */
public interface ItemComboRow {
    Integer getId();
    String getItemName();
    String getItemCategory();
    String getItemCode();
}
