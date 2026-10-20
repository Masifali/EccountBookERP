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
 * Mapped 1:1 onto dbo.JobLotsAllocationToBranch (source: USP_JobLotsAllocationToBranch_Insert /
 * USP_JobLotsAllocationToBranchDeleteById / USP_JobLotsAllocatedOrUnAllocatedToBranch / Proc_JobLot_Insert).
 * Id is app-assigned: the insert proc computes ISNULL(MAX(Id),0)+1 (see JobLotsAllocationToBranchRepository.maxId()).
 * Column order in the table (from the positional INSERT ... SELECT in Proc_JobLot_Insert):
 * Id, BranchId, JobLotId, EntryDate, EntryUserId, ModifyDate, ModifyUserId, OrganizationId, CompanyId, FinancialYearId.
 */
@Entity
@Setter
@Getter
@Table(name = "JobLotsAllocationToBranch")
public class JobLotsAllocationToBranch implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "BranchId")
    private Integer branchId;

    @Column(name = "JobLotId")
    private Integer jobLotId;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUserId")
    private Integer entryUserId;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "ModifyUserId")
    private Integer modifyUserId;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "FinancialYearId")
    private Integer financialYearId;
}
