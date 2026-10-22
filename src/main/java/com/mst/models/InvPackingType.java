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

/** Mapped 1:1 onto dbo.InvPackingType (source: Sp_InvPackingType_GetAllMethod). Description/TermsDescription/WareHouseName/CropYear come from registry.json only. */
@Entity
@Setter
@Getter
@Table(name = "InvPackingType")
public class InvPackingType implements Serializable {

    @Id
    // app-assigned (proc computes max(Id)+1 or the table has no insert proc in this batch)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "PackTypeCode")
    private String packTypeCode;

    @Column(name = "PackTypeDesc")
    private String packTypeDesc;

    @Column(name = "IsActive")
    private Boolean isActive;

    @Column(name = "MinEbWeight")
    private BigDecimal minEbWeight;

    @Column(name = "MaxEbWeight")
    private BigDecimal maxEbWeight;

    @Column(name = "MinEbStockWeight")
    private BigDecimal minEbStockWeight;

    @Column(name = "MaxEbStockWeight")
    private BigDecimal maxEbStockWeight;

    @Column(name = "Description")
    private String description;

    @Column(name = "TermsDescription")
    private String termsDescription;

    @Column(name = "WareHouseName")
    private String wareHouseName;

    @Column(name = "CropYear")
    private String cropYear;
}
