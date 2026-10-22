package com.mst.repositories;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.Item;
import com.mst.projections.InventoryStockTransactionsReportRow;
import com.mst.projections.InventoryStocksReportStoreRow;
import com.mst.projections.ItemStockReportWithValuesStoreRawRow;
import com.mst.projections.ItemStockReportWithValuesStoreRow;

/**
 * Query-only repository for the inventory stock reports:
 * Sp_InventoryTransactions_GenerateStocksReport_Store_Rpt, Sp_ItemStockReportWithValues_Store and
 * pcc.[USP-InventoryStockTransactionsReport].
 * Bound to the existing Item entity only to satisfy Spring Data; no entity methods are exposed.
 */
@Repository
public interface InventoryStockReportQueryRepository extends org.springframework.data.repository.Repository<Item, Integer> {

    /**
     * Sp_InventoryTransactions_GenerateStocksReport_Store_Rpt (single branch).
     * Rewritten as one statement: CTE "AsOn" reproduces the @getAsOnDate lookup (latest InventoryStockAsOnDate with
     * ISNULL(ActionId,1) = 2) and the "@DateFrom = AsOnDate + 1 day when @DateFrom is earlier" adjustment as a scalar;
     * CTE "s" is the procedure's SELECT ... INTO #s (ROW_NUMBER kept inside the derived table); #BranchesIds is inlined as
     * EXISTS over dbo.fnSplitString(:branchesIds, ','). Final SELECT, GROUP BY, window SUM and ORDER BY are verbatim.
     * Proc defaults: all parameters NULL; branchesIds is a comma-separated list of branch ids.
     */
    @Query(value = "WITH AsOn AS ("
         + " SELECT CASE WHEN x.AsOnDate IS NOT NULL AND CAST(:dateFrom AS DATE) < CAST(x.AsOnDate AS DATE)"
         + "   THEN DATEADD(DAY, 1, x.AsOnDate) ELSE :dateFrom END AS DateFrom"
         + " FROM (SELECT (SELECT TOP 1 AsOnDate FROM InventoryStockAsOnDate"
         + "   where OrganizationId = :organizationId and CompanyId = :companyId AND ISNULL(ActionId,1) = 2"
         + "   ORDER BY AsOnDate DESC) AS AsOnDate) x"
         + "), s AS ("
         + " SELECT a.WarehouseId, a.RackId, a.ItemId, a.ItemUom, a.DocumentTypeDescription,"
         + "  a.DocDate, a.DocCodeNo, a.RefDocumentTypeId"
         + "  , isnull(sum(a.CurrInQty),0) CurrInQty"
         + "  , isnull(sum(a.CurrOutQty),0) as CurrOutQty"
         + "  , isnull(sum(a.BalQty),0) as BalQty"
         + "  , a.RecNo, a.BranchesId, a.ItemConditionId"
         + " From ("
         + "  SELECT it.WarehouseId, it.RackId, it.ItemId, it.ItemUom, d.DocumentTypeDescription"
         + "   , it.DocDate, it.DocCodeNo, it.RefDocumentTypeId"
         + "   , isnull(it.QtyIn,0) CurrInQty"
         + "   , isnull(it.QtyOut,0) CurrOutQty"
         + "   , isnull(it.QtyIn,0) - isnull(it.QtyOut,0) as BalQty"
         + "   , ROW_NUMBER() OVER (ORDER BY it.DocDate, it.RefDocumentTypeId, it.DocCodeNo) RecNo,"
         + "   it.BranchesId,"
         + "   it.ItemConditionId"
         + "  FROM dbo.InventoryTransactions it"
         + "   CROSS JOIN AsOn p"
         + "   inner join DocumentType d on it.RefDocumentTypeId = d.Id"
         + "   inner join Item i on it.ItemId = i.Id"
         + "   inner join ItemType itp on i.ItemTypeId = itp.Id and itp.Type in (14,17)"
         + "  where (it.OrganizationId = :organizationId)"
         + "   and (it.CompanyId = :companyId)"
         + "   and (:supplierCustomerId is null or it.SupplierCustomerId = :supplierCustomerId)"
         + "   and (:itemId is null or it.ItemId = :itemId)"
         + "   and (p.DateFrom is null or it.DocDate >= p.DateFrom)"
         + "   and (:dateTo is null or it.DocDate < DATEADD(DAY, 1, :dateTo))"
         + "   and (:warehouseId is null or it.WarehouseId = :warehouseId)"
         + "   and (:itemTypeId is null or i.ItemTypeId = :itemTypeId)"
         + "   and (:itemConditionId is null or it.ItemConditionId = :itemConditionId)"
         + "   and (:rackId is null or it.RackId = :rackId)"
         + "   and (:documentTypeId is null or it.RefDocumentTypeId = :documentTypeId)"
         + "   AND (:branchesIds IS NULL"
         + "    OR EXISTS (SELECT 1 FROM dbo.fnSplitString(:branchesIds, ',') b WHERE b.data = it.BranchesId))"
         + " ) a Group By a.WarehouseId, a.RackId, a.ItemId, a.ItemUom, a.DocumentTypeDescription"
         + "  , a.DocDate, a.DocCodeNo, a.RefDocumentTypeId, a.RecNo, a.BranchesId, a.ItemConditionId"
         + ") "
         + "select s.ItemId, s.DocumentTypeDescription, s.DocDate, s.DocCodeNo, wh.WareHouseName, whr.rackName as RackName,"
         + " i.ItemCode, i.ItemName, pu.UOMCode"
         + " , sum(s.CurrInQty) as InQty"
         + " , sum(s.CurrOutQty) as OutQty"
         + " , sum(sum(s.BalQty)) OVER (Order By s.RecNo) as BalQty, co.CompCountry, co.CompContactPerson, co.CompMobileA, co.CompMobileB,"
         + " co.CompMobileC,"
         + " co.CompEmailA,"
         + " co.CompEmailB,"
         + " CAST('' AS NVARCHAR(MAX)) AS CompLogoImage,"
         + " co.CompanyWebsite, co.CompanyFaxNo, org.ReportingRemarks OrgReportingRemarks,"
         + " bb.BranchName"
         + " , s.ItemConditionId"
         + " , ICond.ConditionStatus As ItemCondition "
         + "from s"
         + " Inner Join item i on s.ItemId = i.Id"
         + " left join V_UomScheduleAndUom pu on s.ItemUom = pu.Id and pu.CompanyId = :companyId"
         + " Left Outer Join InvWareHouse wh on s.WarehouseId = wh.Id"
         + " Inner join Company co on co.Id = :companyId"
         + " Inner join Organization org on org.Id = co.OrgCompanyTypeId"
         + " inner join DocumentType d on s.RefDocumentTypeId = d.Id"
         + " Left Join Branches bb on bb.Id = s.BranchesId"
         + " Left Join V_ItemCondition ICond on ICond.Id = s.ItemConditionId"
         + " Left Join invWarehouseRack whr on s.RackId = whr.Id "
         + "Group by wh.WareHouseName, whr.rackName, s.ItemId, i.ItemCode, i.ItemName, pu.UOMCode"
         + " , s.DocumentTypeDescription, s.DocDate, s.DocCodeNo, s.RecNo, co.CompCountry, co.CompContactPerson, co.CompMobileA, co.CompMobileB,"
         + " co.CompMobileC, co.CompEmailA, co.CompEmailB,"
         + " co.CompanyWebsite, co.CompanyFaxNo, org.ReportingRemarks, bb.BranchName, s.ItemConditionId, ICond.ConditionStatus "
         + "Order by s.RecNo asc",
         nativeQuery = true)
    List<InventoryStocksReportStoreRow> generateStocksReportStore(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("supplierCustomerId") Integer supplierCustomerId,
            @Param("itemId") Integer itemId,
            @Param("dateFrom") LocalDate dateFrom,
            @Param("dateTo") LocalDate dateTo,
            @Param("warehouseId") Integer warehouseId,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("documentTypeId") Integer documentTypeId,
            @Param("itemConditionId") Integer itemConditionId,
            @Param("rackId") Integer rackId,
            @Param("branchesIds") String branchesIds);

    /**
     * Sp_ItemStockReportWithValues_Store @Activity='ItemStockSummary': grouped by item (+ ItemUomId/PackSize when @IsPackSizeOn = 1); RackId = 0, CropYear/WareHouseName/JobLot = ''. The ManualRate/ManualAmount update from StockSummaryByItem requires @IsPackTypeOn = 1, which the procedure forces to 0, so it is dead code.
     * // KEEP-EXEC: ReportCriteria is assembled procedurally (FORMAT / String_agg / IF chains), the opening/period
     * union is staged into #f and re-grouped per @Activity into #TMPFinalData, with a conditional UPDATE from
     * #StockSummaryByItem (ROW_NUMBER) before the final SELECT.
     * Proc defaults: all parameters NULL. @BranchesIds / @InventoryParentCategoriesIds are comma-separated id lists
     * (split with fnSplitString). @DateFrom is moved to InventoryStockAsOnDate.AsOnDate + 1 day when it is earlier
     * than the latest packing-material as-on date. @IsPackTypeOn is forced to 0 inside the procedure. @SaleValue = 1
     * values outgoing stock at AmountOut instead of CgsAmount. @SkipZero (non-null) drops rows with BalWeight <= 0.05.
     * Parameters @ProjectsId, @UomId and @AllowWipItem are accepted but unused by the procedure.
     */
    @Query(value = "EXEC dbo.Sp_ItemStockReportWithValues_Store"
         + " @OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, @ProjectsId = :projectsId,"
         + " @DateFrom = :dateFrom, @DateTo = :dateTo, @ItemTypeId = :itemTypeId, @ItemCategoryId = :itemCategoryId,"
         + " @CropYear = :cropYear, @JobLotId = :jobLotId, @WarehouseId = :warehouseId, @UomId = :uomId,"
         + " @ItemId = :itemId, @PackUom = :packUom, @PackingTypeId = :packingTypeId, @IsPackSizeOn = :isPackSizeOn,"
         + " @IsPackTypeOn = :isPackTypeOn, @ItemStockAc = :itemStockAc, @AllowWipItem = :allowWipItem, @ItemClassGroupId = :itemClassGroupId,"
         + " @InventoryParentCategoriesIds = :inventoryParentCategoriesIds, @SaleValue = :saleValue, @SkipZero = :skipZero, @ItemConditionId = :itemConditionId,"
         + " @RackId = :rackId, @BranchesIds = :branchesIds,"
         + " @Activity = 'ItemStockSummary'",
         nativeQuery = true)
    List<ItemStockReportWithValuesStoreRow> itemStockSummary(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("projectsId") Integer projectsId,
            @Param("dateFrom") LocalDate dateFrom,
            @Param("dateTo") LocalDate dateTo,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("cropYear") String cropYear,
            @Param("jobLotId") Integer jobLotId,
            @Param("warehouseId") Integer warehouseId,
            @Param("uomId") Integer uomId,
            @Param("itemId") Integer itemId,
            @Param("packUom") Double packUom,
            @Param("packingTypeId") Integer packingTypeId,
            @Param("isPackSizeOn") Integer isPackSizeOn,
            @Param("isPackTypeOn") Integer isPackTypeOn,
            @Param("itemStockAc") Integer itemStockAc,
            @Param("allowWipItem") Integer allowWipItem,
            @Param("itemClassGroupId") Integer itemClassGroupId,
            @Param("inventoryParentCategoriesIds") String inventoryParentCategoriesIds,
            @Param("saleValue") Integer saleValue,
            @Param("skipZero") Integer skipZero,
            @Param("itemConditionId") Integer itemConditionId,
            @Param("rackId") Integer rackId,
            @Param("branchesIds") String branchesIds);

    /**
     * Sp_ItemStockReportWithValues_Store @Activity='ItemandCropYearStockSummary': grouped by item + CropBatch; RackId = 0, WareHouseName/JobLot = ''; ordered by ItemName, CropBatch.
     * // KEEP-EXEC: ReportCriteria is assembled procedurally (FORMAT / String_agg / IF chains), the opening/period
     * union is staged into #f and re-grouped per @Activity into #TMPFinalData, with a conditional UPDATE from
     * #StockSummaryByItem (ROW_NUMBER) before the final SELECT.
     * Proc defaults: all parameters NULL. @BranchesIds / @InventoryParentCategoriesIds are comma-separated id lists
     * (split with fnSplitString). @DateFrom is moved to InventoryStockAsOnDate.AsOnDate + 1 day when it is earlier
     * than the latest packing-material as-on date. @IsPackTypeOn is forced to 0 inside the procedure. @SaleValue = 1
     * values outgoing stock at AmountOut instead of CgsAmount. @SkipZero (non-null) drops rows with BalWeight <= 0.05.
     * Parameters @ProjectsId, @UomId and @AllowWipItem are accepted but unused by the procedure.
     */
    @Query(value = "EXEC dbo.Sp_ItemStockReportWithValues_Store"
         + " @OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, @ProjectsId = :projectsId,"
         + " @DateFrom = :dateFrom, @DateTo = :dateTo, @ItemTypeId = :itemTypeId, @ItemCategoryId = :itemCategoryId,"
         + " @CropYear = :cropYear, @JobLotId = :jobLotId, @WarehouseId = :warehouseId, @UomId = :uomId,"
         + " @ItemId = :itemId, @PackUom = :packUom, @PackingTypeId = :packingTypeId, @IsPackSizeOn = :isPackSizeOn,"
         + " @IsPackTypeOn = :isPackTypeOn, @ItemStockAc = :itemStockAc, @AllowWipItem = :allowWipItem, @ItemClassGroupId = :itemClassGroupId,"
         + " @InventoryParentCategoriesIds = :inventoryParentCategoriesIds, @SaleValue = :saleValue, @SkipZero = :skipZero, @ItemConditionId = :itemConditionId,"
         + " @RackId = :rackId, @BranchesIds = :branchesIds,"
         + " @Activity = 'ItemandCropYearStockSummary'",
         nativeQuery = true)
    List<ItemStockReportWithValuesStoreRow> itemAndCropYearStockSummary(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("projectsId") Integer projectsId,
            @Param("dateFrom") LocalDate dateFrom,
            @Param("dateTo") LocalDate dateTo,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("cropYear") String cropYear,
            @Param("jobLotId") Integer jobLotId,
            @Param("warehouseId") Integer warehouseId,
            @Param("uomId") Integer uomId,
            @Param("itemId") Integer itemId,
            @Param("packUom") Double packUom,
            @Param("packingTypeId") Integer packingTypeId,
            @Param("isPackSizeOn") Integer isPackSizeOn,
            @Param("isPackTypeOn") Integer isPackTypeOn,
            @Param("itemStockAc") Integer itemStockAc,
            @Param("allowWipItem") Integer allowWipItem,
            @Param("itemClassGroupId") Integer itemClassGroupId,
            @Param("inventoryParentCategoriesIds") String inventoryParentCategoriesIds,
            @Param("saleValue") Integer saleValue,
            @Param("skipZero") Integer skipZero,
            @Param("itemConditionId") Integer itemConditionId,
            @Param("rackId") Integer rackId,
            @Param("branchesIds") String branchesIds);

    /**
     * Sp_ItemStockReportWithValues_Store @Activity='ItemandCropYearandWarehouseStockSummary': grouped by item + CropBatch + warehouse + rack; JobLot = ''; ordered by ItemName, CropBatch, WareHouseName.
     * // KEEP-EXEC: ReportCriteria is assembled procedurally (FORMAT / String_agg / IF chains), the opening/period
     * union is staged into #f and re-grouped per @Activity into #TMPFinalData, with a conditional UPDATE from
     * #StockSummaryByItem (ROW_NUMBER) before the final SELECT.
     * Proc defaults: all parameters NULL. @BranchesIds / @InventoryParentCategoriesIds are comma-separated id lists
     * (split with fnSplitString). @DateFrom is moved to InventoryStockAsOnDate.AsOnDate + 1 day when it is earlier
     * than the latest packing-material as-on date. @IsPackTypeOn is forced to 0 inside the procedure. @SaleValue = 1
     * values outgoing stock at AmountOut instead of CgsAmount. @SkipZero (non-null) drops rows with BalWeight <= 0.05.
     * Parameters @ProjectsId, @UomId and @AllowWipItem are accepted but unused by the procedure.
     */
    @Query(value = "EXEC dbo.Sp_ItemStockReportWithValues_Store"
         + " @OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, @ProjectsId = :projectsId,"
         + " @DateFrom = :dateFrom, @DateTo = :dateTo, @ItemTypeId = :itemTypeId, @ItemCategoryId = :itemCategoryId,"
         + " @CropYear = :cropYear, @JobLotId = :jobLotId, @WarehouseId = :warehouseId, @UomId = :uomId,"
         + " @ItemId = :itemId, @PackUom = :packUom, @PackingTypeId = :packingTypeId, @IsPackSizeOn = :isPackSizeOn,"
         + " @IsPackTypeOn = :isPackTypeOn, @ItemStockAc = :itemStockAc, @AllowWipItem = :allowWipItem, @ItemClassGroupId = :itemClassGroupId,"
         + " @InventoryParentCategoriesIds = :inventoryParentCategoriesIds, @SaleValue = :saleValue, @SkipZero = :skipZero, @ItemConditionId = :itemConditionId,"
         + " @RackId = :rackId, @BranchesIds = :branchesIds,"
         + " @Activity = 'ItemandCropYearandWarehouseStockSummary'",
         nativeQuery = true)
    List<ItemStockReportWithValuesStoreRow> itemAndCropYearAndWarehouseStockSummary(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("projectsId") Integer projectsId,
            @Param("dateFrom") LocalDate dateFrom,
            @Param("dateTo") LocalDate dateTo,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("cropYear") String cropYear,
            @Param("jobLotId") Integer jobLotId,
            @Param("warehouseId") Integer warehouseId,
            @Param("uomId") Integer uomId,
            @Param("itemId") Integer itemId,
            @Param("packUom") Double packUom,
            @Param("packingTypeId") Integer packingTypeId,
            @Param("isPackSizeOn") Integer isPackSizeOn,
            @Param("isPackTypeOn") Integer isPackTypeOn,
            @Param("itemStockAc") Integer itemStockAc,
            @Param("allowWipItem") Integer allowWipItem,
            @Param("itemClassGroupId") Integer itemClassGroupId,
            @Param("inventoryParentCategoriesIds") String inventoryParentCategoriesIds,
            @Param("saleValue") Integer saleValue,
            @Param("skipZero") Integer skipZero,
            @Param("itemConditionId") Integer itemConditionId,
            @Param("rackId") Integer rackId,
            @Param("branchesIds") String branchesIds);

    /**
     * Sp_ItemStockReportWithValues_Store @Activity='ItemandPackSizeStockSummary': grouped by item + PackSize (PackUom column); RackId = 0, other dimension columns = ''.
     * // KEEP-EXEC: ReportCriteria is assembled procedurally (FORMAT / String_agg / IF chains), the opening/period
     * union is staged into #f and re-grouped per @Activity into #TMPFinalData, with a conditional UPDATE from
     * #StockSummaryByItem (ROW_NUMBER) before the final SELECT.
     * Proc defaults: all parameters NULL. @BranchesIds / @InventoryParentCategoriesIds are comma-separated id lists
     * (split with fnSplitString). @DateFrom is moved to InventoryStockAsOnDate.AsOnDate + 1 day when it is earlier
     * than the latest packing-material as-on date. @IsPackTypeOn is forced to 0 inside the procedure. @SaleValue = 1
     * values outgoing stock at AmountOut instead of CgsAmount. @SkipZero (non-null) drops rows with BalWeight <= 0.05.
     * Parameters @ProjectsId, @UomId and @AllowWipItem are accepted but unused by the procedure.
     */
    @Query(value = "EXEC dbo.Sp_ItemStockReportWithValues_Store"
         + " @OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, @ProjectsId = :projectsId,"
         + " @DateFrom = :dateFrom, @DateTo = :dateTo, @ItemTypeId = :itemTypeId, @ItemCategoryId = :itemCategoryId,"
         + " @CropYear = :cropYear, @JobLotId = :jobLotId, @WarehouseId = :warehouseId, @UomId = :uomId,"
         + " @ItemId = :itemId, @PackUom = :packUom, @PackingTypeId = :packingTypeId, @IsPackSizeOn = :isPackSizeOn,"
         + " @IsPackTypeOn = :isPackTypeOn, @ItemStockAc = :itemStockAc, @AllowWipItem = :allowWipItem, @ItemClassGroupId = :itemClassGroupId,"
         + " @InventoryParentCategoriesIds = :inventoryParentCategoriesIds, @SaleValue = :saleValue, @SkipZero = :skipZero, @ItemConditionId = :itemConditionId,"
         + " @RackId = :rackId, @BranchesIds = :branchesIds,"
         + " @Activity = 'ItemandPackSizeStockSummary'",
         nativeQuery = true)
    List<ItemStockReportWithValuesStoreRow> itemAndPackSizeStockSummary(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("projectsId") Integer projectsId,
            @Param("dateFrom") LocalDate dateFrom,
            @Param("dateTo") LocalDate dateTo,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("cropYear") String cropYear,
            @Param("jobLotId") Integer jobLotId,
            @Param("warehouseId") Integer warehouseId,
            @Param("uomId") Integer uomId,
            @Param("itemId") Integer itemId,
            @Param("packUom") Double packUom,
            @Param("packingTypeId") Integer packingTypeId,
            @Param("isPackSizeOn") Integer isPackSizeOn,
            @Param("isPackTypeOn") Integer isPackTypeOn,
            @Param("itemStockAc") Integer itemStockAc,
            @Param("allowWipItem") Integer allowWipItem,
            @Param("itemClassGroupId") Integer itemClassGroupId,
            @Param("inventoryParentCategoriesIds") String inventoryParentCategoriesIds,
            @Param("saleValue") Integer saleValue,
            @Param("skipZero") Integer skipZero,
            @Param("itemConditionId") Integer itemConditionId,
            @Param("rackId") Integer rackId,
            @Param("branchesIds") String branchesIds);

    /**
     * Sp_ItemStockReportWithValues_Store @Activity='ItemandPackSizeandPackingTypeStockSummary': grouped by item + PackSize + PackingType; RackId = 0, other dimension columns = ''.
     * // KEEP-EXEC: ReportCriteria is assembled procedurally (FORMAT / String_agg / IF chains), the opening/period
     * union is staged into #f and re-grouped per @Activity into #TMPFinalData, with a conditional UPDATE from
     * #StockSummaryByItem (ROW_NUMBER) before the final SELECT.
     * Proc defaults: all parameters NULL. @BranchesIds / @InventoryParentCategoriesIds are comma-separated id lists
     * (split with fnSplitString). @DateFrom is moved to InventoryStockAsOnDate.AsOnDate + 1 day when it is earlier
     * than the latest packing-material as-on date. @IsPackTypeOn is forced to 0 inside the procedure. @SaleValue = 1
     * values outgoing stock at AmountOut instead of CgsAmount. @SkipZero (non-null) drops rows with BalWeight <= 0.05.
     * Parameters @ProjectsId, @UomId and @AllowWipItem are accepted but unused by the procedure.
     */
    @Query(value = "EXEC dbo.Sp_ItemStockReportWithValues_Store"
         + " @OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, @ProjectsId = :projectsId,"
         + " @DateFrom = :dateFrom, @DateTo = :dateTo, @ItemTypeId = :itemTypeId, @ItemCategoryId = :itemCategoryId,"
         + " @CropYear = :cropYear, @JobLotId = :jobLotId, @WarehouseId = :warehouseId, @UomId = :uomId,"
         + " @ItemId = :itemId, @PackUom = :packUom, @PackingTypeId = :packingTypeId, @IsPackSizeOn = :isPackSizeOn,"
         + " @IsPackTypeOn = :isPackTypeOn, @ItemStockAc = :itemStockAc, @AllowWipItem = :allowWipItem, @ItemClassGroupId = :itemClassGroupId,"
         + " @InventoryParentCategoriesIds = :inventoryParentCategoriesIds, @SaleValue = :saleValue, @SkipZero = :skipZero, @ItemConditionId = :itemConditionId,"
         + " @RackId = :rackId, @BranchesIds = :branchesIds,"
         + " @Activity = 'ItemandPackSizeandPackingTypeStockSummary'",
         nativeQuery = true)
    List<ItemStockReportWithValuesStoreRow> itemAndPackSizeAndPackingTypeStockSummary(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("projectsId") Integer projectsId,
            @Param("dateFrom") LocalDate dateFrom,
            @Param("dateTo") LocalDate dateTo,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("cropYear") String cropYear,
            @Param("jobLotId") Integer jobLotId,
            @Param("warehouseId") Integer warehouseId,
            @Param("uomId") Integer uomId,
            @Param("itemId") Integer itemId,
            @Param("packUom") Double packUom,
            @Param("packingTypeId") Integer packingTypeId,
            @Param("isPackSizeOn") Integer isPackSizeOn,
            @Param("isPackTypeOn") Integer isPackTypeOn,
            @Param("itemStockAc") Integer itemStockAc,
            @Param("allowWipItem") Integer allowWipItem,
            @Param("itemClassGroupId") Integer itemClassGroupId,
            @Param("inventoryParentCategoriesIds") String inventoryParentCategoriesIds,
            @Param("saleValue") Integer saleValue,
            @Param("skipZero") Integer skipZero,
            @Param("itemConditionId") Integer itemConditionId,
            @Param("rackId") Integer rackId,
            @Param("branchesIds") String branchesIds);

    /**
     * Sp_ItemStockReportWithValues_Store @Activity='ItemandPackingTypeStockSummary': grouped by item + PackingType; RackId = 0, other dimension columns = ''.
     * // KEEP-EXEC: ReportCriteria is assembled procedurally (FORMAT / String_agg / IF chains), the opening/period
     * union is staged into #f and re-grouped per @Activity into #TMPFinalData, with a conditional UPDATE from
     * #StockSummaryByItem (ROW_NUMBER) before the final SELECT.
     * Proc defaults: all parameters NULL. @BranchesIds / @InventoryParentCategoriesIds are comma-separated id lists
     * (split with fnSplitString). @DateFrom is moved to InventoryStockAsOnDate.AsOnDate + 1 day when it is earlier
     * than the latest packing-material as-on date. @IsPackTypeOn is forced to 0 inside the procedure. @SaleValue = 1
     * values outgoing stock at AmountOut instead of CgsAmount. @SkipZero (non-null) drops rows with BalWeight <= 0.05.
     * Parameters @ProjectsId, @UomId and @AllowWipItem are accepted but unused by the procedure.
     */
    @Query(value = "EXEC dbo.Sp_ItemStockReportWithValues_Store"
         + " @OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, @ProjectsId = :projectsId,"
         + " @DateFrom = :dateFrom, @DateTo = :dateTo, @ItemTypeId = :itemTypeId, @ItemCategoryId = :itemCategoryId,"
         + " @CropYear = :cropYear, @JobLotId = :jobLotId, @WarehouseId = :warehouseId, @UomId = :uomId,"
         + " @ItemId = :itemId, @PackUom = :packUom, @PackingTypeId = :packingTypeId, @IsPackSizeOn = :isPackSizeOn,"
         + " @IsPackTypeOn = :isPackTypeOn, @ItemStockAc = :itemStockAc, @AllowWipItem = :allowWipItem, @ItemClassGroupId = :itemClassGroupId,"
         + " @InventoryParentCategoriesIds = :inventoryParentCategoriesIds, @SaleValue = :saleValue, @SkipZero = :skipZero, @ItemConditionId = :itemConditionId,"
         + " @RackId = :rackId, @BranchesIds = :branchesIds,"
         + " @Activity = 'ItemandPackingTypeStockSummary'",
         nativeQuery = true)
    List<ItemStockReportWithValuesStoreRow> itemAndPackingTypeStockSummary(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("projectsId") Integer projectsId,
            @Param("dateFrom") LocalDate dateFrom,
            @Param("dateTo") LocalDate dateTo,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("cropYear") String cropYear,
            @Param("jobLotId") Integer jobLotId,
            @Param("warehouseId") Integer warehouseId,
            @Param("uomId") Integer uomId,
            @Param("itemId") Integer itemId,
            @Param("packUom") Double packUom,
            @Param("packingTypeId") Integer packingTypeId,
            @Param("isPackSizeOn") Integer isPackSizeOn,
            @Param("isPackTypeOn") Integer isPackTypeOn,
            @Param("itemStockAc") Integer itemStockAc,
            @Param("allowWipItem") Integer allowWipItem,
            @Param("itemClassGroupId") Integer itemClassGroupId,
            @Param("inventoryParentCategoriesIds") String inventoryParentCategoriesIds,
            @Param("saleValue") Integer saleValue,
            @Param("skipZero") Integer skipZero,
            @Param("itemConditionId") Integer itemConditionId,
            @Param("rackId") Integer rackId,
            @Param("branchesIds") String branchesIds);

    /**
     * Sp_ItemStockReportWithValues_Store @Activity='ItemandWarehouseStockSummary': grouped by item + warehouse + rack; CropYear/JobLot = ''; ordered by ItemName, WareHouseName.
     * // KEEP-EXEC: ReportCriteria is assembled procedurally (FORMAT / String_agg / IF chains), the opening/period
     * union is staged into #f and re-grouped per @Activity into #TMPFinalData, with a conditional UPDATE from
     * #StockSummaryByItem (ROW_NUMBER) before the final SELECT.
     * Proc defaults: all parameters NULL. @BranchesIds / @InventoryParentCategoriesIds are comma-separated id lists
     * (split with fnSplitString). @DateFrom is moved to InventoryStockAsOnDate.AsOnDate + 1 day when it is earlier
     * than the latest packing-material as-on date. @IsPackTypeOn is forced to 0 inside the procedure. @SaleValue = 1
     * values outgoing stock at AmountOut instead of CgsAmount. @SkipZero (non-null) drops rows with BalWeight <= 0.05.
     * Parameters @ProjectsId, @UomId and @AllowWipItem are accepted but unused by the procedure.
     */
    @Query(value = "EXEC dbo.Sp_ItemStockReportWithValues_Store"
         + " @OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, @ProjectsId = :projectsId,"
         + " @DateFrom = :dateFrom, @DateTo = :dateTo, @ItemTypeId = :itemTypeId, @ItemCategoryId = :itemCategoryId,"
         + " @CropYear = :cropYear, @JobLotId = :jobLotId, @WarehouseId = :warehouseId, @UomId = :uomId,"
         + " @ItemId = :itemId, @PackUom = :packUom, @PackingTypeId = :packingTypeId, @IsPackSizeOn = :isPackSizeOn,"
         + " @IsPackTypeOn = :isPackTypeOn, @ItemStockAc = :itemStockAc, @AllowWipItem = :allowWipItem, @ItemClassGroupId = :itemClassGroupId,"
         + " @InventoryParentCategoriesIds = :inventoryParentCategoriesIds, @SaleValue = :saleValue, @SkipZero = :skipZero, @ItemConditionId = :itemConditionId,"
         + " @RackId = :rackId, @BranchesIds = :branchesIds,"
         + " @Activity = 'ItemandWarehouseStockSummary'",
         nativeQuery = true)
    List<ItemStockReportWithValuesStoreRow> itemAndWarehouseStockSummary(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("projectsId") Integer projectsId,
            @Param("dateFrom") LocalDate dateFrom,
            @Param("dateTo") LocalDate dateTo,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("cropYear") String cropYear,
            @Param("jobLotId") Integer jobLotId,
            @Param("warehouseId") Integer warehouseId,
            @Param("uomId") Integer uomId,
            @Param("itemId") Integer itemId,
            @Param("packUom") Double packUom,
            @Param("packingTypeId") Integer packingTypeId,
            @Param("isPackSizeOn") Integer isPackSizeOn,
            @Param("isPackTypeOn") Integer isPackTypeOn,
            @Param("itemStockAc") Integer itemStockAc,
            @Param("allowWipItem") Integer allowWipItem,
            @Param("itemClassGroupId") Integer itemClassGroupId,
            @Param("inventoryParentCategoriesIds") String inventoryParentCategoriesIds,
            @Param("saleValue") Integer saleValue,
            @Param("skipZero") Integer skipZero,
            @Param("itemConditionId") Integer itemConditionId,
            @Param("rackId") Integer rackId,
            @Param("branchesIds") String branchesIds);

    /**
     * Sp_ItemStockReportWithValues_Store @Activity='JobLotandItemStockSummary': grouped by item + JobLot; RackId = 0, CropYear/WareHouseName = ''; ordered by ItemName, JobLot.
     * // KEEP-EXEC: ReportCriteria is assembled procedurally (FORMAT / String_agg / IF chains), the opening/period
     * union is staged into #f and re-grouped per @Activity into #TMPFinalData, with a conditional UPDATE from
     * #StockSummaryByItem (ROW_NUMBER) before the final SELECT.
     * Proc defaults: all parameters NULL. @BranchesIds / @InventoryParentCategoriesIds are comma-separated id lists
     * (split with fnSplitString). @DateFrom is moved to InventoryStockAsOnDate.AsOnDate + 1 day when it is earlier
     * than the latest packing-material as-on date. @IsPackTypeOn is forced to 0 inside the procedure. @SaleValue = 1
     * values outgoing stock at AmountOut instead of CgsAmount. @SkipZero (non-null) drops rows with BalWeight <= 0.05.
     * Parameters @ProjectsId, @UomId and @AllowWipItem are accepted but unused by the procedure.
     */
    @Query(value = "EXEC dbo.Sp_ItemStockReportWithValues_Store"
         + " @OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, @ProjectsId = :projectsId,"
         + " @DateFrom = :dateFrom, @DateTo = :dateTo, @ItemTypeId = :itemTypeId, @ItemCategoryId = :itemCategoryId,"
         + " @CropYear = :cropYear, @JobLotId = :jobLotId, @WarehouseId = :warehouseId, @UomId = :uomId,"
         + " @ItemId = :itemId, @PackUom = :packUom, @PackingTypeId = :packingTypeId, @IsPackSizeOn = :isPackSizeOn,"
         + " @IsPackTypeOn = :isPackTypeOn, @ItemStockAc = :itemStockAc, @AllowWipItem = :allowWipItem, @ItemClassGroupId = :itemClassGroupId,"
         + " @InventoryParentCategoriesIds = :inventoryParentCategoriesIds, @SaleValue = :saleValue, @SkipZero = :skipZero, @ItemConditionId = :itemConditionId,"
         + " @RackId = :rackId, @BranchesIds = :branchesIds,"
         + " @Activity = 'JobLotandItemStockSummary'",
         nativeQuery = true)
    List<ItemStockReportWithValuesStoreRow> jobLotAndItemStockSummary(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("projectsId") Integer projectsId,
            @Param("dateFrom") LocalDate dateFrom,
            @Param("dateTo") LocalDate dateTo,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("cropYear") String cropYear,
            @Param("jobLotId") Integer jobLotId,
            @Param("warehouseId") Integer warehouseId,
            @Param("uomId") Integer uomId,
            @Param("itemId") Integer itemId,
            @Param("packUom") Double packUom,
            @Param("packingTypeId") Integer packingTypeId,
            @Param("isPackSizeOn") Integer isPackSizeOn,
            @Param("isPackTypeOn") Integer isPackTypeOn,
            @Param("itemStockAc") Integer itemStockAc,
            @Param("allowWipItem") Integer allowWipItem,
            @Param("itemClassGroupId") Integer itemClassGroupId,
            @Param("inventoryParentCategoriesIds") String inventoryParentCategoriesIds,
            @Param("saleValue") Integer saleValue,
            @Param("skipZero") Integer skipZero,
            @Param("itemConditionId") Integer itemConditionId,
            @Param("rackId") Integer rackId,
            @Param("branchesIds") String branchesIds);

    /**
     * Sp_ItemStockReportWithValues_Store @Activity='WarehouseandJoblotandItemStockSummary': grouped by warehouse + rack + JobLot + item; CropYear = ''; ordered by WareHouseName, JobLot, ItemName.
     * // KEEP-EXEC: ReportCriteria is assembled procedurally (FORMAT / String_agg / IF chains), the opening/period
     * union is staged into #f and re-grouped per @Activity into #TMPFinalData, with a conditional UPDATE from
     * #StockSummaryByItem (ROW_NUMBER) before the final SELECT.
     * Proc defaults: all parameters NULL. @BranchesIds / @InventoryParentCategoriesIds are comma-separated id lists
     * (split with fnSplitString). @DateFrom is moved to InventoryStockAsOnDate.AsOnDate + 1 day when it is earlier
     * than the latest packing-material as-on date. @IsPackTypeOn is forced to 0 inside the procedure. @SaleValue = 1
     * values outgoing stock at AmountOut instead of CgsAmount. @SkipZero (non-null) drops rows with BalWeight <= 0.05.
     * Parameters @ProjectsId, @UomId and @AllowWipItem are accepted but unused by the procedure.
     */
    @Query(value = "EXEC dbo.Sp_ItemStockReportWithValues_Store"
         + " @OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, @ProjectsId = :projectsId,"
         + " @DateFrom = :dateFrom, @DateTo = :dateTo, @ItemTypeId = :itemTypeId, @ItemCategoryId = :itemCategoryId,"
         + " @CropYear = :cropYear, @JobLotId = :jobLotId, @WarehouseId = :warehouseId, @UomId = :uomId,"
         + " @ItemId = :itemId, @PackUom = :packUom, @PackingTypeId = :packingTypeId, @IsPackSizeOn = :isPackSizeOn,"
         + " @IsPackTypeOn = :isPackTypeOn, @ItemStockAc = :itemStockAc, @AllowWipItem = :allowWipItem, @ItemClassGroupId = :itemClassGroupId,"
         + " @InventoryParentCategoriesIds = :inventoryParentCategoriesIds, @SaleValue = :saleValue, @SkipZero = :skipZero, @ItemConditionId = :itemConditionId,"
         + " @RackId = :rackId, @BranchesIds = :branchesIds,"
         + " @Activity = 'WarehouseandJoblotandItemStockSummary'",
         nativeQuery = true)
    List<ItemStockReportWithValuesStoreRow> warehouseAndJobLotAndItemStockSummary(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("projectsId") Integer projectsId,
            @Param("dateFrom") LocalDate dateFrom,
            @Param("dateTo") LocalDate dateTo,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("cropYear") String cropYear,
            @Param("jobLotId") Integer jobLotId,
            @Param("warehouseId") Integer warehouseId,
            @Param("uomId") Integer uomId,
            @Param("itemId") Integer itemId,
            @Param("packUom") Double packUom,
            @Param("packingTypeId") Integer packingTypeId,
            @Param("isPackSizeOn") Integer isPackSizeOn,
            @Param("isPackTypeOn") Integer isPackTypeOn,
            @Param("itemStockAc") Integer itemStockAc,
            @Param("allowWipItem") Integer allowWipItem,
            @Param("itemClassGroupId") Integer itemClassGroupId,
            @Param("inventoryParentCategoriesIds") String inventoryParentCategoriesIds,
            @Param("saleValue") Integer saleValue,
            @Param("skipZero") Integer skipZero,
            @Param("itemConditionId") Integer itemConditionId,
            @Param("rackId") Integer rackId,
            @Param("branchesIds") String branchesIds);

    /**
     * Sp_ItemStockReportWithValues_Store with ISNULL(@Activity,'') = '' (the no-activity branch): returns the
     * un-grouped #f rows (ItemId, ItemClassGroupId, InventoryParentCategoriesId, TypeofTypes, ... BalAmount) plus
     * AvgRate (unrounded), Company / Organization columns and FromDate / ToDate. No ReportCriteria column and
     * @SkipZero is not applied in this branch.
     * // KEEP-EXEC: ReportCriteria is assembled procedurally (FORMAT / String_agg / IF chains), the opening/period
     * union is staged into #f and re-grouped per @Activity into #TMPFinalData, with a conditional UPDATE from
     * #StockSummaryByItem (ROW_NUMBER) before the final SELECT.
     * Proc defaults: all parameters NULL. @BranchesIds / @InventoryParentCategoriesIds are comma-separated id lists
     * (split with fnSplitString). @DateFrom is moved to InventoryStockAsOnDate.AsOnDate + 1 day when it is earlier
     * than the latest packing-material as-on date. @IsPackTypeOn is forced to 0 inside the procedure. @SaleValue = 1
     * values outgoing stock at AmountOut instead of CgsAmount. @SkipZero (non-null) drops rows with BalWeight <= 0.05.
     * Parameters @ProjectsId, @UomId and @AllowWipItem are accepted but unused by the procedure.
     */
    @Query(value = "EXEC dbo.Sp_ItemStockReportWithValues_Store"
         + " @OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, @ProjectsId = :projectsId,"
         + " @DateFrom = :dateFrom, @DateTo = :dateTo, @ItemTypeId = :itemTypeId, @ItemCategoryId = :itemCategoryId,"
         + " @CropYear = :cropYear, @JobLotId = :jobLotId, @WarehouseId = :warehouseId, @UomId = :uomId,"
         + " @ItemId = :itemId, @PackUom = :packUom, @PackingTypeId = :packingTypeId, @IsPackSizeOn = :isPackSizeOn,"
         + " @IsPackTypeOn = :isPackTypeOn, @ItemStockAc = :itemStockAc, @AllowWipItem = :allowWipItem, @ItemClassGroupId = :itemClassGroupId,"
         + " @InventoryParentCategoriesIds = :inventoryParentCategoriesIds, @SaleValue = :saleValue, @SkipZero = :skipZero, @ItemConditionId = :itemConditionId,"
         + " @RackId = :rackId, @BranchesIds = :branchesIds,"
         + " @Activity = NULL",
         nativeQuery = true)
    List<ItemStockReportWithValuesStoreRawRow> itemStockRawSummary(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("projectsId") Integer projectsId,
            @Param("dateFrom") LocalDate dateFrom,
            @Param("dateTo") LocalDate dateTo,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("cropYear") String cropYear,
            @Param("jobLotId") Integer jobLotId,
            @Param("warehouseId") Integer warehouseId,
            @Param("uomId") Integer uomId,
            @Param("itemId") Integer itemId,
            @Param("packUom") Double packUom,
            @Param("packingTypeId") Integer packingTypeId,
            @Param("isPackSizeOn") Integer isPackSizeOn,
            @Param("isPackTypeOn") Integer isPackTypeOn,
            @Param("itemStockAc") Integer itemStockAc,
            @Param("allowWipItem") Integer allowWipItem,
            @Param("itemClassGroupId") Integer itemClassGroupId,
            @Param("inventoryParentCategoriesIds") String inventoryParentCategoriesIds,
            @Param("saleValue") Integer saleValue,
            @Param("skipZero") Integer skipZero,
            @Param("itemConditionId") Integer itemConditionId,
            @Param("rackId") Integer rackId,
            @Param("branchesIds") String branchesIds);

    /**
     * pcc.[USP-InventoryStockTransactionsReport] (single branch).
     * // KEEP-EXEC: ReportCriteria is assembled procedurally (FORMAT, STRING_AGG, IF chains with lookups on ItemType /
     * ItemCategory / Item / InventoryParentCategories / ItemClassGroup / JobLot / InvWareHouse) and prefixed to the
     * #s -> #Data pipeline (ROW_NUMBER + windowed running balances).
     * Proc defaults: all parameters NULL. @ItemTypeIds / @WarehouseIds are comma-separated id lists.
     */
    @Query(value = "EXEC pcc.[USP-InventoryStockTransactionsReport]"
         + " @OrganizationId = :organizationId, @CompanyId = :companyId, @SupplierCustomerId = :supplierCustomerId,"
         + " @ItemId = :itemId, @DateFrom = :dateFrom, @DateTo = :dateTo, @WarehouseId = :warehouseId,"
         + " @JobLotId = :jobLotId, @DocumentTypeId = :documentTypeId, @ItemClassGroupId = :itemClassGroupId,"
         + " @InventoryParentCategoriesId = :inventoryParentCategoriesId, @ItemCategoryId = :itemCategoryId,"
         + " @ItemTypeId = :itemTypeId, @ItemTypeIds = :itemTypeIds, @WarehouseIds = :warehouseIds",
         nativeQuery = true)
    List<InventoryStockTransactionsReportRow> inventoryStockTransactionsReport(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("supplierCustomerId") Integer supplierCustomerId,
            @Param("itemId") Integer itemId,
            @Param("dateFrom") LocalDate dateFrom,
            @Param("dateTo") LocalDate dateTo,
            @Param("warehouseId") Integer warehouseId,
            @Param("jobLotId") Integer jobLotId,
            @Param("documentTypeId") Integer documentTypeId,
            @Param("itemClassGroupId") Integer itemClassGroupId,
            @Param("inventoryParentCategoriesId") Integer inventoryParentCategoriesId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("itemTypeIds") String itemTypeIds,
            @Param("warehouseIds") String warehouseIds);
}
