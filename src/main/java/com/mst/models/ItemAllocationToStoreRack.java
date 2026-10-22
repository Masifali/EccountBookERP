package com.mst.models;

import java.io.Serializable;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/**
 * Mapped 1:1 onto dbo.ItemAllocationToStoreRack (source: USP_ItemAllocationToStoreRack_InsertAndUpdate /
 * usp_GetItemsByRackAllocation). Id is app-assigned: the proc computes ISNULL(MAX(Id),0)+1
 * (see ItemAllocationToStoreRackRepository.maxId()).
 */
@Entity
@Setter
@Getter
@Table(name = "ItemAllocationToStoreRack")
public class ItemAllocationToStoreRack implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "itemId")
    private Integer itemId;

    @Column(name = "storeRackId")
    private Integer storeRackId;

    @Column(name = "isActive")
    private Boolean isActive;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUserId")
    private Integer entryUserId;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "ModifyUserId")
    private Integer modifyUserId;
}
