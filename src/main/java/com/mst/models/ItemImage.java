package com.mst.models;

import java.io.Serializable;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.ItemImage (source: USP_ItemImage_Insert). */
@Entity
@Setter
@Getter
@Table(name = "ItemImage")
public class ItemImage implements Serializable {

    /** App-assigned: USP_ItemImage_Insert computes ISNULL(MAX(Id),0)+1. */
    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "ImagePath")
    private String imagePath;

    @Column(name = "EffetedDate")
    private LocalDateTime effetedDate;

    @Column(name = "IsActive")
    private Boolean isActive;

    @Column(name = "FileName")
    private String fileName;

    @Column(name = "IsProfileImage")
    private Boolean isProfileImage;

    @Column(name = "SortNo")
    private Integer sortNo;
}
