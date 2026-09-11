package com.mst.models;

import lombok.Data;

import javax.persistence.*;

@Entity
@Table(name = "GatePassInwardPurchaseBreakUp")
@Data
public class InwardGatePassPurchaseBreakUp {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "InwardGatePassId")
    private Integer inwardGatePassId;

    @Column(name = "Qty")
    private Double qty = 0.0;

    @Column(name = "UOM")
    private Double uom = 0.0;

    @Column(name = "GrossWeight")
    private Double grossWeight = 0.0;

    @Column(name = "EbWeight")
    private Double ebWeight = 0.0;

    @Column(name = "EBTotal")
    private Double ebTotal = 0.0;

    @Column(name = "NetWeight")
    private Double netWeight = 0.0;
}
