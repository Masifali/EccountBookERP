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

import com.mst.models.VoucherHead;

/**
 * dbo.VoucherHead - Sp_VoucherHead_Insert and Sp_VoucherHead_Update.
 * <p>
 * Both procedures are procedural (financial-year / account-type / date-window validations, document-number
 * generation, nested EXECs into DAW.USp_DocumentApprovalDetail_Insert and USP_UserAudit_Insert, cascades into
 * bank.pdcTransaction, DMSAttachments, VoucherDetail*, PaymentDueSchedule, InventoryStockEvalautionDetail,
 * BankReconciliation and VoucherInvoicesAdjustment), so their full behaviour is kept in
 * {@link #spVoucherHeadInsert} and {@link #spVoucherHeadUpdate}. For the simple case callers may {@code save(entity)}.
 * <p>
 * Pre-insert logic of Sp_VoucherHead_Insert (in order):
 * <ul>
 * <li>Raises if FinancialYearId is null/0; FunCheckValidFinancialYear must pass unless DocumentTypeId in (39,40).</li>
 * <li>BranchId null/0 -> UserAccount.BranchesId of EntryUser ({@link #findUserBranchesId}); ProjectId null/0 ->
 *     Projects.Id for Org/Company/Branch ({@link #findProjectIdForBranch}).</li>
 * <li>DocumentTypeId 1/3: RefAccountId must have AccountTypeId = 2; 2/4: AccountTypeId = 15 ({@link #countRefAccountOfOtherType});
 *     142: InvProductionJobOrder @DocumentTypeSrNo must not be approved ({@link #countApprovedJobOrder}).</li>
 * <li>VoucherCode: DocumentTypeId in (1,2,3,4,5,6,7,8,10,17,26,142) -> dbo.GenerateDocumentNumberDynamicly(...)
 *     ({@link #generateDocumentNumber}); else dbo.getDocNoByReferenceIdsForVoucher(...) ({@link #getDocNoByReferenceIds}),
 *     falling back to the passed VoucherCode when it returns 0.</li>
 * <li>DocumentTypeId in (204,211,212): MultiCurrencyId = ExImInvoice.FcurrencyId ({@link #findExImInvoiceCurrency}).</li>
 * <li>Back/next date window from ConfigrationsAllocation 4051 (default 60) / 5057 (default 0) ({@link #findConfigKey}); raises when outside.</li>
 * <li>DocumentTypeId 27: RefDocNoId must not already exist for Org/Company/DocumentType ({@link #findExistingRefDocNoId}).</li>
 * <li>CheqId > 0 and ChequeNo not null: ChequeNo must not already exist for RefAccountId in DocumentTypeId (2,10,26) ({@link #countChequeNoForAccount}).</li>
 * <li>Empty ChequeNo -> ChequeDate = NULL, PayTitle = NULL. UserLogId null/0 -> UserAccount.LastUserLogId of EntryUser ({@link #findUserLastUserLogId}).</li>
 * <li>ModifyDate/ModifyUser/PostDate/PostUser/PostState are NOT inserted. Returns SELECT SCOPE_IDENTITY().</li>
 * <li>Post-insert: DocumentTypeId <> 1000 -> EXEC DAW.USp_DocumentApprovalDetail_Insert; DocumentTypeId = 2 and
 *     RefDocumentTypeId = 250 -> {@link #updatePdcTransaction} + {@link #copyDmsAttachmentsForPdc};
 *     DocumentTypeId in (1..8,10,26) -> EXEC USP_UserAudit_Insert (ActionId 1).</li>
 * </ul>
 */
@Repository
public interface VoucherHeadRepository extends JpaRepository<VoucherHead, Integer> {

    // ------------------------------------------------------------------ Sp_VoucherHead_Insert

    /**
     * Sp_VoucherHead_Insert (single branch) - whole procedure kept as-is.
     * // KEEP-EXEC: procedural (RAISERROR validations, scalar functions FunCheckValidFinancialYear /
     * GenerateDocumentNumberDynamicly / getDocNoByReferenceIdsForVoucher, nested EXECs, cascades into
     * bank.pdcTransaction and DMSAttachments). All parameters default to NULL in the proc; @Id is accepted but unused.
     * The proc's SELECT SCOPE_IDENTITY() result set is not returned by this @Modifying call - use save() when the id is needed.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_VoucherHead_Insert @Id = :id, @DocumentTypeId = :documentTypeId, @DocumentTypeSrNo = :documentTypeSrNo, @RefDocNoId = :refDocNoId, @VoucherCode = :voucherCode, @VoucherDate = :voucherDate, @Remarks = :remarks, @RemarksOtherLingo = :remarksOtherLingo, @VoucherAmount = :voucherAmount, @FinancialYearId = :financialYearId, @RefAccountId = :refAccountId, @AgainstAccountId = :againstAccountId, @MultiCurrencyId = :multiCurrencyId, @ConversionFormula = :conversionFormula, @ExchangeCurrencyRate = :exchangeCurrencyRate, @FcAmount = :fcAmount, @CheqId = :cheqId, @ChequeNo = :chequeNo, @ChequeDate = :chequeDate, @PayTitle = :payTitle, @BankBranch = :bankBranch, @ChequePrintId = :chequePrintId, @Source = :source, @DrCrNoteType = :drCrNoteType, @IsApproved = :isApproved, @EntryDate = :entryDate, @EntryUser = :entryUser, @ModifyDate = :modifyDate, @ModifyUser = :modifyUser, @PostDate = :postDate, @PostUser = :postUser, @PostState = :postState, @OrganizationId = :organizationId, @CompanyId = :companyId, @IncludeWHT = :includeWHT, @BranchId = :branchId, @ProjectId = :projectId, @ManualBillNo = :manualBillNo, @BillAmount = :billAmount, @DueDate = :dueDate, @DueDays = :dueDays, @ActionId = :actionId, @CostCenterAmount = :costCenterAmount, @AttachmentsValues = :attachmentsValues, @RefDocumentTypeId = :refDocumentTypeId, @CustomAttachmentsValues = :customAttachmentsValues, @CustomAccounts = :customAccounts, @BaseDocumentTypeId = :baseDocumentTypeId, @AdvanceTaxAccountId = :advanceTaxAccountId, @AdvanceTaxAmount = :advanceTaxAmount, @OtherChargesAccountId = :otherChargesAccountId, @OtherChargesAmount = :otherChargesAmount, @IsUploaded = :isUploaded, @UploadedDate = :uploadedDate, @UploadedById = :uploadedById, @InclusiveTax = :inclusiveTax, @FixedAssetEntryTypeId = :fixedAssetEntryTypeId, @UserLogId = :userLogId", nativeQuery = true)
    void spVoucherHeadInsert(
            @Param("id") Integer id,
            @Param("documentTypeId") Integer documentTypeId,
            @Param("documentTypeSrNo") Integer documentTypeSrNo,
            @Param("refDocNoId") Integer refDocNoId,
            @Param("voucherCode") Integer voucherCode,
            @Param("voucherDate") LocalDate voucherDate,
            @Param("remarks") String remarks,
            @Param("remarksOtherLingo") String remarksOtherLingo,
            @Param("voucherAmount") Double voucherAmount,
            @Param("financialYearId") Integer financialYearId,
            @Param("refAccountId") Integer refAccountId,
            @Param("againstAccountId") Integer againstAccountId,
            @Param("multiCurrencyId") Integer multiCurrencyId,
            @Param("conversionFormula") String conversionFormula,
            @Param("exchangeCurrencyRate") Double exchangeCurrencyRate,
            @Param("fcAmount") Double fcAmount,
            @Param("cheqId") Integer cheqId,
            @Param("chequeNo") String chequeNo,
            @Param("chequeDate") LocalDateTime chequeDate,
            @Param("payTitle") String payTitle,
            @Param("bankBranch") String bankBranch,
            @Param("chequePrintId") Integer chequePrintId,
            @Param("source") String source,
            @Param("drCrNoteType") String drCrNoteType,
            @Param("isApproved") Boolean isApproved,
            @Param("entryDate") LocalDateTime entryDate,
            @Param("entryUser") Integer entryUser,
            @Param("modifyDate") LocalDateTime modifyDate,
            @Param("modifyUser") Integer modifyUser,
            @Param("postDate") LocalDateTime postDate,
            @Param("postUser") Integer postUser,
            @Param("postState") Boolean postState,
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("includeWHT") Boolean includeWHT,
            @Param("branchId") Integer branchId,
            @Param("projectId") Integer projectId,
            @Param("manualBillNo") String manualBillNo,
            @Param("billAmount") Double billAmount,
            @Param("dueDate") LocalDate dueDate,
            @Param("dueDays") Integer dueDays,
            @Param("actionId") Integer actionId,
            @Param("costCenterAmount") Double costCenterAmount,
            @Param("attachmentsValues") String attachmentsValues,
            @Param("refDocumentTypeId") Integer refDocumentTypeId,
            @Param("customAttachmentsValues") String customAttachmentsValues,
            @Param("customAccounts") Boolean customAccounts,
            @Param("baseDocumentTypeId") Integer baseDocumentTypeId,
            @Param("advanceTaxAccountId") Integer advanceTaxAccountId,
            @Param("advanceTaxAmount") Double advanceTaxAmount,
            @Param("otherChargesAccountId") Integer otherChargesAccountId,
            @Param("otherChargesAmount") Double otherChargesAmount,
            @Param("isUploaded") Boolean isUploaded,
            @Param("uploadedDate") LocalDateTime uploadedDate,
            @Param("uploadedById") Integer uploadedById,
            @Param("inclusiveTax") Boolean inclusiveTax,
            @Param("fixedAssetEntryTypeId") Integer fixedAssetEntryTypeId,
            @Param("userLogId") Long userLogId);

    // ------------------------------------------------------------------ Sp_VoucherHead_Update

    /**
     * Sp_VoucherHead_Update (single branch) - whole procedure kept as-is.
     * // KEEP-EXEC: procedural (ExportVoucher.AutoUpdate override, approval / financial-year / account-type / voucher-code /
     * date-window / duplicate-cheque validations, FIFO-dependent VoucherDetail deletes, cascades into VoucherDetailMultiCurrency,
     * voucherCostCenterDetail, PaymentDueSchedule, InventoryStockEvalautionDetail, BankReconciliation, VoucherInvoicesAdjustment,
     * bank.pdcTransaction, nested EXEC USP_UserAudit_Insert). All parameters default to NULL in the proc.
     * Pre-update logic: IsApproved is re-read from the row (raises when approved unless AutoUpdate = 1); BranchId/ProjectId fallbacks
     * use ModifyUser; DocumentTypeId <> 142: VoucherCode must equal the stored one; DocumentTypeId = 142: VoucherCode is re-read from the row;
     * DocumentTypeId 27 (AutoUpdate <> 1): RefDocNoId must exist and no ExImFCBankReceipts (203) for it; empty ChequeNo -> ChequeDate/PayTitle NULL;
     * DocumentTypeId in (204,211,212) with MultiCurrencyId <> 0: MultiCurrencyId = ExImInvoice.FcurrencyId; UserLogId fallback from ModifyUser.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_VoucherHead_Update @Id = :id, @DocumentTypeId = :documentTypeId, @DocumentTypeSrNo = :documentTypeSrNo, @RefDocNoId = :refDocNoId, @VoucherCode = :voucherCode, @VoucherDate = :voucherDate, @Remarks = :remarks, @RemarksOtherLingo = :remarksOtherLingo, @VoucherAmount = :voucherAmount, @FinancialYearId = :financialYearId, @RefAccountId = :refAccountId, @AgainstAccountId = :againstAccountId, @MultiCurrencyId = :multiCurrencyId, @ConversionFormula = :conversionFormula, @ExchangeCurrencyRate = :exchangeCurrencyRate, @FcAmount = :fcAmount, @CheqId = :cheqId, @ChequeNo = :chequeNo, @ChequeDate = :chequeDate, @PayTitle = :payTitle, @BankBranch = :bankBranch, @ChequePrintId = :chequePrintId, @Source = :source, @DrCrNoteType = :drCrNoteType, @IsApproved = :isApproved, @EntryDate = :entryDate, @EntryUser = :entryUser, @ModifyDate = :modifyDate, @ModifyUser = :modifyUser, @PostDate = :postDate, @PostUser = :postUser, @PostState = :postState, @OrganizationId = :organizationId, @CompanyId = :companyId, @IncludeWHT = :includeWHT, @BranchId = :branchId, @ProjectId = :projectId, @ManualBillNo = :manualBillNo, @BillAmount = :billAmount, @DueDate = :dueDate, @DueDays = :dueDays, @ActionId = :actionId, @CostCenterAmount = :costCenterAmount, @AttachmentsValues = :attachmentsValues, @RefDocumentTypeId = :refDocumentTypeId, @CustomAttachmentsValues = :customAttachmentsValues, @CustomAccounts = :customAccounts, @BaseDocumentTypeId = :baseDocumentTypeId, @AdvanceTaxAccountId = :advanceTaxAccountId, @AdvanceTaxAmount = :advanceTaxAmount, @OtherChargesAccountId = :otherChargesAccountId, @OtherChargesAmount = :otherChargesAmount, @IsUploaded = :isUploaded, @UploadedDate = :uploadedDate, @UploadedById = :uploadedById, @InclusiveTax = :inclusiveTax, @FixedAssetEntryTypeId = :fixedAssetEntryTypeId, @UserLogId = :userLogId", nativeQuery = true)
    void spVoucherHeadUpdate(
            @Param("id") Integer id,
            @Param("documentTypeId") Integer documentTypeId,
            @Param("documentTypeSrNo") Integer documentTypeSrNo,
            @Param("refDocNoId") Integer refDocNoId,
            @Param("voucherCode") Integer voucherCode,
            @Param("voucherDate") LocalDate voucherDate,
            @Param("remarks") String remarks,
            @Param("remarksOtherLingo") String remarksOtherLingo,
            @Param("voucherAmount") Double voucherAmount,
            @Param("financialYearId") Integer financialYearId,
            @Param("refAccountId") Integer refAccountId,
            @Param("againstAccountId") Integer againstAccountId,
            @Param("multiCurrencyId") Integer multiCurrencyId,
            @Param("conversionFormula") String conversionFormula,
            @Param("exchangeCurrencyRate") Double exchangeCurrencyRate,
            @Param("fcAmount") Double fcAmount,
            @Param("cheqId") Integer cheqId,
            @Param("chequeNo") String chequeNo,
            @Param("chequeDate") LocalDateTime chequeDate,
            @Param("payTitle") String payTitle,
            @Param("bankBranch") String bankBranch,
            @Param("chequePrintId") Integer chequePrintId,
            @Param("source") String source,
            @Param("drCrNoteType") String drCrNoteType,
            @Param("isApproved") Boolean isApproved,
            @Param("entryDate") LocalDateTime entryDate,
            @Param("entryUser") Integer entryUser,
            @Param("modifyDate") LocalDateTime modifyDate,
            @Param("modifyUser") Integer modifyUser,
            @Param("postDate") LocalDateTime postDate,
            @Param("postUser") Integer postUser,
            @Param("postState") Boolean postState,
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("includeWHT") Boolean includeWHT,
            @Param("branchId") Integer branchId,
            @Param("projectId") Integer projectId,
            @Param("manualBillNo") String manualBillNo,
            @Param("billAmount") Double billAmount,
            @Param("dueDate") LocalDate dueDate,
            @Param("dueDays") Integer dueDays,
            @Param("actionId") Integer actionId,
            @Param("costCenterAmount") Double costCenterAmount,
            @Param("attachmentsValues") String attachmentsValues,
            @Param("refDocumentTypeId") Integer refDocumentTypeId,
            @Param("customAttachmentsValues") String customAttachmentsValues,
            @Param("customAccounts") Boolean customAccounts,
            @Param("baseDocumentTypeId") Integer baseDocumentTypeId,
            @Param("advanceTaxAccountId") Integer advanceTaxAccountId,
            @Param("advanceTaxAmount") Double advanceTaxAmount,
            @Param("otherChargesAccountId") Integer otherChargesAccountId,
            @Param("otherChargesAmount") Double otherChargesAmount,
            @Param("isUploaded") Boolean isUploaded,
            @Param("uploadedDate") LocalDateTime uploadedDate,
            @Param("uploadedById") Integer uploadedById,
            @Param("inclusiveTax") Boolean inclusiveTax,
            @Param("fixedAssetEntryTypeId") Integer fixedAssetEntryTypeId,
            @Param("userLogId") Long userLogId);

    /**
     * Sp_VoucherHead_Update: the main "UPDATE VoucherHead SET ... WHERE Id = @Id" statement, every SET column kept.
     * Native because DocumentTypeId is mapped through a @ManyToOne on the entity (not settable in a JPQL UPDATE).
     * IsApproved / EntryDate / EntryUser / PostDate / PostUser / PostState are NOT updated by the proc.
     */
    @Modifying
    @Transactional
    @Query(value = "UPDATE [dbo].[VoucherHead] SET [DocumentTypeId] = :documentTypeId, [DocumentTypeSrNo] = :documentTypeSrNo, "
         + "[RefDocNoId] = :refDocNoId, [VoucherCode] = :voucherCode, [VoucherDate] = :voucherDate, [Remarks] = :remarks, "
         + "[RemarksOtherLingo] = :remarksOtherLingo, [VoucherAmount] = :voucherAmount, [FinancialYearId] = :financialYearId, "
         + "[RefAccountId] = :refAccountId, [AgainstAccountId] = :againstAccountId, [MultiCurrencyId] = :multiCurrencyId, "
         + "[ConversionFormula] = :conversionFormula, [ExchangeCurrencyRate] = :exchangeCurrencyRate, [FcAmount] = :fcAmount, "
         + "[CheqId] = :cheqId, [ChequeNo] = :chequeNo, [ChequeDate] = :chequeDate, [PayTitle] = :payTitle, [BankBranch] = :bankBranch, "
         + "[ChequePrintId] = :chequePrintId, [Source] = :source, [DrCrNoteType] = :drCrNoteType, [ModifyDate] = :modifyDate, "
         + "[ModifyUser] = :modifyUser, [OrganizationId] = :organizationId, [CompanyId] = :companyId, [IncludeWHT] = :includeWHT, "
         + "[BranchId] = :branchId, [ProjectId] = :projectId, [ManualBillNo] = :manualBillNo, [BillAmount] = :billAmount, "
         + "[DueDate] = :dueDate, [DueDays] = :dueDays, ActionId = :actionId, CostCenterAmount = :costCenterAmount, "
         + "AttachmentsValues = :attachmentsValues, RefDocumentTypeId = :refDocumentTypeId, CustomAttachmentsValues = :customAttachmentsValues, "
         + "CustomAccounts = :customAccounts, BaseDocumentTypeId = :baseDocumentTypeId, AdvanceTaxAccountId = :advanceTaxAccountId, "
         + "AdvanceTaxAmount = :advanceTaxAmount, OtherChargesAccountId = :otherChargesAccountId, OtherChargesAmount = :otherChargesAmount, "
         + "IsUploaded = :isUploaded, UploadedDate = :uploadedDate, UploadedById = :uploadedById, InclusiveTax = :inclusiveTax, "
         + "FixedAssetEntryTypeId = :fixedAssetEntryTypeId, UserLogId = :userLogId WHERE [Id] = :id", nativeQuery = true)
    int updateById(@Param("id") Integer id,
                   @Param("documentTypeId") Integer documentTypeId,
                   @Param("documentTypeSrNo") Integer documentTypeSrNo,
                   @Param("refDocNoId") Integer refDocNoId,
                   @Param("voucherCode") Integer voucherCode,
                   @Param("voucherDate") LocalDate voucherDate,
                   @Param("remarks") String remarks,
                   @Param("remarksOtherLingo") String remarksOtherLingo,
                   @Param("voucherAmount") Double voucherAmount,
                   @Param("financialYearId") Integer financialYearId,
                   @Param("refAccountId") Integer refAccountId,
                   @Param("againstAccountId") Integer againstAccountId,
                   @Param("multiCurrencyId") Integer multiCurrencyId,
                   @Param("conversionFormula") String conversionFormula,
                   @Param("exchangeCurrencyRate") Double exchangeCurrencyRate,
                   @Param("fcAmount") Double fcAmount,
                   @Param("cheqId") Integer cheqId,
                   @Param("chequeNo") String chequeNo,
                   @Param("chequeDate") LocalDateTime chequeDate,
                   @Param("payTitle") String payTitle,
                   @Param("bankBranch") String bankBranch,
                   @Param("chequePrintId") Integer chequePrintId,
                   @Param("source") String source,
                   @Param("drCrNoteType") String drCrNoteType,
                   @Param("modifyDate") LocalDateTime modifyDate,
                   @Param("modifyUser") Integer modifyUser,
                   @Param("organizationId") Integer organizationId,
                   @Param("companyId") Integer companyId,
                   @Param("includeWHT") Boolean includeWHT,
                   @Param("branchId") Integer branchId,
                   @Param("projectId") Integer projectId,
                   @Param("manualBillNo") String manualBillNo,
                   @Param("billAmount") Double billAmount,
                   @Param("dueDate") LocalDate dueDate,
                   @Param("dueDays") Integer dueDays,
                   @Param("actionId") Integer actionId,
                   @Param("costCenterAmount") Double costCenterAmount,
                   @Param("attachmentsValues") String attachmentsValues,
                   @Param("refDocumentTypeId") Integer refDocumentTypeId,
                   @Param("customAttachmentsValues") String customAttachmentsValues,
                   @Param("customAccounts") Boolean customAccounts,
                   @Param("baseDocumentTypeId") Integer baseDocumentTypeId,
                   @Param("advanceTaxAccountId") Integer advanceTaxAccountId,
                   @Param("advanceTaxAmount") Double advanceTaxAmount,
                   @Param("otherChargesAccountId") Integer otherChargesAccountId,
                   @Param("otherChargesAmount") Double otherChargesAmount,
                   @Param("isUploaded") Boolean isUploaded,
                   @Param("uploadedDate") LocalDateTime uploadedDate,
                   @Param("uploadedById") Integer uploadedById,
                   @Param("inclusiveTax") Boolean inclusiveTax,
                   @Param("fixedAssetEntryTypeId") Integer fixedAssetEntryTypeId,
                   @Param("userLogId") Long userLogId);

    // ------------------------------------------------------------------ VoucherHead lookups shared by Insert / Update

    /** Sp_VoucherHead_Insert / _Update (DocumentTypeId = 27): "select RefDocNoId from VoucherHead where OrganizationId, CompanyId, RefDocNoId = @RefDocNoId, DocumentTypeId = @DocumentTypeId" (take first; empty = not found). */
    @Query("SELECT h.refDocNoId FROM VoucherHead h WHERE h.organizationId = :organizationId AND h.companyId = :companyId "
         + "AND h.refDocNoId = :refDocNoId AND h.documentTypeId = :documentTypeId")
    List<Integer> findExistingRefDocNoId(@Param("organizationId") Integer organizationId,
                                         @Param("companyId") Integer companyId,
                                         @Param("refDocNoId") Integer refDocNoId,
                                         @Param("documentTypeId") Integer documentTypeId);

    /** Sp_VoucherHead_Insert (CheqId > 0): EXISTS(SELECT 1 FROM VoucherHead WHERE RefAccountId=@RefAccountId AND Org AND Company AND ChequeNo=@ChequeNo AND DocumentTypeId in (2,10,26)). */
    @Query("SELECT COUNT(h.id) FROM VoucherHead h WHERE h.refAccountId = :refAccountId AND h.organizationId = :organizationId "
         + "AND h.companyId = :companyId AND h.chequeNo = :chequeNo AND h.documentTypeId IN (2, 10, 26)")
    Long countChequeNoForAccount(@Param("refAccountId") Integer refAccountId,
                                 @Param("organizationId") Integer organizationId,
                                 @Param("companyId") Integer companyId,
                                 @Param("chequeNo") String chequeNo);

    /** Sp_VoucherHead_Update (CheqId > 0): "SELECT ChequeNo FROM VoucherHead WHERE Org AND Company AND ChequeNo = @ChequeNo" (take first) and the EXISTS check on the same predicate (count > 0). */
    @Query("SELECT h.chequeNo FROM VoucherHead h WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND h.chequeNo = :chequeNo")
    List<String> findExistingChequeNo(@Param("organizationId") Integer organizationId,
                                      @Param("companyId") Integer companyId,
                                      @Param("chequeNo") String chequeNo);

    /** Sp_VoucherHead_Update: "select IsApproved from VoucherHead where Org AND Company AND Id = @Id" (take first). */
    @Query("SELECT h.isApproved FROM VoucherHead h WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND h.id = :id")
    List<Boolean> findIsApproved(@Param("organizationId") Integer organizationId,
                                 @Param("companyId") Integer companyId,
                                 @Param("id") Integer id);

    /** Sp_VoucherHead_Update (DocumentTypeId <> 142): "select VoucherCode from VoucherHead where Org AND Company AND DocumentTypeId = @DocumentTypeId AND Id = @Id" (take first). */
    @Query("SELECT h.voucherCode FROM VoucherHead h WHERE h.organizationId = :organizationId AND h.companyId = :companyId "
         + "AND h.documentTypeId = :documentTypeId AND h.id = :id")
    List<Integer> findVoucherCode(@Param("organizationId") Integer organizationId,
                                  @Param("companyId") Integer companyId,
                                  @Param("documentTypeId") Integer documentTypeId,
                                  @Param("id") Integer id);

    /** Sp_VoucherHead_Update (DocumentTypeId = 142): "SELECT VoucherCode FROM VoucherHead WHERE Id = @Id" (take first). */
    @Query("SELECT h.voucherCode FROM VoucherHead h WHERE h.id = :id")
    List<Integer> findVoucherCodeById(@Param("id") Integer id);

    /** Sp_VoucherHead_Update: "SELECT VoucherAmount FROM VoucherHead WHERE Id = @Id" (previous amount for the audit text; take first). */
    @Query("SELECT h.voucherAmount FROM VoucherHead h WHERE h.id = :id")
    List<Double> findVoucherAmountById(@Param("id") Integer id);

    // ------------------------------------------------------------------ Other-table lookups used by Insert / Update

    /** Sp_VoucherHead_Insert / _Update: EXISTS(SELECT 1 FROM ChartofAccount WHERE Id = @RefAccountId AND AccountTypeId <> @accountTypeId) (2 for CPV/CRV, 15 for BPV/BRV). */
    @Query("SELECT COUNT(c.id) FROM ChartofAccount c WHERE c.id = :refAccountId AND c.accountTypeId <> :accountTypeId")
    Long countRefAccountOfOtherType(@Param("refAccountId") Integer refAccountId, @Param("accountTypeId") Integer accountTypeId);

    /** Sp_VoucherHead_Insert / _Update: "SELECT DocumentTypeDescription FROM DocumentType WHERE Id = @DocumentTypeId" (for the user-audit text; take first). */
    @Query("SELECT d.documentTypeDescription FROM DocumentType d WHERE d.id = :documentTypeId")
    List<String> findDocumentTypeDescription(@Param("documentTypeId") Integer documentTypeId);

    /** Sp_VoucherHead_Insert (@EntryUser) / _Update (@ModifyUser): "select BranchesId from UserAccount where OrganizationId AND CompanyId AND ID = @userId" (native: BranchesId not registered for UserAccount; take first). */
    @Query(value = "select BranchesId from UserAccount where OrganizationId = :organizationId and CompanyId = :companyId and ID = :userId", nativeQuery = true)
    List<Integer> findUserBranchesId(@Param("organizationId") Integer organizationId,
                                     @Param("companyId") Integer companyId,
                                     @Param("userId") Integer userId);

    /** Sp_VoucherHead_Insert / _Update: "SELECT LastUserLogId FROM UserAccount WHERE ID = @userId" (native; take first). */
    @Query(value = "SELECT LastUserLogId FROM UserAccount WHERE ID = :userId", nativeQuery = true)
    List<Long> findUserLastUserLogId(@Param("userId") Integer userId);

    /** Sp_VoucherHead_Update: EXISTS(SELECT 1 FROM UserAccount WHERE Id = @ModifyUser). */
    @Query("SELECT COUNT(u.id) FROM UserAccount u WHERE u.id = :userId")
    Long countUserAccount(@Param("userId") Integer userId);

    /** Sp_VoucherHead_Insert / _Update: "select Id from Projects where OrganizationId AND CompanyId AND BranchesId = @BranchId" (native: no Projects entity; take first). */
    @Query(value = "select Id from Projects where OrganizationId = :organizationId and CompanyId = :companyId and BranchesId = :branchId", nativeQuery = true)
    List<Integer> findProjectIdForBranch(@Param("organizationId") Integer organizationId,
                                         @Param("companyId") Integer companyId,
                                         @Param("branchId") Integer branchId);

    /** Sp_VoucherHead_Insert / _Update (DocumentTypeId = 142): EXISTS(SELECT 1 FROM InvProductionJobOrder WHERE ID = @DocumentTypeSrNo AND ISNULL(IsApproved,0) = 1) (native: no entity). */
    @Query(value = "SELECT COUNT(1) FROM InvProductionJobOrder WHERE ID = :documentTypeSrNo AND ISNULL(IsApproved,0) = 1", nativeQuery = true)
    Integer countApprovedJobOrder(@Param("documentTypeSrNo") Integer documentTypeSrNo);

    /** Sp_VoucherHead_Insert / _Update (DocumentTypeId in (204,211,212)): "select FcurrencyId from ExImInvoice s where Org AND Company AND DocumentTypeId AND Id = @DocumentTypeSrNo AND FinancialYearId" (native: no entity; take first). */
    @Query(value = "select FcurrencyId from ExImInvoice s where s.OrganizationId = :organizationId and s.CompanyId = :companyId "
         + "and s.DocumentTypeId = :documentTypeId and s.Id = :documentTypeSrNo and s.FinancialYearId = :financialYearId", nativeQuery = true)
    List<Integer> findExImInvoiceCurrency(@Param("organizationId") Integer organizationId,
                                          @Param("companyId") Integer companyId,
                                          @Param("documentTypeId") Integer documentTypeId,
                                          @Param("documentTypeSrNo") Integer documentTypeSrNo,
                                          @Param("financialYearId") Integer financialYearId);

    /** Sp_VoucherHead_Insert / _Update: "SELECT ISNULL(ConfigKey, @default) FROM ConfigrationsAllocation WHERE Org AND Company AND ConfigrationsDefinitionId = @configId" (4051 back-days default 60, 5057 next-days default 0; native; take first). */
    @Query(value = "SELECT ISNULL(ConfigKey, :defaultValue) FROM ConfigrationsAllocation WHERE OrganizationId = :organizationId "
         + "AND CompanyId = :companyId AND ConfigrationsDefinitionId = :configrationsDefinitionId", nativeQuery = true)
    List<String> findConfigKey(@Param("organizationId") Integer organizationId,
                               @Param("companyId") Integer companyId,
                               @Param("configrationsDefinitionId") Integer configrationsDefinitionId,
                               @Param("defaultValue") String defaultValue);

    /** Sp_VoucherHead_Update: "SELECT AutoUpdate FROM ExportVoucher WHERE CompanyId AND DocumentTypeId AND Id = @DocumentTypeSrNo" (native: no entity; take first, null -> 0). */
    @Query(value = "SELECT AutoUpdate FROM ExportVoucher WHERE CompanyId = :companyId AND DocumentTypeId = :documentTypeId AND Id = :documentTypeSrNo", nativeQuery = true)
    List<Integer> findExportVoucherAutoUpdate(@Param("companyId") Integer companyId,
                                              @Param("documentTypeId") Integer documentTypeId,
                                              @Param("documentTypeSrNo") Integer documentTypeSrNo);

    /** Sp_VoucherHead_Update (DocumentTypeId = 27): EXISTS(SELECT 1 FROM ExImFCBankReceipts h WHERE Org AND Company AND ExImInvoiceId = @RefDocNoId AND DocumentTypeId = 203 GROUP BY ...) (native: no entity). */
    @Query(value = "SELECT COUNT(1) FROM ExImFCBankReceipts h where h.OrganizationId = :organizationId and h.CompanyId = :companyId "
         + "and h.ExImInvoiceId = :refDocNoId and h.DocumentTypeId = 203", nativeQuery = true)
    Integer countFcBankReceiptsForInvoice(@Param("organizationId") Integer organizationId,
                                          @Param("companyId") Integer companyId,
                                          @Param("refDocNoId") Integer refDocNoId);

    /** Sp_VoucherHead_Insert / _Update: scalar dbo.FunCheckValidFinancialYear(@OrganizationId,@CompanyId,@FinancialYearId,@VoucherDate). */
    @Query(value = "SELECT CAST(dbo.FunCheckValidFinancialYear(:organizationId, :companyId, :financialYearId, :voucherDate) AS BIT)", nativeQuery = true)
    Boolean checkValidFinancialYear(@Param("organizationId") Integer organizationId,
                                    @Param("companyId") Integer companyId,
                                    @Param("financialYearId") Integer financialYearId,
                                    @Param("voucherDate") LocalDate voucherDate);

    /** Sp_VoucherHead_Insert (DocumentTypeId in (1,2,3,4,5,6,7,8,10,17,26,142)): scalar dbo.GenerateDocumentNumberDynamicly(@OrganizationId,@CompanyId,@FinancialYearId,@DocumentTypeId,@VoucherDate,NULL,NULL). */
    @Query(value = "SELECT dbo.GenerateDocumentNumberDynamicly(:organizationId, :companyId, :financialYearId, :documentTypeId, :voucherDate, NULL, NULL)", nativeQuery = true)
    Integer generateDocumentNumber(@Param("organizationId") Integer organizationId,
                                   @Param("companyId") Integer companyId,
                                   @Param("financialYearId") Integer financialYearId,
                                   @Param("documentTypeId") Integer documentTypeId,
                                   @Param("voucherDate") LocalDate voucherDate);

    /** Sp_VoucherHead_Insert (other DocumentTypeIds): scalar dbo.getDocNoByReferenceIdsForVoucher(@OrganizationId,@CompanyId,@DocumentTypeId,@FinancialYearId,@DocumentTypeSrNo,@BranchId); 0 -> keep the passed VoucherCode. */
    @Query(value = "SELECT dbo.getDocNoByReferenceIdsForVoucher(:organizationId, :companyId, :documentTypeId, :financialYearId, :documentTypeSrNo, :branchId)", nativeQuery = true)
    Integer getDocNoByReferenceIds(@Param("organizationId") Integer organizationId,
                                   @Param("companyId") Integer companyId,
                                   @Param("documentTypeId") Integer documentTypeId,
                                   @Param("financialYearId") Integer financialYearId,
                                   @Param("documentTypeSrNo") Integer documentTypeSrNo,
                                   @Param("branchId") Integer branchId);

    // ------------------------------------------------------------------ Standalone writes into other tables (Insert / Update cascades)

    /** Sp_VoucherHead_Insert (@DocId) / _Update (@Id), DocumentTypeId = 2 AND RefDocumentTypeId = 250: "UPDATE bank.pdcTransaction SET voucherId, voucherCode, voucherDate WHERE pdcTransactionId = @RefDocNoId AND documentTypeId = @RefDocumentTypeId" (native: no entity). */
    @Modifying
    @Transactional
    @Query(value = "UPDATE bank.pdcTransaction SET voucherId = :voucherId, voucherCode = :voucherCode, voucherDate = :voucherDate "
         + "WHERE pdcTransactionId = :refDocNoId and documentTypeId = :refDocumentTypeId", nativeQuery = true)
    int updatePdcTransaction(@Param("voucherId") Integer voucherId,
                             @Param("voucherCode") Integer voucherCode,
                             @Param("voucherDate") LocalDate voucherDate,
                             @Param("refDocNoId") Integer refDocNoId,
                             @Param("refDocumentTypeId") Integer refDocumentTypeId);

    /** Sp_VoucherHead_Insert, DocumentTypeId = 2 AND RefDocumentTypeId = 250: copies the PDC attachments to the new voucher ("INSERT INTO DMSAttachments SELECT ... FROM DMSAttachments WHERE ..."; positional column list kept verbatim; native). */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO DMSAttachments SELECT :refAccountId, [DMSFoldersLabelsId], :documentTypeId, :docId, [Attachment], :entryDate, :entryUser, "
         + "NULL, NULL, [OrganizationId], [CompanyId], [BranchId], 'PaymentVoucherNew', [DetailWiseAttachment], [UploadedFileCustomName], "
         + "[UploadedFileSizeMb], [LineId], [FileCustomName] FROM [dbo].[DMSAttachments] WHERE OrganizationId = :organizationId "
         + "AND CompanyId = :companyId AND RefDocumentTypeId = :refDocumentTypeId AND RefDocumentNo = :refDocNoId", nativeQuery = true)
    int copyDmsAttachmentsForPdc(@Param("refAccountId") Integer refAccountId,
                                 @Param("documentTypeId") Integer documentTypeId,
                                 @Param("docId") Integer docId,
                                 @Param("entryDate") LocalDateTime entryDate,
                                 @Param("entryUser") Integer entryUser,
                                 @Param("organizationId") Integer organizationId,
                                 @Param("companyId") Integer companyId,
                                 @Param("refDocumentTypeId") Integer refDocumentTypeId,
                                 @Param("refDocNoId") Integer refDocNoId);

    /** Sp_VoucherHead_Update (ELSE branch of the FIFO check): "Delete VoucherDetailMultiCurrency where VoucherHeadId = @Id" (native: no entity). */
    @Modifying
    @Transactional
    @Query(value = "Delete VoucherDetailMultiCurrency where VoucherHeadId = :id", nativeQuery = true)
    int deleteDetailMultiCurrencyByVoucherHeadId(@Param("id") Integer id);

    /** Sp_VoucherHead_Update (ELSE branch of the FIFO check): "Delete voucherCostCenterDetail where VoucherHeaderId = @Id" (native: no entity). */
    @Modifying
    @Transactional
    @Query(value = "Delete voucherCostCenterDetail where VoucherHeaderId = :id", nativeQuery = true)
    int deleteCostCenterDetailByVoucherHeadId(@Param("id") Integer id);

    /** Sp_VoucherHead_Update: "delete PaymentDueSchedule where refDocumentTypeId = @DocumentTypeId and RefdocNoId = @Id" (native: entity planned by another batch). */
    @Modifying
    @Transactional
    @Query(value = "delete PaymentDueSchedule where refDocumentTypeId = :documentTypeId and RefdocNoId = :id", nativeQuery = true)
    int deletePaymentDueSchedule(@Param("documentTypeId") Integer documentTypeId, @Param("id") Integer id);

    /** Sp_VoucherHead_Update (DocumentTypeId = 27): "DELETE InventoryStockEvalautionDetail WHERE Org AND Company AND RefDocumentTypeId = @DocumentTypeId AND RefDocIdNo = @Id" (native: entity planned by another batch). */
    @Modifying
    @Transactional
    @Query(value = "DELETE InventoryStockEvalautionDetail WHERE OrganizationId = :organizationId and CompanyId = :companyId "
         + "and RefDocumentTypeId = :documentTypeId AND RefDocIdNo = :id", nativeQuery = true)
    int deleteStockEvaluationDetail(@Param("organizationId") Integer organizationId,
                                    @Param("companyId") Integer companyId,
                                    @Param("documentTypeId") Integer documentTypeId,
                                    @Param("id") Integer id);

    /** Sp_VoucherHead_Update (DocumentTypeId in (1..8,10,26,203)): resets BankReconciliation rows of the voucher (native: BankReconciliation columns not registered). */
    @Modifying
    @Transactional
    @Query(value = "UPDATE BankReconciliation SET VoucherDetailId = 0, Reconciledate = null, reconcileuserid = 0, IsReconciled = 0, ActionId = 1, "
         + "DiffAmount = 0, IsApproved = 0, ApprovedUserId = null, ApprovedDate = null, VoucherHeadId = 0 WHERE VoucherHeadId = :id", nativeQuery = true)
    int resetBankReconciliationByVoucherHeadId(@Param("id") Integer id);

    /** Sp_VoucherHead_Update (DocumentTypeId in (1..8,10,26)): "UPDATE VoucherInvoicesAdjustment SET ActionTypeId = 3 WHERE VoucherHeadId = @Id" (native: no entity). */
    @Modifying
    @Transactional
    @Query(value = "UPDATE VoucherInvoicesAdjustment SET ActionTypeId = 3 WHERE VoucherHeadId = :id", nativeQuery = true)
    int cancelVoucherInvoicesAdjustment(@Param("id") Integer id);
}
