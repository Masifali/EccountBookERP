package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;

import java.sql.ResultSet;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.TreeMap;
import java.util.concurrent.ConcurrentHashMap;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screens 777/776 "Define Item" / "Item Definition_Engr" (Architecture.WinApp.Inventory_Definition/frmAddItem_Engr.cs,
 * base.Name "frmAddItem_Engr") and 93 "Define Item Concrete" (frmAddItemConcrete.cs, base.Name "frmAddItemConcrete").
 * Every read the two forms make and the DAL 0436 Item.SetData write, each as its BLL/DAL builds it:
 *
 *   ItemCategory.Getall                        Sp_ItemCategory_GetAllMethod 'ReadByOrganizationCompanyId'
 *   UOM.Getall                                 Sp_UOM_GetAllMethod 'ReadByOrganizationCompanyId'
 *   CommonServices.GetItemTypeForComboServiceBind → ItemType.Getall   Sp_ItemType_GetAllMethod 'ReadByOrganizationCompanyId'
 *   ItemClass.GetAll                           Sp_ItemClass_GetAllMethod 'ReadAll'
 *   COAAllocation.GetLst                       Sp_COAAllocation_GetAllMethod 'COAAllocationSearch'  (Id, AccountTitle, AccountTypeId)
 *   ItemModel.FormHistory                      [item].[USP_itemModel_GetAllMethod] 'FormHistory'    (Engr)
 *   ItemQcGrade.FormHistory                    [dbo].[USP_ItemQcGrade_GetAllMethod] 'FormHistory'   (Concrete)
 *   TaxesTypes.Getall                          Sp_TaxesTypes_GetAllMethod 'ReadByOrganizationCompanyId'
 *   Country.GetAll                             SP_Country_ReadMethod @MethodType 'GetAll'
 *   ItemType.getMotherItems                    usp_getMotherItems                                   (Engr)
 *   SupplierCustomer.GetAllList                Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyId'
 *   CommonServices.CompanyServiceBind → Company.GetAlldt   Sp_Company_GetAllMethod 'ReadByOrganizationId' (@OrgCompanyTypeId)
 *   Item.GenerateCode / GetGLAccountbyItemCategoryId / FormHistory / GetByID / UpdateItemName   Sp_Item_GetAllMethod
 *   CommonServices.GetConfigurationByOrgCompandConfigDescription   Sp_ConfigrationsAllocation_GetAllMethod
 *   DAL 0436 SetData                           Sp_Item_Insert|Update, Sp_ItemTaxSchedule_Insert, Sp_UOMSchedule_Insert,
 *                                              Sp_ItemAllocation_Insert, Sp_ItemPricingSchedule_Insert, Sp_ItemsReorderSchedule_Insert,
 *                                              USP_ItemWiseCommission_InsertOrUpdate, [fed].[usp_ItemPricingSchedule_Insert];
 *                                              SaveCoa: usp_getAccountsInfoByItemCategoryId, Proc_ChartofAccount_Insert,
 *                                              Sp_COAAllocation_Insert, Sp_AccountsOpeningBalances_Insert
 *
 * The model maps (CLR defaults of every non-virtual property, what GenericProvider.SetProc sends) come from
 * {@link InventoryPosDefaults}. {@link #exec} binds only the keys that are parameters of the procedure (read once from
 * sys.parameters), the way DesktopInventoryItemWriter does for screen 111.
 */
@Repository
public class Inv4cDefineItemRepository {

    private final JdbcTemplate jdbc;
    private final Map<String, List<String>> parameters = new ConcurrentHashMap<>();

    public Inv4cDefineItemRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ================================================================================= lookups

    public List<Map<String, Object>> categories(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ItemCategory_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }
    public List<Map<String, Object>> uoms(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_UOM_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }
    public List<Map<String, Object>> itemTypes(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ItemType_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }
    public List<Map<String, Object>> itemClasses() {
        return DesktopProc.rows(jdbc, "Sp_ItemClass_GetAllMethod", params("Activity", "ReadAll"));
    }
    public List<Map<String, Object>> accounts(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "COAAllocationSearch"));
    }
    public List<Map<String, Object>> itemModels(UserAccount u) {
        return DesktopProc.rows(jdbc, "[item].[USP_itemModel_GetAllMethod]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", u.getBranchesId(), "Activity", "FormHistory"));
    }
    public List<Map<String, Object>> qcGrades(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_ItemQcGrade_GetAllMethod]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "FormHistory"));
    }
    public List<Map<String, Object>> taxTypes(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_TaxesTypes_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }
    public List<Map<String, Object>> countries(UserAccount u) {
        return DesktopProc.rows(jdbc, "SP_Country_ReadMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll"));
    }
    public List<Map<String, Object>> motherItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "usp_getMotherItems", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }
    public List<Map<String, Object>> suppliers(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }
    /** CommonServices.CompanyServiceBind — Company.GetAlldt { OrgCompanyTypeId = OrganizationId }. */
    public List<Map<String, Object>> companies(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Company_GetAllMethod", params(
                "OrgCompanyTypeId", u.getOrganizationId(), "Activity", "ReadByOrganizationId"));
    }
    /** CommonServices.GetConfigurationByOrgCompandConfigDescription — ConfigKey of the description, "" when none. */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ConfigDescription", description,
                "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        return r.isEmpty() ? "" : StoreIssuanceRepository.str(StoreIssuanceRepository.ci(r.get(0), "ConfigKey"));
    }
    public static boolean truthy(String s) {
        String t = s == null ? "" : s.trim().toLowerCase(Locale.ROOT);
        return t.equals("true") || t.equals("1");
    }

    // ==================================================================================== item

    /** BLL 0583 GenerateCode — @ItemCategoryId, @ItemTypeId (0 from this form), 'GenerateItemCodeByCategoryId'. */
    public List<Map<String, Object>> generateCode(UserAccount u, int itemCategoryId) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemCategoryId", itemCategoryId, "ItemTypeId", 0, "Activity", "GenerateItemCodeByCategoryId"));
    }
    /** BLL 0583 GetGLAccountbyItemCategoryId — @Id = ItemCategoryId. */
    public List<Map<String, Object>> glByCategory(UserAccount u, int itemCategoryId) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", itemCategoryId, "Activity", "GetGLAccountbyItemCategoryId"));
    }
    /** BLL 0583 FormHistory as grdfrmfill builds it (Org, Company, NoOfRecords, ScreenName, CanViewAllRecord, EntryUser). */
    public List<Map<String, Object>> formHistory(UserAccount u, int noOfRecords, String screenName, boolean canViewAll) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("EntryUser", u.getId());
        if (noOfRecords != 0) p.put("NoOfRecords", noOfRecords);
        p.put("ScreenName", screenName);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", p);
    }
    /** BLL 0583 GetByID — 'ReadById'; null when no row. */
    public Map<String, Object> getById(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }
    /** DAL 0436 UpdateItemName — Sp_Item_GetAllMethod 'Update' with @Id, @ItemName, @ItemNameOtherLingo (null → DBNull on the desktop), @Code. */
    public void updateItemName(int id, String itemName, String code) {
        jdbc.update("EXEC dbo.Sp_Item_GetAllMethod @Id=?, @Activity=?, @ItemName=?, @ItemNameOtherLingo=?, @Code=?",
                id, "Update", itemName, null, code);
    }
    /** DAL 0205 GetParentCategoryIdByItemCategoryId — Sp_Item_GetAllMethod 'GetParentCategoryIdByItemCategoryId'. */
    public int parentCategoryId(UserAccount u, int itemCategoryId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemCategoryId", itemCategoryId, "Activity", "GetParentCategoryIdByItemCategoryId"));
        return r.isEmpty() ? 0 : StoreIssuanceRepository.toInt(StoreIssuanceRepository.ci(r.get(0), "InventoryParentCategoriesId"));
    }
    /** DAL 0436 :52-110 — usp_getAccountsInfoByItemCategoryId @ItemCategoryId, @Activity 'Stock' | 'Sale'. */
    public Map<String, Object> accountsInfoByCategory(int itemCategoryId, String activity) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "usp_getAccountsInfoByItemCategoryId", params("ItemCategoryId", itemCategoryId, "Activity", activity));
        return r.isEmpty() ? null : r.get(0);
    }
    /** DAL 0436 SaveCoa — CommonServices.GetERPFeaturesByCompanyId(…, 17). */
    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (StoreIssuanceRepository.toInt(StoreIssuanceRepository.ci(r, "Id")) == featureId) return true;
        }
        return false;
    }

    // =================================================================================== models

    public static Map<String, Object> itemModel() { return InventoryPosDefaults.item(); }
    public static Map<String, Object> priceModel() { return InventoryPosDefaults.price(); }
    public static Map<String, Object> reorderModel() { return InventoryPosDefaults.reorder(); }
    public static Map<String, Object> feedPriceModel() { return InventoryPosDefaults.feedPrice(); }
    public static Map<String, Object> accountModel() { return InventoryPosDefaults.account(); }
    public static Map<String, Object> accountAllocationModel() { return InventoryPosDefaults.accountAllocation(); }
    public static Map<String, Object> accountOpeningModel() { return InventoryPosDefaults.accountOpening(); }

    // ==================================================================================== write

    /**
     * GenericProvider.SetProc — {@code Convert.ToInt32(ExecuteScalar())}: the first cell of the first row, {@code fallback}
     * when there is none or it is not a positive number. Only keys that are parameters of {@code procedure} are bound.
     */
    public int exec(String procedure, Map<String, Object> values, int fallback) {
        List<String> names = parameters.computeIfAbsent(procedure, p -> jdbc.queryForList(
                "SELECT SUBSTRING(name,2,128) FROM sys.parameters WHERE object_id=OBJECT_ID(?) AND parameter_id>0 ORDER BY parameter_id",
                String.class, p));
        if (names.isEmpty()) throw new IllegalStateException("Procedure contract unavailable: " + procedure);
        TreeMap<String, Object> ci = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        ci.putAll(values);
        List<String> selected = new ArrayList<>();
        for (String n : names) if (ci.containsKey(n) && ci.get(n) != null) selected.add(n);   // ADO.NET: null → not sent
        StringBuilder sql = new StringBuilder("EXEC ").append(procedure);
        for (int i = 0; i < selected.size(); i++) sql.append(i == 0 ? " " : ", ").append('@').append(selected.get(i)).append("=?");
        Integer first = jdbc.execute(sql.toString(), (PreparedStatementCallback<Integer>) s -> {
            int n = 1;
            for (String name : selected) s.setObject(n++, ci.get(name));
            boolean result = s.execute();
            Integer cell = null;
            while (true) {
                if (result) {
                    try (ResultSet rs = s.getResultSet()) {
                        if (cell == null && rs.next()) {
                            Object o = rs.getObject(1);
                            if (o instanceof Number) cell = ((Number) o).intValue();
                            else if (o != null) { try { cell = Integer.valueOf(String.valueOf(o).trim()); } catch (NumberFormatException ignore) { /* not a number */ } }
                        }
                        while (rs.next()) { /* drain */ }
                    }
                } else if (s.getUpdateCount() == -1) {
                    break;
                }
                result = s.getMoreResults();
            }
            return cell;
        });
        return first != null && first > 0 ? first : fallback;
    }

    /** Sp_UOMSchedule_GetAllMethod 'ReadByItemGroupId' (DAL 0436 :174) — not reached by these forms (ItemGroupId is always 0). */
    public List<Map<String, Object>> uomSchedulesOfGroup(int itemGroupId) {
        return DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", params("ItemGroupId", itemGroupId, "Activity", "ReadByItemGroupId"));
    }

    /** Plain query (clsGlobalVariables.ActiveYr.Start_Period — Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId). */
    public List<Map<String, Object>> query(String sql, Object... args) { return jdbc.queryForList(sql, args); }

    public static Timestamp now() { return new Timestamp(System.currentTimeMillis()); }

    public static Map<String, Object> map(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }
}
