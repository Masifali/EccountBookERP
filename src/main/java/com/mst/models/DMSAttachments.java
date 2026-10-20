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
 * Mapped 1:1 onto dbo.DMSAttachments (source: Proc_DMSAttachments_Insert / Sp_DMSAttachments_GetAllMethod).
 * Id is IDENTITY (the Insert proc returns SCOPE_IDENTITY()).
 * Only the real columns of the table are mapped; the many other names the registry lists for this
 * table (ItemQty, NetBillWeight, GrossWeight, ...) come from unrelated SELECTs that merely join
 * DMSAttachments and are not columns of it.
 */
@Entity
@Setter
@Getter
@Table(name = "DMSAttachments")
public class DMSAttachments implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "RefAccountId")
    private Integer refAccountId;

    @Column(name = "DMSFoldersLabelsId")
    private Integer dmsFoldersLabelsId;

    @Column(name = "RefDocumentTypeId")
    private Integer refDocumentTypeId;

    @Column(name = "RefDocumentNo")
    private Integer refDocumentNo;

    @Column(name = "Attachment")
    private String attachment;

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

    @Column(name = "BranchId")
    private Integer branchId;

    @Column(name = "ScreenName")
    private String screenName;

    @Column(name = "DetailWiseAttachment")
    private Boolean detailWiseAttachment;

    @Column(name = "UploadedFileCustomName")
    private String uploadedFileCustomName;

    @Column(name = "UploadedFileSizeMb")
    private Double uploadedFileSizeMb;

    @Column(name = "LineId")
    private Integer lineId;
}
