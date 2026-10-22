package com.mst.repositories;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ChequePrintList;
import com.mst.projections.ChequePrintListLookupRow;

/**
 * dbo.ChequePrintList - written by the cheque-printing block of Sp_VoucherDetail_Insert
 * (only when DocumentTypeId = 2 AND DebitAmount > 0 AND CheqNoDetail <> '').
 * Insert path: callers use {@code save(entity)}. Pre-insert logic in the proc:
 * BankId = Bank.Id where Bank.ChartOfAccountId = @AgainstAccountId (error if none / no ChequeTemplete,
 * when config 265 "cheque printing enable" is on); EntryUser = VoucherHead.EntryUser; EntryDate = GETDATE();
 * ChequeStatus = 0; PrintStatus = 0; D1/D2 = digits of DAY(DCheqDate), M1/M2 = digits of MONTH, Y1..Y4 = digits of YEAR;
 * Amount = @DebitAmount; AmountInWords = dbo.fnNumberToWords(@DebitAmount) (see {@link #numberToWords(Double)}).
 */
@Repository
public interface ChequePrintListRepository extends JpaRepository<ChequePrintList, Integer> {

    /** Sp_VoucherDetail_Insert: SELECT Id, PrintStatus FROM ChequePrintList WHERE VoucherId=@VoucherHeadId AND AccountId=@AccountId AND ChequeId=@InvoiceNoRefId AND OrganizationId AND CompanyId (take first). */
    @Query("SELECT c.id AS id, c.printStatus AS printStatus FROM ChequePrintList c "
         + "WHERE c.voucherId = :voucherHeadId AND c.accountId = :accountId AND c.chequeId = :invoiceNoRefId "
         + "AND c.organizationId = :organizationId AND c.companyId = :companyId")
    List<ChequePrintListLookupRow> findPreCheque(@Param("voucherHeadId") Integer voucherHeadId,
                                                 @Param("accountId") Integer accountId,
                                                 @Param("invoiceNoRefId") Integer invoiceNoRefId,
                                                 @Param("organizationId") Integer organizationId,
                                                 @Param("companyId") Integer companyId);

    /**
     * Sp_VoucherDetail_Insert: UPDATE ChequePrintList SET ... WHERE Id = @PreChequeId
     * (branch: existing row found AND PrintStatus = 0). Native because of getdate() and dbo.fnNumberToWords().
     * d1/d2/m1/m2/y1..y4 are the single digits of DCheqDate (day, month, year) computed by the proc.
     */
    @Modifying
    @Transactional
    @Query(value = "UPDATE [dbo].[ChequePrintList] SET [VoucherId] = :voucherHeadId, [VoucherDetailId] = :voucherDetailId, "
         + "[AccountId] = :accountId, [BankId] = :bankId, [ChequeId] = :invoiceNoRefId, [ChequeNumber] = :cheqNoDetail, "
         + "[ChequeStatus] = :chequeStatus, [ChequeType] = :chequeTypeId, [ChequeDate] = :dCheqDate, [PayeeTitle] = :payeeTitle, "
         + "[Amount] = :debitAmount, [PrintStatus] = 0, [ModifyUser] = :modifyUser, [ModifyDate] = getdate(), "
         + "[OrganizationId] = :organizationId, [CompanyId] = :companyId, [D1] = :d1, [D2] = :d2, [M1] = :m1, [M2] = :m2, "
         + "[Y1] = :y1, [Y2] = :y2, [Y3] = :y3, [Y4] = :y4, AmountInWords = dbo.fnNumberToWords(:debitAmount) "
         + "WHERE Id = :preChequeId", nativeQuery = true)
    int updatePreCheque(@Param("preChequeId") Integer preChequeId,
                        @Param("voucherHeadId") Integer voucherHeadId,
                        @Param("voucherDetailId") Integer voucherDetailId,
                        @Param("accountId") Integer accountId,
                        @Param("bankId") Integer bankId,
                        @Param("invoiceNoRefId") Integer invoiceNoRefId,
                        @Param("cheqNoDetail") String cheqNoDetail,
                        @Param("chequeStatus") Boolean chequeStatus,
                        @Param("chequeTypeId") Integer chequeTypeId,
                        @Param("dCheqDate") LocalDate dCheqDate,
                        @Param("payeeTitle") String payeeTitle,
                        @Param("debitAmount") Double debitAmount,
                        @Param("modifyUser") Integer modifyUser,
                        @Param("organizationId") Integer organizationId,
                        @Param("companyId") Integer companyId,
                        @Param("d1") String d1, @Param("d2") String d2,
                        @Param("m1") String m1, @Param("m2") String m2,
                        @Param("y1") String y1, @Param("y2") String y2,
                        @Param("y3") String y3, @Param("y4") String y4);

    /** Helper for the insert path: dbo.fnNumberToWords(@DebitAmount) used for AmountInWords. */
    @Query(value = "SELECT dbo.fnNumberToWords(:amount)", nativeQuery = true)
    String numberToWords(@Param("amount") Double amount);
}
