package com.mst.projections;

/** Row of Sp_Item_GetAllMethod @Activity='GetPMItemOnly' (native) */
public interface ItemPmRow {
    Integer getId();
    String getItemName();
    String getItemCode();
    Integer getLeadTimeDay();
    String getWeightCapacity();
}
