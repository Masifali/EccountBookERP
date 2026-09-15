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

    public Integer getLineId() { return lineId; }
    public void setLineId(Integer lineId) { this.lineId = lineId; }
    public Integer getItemId() { return itemId; }
    public void setItemId(Integer itemId) { this.itemId = itemId; }
    public String getItemCode() { return itemCode; }
    public void setItemCode(String itemCode) { this.itemCode = itemCode; }
    public String getItemName() { return itemName; }
    public void setItemName(String itemName) { this.itemName = itemName; }
    public Integer getCropYearId() { return cropYearId; }
    public void setCropYearId(Integer cropYearId) { this.cropYearId = cropYearId; }
    public String getCropYear() { return cropYear; }
    public void setCropYear(String cropYear) { this.cropYear = cropYear; }
    public Integer getPackUomId() { return packUomId; }
    public void setPackUomId(Integer packUomId) { this.packUomId = packUomId; }
    public String getPackUom() { return packUom; }
    public void setPackUom(String packUom) { this.packUom = packUom; }
    public Double getPackEquivalent() { return packEquivalent; }
    public void setPackEquivalent(Double packEquivalent) { this.packEquivalent = packEquivalent; }
    public BigDecimal getQuantity() { return quantity; }
    public void setQuantity(BigDecimal quantity) { this.quantity = quantity; }
    public BigDecimal getWeight() { return weight; }
    public void setWeight(BigDecimal weight) { this.weight = weight; }
    public Integer getRateUomId() { return rateUomId; }
    public void setRateUomId(Integer rateUomId) { this.rateUomId = rateUomId; }
    public String getRateUom() { return rateUom; }
    public void setRateUom(String rateUom) { this.rateUom = rateUom; }
    public Double getRateEquivalent() { return rateEquivalent; }
    public void setRateEquivalent(Double rateEquivalent) { this.rateEquivalent = rateEquivalent; }
    public BigDecimal getItemRate() { return itemRate; }
    public void setItemRate(BigDecimal itemRate) { this.itemRate = itemRate; }
    public BigDecimal getAmount() { return amount; }
    public void setAmount(BigDecimal amount) { this.amount = amount; }
    public Integer getWarehouseId() { return warehouseId; }
    public void setWarehouseId(Integer warehouseId) { this.warehouseId = warehouseId; }
    public String getWarehouseName() { return warehouseName; }
    public void setWarehouseName(String warehouseName) { this.warehouseName = warehouseName; }
    public Integer getJobLotId() { return jobLotId; }
    public void setJobLotId(Integer jobLotId) { this.jobLotId = jobLotId; }
    public String getJobLotCode() { return jobLotCode; }
    public void setJobLotCode(String jobLotCode) { this.jobLotCode = jobLotCode; }
    public String getRemarks() { return remarks; }
    public void setRemarks(String remarks) { this.remarks = remarks; }
    public BigDecimal getTaxPercent() { return taxPercent; }
    public void setTaxPercent(BigDecimal taxPercent) { this.taxPercent = taxPercent; }
    public BigDecimal getTaxAmount() { return taxAmount; }
    public void setTaxAmount(BigDecimal taxAmount) { this.taxAmount = taxAmount; }
    public BigDecimal getDiscountPercent() { return discountPercent; }
    public void setDiscountPercent(BigDecimal discountPercent) { this.discountPercent = discountPercent; }
    public BigDecimal getDiscountAmount() { return discountAmount; }
    public void setDiscountAmount(BigDecimal discountAmount) { this.discountAmount = discountAmount; }
    public BigDecimal getNetAmount() { return netAmount; }
    public void setNetAmount(BigDecimal netAmount) { this.netAmount = netAmount; }
}
