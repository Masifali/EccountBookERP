package com.mst.models.dto;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class PurchaseLineItemDto {
    private Integer lineId;
    private Integer itemId;
    private String itemCode;
    private String itemName;
    private Integer cropYearId;
    private String cropYear;
    private Integer packUomId;
    private String packUom;
    private Double packEquivalent;
    private BigDecimal quantity;
    private BigDecimal weight;
    private Integer rateUomId;
    private String rateUom;
    private Double rateEquivalent;
    private BigDecimal itemRate;
    private BigDecimal amount;
    private Integer warehouseId;
    private String warehouseName;
    private Integer jobLotId;
    private String jobLotCode;
    private String remarks;
    private BigDecimal taxPercent;
    private BigDecimal taxAmount;
    private BigDecimal discountPercent;
    private BigDecimal discountAmount;
    private BigDecimal netAmount;
}
