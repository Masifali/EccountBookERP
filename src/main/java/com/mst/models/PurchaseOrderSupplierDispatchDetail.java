package com.mst.models;

import java.io.Serializable;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/**
 * Mapped 1:1 onto dbo.PurchaseOrderSupplierDispatchDetail (source: USP_PurchaseOrderSupplierDispatchDetail_Insert,
 * Sp_PurchaseOrder_GetAllMethod 'ReadByHeaderId_PurchaseOrderSupplierDispatchDetail'). Id is app-assigned: ISNULL(MAX(Id),0)+1.
 */
@Entity
@Setter
@Getter
@Table(name = "PurchaseOrderSupplierDispatchDetail")
public class PurchaseOrderSupplierDispatchDetail implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "Amount")
    private Integer amount;

    @Column(name = "ItemQty")
    private Double itemQty;

    @Column(name = "ItemRate")
    private Double itemRate;

    @Column(name = "NetWeight")
    private Double netWeight;

    @Column(name = "CropYearId")
    private Integer cropYearId;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "ItemUOMId")
    private Integer itemUomId;

    @Column(name = "PurchaseOrderId")
    private Integer purchaseOrderId;

    @Column(name = "RateUOMId")
    private Integer rateUomId;

    @Column(name = "SupplierDispatchId")
    private Integer supplierDispatchId;

    @Column(name = "PackingTypeId")
    private Integer packingTypeId;

    @Column(name = "LoadingDetailId")
    private Integer loadingDetailId;
}
