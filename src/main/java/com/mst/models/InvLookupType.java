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

/** Mapped 1:1 onto dbo.InvLookupType (source: Proc_InvLookupType_ReadAll / Proc_InvLookUpType_GenerateCodeByLookUpType). */
@Entity
@Setter
@Getter
@Table(name = "InvLookupType")
public class InvLookupType implements Serializable {

    @Id
    // app-assigned (proc computes max(Id)+1 or the table has no insert proc in this batch)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ProfileTypeName")
    private String profileTypeName;

    @Column(name = "Prefix")
    private String prefix;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "ModifyUser")
    private Integer modifyUser;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

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

    @Column(name = "SerialFrom")
    private Integer serialFrom;

    @Column(name = "SerialTo")
    private Integer serialTo;
}
