package com.mst.repositories;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InventoryTransactions;

/**
 * Spring Data JPA repository for dbo.InventoryTransactions, generated from
 * Sp_InventoryTransactions_GetALLMethod (single branch, 2893 lines).
 * Named InventoryTransactionsJpaRepository because the project already has a JdbcTemplate based
 * InventoryTransactionsRepository.
 *
 * Procedure behaviour (every call): DELETE every InventoryTransactions row of
 * (OrganizationId, CompanyId, RefDocumentTypeId, RefDocIdNo), then rebuild them from the source document
 * through one INSERT ... SELECT per document type (106 sections, selected by @RefDocumentTypeId).
 * Exact behaviour is kept by {@link #spInventoryTransactionsGetAllMethod} (KEEP-EXEC).
 * Each INSERT ... SELECT is also exposed verbatim as its own @Modifying native method, plus the
 * scalar helpers the proc computes before some sections (existsGrnPestHold, readConfigWarehouseId,
 * readVirtualWarehouseId) and the standalone UPDATE InvGrn / DELETE statements.
 *
 * All proc parameters default to NULL; the sections only use @OrganizationId, @CompanyId,
 * @RefDocIdNo and @RefDocumentTypeId (plus the computed @IsOnHold, @WhId, @VirtualWareHouseId).
 * The other 40 parameters are accepted by the proc but never used by its body.
 */
@Repository
public interface InventoryTransactionsJpaRepository extends JpaRepository<InventoryTransactions, Integer> {

    // ------------------------------------------------------------------------------------------
    // Whole procedure (behaviour preserved)
    // ------------------------------------------------------------------------------------------

    /**
     * Sp_InventoryTransactions_GetALLMethod (single branch) -- KEEP-EXEC: 106 conditional INSERT ... SELECT
     * sections, conditional UPDATE InvGrn, scalar function/config lookups and a RAISERROR
     * ("Virtual Warehouse configuration is missing") for @RefDocumentTypeId IN (1855,1866).
     * Proc defaults: @Id int = null, @OrganizationId INT = null, @CompanyId INT = null, @BranchesId INT = null, @ProjectsId INT = null, @RefDocumentTypeId INT = null, @RefDocIdNo INT = null, @DocCodeNo INT = null, @DocDate DATETIME = null, @SupplierCustomerId INT = null, @IsApproved BIT = null, @RefDocSubIdNo INT = null, @WarehouseId INT = null, @JobLotId INT = null, @ItemId INT = null, @ItemUom INT = null, @CropBatch NVARCHAR(50) = null, @InvPackingTypeId INT = null, @QtyIn FLOAT = null, @QtyOut FLOAT = null, @BillWeightIn FLOAT = null, @BillWeightOut FLOAT = null, @StockWeightIn FLOAT = null, @StockWeightOut FLOAT = null, @CalcType NVARCHAR(50) = null, @ItemRate FLOAT = null, @RateUom INT = null, @AmountIn FLOAT = null, @ExpenseAmountIn FLOAT = null, @AmountOut FLOAT = null, @TranRemarks NVARCHAR(max) = null, @EntryDate DATETIME = null, @EntryUserId int = null, @ModifyDate DATETIME = null, @ModifyUserId int = null, @GrossWeight FLOAT = null, @EbUnit FLOAT = null, @EbTotal FLOAT = null, @WtCut FLOAT = null, @WtCutTotal FLOAT = null, @AddLess FLOAT = null, @GpNo int = null, @VehicleNo NVARCHAR(500) = null, @ItemConditionId int = null, @SecondaryUomId int = null, @SecondaryUomQty DECIMAL(18,2) = null, @SecondaryUomItemRate DECIMAL(18,2) = null.
     * Only @OrganizationId, @CompanyId, @RefDocumentTypeId and @RefDocIdNo drive the body; pass null for the rest.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_InventoryTransactions_GetALLMethod"
         + "     @Id = :id,"
         + "     @OrganizationId = :organizationId,"
         + "     @CompanyId = :companyId,"
         + "     @BranchesId = :branchesId,"
         + "     @ProjectsId = :projectsId,"
         + "     @RefDocumentTypeId = :refDocumentTypeId,"
         + "     @RefDocIdNo = :refDocIdNo,"
         + "     @DocCodeNo = :docCodeNo,"
         + "     @DocDate = :docDate,"
         + "     @SupplierCustomerId = :supplierCustomerId,"
         + "     @IsApproved = :isApproved,"
         + "     @RefDocSubIdNo = :refDocSubIdNo,"
         + "     @WarehouseId = :warehouseId,"
         + "     @JobLotId = :jobLotId,"
         + "     @ItemId = :itemId,"
         + "     @ItemUom = :itemUom,"
         + "     @CropBatch = :cropBatch,"
         + "     @InvPackingTypeId = :invPackingTypeId,"
         + "     @QtyIn = :qtyIn,"
         + "     @QtyOut = :qtyOut,"
         + "     @BillWeightIn = :billWeightIn,"
         + "     @BillWeightOut = :billWeightOut,"
         + "     @StockWeightIn = :stockWeightIn,"
         + "     @StockWeightOut = :stockWeightOut,"
         + "     @CalcType = :calcType,"
         + "     @ItemRate = :itemRate,"
         + "     @RateUom = :rateUom,"
         + "     @AmountIn = :amountIn,"
         + "     @ExpenseAmountIn = :expenseAmountIn,"
         + "     @AmountOut = :amountOut,"
         + "     @TranRemarks = :tranRemarks,"
         + "     @EntryDate = :entryDate,"
         + "     @EntryUserId = :entryUserId,"
         + "     @ModifyDate = :modifyDate,"
         + "     @ModifyUserId = :modifyUserId,"
         + "     @GrossWeight = :grossWeight,"
         + "     @EbUnit = :ebUnit,"
         + "     @EbTotal = :ebTotal,"
         + "     @WtCut = :wtCut,"
         + "     @WtCutTotal = :wtCutTotal,"
         + "     @AddLess = :addLess,"
         + "     @GpNo = :gpNo,"
         + "     @VehicleNo = :vehicleNo,"
         + "     @ItemConditionId = :itemConditionId,"
         + "     @SecondaryUomId = :secondaryUomId,"
         + "     @SecondaryUomQty = :secondaryUomQty,"
         + "     @SecondaryUomItemRate = :secondaryUomItemRate", nativeQuery = true)
    void spInventoryTransactionsGetAllMethod(
            @Param("id") Integer id,
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("projectsId") Integer projectsId,
            @Param("refDocumentTypeId") Integer refDocumentTypeId,
            @Param("refDocIdNo") Integer refDocIdNo,
            @Param("docCodeNo") Integer docCodeNo,
            @Param("docDate") LocalDateTime docDate,
            @Param("supplierCustomerId") Integer supplierCustomerId,
            @Param("isApproved") Boolean isApproved,
            @Param("refDocSubIdNo") Integer refDocSubIdNo,
            @Param("warehouseId") Integer warehouseId,
            @Param("jobLotId") Integer jobLotId,
            @Param("itemId") Integer itemId,
            @Param("itemUom") Integer itemUom,
            @Param("cropBatch") String cropBatch,
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
            @Param("tranRemarks") String tranRemarks,
            @Param("entryDate") LocalDateTime entryDate,
            @Param("entryUserId") Integer entryUserId,
            @Param("modifyDate") LocalDateTime modifyDate,
            @Param("modifyUserId") Integer modifyUserId,
            @Param("grossWeight") Double grossWeight,
            @Param("ebUnit") Double ebUnit,
            @Param("ebTotal") Double ebTotal,
            @Param("wtCut") Double wtCut,
            @Param("wtCutTotal") Double wtCutTotal,
            @Param("addLess") Double addLess,
            @Param("gpNo") Integer gpNo,
            @Param("vehicleNo") String vehicleNo,
            @Param("itemConditionId") Integer itemConditionId,
            @Param("secondaryUomId") Integer secondaryUomId,
            @Param("secondaryUomQty") BigDecimal secondaryUomQty,
            @Param("secondaryUomItemRate") BigDecimal secondaryUomItemRate);

    // ------------------------------------------------------------------------------------------
    // Standalone statements of the procedure
    // ------------------------------------------------------------------------------------------

    /** Sp_InventoryTransactions_GetALLMethod line 58: first statement of every call. */
    @Modifying
    @Transactional
    @Query("DELETE FROM InventoryTransactions it WHERE it.organizationId = :organizationId AND it.companyId = :companyId "
         + "AND it.refDocumentTypeId = :refDocumentTypeId AND it.refDocIdNo = :refDocIdNo")
    int deleteByRefDocument(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
                            @Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("refDocIdNo") Integer refDocIdNo);

    /**
     * Sp_InventoryTransactions_GetALLMethod lines 98-123 (@RefDocumentTypeId IN (46,165,167,169,143,217)):
     * computes @IsOnHold = 1 when feature 24 is active for the company and the GRN's inward gate pass has an
     * approved/accepted lab analysis with PestStatusId = 1. Returns 1 or 0 (ERPConfigurations /
     * InvLabAnalysisPurchaseHeader have no entity -> native).
     */
    @Query(value = "SELECT CASE WHEN EXISTS ("
         + " SELECT 1 FROM ERPConfigurations C WHERE C.CompanyId = :companyId AND C.FeaturesId = 24 AND ISNULL(C.IsActive, 0) = 1"
         + " AND EXISTS (SELECT 1 FROM InvGrn H INNER JOIN InvLabAnalysisPurchaseHeader L ON H.InwardGatePassId = L.GatePassInwardId"
         + " WHERE H.CompanyId = :companyId AND H.DocumentTypeId = :refDocumentTypeId AND H.Id = :refDocIdNo AND L.PestStatusId = 1"
         + " AND ((L.IsApproved = 1 AND L.IsAcceptedApproved = 1) OR L.IsAccepted = 1))"
         + ") THEN 1 ELSE 0 END", nativeQuery = true)
    Integer existsGrnPestHold(@Param("companyId") Integer companyId, @Param("refDocumentTypeId") Integer refDocumentTypeId,
                              @Param("refDocIdNo") Integer refDocIdNo);

    /**
     * Sp_InventoryTransactions_GetALLMethod lines 167-172 (@RefDocumentTypeId IN (46,165,167,169,143,217)):
     * UPDATE InvGrn SET IsOnHold = @IsOnHold WHERE CompanyId = @CompanyId AND DocumentTypeId = @RefDocumentTypeId
     * AND Id = @RefDocIdNo AND ISNULL(IsOnHold,0) <> @IsOnHold. InvGrn has no entity in this project (planned by
     * batch F1_grn_core) -> native.
     */
    @Modifying
    @Transactional
    @Query(value = "UPDATE InvGrn SET IsOnHold = :isOnHold WHERE CompanyId = :companyId AND DocumentTypeId = :refDocumentTypeId "
         + "AND Id = :refDocIdNo AND ISNULL(IsOnHold, 0) <> :isOnHold", nativeQuery = true)
    int updateInvGrnIsOnHold(@Param("isOnHold") Boolean isOnHold, @Param("companyId") Integer companyId,
                             @Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("refDocIdNo") Integer refDocIdNo);

    /**
     * Sp_InventoryTransactions_GetALLMethod line 1185 (@RefDocumentTypeId IN (205,206)):
     * @WhId = ISNULL(ConfigKey,0) FROM ConfigrationsAllocation WHERE ... ConfigrationsDefinitionId = 12 (take first; 0 when none).
     */
    @Query(value = "SELECT TOP 1 ISNULL(ConfigKey, 0) FROM dbo.ConfigrationsAllocation WHERE OrganizationId = :organizationId "
         + "AND CompanyId = :companyId AND ConfigrationsDefinitionId = 12", nativeQuery = true)
    Integer readConfigWarehouseId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /**
     * Sp_InventoryTransactions_GetALLMethod line 2342 (@RefDocumentTypeId IN (1855,1866)):
     * @VirtualWareHouseId = dbo.GetConfigurationByConfigId(@OrganizationId, @CompanyId, 258).
     * The proc RAISERRORs 'Virtual Warehouse configuration is missing.Please Set that in Configuration' and RETURNs
     * when ISNULL(result,0) = 0 -- the caller must do the same before insertPccGdnExtraQtyToParty.
     */
    @Query(value = "SELECT dbo.GetConfigurationByConfigId(:organizationId, :companyId, 258)", nativeQuery = true)
    Integer readVirtualWarehouseId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    // ------------------------------------------------------------------------------------------
    // One method per INSERT ... SELECT section (source tables have no entities -> native, pasted verbatim)
    // ------------------------------------------------------------------------------------------

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (39,40) -- OpeningBalanceRice / OpeningBalanceStore (InvStockOpeningBalanceHeader). Proc line 65. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId"
         + " ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,VarientId,ProductionStageId,WorkOrderId,WorkStationId,CastingTypeId,ItemConditionId,RackId,"
         + " SecondaryUomId,SecondaryUomQty,SecondaryUomItemRate"
         + " )"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " ,0, Grh.IsApproved,GRH.Id, grh.WarehouseId, Grh.JobLotId, Grh.ItemId,Grh.ItemUomSch, Grh.CropYear"
         + " ,Grh.PackingTypeId, Grh.Qty, 0, Grh.WeightKgs, 0, Grh.WeightKgs, 0"
         + " ,CASE WHEN :refDocumentTypeId = 40 THEN 'Weight' ELSE 'Qty' END, 0, 0, 0 , 0, 0"
         + " ,CASE WHEN :refDocumentTypeId = 40 THEN 'OpeningBalanceRice' ELSE 'OpeningBalanceStore' END"
         + " ,grh.EntryDate,grh.EntryUserId ,grh.ModifyDate,grh.ModifyUserId"
         + " ,grh.WeightKgs,0,0,0,0 ,0 ,0,''"
         + " ,grh.ItemVariantId,grh.ProductionStageId,grh.WorkOrderId,0,grh.CastingTypeId"
         + " ,grh.ItemConditionId,Grh.RackId,"
         + " SecondaryUomId,SecondaryUomQty,SecondaryUomItemRate"
         + " FROM InvStockOpeningBalanceHeader Grh"
         + " Where grh.OrganizationId = :organizationId and"
         + " grh.CompanyId = :companyId and"
         + " grh.Id = :refDocIdNo", nativeQuery = true)
    int insertOpeningBalance(@Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (46,165,167,169,143,217) -- Goods Receipts Notes / GRN Sale Return (InvGrn + InvGrnDetail + GatePassInward). needs :isOnHold computed by existsGrnPestHold(); proc then calls updateInvGrnIsOnHold(). Proc line 127. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions]"
         + " ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal,AddLess,EbPurAgainstWeight,GpNo,VehicleNo"
         + " ,[GPType]"
         + " ,[GpTypeSrNo]"
         + " ,ItemConditionId"
         + " ,RackId"
         + " ,IsOnHold"
         + " ,HoldStatus"
         + " ,StockHoldUserId"
         + " )"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.SupplierCustomerId, Grh.IsApproved, grd.Id, grd.WarehouseId, Grd.JobLotId, Grd.ItemId, Grd.ItemUomId, Grd.CropYear"
         + " , Grd.PackingTypeId, Grd.ItemQty, 0, GrD.NetBillWeight, 0, Grd.StockWeight, 0"
         + " , 'Weight', 0, 0, 0 , 0, 0"
         + " ,CASE WHEN :refDocumentTypeId = 143 THEN 'GRN Sale Return' ELSE 'Goods Receipts Notes' END,grd.CityId"
         + " ,Grh.EntryDate,Grh.EntryUser ,Grh.ModifyDate,Grh.ModifyUser ,"
         + " grd.GrossWeight,EBWPerUnit,grd.EBWTotal,grd.WtCut,grd.WtCutTotal,grd.AdLsWeight,grd.EbPurAgainstWeight,Grh.GpNo,Grh.VehicleNo,"
         + " gp.GatepassType,"
         + " gp.GpTypeSrNo,"
         + " GrD.ItemConditionId,"
         + " GrD.RackId,"
         + " :isOnHold,"
         + " CASE WHEN :isOnHold = 1 THEN 'ACTIVE' ELSE '' END,"
         + " grh.EntryUser"
         + " FROM dbo.InvGrn AS GrH"
         + " INNER JOIN dbo.InvGrnDetail AS GrD ON GrH.Id = GrD.InvGrnId"
         + " LEFT JOIN GatePassInward Gp on Gp.Id = Grh.InwardGatePassId"
         + " Where grh.OrganizationId = :organizationId"
         + " and grh.CompanyId = :companyId"
         + " and grh.Id = :refDocIdNo"
         + " and (grh.Status = 'Accepted')", nativeQuery = true)
    int insertGrn(@Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("isOnHold") Boolean isOnHold, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (46,165,167,169,143,217) -- Goods Receipts Notes Empty Bags (InvGrnDetailEmptyBags). only Status='Accepted', PurchaseQty>0, TypeId<>5. Proc line 179. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],"
         + " EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,[GPType]"
         + " ,[GpTypeSrNo]"
         + " ,ItemConditionId"
         + " ,RackId"
         + " )"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.SupplierCustomerId, Grh.IsApproved, grd.Id,GRD.WarehouseId, 0, Grd.ItemId, 0, ''"
         + " , 0, ISNULL(Grd.PurchaseQty,0) + ISNULL(GrD.ReceivedQty,0), 0, ISNULL(Grd.PurchaseQty,0) + ISNULL(GrD.ReceivedQty,0), 0, ISNULL(Grd.PurchaseQty,0) + ISNULL(GrD.ReceivedQty,0), 0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Goods Receipts Notes Empty Bags'"
         + " ,Grh.EntryDate,Grh.EntryUser ,Grh.ModifyDate,Grh.ModifyUser ,"
         + " 0,0,0,0,0 ,0 ,Grh.GpNo,Grh.VehicleNo,"
         + " gp.GatepassType,"
         + " gp.GpTypeSrNo,"
         + " GrD.BagsCondition,"
         + " GrD.RackId"
         + " FROM dbo.InvGrn AS GrH"
         + " INNER JOIN InvGrnDetailEmptyBags AS GrD ON GrH.Id = GrD.InvGrnId"
         + " LEFT JOIN GatePassInward Gp on Gp.Id = Grh.InwardGatePassId"
         + " Where grh.OrganizationId = :organizationId"
         + " and grh.CompanyId = :companyId"
         + " and grh.Id = :refDocIdNo"
         + " and grd.PurchaseQty > 0"
         + " and (grh.Status = 'Accepted')"
         + " and GrD.TypeId <> 5", nativeQuery = true)
    int insertGrnEmptyBags(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 47 -- Goods Receipts Notes Trading. Proc line 216. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess,EbPurAgainstWeight,GpNo,VehicleNo"
         + " ,[GPType]"
         + " ,[GpTypeSrNo]"
         + " ,ItemConditionId"
         + " ,RackId"
         + " )"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.SupplierCustomerId, Grh.IsApproved, grd.Id, grd.WarehouseId, Grd.JobLotId, Grd.ItemId, Grd.ItemUomId, Grd.CropYear"
         + " , Grd.PackingTypeId, Grd.ItemQty, 0, GrD.NetBillWeight, 0, Grd.StockWeight, 0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Goods Receipts Notes Trading',grd.CityId"
         + " ,Grh.EntryDate,Grh.EntryUser ,Grh.ModifyDate,Grh.ModifyUser ,"
         + " grd.GrossWeight,EBWPerUnit,grd.EBWTotal,grd.WtCut,grd.WtCutTotal ,grd.AdLsWeight,grd.EbPurAgainstWeight,Grh.GpNo,Grh.VehicleNo,"
         + " gp.GatepassType,"
         + " gp.GpTypeSrNo,"
         + " GrD.ItemConditionId,"
         + " GrD.RackId"
         + " FROM dbo.InvGrn AS GrH"
         + " INNER JOIN dbo.InvGrnDetail AS GrD ON GrH.Id = GrD.InvGrnId"
         + " LEFT JOIN GatePassInward Gp on Gp.Id = Grh.InwardGatePassId"
         + " Where grh.OrganizationId = :organizationId and"
         + " grh.CompanyId = :companyId and"
         + " grh.Id = :refDocIdNo", nativeQuery = true)
    int insertGrnTrading(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 48 -- Goods Receipts Notes Store. Proc line 249. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess,EbPurAgainstWeight,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.SupplierCustomerId, Grh.IsApproved, grd.Id, grd.WarehouseId, Grd.JobLotId, Grd.ItemId, Grd.ItemUomId, Grd.CropYear"
         + " , Grd.PackingTypeId, Grd.ItemQty, 0, GrD.NetBillWeight, 0, Grd.StockWeight, 0"
         + " , 'Qty', 0, 0, 0 , 0, 0"
         + " ,'Goods Receipts Notes Store',grd.CityId"
         + " ,Grh.EntryDate,Grh.EntryUser ,Grh.ModifyDate,Grh.ModifyUser ,"
         + " grd.GrossWeight,EBWPerUnit,grd.EBWTotal,grd.WtCut,grd.WtCutTotal ,grd.AdLsWeight,grd.EbPurAgainstWeight,Grh.GpNo,Grh.VehicleNo,"
         + " GrD.ItemConditionId,GrD.RackId"
         + " FROM dbo.InvGrn AS GrH INNER JOIN dbo.InvGrnDetail AS GrD ON GrH.Id = GrD.InvGrnId"
         + " Where grh.OrganizationId = :organizationId and"
         + " grh.CompanyId = :companyId and"
         + " grh.Id = :refDocIdNo", nativeQuery = true)
    int insertGrnStore(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 701 -- Goods Receipts Notes Packing Material. Proc line 276. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess,EbPurAgainstWeight ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.SupplierCustomerId, Grh.IsApproved, grd.Id, grd.WarehouseId, Grd.JobLotId, Grd.ItemId, Grd.ItemUomId, Grd.CropYear"
         + " , Grd.PackingTypeId, Grd.ItemQty, 0, GrD.ItemQty, 0, Grd.ItemQty, 0"
         + " , 'Qty', 0, 0, 0 , 0, 0"
         + " ,'Goods Receipts Notes Packing Material',grd.CityId"
         + " ,Grh.EntryDate,Grh.EntryUser ,Grh.ModifyDate,Grh.ModifyUser ,"
         + " grd.GrossWeight,EBWPerUnit,grd.EBWTotal,grd.WtCut,grd.WtCutTotal ,grd.AdLsWeight,grd.EbPurAgainstWeight,Grh.GpNo,Grh.VehicleNo"
         + " ,GrD.ItemConditionId,GrD.RackId"
         + " FROM dbo.InvGrn AS GrH INNER JOIN dbo.InvGrnDetail AS GrD ON GrH.Id = GrD.InvGrnId"
         + " Where grh.OrganizationId = :organizationId and"
         + " grh.CompanyId = :companyId and"
         + " grh.Id = :refDocIdNo", nativeQuery = true)
    int insertGrnPackingMaterial(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (86,124,149,170,221,1659,703) -- Goods Dispatch Notes (InvGdn + InvGdnDetail + GatePassOutward). TranRemarks/CalcType vary by :refDocumentTypeId. Proc line 302. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId"
         + " ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,VarientId,CastingTypeId,JobOrderId"
         + " ,[GPType]"
         + " ,[GpTypeSrNo]"
         + " ,ValidForSale"
         + " ,ItemConditionId"
         + " ,RackId"
         + " ,SecondaryUomQty,"
         + " SecondaryUomId"
         + " )"
         + " Select GdH.OrganizationId, GdH.CompanyId, GdH.BranchesId, GdH.ProjectsId, GdH.DocumentTypeId, GdH.Id, GdH.DocNo, GdH.DocDate"
         + " , GdH.SupplierCustomerId, GdH.IsApproved, GdD.Id, GdD.WarehouseId, GdD.JobLotId, GdD.ItemId, GdD.ItemUomId, GdD.CropYear"
         + " , GdD.PackingTypeId,0,GdD.ItemQty, 0,GdD.NetBillWeight, 0, GdD.StockWeight"
         + " ,CASE WHEN :refDocumentTypeId IN (87,1659,149) THEN 'Qty'ELSE 'Weight' END, 0, 0, 0 , 0, 0,"
         + " CASE"
         + " WHEN :refDocumentTypeId = 221 THEN 'Gdn Sale To Party Processing'"
         + " WHEN :refDocumentTypeId = 87 THEN 'Delivery Challan'"
         + " WHEN :refDocumentTypeId = 124 THEN 'Goods Dispatch Notes Direct'"
         + " WHEN :refDocumentTypeId = 149 THEN 'Goods Dispatch Notes Store & PM'"
         + " WHEN :refDocumentTypeId = 703 THEN 'Gdn (Purchase Return)'"
         + " ELSE 'Goods Dispatch Notes' END"
         + " ,gdd.CityId"
         + " ,Gdh.EntryDate,Gdh.EntryUser ,Gdh.ModifyDate,Gdh.ModifyUser"
         + " ,Gdd.GrossWeight,gdd.EBWPerUnit,Gdd.EBWTotal,Gdd.WtCut,Gdd.WtCutTotal ,Gdd.AdLsWeight ,Gdh.GpNo,Gdh.VehicleNo"
         + " ,gdd.ItemVariantId,gdd.CastingTypeId,gdd.JobOrderId,"
         + " gp.GatepassType,"
         + " gp.GpTypeSrNo,"
         + " 1,"
         + " GdD.ItemConditionId,"
         + " GdD.RackId,"
         + " GdD.SecondaryUomQty,"
         + " GdD.SecondaryUomId"
         + " FROM dbo.InvGdn AS GdH"
         + " INNER JOIN dbo.InvGdnDetail AS GdD ON GdH.Id = GdD.InvGdnId AND ISNULL(GdD.ActionTypeId,0) <> 3"
         + " LEFT JOIN GatePassOutward Gp on Gp.Id = Gdh.OutwardGatePassId"
         + " Where GdH.OrganizationId = :organizationId and"
         + " GdH.CompanyId = :companyId and"
         + " GdH.Id = :refDocIdNo", nativeQuery = true)
    int insertGdn(@Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (57,1804) -- Purchase Invoice Direct (InvPurchaseInvoice). Proc line 352. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select PH.OrganizationId, PH.CompanyId,(CASE WHEN PD.BranchId > 0 THEN PD.BranchId ELSE PH.BranchesId END), PH.ProjectsId, PH.DocumentTypeId, PH.Id, PH.DocNo, PH.DocDate"
         + " , PH.SupplierCustomerId, PH.IsApproved, PD.Id, PD.WarehouseId, PD.JobLotId, PD.ItemId, PD.ItemUomId, PD.CropYear"
         + " , PD.PackingTypeId, PD.ItemQty, 0, PD.NetBillWeight, 0, PD.NetStockWeight, 0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Purchase Invoice Direct'"
         + " ,PH.EntryDate,PH.EntryUser ,PH.ModifyDate,PH.ModifyUser ,"
         + " PD.GrossWeight,PD.EBWeight,PD.EBTotalWt,PD.WeightCut,PD.WeightCutTotal ,PD.AdLsWeight ,PD.GpNo,PD.VehicleNo"
         + " ,PD.ItemConditionId,PD.RackId"
         + " FROM dbo.InvPurchaseInvoice AS PH"
         + " INNER JOIN dbo.InvPurchaseInvoiceDetail AS PD ON PH.Id = PD.InvPurchaseInvoiceId"
         + " Where PH.OrganizationId = :organizationId and"
         + " PH.CompanyId = :companyId and"
         + " PH.Id = :refDocIdNo", nativeQuery = true)
    int insertPurchaseInvoiceDirect(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 57 -- Purchase Invoice Direct Empty Bags (InvPurchaseInvoiceEmptyBags). Proc line 378. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.SupplierCustomerId, Grh.IsApproved, grd.Id, 0, 0, Grd.ItemId, 0, ''"
         + " , 0, ISNULL(Grd.PurchaseQty,0) + ISNULL(GrD.ReceivedQty,0), 0, ISNULL(Grd.PurchaseQty,0) + ISNULL(GrD.ReceivedQty,0), 0, ISNULL(Grd.PurchaseQty,0) + ISNULL(GrD.ReceivedQty,0), 0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Purchase Invoice Direct Empty Bags'"
         + " ,Grh.EntryDate,Grh.EntryUser ,Grh.ModifyDate,Grh.ModifyUser ,"
         + " 0,0,0,0,0 ,0 ,0,0,"
         + " grd.ItemConditionId,GrD.RackId"
         + " FROM dbo.InvPurchaseInvoice AS GrH"
         + " INNER JOIN InvPurchaseInvoiceEmptyBags AS GrD ON GrH.Id = GrD.InvPurchaseInvoiceId"
         + " Where grh.OrganizationId = :organizationId"
         + " and grh.CompanyId = :companyId"
         + " and grh.Id = :refDocIdNo"
         + " and (GrD.PurchaseQty > 0 or grd.ReceivedQty > 0)", nativeQuery = true)
    int insertPurchaseInvoiceDirectEmptyBags(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 59 -- Purchase Invoice Return. only rows with ISNULL(GdnId,0)=0. Proc line 405. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , SH.SupplierCustomerId, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId, SD.CropYear"
         + " , SD.PackingTypeId,0,SD.ItemQty, 0,SD.NetBillWeight, 0, SD.NetStockWeight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Purchase Invoice Return'"
         + " ,Sh.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.GrossWeight,SD.EBWeight,SD.EBTotalWt,SD.WeightCut,SD.WeightCutTotal ,SD.AdLsWeight ,SD.GpNo,SD.VehicleNo"
         + " ,SD.ItemConditionId,SD.RackId"
         + " FROM dbo.InvPurchaseInvoice AS SH"
         + " INNER JOIN dbo.InvPurchaseInvoiceDetail AS SD ON SH.Id = SD.InvPurchaseInvoiceId"
         + " Where SH.OrganizationId = :organizationId and"
         + " SH.CompanyId = :companyId and"
         + " SH.Id = :refDocIdNo and"
         + " ISNULL(SD.GdnId,0) = 0", nativeQuery = true)
    int insertPurchaseInvoiceReturn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (61,245,131) -- Purchase Invoice Direct For Store / With Tax. Proc line 433. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select PH.OrganizationId, PH.CompanyId, PH.BranchesId, PH.ProjectsId, PH.DocumentTypeId, PH.Id, PH.DocNo, PH.DocDate"
         + " , PH.SupplierCustomerId, PH.IsApproved, PD.Id, PD.WarehouseId, PD.JobLotId, PD.ItemId, PD.ItemUomId,pd.AttributeVariantId,PD.CropYear"
         + " , PD.PackingTypeId, PD.ItemQty, 0, PD.ItemQty, 0, PD.ItemQty, 0"
         + " , 'Qty', 0, 0, 0 , 0, 0"
         + " ,CASE WHEN :refDocumentTypeId = 131 THEN 'Purchase Invoice Direct With Tax' ELSE 'Purchase Invoice Direct For Store' END"
         + " ,PH.EntryDate,PH.EntryUser ,PH.ModifyDate,PH.ModifyUser ,"
         + " PD.GrossWeight,PD.EBWeight,PD.EBTotalWt,PD.WeightCut,PD.WeightCutTotal ,PD.AdLsWeight ,PD.GpNo,PD.VehicleNo"
         + " ,PD.ItemConditionId,PD.RackId"
         + " FROM dbo.InvPurchaseInvoice AS PH"
         + " INNER JOIN dbo.InvPurchaseInvoiceDetail AS PD ON PH.Id = PD.InvPurchaseInvoiceId"
         + " Where PH.OrganizationId = :organizationId and"
         + " PH.CompanyId = :companyId and"
         + " PH.Id = :refDocIdNo", nativeQuery = true)
    int insertPurchaseInvoiceDirectStore(@Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (99,139,184,186,1809) -- Sale Invoice Direct (InvSaleInvoice). only rows with ISNULL(InvGdnId,0)=0. Proc line 461. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo,ValidForSale"
         + " ,ItemConditionId,RackId)"
         + " Select SH.OrganizationId, SH.CompanyId"
         + " , (CASE WHEN SD.BranchId > 0 THEN SD.BranchId ELSE SH.BranchesId END)"
         + " , SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , SH.SupplierCustomerId, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId, SD.CropYear"
         + " , SD.PackingTypeId, 0,SD.ItemQty, 0,SD.NetBillWeight, 0, SD.NetStockWeight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Sale Invoice Direct'"
         + " ,Sh.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.GrossWeight,SD.EBWeight,SD.EBTotalWt,SD.WeightCut,SD.WeightCutTotal ,SD.AdLsWeight ,SD.GpNo,SD.VehicleNo,1,"
         + " sd.ItemConditionId,SD.RackId"
         + " FROM dbo.InvSaleInvoice AS SH"
         + " INNER JOIN dbo.InvSaleInvoiceDetail AS SD ON SH.Id = SD.InvSaleInvoiceId"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and ISNULL(SD.InvGdnId,0) = 0", nativeQuery = true)
    int insertSaleInvoiceDirect(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (103,126,133,1608,1661) -- Trading Sale Invoice With Qty / Sale Invoice Direct Packing Material (+With Tax) / Sale Invoice Direct. only rows with ISNULL(InvGdnId,0)=0. Proc line 491. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight"
         + " ,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,VarientId,ProductionStageId,WorkOrderId,WorkStationId,CastingTypeId,ValidForSale"
         + " ,ItemConditionId,RackId,SecondaryUomQty,SecondaryUomId,SecondaryUomItemRate"
         + " )"
         + " SELECT SH.OrganizationId, SH.CompanyId"
         + " , (CASE WHEN SD.BranchId > 0 THEN SD.BranchId ELSE SH.BranchesId END)"
         + " , SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , SH.SupplierCustomerId, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId, SD.CropYear"
         + " , SD.PackingTypeId, 0,SD.ItemQty, 0,SD.NetBillWeight, 0, SD.NetStockWeight"
         + " , 'Weight', 0, 0, 0 , 0, 0"
         + " ,CASE"
         + " WHEN :refDocumentTypeId = 103 THEN 'Trading Sale Invoice With Qty'"
         + " WHEN :refDocumentTypeId = 126 THEN 'Sale Invoice Direct Packing Material'"
         + " WHEN :refDocumentTypeId = 133 THEN 'Sale Invoice Direct Packing Material With Tax'"
         + " ELSE 'Sale Invoice Direct' END"
         + " ,Sh.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser"
         + " ,SD.GrossWeight,SD.EBWeight,SD.EBTotalWt,SD.WeightCut,SD.WeightCutTotal"
         + " ,SD.AdLsWeight ,SD.GpNo,SD.VehicleNo"
         + " ,sd.ItemVariantId,0,0,0,sd.CastingTypeId,1"
         + " ,SD.ItemConditionId,SD.RackId"
         + " ,SD.SecondaryUomQty,SD.SecondaryUomId,SD.SecondaryUomItemRate"
         + " FROM dbo.InvSaleInvoice AS SH"
         + " INNER JOIN dbo.InvSaleInvoiceDetail AS SD ON SH.Id = SD.InvSaleInvoiceId"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and ISNULL(SD.InvGdnId,0) = 0", nativeQuery = true)
    int insertSaleInvoiceDirectWithVariant(@Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 94 -- Sale Invoice Direct For Petrolium. Proc line 531. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , SH.SupplierCustomerId, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId, SD.CropYear"
         + " , SD.PackingTypeId, 0,SD.ItemQty, 0,SD.NetBillWeight, 0, SD.NetStockWeight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Sale Invoice Direct For Petrolium'"
         + " ,Sh.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.GrossWeight,SD.EBWeight,SD.EBTotalWt,SD.WeightCut,SD.WeightCutTotal ,SD.AdLsWeight ,SD.GpNo,SD.VehicleNo,SD.ItemConditionId,SD.RackId"
         + " FROM dbo.InvSaleInvoice AS SH INNER JOIN dbo.InvSaleInvoiceDetail AS SD ON SH.Id = SD.InvSaleInvoiceId"
         + " Where SH.OrganizationId = :organizationId and"
         + " SH.CompanyId = :companyId and"
         + " SH.Id = :refDocIdNo", nativeQuery = true)
    int insertSaleInvoiceDirectPetroleum(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 98 -- Sale Invoice return (InvPurchaseInvoice). only rows with ISNULL(InvGrnId,0)=0. Proc line 555. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select PH.OrganizationId, PH.CompanyId"
         + " , (CASE WHEN PD.BranchId > 0 THEN PD.BranchId ELSE PH.BranchesId END)"
         + " , PH.ProjectsId, PH.DocumentTypeId, PH.Id, PH.DocNo, PH.DocDate"
         + " , PH.SupplierCustomerId, PH.IsApproved, PD.Id, PD.WarehouseId, PD.JobLotId, PD.ItemId, PD.ItemUomId, PD.CropYear"
         + " , PD.PackingTypeId, PD.ItemQty, 0, PD.NetBillWeight, 0, PD.NetStockWeight, 0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Sale Invoice return'"
         + " ,PH.EntryDate,PH.EntryUser ,PH.ModifyDate,PH.ModifyUser ,"
         + " PD.GrossWeight,PD.EBWeight,PD.EBTotalWt,PD.WeightCut,PD.WeightCutTotal ,PD.AdLsWeight ,PD.GpNo,PD.VehicleNo"
         + " ,PD.ItemConditionId,PD.RackId"
         + " FROM dbo.InvPurchaseInvoice AS PH"
         + " INNER JOIN dbo.InvPurchaseInvoiceDetail AS PD ON PH.Id = PD.InvPurchaseInvoiceId"
         + " Where PH.OrganizationId = :organizationId and"
         + " PH.CompanyId = :companyId and"
         + " PH.Id = :refDocIdNo and"
         + " ISNULL(pd.InvGrnId,0) = 0", nativeQuery = true)
    int insertSaleInvoiceReturn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 100 -- Point Of Sale Invoice (PosSaleInvoice). uses dbo.GetEquivalentByItemIdandBaseUnitId(). Proc line 585. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " )"
         + " Select distinct SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , SH.SupplierCustomerId, SH.IsApproved, SD.Id, sd.WarehouseId, 0, SD.ItemId, 0, ''"
         + " , 0, 0,SD.ItemQty, 0,Sd.ItemQty * [dbo].[GetEquivalentByItemIdandBaseUnitId](sh.OrganizationId,sh.CompanyId,sd.ItemId,i.BaseUnitId),"
         + " 0, Sd.ItemQty * [dbo].[GetEquivalentByItemIdandBaseUnitId](sh.OrganizationId,sh.CompanyId,sd.ItemId,i.BaseUnitId)"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Point Of Sale Invoice'"
         + " ,Sh.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.ItemQty,0,0,0,0 ,0 ,0,''"
         + " FROM dbo.PosSaleInvoice AS SH"
         + " INNER JOIN dbo.PosSaleInvoiceDetail AS SD ON SH.Id = SD.InvSaleInvoiceId"
         + " INNER JOIN Item i on sd.ItemId = i.Id"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertPosSaleInvoice(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 100 -- PosSaleInvoice XGetYFree. uses dbo.GetEquivalentByItemIdandBaseUnitId(). Proc line 609. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select distinct SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , SH.SupplierCustomerId, SH.IsApproved, SD.Id,SD.WarehouseId, 0, SD.YItemId,0, ''"
         + " , 0, 0,SD.YItemFactorQty, 0,SD.YItemFactorQty * [dbo].[GetEquivalentByItemIdandBaseUnitId](sh.OrganizationId,sh.CompanyId,sd.YItemId,i.BaseUnitId)"
         + " , 0, SD.YItemFactorQty * [dbo].[GetEquivalentByItemIdandBaseUnitId](sh.OrganizationId,sh.CompanyId,sd.YItemId,i.BaseUnitId)"
         + " , 'Qty', 0, 0, 0 , 0, 0,'PosSaleInvoice XGetYFree'"
         + " ,Sh.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.SalesXItemQty,0,0,0,0 ,0 ,0,''"
         + " FROM dbo.PosSaleInvoice AS SH"
         + " INNER JOIN dbo.PosSaleInvoiceDetailXGetYFree AS SD ON SH.Id = SD.InvSaleInvoiceId"
         + " INNER JOIN Item i on sd.YItemId = i.Id"
         + " Where SH.OrganizationId = :organizationId and"
         + " SH.CompanyId = :companyId and"
         + " SH.Id = :refDocIdNo", nativeQuery = true)
    int insertPosSaleInvoiceXGetYFree(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 106 -- Whole and Retail SaleInvoice (WsRmSaleInvoice). uses dbo.GetEquivalentByItemIdandBaseUnitId(). Proc line 634. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select distinct SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , SH.SupplierCustomerId, SH.IsApproved, SD.Id, sd.WarehouseId, 0, SD.ItemId,0, ''"
         + " , 0, 0,SD.ItemQty, 0,SD.ItemQty * [dbo].[GetEquivalentByItemIdandBaseUnitId](sh.OrganizationId,sh.CompanyId,sd.ItemId,i.BaseUnitId)"
         + " , 0, SD.ItemQty * [dbo].[GetEquivalentByItemIdandBaseUnitId](sh.OrganizationId,sh.CompanyId,sd.ItemId,i.BaseUnitId)"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Whole and Retail SaleInvoice'"
         + " ,Sh.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.ItemQty,0,0,0,0 ,0 ,0,''"
         + " FROM dbo.WsRmSaleInvoice AS SH INNER JOIN dbo.WsRmSaleInvoiceDetail AS SD ON SH.Id = SD.InvSaleInvoiceId"
         + " inner join Item i on sd.ItemId = i.Id"
         + " Where SH.OrganizationId = :organizationId and"
         + " SH.CompanyId = :companyId and"
         + " SH.Id = :refDocIdNo", nativeQuery = true)
    int insertWsRmSaleInvoice(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 106 -- WholeSaleInvoice XGetYFree. uses dbo.GetEquivalentByItemIdandBaseUnitId(). Proc line 658. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select distinct SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , SH.SupplierCustomerId, SH.IsApproved, SD.Id,SD.WarehouseId, 0, SD.YItemId,0, ''"
         + " , 0, 0,SD.YItemFactorQty, 0,SD.YItemFactorQty * [dbo].[GetEquivalentByItemIdandBaseUnitId](sh.OrganizationId,sh.CompanyId,sd.YItemId,i.BaseUnitId)"
         + " , 0, SD.YItemFactorQty * [dbo].[GetEquivalentByItemIdandBaseUnitId](sh.OrganizationId,sh.CompanyId,sd.YItemId,i.BaseUnitId)"
         + " , 'Qty', 0, 0, 0 , 0, 0,'WholeSaleInvoice XGetYFree'"
         + " ,Sh.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.SalesXItemQty,0,0,0,0 ,0 ,0,''"
         + " FROM dbo.WsRmSaleInvoice AS SH"
         + " INNER JOIN dbo.WsRmSaleInvoiceDetailXGetYFree AS SD ON SH.Id = SD.WsRmSaleInvoiceId"
         + " INNER JOIN Item i on sd.YItemId = i.Id"
         + " Where SH.OrganizationId = :organizationId and"
         + " SH.CompanyId = :companyId and"
         + " SH.Id = :refDocIdNo", nativeQuery = true)
    int insertWsRmSaleInvoiceXGetYFree(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 107 -- LOCATION WSRM STOCK TRANSFER. Proc line 683. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, 0, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , SH.SourceLocationId, SH.IsApproved, SD.Id, sd.WarehouseId, 0, SD.ItemId,sd.ItemUomId, ''"
         + " , 0, 0,SD.IssuedQty, 0,sd.BillWeight, 0, sd.StockWeight"
         + " , 'Qty', 0, 0, 0 , 0, 0,'LOCATION WSRM STOCK TRANSFER'"
         + " ,Sh.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId,SD.BillWeight,0,0,0,0 ,0 ,0,''"
         + " FROM dbo.WsRmStockTransferNotes AS SH INNER JOIN dbo.WsRmStockTransferNotesDetail AS SD ON SH.Id = SD.WsRmStockTransferNotesId"
         + " inner join Item i on sd.ItemId = i.Id"
         + " Where SH.OrganizationId = :organizationId and"
         + " SH.CompanyId = :companyId and"
         + " SH.Id = :refDocIdNo", nativeQuery = true)
    int insertWsRmStockTransferNotes(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 108 -- LOCATION WSRMSTOCK RECEIVING. Proc line 705. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, 0, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , SH.SourceLocationId, SH.IsApproved, SD.Id, sd.WarehouseId, 0, SD.ItemId,sd.ItemUomId, ''"
         + " , 0,SD.ReceivedQty, 0,sd.BillWeight, 0, sd.StockWeight,0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'LOCATION WSRMSTOCK RECEIVING'"
         + " ,Sh.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId,SD.BillWeight,0,0,0,0 ,0 ,0,''"
         + " FROM dbo.WsRmStockReceivedNotes AS SH INNER JOIN dbo.WsRmStockReceivedNotesDetail AS SD ON SH.Id = SD.WsRmStockReceivedNotesId"
         + " inner join Item i on sd.ItemId = i.Id"
         + " Where SH.OrganizationId = :organizationId and"
         + " SH.CompanyId = :companyId and"
         + " SH.Id = :refDocIdNo", nativeQuery = true)
    int insertWsRmStockReceivedNotes(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 66 -- VarietyChangeOutPut (InvStockConversion, EntryType<>'Issue'). writes IsOnHold/HoldStatus/labIPmActivityLogId. Proc line 728. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,IsOnHold,HoldStatus,labIPmActivityLogId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchedId, SH.ProjectsId, SH.DocTypeId, SH.Id, SH.DocSrNo, SH.DocDate"
         + " , 0, SH.PostState, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId, SD.CropBatch"
         + " , SD.PackingTypeId,SD.Qty,0,SD.Weight,0,SD.Weight,0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'VarietyChangeOutPut'"
         + " ,Sh.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.Weight,0,0,0,0,0,0,''"
         + " ,SD.IsOnHold,(CASE WHEN SD.IsOnHold = 1 THEN 'ACTIVE' ELSE '' END),SD.labIPmActivityLogId"
         + " FROM dbo.InvStockConversion AS SH"
         + " INNER JOIN dbo.InvStockConversionDetail AS SD ON SH.Id = SD.InvStockConversionId"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and Sd.EntryType <> 'Issue'", nativeQuery = true)
    int insertStockConversionOutput(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 66 -- VarietyChangeIssue (EntryType='Issue'). Proc line 755. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,labIPmActivityLogId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchedId, SH.ProjectsId, SH.DocTypeId, SH.Id, SH.DocSrNo, SH.DocDate"
         + " , 0, SH.PostState, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId, SD.CropBatch"
         + " , SD.PackingTypeId,0,SD.Qty,0,SD.Weight,0,SD.Weight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'VarietyChangeIssue'"
         + " ,Sh.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.Weight,0,0,0,0 ,0 ,0,'',"
         + " sd.labIPmActivityLogId"
         + " FROM dbo.InvStockConversion AS SH"
         + " INNER JOIN dbo.InvStockConversionDetail AS SD ON SH.Id = SD.InvStockConversionId"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and Sd.EntryType = 'Issue'", nativeQuery = true)
    int insertStockConversionIssue(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 66 -- Packing Material (InvStockConversionPackingMaterial). Proc line 782. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchedId, SH.ProjectsId, SH.DocTypeId, SH.Id, SH.DocSrNo, SH.DocDate"
         + " , 0, SH.PostState, SD.Id, SD.WarehouseId, 0, SD.ItemId, 0,''"
         + " , 0,0,SD.ItemQty,0,SD.ItemQty,0,SD.ItemQty"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Packing Material'"
         + " , Sh.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.ItemQty,0,0,0,0,0,0,'',sd.ItemConditionId,SD.RackId"
         + " FROM dbo.InvStockConversion AS SH"
         + " INNER JOIN dbo.InvStockConversionPackingMaterial AS SD ON SH.Id = SD.InvStockConversionId"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertStockConversionPackingMaterial(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 67 -- VarietyChangeOutPut (EntryType<>'Issue'). Proc line 807. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight"
         + " ,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo,ItemConditionId,RackId)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchedId, SH.ProjectsId, SH.DocTypeId, SH.Id, SH.DocSrNo, SH.DocDate"
         + " , 0, SH.PostState, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId, SD.CropBatch"
         + " , SD.PackingTypeId,SD.Qty,0,SD.Weight,0,SD.Weight,0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'VarietyChangeOutPut'"
         + " ,Sh.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.Weight,0,0,0,0,0,0,'',sd.ItemConditionId,SD.RackId"
         + " FROM dbo.InvStockConversion AS SH INNER JOIN dbo.InvStockConversionDetail AS SD ON SH.Id = SD.InvStockConversionId"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and Sd.EntryType <> 'Issue'", nativeQuery = true)
    int insertStockConversionOutput67(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 67 -- VarietyChangeIssue (EntryType='Issue'). Proc line 830. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchedId, SH.ProjectsId, SH.DocTypeId, SH.Id, SH.DocSrNo, SH.DocDate"
         + " , 0, SH.PostState, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId, SD.CropBatch"
         + " , SD.PackingTypeId,0,SD.Qty,0,SD.Weight,0,SD.Weight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'VarietyChangeIssue'"
         + " ,Sh.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.Weight,0,0,0,0,0,0,'',SD.ItemConditionId,SD.RackId"
         + " FROM dbo.InvStockConversion AS SH INNER JOIN dbo.InvStockConversionDetail AS SD ON SH.Id = SD.InvStockConversionId"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and Sd.EntryType = 'Issue'", nativeQuery = true)
    int insertStockConversionIssue67(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (68,806,807) -- StockTransfer / Manual StockTransfer / Stock Transfer Store - QTY IN (ToWarehouseId). Proc line 855. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions]"
         + " ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, wh.BranchId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.ToWarehouseId, SD.JobLotIdTo, SD.ItemId, SD.PackUomId, SD.CropYear"
         + " , SD.InvPackingTypeId,SD.Qty,0,SD.NetWeight,0,SD.NetWeight,0"
         + " ,CASE WHEN :refDocumentTypeId = 807 THEN 'Qty' ELSE 'Weight' END,0, 0, 0 , 0, 0"
         + " ,CASE"
         + " WHEN :refDocumentTypeId = 68 THEN 'StockTransfer'"
         + " WHEN :refDocumentTypeId = 806 THEN 'Manual StockTransfer'"
         + " ELSE 'Stock Transfer Store' END"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.GrossWeight,SD.EbUnit,SD.EbTotal,0,0,SD.AdLsWeight ,0,'',SD.ItemConditionId,SD.RackId"
         + " FROM InvstockTransferHeader AS SH"
         + " INNER JOIN dbo.InvStockTransferDetail AS SD ON SH.Id = SD.InvStockTransferHeaderId"
         + " LEFT JOIN WarehousesAllocationToBranch wh on SD.ToWarehouseId = wh.warehouseId"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and SD.ToWarehouseId > 0", nativeQuery = true)
    int insertStockTransferIn(@Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (68,806,807) -- StockTransfer / Manual StockTransfer / Stock Transfer Store - QTY OUT (FromWarehouseId). Proc line 884. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions]"
         + " ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId,wh.BranchId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.FromWarehouseId, SD.JobLotId, SD.ItemId, SD.PackUomId, SD.CropYear"
         + " , SD.InvPackingTypeId,0,SD.Qty,0,SD.NetWeight,0,SD.NetWeight"
         + " ,CASE WHEN :refDocumentTypeId = 807 THEN 'Qty' ELSE 'Weight' END,0, 0, 0 , 0, 0"
         + " ,CASE"
         + " WHEN :refDocumentTypeId = 68 THEN 'StockTransfer'"
         + " WHEN :refDocumentTypeId = 806 THEN 'Manual StockTransfer'"
         + " ELSE 'Stock Transfer Store' END"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.GrossWeight,SD.EbUnit,SD.EbTotal,0,0,SD.AdLsWeight ,0,'',sd.ItemConditionId,SD.RackId"
         + " FROM InvstockTransferHeader AS SH"
         + " INNER JOIN dbo.InvStockTransferDetail AS SD ON SH.Id = SD.InvStockTransferHeaderId"
         + " LEFT JOIN WarehousesAllocationToBranch wh on SD.FromWarehouseId = wh.warehouseId"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and SD.FromWarehouseId > 0", nativeQuery = true)
    int insertStockTransferOut(@Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 80 -- InvFoodProductionInput (EntryType='Issue'). Proc line 917. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId"
         + " ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,JobOrderId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchId, SH.ProjectId, SH.DocumentTypeId, SH.Id, SH.DocCode, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId, SD.CropBatch"
         + " , SD.PackingTypeId,0,SD.Qty,0,SD.Weight,0,SD.Weight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'InvFoodProductionInput'"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser"
         + " ,SD.GrossWeight,sd.EbUnit,sd.EbTotal,0,0,0,0,''"
         + " ,sd.InvProductionJobOrderId"
         + " FROM InvFoodProduction AS SH INNER JOIN InvFoodProductionDetail AS SD ON SH.Id = SD.InvFoodProductionId"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and Sd.EntryType = 'Issue'", nativeQuery = true)
    int insertFoodProductionInput(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 80 -- InvFoodProductionInputWorkInProcess. Proc line 945. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId"
         + " ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,JobOrderId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchId, SH.ProjectId, SH.DocumentTypeId, SH.Id, SH.DocCode, SH.DocDate"
         + " , SH.WIPAccountId, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SH.WIPItemId, SD.ItemUomId, SD.CropBatch"
         + " , SD.PackingTypeId,SD.Qty,0,SD.Weight,0,SD.Weight,0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'InvFoodProductionInputWorkInProcess'"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser"
         + " ,SD.GrossWeight,sd.EbUnit,sd.EbTotal,0,0,0,0,''"
         + " ,SD.InvProductionJobOrderId"
         + " FROM InvFoodProduction AS SH INNER JOIN InvFoodProductionDetail AS SD ON SH.Id = SD.InvFoodProductionId"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and Sd.EntryType = 'Issue'", nativeQuery = true)
    int insertFoodProductionInputWip(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 112 -- InvFoodProductionOutPut (EntryType<>'Issue'). Proc line 972. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId"
         + " ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,JobOrderId,ValidForSale"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchId, SH.ProjectId, SH.DocumentTypeId, SH.Id, SH.DocCode, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId, SD.CropBatch"
         + " , SD.PackingTypeId,SD.Qty,0,SD.Weight,0,SD.Weight,0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'InvFoodProductionOutPut'"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser"
         + " ,SD.GrossWeight,sd.EbUnit,sd.EbTotal,0,0,0 ,0,''"
         + " ,SD.InvProductionJobOrderId,1"
         + " FROM InvFoodProduction AS SH"
         + " INNER JOIN InvFoodProductionDetail AS SD ON SH.Id = SD.InvFoodProductionId"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and Sd.EntryType <> 'Issue'", nativeQuery = true)
    int insertFoodProductionOutput(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 112 -- InvFoodProductionOutPutWorkInProcess. Proc line 1000. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId"
         + " ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,JobOrderId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchId, SH.ProjectId, SH.DocumentTypeId, SH.Id, SH.DocCode, SH.DocDate"
         + " , SH.WIPAccountId, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SH.WIPItemId, SD.ItemUomId, SD.CropBatch"
         + " , SD.PackingTypeId,0,SD.Qty,0,SD.Weight,0,SD.Weight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'InvFoodProductionOutPutWorkInProcess'"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser"
         + " ,SD.GrossWeight,sd.EbUnit,sd.EbTotal,0,0,0 ,0,''"
         + " ,SD.InvProductionJobOrderId"
         + " FROM InvFoodProduction AS SH INNER JOIN InvFoodProductionDetail AS SD ON SH.Id = SD.InvFoodProductionId"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and Sd.EntryType <> 'Issue'", nativeQuery = true)
    int insertFoodProductionOutputWip(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 111 -- InvFoodProductionPackingMaterial. @RefDocIdNo is the PackingMaterial row Id. Proc line 1026. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select j.OrganizationId, j.CompanyId, j.BranchesId, J.ProjectsId, pm.DocumentTypeId, pm.Id, pm.DocNo, pm.DocDate"
         + " , J.WorkInProccessAcId, 0, pm.InvProductionJobOrderId,pm.WarehouseId, 0, pm.ItemId, pm.itemUomSchId, ''"
         + " , 0,0,pm.Qty,0,pm.Qty,0,pm.Qty"
         + " , 'Qty', 0, 0, 0 , 0, 0,'InvFoodProductionPackingMaterial'"
         + " ,pm.EntryDate,pm.EntryUser ,pm.ModifyDate,pm.ModifyUser"
         + " ,pm.Qty,0,0,0,0,0 ,0,''"
         + " ,pm.ItemConditionId,pm.RackId"
         + " FROM"
         + " InvFoodProductionPackingMaterial pm"
         + " INNER JOIN dbo.Item ON pm.ItemId = dbo.Item.Id"
         + " inner join InvProductionJobOrder J on pm.InvProductionJobOrderId = j.Id"
         + " where j.OrganizationId = :organizationId"
         + " and j.CompanyId = :companyId"
         + " and pm.Id = :refDocIdNo", nativeQuery = true)
    int insertFoodProductionPackingMaterial(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 181 -- Production Consumption (InvFoodProductionConsumptionDetail). Proc line 1055. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId"
         + " ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,JobOrderId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchId, SH.ProjectId, SH.DocumentTypeId, SH.Id, SH.DocCode, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId, SD.CropBatch"
         + " , SD.PackingTypeId,0,SD.Qty,0,SD.Weight,0,SD.Weight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Production Consumption'"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser"
         + " ,SD.Weight,0,0,0,0,0 ,0,''"
         + " ,SD.InvProductionJobOrderId"
         + " FROM InvFoodProduction AS SH"
         + " INNER JOIN InvFoodProductionConsumptionDetail AS SD ON SH.Id = SD.InvFoodProductionId"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and sh.DocumentTypeId = 181"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertFoodProductionConsumption(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 451 -- Store Issuance (header RefDocumentTypeId = 450). Proc line 1083. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal"
         + " ,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo,ItemConditionId,RackId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.WarehouseId,SD.DepartmentId, SD.ItemId, SD.ItemUomSchId, ''"
         + " , SD.AssetsId, 0,SD.IssueQty, 0,SD.IssueQty, 0, SD.IssueQty"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Store Issuance'"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.IssueQty,0,0,0,0,0 ,0,'',sd.ItemConditionId,SD.RackId"
         + " FROM InvGsStoreIssuanceHeader AS SH"
         + " INNER JOIN InvGsStoreIssuanceDetail AS SD ON SH.Id = SD.InvGsStoreIssuanceHeaderId AND ISNULL(ActionTypeId,0) <> 3"
         + " Where SH.OrganizationId = :organizationId and"
         + " SH.CompanyId = :companyId and"
         + " SH.Id = :refDocIdNo"
         + " AND SH.RefDocumentTypeId = 450", nativeQuery = true)
    int insertStoreIssuance(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 451 -- Packing Material Issuance (header RefDocumentTypeId = 85). Proc line 1108. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut"
         + " ,WtCutTotal ,AddLess ,GpNo,VehicleNo,ItemConditionId,RackId)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " ,0, SH.IsApproved, SD.Id, SD.WarehouseId,0, SD.ItemId, SD.ItemUomSchId, ''"
         + " , 0, 0,SD.IssueQty, 0,SD.IssueQty, 0, SD.IssueQty"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Packing Material Issuance'"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.IssueQty,0,0,0,0,0 ,0,'',sd.ItemConditionId,SD.RackId"
         + " FROM InvGsStoreIssuanceHeader AS SH INNER JOIN InvGsStoreIssuanceDetail AS SD ON SH.Id = SD.InvGsStoreIssuanceHeaderId AND ISNULL(ActionTypeId,0) <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " AND SH.RefDocumentTypeId = 85", nativeQuery = true)
    int insertPackingMaterialIssuance(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (452,213) -- Store Issuance Direct. Proc line 1131. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId"
         + " ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo,ItemConditionId,RackId)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.WarehouseId,SD.DepartmentId, SD.ItemId, SD.ItemUomSchId, ''"
         + " , SD.AssetsId, 0,SD.IssueQty, 0,SD.IssueQty, 0, SD.IssueQty"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Store Issuance Direct'"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.IssueQty,0,0,0,0,0 ,0,'',sd.ItemConditionId,SD.RackId"
         + " FROM InvGsStoreIssuanceHeader AS SH"
         + " INNER JOIN InvGsStoreIssuanceDetail AS SD ON SH.Id = SD.InvGsStoreIssuanceHeaderId AND ISNULL(ActionTypeId,0) <> 3"
         + " Where SH.OrganizationId = :organizationId and"
         + " SH.CompanyId = :companyId and"
         + " SH.Id = :refDocIdNo", nativeQuery = true)
    int insertStoreIssuanceDirect(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (205,206) -- Export Goods Forwarding (ExImForwarding). Proc line 1155. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,[GPType]"
         + " ,[GpTypeSrNo]"
         + " ,ValidForSale)"
         + " Select GdH.OrganizationId, GdH.CompanyId, GdH.BranchesId, GdH.ProjectsId, GdH.DocumentTypeId, GdH.Id, GdH.DocNo, GdH.DocDate"
         + " , GdH.SupplierCustomerId, GdH.IsApproved, GdD.Id, GdD.WarehouseId, GdD.LotJobId, GdD.ItemId, GdD.UOMScheduleIdOuter,crop.CropYear"
         + " , GdD.PackingMaterialId, 0,GdD.OuterQty, 0,GdD.StockWeight, 0, GdD.StockWeight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Export Goods Forwarding'"
         + " ,GdH.EntryDate,GdH.EntryUser ,GdH.ModifyDate,GdH.ModifyUser ,"
         + " GdD.GrossWeight,GdD.EbUnit,GdD.EbTotal,0,0,GdD.AdLsWeight ,GdH.GpNo,GdH.VehicleNo,"
         + " gp.GatepassType,"
         + " gp.GpTypeSrNo,"
         + " 1"
         + " FROM ExImForwarding AS GdH"
         + " INNER JOIN dbo.ExImForwardingPackingDetail AS GdD ON GdH.Id = GdD.ExImForwardingId"
         + " LEFT JOIN InvCropYear crop on GdD.CropYearId = crop.Id"
         + " LEFT JOIN GatePassOutward Gp on Gp.Id = Gdh.GatePassOutwardId"
         + " Where GdH.OrganizationId = :organizationId and"
         + " GdH.CompanyId = :companyId and"
         + " GdH.Id = :refDocIdNo", nativeQuery = true)
    int insertExportForwarding(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (205,206) -- Export Goods Forwarding Other Items. needs :whId from readConfigWarehouseId(). Proc line 1186. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select GdH.OrganizationId, GdH.CompanyId, GdH.BranchesId, GdH.ProjectsId, GdH.DocumentTypeId, GdH.Id, GdH.DocNo, GdH.DocDate"
         + " , GdH.SupplierCustomerId, GdH.IsApproved, GdD.Id,:whId,0, GdD.otherItemId, 0,''"
         + " , 0, 0,GdD.oItemQty, 0,GdD.oItemQty, 0, GdD.oItemQty"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Export Goods Forwarding Other Items'"
         + " ,GdH.EntryDate,GdH.EntryUser ,GdH.ModifyDate,GdH.ModifyUser ,"
         + " GdD.oItemWeightKgs,0,0,0,0,0 ,GdH.GpNo,GdH.VehicleNo,GdD.ItemConditionId,GdD.RackId"
         + " FROM dbo.ExImForwarding AS GdH"
         + " INNER JOIN dbo.ExImForwardingOtherItems AS GdD ON GdH.Id = GdD.ExImForwardingId"
         + " Where GdH.OrganizationId = :organizationId and"
         + " GdH.CompanyId = :companyId and"
         + " GdH.Id = :refDocIdNo", nativeQuery = true)
    int insertExportForwardingOtherItems(@Param("whId") Integer whId, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 210 -- Export Transfer - OUT. Proc line 1212. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo,ValidForSale)"
         + " Select GdH.OrganizationId, GdH.CompanyId, GdH.BranchesId, GdH.ProjectsId, GdH.DocumentTypeId, GdH.Id, GdH.DocNo, GdH.DocDate"
         + " , GdH.SupplierCustomerId, GdH.IsApproved, GdD.Id, GdD.WarehouseId, GdD.LotJobId, GdD.ItemId, GdD.UOMScheduleIdOuter,crop.CropYear"
         + " , GdD.PackingMaterialId,0,GdD.OuterQty,0,GdD.NetWeight, 0, GdD.NetWeight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Export Transfer'"
         + " ,GdH.EntryDate,GdH.EntryUser ,GdH.ModifyDate,GdH.ModifyUser ,"
         + " GdD.GrossWeight,GdD.EbUnit,GdD.EbTotal,0,0,GdD.AdLsWeight ,GdH.GpNo,GdH.VehicleNo,1"
         + " FROM ExImForwarding AS GdH"
         + " INNER JOIN dbo.ExImForwardingPackingDetail AS GdD ON GdH.Id = GdD.ExImForwardingId and GdD.ActionTypeId <> 3"
         + " LEFT JOIN InvCropYear crop on GdD.CropYearId = crop.Id"
         + " Where GdH.OrganizationId = :organizationId and"
         + " GdH.CompanyId = :companyId and"
         + " GdH.Id = :refDocIdNo", nativeQuery = true)
    int insertExportTransferOut(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 210 -- Export Goods Forwarding Other Items - OUT. Proc line 1234. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select GdH.OrganizationId, GdH.CompanyId, GdH.BranchesId, GdH.ProjectsId, GdH.DocumentTypeId, GdH.Id, GdH.DocNo, GdH.DocDate"
         + " , GdH.SupplierCustomerId, GdH.IsApproved, GdD.Id,GdD.WareHouseFromId,0, GdD.otherItemId, 0,''"
         + " , 0, 0,GdD.oItemQty, 0,GdD.oItemQty, 0, GdD.oItemQty"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Export Goods Forwarding Other Items'"
         + " ,GdH.EntryDate,GdH.EntryUser ,GdH.ModifyDate,GdH.ModifyUser ,"
         + " GdD.oItemWeightKgs,0,0,0,0,0,GdH.GpNo,GdH.VehicleNo"
         + " FROM dbo.ExImForwarding AS GdH"
         + " INNER JOIN dbo.ExImForwardingOtherItems AS GdD ON GdH.Id = GdD.ExImForwardingId"
         + " Where GdH.OrganizationId = :organizationId and"
         + " GdH.CompanyId = :companyId and"
         + " GdH.Id = :refDocIdNo and"
         + " gdh.ActionId <> 3", nativeQuery = true)
    int insertExportTransferOutOtherItems(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 210 -- Export Transfer - IN. Proc line 1255. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo,ValidForSale)"
         + " Select GdH.OrganizationId, GdH.CompanyId, GdH.BranchesId, GdH.ProjectsId, GdH.DocumentTypeId, GdH.Id, GdH.DocNo, GdH.DocDate"
         + " , GdH.SupplierCustomerId, GdH.IsApproved, GdD.Id, GdD.WarehouseToId, GdD.LotJobId, GdD.ItemId, GdD.UOMScheduleIdOuter,crop.CropYear"
         + " , GdD.PackingMaterialId,GdD.OuterQty,0,GdD.NetWeight,0,GdD.NetWeight,0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Export Transfer'"
         + " ,GdH.EntryDate,GdH.EntryUser ,GdH.ModifyDate,GdH.ModifyUser ,"
         + " GdD.GrossWeight,GdD.EbUnit,GdD.EbTotal,0,0,GdD.AdLsWeight ,GdH.GpNo,GdH.VehicleNo,1"
         + " FROM ExImForwarding AS GdH"
         + " INNER JOIN dbo.ExImForwardingPackingDetail AS GdD ON GdH.Id = GdD.ExImForwardingId and GdD.ActionTypeId <> 3"
         + " LEFT JOIN InvCropYear crop on GdD.CropYearId = crop.Id"
         + " Where GdH.OrganizationId = :organizationId and"
         + " GdH.CompanyId = :companyId and"
         + " GdH.Id = :refDocIdNo", nativeQuery = true)
    int insertExportTransferIn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 210 -- Export Goods Forwarding Other Items - IN. Proc line 1276. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select GdH.OrganizationId, GdH.CompanyId, GdH.BranchesId, GdH.ProjectsId, GdH.DocumentTypeId, GdH.Id, GdH.DocNo, GdH.DocDate"
         + " , GdH.SupplierCustomerId, GdH.IsApproved, GdD.Id,GdD.WareHouseToId,0, GdD.otherItemId, 0,''"
         + " , 0,GdD.oItemQty,0,GdD.oItemQty,0,GdD.oItemQty,0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Export Goods Forwarding Other Items'"
         + " ,GdH.EntryDate,GdH.EntryUser ,GdH.ModifyDate,GdH.ModifyUser ,"
         + " GdD.oItemWeightKgs,0,0,0,0,0 ,GdH.GpNo,GdH.VehicleNo"
         + " FROM dbo.ExImForwarding AS GdH"
         + " INNER JOIN dbo.ExImForwardingOtherItems AS GdD ON GdH.Id = GdD.ExImForwardingId"
         + " Where GdH.OrganizationId = :organizationId and"
         + " GdH.CompanyId = :companyId and"
         + " GdH.Id = :refDocIdNo and"
         + " gdh.ActionId <> 3", nativeQuery = true)
    int insertExportTransferInOtherItems(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 128 -- Booking Invoice (InvSaleInvoice). Proc line 1300. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , SH.SupplierCustomerId, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId, SD.CropYear"
         + " , SD.PackingTypeId, 0,SD.ItemQty, 0,SD.NetBillWeight, 0, SD.NetStockWeight"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Booking Invoice'"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.GrossWeight,SD.EBWeight,SD.EBTotalWt,SD.WeightCut,SD.WeightCutTotal,SD.AdLsWeight ,SD.GpNo,SD.VehicleNo,"
         + " SD.ItemConditionId,SD.RackId"
         + " FROM dbo.InvSaleInvoice AS SH"
         + " INNER JOIN dbo.InvSaleInvoiceDetail AS SD ON SH.Id = SD.InvSaleInvoiceId"
         + " Where SH.OrganizationId = :organizationId and"
         + " SH.CompanyId = :companyId and"
         + " SH.Id = :refDocIdNo", nativeQuery = true)
    int insertBookingInvoice(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 60 -- Purchase Invoice Return Trade Pro (InvSaleInvoice). Proc line 1325. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , SH.SupplierCustomerId, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId, SD.CropYear"
         + " , SD.PackingTypeId, 0,SD.ItemQty, 0,SD.NetBillWeight, 0, SD.NetStockWeight"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Purchase Invoice Return Trade Pro'"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.GrossWeight,SD.EBWeight,SD.EBTotalWt,SD.WeightCut,SD.WeightCutTotal,SD.AdLsWeight ,SD.GpNo,SD.VehicleNo,"
         + " sd.ItemConditionId,SD.RackId"
         + " FROM dbo.InvSaleInvoice AS SH"
         + " INNER JOIN dbo.InvSaleInvoiceDetail AS SD ON SH.Id = SD.InvSaleInvoiceId"
         + " Where SH.OrganizationId = :organizationId and"
         + " SH.CompanyId = :companyId and"
         + " SH.Id = :refDocIdNo", nativeQuery = true)
    int insertPurchaseInvoiceReturnTradePro(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 104 -- Sale Invoice Return Trade Pro (InvPurchaseInvoice). Proc line 1351. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select PH.OrganizationId, PH.CompanyId, PH.BranchesId, PH.ProjectsId, PH.DocumentTypeId, PH.Id, PH.DocNo, PH.DocDate"
         + " , PH.SupplierCustomerId, PH.IsApproved, PD.Id, PD.WarehouseId, PD.JobLotId, PD.ItemId, PD.ItemUomId, PD.CropYear"
         + " , PD.PackingTypeId, PD.ItemQty, 0, PD.NetBillWeight, 0, PD.NetBillWeight, 0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Sale Invoice Return Trade Pro'"
         + " ,PH.EntryDate,PH.EntryUser ,PH.ModifyDate,PH.ModifyUser ,"
         + " PD.GrossWeight,PD.EBWeight,PD.EBTotalWt,PD.WeightCut,PD.WeightCutTotal,PD.AdLsWeight ,PD.GpNo,PD.VehicleNo"
         + " ,PD.ItemConditionId,PD.RackId"
         + " FROM dbo.InvPurchaseInvoice AS PH"
         + " INNER JOIN dbo.InvPurchaseInvoiceDetail AS PD ON PH.Id = PD.InvPurchaseInvoiceId"
         + " Where PH.OrganizationId = :organizationId and"
         + " PH.CompanyId = :companyId and"
         + " PH.Id = :refDocIdNo", nativeQuery = true)
    int insertSaleInvoiceReturnTradePro(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 63 -- Purchase Invoice Trade Pro. Proc line 1379. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select PH.OrganizationId, PH.CompanyId, PH.BranchesId, PH.ProjectsId, PH.DocumentTypeId, PH.Id, PH.DocNo, PH.DocDate"
         + " , PH.SupplierCustomerId, PH.IsApproved, PD.Id, PD.WarehouseId, PD.JobLotId, PD.ItemId, PD.ItemUomId, PD.CropYear"
         + " , PD.PackingTypeId, PD.ItemQty, 0, PD.NetBillWeight, 0, PD.NetStockWeight, 0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Purchase Invoice Trade Pro'"
         + " ,PH.EntryDate,PH.EntryUser ,PH.ModifyDate,PH.ModifyUser ,"
         + " PD.GrossWeight,PD.EBWeight,PD.EBTotalWt,PD.WeightCut,PD.WeightCutTotal,PD.AdLsWeight ,PD.GpNo,PD.VehicleNo"
         + " ,PD.ItemConditionId,PD.RackId"
         + " FROM dbo.InvPurchaseInvoice AS PH INNER JOIN dbo.InvPurchaseInvoiceDetail AS PD ON PH.Id = PD.InvPurchaseInvoiceId"
         + " Where PH.OrganizationId = :organizationId and"
         + " PH.CompanyId = :companyId and"
         + " PH.Id = :refDocIdNo", nativeQuery = true)
    int insertPurchaseInvoiceTradePro(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 234 -- Import Goods Receiving Notes (ImForwarding). Proc line 1407. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select GdH.OrganizationId, GdH.CompanyId, GdH.BranchesId, GdH.ProjectsId, GdH.DocumentTypeId, GdH.Id, GdH.DocNo, GdH.DocDate"
         + " , GdH.SupplierCustomerId, GdH.IsApproved, GdD.Id, GdD.WarehouseId, GdD.LotJobId, GdD.ItemId, GdD.UOMScheduleIdOuter, ''"
         + " , GdD.PackingMaterialId,GdD.OuterQty,0,GdD.NetWeight,0,GdD.StockWeight,0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Import Goods Receiving Notes'"
         + " ,GdH.EntryDate,GdH.EntryUser ,GdH.ModifyDate,GdH.ModifyUser ,"
         + " Gdd.GrossWeight,0,0,0,0,GdD.AdLsWeight ,Gdh.GpNo,Gdh.VehicleNo"
         + " FROM ImForwarding AS GdH INNER JOIN dbo.ImForwardingPackingDetail AS GdD ON GdH.Id = GdD.ExImForwardingId"
         + " Where GdH.OrganizationId = :organizationId and"
         + " GdH.CompanyId = :companyId and"
         + " GdH.Id = :refDocIdNo", nativeQuery = true)
    int insertImportGrn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 235 -- Import Goods Receiving Notes Without WeighBridge. Proc line 1428. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select GdH.OrganizationId, GdH.CompanyId, GdH.BranchesId, GdH.ProjectsId, GdH.DocumentTypeId, GdH.Id, GdH.DocNo, GdH.DocDate"
         + " , GdH.SupplierCustomerId, GdH.IsApproved, GdD.Id, GdD.WarehouseId, GdD.LotJobId, GdD.ItemId, GdD.UOMScheduleIdOuter, ''"
         + " , GdD.PackingMaterialId,GdD.OuterQty,0,GdD.NetWeight,0,GdD.StockWeight,0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Import Goods Receiving Notes Without WeighBridge'"
         + " ,GdH.EntryDate,GdH.EntryUser ,GdH.ModifyDate,GdH.ModifyUser ,"
         + " GdD.GrossWeight,0,0,0,0,GdD.AdLsWeight ,GdH.GpNo,GdH.VehicleNo"
         + " FROM ImForwarding AS GdH INNER JOIN dbo.ImForwardingPackingDetail AS GdD ON GdH.Id = GdD.ExImForwardingId"
         + " Where GdH.OrganizationId = :organizationId and"
         + " GdH.CompanyId = :companyId and"
         + " GdH.Id = :refDocIdNo", nativeQuery = true)
    int insertImportGrnWithoutWeighBridge(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 116 -- Whole and Retail SaleInvoice Return. uses dbo.GetEquivalentByItemIdandBaseUnitId() and view vItemAllocation. Proc line 1450. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " SELECT DISTINCT SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , SH.SupplierCustomerId, SH.IsApproved, SD.Id, sd.WarehouseId, 0, SD.ItemId,0, ''"
         + " , 0,SD.ItemQty,0,SD.ItemQty * [dbo].[GetEquivalentByItemIdandBaseUnitId](sh.OrganizationId,sh.CompanyId,sd.ItemId,i.BaseUnitId),0"
         + " ,SD.ItemQty * [dbo].[GetEquivalentByItemIdandBaseUnitId](sh.OrganizationId,sh.CompanyId,sd.ItemId,i.BaseUnitId),0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Whole and Retail SaleInvoice Return'"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.ItemQty,0,0,0,0,0,0,''"
         + " FROM dbo.WsRmSaleInvoiceReturn AS SH INNER JOIN dbo.WsRmSaleInvoiceReturnDetail AS SD ON SH.Id = SD.WsRmSaleInvoiceReturnId"
         + " inner join dbo.vItemAllocation i on sd.ItemId = i.Id"
         + " Where SH.OrganizationId = :organizationId and"
         + " SH.CompanyId = :companyId and"
         + " SH.Id = :refDocIdNo", nativeQuery = true)
    int insertWsRmSaleInvoiceReturn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 238 -- Stock Transfered From Warehouse. Proc line 1474. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select h.OrganizationId,h.CompanyId,0,0,h.DocumentTypeId,h.Id,h.DocNo,h.DocDate"
         + " ,h.CompanyId,h.IsApproved,D.Id,d.WareHouseFromId,0,d.ItemId,d.ItemUomId,''"
         + " ,0,0,d.ItemQty,0,d.NetWeight,0,d.NetWeight"
         + " ,'Qty',d.ItemRate,0,0,0,d.ItemAmount,'Stock Transfered From Warehouse'"
         + " ,H.EntryDate,H.EntryUser ,H.ModifyDate,H.ModifyUser ,"
         + " D.NetWeight,0,0,0,0,0 ,0,''"
         + " FROM WsRmWareHouseToWareHouseStocTransferHeader h"
         + " INNER JOIN WsRmWareHouseToWareHouseStocTransferDetail d on d.WsRmStockTransferId=h.Id"
         + " Where h.OrganizationId=:organizationId and"
         + " h.CompanyId=:companyId and"
         + " h.Id=:refDocIdNo", nativeQuery = true)
    int insertWsRmWarehouseTransferOut(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 238 -- Stock Transfered In Warehouse. Proc line 1492. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select h.OrganizationId,h.CompanyId,0,0,h.DocumentTypeId,h.Id,h.DocNo,h.DocDate"
         + " ,h.CompanyId,h.IsApproved,D.Id,d.WareHouseToId,0,d.ItemId,d.ItemUomId,''"
         + " ,0,d.ItemQty,0,d.NetWeight,0,d.NetWeight,0"
         + " ,'Qty',d.ItemRate,0,d.ItemAmount,0,0,'Stock Transfered In Warehouse'"
         + " ,H.EntryDate,H.EntryUser ,H.ModifyDate,H.ModifyUser ,"
         + " D.NetWeight,0,0,0,0,0 ,0,''"
         + " FROM WsRmWareHouseToWareHouseStocTransferHeader h"
         + " INNER JOIN WsRmWareHouseToWareHouseStocTransferDetail d on d.WsRmStockTransferId=h.Id"
         + " Where h.OrganizationId=:organizationId and"
         + " h.CompanyId=:companyId and"
         + " h.Id=:refDocIdNo", nativeQuery = true)
    int insertWsRmWarehouseTransferIn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 136 -- ProductionCumPackingMaterialConsumption. Proc line 1513. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.VarietyItemId, Grh.IsApproved, grd.Id,0, 0, Grd.PmItemId,GrH.PackSizeId, GrH.CropYear"
         + " ,GrH.PackTypeId, 0,ISNULL(Grd.Qty,0),0,ISNULL(Grd.Qty,0), 0, ISNULL(Grd.Qty,0)"
         + " , 'Qty', 0, 0, 0 , 0, 0,'ProductionCumPackingMaterialConsumption'"
         + " ,GrH.EntryDate,GrH.EntryUserId ,GrH.ModifyDate,GrH.ModifyUserId ,"
         + " GrD.Weight,0,0,0,0,0 ,0,''"
         + " FROM dbo.InvProductionCumPackMaterialConsumptionHeader AS GrH"
         + " INNER JOIN InvProductionCumPackMaterialConsumptionDetail AS GrD ON GrH.Id = GrD.InvProductionCumPackMaterialConsumptionHeaderId and GrD.ActionTypeId <> 3"
         + " Where grh.OrganizationId = :organizationId and"
         + " grh.CompanyId = :companyId and"
         + " grh.Id = :refDocIdNo", nativeQuery = true)
    int insertProductionCumPackingMaterialConsumption(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 137 -- Goods Receipts Notes Direct. Proc line 1536. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess,EbPurAgainstWeight,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.SupplierCustomerId, Grh.IsApproved, grd.Id, grd.WarehouseId, Grd.JobLotId, Grd.ItemId, Grd.ItemUomId, Grd.CropYear"
         + " , Grd.PackingTypeId, Grd.ItemQty, 0, GrD.NetBillWeight, 0, Grd.StockWeight, 0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Goods Receipts Notes Direct',grd.CityId"
         + " ,Grh.EntryDate,Grh.EntryUser ,Grh.ModifyDate,Grh.ModifyUser ,"
         + " Grd.GrossWeight,Grd.EBWPerUnit,Grd.EBWTotal,Grd.WtCut,Grd.WtCutTotal ,Grd.AdLsWeight,grd.EbPurAgainstWeight,Grh.GpNo,Grh.VehicleNo,"
         + " GrD.ItemConditionId,GrD.RackId"
         + " FROM dbo.InvGrn AS GrH"
         + " INNER JOIN dbo.InvGrnDetail AS GrD ON GrH.Id = GrD.InvGrnId"
         + " Where grh.OrganizationId = :organizationId and"
         + " grh.CompanyId = :companyId and"
         + " grh.Id = :refDocIdNo", nativeQuery = true)
    int insertGrnDirect(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 140 -- RETURN TO STORE (InvStoreReturn). Proc line 1562. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId,SecondaryUomQty,SecondaryUomId,SecondaryUomItemRate"
         + " )"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrD.CrAccountId, Grh.IsApproved, grd.Id, grd.WarehouseId, Grd.AssetsId, Grd.ItemId, Grd.ItemUomId, ''"
         + " , Grd.DepartmentId, Grd.ItemQty, 0, GrD.ItemQty, 0, Grd.ItemQty, 0"
         + " , 'QTY', 0, 0, 0 , 0, 0,'RETURN TO STORE'"
         + " ,GrH.EntryDate,GrH.EntryUser ,GrH.ModifyDate,GrH.ModifyUser ,"
         + " GrD.ItemQty,0,0,0,0,0,0,'',GrD.ItemConditionId,GrD.RackId,Grd.SecondaryUomQty,Grd.SecondaryUomId,GrD.SecondaryUomItemRate"
         + " FROM dbo.InvStoreReturn AS GrH"
         + " INNER JOIN dbo.InvStoreReturnDetail AS GrD ON GrH.Id = GrD.InvStoreReturnId"
         + " Where grh.OrganizationId = :organizationId"
         + " and grh.CompanyId = :companyId"
         + " and grh.Id = :refDocIdNo", nativeQuery = true)
    int insertStoreReturn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (70,215) -- Gain Stock Adjustment (AdjustmentTypeId = 1). Proc line 1587. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions]"
         + " ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.AdjustmentTypeId, Grh.IsApproved, grd.Id, grd.WarehouseId, Grd.JobLotId, Grd.ItemId, Grd.PackUomId, cr.CropYear"
         + " , Grd.PackingTypeId, Grd.Qty, 0, GrD.NetWeight, 0, Grd.NetWeight, 0"
         + " , CASE WHEN :refDocumentTypeId = 70 THEN 'Weight' ELSE 'Qty' END, 0, 0, 0 , 0, 0"
         + " , CASE WHEN :refDocumentTypeId = 70 THEN 'Gain Stock Adjustment' ELSE 'Gain Stock Adjustment_PM' END"
         + " ,GrH.EnteryDate,GrH.EnteryUserId ,GrH.ModifyDate,GrH.ModifyUserId ,"
         + " GrD.NetWeight,0,0,0,0,0,0,'',GrD.ItemConditionId,GrD.RackId"
         + " FROM dbo.InvStockAdjustment AS GrH"
         + " INNER JOIN dbo.InvStockAdjustmentDetail AS GrD ON GrH.Id = GrD.InvStockAdjustmentId"
         + " LEFT JOIN InvCropYear cr on GrD.CropYearId = cr.Id"
         + " Where grh.OrganizationId = :organizationId"
         + " and grh.CompanyId = :companyId"
         + " and grh.Id = :refDocIdNo"
         + " AND GrH.AdjustmentTypeId = 1", nativeQuery = true)
    int insertStockAdjustmentGain(@Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (70,215) -- Loss Stock Adjustment (AdjustmentTypeId = 2). Proc line 1613. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions]"
         + " ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.AdjustmentTypeId, Grh.IsApproved, grd.Id, grd.WarehouseId, Grd.JobLotId, Grd.ItemId, Grd.PackUomId, cr.CropYear"
         + " , Grd.PackingTypeId,0,Grd.Qty,0,GrD.NetWeight,0,Grd.NetWeight"
         + " , CASE WHEN :refDocumentTypeId = 70 THEN 'Weight' ELSE 'Qty' END, 0, 0, 0 , 0, 0"
         + " , CASE WHEN :refDocumentTypeId = 70 THEN 'Loss Stock Adjustment' ELSE 'Loss Stock Adjustment_PM' END"
         + " ,GrH.EnteryDate,GrH.EnteryUserId ,GrH.ModifyDate,GrH.ModifyUserId ,"
         + " GrD.NetWeight,0,0,0,0,0,0,'',GrD.ItemConditionId,GrD.RackId"
         + " FROM dbo.InvStockAdjustment AS GrH"
         + " INNER JOIN dbo.InvStockAdjustmentDetail AS GrD ON GrH.Id = GrD.InvStockAdjustmentId"
         + " LEFT JOIN InvCropYear cr on GrD.CropYearId = cr.Id"
         + " Where grh.OrganizationId = :organizationId"
         + " and grh.CompanyId = :companyId"
         + " and grh.Id = :refDocIdNo"
         + " AND GrH.AdjustmentTypeId = 2", nativeQuery = true)
    int insertStockAdjustmentLoss(@Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (70,215) -- Auto Stock Adjustment (AdjustmentTypeId = 4). no audit columns written. Proc line 1640. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions]"
         + " ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId]"
         + " ,[QtyIn]"
         + " ,[QtyOut]"
         + " ,[BillWeightIn]"
         + " ,[BillWeightOut]"
         + " ,[StockWeightIn]"
         + " ,[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,ItemConditionId"
         + " ,RackId"
         + " )"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.AdjustmentTypeId, Grh.IsApproved, grd.Id, grd.WarehouseId, Grd.JobLotId, Grd.ItemId, Grd.PackUomId, cr.CropYear"
         + " , Grd.PackingTypeId"
         + " ,CASE"
         + " WHEN grd.NetWeight < 0 and ISNULL(grd.Qty, 0) > 0 THEN -ISNULL(grd.Qty, 0)"
         + " WHEN grd.NetWeight < 0 and ISNULL(grd.Qty, 0) < 0 THEN ABS(ISNULL(grd.Qty, 0))"
         + " ELSE 0 END"
         + " ,CASE WHEN Grd.NetWeight > 0 THEN ISNULL(Grd.Qty, 0) ELSE 0 END"
         + " ,CASE WHEN GrD.NetWeight < 0 THEN ABS(ISNULL(GrD.NetWeight,0)) ELSE 0 END"
         + " ,CASE WHEN Grd.NetWeight > 0 THEN ISNULL(Grd.NetWeight,0) ELSE 0 END"
         + " ,CASE WHEN GrD.NetWeight < 0 THEN ABS(ISNULL(GrD.NetWeight,0)) ELSE 0 END"
         + " ,CASE WHEN Grd.NetWeight > 0 THEN ISNULL(Grd.NetWeight,0) ELSE 0 END"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Auto Stock Adjustment'"
         + " , GrD.ItemConditionId"
         + " , GrD.RackId"
         + " FROM dbo.InvStockAdjustment AS GrH"
         + " INNER JOIN dbo.InvStockAdjustmentDetail AS GrD ON GrH.Id = GrD.InvStockAdjustmentId"
         + " LEFT JOIN InvCropYear cr on GrD.CropYearId = cr.Id"
         + " Where grh.OrganizationId = :organizationId"
         + " and grh.CompanyId = :companyId"
         + " and grh.Id = :refDocIdNo"
         + " AND GrH.AdjustmentTypeId = 4", nativeQuery = true)
    int insertStockAdjustmentAuto(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1501 -- Goods Receipts Notes (ST.InvGrn). Proc line 1688. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.SupplierCustomerId, Grh.IsApproved, grd.Id, grd.WarehouseId, Grd.JobLotId, Grd.ItemId, Grd.ItemUomId,''"
         + " , Grd.PackingTypeId, Grd.ItemQty, 0, GrD.NetBillWeight, 0, Grd.StockWeight, 0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Goods Receipts Notes',grd.CityId"
         + " ,GrH.EntryDate,GrH.EntryUserId ,GrH.ModifyDate,GrH.ModifyUserId ,"
         + " GrD.GrossWeight,0,0,0,0,GrD.AdLsWeight ,Grh.GpNo,Grh.VehicleNo"
         + " FROM ST.InvGrn AS GrH"
         + " INNER JOIN ST.InvGrnDetail AS GrD ON GrH.Id = GrD.InvGrnId and GrD.ActionTypeId <> 3"
         + " Where grh.OrganizationId = :organizationId"
         + " and grh.CompanyId = :companyId"
         + " and grh.Id = :refDocIdNo", nativeQuery = true)
    int insertSteelGrn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1503 -- Purchase Invoice Direct (ST.InvPurchaseInvoice). Proc line 1711. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch],[CityId]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select PH.OrganizationId, PH.CompanyId, PH.BranchesId, PH.ProjectsId, PH.DocumentTypeId, PH.Id, PH.DocNo, PH.DocDate"
         + " , PH.SupplierCustomerId, PH.IsApproved, PD.Id, PD.WarehouseId, PD.JobLotId, PD.ItemId, PD.ItemUomId,'',PD.CityId"
         + " , PD.PackingTypeId, PD.ItemQty, 0, PD.NetBillWeight, 0, PD.NetStockWeight, 0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Purchase Invoice Direct'"
         + " ,PH.EntryDate,PH.EntryUserId ,PH.ModifyDate,PH.ModifyUserId ,"
         + " PD.GrossWeight,0,0,0,0,PD.AdLsWeight ,PD.GpNo,PD.VehicleNo"
         + " FROM ST.InvPurchaseInvoice AS PH"
         + " INNER JOIN ST.InvPurchaseInvoiceDetail AS PD ON PH.Id = PD.InvPurchaseInvoiceId AND PD.ActionTypeId <> 3"
         + " Where PH.OrganizationId = :organizationId and"
         + " PH.CompanyId = :companyId and"
         + " PH.Id = :refDocIdNo", nativeQuery = true)
    int insertSteelPurchaseInvoiceDirect(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 808 -- VarietyChangeOutPut (EntryType = 2). Proc line 1734. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchedId, SH.ProjectsId, SH.DocTypeId, SH.Id, SH.DocSrNo, SH.DocDate"
         + " , 0, SH.PostState, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId, SD.CropBatch"
         + " , SD.PackingTypeId,SD.Qty,0,SD.Weight,0,SD.Weight,0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'VarietyChangeOutPut'"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.Weight,0,0,0,0,0 ,0,'',SD.ItemConditionId,SD.RackId"
         + " FROM dbo.InvStockConversion AS SH"
         + " INNER JOIN dbo.InvStockConversionDetail AS SD ON SH.Id = SD.InvStockConversionId"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and Sd.EntryType = 2", nativeQuery = true)
    int insertStockConversionOutput808(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 808 -- VarietyChangeIssue (EntryType = 1). Proc line 1760. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchedId, SH.ProjectsId, SH.DocTypeId, SH.Id, SH.DocSrNo, SH.DocDate"
         + " , 0, SH.PostState, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId, SD.CropBatch"
         + " , SD.PackingTypeId,0,SD.Qty,0,SD.Weight,0,SD.Weight"
         + " , 'Qty', 0, 0, 0 , 0, 0,'VarietyChangeIssue'"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.Weight,0,0,0,0,0 ,0,'',SD.ItemConditionId,SD.RackId"
         + " FROM dbo.InvStockConversion AS SH"
         + " INNER JOIN dbo.InvStockConversionDetail AS SD ON SH.Id = SD.InvStockConversionId"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and Sd.EntryType = 1", nativeQuery = true)
    int insertStockConversionIssue808(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 177 -- Empty Bags Return To Store (InvStoreSendReceipt). Proc line 1786. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select h.OrganizationId, H.CompanyId, h.BranchId,0, H.DocumentTypeId, H.InvStoreSendReceiptId, H.DocNo, H.DocDate"
         + " ,h.SenderWarehouseId,0, 0,h.ReceiverWarehouseId,0, d.ItemId,d.ItemUomId,'***'"
         + " ,d.InvPackingTypeId, d.QtyIn, 0, d.QtyIn, 0,d.QtyIn, 0"
         + " , 'Qty', d.ItemRate, d.ItemUomId, d.ItemAmount , 0, 0,'Empty Bags Return To Store'"
         + " ,H.EntryDate,H.EntryUserId ,H.ApprovedDate,H.ApprovedUserId ,"
         + " D.QtyIn,0,0,0,0,0 ,0,'',d.ItemConditionId,d.RackId"
         + " FROM InvStoreSendReceipt h"
         + " INNER JOIN InvStoreSendReceiptDetail d on h.InvStoreSendReceiptId = d.InvStoreSendReceiptId"
         + " Where h.OrganizationId = :organizationId"
         + " and h.CompanyId = :companyId"
         + " and h.InvStoreSendReceiptId = :refDocIdNo", nativeQuery = true)
    int insertStoreSendReceiptEmptyBags(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1510 -- Sale Invoice Direct (ST.InvSaleInvoice). Proc line 1812. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],[CityId]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , SH.SupplierCustomerId, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.PackUOMId,'***'"
         + " , SD.PackingTypeId, 0,SD.ItemQty, 0,SD.NetBillWeight, 0, SD.NetStockWeight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Sale Invoice Direct',sd.CityId"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.GrossWeight,0,0,SD.WeightCut,SD.WeightCutTotal,SD.AdLsWeight ,SD.GpNo,SD.VehicleNo"
         + " FROM ST.InvSaleInvoice AS SH"
         + " INNER JOIN ST.InvSaleInvoiceDetail AS SD ON SH.Id = SD.InvSaleInvoiceId and sd.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertSteelSaleInvoiceDirect(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (1602,1618,1858) -- Goods Receipts Note / GRN Returned (engineering). Proc line 1837. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.SupplierCustomerId, Grh.IsApproved, grd.Id, grd.WarehouseId, Grd.JobLotId, Grd.ItemId, Grd.ItemUomId, Grd.CropYear"
         + " , Grd.PackingTypeId, Grd.ItemQty, 0, GrD.NetBillWeight, 0, Grd.StockWeight,0"
         + " , 'Weight', 0, 0, 0 , 0, 0"
         + " , CASE WHEN :refDocumentTypeId = 1618 THEN 'GRN Returned' ELSE 'Goods Receipts Note' END"
         + " ,grd.CityId"
         + " ,GrH.EntryDate,GrH.EntryUser ,GrH.ModifyDate,GrH.ModifyUser ,"
         + " GrD.GrossWeight,GrD.EBWPerUnit,GrD.EBWTotal,GrD.WtCut,GrD.WtCutTotal,GrD.AdLsWeight ,GrH.GpNo,GrH.VehicleNo"
         + " ,GrD.ItemConditionId,GrD.RackId"
         + " FROM dbo.InvGrn AS GrH"
         + " INNER JOIN dbo.InvGrnDetail AS GrD ON GrH.Id = GrD.InvGrnId"
         + " Where grh.OrganizationId = :organizationId"
         + " and grh.CompanyId = :companyId"
         + " and grh.Id = :refDocIdNo", nativeQuery = true)
    int insertEngrGrn(@Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (1602,1618,1858) -- Goods Receipts Note / GRN Returned - OUT from WareHouseFromId. only rows with WareHouseFromId > 0. Proc line 1864. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,[GPType]"
         + " ,[GpTypeSrNo]"
         + " ,ItemConditionId"
         + " ,RackId"
         + " )"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.SupplierCustomerId, Grh.IsApproved, grd.Id, grd.WareHouseFromId, Grd.JobLotId, Grd.ItemId, Grd.ItemUomId, Grd.CropYear"
         + " , Grd.PackingTypeId,0,Grd.ItemQty, 0, GrD.NetBillWeight, 0, Grd.StockWeight"
         + " , 'Weight', 0, 0, 0 , 0, 0"
         + " , CASE WHEN :refDocumentTypeId = 1618 THEN 'GRN Returned' ELSE 'Goods Receipts Note' END"
         + " , grd.CityId"
         + " , GrH.EntryDate,GrH.EntryUser ,GrH.ModifyDate,GrH.ModifyUser ,"
         + " GrD.GrossWeight,GrD.EBWPerUnit,GrD.EBWTotal,GrD.WtCut,GrD.WtCutTotal,GrD.AdLsWeight ,GrH.GpNo,GrH.VehicleNo,"
         + " gp.GatepassType,"
         + " gp.GpTypeSrNo,"
         + " GrD.ItemConditionId,"
         + " GrD.RackId"
         + " FROM dbo.InvGrn AS GrH"
         + " INNER JOIN dbo.InvGrnDetail AS GrD ON GrH.Id = GrD.InvGrnId"
         + " LEFT JOIN GatePassInward Gp on Gp.Id = Grh.InwardGatePassId"
         + " Where grh.OrganizationId = :organizationId"
         + " and grh.CompanyId = :companyId"
         + " and grh.Id = :refDocIdNo"
         + " and Isnull(grd.WareHouseFromId,0) > 0", nativeQuery = true)
    int insertEngrGrnWarehouseFrom(@Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (1604,1654) -- Purchase Invoice Direct/Against Order (engineering). Proc line 1901. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId"
         + " ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,VarientId,ProductionStageId,CastingTypeId"
         + " )"
         + " Select PH.OrganizationId, PH.CompanyId, PH.BranchesId, PH.ProjectsId, PH.DocumentTypeId, PH.Id, PH.DocNo, PH.DocDate"
         + " , PH.SupplierCustomerId, PH.IsApproved, PD.Id, PD.WarehouseId, PD.JobLotId, PD.ItemId, PD.ItemUomId, PD.CropYear"
         + " , PD.PackingTypeId, PD.ItemQty, 0, PD.NetBillWeight, 0, PD.NetStockWeight, 0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Purchase Invoice Direct/Against Order'"
         + " ,PH.EntryDate,PH.EntryUser ,PH.ModifyDate,PH.ModifyUser"
         + " ,PD.GrossWeight,PD.EBWeight,PD.EBTotalWt,PD.WeightCut,PD.WeightCutTotal,PD.AdLsWeight ,PD.GpNo,PD.VehicleNo"
         + " ,pd.AttributeVariantId,pd.ProductionStageId,pd.CastingTypeId"
         + " FROM dbo.InvPurchaseInvoice AS PH"
         + " INNER JOIN dbo.InvPurchaseInvoiceDetail AS PD ON PH.Id = PD.InvPurchaseInvoiceId"
         + " Where PH.OrganizationId = :organizationId and"
         + " PH.CompanyId = :companyId and"
         + " PH.Id = :refDocIdNo", nativeQuery = true)
    int insertEngrPurchaseInvoice(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 211 -- Export Packing List (ExImInvoice). Proc line 1928. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select GdH.OrganizationId, GdH.CompanyId, GdH.BranchesId, GdH.ProjectsId, GdH.DocumentTypeId, GdH.Id, GdH.DocCode, GdH.DocDate"
         + " , GdH.SupplierCustomerId, GdH.IsApproved,GdD.Id, GdD.WarehouseId, GdD.JobLotId, GdD.ItemId, d.OuterQtyUomId, c.CropYear"
         + " , d.PackingMaterialTypeId, 0,GdD.OuterQty, 0,GdD.NetWeight, 0, GdD.NetWeight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Export Packing List'"
         + " ,GdH.EntryDate,GdH.EntryUser ,GdH.ModifyDate,GdH.ModifyUser ,"
         + " GdD.GrossWeight,GdD.PackingWeight,GdD.PackingWeightTotal,0,0,0 ,0,''"
         + " FROM dbo.ExImInvoice AS GdH"
         + " INNER JOIN dbo.ExImPackingListDetail AS GdD ON GdH.Id = GdD.ExImInvoiceId"
         + " INNER JOIN ExImInvoicePackingDetail D ON GdD.ExImInvoicePackingDetailId = d.Id"
         + " INNER JOIN InvCropYear c on d.CropYearId = c.Id"
         + " Where GdH.OrganizationId = :organizationId and"
         + " GdH.CompanyId = :companyId and"
         + " GdH.Id = :refDocIdNo", nativeQuery = true)
    int insertExportPackingList(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1512 -- Production Input Steel. Proc line 1952. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocCode, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.PackUomId,''"
         + " , SD.PackingTypeId,0,SD.Qty,0,SD.Weight,0,SD.Weight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Production Input Steel'"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.Weight,0,0,0,0,0 ,0,''"
         + " FROM ST.InvFoodProduction AS SH"
         + " INNER JOIN ST.InvFoodProductionDetail AS SD ON SH.Id = SD.FoodProductionId and sd.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertSteelProductionInput(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1512 -- Production Input WIP Steel. Proc line 1976. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocCode, SH.DocDate"
         + " , SH.WIPAccountId, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SH.WIPItemId, SD.PackUomId,''"
         + " , SD.PackingTypeId,SD.Qty,0,SD.Weight,0,SD.Weight,0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Production Input WIP Steel'"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.Weight,0,0,0,0,0,0,''"
         + " FROM ST.InvFoodProduction AS SH"
         + " INNER JOIN ST.InvFoodProductionDetail AS SD ON SH.Id = SD.FoodProductionId AND SD.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertSteelProductionInputWip(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1513 -- Production OutPut Steel. Proc line 2002. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocCode, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.PackUomId,''"
         + " , SD.PackingTypeId,SD.Qty,0,SD.Weight,0,SD.Weight,0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Production OutPut Steel'"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.Weight,0,0,0,0,0,0,''"
         + " FROM ST.InvFoodProduction AS SH"
         + " INNER JOIN ST.InvFoodProductionDetail AS SD ON SH.Id = SD.FoodProductionId AND SD.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertSteelProductionOutput(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1513 -- InvFoodProduction OutPut WIP Steel. Proc line 2026. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocCode, SH.DocDate"
         + " , SH.WIPAccountId, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SH.WIPItemId, SD.PackUomId, ''"
         + " , SD.PackingTypeId,0,SD.Qty,0,SD.Weight,0,SD.Weight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'InvFoodProduction OutPut WIP Steel'"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.Weight,0,0,0,0,0,0,''"
         + " FROM ST.InvFoodProduction AS SH"
         + " INNER JOIN ST.InvFoodProductionDetail AS SD ON SH.Id = SD.FoodProductionId AND SD.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertSteelProductionOutputWip(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (1612,144,1202) -- Goods Dispatch Note / Purchae Return_Gdn. Proc line 2050. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId,SecondaryUomQty,SecondaryUomId"
         + " )"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.SupplierCustomerId, Grh.IsApproved, grd.Id, grd.WarehouseId, Grd.JobLotId, Grd.ItemId, Grd.ItemUomId, Grd.CropYear"
         + " , Grd.PackingTypeId,0,Grd.ItemQty, 0, GrD.ItemQty, 0, Grd.ItemQty"
         + " , 'Qty', 0, 0, 0 , 0, 0"
         + " ,CASE WHEN :refDocumentTypeId = 144 THEN 'Purchae Return_Gdn' ELSE 'Goods Dispatch Note' END"
         + " ,grd.CityId"
         + " ,GrH.EntryDate,GrH.EntryUser ,GrH.ModifyDate,GrH.ModifyUser ,"
         + " GrD.GrossWeight,GrD.EBWPerUnit,GrD.EBWTotal,GrD.WtCut,GrD.WtCutTotal,GrD.AdLsWeight ,GrD.GpNo,GrD.VehicleNo"
         + " ,GrD.ItemConditionId,GrD.RackId,GrD.SecondaryUomQty,GrD.SecondaryUomId"
         + " FROM dbo.InvGdn AS GrH"
         + " INNER JOIN dbo.InvGdnDetail AS GrD ON GrH.Id = GrD.InvGdnId"
         + " Where grh.OrganizationId = :organizationId"
         + " and grh.CompanyId = :companyId"
         + " and grh.Id = :refDocIdNo", nativeQuery = true)
    int insertEngrGdn(@Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (1612,144,1202) -- Goods Dispatch Note / Purchae Return_Gdn - IN to WareHouseToId. only rows with WareHouseToId > 0. Proc line 2077. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.SupplierCustomerId, Grh.IsApproved, grd.Id, grd.WareHouseToId, Grd.JobLotId, Grd.ItemId, Grd.ItemUomId, Grd.CropYear"
         + " , Grd.PackingTypeId,Grd.ItemQty,0,GrD.ItemQty,0,Grd.ItemQty,0"
         + " , 'Qty', 0, 0, 0 , 0, 0"
         + " ,CASE WHEN :refDocumentTypeId = 144 THEN 'Purchae Return_Gdn' ELSE 'Goods Dispatch Note' END"
         + " ,grd.CityId"
         + " , GrH.EntryDate,GrH.EntryUser ,GrH.ModifyDate,GrH.ModifyUser ,"
         + " GrD.GrossWeight,GrD.EBWPerUnit,GrD.EBWTotal,GrD.WtCut,GrD.WtCutTotal,GrD.AdLsWeight ,GrD.GpNo,GrD.VehicleNo,"
         + " GrD.ItemConditionId,GRD.RackId"
         + " FROM dbo.InvGdn AS GrH"
         + " INNER JOIN dbo.InvGdnDetail AS GrD ON GrH.Id = GrD.InvGdnId"
         + " Where grh.OrganizationId = :organizationId"
         + " and grh.CompanyId = :companyId"
         + " and grh.Id = :refDocIdNo"
         + " and ISNULL(grd.WareHouseToId,0) > 0", nativeQuery = true)
    int insertEngrGdnWarehouseTo(@Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (1611,1662) -- Sale Invoice return (InvSaleInvoice, engineering). Proc line 2110. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess"
         + " ,GpNo,VehicleNo"
         + " ,VarientId,ProductionStageId,CastingTypeId,ItemConditionId,RackId"
         + " )"
         + " Select PH.OrganizationId, PH.CompanyId, PH.BranchesId, PH.ProjectsId, PH.DocumentTypeId, PH.Id, PH.DocNo, PH.DocDate"
         + " , PH.SupplierCustomerId, PH.IsApproved, PD.Id, PD.WarehouseId, PD.JobLotId, PD.ItemId, PD.ItemUomId, PD.CropYear"
         + " , PD.PackingTypeId, PD.ItemQty, 0, PD.NetBillWeight, 0, PD.NetStockWeight, 0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Sale Invoice return',pd.CityId"
         + " ,PH.EntryDate,PH.EntryUser ,PH.ModifyDate,PH.ModifyUser"
         + " ,PD.GrossWeight,PD.EBWeight,PD.EBTotalWt,PD.WeightCut,PD.WeightCutTotal,PD.AdLsWeight"
         + " ,PD.GpNo,PD.VehicleNo"
         + " ,pd.ItemVariantId,pd.ProductionStageId,pd.CastingTypeId,PD.ItemConditionId,PD.RackId"
         + " FROM dbo.InvSaleInvoice AS PH"
         + " INNER JOIN dbo.InvSaleInvoiceDetail AS PD ON PH.Id = PD.InvSaleInvoiceId"
         + " Where PH.OrganizationId = :organizationId and"
         + " PH.CompanyId = :companyId and"
         + " PH.Id = :refDocIdNo", nativeQuery = true)
    int insertEngrSaleInvoiceReturn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1616 -- Store Issuance FromDepartment - OUT (DepartmentId). Proc line 2138. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId"
         + " )"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.DepartmentId,0, SD.ItemId, SD.ItemUomSchId, ''"
         + " , 0,0,SD.IssueQty, 0,SD.IssueQty, 0, SD.IssueQty"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Store Issuance FromDepartment'"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.IssueQty,0,0,0,0,0 ,0,'',SD.ItemConditionId,SD.RackId"
         + " FROM InvGsStoreIssuanceHeader AS SH"
         + " INNER JOIN InvGsStoreIssuanceDetail AS SD ON SH.Id = SD.InvGsStoreIssuanceHeaderId AND ISNULL(ActionTypeId,0) <> 3"
         + " Where SH.OrganizationId = :organizationId and"
         + " SH.CompanyId = :companyId and"
         + " SH.Id = :refDocIdNo", nativeQuery = true)
    int insertStoreIssuanceFromDepartment(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1616 -- Store Issuance FromDepartment - IN (DepartmentToId). Proc line 2163. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,ItemConditionId,RackId)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.DepartmentToId,0, SD.ItemId, SD.ItemUomSchId, ''"
         + " , 0,SD.IssueQty,0,SD.IssueQty,0,SD.IssueQty,0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Store Issuance FromDepartment'"
         + " ,SH.EntryDate,SH.EntryUser ,SH.ModifyDate,SH.ModifyUser ,"
         + " SD.IssueQty,0,0,0,0,0 ,0,'',sd.ItemConditionId,SD.RackId"
         + " FROM InvGsStoreIssuanceHeader AS SH"
         + " INNER JOIN InvGsStoreIssuanceDetail AS SD ON SH.Id = SD.InvGsStoreIssuanceHeaderId AND ISNULL(ActionTypeId,0) <> 3"
         + " Where SH.OrganizationId = :organizationId and"
         + " SH.CompanyId = :companyId and"
         + " SH.Id = :refDocIdNo", nativeQuery = true)
    int insertStoreIssuanceToDepartment(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1859 -- Purchase Invoice FG (pcc.InvPurchaseInvoice). only rows with ISNULL(InvGrnId,0)=0. Proc line 2187. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[VarientId],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select PH.OrganizationId, PH.CompanyId, PH.BranchesId, PH.ProjectsId, PH.DocumentTypeId, PH.Id, PH.DocNo, PH.DocDate"
         + " ,PH.SupplierCustomerId, PH.IsApproved, PD.Id, PD.WarehouseId, PD.JobLotId, PD.ItemId, PD.ItemAttributeVarientId,''"
         + " ,0,PD.ItemQty, 0, PD.ItemQty, 0, PD.ItemQty, 0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Purchase Invoice FG',pd.CityId"
         + " ,PH.EntryDate,PH.EntryUserId ,PH.ModifyDate,PH.ModifyUserId ,"
         + " PD.ItemNetWeight,0,0,0,0,0 ,PD.GpNo,PD.VehicleNo"
         + " FROM pcc.InvPurchaseInvoice AS PH"
         + " INNER JOIN pcc.InvPurchaseInvoiceDetail AS PD ON PH.Id = PD.InvPurchaseInvoiceId and pd.ActionTypeId <> 3"
         + " Where PH.OrganizationId = :organizationId and"
         + " PH.CompanyId = :companyId and"
         + " PH.Id = :refDocIdNo and"
         + " ISNULL(pd.InvGrnId,0) = 0", nativeQuery = true)
    int insertPccPurchaseInvoiceFg(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1865 -- Purchase Invoice FG Return (pcc). Proc line 2211. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[VarientId],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select PH.OrganizationId, PH.CompanyId, PH.BranchesId, PH.ProjectsId, PH.DocumentTypeId, PH.Id, PH.DocNo, PH.DocDate"
         + " ,PH.SupplierCustomerId, PH.IsApproved, PD.Id, PD.WarehouseId, PD.JobLotId, PD.ItemId, PD.ItemAttributeVarientId,''"
         + " ,0,0,PD.ItemQty, 0, PD.ItemQty, 0, PD.ItemQty"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Purchase Invoice FG Return',pd.CityId"
         + " ,PH.EntryDate,PH.EntryUserId ,PH.ModifyDate,PH.ModifyUserId ,"
         + " PD.ItemNetWeight,0,0,0,0,0 ,PD.GpNo,PD.VehicleNo"
         + " FROM pcc.InvPurchaseInvoice AS PH"
         + " INNER JOIN pcc.InvPurchaseInvoiceDetail AS PD ON PH.Id = PD.InvPurchaseInvoiceId and pd.ActionTypeId <> 3"
         + " Where PH.OrganizationId = :organizationId and"
         + " PH.CompanyId = :companyId and"
         + " PH.Id = :refDocIdNo", nativeQuery = true)
    int insertPccPurchaseInvoiceFgReturn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1850 -- Production InPut (pcc.ProductionHeader). Proc line 2234. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId,''"
         + " , 0,0,SD.ItemQty,0,SD.NetWeight,0,SD.NetWeight"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Production InPut'"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.NetWeight,0,0,0,0,0 ,0,''"
         + " FROM pcc.ProductionHeader AS SH"
         + " INNER JOIN pcc.ProductionInputDetail AS SD ON SH.Id = SD.ProductionHeaderId AND SD.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertPccProductionInput(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1850 -- Production OutPut (pcc, WIPWarehouseId, IsApproved=0 only). Proc line 2254. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, Sh.WIPWarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId,sd.ItemAttributeVarientId,''"
         + " , 0,SD.ItemQty,0,SD.NetWeight,0,SD.NetWeight,0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Production OutPut'"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.NetWeight,0,0,0,0,0 ,0,''"
         + " FROM pcc.ProductionHeader AS SH"
         + " INNER JOIN pcc.ProductionOutPutDetail AS SD ON SH.Id = SD.ProductionHeaderId AND SD.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and ISNULL(sh.IsApproved,0) = 0", nativeQuery = true)
    int insertPccProductionOutput(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1857 -- Production OutPut - release IN (pcc.ProductionRelease). Proc line 2278. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select r.OrganizationId, r.CompanyId, r.BranchesId, SH.ProjectsId, r.DocumentTypeId, r.Id, r.DocNo, r.DocDate"
         + " , 0,SH.IsApproved, r.Id, SD.WareHouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId,sd.ItemAttributeVarientId,''"
         + " , 0,SD.ItemQty,0,SD.ItemQty,0,SD.ItemQty,0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Production OutPut'"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.NetWeight,0,0,0,0,0 ,0,''"
         + " FROM pcc.ProductionHeader AS SH"
         + " INNER JOIN pcc.ProductionOutPutDetail AS SD ON SH.Id = SD.ProductionHeaderId AND SD.ActionTypeId <> 3"
         + " INNER JOIN pcc.ProductionRelease r on sh.Id = r.RefDocId and sh.DocumentTypeId = r.RefDocumentTypeId and sh.CompanyId = r.CompanyId"
         + " Where R.OrganizationId = :organizationId"
         + " and R.CompanyId = :companyId"
         + " and R.Id = :refDocIdNo", nativeQuery = true)
    int insertPccProductionReleaseIn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1857 -- Production OutPut - release OUT of WIPWarehouseId. Proc line 2298. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select r.OrganizationId, r.CompanyId, r.BranchesId, SH.ProjectsId, r.DocumentTypeId, r.Id, r.DocNo, r.DocDate"
         + " , 0,SH.IsApproved, r.Id,SH.WIPWarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId,sd.ItemAttributeVarientId,''"
         + " , 0,0,SD.ItemQty,0,SD.ItemQty,0,SD.ItemQty"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Production OutPut'"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.NetWeight,0,0,0,0,0 ,0,''"
         + " FROM pcc.ProductionHeader AS SH"
         + " INNER JOIN pcc.ProductionOutPutDetail AS SD ON SH.Id = SD.ProductionHeaderId AND SD.ActionTypeId <> 3"
         + " INNER JOIN pcc.ProductionRelease r on sh.Id = r.RefDocId and sh.DocumentTypeId = r.RefDocumentTypeId and sh.CompanyId = r.CompanyId"
         + " Where R.OrganizationId = :organizationId"
         + " and R.CompanyId = :companyId"
         + " and R.Id = :refDocIdNo", nativeQuery = true)
    int insertPccProductionReleaseOutWip(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (1855,1866) -- Goods Dispatch Note / Goods Dispatch Note Direct (pcc.InvGdn). Proc line 2322. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , sh.SupplierCustomerId, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId,0,sd.ItemAttributeVarientId,''"
         + " , 0,0,SD.ItemQty,0,SD.ItemQty,0,SD.ItemQty"
         + " , 'Qty', 0, 0, 0 , 0, 0"
         + " ,CASE WHEN :refDocumentTypeId = 1855 THEN 'Goods Dispatch Note' ELSE 'Goods Dispatch Note Direct' END,sd.CityId"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.ItemNetWeight,0,0,0,0,0,SH.GpNo,SH.VehicleNo"
         + " FROM pcc.InvGdn AS SH"
         + " INNER JOIN pcc.InvGdnDetail AS SD ON SH.Id = SD.InvGdnId AND SD.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertPccGdn(@Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (1855,1866) -- Gdn (Extra_Qty To Party) into virtual warehouse. needs :virtualWareHouseId from readVirtualWarehouseId(); proc RAISERRORs when it is 0/NULL. Proc line 2349. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , sh.SupplierCustomerId, SH.IsApproved, SD.Id, :virtualWareHouseId, SD.JobLotId, SD.ItemId,0,sd.ItemAttributeVarientId,''"
         + " , 0,"
         + " SD.ItemQty - SD.ConfirmQty,0,"
         + " (SD.ItemQty - SD.ConfirmQty) as ItemNetWeight,0,"
         + " (SD.ItemQty - SD.ConfirmQty) as ItemNetWeight,0"
         + " , 'ConfirmQty', 0, 0, 0 , 0, 0"
         + " , 'Gdn (Extra_Qty To Party)',sd.CityId"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " (SD.ItemQty - SD.ConfirmQty) * SD.ItemWeight * SD.VarientEquivalent as ItemNetWeight,0,0,0,0,0,SH.GpNo,SH.VehicleNo"
         + " FROM pcc.InvGdn AS SH"
         + " INNER JOIN pcc.InvGdnDetail AS SD ON SH.Id = SD.InvGdnId AND SD.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and SD.ConfirmQty > 0"
         + " and SD.ItemQty - SD.ConfirmQty > 0", nativeQuery = true)
    int insertPccGdnExtraQtyToParty(@Param("virtualWareHouseId") Integer virtualWareHouseId, @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1856 -- Sale Invoice Direct (pcc.InvSaleInvoice). Proc line 2378. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , sh.SupplierCustomerId, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId,0,sd.ItemAttributeVarientId,''"
         + " , 0,0,SD.ItemQty,0,SD.ItemQty,0,SD.ItemQty"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Sale Invoice Direct',sd.CityId"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.ItemNetWeight,0,0,0,0,0,Sd.GpNo,Sd.VehicleNo"
         + " FROM pcc.InvSaleInvoice AS SH"
         + " INNER JOIN pcc.InvSaleInvoiceDetail AS SD ON SH.Id = SD.InvSaleInvoiceId AND SD.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertPccSaleInvoiceDirect(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1862 -- Sale Invoice Return (pcc). Proc line 2401. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , sh.SupplierCustomerId, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId,0,sd.ItemAttributeVarientId,''"
         + " , 0,SD.ItemQty,0,SD.ItemQty,0,SD.ItemQty,0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Sale Invoice Return',sd.CityId"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.ItemNetWeight,0,0,0,0,0,Sd.GpNo,Sd.VehicleNo"
         + " FROM pcc.InvSaleInvoice AS SH"
         + " INNER JOIN pcc.InvSaleInvoiceDetail AS SD ON SH.Id = SD.InvSaleInvoiceId AND SD.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertPccSaleInvoiceReturn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1860 -- Stock Transfer - IN (pcc, WarehouseToId > 0). Proc line 2424. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select sh.OrganizationId, sh.CompanyId,sh.BranchesId, SH.ProjectsId, sh.DocumentTypeId,sh.Id,sh.DocNo,sh.DocDate"
         + " , 0,SH.IsApproved,sd.Id, SD.WarehouseToId, SD.JobLotToId, SD.ItemId,0,sd.AttributeVareintId,''"
         + " , 0,SD.ItemQty,0,SD.ItemQty,0,SD.ItemQty,0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Stock Transfer'"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.NetWeight,0,0,0,0,0,Sh.GPSrNo,SH.VehicleNo"
         + " FROM pcc.InvStockTransferHeader AS SH"
         + " INNER JOIN pcc.InvStockTransferDetail AS SD ON SH.Id = SD.InvStockTransferHeaderId AND SD.ActionTypeId <> 3"
         + " Where sh.OrganizationId = :organizationId"
         + " and sh.CompanyId = :companyId"
         + " and sh.Id = :refDocIdNo"
         + " and sd.WarehouseToId > 0", nativeQuery = true)
    int insertPccStockTransferIn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1860 -- Stock Transfer - OUT (pcc, WarehouseFromId > 0). Proc line 2445. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select sh.OrganizationId, sh.CompanyId, sh.BranchesId, SH.ProjectsId,sh.DocumentTypeId,sh.Id,sh.DocNo, sh.DocDate"
         + " , 0,SH.IsApproved,sd.Id,sd.WarehouseFromId, SD.JobLotFromId, SD.ItemId, 0,sd.AttributeVareintId,''"
         + " , 0,0,SD.ItemQty,0,SD.ItemQty,0,SD.ItemQty"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Stock Transfer'"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.NetWeight,0,0,0,0,0,SH.GPSrNo,SH.VehicleNo"
         + " FROM pcc.InvStockTransferHeader AS SH"
         + " INNER JOIN pcc.InvStockTransferDetail AS SD ON SH.Id = SD.InvStockTransferHeaderId AND SD.ActionTypeId <> 3"
         + " Where sh.OrganizationId = :organizationId"
         + " and sh.CompanyId = :companyId"
         + " and sh.Id = :refDocIdNo"
         + " and sd.WarehouseFromId > 0", nativeQuery = true)
    int insertPccStockTransferOut(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1864 -- Goods Receiving Notes_FG (pcc.InvGrn). Proc line 2469. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , sh.SupplierCustomerId, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId,0,sd.ItemAttributeVarientId,''"
         + " , 0,SD.ItemQty,0,SD.ItemQty,0,SD.ItemQty,0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Goods Receiving Notes_FG',sd.CityId"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.ItemNetWeight,0,0,0,0,0,SH.GpNo,SH.VehicleNo"
         + " FROM pcc.InvGrn AS SH"
         + " INNER JOIN pcc.InvGrnDetail AS SD ON SH.Id = SD.InvGrnId AND SD.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertPccGrnFg(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1818 -- Production OutPut (dbo.ProductionHeader, salt). Proc line 2493. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select r.OrganizationId, r.CompanyId, r.BranchesId, r.ProjectsId, r.DocumentTypeId, r.Id, r.DocNo, r.DocDate"
         + " ,0,r.IsApproved,SD.Id,sd.WareHouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId,0,''"
         + " ,sd.PackingTypeId,SD.ItemQty,0,SD.NetWeight,0,SD.NetWeight,0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Production OutPut'"
         + " ,r.EntryDate,r.EntryUserId ,r.ModifyDate,r.ModifyUserId ,"
         + " SD.NetWeight,0,0,0,0,0,0,''"
         + " FROM dbo.ProductionHeader AS r"
         + " INNER JOIN dbo.ProductionOutPutDetail AS SD ON r.Id = SD.ProductionHeaderId AND SD.ActionTypeId <> 3"
         + " Where r.OrganizationId = :organizationId"
         + " and r.CompanyId = :companyId"
         + " and r.Id = :refDocIdNo", nativeQuery = true)
    int insertSaltProductionOutput(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (1619,1670) -- Production InPut (Mfg.ProductionHeader). @RefDocIdNo = ProductionHeaderId. Proc line 2518. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal"
         + " ,AddLess ,GpNo,VehicleNo"
         + " ,WorkOrderId,BatchNo,ProductionStageId,CastingTypeId"
         + " )"
         + " Select r.OrganizationId, r.CompanyId,W.BranchId, r.ProjectsId, r.DocumentTypeId, r.ProductionHeaderId, r.DocNo, r.DocDate"
         + " ,0,r.IsApproved,SD.productionInputDetailId,sd.WareHouseId,0, SD.ItemId, SD.ItemUomId,0,''"
         + " ,0,0,SD.Qty,0,SD.Weight,0,SD.Weight"
         + " ,'Weight', 0, 0, 0 , 0, 0,'Production InPut'"
         + " ,r.EntryDate,r.EntryUserId ,r.ModifyDate,r.ModifyUserId"
         + " ,SD.Weight,0,0,0,0,0,0,''"
         + " ,0,R.batchCode,0,r.CastingTypeId"
         + " FROM Mfg.ProductionHeader AS r"
         + " INNER JOIN Mfg.productionInputDetail AS SD ON r.ProductionHeaderId = SD.ProductionHeaderId AND SD.ActionTypeId <> 3"
         + " LEFT JOIN WarehousesAllocationToBranch W ON SD.WareHouseId = W.WarehouseId"
         + " Where r.OrganizationId = :organizationId"
         + " and r.CompanyId = :companyId"
         + " and r.ProductionHeaderId = :refDocIdNo", nativeQuery = true)
    int insertMfgCastingProductionInput(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (1619,1670) -- Production OutPut (Mfg.ProductionOutPutDetail). Proc line 2545. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,WorkOrderId,BatchNo,ProductionStageId,CastingTypeId"
         + " )"
         + " Select r.OrganizationId, r.CompanyId,W.BranchId, r.ProjectsId, r.DocumentTypeId, r.ProductionHeaderId, r.DocNo, r.DocDate"
         + " ,0,r.IsApproved,SD.ProductionOutPutDetailId,sd.WareHouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId,sd.ItemVariantId,''"
         + " ,sd.PackingTypeId,SD.ItemQty,0,SD.NetWeight,0,SD.NetWeight,0"
         + " ,'Weight', 0, 0, 0 , 0, 0,'Production OutPut'"
         + " ,r.EntryDate,r.EntryUserId ,r.ModifyDate,r.ModifyUserId"
         + " ,SD.NetWeight,0,0,0,0,0,0,''"
         + " ,SD.WorkOrderId,R.batchCode,sd.ProcessStatusId,r.CastingTypeId"
         + " FROM Mfg.ProductionHeader AS r"
         + " INNER JOIN Mfg.ProductionOutPutDetail AS SD ON r.ProductionHeaderId = SD.ProductionHeaderId AND SD.ActionTypeId <> 3"
         + " LEFT JOIN WarehousesAllocationToBranch W ON SD.WareHouseId = W.WarehouseId"
         + " Where r.OrganizationId = :organizationId"
         + " and r.CompanyId = :companyId"
         + " and r.ProductionHeaderId = :refDocIdNo", nativeQuery = true)
    int insertMfgCastingProductionOutput(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (1663,1665) -- Transfer_Out (Mfg.InvStockTransferHeader). Proc line 2574. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal"
         + " ,AddLess ,GpNo,VehicleNo"
         + " ,WorkOrderId,BatchNo,ProductionStageId,WorkStationId,CastingTypeId"
         + " )"
         + " Select r.OrganizationId, r.CompanyId,W.BranchId, r.ProjectsId, r.DocumentTypeId, r.Id, r.DocNo, r.DocDate"
         + " ,0,r.IsApproved,SD.Id,sd.WarehouseFromId,sd.JobLotId, SD.ItemId, SD.ItemUomId,sd.ItemVariantId,''"
         + " ,0,0,SD.ItemQty,0,SD.NetWeight,0,SD.NetWeight"
         + " ,'Qty', 0, 0, 0 , 0, 0,'Transfer_Out'"
         + " ,r.EntryDate,r.EntryUserId ,r.ModifyDate,r.ModifyUserId"
         + " ,SD.NetWeight,0,0,0,0,0,0,''"
         + " ,sd.WorkOrderId,sd.BatchDescription,sd.ProductionStageFromId,sd.WorkStationFromId,sd.CastingTypeId"
         + " FROM Mfg.InvStockTransferHeader AS r"
         + " INNER JOIN Mfg.InvStockTransferDetail AS SD ON r.Id = SD.InvStockTransferHeaderId AND SD.ActionTypeId <> 3"
         + " LEFT JOIN WarehousesAllocationToBranch W ON SD.WarehouseFromId = W.WarehouseId"
         + " Where r.OrganizationId = :organizationId"
         + " and r.CompanyId = :companyId"
         + " and r.Id = :refDocIdNo"
         + " and sd.WarehouseFromId > 0", nativeQuery = true)
    int insertMfgStockTransferOut(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (1663,1665) -- Transer_In (Mfg.InvStockTransferHeader). Proc line 2602. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo"
         + " ,WorkOrderId,BatchNo,ProductionStageId,WorkStationId,CastingTypeId"
         + " )"
         + " Select r.OrganizationId, r.CompanyId,W.BranchId, r.ProjectsId, r.DocumentTypeId, r.Id, r.DocNo, r.DocDate"
         + " ,0,r.IsApproved,SD.Id,sd.WarehouseToId,sd.JobLotId, SD.ItemId, SD.ItemUomId,sd.ItemVariantId,''"
         + " ,0,SD.ItemQty,0,SD.NetWeight,0,SD.NetWeight,0"
         + " ,'Qty', 0, 0, 0 , 0, 0,'Transer_In'"
         + " ,r.EntryDate,r.EntryUserId ,r.ModifyDate,r.ModifyUserId"
         + " ,SD.NetWeight,0,0,0,0,0,0,''"
         + " ,SD.WorkOrderId,sd.BatchDescription,sd.ProductionStageToId,sd.WorkStationToId,sd.CastingTypeId"
         + " FROM Mfg.InvStockTransferHeader AS r"
         + " INNER JOIN Mfg.InvStockTransferDetail AS SD ON r.Id = SD.InvStockTransferHeaderId AND SD.ActionTypeId <> 3"
         + " LEFT JOIN WarehousesAllocationToBranch W ON SD.WarehouseToId = W.WarehouseId"
         + " Where r.OrganizationId = :organizationId"
         + " and r.CompanyId = :companyId"
         + " and r.Id = :refDocIdNo"
         + " and sd.WarehouseToId > 0", nativeQuery = true)
    int insertMfgStockTransferIn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1702 -- Empty Bags From Loading (CmTr.CommTradeLoadingDelivery). StockTypeId = 1 (company). Proc line 2632. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],"
         + " EntryDate,EntryUserId ,ModifyDate,ModifyUserId)"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId,0, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , 0, Grh.IsApproved, grd.Id,GRD.WarehouseId,0, Grd.ItemId, 0, ''"
         + " , GRD.PackingTypeId,0,ISNULL(Grd.Qty,0),0, ISNULL(Grd.Qty,0), 0, ISNULL(Grd.Qty,0)"
         + " , 'Qty',0, 0, 0 , 0, 0,'Empty Bags From Loading'"
         + " , Grh.EntryDate,Grh.EntryUserId ,Grh.ModifyDate,Grh.ModifyUserId"
         + " FROM CmTr.CommTradeLoadingDelivery AS GrH"
         + " INNER JOIN CmTr.CommTradeLoadingDeliveryPmOther AS GrD ON GrH.Id = GrD.CommTradeLoadingDeliveryId and grh.ActionId <> 3"
         + " Where grh.OrganizationId = :organizationId"
         + " and grh.CompanyId = :companyId"
         + " and grh.Id = :refDocIdNo"
         + " and ISNULL(grd.StockTypeId,0) = 1", nativeQuery = true)
    int insertCommTradeLoadingEmptyBags(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1104 -- Purchase Invoice Direct (fed.InvPurchaseInvoice). Proc line 2658. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch],CropYearId"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks],CityId"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal,AddLess ,GpNo,VehicleNo"
         + " )"
         + " Select PH.OrganizationId, PH.CompanyId, PH.BranchesId, PH.ProjectsId, PH.DocumentTypeId, PH.Id, PH.DocNo, PH.DocDate"
         + " , PH.SupplierCustomerId, PH.IsApproved, PD.Id, PD.WarehouseId, PD.JobLotId, PD.ItemId, PD.ItemUomId,c.CropYear,PD.CropYearId"
         + " , PD.PackingTypeId, PD.ItemQty, 0, PD.NetBillWeight, 0, PD.NetStockWeight, 0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Purchase Invoice Direct',PD.CityId"
         + " ,PH.EntryDate,PH.EntryUserId ,PH.ModifyDate,PH.ModifyUserId,"
         + " PD.GrossWeight,PD.EBWeight,PD.EBTotalWt,PD.WeightCut,PD.WeightCutTotal ,PD.AdLsWeight ,PD.GpNo,PD.VehicleNo"
         + " FROM fed.InvPurchaseInvoice AS PH"
         + " INNER JOIN fed.InvPurchaseInvoiceDetail AS PD ON PH.Id = PD.InvPurchaseInvoiceId and pd.ActionTypeId <> 3"
         + " INNER JOIN InvCropYear c on pd.CropYearId = c.Id"
         + " Where PH.OrganizationId = :organizationId and"
         + " PH.CompanyId = :companyId and"
         + " PH.Id = :refDocIdNo", nativeQuery = true)
    int insertFedPurchaseInvoiceDirect(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1104 -- Purchase Invoice Direct Empty Bags (fed). Proc line 2683. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.SupplierCustomerId, Grh.IsApproved, grd.Id,grd.WarehouseId, 0, Grd.ItemId, 0, ''"
         + " , 0, ISNULL(Grd.PurchaseQty,0) + ISNULL(GrD.ReceivedQty,0), 0, ISNULL(Grd.PurchaseQty,0) + ISNULL(GrD.ReceivedQty,0), 0, ISNULL(Grd.PurchaseQty,0) + ISNULL(GrD.ReceivedQty,0), 0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Purchase Invoice Direct Empty Bags'"
         + " ,Grh.EntryDate,Grh.EntryUserId,Grh.ModifyDate,Grh.ModifyUserId ,"
         + " 0,0,0,0,0 ,0 ,0,0"
         + " FROM fed.InvPurchaseInvoice AS GrH"
         + " INNER JOIN fed.InvPurchaseInvoiceEmptyBags AS GrD ON GrH.Id = GrD.InvPurchaseInvoiceId"
         + " Where grh.OrganizationId = :organizationId"
         + " and grh.CompanyId = :companyId"
         + " and grh.Id = :refDocIdNo"
         + " and (GrD.PurchaseQty > 0 or grd.ReceivedQty > 0)", nativeQuery = true)
    int insertFedPurchaseInvoiceDirectEmptyBags(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1115 -- Sale Invoice Direct (fed.InvSaleInvoice). only rows with ISNULL(InvGdnId,0)=0. Proc line 2708. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch],CropYearId"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , SH.SupplierCustomerId, SH.IsApproved, SD.Id, SD.WarehouseId, SD.JobLotId, SD.ItemId, SD.ItemUomId,'', SD.CropYearId"
         + " , SD.PackingTypeId,0,SD.ItemQty, 0,SD.NetBillWeight, 0, SD.NetStockWeight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Sale Invoice Direct'"
         + " ,Sh.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId,"
         + " SD.GrossWeight,SD.EBWeight,SD.EBTotalWt,SD.WeightCut,SD.WeightCutTotal ,SD.AdLsWeight ,SD.GpNo,SD.VehicleNo"
         + " FROM fed.InvSaleInvoice AS SH"
         + " INNER JOIN fed.InvSaleInvoiceDetail AS SD ON SH.Id = SD.InvSaleInvoiceId and sd.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and ISNULL(SD.InvGdnId,0) = 0", nativeQuery = true)
    int insertFedSaleInvoiceDirect(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (1122,1121) -- Production OutPut (fed.ProductionHeader). Proc line 2735. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch],CropYearId"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.WarehouseId,0, SD.ItemId, SD.ItemUomId,0,0"
         + " , 0,SD.ItemQty,0,SD.NetWeight, 0, SD.NetWeight,0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Production OutPut'"
         + " , Sh.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId,"
         + " SD.NetWeight,0,0,0,0,0,0,Sh.ProductionNo"
         + " FROM fed.ProductionHeader AS SH"
         + " INNER JOIN fed.ProductionOutPutDetail AS SD ON SH.Id = SD.ProductionHeaderId and sd.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertFedProductionOutput(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (1122,1121) -- Production Input (fed). Proc line 2756. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch],CropYearId"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.WarehouseId,0, SD.ItemId, SD.ItemUomId,0,0"
         + " , 0,0,SD.ItemQty, 0,SD.NetWeight, 0, SD.NetWeight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Production Input'"
         + " , Sh.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId,"
         + " SD.NetWeight,0,0,0,0,0,0,Sh.ProductionNo"
         + " FROM fed.ProductionHeader AS SH"
         + " INNER JOIN fed.ProductionInputDetail AS SD ON SH.Id = SD.ProductionHeaderId and sd.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertFedProductionInput(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId IN (1122,1121) -- Production Consumption (fed). Proc line 2778. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch],CropYearId"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.WarehouseId,0, SD.ItemId, SD.ItemUomId,0,0"
         + " , 0,0,SD.ItemQty, 0,SD.NetWeight, 0, SD.NetWeight"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Production Consumption'"
         + " , Sh.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId,"
         + " SD.NetWeight,0,0,0,0,0,0,Sh.ProductionNo"
         + " FROM fed.ProductionHeader AS SH"
         + " INNER JOIN fed.ProductionConsumptionDetail AS SD ON SH.Id = SD.ProductionHeaderId and sd.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertFedProductionConsumption(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 241 -- Export Return GRN. no audit columns written. Proc line 2804. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks])"
         + " Select distinct Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.SupplierCustomerId, Grh.IsApproved, grd.Id, grd.WarehouseId, Grd.JobLotId, Grd.ItemId, Grd.PackUomId, C.CropYear"
         + " , Grd.PackingTypeId, Grd.ItemQty, 0, GrD.NetWeight, 0, Grd.StockWeight, 0"
         + " , 'Weight', 0, 0, 0 , 0, 0,'Export Return GRN'"
         + " FROM dbo.ExportReturnGrn AS GrH"
         + " INNER JOIN dbo.ExportReturnGrnDetail AS GrD ON GrH.Id = GrD.ExportReturnGrnId and grd.ActionTypeId <> 3"
         + " LEFT JOIN InvCropYear C ON GRD.CropYearId = C.Id"
         + " Where grh.OrganizationId = :organizationId"
         + " and grh.CompanyId = :companyId"
         + " and grh.Id = :refDocIdNo", nativeQuery = true)
    int insertExportReturnGrn(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 241 -- Export Return GRN Other Items. no audit columns written. Proc line 2823. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks])"
         + " Select distinct Grh.OrganizationId, GrH.CompanyId, grh.BranchesId, grh.ProjectsId, GrH.DocumentTypeId, GrH.Id, GrH.DocNo, GrH.DocDate"
         + " , GrH.SupplierCustomerId, Grh.IsApproved, grd.Id,0,0, Grd.OtherItemId, 0,''"
         + " , 0, Grd.Qty, 0, GrD.Qty, 0, Grd.Qty, 0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Export Return GRN Other Items'"
         + " FROM dbo.ExportReturnGrn AS GrH"
         + " INNER JOIN dbo.ExportReturnGrnOtherItems AS GrD ON GrH.Id = GrD.ExportReturnGrnId"
         + " Where grh.OrganizationId = :organizationId"
         + " and grh.CompanyId = :companyId"
         + " and grh.Id = :refDocIdNo", nativeQuery = true)
    int insertExportReturnGrnOtherItems(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1867 -- Production InPut (pcc.ConversionHeader). Proc line 2848. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.WarehouseId, 0, SD.ItemId, SD.ItemUomId,''"
         + " , 0,0,SD.ItemQty,0,SD.ItemQty,0,SD.ItemQty"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Production InPut'"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.NetWeight,0,0,0,0,0 ,0,''"
         + " FROM pcc.ConversionHeader AS SH"
         + " INNER JOIN pcc.ConversionInputDetail AS SD ON SH.Id = SD.ConversionHeaderId AND SD.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo", nativeQuery = true)
    int insertPccConversionInput(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);

    /** Sp_InventoryTransactions_GetALLMethod branch @RefDocumentTypeId = 1867 -- Production OutPut (pcc.ConversionOutPutDetail, IsApproved=0 only). Proc line 2868. Native INSERT ... SELECT pasted verbatim. */
    @Modifying
    @Transactional
    @Query(value = "INSERT INTO [dbo].[InventoryTransactions] ("
         + " OrganizationId, [CompanyId],[BranchesId],[ProjectsId],[RefDocumentTypeId],[RefDocIdNo],[DocCodeNo],[DocDate]"
         + " ,[SupplierCustomerId],[IsApproved],[RefDocSubIdNo],[WarehouseId],[JobLotId],[ItemId],[ItemUom],VarientId,[CropBatch]"
         + " ,[InvPackingTypeId],[QtyIn],[QtyOut],[BillWeightIn],[BillWeightOut],[StockWeightIn],[StockWeightOut]"
         + " ,[CalcType],[ItemRate],[RateUom],[AmountIn],[ExpenseAmountIn],[AmountOut],[TranRemarks]"
         + " ,EntryDate,EntryUserId ,ModifyDate,ModifyUserId ,GrossWeight,EbUnit,EbTotal,WtCut,WtCutTotal ,AddLess ,GpNo,VehicleNo)"
         + " Select SH.OrganizationId, SH.CompanyId, SH.BranchesId, SH.ProjectsId, SH.DocumentTypeId, SH.Id, SH.DocNo, SH.DocDate"
         + " , 0, SH.IsApproved, SD.Id, SD.WareHouseId, 0, SD.ItemId, SD.ItemUomId,sd.ItemAttributeVarientId,''"
         + " , 0,SD.ItemQty,0,SD.ItemQty,0,SD.ItemQty,0"
         + " , 'Qty', 0, 0, 0 , 0, 0,'Production OutPut'"
         + " ,SH.EntryDate,SH.EntryUserId ,SH.ModifyDate,SH.ModifyUserId ,"
         + " SD.NetWeight,0,0,0,0,0 ,0,''"
         + " FROM pcc.ConversionHeader AS SH"
         + " INNER JOIN pcc.ConversionOutPutDetail AS SD ON SH.Id = SD.ConversionHeaderId AND SD.ActionTypeId <> 3"
         + " Where SH.OrganizationId = :organizationId"
         + " and SH.CompanyId = :companyId"
         + " and SH.Id = :refDocIdNo"
         + " and ISNULL(sh.IsApproved,0) = 0", nativeQuery = true)
    int insertPccConversionOutput(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("refDocIdNo") Integer refDocIdNo);
}
