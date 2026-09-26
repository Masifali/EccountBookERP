package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Procedure calls of the two forms the Define Asset dialog (frmLookUpDefineAsset) opens with its
 * "+" buttons, that no other repository already makes:
 *
 *   (a) Architecture.WinApp.StoreManagement.frmItemCatagoryStore with FormTypeId = 3 ("Item Category (Fix Assets)")
 *   (b) Architecture.WinApp.FixedAsset.frmDefineAssets ("Add Fixed Assest Item")
 *
 * Calls REUSED from existing repositories (not duplicated here): ItemCategoryStoreRepository
 * (GetItemClassGroup, FormHistory, ReadById, SetProc Sp_ItemCategory_Insert|Update,
 * [item].[USP_ItemAttribute_InsertAndUpdateByCategory]), ItemStoreMasterSupport (Item.InventoryParentCategories,
 * GetERPFeatureById), StoreDefineLookupsRepository.assetCategories (ItemCategory.Getall CategoryCode "5"),
 * ItemPmWriter (BLL Item.Save → DAL 0436 Item.SetData).
 *
 * BLL / DAL / Model files read: 0584 BLL ItemCategory, 0437 DAL ItemCategory, 1046 Model ItemCategory,
 * 0740 Model AssetSchema.CategoryDepreciationSchedule, 0425 BLL / 0480 DAL / 0741 Model
 * AssetSchema.DepreciationMethodSchedule, 0014 BLL / 0008 DAL / 0008 Model AttributeVariant,
 * 0518 BLL / 0365 DAL / 0906 Model CategoryAttribute, 0583 BLL / 0436 DAL / 1044 Model Item,
 * 0997 Model ItemAllocation, 0593 BLL ItemType, 0609 BLL / 0462 DAL / 1074 Model UOM,
 * 0585 BLL / 0438 DAL / 1047 Model ItemClass, 0062 BLL / 0056 DAL / 0085 Model Company,
 * 0379 BLL Main.GlobalServicesMethods (GetGlobalAllAccountsWithCustomGroup);
 * WinApp: DatatableHelper.GetAccountsFromGlobalByTypeIds, CommonServices.CompanyServiceBind,
 * DropDownBind.BindDDL / BindDDLNew, InfragisticsHelper.BindAndRetainSelection.
 * Every parameter below was checked against /root/ddl/procs.json.
 */
@Repository
public class StoreDefineAssetsExtraRepository {

    private final JdbcTemplate jdbc;

    public StoreDefineAssetsExtraRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ============================================================================ shared lookups

    /**
     * DatatableHelper.GetAccountsFromGlobalByTypeIds(withTypeIds) over clsGlobalVariables.AllAccountsWithCustomGroupId,
     * which DatatableHelper.GlobalServicesDbCall("AccountsWithCustomGroupId") fills from
     * GlobalServicesMethods.GetGlobalAllAccountsWithCustomGroup(org, company, 0, 0, "") →
     * [dbo].[USP_GETAllAccountsFromCustomGroups] @OrganizationId, @CompanyId (PageSize / PageNumber 0 and
     * Keyword "" are not sent). Rows kept when AccountTypeId is in {@code typeIds}; distinct by
     * ChartOfAccountId, first row wins; columns Id, AccountTitle, AccountCode, ParentAccountTitle, AccountClass.
     */
    public List<Map<String, Object>> accountsByTypes(UserAccount u, int... typeIds) {
        Set<Integer> types = new HashSet<>();
        for (int t : typeIds) types.add(t);
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> a : DesktopProc.rows(jdbc, "[dbo].[USP_GETAllAccountsFromCustomGroups]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (!types.contains(toInt(ci(a, "AccountTypeId")))) continue;
            int id = toInt(ci(a, "ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("AccountTitle", str(ci(a, "AccountTitle")));
            o.put("AccountCode", str(ci(a, "AccountCode")));
            o.put("ParentAccountTitle", str(ci(a, "ParentAccountTitle")));
            o.put("AccountClass", str(ci(a, "AccountClassName")));
            out.add(o);
        }
        return out;
    }

    // ============================================================= (a) frmItemCatagoryStore FormTypeId 3

    /**
     * DepreciationMethodSchedule.GetActiveDepreciationMethodSchedule(org, company) (BLL 0425:86) —
     * Asset.USP_DepreciationMethodSchedule_GetAllMethod @OrganizationId, @CompanyId,
     * @Activity='GetActiveDepreciationMethodSchedule' → depreciationMethodScheduleId, DepreciatonMethodName.
     */
    public List<Map<String, Object>> depreciationMethods(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[Asset].[USP_DepreciationMethodSchedule_GetAllMethod]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                        "Activity", "GetActiveDepreciationMethodSchedule"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("depreciationMethodScheduleId", toInt(ci(r, "depreciationMethodScheduleId")));
            o.put("DepreciatonMethodName", str(ci(r, "DepreciatonMethodName")));
            out.add(o);
        }
        return out;
    }

    /**
     * AttributeVariant.Attribute_GetForItemCategory(org, company, categoryId) (BLL 0014:161) —
     * item.USP_Attribute_GetForItemCategory @OrganizationId, @CompanyId, @ItemCategoryId (all three
     * have no default and are always sent). Mapped as AttributeGridFill:249 maps them.
     */
    public List<Map<String, Object>> attributesForCategory(UserAccount u, int categoryId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[item].[USP_Attribute_GetForItemCategory]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemCategoryId", categoryId))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("CategoryAttributeId", toLong(ci(r, "CategoryAttributeId")));
            o.put("attributeId", toLong(ci(r, "attributeId")));
            o.put("Name", str(ci(r, "attributename")));
            o.put("Description", str(ci(r, "attributedescription")));
            o.put("Status", str(ci(r, "AllocationStatus")));
            o.put("StatusValue", toInt(ci(r, "StatusValue")));
            out.add(o);
        }
        return out;
    }

    /**
     * DAL 0437 SetData, feature-13 loop: GenericProvider.SetProc(categoryAttribute,
     * "item.USP_CategoryAttribute_InsertAndUpdate") — every non-virtual Model.Inventory.CategoryAttribute
     * property in model order: isActive, EntryDate, ModifyDate, companyId, EntryUserId, itemCategoryId,
     * ModifyUserId, organizationId, seqNo, attributeId, CategoryAttributeId.
     */
    public int saveCategoryAttribute(Map<String, Object> modelOrdered) {
        return DesktopProc.setProc(jdbc, "[item].[USP_CategoryAttribute_InsertAndUpdate]", modelOrdered);
    }

    /**
     * DAL 0437 SetData, schedule loop: GenericProvider.SetProc(categoryDepreciationSchedule,
     * "[Asset].[USP_CategoryDepreciationSchedule_Insert]") — Model 0740 order: isActive,
     * rateofDepreciation, categoryDepreciationScheduleId, categoryId, depreciationMethodScheduleId,
     * sortNo, usefullLifeInMonths.
     */
    public int saveCategoryDepreciationSchedule(Map<String, Object> modelOrdered) {
        return DesktopProc.setProc(jdbc, "[Asset].[USP_CategoryDepreciationSchedule_Insert]", modelOrdered);
    }

    // =================================================================== (b) frmDefineAssets

    /**
     * ItemType.Getall(ReportsParameters{TypeId = 29, ItemIds = "5"}) (BLL 0593:92) — Sp_ItemType_GetAllMethod
     * @OrganizationId, @CompanyId, @Type=29, @ParentCategoryIds='5', @Activity='ReadByOrganizationCompanyId'.
     * BindDDLNew keeps Id, TypeDescription.
     */
    public List<Map<String, Object>> itemTypes(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_ItemType_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Type", 29, "ParentCategoryIds", "5", "Activity", "ReadByOrganizationCompanyId"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("TypeDescription", str(ci(r, "TypeDescription")));
            out.add(o);
        }
        return out;
    }

    /**
     * UOM.Getall(UOM{org, company}) (BLL 0609:55) — Sp_UOM_GetAllMethod @OrganizationId, @CompanyId,
     * @Activity='ReadByOrganizationCompanyId'. BaseUnitFill:356 keeps Id, UomCode, Equivalent.
     */
    public List<Map<String, Object>> uoms(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_UOM_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "ReadByOrganizationCompanyId"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("UomCode", str(ci(r, "UOMCode")));
            o.put("Equivalent", str(ci(r, "Equivalent")));
            out.add(o);
        }
        return out;
    }

    /**
     * ItemClass.GetAll() (BLL 0585) — Sp_ItemClass_GetAllMethod @Activity='ReadAll' only (the form builds an
     * ItemClass with org / company and never passes it). ItemClassFill:502 keeps ClassId, ClassDescription.
     */
    public List<Map<String, Object>> itemClasses() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_ItemClass_GetAllMethod", params("Activity", "ReadAll"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("ClassId", toInt(ci(r, "ClassId")));
            o.put("ClassDescription", str(ci(r, "ClassDescription")));
            out.add(o);
        }
        return out;
    }

    /**
     * CommonServices.CompanyServiceBind() → Company.GetAlldt(Company{OrgCompanyTypeId = OrganizationId})
     * (BLL 0062:110) — Sp_Company_GetAllMethod @OrgCompanyTypeId, @Activity='ReadByOrganizationId'
     * (@Id not sent, it is 0). CompaniesBindInGrid:450 keeps Id, CompName → "Location", Value = true.
     */
    public List<Map<String, Object>> companies(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_Company_GetAllMethod", params(
                "OrgCompanyTypeId", u.getOrganizationId(), "Activity", "ReadByOrganizationId"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Location", str(ci(r, "CompName")));
            o.put("Value", true);
            out.add(o);
        }
        return out;
    }

    /**
     * Item.GenerateCode(Item{org, company, ItemCategoryId}) (BLL 0583:581) — Sp_Item_GetAllMethod
     * @OrganizationId, @CompanyId, @ItemCategoryId, @ItemTypeId (always sent; the form leaves it 0),
     * @Activity='GenerateItemCodeByCategoryId'. First row or null.
     */
    public Map<String, Object> generateItemCode(UserAccount u, int categoryId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemCategoryId", categoryId, "ItemTypeId", 0, "Activity", "GenerateItemCodeByCategoryId"));
        return r.isEmpty() ? null : r.get(0);
    }

    /**
     * Item.GetGLAccountbyItemCategoryIdForFixedAssets(Item{org, company, ItemCategoryId}) (BLL 0583:1191) —
     * Sp_Item_GetAllMethod @OrganizationId, @CompanyId, @Id = ItemCategoryId,
     * @Activity='GetGLAccountbyItemCategoryIdForFixedAssets'. First row or null.
     */
    public Map<String, Object> glAccountsByCategory(UserAccount u, int categoryId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", categoryId, "Activity", "GetGLAccountbyItemCategoryIdForFixedAssets"));
        return r.isEmpty() ? null : r.get(0);
    }

    /**
     * Item.FormHistoryForFixedAssets(ReportsParameters) (BLL 0583:1224) — Sp_Item_GetAllMethod
     * @OrganizationId, @CompanyId, [@ItemCategoryId only when obj.Id != 0 — the form never sets it],
     * @CanViewAllRecord (always), @EntryUser only when !CanViewAllRecord, @NoOfRecords only when != 0,
     * @Activity='FormHistoryForFixedAssets'. (obj.InventoryParentCategories = 5 is set by the form but
     * never sent by the BLL; the procedure hard-codes parent category 5.)
     */
    public List<Map<String, Object>> fixedAssetHistory(UserAccount u, boolean canViewAll, int noOfRecords) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "CanViewAllRecord", canViewAll,
                "EntryUser", canViewAll ? null : u.getId(),
                "NoOfRecords", noOfRecords != 0 ? noOfRecords : null,
                "Activity", "FormHistoryForFixedAssets"));
    }

    /** Item.GetByID (BLL 0583:49) — Sp_Item_GetAllMethod @Id, @Activity='ReadById' (SELECT * FROM Item). Null when no row. */
    public Map<String, Object> itemById(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** Sp_ItemCategory_GetAllMethod ReadById's InventoryParentCategoriesId for a category (tenancy of the item). */
    public Map<String, Object> categoryById(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ItemCategory_GetAllMethod", params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    static long toLong(Object v) {
        if (v == null) return 0L;
        if (v instanceof Number) return ((Number) v).longValue();
        try { return Long.parseLong(String.valueOf(v).trim()); } catch (Exception e) { return 0L; }
    }
}
