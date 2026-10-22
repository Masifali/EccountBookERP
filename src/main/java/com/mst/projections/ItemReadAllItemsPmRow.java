package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_Item_GetAllMethod @Activity='ReadAllItemsIncludedPM' (native) */
public interface ItemReadAllItemsPmRow {
    Integer getId();
    String getItemName();
    String getItemCode();
    String getItemCodeNew();
    Integer getInventoryParentCategoriesId();
    Integer getItemCategoryId();
    Integer getItemTypeId();
    Integer getItemQcGradeId();
    String getItemQcGrade();
    Integer getMaxMeaurementUnitId();
    BigDecimal getWeightKgs();
    BigDecimal getMaxMeaurement();
    Integer getPurchaseGLAC();
    Integer getSaleGLAC();
    Integer getCOGSGLAC();
}
