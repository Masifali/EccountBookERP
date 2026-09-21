package com.mst.models.dto;
public class InventoryCategoryRequest {
 public int Id,SerialFrom,SerialTo,RevenueAccountId,InventoryAccountId,CGSAccountId,InventoryParentCategoriesId,ItemClassGroupId,ItemProductionStageId,ItemVarietyNatureId;
 public String CategoryCode,CategoryDescription;
 public boolean CategoryStatus=true;
 public static class Translation {public int ItemCategoryId,MultiLanguagesId;public String CategoryDescription;}
}
