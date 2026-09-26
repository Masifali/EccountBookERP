package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 336 "Item Category Store" — the procedure calls of InvDeffrmItemCatagory.cs through
 * BLL 0584 / DAL 0437 (ItemCategory), BLL 0648 (COAAllocation.GetLst), BLL 0639
 * (ChartofAccount.ReadAllAccountgroup) and DAL 0205 (GetERPFeaturesByCompanyId).
 */
@Repository
public class ItemCategoryStoreRepository {

    public static final String P_GETALL = "Sp_ItemCategory_GetAllMethod";

    private final JdbcTemplate jdbc;

    public ItemCategoryStoreRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** BLL GetItemClassGroup → @Activity='GetItemClassGroup'. Rows: Id, ClassGroupName. */
    public List<Map<String, Object>> classGroups() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, P_GETALL, params("Activity", "GetItemClassGroup"))) {
            out.add(row("Id", toInt(r.get("Id")), "ClassGroupName", str(r.get("ClassGroupName"))));
        }
        return out;
    }

    /** BLL getItemProductionStage → usp_getItemProductionStage (no parameters). */
    public List<Map<String, Object>> productionStages() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "usp_getItemProductionStage", params())) {
            out.add(row("Id", toInt(r.get("Id")), "productionStageName", str(r.get("productionStageName"))));
        }
        return out;
    }

    /** BLL getItemVarietyNature → usp_getItemVarietyNature (no parameters). */
    public List<Map<String, Object>> varietyNatures() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "usp_getItemVarietyNature", params())) {
            out.add(row("Id", toInt(r.get("Id")), "VarietyNatureName", str(r.get("VarietyNatureName"))));
        }
        return out;
    }

    /**
     * BLL 0648 COAAllocation.GetLst: Sp_COAAllocation_GetAllMethod @OrganizationId, @CompanyId,
     * @Activity='COAAllocationSearch'; the BLL keeps AccountTitle, Id, AccountTypeId per row.
     */
    public List<Map<String, Object>> coaAllocationList(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "COAAllocationSearch"))) {
            out.add(row("Id", toInt(r.get("Id")), "AccountTitle", str(r.get("AccountTitle")),
                    "AccountTypeId", toInt(r.get("AccountTypeId"))));
        }
        return out;
    }

    /**
     * LoadThirdLevelAccounts:564 → BLL 0639 ChartofAccount.ReadAllAccountgroup:
     * Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId, @CompanyId, @FinancialYearId
     * (clsGlobalVariables.ActiveYr.Id), @Account_Level=3, @CoaType='ReadAllAccountGroup'
     * (LanguageId/AccountTypeId/AccountClassId are 0 and so not sent).
     */
    public List<Map<String, Object>> thirdLevelAccounts(UserAccount u, int financialYearId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_ChartofAccount_GetAllMethodFromCOA", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId,
                "Account_Level", 3,
                "CoaType", "ReadAllAccountGroup"))) {
            out.add(row("Id", toInt(r.get("Id")), "AccountTitle", str(r.get("AccountTitle")),
                    "AccountTypeId", toInt(r.get("AccountTypeId"))));
        }
        return out;
    }

    /**
     * BLL GenerateCode: [dbo].[Sp_ItemCategory_GetAllMethod] @OrganizationId, @CompanyId,
     * @InventoryParentCategoriesId (always sent, 0 included), @Activity='GenerateCode';
     * CategoryCode of the first row, "" when none.
     */
    public String generateCode(UserAccount u, int parentCategoryId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "[dbo].[Sp_ItemCategory_GetAllMethod]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "InventoryParentCategoriesId", parentCategoryId,
                "Activity", "GenerateCode"));
        return r.isEmpty() ? "" : str(r.get(0).get("CategoryCode"));
    }

    /**
     * BLL FormHistory(obj): @OrganizationId, @CompanyId, [@InventoryParentCategoriesId only when
     * non-zero — the form never sets it], @ParentCategoryIds = obj.CategoryCode (the form puts
     * ParentCategoryIds there), @Activity='FormHistory'.
     */
    public List<Map<String, Object>> formHistory(UserAccount u, String parentCategoryIds) {
        return DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ParentCategoryIds", (parentCategoryIds == null || parentCategoryIds.isEmpty()) ? null : parentCategoryIds,
                "Activity", "FormHistory"));
    }

    /** BLL GetByID: @Id, @Activity='ReadById' — no company filter in the procedure. */
    public Map<String, Object> readById(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /**
     * DAL 0437 SetData: GenericProvider.SetProc(model, "Sp_ItemCategory_Insert"/"_Update") —
     * Convert.ToInt32(ExecuteScalar()). Parameters are every non-virtual property of
     * Architecture.Model.Inventory.ItemCategory in declaration order; the caller builds them.
     */
    public int setProc(String proc, Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, proc, model);
    }

    /**
     * DAL 0437 SetData, feature-13 branch: [item].[USP_ItemAttribute_InsertAndUpdateByCategory]
     * @OrganizationId, @CompanyId, @ItemCategoryId = obj.Id, @ItemId = null (AddWithValue(null)
     * — omitted, the procedure default NULL applies). ExecuteNonQuery.
     */
    public void itemAttributesByCategory(UserAccount u, int categoryId) {
        DesktopProc.scalar(jdbc, "[item].[USP_ItemAttribute_InsertAndUpdateByCategory]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ItemCategoryId", categoryId,
                "ItemId", null));
    }

    static Map<String, Object> row(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }
}
