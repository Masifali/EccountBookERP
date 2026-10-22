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
 * Mapped 1:1 onto fed.ItemPricingSchedule (source: fed.usp_ItemPricingSchedule_Insert, also read by Sp_ItemAllocation_Insert).
 * This is a separate physical table from dbo.ItemPricingSchedule ({@link ItemPricingSchedule}); the two share only a name.
 */
@Entity
@Setter
@Getter
@Table(name = "ItemPricingSchedule", schema = "fed")
public class FedItemPricingSchedule implements Serializable {

    /** App-assigned: fed.usp_ItemPricingSchedule_Insert computes ISNULL(MAX(Id),0)+1. */
    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "EffectiveDate")
    private LocalDateTime effectiveDate;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "ItemRate")
    private Double itemRate;

    @Column(name = "RateUomId")
    private Integer rateUomId;

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

    @Column(name = "IsApproved")
    private Boolean isApproved;

    @Column(name = "IsActive")
    private Boolean isActive;

    @Column(name = "ApprovedUserId")
    private Integer approvedUserId;

    @Column(name = "ApprovedDate")
    private LocalDateTime approvedDate;

    @Column(name = "PricingCustomGroupId")
    private Integer pricingCustomGroupId;

    @Column(name = "DocumentTypeId")
    private Integer documentTypeId;
}
