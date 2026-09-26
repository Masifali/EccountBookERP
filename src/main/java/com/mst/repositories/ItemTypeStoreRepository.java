package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 337 "Item Type Store" — the procedure calls of InvDeffrmItemType.cs through BLL 0593 /
 * DAL 0446 (ItemType) and BLL 0577 (InvLookUp.GetLookupsByTypeIdDt).
 */
@Repository
public class ItemTypeStoreRepository {

    public static final String P_GETALL = "Sp_ItemType_GetAllMethod";

    private final JdbcTemplate jdbc;

    public ItemTypeStoreRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /**
     * BLL 0577 InvLookUp.GetLookupsByTypeIdDt: Sp_InvLookup_GetAllMethod @OrganizationId,
     * @CompanyId, @InvLookupTypeId (6 on this form), @Activity='ReadByInvlookTypeId'. The procedure
     * has the organization/company filter commented out. Rows: Id, LookupName, ...
     */
    public List<Map<String, Object>> lookupsByType(UserAccount u, int lookupTypeId) {
        return DesktopProc.rows(jdbc, "Sp_InvLookup_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "InvLookupTypeId", lookupTypeId,
                "Activity", "ReadByInvlookTypeId"));
    }

    /**
     * BLL GenerateCode: [dbo].[Sp_ItemType_GetAllMethod] @OrganizationId, @CompanyId,
     * @Activity='GenerateCode'; TypeCode of the first row, "0" when there is none.
     */
    public String generateCode(UserAccount u) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "[dbo].[Sp_ItemType_GetAllMethod]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "GenerateCode"));
        return r.isEmpty() ? "0" : str(r.get(0).get("TypeCode"));
    }

    /**
     * BLL Getall(ReportsParameters): @OrganizationId, @CompanyId, [@Type when TypeId ≠ 0 — the
     * form never sets it], @ParentCategoryIds = obj.ItemIds (the form's ParentCategories),
     * @Activity='ReadByOrganizationCompanyId'.
     */
    public List<Map<String, Object>> getAll(UserAccount u, String parentCategoryIds) {
        return DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ParentCategoryIds", (parentCategoryIds == null || parentCategoryIds.isEmpty()) ? null : parentCategoryIds,
                "Activity", "ReadByOrganizationCompanyId"));
    }

    /** BLL GetByID: @Id, @Activity='ReadById' — no company filter in the procedure. */
    public Map<String, Object> readById(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /**
     * DAL 0446 SetData: one transaction, GenericProvider.SetProc(model, "Sp_ItemType_Insert" /
     * "Sp_ItemType_Update") — every non-virtual property of Architecture.Model.Inventory.ItemType
     * in declaration order (the caller builds the map).
     */
    public int setProc(String proc, Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, proc, model);
    }
}
