package com.mst.projections;

/** Row of Sp_InventoryItemsOther_GetAllMethod (ReadAll / ReadById) */
public interface InventoryItemsOtherRow {
    Integer getId();
    String getOtherItemName();
    String getOtherItemType();
    Integer getStockGLAcId();
    String getAccountTitleStock();
    Integer getSaleGLAcId();
    String getAccountTitleSales();
    Integer getCogsGLAcId();
    String getAccountTitleCgs();
    Integer getCompanyId();
    Integer getOrganizationId();
    Integer getBranchesId();
    Integer getProjectsId();
    String getStockGLAc();
    String getSaleGLAc();
    String getCogsGLAc();
}
