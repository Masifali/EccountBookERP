package com.mst.models;

import lombok.Data;

import javax.persistence.*;

@Entity
@Table(name = "GatePassInwardDetail")
@Data
public class InwardGatePassDetail {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "GatePassInwardId")
    private Integer gatePassInwardId;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "PackUnit")
    private Double packUnit = 0.0;

    @Column(name = "Weight")
    private Double weight = 0.0;

    @Column(name = "ItemUOMId")
    private Integer itemUOMId;

    @Column(name = "ItemQty")
    private Double itemQty = 0.0;

    @Column(name = "CropYear")
    private String cropYear;

    @Column(name = "JobLotId")
    private Integer jobLotId;

    @Column(name = "SupplyScheduleId")
    private Integer supplyScheduleId;

    @Column(name = "WareHouseId")
    private Integer wareHouseId;

    @Column(name = "PackingTypeId")
    private Integer packingTypeId;

    @Column(name = "RefDocumentTypeId")
    private Integer refDocumentTypeId;

    @Column(name = "SupplierCustomerId")
    private Integer supplierCustomerId;

    @Column(name = "PurchaseOrderId")
    private Integer purchaseOrderId;

    @Column(name = "PurchaseOrderDetailId")
    private Integer purchaseOrderDetailId;

    @Column(name = "CityId")
    private Integer cityId;

    @Column(name = "RemarksDetail")
    private String remarksDetail;

    @Transient
    private Integer docNo;

    @Transient
    private Integer orderNo;

    @Transient
    private Double equivalent;

    @Transient
    private String itemName;

    @Transient
    private String uomCode;

    @Transient
    private String jobLotDescription;

    @Transient
    private String wareHouseName;

    @Transient
    private String packingType;

    @Transient
    private String cityName;

    // Explicit Getters and Setters
    public Integer getId() { return id; }
    public void setId(Integer id) { this.id = id; }
    public Integer getGatePassInwardId() { return gatePassInwardId; }
    public void setGatePassInwardId(Integer gatePassInwardId) { this.gatePassInwardId = gatePassInwardId; }
    public Integer getItemId() { return itemId; }
    public void setItemId(Integer itemId) { this.itemId = itemId; }
    public Double getWeight() { return weight; }
    public void setWeight(Double weight) { this.weight = weight; }
    public Integer getSupplierCustomerId() { return supplierCustomerId; }
    public void setSupplierCustomerId(Integer supplierCustomerId) { this.supplierCustomerId = supplierCustomerId; }
    public Integer getCityId() { return cityId; }
    public void setCityId(Integer cityId) { this.cityId = cityId; }
    public Double getPackUnit() { return packUnit; }
    public void setPackUnit(Double packUnit) { this.packUnit = packUnit; }
    public Integer getItemUOMId() { return itemUOMId; }
    public void setItemUOMId(Integer itemUOMId) { this.itemUOMId = itemUOMId; }
    public Double getItemQty() { return itemQty; }
    public void setItemQty(Double itemQty) { this.itemQty = itemQty; }
    public String getCropYear() { return cropYear; }
    public void setCropYear(String cropYear) { this.cropYear = cropYear; }
    public Integer getJobLotId() { return jobLotId; }
    public void setJobLotId(Integer jobLotId) { this.jobLotId = jobLotId; }
    public Integer getSupplyScheduleId() { return supplyScheduleId; }
    public void setSupplyScheduleId(Integer supplyScheduleId) { this.supplyScheduleId = supplyScheduleId; }
    public Integer getWareHouseId() { return wareHouseId; }
    public void setWareHouseId(Integer wareHouseId) { this.wareHouseId = wareHouseId; }
    public Integer getPackingTypeId() { return packingTypeId; }
    public void setPackingTypeId(Integer packingTypeId) { this.packingTypeId = packingTypeId; }
    public Integer getRefDocumentTypeId() { return refDocumentTypeId; }
    public void setRefDocumentTypeId(Integer refDocumentTypeId) { this.refDocumentTypeId = refDocumentTypeId; }
    public Integer getPurchaseOrderId() { return purchaseOrderId; }
    public void setPurchaseOrderId(Integer purchaseOrderId) { this.purchaseOrderId = purchaseOrderId; }
    public Integer getPurchaseOrderDetailId() { return purchaseOrderDetailId; }
    public void setPurchaseOrderDetailId(Integer purchaseOrderDetailId) { this.purchaseOrderDetailId = purchaseOrderDetailId; }
    public String getRemarksDetail() { return remarksDetail; }
    public void setRemarksDetail(String remarksDetail) { this.remarksDetail = remarksDetail; }
}
