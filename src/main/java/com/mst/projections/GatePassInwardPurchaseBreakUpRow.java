package com.mst.projections;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='ReadByHeaderId_PurchaseBrekup' (JPQL on InwardGatePassPurchaseBreakUp). */
public interface GatePassInwardPurchaseBreakUpRow {
    Integer getId();
    Integer getInwardGatePassId();
    Double getQty();
    Double getUom();
    Double getGrossWeight();
    Double getEbWeight();
    Double getEbTotal();
    Double getNetWeight();
}
