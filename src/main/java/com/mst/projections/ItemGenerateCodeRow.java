package com.mst.projections;

/** Row of Sp_Item_GetAllMethod @Activity='GenerateItemCodeByCategoryId' (native-EXEC; ItemCode is int in the plain branch and a zero-padded varchar in the coding-enabled branch, both exposed as String) (native) */
public interface ItemGenerateCodeRow {
    String getItemCode();
    String getItemCodeNew();
}
