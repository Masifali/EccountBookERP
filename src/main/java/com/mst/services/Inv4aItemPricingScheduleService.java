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
import static com.mst.services.Inv4aSupport.day;
import static com.mst.services.Inv4aSupport.dbl;
import static com.mst.services.Inv4aSupport.num;
import static com.mst.services.Inv4aSupport.str;

/**
 * frmItemPricingSchedule (Architecture.WinApp.Lookups/frmItemPricingSchedule.cs) — ScreenDefinition 91
 * "Item Pricing Schedule" (Inventory hub, module 4 "Inventory Defination"). Line refs ":NNN" are that file.
 * base.Name = "frmItemPricingSchedule" (rights key, :193). DocumentTypeId 183 (:476, :505).
 *
 * BLL ItemPricingScheduleForRice → Sp_ItemPricingScheduleForRice_GetAllMethod / _Insert:
 *   GetItemWithRatesForItemPricingSchedule (btnSearch :786), FormHistory (:501, @DocumentTypeId 183),
 *   GetLastItemRateByItemIdAndUomId (CmbRateUom_Leave :746), Save (:482 → _Insert per grid row),
 *   GetItemSchedulePricingSlip (btnPrintHistory :942 → 325-ItemPricingList.rpt). The dump has no
 *   USP_GetItemSchedulePricingSlip; the registry maps 325 to USP_GetItemSchedulePricingForRice, whose
 *   parameters (ItemCategoryId, ItemTypeId, EffectiveDate, Ids) are exactly what :925-941 sets — used here.
 */
@Service
public class Inv4aItemPricingScheduleService {

    public static final String SCREEN = "frmItemPricingSchedule";
    public static final int DOC_TYPE = 183;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;
    private final Inv4aPricingLookups look;

    public Inv4aItemPricingScheduleService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights, Inv4aPricingLookups look) {
        this.jdbc = jdbc; this.ctx = ctx; this.rights = rights; this.look = look;
    }

    /** frmItemPricingSchedule_Load :189 — rights (:193), ParentCategoryBind, ItemCategoryAndTypeFill, ItemFill(parent). */
    public Map<String, Object> init() {
        ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));
        out.putAll(refresh());
        return out;
    }

    /** btnrefresh_Click :886 — ParentCategoryBind, ItemCategoryAndTypeFill, ItemFill(parent). */
    public Map<String, Object> refresh() {
        ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> parents = look.parentCategories124();
        out.put("parentCategories", parents);
        out.put("categories", look.itemCategories());
        out.put("itemTypes", look.itemTypes());
        /* ParentCategoryBind activates Rows[1] (:273) = the first data row (row 0 is the header-less list's first?) —
           BindDDL(ZeroIndex:false) adds no blank row, so Rows[1] is the SECOND parent category; the page does the same. */
        int parentId = parents.size() > 1 ? num(parents.get(1).get("Id")) : (parents.isEmpty() ? 0 : num(parents.get(0).get("Id")));
        out.put("items", look.itemsByParent(parentId));
        return out;
    }

    /** ItemFill(ParentId) :217 (CmbParentCatagory_Leave :723). */
    public List<Map<String, Object>> items(int parentId) { ctx.requireAccountingUser(); return look.itemsByParent(parentId); }

    /** cmbItem_ValueChanged :546 — GetUomScheduleByItemId → CmbRateUom (Id / UOMCode / Equivalent). */
    public List<Map<String, Object>> uoms(int itemId) { ctx.requireAccountingUser(); return look.uomSchedule(itemId); }

    /** CmbRateUom_Leave :736. */
    public Map<String, Object> lastRate(int itemId, int rateUomId) { ctx.requireAccountingUser(); return look.lastItemRate(itemId, rateUomId); }

    /**
     * btnSearch_Click :770 — GetItemWithRatesForItemPricingSchedule(Org, Company, InventoryParentCategories,
     * ItemCategoryId, ItemTypeId, EffectedDate when chkApplyDate). The ReportsParameters guards (a 0 id is not
     * sent) follow the InventoryStockReportWithValues pattern. Returns the procedure rows; the page builds dt
     * (:789-809: applyRateUom, CbOnlyShowRatedItems filter).
     */
    public List<Map<String, Object>> search(Map<String, Object> p) {
        ctx.requireAccountingUser();
        Map<String, Object> a = params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId());
        if (num(p.get("parentCategoryId")) != 0) a.put("InventoryParentCategoriesId", num(p.get("parentCategoryId")));
        if (num(p.get("itemCategoryId")) != 0) a.put("ItemCategoryId", num(p.get("itemCategoryId")));
        if (num(p.get("itemTypeId")) != 0) a.put("ItemTypeId", num(p.get("itemTypeId")));
        if (Inv4aSupport.truthy(p.get("applyDate")) && day(str(p.get("effectedDate"))) != null) a.put("EffectiveDate", day(str(p.get("effectedDate"))));
        a.put("Activity", "GetItemWithRatesForItemPricingSchedule");
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ItemPricingScheduleForRice_GetAllMethod", a);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {                      // dt.Rows.Add(...) :801 column by column
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("ItemId", num(ci(r, "ItemId")));
            o.put("RateUomId", num(ci(r, "RateUomId")));
            o.put("BaseUomId", num(ci(r, "BaseUomId")));
            o.put("ItemCategory", str(ci(r, "CategoryDescription")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("BaseUom", str(ci(r, "BaseUom")));
            o.put("PreviousDate", Inv4aSupport.dayText(ci(r, "PreviousDate")));
            o.put("PreviousRate", dbl(ci(r, "PreviousRate")));
            o.put("RateUom", dbl(ci(r, "RateUom")));
            out.add(o);
        }
        return out;
    }

    /** FormHistory :492 — FormHistory(Org, Company, DocumentTypeId 183); grid hides Id, ItemId, CropYearId (:528). */
    public List<Map<String, Object>> history() {
        ctx.requireAccountingUser();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ItemPricingScheduleForRice_GetAllMethod", params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "DocumentTypeId", DOC_TYPE,
                "Activity", "FormHistory"));
        return Inv4aSupport.plain(rows);
    }

    /**
     * Insert() :432 — after "Are you sure to Save?": per grid row EffectiveDate = txtEffectedDate + datEffectedTime
     * TimeOfDay (:454-456), RateUomId re-resolved from GetUomScheduleByItemId by Equivalent (:459-469, the
     * "RateUom not define against" stop), then ItemPricingScheduleForRice.Save → Sp_ItemPricingScheduleForRice_Insert
     * per row with the model values of :470-479 (DocumentTypeId 183, FinancialYearId = ActiveYr.Id). One transaction.
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        ctx.requireAccountingUser();
        if (!rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        List<Map<String, Object>> rows = Inv4aSupport.rowsOf(body, "rows");
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid record not found");          // :442
        Timestamp eff = Inv4aSupport.dateWithTime(str(body.get("effectedDate")), str(body.get("effectedTime")));
        if (eff == null) throw new IllegalArgumentException("Effected Date is required.");
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            int itemId = num(r.get("ItemId"));
            int rateUomId = num(r.get("RateUomId"));
            Map<String, Object> uom = look.uomRowForEquivalent(itemId, dbl(r.get("RateUom")));
            if (uom != null) {
                if (uom.isEmpty()) {
                    throw new IllegalArgumentException(str(r.get("RateUom")) + " RateUom not define against " + str(r.get("ItemName")));   // :465
                }
                rateUomId = num(uom.get("Id"));                                                   // dr[0].ItemArray[0]
            }
            Map<String, Object> d = new LinkedHashMap<>();                                        // Model ItemPricingScheduleForRice, property order
            d.put("Id", 0);
            d.put("EffectiveDate", eff);
            d.put("ItemId", itemId);
            d.put("ItemRate", dbl(r.get("NewPrice")));
            d.put("RateUomId", rateUomId);
            d.put("EntryDate", Inv4aSupport.now());
            d.put("EntryUserId", ctx.currentUserId());
            d.put("ModifyDate", Inv4aSupport.now());
            d.put("ModifyUserId", ctx.currentUserId());
            d.put("OrganizationId", ctx.currentOrganizationId());
            d.put("CompanyId", ctx.currentCompanyId());
            d.put("FinancialYearId", ctx.currentFinancialYearId());
            d.put("IsApproved", false);                            // never set by the form; @IsApproved BIT has no default
            d.put("ApprovedUserId", 0);
            d.put("ApprovedDate", Inv4aSupport.now());             // :479
            d.put("CropYearId", 0);
            d.put("DocumentTypeId", DOC_TYPE);
            details.add(d);
        }
        for (Map<String, Object> d : details) DesktopProc.scalar(jdbc, "Sp_ItemPricingScheduleForRice_Insert", d);
        return Inv4aSupport.ok("Save Successfully");                                               // :483
    }

    /**
     * btnPrintHistory_Click :916 — ItemCategoryId / ItemTypeId from the History combos; checked history rows → Ids
     * "id,id,", otherwise EffectedDate = datEffectedDateHistory. → USP_GetItemSchedulePricingForRice (see class note).
     * Empty → "Record Not Found For Display" (:945). The page sends the rows to /reports/print/grid as 325-ItemPricingList.rpt.
     */
    public List<Map<String, Object>> printRows(Map<String, Object> p) {
        ctx.requireAccountingUser();
        Map<String, Object> a = params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId());
        if (num(p.get("itemCategoryId")) != 0) a.put("ItemCategoryId", num(p.get("itemCategoryId")));
        if (num(p.get("itemTypeId")) != 0) a.put("ItemTypeId", num(p.get("itemTypeId")));
        String ids = str(p.get("ids"));
        if (!ids.isEmpty()) a.put("Ids", ids);
        else if (day(str(p.get("effectedDate"))) != null) a.put("EffectiveDate", day(str(p.get("effectedDate"))));
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "USP_GetItemSchedulePricingForRice", a);
        if (rows.isEmpty()) throw new IllegalArgumentException("Record Not Found For Display");
        return Inv4aSupport.plain(rows);
    }
}
