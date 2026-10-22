package com.mst.repositories;

import java.math.BigDecimal;
import java.time.LocalDate;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InvPurchaseInvoicePaymentTermsDetail;

/**
 * Repository for dbo.InvPurchaseInvoicePaymentTermsDetail (source: USP_InvPurchaseInvoicePaymentTermsDetail_Insert).
 * Pre-insert logic: Id = ISNULL(MAX(CONVERT(INT, Id)),0)+1 (app-assigned -> maxId()); proc ends with SELECT @Id.
 */
@Repository
public interface InvPurchaseInvoicePaymentTermsDetailRepository extends JpaRepository<InvPurchaseInvoicePaymentTermsDetail, Integer> {

    /**
     * USP_InvPurchaseInvoicePaymentTermsDetail_Insert (single branch) — equivalent to maxId()+1 and save(); kept as EXEC for callers that need the exact proc.
     * Defaults: all NULL except @PrcntOfTotal / @Amount which are mandatory. Returns the new Id (SELECT @Id).
     */
    @Transactional
    @Query(value = "EXEC dbo.USP_InvPurchaseInvoicePaymentTermsDetail_Insert @Id = :id, @InvPurchaseInvoiceId = :invPurchaseInvoiceId, "
         + "@PaymentTermId = :paymentTermId, @PrcntOfTotal = :prcntOfTotal, @Amount = :amount, @DueDays = :dueDays, "
         + "@PaymentRemarks = :paymentRemarks, @SortNo = :sortNo, @DueDate = :dueDate, @PurchaseOrderId = :purchaseOrderId, "
         + "@SystemGeneratedRow = :systemGeneratedRow", nativeQuery = true)
    Integer uspInvPurchaseInvoicePaymentTermsDetailInsert(@Param("id") Integer id,
                                                          @Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId,
                                                          @Param("paymentTermId") Integer paymentTermId,
                                                          @Param("prcntOfTotal") BigDecimal prcntOfTotal,
                                                          @Param("amount") BigDecimal amount,
                                                          @Param("dueDays") Integer dueDays,
                                                          @Param("paymentRemarks") String paymentRemarks,
                                                          @Param("sortNo") Integer sortNo,
                                                          @Param("dueDate") LocalDate dueDate,
                                                          @Param("purchaseOrderId") Integer purchaseOrderId,
                                                          @Param("systemGeneratedRow") Boolean systemGeneratedRow);

    /** USP_InvPurchaseInvoicePaymentTermsDetail_Insert: ISNULL(MAX(CONVERT(INT, Id)),0) — caller adds 1 and sets it on the entity before save(). */
    @Query("SELECT COALESCE(MAX(p.id), 0) FROM InvPurchaseInvoicePaymentTermsDetail p")
    Integer maxId();

    /** Sp_InvPurchaseInvoice_Update / Sp_InvoicesVouchersandStocksDelete: delete InvPurchaseInvoicePaymentTermsDetail where InvPurchaseInvoiceId = @Id. */
    @Modifying
    @Transactional
    @Query("DELETE FROM InvPurchaseInvoicePaymentTermsDetail p WHERE p.invPurchaseInvoiceId = :invPurchaseInvoiceId")
    int deleteByInvPurchaseInvoiceId(@Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId);
}
