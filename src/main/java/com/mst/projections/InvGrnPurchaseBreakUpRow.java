package com.mst.projections;

/**
 * Row of Sp_InvGRN_GetAllMethod @Activity='ReadByHeaderId_PurchaseBreakup' (JPQL) and
 * 'ReadByGPId_InvGrnPurchaseBreakup' (native, first DENSE_RANK group only). Same column set, column order differs
 * slightly between the two branches (SupplierShortWeight position).
 */
public interface InvGrnPurchaseBreakUpRow {
    Integer getId();
    Integer getInvGrnId();
    Integer getInwardGatePassId();
    Integer getInwardBreakupId();
    Double getQty();
    Double getUOM();
    Double getGrossWeight();
    Double getEbWeight();
    Double getEBTotal();
    Double getSupplierShortWeight();
    Double getNetWeight();
    Double getNetPackSize();
    Double getSupplierRcvdWeight();
    Double getScaleShortage();
    Double getBillWeight();
}
