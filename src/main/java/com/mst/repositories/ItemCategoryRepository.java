package com.mst.repositories;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ItemCategory;
import com.mst.projections.ItemCategoryAssetsFormHistoryRow;
import com.mst.projections.ItemCategoryFormHistoryRow;
import com.mst.projections.ItemCategoryGenerateCodeRow;
import com.mst.projections.ItemCategoryReadAllRow;
import com.mst.projections.ItemCategoryReadByIdRow;
import com.mst.projections.ItemCategorySerialRangeRow;
import com.mst.projections.ItemClassGroupRow;
import com.mst.projections.ParentCategoryDashboardRow;

/**
 * dbo.ItemCategory - Sp_ItemCategory_GetAllMethod / Sp_ItemCategory_Insert / Sp_ItemCategory_Update.
 *
 * <p>Pre-insert logic of Sp_ItemCategory_Insert (when using {@code save()} instead of {@link #spItemCategoryInsert}):
 * Id = max(Id)+1 over the whole table ({@link #maxId()}); AssetCategoryId = max(Asset.Category.CategoryId)+1
 * ({@link #nextAssetCategoryId()}); RAISERROR when SerialTo &lt; SerialFrom; RAISERROR when the [SerialFrom,SerialTo]
 * range is already covered by another category of the same Organization/Company ({@link #countSerialRangeOverlap});
 * when configuration 410 (item coding) is enabled ({@link #itemCodingEnabled}) CategoryCode is overwritten with
 * {@link #generateCode} (RIGHT('00'+max(TRY_CAST(CategoryCode))+1, 2)); else RAISERROR when len(CategoryCode) &gt; 4;
 * ModifyDate/ModifyUser are NOT written on insert; afterwards a mirror row is inserted into Asset.Category and the
 * new Id is returned (SELECT @Id).
 *
 * <p>Pre-update logic of Sp_ItemCategory_Update: RAISERROR when SerialTo &lt; SerialFrom; if the serial range changed
 * ({@link #findSerialRangeById}) RAISERROR when SerialFrom falls inside another category's range
 * ({@link #countSerialConflictExcludingId}); RAISERROR when len(CategoryCode) &gt; 4; EntryDate/EntryUser untouched;
 * afterwards Asset.Category is inserted or updated (by AssetCategoryId / ItemCategoryId).
 */
@Repository
public interface ItemCategoryRepository extends JpaRepository<ItemCategory, Integer> {

    // ===================== Sp_ItemCategory_GetAllMethod =====================

    /** Sp_ItemCategory_GetAllMethod @Activity='GenerateCode' (also used by Sp_ItemCategory_Insert when item coding is enabled). */
    @Query(value = "SELECT RIGHT('00' + CAST(ISNULL(MAX(TRY_CAST(CategoryCode AS INT)), 0) + 1 AS VARCHAR(10)), 2) AS CategoryCode "
         + "FROM dbo.ItemCategory "
         + "WHERE OrganizationId = :organizationId "
         + "AND CompanyId = :companyId "
         + "AND InventoryParentCategoriesId = :inventoryParentCategoriesId", nativeQuery = true)
    ItemCategoryGenerateCodeRow generateCode(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("inventoryParentCategoriesId") Integer inventoryParentCategoriesId);

    /** Sp_ItemCategory_GetAllMethod @Activity='ReadById' (CategoryAttributeList is always ''). */
    @Query("SELECT c.id AS id, c.categoryCode AS categoryCode, c.categoryDescription AS categoryDescription, c.serialFrom AS serialFrom, "
         + "c.serialTo AS serialTo, c.categoryStatus AS categoryStatus, c.revenueAccountId AS revenueAccountId, c.cgsAccountId AS cgsAccountId, "
         + "c.inventoryAccountId AS inventoryAccountId, c.accumulatedDepreciationAcId AS accumulatedDepreciationAcId, "
         + "c.depreciationExpenseAcId AS depreciationExpenseAcId, c.capitalWipAcId AS capitalWipAcId, c.entryDate AS entryDate, "
         + "c.entryUser AS entryUser, c.modifyDate AS modifyDate, c.modifyUser AS modifyUser, c.organizationId AS organizationId, "
         + "c.companyId AS companyId, c.inventoryParentCategoriesId AS inventoryParentCategoriesId, "
         + "c.itemProductionStageId AS itemProductionStageId, c.itemVarietyNatureId AS itemVarietyNatureId, "
         + "c.itemClassGroupId AS itemClassGroupId, c.expenseMaintenanceAccountId AS expenseMaintenanceAccountId, "
         + "'' AS categoryAttributeList, c.usefullLifeInMonths AS usefullLifeInMonths, c.depriciationrate AS depriciationrate, "
         + "c.assetCategoryId AS assetCategoryId, c.depreciationMethodScheduleId AS depreciationMethodScheduleId "
         + "FROM ItemCategory c WHERE c.id = :id")
    Optional<ItemCategoryReadByIdRow> readById(@Param("id") Integer id);

    /** Sp_ItemCategory_GetAllMethod @Activity='ReadByOrganizationCompanyId' (@InventoryParentCategoriesId and @Ids optional, @Ids is a CSV). */
    @Query(value = "SELECT ItemCategory.Id, ItemCategory.CategoryCode, ItemCategory.CategoryDescription, ItemCategory.SerialFrom, ItemCategory.SerialTo, "
         + "CASE WHEN CategoryStatus = 0 THEN 'InActive' ELSE 'Active' END CategoryStatus, "
         + "ItemCategory.RevenueAccountId, ItemCategory.InventoryAccountId, ItemCategory.InventoryParentCategoriesId, ic.InvParentCateDescription, "
         + "ItemCategory.CGSAccountId, CGSAccount.AccountTitle AS CGSAccountTitle, RevenueAccount.AccountTitle AS RevenueAccountTitle, "
         + "InventoryAccount.AccountTitle AS InventoryAccountTitle, '' CategoryAttributeList, "
         + "ItemCategory.ItemProductionStageId, IPS.productionStageName, ItemCategory.ItemVarietyNatureId, IVN.VarietyNatureName, "
         + "ItemCategory.ExpenseMaintenanceAccountId, ItemCategory.usefullLifeInMonths, ItemCategory.depriciationrate, "
         + "ItemCategory.AssetCategoryId, ItemCategory.depreciationMethodScheduleId, DMS.depreciatonMethodName as depreciationMethodName "
         + "FROM ItemCategory "
         + "INNER JOIN ChartofAccount AS CGSAccount ON ItemCategory.CGSAccountId = CGSAccount.Id "
         + "INNER JOIN ChartofAccount AS RevenueAccount ON ItemCategory.RevenueAccountId = RevenueAccount.Id "
         + "INNER JOIN ChartofAccount AS InventoryAccount ON ItemCategory.InventoryAccountId = InventoryAccount.Id "
         + "inner join InventoryParentCategories ic on ItemCategory.InventoryParentCategoriesId = ic.Id "
         + "LEFT JOIN ItemProductionStage IPS ON ItemCategory.ItemProductionStageId = IPS.Id "
         + "LEFT JOIN ItemVarietyNature IVN ON ItemCategory.ItemVarietyNatureId = IVN.Id "
         + "LEFT JOIN asset.DepreciationMethodSchedule DMS ON ItemCategory.depreciationMethodScheduleId = DMS.depreciationMethodScheduleId "
         + "where ItemCategory.OrganizationId = :organizationId "
         + "and ItemCategory.CompanyId = :companyId "
         + "and Itemcategory.CategoryStatus = 1 "
         + "and (:inventoryParentCategoriesId is null or ItemCategory.InventoryParentCategoriesId = :inventoryParentCategoriesId) "
         + "and (:ids IS NULL OR ItemCategory.InventoryParentCategoriesId in (select data from dbo.fnSplitString (:ids,',')))", nativeQuery = true)
    List<ItemCategoryReadAllRow> readByOrganizationCompanyId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("inventoryParentCategoriesId") Integer inventoryParentCategoriesId, @Param("ids") String ids);

    /** Sp_ItemCategory_GetAllMethod @Activity='FormHistory' (@InventoryParentCategoriesId and @ParentCategoryIds (CSV) optional). */
    @Query(value = "SELECT ItemCategory.Id, ItemCategory.CategoryCode, ItemCategory.CategoryDescription, ItemCategory.SerialFrom, "
         + "ItemCategory.SerialTo, ItemCategory.InventoryParentCategoriesId, "
         + "CASE WHEN ItemCategory.CategoryStatus = 0 THEN 'InActive' ELSE 'Active' END CategoryStatus, "
         + "ic.InvParentCateDescription, ItemCategory.RevenueAccountId, ItemCategory.InventoryAccountId, "
         + "ItemCategory.CGSAccountId, CGSAccount.AccountTitle AS CGSAccountTitle, "
         + "RevenueAccount.AccountTitle AS RevenueAccountTitle, InventoryAccount.AccountTitle AS InventoryAccountTitle, cg.ClassGroupName, "
         + "ItemCategory.accumulatedDepreciationAcId, accumulatedDepreciationAccount.AccountTitle AccumulatedDepreciationAccount, "
         + "ItemCategory.DepreciationExpenseAcId, DepreciationExpenseAccount.AccountTitle DepreciationExpenseAccount, "
         + "ItemCategory.CapitalWipAcId, CapitalWipAccount.AccountTitle CapitalWipAccount, "
         + "ItemCategory.ItemProductionStageId, IPS.productionStageName, ItemCategory.ItemVarietyNatureId, IVN.VarietyNatureName, "
         + "ItemCategory.ExpenseMaintenanceAccountId, ExpenseMA.AccountTitle As ExpenseMaintenanceAccount, "
         + "ItemCategory.usefullLifeInMonths, ItemCategory.depriciationrate, ItemCategory.AssetCategoryId, "
         + "ItemCategory.depreciationMethodScheduleId, DMS.depreciatonMethodName as depreciationMethodName "
         + "FROM ItemCategory "
         + "LEFT join ItemClassGroup cg on ItemCategory.ItemClassGroupId = cg.Id "
         + "inner join InventoryParentCategories ic on ItemCategory.InventoryParentCategoriesId = ic.Id "
         + "LEft JOIN ChartofAccount AS CGSAccount ON ItemCategory.CGSAccountId = CGSAccount.Id "
         + "LEft JOIN ChartofAccount AS RevenueAccount ON ItemCategory.RevenueAccountId = RevenueAccount.Id "
         + "LEft JOIN ChartofAccount AS InventoryAccount ON ItemCategory.InventoryAccountId = InventoryAccount.Id "
         + "LEFT JOIN ChartofAccount AS accumulatedDepreciationAccount ON ItemCategory.accumulatedDepreciationAcId = accumulatedDepreciationAccount.Id "
         + "LEFT JOIN ChartofAccount AS DepreciationExpenseAccount ON ItemCategory.DepreciationExpenseAcId = DepreciationExpenseAccount.Id "
         + "LEFT JOIN ChartofAccount AS CapitalWipAccount ON ItemCategory.CapitalWipAcId = CapitalWipAccount.Id "
         + "LEFT JOIN ChartofAccount AS ExpenseMA ON ItemCategory.ExpenseMaintenanceAccountId = ExpenseMA.Id "
         + "LEFT JOIN ItemProductionStage IPS ON ItemCategory.ItemProductionStageId = IPS.Id "
         + "LEFT JOIN ItemVarietyNature IVN ON ItemCategory.ItemVarietyNatureId = IVN.Id "
         + "LEFT JOIN asset.DepreciationMethodSchedule DMS ON ItemCategory.depreciationMethodScheduleId = DMS.depreciationMethodScheduleId "
         + "where ItemCategory.OrganizationId = :organizationId "
         + "and ItemCategory.CompanyId = :companyId "
         + "and (:inventoryParentCategoriesId is null or ItemCategory.InventoryParentCategoriesId = :inventoryParentCategoriesId) "
         + "and (:parentCategoryIds is null "
         + "OR Exists (Select 1 From fnSplitString(:parentCategoryIds,',') d Where ItemCategory.InventoryParentCategoriesId in (d.Data))) "
         + "ORDER BY ItemCategory.InventoryParentCategoriesId, ItemCategory.CategoryCode", nativeQuery = true)
    List<ItemCategoryFormHistoryRow> formHistory(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("inventoryParentCategoriesId") Integer inventoryParentCategoriesId, @Param("parentCategoryIds") String parentCategoryIds);

    /** Sp_ItemCategory_GetAllMethod @Activity='FormHistoryForAssetsCategory' (fixed to InventoryParentCategoriesId = 5, active only). */
    @Query(value = "SELECT ItemCategory.Id, ItemCategory.CategoryCode, ItemCategory.CategoryDescription, ItemCategory.SerialFrom, ItemCategory.SerialTo, "
         + "ItemCategory.RevenueAccountId, ItemCategory.InventoryAccountId, ItemCategory.InventoryParentCategoriesId, "
         + "ItemCategory.accumulatedDepreciationAcId, accumulatedDepreciationAccount.AccountTitle AccumulatedDepreciationAccount, "
         + "ItemCategory.DepreciationExpenseAcId, DepreciationExpenseAccount.AccountTitle DepreciationExpenseAccount, "
         + "ItemCategory.CapitalWipAcId, CapitalWipAccount.AccountTitle CapitalWipAccount, "
         + "ItemCategory.ExpenseMaintenanceAccountId, ExpenseMA.AccountTitle As ExpenseMaintenanceAccount, "
         + "ic.InvParentCateDescription as AssetParentCateDescription, "
         + "CASE WHEN ItemCategory.CategoryStatus = 0 THEN 'InActive' ELSE 'Active' END CategoryStatus, "
         + "ItemCategory.ItemProductionStageId, IPS.productionStageName, ItemCategory.ItemVarietyNatureId, IVN.VarietyNatureName "
         + "FROM ItemCategory "
         + "INNER JOIN ChartofAccount AS accumulatedDepreciationAccount ON ItemCategory.accumulatedDepreciationAcId = accumulatedDepreciationAccount.Id "
         + "INNER JOIN ChartofAccount AS DepreciationExpenseAccount ON ItemCategory.DepreciationExpenseAcId = DepreciationExpenseAccount.Id "
         + "INNER JOIN ChartofAccount AS CapitalWipAccount ON ItemCategory.CapitalWipAcId = CapitalWipAccount.Id "
         + "LEFT JOIN ChartofAccount AS ExpenseMA ON ItemCategory.ExpenseMaintenanceAccountId = ExpenseMA.Id "
         + "INNER JOIN InventoryParentCategories ic on ItemCategory.InventoryParentCategoriesId = ic.Id "
         + "LEFT JOIN ItemProductionStage IPS ON ItemCategory.ItemProductionStageId = IPS.Id "
         + "LEFT JOIN ItemVarietyNature IVN ON ItemCategory.ItemVarietyNatureId = IVN.Id "
         + "where ItemCategory.OrganizationId = :organizationId "
         + "and ItemCategory.CompanyId = :companyId "
         + "and ItemCategory.InventoryParentCategoriesId = 5 "
         + "and Itemcategory.CategoryStatus = 1", nativeQuery = true)
    List<ItemCategoryAssetsFormHistoryRow> formHistoryForAssetsCategory(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_ItemCategory_GetAllMethod @Activity='GetItemClassGroup' (no parameters). */
    @Query(value = "SELECT Id, ClassGroupName FROM ItemClassGroup", nativeQuery = true)
    List<ItemClassGroupRow> getItemClassGroup();

    /** Sp_ItemCategory_GetAllMethod @Activity='GetItemParentCategoryForAnalyticalDashBoard' (@ActionId 1 = purchase doc types, 2 = sale doc types). */
    @Query(value = "SELECT distinct ic.InventoryParentCategoriesId, ip.InvParentCateDescription "
         + "FROM InventoryStockEvalautionDetail st "
         + "inner join item i on st.ItemId = i.Id "
         + "inner join ItemCategory ic on i.ItemCategoryId = ic.id "
         + "inner join InventoryParentCategories ip on ic.InventoryParentCategoriesId = ip.Id "
         + "Where st.OrganizationId = :organizationId "
         + "and st.CompanyId = :companyId "
         + "and ((:actionId = 1 AND st.RefDocumentTypeId in (56,57,138,172)) OR (:actionId = 2 AND st.RefDocumentTypeId in (95,99,98,139,171)))", nativeQuery = true)
    List<ParentCategoryDashboardRow> getItemParentCategoryForAnalyticalDashBoard(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("actionId") Integer actionId);

    // ===================== Sp_ItemCategory_Insert helpers =====================

    /** Sp_ItemCategory_Insert: Id = ISNULL(MAX(Id),0)+1 (app-assigned, whole table). */
    @Query("select coalesce(max(e.id),0) from ItemCategory e")
    Integer maxId();

    /** Sp_ItemCategory_Insert: @AssetCategoryId = ISNULL(MAX(CONVERT(int,CategoryId)),0)+1 from Asset.Category (no entity). */
    @Query(value = "SELECT ISNULL(MAX(CONVERT(int,CategoryId)),0) + 1 FROM Asset.Category", nativeQuery = true)
    Integer nextAssetCategoryId();

    /** Sp_ItemCategory_Insert: &gt;0 means 'This serial already add in ItemCategory Please Check'. */
    @Query("SELECT COUNT(c) FROM ItemCategory c WHERE c.organizationId = :organizationId AND c.companyId = :companyId "
         + "AND :serialFrom BETWEEN c.serialFrom AND c.serialTo AND :serialTo BETWEEN c.serialFrom AND c.serialTo")
    Long countSerialRangeOverlap(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("serialFrom") Integer serialFrom, @Param("serialTo") Integer serialTo);

    /** Sp_ItemCategory_Insert: ISNULL(dbo.GetConfigurationByConfigId(org, comp, 410), 0) - item coding switch ('1' = enabled). */
    @Query(value = "SELECT ISNULL(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 410), 0)", nativeQuery = true)
    String itemCodingEnabled(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_ItemCategory_Insert (single branch) - the whole procedure, behaviour preserved.
     *  // KEEP-EXEC: validations (RAISERROR), config-driven CategoryCode generation, second INSERT into Asset.Category; the proc ends with SELECT @Id which a @Modifying call discards - use maxId() beforehand or save() for the simple case. @Id/@AssetCategoryId are overwritten inside the proc.
     *  Defaults in the proc: every parameter defaults to NULL unless noted; callers pass values explicitly.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_ItemCategory_Insert @CategoryCode = :categoryCode, @CategoryDescription = :categoryDescription, "
         + "@SerialFrom = :serialFrom, @SerialTo = :serialTo, @CategoryStatus = :categoryStatus, "
         + "@RevenueAccountId = :revenueAccountId, @InventoryAccountId = :inventoryAccountId, "
         + "@CGSAccountId = :cgsAccountId, @EntryDate = :entryDate, @EntryUser = :entryUser, @ModifyDate = :modifyDate, "
         + "@ModifyUser = :modifyUser, @OrganizationId = :organizationId, @CompanyId = :companyId, @Id = :id, "
         + "@InventoryParentCategoriesId = :inventoryParentCategoriesId, "
         + "@accumulatedDepreciationAcId = :accumulatedDepreciationAcId, "
         + "@DepreciationExpenseAcId = :depreciationExpenseAcId, @CapitalWipAcId = :capitalWipAcId, "
         + "@AssetGlAcId = :assetGlAcId, @ItemClassGroupId = :itemClassGroupId, "
         + "@ItemProductionStageId = :itemProductionStageId, @ItemVarietyNatureId = :itemVarietyNatureId, "
         + "@ExpenseMaintenanceAccountId = :expenseMaintenanceAccountId, @usefullLifeInMonths = :usefullLifeInMonths, "
         + "@depriciationrate = :depriciationrate, @AssetCategoryId = :assetCategoryId, "
         + "@depreciationMethodScheduleId = :depreciationMethodScheduleId", nativeQuery = true)
    void spItemCategoryInsert(
            @Param("categoryCode") String categoryCode,
            @Param("categoryDescription") String categoryDescription,
            @Param("serialFrom") Integer serialFrom,
            @Param("serialTo") Integer serialTo,
            @Param("categoryStatus") Boolean categoryStatus,
            @Param("revenueAccountId") Integer revenueAccountId,
            @Param("inventoryAccountId") Integer inventoryAccountId,
            @Param("cgsAccountId") Integer cgsAccountId,
            @Param("entryDate") LocalDateTime entryDate,
            @Param("entryUser") Integer entryUser,
            @Param("modifyDate") LocalDateTime modifyDate,
            @Param("modifyUser") Integer modifyUser,
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("id") Integer id,
            @Param("inventoryParentCategoriesId") Integer inventoryParentCategoriesId,
            @Param("accumulatedDepreciationAcId") Integer accumulatedDepreciationAcId,
            @Param("depreciationExpenseAcId") Integer depreciationExpenseAcId,
            @Param("capitalWipAcId") Integer capitalWipAcId,
            @Param("assetGlAcId") Integer assetGlAcId,
            @Param("itemClassGroupId") Integer itemClassGroupId,
            @Param("itemProductionStageId") Integer itemProductionStageId,
            @Param("itemVarietyNatureId") Integer itemVarietyNatureId,
            @Param("expenseMaintenanceAccountId") Integer expenseMaintenanceAccountId,
            @Param("usefullLifeInMonths") Integer usefullLifeInMonths,
            @Param("depriciationrate") BigDecimal depriciationrate,
            @Param("assetCategoryId") Integer assetCategoryId,
            @Param("depreciationMethodScheduleId") Integer depreciationMethodScheduleId);

    // ===================== Sp_ItemCategory_Update =====================

    /** Sp_ItemCategory_Update: current SerialFrom/SerialTo of the record (pre-check whether the range changed). */
    @Query("SELECT c.serialFrom AS serialFrom, c.serialTo AS serialTo FROM ItemCategory c "
         + "WHERE c.organizationId = :organizationId AND c.companyId = :companyId AND c.id = :id")
    Optional<ItemCategorySerialRangeRow> findSerialRangeById(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("id") Integer id);

    /** Sp_ItemCategory_Update: &gt;0 means 'This serial already add in ItemCategory Please Check' (SerialFrom inside another category's range). */
    @Query("SELECT COUNT(c) FROM ItemCategory c WHERE c.organizationId = :organizationId AND c.companyId = :companyId "
         + "AND :serialFrom >= c.serialFrom AND :serialFrom <= c.serialTo AND c.id <> :id")
    Long countSerialConflictExcludingId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("serialFrom") Integer serialFrom, @Param("id") Integer id);

    /** Sp_ItemCategory_Update: the standalone UPDATE dbo.ItemCategory ... WHERE Id=@Id (every SET column; EntryDate/EntryUser untouched). */
    @Modifying
    @Transactional
    @Query("UPDATE ItemCategory c SET c.categoryCode = :categoryCode, c.categoryDescription = :categoryDescription, "
         + "c.serialFrom = :serialFrom, c.serialTo = :serialTo, c.categoryStatus = :categoryStatus, "
         + "c.revenueAccountId = :revenueAccountId, c.cgsAccountId = :cgsAccountId, c.inventoryAccountId = :inventoryAccountId, "
         + "c.accumulatedDepreciationAcId = :accumulatedDepreciationAcId, c.depreciationExpenseAcId = :depreciationExpenseAcId, "
         + "c.capitalWipAcId = :capitalWipAcId, c.modifyDate = :modifyDate, c.modifyUser = :modifyUser, "
         + "c.organizationId = :organizationId, c.companyId = :companyId, "
         + "c.inventoryParentCategoriesId = :inventoryParentCategoriesId, c.assetGlAcId = :assetGlAcId, "
         + "c.itemClassGroupId = :itemClassGroupId, c.itemProductionStageId = :itemProductionStageId, "
         + "c.itemVarietyNatureId = :itemVarietyNatureId, c.expenseMaintenanceAccountId = :expenseMaintenanceAccountId, "
         + "c.usefullLifeInMonths = :usefullLifeInMonths, c.depriciationrate = :depriciationrate, "
         + "c.assetCategoryId = :assetCategoryId, c.depreciationMethodScheduleId = :depreciationMethodScheduleId "
         + "WHERE c.id = :id")
    int updateById(@Param("categoryCode") String categoryCode,
            @Param("categoryDescription") String categoryDescription,
            @Param("serialFrom") Integer serialFrom,
            @Param("serialTo") Integer serialTo,
            @Param("categoryStatus") Boolean categoryStatus,
            @Param("revenueAccountId") Integer revenueAccountId,
            @Param("cgsAccountId") Integer cgsAccountId,
            @Param("inventoryAccountId") Integer inventoryAccountId,
            @Param("accumulatedDepreciationAcId") Integer accumulatedDepreciationAcId,
            @Param("depreciationExpenseAcId") Integer depreciationExpenseAcId,
            @Param("capitalWipAcId") Integer capitalWipAcId,
            @Param("modifyDate") LocalDateTime modifyDate,
            @Param("modifyUser") Integer modifyUser,
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("inventoryParentCategoriesId") Integer inventoryParentCategoriesId,
            @Param("assetGlAcId") Integer assetGlAcId,
            @Param("itemClassGroupId") Integer itemClassGroupId,
            @Param("itemProductionStageId") Integer itemProductionStageId,
            @Param("itemVarietyNatureId") Integer itemVarietyNatureId,
            @Param("expenseMaintenanceAccountId") Integer expenseMaintenanceAccountId,
            @Param("usefullLifeInMonths") Integer usefullLifeInMonths,
            @Param("depriciationrate") BigDecimal depriciationrate,
            @Param("assetCategoryId") Integer assetCategoryId,
            @Param("depreciationMethodScheduleId") Integer depreciationMethodScheduleId,
            @Param("id") Integer id);

    /** Sp_ItemCategory_Update: current Asset.Category.CategoryId mirror of this ItemCategory (0 when none). */
    @Query(value = "SELECT ISNULL(CONVERT(int,CategoryId),0) FROM Asset.Category WHERE itemCategoryId = :id", nativeQuery = true)
    List<Integer> findAssetCategoryIdByItemCategoryId(@Param("id") Integer id);

    /** Sp_ItemCategory_Update (single branch) - the whole procedure, behaviour preserved.
     *  // KEEP-EXEC: validations (RAISERROR), then UPDATE ItemCategory plus INSERT-or-UPDATE of the Asset.Category mirror row. @CategoryStatus has no default (mandatory).
     *  Defaults in the proc: every parameter defaults to NULL unless noted; callers pass values explicitly.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_ItemCategory_Update @Id = :id, @CategoryCode = :categoryCode, "
         + "@CategoryDescription = :categoryDescription, @SerialFrom = :serialFrom, @SerialTo = :serialTo, "
         + "@CategoryStatus = :categoryStatus, @RevenueAccountId = :revenueAccountId, @CGSAccountId = :cgsAccountId, "
         + "@InventoryAccountId = :inventoryAccountId, @accumulatedDepreciationAcId = :accumulatedDepreciationAcId, "
         + "@DepreciationExpenseAcId = :depreciationExpenseAcId, @CapitalWipAcId = :capitalWipAcId, "
         + "@EntryDate = :entryDate, @EntryUser = :entryUser, @ModifyDate = :modifyDate, @ModifyUser = :modifyUser, "
         + "@OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@InventoryParentCategoriesId = :inventoryParentCategoriesId, @AssetGlAcId = :assetGlAcId, "
         + "@ItemClassGroupId = :itemClassGroupId, @ItemProductionStageId = :itemProductionStageId, "
         + "@ItemVarietyNatureId = :itemVarietyNatureId, @ExpenseMaintenanceAccountId = :expenseMaintenanceAccountId, "
         + "@usefullLifeInMonths = :usefullLifeInMonths, @depriciationrate = :depriciationrate, "
         + "@AssetCategoryId = :assetCategoryId, @depreciationMethodScheduleId = :depreciationMethodScheduleId", nativeQuery = true)
    void spItemCategoryUpdate(
            @Param("id") Integer id,
            @Param("categoryCode") String categoryCode,
            @Param("categoryDescription") String categoryDescription,
            @Param("serialFrom") Integer serialFrom,
            @Param("serialTo") Integer serialTo,
            @Param("categoryStatus") Boolean categoryStatus,
            @Param("revenueAccountId") Integer revenueAccountId,
            @Param("cgsAccountId") Integer cgsAccountId,
            @Param("inventoryAccountId") Integer inventoryAccountId,
            @Param("accumulatedDepreciationAcId") Integer accumulatedDepreciationAcId,
            @Param("depreciationExpenseAcId") Integer depreciationExpenseAcId,
            @Param("capitalWipAcId") Integer capitalWipAcId,
            @Param("entryDate") LocalDateTime entryDate,
            @Param("entryUser") Integer entryUser,
            @Param("modifyDate") LocalDateTime modifyDate,
            @Param("modifyUser") Integer modifyUser,
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("inventoryParentCategoriesId") Integer inventoryParentCategoriesId,
            @Param("assetGlAcId") Integer assetGlAcId,
            @Param("itemClassGroupId") Integer itemClassGroupId,
            @Param("itemProductionStageId") Integer itemProductionStageId,
            @Param("itemVarietyNatureId") Integer itemVarietyNatureId,
            @Param("expenseMaintenanceAccountId") Integer expenseMaintenanceAccountId,
            @Param("usefullLifeInMonths") Integer usefullLifeInMonths,
            @Param("depriciationrate") BigDecimal depriciationrate,
            @Param("assetCategoryId") Integer assetCategoryId,
            @Param("depreciationMethodScheduleId") Integer depreciationMethodScheduleId);
}
