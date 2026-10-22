package com.mst.repositories;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ItemMinMaxRateSchedule;
import com.mst.projections.LastMinMaxRateRow;
import com.mst.projections.MinMaxDropDownRow;
import com.mst.projections.MinMaxRateFormHistoryRow;
import com.mst.projections.MinMaxRateItemsRow;

/**
 * USP_ItemMinMaxRateSchedule_Insert + usp_MinMaxDeletionById + usp_GetLastItemMinAndMaxRateByItemIdAndUomId +
 * usp_GetItemsFromMinAndMaxRateSchedule + USP_GetItemsFromMinAndMaxRateSchedule_FormHistory + Usp_DropDownFillFromItemMinAndMaxRateSchedule.
 *
 * USP_ItemMinMaxRateSchedule_Insert pre-insert logic: RAISERROR "&lt;ItemName&gt; schedule is already added for this date" when a row exists for
 * Org/Company/Item on the same calendar day ({@link #countSameDay}); UPDATE Item SET MinRate, MaxRate, EffectedDate ({@link #updateItemMinMaxRate});
 * Id = ISNULL(MAX(Id),0)+1 ({@link #maxId()}); INSERT Id, ItemId, EffectedDate, MinRate, MaxRate, EntryUser, EntryDate, OrganizationId, CompanyId,
 * BranchesId, FinancialYearId, RateUomId (ModifyUser/ModifyDate not inserted); SELECT @Id. Full behaviour: {@link #uspItemMinMaxRateScheduleInsert}.
 */
@Repository
public interface ItemMinMaxRateScheduleRepository extends JpaRepository<ItemMinMaxRateSchedule, Integer> {

    /** USP_ItemMinMaxRateSchedule_Insert: ISNULL(MAX(CONVERT(INT,Id)),0) — caller adds 1. */
    @Query("select coalesce(max(e.id),0) from ItemMinMaxRateSchedule e")
    Integer maxId();

    /**
     * USP_ItemMinMaxRateSchedule_Insert — full procedural behaviour. Defaults: @Id, @ModifyUser, @ModifyDate = NULL; the rest mandatory.
     * Returns the new Id (SELECT @Id).
     * // KEEP-EXEC: RAISERROR duplicate-day guard + side UPDATE on Item before the insert.
     */
    @Query(value = "EXEC dbo.USP_ItemMinMaxRateSchedule_Insert @Id = :id, @ItemId = :itemId, @EffectedDate = :effectedDate, @MinRate = :minRate, "
                 + "@MaxRate = :maxRate, @EntryUser = :entryUser, @EntryDate = :entryDate, @ModifyUser = :modifyUser, @ModifyDate = :modifyDate, "
                 + "@OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId, @FinancialYearId = :financialYearId, "
                 + "@RateUomId = :rateUomId", nativeQuery = true)
    Integer uspItemMinMaxRateScheduleInsert(@Param("id") Integer id,
                                            @Param("itemId") Integer itemId,
                                            @Param("effectedDate") LocalDateTime effectedDate,
                                            @Param("minRate") Double minRate,
                                            @Param("maxRate") Double maxRate,
                                            @Param("entryUser") Integer entryUser,
                                            @Param("entryDate") LocalDateTime entryDate,
                                            @Param("modifyUser") Integer modifyUser,
                                            @Param("modifyDate") LocalDateTime modifyDate,
                                            @Param("organizationId") Integer organizationId,
                                            @Param("companyId") Integer companyId,
                                            @Param("branchesId") Integer branchesId,
                                            @Param("financialYearId") Integer financialYearId,
                                            @Param("rateUomId") Integer rateUomId);

    /** USP_ItemMinMaxRateSchedule_Insert guard: rows for Org/Company/Item on the same calendar day. */
    @Query(value = "SELECT COUNT(1) FROM ItemMinMaxRateSchedule R WHERE R.OrganizationId = :organizationId AND R.CompanyId = :companyId "
                 + "AND R.ItemId = :itemId AND CAST(R.EffectedDate AS DATE) = CAST(:effectedDate AS DATE)", nativeQuery = true)
    Long countSameDay(@Param("organizationId") Integer organizationId,
                      @Param("companyId") Integer companyId,
                      @Param("itemId") Integer itemId,
                      @Param("effectedDate") LocalDateTime effectedDate);

    /** USP_ItemMinMaxRateSchedule_Insert side effect: UPDATE Item SET MinRate, MaxRate, EffectedDate WHERE Id = @ItemId. */
    @Modifying
    @Transactional
    @Query("UPDATE Item i SET i.minRate = :minRate, i.maxRate = :maxRate, i.effectedDate = :effectedDate WHERE i.id = :itemId")
    int updateItemMinMaxRate(@Param("itemId") Integer itemId,
                             @Param("minRate") Double minRate,
                             @Param("maxRate") Double maxRate,
                             @Param("effectedDate") LocalDateTime effectedDate);

    /** usp_MinMaxDeletionById */
    @Modifying
    @Transactional
    @Query("DELETE FROM ItemMinMaxRateSchedule e WHERE e.id = :id")
    int deleteScheduleById(@Param("id") Integer id);

    /** usp_GetLastItemMinAndMaxRateByItemIdAndUomId — TOP 1 ORDER BY Id DESC (take first); @RateUomId default NULL. */
    @Query(value = "SELECT TOP 1 d.MinRate,d.MaxRate,d.EffectedDate as PreviousDate FROM ItemMinMaxRateSchedule d "
                 + "Where d.OrganizationId = :organizationId and d.CompanyId = :companyId and d.ItemId = :itemId "
                 + "and (:rateUomId is null or d.RateUomId = :rateUomId) ORDER BY D.Id DESC", nativeQuery = true)
    List<LastMinMaxRateRow> getLastItemMinAndMaxRateByItemIdAndUomId(@Param("organizationId") Integer organizationId,
                                                                     @Param("companyId") Integer companyId,
                                                                     @Param("itemId") Integer itemId,
                                                                     @Param("rateUomId") Integer rateUomId);

    /**
     * USP_GetItemsFromMinAndMaxRateSchedule_FormHistory — #TmpResult rewritten as a CTE (the proc's RecordNo is computed but neither
     * filtered nor returned). Optional filters default NULL: @InventoryParentCategoriesId, @ItemCategoryId, @ItemTypeId, @ItemId, @EffectedDate (date).
     */
    @Query(value = "WITH TmpResult AS ( SELECT ROW_NUMBER() OVER (PARTITION BY R.ItemId ORDER BY R.EffectedDate DESC) AS RecordNo, "
                 + "R.Id, R.ItemId, i.ItemCodeNew, i.ItemName, i.ItemCategoryId, c.CategoryDescription, i.ItemTypeId, c.InventoryParentCategoriesId, "
                 + "R.EffectedDate, R.MinRate, R.MaxRate, R.RateUomId "
                 + "FROM ItemMinMaxRateSchedule R INNER JOIN Item i ON i.Id = R.ItemId AND R.CompanyId = :companyId "
                 + "Inner join ItemCategory c on c.Id=i.ItemCategoryId "
                 + "Where R.OrganizationId = :organizationId and R.CompanyId = :companyId "
                 + "and (:itemId is null or R.ItemId = :itemId) "
                 + "and (:inventoryParentCategoriesId is null or c.InventoryParentCategoriesId=:inventoryParentCategoriesId) "
                 + "and (:itemCategoryId is null or i.ItemCategoryId = :itemCategoryId) "
                 + "and (:itemTypeId is null or i.ItemTypeId = :itemTypeId) "
                 + "and (:effectedDate IS NULL OR CAST(R.EffectedDate AS DATE) <= CAST(:effectedDate AS DATE)) ) "
                 + "SELECT R.EffectedDate, R.Id, R.InventoryParentCategoriesId ParentCategoryId, cp.InvParentCateDescription ParentCategory, "
                 + "R.ItemCategoryId, R.CategoryDescription ItemCategory, R.ItemTypeId, it.TypeDescription ItemType, R.ItemId, R.ItemName, "
                 + "R.ItemCodeNew ItemCode, R.RateUomId, RUom.UOMCode AS RateUom, R.MinRate, R.MaxRate "
                 + "FROM TmpResult R INNER JOIN V_UomScheduleAndUom RUom ON R.RateUomId = RUom.Id AND R.ItemId = RUom.ItemId "
                 + "Inner join InventoryParentCategories cp on cp.Id=R.InventoryParentCategoriesId "
                 + "Inner join ItemType it on it.Id=R.ItemTypeId", nativeQuery = true)
    List<MinMaxRateFormHistoryRow> getItemsFromMinAndMaxRateScheduleFormHistory(@Param("organizationId") Integer organizationId,
                                                                               @Param("companyId") Integer companyId,
                                                                               @Param("inventoryParentCategoriesId") Integer inventoryParentCategoriesId,
                                                                               @Param("itemCategoryId") Integer itemCategoryId,
                                                                               @Param("itemTypeId") Integer itemTypeId,
                                                                               @Param("itemId") Integer itemId,
                                                                               @Param("effectedDate") LocalDate effectedDate);

    /**
     * usp_GetItemsFromMinAndMaxRateSchedule — every item of the company with its base-UOM defaults (GETDATE(), 0, 0) overwritten by the latest
     * schedule row when one exists. The proc's "#TmpResult + #TmpScheduleData + UPDATE" pipeline is rewritten as two CTEs and a LEFT JOIN with
     * CASE WHEN T.ItemId IS NOT NULL (so a NULL schedule value still overwrites, exactly like the UPDATE did). MinRate/MaxRate are CAST to int
     * because the proc materialised them as int columns ("0 as MinRate") and the UPDATE truncated the float schedule values into them.
     * Optional filters default NULL: @InventoryParentCategoriesId, @ItemCategoryId, @ItemTypeId, @EffectedDate (date, exact-day match).
     */
    @Query(value = "WITH TmpResult AS ( SELECT i.Id, i.ItemName, i.ItemCodeNew, RUom.Id as RateUomId, GETDATE() as EffectedDate, 0 as MinRate, 0 as MaxRate, "
                 + "RUom.UOMCode AS RateUom, cp.Id AS ParentcategoryId, cp.InvParentCateDescription AS Parentcategory, C.CategoryDescription, it.TypeDescription "
                 + "FROM Item i Inner join ItemCategory c on c.Id=i.ItemCategoryId Inner join ItemType it on it.Id=i.ItemTypeId "
                 + "Inner join InventoryParentCategories cp on cp.Id=c.InventoryParentCategoriesId "
                 + "inner join V_UomScheduleAndUom RUom on i.BaseUnitId = RUom.ScheduleUnitId and RUom.ItemId = i.Id "
                 + "Where i.OrganizationId = :organizationId and i.CompanyId = :companyId "
                 + "and (:inventoryParentCategoriesId is null or c.InventoryParentCategoriesId=:inventoryParentCategoriesId) "
                 + "and (:itemCategoryId is null or i.ItemCategoryId = :itemCategoryId) "
                 + "and (:itemTypeId is null or i.ItemTypeId = :itemTypeId) ), "
                 + "TmpScheduleData AS ( SELECT A.ItemId, A.EffectedDate, A.MinRate, A.MaxRate, A.CompanyId, A.RateUomId, A.RateUom FROM ( "
                 + "SELECT ROW_NUMBER() OVER (PARTITION BY R.ItemId ORDER BY R.EffectedDate DESC) AS RecordNo, R.ItemId, R.EffectedDate, R.MinRate, R.MaxRate, "
                 + "R.CompanyId, R.RateUomId, RUom.UOMCode AS RateUom "
                 + "FROM TmpResult A INNER JOIN ItemMinMaxRateSchedule R ON A.Id = R.ItemId AND R.CompanyId = :companyId "
                 + "INNER JOIN V_UomScheduleAndUom RUom ON R.RateUomId = RUom.Id AND R.ItemId = RUom.ItemId "
                 + "WHERE (:effectedDate IS NULL OR CAST(R.EffectedDate AS DATE) = CAST(:effectedDate AS DATE)) ) A WHERE A.RecordNo = 1 ) "
                 + "SELECT R.Id, R.ItemName, R.ItemCodeNew, "
                 + "CASE WHEN T.ItemId IS NOT NULL THEN T.RateUomId ELSE R.RateUomId END AS RateUomId, "
                 + "CASE WHEN T.ItemId IS NOT NULL THEN T.RateUom ELSE R.RateUom END AS RateUom, "
                 + "CASE WHEN T.ItemId IS NOT NULL THEN T.EffectedDate ELSE R.EffectedDate END AS EffectedDate, "
                 + "CASE WHEN T.ItemId IS NOT NULL THEN CAST(T.MinRate AS int) ELSE R.MinRate END AS MinRate, "
                 + "CASE WHEN T.ItemId IS NOT NULL THEN CAST(T.MaxRate AS int) ELSE R.MaxRate END AS MaxRate, "
                 + "R.ParentcategoryId, R.Parentcategory, R.CategoryDescription, R.TypeDescription "
                 + "FROM TmpResult R LEFT JOIN TmpScheduleData T ON R.Id = T.ItemId AND T.CompanyId = :companyId", nativeQuery = true)
    List<MinMaxRateItemsRow> getItemsFromMinAndMaxRateSchedule(@Param("organizationId") Integer organizationId,
                                                               @Param("companyId") Integer companyId,
                                                               @Param("inventoryParentCategoriesId") Integer inventoryParentCategoriesId,
                                                               @Param("itemCategoryId") Integer itemCategoryId,
                                                               @Param("itemTypeId") Integer itemTypeId,
                                                               @Param("effectedDate") LocalDate effectedDate);

    /**
     * Usp_DropDownFillFromItemMinAndMaxRateSchedule — @BranchesIds (comma-separated, default NULL = all branches), @Activity default NULL
     * (all groups) or one of ItemClassGroup / ItemParentCategory / ItemCategory / ItemType / ItemName. Temp-table pipeline rewritten as
     * CTE + UNION ALL (the proc only INSERTs into #tmpResult).
     */
    @Query(value = "WITH TmpFinalResult AS ( SELECT distinct h.ItemId FROM ItemMinMaxRateSchedule h "
                 + "WHERE h.OrganizationId = :organizationId and h.CompanyId = :companyId "
                 + "and ( :branchesIds is null OR EXISTS (select 1 from dbo.fnSplitString (:branchesIds,',') AS B Where b.data = h.BranchesId) ) ), "
                 + "TmpResult AS ( "
                 + "select Distinct 'ItemClassGroup' as Activity, ic.ItemClassGroupId as Id, c.ClassGroupName as name "
                 + "from TmpFinalResult Pid inner join Item i on Pid.ItemId = i.Id inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
                 + "Inner join ItemClassGroup c on c.Id=ic.ItemClassGroupId where (:activity is null or 'ItemClassGroup' = :activity) "
                 + "UNION ALL "
                 + "select Distinct 'ItemParentCategory' as Activity, ic.InventoryParentCategoriesId as Id, pc.InvParentCateDescription as name "
                 + "from TmpFinalResult Pid inner join Item i on Pid.ItemId = i.Id inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
                 + "Inner join InventoryParentCategories pc on pc.Id=ic.InventoryParentCategoriesId where (:activity is null or 'ItemParentCategory' = :activity) "
                 + "UNION ALL "
                 + "select Distinct 'ItemCategory' as Activity, i.ItemCategoryId as Id, ic.CategoryDescription as name "
                 + "from TmpFinalResult PId inner join Item i on Pid.ItemId = i.Id inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
                 + "where (:activity is null or 'ItemCategory' = :activity) "
                 + "UNION ALL "
                 + "select Distinct 'ItemType' as Activity, i.ItemTypeId as Id, it.TypeDescription as name "
                 + "from TmpFinalResult Pid inner join Item i on Pid.ItemId = i.Id inner join ItemType it on i.ItemTypeId = it.Id "
                 + "where (:activity is null or 'ItemType' = :activity) "
                 + "UNION ALL "
                 + "select Distinct 'ItemName' as Activity, Pid.ItemId as Id, i.ItemName as name "
                 + "from TmpFinalResult Pid inner join Item i on Pid.ItemId = i.Id where (:activity is null or 'ItemName' = :activity) ) "
                 + "SELECT R.Id, R.name, R.Activity FROM TmpResult R", nativeQuery = true)
    List<MinMaxDropDownRow> dropDownFillFromItemMinAndMaxRateSchedule(@Param("organizationId") Integer organizationId,
                                                                      @Param("companyId") Integer companyId,
                                                                      @Param("branchesIds") String branchesIds,
                                                                      @Param("activity") String activity);
}
