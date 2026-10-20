package com.mst.models;

import java.io.Serializable;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.ItemsReorderSchedule (source: Sp_ItemsReorderSchedule_Insert). */
@Entity
@Setter
@Getter
@Table(name = "ItemsReorderSchedule")
public class ItemsReorderSchedule implements Serializable {

    /** App-assigned: Sp_ItemsReorderSchedule_Insert computes ISNULL(MAX(Id),0)+1. */
    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "BranchesId")
    private Integer branchesId;

    @Column(name = "EffectiveDate")
    private LocalDateTime effectiveDate;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "ItemUomId")
    private Integer itemUomId;

    @Column(name = "MinQty")
    private Double minQty;

    @Column(name = "MaxQty")
    private Double maxQty;

    @Column(name = "OptimalQty")
    private Double optimalQty;

    @Column(name = "ReOrderPoint")
    private Double reOrderPoint;

    @Column(name = "SortNo")
    private Integer sortNo;

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
