package com.mst.services;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
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
 * frmPosWhSalePricing - ScreenDefinition 107 "Whole Sale Pricing" (Inventory hub, module 4). The menu row resolves to
 * Architecture.WinApp.WholeSale/frmItemPricingSchedule.cs (migration/inventory/data/menu-form-mapping.csv); that is NOT the
 * Lookups/frmItemPricingSchedule (screen 91, Inv4aItemPricingScheduleService) - different BLL (Architecture.BLL.Pos), different grid.
 * Line refs ":NNN" are WholeSale/frmItemPricingSchedule.cs. base.Name = "frmItemPricingSchedule" (:1351) is the rights key (:127).
 *
 * priceTypeId (:130-137): base.Tag == "frmPosRetailPricing" -> 7, otherwise 6. Screen 107 carries Tag frmPosWhSalePricing -> 6.
 * BLL Architecture.BLL.Pos.ItemPricingSchedule -> Sp_ItemPricingSchedule_GetAllMethod / _Insert / _Update:
 *   ItemBindFromPricingSchedule(6)  = ReadByItemIdAndPriceLookUpId (@PriceTypeId, optional @ItemIds)  - cmbItem
 *   GetItemCategoryFromPricingShedule / GetItemTypesFromPricingShedule (@PriceTypeId)                  - cmbItemCategory / cmbItemType
 *   GetPreviousPricesByItemId (:271) = ReadByItemIdAndPriceLookUpId with ItemIds "," + id, ItemCategoryId, ItemTypeId
 *   Insert (:355) = Save: Id 0 -> Sp_ItemPricingSchedule_Insert, else Sp_ItemPricingSchedule_Update, one transaction.
 *   FormHistory (:439) = ItemPricingScheduleFormHistory (@Activity='FormHistory').
 *
 * DEVIATIONS (each is a place where running the desktop code against this database would throw):
 *  1. FormHistory: the dumped procedure's FormHistory activity returns ItemRate / UserName (no PriceTypeId filter, no ItemPrice /
 *     EntryUserName), while :454-460 read dtHistory["ItemPrice"] and ["EntryUserName"]. We map ItemPrice := ItemPrice else ItemRate
 *     and EntryUserName := EntryUserName else UserName so the grid shows the desktop's six columns.
 *  2. Edit (GetByItemId :498): the desktop reads ScheduleDesc / EffectedDate from CommonServices.ItemBindFromPricingSchedule, whose
 *     wrapper table has only ItemId, ItemName, ItemPrice, RateUomId, Id, ItemCode, so those two reads raise "Column ... does not belong
 *     to table". We read the same procedure activity directly (it returns both columns) instead of the wrapper.
 */
@Service
public class W2BWholeSalePricingScheduleService {

    /** base.Name :1351 - the rights key. */
    public static final String SCREEN = "frmItemPricingSchedule";
    /** :130-137 - Tag is frmPosWhSalePricing, not frmPosRetailPricing. */
    public static final int PRICE_TYPE = 6;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;

    public W2BWholeSalePricingScheduleService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights) {
        this.jdbc = jdbc; this.ctx = ctx; this.rights = rights;
    }

    /** frmItemPricingSchedule_Load :122 - rights (:127-129), ItemFill, ItemTypeFill, ItemCategoryFill. */
    public Map<String, Object> init() {
        ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));
        out.put("items", itemFill());
        out.put("itemTypes", typeFill());
        out.put("categories", categoryFill());
        return out;
    }

    /** ItemFill :183 - CommonServices.ItemBindFromPricingSchedule(priceTypeId): ItemId, ItemName shown (latest active row per item). */
    public List<Map<String, Object>> itemFill() {
        ctx.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : readPrices(null, 0, 0)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("ItemId", num(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            out.add(o);
        }
        return out;
    }

    /** ItemCategoryFill :199 - GetItemCategoryFromPricingShedule -> ItemCategoryId, CategoryDescription. */
    private List<Map<String, Object>> categoryFill() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_ItemPricingSchedule_GetAllMethod", params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "PriceTypeId", PRICE_TYPE, "Activity", "GetItemCategoryFromPricingShedule"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "ItemCategoryId")));
            o.put("Name", str(ci(r, "CategoryDescription")));
            out.add(o);
        }
        return out;
    }

    /** ItemTypeFill :218 - GetItemTypesFromPricingShedule -> ItemTypeId, TypeDescription. */
    private List<Map<String, Object>> typeFill() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_ItemPricingSchedule_GetAllMethod", params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "PriceTypeId", PRICE_TYPE, "Activity", "GetItemTypesFromPricingShedule"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "ItemTypeId")));
            o.put("Name", str(ci(r, "TypeDescription")));
            out.add(o);
        }
        return out;
    }

    /** ItemPricingSchedule.ReadByItemIdAndPriceLookUpId (BLL.Pos): optional @ItemIds, @ItemCategoryId, @ItemTypeId when non-zero. */
    private List<Map<String, Object>> readPrices(String itemIds, int categoryId, int typeId) {
        Map<String, Object> a = params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "PriceTypeId", PRICE_TYPE);
        if (itemIds != null && !itemIds.isEmpty()) a.put("ItemIds", itemIds);
        if (categoryId != 0) a.put("ItemCategoryId", categoryId);
        if (typeId != 0) a.put("ItemTypeId", typeId);
        a.put("Activity", "ReadByItemIdAndPriceLookUpId");
        return DesktopProc.rows(jdbc, "Sp_ItemPricingSchedule_GetAllMethod", a);
    }

    /**
     * GetPreviousPricesByItemId :271 (btnAdd :667 with the chosen item, btnSearch :640 via GetFilteredItems :323 with category / type).
     * Each result row becomes a grid row (ItemId, ItemName, EffectedDate, BranchId 0, ScheduleDesc, ItemPrice, NewPrice 0).
     * ItemIds is "," + id as :280. An empty result clears the grid (:306-309).
     */
    public List<Map<String, Object>> previousPrices(Map<String, Object> p) {
        ctx.requireAccountingUser();
        String ids = str(p.get("itemIds"));
        int cat = num(p.get("itemCategoryId")), typ = num(p.get("itemTypeId"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : readPrices(ids.isEmpty() ? null : "," + ids, cat, typ)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("ItemId", num(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("BranchId", 0);
            o.put("ItemPrice", dbl(ci(r, "ItemPrice")));
            o.put("NewPrice", 0d);
            out.add(o);
        }
        return out;
    }

    /** FormHistory :439 - ItemPricingScheduleFormHistory; columns Id, ItemId, ItemName, EffectedDate, ItemPrice, EntryUserName (see DEVIATION 1). */
    public List<Map<String, Object>> history() {
        ctx.requireAccountingUser();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ItemPricingSchedule_GetAllMethod", params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "Activity", "FormHistory", "PriceTypeId", PRICE_TYPE));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "Id")));
            o.put("ItemId", num(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("EffectedDate", Inv4aSupport.dateTimeText(ci(r, "EffectedDate")));
            Object price = ci(r, "ItemPrice") != null ? ci(r, "ItemPrice") : ci(r, "ItemRate");
            o.put("ItemPrice", dbl(price));
            Object user = ci(r, "EntryUserName") != null ? ci(r, "EntryUserName") : ci(r, "UserName");
            o.put("EntryUserName", str(user));
            out.add(o);
        }
        return out;
    }

    /** GetByItemId :498 - the history "Edit" button; one item's latest active schedule row (see DEVIATION 2). Null when none (the desktop then does nothing). */
    public Map<String, Object> edit(int itemId) {
        ctx.requireAccountingUser();
        List<Map<String, Object>> rows = readPrices(String.valueOf(itemId), 0, 0);
        if (rows.isEmpty()) return null;
        Map<String, Object> r = rows.get(0);
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", num(ci(r, "Id")));
        o.put("ScheduleDesc", str(ci(r, "ScheduleDesc")));
        o.put("EffectedDate", Inv4aSupport.dateTimeText(ci(r, "EffectedDate")));
        o.put("ItemId", num(ci(r, "ItemId")));
        o.put("ItemName", str(ci(r, "ItemName")));
        o.put("ItemPrice", dbl(ci(r, "ItemPrice")));
        return o;
    }

    /**
     * Insert() :355 - "Grid record not found" (:361); per row: EffectedDate = txtEffectedDate date + datEffectedTime time-of-day (:386-388),
     * "Item Required" (:392), "Check Item Price This Should be Not Zero" (:398); then ItemPricingSchedule.Save: row Id (RecId) 0 ->
     * Sp_ItemPricingSchedule_Insert, else _Update. The model object is posted whole, so the properties the form never sets go as the
     * C# defaults (Margin 0, PackingRate 0, PackingUom 0, RateUomId 0, CropYearId 0, IsActive false) - GenericProvider.SetProc sends every non-virtual property.
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        ctx.requireAccountingUser();
        int recId = num(body.get("recId"));
        if (recId > 0 ? !rights.has(SCREEN, "update") : !rights.has(SCREEN, "save")) {
            throw new IllegalStateException(recId > 0 ? "You do not have the Update right for this screen." : "You do not have the Save right for this screen.");
        }
        List<Map<String, Object>> rows = Inv4aSupport.rowsOf(body, "rows");
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid record not found");
        Timestamp eff = Inv4aSupport.dateWithTime(str(body.get("effectedDate")), str(body.get("effectedTime")));
        if (eff == null) throw new IllegalArgumentException("Effected Date is required.");
        String desc = str(body.get("scheduleDesc"));
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (num(r.get("ItemId")) == 0) throw new IllegalArgumentException("Item Required");
            if (dbl(r.get("NewPrice")) == 0.0) throw new IllegalArgumentException("Check Item Price This Should be Not Zero");
            Map<String, Object> d = new LinkedHashMap<>();                       // Model.POS.ItemPricingSchedule properties in declaration order
            d.put("Id", recId);
            d.put("ScheduleDesc", desc);
            d.put("ItemId", num(r.get("ItemId")));
            d.put("EffectedDate", eff);
            d.put("EntryDate", Inv4aSupport.now());
            d.put("EntryUser", ctx.currentUserId());
            d.put("ModifyDate", Inv4aSupport.now());
            d.put("ModifyUser", ctx.currentUserId());
            d.put("OrganizationId", ctx.currentOrganizationId());
            d.put("CompanyId", ctx.currentCompanyId());
            d.put("BranchesId", num(r.get("BranchId")));
            d.put("ChangedPriceLookUpId", PRICE_TYPE);
            d.put("PriceTypeId", PRICE_TYPE);
            d.put("ItemPrice", dbl(r.get("NewPrice")));
            d.put("PackingRate", 0d);
            d.put("Margin", 0d);
            d.put("PackingUom", 0);
            d.put("RateUomId", 0);
            d.put("CropYearId", 0);
            d.put("IsActive", false);
            details.add(d);
        }
        String proc = recId == 0 ? "Sp_ItemPricingSchedule_Insert" : "Sp_ItemPricingSchedule_Update";   // BLL Save: list[0].Id == 0
        for (Map<String, Object> d : details) DesktopProc.setProc(jdbc, proc, d);
        return Inv4aSupport.ok(recId > 0 ? "Update Successfully" : "Save Successfully");
    }
}
