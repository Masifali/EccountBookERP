package com.mst.repositories;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.Item;
import com.mst.projections.AvailableQtyRow;
import com.mst.projections.AvgRateRow;
import com.mst.projections.ItemAllocateWarehouseRow;
import com.mst.projections.ItemAllocationRow;
import com.mst.projections.ItemByTypeRow;
import com.mst.projections.ItemCategoryGlAccountFixedAssetsRow;
import com.mst.projections.ItemCategoryGlAccountRow;
import com.mst.projections.ItemCategoryParentIdRow;
import com.mst.projections.ItemCodeNameRow;
import com.mst.projections.ItemComboParentRow;
import com.mst.projections.ItemComboRow;
import com.mst.projections.ItemCommissionHistoryRow;
import com.mst.projections.ItemCommissionRow;
import com.mst.projections.ItemEngrModalRow;
import com.mst.projections.ItemEngrRow;
import com.mst.projections.ItemExportRow;
import com.mst.projections.ItemFixedAssetsFormHistoryRow;
import com.mst.projections.ItemFormHistoryRow;
import com.mst.projections.ItemGenerateCodeRow;
import com.mst.projections.ItemGlAccountRow;
import com.mst.projections.ItemGlIdsRow;
import com.mst.projections.ItemGlReferedRow;
import com.mst.projections.ItemIdNameRow;
import com.mst.projections.ItemIdRow;
import com.mst.projections.ItemImageRow;
import com.mst.projections.ItemPmRow;
import com.mst.projections.ItemPricingScheduleRow;
import com.mst.projections.ItemReadAllItemsPmRow;
import com.mst.projections.ItemReadAllItemsRow;
import com.mst.projections.ItemReadAllRow;
import com.mst.projections.ItemUomRow;
import com.mst.projections.LeadTimeRow;
import com.mst.projections.LookupDocumentTypeRow;
import com.mst.projections.LookupItemCategoryRow;
import com.mst.projections.LookupItemRow;
import com.mst.projections.LookupItemTypeRow;
import com.mst.projections.LookupJobLotRow;
import com.mst.projections.LookupSupplierCustomerRow;
import com.mst.projections.LookupWarehouseRow;
import com.mst.projections.PackUomRow;
import com.mst.projections.ParentCategoryCodeRow;
import com.mst.projections.StoreWarehouseRow;
import com.mst.projections.UomScheduleEquivalentRow;

/**
 * dbo.Item - Sp_Item_GetAllMethod (65 distinct @Activity branches) / Sp_Item_Insert / Sp_Item_Update.
 *
 * <p>Pre-insert logic of Sp_Item_Insert (when using {@code save()} instead of {@link #spItemInsert}):
 * Id = ISNULL(MAX(Id),0)+1 ({@link #maxId()}); BarcodeNo, when empty, = '1' + RIGHT('00'+OrganizationId,2) + RIGHT('00'+CompanyId,2)
 * + LEFT(NEWID(),11); ItemCode = {@link #nextItemCodeInCategory} or, when that is 1, ItemCategory.SerialFrom
 * ({@link #findSerialFromByCategory}); ItemCodeNew = ItemCode; RAISERROR on duplicate ItemName ({@link #countByItemName})
 * or duplicate ItemCodeNew within the category ({@link #countByItemCodeNewInCategory}); when PurchaseGLAC, SaleGLAC and
 * COGSGLAC are all &gt; 0 each must be a 'Detail' ChartofAccount ({@link #findDetailAccountIds}); UserLogId defaults to
 * UserAccount.LastUserLogId of EntryUser ({@link #lastUserLogIdByUserId}); ModifyDate/ModifyUser are NOT written; after the
 * INSERT the proc calls item.USP_ItemAttribute_InsertAndUpdateByCategory (unless ERP feature 13 exists) and
 * dbo.USP_ItemAllocationToStoreRack_InsertAndUpdate when RackId &gt; 0, then returns the new Id.
 *
 * <p>Pre-update logic of Sp_Item_Update: BarcodeNo, when empty, = '1' + RIGHT(OrganizationId+100000000,2) +
 * RIGHT(CompanyId+100000000,2) + RIGHT(Id+1E20,11); when it differs from the stored one ({@link #findBarcodeNoById}) it must
 * be unique ({@link #countByBarcodeNo}); Detail-account check as on insert; duplicate name / code checks excluding the row
 * ({@link #countByItemNameExcludingId}, {@link #countByItemCodeNewExcludingId}); UserLogId defaults from ModifyUser; then
 * {@link #updateById}, {@link #deleteItemImagesByItemId}, the two EXECs as on insert, and SELECT 0.
 */
@Repository
public interface ItemRepository extends JpaRepository<Item, Integer> {

    // ===================== Sp_Item_GetAllMethod =====================

    /** Sp_Item_GetAllMethod @Activity='ReadByItemCode' */
    @Query("SELECT i FROM Item i WHERE i.itemCodeNew = :code")
    List<Item> readByItemCode(@Param("code") String code);

    /** Sp_Item_GetAllMethod @Activity='ReadById' */
    @Query("SELECT i FROM Item i WHERE i.id = :id")
    Optional<Item> readById(@Param("id") Integer id);

    /** Sp_Item_GetAllMethod @Activity='ReadItemAllocationByItemId' */
    @Query(value = "select al.*, b.BranchName, '' ItemAllocationlist "
         + "from ItemAllocation al "
         + "inner join Branches b on al.BranchId = b.Id "
         + "where al.ItemId = :id", nativeQuery = true)
    List<ItemAllocationRow> readItemAllocationByItemId(@Param("id") Integer id);

    /** Sp_Item_GetAllMethod @Activity='ItemImagesByItemId' */
    @Query("SELECT i.id AS id, i.itemId AS itemId, i.imagePath AS imagePath, i.effetedDate AS effetedDate, i.isActive AS isActive, "
         + "i.fileName AS fileName, i.isProfileImage AS isProfileImage, i.sortNo AS sortNo "
         + "FROM ItemImage i WHERE i.itemId = :id ORDER BY i.id")
    List<ItemImageRow> itemImagesByItemId(@Param("id") Integer id);

    /** Sp_Item_GetAllMethod @Activity='ReadByOrganizationCompanyId' */
    @Query(value = "SELECT Item.Id, Item.ItemCode, Item.ItemName, Item.BaseUnitId, Item.ItemCategoryId, Item.ItemTypeId, Item.ItemClassId, Item.PurchaseGLAC, Item.SaleGLAC, Item.COGSGLAC, Item.HSCode, Item.PackQty, Item.ItemStatus, "
         + "Item.ManufactureId, Item.ManufacturePartNo, Item.BuyerSupplierId, Item.BuyerPartNo, Item.ProductNo, Item.MinStockLevel, Item.OptimalStockLevel, Item.MaxStockLevel, Item.ReorderLevel, Item.ItemOriginId, Item.CostPrice, "
         + "Item.UOMScheduleIdCostRate, Item.PurchasePrice, Item.UOMScheduleIdPurRate, Item.RetailPrice, Item.UOMScheduleIdRetailRate, Item.WholeSalePrice, Item.UOMScheduleIdWhsRate, Item.AmountCalcType, "
         + "Item.BarcodeImage, Item.BarcodeNo, Item.ProductType, Item.BaseCropYear, Item.ApplyGST, Item.ApplyVAT, Item.ApplyExciese, Item.OtherTax, "
         + "Case When IsNull(Pic1,'')='' Then cfg.AttachmentsUrl+'RiceImage.jpg' Else cfg.AttachmentsUrl+Pic1 End Pic1, "
         + "Item.Pic2, "
         + "Item.ModifyUser, Item.PostDate, Item.PostUser, Item.PostState, Item.OrganizationId, Item.CompanyId, Item.BranchesId, Item.ProjectsId, StockAc.AccountTitle AS GlStockAccountTitle, SaleAc.AccountTitle AS GlSaleAccountTitle, "
         + "CGSAc.AccountTitle AS GlCgsAccountTitle, ItemCategory.CategoryDescription, ItemType.TypeDescription, UOM.UOMCode "
         + ",ProductType, MasterItemId, ItemSpecification, UomLookUpId, ItemAliasName, PurchaseTypeLookUpId, IsExpirable, ShelfLife, IsDiscountable, "
         + "SaleTaxPurPercent, AtRetailPriceTax, AtPurcahsePriceTax, SaleTaxSalesPercent, IsTaxable, Item.DepreciationExpenseAcId, Item.accumulatedDepreciationAcId, Item.CapitalWipAcId "
         + ",entryUser.FirstName+entryUser.LastName EntryUserName, Item.EntryUser EntryUserId, Item.EntryDate "
         + ",modifyUser.FirstName+modifyUser.LastName ModifyUserName, Item.ModifyUser ModifyUserId, Item.ModifyDate "
         + ",CASE WHEN ISNULL(entryUserProfile.ProfileImageFileName,'')='' THEN cfg.AttachmentsUrl+'DummyProfilePicture.jpg' ELSE cfg.AttachmentsUrl+entryUserProfile.ProfileImageFileName END EntryUserProfileImage "
         + ",CASE WHEN ISNULL(modifyuserProfile.ProfileImageFileName,'')='' THEN cfg.AttachmentsUrl+'DummyProfilePicture.jpg' ELSE cfg.AttachmentsUrl+modifyuserProfile.ProfileImageFileName END MoidifyUserProfileImage, Item.CropYearId, 0 CommOnSale, 0 CommOnPurchase, "
         + "Item.ItemQcGradeId, qc.QcGradeDesc ItemQcGrade, Item.MaxMeaurementUnitId, "
         + "Item.WeightKgs, Item.MaxMeaurement, item.ModelName, item.ItemWithVarient, Item.ItemNameOtherLingo "
         + ",cp.Id ParentcategoryId, cp.InvParentCateDescription Parentcategory, Item.IsCompany, Item.IsThirdParty, Item.ReOrderQty, Item.IsImport, "
         + "Item.LeadTimeDay, Item.AllowMultiUom, Item.PackSizeId, "
         + "Item.PackSize, Item.MotherItemId, MIT.TypeDescription AS MotherItem, Item.itemModalId, modl.ModalName, "
         + "item.WeightSemiFinish, item.WeightFinishGoods, item.WeightSemiFinishSand, item.ServicesMasterItemId, "
         + "item.RackId, rack.rackName "
         + "FROM Item "
         + "CROSS JOIN (SELECT dbo.GetConfigurationByConfigId(:organizationId,:companyId,91) AS AttachmentsUrl) cfg "
         + "INNER JOIN vItemAllocation IOA on item.Id = IOA.Id "
         + "inner join ChartofAccount AS StockAc ON Item.PurchaseGLAC = StockAc.Id "
         + "INNER JOIN ChartofAccount AS SaleAc ON Item.SaleGLAC = SaleAc.Id "
         + "INNER JOIN ChartofAccount AS CGSAc ON Item.COGSGLAC = CGSAc.Id "
         + "INNER JOIN ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "Inner join InventoryParentCategories cp on cp.Id=ItemCategory.InventoryParentCategoriesId "
         + "INNER JOIN ItemType ON Item.ItemTypeId = ItemType.Id "
         + "INNER JOIN UOM ON Item.BaseUnitId = UOM.Id "
         + "INNER JOIN UserAccount entryUser on entryUser.ID=Item.EntryUser "
         + "LEFT JOIN UserProfile entryUserProfile on entryUserProfile.UserId=entryUser.ID "
         + "LEFT JOIN UserAccount modifyUser on modifyUser.ID=Item.ModifyUser "
         + "LEFT JOIN UserProfile modifyuserProfile on modifyuserProfile.UserId=ModifyUser.ID "
         + "Left join ItemQcGrade qc on qc.Id=Item.ItemQcGradeId "
         + "LEFT JOIN ItemType MIT ON ITEM.MotherItemId = MIT.Id "
         + "Left Join item.itemModel modl on modl.itemModelId=item.itemModalId "
         + "LEFT JOIN invWarehouseRack rack on Item.RackId = rack.Id "
         + "where IOA.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "and IOA.IsActive = 1 "
         + "and ItemCategory.InventoryParentCategoriesId <> 5 "
         + "and (:isTaxable is null or ISNULL(Item.IsTaxable,0)=:isTaxable) "
         + "and (:parentIds is null or ItemCategory.InventoryParentCategoriesId in (select data from dbo.fnSplitString(:parentIds,',')))", nativeQuery = true)
    List<ItemReadAllRow> readByOrganizationCompanyId(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("isTaxable") Integer isTaxable,
            @Param("parentIds") String parentIds);

    /** Sp_Item_GetAllMethod @Activity='ReadAllItems' */
    @Query(value = "SELECT Item.Id, Item.ItemName, Item.ItemCode, Item.ItemCodeNew, ItemCategory.InventoryParentCategoriesId, "
         + "item.ItemCategoryId, Item.ItemTypeId, Item.ItemQcGradeId, qc.QcGradeDesc ItemQcGrade, Item.MaxMeaurementUnitId, "
         + "Item.WeightKgs, Item.MaxMeaurement, item.PurchaseGLAC, Item.SaleGLAC, Item.COGSGLAC, "
         + "ItemCategory.CategoryDescription, ItemType.TypeDescription "
         + "FROM Item "
         + "INNER JOIN vItemAllocation IOA on item.Id = IOA.Id "
         + "INNER JOIN ChartofAccount AS StockAc ON Item.PurchaseGLAC = StockAc.Id "
         + "INNER JOIN ChartofAccount AS SaleAc ON Item.SaleGLAC = SaleAc.Id "
         + "INNER JOIN ChartofAccount AS CGSAc ON Item.COGSGLAC = CGSAc.Id "
         + "INNER JOIN ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "LEFT JOIN ItemType ON Item.ItemTypeId = ItemType.Id "
         + "LEFT JOIN InvLookUp l on ItemType.Type = l.Id "
         + "LEFT JOIN UOM ON Item.BaseUnitId = UOM.Id "
         + "Left join ItemQcGrade qc on qc.Id=Item.ItemQcGradeId "
         + "where Item.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "and ItemCategory.InventoryParentCategoriesId <> 5 "
         + "and IOA.IsActive = 1 "
         + "and Item.itemstatus = 1 "
         + "AND (ItemType.Type not in (14,17))", nativeQuery = true)
    List<ItemReadAllItemsRow> readAllItems(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='ReadAllItemsIncludedPM' */
    @Query(value = "SELECT Item.Id, Item.ItemName, Item.ItemCode, Item.ItemCodeNew, ItemCategory.InventoryParentCategoriesId, "
         + "item.ItemCategoryId, Item.ItemTypeId, Item.ItemQcGradeId, qc.QcGradeDesc ItemQcGrade, Item.MaxMeaurementUnitId, "
         + "Item.WeightKgs, Item.MaxMeaurement, item.PurchaseGLAC, Item.SaleGLAC, Item.COGSGLAC "
         + "FROM Item "
         + "INNER JOIN vItemAllocation IOA on item.Id = IOA.Id "
         + "INNER JOIN ChartofAccount AS StockAc ON Item.PurchaseGLAC = StockAc.Id "
         + "INNER JOIN ChartofAccount AS SaleAc ON Item.SaleGLAC = SaleAc.Id "
         + "INNER JOIN ChartofAccount AS CGSAc ON Item.COGSGLAC = CGSAc.Id "
         + "INNER JOIN ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "LEFT JOIN ItemType ON Item.ItemTypeId = ItemType.Id "
         + "LEFT JOIN InvLookUp l on ItemType.Type = l.Id "
         + "LEFT JOIN UOM ON Item.BaseUnitId = UOM.Id "
         + "Left join ItemQcGrade qc on qc.Id=Item.ItemQcGradeId "
         + "where Item.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "and ItemCategory.InventoryParentCategoriesId <> 5 "
         + "and IOA.IsActive = 1 "
         + "and Item.itemstatus = 1", nativeQuery = true)
    List<ItemReadAllItemsPmRow> readAllItemsIncludedPM(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='GetExportItemsByOrganizationCompanyId' */
    @Query(value = "SELECT Item.Id, Item.ItemCode, Item.ItemName, Item.BaseUnitId, Item.ItemCategoryId, Item.ItemTypeId, "
         + "Item.ItemClassId, Item.PurchaseGLAC, Item.SaleGLAC, Item.COGSGLAC, Item.HSCode, Item.PackQty, "
         + "Item.ItemStatus, Item.ManufactureId, Item.ManufacturePartNo, Item.BuyerSupplierId, Item.BuyerPartNo, "
         + "Item.ProductNo, Item.MinStockLevel, Item.OptimalStockLevel, Item.MaxStockLevel, Item.ReorderLevel, "
         + "Item.ItemOriginId, Item.CostPrice, Item.UOMScheduleIdCostRate, Item.PurchasePrice, "
         + "Item.UOMScheduleIdPurRate, Item.RetailPrice, Item.UOMScheduleIdRetailRate, Item.WholeSalePrice, "
         + "Item.UOMScheduleIdWhsRate, Item.AmountCalcType, Item.BarcodeImage, Item.BarcodeNo, Item.ProductType, "
         + "Item.BaseCropYear, Item.ApplyGST, Item.ApplyVAT, Item.ApplyExciese, Item.OtherTax, "
         + "Item.Pic1, Item.Pic2, Item.EntryDate, Item.EntryUser, Item.ModifyDate, "
         + "Item.ModifyUser, Item.PostDate, Item.PostUser, Item.PostState, Item.OrganizationId, "
         + "Item.CompanyId, Item.BranchesId, Item.ProjectsId, "
         + "StockAc.AccountTitle AS GlStockAccountTitle, SaleAc.AccountTitle AS GlSaleAccountTitle, "
         + "CGSAc.AccountTitle AS GlCgsAccountTitle, Item.ItemQcGradeId, qc.QcGradeDesc ItemQcGrade, "
         + "Item.MaxMeaurementUnitId, Item.WeightKgs, Item.MaxMeaurement, ItemCategory.CategoryDescription, "
         + "ItemType.TypeDescription, UOM.UOMCode, Item.CropYearId, 0 CommOnSale, 0 CommOnPurchase, Item.ItemNameOtherLingo, Item.IsCompany, Item.IsThirdParty "
         + ",Item.MotherItemId, MIT.TypeDescription AS MotherItem, Item.itemModalId, modl.ModalName, "
         + "item.WeightSemiFinish, item.WeightFinishGoods, item.WeightSemiFinishSand "
         + "FROM Item "
         + "INNER JOIN vItemAllocation IOA on item.Id = IOA.Id "
         + "INNER JOIN ChartofAccount AS StockAc ON Item.PurchaseGLAC = StockAc.Id "
         + "INNER JOIN ChartofAccount AS SaleAc ON Item.SaleGLAC = SaleAc.Id "
         + "INNER JOIN ChartofAccount AS CGSAc ON Item.COGSGLAC = CGSAc.Id "
         + "INNER JOIN ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "INNER JOIN ItemType ON Item.ItemTypeId = ItemType.Id "
         + "INNER JOIN UOM ON Item.BaseUnitId = UOM.Id "
         + "Left join ItemQcGrade qc on qc.Id=Item.ItemQcGradeId "
         + "LEFT JOIN ItemType MIT ON ITEM.MotherItemId = MIT.Id "
         + "Left Join item.itemModel modl on modl.itemModelId=item.itemModalId "
         + "where Item.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "AND item.ItemClassId = 8 "
         + "and ItemCategory.InventoryParentCategoriesId <> 5", nativeQuery = true)
    List<ItemExportRow> getExportItemsByOrganizationCompanyId(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='GetGLAccountByItemId' */
    @Query(value = "SELECT Item.Id, Item.ItemName, Coa.AccountCode, Coa.AccountTitle, Item.PurchaseGLAC, Item.SaleGLAC "
         + "FROM ChartofAccount AS Coa "
         + "INNER JOIN Item ON Coa.Id = Item.PurchaseGLAC "
         + "INNER JOIN vItemAllocation IOA ON ITEM.Id = IOA.Id "
         + "where Item.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "and IOA.Id = :id "
         + "GROUP BY Coa.AccountCode, Coa.AccountTitle, Item.ItemName, Item.Id, Item.PurchaseGLAC, Item.SaleGLAC", nativeQuery = true)
    List<ItemGlAccountRow> getGLAccountByItemId(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("id") Integer id);

    /** Sp_Item_GetAllMethod @Activity='GenerateItemCodeByCategoryId' // KEEP-EXEC: procedural (config 410 switch, variable assignments, TRY_CAST/RIGHT padding) */
    @Query(value = "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId = :organizationId, @CompanyId = :companyId, @ItemCategoryId = :itemCategoryId, @ItemTypeId = :itemTypeId, @Activity = 'GenerateItemCodeByCategoryId'", nativeQuery = true)
    ItemGenerateCodeRow generateItemCodeByCategoryId(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("itemTypeId") Integer itemTypeId);

    /** Sp_Item_GetAllMethod @Activity='FormHistory' */
    @Query(value = "Select * from ( "
         + "SELECT ROW_NUMBER () Over (Order By Item.Id Desc) RecordNo, Item.Id, Item.ItemCode, Item.ItemCodeNew, Item.ItemName, "
         + "Item.BaseUnitId, Item.ItemCategoryId, Item.ItemTypeId, Item.ItemClassId, Item.PurchaseGLAC, "
         + "Item.SaleGLAC, Item.COGSGLAC, Item.HSCode, Item.PackQty, Item.ItemStatus, "
         + "Item.ManufactureId, Item.ManufacturePartNo, Item.BuyerSupplierId, "
         + "Item.BuyerPartNo, Item.ProductNo, Item.MinStockLevel, Item.OptimalStockLevel, "
         + "Item.MaxStockLevel, Item.ReorderLevel, Item.ItemOriginId, Item.CostPrice, "
         + "Item.UOMScheduleIdCostRate, Item.PurchasePrice, Item.UOMScheduleIdPurRate, "
         + "Item.RetailPrice, Item.UOMScheduleIdRetailRate, Item.WholeSalePrice, "
         + "Item.UOMScheduleIdWhsRate, Item.AmountCalcType, Item.BarcodeImage, Item.BarcodeNo, "
         + "Item.BaseCropYear, Item.ApplyGST, Item.ApplyVAT, Item.ApplyExciese, Item.OtherTax, "
         + "Case When IsNull(Pic1,'')='' Then cfg.AttachmentsUrl+'RiceImage.jpg' Else cfg.AttachmentsUrl+Pic1 End Pic1, Item.Pic2, Item.PostDate, Item.PostUser, "
         + "Item.PostState, Item.OrganizationId, Item.CompanyId, Item.BranchesId, "
         + "Item.ProjectsId, StockAc.AccountTitle AS GlStockAccountTitle, "
         + "SaleAc.AccountTitle AS GlSaleAccountTitle, CGSAc.AccountTitle AS GlCgsAccountTitle, "
         + "ItemCategory.CategoryDescription, ItemType.TypeDescription, "
         + "UOM.UOMCode, UOM.UOMCode BaseUnit, PC.InvParentCateDescription, CG.ClassGroupName "
         + ",ProductType, MasterItemId, ItemSpecification, UomLookUpId, "
         + "ItemAliasName, PurchaseTypeLookUpId, IsExpirable, ShelfLife, IsDiscountable, "
         + "SaleTaxPurPercent, AtRetailPriceTax, AtPurcahsePriceTax, "
         + "SaleTaxSalesPercent, IsTaxable, Item.DepreciationExpenseAcId, "
         + "Item.accumulatedDepreciationAcId, Item.CapitalWipAcId, Item.IsCompany, Item.IsThirdParty, Item.ReOrderQty, Item.IsImport, Item.LeadTimeDay, Item.AllowMultiUom, Item.PackSizeId, Item.PackSize "
         + ",entryUser.FirstName+entryUser.LastName EntryUserName, Item.EntryUser EntryUserId, "
         + "Item.EntryDate, modifyUser.FirstName+modifyUser.LastName ModifyUserName, Item.ItemQcGradeId, qc.QcGradeDesc ItemQcGrade, "
         + "Item.MaxMeaurementUnitId, MaxUOM.UOMCode MaxUOMCode, Item.WeightKgs, Item.MaxMeaurement, "
         + "Item.ModifyUser, Item.ModifyDate, item.ItemWithVarient, Item.ItemNameOtherLingo, "
         + "CASE WHEN ISNULL(entryUserProfile.ProfileImageFileName,'')='' THEN cfg.AttachmentsUrl+'DummyProfilePicture.jpg' ELSE cfg.AttachmentsUrl+entryUserProfile.ProfileImageFileName END EntryUserProfileImage "
         + ",CASE WHEN ISNULL(modifyuserProfile.ProfileImageFileName,'')='' THEN cfg.AttachmentsUrl+'DummyProfilePicture.jpg' ELSE cfg.AttachmentsUrl+modifyuserProfile.ProfileImageFileName END MoidifyUserProfileImage "
         + ",(select count(*) from DMSAttachments where RefDocumentNo = item.Id "
         + "and ScreenName = Case when ISNULL(:screenName,'') = '' then 'InvAddItemsPOS' else :screenName END) AS NoOfAttachments "
         + ",Item.MotherItemId, MIT.TypeDescription AS MotherItem, Item.itemModalId, modl.ModalName, "
         + "item.WeightSemiFinish, item.WeightFinishGoods, item.WeightSemiFinishSand, Item.EmptyBagWeight, MI.MasterItem, item.ServicesMasterItemId, lgs.LookUpName ServicesMasterItem "
         + "FROM Item "
         + "CROSS JOIN (SELECT dbo.GetConfigurationByConfigId(:organizationId,:companyId,91) AS AttachmentsUrl) cfg "
         + "LEFT JOIN vItemAllocation IOA on item.Id = IOA.Id and ioa.CompanyId = :companyId "
         + "INNER JOIN ChartofAccount AS StockAc ON Item.PurchaseGLAC = StockAc.Id "
         + "INNER JOIN ChartofAccount AS SaleAc ON Item.SaleGLAC = SaleAc.Id "
         + "INNER JOIN ChartofAccount AS CGSAc ON Item.COGSGLAC = CGSAc.Id "
         + "INNER JOIN ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "LEFT JOIN ItemType ON Item.ItemTypeId = ItemType.Id "
         + "LEFT JOIN UOM ON Item.BaseUnitId = UOM.Id "
         + "LEFT JOIN UOM MaxUOM ON Item.MaxMeaurementUnitId = MaxUOM.Id "
         + "INNER JOIN UserAccount entryUser on entryUser.ID=Item.EntryUser "
         + "LEFT JOIN UserProfile entryUserProfile on entryUserProfile.UserId=entryUser.ID "
         + "LEFT JOIN UserAccount modifyUser on modifyUser.ID=Item.ModifyUser "
         + "LEFT JOIN UserProfile modifyuserProfile on modifyuserProfile.UserId=ModifyUser.ID "
         + "LEFT JOIN InventoryParentCategories PC on ItemCategory.InventoryParentCategoriesId = PC.Id "
         + "LEFT JOIN ItemClassGroup CG ON ItemCategory.ItemClassGroupId = CG.Id "
         + "Left join ItemQcGrade qc on qc.Id=Item.ItemQcGradeId "
         + "LEFT JOIN ItemType MIT ON ITEM.MotherItemId = MIT.Id "
         + "Left Join item.itemModel modl on modl.itemModelId=item.itemModalId "
         + "Left join MasterItems MI ON MI.Id = Item.MasterItemId "
         + "Left join lgstcm.lookUp lgs on lgs.LookUpId = item.ServicesMasterItemId "
         + "where Item.OrganizationId = :organizationId "
         + "and Item.CompanyId = :companyId "
         + "and ItemCategory.InventoryParentCategoriesId <> 5 "
         + "and (:itemCategoryId is null or item.ItemCategoryId = :itemCategoryId) "
         + "and (:itemTypeId is null or item.ItemTypeId = :itemTypeId) "
         + "and (:lookupTypeIds IS NULL OR ItemType.Type in (select data from dbo.fnSplitString (:lookupTypeIds,','))) "
         + "and (:canViewAllRecord = 1 or (:canViewAllRecord = 0 and Item.EntryUser = :entryUser)) "
         + "and (:isTaxable is null or ISNULL(Item.IsTaxable,0)=:isTaxable) "
         + "and (:inventoryParentCategoriesId is null or ItemCategory.InventoryParentCategoriesId = :inventoryParentCategoriesId) "
         + "AND (:masterItemId IS NULL OR Item.MasterItemId = :masterItemId) "
         + "and (:parentIds is null or ItemCategory.InventoryParentCategoriesId in (select data from dbo.fnSplitString(:parentIds,','))) "
         + ")a Where (:noOfRecords is null or a.RecordNo <= :noOfRecords) "
         + "order by a.ItemCode asc", nativeQuery = true)
    List<ItemFormHistoryRow> formHistory(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("lookupTypeIds") String lookupTypeIds,
            @Param("canViewAllRecord") Boolean canViewAllRecord,
            @Param("entryUser") Integer entryUser,
            @Param("isTaxable") Integer isTaxable,
            @Param("inventoryParentCategoriesId") Integer inventoryParentCategoriesId,
            @Param("masterItemId") Integer masterItemId,
            @Param("parentIds") String parentIds,
            @Param("screenName") String screenName,
            @Param("noOfRecords") Integer noOfRecords);

    /** Sp_Item_GetAllMethod @Activity='GetGLAccountbyItemCategoryId' */
    @Query("SELECT c.id AS id, c.revenueAccountId AS revenueAccountId, c.inventoryAccountId AS inventoryAccountId, c.cgsAccountId AS cgsAccountId "
         + "FROM ItemCategory c WHERE c.organizationId = :organizationId AND c.companyId = :companyId AND c.id = :id")
    List<ItemCategoryGlAccountRow> getGLAccountbyItemCategoryId(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("id") Integer id);

    /** Sp_Item_GetAllMethod @Activity='GetGLAccountbyItemCategoryIdForFixedAssets' */
    @Query("SELECT c.id AS id, c.revenueAccountId AS revenueAccountId, c.inventoryAccountId AS inventoryAccountId, c.cgsAccountId AS cgsAccountId, "
         + "c.accumulatedDepreciationAcId AS accumulatedDepreciationAcId, c.depreciationExpenseAcId AS depreciationExpenseAcId, "
         + "c.expenseMaintenanceAccountId AS expenseMaintenanceAccountId, c.capitalWipAcId AS capitalWipAcId "
         + "FROM ItemCategory c WHERE c.organizationId = :organizationId AND c.companyId = :companyId AND c.id = :id")
    List<ItemCategoryGlAccountFixedAssetsRow> getGLAccountbyItemCategoryIdForFixedAssets(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("id") Integer id);

    /** Sp_Item_GetAllMethod @Activity='GetGLAccountReferedInItemDifinition' */
    @Query(value = "select Distinct i.PurchaseGLAC, cp.AccountTitle StockGLAccountTitle, "
         + "i.SaleGLAC, cs.AccountTitle as SalesGLAccountTitle, "
         + "i.COGSGLAC, cg.AccountTitle as CgsGLAccountTitle "
         + "from Item i "
         + "INNER JOIN vItemAllocation ioa on i.Id = ioa.Id "
         + "INNER JOIN ChartofAccount cp on i.PurchaseGLAC = cp.Id "
         + "INNER JOIN ChartofAccount cs on i.SaleGLAC = cs.Id "
         + "INNER JOIN ChartofAccount cg on i.COGSGLAC= cg.Id "
         + "Where i.OrganizationId = :organizationId "
         + "and (:companyId is null or ioa.CompanyId = :companyId)", nativeQuery = true)
    List<ItemGlReferedRow> getGLAccountReferedInItemDifinition(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='ReadAllForComboTwoColumns' */
    @Query(value = "select Item.Id, Item.ItemName, ItemCategory.CategoryDescription as ItemCategory, item.ItemCodeNew as ItemCode, dbo.ItemCategory.InventoryParentCategoriesId "
         + "FROM dbo.Item "
         + "INNER JOIN dbo.vItemAllocation IOA on item.Id = IOA.Id and Item.ItemStatus = 1 "
         + "LEFT JOIN dbo.ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "LEFT JOIN dbo.InventoryParentCategories PC ON dbo.ItemCategory.InventoryParentCategoriesId = PC.Id "
         + "LEFT JOIN dbo.ItemType ON Item.ItemTypeId = ItemType.Id "
         + "where Item.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "AND (:inventoryParentCategoriesId IS NULL OR dbo.ItemCategory.InventoryParentCategoriesId = :inventoryParentCategoriesId) "
         + "and IOA.IsActive = 1 "
         + "and ItemType.Type not in (14,17) "
         + "and (:inventoryParentCategoriesIdsNotIn IS NULL "
         + "OR NOT EXISTS (select 1 from dbo.fnSplitString (:inventoryParentCategoriesIdsNotIn,',') Where ItemCategory.InventoryParentCategoriesId in (data)))", nativeQuery = true)
    List<ItemComboParentRow> readAllForComboTwoColumns(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("inventoryParentCategoriesId") Integer inventoryParentCategoriesId,
            @Param("inventoryParentCategoriesIdsNotIn") String inventoryParentCategoriesIdsNotIn);

    /** Sp_Item_GetAllMethod @Activity='ReadAllForComboTwoColumnsWithParentId' */
    @Query(value = "select Item.Id, Item.ItemName, ItemCategory.CategoryDescription as ItemCategory, item.ItemCodeNew as ItemCode, dbo.ItemCategory.InventoryParentCategoriesId "
         + "FROM dbo.Item "
         + "INNER JOIN dbo.vItemAllocation IOA on item.Id = IOA.Id and Item.ItemStatus = 1 "
         + "INNER JOIN dbo.ChartofAccount AS StockAc ON Item.PurchaseGLAC = StockAc.Id "
         + "INNER JOIN dbo.ChartofAccount AS SaleAc ON Item.SaleGLAC = SaleAc.Id "
         + "INNER JOIN dbo.ChartofAccount AS CGSAc ON Item.COGSGLAC = CGSAc.Id "
         + "LEFT JOIN dbo.ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "LEFT JOIN dbo.InventoryParentCategories PC ON dbo.ItemCategory.InventoryParentCategoriesId = PC.Id "
         + "LEFT JOIN dbo.ItemType ON Item.ItemTypeId = ItemType.Id "
         + "where Item.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "AND (:inventoryParentCategoriesId IS NULL OR dbo.ItemCategory.InventoryParentCategoriesId = :inventoryParentCategoriesId) "
         + "and IOA.IsActive = 1 "
         + "and ItemType.Type not in (14,17) "
         + "and (:inventoryParentCategoriesIdsNotIn IS NULL "
         + "OR NOT EXISTS (select 1 from dbo.fnSplitString (:inventoryParentCategoriesIdsNotIn,',') Where ItemCategory.InventoryParentCategoriesId in (data)))", nativeQuery = true)
    List<ItemComboParentRow> readAllForComboTwoColumnsWithParentId(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("inventoryParentCategoriesId") Integer inventoryParentCategoriesId,
            @Param("inventoryParentCategoriesIdsNotIn") String inventoryParentCategoriesIdsNotIn);

    /** Sp_Item_GetAllMethod @Activity='ReadAllItemsByParentId' */
    @Query(value = "select Item.Id, Item.ItemName, ItemCategory.CategoryDescription as ItemCategory, item.ItemCodeNew as ItemCode "
         + "FROM dbo.Item "
         + "INNER JOIN dbo.vItemAllocation IOA on item.Id = IOA.Id and Item.ItemStatus = 1 "
         + "LEFT JOIN dbo.ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "where Item.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "AND (:inventoryParentCategoriesId IS NULL OR dbo.ItemCategory.InventoryParentCategoriesId = :inventoryParentCategoriesId) "
         + "and IOA.IsActive = 1", nativeQuery = true)
    List<ItemComboRow> readAllItemsByParentId(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("inventoryParentCategoriesId") Integer inventoryParentCategoriesId);

    /** Sp_Item_GetAllMethod @Activity='ReadAllItemsTaxable' */
    @Query(value = "select Item.Id, Item.ItemName, ItemCategory.CategoryDescription as ItemCategory, item.ItemCodeNew as ItemCode "
         + "FROM dbo.Item "
         + "INNER JOIN dbo.vItemAllocation IOA on item.Id = IOA.Id and Item.ItemStatus = 1 "
         + "INNER JOIN dbo.ChartofAccount AS StockAc ON Item.PurchaseGLAC = StockAc.Id "
         + "INNER JOIN dbo.ChartofAccount AS SaleAc ON Item.SaleGLAC = SaleAc.Id "
         + "INNER JOIN dbo.ChartofAccount AS CGSAc ON Item.COGSGLAC = CGSAc.Id "
         + "LEFT JOIN dbo.ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "LEFT JOIN dbo.InventoryParentCategories PC ON dbo.ItemCategory.InventoryParentCategoriesId = PC.Id "
         + "LEFT JOIN dbo.ItemType ON Item.ItemTypeId = ItemType.Id "
         + "where Item.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "and IOA.IsActive = 1 "
         + "and ItemType.Type not in (14,17) "
         + "and (ISNULL(Item.IsTaxable,0)=1)", nativeQuery = true)
    List<ItemComboRow> readAllItemsTaxable(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='ReadAllForExportCombo' */
    @Query(value = "select Item.Id, item.ItemName, item.ItemCodeNew as ItemCode "
         + "FROM Item "
         + "INNER JOIN vItemAllocation IOA on item.Id = IOA.Id "
         + "INNER JOIN ChartofAccount AS StockAc ON Item.PurchaseGLAC = StockAc.Id "
         + "INNER JOIN ChartofAccount AS SaleAc ON Item.SaleGLAC = SaleAc.Id "
         + "INNER JOIN ChartofAccount AS CGSAc ON Item.COGSGLAC = CGSAc.Id "
         + "INNER JOIN ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "INNER JOIN ItemType ON Item.ItemTypeId = ItemType.Id "
         + "where Item.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "and IOA.IsActive = 1 "
         + "and item.ItemClassId = 8 "
         + "and ItemCategory.InventoryParentCategoriesId <> 5", nativeQuery = true)
    List<ItemCodeNameRow> readAllForExportCombo(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='GetItemTypeFromInventoryTransactions' */
    @Query(value = "select Distinct i.ItemTypeId, it.TypeDescription "
         + "from InventoryTransactions sd "
         + "Inner join item i on sd.ItemId = i.Id "
         + "inner join ItemType it on i.ItemTypeId = it.Id "
         + "inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
         + "Where i.OrganizationId = :organizationId "
         + "and i.CompanyId = :companyId "
         + "and (:parentIds IS NULL OR ic.InventoryParentCategoriesId in (select data from dbo.fnSplitString (:parentIds,',')))", nativeQuery = true)
    List<LookupItemTypeRow> getItemTypeFromInventoryTransactions(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("parentIds") String parentIds);

    /** Sp_Item_GetAllMethod @Activity='ReadAllForItemsForServices' */
    @Query(value = "select Item.Id, Item.ItemName, item.ItemCodeNew ItemCode "
         + "FROM Item "
         + "INNER JOIN vItemAllocation IOA ON Item.Id = IOA.Id "
         + "INNER JOIN ChartofAccount AS StockAc ON Item.PurchaseGLAC = StockAc.Id "
         + "INNER JOIN ChartofAccount AS SaleAc ON Item.SaleGLAC = SaleAc.Id "
         + "INNER JOIN ChartofAccount AS CGSAc ON Item.COGSGLAC = CGSAc.Id "
         + "INNER JOIN ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "LEFT JOIN ItemType ON Item.ItemTypeId = ItemType.Id "
         + "where item.ItemClassId = 6 "
         + "and Item.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "and Item.ItemStatus = 1 "
         + "and ItemCategory.InventoryParentCategoriesId <> 5", nativeQuery = true)
    List<ItemCodeNameRow> readAllForItemsForServices(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='ReadAllForItemsForBrands' */
    @Query(value = "select Item.Id, Item.ItemName "
         + "FROM Item "
         + "INNER JOIN vItemAllocation IOA ON Item.Id = IOA.Id "
         + "INNER JOIN ChartofAccount AS StockAc ON Item.PurchaseGLAC = StockAc.Id "
         + "INNER JOIN ChartofAccount AS SaleAc ON Item.SaleGLAC = SaleAc.Id "
         + "INNER JOIN ChartofAccount AS CGSAc ON Item.COGSGLAC = CGSAc.Id "
         + "INNER JOIN ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "LEFT JOIN ItemType ON Item.ItemTypeId = ItemType.Id "
         + "where item.ItemClassId = 7 "
         + "and Item.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "and Item.ItemStatus = 1", nativeQuery = true)
    List<ItemIdNameRow> readAllForItemsForBrands(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='ReadAllForItemsForPartyProcessing' */
    @Query(value = "select Item.Id, Item.ItemName, Item.ItemCodeNew ItemCode "
         + "FROM Item "
         + "INNER JOIN vItemAllocation IOA ON Item.Id = IOA.Id "
         + "INNER JOIN ChartofAccount AS StockAc ON Item.PurchaseGLAC = StockAc.Id "
         + "INNER JOIN ChartofAccount AS SaleAc ON Item.SaleGLAC = SaleAc.Id "
         + "INNER JOIN ChartofAccount AS CGSAc ON Item.COGSGLAC = CGSAc.Id "
         + "INNER JOIN ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "LEFT JOIN ItemType ON Item.ItemTypeId = ItemType.Id "
         + "where Item.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "and Item.ItemStatus = 1 "
         + "And ISNULL(Item.IsThirdParty,0)=1 "
         + "and (:parentIds IS NULL OR ItemCategory.InventoryParentCategoriesId in (select data from dbo.fnSplitString (:parentIds,',')))", nativeQuery = true)
    List<ItemCodeNameRow> readAllForItemsForPartyProcessing(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("parentIds") String parentIds);

    /** Sp_Item_GetAllMethod @Activity='ReadAllForItemsForPartyProcessingPM' */
    @Query(value = "select Item.Id, Item.ItemName, Item.ItemCodeNew ItemCode "
         + "FROM Item "
         + "INNER JOIN vItemAllocation IOA ON Item.Id = IOA.Id "
         + "INNER JOIN ChartofAccount AS StockAc ON Item.PurchaseGLAC = StockAc.Id "
         + "INNER JOIN ChartofAccount AS SaleAc ON Item.SaleGLAC = SaleAc.Id "
         + "INNER JOIN ChartofAccount AS CGSAc ON Item.COGSGLAC = CGSAc.Id "
         + "INNER JOIN ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "LEFT JOIN ItemType ON Item.ItemTypeId = ItemType.Id "
         + "where Item.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "and IOA.IsActive = 1 "
         + "And ISNULL(Item.IsThirdParty,0)=1 "
         + "AND ItemCategory.InventoryParentCategoriesId=7", nativeQuery = true)
    List<ItemCodeNameRow> readAllForItemsForPartyProcessingPM(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='GetItemsFromInventoryStocksEvaluations' */
    @Query(value = "select Distinct sd.ItemId, i.ItemName "
         + "from InventoryStockEvalautionDetail sd "
         + "Inner join Item i on sd.ItemId = i.Id "
         + "inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
         + "Where sd.OrganizationId = :organizationId "
         + "and sd.CompanyId = :companyId "
         + "and (:parentIds IS NULL OR ic.InventoryParentCategoriesId in (select data from dbo.fnSplitString (:parentIds,','))) "
         + "and (:itemCategoryId is null or i.ItemCategoryId = :itemCategoryId) "
         + "and (:actionId is null Or (:actionId = 1 AND sd.RefDocumentTypeId in (56,57,98,138,172)) OR (:actionId = 2 AND sd.RefDocumentTypeId in (95,99,139,171)))", nativeQuery = true)
    List<LookupItemRow> getItemsFromInventoryStocksEvaluations(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("parentIds") String parentIds,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("actionId") Integer actionId);

    /** Sp_Item_GetAllMethod @Activity='GetItemTypeFromInventoryStocksEvaluations' */
    @Query(value = "select Distinct i.ItemTypeId, it.TypeDescription "
         + "from InventoryStockEvalautionDetail sd "
         + "Inner join item i on sd.ItemId = i.Id "
         + "inner join ItemType it on i.ItemTypeId = it.Id "
         + "inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
         + "Where i.OrganizationId = :organizationId "
         + "and i.CompanyId = :companyId "
         + "and (:parentIds IS NULL OR ic.InventoryParentCategoriesId in (select data from dbo.fnSplitString (:parentIds,','))) "
         + "and (:actionId is null Or (:actionId = 1 AND sd.RefDocumentTypeId in (56,57,98,138,172)) OR (:actionId = 2 AND sd.RefDocumentTypeId in (95,99,139,171)))", nativeQuery = true)
    List<LookupItemTypeRow> getItemTypeFromInventoryStocksEvaluations(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("parentIds") String parentIds,
            @Param("actionId") Integer actionId);

    /** Sp_Item_GetAllMethod @Activity='GetItemCategoryFromInventoryStocksEvaluations' */
    @Query(value = "select Distinct i.ItemCategoryId, ic.CategoryDescription "
         + "from InventoryStockEvalautionDetail sd "
         + "Inner join item i on sd.ItemId = i.Id "
         + "inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
         + "Where i.OrganizationId = :organizationId "
         + "and i.CompanyId = :companyId "
         + "and (:parentIds IS NULL OR ic.InventoryParentCategoriesId in (select data from dbo.fnSplitString (:parentIds,','))) "
         + "and (:actionId is null Or (:actionId = 1 AND sd.RefDocumentTypeId in (56,57,98,138,172)) OR (:actionId = 2 AND sd.RefDocumentTypeId in (95,99,139,171)))", nativeQuery = true)
    List<LookupItemCategoryRow> getItemCategoryFromInventoryStocksEvaluations(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("parentIds") String parentIds,
            @Param("actionId") Integer actionId);

    /** Sp_Item_GetAllMethod @Activity='GetWarehouseFromInventoryStocksEvaluations' */
    @Query(value = "select Distinct sd.WarehouseId, w.WareHouseName "
         + "from InventoryStockEvalautionDetail sd "
         + "Inner join item i on sd.ItemId = i.Id "
         + "inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
         + "Inner join InvWareHouse w on sd.WarehouseId = w.Id "
         + "Where w.OrganizationId = :organizationId "
         + "and w.CompanyId = :companyId "
         + "and (:parentIds IS NULL OR ic.InventoryParentCategoriesId in (select data from dbo.fnSplitString (:parentIds,','))) "
         + "and (:actionId is null Or (:actionId = 1 AND sd.RefDocumentTypeId in (56,57,98,138,172)) OR (:actionId = 2 AND sd.RefDocumentTypeId in (95,99,139,171)))", nativeQuery = true)
    List<LookupWarehouseRow> getWarehouseFromInventoryStocksEvaluations(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("parentIds") String parentIds,
            @Param("actionId") Integer actionId);

    /** Sp_Item_GetAllMethod @Activity='GetJobLotFromInventoryStocksEvaluations' */
    @Query(value = "select Distinct sd.JobLotId, j.JobLotDescription "
         + "from InventoryStockEvalautionDetail sd "
         + "Inner join item i on sd.ItemId = i.Id "
         + "inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
         + "Inner join JobLot j on sd.JobLotId = j.Id "
         + "Where j.OrganizationId = :organizationId "
         + "and j.CompanyId = :companyId "
         + "and (:parentIds IS NULL OR ic.InventoryParentCategoriesId in (select data from dbo.fnSplitString (:parentIds,','))) "
         + "and (:actionId is null Or (:actionId = 1 AND sd.RefDocumentTypeId in (56,57,98,138,172)) OR (:actionId = 2 AND sd.RefDocumentTypeId in (95,99,139,171)))", nativeQuery = true)
    List<LookupJobLotRow> getJobLotFromInventoryStocksEvaluations(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("parentIds") String parentIds,
            @Param("actionId") Integer actionId);

    /** Sp_Item_GetAllMethod @Activity='GetDocumentTypeFromInventoryStocksEvaluations' */
    @Query("SELECT DISTINCT sd.refDocumentTypeId AS refDocumentTypeId, dt.documentTypeDescription AS documentTypeDescription "
         + "FROM InventoryStockEvalautionDetail sd JOIN DocumentType dt ON sd.refDocumentTypeId = dt.id "
         + "WHERE sd.organizationId = :organizationId AND sd.companyId = :companyId "
         + "AND (:actionId IS NULL OR (:actionId = 1 AND sd.refDocumentTypeId IN (56,57,98,138,172)) OR (:actionId = 2 AND sd.refDocumentTypeId IN (95,99,139,171)))")
    List<LookupDocumentTypeRow> getDocumentTypeFromInventoryStocksEvaluations(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("actionId") Integer actionId);

    /** Sp_Item_GetAllMethod @Activity='GetSupplierCustomerFromInventoryStocksEvaluations' */
    @Query(value = "select Distinct sd.SupplierCustomerId, sp.CompanyName "
         + "from InventoryStockEvalautionDetail sd "
         + "Inner join item i on sd.ItemId = i.Id "
         + "inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
         + "Inner join SupplierCustomer sp on sd.SupplierCustomerId = sp.Id "
         + "Where sd.OrganizationId = :organizationId "
         + "and sd.CompanyId = :companyId "
         + "and (:parentIds IS NULL OR ic.InventoryParentCategoriesId in (select data from dbo.fnSplitString (:parentIds,','))) "
         + "and (:actionId is null Or (:actionId = 1 AND sd.RefDocumentTypeId in (56,57,98,138,172)) OR (:actionId = 2 AND sd.RefDocumentTypeId in (95,99,139,171)))", nativeQuery = true)
    List<LookupSupplierCustomerRow> getSupplierCustomerFromInventoryStocksEvaluations(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("parentIds") String parentIds,
            @Param("actionId") Integer actionId);

    /** Sp_Item_GetAllMethod @Activity='GetItemByItemTypeId' */
    @Query(value = "select i.Id, i.ItemName, i.ItemCodeNew, ItemCategory.InventoryParentCategoriesId, ipc.InvParentCateCode, ipc.InvParentCateDescription As ParentCategory "
         + "from vItemAllocation AI "
         + "inner join Item i on AI.Id = i.Id "
         + "left join ItemCategory on i.ItemCategoryId = ItemCategory.Id "
         + "left join ItemType t on i.ItemTypeId = t.Id "
         + "left join InvLookUp l on t.Type = l.Id "
         + "INNER JOIN InventoryParentCategories ipc on ItemCategory.InventoryParentCategoriesId = ipc.Id "
         + "where i.OrganizationId = :organizationId "
         + "and AI.CompanyId = :companyId "
         + "and ItemCategory.InventoryParentCategoriesId <> 5 "
         + "and (t.Type in (select data from dbo.fnSplitString (:lookupTypeIds,',')))", nativeQuery = true)
    List<ItemByTypeRow> getItemByItemTypeId(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("lookupTypeIds") String lookupTypeIds);

    /** Sp_Item_GetAllMethod @Activity='AllItemsBindForEngr' */
    @Query("SELECT DISTINCT i.id AS id, i.itemName AS itemName, i.itemCodeNew AS itemCodeNew, i.itemCategoryId AS itemCategoryId, "
         + "i.weightFinishGoods AS finishWeight, i.weightSemiFinish AS dyeWeight, i.weightSemiFinishSand AS sandWeight, i.itemModalId AS itemModalId "
         + "FROM ItemAllocation ai JOIN Item i ON ai.itemId = i.id "
         + "WHERE i.organizationId = :organizationId AND ai.companyId = :companyId")
    List<ItemEngrRow> allItemsBindForEngr(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='AllItemsBindForEngrWithModalOnly' */
    @Query(value = "select distinct i.Id, i.ItemName, i.ItemCodeNew, i.ItemCategoryId, m.SampleNo "
         + "from ItemAllocation AI "
         + "inner join Item i on AI.ItemId = i.Id "
         + "Inner Join item.itemModel m on m.itemModelId=i.itemModalId "
         + "where i.OrganizationId = :organizationId "
         + "and AI.CompanyId = :companyId", nativeQuery = true)
    List<ItemEngrModalRow> allItemsBindForEngrWithModalOnly(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='AllItemsBindForSteel' */
    @Query(value = "select i.Id, i.ItemName "
         + "from Item i "
         + "INNER JOIN vItemAllocation IOA ON i.Id = IOA.Id "
         + "where i.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId", nativeQuery = true)
    List<ItemIdNameRow> allItemsBindForSteel(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='Update' (UPDATE Item SET ItemName, ItemNameOtherLingo WHERE Id) */
    @Modifying
    @Transactional
    @Query("UPDATE Item i SET i.itemName = :itemName, i.itemNameOtherLingo = :itemNameOtherLingo WHERE i.id = :id")
    int updateNameById(@Param("itemName") String itemName,
            @Param("itemNameOtherLingo") String itemNameOtherLingo,
            @Param("id") Integer id);

    /** Sp_Item_GetAllMethod @Activity='GetItemByWarehouseIdfromInventoryTransactions' */
    @Query("SELECT DISTINCT t.itemId AS itemId, i.itemName AS itemName "
         + "FROM InventoryTransactions t "
         + "JOIN Item i ON t.itemId = i.id "
         + "JOIN ItemType it ON i.itemTypeId = it.id "
         + "LEFT JOIN InvLookUp l ON it.type = l.id "
         + "WHERE t.organizationId = :organizationId AND t.companyId = :companyId AND t.warehouseId = :warehouseId "
         + "AND it.type NOT IN (14,17)")
    List<LookupItemRow> getItemByWarehouseIdfromInventoryTransactions(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("warehouseId") Integer warehouseId);

    /** Sp_Item_GetAllMethod @Activity='GetItemsForAllocatetoWarehouse' */
    @Query(value = "SELECT Item.Id, Item.ItemName, item.ItemCategoryId, Item.ItemTypeId, item.PurchaseGLAC, Item.SaleGLAC, Item.COGSGLAC "
         + "FROM Item "
         + "INNER JOIN vItemAllocation IOA ON Item.Id = IOA.Id "
         + "INNER JOIN ChartofAccount AS StockAc ON Item.PurchaseGLAC = StockAc.Id "
         + "INNER JOIN ChartofAccount AS SaleAc ON Item.SaleGLAC = SaleAc.Id "
         + "INNER JOIN ChartofAccount AS CGSAc ON Item.COGSGLAC = CGSAc.Id "
         + "INNER JOIN ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "INNER JOIN ItemType ON Item.ItemTypeId = ItemType.Id "
         + "INNER JOIN UOM ON Item.BaseUnitId = UOM.Id "
         + "where Item.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "and (:itemCategoryId is null or item.ItemCategoryId = :itemCategoryId) "
         + "and (:itemTypeId is null or item.ItemTypeId = :itemTypeId) "
         + "and Item.ItemStatus = 1 "
         + "and ItemCategory.InventoryParentCategoriesId <> 5", nativeQuery = true)
    List<ItemAllocateWarehouseRow> getItemsForAllocatetoWarehouse(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("itemTypeId") Integer itemTypeId);

    /** Sp_Item_GetAllMethod @Activity='GetParentCategoryIdByItemCategoryId' */
    @Query("SELECT c.inventoryParentCategoriesId AS inventoryParentCategoriesId FROM ItemCategory c "
         + "WHERE c.organizationId = :organizationId AND c.companyId = :companyId AND (:itemCategoryId IS NULL OR c.id = :itemCategoryId)")
    List<ItemCategoryParentIdRow> getParentCategoryIdByItemCategoryId(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("itemCategoryId") Integer itemCategoryId);

    /** Sp_Item_GetAllMethod @Activity='ReadByCategoryTypeClassId' */
    @Query(value = "SELECT Item.Id, Item.ItemCode, Item.ItemName, UOM.UOMCode "
         + "FROM Item "
         + "INNER JOIN vItemAllocation i on item.Id = i.Id "
         + "INNER JOIN ChartofAccount AS StockAc ON Item.PurchaseGLAC = StockAc.Id "
         + "INNER JOIN ChartofAccount AS SaleAc ON Item.SaleGLAC = SaleAc.Id "
         + "INNER JOIN ChartofAccount AS CGSAc ON Item.COGSGLAC = CGSAc.Id "
         + "LEFT JOIN ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "INNER JOIN ItemType ON Item.ItemTypeId = ItemType.Id "
         + "INNER JOIN UOM ON Item.BaseUnitId = UOM.Id "
         + "where Item.OrganizationId = :organizationId "
         + "and (:companyId IS NULL or i.CompanyId = :companyId) "
         + "and (Item.ItemStatus = 1) "
         + "AND (:itemTypeId IS NULL OR item.ItemTypeId = :itemTypeId) "
         + "AND (:itemClassId IS NULL OR item.ItemClassId = :itemClassId) "
         + "AND (:purchaseTypeLookupId IS NULL or item.PurchaseTypeLookUpId = :purchaseTypeLookupId)", nativeQuery = true)
    List<ItemUomRow> readByCategoryTypeClassId(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("itemClassId") Integer itemClassId,
            @Param("purchaseTypeLookupId") Integer purchaseTypeLookupId);

    /** Sp_Item_GetAllMethod @Activity='GetItemsFromItemPricingSchedule' */
    @Query(value = "SELECT * FROM ( "
         + "SELECT ROW_NUMBER() OVER (PARTITION BY p.ItemId ORDER BY EffectedDate DESC) RecordNo, p.Id, p.ScheduleDesc, p.EffectedDate, p.ItemId, i.ItemName, p.PriceTypeId, p.ItemPrice "
         + "FROM [dbo].[ItemPricingSchedule] p "
         + "INNER JOIN dbo.vItemAllocation i ON i.Id = p.ItemId "
         + "WHERE p.OrganizationId = :organizationId "
         + "AND (:companyId IS NULL or p.CompanyId = :companyId) "
         + "AND (i.IsActive = 1) "
         + "AND p.PriceTypeId = :priceTypeId "
         + ")a WHERE a.RecordNo = 1", nativeQuery = true)
    List<ItemPricingScheduleRow> getItemsFromItemPricingSchedule(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("priceTypeId") Integer priceTypeId);

    /** Sp_Item_GetAllMethod @Activity='GetItemGlIdsandItemName' */
    @Query(value = "SELECT i.Id, i.ItemName, PurchaseGLAC, SaleGLAC, COGSGLAC "
         + "FROM item i "
         + "left join dbo.vItemAllocation ioa on i.Id = ioa.Id "
         + "WHERE i.OrganizationId = :organizationId "
         + "AND ioa.CompanyId = :companyId", nativeQuery = true)
    List<ItemGlIdsRow> getItemGlIdsandItemName(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='GetPartyProcessingItemGlIdsandItemName' */
    @Query(value = "SELECT i.Id, i.ItemName, i.PurchaseGLAC, i.SaleGLAC, i.COGSGLAC "
         + "FROM Item i "
         + "INNER JOIN vItemAllocation IOA ON i.Id = IOA.Id "
         + "WHERE i.OrganizationId = :organizationId "
         + "AND IOA.CompanyId = :companyId "
         + "AND Coalesce(i.IsThirdPArty,0) = 1", nativeQuery = true)
    List<ItemGlIdsRow> getPartyProcessingItemGlIdsandItemName(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='GetCurrQtyByItemIdandWarehouse' */
    @Query(value = "SELECT dbo.FunGetQtyCurrStockByItem(:organizationId, :companyId, :itemId, :toDate, :warehouseId, NULL) AS AvailableQty", nativeQuery = true)
    AvailableQtyRow getCurrQtyByItemIdandWarehouse(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("itemId") Integer itemId,
            @Param("toDate") LocalDate toDate,
            @Param("warehouseId") Integer warehouseId);

    /** Sp_Item_GetAllMethod @Activity='GetEqvilentByItemId' */
    @Query(value = "SELECT dbo.GetEquivalentByItemIdandBaseUnitId(:organizationId, :companyId, :itemId, NULL) AS PackUom", nativeQuery = true)
    PackUomRow getEqvilentByItemId(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("itemId") Integer itemId);

    /** Sp_Item_GetAllMethod @Activity='GetEqvilentByItemIdAndUomScheduleId' */
    @Query("SELECT u.equivalent AS equivalent FROM UOMSchedule u "
         + "WHERE u.organizationId = :organizationId AND u.companyId = :companyId AND u.itemId = :itemId AND u.id = :scheduleId")
    List<UomScheduleEquivalentRow> getEqvilentByItemIdAndUomScheduleId(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("itemId") Integer itemId,
            @Param("scheduleId") Integer scheduleId);

    /** Sp_Item_GetAllMethod @Activity='GetWeightAvgRateByItem' */
    @Query(value = "SELECT dbo.FunGetWeightAvgRateByItem(:organizationId, :companyId, :itemId, :toDate, :warehouseId) AS AvgRate", nativeQuery = true)
    AvgRateRow getWeightAvgRateByItem(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("itemId") Integer itemId,
            @Param("toDate") LocalDate toDate,
            @Param("warehouseId") Integer warehouseId);

    /** Sp_Item_GetAllMethod @Activity='FormHistoryForFixedAssets' */
    @Query(value = "Select * from ( "
         + "SELECT ROW_NUMBER () Over (Order By Item.Id Desc) RecordNo, Item.Id, Item.ItemCodeNew, Item.ItemCode AssetCode, Item.ItemName AssetName, "
         + "ItemCategory.CategoryDescription as AssetCategory, ItemType.TypeDescription as AssetType, item.EntryDate "
         + ",USR.FirstName + ' ' + USR.LastName AS UserName, "
         + "(select count(Attachment) from DMSAttachments where RefDocumentNo = item.Id and ScreenName = 'frmDefineAssets') as NoOfAttachments, "
         + "accumulatedDepreciationAccount.AccountTitle Accumulated_Depreciation, "
         + "DepreciationExpenseAccount.AccountTitle Depreciation_Expense, Item.ItemQcGradeId, qc.QcGradeDesc ItemQcGrade, clg.ClassGroupName, "
         + "Item.MaxMeaurementUnitId, Item.WeightKgs, Item.MaxMeaurement, cp.InvParentCateDescription, item.ItemStatus, "
         + "CapitalWipAccount.AccountTitle Assets_GL, Item.IsCompany, Item.IsThirdParty, Item.ReOrderQty, Item.IsImport, Item.LeadTimeDay, Item.AllowMultiUom, Item.PackSizeId, Item.PackSize "
         + ",Item.MotherItemId "
         + "FROM Item "
         + "Left JOIN ChartofAccount AS StockAc ON Item.PurchaseGLAC = StockAc.Id "
         + "Left JOIN ChartofAccount AS SaleAc ON Item.SaleGLAC = SaleAc.Id "
         + "Left JOIN ChartofAccount AS CGSAc ON Item.COGSGLAC = CGSAc.Id "
         + "INNER JOIN ChartofAccount as accumulatedDepreciationAccount ON item.accumulatedDepreciationAcId = accumulatedDepreciationAccount.Id "
         + "INNER JOIN ChartofAccount as DepreciationExpenseAccount ON item.DepreciationExpenseAcId = DepreciationExpenseAccount.Id "
         + "INNER JOIN ChartofAccount as CapitalWipAccount on item.CapitalWipAcId = CapitalWipAccount.Id "
         + "INNER JOIN ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "INNER JOIN ItemType ON Item.ItemTypeId = ItemType.Id "
         + "INNER JOIN UserAccount USR ON Item.EntryUser = USR.ID "
         + "Left join ItemQcGrade qc on qc.Id=Item.ItemQcGradeId "
         + "Inner join InventoryParentCategories cp on cp.Id=ItemCategory.InventoryParentCategoriesId "
         + "Inner join ItemClassGroup clg on clg.Id=ItemCategory.ItemClassGroupId "
         + "where Item.OrganizationId = :organizationId "
         + "and Item.CompanyId = :companyId "
         + "and ItemCategory.InventoryParentCategoriesId = 5 "
         + "and (:itemCategoryId is null or item.ItemCategoryId = :itemCategoryId) "
         + "and (:canViewAllRecord = 1 or (:canViewAllRecord = 0 and Item.EntryUser = :entryUser)) "
         + ")a Where (:noOfRecords is null or a.RecordNo <= :noOfRecords) "
         + "order by convert(int,a.AssetCode) asc", nativeQuery = true)
    List<ItemFixedAssetsFormHistoryRow> formHistoryForFixedAssets(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("canViewAllRecord") Boolean canViewAllRecord,
            @Param("entryUser") Integer entryUser,
            @Param("noOfRecords") Integer noOfRecords);

    /** Sp_Item_GetAllMethod @Activity='ReadAllForFixedAssetsCombo' */
    @Query(value = "select Item.Id, item.ItemName "
         + "FROM Item "
         + "INNER JOIN vItemAllocation IOA on item.Id = IOA.Id "
         + "INNER JOIN ChartofAccount AS DepreciationExpenseAcId ON Item.DepreciationExpenseAcId = DepreciationExpenseAcId.Id "
         + "INNER JOIN ChartofAccount AS accumulatedDepreciationAcId ON Item.accumulatedDepreciationAcId= accumulatedDepreciationAcId.Id "
         + "INNER JOIN ChartofAccount AS CapitalWipAcId ON Item.CapitalWipAcId = CapitalWipAcId.Id "
         + "INNER JOIN ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "INNER JOIN ItemType ON Item.ItemTypeId = ItemType.Id "
         + "where Item.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "and IOA.IsActive = 1 "
         + "and ItemCategory.InventoryParentCategoriesId = 5", nativeQuery = true)
    List<ItemIdNameRow> readAllForFixedAssetsCombo(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='GetItemIdByBarcodeNo' */
    @Query("SELECT i.id AS id FROM Item i WHERE i.organizationId = :organizationId AND i.companyId = :companyId AND i.barcodeNo = :barcodeNo")
    List<ItemIdRow> getItemIdByBarcodeNo(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("barcodeNo") String barcodeNo);

    /** Sp_Item_GetAllMethod @Activity='GetItemsByItemCategoryAndItemType' */
    @Query(value = "SELECT ioa.Id, ioa.ItemName, PurchaseGLAC, SaleGLAC, COGSGLAC "
         + "FROM dbo.vItemAllocation ioa "
         + "inner join item i on ioa.Id = i.Id "
         + "WHERE i.OrganizationId = :organizationId "
         + "AND ioa.CompanyId = :companyId "
         + "and (:itemCategoryId is null or i.ItemCategoryId = :itemCategoryId) "
         + "and (:itemTypeId is null or i.ItemTypeId = :itemTypeId) "
         + "and (:id is null or i.Id = :id)", nativeQuery = true)
    List<ItemGlIdsRow> getItemsByItemCategoryAndItemType(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("id") Integer id);

    /** Sp_Item_GetAllMethod @Activity='GetPaddyRiceParentCategories' */
    @Query(value = "select distinct ipc.Id, ipc.InvParentCateCode "
         + "from InventoryTransactions it "
         + "INNER JOIN InvWareHouse wh on it.WarehouseId = wh.Id "
         + "INNER JOIN item i on it.ItemId = i.Id "
         + "INNER JOIN ItemCategory ic on ic.id = i.ItemCategoryId "
         + "INNER JOIN InventoryParentCategories ipc on ic.InventoryParentCategoriesId = ipc.Id "
         + "where it.OrganizationId = :organizationId "
         + "and it.CompanyId = :companyId "
         + "and (:parentIds IS NULL OR ic.InventoryParentCategoriesId in (select data from dbo.fnSplitString (:parentIds,',')))", nativeQuery = true)
    List<ParentCategoryCodeRow> getPaddyRiceParentCategories(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("parentIds") String parentIds);

    /** Sp_Item_GetAllMethod @Activity='GetItemCategoryFromInventoryTransactions' */
    @Query(value = "select Distinct i.ItemCategoryId, ic.CategoryDescription "
         + "from InventoryTransactions sd "
         + "Inner join item i on sd.ItemId = i.Id "
         + "inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
         + "Where i.OrganizationId = :organizationId "
         + "and i.CompanyId = :companyId "
         + "and (:parentIds IS NULL OR ic.InventoryParentCategoriesId in (select data from dbo.fnSplitString (:parentIds,',')))", nativeQuery = true)
    List<LookupItemCategoryRow> getItemCategoryFromInventoryTransactions(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("parentIds") String parentIds);

    /** Sp_Item_GetAllMethod @Activity='GetItemsFromInventoryTransactions' */
    @Query(value = "select Distinct sd.ItemId, i.ItemName "
         + "from InventoryTransactions sd "
         + "Inner join Item i on sd.ItemId = i.Id "
         + "INNER JOIN ItemCategory ic on ic.id = i.ItemCategoryId "
         + "Where sd.OrganizationId = :organizationId "
         + "and sd.CompanyId = :companyId "
         + "and (:parentIds IS NULL OR ic.InventoryParentCategoriesId in (select data from dbo.fnSplitString (:parentIds,',')))", nativeQuery = true)
    List<LookupItemRow> getItemsFromInventoryTransactions(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("parentIds") String parentIds);

    /** Sp_Item_GetAllMethod @Activity='GetWarehouseFromInventoryTransactions' */
    @Query(value = "select Distinct sd.WarehouseId, w.WareHouseName "
         + "from InventoryTransactions sd "
         + "Inner join InvWareHouse w on sd.WarehouseId = w.Id "
         + "Inner join item i on sd.ItemId = i.Id "
         + "inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
         + "Where sd.OrganizationId = :organizationId "
         + "and sd.CompanyId = :companyId "
         + "and (:parentIds IS NULL OR ic.InventoryParentCategoriesId in (select data from dbo.fnSplitString (:parentIds,',')))", nativeQuery = true)
    List<LookupWarehouseRow> getWarehouseFromInventoryTransactions(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("parentIds") String parentIds);

    /** Sp_Item_GetAllMethod @Activity='GetJobLotFromInventoryTransactions' */
    @Query(value = "select Distinct sd.JobLotId, j.JobLotDescription "
         + "from InventoryTransactions sd "
         + "Inner join JobLot j on sd.JobLotId = j.Id "
         + "Inner join item i on sd.ItemId = i.Id "
         + "inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
         + "Where sd.OrganizationId = :organizationId "
         + "and sd.CompanyId = :companyId "
         + "and (:parentIds IS NULL OR ic.InventoryParentCategoriesId in (select data from dbo.fnSplitString (:parentIds,',')))", nativeQuery = true)
    List<LookupJobLotRow> getJobLotFromInventoryTransactions(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("parentIds") String parentIds);

    /** Sp_Item_GetAllMethod @Activity='GetSupplierCustomerFromInventoryTransactions' */
    @Query(value = "select Distinct sd.SupplierCustomerId, sp.CompanyName "
         + "from InventoryTransactions sd "
         + "Inner join item i on sd.ItemId = i.Id "
         + "inner join ItemCategory ic on i.ItemCategoryId = ic.Id "
         + "Inner join SupplierCustomer sp on sd.SupplierCustomerId = sp.Id "
         + "Where sd.OrganizationId = :organizationId "
         + "and sd.CompanyId = :companyId "
         + "and (:parentIds IS NULL OR ic.InventoryParentCategoriesId in (select data from dbo.fnSplitString (:parentIds,',')))", nativeQuery = true)
    List<LookupSupplierCustomerRow> getSupplierCustomerFromInventoryTransactions(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("parentIds") String parentIds);

    /** Sp_Item_GetAllMethod @Activity='GetWarehouseForStoreFromInventoryTransactions' */
    @Query(value = "SELECT distinct it.WarehouseId as Id, w.WareHouseName "
         + "FROM InventoryTransactions it "
         + "inner join Item i on it.ItemId = i.Id "
         + "inner join ItemType itp on i.ItemTypeId = itp.Id "
         + "inner join InvWareHouse w on it.WarehouseId = w.Id "
         + "Where it.OrganizationId = :organizationId "
         + "and it.CompanyId = :companyId "
         + "and (CAST(w.WarehouseType AS INT) IN (2,3)) "
         + "and (:parentIds IS NULL OR itp.Type in (select data from dbo.fnSplitString (:parentIds,',')))", nativeQuery = true)
    List<StoreWarehouseRow> getWarehouseForStoreFromInventoryTransactions(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("parentIds") String parentIds);

    /** Sp_Item_GetAllMethod @Activity='DocumentTypeGetFromInventoryTransactionsForStore' */
    @Query("SELECT DISTINCT it.refDocumentTypeId AS refDocumentTypeId, d.documentTypeDescription AS documentTypeDescription "
         + "FROM InventoryTransactions it "
         + "JOIN DocumentType d ON it.refDocumentTypeId = d.id "
         + "JOIN Item i ON i.id = it.itemId "
         + "JOIN ItemType itp ON itp.id = i.itemTypeId AND itp.type IN (14,17) "
         + "WHERE it.organizationId = :organizationId AND it.companyId = :companyId")
    List<LookupDocumentTypeRow> documentTypeGetFromInventoryTransactionsForStore(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='ReadAllItemsWithPackingAndStore' */
    @Query(value = "SELECT distinct Item.Id, Item.ItemName "
         + "FROM Item "
         + "INNER JOIN vItemAllocation IOA on item.Id = IOA.Id "
         + "INNER JOIN ChartofAccount AS StockAc ON Item.PurchaseGLAC = StockAc.Id "
         + "INNER JOIN ChartofAccount AS SaleAc ON Item.SaleGLAC = SaleAc.Id "
         + "INNER JOIN ChartofAccount AS CGSAc ON Item.COGSGLAC = CGSAc.Id "
         + "INNER JOIN ItemCategory ON Item.ItemCategoryId = ItemCategory.Id "
         + "LEFT JOIN ItemType ON Item.ItemTypeId = ItemType.Id "
         + "LEFT JOIN InvLookUp l on ItemType.Type = l.Id "
         + "LEFT JOIN UOM ON Item.BaseUnitId = UOM.Id "
         + "where Item.OrganizationId = :organizationId "
         + "and IOA.CompanyId = :companyId "
         + "and ItemCategory.InventoryParentCategoriesId <> 5 "
         + "and IOA.IsActive = 1 "
         + "UNION ALL "
         + "SELECT p.Id, p.ItemName "
         + "FROM ItemPartyProcessing p "
         + "where p.OrganizationId = :organizationId "
         + "and p.CompanyId = :companyId", nativeQuery = true)
    List<ItemIdNameRow> readAllItemsWithPackingAndStore(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='ItemWiseCommissionScheduleHistory' */
    @Query(value = "select ISNULL(s.Id,0) Id, c.Id ItemCategoryId, c.CategoryDescription ItemCategory, "
         + "T.Id ItemTypeId, T.TypeDescription ItemType, i.Id ItemId, "
         + "i.ItemName, ISNULL(s.OnSale,0) OnSale, ISNULL(s.OnPurchase,0) OnPurchase, S.EffectedFromDate, "
         + "s.EntryDate, Ue.FirstName + ' ' + UE.LastName EntryUserName, "
         + "s.ModifyDate, UM.FirstName + ' ' + UM.LastName ModifyUserName "
         + "from Item i "
         + "INNER JOIN vItemAllocation IOA ON i.Id = IOA.Id "
         + "Inner join ItemCategory c on c.Id=i.ItemCategoryId and c.CompanyId=i.CompanyId "
         + "Inner join ItemType T on T.Id=i.ItemTypeId and T.CompanyId=i.CompanyId "
         + "Left join ItemWiseCommissionSchedule s on i.Id=s.ItemId and i.CompanyId=s.CompanyId "
         + "Left join UserAccount UE on UE.ID=s.EntryUserId and UE.CompanyId=s.CompanyId "
         + "Left join UserAccount UM on UM.ID=s.EntryUserId and UM.CompanyId=s.CompanyId "
         + "Where IOA.CompanyId=:companyId "
         + "and i.OrganizationId=:organizationId "
         + "and (:itemCategoryId is null or i.ItemCategoryId =:itemCategoryId) "
         + "and (:itemTypeId is null or i.ItemTypeId =:itemTypeId) "
         + "and (:inventoryParentCategoriesId is null or c.InventoryParentCategoriesId =:inventoryParentCategoriesId)", nativeQuery = true)
    List<ItemCommissionHistoryRow> itemWiseCommissionScheduleHistory(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("inventoryParentCategoriesId") Integer inventoryParentCategoriesId);

    /** Sp_Item_GetAllMethod @Activity='ItemCommissionScheduleByItemId' */
    @Query(value = "Select top 1 OnSale, OnPurchase "
         + "from ItemWiseCommissionSchedule "
         + "Where OrgnizationId=:organizationId "
         + "and CompanyId=:companyId "
         + "and ItemId=:itemId", nativeQuery = true)
    List<ItemCommissionRow> itemCommissionScheduleByItemId(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("itemId") Integer itemId);

    /** Sp_Item_GetAllMethod @Activity='GetItemsWithVarientOrWithoutVarient' */
    @Query(value = "SELECT i.Id, i.ItemName "
         + "FROM Item i "
         + "INNER JOIN vItemAllocation IOA ON i.Id = IOA.Id "
         + "WHERE i.OrganizationId=:organizationId "
         + "and IOA.CompanyId=:companyId "
         + "and (:itemWithVarient is null or i.ItemWithVarient = :itemWithVarient) "
         + "and ISNULL(IOA.IsActive,0) = 1", nativeQuery = true)
    List<ItemIdNameRow> getItemsWithVarientOrWithoutVarient(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("itemWithVarient") Boolean itemWithVarient);

    /** Sp_Item_GetAllMethod @Activity='GetPMItemOnly' */
    @Query(value = "select i.Id, i.ItemName, i.ItemCodeNew ItemCode, i.LeadTimeDay, CONCAT(i.PackSize, Case when i.PackSizeId =1 then 'KG' else '' End) WeightCapacity "
         + "from vItemAllocation AI "
         + "inner join Item i on AI.Id = i.Id "
         + "left join ItemType t on i.ItemTypeId = t.Id "
         + "left join InvLookUp l on t.Type = l.Id "
         + "where i.OrganizationId = :organizationId "
         + "and AI.CompanyId = :companyId "
         + "and (t.Type =14)", nativeQuery = true)
    List<ItemPmRow> getPMItemOnly(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_Item_GetAllMethod @Activity='GetLeadTimeBySupplierAndItem' */
    @Query(value = "select AI.LeadTimeDay "
         + "from ItemAllocationToSupplierCustomer AI "
         + "INNER JOIN Item i on AI.ItemId = i.Id "
         + "INNER join ItemType t on i.ItemTypeId = t.Id "
         + "where AI.OrganizationId = :organizationId "
         + "and AI.CompanyId = :companyId "
         + "and (t.Type =14) "
         + "and AI.SupplierCustomerId=:supplierCustomerId "
         + "and AI.ItemId=:itemId", nativeQuery = true)
    List<LeadTimeRow> getLeadTimeBySupplierAndItem(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("supplierCustomerId") Integer supplierCustomerId,
            @Param("itemId") Integer itemId);


    // ===================== Sp_Item_Insert helpers (pre-insert logic, see class Javadoc) =====================

    /** Sp_Item_Insert: @Id = ISNULL(MAX(CONVERT(int,Id)),0)+1 (app-assigned, whole table). */
    @Query("select coalesce(max(e.id),0) from Item e")
    Integer maxId();

    /** Sp_Item_Insert / GenerateItemCodeByCategoryId: next numeric ItemCode within the category (ISNULL(MAX(CONVERT(int,ItemCode)),0)+1; when it yields 1 the proc uses ItemCategory.SerialFrom instead). */
    @Query(value = "SELECT ISNULL(MAX(CONVERT(int, Item.ItemCode)), 0) + 1 FROM Item "
         + "WHERE ITEM.ItemCategoryId = :itemCategoryId AND ITEM.OrganizationId = :organizationId AND ITEM.CompanyId = :companyId", nativeQuery = true)
    Integer nextItemCodeInCategory(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("itemCategoryId") Integer itemCategoryId);

    /** Sp_Item_Insert: SerialFrom of the item's category (fallback ItemCode when the category has no items yet). */
    @Query("SELECT c.serialFrom FROM ItemCategory c WHERE c.organizationId = :organizationId AND c.companyId = :companyId AND c.id = :itemCategoryId")
    Integer findSerialFromByCategory(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("itemCategoryId") Integer itemCategoryId);

    /** Sp_Item_Insert: &gt;0 means '&lt;ItemName&gt; already define in table please check.' */
    @Query("SELECT COUNT(i) FROM Item i WHERE i.organizationId = :organizationId AND i.companyId = :companyId AND i.itemName = :itemName")
    Long countByItemName(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("itemName") String itemName);

    /** Sp_Item_Insert: &gt;0 means '&lt;ItemCodeNew&gt; already define in table please check.' (within the category). */
    @Query("SELECT COUNT(i) FROM Item i WHERE i.organizationId = :organizationId AND i.companyId = :companyId "
         + "AND i.itemCategoryId = :itemCategoryId AND i.itemCodeNew = :itemCodeNew")
    Long countByItemCodeNewInCategory(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("itemCategoryId") Integer itemCategoryId, @Param("itemCodeNew") String itemCodeNew);

    /** Sp_Item_Insert / Sp_Item_Update: of the three GL accounts, those that ARE 'Detail' accounts; any missing one triggers
     *  '&lt;name&gt; must be a transactional (Detail) account. Control accounts cannot be assigned.' (checked only when all three &gt; 0). */
    @Query("SELECT c.id FROM ChartofAccount c WHERE c.id IN (:purchaseGLAC, :saleGLAC, :cogsglac) AND c.accountGroup = 'Detail'")
    List<Integer> findDetailAccountIds(@Param("purchaseGLAC") Integer purchaseGLAC, @Param("saleGLAC") Integer saleGLAC, @Param("cogsglac") Integer cogsglac);

    /** Sp_Item_Insert / Sp_Item_Update: @UserLogId defaults to UserAccount.LastUserLogId of @EntryUser (insert) / @ModifyUser (update). */
    @Query(value = "SELECT LastUserLogId FROM UserAccount WHERE ID = :userId", nativeQuery = true)
    Long lastUserLogIdByUserId(@Param("userId") Integer userId);

    /** Sp_Item_Insert (single branch) - the whole procedure, behaviour preserved.
     *  // KEEP-EXEC: @Id/@ItemCode/@ItemCodeNew/@BarcodeNo/@UserLogId are recomputed inside (max+1, category serial, '1'+org+comp+NEWID barcode), RAISERROR validations, then INSERT Item, EXEC item.USP_ItemAttribute_InsertAndUpdateByCategory (feature 13) and EXEC dbo.USP_ItemAllocationToStoreRack_InsertAndUpdate when @RackId > 0; ends with SELECT @Id (discarded by @Modifying - use maxId() before or save()).
     *  Defaults in the proc: every parameter defaults to NULL unless noted; callers pass values explicitly.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_Item_Insert @Id = :id, @ItemCode = :itemCode, @ItemName = :itemName, @BaseUnitId = :baseUnitId, "
         + "@ItemCategoryId = :itemCategoryId, @ItemTypeId = :itemTypeId, @ItemClassId = :itemClassId, "
         + "@PurchaseGLAC = :purchaseGLAC, @SaleGLAC = :saleGLAC, @COGSGLAC = :cogsglac, @HSCode = :hsCode, "
         + "@PackQty = :packQty, @ItemStatus = :itemStatus, @ManufactureId = :manufactureId, "
         + "@ManufacturePartNo = :manufacturePartNo, @BuyerSupplierId = :buyerSupplierId, @BuyerPartNo = :buyerPartNo, "
         + "@ProductNo = :productNo, @MinStockLevel = :minStockLevel, @MaxStockLevel = :maxStockLevel, "
         + "@OptimalStockLevel = :optimalStockLevel, @ReorderLevel = :reorderLevel, @CostPrice = :costPrice, "
         + "@UOMScheduleIdCostRate = :uomScheduleIdCostRate, @PurchasePrice = :purchasePrice, "
         + "@UOMScheduleIdPurRate = :uomScheduleIdPurRate, @RetailPrice = :retailPrice, "
         + "@UOMScheduleIdRetailRate = :uomScheduleIdRetailRate, @WholeSalePrice = :wholeSalePrice, "
         + "@UOMScheduleIdWhsRate = :uomScheduleIdWhsRate, @AmountCalcType = :amountCalcType, "
         + "@ProductType = :productType, @BaseCropYear = :baseCropYear, @ApplyGST = :applyGST, @ApplyVAT = :applyVAT, "
         + "@ApplyExciese = :applyExciese, @OtherTax = :otherTax, @BarcodeImage = :barcodeImage, @BarcodeNo = :barcodeNo, "
         + "@EntryDate = :entryDate, @EntryUser = :entryUser, @ModifyDate = :modifyDate, @ModifyUser = :modifyUser, "
         + "@PostDate = :postDate, @PostUser = :postUser, @PostState = :postState, @OrganizationId = :organizationId, "
         + "@CompanyId = :companyId, @BranchesId = :branchesId, @ProjectsId = :projectsId, "
         + "@ItemAliasName = :itemAliasName, @UomLookUpId = :uomLookUpId, @ItemSpecification = :itemSpecification, "
         + "@ItemCodeNew = :itemCodeNew, @MasterItemId = :masterItemId, @ItemOriginId = :itemOriginId, "
         + "@PurchaseTypeLookUpId = :purchaseTypeLookUpId, @IsExpirable = :isExpirable, @ShelfLife = :shelfLife, "
         + "@IsDiscountable = :isDiscountable, @SaleTaxPurPercent = :saleTaxPurPercent, "
         + "@AtRetailPriceTax = :atRetailPriceTax, @AtPurcahsePriceTax = :atPurcahsePriceTax, "
         + "@SaleTaxSalesPercent = :saleTaxSalesPercent, @IsTaxable = :isTaxable, "
         + "@accumulatedDepreciationAcId = :accumulatedDepreciationAcId, "
         + "@DepreciationExpenseAcId = :depreciationExpenseAcId, @CapitalWipAcId = :capitalWipAcId, "
         + "@CropYearId = :cropYearId, @ItemQcGradeId = :itemQcGradeId, @MaxMeaurementUnitId = :maxMeaurementUnitId, "
         + "@WeightKgs = :weightKgs, @MaxMeaurement = :maxMeaurement, @ModelName = :modelName, "
         + "@ItemWithVarient = :itemWithVarient, @ItemNameOtherLingo = :itemNameOtherLingo, @IsCompany = :isCompany, "
         + "@IsThirdParty = :isThirdParty, @MinRate = :minRate, @MaxRate = :maxRate, @EffectedDate = :effectedDate, "
         + "@ReOrderQty = :reOrderQty, @IsImport = :isImport, @LeadTimeDay = :leadTimeDay, "
         + "@AllowMultiUom = :allowMultiUom, @PackSizeId = :packSizeId, @PackSize = :packSize, "
         + "@MotherItemId = :motherItemId, @itemModalId = :itemModalId, @WeightSemiFinish = :weightSemiFinish, "
         + "@WeightFinishGoods = :weightFinishGoods, @WeightSemiFinishSand = :weightSemiFinishSand, "
         + "@ExpenseMaintenanceAccountId = :expenseMaintenanceAccountId, @EmptyBagWeight = :emptyBagWeight, "
         + "@ServicesMasterItemId = :servicesMasterItemId, @RackId = :rackId, @UserLogId = :userLogId", nativeQuery = true)
    void spItemInsert(
            @Param("id") Integer id,
            @Param("itemCode") String itemCode,
            @Param("itemName") String itemName,
            @Param("baseUnitId") Integer baseUnitId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("itemClassId") Integer itemClassId,
            @Param("purchaseGLAC") Integer purchaseGLAC,
            @Param("saleGLAC") Integer saleGLAC,
            @Param("cogsglac") Integer cogsglac,
            @Param("hsCode") String hsCode,
            @Param("packQty") Double packQty,
            @Param("itemStatus") Boolean itemStatus,
            @Param("manufactureId") Integer manufactureId,
            @Param("manufacturePartNo") String manufacturePartNo,
            @Param("buyerSupplierId") Integer buyerSupplierId,
            @Param("buyerPartNo") String buyerPartNo,
            @Param("productNo") String productNo,
            @Param("minStockLevel") Double minStockLevel,
            @Param("maxStockLevel") Double maxStockLevel,
            @Param("optimalStockLevel") Double optimalStockLevel,
            @Param("reorderLevel") Double reorderLevel,
            @Param("costPrice") Double costPrice,
            @Param("uomScheduleIdCostRate") Integer uomScheduleIdCostRate,
            @Param("purchasePrice") Double purchasePrice,
            @Param("uomScheduleIdPurRate") Integer uomScheduleIdPurRate,
            @Param("retailPrice") Double retailPrice,
            @Param("uomScheduleIdRetailRate") Integer uomScheduleIdRetailRate,
            @Param("wholeSalePrice") Double wholeSalePrice,
            @Param("uomScheduleIdWhsRate") Integer uomScheduleIdWhsRate,
            @Param("amountCalcType") String amountCalcType,
            @Param("productType") String productType,
            @Param("baseCropYear") String baseCropYear,
            @Param("applyGST") Boolean applyGST,
            @Param("applyVAT") Boolean applyVAT,
            @Param("applyExciese") Boolean applyExciese,
            @Param("otherTax") Boolean otherTax,
            @Param("barcodeImage") String barcodeImage,
            @Param("barcodeNo") String barcodeNo,
            @Param("entryDate") LocalDateTime entryDate,
            @Param("entryUser") Integer entryUser,
            @Param("modifyDate") LocalDateTime modifyDate,
            @Param("modifyUser") Integer modifyUser,
            @Param("postDate") LocalDateTime postDate,
            @Param("postUser") Integer postUser,
            @Param("postState") Boolean postState,
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("projectsId") Integer projectsId,
            @Param("itemAliasName") String itemAliasName,
            @Param("uomLookUpId") Integer uomLookUpId,
            @Param("itemSpecification") String itemSpecification,
            @Param("itemCodeNew") String itemCodeNew,
            @Param("masterItemId") Integer masterItemId,
            @Param("itemOriginId") Integer itemOriginId,
            @Param("purchaseTypeLookUpId") Integer purchaseTypeLookUpId,
            @Param("isExpirable") Boolean isExpirable,
            @Param("shelfLife") Integer shelfLife,
            @Param("isDiscountable") Boolean isDiscountable,
            @Param("saleTaxPurPercent") Double saleTaxPurPercent,
            @Param("atRetailPriceTax") Boolean atRetailPriceTax,
            @Param("atPurcahsePriceTax") Boolean atPurcahsePriceTax,
            @Param("saleTaxSalesPercent") Double saleTaxSalesPercent,
            @Param("isTaxable") Boolean isTaxable,
            @Param("accumulatedDepreciationAcId") Integer accumulatedDepreciationAcId,
            @Param("depreciationExpenseAcId") Integer depreciationExpenseAcId,
            @Param("capitalWipAcId") Integer capitalWipAcId,
            @Param("cropYearId") Integer cropYearId,
            @Param("itemQcGradeId") Integer itemQcGradeId,
            @Param("maxMeaurementUnitId") Integer maxMeaurementUnitId,
            @Param("weightKgs") BigDecimal weightKgs,
            @Param("maxMeaurement") BigDecimal maxMeaurement,
            @Param("modelName") String modelName,
            @Param("itemWithVarient") Boolean itemWithVarient,
            @Param("itemNameOtherLingo") String itemNameOtherLingo,
            @Param("isCompany") Boolean isCompany,
            @Param("isThirdParty") Boolean isThirdParty,
            @Param("minRate") Double minRate,
            @Param("maxRate") Double maxRate,
            @Param("effectedDate") LocalDateTime effectedDate,
            @Param("reOrderQty") Double reOrderQty,
            @Param("isImport") Boolean isImport,
            @Param("leadTimeDay") Integer leadTimeDay,
            @Param("allowMultiUom") Boolean allowMultiUom,
            @Param("packSizeId") Integer packSizeId,
            @Param("packSize") Double packSize,
            @Param("motherItemId") Integer motherItemId,
            @Param("itemModalId") Integer itemModalId,
            @Param("weightSemiFinish") Double weightSemiFinish,
            @Param("weightFinishGoods") Double weightFinishGoods,
            @Param("weightSemiFinishSand") Double weightSemiFinishSand,
            @Param("expenseMaintenanceAccountId") Integer expenseMaintenanceAccountId,
            @Param("emptyBagWeight") Double emptyBagWeight,
            @Param("servicesMasterItemId") Integer servicesMasterItemId,
            @Param("rackId") Integer rackId,
            @Param("userLogId") Long userLogId);

    // ===================== Sp_Item_Update helpers =====================

    /** Sp_Item_Update: &gt;0 means '&lt;ItemName&gt; already define in table please check.' (other items). */
    @Query("SELECT COUNT(i) FROM Item i WHERE i.organizationId = :organizationId AND i.companyId = :companyId AND i.itemName = :itemName AND i.id <> :id")
    Long countByItemNameExcludingId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("itemName") String itemName, @Param("id") Integer id);

    /** Sp_Item_Update: &gt;0 means '&lt;ItemCodeNew&gt; already define in table please check.' (other items, any category). */
    @Query("SELECT COUNT(i) FROM Item i WHERE i.organizationId = :organizationId AND i.companyId = :companyId AND i.itemCodeNew = :itemCodeNew AND i.id <> :id")
    Long countByItemCodeNewExcludingId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("itemCodeNew") String itemCodeNew, @Param("id") Integer id);

    /** Sp_Item_Update: current BarcodeNo of the item (@PreBarCode). */
    @Query("SELECT i.barcodeNo FROM Item i WHERE i.organizationId = :organizationId AND i.companyId = :companyId AND i.id = :id")
    String findBarcodeNoById(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("id") Integer id);

    /** Sp_Item_Update: &gt;0 means 'BarcodeNo already exist in table please check' (checked when the barcode changed). */
    @Query("SELECT COUNT(i) FROM Item i WHERE i.organizationId = :organizationId AND i.companyId = :companyId AND i.barcodeNo = :barcodeNo")
    Long countByBarcodeNo(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("barcodeNo") String barcodeNo);

    /** Sp_Item_Update: the standalone UPDATE dbo.Item SET ... WHERE Id=@Id with every SET column of the proc (EntryDate/EntryUser untouched).
     *  Native because ItemCategoryId/ItemTypeId/RackId are @ManyToOne relations in the existing entity. */
    @Modifying
    @Transactional
    @Query(value = "UPDATE dbo.Item SET [ItemCode] = :itemCode, [ItemName] = :itemName, [BaseUnitId] = :baseUnitId, [ItemCategoryId] = :itemCategoryId, [ItemTypeId] = :itemTypeId, [ItemClassId] = :itemClassId, [PurchaseGLAC] = :purchaseGLAC, [SaleGLAC] = :saleGLAC, [COGSGLAC] = :cogsglac, [HSCode] = :hsCode, [PackQty] = :packQty, [ItemStatus] = :itemStatus, [ManufactureId] = :manufactureId, [ManufacturePartNo] = :manufacturePartNo, [BuyerSupplierId] = :buyerSupplierId, [BuyerPartNo] = :buyerPartNo, [ProductNo] = :productNo, [MinStockLevel] = :minStockLevel, [MaxStockLevel] = :maxStockLevel, [OptimalStockLevel] = :optimalStockLevel, [ReorderLevel] = :reorderLevel, [CostPrice] = :costPrice, [UOMScheduleIdCostRate] = :uomScheduleIdCostRate, [PurchasePrice] = :purchasePrice, [UOMScheduleIdPurRate] = :uomScheduleIdPurRate, [RetailPrice] = :retailPrice, [UOMScheduleIdRetailRate] = :uomScheduleIdRetailRate, [WholeSalePrice] = :wholeSalePrice, [UOMScheduleIdWhsRate] = :uomScheduleIdWhsRate, [AmountCalcType] = :amountCalcType, [ProductType] = :productType, [BaseCropYear] = :baseCropYear, [ApplyGST] = :applyGST, [ApplyVAT] = :applyVAT, [ApplyExciese] = :applyExciese, [OtherTax] = :otherTax, [BarcodeImage] = :barcodeImage, [BarcodeNo] = :barcodeNo, [ModifyDate] = :modifyDate, [ModifyUser] = :modifyUser, [PostDate] = :postDate, [PostUser] = :postUser, [PostState] = :postState, [OrganizationId] = :organizationId, [CompanyId] = :companyId, [BranchesId] = :branchesId, [ProjectsId] = :projectsId, [ItemAliasName] = :itemAliasName, [UomLookUpId] = :uomLookUpId, [ItemSpecification] = :itemSpecification, [MasterItemId] = :masterItemId, [ItemOriginId] = :itemOriginId, [PurchaseTypeLookUpId] = :purchaseTypeLookUpId, [IsExpirable] = :isExpirable, [ShelfLife] = :shelfLife, [IsDiscountable] = :isDiscountable, [SaleTaxPurPercent] = :saleTaxPurPercent, [AtRetailPriceTax] = :atRetailPriceTax, [AtPurcahsePriceTax] = :atPurcahsePriceTax, [SaleTaxSalesPercent] = :saleTaxSalesPercent, [IsTaxable] = :isTaxable, [accumulatedDepreciationAcId] = :accumulatedDepreciationAcId, [DepreciationExpenseAcId] = :depreciationExpenseAcId, [ItemCodeNew] = :itemCodeNew, [CapitalWipAcId] = :capitalWipAcId, [CropYearId] = :cropYearId, [ItemQcGradeId] = :itemQcGradeId, [MaxMeaurementUnitId] = :maxMeaurementUnitId, [WeightKgs] = :weightKgs, [MaxMeaurement] = :maxMeaurement, [ModelName] = :modelName, [ItemWithVarient] = :itemWithVarient, [ItemNameOtherLingo] = :itemNameOtherLingo, [IsCompany] = :isCompany, [IsThirdParty] = :isThirdParty, [MinRate] = :minRate, [MaxRate] = :maxRate, [EffectedDate] = :effectedDate, [ReOrderQty] = :reOrderQty, [IsImport] = :isImport, [LeadTimeDay] = :leadTimeDay, [AllowMultiUom] = :allowMultiUom, [PackSizeId] = :packSizeId, [PackSize] = :packSize, [MotherItemId] = :motherItemId, [itemModalId] = :itemModalId, [WeightSemiFinish] = :weightSemiFinish, [WeightFinishGoods] = :weightFinishGoods, [WeightSemiFinishSand] = :weightSemiFinishSand, [ExpenseMaintenanceAccountId] = :expenseMaintenanceAccountId, [EmptyBagWeight] = :emptyBagWeight, [ServicesMasterItemId] = :servicesMasterItemId, [RackId] = :rackId, [UserLogId] = :userLogId WHERE [Id] = :id", nativeQuery = true)
    int updateById(@Param("itemCode") String itemCode,
            @Param("itemName") String itemName,
            @Param("baseUnitId") Integer baseUnitId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("itemClassId") Integer itemClassId,
            @Param("purchaseGLAC") Integer purchaseGLAC,
            @Param("saleGLAC") Integer saleGLAC,
            @Param("cogsglac") Integer cogsglac,
            @Param("hsCode") String hsCode,
            @Param("packQty") Double packQty,
            @Param("itemStatus") Boolean itemStatus,
            @Param("manufactureId") Integer manufactureId,
            @Param("manufacturePartNo") String manufacturePartNo,
            @Param("buyerSupplierId") Integer buyerSupplierId,
            @Param("buyerPartNo") String buyerPartNo,
            @Param("productNo") String productNo,
            @Param("minStockLevel") Double minStockLevel,
            @Param("maxStockLevel") Double maxStockLevel,
            @Param("optimalStockLevel") Double optimalStockLevel,
            @Param("reorderLevel") Double reorderLevel,
            @Param("costPrice") Double costPrice,
            @Param("uomScheduleIdCostRate") Integer uomScheduleIdCostRate,
            @Param("purchasePrice") Double purchasePrice,
            @Param("uomScheduleIdPurRate") Integer uomScheduleIdPurRate,
            @Param("retailPrice") Double retailPrice,
            @Param("uomScheduleIdRetailRate") Integer uomScheduleIdRetailRate,
            @Param("wholeSalePrice") Double wholeSalePrice,
            @Param("uomScheduleIdWhsRate") Integer uomScheduleIdWhsRate,
            @Param("amountCalcType") String amountCalcType,
            @Param("productType") String productType,
            @Param("baseCropYear") String baseCropYear,
            @Param("applyGST") Boolean applyGST,
            @Param("applyVAT") Boolean applyVAT,
            @Param("applyExciese") Boolean applyExciese,
            @Param("otherTax") Boolean otherTax,
            @Param("barcodeImage") String barcodeImage,
            @Param("barcodeNo") String barcodeNo,
            @Param("modifyDate") LocalDateTime modifyDate,
            @Param("modifyUser") Integer modifyUser,
            @Param("postDate") LocalDateTime postDate,
            @Param("postUser") Integer postUser,
            @Param("postState") Boolean postState,
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("projectsId") Integer projectsId,
            @Param("itemAliasName") String itemAliasName,
            @Param("uomLookUpId") Integer uomLookUpId,
            @Param("itemSpecification") String itemSpecification,
            @Param("masterItemId") Integer masterItemId,
            @Param("itemOriginId") Integer itemOriginId,
            @Param("purchaseTypeLookUpId") Integer purchaseTypeLookUpId,
            @Param("isExpirable") Boolean isExpirable,
            @Param("shelfLife") Integer shelfLife,
            @Param("isDiscountable") Boolean isDiscountable,
            @Param("saleTaxPurPercent") Double saleTaxPurPercent,
            @Param("atRetailPriceTax") Boolean atRetailPriceTax,
            @Param("atPurcahsePriceTax") Boolean atPurcahsePriceTax,
            @Param("saleTaxSalesPercent") Double saleTaxSalesPercent,
            @Param("isTaxable") Boolean isTaxable,
            @Param("accumulatedDepreciationAcId") Integer accumulatedDepreciationAcId,
            @Param("depreciationExpenseAcId") Integer depreciationExpenseAcId,
            @Param("itemCodeNew") String itemCodeNew,
            @Param("capitalWipAcId") Integer capitalWipAcId,
            @Param("cropYearId") Integer cropYearId,
            @Param("itemQcGradeId") Integer itemQcGradeId,
            @Param("maxMeaurementUnitId") Integer maxMeaurementUnitId,
            @Param("weightKgs") BigDecimal weightKgs,
            @Param("maxMeaurement") BigDecimal maxMeaurement,
            @Param("modelName") String modelName,
            @Param("itemWithVarient") Boolean itemWithVarient,
            @Param("itemNameOtherLingo") String itemNameOtherLingo,
            @Param("isCompany") Boolean isCompany,
            @Param("isThirdParty") Boolean isThirdParty,
            @Param("minRate") Double minRate,
            @Param("maxRate") Double maxRate,
            @Param("effectedDate") LocalDateTime effectedDate,
            @Param("reOrderQty") Double reOrderQty,
            @Param("isImport") Boolean isImport,
            @Param("leadTimeDay") Integer leadTimeDay,
            @Param("allowMultiUom") Boolean allowMultiUom,
            @Param("packSizeId") Integer packSizeId,
            @Param("packSize") Double packSize,
            @Param("motherItemId") Integer motherItemId,
            @Param("itemModalId") Integer itemModalId,
            @Param("weightSemiFinish") Double weightSemiFinish,
            @Param("weightFinishGoods") Double weightFinishGoods,
            @Param("weightSemiFinishSand") Double weightSemiFinishSand,
            @Param("expenseMaintenanceAccountId") Integer expenseMaintenanceAccountId,
            @Param("emptyBagWeight") Double emptyBagWeight,
            @Param("servicesMasterItemId") Integer servicesMasterItemId,
            @Param("rackId") Integer rackId,
            @Param("userLogId") Long userLogId,
            @Param("id") Integer id);

    /** Sp_Item_Update: the standalone DELETE FROM ItemImage WHERE ItemId=@Id (ItemImage entity planned by C2_item_schedules). */
    @Modifying
    @Transactional
    @Query("DELETE FROM ItemImage im WHERE im.itemId = :itemId")
    int deleteItemImagesByItemId(@Param("itemId") Integer itemId);

    /** Sp_Item_Update (single branch) - the whole procedure, behaviour preserved.
     *  // KEEP-EXEC: barcode default/uniqueness check, Detail-account check, duplicate name/code checks (RAISERROR), UPDATE Item, DELETE ItemImage, EXEC item.USP_ItemAttribute_InsertAndUpdateByCategory (feature 13), EXEC dbo.USP_ItemAllocationToStoreRack_InsertAndUpdate when @RackId > 0; ends with SELECT 0.
     *  Defaults in the proc: every parameter defaults to NULL unless noted; callers pass values explicitly.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_Item_Update @Id = :id, @ItemCode = :itemCode, @ItemName = :itemName, @BaseUnitId = :baseUnitId, "
         + "@ItemCategoryId = :itemCategoryId, @ItemTypeId = :itemTypeId, @ItemClassId = :itemClassId, "
         + "@PurchaseGLAC = :purchaseGLAC, @SaleGLAC = :saleGLAC, @COGSGLAC = :cogsglac, @HSCode = :hsCode, "
         + "@PackQty = :packQty, @ItemStatus = :itemStatus, @ManufactureId = :manufactureId, "
         + "@ManufacturePartNo = :manufacturePartNo, @BuyerSupplierId = :buyerSupplierId, @BuyerPartNo = :buyerPartNo, "
         + "@ProductNo = :productNo, @MinStockLevel = :minStockLevel, @MaxStockLevel = :maxStockLevel, "
         + "@OptimalStockLevel = :optimalStockLevel, @ReorderLevel = :reorderLevel, @CostPrice = :costPrice, "
         + "@UOMScheduleIdCostRate = :uomScheduleIdCostRate, @PurchasePrice = :purchasePrice, "
         + "@UOMScheduleIdPurRate = :uomScheduleIdPurRate, @RetailPrice = :retailPrice, "
         + "@UOMScheduleIdRetailRate = :uomScheduleIdRetailRate, @WholeSalePrice = :wholeSalePrice, "
         + "@UOMScheduleIdWhsRate = :uomScheduleIdWhsRate, @AmountCalcType = :amountCalcType, "
         + "@ProductType = :productType, @BaseCropYear = :baseCropYear, @ApplyGST = :applyGST, @ApplyVAT = :applyVAT, "
         + "@ApplyExciese = :applyExciese, @OtherTax = :otherTax, @BarcodeImage = :barcodeImage, @BarcodeNo = :barcodeNo, "
         + "@EntryDate = :entryDate, @EntryUser = :entryUser, @ModifyDate = :modifyDate, @ModifyUser = :modifyUser, "
         + "@PostDate = :postDate, @PostUser = :postUser, @PostState = :postState, @OrganizationId = :organizationId, "
         + "@CompanyId = :companyId, @BranchesId = :branchesId, @ProjectsId = :projectsId, "
         + "@ItemAliasName = :itemAliasName, @UomLookUpId = :uomLookUpId, @ItemSpecification = :itemSpecification, "
         + "@MasterItemId = :masterItemId, @ItemOriginId = :itemOriginId, @PurchaseTypeLookUpId = :purchaseTypeLookUpId, "
         + "@IsExpirable = :isExpirable, @ShelfLife = :shelfLife, @IsDiscountable = :isDiscountable, "
         + "@SaleTaxPurPercent = :saleTaxPurPercent, @AtRetailPriceTax = :atRetailPriceTax, "
         + "@AtPurcahsePriceTax = :atPurcahsePriceTax, @SaleTaxSalesPercent = :saleTaxSalesPercent, "
         + "@IsTaxable = :isTaxable, @accumulatedDepreciationAcId = :accumulatedDepreciationAcId, "
         + "@DepreciationExpenseAcId = :depreciationExpenseAcId, @ItemCodeNew = :itemCodeNew, "
         + "@CapitalWipAcId = :capitalWipAcId, @CropYearId = :cropYearId, @ItemQcGradeId = :itemQcGradeId, "
         + "@MaxMeaurementUnitId = :maxMeaurementUnitId, @WeightKgs = :weightKgs, @MaxMeaurement = :maxMeaurement, "
         + "@ModelName = :modelName, @ItemWithVarient = :itemWithVarient, @ItemNameOtherLingo = :itemNameOtherLingo, "
         + "@IsCompany = :isCompany, @IsThirdParty = :isThirdParty, @MinRate = :minRate, @MaxRate = :maxRate, "
         + "@EffectedDate = :effectedDate, @ReOrderQty = :reOrderQty, @IsImport = :isImport, @LeadTimeDay = :leadTimeDay, "
         + "@AllowMultiUom = :allowMultiUom, @PackSizeId = :packSizeId, @PackSize = :packSize, "
         + "@MotherItemId = :motherItemId, @itemModalId = :itemModalId, @WeightSemiFinish = :weightSemiFinish, "
         + "@WeightFinishGoods = :weightFinishGoods, @WeightSemiFinishSand = :weightSemiFinishSand, "
         + "@ExpenseMaintenanceAccountId = :expenseMaintenanceAccountId, @EmptyBagWeight = :emptyBagWeight, "
         + "@ServicesMasterItemId = :servicesMasterItemId, @RackId = :rackId, @UserLogId = :userLogId", nativeQuery = true)
    void spItemUpdate(
            @Param("id") Integer id,
            @Param("itemCode") String itemCode,
            @Param("itemName") String itemName,
            @Param("baseUnitId") Integer baseUnitId,
            @Param("itemCategoryId") Integer itemCategoryId,
            @Param("itemTypeId") Integer itemTypeId,
            @Param("itemClassId") Integer itemClassId,
            @Param("purchaseGLAC") Integer purchaseGLAC,
            @Param("saleGLAC") Integer saleGLAC,
            @Param("cogsglac") Integer cogsglac,
            @Param("hsCode") String hsCode,
            @Param("packQty") Double packQty,
            @Param("itemStatus") Boolean itemStatus,
            @Param("manufactureId") Integer manufactureId,
            @Param("manufacturePartNo") String manufacturePartNo,
            @Param("buyerSupplierId") Integer buyerSupplierId,
            @Param("buyerPartNo") String buyerPartNo,
            @Param("productNo") String productNo,
            @Param("minStockLevel") Double minStockLevel,
            @Param("maxStockLevel") Double maxStockLevel,
            @Param("optimalStockLevel") Double optimalStockLevel,
            @Param("reorderLevel") Double reorderLevel,
            @Param("costPrice") Double costPrice,
            @Param("uomScheduleIdCostRate") Integer uomScheduleIdCostRate,
            @Param("purchasePrice") Double purchasePrice,
            @Param("uomScheduleIdPurRate") Integer uomScheduleIdPurRate,
            @Param("retailPrice") Double retailPrice,
            @Param("uomScheduleIdRetailRate") Integer uomScheduleIdRetailRate,
            @Param("wholeSalePrice") Double wholeSalePrice,
            @Param("uomScheduleIdWhsRate") Integer uomScheduleIdWhsRate,
            @Param("amountCalcType") String amountCalcType,
            @Param("productType") String productType,
            @Param("baseCropYear") String baseCropYear,
            @Param("applyGST") Boolean applyGST,
            @Param("applyVAT") Boolean applyVAT,
            @Param("applyExciese") Boolean applyExciese,
            @Param("otherTax") Boolean otherTax,
            @Param("barcodeImage") String barcodeImage,
            @Param("barcodeNo") String barcodeNo,
            @Param("entryDate") LocalDateTime entryDate,
            @Param("entryUser") Integer entryUser,
            @Param("modifyDate") LocalDateTime modifyDate,
            @Param("modifyUser") Integer modifyUser,
            @Param("postDate") LocalDateTime postDate,
            @Param("postUser") Integer postUser,
            @Param("postState") Boolean postState,
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("projectsId") Integer projectsId,
            @Param("itemAliasName") String itemAliasName,
            @Param("uomLookUpId") Integer uomLookUpId,
            @Param("itemSpecification") String itemSpecification,
            @Param("masterItemId") Integer masterItemId,
            @Param("itemOriginId") Integer itemOriginId,
            @Param("purchaseTypeLookUpId") Integer purchaseTypeLookUpId,
            @Param("isExpirable") Boolean isExpirable,
            @Param("shelfLife") Integer shelfLife,
            @Param("isDiscountable") Boolean isDiscountable,
            @Param("saleTaxPurPercent") Double saleTaxPurPercent,
            @Param("atRetailPriceTax") Boolean atRetailPriceTax,
            @Param("atPurcahsePriceTax") Boolean atPurcahsePriceTax,
            @Param("saleTaxSalesPercent") Double saleTaxSalesPercent,
            @Param("isTaxable") Boolean isTaxable,
            @Param("accumulatedDepreciationAcId") Integer accumulatedDepreciationAcId,
            @Param("depreciationExpenseAcId") Integer depreciationExpenseAcId,
            @Param("itemCodeNew") String itemCodeNew,
            @Param("capitalWipAcId") Integer capitalWipAcId,
            @Param("cropYearId") Integer cropYearId,
            @Param("itemQcGradeId") Integer itemQcGradeId,
            @Param("maxMeaurementUnitId") Integer maxMeaurementUnitId,
            @Param("weightKgs") BigDecimal weightKgs,
            @Param("maxMeaurement") BigDecimal maxMeaurement,
            @Param("modelName") String modelName,
            @Param("itemWithVarient") Boolean itemWithVarient,
            @Param("itemNameOtherLingo") String itemNameOtherLingo,
            @Param("isCompany") Boolean isCompany,
            @Param("isThirdParty") Boolean isThirdParty,
            @Param("minRate") Double minRate,
            @Param("maxRate") Double maxRate,
            @Param("effectedDate") LocalDateTime effectedDate,
            @Param("reOrderQty") Double reOrderQty,
            @Param("isImport") Boolean isImport,
            @Param("leadTimeDay") Integer leadTimeDay,
            @Param("allowMultiUom") Boolean allowMultiUom,
            @Param("packSizeId") Integer packSizeId,
            @Param("packSize") Double packSize,
            @Param("motherItemId") Integer motherItemId,
            @Param("itemModalId") Integer itemModalId,
            @Param("weightSemiFinish") Double weightSemiFinish,
            @Param("weightFinishGoods") Double weightFinishGoods,
            @Param("weightSemiFinishSand") Double weightSemiFinishSand,
            @Param("expenseMaintenanceAccountId") Integer expenseMaintenanceAccountId,
            @Param("emptyBagWeight") Double emptyBagWeight,
            @Param("servicesMasterItemId") Integer servicesMasterItemId,
            @Param("rackId") Integer rackId,
            @Param("userLogId") Long userLogId);
}
