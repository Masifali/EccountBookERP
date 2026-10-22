package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InvPurchaseInvoiceExpense;

/**
 * Repository for dbo.InvPurchaseInvoiceExpense (source: Sp_InvPurchaseInvoiceExpense_Insert).
 * The proc is a plain single-table INSERT (Id = IDENTITY, @Id parameter ignored) ending with SELECT SCOPE_IDENTITY()
 * -> callers use save() and read the id from the entity. No pre-insert logic.
 */
@Repository
public interface InvPurchaseInvoiceExpenseRepository extends JpaRepository<InvPurchaseInvoiceExpense, Integer> {

    /** Sp_InvPurchaseInvoiceExpense_Insert (single branch) — equivalent to save(); kept as EXEC for callers that need the exact proc. All parameters default to NULL. Returns SCOPE_IDENTITY(). */
    @Transactional
    @Query(value = "EXEC dbo.Sp_InvPurchaseInvoiceExpense_Insert @Id = :id, @PurchaseOrderId = :purchaseOrderId, "
         + "@PurchaseOrderSupplierExpId = :purchaseOrderSupplierExpId, @InvPurchaseInvoiceId = :invPurchaseInvoiceId, "
         + "@InvRevExpItemId = :invRevExpItemId, @Qty = :qty, @Rate = :rate, @Amount = :amount, @Remarks = :remarks, "
         + "@CustomRemarks = :customRemarks, @GdnId = :gdnId, @GdnExpId = :gdnExpId, @GrnId = :grnId, "
         + "@SupplierDispatchId = :supplierDispatchId, @SupplierDispatchExpenseId = :supplierDispatchExpenseId", nativeQuery = true)
    Integer spInvPurchaseInvoiceExpenseInsert(@Param("id") Integer id,
                                              @Param("purchaseOrderId") Integer purchaseOrderId,
                                              @Param("purchaseOrderSupplierExpId") Integer purchaseOrderSupplierExpId,
                                              @Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId,
                                              @Param("invRevExpItemId") Integer invRevExpItemId,
                                              @Param("qty") Double qty,
                                              @Param("rate") Double rate,
                                              @Param("amount") Double amount,
                                              @Param("remarks") String remarks,
                                              @Param("customRemarks") String customRemarks,
                                              @Param("gdnId") Integer gdnId,
                                              @Param("gdnExpId") Integer gdnExpId,
                                              @Param("grnId") Integer grnId,
                                              @Param("supplierDispatchId") Integer supplierDispatchId,
                                              @Param("supplierDispatchExpenseId") Integer supplierDispatchExpenseId);

    /** Sp_InvPurchaseInvoice_Update / Sp_InvoicesVouchersandStocksDelete: delete InvPurchaseInvoiceExpense where InvPurchaseInvoiceId = @Id. */
    @Modifying
    @Transactional
    @Query("DELETE FROM InvPurchaseInvoiceExpense e WHERE e.invPurchaseInvoiceId = :invPurchaseInvoiceId")
    int deleteByInvPurchaseInvoiceId(@Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId);

    /** Sp_InvoicesVouchersandStocksDelete (purchase invoice branch): archive copy INSERT INTO H_InvPurchaseInvoiceExpense (verbatim column list). Native: history table has no entity. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO H_InvPurchaseInvoiceExpense SELECT [Id], [InvPurchaseInvoiceId], [InvRevExpItemId], [Qty], [Rate], [Amount], [Remarks] "
         + "FROM [dbo].[InvPurchaseInvoiceExpense] WHERE InvPurchaseInvoiceId = :invPurchaseInvoiceId", nativeQuery = true)
    int archiveByInvPurchaseInvoiceId(@Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId);
}
