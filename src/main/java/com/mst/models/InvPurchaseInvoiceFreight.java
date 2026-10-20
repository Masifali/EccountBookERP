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

/** Mapped 1:1 onto dbo.InvPurchaseInvoiceFreight (source: Sp_InvPurchaseInvoiceFreight_Insert, returns SCOPE_IDENTITY()). */
@Entity
@Setter
@Getter
@Table(name = "InvPurchaseInvoiceFreight")
public class InvPurchaseInvoiceFreight implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "InvPurchaseInvoiceId")
    private Integer invPurchaseInvoiceId;

    @Column(name = "InvGrnId")
    private Integer invGrnId;

    @Column(name = "FrQty")
    private Double frQty;

    @Column(name = "FrWeight")
    private Double frWeight;

    @Column(name = "FrRate")
    private Double frRate;

    @Column(name = "FreightAmount")
    private Double freightAmount;

    @Column(name = "Debit")
    private Double debit;

    @Column(name = "TansporterId")
    private Integer tansporterId;

    @Column(name = "Remarks")
    private String remarks;

    @Column(name = "Percentage")
    private Double percentage;

    @Column(name = "PurchaseOrderId")
    private Integer purchaseOrderId;

    @Column(name = "FreightId")
    private Integer freightId;

    @Column(name = "SupplierCustomerId")
    private Integer supplierCustomerId;

    @Column(name = "CustomRemarks")
    private String customRemarks;

}
