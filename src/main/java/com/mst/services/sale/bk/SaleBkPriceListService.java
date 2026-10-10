package com.mst.services.sale.bk;

import com.mst.models.UserAccount;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.*;

/**
 * Screen 764 PriceList = Architecture.WinApp.WholeSale.frmItemPricingSchedule ("Item Pricing Schedule", tabs Form / history).
 * BLL class: Architecture.BLL.Pos.ItemPricingSchedule (the form's "using Architecture.BLL.Pos"), DAL Architecture.DAL.Pos.ItemPricingSchedule.SetData.
 *
 *   frmItemPricingSchedule_Load :123      rights of control name "frmItemPricingSchedule" (CommonServices.SetRightsValueInRightsObject(base.Name)): Save / Update enable;
 *                                        priceTypeId = 7 when Tag == "frmPosRetailPricing", else 6. ScreenName of 764 is "PriceList" -> 6.
 *   ItemFill :184 / ItemCategoryFill :201 / ItemTypeFill :218
 *                                        CommonServices.ItemBindFromPricingSchedule / GetItemCategoryFromPricingShedule / GetItemTypesFromPricingShedule
 *                                        = Sp_ItemPricingSchedule_GetAllMethod, Activity ReadByItemIdAndPriceLookUpId / GetItemCategoryFromPricingShedule /
 *                                        GetItemTypesFromPricingShedule (@OrganizationId, @CompanyId, @PriceTypeId, ...)
 *   GetPreviousPricesByItemId :320       ReadByItemIdAndPriceLookUpId (@ItemIds = "," + ids, @ItemCategoryId / @ItemTypeId when != 0)
 *   Insert :378                          validation order and messages as the form, then ItemPricingSchedule.Save: Id == 0 -> Sp_ItemPricingSchedule_Insert else _Update,
 *                                        one call per grid row, all in one transaction (DAL SetData)
 *   FormHistory :452                     ItemPricingScheduleFormHistory = Sp_ItemPricingSchedule_GetAllMethod Activity FormHistory (@PriceTypeId)
 *   GetByItemId :510                     ItemBindFromPricingSchedule(priceTypeId, ItemId)
 */
@Service
public class SaleBkPriceListService {
    private static final String P_GET = "Sp_ItemPricingSchedule_GetAllMethod";
    /** ScreenDefinition 764 ScreenName "PriceList" is the Tag; only "frmPosRetailPricing" selects price type 7. */
    private static final int PRICE_TYPE_ID = 6;

    private final SaleBkSupport bk;

    public SaleBkPriceListService(SaleBkSupport bk) { this.bk = bk; }

    private static String s(Object o) { return SaleBkSupport.s(o); }

    private List<Map<String, Object>> readByItem(int priceTypeId, String itemIds, int categoryId, int typeId) {
        UserAccount u = bk.user();
        Map<String, Object> p = SaleBkSupport.p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "PriceTypeId", priceTypeId);
        if (itemIds != null && !itemIds.isEmpty()) p.put("ItemIds", itemIds);
        SaleBkSupport.nz(p, "ItemCategoryId", categoryId);
        SaleBkSupport.nz(p, "ItemTypeId", typeId);
        p.put("Activity", "ReadByItemIdAndPriceLookUpId");
        return bk.steel().table(P_GET, p).rows;
    }

    private List<Map<String, Object>> priceLookup(String activity) {
        UserAccount u = bk.user();
        return bk.steel().table(P_GET, SaleBkSupport.p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "PriceTypeId", PRICE_TYPE_ID, "Activity", activity)).rows;
    }

    /** Load: rights, then ItemFill / ItemTypeFill / ItemCategoryFill. A list with no rows is not bound (the combo stays empty). */
    public Map<String, Object> init() {
        Map<String, Boolean> r = bk.sup().rights("frmItemPricingSchedule");
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("canSave", Boolean.TRUE.equals(r.get("save")));
        d.put("canUpdate", Boolean.TRUE.equals(r.get("update")));
        d.put("priceTypeId", PRICE_TYPE_ID);
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> x : readByItem(PRICE_TYPE_ID, "", 0, 0)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", SaleBkSupport.ci(x, "ItemId")); o.put("name", SaleBkSupport.ci(x, "ItemName"));
            items.add(o);
        }
        d.put("items", items);
        List<Map<String, Object>> types = new ArrayList<>();
        for (Map<String, Object> x : priceLookup("GetItemTypesFromPricingShedule")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", SaleBkSupport.ci(x, "ItemTypeId")); o.put("name", SaleBkSupport.ci(x, "TypeDescription"));
            types.add(o);
        }
        d.put("types", types);
        List<Map<String, Object>> cats = new ArrayList<>();
        for (Map<String, Object> x : priceLookup("GetItemCategoryFromPricingShedule")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", SaleBkSupport.ci(x, "ItemCategoryId")); o.put("name", SaleBkSupport.ci(x, "CategoryDescription"));
            cats.add(o);
        }
        d.put("categories", cats);
        return d;
    }

    /** GetPreviousPricesByItemId(Ids, ItemCategoryId, ItemTypeId): {ItemId, ItemName, EffectedDate, ItemPrice} of the procedure rows. */
    public List<Map<String, Object>> previousPrices(String ids, int categoryId, int typeId) {
        String itemIds = ids != null && !ids.isEmpty() ? "," + ids : "";
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> x : readByItem(PRICE_TYPE_ID, itemIds, categoryId, typeId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("ItemId", SaleBkSupport.ci(x, "ItemId"));
            o.put("ItemName", SaleBkSupport.ci(x, "ItemName"));
            o.put("EffectedDate", SaleBkSupport.ci(x, "EffectedDate"));
            o.put("ItemPrice", SaleBkSupport.ci(x, "ItemPrice"));
            out.add(o);
        }
        return out;
    }

    /** FormHistory: Id, ItemId, ItemName, EffectedDate, ItemPrice, EntryUserName. */
    public List<Map<String, Object>> history() {
        UserAccount u = bk.user();
        Map<String, Object> p = SaleBkSupport.p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "FormHistory",
                "PriceTypeId", PRICE_TYPE_ID);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> x : bk.steel().table(P_GET, p).rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String k : new String[]{"Id", "ItemId", "ItemName", "EffectedDate", "ItemPrice", "EntryUserName"}) o.put(k, SaleBkSupport.ci(x, k));
            out.add(o);
        }
        return out;
    }

    /**
     * GetByItemId (history "Edit" button): CommonServices.ItemBindFromPricingSchedule(priceTypeId, ItemId) returns a table of ONLY
     * ItemId, ItemName, ItemPrice, RateUomId, Id, ItemCode, and the form then reads ["ScheduleDesc"] / ["EffectedDate"] from that table, so the desktop
     * throws "Column 'ScheduleDesc' does not belong to table ." after it has cleared the grid. The same call and the same error are kept here.
     * (The desktop has also assigned RecId by then, which would turn the next Save into an Update of every row; that part is NOT reproduced.)
     */
    public Map<String, Object> editInfo(String itemId) {
        List<Map<String, Object>> rows = readByItem(PRICE_TYPE_ID, itemId, 0, 0);
        if (rows.isEmpty()) return SaleBkSupport.p("found", false);
        throw new IllegalArgumentException("Column 'ScheduleDesc' does not belong to table .");
    }

    /** Insert(): validations in the desktop order, then ItemPricingSchedule.Save. Returns the last procedure result. */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = bk.user();
        @SuppressWarnings("unchecked")
        List<Map<String, Object>> rows = (List<Map<String, Object>>) body.get("rows");
        if (rows == null || rows.isEmpty()) throw new IllegalArgumentException("Grid record not found");
        int recId = SaleBkSupport.i(body.get("recId"));
        String desc = s(body.get("description"));
        LocalDate date = LocalDate.parse(s(body.get("effectedDate")).substring(0, 10));
        LocalTime time = s(body.get("effectedTime")).length() >= 5 ? LocalTime.parse(normTime(s(body.get("effectedTime")))) : LocalTime.MIDNIGHT;
        Timestamp effected = Timestamp.valueOf(LocalDateTime.of(date, time));
        List<Map<String, Object>> list = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            int itemId = SaleBkSupport.i(r.get("ItemId"));
            if (itemId == 0) throw new IllegalArgumentException("Item Required");
            double newPrice = SaleBkSupport.dbl(r.get("NewPrice"));
            if (newPrice == 0.0) throw new IllegalArgumentException("Check Item Price This Should be Not Zero");
            Timestamp now = new Timestamp(System.currentTimeMillis());
            Map<String, Object> p = new LinkedHashMap<>();
            p.put("Id", recId);
            p.put("ScheduleDesc", desc);
            p.put("ItemId", itemId);
            p.put("EffectedDate", effected);
            p.put("EntryDate", now);
            p.put("EntryUser", u.getId());
            p.put("ModifyDate", now);
            p.put("ModifyUser", u.getId());
            p.put("OrganizationId", u.getOrganizationId());
            p.put("CompanyId", u.getCompanyId());
            p.put("BranchesId", SaleBkSupport.i(r.get("BranchId")));
            p.put("ChangedPriceLookUpId", PRICE_TYPE_ID);
            p.put("PriceTypeId", PRICE_TYPE_ID);
            p.put("ItemPrice", newPrice);
            p.put("PackingRate", 0.0);
            p.put("Margin", 0.0);
            p.put("PackingUom", 0);
            p.put("RateUomId", 0);
            p.put("CropYearId", 0);
            p.put("IsActive", Boolean.FALSE);
            list.add(p);
        }
        String proc = SaleBkSupport.i(list.get(0).get("Id")) == 0 ? "Sp_ItemPricingSchedule_Insert" : "Sp_ItemPricingSchedule_Update";
        int result = 0;
        for (Map<String, Object> p : list) result = bk.sup().setProcMap(proc, p);
        return SaleBkSupport.p("result", result, "message", recId > 0 ? "Update Successfully" : "Save Successfully");
    }

    private static String normTime(String t) {
        String x = t.trim();
        return x.length() == 5 ? x + ":00" : x.substring(0, 8);
    }
}
