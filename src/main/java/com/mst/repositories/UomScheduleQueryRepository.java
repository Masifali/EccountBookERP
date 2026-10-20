package com.mst.repositories;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.UOM;
import com.mst.projections.ItemBaseUomRow;
import com.mst.projections.UomForAssignToItemRow;
import com.mst.projections.UomScheduleByItemGroupRow;
import com.mst.projections.UomScheduleByItemRow;
import com.mst.projections.UomScheduleByOrganizationRow;
import com.mst.projections.UomScheduleReadByIdRow;
import com.mst.projections.UomsByCompanyRow;

/**
 * Query-only repository for the UOMSchedule procedures (Sp_UOMSchedule_GetAllMethod, Sp_UOMSchedule_Insert,
 * Sp_UOMSchedule_Update, usp_getAllUomsByCompanyId). The UOMSchedule entity belongs to batch C2_item_schedules and is
 * referenced here in JPQL by its registry column names; this repository is bound to the owned UOM entity.
 * <p>
 * Sp_UOMSchedule_Insert (procedural): if no row for (Org, Company, ItemId, ScheduleUnitId): Id = max(Id)+1 ({@link #maxId()}),
 * reset BaseRateUom / BasePackUom / BaseSecondaryUom flags of the item's other rows when the new row sets them
 * ({@link #resetBaseRateUom} / {@link #resetBasePackUom} / {@link #resetBaseSecondaryUom}), INSERT (ModifyDate/ModifyUser not
 * inserted); else re-activate the inactive existing row ({@link #findInactiveId} + {@link #activateById}) or RAISERROR
 * 'Record cannot be Inserted beacuase ScheduleUnit has already Exist'. Returns @Id. Whole proc: {@link #spUomScheduleInsert}.
 * <p>
 * Sp_UOMSchedule_Update (procedural): RAISERROR when @Equivalent = 0; when ScheduleUnitId changes and the new unit already
 * exists for the item RAISERROR; then the same three flag resets; then UPDATE ({@link #updateById}). Whole proc: {@link #spUomScheduleUpdate}.
 */
@Repository
public interface UomScheduleQueryRepository extends org.springframework.data.repository.Repository<UOM, Integer> {

    /** Sp_UOMSchedule_GetAllMethod @Activity='ReadById' */
    @Query("SELECT s.id AS id, s.itemId AS itemId, s.scheduleUnitId AS scheduleUnitId, s.equivalent AS equivalent, "
         + "s.entryDate AS entryDate, s.entryUser AS entryUser, s.modifyDate AS modifyDate, s.modifyUser AS modifyUser, "
         + "s.organizationId AS organizationId, s.companyId AS companyId, s.qtyEquivalent AS qtyEquivalent, "
         + "u.uomCode AS uomCode, u.uomDescription AS scheduleUnitName, "
         + "s.baseRateUom AS baseRateUom, s.basePackUom AS basePackUom, s.baseSecondaryUom AS baseSecondaryUom, s.active AS active "
         + "FROM UOMSchedule s JOIN UOM u ON s.scheduleUnitId = u.id WHERE s.id = :id")
    List<UomScheduleReadByIdRow> readById(@Param("id") Integer id);

    /** Sp_UOMSchedule_GetAllMethod @Activity='ReadByOrganizationCompanyId' (native: ItemPartyProcessing has no entity; @CompanyId filter commented out) */
    @Query(value = "SELECT UOMSchedule.Id, UOMSchedule.ItemId, ItemCodeNew as ItemCode, UOMSchedule.ScheduleUnitId, UOMSchedule.Equivalent, "
         + "UOMSchedule.EntryDate, UOMSchedule.EntryUser, UOMSchedule.ModifyDate, UOMSchedule.ModifyUser, "
         + "UOMSchedule.OrganizationId, UOMSchedule.CompanyId, UOM.UOMDescription, "
         + "CASE WHEN ISNULL(Item.ItemName,'') = '' THEN i.ItemName ELSE ITEM.ItemName END AS ItemName, "
         + "UOM.UOMCode, UOMSchedule.QtyEquivalent, UOMSchedule.BaseRateUom, UOMSchedule.BasePackUom, UOMSchedule.BaseSecondaryUom, UOMSchedule.Active "
         + "FROM UOMSchedule INNER JOIN UOM ON UOMSchedule.ScheduleUnitId = UOM.Id "
         + "LEFT JOIN Item ON UOMSchedule.ItemId = Item.Id "
         + "LEFT JOIN ItemPartyProcessing i ON UOMSchedule.ItemId = i.Id "
         + "where UOMSchedule.OrganizationId = :organizationId", nativeQuery = true)
    List<UomScheduleByOrganizationRow> readByOrganizationCompanyId(@Param("organizationId") Integer organizationId);

    /** Sp_UOMSchedule_GetAllMethod @Activity='ReadByItemID' (active schedules of the item, ordered by Equivalent; @CompanyId filter commented out) */
    @Query("SELECT s.id AS id, s.itemId AS itemId, s.scheduleUnitId AS scheduleUnitId, s.equivalent AS equivalent, u.uomCode AS uomCode, "
         + "s.entryDate AS entryDate, s.entryUser AS entryUser, s.modifyDate AS modifyDate, s.modifyUser AS modifyUser, "
         + "s.organizationId AS organizationId, s.companyId AS companyId, s.qtyEquivalent AS qtyEquivalent, "
         + "i.itemName AS itemName, u.uomDescription AS scheduleUnitName, "
         + "s.baseRateUom AS baseRateUom, s.basePackUom AS basePackUom, s.baseSecondaryUom AS baseSecondaryUom, s.active AS active "
         + "FROM UOMSchedule s LEFT JOIN UOM u ON u.id = s.scheduleUnitId LEFT JOIN Item i ON s.itemId = i.id "
         + "WHERE s.itemId = :itemId AND s.organizationId = :organizationId AND COALESCE(s.active, false) = true "
         + "ORDER BY s.equivalent ASC")
    List<UomScheduleByItemRow> readByItemID(@Param("itemId") Integer itemId,
                                            @Param("organizationId") Integer organizationId);

    /** Sp_UOMSchedule_GetAllMethod @Activity='ReadByItemGroupId' */
    @Query("SELECT gu.id AS id, gu.itemGroupId AS itemGroupId, gu.uomId AS uomId, gu.equivalent AS equivalent, "
         + "g.itemGroupName AS itemGroupName, u.qtyEquivalent AS qtyEquivalent "
         + "FROM ItemGroup_UOMSchedule gu JOIN ItemGroup g ON gu.itemGroupId = g.id JOIN UOM u ON gu.uomId = u.id "
         + "WHERE gu.itemGroupId = :itemGroupId")
    List<UomScheduleByItemGroupRow> readByItemGroupId(@Param("itemGroupId") Integer itemGroupId);

    /** Sp_UOMSchedule_GetAllMethod @Activity='GetItemBaseUomId' (native: view V_UomScheduleAndUom) */
    @Query(value = "SELECT u.Id, u.Equivalent FROM item i "
         + "INNER JOIN V_UomScheduleAndUom u on i.BaseUnitId = ScheduleUnitId and u.ItemId = i.Id "
         + "WHERE u.ItemId = :itemId", nativeQuery = true)
    List<ItemBaseUomRow> getItemBaseUomId(@Param("itemId") Integer itemId);

    /** Sp_UOMSchedule_GetAllMethod @Activity='GetUomForAssigntoItem' (group units not yet scheduled for the item; @CompanyId filter commented out) */
    @Query("SELECT g.id AS id, g.itemGroupId AS itemGroupId, g.uomId AS uomId, g.equivalent AS equivalent, "
         + "g.organizationId AS organizationId, g.companyId AS companyId, u.qtyEquivalent AS qtyEquivalent "
         + "FROM ItemGroup_UOMSchedule g LEFT JOIN UOM u ON g.uomId = u.id "
         + "WHERE g.itemGroupId = :itemGroupId "
         + "AND NOT EXISTS (SELECT 1 FROM ItemGroup_UOMSchedule iu, UOMSchedule us, Item i "
         + "WHERE iu.uomId = us.scheduleUnitId AND us.itemId = i.id "
         + "AND i.organizationId = :organizationId AND iu.itemGroupId = :itemGroupId AND us.itemId = :itemId AND iu.uomId = g.uomId)")
    List<UomForAssignToItemRow> getUomForAssigntoItem(@Param("itemGroupId") Integer itemGroupId,
                                                      @Param("organizationId") Integer organizationId,
                                                      @Param("itemId") Integer itemId);

    /** usp_getAllUomsByCompanyId (single branch). @ItemId default null = all items; @Active (INT in the proc) default null = any -> pass Boolean. */
    @Query("SELECT s.id AS id, u.uomCode AS uomCode, s.equivalent AS equivalent, s.baseRateUom AS baseRateUom, s.itemId AS itemId, "
         + "s.basePackUom AS basePackUom, s.baseSecondaryUom AS baseSecondaryUom, s.qtyEquivalent AS qtyEquivalent, "
         + "u.uomDescription AS uomDescription, s.active AS active, i.itemName AS itemName, i.itemCodeNew AS itemCode "
         + "FROM UOMSchedule s JOIN UOM u ON u.id = s.scheduleUnitId JOIN Item i ON i.id = s.itemId "
         + "WHERE s.organizationId = :organizationId AND s.companyId = :companyId "
         + "AND (:itemId IS NULL OR s.itemId = :itemId) "
         + "AND (:active IS NULL OR COALESCE(s.active, false) = :active) "
         + "ORDER BY s.baseRateUom DESC")
    List<UomsByCompanyRow> getAllUomsByCompanyId(@Param("organizationId") Integer organizationId,
                                                 @Param("companyId") Integer companyId,
                                                 @Param("itemId") Integer itemId,
                                                 @Param("active") Boolean active);

    // ---- Sp_UOMSchedule_Insert / Sp_UOMSchedule_Update building blocks -------------------------------------------

    /** Sp_UOMSchedule_Insert: next app-assigned Id = maxId()+1 */
    @Query("select coalesce(max(s.id),0) from UOMSchedule s")
    Integer maxId();

    /** Sp_UOMSchedule_Insert (IF NOT EXISTS) / Sp_UOMSchedule_Update (IF EXISTS): rows for (Org, Company, ItemId, ScheduleUnitId) */
    @Query("SELECT COUNT(s) FROM UOMSchedule s WHERE s.organizationId = :organizationId AND s.companyId = :companyId "
         + "AND s.itemId = :itemId AND s.scheduleUnitId = :scheduleUnitId")
    Long countSchedule(@Param("organizationId") Integer organizationId,
                       @Param("companyId") Integer companyId,
                       @Param("itemId") Integer itemId,
                       @Param("scheduleUnitId") Integer scheduleUnitId);

    /** Sp_UOMSchedule_Insert (ELSE path): SELECT @Id = Id ... AND ISNULL(Active,0) = 0 */
    @Query("SELECT s.id FROM UOMSchedule s WHERE s.organizationId = :organizationId AND s.companyId = :companyId "
         + "AND s.itemId = :itemId AND s.scheduleUnitId = :scheduleUnitId AND COALESCE(s.active, false) = false")
    List<Integer> findInactiveId(@Param("organizationId") Integer organizationId,
                                 @Param("companyId") Integer companyId,
                                 @Param("itemId") Integer itemId,
                                 @Param("scheduleUnitId") Integer scheduleUnitId);

    /** Sp_UOMSchedule_Update: SELECT @PreScheduleUnitId = ScheduleUnitId FROM UOMSchedule WHERE Id = @Id */
    @Query("SELECT s.scheduleUnitId FROM UOMSchedule s WHERE s.id = :id")
    Integer findScheduleUnitIdById(@Param("id") Integer id);

    /** Sp_UOMSchedule_Insert / _Update: UPDATE UOMSchedule SET BaseRateUom = 0 WHERE CompanyId = @CompanyId AND ItemId = @ItemId AND ISNULL(BaseRateUom,0) = 1 */
    @Modifying
    @Transactional
    @Query("UPDATE UOMSchedule s SET s.baseRateUom = false WHERE s.companyId = :companyId AND s.itemId = :itemId AND COALESCE(s.baseRateUom, false) = true")
    int resetBaseRateUom(@Param("companyId") Integer companyId, @Param("itemId") Integer itemId);

    /** Sp_UOMSchedule_Insert / _Update: UPDATE UOMSchedule SET BasePackUom = 0 WHERE CompanyId = @CompanyId AND ItemId = @ItemId AND ISNULL(BasePackUom,0) = 1 */
    @Modifying
    @Transactional
    @Query("UPDATE UOMSchedule s SET s.basePackUom = false WHERE s.companyId = :companyId AND s.itemId = :itemId AND COALESCE(s.basePackUom, false) = true")
    int resetBasePackUom(@Param("companyId") Integer companyId, @Param("itemId") Integer itemId);

    /** Sp_UOMSchedule_Insert / _Update: UPDATE UOMSchedule SET BaseSecondaryUom = 0 WHERE CompanyId = @CompanyId AND ItemId = @ItemId AND ISNULL(BaseSecondaryUom,0) = 1 */
    @Modifying
    @Transactional
    @Query("UPDATE UOMSchedule s SET s.baseSecondaryUom = false WHERE s.companyId = :companyId AND s.itemId = :itemId AND COALESCE(s.baseSecondaryUom, false) = true")
    int resetBaseSecondaryUom(@Param("companyId") Integer companyId, @Param("itemId") Integer itemId);

    /** Sp_UOMSchedule_Insert (ELSE path): UPDATE UOMSchedule SET Active = 1 WHERE ID = @Id */
    @Modifying
    @Transactional
    @Query("UPDATE UOMSchedule s SET s.active = true WHERE s.id = :id")
    int activateById(@Param("id") Integer id);

    /** Sp_UOMSchedule_Update: the final UPDATE with every SET column (EntryDate/EntryUser not updated) */
    @Modifying
    @Transactional
    @Query("UPDATE UOMSchedule s SET s.itemId = :itemId, s.scheduleUnitId = :scheduleUnitId, s.equivalent = :equivalent, "
         + "s.modifyDate = :modifyDate, s.modifyUser = :modifyUser, s.organizationId = :organizationId, s.companyId = :companyId, "
         + "s.qtyEquivalent = :qtyEquivalent, s.baseRateUom = :baseRateUom, s.basePackUom = :basePackUom, "
         + "s.baseSecondaryUom = :baseSecondaryUom, s.active = :active WHERE s.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("itemId") Integer itemId,
                   @Param("scheduleUnitId") Integer scheduleUnitId,
                   @Param("equivalent") Double equivalent,
                   @Param("modifyDate") LocalDateTime modifyDate,
                   @Param("modifyUser") Integer modifyUser,
                   @Param("organizationId") Integer organizationId,
                   @Param("companyId") Integer companyId,
                   @Param("qtyEquivalent") Double qtyEquivalent,
                   @Param("baseRateUom") Boolean baseRateUom,
                   @Param("basePackUom") Boolean basePackUom,
                   @Param("baseSecondaryUom") Boolean baseSecondaryUom,
                   @Param("active") Boolean active);

    /** Sp_UOMSchedule_Insert (single branch, whole procedure). // KEEP-EXEC: conditional insert / re-activate with flag resets and RAISERROR. All params default NULL. Proc returns @Id (not returned here). */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_UOMSchedule_Insert @CompanyId = :companyId, @EntryDate = :entryDate, @EntryUser = :entryUser, "
         + "@Equivalent = :equivalent, @Id = :id, @ItemId = :itemId, @ModifyDate = :modifyDate, @ModifyUser = :modifyUser, "
         + "@OrganizationId = :organizationId, @ScheduleUnitId = :scheduleUnitId, @QtyEquivalent = :qtyEquivalent, "
         + "@BaseRateUom = :baseRateUom, @BasePackUom = :basePackUom, @BaseSecondaryUom = :baseSecondaryUom, @Active = :active",
         nativeQuery = true)
    void spUomScheduleInsert(@Param("companyId") Integer companyId,
                             @Param("entryDate") LocalDateTime entryDate,
                             @Param("entryUser") Integer entryUser,
                             @Param("equivalent") Double equivalent,
                             @Param("id") Integer id,
                             @Param("itemId") Integer itemId,
                             @Param("modifyDate") LocalDateTime modifyDate,
                             @Param("modifyUser") Integer modifyUser,
                             @Param("organizationId") Integer organizationId,
                             @Param("scheduleUnitId") Integer scheduleUnitId,
                             @Param("qtyEquivalent") Double qtyEquivalent,
                             @Param("baseRateUom") Boolean baseRateUom,
                             @Param("basePackUom") Boolean basePackUom,
                             @Param("baseSecondaryUom") Boolean baseSecondaryUom,
                             @Param("active") Boolean active);

    /** Sp_UOMSchedule_Update (single branch, whole procedure). // KEEP-EXEC: validation RAISERRORs + flag resets + UPDATE. All params default NULL. */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_UOMSchedule_Update @CompanyId = :companyId, @EntryDate = :entryDate, @EntryUser = :entryUser, "
         + "@Equivalent = :equivalent, @Id = :id, @ItemId = :itemId, @ModifyDate = :modifyDate, @ModifyUser = :modifyUser, "
         + "@OrganizationId = :organizationId, @ScheduleUnitId = :scheduleUnitId, @QtyEquivalent = :qtyEquivalent, "
         + "@BaseRateUom = :baseRateUom, @BasePackUom = :basePackUom, @BaseSecondaryUom = :baseSecondaryUom, @Active = :active",
         nativeQuery = true)
    void spUomScheduleUpdate(@Param("companyId") Integer companyId,
                             @Param("entryDate") LocalDateTime entryDate,
                             @Param("entryUser") Integer entryUser,
                             @Param("equivalent") Double equivalent,
                             @Param("id") Integer id,
                             @Param("itemId") Integer itemId,
                             @Param("modifyDate") LocalDateTime modifyDate,
                             @Param("modifyUser") Integer modifyUser,
                             @Param("organizationId") Integer organizationId,
                             @Param("scheduleUnitId") Integer scheduleUnitId,
                             @Param("qtyEquivalent") Double qtyEquivalent,
                             @Param("baseRateUom") Boolean baseRateUom,
                             @Param("basePackUom") Boolean basePackUom,
                             @Param("baseSecondaryUom") Boolean baseSecondaryUom,
                             @Param("active") Boolean active);
}
