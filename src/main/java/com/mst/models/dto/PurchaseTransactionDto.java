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

    public Integer getId() { return id; }
    public void setId(Integer id) { this.id = id; }
    public Integer getDocumentTypeId() { return documentTypeId; }
    public void setDocumentTypeId(Integer documentTypeId) { this.documentTypeId = documentTypeId; }
    public Integer getDocNo() { return docNo; }
    public void setDocNo(Integer docNo) { this.docNo = docNo; }
    public String getDocNoDisplay() { return docNoDisplay; }
    public void setDocNoDisplay(String docNoDisplay) { this.docNoDisplay = docNoDisplay; }
    public String getDocDate() { return docDate; }
    public void setDocDate(String docDate) { this.docDate = docDate; }
    public Integer getSupplierId() { return supplierId; }
    public void setSupplierId(Integer supplierId) { this.supplierId = supplierId; }
    public String getSupplierCode() { return supplierCode; }
    public void setSupplierCode(String supplierCode) { this.supplierCode = supplierCode; }
    public String getSupplierName() { return supplierName; }
    public void setSupplierName(String supplierName) { this.supplierName = supplierName; }
    public Integer getSalesmanId() { return salesmanId; }
    public void setSalesmanId(Integer salesmanId) { this.salesmanId = salesmanId; }
    public Integer getBookingPersonId() { return bookingPersonId; }
    public void setBookingPersonId(Integer bookingPersonId) { this.bookingPersonId = bookingPersonId; }
    public Integer getCategoryId() { return categoryId; }
    public void setCategoryId(Integer categoryId) { this.categoryId = categoryId; }
    public Integer getCityId() { return cityId; }
    public void setCityId(Integer cityId) { this.cityId = cityId; }
    public Integer getLocationTypeId() { return locationTypeId; }
    public void setLocationTypeId(Integer locationTypeId) { this.locationTypeId = locationTypeId; }
    public Integer getCropYearId() { return cropYearId; }
    public void setCropYearId(Integer cropYearId) { this.cropYearId = cropYearId; }
    public Integer getMultiCurrencyId() { return multiCurrencyId; }
    public void setMultiCurrencyId(Integer multiCurrencyId) { this.multiCurrencyId = multiCurrencyId; }
    public Double getExchangeRate() { return exchangeRate; }
    public void setExchangeRate(Double exchangeRate) { this.exchangeRate = exchangeRate; }
    public String getVehicleNo() { return vehicleNo; }
    public void setVehicleNo(String vehicleNo) { this.vehicleNo = vehicleNo; }
    public String getGatePassNo() { return gatePassNo; }
    public void setGatePassNo(String gatePassNo) { this.gatePassNo = gatePassNo; }
    public String getBiltyNo() { return biltyNo; }
    public void setBiltyNo(String biltyNo) { this.biltyNo = biltyNo; }
    public BigDecimal getGrossWeight() { return grossWeight; }
    public void setGrossWeight(BigDecimal grossWeight) { this.grossWeight = grossWeight; }
    public BigDecimal getTareWeight() { return tareWeight; }
    public void setTareWeight(BigDecimal tareWeight) { this.tareWeight = tareWeight; }
    public BigDecimal getNetWeight() { return netWeight; }
    public void setNetWeight(BigDecimal netWeight) { this.netWeight = netWeight; }
    public String getRemarks() { return remarks; }
    public void setRemarks(String remarks) { this.remarks = remarks; }
    public String getDeliveryStartDate() { return deliveryStartDate; }
    public void setDeliveryStartDate(String deliveryStartDate) { this.deliveryStartDate = deliveryStartDate; }
    public Integer getDeliveryDays() { return deliveryDays; }
    public void setDeliveryDays(Integer deliveryDays) { this.deliveryDays = deliveryDays; }
    public String getDueDate() { return dueDate; }
    public void setDueDate(String dueDate) { this.dueDate = dueDate; }
    public Integer getDueDays() { return dueDays; }
    public void setDueDays(Integer dueDays) { this.dueDays = dueDays; }
    public Boolean getHasCommission() { return hasCommission; }
    public void setHasCommission(Boolean hasCommission) { this.hasCommission = hasCommission; }
    public BigDecimal getCommissionRate() { return commissionRate; }
    public void setCommissionRate(BigDecimal commissionRate) { this.commissionRate = commissionRate; }
    public BigDecimal getCommissionAmount() { return commissionAmount; }
    public void setCommissionAmount(BigDecimal commissionAmount) { this.commissionAmount = commissionAmount; }
    public BigDecimal getSubTotal() { return subTotal; }
    public void setSubTotal(BigDecimal subTotal) { this.subTotal = subTotal; }
    public BigDecimal getTotalTax() { return totalTax; }
    public void setTotalTax(BigDecimal totalTax) { this.totalTax = totalTax; }
    public BigDecimal getTotalFreight() { return totalFreight; }
    public void setTotalFreight(BigDecimal totalFreight) { this.totalFreight = totalFreight; }
    public BigDecimal getTotalDiscount() { return totalDiscount; }
    public void setTotalDiscount(BigDecimal totalDiscount) { this.totalDiscount = totalDiscount; }
    public BigDecimal getNetTotal() { return netTotal; }
    public void setNetTotal(BigDecimal netTotal) { this.netTotal = netTotal; }
    public List<PurchaseLineItemDto> getLineItems() { return lineItems; }
    public void setLineItems(List<PurchaseLineItemDto> lineItems) { this.lineItems = lineItems; }
}
