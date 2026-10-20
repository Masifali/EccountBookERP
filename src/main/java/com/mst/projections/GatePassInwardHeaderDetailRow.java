package com.mst.projections;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='ReadByHeaderId' (native). */
public interface GatePassInwardHeaderDetailRow {
    String getItemName();
    String getPackTypecode();
    Double getPackUnit();
    Double getItemQty();
    Double getWeight();
    Double getEquivalent();
    String getUOMCode();
    Integer getDocNo();
    String getCityName();
    Integer getOrderNo();
    String getRemarksDetail();
    Integer getRefDocumentTypeId();
}
