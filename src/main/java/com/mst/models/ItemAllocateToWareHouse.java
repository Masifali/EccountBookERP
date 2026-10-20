package com.mst.models;

import java.io.Serializable;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/**
 * Mapped 1:1 onto dbo.ItemAllocateToWareHouse (source: Sp_ItemAllocateToWareHouse_Insert).
 * The insert proc returns SCOPE_IDENTITY(), so Id is a SQL Server IDENTITY column.
 */
@Entity
@Setter
@Getter
@Table(name = "ItemAllocateToWareHouse")
public class ItemAllocateToWareHouse implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "WarehouseId")
    private Integer warehouseId;

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

    @Column(name = "BrancheId")
    private Integer brancheId;

    @Column(name = "ProjectsId")
    private Integer projectsId;

    @Column(name = "ActionId")
    private Integer actionId;
}
