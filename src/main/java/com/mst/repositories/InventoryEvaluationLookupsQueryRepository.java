package com.mst.repositories;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.projections.EvaluationDropDownByParentCategoriesRow;
import com.mst.projections.InventoryEvaluationLedgerStoreRow;
import com.mst.projections.StockEvaluationDetailByRefRefIdsRow;
import com.mst.projections.StockEvaluationDetailDropDownRow;

/**
 * Query-only repository for the InventoryStockEvalautionDetail lookup / report procedures
 * (USP_Evalaution_DropDown_ByParentCategories, USP_Inventory_StockEvalautionDetail_DropDownAndLists,
 * USP_GetStockEvalautionDetailByRefRefIds, USP_InventoryEvaluationLedgerStore).
 * Bound to InventoryItemsOther only so Spring Data has a managed entity.
 */
@Repository
public interface InventoryEvaluationLookupsQueryRepository
        extends org.springframework.data.repository.Repository<com.mst.models.InventoryItemsOther, Integer> {

    /**
     * USP_Evalaution_DropDown_ByParentCategories (single branch; row kind chosen by @ActivityType, NULL = all kinds).
     * Temp-table pipeline (#DATA -> 23 INSERTs into #EvaluationDropDown -> SELECT DISTINCT) rewritten as one CTE + UNION ALL statement.
     * All proc parameters default to NULL. @DocumentTypeIds / @BranchesIds / @InventoryParentCategory are comma-separated lists
     * split by dbo.fnSplitString. Note (kept from the proc): the 'District' rows are emitted under @ActivityType='City',
     * and the 'BatchNo' rows are emitted regardless of @ActivityType.
     */
    @Query(value = "WITH DATA AS ("
                 + "SELECT DISTINCT st.ItemId, st.CropBatch, st.CropYearId, st.JobLotId, st.WarehouseId, st.InvPackingTypeId, "
                 + "st.SupplierCustomerId, st.RefDocumentTypeId, st.CityId, ic.InventoryParentCategoriesId, st.VarientId, st.WorkOrderId, "
                 + "st.ProductionStageId, st.WorkStationId, st.BatchNo, st.CastingTypeId, st.ItemConditionId, st.RackId "
                 + "FROM InventoryStockEvalautionDetail st "
                 + "INNER JOIN item i on st.ItemId = i.Id "
                 + "INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.id "
                 + "WHERE st.OrganizationId = :organizationId AND st.CompanyId = :companyId "
                 + "AND (:documentTypeIds IS NULL OR EXISTS (SELECT Data FROM fnSplitString(:documentTypeIds, ',') WHERE st.RefDocumentTypeId in (data))) "
                 + "AND (:branchesIds IS NULL OR EXISTS (SELECT 1 FROM dbo.fnSplitString(:branchesIds, ',') WHERE st.BranchesId in (data))) "
                 + "AND (:inventoryParentCategory IS NULL OR EXISTS (SELECT 1 FROM fnSplitString(:inventoryParentCategory, ',') AS dt WHERE dt.Data = ic.InventoryParentCategoriesId)) "
                 + "AND (:itemConditionId IS NULL OR st.ItemConditionId = :itemConditionId)"
                 + ") "
                 + "SELECT DISTINCT R.ActivityType, R.Id, R.name, R.InventoryParentCategoriesId FROM ("
                 + "SELECT DISTINCT 'TypeOfType' as ActivityType, il.Id as Id, il.LookupName as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ItemType it on i.ItemTypeId = it.Id INNER JOIN InvLookUp il on il.Id = it.Type "
                 + "WHERE (:activityType IS NULL OR 'TypeOfType' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'ItemTypes' as ActivityType, it.Id as Id, it.TypeDescription as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ItemType it on i.ItemTypeId = it.Id "
                 + "WHERE (:activityType IS NULL OR 'ItemTypes' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'ItemCategories' as ActivityType, ic.Id as Id, ic.CategoryDescription as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.id "
                 + "WHERE (:activityType IS NULL OR 'ItemCategories' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'ParentCategories' as ActivityType, ip.Id as Id, ip.InvParentCateDescription as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.id INNER JOIN InventoryParentCategories ip on ic.InventoryParentCategoriesId = ip.Id "
                 + "WHERE (:activityType IS NULL OR 'ParentCategories' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'ItemClassGroup' as ActivityType, cg.Id as Id, cg.ClassGroupName as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.id INNER JOIN ItemClassGroup cg on ic.ItemClassGroupId = cg.Id "
                 + "WHERE (:activityType IS NULL OR 'ItemClassGroup' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'Items' as ActivityType, st.ItemId as Id, ItemName as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN item i on st.ItemId = i.Id "
                 + "WHERE (:activityType IS NULL OR 'Items' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'ItemCondition' as ActivityType, st.ItemConditionId as Id, i.ConditionStatus as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN V_ItemCondition i on st.ItemConditionId = i.ID "
                 + "WHERE (:activityType IS NULL OR 'ItemCondition' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'RackName' as ActivityType, st.RackId as Id, i.rackName as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN invWarehouseRack i on st.RackId = i.ID "
                 + "WHERE (:activityType IS NULL OR 'RackName' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'JobLot' as ActivityType, st.JobLotId as Id, jb.JobLotCode as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN JobLot jb on st.JobLotId = jb.Id "
                 + "WHERE (:activityType IS NULL OR 'JobLot' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'CropYear' as ActivityType, cy.Id as Id, cy.CropYear as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN InvCropYear cy on st.CropBatch = cy.CropYear and cy.CompanyId = :companyId "
                 + "WHERE (:activityType IS NULL OR 'CropYear' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'PackingType' as ActivityType, st.InvPackingTypeId as Id, wr.PackTypeDesc as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN InvPackingType wr on st.InvPackingTypeId = wr.Id "
                 + "WHERE (:activityType IS NULL OR 'PackingType' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'Warehouse' as ActivityType, st.WarehouseId as Id, wr.WareHouseName as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN InvWareHouse wr on st.WarehouseId = wr.Id "
                 + "WHERE (:activityType IS NULL OR 'Warehouse' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'Supplier_Customer' as ActivityType, st.SupplierCustomerId as Id, s.CompanyName as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN SupplierCustomer s on st.SupplierCustomerId = s.Id "
                 + "WHERE (:activityType IS NULL OR 'Supplier_Customer' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'DocumentType' as ActivityType, st.RefDocumentTypeId as Id, dt.DocumentTypeDescription as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN DocumentType dt on st.RefDocumentTypeId = dt.Id "
                 + "WHERE (:activityType IS NULL OR 'DocumentType' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'Stock_Account' as ActivityType, i.PurchaseGLAC as Id, c.AccountTitle as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ChartofAccount c on i.PurchaseGLAC = c.Id "
                 + "WHERE (:activityType IS NULL OR 'Stock_Account' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'GroupAccount' as ActivityType, c.ParentCodeId as Id, P.AccountTitle as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ChartofAccount c on i.PurchaseGLAC = c.Id INNER JOIN ChartofAccount P ON C.ParentCodeId = P.Id "
                 + "WHERE (:activityType IS NULL OR 'GroupAccount' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'City' as ActivityType, st.CityId as Id, i.Description as name, st.InventoryParentCategoriesId "
                 + "FROM City i INNER JOIN DATA st on st.CityId = i.Id "
                 + "WHERE (:activityType IS NULL OR 'City' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'District' as ActivityType, t.DistrictId as Id, dd.Description as name, st.InventoryParentCategoriesId "
                 + "FROM City i INNER JOIN Tehsil t on i.TehsilId = t.Id INNER JOIN District dd on t.DistrictId = dd.Id INNER JOIN DATA st on st.CityId = i.Id "
                 + "WHERE (:activityType IS NULL OR 'City' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'ModalDescription' as ActivityType, st.VarientId as Id, vi.ComputedName as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN ItemAttributeVarient vi on st.VarientId = vi.Id "
                 + "WHERE (:activityType IS NULL OR 'ModalDescription' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'WorkOrder' as ActivityType, st.WorkOrderId as Id, r.ManualDocCode as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN mfg.WorkOrder r on st.WorkOrderId = r.WorkOrderId "
                 + "WHERE (:activityType IS NULL OR 'WorkOrder' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'ProductionStage' as ActivityType, st.ProductionStageId as Id, r.LookupName as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN mfg.LookUps r on st.ProductionStageId = r.Id "
                 + "WHERE (:activityType IS NULL OR 'ProductionStage' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'CastingType' as ActivityType, st.CastingTypeId as Id, r.LookupName as name, st.InventoryParentCategoriesId "
                 + "FROM DATA st INNER JOIN mfg.LookUps r on st.CastingTypeId = r.Id "
                 + "WHERE (:activityType IS NULL OR 'CastingType' = :activityType) "
                 + "UNION ALL "
                 + "SELECT 'BatchNo' as ActivityType, ROW_NUMBER() OVER (ORDER BY A.BatchNo) AS Id, CAST(A.BatchNo AS nvarchar(max)) as name, A.InventoryParentCategoriesId "
                 + "FROM (SELECT DISTINCT st.BatchNo, st.InventoryParentCategoriesId FROM DATA st WHERE NOT st.BatchNo IS NULL) A"
                 + ") R",
           nativeQuery = true)
    List<EvaluationDropDownByParentCategoriesRow> evaluationDropDownByParentCategories(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("documentTypeIds") String documentTypeIds,
            @Param("activityType") String activityType,
            @Param("branchesIds") String branchesIds,
            @Param("inventoryParentCategory") String inventoryParentCategory,
            @Param("itemConditionId") Integer itemConditionId);

    /**
     * USP_Inventory_StockEvalautionDetail_DropDownAndLists (single branch; row kind chosen by @ActivityType, NULL = all kinds).
     * Temp-table pipeline (#DATA -> 16 INSERTs into #EvaluationDropDown -> SELECT *) rewritten as one CTE + UNION ALL statement.
     * All proc parameters default to NULL. @DocumentTypeIds / @BranchesIds / @InventoryParentCategory are comma-separated lists
     * split by dbo.fnSplitString. Note (kept from the proc): the 'District' rows are emitted under @ActivityType='City'.
     */
    @Query(value = "WITH DATA AS ("
                 + "SELECT DISTINCT st.ItemId, st.CropBatch, st.CropYearId, st.JobLotId, st.WarehouseId, st.InvPackingTypeId, "
                 + "st.SupplierCustomerId, st.RefDocumentTypeId, st.CityId, st.RefDocIdNo "
                 + "FROM InventoryStockEvalautionDetail st "
                 + "INNER JOIN item i on st.ItemId = i.Id "
                 + "INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.id "
                 + "WHERE st.OrganizationId = :organizationId AND st.CompanyId = :companyId "
                 + "AND (:documentTypeIds IS NULL OR EXISTS (SELECT Data FROM fnSplitString(:documentTypeIds, ',') WHERE st.RefDocumentTypeId in (data))) "
                 + "AND (:branchesIds IS NULL OR EXISTS (SELECT 1 FROM dbo.fnSplitString(:branchesIds, ',') WHERE st.BranchesId in (data))) "
                 + "AND (:inventoryParentCategory IS NULL OR EXISTS (SELECT 1 FROM fnSplitString(:inventoryParentCategory, ',') AS dt WHERE dt.Data = ic.InventoryParentCategoriesId))"
                 + ") "
                 + "SELECT R.ActivityType, R.Id, R.name FROM ("
                 + "SELECT DISTINCT 'TypeOfType' as ActivityType, il.Id as Id, il.LookupName as name "
                 + "FROM DATA st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ItemType it on i.ItemTypeId = it.Id INNER JOIN InvLookUp il on il.Id = it.Type "
                 + "WHERE (:activityType IS NULL OR 'TypeOfType' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'ItemTypes' as ActivityType, it.Id as Id, it.TypeDescription as name "
                 + "FROM DATA st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ItemType it on i.ItemTypeId = it.Id "
                 + "WHERE (:activityType IS NULL OR 'ItemTypes' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'ItemCategories' as ActivityType, ic.Id as Id, ic.CategoryDescription as name "
                 + "FROM DATA st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.id "
                 + "WHERE (:activityType IS NULL OR 'ItemCategories' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'ParentCategories' as ActivityType, ip.Id as Id, ip.InvParentCateDescription as name "
                 + "FROM DATA st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.id INNER JOIN InventoryParentCategories ip on ic.InventoryParentCategoriesId = ip.Id "
                 + "WHERE (:activityType IS NULL OR 'ParentCategories' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'ItemClassGroup' as ActivityType, cg.Id as Id, cg.ClassGroupName as name "
                 + "FROM DATA st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.id INNER JOIN ItemClassGroup cg on ic.ItemClassGroupId = cg.Id "
                 + "WHERE (:activityType IS NULL OR 'ItemClassGroup' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'Items' as ActivityType, st.ItemId as Id, ItemName as name "
                 + "FROM DATA st INNER JOIN item i on st.ItemId = i.Id "
                 + "WHERE (:activityType IS NULL OR 'Items' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'JobLot' as ActivityType, st.JobLotId as Id, jb.JobLotCode as name "
                 + "FROM DATA st INNER JOIN JobLot jb on st.JobLotId = jb.Id "
                 + "WHERE (:activityType IS NULL OR 'JobLot' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'CropYear' as ActivityType, cy.Id as Id, cy.CropYear as name "
                 + "FROM DATA st INNER JOIN InvCropYear cy on st.CropBatch = cy.CropYear "
                 + "WHERE (:activityType IS NULL OR 'CropYear' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'PackingType' as ActivityType, st.InvPackingTypeId as Id, wr.PackTypeDesc as name "
                 + "FROM DATA st INNER JOIN InvPackingType wr on st.InvPackingTypeId = wr.Id "
                 + "WHERE (:activityType IS NULL OR 'PackingType' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'Warehouse' as ActivityType, st.WarehouseId as Id, wr.WareHouseName as name "
                 + "FROM DATA st INNER JOIN InvWareHouse wr on st.WarehouseId = wr.Id "
                 + "WHERE (:activityType IS NULL OR 'Warehouse' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'Supplier_Customer' as ActivityType, st.SupplierCustomerId as Id, s.CompanyName as name "
                 + "FROM DATA st INNER JOIN SupplierCustomer s on st.SupplierCustomerId = s.Id "
                 + "WHERE (:activityType IS NULL OR 'Supplier_Customer' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'PurchaseBookingPerson' as ActivityType, d.BookingPersonId as Id, sp.ReferencePartyName as Name "
                 + "FROM DATA pih INNER JOIN InvPurchaseInvoiceDetail pd on pd.InvPurchaseInvoiceId = pih.RefDocIdNo INNER JOIN PurchaseOrder d on d.Id = pd.PurchaseOrderId INNER JOIN ReferenceParties sp on d.BookingPersonId = sp.Id "
                 + "WHERE (:activityType IS NULL OR 'PurchaseBookingPerson' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'DocumentType' as ActivityType, st.RefDocumentTypeId as Id, dt.DocumentTypeDescription as name "
                 + "FROM DATA st INNER JOIN DocumentType dt on st.RefDocumentTypeId = dt.Id "
                 + "WHERE (:activityType IS NULL OR 'DocumentType' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'Stock_Account' as ActivityType, i.PurchaseGLAC as Id, c.AccountTitle as name "
                 + "FROM DATA st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ChartofAccount c on i.PurchaseGLAC = c.Id "
                 + "WHERE (:activityType IS NULL OR 'Stock_Account' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'City' as ActivityType, st.CityId as Id, i.Description as name "
                 + "FROM City i INNER JOIN DATA st on st.CityId = i.Id "
                 + "WHERE (:activityType IS NULL OR 'City' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'District' as ActivityType, t.DistrictId as Id, dd.Description as name "
                 + "FROM City i INNER JOIN Tehsil t on i.TehsilId = t.Id INNER JOIN District dd on t.DistrictId = dd.Id INNER JOIN DATA st on st.CityId = i.Id "
                 + "WHERE (:activityType IS NULL OR 'City' = :activityType)"
                 + ") R",
           nativeQuery = true)
    List<StockEvaluationDetailDropDownRow> stockEvaluationDetailDropDownAndLists(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("documentTypeIds") String documentTypeIds,
            @Param("activityType") String activityType,
            @Param("branchesIds") String branchesIds,
            @Param("inventoryParentCategory") String inventoryParentCategory);

    /**
     * USP_GetStockEvalautionDetailByRefRefIds (single branch).
     * // KEEP-EXEC: multi-step temp-table pipeline (#Intemp -> #TmpFinal -> #TmpRefNos with STRING_AGG -> UPDATE #TmpFinal joined
     * // on a non-unique key (RefDocumentTypeId, RefDocIdNo only) -> second INSERT self-joined to the already-updated #TmpFinal);
     * // the UPDATE is not expressible as a deterministic computed column, so the procedure is kept.
     * @OrganizationId / @CompanyId are required; every other parameter defaults to NULL (= no filter). @DateFrom/@DateTo are DATE.
     * Output: all #TmpFinal columns plus Company / Organization report-header columns, ORDER BY DocDate.
     */
    @Query(value = "EXEC dbo.USP_GetStockEvalautionDetailByRefRefIds @OrganizationId = :organizationId, @CompanyId = :companyId, "
                 + "@InventoryParentCategories = :inventoryParentCategories, @WarehouseId = :warehouseId, "
                 + "@SupplierCustomerId = :supplierCustomerId, @ItemId = :itemId, @CropYear = :cropYear, "
                 + "@ReferenceDocumentTypeId = :referenceDocumentTypeId, @JobLotId = :jobLotId, @PackingTypeId = :packingTypeId, "
                 + "@ItemUomId = :itemUomId, @DateFrom = :dateFrom, @DateTo = :dateTo",
           nativeQuery = true)
    List<StockEvaluationDetailByRefRefIdsRow> getStockEvaluationDetailByRefRefIds(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("inventoryParentCategories") Integer inventoryParentCategories,
            @Param("warehouseId") Integer warehouseId,
            @Param("supplierCustomerId") Integer supplierCustomerId,
            @Param("itemId") Integer itemId,
            @Param("cropYear") String cropYear,
            @Param("referenceDocumentTypeId") Integer referenceDocumentTypeId,
            @Param("jobLotId") Integer jobLotId,
            @Param("packingTypeId") Integer packingTypeId,
            @Param("itemUomId") Integer itemUomId,
            @Param("dateFrom") LocalDate dateFrom,
            @Param("dateTo") LocalDate dateTo);

    /**
     * USP_InventoryEvaluationLedgerStore (single branch; store / packing-material item ledger, parent categories 7 and 8 only).
     * // KEEP-EXEC: procedural report - builds the ReportCriteria string with FORMAT(), looks up filter descriptions into variables,
     * // moves @DateFrom forward past the latest InventoryStockAsOnDate (ActionId = 2), fills #AllData (opening + period rows with
     * // ROW_NUMBER) and ends with a GROUP BY + windowed running balances. Kept as a procedure call.
     * All parameters default to NULL. @SaleValue = 1 uses AmountOut instead of CgsAmount for outgoing value.
     * @BranchesIds / @Ids (parent category ids) are comma-separated lists split by dbo.fnSplitString. @DateFrom/@DateTo are DATE.
     */
    @Query(value = "EXEC dbo.USP_InventoryEvaluationLedgerStore @OrganizationId = :organizationId, @CompanyId = :companyId, "
                 + "@DateFrom = :dateFrom, @DateTo = :dateTo, @ItemTypeId = :itemTypeId, @ItemCategoryId = :itemCategoryId, "
                 + "@WarehouseId = :warehouseId, @ItemId = :itemId, @ItemStockAc = :itemStockAc, @ItemClassGroupId = :itemClassGroupId, "
                 + "@InventoryParentCategoriesId = :inventoryParentCategoriesId, @SaleValue = :saleValue, @DocumentTypeId = :documentTypeId, "
                 + "@BranchesIds = :branchesIds, @SupplierCustomerId = :supplierCustomerId, @ItemConditionId = :itemConditionId, "
                 + "@RackId = :rackId, @Ids = :ids",
           nativeQuery = true)
    List<InventoryEvaluationLedgerStoreRow> inventoryEvaluationLedgerStore(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("dateFrom") LocalDate dateFrom,
            @Param("dateTo") LocalDate dateTo,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("warehouseId") Integer warehouseId,
            @Param("itemId") Integer itemId,
            @Param("itemStockAc") Integer itemStockAc,
            @Param("itemClassGroupId") Integer itemClassGroupId,
            @Param("inventoryParentCategoriesId") Integer inventoryParentCategoriesId,
            @Param("saleValue") Integer saleValue,
            @Param("documentTypeId") Integer documentTypeId,
            @Param("branchesIds") String branchesIds,
            @Param("supplierCustomerId") Integer supplierCustomerId,
            @Param("itemConditionId") Integer itemConditionId,
            @Param("rackId") Integer rackId,
            @Param("ids") String ids);
}
