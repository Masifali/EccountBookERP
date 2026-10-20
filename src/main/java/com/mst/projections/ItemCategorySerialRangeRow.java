package com.mst.projections;

/** Helper row for Sp_ItemCategory_Update pre-check (SerialFrom/SerialTo of the record being updated) */
public interface ItemCategorySerialRangeRow {
    Integer getSerialFrom();
    Integer getSerialTo();
}
