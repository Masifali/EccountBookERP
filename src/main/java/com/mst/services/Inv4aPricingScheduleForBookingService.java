package com.mst.services;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.support.DesktopProc.params;
import static com.mst.services.Inv4aSupport.ci;
import static com.mst.services.Inv4aSupport.day;
import static com.mst.services.Inv4aSupport.dbl;
import static com.mst.services.Inv4aSupport.num;
import static com.mst.services.Inv4aSupport.str;

/**
 * frmItemPricingSchedule_New (Architecture.WinApp.BookingOffice/frmItemPricingSchedule_New.cs) — ScreenDefinition 796
 * "Pricing Schedule For Booking". Line refs ":NNN" are that file. base.Name = "frmItemPricingSchedule_New" (:224).
 *
 * BLL ItemPricingSchedule → Sp_ItemPricingSchedule_GetAllMethod / _Insert (PriceTypeId 6, IsActive true :610-611):
 *   GetItemWithRatesForItemPricingSchedule (btnSearch :972), FormHistory (:638, parent/category/type/item/crop/date),
 *   GetDataForDropDownFromItemPricingSchedule (HistoryComboFill :425 → USP_GetDataForDropDownFromItemPricingSchedule,
 *   split on Activity), Save (:614 → _Insert per grid row), GetItemSchedulePricingSlip (btnPrint325 :1178).
 * CommonServices.CropYearGetAllService → Sp_InvCropYear_GetAllMethod @Activity='ReadAll' (Id, CropYear).
 * The last-rate read is the ForRice one (CmbRateUom_Leave :932), as on the desktop.
 *
 * DEVIATION (print): USP_GetItemSchedulePricingSlip is not in the procedure dump. The 325 rows are produced from
 * this screen's own data instead — checked history rows → the FormHistory rows with those Ids; no checked rows →
 * dbo.USP_GetItemPricingList (PriceTypeId 6, ItemCategoryId, ItemTypeId, EffectedDate = cmbEffectedDate).
 */
@Service
public class Inv4aPricingScheduleForBookingService {

    public static final String SCREEN = "frmItemPricingSchedule_New";
    public static final int PRICE_TYPE = 6;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;
    private final Inv4aPricingLookups look;

    public Inv4aPricingScheduleForBookingService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights, Inv4aPricingLookups look) {
        this.jdbc = jdbc; this.ctx = ctx; this.rights = rights; this.look = look;
    }

    /** frmItemPricingSchedule_Load :220 — rights, ParentCategoryBind, CropYearBind, ItemCategoryAndTypeFill, ItemFill, HistoryComboFill. */
    public Map<String, Object> init() {
        ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));
        out.putAll(refresh());
        out.put("history", historyLookups());
        return out;
    }

    /** btnrefresh_Click :1072 — ParentCategoryBind, CropYearBind, ItemCategoryAndTypeFill, ItemFill(parent), GrdCropYearBind. */
    public Map<String, Object> refresh() {
        ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> parents = look.parentCategories124();
        out.put("parentCategories", parents);
        out.put("cropYears", cropYears());
        out.put("categories", look.itemCategories());
        out.put("itemTypes", look.itemTypes());
        int parentId = parents.size() > 1 ? num(parents.get(1).get("Id")) : (parents.isEmpty() ? 0 : num(parents.get(0).get("Id")));   // Rows[1].Activate :329
        out.put("items", look.itemsByParent(parentId));
        return out;
    }

    /** CropYearBind :279 — CommonServices.CropYearGetAllService → Sp_InvCropYear_GetAllMethod 'ReadAll' (Id, CropYear). */
    private List<Map<String, Object>> cropYears() {
        return Inv4aSupport.idName(DesktopProc.rows(jdbc, "Sp_InvCropYear_GetAllMethod", params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "Activity", "ReadAll")), "Id", "CropYear");
    }

    /** HistoryComboFill :399 — one procedure call, rows split on Activity (:434-457); EffectedDate shown "dd-MMM-yy". */
    public Map<String, Object> historyLookups() {
        ctx.requireAccountingUser();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromItemPricingSchedule", params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId()));
        Map<String, List<Map<String, Object>>> by = new LinkedHashMap<>();
        for (String a : new String[] { "ParentCategories", "ItemCategories", "ItemTypes", "Item", "CropYear", "EffectedDate" }) by.put(a, new ArrayList<>());
        for (Map<String, Object> r : rows) {
            List<Map<String, Object>> t = by.get(str(ci(r, "Activity")));
            if (t == null) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "Id")));
            if ("EffectedDate".equals(str(ci(r, "Activity")))) {
                o.put("Name", Inv4aSupport.dayText(ci(r, "ReferenceName")));      // ISO; the page shows dd-MMM-yy
            } else {
                o.put("Name", str(ci(r, "ReferenceName")));
            }
            t.add(o);
        }
        return new LinkedHashMap<>(by);
    }

    public List<Map<String, Object>> items(int parentId) { ctx.requireAccountingUser(); return look.itemsByParent(parentId); }
    public List<Map<String, Object>> uoms(int itemId) { ctx.requireAccountingUser(); return look.uomSchedule(itemId); }
    public Map<String, Object> lastRate(int itemId, int rateUomId) { ctx.requireAccountingUser(); return look.lastItemRate(itemId, rateUomId); }

    /** btnSearch_Click :956 — ItemPricingSchedule.GetItemWithRatesForItemPricingSchedule (same parameters as 91). */
    public List<Map<String, Object>> search(Map<String, Object> p) {
        ctx.requireAccountingUser();
        Map<String, Object> a = params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId());
        if (num(p.get("parentCategoryId")) != 0) a.put("InventoryParentCategoriesId", num(p.get("parentCategoryId")));
        if (num(p.get("itemCategoryId")) != 0) a.put("ItemCategoryId", num(p.get("itemCategoryId")));
        if (num(p.get("itemTypeId")) != 0) a.put("ItemTypeId", num(p.get("itemTypeId")));
        if (Inv4aSupport.truthy(p.get("applyDate")) && day(str(p.get("effectedDate"))) != null) a.put("EffectiveDate", day(str(p.get("effectedDate"))));
        a.put("Activity", "GetItemWithRatesForItemPricingSchedule");
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ItemPricingSchedule_GetAllMethod", a);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {                      // dt.Rows.Add(...) :987
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("ItemId", num(ci(r, "ItemId")));
            o.put("RateUomId", num(ci(r, "RateUomId")));
            o.put("BaseUomId", num(ci(r, "BaseUomId")));
            o.put("ItemCategory", str(ci(r, "CategoryDescription")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("CropYearId", num(ci(r, "CropYearId")));
            o.put("BaseUom", str(ci(r, "BaseUom")));
            o.put("PreviousDate", Inv4aSupport.dayText(ci(r, "PreviousDate")));
            o.put("PreviousRate", dbl(ci(r, "PreviousRate")));
            o.put("RateUom", dbl(ci(r, "RateUom")));
            out.add(o);
        }
        return out;
    }

    /** FormHistory :624 — the six history filters; the grid shows Id(hidden) EffectedDate ItemId(hidden) ItemName CropYearId(hidden) CropYear ItemRate RateUom EntryDate UserName (:652-664). */
    public List<Map<String, Object>> history(Map<String, Object> p) {
        ctx.requireAccountingUser();
        List<Map<String, Object>> rows = historyRows(p);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "Id")));
            o.put("EffectedDate", Inv4aSupport.dateTimeText(ci(r, "EffectedDate")));
            o.put("ItemId", num(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("CropYearId", num(ci(r, "CropYearId")));
            o.put("CropYear", str(ci(r, "CropYear")));
            o.put("ItemRate", dbl(ci(r, "ItemRate")));
            o.put("RateUom", str(ci(r, "RateUom")));
            o.put("EntryDate", Inv4aSupport.dateTimeText(ci(r, "EntryDate")));
            o.put("UserName", str(ci(r, "UserName")));
            out.add(o);
        }
        return out;
    }

    private List<Map<String, Object>> historyRows(Map<String, Object> p) {
        Map<String, Object> a = params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId());
        if (num(p.get("parentCategoryId")) != 0) a.put("InventoryParentCategoriesId", num(p.get("parentCategoryId")));
        if (num(p.get("itemCategoryId")) != 0) a.put("ItemCategoryId", num(p.get("itemCategoryId")));
        if (num(p.get("itemTypeId")) != 0) a.put("ItemTypeId", num(p.get("itemTypeId")));
        if (num(p.get("itemId")) != 0) a.put("ItemId", num(p.get("itemId")));
        if (num(p.get("cropYearId")) != 0) a.put("CropYearId", num(p.get("cropYearId")));
        if (day(str(p.get("effectedDate"))) != null) a.put("EffectiveDate", day(str(p.get("effectedDate"))));   // cmbEffectedDate.Text → ToDateTime (:647)
        a.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, "Sp_ItemPricingSchedule_GetAllMethod", a);
    }

    /**
     * Insert() :558 — "Grid record not found"; confirm; per row: EffectedDate = date + time (:580-582),
     * " Crop Year Field Required In Grid " when CropYearId is 0 and (int)NewPrice != 0 (:584), RateUomId / PackingUom
     * from the UOM schedule row whose Equivalent = RateUom (:591-602), then Sp_ItemPricingSchedule_Insert with
     * PriceTypeId 6 and IsActive true (:603-611). One transaction.
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        ctx.requireAccountingUser();
        if (!rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        List<Map<String, Object>> rows = Inv4aSupport.rowsOf(body, "rows");
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid record not found");          // :568
        Timestamp eff = Inv4aSupport.dateWithTime(str(body.get("effectedDate")), str(body.get("effectedTime")));
        if (eff == null) throw new IllegalArgumentException("Effected Date is required.");
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            int itemId = num(r.get("ItemId"));
            int cropYearId = num(r.get("CropYearId"));
            if ((cropYearId == 0 || str(r.get("CropYearId")).isEmpty()) && num(r.get("NewPrice")) != 0) {
                throw new IllegalArgumentException(" Crop Year Field Required In Grid ");          // :586
            }
            int rateUomId = num(r.get("RateUomId"));
            int packingUom = 0;
            Map<String, Object> uom = look.uomRowForEquivalent(itemId, dbl(r.get("RateUom")));
            if (uom != null) {
                if (uom.isEmpty()) {
                    throw new IllegalArgumentException(str(r.get("RateUom")) + " RateUom not define against " + str(r.get("ItemName")));   // :597
                }
                rateUomId = num(uom.get("Id"));                                                   // dr[0].ItemArray[0]
                packingUom = num(uom.get("Equivalent"));                                          // dr[0].ItemArray[2]
            }
            Map<String, Object> d = new LinkedHashMap<>();                                        // Sp_ItemPricingSchedule_Insert parameters
            d.put("Id", 0);                                        // ScheduleDesc is never set by this form → NULL (not bound)
            d.put("ItemId", itemId);
            d.put("EffectedDate", eff);
            d.put("EntryDate", Inv4aSupport.now());
            d.put("EntryUser", ctx.currentUserId());
            d.put("ModifyDate", Inv4aSupport.now());
            d.put("ModifyUser", ctx.currentUserId());
            d.put("OrganizationId", ctx.currentOrganizationId());
            d.put("CompanyId", ctx.currentCompanyId());
            d.put("BranchesId", 0);
            d.put("ChangedPriceLookUpId", 0);
            d.put("PriceTypeId", PRICE_TYPE);
            d.put("ItemPrice", dbl(r.get("NewPrice")));
            d.put("Margin", 0d);
            d.put("PackingUom", packingUom);
            d.put("RateUomId", rateUomId);
            d.put("PackingRate", 0d);
            d.put("CropYearId", cropYearId);
            d.put("IsActive", true);
            details.add(d);
        }
        for (Map<String, Object> d : details) DesktopProc.scalar(jdbc, "Sp_ItemPricingSchedule_Insert", d);
        return Inv4aSupport.ok("Save Successfully");                                               // :615
    }

    /** btnPrint325_Click :1152 — see the class DEVIATION note. Empty → "Record Not Found For Display" (:1181). */
    public List<Map<String, Object>> printRows(Map<String, Object> p) {
        ctx.requireAccountingUser();
        String ids = str(p.get("ids"));
        List<Map<String, Object>> rows;
        if (!ids.isEmpty()) {
            Set<String> want = new HashSet<>(Arrays.asList(ids.split(",")));
            Map<String, Object> f = new LinkedHashMap<>();
            f.put("itemCategoryId", p.get("itemCategoryId"));
            f.put("itemTypeId", p.get("itemTypeId"));
            rows = new ArrayList<>();
            for (Map<String, Object> r : historyRows(f)) if (want.contains(String.valueOf(num(ci(r, "Id"))))) rows.add(r);
        } else {
            Map<String, Object> a = params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId());
            if (num(p.get("itemCategoryId")) != 0) a.put("ItemCategoryId", num(p.get("itemCategoryId")));
            if (num(p.get("itemTypeId")) != 0) a.put("ItemTypeId", num(p.get("itemTypeId")));
            if (day(str(p.get("effectedDate"))) != null) a.put("EffectedDate", day(str(p.get("effectedDate"))));
            rows = DesktopProc.rows(jdbc, "USP_GetItemPricingList", a);
        }
        if (rows.isEmpty()) throw new IllegalArgumentException("Record Not Found For Display");
        return Inv4aSupport.plain(rows);
    }
}
