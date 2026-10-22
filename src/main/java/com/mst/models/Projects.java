package com.mst.models;

import java.io.Serializable;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.Projects (source: Sp_Projects_GetAllMethod). No insert proc in this batch - Id left app-assigned. */
@Entity
@Setter
@Getter
@Table(name = "Projects")
public class Projects implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ProjectCode")
    private String projectCode;

    @Column(name = "ProjectName")
    private String projectName;

    @Column(name = "ProjectDescription")
    private String projectDescription;

    @Column(name = "InternalExternal")
    private String internalExternal;

    @Column(name = "ProjectType")
    private String projectType;

    @Column(name = "StartDate")
    private LocalDateTime startDate;

    @Column(name = "EndDate")
    private LocalDateTime endDate;

    @Column(name = "SiteLocationName")
    private String siteLocationName;

    @Column(name = "ProjectManger")
    private String projectManger;

    @Column(name = "CustomerId")
    private Integer customerId;

    @Column(name = "CustomerClient")
    private String customerClient;

    @Column(name = "OrderNo")
    private Integer orderNo;

    @Column(name = "AddressDescription")
    private String addressDescription;

    @Column(name = "CityId")
    private Integer cityId;

    @Column(name = "ProjectGroupID")
    private Integer projectGroupID;

    @Column(name = "IsActive")
    private Boolean isActive;

    @Column(name = "ProjectStatus")
    private String projectStatus;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "ModifyUser")
    private Integer modifyUser;

    @Column(name = "IsApproved")
    private Boolean isApproved;

    @Column(name = "ApprovedUserId")
    private Integer approvedUserId;

    @Column(name = "ApproveDate")
    private LocalDateTime approveDate;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "BranchesId")
    private Integer branchesId;
}
