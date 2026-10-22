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

/** Mapped 1:1 onto dbo.InvCropYear (source: Sp_InvCropYear_Insert / _Update / _GetAllMethod). Insert uses SCOPE_IDENTITY() -> IDENTITY. */
@Entity
@Setter
@Getter
@Table(name = "InvCropYear")
public class InvCropYear implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "CropYear")
    private String cropYear;

    @Column(name = "DefaultKey")
    private Boolean defaultKey;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "EntryUserId")
    private Integer entryUserId;

    @Column(name = "ModifyUserId")
    private Integer modifyUserId;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "FeaturesId")
    private Integer featuresId;

    @Column(name = "IsActive")
    private Boolean isActive;
}
