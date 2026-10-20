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
 * Mapped 1:1 onto dbo.GridLayout (source: Sp_GridLayout_Insert / USP_GridLayout_ReadById / USP_GridLayout_DeleteById).
 * Id is app-assigned (Insert proc: ISNULL(MAX(Id),0)+1 - see GridLayoutRepository#maxId()).
 * LayoutFile is VARBINARY(MAX); LayoutFileXml is an XML column and is carried as its string form.
 */
@Entity
@Setter
@Getter
@Table(name = "GridLayout")
public class GridLayout implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ScreenName")
    private String screenName;

    @Column(name = "Layout")
    private String layout;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUserId")
    private Integer entryUserId;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "LayoutFile")
    private byte[] layoutFile;

    @Column(name = "LayoutFileXml")
    private String layoutFileXml;
}
