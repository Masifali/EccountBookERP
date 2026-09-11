package com.mst.models;

import javax.persistence.*;
import lombok.Data;

@Entity
@Table(name = "InvPurchaseInvoiceDetail")
@Data
public class InvPurchaseInvoiceDetail {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "InvPurchaseInvoiceId", nullable = false)
    private Integer invPurchaseInvoiceId;

    @Column(name = "InvGrnId")
    private Integer invGrnId;

    @Column(name = "PurchaseOrderId")
    private Integer purchaseOrderId;

    @Column(name = "ItemId", nullable = false)
    private Integer itemId;

    @Column(name = "PackingTypeId")
    private Integer packingTypeId;

    @Column(name = "CropYear", length = 50)
    private String cropYear;

    @Column(name = "JobLotId")
    private Integer jobLotId;

    @Column(name = "ItemQty")
    private Double itemQty;

    @Column(name = "GrossWeight")
    private Double grossWeight;

    @Column(name = "NetBillWeight")
    private Double netBillWeight;

    @Column(name = "ItemRate")
    private Double itemRate;

    @Column(name = "ItemAmount")
    private Double itemAmount;

    @Column(name = "WarehouseId")
    private Integer warehouseId;

    @Column(name = "RemarksDetail", length = 250)
    private String remarksDetail;
}
