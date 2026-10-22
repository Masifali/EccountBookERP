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
 * Mapped 1:1 onto dbo.WarehousesAllocationToBranch (source: USP_WarehousesAllocationToBranch_Insert /
 * USP_WarehousesAllocationToBranchDeleteById / USP_WarehousesAllocatedOrUnAllocatedToBranch /
 * USP_GetWarehousesAllocatedToBranch / Sp_InvWareHouse_Insert).
 * Id is app-assigned: the insert proc computes ISNULL(MAX(Id),0)+1 (see WarehousesAllocationToBranchRepository.maxId()).
 * Column order in the table (from the positional INSERT ... SELECT in Sp_InvWareHouse_Insert):
 * Id, BranchId, WarehouseId, EntryDate, EntryUserId, ModifyDate, ModifyUserId, OrganizationId, CompanyId, FinancialYearId.
 */
@Entity
@Setter
@Getter
@Table(name = "WarehousesAllocationToBranch")
public class WarehousesAllocationToBranch implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "BranchId")
    private Integer branchId;

    @Column(name = "WarehouseId")
    private Integer warehouseId;

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
