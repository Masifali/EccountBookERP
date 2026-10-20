package com.mst.projections;

/** Row of Sp_Item_GetAllMethod @Activity='GetItemsFromInventoryStocksEvaluations' / 'GetItemsFromInventoryTransactions' (native) and 'GetItemByWarehouseIdfromInventoryTransactions' (JPQL) (native) */
public interface LookupItemRow {
    Integer getItemId();
    String getItemName();
}
