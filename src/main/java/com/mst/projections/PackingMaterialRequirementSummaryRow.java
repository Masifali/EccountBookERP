package com.mst.projections;

/** Row of usp_PackingMaterialRequirementPlanning with @Activity = NULL (summary per item; native EXEC). */
public interface PackingMaterialRequirementSummaryRow {
    Integer getPMItemId();
    String getItemCodeNew();
    String getItemName();
    Double getTotalRequiredQty();
    Double getReservedStock();
    Double getRemainingAfterReserved();
    Double getAvailableStock();
    Double getNeedFromOrders();
    Double getOutstandingOrders();
    Double getMoreRequired();
    Double getWeightCapacity();
    Double getWeightPerQty();
    Integer getLeadTimeDay();
    Integer getPackUomId();
    String getUOMCode();
    Double getEquivalent();
}
