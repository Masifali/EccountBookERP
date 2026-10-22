package com.mst.repositories;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.PurchaseOrderDetail;

/**
 * Batch E_purchase_order repository for dbo.PurchaseOrderDetail (the project already has PurchaseOrderDetailRepository,
 * hence the "Jpa" suffix).
 *
 * Sp_PurchaseOrderDetail_Insert is procedural (min/max rate validation via dbo.CheckMinAndMaxRateOfItems, UOM equivalents,
 * DocumentTypeId 41/42 specific rules, lab sample validation, RAISERROR checks, revise-remarks computation, history row into
 * PurchaseOrderDetail_H, and two different UPDATE shapes). Behaviour is kept through {@link #spPurchaseOrderDetailInsert};
 * the standalone statements of the proc are also exposed ({@link #maxId()}, {@link #updateById}, {@link #updateRateOnlyById}).
 * Plain inserts can use {@code save(entity)} with {@code id = maxId() + 1} (proc computes Id = ISNULL(MAX(CONVERT(INT,Id)),0)+1
 * and returns it with SELECT @Id).
 */
@Repository
public interface PurchaseOrderDetailJpaRepository extends JpaRepository<PurchaseOrderDetail, Integer> {

    // =============================================================================================
    // Sp_PurchaseOrderDetail_Insert
    // =============================================================================================

    /**
     * Sp_PurchaseOrderDetail_Insert (single branch; @Id null/0 → INSERT, otherwise UPDATE).
     * // KEEP-EXEC: procedural validations (CheckMinAndMaxRateOfItems, config checks 100/104/105, RAISERRORs),
     * computed NetWeight/Amount/FcyAmount for DocumentTypeId 41, history insert into PurchaseOrderDetail_H and
     * conditional UPDATE shapes cannot be expressed as one JPQL statement. All parameters default to NULL in the proc.
     * The insert branch returns the new Id (SELECT @Id); it is not returned by this void method — use maxId()/save() when the id is needed.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_PurchaseOrderDetail_Insert "
         + "@Amount = :amount, @AmountCalcType = :amountCalcType, @BagPrice = :bagPrice, @BagWeight = :bagWeight, "
         + "@CityArea = :cityArea, @Crop = :crop, @Id = :id, @JobLotId = :jobLotId, @LabSampleNo = :labSampleNo, "
         + "@LineId = :lineId, @Moisture = :moisture, @NetWeight = :netWeight, @OrderItemId = :orderItemId, "
         + "@OrderItemQty = :orderItemQty, @OrderItemRate = :orderItemRate, @OrderItemRateUOMId = :orderItemRateUomId, "
         + "@OrderItemUOMId = :orderItemUomId, @OrderRemarks = :orderRemarks, @PurchaseOrderId = :purchaseOrderId, "
         + "@RetailRate = :retailRate, @TaxableStatus = :taxableStatus, @TaxAmount = :taxAmount, @TaxNameId = :taxNameId, "
         + "@TaxPercent = :taxPercent, @TotalAmount = :totalAmount, @ReviseRemarks = :reviseRemarks, "
         + "@InvLabSampleAnalysisHeaderId = :invLabSampleAnalysisHeaderId, @WeightPerQty = :weightPerQty, "
         + "@CurrencyId = :currencyId, @ExchangeRate = :exchangeRate, @FcyAmount = :fcyAmount, @CityId = :cityId, "
         + "@PackingDate = :packingDate, @ExpiryDate = :expiryDate, @CropYearId = :cropYearId, @LeadTime = :leadTime, "
         + "@WeightCapacity = :weightCapacity, @RefDocumentTypeId = :refDocumentTypeId, @RefDocId = :refDocId, "
         + "@RefDocInvoiceId = :refDocInvoiceId, @LabAnalysisStandardScheduleId = :labAnalysisStandardScheduleId",
         nativeQuery = true)
    void spPurchaseOrderDetailInsert(@Param("amount") Double amount,
                                     @Param("amountCalcType") String amountCalcType,
                                     @Param("bagPrice") Double bagPrice,
                                     @Param("bagWeight") Double bagWeight,
                                     @Param("cityArea") String cityArea,
                                     @Param("crop") String crop,
                                     @Param("id") Integer id,
                                     @Param("jobLotId") Integer jobLotId,
                                     @Param("labSampleNo") String labSampleNo,
                                     @Param("lineId") Integer lineId,
                                     @Param("moisture") String moisture,
                                     @Param("netWeight") Double netWeight,
                                     @Param("orderItemId") Integer orderItemId,
                                     @Param("orderItemQty") Double orderItemQty,
                                     @Param("orderItemRate") Double orderItemRate,
                                     @Param("orderItemRateUomId") Integer orderItemRateUomId,
                                     @Param("orderItemUomId") Integer orderItemUomId,
                                     @Param("orderRemarks") String orderRemarks,
                                     @Param("purchaseOrderId") Integer purchaseOrderId,
                                     @Param("retailRate") Double retailRate,
                                     @Param("taxableStatus") String taxableStatus,
                                     @Param("taxAmount") Double taxAmount,
                                     @Param("taxNameId") Integer taxNameId,
                                     @Param("taxPercent") Double taxPercent,
                                     @Param("totalAmount") Double totalAmount,
                                     @Param("reviseRemarks") String reviseRemarks,
                                     @Param("invLabSampleAnalysisHeaderId") Integer invLabSampleAnalysisHeaderId,
                                     @Param("weightPerQty") Double weightPerQty,
                                     @Param("currencyId") Integer currencyId,
                                     @Param("exchangeRate") Double exchangeRate,
                                     @Param("fcyAmount") Double fcyAmount,
                                     @Param("cityId") Integer cityId,
                                     @Param("packingDate") LocalDateTime packingDate,
                                     @Param("expiryDate") LocalDateTime expiryDate,
                                     @Param("cropYearId") Integer cropYearId,
                                     @Param("leadTime") Integer leadTime,
                                     @Param("weightCapacity") String weightCapacity,
                                     @Param("refDocumentTypeId") Integer refDocumentTypeId,
                                     @Param("refDocId") Integer refDocId,
                                     @Param("refDocInvoiceId") Integer refDocInvoiceId,
                                     @Param("labAnalysisStandardScheduleId") Integer labAnalysisStandardScheduleId);

    /** Sp_PurchaseOrderDetail_Insert (insert branch): SELECT ISNULL(MAX(CONVERT(INT,Id)),0) FROM PurchaseOrderDetail (caller adds 1). */
    @Query("select coalesce(max(e.id),0) from PurchaseOrderDetail e")
    Integer maxId();

    /**
     * Sp_PurchaseOrderDetail_Insert (update branch, normal case): full UPDATE PurchaseOrderDetail SET ... WHERE Id = @Id.
     * ModifyDate = GETDATE(); modifyUserId is PurchaseOrder.ModifyUser of the header in the proc; reviseRemarks is the
     * proc-computed 'ReviseQty/ReviseWeight/ReviseRate ...' text.
     */
    @Modifying
    @Transactional
    @Query("UPDATE PurchaseOrderDetail d SET d.purchaseOrderId = :purchaseOrderId, d.orderItemId = :orderItemId, "
         + "d.orderItemUomId = :orderItemUomId, d.orderItemQty = :orderItemQty, d.netWeight = :netWeight, "
         + "d.orderItemRate = :orderItemRate, d.orderItemRateUomId = :orderItemRateUomId, d.amount = :amount, "
         + "d.jobLotId = :jobLotId, d.cityArea = :cityArea, d.labSampleNo = :labSampleNo, d.amountCalcType = :amountCalcType, "
         + "d.taxableStatus = :taxableStatus, d.orderRemarks = :orderRemarks, d.crop = :crop, d.bagWeight = :bagWeight, "
         + "d.bagPrice = :bagPrice, d.moisture = :moisture, d.taxAmount = :taxAmount, d.lineId = :lineId, "
         + "d.retailRate = :retailRate, d.taxNameId = :taxNameId, d.taxPercent = :taxPercent, d.totalAmount = :totalAmount, "
         + "d.invLabSampleAnalysisHeaderId = :invLabSampleAnalysisHeaderId, d.reviseRemarks = :reviseRemarks, "
         + "d.weightPerQty = :weightPerQty, d.currencyId = :currencyId, d.exchangeRate = :exchangeRate, "
         + "d.fcyAmount = :fcyAmount, d.cityId = :cityId, d.modifyDate = CURRENT_TIMESTAMP, d.modifyUserId = :modifyUserId, "
         + "d.packingDate = :packingDate, d.expiryDate = :expiryDate, d.cropYearId = :cropYearId, d.leadTime = :leadTime, "
         + "d.weightCapacity = :weightCapacity, d.refDocumentTypeId = :refDocumentTypeId, d.refDocId = :refDocId, "
         + "d.refDocInvoiceId = :refDocInvoiceId, d.labAnalysisStandardScheduleId = :labAnalysisStandardScheduleId "
         + "WHERE d.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("purchaseOrderId") Integer purchaseOrderId,
                   @Param("orderItemId") Integer orderItemId,
                   @Param("orderItemUomId") Integer orderItemUomId,
                   @Param("orderItemQty") Double orderItemQty,
                   @Param("netWeight") Double netWeight,
                   @Param("orderItemRate") Double orderItemRate,
                   @Param("orderItemRateUomId") Integer orderItemRateUomId,
                   @Param("amount") Double amount,
                   @Param("jobLotId") Integer jobLotId,
                   @Param("cityArea") String cityArea,
                   @Param("labSampleNo") String labSampleNo,
                   @Param("amountCalcType") String amountCalcType,
                   @Param("taxableStatus") String taxableStatus,
                   @Param("orderRemarks") String orderRemarks,
                   @Param("crop") String crop,
                   @Param("bagWeight") Double bagWeight,
                   @Param("bagPrice") Double bagPrice,
                   @Param("moisture") String moisture,
                   @Param("taxAmount") Double taxAmount,
                   @Param("lineId") Integer lineId,
                   @Param("retailRate") Double retailRate,
                   @Param("taxNameId") Integer taxNameId,
                   @Param("taxPercent") Double taxPercent,
                   @Param("totalAmount") Double totalAmount,
                   @Param("invLabSampleAnalysisHeaderId") Integer invLabSampleAnalysisHeaderId,
                   @Param("reviseRemarks") String reviseRemarks,
                   @Param("weightPerQty") Double weightPerQty,
                   @Param("currencyId") Integer currencyId,
                   @Param("exchangeRate") Double exchangeRate,
                   @Param("fcyAmount") Double fcyAmount,
                   @Param("cityId") Integer cityId,
                   @Param("modifyUserId") Integer modifyUserId,
                   @Param("packingDate") LocalDateTime packingDate,
                   @Param("expiryDate") LocalDateTime expiryDate,
                   @Param("cropYearId") Integer cropYearId,
                   @Param("leadTime") Integer leadTime,
                   @Param("weightCapacity") String weightCapacity,
                   @Param("refDocumentTypeId") Integer refDocumentTypeId,
                   @Param("refDocId") Integer refDocId,
                   @Param("refDocInvoiceId") Integer refDocInvoiceId,
                   @Param("labAnalysisStandardScheduleId") Integer labAnalysisStandardScheduleId);

    /**
     * Sp_PurchaseOrderDetail_Insert (update branch when DocumentTypeId = 41 AND the order is referred by a deleted invoice
     * (DeleteInvoicesData RefDocumentTypeId 56 / ActionTypeId 1) AND a GRN exists): only rate related columns are updated.
     * In the proc amount = existing NetWeight / RateEquivalent * OrderItemRate and fcyAmount = amount * ExchangeRate.
     */
    @Modifying
    @Transactional
    @Query("UPDATE PurchaseOrderDetail d SET d.purchaseOrderId = :purchaseOrderId, d.orderItemRate = :orderItemRate, "
         + "d.orderItemRateUomId = :orderItemRateUomId, d.amount = :amount, d.totalAmount = :totalAmount, "
         + "d.reviseRemarks = :reviseRemarks, d.fcyAmount = :fcyAmount, d.cityId = :cityId, "
         + "d.refDocumentTypeId = :refDocumentTypeId, d.refDocId = :refDocId, d.refDocInvoiceId = :refDocInvoiceId "
         + "WHERE d.id = :id")
    int updateRateOnlyById(@Param("id") Integer id,
                           @Param("purchaseOrderId") Integer purchaseOrderId,
                           @Param("orderItemRate") Double orderItemRate,
                           @Param("orderItemRateUomId") Integer orderItemRateUomId,
                           @Param("amount") Double amount,
                           @Param("totalAmount") Double totalAmount,
                           @Param("reviseRemarks") String reviseRemarks,
                           @Param("fcyAmount") Double fcyAmount,
                           @Param("cityId") Integer cityId,
                           @Param("refDocumentTypeId") Integer refDocumentTypeId,
                           @Param("refDocId") Integer refDocId,
                           @Param("refDocInvoiceId") Integer refDocInvoiceId);

    /** Sp_PurchaseOrderDetail_Insert: SELECT Equivalent FROM UOMSchedule WHERE Id = @UomScheduleId (used for @OrderItemRateUOMId and @OrderItemUOMId). */
    @Query(value = "SELECT Equivalent FROM UOMSchedule WHERE Id = :uomScheduleId", nativeQuery = true)
    Double uomScheduleEquivalent(@Param("uomScheduleId") Integer uomScheduleId);

    /**
     * Sp_PurchaseOrderDetail_Insert / Sp_PurchaseOrder_GetAllMethod 'PoWeightAndGpWeightValidation': lab purchase analysis
     * item mismatch check (count > 0 means the proc raises
     * 'Purchase Order cannot be update because Lab Purchase Analysis item mismatch with purchase order items...').
     */
    @Query(value = "SELECT COUNT(1) FROM InvLabAnalysisPurchaseHeader L "
         + "INNER JOIN GatePassInward GP ON L.GatePassInwardId = GP.Id AND GP.RefDocumentTypeId = 41 "
         + "WHERE GP.PurchaseOrderId = :purchaseOrderId "
         + "AND NOT EXISTS (SELECT 1 FROM PurchaseOrderDetail D WHERE D.PurchaseOrderId = :purchaseOrderId AND D.OrderItemId = L.ItemId)",
         nativeQuery = true)
    Integer countLabAnalysisItemMismatch(@Param("purchaseOrderId") Integer purchaseOrderId);

    // =============================================================================================
    // USP_DeletePurchaseOrderDetailIfNotExistInGrn
    // =============================================================================================

    /**
     * USP_DeletePurchaseOrderDetailIfNotExistInGrn (single branch).
     * // KEEP-EXEC: fnSplitString temp table, GRN / Purchase Invoice existence RAISERRORs, ROW_NUMBER() history insert
     * into PurchaseOrderDetail_H (ActionTypeId = 3, ModifyUserId = @UserId) and conditional DELETE.
     * orderDetailIds is the comma separated list of PurchaseOrderDetail.Id values.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.USP_DeletePurchaseOrderDetailIfNotExistInGrn @OrganizationId = :organizationId, "
         + "@CompanyId = :companyId, @OrderId = :orderId, @OrderDetailIds = :orderDetailIds, @UserId = :userId",
         nativeQuery = true)
    void uspDeletePurchaseOrderDetailIfNotExistInGrn(@Param("organizationId") Integer organizationId,
                                                     @Param("companyId") Integer companyId,
                                                     @Param("orderId") Integer orderId,
                                                     @Param("orderDetailIds") String orderDetailIds,
                                                     @Param("userId") Integer userId);

    /** USP_DeletePurchaseOrderDetailIfNotExistInGrn: GRN exists check — DocNo of the latest GRN referencing any of the detail ids (take first; empty list = none). */
    @Query(value = "SELECT TOP 1 h.DocNo FROM InvGrnDetail d WITH (NOLOCK) "
         + "INNER JOIN InvGrn h WITH (NOLOCK) ON h.Id = d.InvGrnId "
         + "WHERE d.PurchaseOrderID = :orderId "
         + "AND EXISTS (SELECT 1 FROM dbo.fnSplitString(:orderDetailIds, ',') s WHERE s.data = d.PurchaseOrderDetailId) "
         + "ORDER BY h.DocDate DESC", nativeQuery = true)
    List<Integer> findLatestGrnDocNoReferencingDetails(@Param("orderId") Integer orderId,
                                                       @Param("orderDetailIds") String orderDetailIds);

    /** USP_DeletePurchaseOrderDetailIfNotExistInGrn: INVOICE exists check — DocNo of the latest Purchase Invoice referencing any of the detail ids (take first; empty list = none). */
    @Query(value = "SELECT TOP 1 h.DocNo FROM InvPurchaseInvoiceDetail d WITH (NOLOCK) "
         + "INNER JOIN InvPurchaseInvoice h WITH (NOLOCK) ON h.Id = d.InvPurchaseInvoiceId "
         + "WHERE d.PurchaseOrderID = :orderId "
         + "AND EXISTS (SELECT 1 FROM dbo.fnSplitString(:orderDetailIds, ',') s WHERE s.data = d.PurchaseOrderDetailId) "
         + "ORDER BY h.DocDate DESC", nativeQuery = true)
    List<Integer> findLatestInvoiceDocNoReferencingDetails(@Param("orderId") Integer orderId,
                                                           @Param("orderDetailIds") String orderDetailIds);

    /** USP_DeletePurchaseOrderDetailIfNotExistInGrn: DELETE FROM PurchaseOrderDetail WHERE PurchaseOrderID = @OrderId AND Id IN (@OrderDetailIds). */
    @Modifying
    @Transactional
    @Query("DELETE FROM PurchaseOrderDetail d WHERE d.purchaseOrderId = :orderId AND d.id IN (:ids)")
    int deleteByPurchaseOrderIdAndIdIn(@Param("orderId") Integer orderId, @Param("ids") List<Integer> ids);
}
