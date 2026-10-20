package com.mst.projections;

/** Row of Sp_Inventory_InventoryTransactions_DropDownAndLists (native; final SELECT from #InventoryTransactions) */
public interface InventoryTransactionsDropDownRow {
    Integer getId();
    String getName();
    String getActivityType();
    Integer getParentCategoryId();
}
