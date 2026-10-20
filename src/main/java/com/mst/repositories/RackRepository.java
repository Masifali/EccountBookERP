package com.mst.repositories;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.Rack;
import com.mst.projections.RackItemAllocationRow;
import com.mst.projections.RackReadAllRow;
import com.mst.projections.RackWithWarehouseRow;

/**
 * dbo.invWarehouseRack (entity Rack) — USP_invWarehouseRack_InsertAndUpdate, usp_invWarehouseRack_GetAllMethod,
 * usp_getRackswithWarehouse, usp_GetItemsByRackAllocation.
 * (The project already has IRackRepository; this one is additive and does not touch it.)
 *
 * USP_invWarehouseRack_InsertAndUpdate logic (callers may use save() for the insert part):
 *  - @Id NULL or 0 → INSERT with Id = ISNULL(MAX(Id),0)+1 (maxId() + 1), EntryDate = GETDATE(), EntryUserId; ActionId 1.
 *  - otherwise → UPDATE invWarehouseId, rackName, sortNo, isActive, ModifyDate = GETDATE(), ModifyUserId WHERE Id = @Id
 *    (updateById below); ActionId 2.
 *  - then EXEC USP_UserAudit_Insert (user = EntryUserId on insert / ModifyUserId on update, screen 'frmRackDefine',
 *    OrganizationId/CompanyId looked up from InvWareHouse by @invWarehouseId); proc returns SELECT @Id.
 */
@Repository
public interface RackRepository extends JpaRepository<Rack, Integer> {

    /** USP_invWarehouseRack_InsertAndUpdate — next app-assigned Id: ISNULL(MAX(Id),0) (+1 by the caller). */
    @Query("select coalesce(max(e.id),0) from Rack e")
    Integer maxId();

    /**
     * USP_invWarehouseRack_InsertAndUpdate (single branch) — full procedure (insert-or-update + user audit).
     * All proc parameters default to NULL.
     * // KEEP-EXEC: calls USP_UserAudit_Insert after the insert/update.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.USP_invWarehouseRack_InsertAndUpdate @Id = :id, @invWarehouseId = :invWarehouseId, "
         + "@rackName = :rackName, @sortNo = :sortNo, @isActive = :isActive, @EntryDate = :entryDate, "
         + "@EntryUserId = :entryUserId, @ModifyDate = :modifyDate, @ModifyUserId = :modifyUserId", nativeQuery = true)
    void uspInvWarehouseRackInsertAndUpdate(@Param("id") Integer id,
                                            @Param("invWarehouseId") Integer invWarehouseId,
                                            @Param("rackName") String rackName,
                                            @Param("sortNo") Integer sortNo,
                                            @Param("isActive") Boolean isActive,
                                            @Param("entryDate") LocalDateTime entryDate,
                                            @Param("entryUserId") Integer entryUserId,
                                            @Param("modifyDate") LocalDateTime modifyDate,
                                            @Param("modifyUserId") Integer modifyUserId);

    /**
     * USP_invWarehouseRack_InsertAndUpdate — the UPDATE part (every SET column; ModifyDate = GETDATE() as in the proc).
     * Native because invWarehouseId is mapped through the Rack.warehouse relation (no JPQL path update).
     */
    @Modifying
    @Transactional
    @Query(value = "UPDATE [dbo].[invWarehouseRack] SET [invWarehouseId] = :invWarehouseId, [rackName] = :rackName, "
         + "[sortNo] = :sortNo, isActive = :isActive, [ModifyDate] = GETDATE(), [ModifyUserId] = :modifyUserId "
         + "WHERE [Id] = :id", nativeQuery = true)
    int updateById(@Param("id") Integer id,
                   @Param("invWarehouseId") Integer invWarehouseId,
                   @Param("rackName") String rackName,
                   @Param("sortNo") Integer sortNo,
                   @Param("isActive") Boolean isActive,
                   @Param("modifyUserId") Integer modifyUserId);

    /**
     * usp_invWarehouseRack_GetAllMethod @Activity='ReadAll' — native because UserAccount.UserTitle (computed column)
     * is not mapped on the UserAccount entity.
     */
    @Query(value = "SELECT W.[Id], W.[invWarehouseId], WH.WareHouseCode, WH.WareHouseName, W.[rackName], W.[sortNo], W.isActive, "
         + "W.[EntryDate], W.[EntryUserId], UR.UserTitle AS EntryUser, W.[ModifyDate], W.[ModifyUserId], MUR.UserTitle AS ModifyUser "
         + "FROM [dbo].[invWarehouseRack] W "
         + "INNER JOIN InvWareHouse WH ON W.invWarehouseId = WH.Id "
         + "INNER JOIN UserAccount UR ON W.EntryUserId = UR.ID "
         + "LEFT JOIN UserAccount MUR ON W.ModifyUserId = MUR.ID "
         + "WHERE WH.OrganizationId = :organizationId AND WH.CompanyId = :companyId", nativeQuery = true)
    List<RackReadAllRow> readAll(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** usp_getRackswithWarehouse (single branch) — active racks with their warehouse name. */
    @Query("SELECT h.id AS id, h.rackName AS rackName, wh.id AS invWarehouseId, wh.wareHouseName AS wareHouseName "
         + "FROM Rack h JOIN h.warehouse wh "
         + "WHERE wh.organizationId = :organizationId AND wh.companyId = :companyId AND h.isActive = true")
    List<RackWithWarehouseRow> getRackswithWarehouse(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /**
     * usp_GetItemsByRackAllocation (single branch) — native (InventoryParentCategories has no entity).
     * @ActionId 1 = items not allocated to @StoreRackId, 2 = allocated; the proc RAISERRORs for any other value
     * ('Invalid ActionId. Use 1 (Unallocated) or 2 (Allocated)') — this query simply returns no rows in that case, so
     * callers must validate ActionId themselves. Only items whose category parent is IN (7,8) are returned.
     */
    @Query(value = "SELECT I.Id, I.ItemName, ISNULL(A.isActive, CAST(1 AS BIT)) AS isActive, p.InvParentCateDescription As ParentCategory "
         + "FROM Item I "
         + "INNER JOIN ItemCategory IC ON I.ItemCategoryId = IC.Id "
         + "LEFT JOIN ItemAllocationToStoreRack A ON I.Id = A.itemId AND A.storeRackId = :storeRackId "
         + "INNER JOIN InventoryParentCategories p ON p.Id = IC.InventoryParentCategoriesId "
         + "WHERE I.OrganizationId = :organizationId AND I.CompanyId = :companyId "
         + "AND IC.InventoryParentCategoriesId IN (7,8) "
         + "AND ((:actionId = 1 AND A.itemId IS NULL) OR (:actionId = 2 AND A.itemId IS NOT NULL))", nativeQuery = true)
    List<RackItemAllocationRow> getItemsByRackAllocation(@Param("organizationId") Integer organizationId,
                                                         @Param("companyId") Integer companyId,
                                                         @Param("actionId") Integer actionId,
                                                         @Param("storeRackId") Integer storeRackId);
}
