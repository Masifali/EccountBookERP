package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ItemAllocation;
import com.mst.projections.ItemPendingForAllocationRow;
import com.mst.projections.ItemsAllocatedOrUnAllocatedRow;

/**
 * Sp_ItemAllocation_GetAllMethod + Sp_ItemAllocation_Insert.
 *
 * Sp_ItemAllocation_Insert pre-insert logic (all inside IF NOT EXISTS(ItemAllocation for Org/Company/Item)):
 *  - if no fed.ItemPricingSchedule row exists for Org/Company/Item: reads Item.EntryUser/BaseUnitId, UOM.Equivalent of that base unit,
 *    creates a UOMSchedule row (Id = max+1, Equivalent, EntryDate = GETDATE()-7, BaseRateUom = 1) when none exists for
 *    Org/Item/ScheduleUnitId (else reuses the existing one), then EXECs fed.usp_ItemPricingSchedule_Insert with ItemRate = 1
 *    if no fed.ItemPricingSchedule row exists for that RateUomId;
 *  - then Id = ISNULL(MAX(Id),0)+1 (use {@link #maxId()}), INSERT Id, ItemId, OrganizationId, CompanyId, BranchId, IsActive, SELECT @Id.
 * Full behaviour: {@link #spItemAllocationInsert}. Simple insert (no pricing bootstrap): save() after {@link #existsByOrganizationIdAndCompanyIdAndItemId}.
 */
@Repository
public interface ItemAllocationRepository extends JpaRepository<ItemAllocation, Integer> {

    /** Sp_ItemAllocation_Insert pre-insert: ISNULL(MAX(CONVERT(int,Id)),0) — caller adds 1. */
    @Query("select coalesce(max(e.id),0) from ItemAllocation e")
    Integer maxId();

    /** Sp_ItemAllocation_Insert guard: IF NOT EXISTS(... WHERE OrganizationId AND CompanyId AND ItemId). */
    boolean existsByOrganizationIdAndCompanyIdAndItemId(Integer organizationId, Integer companyId, Integer itemId);

    /**
     * Sp_ItemAllocation_Insert — full procedural behaviour (UOMSchedule bootstrap + fed.usp_ItemPricingSchedule_Insert + insert).
     * All params default NULL. Returns the new Id (SELECT @Id) when a row was inserted.
     * // KEEP-EXEC: multi-table insert (UOMSchedule, fed.ItemPricingSchedule via nested EXEC) with IF/ELSE logic.
     */
    @Query(value = "EXEC dbo.Sp_ItemAllocation_Insert @Id = :id, @ItemId = :itemId, @OrganizationId = :organizationId, "
                 + "@CompanyId = :companyId, @BranchId = :branchId, @IsActive = :isActive", nativeQuery = true)
    Integer spItemAllocationInsert(@Param("id") Integer id,
                                   @Param("itemId") Integer itemId,
                                   @Param("organizationId") Integer organizationId,
                                   @Param("companyId") Integer companyId,
                                   @Param("branchId") Integer branchId,
                                   @Param("isActive") Boolean isActive);

    /** Sp_ItemAllocation_GetAllMethod @Activity='ItemPendingForAllocation' (items of the company with no ItemAllocation row). */
    @Query("SELECT i.organizationId AS organizationId, i.companyId AS companyId, i.id AS id, i.itemName AS itemName "
         + "FROM Item i LEFT JOIN ItemAllocation a ON i.id = a.itemId AND i.companyId = a.companyId "
         + "WHERE i.organizationId = :organizationId AND i.companyId = :companyId AND a.itemId IS NULL "
         + "GROUP BY i.organizationId, i.companyId, i.id, i.itemName")
    List<ItemPendingForAllocationRow> itemPendingForAllocation(@Param("organizationId") Integer organizationId,
                                                               @Param("companyId") Integer companyId);

    /**
     * Sp_ItemAllocation_GetAllMethod @Activity='UpdateItemStatusInAllocation' — sets IsActive = ISNULL(@Status,0) for the
     * comma-separated @ItemIds of the company. Native because of dbo.fnSplitString.
     */
    @Modifying
    @Transactional
    @Query(value = "update AoaAL set AoaAL.IsActive = ISNULL(:status,0) "
                 + "FROM dbo.Item AS i INNER JOIN dbo.ItemAllocation AS AoaAL ON i.Id = AoaAL.ItemId AND i.CompanyId = AoaAL.CompanyId "
                 + "Where i.OrganizationId = :organizationId And AoaAL.CompanyId = :companyId "
                 + "and ( EXISTS (select 1 from dbo.fnSplitString (:itemIds,',') Where AoaAL.ItemId in (data)) )", nativeQuery = true)
    int updateItemStatusInAllocation(@Param("organizationId") Integer organizationId,
                                     @Param("companyId") Integer companyId,
                                     @Param("itemIds") String itemIds,
                                     @Param("status") Boolean status);

    /** Sp_ItemAllocation_GetAllMethod @Activity='ItemsAllocatedOrUnAllocatedToCompany' (@ActionId 1 = not allocated, 2 = allocated). */
    @Query("SELECT i.organizationId AS organizationId, i.companyId AS companyId, i.id AS id, i.itemName AS itemName, a.itemId AS itemId "
         + "FROM Item i LEFT JOIN ItemAllocation a ON i.id = a.itemId AND i.companyId = a.companyId "
         + "WHERE i.organizationId = :organizationId "
         + "AND ( (:actionId = 1 AND i.id NOT IN (SELECT c.itemId FROM ItemAllocation c WHERE c.companyId = :companyId)) "
         + "   OR (:actionId = 2 AND i.id IN (SELECT c2.itemId FROM ItemAllocation c2 WHERE c2.companyId = :companyId)) )")
    List<ItemsAllocatedOrUnAllocatedRow> itemsAllocatedOrUnAllocatedToCompany(@Param("organizationId") Integer organizationId,
                                                                              @Param("companyId") Integer companyId,
                                                                              @Param("actionId") Integer actionId);

    /**
     * Sp_ItemAllocation_GetAllMethod @Activity='DeleteAllocatedItemsCompanyWise' — full behaviour: RAISERROR when any of the
     * @ItemIds is referenced in InventoryStockEvalautionDetail, otherwise deletes the ItemAllocation rows.
     * // KEEP-EXEC: RAISERROR guard + fnSplitString.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_ItemAllocation_GetAllMethod @OrganizationId = :organizationId, @CompanyId = :companyId, "
                 + "@Activity = 'DeleteAllocatedItemsCompanyWise', @ItemIds = :itemIds", nativeQuery = true)
    void deleteAllocatedItemsCompanyWise(@Param("organizationId") Integer organizationId,
                                         @Param("companyId") Integer companyId,
                                         @Param("itemIds") String itemIds);

    /** DeleteAllocatedItemsCompanyWise guard: count of InventoryStockEvalautionDetail rows referencing the @ItemIds (proc raises when &gt; 0). */
    @Query(value = "SELECT COUNT(1) FROM InventoryStockEvalautionDetail WHERE OrganizationId = :organizationId AND CompanyId = :companyId "
                 + "AND ItemId IN (select data From dbo.fnSplitString(:itemIds,','))", nativeQuery = true)
    Long countStockEvaluationReferences(@Param("organizationId") Integer organizationId,
                                        @Param("companyId") Integer companyId,
                                        @Param("itemIds") String itemIds);

    /** DeleteAllocatedItemsCompanyWise — the standalone DELETE statement (no guard). Native because of dbo.fnSplitString. */
    @Modifying
    @Transactional
    @Query(value = "Delete ItemAllocation WHERE OrganizationId = :organizationId AND CompanyId = :companyId "
                 + "AND (ItemId in (select data From dbo.fnSplitString(:itemIds,',')))", nativeQuery = true)
    int deleteByCompanyAndItemIds(@Param("organizationId") Integer organizationId,
                                  @Param("companyId") Integer companyId,
                                  @Param("itemIds") String itemIds);
}
