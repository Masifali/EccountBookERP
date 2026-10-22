package com.mst.models;

import java.io.Serializable;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.ItemAllocation (source: Sp_ItemAllocation_Insert / Sp_ItemAllocation_GetAllMethod). */
@Entity
@Setter
@Getter
@Table(name = "ItemAllocation")
public class ItemAllocation implements Serializable {

    /** App-assigned: Sp_ItemAllocation_Insert computes ISNULL(MAX(Id),0)+1. */
    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "BranchId")
    private Integer branchId;

    @Column(name = "IsActive")
    private Boolean isActive;
}
