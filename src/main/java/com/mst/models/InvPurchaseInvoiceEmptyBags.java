package com.mst.models;

import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.InvPurchaseInvoiceEmptyBags (source: Sp_InvPurchaseInvoiceEmptyBags_Insert). Id is app-assigned: proc computes ISNULL(MAX(Id),0)+1 -> use InvPurchaseInvoiceEmptyBagsRepository.maxId(). */
@Entity
@Setter
@Getter
@Table(name = "InvPurchaseInvoiceEmptyBags")
public class InvPurchaseInvoiceEmptyBags implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "InvPurchaseInvoiceId")
    private Integer invPurchaseInvoiceId;

    @Column(name = "InvGrnId")
    private Integer invGrnId;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "PurchaseQty")
    private Double purchaseQty;

    @Column(name = "Rate")
    private Double rate;

    @Column(name = "Amount")
    private Double amount;

    @Column(name = "Remarks")
    private String remarks;

    @Column(name = "ReceivedQty")
    private Double receivedQty;

    @Column(name = "TypeId")
    private Integer typeId;

    @Column(name = "PurchaseOrderId")
    private Integer purchaseOrderId;

    @Column(name = "CreditAccountId")
    private Integer creditAccountId;

    @Column(name = "WarehouseId")
    private Integer warehouseId;

    @Column(name = "RateUomId")
    private Integer rateUomId;

    @Column(name = "CustomRemarks")
    private String customRemarks;

    @Column(name = "ItemConditionId")
    private Integer itemConditionId;

    @Column(name = "RackId")
    private Integer rackId;

    @Column(name = "BagsCondition")
    private String bagsCondition;

    @Column(name = "CommTradeTransactionHeaderId")
    private Integer commTradeTransactionHeaderId;

    @Column(name = "PackingMaterialId")
    private Integer packingMaterialId;

    @Column(name = "Qty")
    private BigDecimal qty;

    @Column(name = "StockTypeId")
    private Integer stockTypeId;

}
