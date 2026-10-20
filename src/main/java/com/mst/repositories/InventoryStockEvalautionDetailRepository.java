package com.mst.repositories;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InventoryStockEvalautionDetail;

/**
 * Repository for dbo.InventoryStockEvalautionDetail.
 * <p>
 * Source procedure: <b>Sp_InventoryStockEvalautionDetail_Update</b> (single branch, 3832 lines).
 * The procedure is a procedural, multi-table "rebuild" of the stock-evaluation rows of ONE source document:
 * <ol>
 *   <li>DELETE the existing rows for (RefDocumentTypeId, RefDocIdNo, OrganizationId, CompanyId)
 *       (only rows with QtyIn &gt; 0 when RefDocumentTypeId in (1850,1867) and ERP feature 5 is enabled);</li>
 *   <li>reads config 56 "ContractWagesChargetoProduct" (used by all purchase-invoice sections);</li>
 *   <li>then runs 101 {@code INSERT INTO InventoryStockEvalautionDetail SELECT ... FROM <source doc tables>}
 *       statements (+1 into StockEvalautionForPartyReserveStocks) spread over 86 {@code if @RefDocumentTypeId = / in (...)}
 *       blocks; the source tables (InvSaleInvoice,
 *       InvGdn, InvStockConversion, InvFoodProduction, ST.*, pcc.*, Mfg.*, fed.*, CmTr.* ...) mostly have no JPA
 *       entity and the SELECTs use temp tables (#TransferTo/#TransferFrom/#ItemLatestRate), UNION ALL,
 *       ROW_NUMBER(), scalar UDFs (GetOnlyAvgRateForCGS, GetEquivalentByItemIdandBaseUnitId ...) and RAISERROR.</li>
 * </ol>
 * Behaviour must stay identical, so the whole rebuild is exposed as a KEEP-EXEC native call
 * ({@link #spInventoryStockEvalautionDetailUpdate}) and only the standalone single-table statements are
 * re-expressed as JPQL. See OUT/MAP_J1_stock_eval_update.md for the per-document-type section list.
 * <p>
 * Of the 70 declared parameters the procedure body actually reads only
 * {@code @RefDocumentTypeId, @RefDocIdNo, @OrganizationId, @CompanyId}; every other parameter is accepted
 * and ignored (all default to NULL). {@link #rebuildForDocument} is the 4-parameter convenience form.
 */
@Repository
public interface InventoryStockEvalautionDetailRepository extends JpaRepository<InventoryStockEvalautionDetail, Integer> {

    // ------------------------------------------------------------------------------------------------
    // KEEP-EXEC: full procedure, every declared parameter exposed (all proc defaults are NULL)
    // ------------------------------------------------------------------------------------------------

    /**
     * Sp_InventoryStockEvalautionDetail_Update (single branch) — KEEP-EXEC: procedural multi-table rebuild
     * (delete + 102 conditional INSERT ... SELECT into InventoryStockEvalautionDetail /
     * StockEvalautionForPartyReserveStocks, temp tables, UNION ALL, ROW_NUMBER, scalar UDFs, RAISERROR).
     * All 70 procedure parameters are exposed; every one defaults to NULL in the proc, so callers may pass null
     * for everything except refDocumentTypeId, refDocIdNo, organizationId and companyId (the only ones the body reads).
     * <p>
     * Throws a DataAccessException ("Virtual Warehouse configuration is missing...") for
     * refDocumentTypeId 1856/1861/1855 when configuration 258 is not set — that is the proc's RAISERROR 16.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_InventoryStockEvalautionDetail_Update "
         + "@hId = :hId, @Id = :id, @RefDocumentTypeId = :refDocumentTypeId, @RefDocIdNo = :refDocIdNo, "
         + "@DocCodeNo = :docCodeNo, @DocDate = :docDate, @SupplierCustomerId = :supplierCustomerId, "
         + "@RefDocSubIdNo = :refDocSubIdNo, @OrderNo = :orderNo, @WarehouseId = :warehouseId, "
         + "@PrdJobOrderNo = :prdJobOrderNo, @VehicleNo = :vehicleNo, @GpNoDcNo = :gpNoDcNo, "
         + "@TranRemarks = :tranRemarks, @ItemId = :itemId, @ItemUom = :itemUom, @CropBatch = :cropBatch, "
         + "@JobLotId = :jobLotId, @InvPackingTypeId = :invPackingTypeId, @QtyIn = :qtyIn, @QtyOut = :qtyOut, "
         + "@BillWeightIn = :billWeightIn, @BillWeightOut = :billWeightOut, @StockWeightIn = :stockWeightIn, "
         + "@StockWeightOut = :stockWeightOut, @CalcType = :calcType, @ItemRate = :itemRate, @RateUom = :rateUom, "
         + "@AmountIn = :amountIn, @ExpenseAmountIn = :expenseAmountIn, @AmountOut = :amountOut, @CgsRate = :cgsRate, "
         + "@IsApproved = :isApproved, @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@BranchesId = :branchesId, @ProjectsId = :projectsId, @CityId = :cityId, @LineId = :lineId, "
         + "@RefRefDocumentTypeId = :refRefDocumentTypeId, @RefRefDocIdNo = :refRefDocIdNo, "
         + "@RefRefDocSubIdNo = :refRefDocSubIdNo, @EntryUser = :entryUser, @ModifyUser = :modifyUser, "
         + "@CgsAmount = :cgsAmount, @OtherDocumentTypeId = :otherDocumentTypeId, @OtherDocNoId = :otherDocNoId, "
         + "@OtherSubDocNoId = :otherSubDocNoId, @InvoiceId = :invoiceId, @InvoiceDetailId = :invoiceDetailId, "
         + "@VarientId = :varientId, @RefWarehouseId = :refWarehouseId, @EbPurAgainstWeight = :ebPurAgainstWeight, "
         + "@FreightDeduction = :freightDeduction, @BiltyNo = :biltyNo, @BatchNo = :batchNo, "
         + "@WorkOrderId = :workOrderId, @ProductionStageId = :productionStageId, @WorkStationId = :workStationId, "
         + "@CastingTypeId = :castingTypeId, @GrnGdnId = :grnGdnId, @GrnGdnNo = :grnGdnNo, @GrnGdnDate = :grnGdnDate, "
         + "@GrnGdnDetailId = :grnGdnDetailId, @GPType = :gpType, @GpTypeSrNo = :gpTypeSrNo, "
         + "@ItemConditionId = :itemConditionId, @SecondaryUomId = :secondaryUomId, "
         + "@SecondaryUomQty = :secondaryUomQty, @SecondaryUomItemRate = :secondaryUomItemRate",
         nativeQuery = true)
    void spInventoryStockEvalautionDetailUpdate(
            @Param("hId") Integer hId,
            @Param("id") Integer id,
            @Param("refDocumentTypeId") Integer refDocumentTypeId,
            @Param("refDocIdNo") Integer refDocIdNo,
            @Param("docCodeNo") Integer docCodeNo,
            @Param("docDate") LocalDateTime docDate,
            @Param("supplierCustomerId") Integer supplierCustomerId,
            @Param("refDocSubIdNo") Integer refDocSubIdNo,
            @Param("orderNo") Integer orderNo,
            @Param("warehouseId") Integer warehouseId,
            @Param("prdJobOrderNo") Integer prdJobOrderNo,
            @Param("vehicleNo") String vehicleNo,
            @Param("gpNoDcNo") Integer gpNoDcNo,
            @Param("tranRemarks") String tranRemarks,
            @Param("itemId") Integer itemId,
            @Param("itemUom") Integer itemUom,
            @Param("cropBatch") String cropBatch,
            @Param("jobLotId") Integer jobLotId,
            @Param("invPackingTypeId") Integer invPackingTypeId,
            @Param("qtyIn") Double qtyIn,
            @Param("qtyOut") Double qtyOut,
            @Param("billWeightIn") Double billWeightIn,
            @Param("billWeightOut") Double billWeightOut,
            @Param("stockWeightIn") Double stockWeightIn,
            @Param("stockWeightOut") Double stockWeightOut,
            @Param("calcType") String calcType,
            @Param("itemRate") Double itemRate,
            @Param("rateUom") Integer rateUom,
            @Param("amountIn") Double amountIn,
            @Param("expenseAmountIn") Double expenseAmountIn,
            @Param("amountOut") Double amountOut,
            @Param("cgsRate") Double cgsRate,
            @Param("isApproved") Boolean isApproved,
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("projectsId") Integer projectsId,
            @Param("cityId") Integer cityId,
            @Param("lineId") Integer lineId,
            @Param("refRefDocumentTypeId") Integer refRefDocumentTypeId,
            @Param("refRefDocIdNo") Integer refRefDocIdNo,
            @Param("refRefDocSubIdNo") Integer refRefDocSubIdNo,
            @Param("entryUser") Integer entryUser,
            @Param("modifyUser") Integer modifyUser,
            @Param("cgsAmount") Double cgsAmount,
            @Param("otherDocumentTypeId") Integer otherDocumentTypeId,
            @Param("otherDocNoId") Integer otherDocNoId,
            @Param("otherSubDocNoId") Integer otherSubDocNoId,
            @Param("invoiceId") Integer invoiceId,
            @Param("invoiceDetailId") Integer invoiceDetailId,
            @Param("varientId") Integer varientId,
            @Param("refWarehouseId") Integer refWarehouseId,
            @Param("ebPurAgainstWeight") Double ebPurAgainstWeight,
            @Param("freightDeduction") BigDecimal freightDeduction,
            @Param("biltyNo") String biltyNo,
            @Param("batchNo") String batchNo,
            @Param("workOrderId") Integer workOrderId,
            @Param("productionStageId") Integer productionStageId,
            @Param("workStationId") Integer workStationId,
            @Param("castingTypeId") Integer castingTypeId,
            @Param("grnGdnId") Integer grnGdnId,
            @Param("grnGdnNo") Integer grnGdnNo,
            @Param("grnGdnDate") LocalDateTime grnGdnDate,
            @Param("grnGdnDetailId") Integer grnGdnDetailId,
            @Param("gpType") String gpType,
            @Param("gpTypeSrNo") Integer gpTypeSrNo,
            @Param("itemConditionId") Integer itemConditionId,
            @Param("secondaryUomId") Integer secondaryUomId,
            @Param("secondaryUomQty") BigDecimal secondaryUomQty,
            @Param("secondaryUomItemRate") BigDecimal secondaryUomItemRate);

    /**
     * Sp_InventoryStockEvalautionDetail_Update — KEEP-EXEC convenience form passing only the four parameters
     * the procedure body actually reads; every other parameter keeps its proc default (NULL).
     * Same semantics / same RAISERROR behaviour as {@link #spInventoryStockEvalautionDetailUpdate}.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_InventoryStockEvalautionDetail_Update "
         + "@RefDocumentTypeId = :refDocumentTypeId, @RefDocIdNo = :refDocIdNo, "
         + "@OrganizationId = :organizationId, @CompanyId = :companyId",
         nativeQuery = true)
    void rebuildForDocument(@Param("refDocumentTypeId") Integer refDocumentTypeId,
                            @Param("refDocIdNo") Integer refDocIdNo,
                            @Param("organizationId") Integer organizationId,
                            @Param("companyId") Integer companyId);

    // ------------------------------------------------------------------------------------------------
    // Standalone single-table statements of the procedure (JPQL)
    // ------------------------------------------------------------------------------------------------

    /**
     * Sp_InventoryStockEvalautionDetail_Update, preamble ELSE branch (the normal case):
     * {@code DELETE FROM InventoryStockEvalautionDetail WHERE RefDocumentTypeId=@RefDocumentTypeId AND RefDocIdNo=@RefDocIdNo
     * AND OrganizationId=@OrganizationId AND CompanyId=@CompanyId}. Returns the number of rows removed.
     */
    @Modifying
    @Transactional
    @Query("DELETE FROM InventoryStockEvalautionDetail d "
         + "WHERE d.refDocumentTypeId = :refDocumentTypeId AND d.refDocIdNo = :refDocIdNo "
         + "AND d.organizationId = :organizationId AND d.companyId = :companyId")
    int deleteByRefDocument(@Param("refDocumentTypeId") Integer refDocumentTypeId,
                            @Param("refDocIdNo") Integer refDocIdNo,
                            @Param("organizationId") Integer organizationId,
                            @Param("companyId") Integer companyId);

    /**
     * Sp_InventoryStockEvalautionDetail_Update, preamble IF branch: when RefDocumentTypeId in (1850,1867)
     * (production / conversion concrete) AND dbo.fnCheckERPFeaturesExistorNot(org,company,5) = 1, only the
     * OUTPUT rows are removed: {@code DELETE ... WHERE ... AND QtyIn > 0}. Returns the number of rows removed.
     */
    @Modifying
    @Transactional
    @Query("DELETE FROM InventoryStockEvalautionDetail d "
         + "WHERE d.refDocumentTypeId = :refDocumentTypeId AND d.refDocIdNo = :refDocIdNo "
         + "AND d.organizationId = :organizationId AND d.companyId = :companyId AND d.qtyIn > 0")
    int deleteOutputRowsByRefDocument(@Param("refDocumentTypeId") Integer refDocumentTypeId,
                                      @Param("refDocIdNo") Integer refDocIdNo,
                                      @Param("organizationId") Integer organizationId,
                                      @Param("companyId") Integer companyId);

    /** Rows the procedure (re)built for one source document — handy for verifying a rebuild. Not a proc branch. */
    List<InventoryStockEvalautionDetail> findByRefDocumentTypeIdAndRefDocIdNoAndOrganizationIdAndCompanyId(
            Integer refDocumentTypeId, Integer refDocIdNo, Integer organizationId, Integer companyId);

    // ------------------------------------------------------------------------------------------------
    // Scalar helpers the procedure evaluates before branching (native: UDFs / tables without entity)
    // ------------------------------------------------------------------------------------------------

    /**
     * Sp_InventoryStockEvalautionDetail_Update preamble: {@code SET @FeatureFlag = dbo.fnCheckERPFeaturesExistorNot(@OrganizationId,@CompanyId,5)}
     * — decides which of the two DELETE forms applies for doc types 1850/1867 and whether the pcc.* INPUT rows
     * (sections 1850 / 1867) are inserted at all ({@code ISNULL(@FeatureFlag,0) = 0}).
     */
    @Query(value = "SELECT CAST(dbo.fnCheckERPFeaturesExistorNot(:organizationId, :companyId, 5) AS bit)", nativeQuery = true)
    Boolean erpFeature5Enabled(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /**
     * Sp_InventoryStockEvalautionDetail_Update preamble: {@code SET @ContractWagesChargetoProduct = dbo.GetConfigurationByConfigId(@OrganizationId,@CompanyId,56)}
     * — when 1, every purchase-invoice section adds InvPurchaseInvoiceDetail.WagesAmount to ExpenseAmountIn / WagesAmount.
     */
    @Query(value = "SELECT CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 56) AS bit)", nativeQuery = true)
    Boolean contractWagesChargeToProduct(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /**
     * Sp_InventoryStockEvalautionDetail_Update sections 1856/1861 and 1855:
     * {@code SET @VirtualWareHouseId = dbo.GetConfigurationByConfigId(@OrganizationId,@CompanyId,258)}; the proc
     * RAISERRORs "Virtual Warehouse configuration is missing" when this is NULL/0.
     */
    @Query(value = "SELECT CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 258) AS int)", nativeQuery = true)
    Integer virtualWarehouseId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /**
     * Sp_InventoryStockEvalautionDetail_Update section RefDocumentTypeId = 99 (SaleInvoiceDirect):
     * {@code SELECT @IsReserve = IsReserve FROM InvSaleInvoice WHERE Id = @RefDocIdNo}
     * (InvSaleInvoice has no entity -> native). When 1 the proc also fills StockEvalautionForPartyReserveStocks.
     */
    @Query(value = "SELECT IsReserve FROM InvSaleInvoice WHERE Id = :refDocIdNo", nativeQuery = true)
    Boolean isReserveSaleInvoice(@Param("refDocIdNo") Integer refDocIdNo);
}
