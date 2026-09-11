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
}
