package com.mst.models;

import java.io.Serializable;
import java.time.LocalDate;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.ItemWiseCommissionSchedule (source: USP_ItemWiseCommission_InsertOrUpdate). Note the column is spelled OrgnizationId in the table. */
@Entity
@Setter
@Getter
@Table(name = "ItemWiseCommissionSchedule")
public class ItemWiseCommissionSchedule implements Serializable {

    /** App-assigned: USP_ItemWiseCommission_InsertOrUpdate computes ISNULL(MAX(Id),0)+1 on insert. */
    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "OnPurchase")
    private Boolean onPurchase;

    @Column(name = "OnSale")
    private Boolean onSale;

    @Column(name = "EffectedFromDate")
    private LocalDate effectedFromDate;

    @Column(name = "EntryDate")
    private LocalDate entryDate;

    @Column(name = "ModifyDate")
    private LocalDate modifyDate;

    @Column(name = "BranchesId")
    private Integer branchesId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "EntryUserId")
    private Integer entryUserId;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "ModifyUserId")
    private Integer modifyUserId;

    @Column(name = "OrgnizationId")
    private Integer orgnizationId;
}
