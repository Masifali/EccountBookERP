package com.mst.models;

import java.io.Serializable;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.UOMSchedule (source: Sp_ItemAllocation_Insert / Sp_ItemPricingSchedule_Insert / fed.usp_ItemPricingSchedule_Insert). */
@Entity
@Setter
@Getter
@Table(name = "UOMSchedule")
public class UOMSchedule implements Serializable {

    /** App-assigned: the inserting procs compute ISNULL(MAX(Id),0)+1. */
    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ScheduleUnitId")
    private Integer scheduleUnitId;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "Equivalent")
    private Double equivalent;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "BaseRateUom")
    private Boolean baseRateUom;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "ModifyUser")
    private Integer modifyUser;

    @Column(name = "QtyEquivalent")
    private Double qtyEquivalent;

    @Column(name = "BasePackUom")
    private Boolean basePackUom;

    @Column(name = "BaseSecondaryUom")
    private Boolean baseSecondaryUom;

    @Column(name = "Active")
    private Boolean active;
}
