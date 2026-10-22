package com.mst.projections;

/** Row of USP_Evalaution_DropDown_ByParentCategories (native; final SELECT DISTINCT from #EvaluationDropDown) */
public interface EvaluationDropDownByParentCategoriesRow {
    String getActivityType();
    Integer getId();
    String getName();
    Integer getInventoryParentCategoriesId();
}
