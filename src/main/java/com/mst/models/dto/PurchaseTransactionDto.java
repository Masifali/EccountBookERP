package com.mst.models.dto;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.util.List;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class PurchaseTransactionDto {
    private Integer id;
    private Integer documentTypeId;
    private Integer docNo;
    private String docNoDisplay;
    private String docDate;
    private Integer supplierId;
    private String supplierCode;
    private String supplierName;
    private Integer salesmanId;
    private Integer bookingPersonId;
    private Integer categoryId;
    private Integer cityId;
    private Integer locationTypeId;
    private Integer cropYearId;
    private Integer multiCurrencyId;
    private Double exchangeRate;
    private String vehicleNo;
    private String gatePassNo;
    private String biltyNo;
    private BigDecimal grossWeight;
    private BigDecimal tareWeight;
    private BigDecimal netWeight;
    private String remarks;
    private String deliveryStartDate;
    private Integer deliveryDays;
    private String dueDate;
    private Integer dueDays;
    private Boolean hasCommission;
    private BigDecimal commissionRate;
    private BigDecimal commissionAmount;
    private BigDecimal subTotal;
    private BigDecimal totalTax;
    private BigDecimal totalFreight;
    private BigDecimal totalDiscount;
    private BigDecimal netTotal;
    private List<PurchaseLineItemDto> lineItems;
}
