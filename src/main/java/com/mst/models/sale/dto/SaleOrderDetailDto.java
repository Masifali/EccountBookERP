package com.mst.models.sale.dto;

import lombok.Data;
import java.math.BigDecimal;

@Data
public class SaleOrderDetailDto {
    private Integer id;
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
}
