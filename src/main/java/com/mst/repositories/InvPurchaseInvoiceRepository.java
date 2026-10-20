package com.mst.repositories;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InvPurchaseInvoice;
import com.mst.projections.InvPurchaseInvoiceHeaderContextRow;
import com.mst.projections.InvPurchaseInvoiceStateRow;

/**
 * Write-side repository for dbo.InvPurchaseInvoice.
 *
 * Sources: Sp_InvPurchaseInvoice_Insert, Sp_InvPurchaseInvoice_Update (plus header reads done by
 * Sp_InvPurchaseInvoiceDetail_Insert / Sp_InvPurchaseInvoiceEmptyBags_Insert).
 *
 * Pre-insert logic of Sp_InvPurchaseInvoice_Insert (when using save() instead of the KEEP-EXEC method):
 *  - RevisionNo = 1, ActionId = 1; SupplierReferenceNo = '0' when null/empty
 *  - raises 'BranchId not found' when BranchesId is 0 and ERPConfigurations feature 11 is active (see erpFeatureActiveCount)
 *  - validates DocDate against the active financial year (dbo.FunCheckValidFinancialYear - DB function, see financialYearValid)
 *  - DocNo = dbo.GenerateDocumentNumberDynamicly(Org, Comp, FY, DocumentTypeId, DocDate, NULL, NULL)  (see generateDocNo)
 *  - BranchSrNo = max(BranchSrNo)+1 within Org/Comp/FY/DocumentTypeId/BranchesId (see maxBranchSrNo)
 *  - SalesTaxNo = max(SalesTaxNo)+1 within Org/Comp/FY/IsTaxable when IsTaxable=1, else 0 (see maxSalesTaxNo)
 *  - PaymentTermsId defaults to 2 (credit) when null/0
 *  - Commission / Brokery 'Percent' rate must not exceed 1.5
 *  - UserLogId defaults to UserAccount.LastUserLogId of EntryUser (see lastUserLogIdOfUser)
 *  - ModifyDate/ModifyUser/PostDate/PostUser are NOT inserted
 *  - after insert: DAW.USp_DocumentApprovalDetail_Insert and USP_UserAudit_Insert (ActionId 1) are executed, SCOPE_IDENTITY() returned
 *
 * Pre-update logic of Sp_InvPurchaseInvoice_Update (when using updateById):
 *  - ActionId = 2; RevisionNo = max(RevisionNo)+1 of the row (see maxRevisionNo)
 *  - rejects: invalid FY date, FY change (FinancialYear table, see financialYearCode), approved invoice, lab fumigation reference
 *    (labIPmActivityLog, see labFumigationReferenceCount), commission/brokery > 1.5 percent
 *  - PaymentTermsId defaults to 2; UserLogId defaults from UserAccount.LastUserLogId of ModifyUser
 *  - VoucherInvoicesAdjustment.ActionTypeId = 3 for the doc (see markVoucherInvoicesAdjustmentDeleted)
 *  - deletes child rows (Freight/Journal/Expense/EmptyBags/PaymentTermsDetail -> see the child repositories), PaymentDueSchedule
 *  - DocumentTypeId = 59: deletes InventoryTransactions and (when ERP feature 5 is off) InventoryStockEvalautionDetail rows of the doc
 *  - after update: USP_UserAudit_Insert (ActionId 2)
 */
@Repository
public interface InvPurchaseInvoiceRepository extends JpaRepository<InvPurchaseInvoice, Integer> {

    // =====================================================================================
    //  Sp_InvPurchaseInvoice_Insert
    // =====================================================================================

    /**
     * Sp_InvPurchaseInvoice_Insert (single branch) — KEEP-EXEC: calls DB functions (FunCheckValidFinancialYear,
     * GenerateDocumentNumberDynamicly), DAW.USp_DocumentApprovalDetail_Insert and USP_UserAudit_Insert; RAISERRORs on validation.
     * All parameters default to NULL in the proc; @RowVersion (timestamp) is accepted but unused -> pass null.
     * Returns the new InvPurchaseInvoice.Id (proc ends with SELECT SCOPE_IDENTITY()).
     */
    @Transactional
    @Query(value = "EXEC dbo.Sp_InvPurchaseInvoice_Insert @Id = :id, @DocumentTypeId = :documentTypeId, @DocumentTypeSrNo = :documentTypeSrNo, "
         + "@DocNo = :docNo, @DocDate = :docDate, @SupplierCustomerId = :supplierCustomerId, @FinancialYearId = :financialYearId, "
         + "@SupplierReferenceNo = :supplierReferenceNo, @SupplierInvoiceNo = :supplierInvoiceNo, @SupplierInvoiceDate = :supplierInvoiceDate, "
         + "@ManualBillNo = :manualBillNo, @StockPartyId = :stockPartyId, @ReferencePartyId = :referencePartyId, "
         + "@CommissionAgentId = :commissionAgentId, @CommissionType = :commissionType, @CommRate = :commRate, "
         + "@UomScheduleIdCmRate = :uomScheduleIdCmRate, @CommAmount = :commAmount, @CommissionRemarks = :commissionRemarks, "
         + "@CashReceived = :cashReceived, @RemarksHeader = :remarksHeader, @OtherRemarks = :otherRemarks, @IsAttachments = :isAttachments, "
         + "@IsTaxable = :isTaxable, @TaxAccountId = :taxAccountId, @IsApproved = :isApproved, @EntryDate = :entryDate, @EntryUser = :entryUser, "
         + "@ModifyDate = :modifyDate, @ModifyUser = :modifyUser, @PostDate = :postDate, @PostUser = :postUser, "
         + "@OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, @ProjectsId = :projectsId, "
         + "@BillAmount = :billAmount, @PaymentTermsId = :paymentTermsId, @DeliveryTerm = :deliveryTerm, @BrokerAgentId = :brokerAgentId, "
         + "@BrokeryType = :brokeryType, @BrokeryRate = :brokeryRate, @BrokeryUom = :brokeryUom, @BrokeryAmount = :brokeryAmount, "
         + "@DueDate = :dueDate, @DueDays = :dueDays, @DeliveryStartDate = :deliveryStartDate, @DeliveryDays = :deliveryDays, "
         + "@SalesTaxNo = :salesTaxNo, @InvoiceTypeId = :invoiceTypeId, @TransportAccountId = :transportAccountId, "
         + "@FreightAmount = :freightAmount, @ActionId = :actionId, @GhallaMandiId = :ghallaMandiId, @ReferencePartyName = :referencePartyName, "
         + "@BillCalculateTypeId = :billCalculateTypeId, @InvoiceQty = :invoiceQty, @InvoiceWeight = :invoiceWeight, @CurrencyId = :currencyId, "
         + "@ExchangeRate = :exchangeRate, @FcyAmount = :fcyAmount, @RowVersion = :rowVersion, @WagesAmount = :wagesAmount, "
         + "@ScreenName = :screenName, @BranchSrNo = :branchSrNo, @CommissionCreditAccountId = :commissionCreditAccountId, "
         + "@AttachmentsValues = :attachmentsValues, @CustomAttachmentsValues = :customAttachmentsValues, @DiscountAmount = :discountAmount, "
         + "@DiscountAccountId = :discountAccountId, @TransporterCreditPartyId = :transporterCreditPartyId, "
         + "@TransporterDebitGLId = :transporterDebitGLId, @TransporterDebitPartyId = :transporterDebitPartyId, @FreightRemark = :freightRemark, "
         + "@CustomAccounts = :customAccounts, @IsUploaded = :isUploaded, @UploadedById = :uploadedById, @UploadedDate = :uploadedDate, "
         + "@UserLogId = :userLogId", nativeQuery = true)
    Integer spInvPurchaseInvoiceInsert(@Param("id") Integer id,
                                       @Param("documentTypeId") Integer documentTypeId,
                                       @Param("documentTypeSrNo") Integer documentTypeSrNo,
                                       @Param("docNo") Integer docNo,
                                       @Param("docDate") LocalDateTime docDate,
                                       @Param("supplierCustomerId") Integer supplierCustomerId,
                                       @Param("financialYearId") Integer financialYearId,
                                       @Param("supplierReferenceNo") String supplierReferenceNo,
                                       @Param("supplierInvoiceNo") Integer supplierInvoiceNo,
                                       @Param("supplierInvoiceDate") LocalDateTime supplierInvoiceDate,
                                       @Param("manualBillNo") String manualBillNo,
                                       @Param("stockPartyId") Integer stockPartyId,
                                       @Param("referencePartyId") Integer referencePartyId,
                                       @Param("commissionAgentId") Integer commissionAgentId,
                                       @Param("commissionType") String commissionType,
                                       @Param("commRate") Double commRate,
                                       @Param("uomScheduleIdCmRate") String uomScheduleIdCmRate,
                                       @Param("commAmount") Double commAmount,
                                       @Param("commissionRemarks") String commissionRemarks,
                                       @Param("cashReceived") Double cashReceived,
                                       @Param("remarksHeader") String remarksHeader,
                                       @Param("otherRemarks") String otherRemarks,
                                       @Param("isAttachments") String isAttachments,
                                       @Param("isTaxable") Boolean isTaxable,
                                       @Param("taxAccountId") Integer taxAccountId,
                                       @Param("isApproved") Boolean isApproved,
                                       @Param("entryDate") LocalDateTime entryDate,
                                       @Param("entryUser") Integer entryUser,
                                       @Param("modifyDate") LocalDateTime modifyDate,
                                       @Param("modifyUser") Integer modifyUser,
                                       @Param("postDate") LocalDateTime postDate,
                                       @Param("postUser") Integer postUser,
                                       @Param("organizationId") Integer organizationId,
                                       @Param("companyId") Integer companyId,
                                       @Param("branchesId") Integer branchesId,
                                       @Param("projectsId") Integer projectsId,
                                       @Param("billAmount") Double billAmount,
                                       @Param("paymentTermsId") Integer paymentTermsId,
                                       @Param("deliveryTerm") String deliveryTerm,
                                       @Param("brokerAgentId") Integer brokerAgentId,
                                       @Param("brokeryType") String brokeryType,
                                       @Param("brokeryRate") Double brokeryRate,
                                       @Param("brokeryUom") Double brokeryUom,
                                       @Param("brokeryAmount") Double brokeryAmount,
                                       @Param("dueDate") LocalDate dueDate,
                                       @Param("dueDays") Integer dueDays,
                                       @Param("deliveryStartDate") LocalDate deliveryStartDate,
                                       @Param("deliveryDays") Integer deliveryDays,
                                       @Param("salesTaxNo") Integer salesTaxNo,
                                       @Param("invoiceTypeId") Integer invoiceTypeId,
                                       @Param("transportAccountId") Integer transportAccountId,
                                       @Param("freightAmount") Double freightAmount,
                                       @Param("actionId") Integer actionId,
                                       @Param("ghallaMandiId") Integer ghallaMandiId,
                                       @Param("referencePartyName") String referencePartyName,
                                       @Param("billCalculateTypeId") Integer billCalculateTypeId,
                                       @Param("invoiceQty") BigDecimal invoiceQty,
                                       @Param("invoiceWeight") BigDecimal invoiceWeight,
                                       @Param("currencyId") Integer currencyId,
                                       @Param("exchangeRate") BigDecimal exchangeRate,
                                       @Param("fcyAmount") BigDecimal fcyAmount,
                                       @Param("rowVersion") byte[] rowVersion,
                                       @Param("wagesAmount") Double wagesAmount,
                                       @Param("screenName") String screenName,
                                       @Param("branchSrNo") Integer branchSrNo,
                                       @Param("commissionCreditAccountId") Integer commissionCreditAccountId,
                                       @Param("attachmentsValues") String attachmentsValues,
                                       @Param("customAttachmentsValues") String customAttachmentsValues,
                                       @Param("discountAmount") Double discountAmount,
                                       @Param("discountAccountId") Integer discountAccountId,
                                       @Param("transporterCreditPartyId") Integer transporterCreditPartyId,
                                       @Param("transporterDebitGLId") Integer transporterDebitGLId,
                                       @Param("transporterDebitPartyId") Integer transporterDebitPartyId,
                                       @Param("freightRemark") String freightRemark,
                                       @Param("customAccounts") Boolean customAccounts,
                                       @Param("isUploaded") Boolean isUploaded,
                                       @Param("uploadedById") Integer uploadedById,
                                       @Param("uploadedDate") LocalDateTime uploadedDate,
                                       @Param("userLogId") Long userLogId);

    /** Sp_InvPurchaseInvoice_Insert: ISNULL(MAX(BranchSrNo),0) within Org/Comp/FY/DocumentTypeId/BranchesId — caller adds 1. */
    @Query("SELECT COALESCE(MAX(h.branchSrNo), 0) FROM InvPurchaseInvoice h "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND h.financialYearId = :financialYearId "
         + "AND h.documentTypeId = :documentTypeId AND h.branchesId = :branchesId")
    Integer maxBranchSrNo(@Param("organizationId") Integer organizationId,
                          @Param("companyId") Integer companyId,
                          @Param("financialYearId") Integer financialYearId,
                          @Param("documentTypeId") Integer documentTypeId,
                          @Param("branchesId") Integer branchesId);

    /** Sp_InvPurchaseInvoice_Insert (IsTaxable = 1 branch): ISNULL(MAX(SalesTaxNo),0) within Org/Comp/FY/IsTaxable — caller adds 1; when IsTaxable<>1 the proc uses 0. */
    @Query("SELECT COALESCE(MAX(h.salesTaxNo), 0) FROM InvPurchaseInvoice h "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND h.financialYearId = :financialYearId "
         + "AND h.isTaxable = :isTaxable")
    Integer maxSalesTaxNo(@Param("organizationId") Integer organizationId,
                          @Param("companyId") Integer companyId,
                          @Param("financialYearId") Integer financialYearId,
                          @Param("isTaxable") Boolean isTaxable);

    /** Sp_InvPurchaseInvoice_Insert / _Update: EXISTS(ERPConfigurations feature 11 active) — BranchesId required when count > 0. Native: ERPConfigurations has no entity. */
    @Query(value = "SELECT COUNT(1) FROM ERPConfigurations WHERE OrganizationId = :organizationId AND CompanyId = :companyId AND FeaturesId = 11 AND IsActive = 1",
           nativeQuery = true)
    Long erpFeatureActiveCount(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_InvPurchaseInvoice_Insert / _Update: dbo.FunCheckValidFinancialYear(Org, Comp, FY, DocDate) — 0 means the DocDate is not inside the active financial year. Native: DB scalar function. */
    @Query(value = "SELECT CAST(dbo.FunCheckValidFinancialYear(:organizationId, :companyId, :financialYearId, :docDate) AS INT)", nativeQuery = true)
    Integer financialYearValid(@Param("organizationId") Integer organizationId,
                               @Param("companyId") Integer companyId,
                               @Param("financialYearId") Integer financialYearId,
                               @Param("docDate") LocalDateTime docDate);

    /** Sp_InvPurchaseInvoice_Insert: DocNo = dbo.GenerateDocumentNumberDynamicly(Org, Comp, FY, DocumentTypeId, DocDate, NULL, NULL). Native: DB scalar function. */
    @Query(value = "SELECT dbo.GenerateDocumentNumberDynamicly(:organizationId, :companyId, :financialYearId, :documentTypeId, :docDate, NULL, NULL)", nativeQuery = true)
    Integer generateDocNo(@Param("organizationId") Integer organizationId,
                          @Param("companyId") Integer companyId,
                          @Param("financialYearId") Integer financialYearId,
                          @Param("documentTypeId") Integer documentTypeId,
                          @Param("docDate") LocalDateTime docDate);

    /** Sp_InvPurchaseInvoice_Insert / _Update: UserLogId default = UserAccount.LastUserLogId of the entry/modify user. Native: column not in the UserAccount entity. */
    @Query(value = "SELECT LastUserLogId FROM UserAccount WHERE ID = :userId", nativeQuery = true)
    Long lastUserLogIdOfUser(@Param("userId") Integer userId);

    // =====================================================================================
    //  Sp_InvPurchaseInvoice_Update
    // =====================================================================================

    /**
     * Sp_InvPurchaseInvoice_Update (single branch) — KEEP-EXEC: validations via DB functions / FinancialYear / labIPmActivityLog,
     * cascading child deletes, conditional InventoryTransactions/InventoryStockEvalautionDetail deletes (DocumentTypeId 59) and USP_UserAudit_Insert.
     * All parameters default to NULL in the proc.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_InvPurchaseInvoice_Update @Id = :id, @DocumentTypeId = :documentTypeId, @DocumentTypeSrNo = :documentTypeSrNo, "
         + "@DocNo = :docNo, @DocDate = :docDate, @SupplierCustomerId = :supplierCustomerId, @FinancialYearId = :financialYearId, "
         + "@SupplierReferenceNo = :supplierReferenceNo, @SupplierInvoiceNo = :supplierInvoiceNo, @SupplierInvoiceDate = :supplierInvoiceDate, "
         + "@ManualBillNo = :manualBillNo, @StockPartyId = :stockPartyId, @ReferencePartyId = :referencePartyId, "
         + "@CommissionAgentId = :commissionAgentId, @CommissionType = :commissionType, @CommRate = :commRate, "
         + "@UomScheduleIdCmRate = :uomScheduleIdCmRate, @CommAmount = :commAmount, @CommissionRemarks = :commissionRemarks, "
         + "@CashReceived = :cashReceived, @RemarksHeader = :remarksHeader, @OtherRemarks = :otherRemarks, @IsAttachments = :isAttachments, "
         + "@IsTaxable = :isTaxable, @IsApproved = :isApproved, @EntryDate = :entryDate, @EntryUser = :entryUser, "
         + "@ModifyDate = :modifyDate, @ModifyUser = :modifyUser, @PostDate = :postDate, @PostUser = :postUser, "
         + "@OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, @ProjectsId = :projectsId, "
         + "@BillAmount = :billAmount, @PaymentTermsId = :paymentTermsId, @DeliveryTerm = :deliveryTerm, @BrokerAgentId = :brokerAgentId, "
         + "@BrokeryType = :brokeryType, @BrokeryRate = :brokeryRate, @BrokeryUom = :brokeryUom, @BrokeryAmount = :brokeryAmount, "
         + "@DueDate = :dueDate, @DueDays = :dueDays, @SalesTaxNo = :salesTaxNo, @TaxAccountId = :taxAccountId, "
         + "@InvoiceTypeId = :invoiceTypeId, @TransportAccountId = :transportAccountId, @FreightAmount = :freightAmount, "
         + "@ActionId = :actionId, @GhallaMandiId = :ghallaMandiId, @ReferencePartyName = :referencePartyName, "
         + "@DeliveryStartDate = :deliveryStartDate, @DeliveryDays = :deliveryDays, @BillCalculateTypeId = :billCalculateTypeId, "
         + "@InvoiceQty = :invoiceQty, @InvoiceWeight = :invoiceWeight, @CurrencyId = :currencyId, @ExchangeRate = :exchangeRate, "
         + "@FcyAmount = :fcyAmount, @WagesAmount = :wagesAmount, @ScreenName = :screenName, @BranchSrNo = :branchSrNo, "
         + "@CommissionCreditAccountId = :commissionCreditAccountId, @AttachmentsValues = :attachmentsValues, "
         + "@CustomAttachmentsValues = :customAttachmentsValues, @DiscountAmount = :discountAmount, @DiscountAccountId = :discountAccountId, "
         + "@TransporterCreditPartyId = :transporterCreditPartyId, @TransporterDebitGLId = :transporterDebitGLId, "
         + "@TransporterDebitPartyId = :transporterDebitPartyId, @FreightRemark = :freightRemark, @CustomAccounts = :customAccounts, "
         + "@IsUploaded = :isUploaded, @UploadedById = :uploadedById, @UploadedDate = :uploadedDate, @UserLogId = :userLogId",
           nativeQuery = true)
    void spInvPurchaseInvoiceUpdate(@Param("id") Integer id,
                                    @Param("documentTypeId") Integer documentTypeId,
                                    @Param("documentTypeSrNo") Integer documentTypeSrNo,
                                    @Param("docNo") Integer docNo,
                                    @Param("docDate") LocalDateTime docDate,
                                    @Param("supplierCustomerId") Integer supplierCustomerId,
                                    @Param("financialYearId") Integer financialYearId,
                                    @Param("supplierReferenceNo") String supplierReferenceNo,
                                    @Param("supplierInvoiceNo") Integer supplierInvoiceNo,
                                    @Param("supplierInvoiceDate") LocalDateTime supplierInvoiceDate,
                                    @Param("manualBillNo") String manualBillNo,
                                    @Param("stockPartyId") Integer stockPartyId,
                                    @Param("referencePartyId") Integer referencePartyId,
                                    @Param("commissionAgentId") Integer commissionAgentId,
                                    @Param("commissionType") String commissionType,
                                    @Param("commRate") Double commRate,
                                    @Param("uomScheduleIdCmRate") String uomScheduleIdCmRate,
                                    @Param("commAmount") Double commAmount,
                                    @Param("commissionRemarks") String commissionRemarks,
                                    @Param("cashReceived") Double cashReceived,
                                    @Param("remarksHeader") String remarksHeader,
                                    @Param("otherRemarks") String otherRemarks,
                                    @Param("isAttachments") String isAttachments,
                                    @Param("isTaxable") Boolean isTaxable,
                                    @Param("isApproved") Boolean isApproved,
                                    @Param("entryDate") LocalDateTime entryDate,
                                    @Param("entryUser") Integer entryUser,
                                    @Param("modifyDate") LocalDateTime modifyDate,
                                    @Param("modifyUser") Integer modifyUser,
                                    @Param("postDate") LocalDateTime postDate,
                                    @Param("postUser") Integer postUser,
                                    @Param("organizationId") Integer organizationId,
                                    @Param("companyId") Integer companyId,
                                    @Param("branchesId") Integer branchesId,
                                    @Param("projectsId") Integer projectsId,
                                    @Param("billAmount") Double billAmount,
                                    @Param("paymentTermsId") Integer paymentTermsId,
                                    @Param("deliveryTerm") String deliveryTerm,
                                    @Param("brokerAgentId") Integer brokerAgentId,
                                    @Param("brokeryType") String brokeryType,
                                    @Param("brokeryRate") Double brokeryRate,
                                    @Param("brokeryUom") Double brokeryUom,
                                    @Param("brokeryAmount") Double brokeryAmount,
                                    @Param("dueDate") LocalDate dueDate,
                                    @Param("dueDays") Integer dueDays,
                                    @Param("salesTaxNo") Integer salesTaxNo,
                                    @Param("taxAccountId") Integer taxAccountId,
                                    @Param("invoiceTypeId") Integer invoiceTypeId,
                                    @Param("transportAccountId") Integer transportAccountId,
                                    @Param("freightAmount") Double freightAmount,
                                    @Param("actionId") Integer actionId,
                                    @Param("ghallaMandiId") Integer ghallaMandiId,
                                    @Param("referencePartyName") String referencePartyName,
                                    @Param("deliveryStartDate") LocalDate deliveryStartDate,
                                    @Param("deliveryDays") Integer deliveryDays,
                                    @Param("billCalculateTypeId") Integer billCalculateTypeId,
                                    @Param("invoiceQty") BigDecimal invoiceQty,
                                    @Param("invoiceWeight") BigDecimal invoiceWeight,
                                    @Param("currencyId") Integer currencyId,
                                    @Param("exchangeRate") BigDecimal exchangeRate,
                                    @Param("fcyAmount") BigDecimal fcyAmount,
                                    @Param("wagesAmount") Double wagesAmount,
                                    @Param("screenName") String screenName,
                                    @Param("branchSrNo") Integer branchSrNo,
                                    @Param("commissionCreditAccountId") Integer commissionCreditAccountId,
                                    @Param("attachmentsValues") String attachmentsValues,
                                    @Param("customAttachmentsValues") String customAttachmentsValues,
                                    @Param("discountAmount") Double discountAmount,
                                    @Param("discountAccountId") Integer discountAccountId,
                                    @Param("transporterCreditPartyId") Integer transporterCreditPartyId,
                                    @Param("transporterDebitGLId") Integer transporterDebitGLId,
                                    @Param("transporterDebitPartyId") Integer transporterDebitPartyId,
                                    @Param("freightRemark") String freightRemark,
                                    @Param("customAccounts") Boolean customAccounts,
                                    @Param("isUploaded") Boolean isUploaded,
                                    @Param("uploadedById") Integer uploadedById,
                                    @Param("uploadedDate") LocalDateTime uploadedDate,
                                    @Param("userLogId") Long userLogId);

    /**
     * Sp_InvPurchaseInvoice_Update: the UPDATE statement itself (every SET column; EntryDate/EntryUser/PostDate/PostUser/BranchSrNo are not updated).
     * revisionNo = maxRevisionNo(id)+1 and actionId = 2 are computed by the proc and passed in here.
     */
    @Modifying
    @Transactional
    @Query("UPDATE InvPurchaseInvoice h SET h.documentTypeId = :documentTypeId, h.documentTypeSrNo = :documentTypeSrNo, h.docNo = :docNo, "
         + "h.docDate = :docDate, h.supplierCustomerId = :supplierCustomerId, h.financialYearId = :financialYearId, "
         + "h.supplierReferenceNo = :supplierReferenceNo, h.supplierInvoiceNo = :supplierInvoiceNo, h.supplierInvoiceDate = :supplierInvoiceDate, "
         + "h.manualBillNo = :manualBillNo, h.stockPartyId = :stockPartyId, h.referencePartyId = :referencePartyId, "
         + "h.commissionAgentId = :commissionAgentId, h.commissionType = :commissionType, h.commRate = :commRate, "
         + "h.uomScheduleIdCmRate = :uomScheduleIdCmRate, h.commAmount = :commAmount, h.commissionRemarks = :commissionRemarks, "
         + "h.cashReceived = :cashReceived, h.remarksHeader = :remarksHeader, h.otherRemarks = :otherRemarks, h.isAttachments = :isAttachments, "
         + "h.isTaxable = :isTaxable, h.isApproved = :isApproved, h.modifyDate = :modifyDate, h.modifyUser = :modifyUser, "
         + "h.organizationId = :organizationId, h.companyId = :companyId, h.branchesId = :branchesId, h.projectsId = :projectsId, "
         + "h.billAmount = :billAmount, h.paymentTermsId = :paymentTermsId, h.deliveryTerm = :deliveryTerm, h.brokerAgentId = :brokerAgentId, "
         + "h.brokeryType = :brokeryType, h.brokeryUom = :brokeryUom, h.brokeryRate = :brokeryRate, h.brokeryAmount = :brokeryAmount, "
         + "h.dueDate = :dueDate, h.dueDays = :dueDays, h.salesTaxNo = :salesTaxNo, h.invoiceTypeId = :invoiceTypeId, "
         + "h.transportAccountId = :transportAccountId, h.freightAmount = :freightAmount, h.actionId = :actionId, h.ghallaMandiId = :ghallaMandiId, "
         + "h.referencePartyName = :referencePartyName, h.deliveryStartDate = :deliveryStartDate, h.deliveryDays = :deliveryDays, "
         + "h.taxAccountId = :taxAccountId, h.billCalculateTypeId = :billCalculateTypeId, h.invoiceQty = :invoiceQty, h.invoiceWeight = :invoiceWeight, "
         + "h.currencyId = :currencyId, h.exchangeRate = :exchangeRate, h.fcyAmount = :fcyAmount, h.revisionNo = :revisionNo, "
         + "h.wagesAmount = :wagesAmount, h.screenName = :screenName, h.commissionCreditAccountId = :commissionCreditAccountId, "
         + "h.attachmentsValues = :attachmentsValues, h.customAttachmentsValues = :customAttachmentsValues, h.discountAmount = :discountAmount, "
         + "h.discountAccountId = :discountAccountId, h.transporterCreditPartyId = :transporterCreditPartyId, "
         + "h.transporterDebitGLId = :transporterDebitGLId, h.transporterDebitPartyId = :transporterDebitPartyId, h.freightRemark = :freightRemark, "
         + "h.customAccounts = :customAccounts, h.isUploaded = :isUploaded, h.uploadedById = :uploadedById, h.uploadedDate = :uploadedDate, "
         + "h.userLogId = :userLogId "
         + "WHERE h.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("documentTypeId") Integer documentTypeId,
                   @Param("documentTypeSrNo") Integer documentTypeSrNo,
                   @Param("docNo") Integer docNo,
                   @Param("docDate") java.util.Date docDate,
                   @Param("supplierCustomerId") Integer supplierCustomerId,
                   @Param("financialYearId") Integer financialYearId,
                   @Param("supplierReferenceNo") String supplierReferenceNo,
                   @Param("supplierInvoiceNo") Integer supplierInvoiceNo,
                   @Param("supplierInvoiceDate") LocalDateTime supplierInvoiceDate,
                   @Param("manualBillNo") String manualBillNo,
                   @Param("stockPartyId") Integer stockPartyId,
                   @Param("referencePartyId") Integer referencePartyId,
                   @Param("commissionAgentId") Integer commissionAgentId,
                   @Param("commissionType") String commissionType,
                   @Param("commRate") Double commRate,
                   @Param("uomScheduleIdCmRate") String uomScheduleIdCmRate,
                   @Param("commAmount") Double commAmount,
                   @Param("commissionRemarks") String commissionRemarks,
                   @Param("cashReceived") Double cashReceived,
                   @Param("remarksHeader") String remarksHeader,
                   @Param("otherRemarks") String otherRemarks,
                   @Param("isAttachments") String isAttachments,
                   @Param("isTaxable") Boolean isTaxable,
                   @Param("isApproved") Boolean isApproved,
                   @Param("modifyDate") LocalDateTime modifyDate,
                   @Param("modifyUser") Integer modifyUser,
                   @Param("organizationId") Integer organizationId,
                   @Param("companyId") Integer companyId,
                   @Param("branchesId") Integer branchesId,
                   @Param("projectsId") Integer projectsId,
                   @Param("billAmount") Double billAmount,
                   @Param("paymentTermsId") Integer paymentTermsId,
                   @Param("deliveryTerm") String deliveryTerm,
                   @Param("brokerAgentId") Integer brokerAgentId,
                   @Param("brokeryType") String brokeryType,
                   @Param("brokeryUom") Double brokeryUom,
                   @Param("brokeryRate") Double brokeryRate,
                   @Param("brokeryAmount") Double brokeryAmount,
                   @Param("dueDate") LocalDate dueDate,
                   @Param("dueDays") Integer dueDays,
                   @Param("salesTaxNo") Integer salesTaxNo,
                   @Param("invoiceTypeId") Integer invoiceTypeId,
                   @Param("transportAccountId") Integer transportAccountId,
                   @Param("freightAmount") Double freightAmount,
                   @Param("actionId") Integer actionId,
                   @Param("ghallaMandiId") Integer ghallaMandiId,
                   @Param("referencePartyName") String referencePartyName,
                   @Param("deliveryStartDate") LocalDate deliveryStartDate,
                   @Param("deliveryDays") Integer deliveryDays,
                   @Param("taxAccountId") Integer taxAccountId,
                   @Param("billCalculateTypeId") Integer billCalculateTypeId,
                   @Param("invoiceQty") BigDecimal invoiceQty,
                   @Param("invoiceWeight") BigDecimal invoiceWeight,
                   @Param("currencyId") Integer currencyId,
                   @Param("exchangeRate") BigDecimal exchangeRate,
                   @Param("fcyAmount") BigDecimal fcyAmount,
                   @Param("revisionNo") Integer revisionNo,
                   @Param("wagesAmount") Double wagesAmount,
                   @Param("screenName") String screenName,
                   @Param("commissionCreditAccountId") Integer commissionCreditAccountId,
                   @Param("attachmentsValues") String attachmentsValues,
                   @Param("customAttachmentsValues") String customAttachmentsValues,
                   @Param("discountAmount") Double discountAmount,
                   @Param("discountAccountId") Integer discountAccountId,
                   @Param("transporterCreditPartyId") Integer transporterCreditPartyId,
                   @Param("transporterDebitGLId") Integer transporterDebitGLId,
                   @Param("transporterDebitPartyId") Integer transporterDebitPartyId,
                   @Param("freightRemark") String freightRemark,
                   @Param("customAccounts") Boolean customAccounts,
                   @Param("isUploaded") Boolean isUploaded,
                   @Param("uploadedById") Integer uploadedById,
                   @Param("uploadedDate") LocalDateTime uploadedDate,
                   @Param("userLogId") Long userLogId);

    /** Sp_InvPurchaseInvoice_Update: ISNULL(MAX(RevisionNo),0) of the row — caller adds 1. */
    @Query("SELECT COALESCE(MAX(h.revisionNo), 0) FROM InvPurchaseInvoice h WHERE h.id = :id")
    Integer maxRevisionNo(@Param("id") Integer id);

    /** Sp_InvPurchaseInvoice_Update: current IsApproved / PostDate (approval date) / BillAmount / FinancialYearId of the row (Org/Comp/DocumentTypeId/Id). Take first. */
    @Query("SELECT h.isApproved AS isApproved, h.postDate AS postDate, h.billAmount AS billAmount, h.docNo AS docNo, h.financialYearId AS financialYearId "
         + "FROM InvPurchaseInvoice h WHERE h.organizationId = :organizationId AND h.companyId = :companyId "
         + "AND h.documentTypeId = :documentTypeId AND h.id = :id")
    List<InvPurchaseInvoiceStateRow> readState(@Param("organizationId") Integer organizationId,
                                               @Param("companyId") Integer companyId,
                                               @Param("documentTypeId") Integer documentTypeId,
                                               @Param("id") Integer id);

    /** Sp_InvPurchaseInvoice_Update: FinancialYear.FinancialYearCode by Id (used for the 'cannot change financial year' message). Native: FinancialYear has no entity. */
    @Query(value = "SELECT FinancialYearCode FROM FinancialYear WHERE Id = :financialYearId", nativeQuery = true)
    String financialYearCode(@Param("financialYearId") Integer financialYearId);

    /** Sp_InvPurchaseInvoice_Update / Sp_InvoicesVouchersandStocksDelete: EXISTS(labIPmActivityLog ... actionId <> 3) -> record is referenced in a lab fumigation process. Native: no entity. */
    @Query(value = "SELECT COUNT(1) FROM labIPmActivityLog WHERE companyId = :companyId AND refDocumentTypeId = :documentTypeId "
         + "AND refDocumentHeaderId = :id AND actionId <> 3", nativeQuery = true)
    Long labFumigationReferenceCount(@Param("companyId") Integer companyId,
                                     @Param("documentTypeId") Integer documentTypeId,
                                     @Param("id") Integer id);

    /** Sp_InvPurchaseInvoice_Update / Sp_InvoicesVouchersandStocksDelete: UPDATE VoucherInvoicesAdjustment SET ActionTypeId = 3 for the document. Native: no entity. */
    @Modifying
    @Transactional
    @Query(value = "UPDATE VoucherInvoicesAdjustment SET ActionTypeId = 3 WHERE RefDocumentTypeId = :documentTypeId AND RefDocNoId = :id", nativeQuery = true)
    int markVoucherInvoicesAdjustmentDeleted(@Param("documentTypeId") Integer documentTypeId, @Param("id") Integer id);

    /** Sp_InvPurchaseInvoice_Update: delete PaymentDueSchedule where RefdocNoId = @Id and refDocumentTypeId = @DocumentTypeId. */
    @Modifying
    @Transactional
    @Query("DELETE FROM PaymentDueSchedule p WHERE p.refdocNoId = :id AND p.refDocumentTypeId = :documentTypeId")
    int deletePaymentDueScheduleByDoc(@Param("id") Integer id, @Param("documentTypeId") Integer documentTypeId);

    /** Sp_InvPurchaseInvoice_Update (DocumentTypeId = 59) / Sp_InvoicesVouchersandStocksDelete: DELETE InventoryTransactions of the document. */
    @Modifying
    @Transactional
    @Query("DELETE FROM InventoryTransactions t WHERE t.organizationId = :organizationId AND t.companyId = :companyId "
         + "AND t.refDocumentTypeId = :documentTypeId AND t.refDocIdNo = :id")
    int deleteInventoryTransactionsByDoc(@Param("organizationId") Integer organizationId,
                                         @Param("companyId") Integer companyId,
                                         @Param("documentTypeId") Integer documentTypeId,
                                         @Param("id") Integer id);

    /** Sp_InvPurchaseInvoice_Update (DocumentTypeId = 59 and ERP feature 5 off) / Sp_InvoicesVouchersandStocksDelete: DELETE InventoryStockEvalautionDetail of the document. */
    @Modifying
    @Transactional
    @Query("DELETE FROM InventoryStockEvalautionDetail e WHERE e.refDocumentTypeId = :documentTypeId AND e.refDocIdNo = :id "
         + "AND e.organizationId = :organizationId AND e.companyId = :companyId")
    int deleteStockEvaluationByDoc(@Param("documentTypeId") Integer documentTypeId,
                                   @Param("id") Integer id,
                                   @Param("organizationId") Integer organizationId,
                                   @Param("companyId") Integer companyId);

    /** Sp_InvPurchaseInvoice_Update / Sp_InvPurchaseInvoiceDetail_Insert: dbo.fnCheckERPFeaturesExistorNot(Org, Comp, 5) — FIFO feature flag. Native: DB scalar function. */
    @Query(value = "SELECT CAST(dbo.fnCheckERPFeaturesExistorNot(:organizationId, :companyId, :featureId) AS INT)", nativeQuery = true)
    Integer erpFeatureExists(@Param("organizationId") Integer organizationId,
                             @Param("companyId") Integer companyId,
                             @Param("featureId") Integer featureId);

    /** Sp_InvPurchaseInvoice_Insert / _Update / Sp_InvoicesVouchersandStocksDelete: DocumentType.DocumentTypeDescription for the audit text. */
    @Query("SELECT dt.documentTypeDescription FROM DocumentType dt WHERE dt.id = :documentTypeId")
    String documentTypeDescription(@Param("documentTypeId") Integer documentTypeId);

    // =====================================================================================
    //  Header reads used by the child-insert procedures
    // =====================================================================================

    /** Sp_InvPurchaseInvoiceDetail_Insert / Sp_InvPurchaseInvoiceEmptyBags_Insert: header context (Org, Comp, DocumentTypeId, DocDate, InvoiceTypeId, ActionId, ...) of the invoice. Take first. */
    @Query("SELECT h.organizationId AS organizationId, h.revisionNo AS revisionNo, h.modifyUser AS modifyUser, h.companyId AS companyId, "
         + "h.documentTypeId AS documentTypeId, h.docDate AS docDate, h.invoiceTypeId AS invoiceTypeId, h.actionId AS actionId, "
         + "h.supplierCustomerId AS supplierCustomerId, h.billCalculateTypeId AS billCalculateTypeId, h.financialYearId AS financialYearId, "
         + "h.branchesId AS branchesId, h.entryUser AS entryUser "
         + "FROM InvPurchaseInvoice h WHERE h.id = :invPurchaseInvoiceId")
    List<InvPurchaseInvoiceHeaderContextRow> readHeaderContext(@Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId);
}
