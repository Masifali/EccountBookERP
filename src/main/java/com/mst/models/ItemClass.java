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

/** Mapped 1:1 onto dbo.ItemClass (source: Sp_ItemClass_GetAllMethod). PK column is ClassId. */
@Entity
@Setter
@Getter
@Table(name = "ItemClass")
public class ItemClass implements Serializable {

    @Id
    // app-assigned (proc computes max(Id)+1 or the table has no insert proc in this batch)
    @Column(name = "ClassId")
    private Integer classId;

    @Column(name = "ClassCode")
    private String classCode;

    @Column(name = "ClassDescription")
    private String classDescription;

    @Column(name = "PostState")
    private Boolean postState;

    @Column(name = "PostUser")
    private Integer postUser;

    @Column(name = "PostDate")
    private LocalDateTime postDate;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "EnteryUser")
    private Integer enteryUser;

    @Column(name = "EnteryDate")
    private LocalDateTime enteryDate;

    @Column(name = "ModifyUser")
    private Integer modifyUser;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;
}
