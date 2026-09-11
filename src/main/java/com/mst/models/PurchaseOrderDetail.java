package com.mst.models;

import javax.persistence.*;
import lombok.Data;

@Entity
@Table(name = "PurchaseOrderDetail")
@Data
public class PurchaseOrderDetail {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "PurchaseOrderId", nullable = false)
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

    @Column(name = "ItemUomId")
    private Integer itemUomId;

    @Column(name = "GrossWeight")
    private Double grossWeight;

    @Column(name = "ItemRate")
    private Double itemRate;

    @Column(name = "ItemAmount")
    private Double itemAmount;

    @Column(name = "WarehouseId")
    private Integer warehouseId;

    @Column(name = "CommentsDetail", length = 250)
    private String commentsDetail;
}
