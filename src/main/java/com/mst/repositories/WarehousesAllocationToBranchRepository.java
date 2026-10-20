package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.WarehousesAllocationToBranch;
import com.mst.projections.WarehouseAllocatedToBranchRow;
import com.mst.projections.WarehouseBranchAllocationRow;

/**
 * dbo.WarehousesAllocationToBranch — USP_WarehousesAllocationToBranch_Insert, USP_WarehousesAllocationToBranchDeleteById,
 * USP_WarehousesAllocatedOrUnAllocatedToBranch, USP_GetWarehousesAllocatedToBranch.
 *
 * Pre-insert logic of USP_WarehousesAllocationToBranch_Insert (callers use save()):
 *  - Id = ISNULL(MAX(Id),0)+1 over the whole table (maxId() + 1); proc returns SELECT @Id.
 *  - Inserted columns: Id, WarehouseId, BranchId, EntryDate, EntryUserId, OrganizationId, CompanyId, FinancialYearId
 *    (ModifyDate/ModifyUserId parameters are ignored). All parameters default to NULL.
 */
@Repository
public interface WarehousesAllocationToBranchRepository extends JpaRepository<WarehousesAllocationToBranch, Integer> {

    /** USP_WarehousesAllocationToBranch_Insert — next app-assigned Id: ISNULL(MAX(Id),0) (+1 by the caller). */
    @Query("select coalesce(max(e.id),0) from WarehousesAllocationToBranch e")
    Integer maxId();

    /**
     * USP_WarehousesAllocationToBranchDeleteById (single branch) — verbatim: @WarehouseIds is a comma-separated list
     * split by dbo.fnSplitString. Both parameters default to NULL.
     */
    @Modifying
    @Transactional
    @Query(value = "DELETE FROM dbo.WarehousesAllocationToBranch WHERE (BranchId = :branchId) "
         + "AND WarehouseId IN (SELECT data FROM dbo.fnSplitString(:warehouseIds, ','))", nativeQuery = true)
    int deleteByBranchIdAndWarehouseIds(@Param("branchId") Integer branchId, @Param("warehouseIds") String warehouseIds);

    /** USP_WarehousesAllocationToBranchDeleteById — JPQL equivalent for callers that already hold the ids as a list. */
    @Modifying
    @Transactional
    @Query("DELETE FROM WarehousesAllocationToBranch a WHERE a.branchId = :branchId AND a.warehouseId IN :warehouseIds")
    int deleteByBranchIdAndWarehouseIdIn(@Param("branchId") Integer branchId, @Param("warehouseIds") List<Integer> warehouseIds);

    /**
     * USP_WarehousesAllocatedOrUnAllocatedToBranch (single branch). @ActionId 1 = warehouses not allocated to any branch
     * of the company (BranchId is ignored), 2 = warehouses allocated to @BranchId. All parameters default to NULL.
     */
    @Query("SELECT g.id AS branchId, g.branchName AS branchName, i.id AS warehouseId, i.wareHouseName AS wareHouseName "
         + "FROM Warehouse i "
         + "LEFT JOIN WarehousesAllocationToBranch c ON i.id = c.warehouseId AND i.companyId = c.companyId "
         + "LEFT JOIN Branch g ON c.branchId = g.id AND g.companyId = c.companyId "
         + "WHERE i.organizationId = :organizationId AND i.companyId = :companyId "
         + "AND ((:actionId = 1 AND i.id NOT IN (SELECT c1.warehouseId FROM WarehousesAllocationToBranch c1 WHERE c1.companyId = :companyId)) "
         + "OR (:actionId = 2 AND i.id IN (SELECT c2.warehouseId FROM WarehousesAllocationToBranch c2 WHERE c2.companyId = :companyId AND c2.branchId = :branchId)))")
    List<WarehouseBranchAllocationRow> warehousesAllocatedOrUnAllocatedToBranch(@Param("organizationId") Integer organizationId,
                                                                                @Param("companyId") Integer companyId,
                                                                                @Param("actionId") Integer actionId,
                                                                                @Param("branchId") Integer branchId);

    /**
     * USP_GetWarehousesAllocatedToBranch (single branch) — native because WareHouseDempartmentType,
     * WareHouseAllocationToPlants and InvProductionPlant have no entities. @BranchId (default NULL) is an optional filter.
     */
    @Query(value = "SELECT c.WarehouseId Id, i.WareHouseName, c.BranchId, g.BranchName, i.WarehouseType As WareHouseTypeId, "
         + "WT.WareHouseType, i.IsActive, WP.PlantId, pp.Description PlantName "
         + "FROM dbo.WarehousesAllocationToBranch AS c "
         + "Inner JOIN dbo.InvWareHouse AS i ON i.Id = c.WarehouseId AND i.CompanyId = c.CompanyId "
         + "Inner JOIN dbo.Branches AS g ON c.BranchId = g.Id AND g.CompanyId = c.CompanyId "
         + "LEFT JOIN WareHouseDempartmentType WT ON WT.Id = CONVERT(int, i.WarehouseType) "
         + "LEFT Join WareHouseAllocationToPlants WP on WP.WarehouseId = i.Id and WP.CompanyId = i.CompanyId "
         + "LEFt Join InvProductionPlant pp on pp.Id = WP.PlantId "
         + "Where c.OrganizationId = :organizationId and c.CompanyId = :companyId "
         + "and (:branchId is null or c.BranchId = :branchId)", nativeQuery = true)
    List<WarehouseAllocatedToBranchRow> getWarehousesAllocatedToBranch(@Param("organizationId") Integer organizationId,
                                                                        @Param("companyId") Integer companyId,
                                                                        @Param("branchId") Integer branchId);
}
