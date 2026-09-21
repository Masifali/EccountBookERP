package com.mst.models.cmagt.dto;

import lombok.Data;

import java.math.BigDecimal;

/**
 * Mirrors Architecture.Model.CommissionAgent.saleOrderEmptyBagDetail.
 * Non-virtual properties become the parameters of
 * [cmagt].[USP_saleOrderEmptyBagDetail_Insert]. PackingType is virtual (display only).
 */
@Data
public class SaleOrderCmagtEmptyBagDto {

    private BigDecimal Rate = BigDecimal.ZERO;
    private BigDecimal weightCutKg = BigDecimal.ZERO;
    private Integer PackingTypeId = 0;
    private Integer purchaseOrderEmptyBagDetailId = 0;
    private Integer purchaseOrderMasterId = 0;
    private Integer saleOrderEmptyBagDetailId = 0;
    private Integer saleOrderMasterId = 0;
    private Integer sortNo = 0;
    private String remarks = "";

    /** virtual on the desktop model - display only */
    private String PackingType;
}
