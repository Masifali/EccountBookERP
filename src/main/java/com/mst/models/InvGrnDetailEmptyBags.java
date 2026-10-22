package com.mst.models;

import java.io.Serializable;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/**
 * Mapped 1:1 onto dbo.InvGrnDetailEmptyBags (empty bags received / purchased / returned per GRN).
 * Source: Sp_InvGrnDetailEmptyBags_Insert / Sp_InvGrnDetail_GetAllMethod @Activity='ReadByInvGrnIdEmptyBagsDetail'.
 * Id is identity (the Insert proc returns SCOPE_IDENTITY()).
 * ItemUomId is commented out of the Insert proc and is not mapped.
 */
@Entity
@Setter
@Getter
@Table(name = "InvGrnDetailEmptyBags")
public class InvGrnDetailEmptyBags implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "InvGrnId")
    private Integer invGrnId;

    @Column(name = "ReceivedQty")
    private Double receivedQty;

    @Column(name = "PurchaseQty")
    private Double purchaseQty;

    @Column(name = "Remarks")
    private String remarks;

    @Column(name = "WarehouseId")
    private Integer warehouseId;

    @Column(name = "TypeId")
    private Integer typeId;

    @Column(name = "PurchaseOrderId")
    private Integer purchaseOrderId;

    @Column(name = "BagsCondition")
    private Integer bagsCondition;

    @Column(name = "RackId")
    private Integer rackId;
}
