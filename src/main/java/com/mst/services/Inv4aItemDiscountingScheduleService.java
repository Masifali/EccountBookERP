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
 * frmItemDiscountingScheduleForBooking (Architecture.WinApp.WholeSale/frmItemDiscountingScheduleForBooking.cs) —
 * ScreenDefinition 99 "Item Discounting Schedule". Line refs ":NNN" are that file.
 * base.Name = "frmItemDiscountingScheduleForBooking"; priceTypeId = 6 when Tag is that name, else 7 (:175-182) — the
 * screen 99 menu entry opens it under its own name, so 6.
 *
 * Reads:
 *   DrmLookUps.GetDiscountsPolicyByLookupTypeIds (:187) → Sp_drmLookUpsGetAllMethods 'GetDiscountsPolicyByLookupTypeIds';
 *     only "Regular Discount" becomes a policy card (:192)
 *   DrmLookUps.GetByLookUpsTypeId(1) (:301) → 'GetByLookUpsTypeId' @Id 1; only "Percent Discount On Item" (:308)
 *   DrmLookUps.GetByLookUpsTypeId(7) (:330) → @Id 7; only "Value" (:337)
 *   ItemPricingSchedule.ReadByItemIdAndPriceLookUpId (:423 / :542) → Sp_ItemPricingSchedule_GetAllMethod
 *     'ReadByItemIdAndPriceLookUpId' @PriceTypeId (+ ItemCategoryId / ItemTypeId for the filter search)
 *   CommonServices.GetItemCategoryFromPricingShedule / GetItemTypesFromPricingShedule(priceTypeId) (:359 / :383)
 *   ItemDiscounting.DiscountingHistory (:678) → Sp_ItemDiscountingSchedule_GetAllMethod 'DiscountingHistory' @PriceTypeId
 * Write: ItemDiscounting.Save (:641) → Sp_ItemDiscountingSchedule_Insert per grid row (RecId is always 0: ReadById :934
 *   is empty, so the desktop never enters update mode).
 * No print on this form. The desktop form does not read rights (no SetRightsValueInRightsObject call).
 */
@Service
public class Inv4aItemDiscountingScheduleService {

    public static final String SCREEN = "frmItemDiscountingScheduleForBooking";
    public static final int PRICE_TYPE = 6;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final Inv4aPricingLookups look;

    public Inv4aItemDiscountingScheduleService(JdbcTemplate jdbc, CurrentUserContext ctx, Inv4aPricingLookups look) {
        this.jdbc = jdbc; this.ctx = ctx; this.look = look;
    }

    /** frmItemPricingSchedule_Load :167. "No Policy Types Define" when the policy read returns nothing (:226). */
    public Map<String, Object> init() {
        ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("priceTypeId", PRICE_TYPE);
        List<Map<String, Object>> policies = DesktopProc.rows(jdbc, "Sp_drmLookUpsGetAllMethods", params("Activity", "GetDiscountsPolicyByLookupTypeIds"));
        out.put("policyTypesFound", !policies.isEmpty());
        out.put("policyTypes", keep(policies, "Regular Discount"));
        if (policies.isEmpty()) return out;                                             // :224-227 — nothing else is bound
        out.put("targetTypes", keep(lookups(7), "Value"));                              // TargetTypeFill :323
        out.put("discountTypes", keep(lookups(1), "Percent Discount On Item"));         // DiscountTypeFill :294
        out.put("items", items(0, 0));                                                  // ItemFill :404
        out.put("categories", look.categoriesFromPricingSchedule(PRICE_TYPE));          // ItemCategoryFill :352
        out.put("itemTypes", look.typesFromPricingSchedule(PRICE_TYPE));                // ItemTypeFill :376
        return out;
    }

    private List<Map<String, Object>> lookups(int lookUpTypeId) {
        return DesktopProc.rows(jdbc, "Sp_drmLookUpsGetAllMethods", params("Id", lookUpTypeId, "Activity", "GetByLookUpsTypeId"));
    }

    private static List<Map<String, Object>> keep(List<Map<String, Object>> rows, String lookUpName) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (lookUpName.equals(str(ci(r, "LookUpName")))) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", num(ci(r, "Id")));
                o.put("Name", str(ci(r, "LookUpName")));
                out.add(o);
            }
        }
        return out;
    }

    /** ReadByItemIdAndPriceLookUpId (:423 all items; :542 GetFilteredItems with category/type) → (ItemId, ItemName). */
    public List<Map<String, Object>> items(int itemCategoryId, int itemTypeId) {
        ctx.requireAccountingUser();
        Map<String, Object> a = params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "PriceTypeId", PRICE_TYPE);
        if (itemCategoryId != 0) a.put("ItemCategoryId", itemCategoryId);
        if (itemTypeId != 0) a.put("ItemTypeId", itemTypeId);
        a.put("Activity", "ReadByItemIdAndPriceLookUpId");
        return Inv4aSupport.idName(DesktopProc.rows(jdbc, "Sp_ItemPricingSchedule_GetAllMethod", a), "ItemId", "ItemName");
    }

    /** FormHistory :658 — the ten grid columns of :668-677 built from DiscountingHistory rows (:688). */
    public List<Map<String, Object>> history() {
        ctx.requireAccountingUser();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ItemDiscountingSchedule_GetAllMethod", params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "PriceTypeId", PRICE_TYPE,
                "Activity", "DiscountingHistory"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "Id")));
            o.put("DocNo", str(ci(r, "DiscountDocNo")));
            o.put("FromDate", Inv4aSupport.dayText(ci(r, "FromDate")));
            o.put("ToDate", Inv4aSupport.dayText(ci(r, "ToDate")));
            o.put("DiscountName", str(ci(r, "DiscountName")));
            o.put("DiscountType", str(ci(r, "DiscountType")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("Percent", str(ci(r, "DiscountValue")));
            o.put("DiscountDescription", str(ci(r, "DiscountDescription")));
            o.put("Status", str(ci(r, "Status")));
            out.add(o);
        }
        return out;
    }

    /**
     * Insert() :573 — FormValiadation (:836) first, then the confirm, then "At Least One Item Required" (:600) and
     * "Item Required" (:635) per row; Sp_ItemDiscountingSchedule_Insert per grid row with the fixed values of :607-631.
     * The page does the confirm in the desktop's order; this method re-runs the checks and saves in one transaction.
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        ctx.requireAccountingUser();
        validate(body);
        List<Map<String, Object>> rows = Inv4aSupport.rowsOf(body, "rows");
        if (rows.isEmpty()) throw new IllegalArgumentException("At Least One Item Required");
        Timestamp now = Inv4aSupport.now();
        Timestamp from = day(str(body.get("fromDate"))), to = day(str(body.get("toDate")));
        if (from == null || to == null) throw new IllegalArgumentException("From Date and To Date are required.");
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (num(r.get("ItemId")) == 0) throw new IllegalArgumentException("Item Required");
            Map<String, Object> d = new LinkedHashMap<>();                       // Sp_ItemDiscountingSchedule_Insert parameters
            d.put("Id", 0);                                                      // RecId (always 0, see class note)
            d.put("DiscountName", str(body.get("discountName")));
            d.put("DiscountDescription", str(body.get("discountDescription")));
            d.put("MasterDiscountTypeId", num(body.get("policyId")));
            d.put("drmLookUpId", num(body.get("discountTypeId")));
            d.put("ItemId", num(r.get("ItemId")));
            d.put("XGroupId", 0);
            d.put("YGroupId", 0);
            d.put("ToDate", to);
            d.put("FromDate", from);
            d.put("FromTime", now);                                              // hidden pickers = DateTime.Now (:171-174, :760)
            d.put("ToTime", now);
            d.put("DiscountValue", dbl(r.get("ItemPercent")));
            d.put("EntryDate", now);
            d.put("EntryUser", ctx.currentUserId());
            d.put("ModifyDate", now);
            d.put("ModifyUser", ctx.currentUserId());
            d.put("OrganizationId", ctx.currentOrganizationId());
            d.put("CompanyId", ctx.currentCompanyId());
            d.put("BranchesId", 0);
            d.put("PriceTypeId", PRICE_TYPE);
            d.put("DiscountDocNo", 0);
            d.put("WarehouseId", 0);
            d.put("StatusChangedDate", now);
            d.put("PostDate", now);
            d.put("PostUser", ctx.currentUserId());
            d.put("DiscountStatus", "Active");
            d.put("TargetTypeId", num(body.get("targetTypeId")));
            d.put("RangeFrom", 1d);
            d.put("RangeTo", 50000000d);
            details.add(d);
        }
        int id = 0;
        for (Map<String, Object> d : details) {
            Integer v = DesktopProc.scalar(jdbc, "Sp_ItemDiscountingSchedule_Insert", d);
            if (v != null) id = v;
        }
        /* :642-649 — "Save Successfully" only when the save returned an id > 0 (RecId is 0); otherwise no message. */
        return Inv4aSupport.ok(id > 0 ? "Save Successfully" : "");
    }

    /** FormValiadation :836 — the four messages in order; the page sends the combo texts to test "ActiveRow == null". */
    public Map<String, Object> validate(Map<String, Object> body) {
        ctx.requireAccountingUser();
        if (str(body.get("discountName")).isEmpty()) throw new IllegalArgumentException("Discount Name Required");
        if (num(body.get("discountTypeId")) == 0) throw new IllegalArgumentException("Discount Type Required");
        if (num(body.get("targetTypeId")) == 0) throw new IllegalArgumentException("Target Type Required");
        if (num(body.get("discountById")) == 0) throw new IllegalArgumentException("Discount By Required");
        return Inv4aSupport.ok("");
    }
}
