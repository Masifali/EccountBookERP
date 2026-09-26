package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Procedure calls of the two lookup dialogs Define_Department and frmLookUpDefineAsset, each with
 * the parameter list its BLL builds (order = BLL list order; SetProc = model property order).
 *
 * BLL / DAL / Model:
 *   0067_Architecture.BLL.Department.cs, 0061_Architecture.DAL.Department.cs, 0072_Architecture.Model.Department.cs;
 *   0223_Architecture.BLL.FixedAssets.FixedAssetsRegister.cs, 0193_Architecture.DAL.FixedAssets.FixedAssetsRegister.cs,
 *   0132_Architecture.Model.FixedAssets.FixedAssetsRegister.cs;
 *   0584_Architecture.BLL.Inventory.ItemCategory.cs (Getall);
 *   0583_Architecture.BLL.Inventory.Item.cs (ReadAllItemsByParentId) via WinApp CommonServices.ReadAllItemsByParentId:1594.
 * Every parameter checked against /root/ddl/procs.json (all of them have defaults except
 * @Activity on Sp_ItemCategory_GetAllMethod / Sp_Item_GetAllMethod, which is always sent).
 */
@Repository
public class StoreDefineLookupsRepository {

    private final JdbcTemplate jdbc;

    public StoreDefineLookupsRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // =============================================================================== Department

    /** BLL Department.GetAll — Sp_Department_GetAllMethod @OrganizationId, @CompanyId, @Activity='ReadAll' → Id, DepartmentName. */
    public List<Map<String, Object>> departments(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Department_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** BLL Department.GetByID — Sp_Department_GetAllMethod @Id, @Activity='ReadById' (SELECT * WHERE Id). Null when no row. */
    public Map<String, Object> department(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Department_GetAllMethod",
                params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /**
     * BLL Department.Save → DAL SetData(obj, "Sp_Department_Insert" | "Sp_Department_Update"):
     * GenericProvider.SetProc sends every Model.Department property (model order):
     * Id, DepartmentName, EntryDate, EntryUser, ModifyDate, ModifyUser, PostDate, PostUser,
     * PostState, OrganizationId, CompanyId. ModifyUser is never set by the form → 0.
     * Returns Convert.ToInt32(ExecuteScalar()) — the new Id on insert, 0 on update (no SELECT).
     */
    public int saveDepartment(boolean insert, int id, String name, Timestamp now, int userId, UserAccount u) {
        return DesktopProc.setProc(jdbc, insert ? "Sp_Department_Insert" : "Sp_Department_Update", params(
                "Id", id,
                "DepartmentName", name,
                "EntryDate", now,
                "EntryUser", userId,
                "ModifyDate", now,
                "ModifyUser", 0,
                "PostDate", now,
                "PostUser", 0,
                "PostState", false,
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId()));
    }

    // ==================================================================================== Asset

    /**
     * ItemCategory.Getall(CategoryCode = "5") — Sp_ItemCategory_GetAllMethod @OrganizationId,
     * @CompanyId, @Ids='5' (CategoryCode goes to @Ids), @Activity='ReadByOrganizationCompanyId'.
     * InventoryParentCategoriesId is 0 on the model, so @InventoryParentCategoriesId is not sent.
     */
    public List<Map<String, Object>> assetCategories(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ItemCategory_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                        "Ids", "5", "Activity", "ReadByOrganizationCompanyId"));
    }

    /**
     * CommonServices.ReadAllItemsByParentId(5) → BLL Item.ReadAllItemsByParentId — Sp_Item_GetAllMethod
     * @OrganizationId, @CompanyId, @InventoryParentCategoriesId=5, @Activity='ReadAllItemsByParentId'
     * → Id, ItemName, ItemCategory, ItemCode.
     */
    public List<Map<String, Object>> assetItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                        "InventoryParentCategoriesId", 5, "Activity", "ReadAllItemsByParentId"));
    }

    /** BLL FixedAssetsRegister.GetById — Sp_FixedAssetsRegister_GetAllMethod @Id, @Activity='ReadById'. Null when no row. */
    public Map<String, Object> asset(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_FixedAssetsRegister_GetAllMethod",
                params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /**
     * BLL FixedAssetsRegister.GetHistory(ReportsParameters) — @OrganizationId, @CompanyId, then each of
     * @EntryFromDate / @EntryToDate / @ModifyFromDate / @ModifyToDate only when set, @CategoryId only
     * when != 0, @Activity='GetHistory'. A null here is omitted (DesktopProc), which is exactly the
     * BLL's "if (!CheckDateTimeNull(..))" / "if (ItemCategoryId != 0)".
     */
    public List<Map<String, Object>> assetHistory(UserAccount u, Timestamp entryFrom, Timestamp entryTo,
                                                  Timestamp modifyFrom, Timestamp modifyTo, int categoryId) {
        return DesktopProc.rows(jdbc, "Sp_FixedAssetsRegister_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "EntryFromDate", entryFrom,
                "EntryToDate", entryTo,
                "ModifyFromDate", modifyFrom,
                "ModifyToDate", modifyTo,
                "CategoryId", categoryId != 0 ? categoryId : null,
                "Activity", "GetHistory"));
    }

    /**
     * BLL FixedAssetsRegister.Save → DAL SetData(obj, "Sp_FixedAssetsRegister_Insert" | "_Update").
     * SetProc sends every non-virtual Model.FixedAssets.FixedAssetsRegister property in model order.
     * Properties the form never sets: strings → null → NOT sent (proc default NULL); ints / doubles →
     * 0 / 0.0 (sent); bool → false.
     */
    public int saveAsset(boolean insert, Map<String, Object> m) {
        return DesktopProc.setProc(jdbc, insert ? "Sp_FixedAssetsRegister_Insert" : "Sp_FixedAssetsRegister_Update", params(
                "AssetsDepartmentId", m.get("AssetsDepartmentId"),
                "BranchesId", m.get("BranchesId"),
                "Vendor", null,
                "OrganizationId", m.get("OrganizationId"),
                "AssetCondition", m.get("AssetCondition"),
                "AssetDepriciationAcId", 0,
                "Manufacturer", null,
                "CompanyId", m.get("CompanyId"),
                "Pic1Path", null,
                "Pic2Path", null,
                "AssetSerialNo", null,
                "AssetName", m.get("AssetName"),
                "Brand", m.get("Brand"),
                "Id", m.get("Id"),
                "PurchasePrice", 0d,
                "AssetsType", "",
                "AssetLocationId", m.get("AssetLocationId"),
                "ItemId", m.get("ItemId"),
                "IsApproved", false,
                "EntryDate", m.get("Now"),
                "PurchaseDate", m.get("Now"),
                "CurrentValue", 0d,
                "ApprovedDate", m.get("Now"),
                "AssetStatus", null,
                "AssetGLAccountId", 0,
                "ExpenseMaintenanceAccountId", 0,
                "MakeDesc", null,
                "ExpiryDate", m.get("Now"),
                "EntryUserId", m.get("UserId"),
                "FixedAssetsCategoryId", m.get("FixedAssetsCategoryId"),
                "ProjectsId", m.get("BranchesId"),
                "ModelDesc", m.get("ModelDesc"),
                "ApprovedUserId", m.get("UserId"),
                "AssetAcmltvAcId", 0,
                "UseableLifeMonth", 0));
    }
}
