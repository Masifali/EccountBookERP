package com.mst.services;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;
import static com.mst.services.Inv4aSupport.ci;
import static com.mst.services.Inv4aSupport.dbl;
import static com.mst.services.Inv4aSupport.num;
import static com.mst.services.Inv4aSupport.str;

/**
 * Batch inv4a — the CommonServices / BLL reads that frmItemPricingSchedule (91) and
 * frmItemPricingSchedule_New (796) share. Each method names the desktop call it stands for and the
 * procedure + @Activity behind it (procedure bodies read in procdure.utf8.sql; no BLL/DAL source
 * is in recovered_source, so where the Activity had to be inferred it is marked INFERRED).
 */
@Service
public class Inv4aPricingLookups {

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;

    public Inv4aPricingLookups(JdbcTemplate jdbc, CurrentUserContext ctx) { this.jdbc = jdbc; this.ctx = ctx; }

    /**
     * Item.InventoryParentCategories(Org, Company) → Sp_InventoryItemsOther_GetAllMethod
     * @Activity='InventoryParentCategories' (columns Id, InvParentCateDescription; @Ids NULL = all).
     * ParentCategoryBind (91 :245 / 796 :301) keeps only Id 1, 2 and 4.
     */
    public List<Map<String, Object>> parentCategories124() {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_InventoryItemsOther_GetAllMethod",
                params("Activity", "InventoryParentCategories"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            int id = num(ci(r, "Id"));
            if (id == 1 || id == 2 || id == 4) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", id);
                o.put("Name", str(ci(r, "InvParentCateDescription")));
                out.add(o);
            }
        }
        return out;
    }

    /**
     * CommonServices.GetItemCategoryForComboServiceBind → Sp_ItemCategory_GetAllMethod
     * @Activity='ReadByOrganizationCompanyId' (Id, CategoryDescription, InventoryParentCategoriesId …).
     * The forms filter the table by InventoryParentCategoriesId themselves (ItemCategoryByParentIdFill).
     */
    public List<Map<String, Object>> itemCategories() {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ItemCategory_GetAllMethod", params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "Activity", "ReadByOrganizationCompanyId"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "Id")));
            o.put("CategoryDescription", str(ci(r, "CategoryDescription")));
            o.put("InventoryParentCategoriesId", num(ci(r, "InventoryParentCategoriesId")));
            out.add(o);
        }
        return out;
    }

    /**
     * CommonServices.GetItemTypeForComboServiceBind → Sp_ItemType_GetAllMethod
     * @Activity='ReadByOrganizationCompanyId' (Id, TypeDescription, ItemCategoryId — the procedure
     * returns {@code 0 as ItemCategoryId}, which is why ItemTypeByCategoryIdFill's filter never
     * matches a real category and the forms fall back to the whole list; reproduced in the page).
     */
    public List<Map<String, Object>> itemTypes() {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ItemType_GetAllMethod", params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "Activity", "ReadByOrganizationCompanyId"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "Id")));
            o.put("TypeDescription", str(ci(r, "TypeDescription")));
            o.put("ItemCategoryId", num(ci(r, "ItemCategoryId")));
            out.add(o);
        }
        return out;
    }

    /**
     * CommonServices.ItemGetForComboServiceBind(ParentId) → INFERRED Sp_Item_GetAllMethod
     * @Activity='ReadAllItemsByParentId' @InventoryParentCategoriesId (Id, ItemName, ItemCategory,
     * ItemCode — the form reads SelectedRow.Cells[2] as the category, :605). ParentId 0 → no list.
     */
    public List<Map<String, Object>> itemsByParent(int parentId) {
        if (parentId == 0) return new ArrayList<>();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "InventoryParentCategoriesId", parentId,
                "Activity", "ReadAllItemsByParentId"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "Id")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemCategory", str(ci(r, "ItemCategory")));
            o.put("ItemCode", str(ci(r, "ItemCode")));
            out.add(o);
        }
        return out;
    }

    /**
     * CommonServices.GetUomScheduleByItemId(ItemId) → Sp_UOMSchedule_GetAllMethod @Activity='ReadByItemID',
     * reshaped to (Id, UOMCode, Equivalent, QtyEquivalent, BaseRateUom) as the desktop table is;
     * CmbRateUom shows UOMCode and the form reads Cells[2] = Equivalent (:605).
     */
    public List<Map<String, Object>> uomSchedule(int itemId) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "ItemId", itemId,
                "Activity", "ReadByItemID"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "Id")));
            o.put("UOMCode", str(ci(r, "UOMCode")));
            o.put("Equivalent", dbl(ci(r, "Equivalent")));
            o.put("QtyEquivalent", dbl(ci(r, "QtyEquivalent")));
            o.put("BaseRateUom", Inv4aSupport.truthy(ci(r, "BaseRateUom")));
            out.add(o);
        }
        return out;
    }

    /**
     * Insert() :459-469 — {@code dt.Select("Equivalent='" + ToInt(RateUom) + "'")}: the UOM schedule row
     * whose Equivalent equals the grid's RateUom (compared as the desktop does, after ToInt). Null when
     * the item has no schedule rows at all (then the desktop keeps the grid's RateUomId), and
     * {@code Collections.emptyMap()} when it has rows but none matches ("… RateUom not define against …").
     */
    public Map<String, Object> uomRowForEquivalent(int itemId, double rateUom) {
        List<Map<String, Object>> rows = uomSchedule(itemId);
        if (rows.isEmpty()) return null;
        int want = (int) rateUom;
        for (Map<String, Object> r : rows) {
            if ((int) dbl(r.get("Equivalent")) == want && dbl(r.get("Equivalent")) == (double) want) return r;
        }
        return java.util.Collections.emptyMap();
    }

    /**
     * ItemPricingScheduleForRice.GetLastItemRateByItemIdAndUomId(Org, Company, ItemId, RateUomId)
     * → Sp_ItemPricingScheduleForRice_GetAllMethod @Activity='GetLastItemRateByItemIdAndUomId'
     * (TOP 1 ItemRate, PreviousDate). Both 91 (:746) and 796 (:932) call this ForRice read.
     */
    public Map<String, Object> lastItemRate(int itemId, int rateUomId) {
        Map<String, Object> p = params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId());
        if (itemId != 0) p.put("ItemId", itemId);
        if (rateUomId != 0) p.put("RateUomId", rateUomId);
        p.put("Activity", "GetLastItemRateByItemIdAndUomId");
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ItemPricingScheduleForRice_GetAllMethod", p);
        Map<String, Object> out = new LinkedHashMap<>();
        if (rows.isEmpty()) {                                   // :758 — "0" and DateTime.Now
            out.put("found", false);
            out.put("ItemRate", "0");
            out.put("PreviousDate", Inv4aSupport.dayText(Inv4aSupport.now()));
        } else {
            out.put("found", true);
            out.put("ItemRate", str(ci(rows.get(0), "ItemRate")));
            out.put("PreviousDate", Inv4aSupport.dayText(ci(rows.get(0), "PreviousDate")));
        }
        return out;
    }

    /**
     * ItemPricingSchedule.GetItemCategoryFromPricingShedule / GetItemTypesFromPricingShedule (priceTypeId)
     * → Sp_ItemPricingSchedule_GetAllMethod @Activity='GetItemCategoryFromPricingShedule' (ItemCategoryId,
     * CategoryDescription) / 'GetItemTypesFromPricingShedule' (ItemTypeId, TypeDescription).
     */
    public List<Map<String, Object>> categoriesFromPricingSchedule(int priceTypeId) {
        return Inv4aSupport.idName(pricingScheduleRead(priceTypeId, "GetItemCategoryFromPricingShedule"), "ItemCategoryId", "CategoryDescription");
    }

    public List<Map<String, Object>> typesFromPricingSchedule(int priceTypeId) {
        return Inv4aSupport.idName(pricingScheduleRead(priceTypeId, "GetItemTypesFromPricingShedule"), "ItemTypeId", "TypeDescription");
    }

    private List<Map<String, Object>> pricingScheduleRead(int priceTypeId, String activity) {
        return DesktopProc.rows(jdbc, "Sp_ItemPricingSchedule_GetAllMethod", params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "PriceTypeId", priceTypeId,
                "Activity", activity));
    }

    /** clsGlobalVariables.ActiveYr.Start_Period of the current financial year (ISO) — used for date defaults. */
    public String financialYearStart() {
        try {
            List<Map<String, Object>> fy = jdbc.queryForList("SELECT Start_Period FROM dbo.FinancialYear WHERE Id=?", ctx.currentFinancialYearId());
            if (!fy.isEmpty() && fy.get(0).get("Start_Period") != null) return String.valueOf(fy.get(0).get("Start_Period")).substring(0, 10);
        } catch (RuntimeException ignored) { /* the default stays today */ }
        return null;
    }
}
