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
 * Mapped 1:1 onto dbo.driverBiodata (source: USP_DriverBiodata_InsertIfNotExists / USP_driverBiodata_GetAllMethod).
 * The key column is driverBiodataId (app-assigned: ISNULL(MAX(driverBiodataId),0)+1 - see
 * DriverBiodataRepository#maxDriverBiodataId()). RowVersion is a SQL Server TIMESTAMP (rowversion) -> byte[].
 */
@Entity
@Setter
@Getter
@Table(name = "driverBiodata")
public class DriverBiodata implements Serializable {

    @Id
    @Column(name = "driverBiodataId")
    private Integer driverBiodataId;

    @Column(name = "DocumentTypeId")
    private Integer documentTypeId;

    @Column(name = "driverName")
    private String driverName;

    @Column(name = "cnicNo")
    private String cnicNo;

    @Column(name = "cellNo")
    private String cellNo;

    @Column(name = "whatsappNo")
    private String whatsappNo;

    @Column(name = "alternateCellNo")
    private String alternateCellNo;

    @Column(name = "fatherName")
    private String fatherName;

    @Column(name = "fatherCnicNo")
    private String fatherCnicNo;

    @Column(name = "driverPicture")
    private String driverPicture;

    @Column(name = "driverFingerPrint")
    private String driverFingerPrint;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "BranchId")
    private Integer branchId;

    @Column(name = "EntryUserId")
    private Integer entryUserId;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "ModifyUserId")
    private Integer modifyUserId;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "IsApproved")
    private Boolean isApproved;

    @Column(name = "ApprovedUserId")
    private Integer approvedUserId;

    @Column(name = "ApprovedDate")
    private LocalDateTime approvedDate;

    @Column(name = "ActionId")
    private Integer actionId;

    @Column(name = "RevisionNo")
    private Integer revisionNo;

    @Column(name = "RowVersion", insertable = false, updatable = false)
    private byte[] rowVersion;

    @Column(name = "AttachmentsValues")
    private String attachmentsValues;

    @Column(name = "CustomAttachmentsValues")
    private String customAttachmentsValues;
}
