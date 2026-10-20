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

import com.mst.models.InvStockOpeningBalanceHeader;
import com.mst.projections.InvStockOpeningBalanceDropDownRow;
import com.mst.projections.InvStockOpeningBalanceReadAllRow;

/**
 * Sp_InvStockOpeningBalanceHeader_GetAllMethod / _Insert / _Update and
 * USP_GetDataForDropDownFromInvStockOpeningBalanceHeader.
 *
 * Insert proc pre-insert logic (Sp_InvStockOpeningBalanceHeader_Insert):
 *  - validates min/max rate via dbo.CheckMinAndMaxRateOfItems(...,'Stock Opening') and RAISERRORs on failure;
 *  - Id = ISNULL(MAX(CONVERT(int,Id)),0)+1 over the whole table (see maxId());
 *  - DocNo = dbo.GenerateDocumentNumberDynamicly(OrganizationId,CompanyId,FinancialYearId,DocumentTypeId,DocDate,NULL,NULL) (see generateDocNo());
 *  - RAISERROR if FinancialYearId is null/0;
 *  - RAISERROR if DocDate <= dbo.GetReconcilationMaxDate(OrganizationId,CompanyId,ItemId,JobLotId);
 *  - UserLogId defaults to UserAccount.LastUserLogId of EntryUserId when null/0 (see lastUserLogIdOfUser());
 *  - ModifyUserId/ModifyDate/ApprovedUserId/ApprovedDate are NOT written on insert;
 *  - returns SELECT @Id (with save() the caller reads the id from the entity);
 *  - afterwards EXEC USP_UserAudit_Insert (ActionId 1).
 * Update proc (Sp_InvStockOpeningBalanceHeader_Update) additionally refuses when IsApproved=1, checks issued weight
 * (FunGetTotalIssuedStockFromStockEvaluationByDocTypeDocIdSubDocId) and forbids changing Item/JobLot/PackingType/
 * PackUom/Warehouse/Crop/Rate(DocumentTypeId 39) when referenced, then audits with ActionId 2.
 */
@Repository
public interface InvStockOpeningBalanceHeaderRepository extends JpaRepository<InvStockOpeningBalanceHeader, Integer> {

    // ------------------------------------------------------------------ Sp_InvStockOpeningBalanceHeader_GetAllMethod

    /** Sp_InvStockOpeningBalanceHeader_GetAllMethod @Activity='GenerateCode' (scalar function; today's date). */
    @Query(value = "SELECT dbo.GenerateDocumentNumberDynamicly(:organizationId, :companyId, :financialYearId, :documentTypeId, GETDATE(), NULL, NULL) AS DocNo",
           nativeQuery = true)
    Integer generateCode(@Param("organizationId") Integer organizationId,
                         @Param("companyId") Integer companyId,
                         @Param("financialYearId") Integer financialYearId,
                         @Param("documentTypeId") Integer documentTypeId);

    /**
     * Sp_InvStockOpeningBalanceHeader_GetAllMethod @Activity='ReadAll'
     * (native: views V_UomScheduleAndUom / V_ItemCondition, Mfg.LookUps, scalar function, DATEADD).
     * All filter parameters other than organizationId/companyId/documentTypeId are optional (NULL = no filter).
     */
    @Query(value = "SELECT distinct "
         + " OB.Id, OB.DocumentTypeId, OB.TransactionType, OB.DocDate, OB.DocNo, OB.WarehouseId, "
         + " OB.StockCrGLAcId, OB.ItemId, OB.CropYear, OB.JobLotId, OB.ItemUomSch, OB.Qty, "
         + " OB.WeightKgs, OB.ItemRate, OB.RateUomSch, OB.ItemAmount, OB.Remarks, OB.OrganizationId, "
         + " OB.CompanyId, OB.BranchesId, OB.ProjectsId, OB.EntryUserId, OB.EntryDate, "
         + " OB.ModifyUserId, UR.FirstName + ' ' + UR.LastName AS EntryUser, "
         + " OB.ModifyDate, MUR.FirstName + ' ' + MUR.LastName AS ModifyUser, "
         + " OB.IsApproved, OB.ApprovedUserId, OB.ApprovedDate, InvWareHouse.WareHouseName, "
         + " ChartofAccount.AccountTitle, Item.ItemName, "
         + " JobLot.JobLotDescription, JobLot.JobLotCode, RateUOM.Equivalent, RateUOM.UOMCode as RateUom, "
         + " ItemUOM.Equivalent AS ItemUOMEquivqlent, ItemUOM.UOMCode as PackUom, "
         + " Item.ItemName, OB.PackingTypeId, pt.PackTypeDesc, OB.FinancialYearId "
         + " ,[dbo].[FunGetTotalIssuedStockFromStockEvaluationByDocTypeDocIdSubDocId](OB.OrganizationId,OB.CompanyId,OB.DocumentTypeId,OB.Id,OB.Id) AS IssueWeight "
         + " ,(select count(*) from DMSAttachments where RefDocumentTypeId = OB.DocumentTypeId and RefDocumentNo = OB.Id) as NoOfAttachments, "
         + " OB.ItemVariantId, AV.VarientDescription, OB.AttachmentsValues, OB.CustomAttachmentsValues, "
         + " ob.WorkOrderId, ob.BatchNo, ob.ProductionStageId, lp.LookupName as ProductionStage, "
         + " ob.CastingTypeId, ob.BaseDocumentTypeId, Clp.LookupName as CastingType, "
         + " OB.ItemConditionId, IC.ConditionStatus As ItemCondition, OB.RackId, R.rackName, "
         + " OB.SecondaryUomId, SecUom.UomCode as SecondaryUomCode, SecUom.Equivalent SecondaryUomEquivalent, "
         + " OB.SecondaryUomQty, "
         + " CASE when OB.SecondaryUomQty > 0 And OB.Qty > 0 then CAST(OB.SecondaryUomQty as float) / OB.Qty ELSE 0 END as PerItemSecondaryUomQty "
         + " ,OB.SecondaryUomItemRate "
         + "FROM InvStockOpeningBalanceHeader OB "
         + "INNER JOIN Item ON OB.ItemId = Item.Id "
         + "INNER JOIN InvWareHouse ON OB.WarehouseId = InvWareHouse.Id "
         + "INNER JOIN UserAccount UR ON OB.EntryUserId = ur.ID "
         + "LEFT  JOIN UserAccount MUR ON OB.ModifyUserId = MUR.ID "
         + "LEFT  JOIN V_UomScheduleAndUom AS ItemUOM ON OB.ItemUomSch = ItemUOM.Id "
         + "LEFT  JOIN V_UomScheduleAndUom AS RateUOM ON OB.RateUomSch = RateUOM.Id "
         + "LEFT  JOIN JobLot ON OB.JobLotId = JobLot.Id "
         + "LEFT  JOIN ChartofAccount ON OB.StockCrGLAcId = ChartofAccount.Id "
         + "LEFT  JOIN InvPackingType pt on OB.PackingTypeId = pt.Id "
         + "LEFT  JOIN ItemAttributeVarient AV on AV.Id = OB.ItemVariantId "
         + "LEFT  JOIN Mfg.LookUps lp on ob.ProductionStageId = lp.Id "
         + "LEFT  JOIN Mfg.LookUps Clp on ob.CastingTypeId = Clp.Id "
         + "LEFT  JOIN V_ItemCondition IC ON IC.ID = OB.ItemConditionId "
         + "LEFT  JOIN invWarehouseRack R ON OB.RackId = R.Id "
         + "LEFT  JOIN V_UomScheduleAndUom SecUom on OB.SecondaryUomId = SecUom.Id AND OB.ItemId = SecUom.ItemId "
         + "WHERE OB.OrganizationId = :organizationId "
         + " and OB.CompanyId = :companyId "
         + " and OB.DocumentTypeId = :documentTypeId "
         + " and (:financialYearId is null or OB.FinancialYearId = :financialYearId) "
         + " and (:id is null or OB.Id = :id) "
         + " and (:docNo is null or OB.DocNo = :docNo) "
         + " AND (:baseDocumentTypeId is null OR ob.BaseDocumentTypeId = :baseDocumentTypeId) "
         + " AND (:fromDate IS NULL OR OB.DocDate >= :fromDate) "
         + " AND (:toDate IS NULL OR OB.DocDate < DATEADD(DAY,1,:toDate)) "
         + " AND (:fromDocNo IS NULL OR OB.DocNo >= :fromDocNo) "
         + " AND (:toDocNo IS NULL OR OB.DocNo <= :toDocNo) "
         + " and (:accountId is null or OB.StockCrGLAcId = :accountId) "
         + " and (:itemId is null or OB.ItemId = :itemId) "
         + " and (:itemStockAccountId is null or item.PurchaseGLAC = :itemStockAccountId)",
           nativeQuery = true)
    List<InvStockOpeningBalanceReadAllRow> readAll(@Param("organizationId") Integer organizationId,
                                                   @Param("companyId") Integer companyId,
                                                   @Param("documentTypeId") Integer documentTypeId,
                                                   @Param("financialYearId") Integer financialYearId,
                                                   @Param("id") Integer id,
                                                   @Param("docNo") Integer docNo,
                                                   @Param("baseDocumentTypeId") Integer baseDocumentTypeId,
                                                   @Param("fromDate") LocalDate fromDate,
                                                   @Param("toDate") LocalDate toDate,
                                                   @Param("fromDocNo") Integer fromDocNo,
                                                   @Param("toDocNo") Integer toDocNo,
                                                   @Param("accountId") Integer accountId,
                                                   @Param("itemId") Integer itemId,
                                                   @Param("itemStockAccountId") Integer itemStockAccountId);

    /** Sp_InvStockOpeningBalanceHeader_GetAllMethod @Activity='GetPreviousDateById' (take first). */
    @Query("SELECT e.docDate FROM InvStockOpeningBalanceHeader e WHERE e.id = :id")
    List<LocalDate> getPreviousDateById(@Param("id") Integer id);

    // ------------------------------------------------------------------ Sp_InvStockOpeningBalanceHeader_Insert helpers

    /** Sp_InvStockOpeningBalanceHeader_Insert: Id = ISNULL(MAX(CONVERT(int,Id)),0)+1 (whole table). */
    @Query("select coalesce(max(e.id),0) from InvStockOpeningBalanceHeader e")
    Integer maxId();

    /** Sp_InvStockOpeningBalanceHeader_Insert: DocNo = dbo.GenerateDocumentNumberDynamicly(...,@DocDate,NULL,NULL). */
    @Query(value = "SELECT dbo.GenerateDocumentNumberDynamicly(:organizationId, :companyId, :financialYearId, :documentTypeId, :docDate, NULL, NULL) AS DocNo",
           nativeQuery = true)
    Integer generateDocNo(@Param("organizationId") Integer organizationId,
                          @Param("companyId") Integer companyId,
                          @Param("financialYearId") Integer financialYearId,
                          @Param("documentTypeId") Integer documentTypeId,
                          @Param("docDate") LocalDate docDate);

    /** Sp_InvStockOpeningBalanceHeader_Insert/_Update: UserLogId fallback = UserAccount.LastUserLogId of the acting user. */
    @Query(value = "SELECT LastUserLogId FROM UserAccount WHERE ID = :userId", nativeQuery = true)
    Long lastUserLogIdOfUser(@Param("userId") Integer userId);

    /** Sp_InvStockOpeningBalanceHeader_Insert: reconciliation guard date (insert/update refused when DocDate <= this). */
    @Query(value = "SELECT dbo.GetReconcilationMaxDate(:organizationId, :companyId, :itemId, :jobLotId) AS AsOnDate", nativeQuery = true)
    java.util.Date reconciliationMaxDate(@Param("organizationId") Integer organizationId,
                                         @Param("companyId") Integer companyId,
                                         @Param("itemId") Integer itemId,
                                         @Param("jobLotId") Integer jobLotId);

    /**
     * Sp_InvStockOpeningBalanceHeader_Insert (single branch).
     * // KEEP-EXEC: procedural validation (min/max rate function, reconciliation date, RAISERROR), code generation and USP_UserAudit_Insert call.
     * All parameters default to NULL in the proc; @Id and @DocNo are recomputed by the proc and ignored on input.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_InvStockOpeningBalanceHeader_Insert "
         + "@Id = :id, @DocumentTypeId = :documentTypeId, @DocDate = :docDate, @DocNo = :docNo, @TransactionType = :transactionType, "
         + "@WarehouseId = :warehouseId, @StockCrGLAcId = :stockCrGLAcId, @ItemId = :itemId, @CropYear = :cropYear, @JobLotId = :jobLotId, "
         + "@ItemUomSch = :itemUomSch, @Qty = :qty, @WeightKgs = :weightKgs, @ItemRate = :itemRate, @RateUomSch = :rateUomSch, "
         + "@ItemAmount = :itemAmount, @Remarks = :remarks, @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@BranchesId = :branchesId, @ProjectsId = :projectsId, @EntryUserId = :entryUserId, @EntryDate = :entryDate, "
         + "@ModifyUserId = :modifyUserId, @ModifyDate = :modifyDate, @IsApproved = :isApproved, @ApprovedUserId = :approvedUserId, "
         + "@ApprovedDate = :approvedDate, @PackingTypeId = :packingTypeId, @FinancialYearId = :financialYearId, @ItemVariantId = :itemVariantId, "
         + "@WorkOrderId = :workOrderId, @BatchNo = :batchNo, @ProductionStageId = :productionStageId, @AttachmentsValues = :attachmentsValues, "
         + "@CustomAttachmentsValues = :customAttachmentsValues, @CastingTypeId = :castingTypeId, @BaseDocumentTypeId = :baseDocumentTypeId, "
         + "@ItemConditionId = :itemConditionId, @RackId = :rackId, @SecondaryUomId = :secondaryUomId, @SecondaryUomQty = :secondaryUomQty, "
         + "@SecondaryUomItemRate = :secondaryUomItemRate, @UserLogId = :userLogId",
           nativeQuery = true)
    void spInvStockOpeningBalanceHeaderInsert(@Param("id") Integer id,
                                              @Param("documentTypeId") Integer documentTypeId,
                                              @Param("docDate") LocalDate docDate,
                                              @Param("docNo") Integer docNo,
                                              @Param("transactionType") String transactionType,
                                              @Param("warehouseId") Integer warehouseId,
                                              @Param("stockCrGLAcId") Integer stockCrGLAcId,
                                              @Param("itemId") Integer itemId,
                                              @Param("cropYear") String cropYear,
                                              @Param("jobLotId") Integer jobLotId,
                                              @Param("itemUomSch") Integer itemUomSch,
                                              @Param("qty") Double qty,
                                              @Param("weightKgs") Double weightKgs,
                                              @Param("itemRate") Double itemRate,
                                              @Param("rateUomSch") Integer rateUomSch,
                                              @Param("itemAmount") Double itemAmount,
                                              @Param("remarks") String remarks,
                                              @Param("organizationId") Integer organizationId,
                                              @Param("companyId") Integer companyId,
                                              @Param("branchesId") Integer branchesId,
                                              @Param("projectsId") Integer projectsId,
                                              @Param("entryUserId") Integer entryUserId,
                                              @Param("entryDate") LocalDateTime entryDate,
                                              @Param("modifyUserId") Integer modifyUserId,
                                              @Param("modifyDate") LocalDateTime modifyDate,
                                              @Param("isApproved") Boolean isApproved,
                                              @Param("approvedUserId") Integer approvedUserId,
                                              @Param("approvedDate") LocalDateTime approvedDate,
                                              @Param("packingTypeId") Integer packingTypeId,
                                              @Param("financialYearId") Integer financialYearId,
                                              @Param("itemVariantId") Integer itemVariantId,
                                              @Param("workOrderId") Integer workOrderId,
                                              @Param("batchNo") String batchNo,
                                              @Param("productionStageId") Integer productionStageId,
                                              @Param("attachmentsValues") String attachmentsValues,
                                              @Param("customAttachmentsValues") String customAttachmentsValues,
                                              @Param("castingTypeId") Integer castingTypeId,
                                              @Param("baseDocumentTypeId") Integer baseDocumentTypeId,
                                              @Param("itemConditionId") Integer itemConditionId,
                                              @Param("rackId") Integer rackId,
                                              @Param("secondaryUomId") Integer secondaryUomId,
                                              @Param("secondaryUomQty") BigDecimal secondaryUomQty,
                                              @Param("secondaryUomItemRate") BigDecimal secondaryUomItemRate,
                                              @Param("userLogId") Long userLogId);

    // ------------------------------------------------------------------ Sp_InvStockOpeningBalanceHeader_Update

    /**
     * Sp_InvStockOpeningBalanceHeader_Update (single branch).
     * // KEEP-EXEC: approval guard, min/max rate validation, referenced-balance validation, reconciliation date and USP_UserAudit_Insert call.
     * All parameters default to NULL in the proc.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_InvStockOpeningBalanceHeader_Update "
         + "@Id = :id, @DocumentTypeId = :documentTypeId, @DocDate = :docDate, @DocNo = :docNo, @TransactionType = :transactionType, "
         + "@WarehouseId = :warehouseId, @StockCrGLAcId = :stockCrGLAcId, @ItemId = :itemId, @CropYear = :cropYear, @JobLotId = :jobLotId, "
         + "@ItemUomSch = :itemUomSch, @Qty = :qty, @WeightKgs = :weightKgs, @ItemRate = :itemRate, @RateUomSch = :rateUomSch, "
         + "@ItemAmount = :itemAmount, @Remarks = :remarks, @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@BranchesId = :branchesId, @ProjectsId = :projectsId, @EntryUserId = :entryUserId, @EntryDate = :entryDate, "
         + "@ModifyUserId = :modifyUserId, @ModifyDate = :modifyDate, @IsApproved = :isApproved, @ApprovedUserId = :approvedUserId, "
         + "@ApprovedDate = :approvedDate, @PackingTypeId = :packingTypeId, @FinancialYearId = :financialYearId, @ItemVariantId = :itemVariantId, "
         + "@AttachmentsValues = :attachmentsValues, @CustomAttachmentsValues = :customAttachmentsValues, @WorkOrderId = :workOrderId, "
         + "@BatchNo = :batchNo, @ProductionStageId = :productionStageId, @CastingTypeId = :castingTypeId, @BaseDocumentTypeId = :baseDocumentTypeId, "
         + "@ItemConditionId = :itemConditionId, @RackId = :rackId, @SecondaryUomId = :secondaryUomId, @SecondaryUomQty = :secondaryUomQty, "
         + "@SecondaryUomItemRate = :secondaryUomItemRate, @UserLogId = :userLogId",
           nativeQuery = true)
    void spInvStockOpeningBalanceHeaderUpdate(@Param("id") Integer id,
                                              @Param("documentTypeId") Integer documentTypeId,
                                              @Param("docDate") LocalDate docDate,
                                              @Param("docNo") Integer docNo,
                                              @Param("transactionType") String transactionType,
                                              @Param("warehouseId") Integer warehouseId,
                                              @Param("stockCrGLAcId") Integer stockCrGLAcId,
                                              @Param("itemId") Integer itemId,
                                              @Param("cropYear") String cropYear,
                                              @Param("jobLotId") Integer jobLotId,
                                              @Param("itemUomSch") Integer itemUomSch,
                                              @Param("qty") Double qty,
                                              @Param("weightKgs") Double weightKgs,
                                              @Param("itemRate") Double itemRate,
                                              @Param("rateUomSch") Integer rateUomSch,
                                              @Param("itemAmount") Double itemAmount,
                                              @Param("remarks") String remarks,
                                              @Param("organizationId") Integer organizationId,
                                              @Param("companyId") Integer companyId,
                                              @Param("branchesId") Integer branchesId,
                                              @Param("projectsId") Integer projectsId,
                                              @Param("entryUserId") Integer entryUserId,
                                              @Param("entryDate") LocalDateTime entryDate,
                                              @Param("modifyUserId") Integer modifyUserId,
                                              @Param("modifyDate") LocalDateTime modifyDate,
                                              @Param("isApproved") Boolean isApproved,
                                              @Param("approvedUserId") Integer approvedUserId,
                                              @Param("approvedDate") LocalDateTime approvedDate,
                                              @Param("packingTypeId") Integer packingTypeId,
                                              @Param("financialYearId") Integer financialYearId,
                                              @Param("itemVariantId") Integer itemVariantId,
                                              @Param("attachmentsValues") String attachmentsValues,
                                              @Param("customAttachmentsValues") String customAttachmentsValues,
                                              @Param("workOrderId") Integer workOrderId,
                                              @Param("batchNo") String batchNo,
                                              @Param("productionStageId") Integer productionStageId,
                                              @Param("castingTypeId") Integer castingTypeId,
                                              @Param("baseDocumentTypeId") Integer baseDocumentTypeId,
                                              @Param("itemConditionId") Integer itemConditionId,
                                              @Param("rackId") Integer rackId,
                                              @Param("secondaryUomId") Integer secondaryUomId,
                                              @Param("secondaryUomQty") BigDecimal secondaryUomQty,
                                              @Param("secondaryUomItemRate") BigDecimal secondaryUomItemRate,
                                              @Param("userLogId") Long userLogId);

    /**
     * Sp_InvStockOpeningBalanceHeader_Update: the single UPDATE statement of the proc (every SET column; EntryUserId/EntryDate/
     * IsApproved/ApprovedUserId/ApprovedDate are intentionally not updated). Validation lives in the proc / caller.
     */
    @Modifying
    @Transactional
    @Query("UPDATE InvStockOpeningBalanceHeader t SET t.documentTypeId = :documentTypeId, t.docDate = :docDate, t.docNo = :docNo, "
         + "t.transactionType = :transactionType, t.warehouseId = :warehouseId, t.stockCrGLAcId = :stockCrGLAcId, t.itemId = :itemId, "
         + "t.cropYear = :cropYear, t.jobLotId = :jobLotId, t.itemUomSch = :itemUomSch, t.qty = :qty, t.weightKgs = :weightKgs, "
         + "t.itemRate = :itemRate, t.rateUomSch = :rateUomSch, t.itemAmount = :itemAmount, t.remarks = :remarks, "
         + "t.organizationId = :organizationId, t.companyId = :companyId, t.branchesId = :branchesId, t.projectsId = :projectsId, "
         + "t.modifyUserId = :modifyUserId, t.modifyDate = :modifyDate, t.packingTypeId = :packingTypeId, t.financialYearId = :financialYearId, "
         + "t.itemVariantId = :itemVariantId, t.attachmentsValues = :attachmentsValues, t.customAttachmentsValues = :customAttachmentsValues, "
         + "t.workOrderId = :workOrderId, t.batchNo = :batchNo, t.productionStageId = :productionStageId, t.castingTypeId = :castingTypeId, "
         + "t.baseDocumentTypeId = :baseDocumentTypeId, t.itemConditionId = :itemConditionId, t.rackId = :rackId, "
         + "t.secondaryUomId = :secondaryUomId, t.secondaryUomQty = :secondaryUomQty, t.secondaryUomItemRate = :secondaryUomItemRate, "
         + "t.userLogId = :userLogId "
         + "WHERE t.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("documentTypeId") Integer documentTypeId,
                   @Param("docDate") LocalDate docDate,
                   @Param("docNo") Integer docNo,
                   @Param("transactionType") String transactionType,
                   @Param("warehouseId") Integer warehouseId,
                   @Param("stockCrGLAcId") Integer stockCrGLAcId,
                   @Param("itemId") Integer itemId,
                   @Param("cropYear") String cropYear,
                   @Param("jobLotId") Integer jobLotId,
                   @Param("itemUomSch") Integer itemUomSch,
                   @Param("qty") Double qty,
                   @Param("weightKgs") Double weightKgs,
                   @Param("itemRate") Double itemRate,
                   @Param("rateUomSch") Integer rateUomSch,
                   @Param("itemAmount") Double itemAmount,
                   @Param("remarks") String remarks,
                   @Param("organizationId") Integer organizationId,
                   @Param("companyId") Integer companyId,
                   @Param("branchesId") Integer branchesId,
                   @Param("projectsId") Integer projectsId,
                   @Param("modifyUserId") Integer modifyUserId,
                   @Param("modifyDate") LocalDateTime modifyDate,
                   @Param("packingTypeId") Integer packingTypeId,
                   @Param("financialYearId") Integer financialYearId,
                   @Param("itemVariantId") Integer itemVariantId,
                   @Param("attachmentsValues") String attachmentsValues,
                   @Param("customAttachmentsValues") String customAttachmentsValues,
                   @Param("workOrderId") Integer workOrderId,
                   @Param("batchNo") String batchNo,
                   @Param("productionStageId") Integer productionStageId,
                   @Param("castingTypeId") Integer castingTypeId,
                   @Param("baseDocumentTypeId") Integer baseDocumentTypeId,
                   @Param("itemConditionId") Integer itemConditionId,
                   @Param("rackId") Integer rackId,
                   @Param("secondaryUomId") Integer secondaryUomId,
                   @Param("secondaryUomQty") BigDecimal secondaryUomQty,
                   @Param("secondaryUomItemRate") BigDecimal secondaryUomItemRate,
                   @Param("userLogId") Long userLogId);

    /** Sp_InvStockOpeningBalanceHeader_Update: issued weight check source (FunGetTotalIssuedStockFromStockEvaluationByDocTypeDocIdSubDocId). */
    @Query(value = "SELECT [dbo].[FunGetTotalIssuedStockFromStockEvaluationByDocTypeDocIdSubDocId](:organizationId, :companyId, :documentTypeId, :id, :id) AS IssuedWeight",
           nativeQuery = true)
    BigDecimal issuedWeight(@Param("organizationId") Integer organizationId,
                            @Param("companyId") Integer companyId,
                            @Param("documentTypeId") Integer documentTypeId,
                            @Param("id") Integer id);

    // ------------------------------------------------------------------ USP_GetDataForDropDownFromInvStockOpeningBalanceHeader

    /**
     * USP_GetDataForDropDownFromInvStockOpeningBalanceHeader (single branch; the #MainTable / #tmpResult temp-table pipeline
     * is rewritten as one CTE + UNION ALL statement). @DocumentTypeId and @Activity default to NULL (NULL activity = all groups).
     * Activity values: StockAccount, PackingType, Warehouse, CastingType, Item, TypeOfType, ItemTypes, ItemCategories,
     * ParentCategories, ItemClassGroup, ItemStockAccount.
     */
    @Query(value = "WITH MainTable AS ( "
         + " Select distinct h.StockCrGLAcId, h.PackingTypeId, h.WarehouseId, h.CastingTypeId, h.TransactionType, h.ItemId "
         + " from InvStockOpeningBalanceHeader h "
         + " where h.OrganizationId = :organizationId and h.CompanyId = :companyId "
         + " and (:documentTypeId is null or h.DocumentTypeId = :documentTypeId) ) "
         + "SELECT r.Id, r.ReferenceName, r.Activity FROM ( "
         + " select distinct h.StockCrGLAcId AS Id, i.AccountTitle AS ReferenceName, 'StockAccount' AS Activity "
         + " from ChartofAccount i INNER JOIN MainTable h on h.StockCrGLAcId = i.Id "
         + " where (:activity IS NULL OR 'StockAccount' = :activity) "
         + " UNION ALL "
         + " select distinct h.PackingTypeId, i.PackTypeDesc, 'PackingType' "
         + " from InvPackingType i INNER JOIN MainTable h on h.PackingTypeId = i.Id "
         + " where (:activity IS NULL OR 'PackingType' = :activity) "
         + " UNION ALL "
         + " select distinct h.WarehouseId, i.WareHouseName, 'Warehouse' "
         + " from InvWareHouse i INNER JOIN MainTable h on h.WarehouseId = i.Id "
         + " where (:activity IS NULL OR 'Warehouse' = :activity) "
         + " UNION ALL "
         + " select distinct h.CastingTypeId, i.CastingType, 'CastingType' "
         + " from mfg.vCastingType i INNER JOIN MainTable h on h.CastingTypeId = i.CastingTypeId "
         + " where (:activity IS NULL OR 'CastingType' = :activity) "
         + " UNION ALL "
         + " select distinct h.ItemId, i.ItemName, 'Item' "
         + " from Item i INNER JOIN MainTable h on h.ItemId = i.Id "
         + " where (:activity IS NULL OR 'Item' = :activity) "
         + " UNION ALL "
         + " select distinct il.Id, il.LookupName, 'TypeOfType' "
         + " from MainTable d inner join Item i on d.ItemId = i.Id "
         + " inner join ItemCategory ic on i.ItemCategoryId = ic.id "
         + " inner join ItemType it on i.ItemTypeId = it.Id "
         + " inner join InvLookUp il on il.Id = it.Type "
         + " Where (:activity IS NULL OR 'TypeOfType' = :activity) "
         + " UNION ALL "
         + " select distinct it.Id, it.TypeDescription, 'ItemTypes' "
         + " from MainTable d inner join Item i on d.ItemId = i.Id "
         + " inner join ItemCategory ic on i.ItemCategoryId = ic.id "
         + " inner join ItemType it on i.ItemTypeId = it.Id "
         + " Where (:activity IS NULL OR 'ItemTypes' = :activity) "
         + " UNION ALL "
         + " select distinct ic.Id, ic.CategoryDescription, 'ItemCategories' "
         + " from MainTable d inner join Item i on d.ItemId = i.Id "
         + " inner join ItemCategory ic on i.ItemCategoryId = ic.id "
         + " Where (:activity IS NULL OR 'ItemCategories' = :activity) "
         + " UNION ALL "
         + " select distinct ip.Id, ip.InvParentCateDescription, 'ParentCategories' "
         + " from MainTable d inner join Item i on d.ItemId = i.Id "
         + " inner join ItemCategory ic on i.ItemCategoryId = ic.id "
         + " inner join InventoryParentCategories ip on ic.InventoryParentCategoriesId = ip.Id "
         + " Where (:activity IS NULL OR 'ParentCategories' = :activity) "
         + " UNION ALL "
         + " select distinct cg.Id, cg.ClassGroupName, 'ItemClassGroup' "
         + " from MainTable d inner join Item i on d.ItemId = i.Id "
         + " inner join ItemCategory ic on i.ItemCategoryId = ic.id "
         + " inner join ItemClassGroup cg on ic.ItemClassGroupId = cg.Id "
         + " Where (:activity IS NULL OR 'ItemClassGroup' = :activity) "
         + " UNION ALL "
         + " select distinct ic.Id, ic.AccountTitle, 'ItemStockAccount' "
         + " from MainTable d inner join Item i on d.ItemId = i.Id "
         + " inner join ChartofAccount ic on i.PurchaseGLAC = ic.id "
         + " Where (:activity IS NULL OR 'ItemStockAccount' = :activity) "
         + ") r",
           nativeQuery = true)
    List<InvStockOpeningBalanceDropDownRow> getDataForDropDown(@Param("organizationId") Integer organizationId,
                                                               @Param("companyId") Integer companyId,
                                                               @Param("documentTypeId") Integer documentTypeId,
                                                               @Param("activity") String activity);
}
