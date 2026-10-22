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
 * Mapped 1:1 onto dbo.ItemPricingSchedule (source: Sp_ItemPricingSchedule_Insert).
 * Note: fed.ItemPricingSchedule is a different table (different columns) and is mapped by {@link FedItemPricingSchedule}.
 */
@Entity
@Setter
@Getter
@Table(name = "ItemPricingSchedule")
public class ItemPricingSchedule implements Serializable {

    /** Identity column: Sp_ItemPricingSchedule_Insert does not supply Id and returns SCOPE_IDENTITY(). */
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ScheduleDesc")
    private String scheduleDesc;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "EffectedDate")
    private LocalDateTime effectedDate;

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

    @Column(name = "BranchesId")
    private Integer branchesId;

    @Column(name = "PriceTypeId")
    private Integer priceTypeId;

    @Column(name = "ItemPrice")
    private Double itemPrice;

    @Column(name = "ChangedPriceLookUpId")
    private Integer changedPriceLookUpId;

    @Column(name = "Margin")
    private Double margin;

    @Column(name = "PackingUom")
    private Integer packingUom;

    @Column(name = "RateUomId")
    private Integer rateUomId;

    @Column(name = "PackingRate")
    private Double packingRate;

    @Column(name = "CropYearId")
    private Integer cropYearId;

    @Column(name = "IsActive")
    private Boolean isActive;
}
