package com.mst.models;

import java.io.Serializable;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.ConsumptionItems (source: USP_ConsumptionItems_Insert / USP_ConsumptionItems_FormHistory / USP_GetItemsAllocationForConsumption). */
@Entity
@Setter
@Getter
@Table(name = "ConsumptionItems")
public class ConsumptionItems implements Serializable {

    /** App-assigned: USP_ConsumptionItems_Insert computes ISNULL(MAX(Id),0)+1. */
    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "IsActive")
    private Boolean isActive;

    @Column(name = "Remarks")
    private String remarks;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUserId")
    private Integer entryUserId;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "BranchesId")
    private Integer branchesId;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "ModifyUserId")
    private Integer modifyUserId;
}
