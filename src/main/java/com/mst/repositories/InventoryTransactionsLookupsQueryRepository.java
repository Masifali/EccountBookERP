package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.projections.InventoryTransactionsDropDownRow;

/**
 * Sp_Inventory_InventoryTransactions_DropDownAndLists (query-only; bound to InventoryItemsOther for Spring Data).
 */
@Repository
public interface InventoryTransactionsLookupsQueryRepository
        extends org.springframework.data.repository.Repository<com.mst.models.InventoryItemsOther, Integer> {

    /**
     * Sp_Inventory_InventoryTransactions_DropDownAndLists (single branch; row kind chosen by @ActivityType, NULL = all kinds).
     * Temp-table pipeline (#MainTable -> 14 INSERTs into #InventoryTransactions -> SELECT) rewritten as one CTE + UNION ALL statement.
     * All proc parameters default to NULL; @BranchesId, @ProjectsId and @ReferenceId are declared by the proc but never used.
     * @BranchesIds / @InventoryParentCategory are comma-separated lists split by dbo.fnSplitString.
     */
    @Query(value = "WITH MainTable AS ("
                 + "SELECT DISTINCT st.ItemId, st.SupplierCustomerId, st.InvPackingTypeId, st.JobLotId, st.WarehouseId, st.CropBatch, "
                 + "ic.InventoryParentCategoriesId As ParentCategoryId, st.RefDocumentTypeId, st.ItemConditionId, st.RackId "
                 + "FROM InventoryTransactions st "
                 + "INNER JOIN item i on st.ItemId = i.Id "
                 + "INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.id "
                 + "WHERE st.OrganizationId = :organizationId AND st.CompanyId = :companyId "
                 + "AND (:inventoryParentCategory IS NULL OR EXISTS (SELECT 1 FROM fnSplitString(:inventoryParentCategory, ',') AS dt WHERE dt.Data = ic.InventoryParentCategoriesId)) "
                 + "AND (:branchesIds IS NULL OR EXISTS (SELECT 1 FROM fnSplitString(:branchesIds, ',') AS dt WHERE dt.Data = st.BranchesId))"
                 + ") "
                 + "SELECT R.Id, R.name, R.ActivityType, R.ParentCategoryId FROM ("
                 + "SELECT DISTINCT 'TypeOfType' as ActivityType, il.Id as Id, il.LookupName as name, st.ParentCategoryId "
                 + "FROM MainTable st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ItemType it on i.ItemTypeId = it.Id INNER JOIN InvLookUp il on il.Id = it.Type "
                 + "WHERE (:activityType IS NULL OR 'TypeOfType' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'ItemTypes' as ActivityType, it.Id as Id, it.TypeDescription as name, st.ParentCategoryId "
                 + "FROM MainTable st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ItemType it on i.ItemTypeId = it.Id "
                 + "WHERE (:activityType IS NULL OR 'ItemTypes' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'ItemCategories' as ActivityType, ic.Id as Id, ic.CategoryDescription as name, st.ParentCategoryId "
                 + "FROM MainTable st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.id "
                 + "WHERE (:activityType IS NULL OR 'ItemCategories' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'ParentCategories' as ActivityType, ip.Id as Id, ip.InvParentCateDescription as name, st.ParentCategoryId "
                 + "FROM MainTable st INNER JOIN InventoryParentCategories ip on st.ParentCategoryId = ip.Id "
                 + "WHERE (:activityType IS NULL OR 'ParentCategories' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'ItemClassGroup' as ActivityType, cg.Id as Id, cg.ClassGroupName as name, st.ParentCategoryId "
                 + "FROM MainTable st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.id INNER JOIN ItemClassGroup cg on ic.ItemClassGroupId = cg.Id "
                 + "WHERE (:activityType IS NULL OR 'ItemClassGroup' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'Items' as ActivityType, st.ItemId as Id, ItemName as name, st.ParentCategoryId "
                 + "FROM MainTable st INNER JOIN item i on st.ItemId = i.Id INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.id "
                 + "WHERE (:activityType IS NULL OR 'Items' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'ItemCondition' as ActivityType, st.ItemConditionId as Id, jb.ConditionStatus as name, st.ParentCategoryId "
                 + "FROM MainTable st INNER JOIN V_ItemCondition jb on st.ItemConditionId = jb.Id "
                 + "WHERE (:activityType IS NULL OR 'ItemCondition' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'RackName' as ActivityType, st.RackId as Id, jb.rackName as name, st.ParentCategoryId "
                 + "FROM MainTable st INNER JOIN invWarehouseRack jb on st.RackId = jb.Id "
                 + "WHERE (:activityType IS NULL OR 'RackName' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'PackingType' as ActivityType, st.InvPackingTypeId as Id, jb.PackTypeDesc as name, st.ParentCategoryId "
                 + "FROM MainTable st INNER JOIN InvPackingType jb on st.InvPackingTypeId = jb.Id "
                 + "WHERE (:activityType IS NULL OR 'PackingType' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'JobLot' as ActivityType, st.JobLotId as Id, jb.JobLotCode as name, st.ParentCategoryId "
                 + "FROM MainTable st INNER JOIN JobLot jb on st.JobLotId = jb.Id "
                 + "WHERE (:activityType IS NULL OR 'JobLot' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'CropYear' as ActivityType, cy.Id as Id, cy.CropYear as name, st.ParentCategoryId "
                 + "FROM MainTable st INNER JOIN InvCropYear cy on st.CropBatch = cy.CropYear "
                 + "WHERE (:activityType IS NULL OR 'CropYear' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'Warehouse' as ActivityType, st.WarehouseId as Id, wr.WareHouseName as name, st.ParentCategoryId "
                 + "FROM MainTable st INNER JOIN InvWareHouse wr on st.WarehouseId = wr.Id "
                 + "WHERE (:activityType IS NULL OR 'Warehouse' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'Supplier_Customer' as ActivityType, st.SupplierCustomerId as Id, s.CompanyName as name, st.ParentCategoryId "
                 + "FROM MainTable st INNER JOIN SupplierCustomer s on st.SupplierCustomerId = s.Id "
                 + "WHERE (:activityType IS NULL OR 'Supplier_Customer' = :activityType) "
                 + "UNION ALL "
                 + "SELECT DISTINCT 'DocumentType' as ActivityType, st.RefDocumentTypeId as Id, s.DocumentTypeDescription as name, st.ParentCategoryId "
                 + "FROM MainTable st INNER JOIN DocumentType s on st.RefDocumentTypeId = s.Id "
                 + "WHERE (:activityType IS NULL OR 'DocumentType' = :activityType)"
                 + ") R",
           nativeQuery = true)
    List<InventoryTransactionsDropDownRow> dropDownAndLists(@Param("organizationId") Integer organizationId,
                                                            @Param("companyId") Integer companyId,
                                                            @Param("activityType") String activityType,
                                                            @Param("branchesIds") String branchesIds,
                                                            @Param("inventoryParentCategory") String inventoryParentCategory);
}
