package com.mst.projections;

/** Row of Sp_Item_GetAllMethod @Activity='GetGLAccountbyItemCategoryId' (JPQL) */
public interface ItemCategoryGlAccountRow {
    Integer getId();
    Integer getRevenueAccountId();
    Integer getInventoryAccountId();
    Integer getCgsAccountId();
}
