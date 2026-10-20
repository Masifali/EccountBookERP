package com.mst.models;

import java.io.Serializable;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.ItemTaxSchedule (source: Sp_ItemTaxSchedule_Insert / _Update / _GetAllMethod / Usp_AllComboAgainstItemTaxSchedule). */
@Entity
@Setter
@Getter
@Table(name = "ItemTaxSchedule")
public class ItemTaxSchedule implements Serializable {

    /** App-assigned: Sp_ItemTaxSchedule_Insert computes ISNULL(MAX(Id),0)+1. */
    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "TaxTypeId")
    private Integer taxTypeId;

    @Column(name = "EffectedDate")
    private LocalDateTime effectedDate;

    @Column(name = "TaxName")
    private String taxName;

    @Column(name = "IsActive")
    private Boolean isActive;

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
}
