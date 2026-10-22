package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InvPurchaseInvoiceFreight;

/**
 * Repository for dbo.InvPurchaseInvoiceFreight (source: Sp_InvPurchaseInvoiceFreight_Insert).
 * The proc is a plain single-table INSERT (Id = IDENTITY, @Id parameter ignored) ending with SELECT SCOPE_IDENTITY()
 * -> callers use save() and read the id from the entity. No pre-insert logic.
 */
@Repository
public interface InvPurchaseInvoiceFreightRepository extends JpaRepository<InvPurchaseInvoiceFreight, Integer> {

    /** Sp_InvPurchaseInvoiceFreight_Insert (single branch) — equivalent to save(); kept as EXEC for callers that need the exact proc. All parameters default to NULL. Returns SCOPE_IDENTITY(). */
    @Transactional
    @Query(value = "EXEC dbo.Sp_InvPurchaseInvoiceFreight_Insert @Id = :id, @InvPurchaseInvoiceId = :invPurchaseInvoiceId, @InvGrnId = :invGrnId, "
         + "@FrQty = :frQty, @FrWeight = :frWeight, @FrRate = :frRate, @FreightAmount = :freightAmount, @Debit = :debit, "
         + "@TansporterId = :tansporterId, @Remarks = :remarks, @Percentage = :percentage, @PurchaseOrderId = :purchaseOrderId, "
         + "@FreightId = :freightId, @CustomRemarks = :customRemarks, @SupplierCustomerId = :supplierCustomerId", nativeQuery = true)
    Integer spInvPurchaseInvoiceFreightInsert(@Param("id") Integer id,
                                              @Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId,
                                              @Param("invGrnId") Integer invGrnId,
                                              @Param("frQty") Double frQty,
                                              @Param("frWeight") Double frWeight,
                                              @Param("frRate") Double frRate,
                                              @Param("freightAmount") Double freightAmount,
                                              @Param("debit") Double debit,
                                              @Param("tansporterId") Integer tansporterId,
                                              @Param("remarks") String remarks,
                                              @Param("percentage") Double percentage,
                                              @Param("purchaseOrderId") Integer purchaseOrderId,
                                              @Param("freightId") Integer freightId,
                                              @Param("customRemarks") String customRemarks,
                                              @Param("supplierCustomerId") Integer supplierCustomerId);

    /** Sp_InvPurchaseInvoice_Update / Sp_InvoicesVouchersandStocksDelete: delete InvPurchaseInvoiceFreight where InvPurchaseInvoiceId = @Id. */
    @Modifying
    @Transactional
    @Query("DELETE FROM InvPurchaseInvoiceFreight f WHERE f.invPurchaseInvoiceId = :invPurchaseInvoiceId")
    int deleteByInvPurchaseInvoiceId(@Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId);

    /** Sp_InvoicesVouchersandStocksDelete (purchase invoice branch): archive copy INSERT INTO H_InvPurchaseInvoiceFreight (verbatim column list). Native: history table has no entity. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO H_InvPurchaseInvoiceFreight SELECT [Id], [InvPurchaseInvoiceId], [InvGrnId], [FrQty], [FrWeight], [FrRate], [FreightAmount], "
         + "[Debit], [TansporterId], [Remarks], [Percentage], [PurchaseOrderId], [FreightId], [SupplierCustomerId] "
         + "FROM [dbo].[InvPurchaseInvoiceFreight] WHERE InvPurchaseInvoiceId = :invPurchaseInvoiceId", nativeQuery = true)
    int archiveByInvPurchaseInvoiceId(@Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId);
}
