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
 * Mapped 1:1 onto dbo.UserAudit (source: USP_InvGrn_UpdateSpecialApproval).
 * Id (bigint) is app-assigned: the proc computes ISNULL(MAX(Id),0) + ROW_NUMBER() — see
 * {@code UserAuditRepository.maxId()}.
 */
@Entity
@Setter
@Getter
@Table(name = "UserAudit")
public class UserAudit implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    @Column(name = "Id")
    private Long id;

    @Column(name = "ActivityDate")
    private LocalDateTime activityDate;

    @Column(name = "UserId")
    private Integer userId;

    @Column(name = "ClientIP")
    private String clientIP;

    @Column(name = "ActivityDescription")
    private String activityDescription;

    @Column(name = "RefDocumentTypeId")
    private Integer refDocumentTypeId;

    @Column(name = "ReferenceId")
    private Long referenceId;

    @Column(name = "ScreenName")
    private String screenName;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "ActionId")
    private Integer actionId;
}
