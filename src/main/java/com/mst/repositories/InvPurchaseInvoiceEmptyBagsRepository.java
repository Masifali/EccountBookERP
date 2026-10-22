package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InvPurchaseInvoiceEmptyBags;

/**
 * Repository for dbo.InvPurchaseInvoiceEmptyBags (source: Sp_InvPurchaseInvoiceEmptyBags_Insert).
 *
 * Pre-insert logic of the proc (when using save()):
 *  - OrganizationId/CompanyId are read from InvPurchaseInvoice (InvPurchaseInvoiceRepository.readHeaderContext)
 *  - WarehouseId is OVERRIDDEN with CAST(dbo.GetConfigurationByConfigId(Org, Comp, 219) AS INT) (packing-material default warehouse);
 *    0/NULL -> RAISERROR 'Packing Material Default Warehouse not map in configurations' (defaultPackingMaterialWarehouseId)
 *  - RateUomId is OVERRIDDEN with TOP 1 UOMSchedule.Id where Org/Comp/ItemId and Equivalent = 1; none -> RAISERROR (equivalentUomScheduleId)
 *  - Id = ISNULL(MAX(CONVERT(INT, Id)),0)+1 (app-assigned, maxId()); proc ends with SELECT @Id
 */
@Repository
public interface InvPurchaseInvoiceEmptyBagsRepository extends JpaRepository<InvPurchaseInvoiceEmptyBags, Integer> {

    /**
     * Sp_InvPurchaseInvoiceEmptyBags_Insert (single branch) — KEEP-EXEC: DB function GetConfigurationByConfigId, RAISERROR validations,
     * max(Id)+1 assignment. All parameters default to NULL (WarehouseId / RateUomId are recomputed by the proc). Returns the new Id (SELECT @Id).
     */
    @Transactional
    @Query(value = "EXEC dbo.Sp_InvPurchaseInvoiceEmptyBags_Insert @Id = :id, @InvPurchaseInvoiceId = :invPurchaseInvoiceId, @InvGrnId = :invGrnId, "
         + "@ItemId = :itemId, @PurchaseQty = :purchaseQty, @Rate = :rate, @Amount = :amount, @Remarks = :remarks, @ReceivedQty = :receivedQty, "
         + "@TypeId = :typeId, @PurchaseOrderId = :purchaseOrderId, @CreditAccountId = :creditAccountId, @WarehouseId = :warehouseId, "
         + "@RateUomId = :rateUomId, @CustomRemarks = :customRemarks, @ItemConditionId = :itemConditionId, @RackId = :rackId", nativeQuery = true)
    Integer spInvPurchaseInvoiceEmptyBagsInsert(@Param("id") Integer id,
                                                @Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId,
                                                @Param("invGrnId") Integer invGrnId,
                                                @Param("itemId") Integer itemId,
                                                @Param("purchaseQty") Double purchaseQty,
                                                @Param("rate") Double rate,
                                                @Param("amount") Double amount,
                                                @Param("remarks") String remarks,
                                                @Param("receivedQty") Double receivedQty,
                                                @Param("typeId") Integer typeId,
                                                @Param("purchaseOrderId") Integer purchaseOrderId,
                                                @Param("creditAccountId") Integer creditAccountId,
                                                @Param("warehouseId") Integer warehouseId,
                                                @Param("rateUomId") Integer rateUomId,
                                                @Param("customRemarks") String customRemarks,
                                                @Param("itemConditionId") Integer itemConditionId,
                                                @Param("rackId") Integer rackId);

    /** Sp_InvPurchaseInvoiceEmptyBags_Insert: ISNULL(MAX(CONVERT(INT, Id)),0) — caller adds 1 and sets it on the entity before save(). */
    @Query("SELECT COALESCE(MAX(e.id), 0) FROM InvPurchaseInvoiceEmptyBags e")
    Integer maxId();

    /** Sp_InvPurchaseInvoiceEmptyBags_Insert: CAST(dbo.GetConfigurationByConfigId(Org, Comp, 219) AS INT) — packing-material default warehouse. Native: DB scalar function. */
    @Query(value = "SELECT CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 219) AS INT)", nativeQuery = true)
    Integer defaultPackingMaterialWarehouseId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_InvPurchaseInvoiceEmptyBags_Insert: TOP 1 UOMSchedule.Id for Org/Comp/ItemId with Equivalent = 1. Take first; empty -> 'UomSchedule was not found for this item'. */
    @Query("SELECT u.id FROM UOMSchedule u WHERE u.organizationId = :organizationId AND u.companyId = :companyId AND u.itemId = :itemId AND u.equivalent = 1")
    List<Integer> equivalentUomScheduleId(@Param("organizationId") Integer organizationId,
                                          @Param("companyId") Integer companyId,
                                          @Param("itemId") Integer itemId);

    /** Sp_InvPurchaseInvoice_Update / Sp_InvoicesVouchersandStocksDelete: delete InvPurchaseInvoiceEmptyBags where InvPurchaseInvoiceId = @Id. */
    @Modifying
    @Transactional
    @Query("DELETE FROM InvPurchaseInvoiceEmptyBags e WHERE e.invPurchaseInvoiceId = :invPurchaseInvoiceId")
    int deleteByInvPurchaseInvoiceId(@Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId);

    /** Sp_InvoicesVouchersandStocksDelete (purchase invoice branch): archive copy INSERT INTO H_InvPurchaseInvoiceEmptyBags (verbatim column list). Native: history table has no entity. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO H_InvPurchaseInvoiceEmptyBags SELECT [Id], [InvPurchaseInvoiceId], [InvGrnId], [ItemId], [PurchaseQty], [Rate], [Amount], "
         + "[Remarks], [ReceivedQty], [TypeId], [PurchaseOrderId], [CreditAccountId], [WarehouseId], [RateUomId] "
         + "FROM [dbo].[InvPurchaseInvoiceEmptyBags] WHERE InvPurchaseInvoiceId = :invPurchaseInvoiceId", nativeQuery = true)
    int archiveByInvPurchaseInvoiceId(@Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId);
}
