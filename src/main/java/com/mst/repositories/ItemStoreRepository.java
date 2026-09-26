package com.mst.repositories;

import com.mst.models.UserAccount;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 329 "Item Store" - {@code Architecture.WinApp.StoreManagement.AddItemStore} (3,869 lines).
 *
 * Only the reads that DIFFER from the already-ported sibling AddItemPM (screen 496) live here. A
 * line-by-line diff of AddItemStore.cs against AddItemPM.cs shows these four differ:
 *
 * <pre>
 * Form method                BLL                                   Procedure / @Activity                       Differs from 496 by
 * -------------------------  ------------------------------------  ------------------------------------------  -------------------------------
 * ItemCatagoryHistoryFill:347 ItemCategory.Getall (CategoryCode="8") Sp_ItemCategory_GetAllMethod               @Ids='8' (PM '7'); the SAME
 *                                                                   ReadByOrganizationCompanyId @Ids='8'        table feeds cmbItemCategory AND
 *                                                                                                               cmbitemcathistory
 * ItemTypeFill :414          ItemType.Getall (TypeId=17, ItemIds=8) Sp_ItemType_GetAllMethod                   @Type=17 @ParentCategoryIds='8'
 *                                                                   ReadByOrganizationCompanyId                 (PM 14/'7'); feeds cmbItemType AND
 *                                                                                                               CmbItemTypeHistory
 * grdfrmfill :1254           Item.FormHistory (BLL 0583 :468)       Sp_Item_GetAllMethod FormHistory            @NoOfRecords (50 on tab switch),
 *                                                                                                               parent 8, no MasterItem, ScreenName
 *                                                                                                               "AddItemStore"
 * grdhistory_DoubleClick:1010 Item.GetByID                          Sp_Item_GetAllMethod ReadById /             "is a Store item" = parent 8
 *                                                                   ItemImagesByItemId
 * </pre>
 *
 * Every other read (UOM, COA allocation, taxes, racks, pack sizes, companies, code generation,
 * GL accounts by category, configuration, ERP features, item images) is literally the same code
 * in both forms and is read through {@link ItemPmRepository} (read-only use).
 *
 * ItemCategory.Getall (BLL 0584 :96) sends @InventoryParentCategoriesId only when non-zero (never
 * set here) and @Ids only when CategoryCode is not empty. ItemType.Getall (BLL 0593 :92) sends
 * @Type when TypeId != 0 and @ParentCategoryIds when ItemIds is not empty. Both are sent.
 */
@Repository
public class ItemStoreRepository {

    /** base.Name (AddItemStore.cs designer :2109) - history ScreenName, rights and attachments key. */
    public static final String SCREEN = "AddItemStore";
    /** obj.InventoryParentCategories = 8 (:1265) / CategoryCode = "8" (:355) / ItemIds = "8" (:425). */
    public static final int STORE_PARENT = 8;
    /** ItemTypeFill :424 TypeId = 17. */
    public static final int STORE_TYPE = 17;

    private final JdbcTemplate jdbc;
    private final ItemPmRepository pm;

    public ItemStoreRepository(JdbcTemplate jdbc, ItemPmRepository pm) {
        this.jdbc = jdbc;
        this.pm = pm;
    }

    /** ItemCatagoryHistoryFill :347 - dtcath keeps two columns, Id and CategoryDescription (:362-367). */
    public List<Map<String, Object>> categories(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbc.queryForList(
                "EXEC dbo.Sp_ItemCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, @Ids=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), String.valueOf(STORE_PARENT), "ReadByOrganizationCompanyId")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ItemPmRepository.col(r, "Id"));
            m.put("CategoryDescription", ItemPmRepository.col(r, "CategoryDescription"));
            out.add(m);
        }
        return out;
    }

    /** ItemTypeFill :414 - BindDDLNew keeps Id and TypeDescription. */
    public List<Map<String, Object>> types(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbc.queryForList(
                "EXEC dbo.Sp_ItemType_GetAllMethod @OrganizationId=?, @CompanyId=?, @Type=?, @ParentCategoryIds=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), STORE_TYPE, String.valueOf(STORE_PARENT), "ReadByOrganizationCompanyId")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ItemPmRepository.col(r, "Id"));
            m.put("TypeDescription", ItemPmRepository.col(r, "TypeDescription"));
            out.add(m);
        }
        return out;
    }

    /**
     * grdfrmfill :1254 -> Item.FormHistory (BLL 0583 :468). Parameters in the BLL's order, each
     * guarded exactly as the BLL guards it:
     * <ul>
     *   <li>@ItemCategoryId / @ItemTypeId only when non-zero (the two history combos);</li>
     *   <li>@CanViewAllRecord always; @EntryUser only when it is false;</li>
     *   <li>@NoOfRecords only when non-zero - 50 from tabCAddItem_SelectedIndexChanged (:1346), 0
     *       (omitted) from Search (:1251) and LoadAll (:1354);</li>
     *   <li>@InventoryParentCategoriesId = 8;</li>
     *   <li>@MasterItemId, @LookupTypeIds, @ParentIds: never set on this form, never sent;</li>
     *   <li>@IsTaxable = false: the BLL sends it whenever ApprovedFilter != "All" and this form
     *       never sets ApprovedFilter;</li>
     *   <li>@ScreenName = "AddItemStore".</li>
     * </ul>
     */
    public List<Map<String, Object>> history(UserAccount u, boolean canViewAll, int noOfRecords, int category, int type) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?");
        List<Object> args = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId()));
        if (category != 0) { sql.append(", @ItemCategoryId=?"); args.add(category); }
        if (type != 0) { sql.append(", @ItemTypeId=?"); args.add(type); }
        sql.append(", @CanViewAllRecord=?"); args.add(canViewAll);
        if (!canViewAll) { sql.append(", @EntryUser=?"); args.add(u.getId()); }
        if (noOfRecords != 0) { sql.append(", @NoOfRecords=?"); args.add(noOfRecords); }
        sql.append(", @InventoryParentCategoriesId=?"); args.add(STORE_PARENT);
        sql.append(", @IsTaxable=?"); args.add(false);
        sql.append(", @ScreenName=?"); args.add(SCREEN);
        sql.append(", @Activity=?"); args.add("FormHistory");
        return jdbc.queryForList(sql.toString(), args.toArray());
    }

    /**
     * Item.GetByID + ItemImageslist, tenancy-checked (organization and company of the signed-in
     * user) and limited to Store items: the item's category must belong to parent category 8,
     * the only rows this form's history can show.
     */
    public Map<String, Object> details(UserAccount u, int id) {
        Map<String, Object> item = pm.record(u, id);
        if (pm.parentOfCategory(ItemPmRepository.intOf(ItemPmRepository.col(item, "ItemCategoryId"))) != STORE_PARENT) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "This item is not a Store item");
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("item", item);
        out.put("images", pm.images(id));
        return out;
    }
}
