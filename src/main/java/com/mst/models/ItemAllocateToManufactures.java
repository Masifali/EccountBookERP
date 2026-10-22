package com.mst.models;

import java.io.Serializable;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.ItemAllocateToManufactures (source: USP_ItemAllocateToManufactures_Insert / Sp_ItemAllocateToManufactures_GetAllMethod). */
@Entity
@Setter
@Getter
@Table(name = "ItemAllocateToManufactures")
public class ItemAllocateToManufactures implements Serializable {

    /** App-assigned: USP_ItemAllocateToManufactures_Insert computes ISNULL(MAX(Id),0)+1. */
    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "ManufactureId")
    private Integer manufactureId;

    @Column(name = "IsActive")
    private Boolean isActive;
}
