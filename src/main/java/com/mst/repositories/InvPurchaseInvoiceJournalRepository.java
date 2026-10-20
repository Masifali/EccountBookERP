package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InvPurchaseInvoiceJournal;

/**
 * Repository for dbo.InvPurchaseInvoiceJournal (source: Sp_InvPurchaseInvoiceJournal_Insert).
 * The proc is a plain single-table INSERT (Id = IDENTITY, @Id parameter ignored) ending with SELECT SCOPE_IDENTITY()
 * -> callers use save() and read the id from the entity. No pre-insert logic.
 */
@Repository
public interface InvPurchaseInvoiceJournalRepository extends JpaRepository<InvPurchaseInvoiceJournal, Integer> {

    /** Sp_InvPurchaseInvoiceJournal_Insert (single branch) — equivalent to save(); kept as EXEC for callers that need the exact proc. All parameters default to NULL. Returns SCOPE_IDENTITY(). */
    @Transactional
    @Query(value = "EXEC dbo.Sp_InvPurchaseInvoiceJournal_Insert @Id = :id, @InvPurchaseInvoiceId = :invPurchaseInvoiceId, "
         + "@ChartofAccountId = :chartofAccountId, @JvRemarks = :jvRemarks, @JvDebit = :jvDebit, @JvCredit = :jvCredit, @JvPrcnt = :jvPrcnt, "
         + "@JvQty = :jvQty, @JvRate = :jvRate, @InvGrnId = :invGrnId, @FreightId = :freightId, @SupplierCustomerId = :supplierCustomerId, "
         + "@TransporterSupCustId = :transporterSupCustId, @RowType = :rowType", nativeQuery = true)
    Integer spInvPurchaseInvoiceJournalInsert(@Param("id") Integer id,
                                              @Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId,
                                              @Param("chartofAccountId") Integer chartofAccountId,
                                              @Param("jvRemarks") String jvRemarks,
                                              @Param("jvDebit") Double jvDebit,
                                              @Param("jvCredit") Double jvCredit,
                                              @Param("jvPrcnt") Double jvPrcnt,
                                              @Param("jvQty") Double jvQty,
                                              @Param("jvRate") Double jvRate,
                                              @Param("invGrnId") Integer invGrnId,
                                              @Param("freightId") Integer freightId,
                                              @Param("supplierCustomerId") Integer supplierCustomerId,
                                              @Param("transporterSupCustId") Integer transporterSupCustId,
                                              @Param("rowType") Integer rowType);

    /** Sp_InvPurchaseInvoice_Update / Sp_InvoicesVouchersandStocksDelete: delete InvPurchaseInvoiceJournal where InvPurchaseInvoiceId = @Id. */
    @Modifying
    @Transactional
    @Query("DELETE FROM InvPurchaseInvoiceJournal j WHERE j.invPurchaseInvoiceId = :invPurchaseInvoiceId")
    int deleteByInvPurchaseInvoiceId(@Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId);

    /** Sp_InvoicesVouchersandStocksDelete (purchase invoice branch): archive copy INSERT INTO H_InvPurchaseInvoiceJournal (verbatim column list). Native: history table has no entity. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO H_InvPurchaseInvoiceJournal SELECT [Id], [InvPurchaseInvoiceId], [ChartofAccountId], [JvRemarks], [JvDebit], [JvCredit], "
         + "[JvPrcnt], [JvQty], [JvRate], [InvGrnId], [FreightId], [SupplierCustomerId] "
         + "FROM [dbo].[InvPurchaseInvoiceJournal] WHERE InvPurchaseInvoiceId = :invPurchaseInvoiceId", nativeQuery = true)
    int archiveByInvPurchaseInvoiceId(@Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId);
}
