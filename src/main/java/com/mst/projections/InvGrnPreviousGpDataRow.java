package com.mst.projections;

/** Row of Sp_InvGRN_GetAllMethod @Activity='GrnPrevoiusDataOfSelectedGP' (native, grouped by UOM). */
public interface InvGrnPreviousGpDataRow {
    Integer getItemUomId();
    String getPackUom();
    Double getPackUomEquivalent();
    Double getItemQty();
}
