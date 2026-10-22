package com.mst.models;

import java.io.Serializable;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.InventoryItemsOther (source: Sp_InventoryItemsOther_GetAllMethod). */
@Entity
@Setter
@Getter
@Table(name = "InventoryItemsOther")
public class InventoryItemsOther implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "OtherItemName")
    private String otherItemName;

    @Column(name = "OtherItemType")
    private String otherItemType;

    @Column(name = "StockGLAcId")
    private Integer stockGLAcId;

    @Column(name = "SaleGLAcId")
    private Integer saleGLAcId;

    @Column(name = "CogsGLAcId")
    private Integer cogsGLAcId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "BranchesId")
    private Integer branchesId;

    @Column(name = "ProjectsId")
    private Integer projectsId;

    @Column(name = "ItemName")
    private String itemName;
}
