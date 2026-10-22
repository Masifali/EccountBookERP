package com.mst.projections;

import java.math.BigDecimal;
import java.util.Date;

/** Row of Sp_SaleOrder_GetAllMethod (GetSaleOrderDetailForBookingInvoice) - native projection. */
public interface SaleOrderDetailForBookingInvoiceRow {
    Integer getSaleOrderId();
    Date getDocDate();
    Integer getSaleOrderNo();
    String getRemarksHeader();
    String getDeliveryTerm();
    Integer getDeliveryDays();
    Integer getBrokerAgentSupCustId();
    Integer getOrderSupCustId();
    String getCustomerName();
    String getOrderStatus();
    String getCommissionType();
    BigDecimal getCommRate();
    Integer getUomScheduleIdCmRate();
    BigDecimal getCommAmount();
    String getCommissionRemarks();
    Integer getOrderDueDays();
    Boolean getIsAproved();
    String getEntryUser();
    String getApprovedUser();
    Integer getSaleOrderDetailId();
    Integer getItemId();
    String getItemName();
    Integer getJobLotId();
    String getJobLotDescription();
    BigDecimal getItemQty();
    Integer getItemUOMId();
    String getPackUom();
    String getUOMCode();
    BigDecimal getNetWeight();
    BigDecimal getItemRate();
    BigDecimal getCommOnSale();
    Integer getUomScheduleIdRate();
    String getRateUom();
    BigDecimal getItemAmount();
    String getDetailRemarks();
    Integer getDiscountTypeId();
    String getDiscountType();
}
