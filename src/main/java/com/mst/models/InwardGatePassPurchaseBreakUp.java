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

    public Integer getId() { return id; }
    public void setId(Integer id) { this.id = id; }
    public Integer getInwardGatePassId() { return inwardGatePassId; }
    public void setInwardGatePassId(Integer inwardGatePassId) { this.inwardGatePassId = inwardGatePassId; }
    public Double getQty() { return qty; }
    public void setQty(Double qty) { this.qty = qty; }
    public Double getUom() { return uom; }
    public void setUom(Double uom) { this.uom = uom; }
    public Double getGrossWeight() { return grossWeight; }
    public void setGrossWeight(Double grossWeight) { this.grossWeight = grossWeight; }
    public Double getEbWeight() { return ebWeight; }
    public void setEbWeight(Double ebWeight) { this.ebWeight = ebWeight; }
    public Double getEbTotal() { return ebTotal; }
    public void setEbTotal(Double ebTotal) { this.ebTotal = ebTotal; }
    public Double getNetWeight() { return netWeight; }
    public void setNetWeight(Double netWeight) { this.netWeight = netWeight; }
}
