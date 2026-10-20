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
 * Mapped 1:1 onto dbo.JobLot (source: Proc_JobLot_Insert / Proc_JobLot_Update / SP_JobLot_ReadMethod).
 * Id is app-assigned: Proc_JobLot_Insert computes ISNULL(MAX(Id),0)+1 (see JobLotRepository.maxId()).
 */
@Entity
@Setter
@Getter
@Table(name = "JobLot")
public class JobLot implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "JobLotCode")
    private String jobLotCode;

    @Column(name = "JobLotDescription")
    private String jobLotDescription;

    @Column(name = "StartDate")
    private LocalDateTime startDate;

    @Column(name = "EndDate")
    private LocalDateTime endDate;

    @Column(name = "IsApproved")
    private Boolean isApproved;

    @Column(name = "JobStatus")
    private String jobStatus;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "ModifyUser")
    private Integer modifyUser;

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

    @Column(name = "JobTypeId")
    private Integer jobTypeId;

    @Column(name = "IsCompany")
    private Boolean isCompany;

    @Column(name = "IsThirdParty")
    private Boolean isThirdParty;

    @Column(name = "AccountId")
    private Integer accountId;

    @Column(name = "RefDocumentTypeId")
    private Integer refDocumentTypeId;

    @Column(name = "RefDocNoId")
    private Integer refDocNoId;

    // The three columns below are listed for dbo.JobLot in the batch registry but are NOT referenced by
    // any procedure in this batch. Verify they exist on dbo.JobLot before relying on them.

    @Column(name = "ConditionStatus")
    private String conditionStatus;

    @Column(name = "rackName")
    private String rackName;

    @Column(name = "PackTypeDesc")
    private String packTypeDesc;
}
