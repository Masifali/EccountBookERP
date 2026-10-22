package com.mst.projections;

/** Row of Sp_Item_GetAllMethod @Activity='AllItemsBindForEngr' (JPQL) */
public interface ItemEngrRow {
    Integer getId();
    String getItemName();
    String getItemCodeNew();
    Integer getItemCategoryId();
    Double getFinishWeight();
    Double getDyeWeight();
    Double getSandWeight();
    Integer getItemModalId();
}
