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
 * frmItemSalesPricingSchedule (Architecture.WinApp.WholeSale/frmItemSalesPricingSchedule.cs) — ScreenDefinition 100
 * "Sales Pricing (With Margin)". Line refs ":NNN" are that file. base.Name = "frmItemSalesPricingSchedule" (:219).
 * priceTypeId = 6 unless the form is opened with Tag "frmPosRetailPricing" (then 7, :227-234); screen 100 is the
 * whole-sale entry, so 6.
 *
 * Reads:
 *   RoundingForItemPricing (clsGlobalVariables.configrationsAllocation :222) → Sp_ConfigrationsAllocation_GetAllMethod
 *   InvAllocateDiscItemToDiscType.GetDiscountTypes (:314)     → Usp_GetDiscountTypes (Id, DiscountTypeCode)
 *   ItemPricingSchedule.GetItemsFromRecipe (:281)            → Sp_ItemPricingSchedule_GetAllMethod 'GetItemsFromRecipe' @DiscountTypeId
 *   InvManufacturingBOMHeader.GetDataForDropDown… 'CropYear' (:340) → USP_GetDataForDropDownFromInvManufacturingBOMHeader
 *   getCropYearFromPricingSchedule / getItemsFromPricingSchedule (:362/:378) → Sp_ItemPricingSchedule_GetAllMethod
 *   ItemPricingScheduleForRice.GetRecipsForFinalList (:719)  → USP_GetRecipsForFinalList
 *   ItemPricingSchedule.FinalItemSalePriceListSlip (:613/:978/:1017) → USP_GetFinalItemSalePriceList
 * Writes (ItemPricingSchedule.Save :528, one transaction): Sp_ItemPricingSchedule_Insert per grdFinal row and
 *   USP_SalesPricingScheduleBreakup_Insert per grd row; ItemSalePriceList_ActiveInActive (:1209) →
 *   USP_ItemSalePriceList_ActiveInActive per changed row.
 * Prints: 324-SalesItemPricingList.rpt (FinalItemSalePriceListSlip rows), 324A_SalesPricingList.rpt (dtForFinalPrice).
 */
@Service
public class Inv4aSalesPricingWithMarginService {

    public static final String SCREEN = "frmItemSalesPricingSchedule";
    public static final int PRICE_TYPE = 6;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;

    public Inv4aSalesPricingWithMarginService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights) {
        this.jdbc = jdbc; this.ctx = ctx; this.rights = rights;
    }

    /** frmItemPricingSchedule_Load :215. */
    public Map<String, Object> init() {
        ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));
        out.put("roundingFactor", roundingFactor());
        out.put("priceTypeId", PRICE_TYPE);
        out.put("discountTypes", discountTypes());
        out.put("items", items(0));
        out.put("cropYears", cropYears());
        out.put("historyCropYears", Inv4aSupport.idName(scheduleRead("getCropYearFromPricingSchedule"), "Id", "CropYear"));
        out.put("historyItems", Inv4aSupport.idName(scheduleRead("getItemsFromPricingSchedule"), "Id", "ItemName"));
        return out;
    }

    /** :222-226 — ConfigKey of "RoundingForItemPricing" as double (0 when not configured). */
    private double roundingFactor() {
        try {
            List<Map<String, Object>> rows = jdbc.queryForList(
                    "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@ConfigDescription=?,@Activity='GetConfigurationByOrgCompandConfigDescription'",
                    ctx.currentOrganizationId(), ctx.currentCompanyId(), "RoundingForItemPricing");
            return rows.isEmpty() ? 0d : dbl(ci(rows.get(0), "ConfigKey"));
        } catch (RuntimeException e) { return 0d; }
    }

    /** FillDiscountTypes :306 → Usp_GetDiscountTypes (Id, DiscountTypeCode, DiscountTypeDescription). */
    private List<Map<String, Object>> discountTypes() {
        return Inv4aSupport.idName(DesktopProc.rows(jdbc, "Usp_GetDiscountTypes", params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId())), "Id", "DiscountTypeCode");
    }

    /** ItemFill :268 — GetItemsFromRecipe(@DiscountTypeId when selected); multi-check combo "Item Name". */
    public List<Map<String, Object>> items(int discountTypeId) {
        ctx.requireAccountingUser();
        Map<String, Object> a = params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId());
        if (discountTypeId != 0) a.put("DiscountTypeId", discountTypeId);
        a.put("Activity", "GetItemsFromRecipe");
        return Inv4aSupport.idName(DesktopProc.rows(jdbc, "Sp_ItemPricingSchedule_GetAllMethod", a), "Id", "ItemName");
    }

    /** BindCropYear :331 — Activity 'CropYear' rows (Id, ReferenceName; column 2 hidden). */
    private List<Map<String, Object>> cropYears() {
        return Inv4aSupport.idName(DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromInvManufacturingBOMHeader", params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "Activity", "CropYear")), "Id", "ReferenceName");
    }

    private List<Map<String, Object>> scheduleRead(String activity) {
        return DesktopProc.rows(jdbc, "Sp_ItemPricingSchedule_GetAllMethod", params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "Activity", activity));
    }

    /**
     * btnAdd_Click :685 ("Show") — "Packing Uom Field Required" when txtPackingUom is 0; GetRecipsForFinalList(Org, Company,
     * EffectedDate = datEffectedDate, DiscountTypeId, CropYearId, ItemIds = ",id,id" of the checked item names).
     * Returns the raw rows; the page builds dtGrid (:722-726) and runs FinalAmountCalculate / FinalGridFill.
     */
    public List<Map<String, Object>> recipes(Map<String, Object> p) {
        ctx.requireAccountingUser();
        if (dbl(p.get("packingUom")) == 0d) throw new IllegalArgumentException("Packing Uom Field Required");   // :695
        Timestamp eff = day(str(p.get("effectedDate")));
        if (eff == null) throw new IllegalArgumentException("Effected Date is required.");
        Map<String, Object> a = params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "EffectedDate", eff);
        if (!str(p.get("itemIds")).isEmpty()) a.put("ItemIds", str(p.get("itemIds")));
        if (num(p.get("discountTypeId")) != 0) a.put("DiscountTypeId", num(p.get("discountTypeId")));
        if (num(p.get("cropYearId")) != 0) a.put("CropYearId", num(p.get("cropYearId")));
        return Inv4aSupport.plain(DesktopProc.rows(jdbc, "USP_GetRecipsForFinalList", a));
    }

    /** FinalItemSalePriceListSlip (BtnShowHistoryReport :1001 / btnPrint :964 / history "Print" :613). Empty → "Record Not Found For Display". */
    public List<Map<String, Object>> finalPriceList(Map<String, Object> p) {
        ctx.requireAccountingUser();
        Timestamp eff = day(str(p.get("effectedDate")));
        if (eff == null) throw new IllegalArgumentException("Effected Date is required.");
        Map<String, Object> a = params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "EffectedDate", eff);
        if (num(p.get("itemId")) != 0) a.put("ItemId", num(p.get("itemId")));
        if (num(p.get("cropYearId")) != 0) a.put("CropYearId", num(p.get("cropYearId")));
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "USP_GetFinalItemSalePriceList", a);
        if (rows.isEmpty()) throw new IllegalArgumentException("Record Not Found For Display");               // :1022 / :988 / :621
        return Inv4aSupport.plain(rows);
    }

    /**
     * Insert() :455 — after "Are you sure to Save?": one ItemPricingSchedule per grdFinal row (:473-501; "Item Required"
     * when BrandId is 0) and one SalesPricingScheduleBreakup per grd row (:503-527), saved together.
     * DiscountTypeId of the breakup is {@code Conversion.ToInt(cmbDiscType.Text)} — the TEXT, so a non-numeric code is 0
     * (:510); the page sends the text and it is converted here the same way.
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        ctx.requireAccountingUser();
        if (!rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        List<Map<String, Object>> finals = Inv4aSupport.rowsOf(body, "finalRows");
        List<Map<String, Object>> details = Inv4aSupport.rowsOf(body, "rows");
        Timestamp effDate = day(str(body.get("effectedDate")));
        if (effDate == null) throw new IllegalArgumentException("Effected Date is required.");
        Timestamp eff = Inv4aSupport.dateWithTime(str(body.get("effectedDate")), str(body.get("effectedTime")));
        double margin = dbl(body.get("marginPercent"));
        int packingUom = num(body.get("packingUom"));
        String desc = str(body.get("description"));
        int discountTypeText = num(str(body.get("discountTypeText")));
        int branch = ctx.currentBranchId();

        List<Map<String, Object>> heads = new ArrayList<>();
        for (Map<String, Object> r : finals) {
            if (num(r.get("BrandId")) == 0) throw new IllegalArgumentException("Item Required");                 // :490
            Map<String, Object> d = new LinkedHashMap<>();                                                     // Sp_ItemPricingSchedule_Insert
            d.put("Id", 0);
            d.put("ScheduleDesc", desc);
            d.put("ItemId", num(r.get("BrandId")));
            d.put("EffectedDate", eff);
            d.put("EntryDate", Inv4aSupport.now());
            d.put("EntryUser", ctx.currentUserId());
            d.put("ModifyDate", Inv4aSupport.now());
            d.put("ModifyUser", ctx.currentUserId());
            d.put("OrganizationId", ctx.currentOrganizationId());
            d.put("CompanyId", ctx.currentCompanyId());
            d.put("BranchesId", branch);
            d.put("ChangedPriceLookUpId", PRICE_TYPE);
            d.put("PriceTypeId", PRICE_TYPE);
            d.put("ItemPrice", dbl(r.get("FinalRate")));
            d.put("Margin", margin);
            d.put("PackingUom", packingUom);
            d.put("RateUomId", 0);
            d.put("PackingRate", dbl(r.get("PackingRate")));
            d.put("CropYearId", num(r.get("CropYearId")));
            heads.add(d);
        }
        List<Map<String, Object>> breakups = new ArrayList<>();
        for (Map<String, Object> r : details) {
            Map<String, Object> b = new LinkedHashMap<>();                                                     // USP_SalesPricingScheduleBreakup_Insert
            b.put("Id", 0);
            b.put("DiscountTypeId", discountTypeText);
            b.put("BrandId", num(r.get("BrandId")));
            b.put("ItemId", num(r.get("ItemId")));
            Timestamp semi = Inv4aSupport.day(str(r.get("SemiEffectedDate")));
            if (semi != null) b.put("SemiEffectedDate", semiWithTime(str(r.get("SemiEffectedDate"))));
            b.put("SemiRate", dbl(r.get("ItemRate")));
            b.put("BrandEffectedDate", effDate);
            b.put("BrandRate", dbl(r.get("PackingRate")));
            b.put("RateChangeable", Inv4aSupport.truthy(r.get("RateChangeable")));
            b.put("Percent", dbl(r.get("Prcnt")));
            b.put("PercentRate", dbl(r.get("PercentRate")));
            b.put("Expenses", dbl(r.get("Expenses")));
            b.put("PackingRate", dbl(r.get("PackingRate")));
            b.put("RateUom", (double) packingUom);
            b.put("MarginPercent", margin);
            b.put("ScheduleDescription", desc);
            b.put("OrganizationId", ctx.currentOrganizationId());
            b.put("CompanyId", ctx.currentCompanyId());
            b.put("CropYearId", num(r.get("CropYearId")));
            b.put("BrandCropYearId", num(r.get("BrandCropYearId")));
            breakups.add(b);
        }
        for (Map<String, Object> d : heads) DesktopProc.scalar(jdbc, "Sp_ItemPricingSchedule_Insert", d);
        for (Map<String, Object> b : breakups) DesktopProc.scalar(jdbc, "USP_SalesPricingScheduleBreakup_Insert", b);
        return Inv4aSupport.ok(num(body.get("recId")) > 0 ? "Update Successfully" : "Save Successfully");      // :529-536
    }

    private static Timestamp semiWithTime(String s) {
        if (s.length() >= 19) {
            try { return Timestamp.valueOf(s.substring(0, 19).replace('T', ' ')); } catch (RuntimeException ignored) { /* date only */ }
        }
        return day(s);
    }

    /**
     * UpdateIsActiveFromHistory :1179 — after "Are you sure to Update?": the GrdForFinalPrice rows whose IsActive differs
     * from the loaded copy → USP_ItemSalePriceList_ActiveInActive(Org, Company, BrandId, CropYearId = BrandCropYearId, IsActive).
     * The page sends only the changed rows.
     */
    @Transactional
    public Map<String, Object> activeInactive(Map<String, Object> body) {
        ctx.requireAccountingUser();
        if (!rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        for (Map<String, Object> r : Inv4aSupport.rowsOf(body, "rows")) {
            DesktopProc.rows(jdbc, "USP_ItemSalePriceList_ActiveInActive", params(
                    "OrganizationId", ctx.currentOrganizationId(),
                    "CompanyId", ctx.currentCompanyId(),
                    "BrandId", num(r.get("BrandId")),
                    "CropYearId", num(r.get("BrandCropYearId")),
                    "IsActive", Inv4aSupport.truthy(r.get("IsActive"))));
        }
        return Inv4aSupport.ok("Updated Successfully");                                                         // :1210
    }
}
