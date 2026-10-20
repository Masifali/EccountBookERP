package com.mst.projections;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/** Row of Sp_ItemCategory_GetAllMethod @Activity='ReadById' (JPQL) */
public interface ItemCategoryReadByIdRow {
    Integer getId();
    String getCategoryCode();
    String getCategoryDescription();
    Integer getSerialFrom();
    Integer getSerialTo();
    Boolean getCategoryStatus();
    Integer getRevenueAccountId();
    Integer getCgsAccountId();
    Integer getInventoryAccountId();
    Integer getAccumulatedDepreciationAcId();
    Integer getDepreciationExpenseAcId();
    Integer getCapitalWipAcId();
    LocalDateTime getEntryDate();
    Integer getEntryUser();
    LocalDateTime getModifyDate();
    Integer getModifyUser();
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getInventoryParentCategoriesId();
    Integer getItemProductionStageId();
    Integer getItemVarietyNatureId();
    Integer getItemClassGroupId();
    Integer getExpenseMaintenanceAccountId();
    String getCategoryAttributeList();
    Integer getUsefullLifeInMonths();
    BigDecimal getDepriciationrate();
    Integer getAssetCategoryId();
    Integer getDepreciationMethodScheduleId();
}
