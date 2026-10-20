package com.mst.projections;

/** Row of Sp_PurchaseOrder_GetAllMethod @Activity='GetPurchaseOrderForPurchaseInvoice' (native). */
public interface PurchaseOrderForPurchaseInvoiceRow {
    Integer getId();
    java.sql.Timestamp getDocDate();
    Integer getDocNo();
    Integer getSupplierCustomerId();
    String getSupplierName();
    String getItemName();
    Double getItemQty();
    Double getItemWeight();
    String getItemCode();
    String getUOMCode();
    String getJobLotDescription();
    Double getEquivalent();
    Integer getCropYearId();
    String getCrop();
    Integer getPackUomId();
    String getPackUom();
    Double getPackEquivalent();
    Integer getRateUomId();
    Double getOrderItemRate();
    String getDeliveryTerm();
    Integer getBrokerAgentId();
    String getBrokeryType();
    Double getBrokeryRate();
    Double getBrokeryUom();
    Double getBrokeryAmount();
    Integer getBrokerAgentSupCustId();
    String getCommissionType();
    Double getCommRate();
    Integer getUomScheduleIdCmRate();
    Double getCommAmount();
    String getRemarksHeader();
    Integer getPODetailId();
    Double getEmptyBags();
    Double getEmptyBagsTotal();
    Double getGrossWeight();
    Integer getOrderItemId();
}
