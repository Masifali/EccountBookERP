package com.mst.services;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;
import static com.mst.services.Inv4aSupport.ci;
import static com.mst.services.Inv4aSupport.day;
import static com.mst.services.Inv4aSupport.num;
import static com.mst.services.Inv4aSupport.str;

/**
 * frmGetItemPricingList (Architecture.WinApp.Inventory_Reports/frmGetItemPricingList.cs) — ScreenDefinition 803
 * "Item Pricing List" (Inventory hub, module 8 "Inventory Reports"). Line refs ":NNN" are that file.
 *
 * BLL ItemPricingSchedule:
 *   GetItemCategoryFromPricingShedule / GetItemTypesFromPricingShedule (PriceTypeId 6, :185/:228)
 *     → Sp_ItemPricingSchedule_GetAllMethod 'GetItemCategoryFromPricingShedule' / 'GetItemTypesFromPricingShedule'
 *   getPricingDates (:138) → usp_getPricingDates (distinct EffectedDate desc)
 *   GetItemPricingList (:286) → USP_GetItemPricingList @OrganizationId,@CompanyId,@ItemCategoryId,@ItemTypeId,@EffectedDate
 * Print: btnRegister :542 → 325_01-ItemPricingList.rpt with the procedure rows (dtGrid) as returned.
 */
@Service
public class Inv4aItemPricingListService {

    public static final int SCREEN_ID = 803;
    public static final int PRICE_TYPE = 6;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final DesktopReportRights rights;
    private final Inv4aPricingLookups look;

    public Inv4aItemPricingListService(JdbcTemplate jdbc, CurrentUserContext ctx, DesktopReportRights rights, Inv4aPricingLookups look) {
        this.jdbc = jdbc; this.ctx = ctx; this.rights = rights; this.look = look;
    }

    /** frmGetItemPricingList_Load :98 / toolStripButton1_Click :363 (Refresh) — CategoryFill, TypeFill, EffectedDateFill. */
    public Map<String, Object> init() {
        rights.require(ctx.requireAccountingUser(), SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("categories", look.categoriesFromPricingSchedule(PRICE_TYPE));
        out.put("itemTypes", look.typesFromPricingSchedule(PRICE_TYPE));
        List<Map<String, Object>> dates = new ArrayList<>();
        int n = 0;
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "usp_getPricingDates", params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId()))) {
            Map<String, Object> o = new LinkedHashMap<>();                 // dtDate.Rows.Add(Count + 1, r["EffectedDate"]) :145 — Count never increments
            o.put("Id", n + 1);
            o.put("EffectedDate", Inv4aSupport.dateTimeText(ci(r, "EffectedDate")));
            dates.add(o);
        }
        out.put("effectedDates", dates);
        return out;
    }

    /**
     * gridHisory :273 — GetItemPricingList(Org, Company, ItemCategoryId, ItemTypeId, EffectedDate = ToDateTime(CmbEffectedDate.Text)).
     * A 0 id / blank date is not sent (the ReportsParameters guard, as in the pattern). Returns the raw rows (dtGrid);
     * the page shows RecordNo(hidden) ItemCategory ItemType EffectedDate ItemId(hidden) ItemName RateUom ItemPrice ItemRate (:290-302).
     */
    public List<Map<String, Object>> show(Map<String, Object> p) {
        rights.require(ctx.requireAccountingUser(), SCREEN_ID, "View");
        Map<String, Object> a = params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId());
        if (num(p.get("itemCategoryId")) != 0) a.put("ItemCategoryId", num(p.get("itemCategoryId")));
        if (num(p.get("itemTypeId")) != 0) a.put("ItemTypeId", num(p.get("itemTypeId")));
        Timestamp d = day(str(p.get("effectedDate")));
        if (d != null) a.put("EffectedDate", d);
        return Inv4aSupport.plain(DesktopProc.rows(jdbc, "USP_GetItemPricingList", a));
    }
}
