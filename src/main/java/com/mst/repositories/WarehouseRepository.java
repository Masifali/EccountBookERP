package com.mst.repositories;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.Warehouse;
import com.mst.projections.WarehouseCodeNameRow;
import com.mst.projections.WarehouseIdNameRow;
import com.mst.projections.WarehouseReadRow;
import com.mst.projections.WarehouseStockEvalRow;
import com.mst.projections.WarehouseTypeRow;

/**
 * dbo.InvWareHouse (entity Warehouse) — Sp_InvWareHouse_Insert, Sp_InvWareHouse_Update, Sp_InvWareHouse_GetAllMethod.
 * (The project already has IWarehouseRepository; this one is additive and does not touch it.)
 *
 * Pre-insert logic of Sp_InvWareHouse_Insert (callers use save(); Id is IDENTITY, proc returns SCOPE_IDENTITY()):
 *  - Inserted columns: WareHouseCode, WareHouseName, EntryDate, EntryUser, OrganizationId, CompanyId, IsActive,
 *    BranchesId, WarehouseType (ModifyDate/ModifyUser parameters are ignored). All parameters default to NULL.
 *  - AFTER the insert, if dbo.fnCheckERPFeaturesExistorNot(@OrganizationId,@CompanyId,11) = 0 the proc also inserts one
 *    WarehousesAllocationToBranch row per Branch of the company (latest FinancialYear, first UserAccount of the company).
 *    That block is only reproduced by spInvWareHouseInsert() (KEEP-EXEC).
 */
@Repository
public interface WarehouseRepository extends JpaRepository<Warehouse, Integer> {

    /**
     * Sp_InvWareHouse_Insert (single branch) — full procedure incl. the conditional WarehousesAllocationToBranch
     * auto-allocation. All proc parameters default to NULL; @Id is an input that the proc overwrites with SCOPE_IDENTITY().
     * // KEEP-EXEC: calls dbo.fnCheckERPFeaturesExistorNot and inserts into WarehousesAllocationToBranch from
     * Company/Branches/FinancialYear/UserAccount (no entity for FinancialYear) after the main insert.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_InvWareHouse_Insert @CompanyId = :companyId, @EntryDate = :entryDate, @EntryUser = :entryUser, "
         + "@Id = :id, @ModifyDate = :modifyDate, @ModifyUser = :modifyUser, @OrganizationId = :organizationId, "
         + "@WareHouseCode = :wareHouseCode, @WareHouseName = :wareHouseName, @IsActive = :isActive, "
         + "@BranchesId = :branchesId, @WarehouseType = :warehouseType", nativeQuery = true)
    void spInvWareHouseInsert(@Param("companyId") Integer companyId,
                              @Param("entryDate") LocalDateTime entryDate,
                              @Param("entryUser") Integer entryUser,
                              @Param("id") Integer id,
                              @Param("modifyDate") LocalDateTime modifyDate,
                              @Param("modifyUser") Integer modifyUser,
                              @Param("organizationId") Integer organizationId,
                              @Param("wareHouseCode") String wareHouseCode,
                              @Param("wareHouseName") String wareHouseName,
                              @Param("isActive") Boolean isActive,
                              @Param("branchesId") Integer branchesId,
                              @Param("warehouseType") String warehouseType);

    /**
     * Sp_InvWareHouse_Update (single branch) — every SET column of the proc; EntryDate/EntryUser are NOT updated.
     * All parameters default to NULL.
     */
    @Modifying
    @Transactional
    @Query("UPDATE Warehouse w SET w.wareHouseCode = :wareHouseCode, w.wareHouseName = :wareHouseName, "
         + "w.modifyDate = :modifyDate, w.modifyUser = :modifyUser, w.organizationId = :organizationId, "
         + "w.companyId = :companyId, w.isActive = :isActive, w.branchesId = :branchesId, w.warehouseType = :warehouseType "
         + "WHERE w.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("wareHouseCode") String wareHouseCode,
                   @Param("wareHouseName") String wareHouseName,
                   @Param("modifyDate") LocalDateTime modifyDate,
                   @Param("modifyUser") Integer modifyUser,
                   @Param("organizationId") Integer organizationId,
                   @Param("companyId") Integer companyId,
                   @Param("isActive") Boolean isActive,
                   @Param("branchesId") Integer branchesId,
                   @Param("warehouseType") String warehouseType);

    /** Sp_InvWareHouse_GetAllMethod @Activity='ReadById' — SELECT * FROM InvWareHouse WHERE Id=@Id (take first). */
    @Query("SELECT w FROM Warehouse w WHERE w.id = :id")
    List<Warehouse> readById(@Param("id") Integer id);

    /**
     * Sp_InvWareHouse_GetAllMethod @Activity='ReadByOrganizationCompanyId' — native because WareHouseDempartmentType
     * has no entity (the output column WarehouseType is wt.WarehouseType, the type NAME, not the id).
     */
    @Query(value = "SELECT w.[Id], w.[WareHouseCode], w.[WareHouseName], w.[EntryDate], w.[EntryUser], w.[ModifyDate], "
         + "w.[ModifyUser], w.[OrganizationId], w.[CompanyId], w.IsActive, w.BranchesId, b.BranchName, wt.WarehouseType "
         + "FROM [dbo].[InvWareHouse] w "
         + "Left Join Branches b on b.Id = w.BranchesId "
         + "LEFT JOIN WareHouseDempartmentType wt on w.WarehouseType = wt.Id "
         + "where w.OrganizationId = :organizationId and w.CompanyId = :companyId", nativeQuery = true)
    List<WarehouseReadRow> readByOrganizationCompanyId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /**
     * Sp_InvWareHouse_GetAllMethod @Activity='GetWarehouseFromInventoryStockEvalution' — only warehouses used in
     * InventoryStockEvalautionDetail (entity planned by batch J1_stock_eval_update).
     */
    @Query("SELECT DISTINCT sd.warehouseId AS warehouseId, wh.wareHouseName AS wareHouseName "
         + "FROM InventoryStockEvalautionDetail sd "
         + "JOIN Warehouse wh ON sd.warehouseId = wh.id "
         + "WHERE wh.organizationId = :organizationId AND wh.companyId = :companyId")
    List<WarehouseStockEvalRow> getWarehouseFromInventoryStockEvalution(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_InvWareHouse_GetAllMethod @Activity='GetActiveWareHouse'. */
    @Query("SELECT DISTINCT w.id AS id, w.wareHouseName AS wareHouseName FROM Warehouse w "
         + "WHERE w.organizationId = :organizationId AND w.companyId = :companyId AND w.isActive = true")
    List<WarehouseIdNameRow> getActiveWareHouse(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /**
     * Sp_InvWareHouse_GetAllMethod @Activity='GetActiveWareHouseByWareHouseType' — native (CONVERT + dbo.fnSplitString).
     * @WarehouseType, @WarehouseTypeIds (comma list) and @BranchesIds (comma list) all default to NULL = no filter.
     */
    @Query(value = "SELECT distinct w.Id, w.WareHouseName FROM InvWareHouse w "
         + "INNEr JOIN WarehousesAllocationToBranch wB on Wb.WarehouseId = w.Id "
         + "WHERE w.OrganizationId = :organizationId AND w.CompanyId = :companyId "
         + "AND (:warehouseType IS NULL OR ISNULL(CONVERT(INT, w.WarehouseType), 0) = :warehouseType) "
         + "AND (:warehouseTypeIds IS NULL OR ISNULL(CONVERT(INT, w.WarehouseType), 0) IN (SELECT DATA FROM dbo.fnSplitString(:warehouseTypeIds, ','))) "
         + "AND w.IsActive = 1 "
         + "and (:branchesIds IS NULL OR EXISTS (select 1 from dbo.fnSplitString(:branchesIds, ',') Where wb.BranchId in (data)))", nativeQuery = true)
    List<WarehouseIdNameRow> getActiveWareHouseByWareHouseType(@Param("organizationId") Integer organizationId,
                                                               @Param("companyId") Integer companyId,
                                                               @Param("warehouseType") Integer warehouseType,
                                                               @Param("warehouseTypeIds") String warehouseTypeIds,
                                                               @Param("branchesIds") String branchesIds);

    /** Sp_InvWareHouse_GetAllMethod @Activity='GetWareHouseType' — native (WareHouseDempartmentType has no entity). */
    @Query(value = "SELECT Id, WareHouseType FROM WareHouseDempartmentType", nativeQuery = true)
    List<WarehouseTypeRow> getWareHouseType();

    /**
     * Sp_InvWareHouse_GetAllMethod @Activity='WareHouseAllocationToPlantsByProductionPlantId' — native
     * (WareHouseAllocationToPlants / InvProductionPlant have no entities).
     */
    @Query(value = "SELECT Distinct w.[Id], w.[WareHouseCode], w.[WareHouseName] FROM [dbo].[InvWareHouse] w "
         + "Inner Join WareHouseAllocationToPlants b on b.WarehouseId = w.Id and b.CompanyId = w.CompanyId "
         + "Inner join InvProductionPlant p on p.Id = b.PlantId and b.CompanyId = p.CompanyId "
         + "where w.OrganizationId = :organizationId and w.CompanyId = :companyId and (b.PlantId = :plantId)", nativeQuery = true)
    List<WarehouseCodeNameRow> wareHouseAllocationToPlantsByProductionPlantId(@Param("organizationId") Integer organizationId,
                                                                              @Param("companyId") Integer companyId,
                                                                              @Param("plantId") Integer plantId);
}
