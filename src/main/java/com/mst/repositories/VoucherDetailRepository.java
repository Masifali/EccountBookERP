package com.mst.repositories;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.VoucherDetail;
import com.mst.projections.BankChequeTemplateRow;
import com.mst.projections.ChartofAccountSubsidiaryRow;
import com.mst.projections.VoucherHeadLookupRow;
import com.mst.projections.VoucherHeadUsersRow;

/**
 * dbo.VoucherDetail - Sp_VoucherDetail_Insert (insert) and the detail-side statements of Sp_VoucherHead_Update.
 * <p>
 * Sp_VoucherDetail_Insert is procedural (validations via usp_VoucherDetail_Validations, subsidiary checks,
 * configuration-driven cheque logic, ChequePrintList upsert), so its full behaviour is kept in
 * {@link #spVoucherDetailInsert}. For the simple case callers may {@code save(entity)}; the proc's pre-insert logic is:
 * <ul>
 * <li>Raises if AccountId or VoucherHeadId is null/0; reads Org/Company/FinancialYear/DocumentType/SrNo/VoucherDate/
 *     RefAccountId/CheqId/ChequeDate from VoucherHead ({@link #findHeaderForDetail}).</li>
 * <li>EXEC usp_VoucherDetail_Validations (...)</li>
 * <li>If ERP feature 4 is on: when SupplierCustomerId is 0 and exactly one SupplierCustomer has GlAccountId = AccountId,
 *     SupplierCustomerId = SubsidiaryAccountId = that Id and SubsidiaryTypeId = 1; raises when the account is a subsidiary
 *     parent with no party/employee; for DocumentTypeId in (1..10,26,34,35) the party must be configured on the account.</li>
 * <li>DocumentTypeId not in (111,112,101): raises if DebitAmount = 0 and CreditAmount = 0.</li>
 * <li>DocumentTypeId in (101,810) and OrderNo 0: OrderNo = InvContractorWagesBillHeader.JobOrderId ({@link #findJobOrderIdForWagesBill}).</li>
 * <li>Config 134 (PdcDays) / 135 (cheque compulsory on BPV): DocumentTypeId = 2 and InstrumentTypeId = 1 requires InvoiceNoRefId;
 *     DocumentTypeId in (2,35) with InvoiceNoRefId > 0 and PdcDays > 0: ChequeStatus = 1 when VoucherDate + PdcDays <= DCheqDate.</li>
 * <li>EntryDate = GETDATE(); if CheqNoDetail is empty: DCheqDate = NULL and PayeeTitle = ''.</li>
 * <li>DebitAmount / CreditAmount are stored as ABS(...).</li>
 * <li>After insert, for DocumentTypeId = 2 with DebitAmount > 0 and a cheque number: ChequePrintList upsert
 *     (see ChequePrintListRepository). Returns SELECT @VoucherDetailId (SCOPE_IDENTITY()).</li>
 * </ul>
 */
@Repository
public interface VoucherDetailRepository extends JpaRepository<VoucherDetail, Integer> {

    // ------------------------------------------------------------------ Sp_VoucherDetail_Insert

    /**
     * Sp_VoucherDetail_Insert (single branch) - whole procedure kept as-is.
     * // KEEP-EXEC: procedural (RAISERROR validations, EXEC usp_VoucherDetail_Validations, scalar functions
     * fnCheckERPFeaturesExistorNot / GetConfigurationByConfigId / fnNumberToWords, ChequePrintList upsert).
     * All parameters default to NULL in the proc. The proc ends with SELECT @VoucherDetailId (the new Id); that
     * result set is not returned by this @Modifying call - use save() when the id is needed.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_VoucherDetail_Insert @Id = :id, @VoucherHeadId = :voucherHeadId, @AccountId = :accountId, @AgainstAccountId = :againstAccountId, @Comments = :comments, @CommentsOtherLingo = :commentsOtherLingo, @DebitAmount = :debitAmount, @CreditAmount = :creditAmount, @JobLotId = :jobLotId, @RefInvoiceNo = :refInvoiceNo, @TaxesTotalAmount = :taxesTotalAmount, @TaxesRemarks = :taxesRemarks, @IsTaxable = :isTaxable, @TaxTypeId = :taxTypeId, @TaxPrcnt = :taxPrcnt, @DCheqDate = :dCheqDate, @CheqNoDetail = :cheqNoDetail, @DocumentTypeIdRef = :documentTypeIdRef, @InvoiceNoRefId = :invoiceNoRefId, @ItemId = :itemId, @OrderNo = :orderNo, @GpNo = :gpNo, @VehicleNo = :vehicleNo, @GpDate = :gpDate, @QtyIn = :qtyIn, @QtyOut = :qtyOut, @WeightIn = :weightIn, @WeightOut = :weightOut, @SupplierCustomerId = :supplierCustomerId, @ItemRate = :itemRate, @RateCut = :rateCut, @RateCutAmount = :rateCutAmount, @ItemAmount = :itemAmount, @Expenses = :expenses, @Freight = :freight, @Journal = :journal, @Commission = :commission, @DMultiCurrencyId = :dMultiCurrencyId, @DConversionFormula = :dConversionFormula, @DExchangeCurrencyRate = :dExchangeCurrencyRate, @DCurrencyAmount = :dCurrencyAmount, @PaymentType = :paymentType, @AdvanceAmount = :advanceAmount, @WhtHolding = :whtHolding, @SaleTax = :saleTax, @ExTax = :exTax, @Adjustment = :adjustment, @ActionId = :actionId, @LineId = :lineId, @RefDocumentTypeId = :refDocumentTypeId, @RefDocNoId = :refDocNoId, @RefDocNoDetailId = :refDocNoDetailId, @RefDocSubIdNo = :refDocSubIdNo, @ItemCgsRate = :itemCgsRate, @TotalCreditAmount = :totalCreditAmount, @TotalDebitAmount = :totalDebitAmount, @SubNo = :subNo, @EntryDate = :entryDate, @ChequeStatus = :chequeStatus, @PayeeTitle = :payeeTitle, @SubsidiaryTypeId = :subsidiaryTypeId, @EmployeeId = :employeeId, @SubsidiaryAccountId = :subsidiaryAccountId, @TotalPaidAmount = :totalPaidAmount, @InvoiceAmount = :invoiceAmount, @IsCGS = :isCGS, @SubsidiaryAgainstTypeId = :subsidiaryAgainstTypeId, @SubsidiaryAgainstAccountId = :subsidiaryAgainstAccountId, @ThirdCurrencyId = :thirdCurrencyId, @ThirdCurrencyFcyExchangeRate = :thirdCurrencyFcyExchangeRate, @ThirdCurrencyHcyExchangeRate = :thirdCurrencyHcyExchangeRate, @ThirdCurrencyAmount = :thirdCurrencyAmount, @ThirdCurrencyReceiverExchangeRate = :thirdCurrencyReceiverExchangeRate, @ThirdCurrencyReceiverFcyAmount = :thirdCurrencyReceiverFcyAmount, @SortNo = :sortNo, @PayeeOnly = :payeeOnly, @InstrumentTypeId = :instrumentTypeId, @ChequeTypeId = :chequeTypeId, @BranchesId = :branchesId, @CostCenterId = :costCenterId, @SBRTaxAmount = :sbrTaxAmount, @DiscountPercent = :discountPercent, @DiscountAmount = :discountAmount, @ReferenceAccountId = :referenceAccountId, @LocationTypeId = :locationTypeId, @PaymentTypeId = :paymentTypeId, @TaxAmount = :taxAmount, @BaseFcyId = :baseFcyId, @BaseFcyExchangeRate = :baseFcyExchangeRate, @BaseFcyAmount = :baseFcyAmount, @InventoryId = :inventoryId", nativeQuery = true)
    void spVoucherDetailInsert(
            @Param("id") Integer id,
            @Param("voucherHeadId") Integer voucherHeadId,
            @Param("accountId") Integer accountId,
            @Param("againstAccountId") Integer againstAccountId,
            @Param("comments") String comments,
            @Param("commentsOtherLingo") String commentsOtherLingo,
            @Param("debitAmount") Double debitAmount,
            @Param("creditAmount") Double creditAmount,
            @Param("jobLotId") Integer jobLotId,
            @Param("refInvoiceNo") String refInvoiceNo,
            @Param("taxesTotalAmount") Double taxesTotalAmount,
            @Param("taxesRemarks") String taxesRemarks,
            @Param("isTaxable") String isTaxable,
            @Param("taxTypeId") Integer taxTypeId,
            @Param("taxPrcnt") Double taxPrcnt,
            @Param("dCheqDate") LocalDate dCheqDate,
            @Param("cheqNoDetail") String cheqNoDetail,
            @Param("documentTypeIdRef") Integer documentTypeIdRef,
            @Param("invoiceNoRefId") Integer invoiceNoRefId,
            @Param("itemId") Integer itemId,
            @Param("orderNo") Integer orderNo,
            @Param("gpNo") Integer gpNo,
            @Param("vehicleNo") String vehicleNo,
            @Param("gpDate") LocalDate gpDate,
            @Param("qtyIn") Double qtyIn,
            @Param("qtyOut") Double qtyOut,
            @Param("weightIn") Double weightIn,
            @Param("weightOut") Double weightOut,
            @Param("supplierCustomerId") Integer supplierCustomerId,
            @Param("itemRate") Double itemRate,
            @Param("rateCut") Double rateCut,
            @Param("rateCutAmount") Double rateCutAmount,
            @Param("itemAmount") Double itemAmount,
            @Param("expenses") Double expenses,
            @Param("freight") Double freight,
            @Param("journal") Double journal,
            @Param("commission") Double commission,
            @Param("dMultiCurrencyId") Integer dMultiCurrencyId,
            @Param("dConversionFormula") String dConversionFormula,
            @Param("dExchangeCurrencyRate") Double dExchangeCurrencyRate,
            @Param("dCurrencyAmount") Double dCurrencyAmount,
            @Param("paymentType") String paymentType,
            @Param("advanceAmount") Double advanceAmount,
            @Param("whtHolding") Double whtHolding,
            @Param("saleTax") Double saleTax,
            @Param("exTax") Double exTax,
            @Param("adjustment") Double adjustment,
            @Param("actionId") Integer actionId,
            @Param("lineId") Integer lineId,
            @Param("refDocumentTypeId") Integer refDocumentTypeId,
            @Param("refDocNoId") Integer refDocNoId,
            @Param("refDocNoDetailId") Integer refDocNoDetailId,
            @Param("refDocSubIdNo") Integer refDocSubIdNo,
            @Param("itemCgsRate") Double itemCgsRate,
            @Param("totalCreditAmount") Double totalCreditAmount,
            @Param("totalDebitAmount") Double totalDebitAmount,
            @Param("subNo") Integer subNo,
            @Param("entryDate") LocalDateTime entryDate,
            @Param("chequeStatus") Boolean chequeStatus,
            @Param("payeeTitle") String payeeTitle,
            @Param("subsidiaryTypeId") Integer subsidiaryTypeId,
            @Param("employeeId") Integer employeeId,
            @Param("subsidiaryAccountId") Integer subsidiaryAccountId,
            @Param("totalPaidAmount") Integer totalPaidAmount,
            @Param("invoiceAmount") Integer invoiceAmount,
            @Param("isCGS") Integer isCGS,
            @Param("subsidiaryAgainstTypeId") Integer subsidiaryAgainstTypeId,
            @Param("subsidiaryAgainstAccountId") Integer subsidiaryAgainstAccountId,
            @Param("thirdCurrencyId") Integer thirdCurrencyId,
            @Param("thirdCurrencyFcyExchangeRate") Double thirdCurrencyFcyExchangeRate,
            @Param("thirdCurrencyHcyExchangeRate") Double thirdCurrencyHcyExchangeRate,
            @Param("thirdCurrencyAmount") Double thirdCurrencyAmount,
            @Param("thirdCurrencyReceiverExchangeRate") Double thirdCurrencyReceiverExchangeRate,
            @Param("thirdCurrencyReceiverFcyAmount") Double thirdCurrencyReceiverFcyAmount,
            @Param("sortNo") Integer sortNo,
            @Param("payeeOnly") Boolean payeeOnly,
            @Param("instrumentTypeId") Integer instrumentTypeId,
            @Param("chequeTypeId") Integer chequeTypeId,
            @Param("branchesId") Integer branchesId,
            @Param("costCenterId") Integer costCenterId,
            @Param("sbrTaxAmount") Double sbrTaxAmount,
            @Param("discountPercent") Double discountPercent,
            @Param("discountAmount") Double discountAmount,
            @Param("referenceAccountId") Integer referenceAccountId,
            @Param("locationTypeId") Integer locationTypeId,
            @Param("paymentTypeId") Integer paymentTypeId,
            @Param("taxAmount") Double taxAmount,
            @Param("baseFcyId") Integer baseFcyId,
            @Param("baseFcyExchangeRate") Double baseFcyExchangeRate,
            @Param("baseFcyAmount") Double baseFcyAmount,
            @Param("inventoryId") Integer inventoryId);

    /** Sp_VoucherDetail_Insert: header lookup "SELECT OrganizationId, CompanyId, ... FROM VoucherHead WHERE Id = @VoucherHeadId" (take first). */
    @Query("SELECT h.organizationId AS organizationId, h.companyId AS companyId, h.financialYearId AS financialYearId, "
         + "h.documentTypeId AS documentTypeId, h.documentTypeSrNo AS documentTypeSrNo, h.voucherDate AS voucherDate, "
         + "COALESCE(h.refAccountId, 0) AS bankAccountId, h.cheqId AS chequeId, h.chequeDate AS chequeDate "
         + "FROM VoucherHead h WHERE h.id = :voucherHeadId")
    List<VoucherHeadLookupRow> findHeaderForDetail(@Param("voucherHeadId") Integer voucherHeadId);

    /** Sp_VoucherDetail_Insert: "Select EntryUser, ModifyUser From VoucherHead Where Id = @VoucherHeadId" (take first). */
    @Query("SELECT h.entryUser AS entryUser, h.modifyUser AS modifyUser FROM VoucherHead h WHERE h.id = :voucherHeadId")
    List<VoucherHeadUsersRow> findHeaderUsers(@Param("voucherHeadId") Integer voucherHeadId);

    /** Sp_VoucherDetail_Insert: "SELECT count(Id) FROM SupplierCustomer WHERE GlAccountId = @AccountId". */
    @Query("SELECT COUNT(s.id) FROM SupplierCustomer s WHERE s.glAccountId = :accountId")
    Long countPartiesByGlAccount(@Param("accountId") Integer accountId);

    /** Sp_VoucherDetail_Insert: "SELECT Id FROM SupplierCustomer WHERE GlAccountId = @AccountId" (take first). */
    @Query("SELECT s.id FROM SupplierCustomer s WHERE s.glAccountId = :accountId")
    List<Integer> findPartyIdsByGlAccount(@Param("accountId") Integer accountId);

    /** Sp_VoucherDetail_Insert: EXISTS(SELECT 1 FROM SupplierCustomer WHERE CompanyId=@CompanyId AND GlAccountId=@AccountId AND Id=@SupplierCustomerId). */
    @Query("SELECT COUNT(s.id) FROM SupplierCustomer s WHERE s.companyId = :companyId AND s.glAccountId = :accountId AND s.id = :supplierCustomerId")
    Long countPartyConfiguredOnAccount(@Param("companyId") Integer companyId,
                                       @Param("accountId") Integer accountId,
                                       @Param("supplierCustomerId") Integer supplierCustomerId);

    /** Sp_VoucherDetail_Insert: "SELECT CompanyName FROM SupplierCustomer WHERE Id = @SupplierCustomerId AND CompanyId = @CompanyId" (take first). */
    @Query("SELECT s.companyName FROM SupplierCustomer s WHERE s.id = :supplierCustomerId AND s.companyId = :companyId")
    List<String> findPartyName(@Param("supplierCustomerId") Integer supplierCustomerId, @Param("companyId") Integer companyId);

    /** Sp_VoucherDetail_Insert: "SELECT ISNULL(SubSidiaryAcStatus,0), AccountTitle FROM ChartofAccount WHERE Id = @AccountId" (native: SubSidiaryAcStatus is not on the entity; take first). */
    @Query(value = "SELECT ISNULL(SubSidiaryAcStatus,0) AS SubSidiaryAcStatus, AccountTitle FROM ChartofAccount WHERE Id = :accountId", nativeQuery = true)
    List<ChartofAccountSubsidiaryRow> findSubsidiaryStatus(@Param("accountId") Integer accountId);

    /** Sp_VoucherDetail_Insert: "SELECT AccountTitle FROM ChartofAccount WHERE Id = @AgainstAccountId" (take first). */
    @Query("SELECT c.accountTitle FROM ChartofAccount c WHERE c.id = :accountId")
    List<String> findAccountTitle(@Param("accountId") Integer accountId);

    /** Sp_VoucherDetail_Insert: "SELECT h.JobOrderId FROM InvContractorWagesBillHeader h WHERE h.CompanyId=@CompanyId AND h.DocumentTypeId=@DocumentTypeId AND h.Id=@DocumentTypeSrNo" (native: no entity; take first). */
    @Query(value = "SELECT h.JobOrderId FROM InvContractorWagesBillHeader h WHERE h.CompanyId = :companyId AND h.DocumentTypeId = :documentTypeId AND h.Id = :documentTypeSrNo", nativeQuery = true)
    List<Integer> findJobOrderIdForWagesBill(@Param("companyId") Integer companyId,
                                             @Param("documentTypeId") Integer documentTypeId,
                                             @Param("documentTypeSrNo") Integer documentTypeSrNo);

    /** Sp_VoucherDetail_Insert: "Select Id, ChequeTemplete from Bank Where ChartOfAccountId = @AgainstAccountId" (native: Bank columns not registered; take first). */
    @Query(value = "Select Id, ChequeTemplete from Bank Where ChartOfAccountId = :againstAccountId", nativeQuery = true)
    List<BankChequeTemplateRow> findBankByChartOfAccount(@Param("againstAccountId") Integer againstAccountId);

    /** Sp_VoucherDetail_Insert / Sp_VoucherHead_Insert: scalar dbo.GetConfigurationByConfigId(@OrganizationId,@CompanyId,@ConfigId) (134 PdcDays, 135 CheqCompOnBpv, 194 BranchWiseSerial, 265 ChequePrintingEnable). Returned as text. */
    @Query(value = "SELECT CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, :configId) AS NVARCHAR(100))", nativeQuery = true)
    String getConfigurationByConfigId(@Param("organizationId") Integer organizationId,
                                      @Param("companyId") Integer companyId,
                                      @Param("configId") Integer configId);

    /** Sp_VoucherDetail_Insert / Sp_VoucherHead_Update: scalar dbo.fnCheckERPFeaturesExistorNot(@OrganizationId,@CompanyId,@FeatureId) (4 = subsidiary, 5 = FIFO). */
    @Query(value = "SELECT CAST(dbo.fnCheckERPFeaturesExistorNot(:organizationId, :companyId, :featureId) AS BIT)", nativeQuery = true)
    Boolean checkErpFeature(@Param("organizationId") Integer organizationId,
                            @Param("companyId") Integer companyId,
                            @Param("featureId") Integer featureId);

    // ------------------------------------------------------------------ Sp_VoucherHead_Update (detail side)

    /** Sp_VoucherHead_Update: "Delete VoucherDetail where VoucherHeadId = @Id" (ELSE branch: not (DocumentTypeId = 211 AND FIFO feature on)). */
    @Modifying
    @Transactional
    @Query("DELETE FROM VoucherDetail d WHERE d.voucherHeadId = :id")
    int deleteByVoucherHeadId(@Param("id") Integer id);

    /** Sp_VoucherHead_Update: DocumentTypeId = 211 AND FIFO feature on AND ActionId = 2 (CGS transactions): "Delete VoucherDetail where VoucherHeadId = @Id AND RefInvoiceNo = 'CGS'". */
    @Modifying
    @Transactional
    @Query("DELETE FROM VoucherDetail d WHERE d.voucherHeadId = :id AND d.refInvoiceNo = 'CGS'")
    int deleteCgsByVoucherHeadId(@Param("id") Integer id);

    /** Sp_VoucherHead_Update: DocumentTypeId = 211 AND FIFO feature on AND ActionId <> 2 (sale transactions): "Delete VoucherDetail where VoucherHeadId = @Id AND RefInvoiceNo <> 'CGS'". */
    @Modifying
    @Transactional
    @Query("DELETE FROM VoucherDetail d WHERE d.voucherHeadId = :id AND d.refInvoiceNo <> 'CGS'")
    int deleteNonCgsByVoucherHeadId(@Param("id") Integer id);
}
