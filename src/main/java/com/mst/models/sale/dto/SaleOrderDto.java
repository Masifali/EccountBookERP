package com.mst.models.sale.dto;

import lombok.Data;
import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

@Data
public class SaleOrderDto {
    private Integer id;
    private Integer voucherCode;
    private String orderDate;
    private String dueDate;
    private Integer dueDays = 0;
    private String deliveryStartDate;
    private Integer deliveryDays = 7;
    
    private Integer orderCategoryId;
    private Integer orderCategoryNo;
    private Integer categoryI_Id;
    private Integer categoryII_Id;
    
    private Integer customerId;
    private String partyRefNo;
    private Integer bookingPersonId;
    
    private Integer paymentTermId;
    private Integer deliveryTermId;
    private Integer orderStatusId = 1;
    private Integer locationTypeId = 1;
    private Integer branchId;
    
    private Integer salesManId;
    private String commType = "Flat";
    private BigDecimal commRate = BigDecimal.ZERO;
    private Integer commUomId;
    private BigDecimal commAmount = BigDecimal.ZERO;
    private String commRemarks;
    
    private Integer otherSalesManId;
    private String otherCommType = "Flat";
    private BigDecimal otherCommRate = BigDecimal.ZERO;
    private Integer otherCommUomId;
    private BigDecimal otherCommAmount = BigDecimal.ZERO;
    private String otherCommRemarks;
    
    private String remarks;
    private Integer currencyId;
    private BigDecimal exchangeRate = BigDecimal.ONE;
    
    private BigDecimal totalAmount = BigDecimal.ZERO;
    private BigDecimal totalWeight = BigDecimal.ZERO;
    private BigDecimal totalQuantity = BigDecimal.ZERO;

    private List<SaleOrderDetailDto> lineItems = new ArrayList<>();
    private List<SaleOrderExpenseDto> expenseItems = new ArrayList<>();
    private List<SaleOrderPaymentScheduleDto> paymentSchedules = new ArrayList<>();
}
