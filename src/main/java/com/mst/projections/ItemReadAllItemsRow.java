package com.mst.projections;

/** Row of Sp_Item_GetAllMethod @Activity='ReadAllItems' (= ReadAllItemsIncludedPM columns + CategoryDescription, TypeDescription) (native) */
public interface ItemReadAllItemsRow extends ItemReadAllItemsPmRow {
    String getCategoryDescription();
    String getTypeDescription();
}
