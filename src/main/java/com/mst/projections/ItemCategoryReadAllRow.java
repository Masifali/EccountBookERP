package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_ItemCategory_GetAllMethod @Activity='ReadByOrganizationCompanyId' (native) */
public interface ItemCategoryReadAllRow {
    Integer getId();
    String getCategoryCode();
    String getCategoryDescription();
    Integer getSerialFrom();
    Integer getSerialTo();
    String getCategoryStatus();
    Integer getRevenueAccountId();
    Integer getInventoryAccountId();
    Integer getInventoryParentCategoriesId();
    String getInvParentCateDescription();
    Integer getCGSAccountId();
    String getCGSAccountTitle();
    String getRevenueAccountTitle();
    String getInventoryAccountTitle();
    String getCategoryAttributeList();
    Integer getItemProductionStageId();
    String getProductionStageName();
    Integer getItemVarietyNatureId();
    String getVarietyNatureName();
    Integer getExpenseMaintenanceAccountId();
    Integer getUsefullLifeInMonths();
    BigDecimal getDepriciationrate();
    Integer getAssetCategoryId();
    Integer getDepreciationMethodScheduleId();
    String getDepreciationMethodName();
}
