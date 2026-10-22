package com.mst.projections;

/** Row of Sp_Item_GetAllMethod @Activity='GetGLAccountByItemId' (native) */
public interface ItemGlAccountRow {
    Integer getId();
    String getItemName();
    String getAccountCode();
    String getAccountTitle();
    Integer getPurchaseGLAC();
    Integer getSaleGLAC();
}
