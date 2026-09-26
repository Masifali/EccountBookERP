package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Purchase Analytic Periodic ItemWise.
 *
 * Ported from Architecture.WinApp.AnalyticDashboard\PurchaseAnalyticPeriodicItemWise.cs
 * (715 lines). This is the screen the Purchase Analytics (Periodic) card caption opens, and it
 * is also a DashBoard entry of its own under Purchase &amp; Sales Dashboard.
 *
 * Procedure contract (BLL.Inventory.InvPurchaseInvoice.PurchaseAnalyticsAB_Report :2778-2812):
 *
 *   USP_PurchaseAnalyticsAB_Report
 *       @OrganizationId, @CompanyId                    always
 *       @BranchId                                      only when non-zero - the form never sets it
 *       @FromDate, @ToDate, @SeasonStart, @SeasonEnd   always
 *       @ParentCategoryIdsForReports                   only when non-empty
 *       @SortNo                                        only when non-zero - not used here
 *       @ItemId                                        only when non-zero - not used here
 *       @Activity                                      always, = 'Product_Wise'
 *
 * Parent categories come from
 * USP_GetParentCategoriesForPurchaseAndSaleReports @OrganizationId, @CompanyId
 * (BLL.Common.CommonServies :413-434) and the combo is a CHECKED list, so more than one can be
 * picked and their ids go over as a comma-separated string. btnshow_Click (:259-262) refuses to
 * run with none selected: "Please Select a Parent Category".
 *
 * Season dates are the same USP_SeasonYearSchedule_GetAll the periodic screen uses.
 *
 * Read-only: no save, update or delete on the desktop form.
 */
@Service
public class PurchaseAnalyticItemWiseService {
    private static final Logger LOG = LoggerFactory.getLogger(PurchaseAnalyticItemWiseService.class);

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    /** GetSeasonScheduleDates(), form :154-187 - identical to the periodic screen's. */
    public Map<String, Object> seasonSchedule() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("found", false);
        out.put("message", "No Schedule found. Please Make a season year schedule first!");
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC USP_SeasonYearSchedule_GetAll @OrganizationId=?, @CompanyId=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId());
            if (!rows.isEmpty()) {
                Map<String, Object> r = rows.get(0);
                out.put("found", true);
                out.put("message", "");
                out.put("seasonStartDate", ymd(col(r, "SeasonStartDate")));
                out.put("seasonEndDate", ymd(col(r, "SeasonEndDate")));
                out.put("fromDate", ymd(col(r, "SeasonStartDate")));
                out.put("toDate", LocalDate.now().toString());
            }
        } catch (Exception e) {
            LOG.error("Season year schedule read failed", e);
            out.put("message", e.getMessage());
            out.put("error", true);
        }
        return out;
    }

    /** ParentCategoryComboFill(), form :126-152. */
    public List<Map<String, Object>> parentCategories() {
        try {
            return jdbcTemplate.queryForList(
                    "EXEC USP_GetParentCategoriesForPurchaseAndSaleReports @OrganizationId=?, @CompanyId=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId());
        } catch (Exception e) {
            LOG.error("Parent categories read failed", e);
            return Collections.emptyList();
        }
    }

    /**
     * btnshow_Click, form :238-341.
     *
     * Cards are grouped by ItemId in first-seen order, captioned with that group's ItemName.
     * "This Season" is relabelled "Current Season" (:327-330), the same relabel the periodic
     * screen does.
     *
     * Card grid for RequestFor = "Product_Wise" (PurchaseAnalyticCard :225-231): Supplier and
     * Exp+Amount are hidden and Descriptions becomes a link; Empty Shell / Trash, Dust / Stone,
     * AGL and Damage are hidden globally (:161-164). Whether Moisture or Broken shows depends on
     * ParentIds: the card hides Moisture when ParentIds contains "2" and otherwise hides Broken
     * (:202-210). That test is on the string, so it is returned to the page to apply per card.
     *
     * Three procedure columns are misspelled at the source and are read exactly as they arrive:
     * "Dust/Stone" (no spaces), and on the periodic screen "Suppplier" and "AmountWithExp".
     */
    public Map<String, Object> itemWise(String fromDate, String toDate,
                                        String seasonStart, String seasonEnd, String ids) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> cards = new ArrayList<>();

        if (ids == null || ids.trim().isEmpty()) {
            /* :259-262 - the form's own message, and it does not run the report. */
            out.put("cards", Collections.emptyList());
            out.put("error", "Please Select a Parent Category");
            return out;
        }

        try {
            List<String> names = new ArrayList<>();
            List<Object> args = new ArrayList<>();
            names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
            names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
            names.add("@FromDate");       args.add(date(fromDate));
            names.add("@ToDate");         args.add(date(toDate));
            names.add("@SeasonStart");    args.add(date(seasonStart));
            names.add("@SeasonEnd");      args.add(date(seasonEnd));
            names.add("@ParentCategoryIdsForReports"); args.add(ids.trim());
            names.add("@Activity");       args.add("Product_Wise");

            StringBuilder sql = new StringBuilder("EXEC USP_PurchaseAnalyticsAB_Report ");
            for (int i = 0; i < names.size(); i++) {
                if (i > 0) sql.append(", ");
                sql.append(names.get(i)).append("=?");
            }

            Map<String, Map<String, Object>> byItem = new LinkedHashMap<>();
            for (Map<String, Object> r : jdbcTemplate.queryForList(sql.toString(), args.toArray())) {
                String key = String.valueOf(asInt(col(r, "ItemId")));
                Map<String, Object> card = byItem.get(key);
                if (card == null) {
                    card = new LinkedHashMap<>();
                    card.put("id", asInt(col(r, "ItemId")));
                    card.put("caption", str(col(r, "ItemName")));
                    card.put("rows", new ArrayList<Map<String, Object>>());
                    byItem.put(key, card);
                    cards.add(card);
                }

                String desc = str(col(r, "Descriptions"));
                if ("This Season".equals(desc)) desc = "Current Season";

                Map<String, Object> line = new LinkedHashMap<>();
                line.put("SortNo", asInt(col(r, "SortNo")));
                line.put("Descriptions", desc);
                line.put("Qty", num(col(r, "Qty")));
                line.put("Weight", num(col(r, "NetWeight")));
                line.put("ItemRate", num(col(r, "Rate")));
                line.put("Exp40Kg", num(col(r, "Exp40Kg")));
                line.put("AvgRate", num(col(r, "AvgRate")));
                line.put("AnalysisQty", num(col(r, "AnalysisQty")));
                line.put("Moisture", num(col(r, "Moisture")));
                line.put("Broken", num(col(r, "Broken")));

                @SuppressWarnings("unchecked")
                List<Map<String, Object>> lines = (List<Map<String, Object>>) card.get("rows");
                lines.add(line);
            }
            out.put("cards", cards);
            /* PurchaseAnalyticCard :202-210 tests the ParentIds STRING for "2". */
            out.put("parentIds", ids.trim());
            out.put("hideMoisture", ids.contains("2"));
        } catch (Exception e) {
            LOG.error("Purchase analytics item-wise failed", e);
            out.put("cards", Collections.emptyList());
            out.put("error", e.getMessage());
        }
        return out;
    }

    // ---------------------------------------------------------------- helpers

    private static Object col(Map<String, Object> row, String name) {
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    private static int asInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    private static double num(Object o) {
        if (o == null) return 0d;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0d; }
    }

    private static Object date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, 10))); }
        catch (Exception e) { LOG.warn("lookup failed", e); return null; }
    }

    private static String ymd(Object o) {
        if (o == null) return "";
        String s = String.valueOf(o);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }
}
