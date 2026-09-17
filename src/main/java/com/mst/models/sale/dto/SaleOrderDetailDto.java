package com.mst.models.sale.dto;

import lombok.Data;
import java.math.BigDecimal;

@Data
public class SaleOrderDetailDto {
    /** Real SaleOrderDetail.Id (IDENTITY). Null/0 for a brand-new row not yet saved. */
    private Integer id;
    /**
     * Real per-row ActionTypeId consumed by Sp_SaleOrderDetail_Insert (1=insert, 2=update,
     * 3=soft-delete - see SALE-ORDER-PROGRESS.md Pass 3). Persisted rows removed from the browser
     * grid are retained in the request with ActionTypeId=3.
     */
    private Integer actionTypeId;
    private Integer itemId;
    private String itemCode;
    private String itemName;
    private String cropYear = "2025-26";
    private Integer cropYearId;
    private Integer jobLotId;
    private String jobLot;
    private Integer packingTypeId;
    private String packingType;
    private String packSize;
    /** Real OrderItemUOMId ("Pack Uom" cell in desktop) - required by the real save proc. */
    private Integer packUomId;
    private BigDecimal quantity = BigDecimal.ZERO;
    private BigDecimal weight = BigDecimal.ZERO;
    private Integer rateUomId;
    private String rateUom;
    private BigDecimal rate = BigDecimal.ZERO;
    private BigDecimal amount = BigDecimal.ZERO;
    private BigDecimal bagPrice = BigDecimal.ZERO;
    private BigDecimal weightCut = BigDecimal.ZERO;
    private Integer cityId;
    private String cityArea;
    private Integer warehouseId;
    private String warehouse;
    private String labSample;
    private String remarks;
    private Boolean commOnSale = false;
    /** Pre-order/pre-booking linkage (real RefDocId/RefDocDetailId) - null for a manually entered row. */
    private Integer refDocId;
    private Integer refDocDetailId;
    private Integer costCenterId;
}
