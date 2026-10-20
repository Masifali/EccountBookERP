package com.mst.projections;

/** Row of Sp_Item_GetAllMethod @Activity='GetGLAccountReferedInItemDifinition' (native) */
public interface ItemGlReferedRow {
    Integer getPurchaseGLAC();
    String getStockGLAccountTitle();
    Integer getSaleGLAC();
    String getSalesGLAccountTitle();
    Integer getCOGSGLAC();
    String getCgsGLAccountTitle();
}
