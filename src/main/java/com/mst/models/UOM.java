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

/** Mapped 1:1 onto dbo.UOM (source: Sp_UOM_Insert / _Update / _GetAllMethod). Id app-assigned: Sp_UOM_Insert does max(Id)+1. */
@Entity
@Setter
@Getter
@Table(name = "UOM")
public class UOM implements Serializable {

    @Id
    // app-assigned (proc computes max(Id)+1 or the table has no insert proc in this batch)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "UOMCode")
    private String uomCode;

    @Column(name = "UOMDescription")
    private String uomDescription;

    @Column(name = "UOMSymbol")
    private String uomSymbol;

    @Column(name = "UOMStatus")
    private Boolean uomStatus;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "ModifyUser")
    private Integer modifyUser;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "Equivalent")
    private Double equivalent;

    @Column(name = "QtyEquivalent")
    private Double qtyEquivalent;

    @Column(name = "ParentUOMId")
    private Integer parentUOMId;
}
