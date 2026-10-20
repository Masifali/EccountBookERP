package com.mst.projections;

/** Row of Sp_Item_GetAllMethod @Activity='GetGLAccountbyItemCategoryIdForFixedAssets' (JPQL) */
public interface ItemCategoryGlAccountFixedAssetsRow extends ItemCategoryGlAccountRow {
    Integer getAccumulatedDepreciationAcId();
    Integer getDepreciationExpenseAcId();
    Integer getExpenseMaintenanceAccountId();
    Integer getCapitalWipAcId();
}
