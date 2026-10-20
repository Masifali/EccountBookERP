package com.mst.projections;

/** Row of usp_PackingMaterialRequirementPlanning with @Activity = 'Detail' (native EXEC). */
public interface PackingMaterialRequirementDetailRow {
    Integer getPMItemId();
    String getItemCodeNew();
    String getItemName();
    String getCustomerName();
    String getScheduleCode();
    String getCustomerContractNo();
    String getPortName();
    java.sql.Timestamp getLoadingDate();
    java.sql.Timestamp getProductionDate();
    java.sql.Timestamp getPackingMaterialDate();
    Integer getInvoiceId();
    java.sql.Timestamp getInvoiceDate();
    Double getTotalRequiredQty();
    Double getReservedStock();
    Double getConsumedReserved();
    Double getRemainingAfterReserved();
    Double getAvailableStock();
    Double getConsumedFromStock();
    Double getNeedFromOrders();
    Double getOutstandingOrders();
    Double getMoreRequired();
}
