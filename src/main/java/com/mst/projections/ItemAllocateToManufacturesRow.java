package com.mst.projections;

/** Row of Sp_ItemAllocateToManufactures_GetAllMethod (ReadAll) — JPQL projection. */
public interface ItemAllocateToManufacturesRow {
    Integer getId();
    Integer getItemId();
    Integer getManufactureId();
    Boolean getIsActive();
    String getItemName();
    String getManufacture();
}
