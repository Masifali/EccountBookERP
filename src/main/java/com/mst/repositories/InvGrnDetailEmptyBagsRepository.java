package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InvGrnDetailEmptyBags;

/**
 * Repository for dbo.InvGrnDetailEmptyBags.
 *
 * Covers Sp_InvGrnDetailEmptyBags_Insert and Sp_InvGrnDetail_GetAllMethod @Activity='ReadByInvGrnIdEmptyBagsDetail'.
 *
 * Sp_InvGrnDetailEmptyBags_Insert pre-insert logic (in plain words):
 * - reads OrganizationId / CompanyId from InvGrn (@InvGrnId);
 * - WarehouseId is ALWAYS overwritten with configuration 219 (packing material default warehouse,
 *   dbo.GetConfigurationByConfigId) — error if not mapped (see {@link #defaultEmptyBagsWarehouseId});
 * - TypeId 2/3 (purchase against weight / free of cost): ReceivedQty must not be > 0;
 *   TypeId 4/5 (returned / retained): PurchaseQty must not be > 0;
 * - @ItemUomId is accepted but NOT inserted (column commented out); @Id is ignored (identity);
 * - INSERT then SELECT SCOPE_IDENTITY() (with save() the id is on the entity).
 */
@Repository
public interface InvGrnDetailEmptyBagsRepository extends JpaRepository<InvGrnDetailEmptyBags, Integer> {

    /** Sp_InvGrnDetail_GetAllMethod @Activity='ReadByInvGrnIdEmptyBagsDetail' — SELECT * FROM InvGrnDetailEmptyBags WHERE InvGrnId = @Id. */
    List<InvGrnDetailEmptyBags> findByInvGrnId(Integer invGrnId);

    /** Sp_InvGrnDetailEmptyBags_Insert helper: packing material default warehouse (configuration 219) for the GRN's organization / company. */
    @Query(nativeQuery = true, value = "SELECT CAST(dbo.GetConfigurationByConfigId(g.OrganizationId, g.CompanyId, 219) AS INT) FROM InvGrn g WHERE g.Id = :invGrnId")
    Integer defaultEmptyBagsWarehouseId(@Param("invGrnId") Integer invGrnId);

    /**
     * Sp_InvGrnDetailEmptyBags_Insert — full procedure (configuration lookup, TypeId validations, INSERT, SELECT SCOPE_IDENTITY()).
     * Returns the new identity. All parameters default to NULL. KEEP-EXEC: RAISERROR validations + config-driven WarehouseId.
     * Not @Modifying on purpose: the proc ends with SELECT SCOPE_IDENTITY() (SET NOCOUNT ON).
     */
    @Transactional
    @Query(nativeQuery = true, value = "EXEC dbo.Sp_InvGrnDetailEmptyBags_Insert @Id = :id, @ItemId = :itemId, @ItemUomId = :itemUomId, @InvGrnId = :invGrnId, "
            + "@ReceivedQty = :receivedQty, @PurchaseQty = :purchaseQty, @Remarks = :remarks, @WarehouseId = :warehouseId, @TypeId = :typeId, "
            + "@PurchaseOrderId = :purchaseOrderId, @BagsCondition = :bagsCondition, @RackId = :rackId")
    Integer spInvGrnDetailEmptyBagsInsert(@Param("id") Integer id, @Param("itemId") Integer itemId, @Param("itemUomId") Integer itemUomId,
            @Param("invGrnId") Integer invGrnId, @Param("receivedQty") Double receivedQty, @Param("purchaseQty") Double purchaseQty,
            @Param("remarks") String remarks, @Param("warehouseId") Integer warehouseId, @Param("typeId") Integer typeId,
            @Param("purchaseOrderId") Integer purchaseOrderId, @Param("bagsCondition") Integer bagsCondition, @Param("rackId") Integer rackId);

    /** Sp_InvGrn_Update standalone step (DocumentType 46/143/165/167/169/137/217): delete InvGrnDetailEmptyBags where InvGrnId = @Id. */
    @Modifying
    @Transactional
    @Query("DELETE FROM InvGrnDetailEmptyBags e WHERE e.invGrnId = :invGrnId")
    int deleteByInvGrnId(@Param("invGrnId") Integer invGrnId);
}
