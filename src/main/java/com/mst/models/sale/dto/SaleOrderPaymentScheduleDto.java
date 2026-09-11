package com.mst.models.sale.dto;

import lombok.Data;
import java.math.BigDecimal;

@Data
public class SaleOrderPaymentScheduleDto {
    private Integer id;
    private Integer paymentTermId;
    private String paymentTerm;
    private Integer dueDays = 0;
    private String dueDate;
    private BigDecimal percentOfTotal = BigDecimal.ZERO;
    private BigDecimal amount = BigDecimal.ZERO;
    private String remarks;
}
