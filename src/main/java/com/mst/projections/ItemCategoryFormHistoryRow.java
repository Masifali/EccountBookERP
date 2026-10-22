package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_ItemCategory_GetAllMethod @Activity='FormHistory' (native) */
public interface ItemCategoryFormHistoryRow {
    Integer getId();
    String getCategoryCode();
    String getCategoryDescription();
    Integer getSerialFrom();
    Integer getSerialTo();
    Integer getInventoryParentCategoriesId();
    String getCategoryStatus();
    String getInvParentCateDescription();
    Integer getRevenueAccountId();
    Integer getInventoryAccountId();
    Integer getCGSAccountId();
    String getCGSAccountTitle();
    String getRevenueAccountTitle();
    String getInventoryAccountTitle();
    String getClassGroupName();
    Integer getAccumulatedDepreciationAcId();
    String getAccumulatedDepreciationAccount();
    Integer getDepreciationExpenseAcId();
    String getDepreciationExpenseAccount();
    Integer getCapitalWipAcId();
    String getCapitalWipAccount();
    Integer getItemProductionStageId();
    String getProductionStageName();
    Integer getItemVarietyNatureId();
    String getVarietyNatureName();
    Integer getExpenseMaintenanceAccountId();
    String getExpenseMaintenanceAccount();
    Integer getUsefullLifeInMonths();
    BigDecimal getDepriciationrate();
    Integer getAssetCategoryId();
    Integer getDepreciationMethodScheduleId();
    String getDepreciationMethodName();
}
