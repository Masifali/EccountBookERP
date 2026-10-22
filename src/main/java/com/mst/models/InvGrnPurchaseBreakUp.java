package com.mst.models;

import java.io.Serializable;
import java.math.BigDecimal;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/**
 * Mapped 1:1 onto dbo.InvGrnPurchaseBreakUp (source: USP_InvGrnPurchaseBreakUp_Insert).
 * Id is app-assigned: the insert proc computes ISNULL(MAX(CONVERT(int,Id)),0)+1 — see
 * {@code InvGrnPurchaseBreakUpRepository.maxId()}.
 */
@Entity
@Setter
@Getter
@Table(name = "InvGrnPurchaseBreakUp")
public class InvGrnPurchaseBreakUp implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "InvGrnId")
    private Integer invGrnId;

    @Column(name = "InwardGatePassId")
    private Integer inwardGatePassId;

    @Column(name = "InwardBreakupId")
    private Integer inwardBreakupId;

    @Column(name = "Qty")
    private BigDecimal qty;

    @Column(name = "UOM")
    private BigDecimal uom;

    @Column(name = "GrossWeight")
    private BigDecimal grossWeight;

    @Column(name = "EbWeight")
    private BigDecimal ebWeight;

    @Column(name = "EBTotal")
    private BigDecimal ebTotal;

    @Column(name = "SupplierShortWeight")
    private BigDecimal supplierShortWeight;

    @Column(name = "NetWeight")
    private BigDecimal netWeight;

    @Column(name = "NetPackSize")
    private BigDecimal netPackSize;

    @Column(name = "SupplierRcvdWeight")
    private BigDecimal supplierRcvdWeight;

    @Column(name = "ScaleShortage")
    private BigDecimal scaleShortage;

    @Column(name = "BillWeight")
    private BigDecimal billWeight;

    @Column(name = "NetBillWeight")
    private BigDecimal netBillWeight;
}
