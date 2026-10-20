package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetHistoryForPdfReport' */
public interface PurchaseInvoicePdfDetailRow {
    Integer getId();
    Integer getInvoiceId();
    Integer getGdn_Grn_Id();
    Integer getGdn_Grn_Number();
    Integer getGatePassId();
    Integer getGatePassNumber();
    Integer getWarehouseId();
    String getWarehouseName();
    Integer getJob_LotId();
    String getJobLotName();
    String getCropYear();
    Integer getItemId();
    String getItemName();
    String getPackingType();
    BigDecimal getItemQty();
    String getPackUom();
    BigDecimal getGrossWeight();
    BigDecimal getBagWeight();
    BigDecimal getWeightCut();
    BigDecimal getNetWeight();
    BigDecimal getStockWeight();
    BigDecimal getRate();
    String getRateUom();
    BigDecimal getItemAmount();
    BigDecimal getRateCutAmount();
    BigDecimal getFreightAmount();
    BigDecimal getExpenseAmount();
    BigDecimal getCommissionAmount();
    BigDecimal getBillAmount();
    BigDecimal getExcessWeight();
    BigDecimal getRateCutForExcessWeight();
    BigDecimal getRateCutAmountForExcessWeight();
    Integer getPurchaseGLAC();
    Integer getSaleGLAC();
}
