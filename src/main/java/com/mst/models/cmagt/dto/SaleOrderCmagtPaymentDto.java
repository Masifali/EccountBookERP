package com.mst.models.cmagt.dto;

import lombok.Data;

import java.math.BigDecimal;

/**
 * Mirrors Architecture.Model.CommissionAgent.saleOrderPaymentDetail.
 * Non-virtual properties become the parameters of
 * [cmagt].[USP_saleOrderPaymentDetail_Insert].
 *
 * DueDate is nullable on the desktop model (DateTime?) and is left null unless the
 * row's BaseDueDateTypeId is 1 (derived from DocDate + DueDays) or 4 (entered directly),
 * exactly as frmSaleOrderCmagt.Insert() computes it.
 */
@Data
public class SaleOrderCmagtPaymentDto {

    private String DueDate;                 // ISO yyyy-MM-dd, or null
    private BigDecimal dueAmount = BigDecimal.ZERO;
    private BigDecimal pctOfTotal = BigDecimal.ZERO;
    private Integer BaseDueDateTypeId = 0;
    private Integer DueDays = 0;
    private Integer PaymentTermId = 0;
    private Integer saleOrderMasterId = 0;
    private Integer saleOrderPaymentDetailId = 0;
    private Integer sortNo = 0;
    private String remarks = "";

    /** virtual on the desktop model - display only */
    private String PaymentTerm;
    private String BaseDateType;
}
