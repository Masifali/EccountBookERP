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
 * Mapped 1:1 onto dbo.AccountsPayRecFollowUpHistory
 * (source: Sp_AccountsPayRecFollowUpHistory_Insert / _Update / _GetAllMethod).
 * Id is app-assigned: the Insert proc computes ISNULL(MAX(Id),0)+1 - see
 * AccountsPayRecFollowUpHistoryRepository#maxId().
 */
@Entity
@Setter
@Getter
@Table(name = "AccountsPayRecFollowUpHistory")
public class AccountsPayRecFollowUpHistory implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "DocumentTypeIdRef")
    private Integer documentTypeIdRef;

    @Column(name = "ChartOfAccountId")
    private Integer chartOfAccountId;

    @Column(name = "CommentsDetail")
    private String commentsDetail;

    @Column(name = "CommentsDate")
    private LocalDateTime commentsDate;

    @Column(name = "FollowupDate")
    private LocalDateTime followupDate;

    @Column(name = "NextFollowupDays")
    private Integer nextFollowupDays;

    @Column(name = "PromiseDate")
    private LocalDateTime promiseDate;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;
}
