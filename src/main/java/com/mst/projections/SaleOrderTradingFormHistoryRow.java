package com.mst.projections;

import java.math.BigDecimal;
import java.util.Date;

/** Row of Sp_SaleOrder_GetAllMethod (SaleOrderTradingFormHistory) - native projection. */
public interface SaleOrderTradingFormHistoryRow {
    Long getRecordNo();
    Integer getId();
    Integer getDocNo();
    Date getDocDate();
    String getCustomerName();
    Integer getSupplierCustomerId();
    String getPymentTerms();
    Date getOrderDueDate();
    Integer getOrderDueDays();
    String getDeliveryTerm();
    Boolean getIsAproved();
    String getOrderStatus();
    String getRemarksHeader();
    BigDecimal getItemAmountHeader();
    BigDecimal getDiscountAmountHeader();
    BigDecimal getTaxAmountHeader();
    BigDecimal getOrderAmount();
    BigDecimal getOrderQty();
    BigDecimal getOrderWeight();
    String getEntryUser();
    String getModifyUser();
    String getPostUser();
    Integer getCommissionAgentId();
    String getComissionAgent();
    String getCommissionType();
    BigDecimal getCommRate();
    String getComissionRateUom();
    BigDecimal getCommAmount();
    String getItemName();
    String getUOMCode();
    BigDecimal getItemQty();
    BigDecimal getItemRate();
    BigDecimal getItemAmount();
    String getIsRefered();
}
