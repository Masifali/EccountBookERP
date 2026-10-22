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

import com.mst.models.InvPurchaseInvoiceDetail;
import com.mst.projections.ContractorWagesBillHeaderRow;
import com.mst.projections.GrnGrossWeightRow;
import com.mst.projections.GrnWagesTotalsRow;
import com.mst.projections.InvGrnInfoRow;
import com.mst.projections.LabDeductionRow;
import com.mst.projections.MinMaxRateCheckRow;
import com.mst.projections.RefRecordCheckRow;
import com.mst.projections.StockEvaluationReferenceRow;
import com.mst.projections.UnmatchedDetailRow;

/**
 * Write-side repository for dbo.InvPurchaseInvoiceDetail.
 *
 * Sources: Sp_InvPurchaseInvoiceDetail_Insert (branches by @Activity: 'Purchase Invoice' (default), 'Purchase Invoice Return' (DocumentTypeId 59),
 * 'Sale Invoice Return' (DocumentTypeId 98); and by @ActionTypeId: 1 = insert (@Id null/0), 2 = update (@Id > 0), 3 = soft delete)
 * and usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations.
 *
 * Pre-insert/update logic of Sp_InvPurchaseInvoiceDetail_Insert (when using save()/updateById instead of the KEEP-EXEC method):
 *  - header context (Org, Comp, DocumentTypeId, DocDate, InvoiceTypeId, ActionId, SupplierCustomerId, BillCalculateTypeId, FY, BranchesId,
 *    EntryUser -> EntryUserId, ModifyUser -> ModifyUserId, RevisionNo) is read from InvPurchaseInvoice (InvPurchaseInvoiceRepository.readHeaderContext)
 *  - min/max item rate validation via dbo.CheckMinAndMaxRateOfItems (checkMinAndMaxRate)
 *  - empty-bag weight (EBWeight) min/max per PackingTypeId 1/2/5 from ConfigrationsAllocation definition ids 196..201 for doc types 56,57,138 (configKeyByDefinitionId)
 *  - DocDate must not be after the server date; DocDate must be after dbo.GetReconcilationMaxDate (reconciliationMaxDate)
 *  - Purchase return (59, no GdnId): stock availability checks via GetBalanceQtyAndWeightFromStockTransactions / ...FromStockEvaluation
 *  - GRN checks: detail must exist (grnDetailExistsCount), PurchaseOrderDetailId taken from InvGrnDetail (purchaseOrderDetailIdOfGrnDetail),
 *    GrnDate <= DocDate and GrnTypeId <> 2 (readGrnInfo), config 5616 may force GrnDate = DocDate, duplicate GRN detail (duplicateGrnDetailCount),
 *    invoice gross weight <= GRN gross weight (sumInvoiceGrossWeightByGrn, grnGrossWeightByGrn)
 *  - FIFO (feature 5): CheckRecordReferedInRefRefIdsStockEvalautionDetail (checkRecordReferredInStockEvaluation)
 *  - doc types 57,63,1804 with PurchaseOrderId: config 36 -> NetBillWeight <= GetPurchaseOrderandGatepassQtyBalance (purchaseOrderBalanceQty)
 *  - doc type 56, InvoiceTypeId 2/3, header ActionId 2: InvGrn.SupplierCustomerId and InventoryTransactions.SupplierCustomerId are updated
 *  - labour wages checks against InvContractorWagesBillHeader (wagesBillHeaderByRefDoc / grnWagesTotals / gdnWagesTotals), configs 103, 72, 173, 174
 *  - ActionTypeId <> 3: ItemId, ItemQty, NetBillWeight, ItemRate, ItemAmount, BillAmount must be > 0
 *  - NetStockWeight = NetBillWeight when 0; doc type 702: OtherRate/OtherRateUomId computed (firstEquivalentUomScheduleId)
 *  - GRN with lab: rate cut compulsory check (readLabDeduction) and USP_InvLabAnalysisPurchaseHeader_UpdateRateStatus (uspLabAnalysisUpdateRateStatus)
 *  - insert: EntryDate = GETDATE(); update: ModifyDate = GETDATE(); proc ends with SELECT @Id
 */
@Repository
public interface InvPurchaseInvoiceDetailRepository extends JpaRepository<InvPurchaseInvoiceDetail, Integer> {

    // =====================================================================================
    //  Sp_InvPurchaseInvoiceDetail_Insert — KEEP-EXEC (covers all branches)
    // =====================================================================================

    /**
     * Sp_InvPurchaseInvoiceDetail_Insert — all branches ('Purchase Invoice' / 'Purchase Invoice Return' / 'Sale Invoice Return';
     * ActionTypeId 1 insert when @Id is null/0, 2 update when @Id > 0, 3 soft delete).
     * KEEP-EXEC: many DB-function validations, nested EXEC USP_InvLabAnalysisPurchaseHeader_UpdateRateStatus, cross-table updates.
     * All parameters default to NULL. Returns the detail Id (proc ends with SELECT @Id).
     */
    @Transactional
    @Query(value = "EXEC dbo.Sp_InvPurchaseInvoiceDetail_Insert @Id = :id, @InvPurchaseInvoiceId = :invPurchaseInvoiceId, @InvGrnId = :invGrnId, "
         + "@InvGrnDetailId = :invGrnDetailId, @PurchaseOrderId = :purchaseOrderId, @ItemId = :itemId, @PackingTypeId = :packingTypeId, "
         + "@CropYear = :cropYear, @JobLotId = :jobLotId, @ItemQty = :itemQty, @ItemUOMId = :itemUomId, @GrossWeight = :grossWeight, "
         + "@EBWeight = :ebWeight, @EBTotalWt = :ebTotalWt, @AdLsWeight = :adLsWeight, @NetBillWeight = :netBillWeight, "
         + "@NetStockWeight = :netStockWeight, @ItemRate = :itemRate, @UomScheduleIdRate = :uomScheduleIdRate, @ItemAmount = :itemAmount, "
         + "@WarehouseId = :warehouseId, @LabAnalisysNo = :labAnalisysNo, @GpDate = :gpDate, @IsTaxable = :isTaxable, "
         + "@TaxDescriptions = :taxDescriptions, @TaxAmount = :taxAmount, @ItemCgsRate = :itemCgsRate, @AvgCgsRate = :avgCgsRate, "
         + "@RemarksDetail = :remarksDetail, @RateCut = :rateCut, @RateCutAmount = :rateCutAmount, @BillAmount = :billAmount, "
         + "@FreightAmount = :freightAmount, @ExpenseAmount = :expenseAmount, @JournalAmount = :journalAmount, "
         + "@CommissionAmount = :commissionAmount, @WeightCut = :weightCut, @WeightCutTotal = :weightCutTotal, @GpNo = :gpNo, "
         + "@VehicleNo = :vehicleNo, @TaxNameId = :taxNameId, @TaxPercent = :taxPercent, @Brokery = :brokery, @ScaleKart = :scaleKart, "
         + "@PurchaseOrderDetailId = :purchaseOrderDetailId, @OtherRate = :otherRate, @OtherRateUomId = :otherRateUomId, @LineId = :lineId, "
         + "@CurrencyId = :currencyId, @ExchangeRate = :exchangeRate, @FcyAmount = :fcyAmount, @ActionTypeId = :actionTypeId, "
         + "@AttributeVariantId = :attributeVariantId, @WagesAmount = :wagesAmount, @EbPurAgainstWeight = :ebPurAgainstWeight, "
         + "@BranchId = :branchId, @PackingDate = :packingDate, @ExpiryDate = :expiryDate, @CropYearId = :cropYearId, "
         + "@RefDocumentTypeId = :refDocumentTypeId, @RefDocId = :refDocId, @RefDocInvoiceId = :refDocInvoiceId, @RefDocSubId = :refDocSubId, "
         + "@DiscountTypeId = :discountTypeId, @DiscountPercent = :discountPercent, @DiscountAmount = :discountAmount, @CityId = :cityId, "
         + "@CastingTypeId = :castingTypeId, @ProductionStageId = :productionStageId, @GdnDocumentTypeId = :gdnDocumentTypeId, "
         + "@GdnId = :gdnId, @GdnDetailId = :gdnDetailId, @LocationTypeId = :locationTypeId, "
         + "@EbPurAgainstWeightAmount = :ebPurAgainstWeightAmount, @FreightDeduction = :freightDeduction, "
         + "@RateCutForExcessWeight = :rateCutForExcessWeight, @RateCutAmountForExcessWeight = :rateCutAmountForExcessWeight, "
         + "@ExcessWeight = :excessWeight, @ItemConditionId = :itemConditionId, @RackId = :rackId, "
         + "@ItemAmountWithoutDiscount = :itemAmountWithoutDiscount", nativeQuery = true)
    Integer spInvPurchaseInvoiceDetailInsert(@Param("id") Integer id,
                                             @Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId,
                                             @Param("invGrnId") Integer invGrnId,
                                             @Param("invGrnDetailId") Integer invGrnDetailId,
                                             @Param("purchaseOrderId") Integer purchaseOrderId,
                                             @Param("itemId") Integer itemId,
                                             @Param("packingTypeId") Integer packingTypeId,
                                             @Param("cropYear") String cropYear,
                                             @Param("jobLotId") Integer jobLotId,
                                             @Param("itemQty") Double itemQty,
                                             @Param("itemUomId") Integer itemUomId,
                                             @Param("grossWeight") Double grossWeight,
                                             @Param("ebWeight") Double ebWeight,
                                             @Param("ebTotalWt") Double ebTotalWt,
                                             @Param("adLsWeight") Double adLsWeight,
                                             @Param("netBillWeight") Double netBillWeight,
                                             @Param("netStockWeight") Double netStockWeight,
                                             @Param("itemRate") Double itemRate,
                                             @Param("uomScheduleIdRate") Integer uomScheduleIdRate,
                                             @Param("itemAmount") Double itemAmount,
                                             @Param("warehouseId") Integer warehouseId,
                                             @Param("labAnalisysNo") String labAnalisysNo,
                                             @Param("gpDate") LocalDate gpDate,
                                             @Param("isTaxable") String isTaxable,
                                             @Param("taxDescriptions") String taxDescriptions,
                                             @Param("taxAmount") Double taxAmount,
                                             @Param("itemCgsRate") Double itemCgsRate,
                                             @Param("avgCgsRate") Double avgCgsRate,
                                             @Param("remarksDetail") String remarksDetail,
                                             @Param("rateCut") Double rateCut,
                                             @Param("rateCutAmount") Double rateCutAmount,
                                             @Param("billAmount") Double billAmount,
                                             @Param("freightAmount") Double freightAmount,
                                             @Param("expenseAmount") Double expenseAmount,
                                             @Param("journalAmount") Double journalAmount,
                                             @Param("commissionAmount") Double commissionAmount,
                                             @Param("weightCut") Double weightCut,
                                             @Param("weightCutTotal") Double weightCutTotal,
                                             @Param("gpNo") Integer gpNo,
                                             @Param("vehicleNo") String vehicleNo,
                                             @Param("taxNameId") Integer taxNameId,
                                             @Param("taxPercent") Double taxPercent,
                                             @Param("brokery") Double brokery,
                                             @Param("scaleKart") Double scaleKart,
                                             @Param("purchaseOrderDetailId") Integer purchaseOrderDetailId,
                                             @Param("otherRate") Double otherRate,
                                             @Param("otherRateUomId") Integer otherRateUomId,
                                             @Param("lineId") Integer lineId,
                                             @Param("currencyId") Integer currencyId,
                                             @Param("exchangeRate") BigDecimal exchangeRate,
                                             @Param("fcyAmount") BigDecimal fcyAmount,
                                             @Param("actionTypeId") Integer actionTypeId,
                                             @Param("attributeVariantId") Integer attributeVariantId,
                                             @Param("wagesAmount") Double wagesAmount,
                                             @Param("ebPurAgainstWeight") Double ebPurAgainstWeight,
                                             @Param("branchId") Integer branchId,
                                             @Param("packingDate") LocalDateTime packingDate,
                                             @Param("expiryDate") LocalDateTime expiryDate,
                                             @Param("cropYearId") Integer cropYearId,
                                             @Param("refDocumentTypeId") Integer refDocumentTypeId,
                                             @Param("refDocId") Integer refDocId,
                                             @Param("refDocInvoiceId") Integer refDocInvoiceId,
                                             @Param("refDocSubId") Integer refDocSubId,
                                             @Param("discountTypeId") Integer discountTypeId,
                                             @Param("discountPercent") Double discountPercent,
                                             @Param("discountAmount") Double discountAmount,
                                             @Param("cityId") Integer cityId,
                                             @Param("castingTypeId") Integer castingTypeId,
                                             @Param("productionStageId") Integer productionStageId,
                                             @Param("gdnDocumentTypeId") Integer gdnDocumentTypeId,
                                             @Param("gdnId") Integer gdnId,
                                             @Param("gdnDetailId") Integer gdnDetailId,
                                             @Param("locationTypeId") Integer locationTypeId,
                                             @Param("ebPurAgainstWeightAmount") Double ebPurAgainstWeightAmount,
                                             @Param("freightDeduction") Double freightDeduction,
                                             @Param("rateCutForExcessWeight") Double rateCutForExcessWeight,
                                             @Param("rateCutAmountForExcessWeight") Double rateCutAmountForExcessWeight,
                                             @Param("excessWeight") Double excessWeight,
                                             @Param("itemConditionId") Integer itemConditionId,
                                             @Param("rackId") Integer rackId,
                                             @Param("itemAmountWithoutDiscount") BigDecimal itemAmountWithoutDiscount);

    // =====================================================================================
    //  Sp_InvPurchaseInvoiceDetail_Insert — standalone statements
    // =====================================================================================

    /**
     * Sp_InvPurchaseInvoiceDetail_Insert @ActionTypeId=2 (update): every SET column of the UPDATE.
     * ModifyDate = CURRENT_TIMESTAMP; modifyUserId / revisionNo come from the header; actionTypeId is 2.
     * PackingDate / ExpiryDate / CropYearId are set to themselves in the proc (unchanged) and are therefore not listed.
     */
    @Modifying
    @Transactional
    @Query("UPDATE InvPurchaseInvoiceDetail d SET d.invPurchaseInvoiceId = :invPurchaseInvoiceId, d.invGrnId = :invGrnId, "
         + "d.invGrnDetailId = :invGrnDetailId, d.purchaseOrderId = :purchaseOrderId, d.itemId = :itemId, d.packingTypeId = :packingTypeId, "
         + "d.cropYear = :cropYear, d.jobLotId = :jobLotId, d.itemQty = :itemQty, d.itemUomId = :itemUomId, d.grossWeight = :grossWeight, "
         + "d.ebWeight = :ebWeight, d.ebTotalWt = :ebTotalWt, d.adLsWeight = :adLsWeight, d.netBillWeight = :netBillWeight, "
         + "d.netStockWeight = :netStockWeight, d.itemRate = :itemRate, d.uomScheduleIdRate = :uomScheduleIdRate, d.itemAmount = :itemAmount, "
         + "d.warehouseId = :warehouseId, d.labAnalisysNo = :labAnalisysNo, d.gpDate = :gpDate, d.isTaxable = :isTaxable, "
         + "d.taxNameId = :taxNameId, d.taxPercent = :taxPercent, d.taxDescriptions = :taxDescriptions, d.taxAmount = :taxAmount, "
         + "d.itemCgsRate = :itemCgsRate, d.avgCgsRate = :avgCgsRate, d.remarksDetail = :remarksDetail, d.rateCut = :rateCut, "
         + "d.rateCutAmount = :rateCutAmount, d.billAmount = :billAmount, d.freightAmount = :freightAmount, d.expenseAmount = :expenseAmount, "
         + "d.journalAmount = :journalAmount, d.commissionAmount = :commissionAmount, d.weightCut = :weightCut, d.weightCutTotal = :weightCutTotal, "
         + "d.gpNo = :gpNo, d.vehicleNo = :vehicleNo, d.brokery = :brokery, d.scaleKart = :scaleKart, "
         + "d.purchaseOrderDetailId = :purchaseOrderDetailId, d.otherRate = :otherRate, d.otherRateUomId = :otherRateUomId, d.lineId = :lineId, "
         + "d.currencyId = :currencyId, d.exchangeRate = :exchangeRate, d.fcyAmount = :fcyAmount, d.modifyDate = CURRENT_TIMESTAMP, "
         + "d.modifyUserId = :modifyUserId, d.actionTypeId = :actionTypeId, d.revisionNo = :revisionNo, d.wagesAmount = :wagesAmount, "
         + "d.attributeVariantId = :attributeVariantId, d.ebPurAgainstWeight = :ebPurAgainstWeight, d.branchId = :branchId, "
         + "d.refDocumentTypeId = :refDocumentTypeId, d.refDocId = :refDocId, d.refDocInvoiceId = :refDocInvoiceId, d.refDocSubId = :refDocSubId, "
         + "d.discountTypeId = :discountTypeId, d.discountPercent = :discountPercent, d.discountAmount = :discountAmount, d.cityId = :cityId, "
         + "d.castingTypeId = :castingTypeId, d.productionStageId = :productionStageId, d.gdnDocumentTypeId = :gdnDocumentTypeId, "
         + "d.gdnId = :gdnId, d.gdnDetailId = :gdnDetailId, d.locationTypeId = :locationTypeId, "
         + "d.ebPurAgainstWeightAmount = :ebPurAgainstWeightAmount, d.freightDeduction = :freightDeduction, "
         + "d.rateCutForExcessWeight = :rateCutForExcessWeight, d.rateCutAmountForExcessWeight = :rateCutAmountForExcessWeight, "
         + "d.excessWeight = :excessWeight, d.itemConditionId = :itemConditionId, d.rackId = :rackId, "
         + "d.itemAmountWithoutDiscount = :itemAmountWithoutDiscount "
         + "WHERE d.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId,
                   @Param("invGrnId") Integer invGrnId,
                   @Param("invGrnDetailId") Integer invGrnDetailId,
                   @Param("purchaseOrderId") Integer purchaseOrderId,
                   @Param("itemId") Integer itemId,
                   @Param("packingTypeId") Integer packingTypeId,
                   @Param("cropYear") String cropYear,
                   @Param("jobLotId") Integer jobLotId,
                   @Param("itemQty") Double itemQty,
                   @Param("itemUomId") Integer itemUomId,
                   @Param("grossWeight") Double grossWeight,
                   @Param("ebWeight") Double ebWeight,
                   @Param("ebTotalWt") Double ebTotalWt,
                   @Param("adLsWeight") Double adLsWeight,
                   @Param("netBillWeight") Double netBillWeight,
                   @Param("netStockWeight") Double netStockWeight,
                   @Param("itemRate") Double itemRate,
                   @Param("uomScheduleIdRate") Integer uomScheduleIdRate,
                   @Param("itemAmount") Double itemAmount,
                   @Param("warehouseId") Integer warehouseId,
                   @Param("labAnalisysNo") String labAnalisysNo,
                   @Param("gpDate") LocalDate gpDate,
                   @Param("isTaxable") String isTaxable,
                   @Param("taxNameId") Integer taxNameId,
                   @Param("taxPercent") Double taxPercent,
                   @Param("taxDescriptions") String taxDescriptions,
                   @Param("taxAmount") Double taxAmount,
                   @Param("itemCgsRate") Double itemCgsRate,
                   @Param("avgCgsRate") Double avgCgsRate,
                   @Param("remarksDetail") String remarksDetail,
                   @Param("rateCut") Double rateCut,
                   @Param("rateCutAmount") Double rateCutAmount,
                   @Param("billAmount") Double billAmount,
                   @Param("freightAmount") Double freightAmount,
                   @Param("expenseAmount") Double expenseAmount,
                   @Param("journalAmount") Double journalAmount,
                   @Param("commissionAmount") Double commissionAmount,
                   @Param("weightCut") Double weightCut,
                   @Param("weightCutTotal") Double weightCutTotal,
                   @Param("gpNo") Integer gpNo,
                   @Param("vehicleNo") String vehicleNo,
                   @Param("brokery") Double brokery,
                   @Param("scaleKart") Double scaleKart,
                   @Param("purchaseOrderDetailId") Integer purchaseOrderDetailId,
                   @Param("otherRate") Double otherRate,
                   @Param("otherRateUomId") Integer otherRateUomId,
                   @Param("lineId") Integer lineId,
                   @Param("currencyId") Integer currencyId,
                   @Param("exchangeRate") BigDecimal exchangeRate,
                   @Param("fcyAmount") BigDecimal fcyAmount,
                   @Param("modifyUserId") Integer modifyUserId,
                   @Param("actionTypeId") Integer actionTypeId,
                   @Param("revisionNo") Integer revisionNo,
                   @Param("wagesAmount") Double wagesAmount,
                   @Param("attributeVariantId") Integer attributeVariantId,
                   @Param("ebPurAgainstWeight") Double ebPurAgainstWeight,
                   @Param("branchId") Integer branchId,
                   @Param("refDocumentTypeId") Integer refDocumentTypeId,
                   @Param("refDocId") Integer refDocId,
                   @Param("refDocInvoiceId") Integer refDocInvoiceId,
                   @Param("refDocSubId") Integer refDocSubId,
                   @Param("discountTypeId") Integer discountTypeId,
                   @Param("discountPercent") Double discountPercent,
                   @Param("discountAmount") Double discountAmount,
                   @Param("cityId") Integer cityId,
                   @Param("castingTypeId") Integer castingTypeId,
                   @Param("productionStageId") Integer productionStageId,
                   @Param("gdnDocumentTypeId") Integer gdnDocumentTypeId,
                   @Param("gdnId") Integer gdnId,
                   @Param("gdnDetailId") Integer gdnDetailId,
                   @Param("locationTypeId") Integer locationTypeId,
                   @Param("ebPurAgainstWeightAmount") Double ebPurAgainstWeightAmount,
                   @Param("freightDeduction") Double freightDeduction,
                   @Param("rateCutForExcessWeight") Double rateCutForExcessWeight,
                   @Param("rateCutAmountForExcessWeight") Double rateCutAmountForExcessWeight,
                   @Param("excessWeight") Double excessWeight,
                   @Param("itemConditionId") Integer itemConditionId,
                   @Param("rackId") Integer rackId,
                   @Param("itemAmountWithoutDiscount") BigDecimal itemAmountWithoutDiscount);

    /** Sp_InvPurchaseInvoiceDetail_Insert @ActionTypeId=3 (soft delete): ActionTypeId = 3, ModifyDate = GETDATE(), ModifyUserId, RevisionNo (from header). */
    @Modifying
    @Transactional
    @Query("UPDATE InvPurchaseInvoiceDetail d SET d.actionTypeId = 3, d.modifyDate = CURRENT_TIMESTAMP, d.modifyUserId = :modifyUserId, "
         + "d.revisionNo = :revisionNo WHERE d.id = :id")
    int markDeleted(@Param("id") Integer id, @Param("modifyUserId") Integer modifyUserId, @Param("revisionNo") Integer revisionNo);

    /** Sp_InvPurchaseInvoiceDetail_Insert: GRN duplicate check — count of other detail rows (d.Id <> @Id) already invoicing the same InvGrnId/InvGrnDetailId within Org/Comp. */
    @Query("SELECT COUNT(d) FROM InvPurchaseInvoiceDetail d JOIN InvPurchaseInvoice h ON d.invPurchaseInvoiceId = h.id "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND d.invGrnId = :invGrnId "
         + "AND d.invGrnDetailId = :invGrnDetailId AND d.id <> :id")
    Long duplicateGrnDetailCount(@Param("organizationId") Integer organizationId,
                                 @Param("companyId") Integer companyId,
                                 @Param("invGrnId") Integer invGrnId,
                                 @Param("invGrnDetailId") Integer invGrnDetailId,
                                 @Param("id") Integer id);

    /** Sp_InvPurchaseInvoiceDetail_Insert: ISNULL(SUM(GrossWeight),0) already invoiced against the GRN (excluding @Id when given); proc adds the new GrossWeight before comparing with grnGrossWeightByGrn. */
    @Query("SELECT COALESCE(SUM(d.grossWeight), 0) FROM InvPurchaseInvoiceDetail d JOIN InvPurchaseInvoice h ON d.invPurchaseInvoiceId = h.id "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND h.documentTypeId = :documentTypeId "
         + "AND d.invGrnId = :invGrnId AND (:id IS NULL OR d.id <> :id)")
    Double sumInvoiceGrossWeightByGrn(@Param("organizationId") Integer organizationId,
                                      @Param("companyId") Integer companyId,
                                      @Param("documentTypeId") Integer documentTypeId,
                                      @Param("invGrnId") Integer invGrnId,
                                      @Param("id") Integer id);

    /** Sp_InvPurchaseInvoiceDetail_Insert: SUM(InvGrnDetail.GrossWeight) and InvGrn.DocNo of the GRN (GROUP BY h.DocNo). Take first. */
    @Query("SELECT SUM(d.grossWeight) AS grnGrossWeight, h.docNo AS grnNo FROM InvGrnDetail d JOIN InvGrn h ON d.invGrnId = h.id "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND d.invGrnId = :invGrnId GROUP BY h.docNo")
    List<GrnGrossWeightRow> grnGrossWeightByGrn(@Param("organizationId") Integer organizationId,
                                                @Param("companyId") Integer companyId,
                                                @Param("invGrnId") Integer invGrnId);

    /** Sp_InvPurchaseInvoiceDetail_Insert: EXISTS(InvGrnDetail WHERE InvGrnId = @InvGrnId AND Id = @InvGrnDetailId). */
    @Query("SELECT COUNT(d) FROM InvGrnDetail d WHERE d.invGrnId = :invGrnId AND d.id = :invGrnDetailId")
    Long grnDetailExistsCount(@Param("invGrnId") Integer invGrnId, @Param("invGrnDetailId") Integer invGrnDetailId);

    /** Sp_InvPurchaseInvoiceDetail_Insert: @PurchaseOrderDetailId = InvGrnDetail.PurchaseOrderDetailId. Take first. */
    @Query("SELECT d.purchaseOrderDetailId FROM InvGrnDetail d WHERE d.id = :invGrnDetailId")
    List<Integer> purchaseOrderDetailIdOfGrnDetail(@Param("invGrnDetailId") Integer invGrnDetailId);

    /** Sp_InvPurchaseInvoiceDetail_Insert: InvGrn DocDate / GrnTypeId / DocumentTypeId / AddWages / DocNo of the GRN (Org/Comp optional: pass null to match by Id only). Take first. */
    @Query("SELECT h.docDate AS docDate, h.grnTypeId AS grnTypeId, h.documentTypeId AS documentTypeId, h.addWages AS addWages, h.docNo AS docNo "
         + "FROM InvGrn h WHERE h.id = :invGrnId AND (:organizationId IS NULL OR h.organizationId = :organizationId) "
         + "AND (:companyId IS NULL OR h.companyId = :companyId)")
    List<InvGrnInfoRow> readGrnInfo(@Param("organizationId") Integer organizationId,
                                    @Param("companyId") Integer companyId,
                                    @Param("invGrnId") Integer invGrnId);

    /** Sp_InvPurchaseInvoiceDetail_Insert: @LabId = InvGrnDetail.LabId of the GRN detail. Take first. */
    @Query("SELECT d.labId FROM InvGrnDetail d WHERE d.invGrnId = :invGrnId AND d.id = :invGrnDetailId")
    List<Integer> labIdOfGrnDetail(@Param("invGrnId") Integer invGrnId, @Param("invGrnDetailId") Integer invGrnDetailId);

    /** Sp_InvPurchaseInvoiceDetail_Insert: LabId of the GRN detail whose InvLabAnalysisPurchaseHeader.RateApplied is still 0. Native: lab header has no entity. Take first. */
    @Query(value = "SELECT gd.LabId FROM InvGrnDetail gd INNER JOIN InvLabAnalysisPurchaseHeader L ON L.Id = gd.LabId "
         + "WHERE gd.InvGrnId = :invGrnId AND gd.Id = :invGrnDetailId AND ISNULL(L.RateApplied, 0) = 0", nativeQuery = true)
    List<Integer> labIdPendingRateApplied(@Param("invGrnId") Integer invGrnId, @Param("invGrnDetailId") Integer invGrnDetailId);

    /** Sp_InvPurchaseInvoiceDetail_Insert: deduction info of the purchase lab analysis (rate cut compulsory check). Native: no entity. Take first. */
    @Query(value = "SELECT DeductionRate AS deductionRate, DeductionWeight AS deductionWeight, ISNULL(IsRateCutCompulsory, 0) AS rateCutCompulsory, "
         + "ISNULL(IsWeightCutCompulsory, 0) AS weightCutCompulsory, CAST(DocNo AS NVARCHAR(50)) AS docNo "
         + "FROM InvLabAnalysisPurchaseHeader WHERE Id = :labId", nativeQuery = true)
    List<LabDeductionRow> readLabDeduction(@Param("labId") Integer labId);

    /** Sp_InvPurchaseInvoiceDetail_Insert: nested EXEC USP_InvLabAnalysisPurchaseHeader_UpdateRateStatus (RateApplied = 1, RateAppliedFromId = 3). KEEP-EXEC. */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.USP_InvLabAnalysisPurchaseHeader_UpdateRateStatus @Id = :labId, @RateApplied = 1, @RateAppliedFromId = 3, "
         + "@RateAppliedUserId = :entryUserId, @ItemRate = :itemRate, @DeductionRate = :rateCut, @RateUomId = :uomScheduleIdRate", nativeQuery = true)
    void uspLabAnalysisUpdateRateStatus(@Param("labId") Integer labId,
                                        @Param("entryUserId") Integer entryUserId,
                                        @Param("itemRate") Double itemRate,
                                        @Param("rateCut") Double rateCut,
                                        @Param("uomScheduleIdRate") Integer uomScheduleIdRate);

    /** Sp_InvPurchaseInvoiceDetail_Insert: ConfigrationsAllocation.ConfigKey by ConfigrationsDefinitionId (196..201 empty-bag min/max weights). Take first; ISNULL(...,0). */
    @Query("SELECT c.configKey FROM ConfigrationsAllocation c WHERE c.configrationsDefinitionId = :configrationsDefinitionId")
    List<String> configKeyByDefinitionId(@Param("configrationsDefinitionId") Integer configrationsDefinitionId);

    /** Sp_InvPurchaseInvoiceDetail_Insert: DISTINCT ISNULL(ConfigKey,0) of ConfigrationsDefinitionId 36 (order qty check) within Org/Comp. Take first. */
    @Query("SELECT DISTINCT COALESCE(c.configKey, '0') FROM ConfigrationsAllocation c WHERE c.organizationId = :organizationId "
         + "AND c.companyId = :companyId AND c.configrationsDefinitionId = :configrationsDefinitionId")
    List<String> configKeyByOrgCompanyDefinitionId(@Param("organizationId") Integer organizationId,
                                                   @Param("companyId") Integer companyId,
                                                   @Param("configrationsDefinitionId") Integer configrationsDefinitionId);

    /** Sp_InvPurchaseInvoiceDetail_Insert / Sp_InvPurchaseInvoiceEmptyBags_Insert: Item.ItemName for error messages. */
    @Query("SELECT i.itemName FROM Item i WHERE i.id = :itemId")
    String itemNameById(@Param("itemId") Integer itemId);

    /** Sp_InvPurchaseInvoiceDetail_Insert (DocumentTypeId 702, BillCalculateTypeId 1): TOP 1 UOMSchedule.Id with Equivalent = 1 for the item/company. Take first. */
    @Query("SELECT us.id FROM UOMSchedule us JOIN UOM u ON us.scheduleUnitId = u.id WHERE us.itemId = :itemId AND us.companyId = :companyId AND us.equivalent = 1")
    List<Integer> firstEquivalentUomScheduleId(@Param("itemId") Integer itemId, @Param("companyId") Integer companyId);

    /** Sp_InvPurchaseInvoiceDetail_Insert (DocumentTypeId 56, InvoiceTypeId 2/3, header ActionId 2): UPDATE InvGrn SET SupplierCustomerId. */
    @Modifying
    @Transactional
    @Query("UPDATE InvGrn g SET g.supplierCustomerId = :supplierCustomerId WHERE g.id = :invGrnId")
    int updateGrnSupplier(@Param("invGrnId") Integer invGrnId, @Param("supplierCustomerId") Integer supplierCustomerId);

    /** Sp_InvPurchaseInvoiceDetail_Insert (DocumentTypeId 56, InvoiceTypeId 2/3, header ActionId 2): UPDATE InventoryTransactions SET SupplierCustomerId WHERE RefDocIdNo = GRN id. */
    @Modifying
    @Transactional
    @Query("UPDATE InventoryTransactions t SET t.supplierCustomerId = :supplierCustomerId WHERE t.organizationId = :organizationId "
         + "AND t.companyId = :companyId AND t.refDocIdNo = :invGrnId")
    int updateInventoryTransactionsSupplierByGrn(@Param("organizationId") Integer organizationId,
                                                 @Param("companyId") Integer companyId,
                                                 @Param("invGrnId") Integer invGrnId,
                                                 @Param("supplierCustomerId") Integer supplierCustomerId);

    /** Sp_InvPurchaseInvoiceDetail_Insert: labour-wages bill header(s) referencing a GRN/GDN (RefDocumentTypeId/RefDocNoId) within Org/Comp. Empty list = 'LabourWages entry is pending'. Native: no entity. */
    @Query(value = "SELECT Id AS id, DocumentTypeId AS documentTypeId, DocNo AS docNo, QtyTotal AS qtyTotal, WeightTotal AS weightTotal "
         + "FROM InvContractorWagesBillHeader WHERE OrganizationId = :organizationId AND CompanyId = :companyId "
         + "AND RefDocumentTypeId = :refDocumentTypeId AND RefDocNoId = :refDocNoId", nativeQuery = true)
    List<ContractorWagesBillHeaderRow> wagesBillHeaderByRefDoc(@Param("organizationId") Integer organizationId,
                                                               @Param("companyId") Integer companyId,
                                                               @Param("refDocumentTypeId") Integer refDocumentTypeId,
                                                               @Param("refDocNoId") Integer refDocNoId);

    /** Sp_InvPurchaseInvoiceDetail_Insert: GRN DocNo, SUM(GrossWeight), SUM(ItemQty) (GROUP BY DocNo) for the wages comparison. Take first. */
    @Query("SELECT h.docNo AS docNo, COALESCE(SUM(d.grossWeight), 0) AS grnWagesWeight, COALESCE(SUM(d.itemQty), 0) AS grnQty "
         + "FROM InvGrn h JOIN InvGrnDetail d ON h.id = d.invGrnId WHERE h.id = :invGrnId GROUP BY h.docNo")
    List<GrnWagesTotalsRow> grnWagesTotals(@Param("invGrnId") Integer invGrnId);

    /** Sp_InvPurchaseInvoiceDetail_Insert (DocumentTypeId 59/98 with GdnId): GDN DocNo, SUM(GrossWeight), SUM(ItemQty) for the wages comparison. Native: InvGdn has no entity. Take first. */
    @Query(value = "SELECT h.DocNo AS docNo, ISNULL(SUM(d.GrossWeight), 0) AS grnWagesWeight, ISNULL(SUM(d.ItemQty), 0) AS grnQty "
         + "FROM InvGdn h INNER JOIN InvGdnDetail d ON h.Id = d.InvGdnId WHERE h.Id = :gdnId GROUP BY h.DocNo", nativeQuery = true)
    List<GrnWagesTotalsRow> gdnWagesTotals(@Param("gdnId") Integer gdnId);

    /** Sp_InvPurchaseInvoiceDetail_Insert: InvGdn.DocNo for the 'LabourWages entry is pending against this GdnNo' message. Native: no entity. */
    @Query(value = "SELECT DocNo FROM InvGdn WHERE Id = :gdnId", nativeQuery = true)
    Integer gdnDocNo(@Param("gdnId") Integer gdnId);

    /** Sp_InvPurchaseInvoiceDetail_Insert: dbo.GetConfigurationByConfigId(Org, Comp, configId) — configs 314, 5053, 5616, 103, 72, 173, 174 (also 219 in EmptyBags). Native: DB scalar function. */
    @Query(value = "SELECT dbo.GetConfigurationByConfigId(:organizationId, :companyId, :configId)", nativeQuery = true)
    String configurationByConfigId(@Param("organizationId") Integer organizationId,
                                   @Param("companyId") Integer companyId,
                                   @Param("configId") Integer configId);

    /** Sp_InvPurchaseInvoiceDetail_Insert: dbo.CheckMinAndMaxRateOfItems(Org, Comp, ItemId, DocDate, UomScheduleIdRate, ItemRate, Activity) -> IsError / ErrorMessage. Native: table-valued function. Take first. */
    @Query(value = "SELECT IsError, ErrorMessage FROM dbo.CheckMinAndMaxRateOfItems(:organizationId, :companyId, :itemId, :docDate, :uomScheduleIdRate, :itemRate, :activity)",
           nativeQuery = true)
    List<MinMaxRateCheckRow> checkMinAndMaxRate(@Param("organizationId") Integer organizationId,
                                                @Param("companyId") Integer companyId,
                                                @Param("itemId") Integer itemId,
                                                @Param("docDate") LocalDate docDate,
                                                @Param("uomScheduleIdRate") Integer uomScheduleIdRate,
                                                @Param("itemRate") Double itemRate,
                                                @Param("activity") String activity);

    /** Sp_InvPurchaseInvoiceDetail_Insert: dbo.GetReconcilationMaxDate(Org, Comp, ItemId, JobLotId) — DocDate must be after it. Native: DB scalar function. */
    @Query(value = "SELECT dbo.GetReconcilationMaxDate(:organizationId, :companyId, :itemId, :jobLotId)", nativeQuery = true)
    java.sql.Date reconciliationMaxDate(@Param("organizationId") Integer organizationId,
                                        @Param("companyId") Integer companyId,
                                        @Param("itemId") Integer itemId,
                                        @Param("jobLotId") Integer jobLotId);

    /** Sp_InvPurchaseInvoiceDetail_Insert (purchase return 59): BalWeight from dbo.GetBalanceQtyAndWeightFromStockTransactions(...). Native: table-valued function. */
    @Query(value = "SELECT BalWeight FROM dbo.GetBalanceQtyAndWeightFromStockTransactions(:organizationId, :companyId, :itemId, :docDate, :warehouseId, :jobLotId, :cropYear, :packingTypeId, :itemUomId)",
           nativeQuery = true)
    Double balanceWeightFromStockTransactions(@Param("organizationId") Integer organizationId,
                                              @Param("companyId") Integer companyId,
                                              @Param("itemId") Integer itemId,
                                              @Param("docDate") LocalDate docDate,
                                              @Param("warehouseId") Integer warehouseId,
                                              @Param("jobLotId") Integer jobLotId,
                                              @Param("cropYear") String cropYear,
                                              @Param("packingTypeId") Integer packingTypeId,
                                              @Param("itemUomId") Integer itemUomId);

    /** Sp_InvPurchaseInvoiceDetail_Insert (purchase return 59, non-FIFO): BalWeight from dbo.GetBalanceQtyAndWeightFromStockEvaluation(...). Native: table-valued function. */
    @Query(value = "SELECT BalWeight FROM dbo.GetBalanceQtyAndWeightFromStockEvaluation(:organizationId, :companyId, :itemId, :docDate, :warehouseId, :jobLotId, :cropYear, :packingTypeId, :itemUomId)",
           nativeQuery = true)
    Double balanceWeightFromStockEvaluation(@Param("organizationId") Integer organizationId,
                                            @Param("companyId") Integer companyId,
                                            @Param("itemId") Integer itemId,
                                            @Param("docDate") LocalDate docDate,
                                            @Param("warehouseId") Integer warehouseId,
                                            @Param("jobLotId") Integer jobLotId,
                                            @Param("cropYear") String cropYear,
                                            @Param("packingTypeId") Integer packingTypeId,
                                            @Param("itemUomId") Integer itemUomId);

    /** Sp_InvPurchaseInvoiceDetail_Insert (FIFO) / Sp_InvoicesVouchersandStocksDelete: dbo.CheckRecordReferedInRefRefIdsStockEvalautionDetail(Org, Comp, DocumentTypeId, Id) -> RefId / CodeNo / DocType. Native: table-valued function. Take first. */
    @Query(value = "SELECT RefId, CodeNo, DocType FROM dbo.CheckRecordReferedInRefRefIdsStockEvalautionDetail(:organizationId, :companyId, :documentTypeId, :id)",
           nativeQuery = true)
    List<RefRecordCheckRow> checkRecordReferredInStockEvaluation(@Param("organizationId") Integer organizationId,
                                                                 @Param("companyId") Integer companyId,
                                                                 @Param("documentTypeId") Integer documentTypeId,
                                                                 @Param("id") Integer id);

    /** Sp_InvPurchaseInvoiceDetail_Insert (doc types 57,63,1804 with PurchaseOrderId, config 36): dbo.GetPurchaseOrderandGatepassQtyBalance(Org, Comp, PO, Item, 1, NULL). Native: DB scalar function. */
    @Query(value = "SELECT dbo.GetPurchaseOrderandGatepassQtyBalance(:organizationId, :companyId, :purchaseOrderId, :itemId, 1, NULL)", nativeQuery = true)
    Double purchaseOrderBalanceQty(@Param("organizationId") Integer organizationId,
                                   @Param("companyId") Integer companyId,
                                   @Param("purchaseOrderId") Integer purchaseOrderId,
                                   @Param("itemId") Integer itemId);

    // =====================================================================================
    //  usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations
    // =====================================================================================

    /**
     * usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations (single branch) — KEEP-EXEC: temp tables + dbo.fnSplitString;
     * deletes every detail of invoice @Id whose Id is NOT in the comma-separated @DetailIds unless one of them is referenced in
     * InventoryStockEvalautionDetail (RAISERROR). No proc defaults.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@Id = :id, @DetailIds = :detailIds", nativeQuery = true)
    void uspPurchaseInvoiceDetailRemoveByDetailIdsAndValidations(@Param("organizationId") Integer organizationId,
                                                                 @Param("companyId") Integer companyId,
                                                                 @Param("id") Integer id,
                                                                 @Param("detailIds") String detailIds);

    /** usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations: #TmpUnmatchedIds — details of invoice @Id whose Id is NOT in detailIds. */
    @Query("SELECT pd.id AS detailId, pd.invPurchaseInvoiceId AS invPurchaseInvoiceId, ph.documentTypeId AS documentTypeId "
         + "FROM InvPurchaseInvoiceDetail pd JOIN InvPurchaseInvoice ph ON ph.id = pd.invPurchaseInvoiceId "
         + "WHERE ph.id = :id AND pd.id NOT IN (:detailIds)")
    List<UnmatchedDetailRow> findUnmatchedDetails(@Param("id") Integer id, @Param("detailIds") List<Integer> detailIds);

    /** usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations: stock-evaluation rows referencing the unmatched details (RefRef* ids) -> non-empty means 'Cannot delete record because record exist in <docType> against DocNo <codeNo>'. */
    @Query("SELECT dt.documentTypeDescription AS docType, it.docCodeNo AS codeNo "
         + "FROM InventoryStockEvalautionDetail it "
         + "JOIN InvPurchaseInvoiceDetail pd ON it.refRefDocIdNo = pd.invPurchaseInvoiceId AND it.refRefDocSubIdNo = pd.id "
         + "JOIN InvPurchaseInvoice ph ON ph.id = pd.invPurchaseInvoiceId AND it.refRefDocumentTypeId = ph.documentTypeId "
         + "LEFT JOIN DocumentType dt ON it.refDocumentTypeId = dt.id "
         + "WHERE it.organizationId = :organizationId AND it.companyId = :companyId AND ph.id = :id AND pd.id NOT IN (:detailIds)")
    List<StockEvaluationReferenceRow> findStockEvaluationReferences(@Param("organizationId") Integer organizationId,
                                                                    @Param("companyId") Integer companyId,
                                                                    @Param("id") Integer id,
                                                                    @Param("detailIds") List<Integer> detailIds);

    /** usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations: DELETE the unmatched details (ids from findUnmatchedDetails). */
    @Modifying
    @Transactional
    @Query("DELETE FROM InvPurchaseInvoiceDetail d WHERE d.id IN (:ids)")
    int deleteByIdIn(@Param("ids") List<Integer> ids);

    // =====================================================================================
    //  Sp_InvoicesVouchersandStocksDelete — detail statements
    // =====================================================================================

    /** Sp_InvoicesVouchersandStocksDelete (purchase invoice branch): DELETE FROM InvPurchaseInvoiceDetail WHERE InvPurchaseInvoiceId = @Id. */
    @Modifying
    @Transactional
    @Query("DELETE FROM InvPurchaseInvoiceDetail d WHERE d.invPurchaseInvoiceId = :invPurchaseInvoiceId")
    int deleteByInvPurchaseInvoiceId(@Param("invPurchaseInvoiceId") Integer invPurchaseInvoiceId);

    /** Sp_InvoicesVouchersandStocksDelete (purchase invoice branch): DISTINCT InvGrnId (> 0) of the invoice details (#TmpGrnidsFromPurchaseInvoice). */
    @Query("SELECT DISTINCT d.invGrnId FROM InvPurchaseInvoiceDetail d JOIN InvPurchaseInvoice h ON d.invPurchaseInvoiceId = h.id "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND h.documentTypeId = :documentTypeId "
         + "AND h.id = :id AND d.invGrnId > 0")
    List<Integer> findGrnIdsOfInvoice(@Param("organizationId") Integer organizationId,
                                      @Param("companyId") Integer companyId,
                                      @Param("documentTypeId") Integer documentTypeId,
                                      @Param("id") Integer id);

    /** Sp_InvoicesVouchersandStocksDelete (GRN branch 46,48,137,...): purchase-invoice DocNo(s) referencing the GRN -> non-empty means 'Cannot Update This Document Because it has been Referred In Invoice'. */
    @Query("SELECT DISTINCT h.docNo FROM InvPurchaseInvoice h JOIN InvPurchaseInvoiceDetail d ON d.invPurchaseInvoiceId = h.id "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND d.invGrnId = :invGrnId")
    List<Integer> findInvoiceDocNosByGrnId(@Param("organizationId") Integer organizationId,
                                           @Param("companyId") Integer companyId,
                                           @Param("invGrnId") Integer invGrnId);

    /** Sp_InvoicesVouchersandStocksDelete (GDN branch, DocumentTypeId 703): purchase-return DocNo(s) referencing the GDN (GdnId / GdnDocumentTypeId). */
    @Query("SELECT DISTINCT h.docNo FROM InvPurchaseInvoice h JOIN InvPurchaseInvoiceDetail d ON d.invPurchaseInvoiceId = h.id "
         + "WHERE h.organizationId = :organizationId AND h.companyId = :companyId AND d.gdnId = :gdnId "
         + "AND (:gdnDocumentTypeId IS NULL OR d.gdnDocumentTypeId = :gdnDocumentTypeId)")
    List<Integer> findInvoiceDocNosByGdnId(@Param("organizationId") Integer organizationId,
                                           @Param("companyId") Integer companyId,
                                           @Param("gdnId") Integer gdnId,
                                           @Param("gdnDocumentTypeId") Integer gdnDocumentTypeId);
}
