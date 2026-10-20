package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='PurchaseOrderLoadForPurchaseInvoice' */
public interface PurchaseInvoicePurchaseOrderLoadRow {
    Integer getId();
    Integer getPODetailId();
    Integer getOrderNo();
    Timestamp getDocDate();
    Integer getSupplierCustomerId();
    String getSupplierName();
    Timestamp getOrderDueDate();
    Integer getOrderDueDays();
    Integer getItemId();
    String getItemCode();
    String getItemName();
    String getCrop();
    Integer getOrderItemUOMId();
    BigDecimal getUOMCodeItem();
    String getPackUom();
    BigDecimal getItemQty();
    BigDecimal getNetWeight();
    Integer getLabSampleNo();
    BigDecimal getItemRate();
    Integer getOrderItemRateUOMId();
    BigDecimal getRateUOM();
    String getRateUomCode();
    BigDecimal getAmount();
    BigDecimal getEmptyBags();
    BigDecimal getEmptyBagsTotal();
    BigDecimal getGrossWeight();
    BigDecimal getTaxPercent();
    BigDecimal getTaxAmount();
    String getBrokerAgentName();
    String getCommissionType();
    BigDecimal getCommRate();
    Integer getUomScheduleIdCmRate();
    BigDecimal getCommAmount();
    String getDeliveryTerm();
    Integer getPaymentTermsId();
    String getCommissionRemarks();
    Integer getBrokerAgentSupCustId();
    Integer getPurchaseGLAC();
    String getRemarksHeader();
    Integer getJobLotId();
    String getJobLotDescription();
    Integer getBrokerAgentId();
    String getBrokeryType();
    BigDecimal getBrokeryRate();
    String getBrokeryUom();
    BigDecimal getBrokeryAmount();
    String getCityArea();
    String getOrderRemarks();
    Integer getBranchId();
    String getBranchName();
}
