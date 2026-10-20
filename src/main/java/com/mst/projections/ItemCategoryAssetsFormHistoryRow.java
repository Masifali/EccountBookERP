package com.mst.projections;

/** Row of Sp_ItemCategory_GetAllMethod @Activity='FormHistoryForAssetsCategory' (native) */
public interface ItemCategoryAssetsFormHistoryRow {
    Integer getId();
    String getCategoryCode();
    String getCategoryDescription();
    Integer getSerialFrom();
    Integer getSerialTo();
    Integer getRevenueAccountId();
    Integer getInventoryAccountId();
    Integer getInventoryParentCategoriesId();
    Integer getAccumulatedDepreciationAcId();
    String getAccumulatedDepreciationAccount();
    Integer getDepreciationExpenseAcId();
    String getDepreciationExpenseAccount();
    Integer getCapitalWipAcId();
    String getCapitalWipAccount();
    Integer getExpenseMaintenanceAccountId();
    String getExpenseMaintenanceAccount();
    String getAssetParentCateDescription();
    String getCategoryStatus();
    Integer getItemProductionStageId();
    String getProductionStageName();
    Integer getItemVarietyNatureId();
    String getVarietyNatureName();
}
