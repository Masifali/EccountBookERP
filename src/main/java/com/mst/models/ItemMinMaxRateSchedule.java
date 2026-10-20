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
 * Mapped 1:1 onto dbo.ItemMinMaxRateSchedule (source: USP_ItemMinMaxRateSchedule_Insert /
 * usp_GetItemsFromMinAndMaxRateSchedule / USP_GetItemsFromMinAndMaxRateSchedule_FormHistory /
 * usp_GetLastItemMinAndMaxRateByItemIdAndUomId / usp_MinMaxDeletionById / fed.usp_ItemPricingSchedule_Insert).
 *
 * NOTE: the registry also lists CategoryDescription, InventoryParentCategoriesId, ItemCategoryId, ItemTypeId,
 * ItemName and ItemCodeNew for this table. Those are columns of the #TmpResult temp table aliased "R" in
 * USP_GetItemsFromMinAndMaxRateSchedule_FormHistory, NOT real columns of dbo.ItemMinMaxRateSchedule (they come
 * from Item / ItemCategory). They are deliberately left out so that save()/JPQL on this entity do not hit
 * non-existent columns.
 */
@Entity
@Setter
@Getter
@Table(name = "ItemMinMaxRateSchedule")
public class ItemMinMaxRateSchedule implements Serializable {

    /** App-assigned: USP_ItemMinMaxRateSchedule_Insert computes ISNULL(MAX(Id),0)+1. */
    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "EffectedDate")
    private LocalDateTime effectedDate;

    @Column(name = "MinRate")
    private Double minRate;

    @Column(name = "MaxRate")
    private Double maxRate;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "BranchesId")
    private Integer branchesId;

    @Column(name = "FinancialYearId")
    private Integer financialYearId;

    @Column(name = "RateUomId")
    private Integer rateUomId;
}
