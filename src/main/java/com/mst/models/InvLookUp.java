package com.mst.models;

import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.InvLookUp (source: Proc_InvLookup_Insert / _Update / _ReadById, Sp_InvLookup_GetAllMethod). Id is app-assigned: Proc_InvLookup_Insert does max(Id)+1. */
@Entity
@Setter
@Getter
@Table(name = "InvLookUp")
public class InvLookUp implements Serializable {

    @Id
    // app-assigned (proc computes max(Id)+1 or the table has no insert proc in this batch)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "InvLookupTypeId")
    private Integer invLookupTypeId;

    @Column(name = "Code")
    private Integer code;

    @Column(name = "LookupName")
    private String lookupName;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "EntryDate")
    private LocalDate entryDate;

    @Column(name = "ModifyUser")
    private Integer modifyUser;

    @Column(name = "ModifyDate")
    private String modifyDate;

    @Column(name = "PostDate")
    private LocalDateTime postDate;

    @Column(name = "PostUser")
    private Integer postUser;

    @Column(name = "PostState")
    private Boolean postState;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "OtherItemName")
    private String otherItemName;
}
