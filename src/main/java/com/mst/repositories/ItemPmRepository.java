package com.mst.repositories;

import com.mst.models.UserAccount;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;

/**
 * Screen 496 "Item PM" - {@code Architecture.WinApp.StoreManagement.AddItemPM} (4,384 lines).
 *
 * Every read the form makes, each traced Event -> BLL -> procedure. Parameters are sent exactly
 * when the BLL sends them: a guarded parameter the BLL omits is omitted here too, because a
 * guarded parameter sent as NULL or 0 is a different call.
 *
 * <pre>
 * Form method            BLL                                   Procedure / @Activity
 * ---------------------  ------------------------------------  -----------------------------------------------
 * ItemCategoryBind :418  ItemCategory.Getall  (CategoryCode=7)  Sp_ItemCategory_GetAllMethod  ReadByOrganizationCompanyId  @Ids='7'
 * ItemTypeFill     :506  ItemType.Getall (TypeId=14, Ids=7)     Sp_ItemType_GetAllMethod      ReadByOrganizationCompanyId  @Type=14 @ParentCategoryIds='7'
 * MasterItemFill   :451  Item.getMasterItems                    usp_getMasterItems            @OrganizationId only
 * BaseUnitFill     :474  UOM.Getall                             Sp_UOM_GetAllMethod           ReadByOrganizationCompanyId
 * PurchaseGLAcFill :537  COAAllocation.GetForComboBind          Sp_COAAllocation_GetAllMethod COAForCombobindig  (+@UserId)
 * CmbTaxTypeFill   :558  TaxesTypes.Getall, Id != 1 dropped     Sp_TaxesTypes_GetAllMethod    ReadByOrganizationCompanyId
 * RackNameFill...  :597  invWarehouseRack.GetRackswithWarehouse usp_getRackswithWarehouse     @organizationId @CompanyId
 * PackSizeIdFor... :752  GeneralReprots.StaticColumnNames       SpStaticColumnNames           PackSizeIdForItem
 * CompaniesBind... :707  Company.GetAlldt                       Sp_Company_GetAllMethod       ReadByOrganizationId  @OrgCompanyTypeId
 * HistoryComboBind :374  getGlobalAllItems, parent 7 only       USP_Item_AllItemsWithModal    @OrganizationId @CompanyId
 * GenerateItemCode :671  Item.GenerateCode                      Sp_Item_GetAllMethod          GenerateItemCodeByCategoryId
 * SetGLAccounts    :688  Item.GetGLAccountbyItemCategoryId      Sp_Item_GetAllMethod          GetGLAccountbyItemCategoryId  (@Id = category)
 * HistoryGridfill  :1359 Item.FormHistory                       Sp_Item_GetAllMethod          FormHistory
 * grdhistory_DblClk:1105 Item.GetByID (+ ItemImagesByItemId)    Sp_Item_GetAllMethod          ReadById / ItemImagesByItemId
 * </pre>
 */
@Repository
public class ItemPmRepository {

    /** base.Name at :4333 - the ScreenName the history and the attachments are keyed on. */
    public static final String SCREEN = "AddItemPM";
    /** InventoryParentCategories = 7: Packing Material. The form hard-codes it in four places. */
    public static final int PM_PARENT = 7;

    private final JdbcTemplate jdbc;
    private final InventoryPosItemRepository items;

    public ItemPmRepository(JdbcTemplate jdbc, InventoryPosItemRepository items) {
        this.jdbc = jdbc;
        this.items = items;
    }

    // ------------------------------------------------------------------ InvDefrmAddItem_Load :332

    public List<Map<String, Object>> categories(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList(
                "EXEC dbo.Sp_ItemCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, @Ids=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "7", "ReadByOrganizationCompanyId")) {
            out.add(pick(r, "Id", "CategoryDescription"));      // dtcath, :431-437
        }
        return out;
    }

    public List<Map<String, Object>> types(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList(
                "EXEC dbo.Sp_ItemType_GetAllMethod @OrganizationId=?, @CompanyId=?, @Type=?, @ParentCategoryIds=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), 14, "7", "ReadByOrganizationCompanyId")) {
            out.add(pick(r, "Id", "TypeDescription"));          // BindDDLNew keeps two columns
        }
        return out;
    }

    /** BindDDL shows every column after the first, so the rows go out whole. */
    public List<Map<String, Object>> masterItems(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.usp_getMasterItems @OrganizationId=?", u.getOrganizationId());
    }

    public List<Map<String, Object>> units(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList(
                "EXEC dbo.Sp_UOM_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadByOrganizationCompanyId")) {
            Map<String, Object> m = new LinkedHashMap<>();       // :489-496 Id, UomCode, Equivalent
            m.put("Id", col(r, "Id"));
            m.put("UomCode", col(r, "UOMCode"));
            m.put("Equivalent", col(r, "Equivalent"));
            out.add(m);
        }
        return out;
    }

    /** CommonServices.CoaAllocationGetForComboServiceBind - @UserId is sent because ID != 0. */
    public List<Map<String, Object>> accounts(UserAccount u) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @UserId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), u.getId(), "COAForCombobindig");
    }

    public List<Map<String, Object>> taxes(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList(
                "EXEC dbo.Sp_TaxesTypes_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadByOrganizationCompanyId")) {
            if (intOf(col(r, "Id")) != 1) out.add(pick(r, "Id", "TaxName"));   // :580 - by Id, not by Type
        }
        return out;
    }

    public List<Map<String, Object>> racks(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.usp_getRackswithWarehouse @organizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId())) {
            Map<String, Object> m = new LinkedHashMap<>();       // GlobalServicesMethods.GetWarehouseRacks :805-809
            m.put("Id", col(r, "Id"));
            m.put("RackName", col(r, "rackName"));
            m.put("invWarehouseId", col(r, "invWarehouseId"));
            m.put("WarehouseName", col(r, "WareHouseName"));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> packSizes() {
        return jdbc.queryForList("EXEC dbo.SpStaticColumnNames @Activity=?", "PackSizeIdForItem");
    }

    public List<Map<String, Object>> companies(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.Sp_Company_GetAllMethod @OrgCompanyTypeId=?, @Activity=?",
                u.getOrganizationId(), "ReadByOrganizationId")) {
            Map<String, Object> m = new LinkedHashMap<>();       // :714-722 Id, Location <- CompName, Value = true
            m.put("Id", col(r, "Id"));
            m.put("Location", col(r, "CompName"));
            out.add(m);
        }
        return out;
    }

    /**
     * HistoryComboBind :374. Distinct Type / Category / Master Item of the company's existing PM
     * items (InventoryParentCategoriesId == 7), zero ids skipped (:395-405).
     */
    public Map<String, List<Map<String, Object>>> historyCombos(UserAccount u) {
        Map<String, Map<Integer, Object>> sets = new LinkedHashMap<>();
        sets.put("types", new LinkedHashMap<>());
        sets.put("categories", new LinkedHashMap<>());
        sets.put("masterItems", new LinkedHashMap<>());
        for (var r : jdbc.queryForList("EXEC dbo.USP_Item_AllItemsWithModal @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId())) {
            if (intOf(col(r, "InventoryParentCategoriesId")) != PM_PARENT) continue;
            put(sets.get("types"), col(r, "ItemTypeId"), col(r, "ItemType"));
            put(sets.get("categories"), col(r, "ItemCategoryId"), col(r, "ItemCategory"));
            put(sets.get("masterItems"), col(r, "MasterItemId"), col(r, "MasterItem"));
        }
        Map<String, List<Map<String, Object>>> out = new LinkedHashMap<>();
        sets.forEach((k, v) -> {
            List<Map<String, Object>> rows = new ArrayList<>();
            v.forEach((id, name) -> { Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", id); m.put("Name", name); rows.add(m); });
            out.put(k, rows);
        });
        return out;
    }

    // ------------------------------------------------------------------ code + accounts

    public Map<String, Object> generateCode(UserAccount u, int category, int type) {
        var rows = jdbc.queryForList(
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemCategoryId=?, @ItemTypeId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), category, type, "GenerateItemCodeByCategoryId");
        return rows.isEmpty() ? Map.of() : rows.get(0);
    }

    public Map<String, Object> glAccounts(UserAccount u, int category) {
        var rows = jdbc.queryForList(
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Id=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), category, "GetGLAccountbyItemCategoryId");
        return rows.isEmpty() ? Map.of() : rows.get(0);
    }

    // ------------------------------------------------------------------ history

    /**
     * Item.FormHistory (BLL :468), with the ReportsParameters the form builds at :1365-1377.
     *
     * Two parameters are easy to miss and both are sent:
     * <ul>
     *   <li>{@code @InventoryParentCategoriesId = 7} - the form sets InventoryParentCategories = 7;</li>
     *   <li>{@code @IsTaxable = 0} - the BLL sends it whenever {@code ApprovedFilter != "All"}, and
     *       this form never sets ApprovedFilter, so null != "All" is true and IsTaxable goes out
     *       as its CLR default, false.</li>
     * </ul>
     * NoOfRecords, Ids and ParentCategoryIds are unset on this form and are not sent.
     */
    public List<Map<String, Object>> history(UserAccount u, boolean canViewAll, int category, int type, int master) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?");
        List<Object> args = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId()));
        if (category != 0) { sql.append(", @ItemCategoryId=?"); args.add(category); }
        if (type != 0) { sql.append(", @ItemTypeId=?"); args.add(type); }
        sql.append(", @CanViewAllRecord=?"); args.add(canViewAll);
        if (!canViewAll) { sql.append(", @EntryUser=?"); args.add(u.getId()); }
        sql.append(", @InventoryParentCategoriesId=?"); args.add(PM_PARENT);
        if (master != 0) { sql.append(", @MasterItemId=?"); args.add(master); }
        sql.append(", @IsTaxable=?"); args.add(false);
        sql.append(", @ScreenName=?"); args.add(SCREEN);
        sql.append(", @Activity=?"); args.add("FormHistory");
        return jdbc.queryForList(sql.toString(), args.toArray());
    }

    // ------------------------------------------------------------------ one record

    /** Tenancy-checked ReadById. */
    public Map<String, Object> record(UserAccount u, int id) {
        return items.record(u, id);
    }

    /** The category's parent, to keep this screen to Packing Material items only. */
    public int parentOfCategory(int categoryId) {
        var rows = jdbc.queryForList("SELECT InventoryParentCategoriesId FROM dbo.ItemCategory WHERE Id=?", categoryId);
        return rows.isEmpty() ? 0 : intOf(col(rows.get(0), "InventoryParentCategoriesId"));
    }

    public Map<String, Object> details(UserAccount u, int id) {
        Map<String, Object> item = record(u, id);
        if (parentOfCategory(intOf(col(item, "ItemCategoryId"))) != PM_PARENT) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "This item is not a Packing Material item");
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("item", item);
        out.put("images", images(id));
        out.put("attachments", attachments(u, id));
        return out;
    }

    public List<Map<String, Object>> images(int id) {
        return jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @Id=?, @Activity=?", id, "ItemImagesByItemId");
    }

    public List<Map<String, Object>> attachments(UserAccount u, int id) {
        return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?, @Id=?, @Activity=?", SCREEN, id, "ReadById")
                .stream()
                .filter(r -> intOf(col(r, "OrganizationId")) == u.getOrganizationId() && intOf(col(r, "CompanyId")) == u.getCompanyId())
                .toList();
    }

    // ------------------------------------------------------------------ configuration

    /** GlobalVariables_Helper.GetConfigValueFromGlobal - the allocation row's ConfigKey. */
    public boolean configuration(UserAccount u, String name) {
        var rows = jdbc.queryForList(
                "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), name, "GetConfigurationByOrgCompandConfigDescription");
        return !rows.isEmpty() && Set.of("true", "1").contains(Objects.toString(col(rows.get(0), "ConfigKey"), "").trim().toLowerCase(Locale.ROOT));
    }

    /** CommonServices.GetERPFeatureById - present in the company's feature list. */
    public boolean feature(UserAccount u, int id) {
        return jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId()).stream().anyMatch(r -> intOf(col(r, "Id")) == id);
    }

    // ------------------------------------------------------------------ helpers

    private static void put(Map<Integer, Object> set, Object id, Object name) {
        int k = intOf(id);
        if (k > 0 && !set.containsKey(k)) set.put(k, name);
    }

    private static Map<String, Object> pick(Map<String, Object> r, String... names) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (String n : names) m.put(n, col(r, n));
        return m;
    }

    /** Case-insensitive column read - a procedure's casing is not guaranteed. */
    public static Object col(Map<String, Object> r, String name) {
        if (r == null) return null;
        if (r.containsKey(name)) return r.get(name);
        for (var e : r.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    public static int intOf(Object v) {
        if (v == null) return 0;
        if (v instanceof Number n) return n.intValue();
        if (v instanceof Boolean b) return b ? 1 : 0;
        try { return (int) Double.parseDouble(v.toString().trim()); } catch (NumberFormatException e) { return 0; }
    }
}
