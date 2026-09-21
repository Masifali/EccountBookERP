package com.mst.models.cmagt.dto;

import lombok.Data;

import java.math.BigDecimal;

/**
 * Mirrors Architecture.Model.CommissionAgent.saleOrderbuyerExpenseDetail.
 * Non-virtual properties become the parameters of
 * [cmagt].[USP_saleOrderBuyerExpenseDetail_Insert] (GenericProvider.SetProc convention).
 * OtherItemName is virtual on the desktop model and is therefore display-only.
 */
@Data
public class SaleOrderCmagtExpenseDto {

    private BigDecimal Qty = BigDecimal.ZERO;
    private Double amount = 0d;
    private Double rate = 0d;
    private Integer ItemId = 0;
    private Integer purchaseOrderMasterId = 0;
    private Integer purchaseOrderSupplierExpenseDetailId = 0;
    private Integer saleOrderbuyerExpenseDetailId = 0;
    private Integer saleOrderMasterId = 0;
    private Integer sortNo = 0;
    private String remarks = "";

    /** virtual on the desktop model - display only, never a procedure parameter */
    private String OtherItemName;
}
