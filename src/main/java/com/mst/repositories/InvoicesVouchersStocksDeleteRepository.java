package com.mst.repositories;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InvPurchaseInvoice;
import com.mst.projections.ContractorWagesBillHeaderRow;
import com.mst.projections.GrnIdDocTypeRow;
import com.mst.projections.InvPurchaseInvoiceStateRow;
import com.mst.projections.RefRecordCheckRow;

/**
 * Sp_InvoicesVouchersandStocksDelete — generic "delete document + vouchers + stocks" procedure (1746 lines, WHILE loop + 3 cursors,
 * nested EXECs). The whole procedure is exposed as KEEP-EXEC (spInvoicesVouchersandStocksDelete); the standalone statements of the
 * purchase-invoice branch (DocumentTypeId IN 56,57,131,138,58,64,61,63,98,702,1804,245,1653,1654), the contractor-wages branch and the
 * final voucher cleanup are additionally exposed as JPQL/native helpers below (child-table deletes/archives live in the child repositories).
 *
 * Branches of the proc (by @DocumentTypeId):
 *  - guard: UserId required; CheckRecordReferedInRefRefIdsStockEvalautionDetail (<> 221) / ...InventoryTransactionsPartyProcessing (221); labIPmActivityLog reference
 *  - contractor wages (46,48,131,61,68,806,807,137,143,149,701,703,86,124,170,57,99,98,184,186,210,217,221,1612,205,206,222,66,1653,1654,145):
 *    WHILE over InvContractorWagesBillHeader rows -> delete their VoucherDetail/VoucherHead, wages detail/header, audit; then stock-in-transit evaluation rows
 *  - purchase invoice (56,57,131,138,58,64,61,63,98,702,1804,245,1653,1654): approved check, archive to H_* tables, delete children + header,
 *    InventoryStockEvalautionDetail, InventoryTransactions (57,131,61,98,1804,245,1653,1654), DMSAttachments, VoucherInvoicesAdjustment,
 *    cursor over GRNs -> usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn, audit
 *  - sale invoice (95,99,184,103,126,133,186,1608,1609,1660,1661,1662,171,222,145), export forwarding (210,205,206), GDN (86,149,170,124,703,1612,221,1659),
 *    GRN (46,48,137,165,167,169,701,1602,143,217), stock transfer (68,806,807), PDC voucher (16), delivery order (84), conversion (66),
 *    export invoice voucher (27), store issuance (451,452), export packing list (211), sale order (81,90): non-owned tables -> KEEP-EXEC only
 *  - finally: VoucherDetail/VoucherHead of (DocumentTypeId, DocumentTypeSrNo = Id) are deleted
 */
@Repository
public interface InvoicesVouchersStocksDeleteRepository extends org.springframework.data.repository.Repository<InvPurchaseInvoice, Integer> {

    /**
     * Sp_InvoicesVouchersandStocksDelete (all branches) — KEEP-EXEC: temp table + WHILE loop, 3 cursors, nested EXECs
     * (USP_UserAudit_Insert, usp_StockInTransitUpdate_*), many non-owned tables. No proc defaults (all 5 parameters required).
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_InvoicesVouchersandStocksDelete @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@DocumentTypeId = :documentTypeId, @Id = :id, @UserId = :userId", nativeQuery = true)
    void spInvoicesVouchersandStocksDelete(@Param("organizationId") Integer organizationId,
                                           @Param("companyId") Integer companyId,
                                           @Param("documentTypeId") Integer documentTypeId,
                                           @Param("id") Integer id,
                                           @Param("userId") Integer userId);

    // ---------------------------------------------------------------------------------
    //  guards
    // ---------------------------------------------------------------------------------

    /** Sp_InvoicesVouchersandStocksDelete (DocumentTypeId = 221): dbo.CheckRecordReferedInRefRefIdsInventoryTransactionsPartyProcessing(...) -> RefId / CodeNo / DocType. Native: table-valued function. Take first. (Other doc types: InvPurchaseInvoiceDetailRepository.checkRecordReferredInStockEvaluation) */
    @Query(value = "SELECT RefId, CodeNo, DocType FROM dbo.CheckRecordReferedInRefRefIdsInventoryTransactionsPartyProcessing(:organizationId, :companyId, :documentTypeId, :id)",
           nativeQuery = true)
    List<RefRecordCheckRow> checkRecordReferredInPartyProcessing(@Param("organizationId") Integer organizationId,
                                                                 @Param("companyId") Integer companyId,
                                                                 @Param("documentTypeId") Integer documentTypeId,
                                                                 @Param("id") Integer id);

    // ---------------------------------------------------------------------------------
    //  contractor wages branch
    // ---------------------------------------------------------------------------------

    /** Sp_InvoicesVouchersandStocksDelete (wages branch): #TmpWagesData — InvContractorWagesBillHeader rows referencing the document (processed ORDER BY Id DESC). Native: no entity. */
    @Query(value = "SELECT Id AS id, DocumentTypeId AS documentTypeId, DocNo AS docNo, QtyTotal AS qtyTotal, WeightTotal AS weightTotal "
         + "FROM InvContractorWagesBillHeader WHERE OrganizationId = :organizationId AND CompanyId = :companyId "
         + "AND RefDocumentTypeId = :documentTypeId AND RefDocNoId = :id ORDER BY Id DESC", nativeQuery = true)
    List<ContractorWagesBillHeaderRow> findWagesBillsOfDocument(@Param("organizationId") Integer organizationId,
                                                                @Param("companyId") Integer companyId,
                                                                @Param("documentTypeId") Integer documentTypeId,
                                                                @Param("id") Integer id);

    /** Sp_InvoicesVouchersandStocksDelete (wages branch): DELETE VoucherDetail of the voucher heads keyed by (DocumentTypeId, DocumentTypeSrNo) within Org/Comp. */
    @Modifying
    @Transactional
    @Query("DELETE FROM VoucherDetail d WHERE d.voucherHeadId IN (SELECT h.id FROM VoucherHead h WHERE h.organizationId = :organizationId "
         + "AND h.companyId = :companyId AND h.documentTypeId = :documentTypeId AND h.documentTypeSrNo = :documentTypeSrNo)")
    int deleteVoucherDetailsByDocument(@Param("organizationId") Integer organizationId,
                                       @Param("companyId") Integer companyId,
                                       @Param("documentTypeId") Integer documentTypeId,
                                       @Param("documentTypeSrNo") Integer documentTypeSrNo);

    /** Sp_InvoicesVouchersandStocksDelete (wages branch): DELETE VoucherHead keyed by (DocumentTypeId, DocumentTypeSrNo) within Org/Comp. */
    @Modifying
    @Transactional
    @Query("DELETE FROM VoucherHead h WHERE h.organizationId = :organizationId AND h.companyId = :companyId "
         + "AND h.documentTypeId = :documentTypeId AND h.documentTypeSrNo = :documentTypeSrNo")
    int deleteVoucherHeadsByDocument(@Param("organizationId") Integer organizationId,
                                     @Param("companyId") Integer companyId,
                                     @Param("documentTypeId") Integer documentTypeId,
                                     @Param("documentTypeSrNo") Integer documentTypeSrNo);

    /** Sp_InvoicesVouchersandStocksDelete (wages branch): DELETE FROM InvContractorWagesBillDetail WHERE InvContractorWagesBillHeaderId = @getContractId. Native: no entity. */
    @Modifying
    @Transactional
    @Query(value = "DELETE FROM InvContractorWagesBillDetail WHERE InvContractorWagesBillHeaderId = :wagesId", nativeQuery = true)
    int deleteWagesBillDetails(@Param("wagesId") Integer wagesId);

    /** Sp_InvoicesVouchersandStocksDelete (wages branch): DELETE FROM InvContractorWagesBillHeader WHERE Id = @getContractId. Native: no entity. */
    @Modifying
    @Transactional
    @Query(value = "DELETE FROM InvContractorWagesBillHeader WHERE Id = :wagesId", nativeQuery = true)
    int deleteWagesBillHeader(@Param("wagesId") Integer wagesId);

    /** Sp_InvoicesVouchersandStocksDelete (wages / forwarding / GDN branches): DELETE InventoryStockEvalautionDetail where OtherDocumentTypeId / OtherDocNoId reference the document (stock in transit). */
    @Modifying
    @Transactional
    @Query("DELETE FROM InventoryStockEvalautionDetail e WHERE e.organizationId = :organizationId AND e.companyId = :companyId "
         + "AND e.otherDocumentTypeId = :documentTypeId AND e.otherDocNoId = :id")
    int deleteStockEvaluationByOtherDoc(@Param("organizationId") Integer organizationId,
                                        @Param("companyId") Integer companyId,
                                        @Param("documentTypeId") Integer documentTypeId,
                                        @Param("id") Integer id);

    // ---------------------------------------------------------------------------------
    //  purchase invoice branch
    // ---------------------------------------------------------------------------------

    /** Sp_InvoicesVouchersandStocksDelete (purchase invoice branch): IsApproved / DocNo / BillAmount of the invoice (Org/Comp/Id) — IsApproved <> 0 -> 'Record cannot be delete because record has been approved'. Take first. */
    @Query("SELECT h.isApproved AS isApproved, h.postDate AS postDate, h.billAmount AS billAmount, h.docNo AS docNo, h.financialYearId AS financialYearId "
         + "FROM InvPurchaseInvoice h WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND h.id = :id")
    List<InvPurchaseInvoiceStateRow> readPurchaseInvoiceState(@Param("organizationId") Integer organizationId,
                                                              @Param("companyId") Integer companyId,
                                                              @Param("id") Integer id);

    /** Sp_InvoicesVouchersandStocksDelete (purchase invoice branch): archive copy INSERT INTO H_InvPurchaseInvoice (verbatim column list, trailing NULL). Native: history table has no entity. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO H_InvPurchaseInvoice SELECT [Id], [DocumentTypeId], [DocumentTypeSrNo], [DocNo], [DocDate], [FinancialYearId], [SupplierCustomerId], "
         + "[SupplierReferenceNo], [SupplierInvoiceNo], [SupplierInvoiceDate], [ManualBillNo], [StockPartyId], [ReferencePartyId], [CommissionAgentId], "
         + "[CommissionType], [CommRate], [UomScheduleIdCmRate], [CommAmount], [CommissionRemarks], [CashReceived], [RemarksHeader], [OtherRemarks], "
         + "[IsAttachments], [IsTaxable], [IsApproved], [EntryDate], [EntryUser], [ModifyDate], [ModifyUser], [PostDate], [PostUser], [OrganizationId], "
         + "[CompanyId], [BranchesId], [ProjectsId], [BillAmount], [PaymentTermsId], [DeliveryTerm], [BrokerAgentId], [BrokeryType], [BrokeryRate], "
         + "[BrokeryUom], [BrokeryAmount], [DueDate], [DueDays], [SalesTaxNo], [InvoiceTypeId], [TransportAccountId], [FreightAmount], [ActionId], "
         + "[GhallaMandiId], [ReferencePartyName], [DeliveryDays], [DeliveryStartDate], [TaxAccountId], [BillCalculateTypeId], [InvoiceQty], "
         + "[InvoiceWeight], [CurrencyId], [ExchangeRate], [FcyAmount], [RevisionNo], NULL "
         + "FROM [dbo].[InvPurchaseInvoice] WHERE Id = :id", nativeQuery = true)
    int archivePurchaseInvoiceHeader(@Param("id") Integer id);

    /** Sp_InvoicesVouchersandStocksDelete (purchase invoice branch): archive copy INSERT INTO H_InvPurchaseInvoiceDetail (verbatim column list, trailing NULL). Native: history table has no entity. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO H_InvPurchaseInvoiceDetail SELECT [Id], [InvPurchaseInvoiceId], [InvGrnId], [InvGrnDetailId], [PurchaseOrderId], [ItemId], "
         + "[PackingTypeId], [CropYear], [JobLotId], [ItemQty], [ItemUOMId], [GrossWeight], [EBWeight], [EBTotalWt], [AdLsWeight], [NetBillWeight], "
         + "[NetStockWeight], [ItemRate], [UomScheduleIdRate], [ItemAmount], [WarehouseId], [LabAnalisysNo], [GpDate], [IsTaxable], [TaxNameId], "
         + "[TaxPercent], [TaxDescriptions], [TaxAmount], [ItemCgsRate], [AvgCgsRate], [RemarksDetail], [RateCut], [RateCutAmount], [BillAmount], "
         + "[FreightAmount], [ExpenseAmount], [JournalAmount], [CommissionAmount], [WeightCut], [WeightCutTotal], [GpNo], [VehicleNo], [Brokery], "
         + "[ScaleKart], [PurchaseOrderDetailId], [OtherRate], [OtherRateUomId], [LineId], [CurrencyId], [ExchangeRate], [FcyAmount], [EntryDate], "
         + "[EntryUserId], [ModifyDate], [ModifyUserId], [ActionTypeId], [RevisionNo], NULL "
         + "FROM [dbo].[InvPurchaseInvoiceDetail] WHERE InvPurchaseInvoiceId = :id", nativeQuery = true)
    int archivePurchaseInvoiceDetails(@Param("id") Integer id);

    /** Sp_InvoicesVouchersandStocksDelete (purchase invoice branch): DELETE FROM InvPurchaseInvoice where OrganizationId/CompanyId/Id (children are deleted first via the child repositories). */
    @Modifying
    @Transactional
    @Query("DELETE FROM InvPurchaseInvoice h WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND h.id = :id")
    int deletePurchaseInvoiceHeader(@Param("organizationId") Integer organizationId,
                                    @Param("companyId") Integer companyId,
                                    @Param("id") Integer id);

    /** Sp_InvoicesVouchersandStocksDelete (purchase invoice + other branches): delete from DMSAttachments where RefDocumentTypeId / RefDocumentNo / Org / Comp. */
    @Modifying
    @Transactional
    @Query("DELETE FROM DMSAttachments a WHERE a.refDocumentTypeId = :documentTypeId AND a.refDocumentNo = :id "
         + "AND a.organizationId = :organizationId AND a.companyId = :companyId")
    int deleteDmsAttachmentsByDoc(@Param("documentTypeId") Integer documentTypeId,
                                  @Param("id") Integer id,
                                  @Param("organizationId") Integer organizationId,
                                  @Param("companyId") Integer companyId);

    /** Sp_InvoicesVouchersandStocksDelete (purchase invoice branch, GrnIds_cursor): DISTINCT InvGrnId + InvGrn.DocumentTypeId of the invoice's GRNs (#TmpGrnidsFromPurchaseInvoice JOIN InvGrn). */
    @Query("SELECT DISTINCT d.invGrnId AS invGrnId, g.documentTypeId AS documentTypeId "
         + "FROM InvPurchaseInvoiceDetail d JOIN InvPurchaseInvoice h ON d.invPurchaseInvoiceId = h.id JOIN InvGrn g ON d.invGrnId = g.id "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND h.documentTypeId = :documentTypeId "
         + "AND h.id = :id AND d.invGrnId > 0")
    List<GrnIdDocTypeRow> findGrnIdsWithDocType(@Param("organizationId") Integer organizationId,
                                                @Param("companyId") Integer companyId,
                                                @Param("documentTypeId") Integer documentTypeId,
                                                @Param("id") Integer id);

    /** Sp_InvoicesVouchersandStocksDelete (purchase invoice branch, per GRN of the cursor): nested EXEC usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn (positional params). KEEP-EXEC. */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn :organizationId, :companyId, :grnDocumentTypeId, :grnId", nativeQuery = true)
    void uspStockInTransitUpdateFromGrn(@Param("organizationId") Integer organizationId,
                                        @Param("companyId") Integer companyId,
                                        @Param("grnDocumentTypeId") Integer grnDocumentTypeId,
                                        @Param("grnId") Integer grnId);

    /** Sp_InvoicesVouchersandStocksDelete (sale invoice / export voucher branches, per GDN or forwarding of the cursor): nested EXEC usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding (positional params). KEEP-EXEC. */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding :organizationId, :companyId, :refDocumentTypeId, :refId", nativeQuery = true)
    void uspStockInTransitUpdateFromGdnOrForwarding(@Param("organizationId") Integer organizationId,
                                                    @Param("companyId") Integer companyId,
                                                    @Param("refDocumentTypeId") Integer refDocumentTypeId,
                                                    @Param("refId") Integer refId);

    /**
     * USP_UserAudit_Insert as called by Sp_InvoicesVouchersandStocksDelete / Sp_InvPurchaseInvoice_Insert / _Update (positional params:
     * @UserAuditId NULL, @ActivityDate, @UserId, @ClientIP NULL, @ActivityDescription, @RefDocumentTypeId, @ReferenceId, @ScreenName, @OrganizationId, @CompanyId, @ActionId 1/2/3). KEEP-EXEC.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.USP_UserAudit_Insert NULL, :activityDate, :userId, NULL, :activityDescription, :refDocumentTypeId, :referenceId, :screenName, "
         + ":organizationId, :companyId, :actionId", nativeQuery = true)
    void uspUserAuditInsert(@Param("activityDate") LocalDateTime activityDate,
                            @Param("userId") Integer userId,
                            @Param("activityDescription") String activityDescription,
                            @Param("refDocumentTypeId") Integer refDocumentTypeId,
                            @Param("referenceId") Integer referenceId,
                            @Param("screenName") String screenName,
                            @Param("organizationId") Integer organizationId,
                            @Param("companyId") Integer companyId,
                            @Param("actionId") Integer actionId);

    // ---------------------------------------------------------------------------------
    //  final voucher cleanup (all branches)
    // ---------------------------------------------------------------------------------

    /** Sp_InvoicesVouchersandStocksDelete (end): VoucherHead.Id where DocumentTypeId = @DocumentTypeId AND DocumentTypeSrNo = @Id within Org/Comp. Take first. */
    @Query("SELECT h.id FROM VoucherHead h WHERE h.documentTypeId = :documentTypeId AND h.documentTypeSrNo = :id "
         + "AND h.organizationId = :organizationId AND h.companyId = :companyId")
    List<Integer> findVoucherHeadIdOfDocument(@Param("documentTypeId") Integer documentTypeId,
                                              @Param("id") Integer id,
                                              @Param("organizationId") Integer organizationId,
                                              @Param("companyId") Integer companyId);

    /** Sp_InvoicesVouchersandStocksDelete (end): DELETE FROM VoucherDetail WHERE VoucherHeadId = @VHId. */
    @Modifying
    @Transactional
    @Query("DELETE FROM VoucherDetail d WHERE d.voucherHeadId = :voucherHeadId")
    int deleteVoucherDetailByHeadId(@Param("voucherHeadId") Integer voucherHeadId);

    /** Sp_InvoicesVouchersandStocksDelete (end): DELETE FROM VoucherHead WHERE Id = @VHId. */
    @Modifying
    @Transactional
    @Query("DELETE FROM VoucherHead h WHERE h.id = :voucherHeadId")
    int deleteVoucherHeadById(@Param("voucherHeadId") Integer voucherHeadId);
}
