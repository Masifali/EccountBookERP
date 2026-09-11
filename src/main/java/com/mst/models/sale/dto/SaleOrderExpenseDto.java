package com.mst.models.sale.dto;

import lombok.Data;
import java.math.BigDecimal;

@Data
public class SaleOrderExpenseDto {
    private Integer id;
    private Integer itemId;
    private String itemName;
    private BigDecimal quantity = BigDecimal.ZERO;
    private BigDecimal rate = BigDecimal.ZERO;
    private BigDecimal amount = BigDecimal.ZERO;
    private String remarks;
}
