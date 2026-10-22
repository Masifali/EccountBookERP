package com.mst.projections;

/** Row of Sp_PurchaseOrder_GetAllMethod @Activity='GetPurchaseOrderdetailforSupplySchedule' (native). */
public interface PurchaseOrderForSupplyScheduleRow {
    Integer getId();
    java.sql.Timestamp getDocDate();
    Integer getDocNo();
    Integer getOrderSupCustId();
    String getCompanyName();
    String getSupplierRefNo();
    Integer getItemId();
    Integer getOrderItemUOMId();
    String getItemName();
    Double getOrderItemQty();
    String getItemCode();
    String getUOMCode();
}
