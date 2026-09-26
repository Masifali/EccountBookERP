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
 * Purchase Analytics (Periodic).
 *
 * Ported from Architecture.WinApp.AnalyticDashboard\PurchaseAnalyticPeriodic.cs (512 lines) and
 * its drill-down PurchaseAnalyticPeriodicPartyAndItemWise.cs (210 lines). The card control both
 * forms render is Architecture.WinApp.DynamicCards\PurchaseAnalyticCard.cs, which is where the
 * column visibility and number formats below come from.
 *
 * This one desktop form backs TWO of the DashBoard entries - "Purchase Analytics (Periodic)"
 * under Exective DashBoards and "Purchase Analytic Periodic Report" under Purchase & Sales
 * Dashboard - because both ScreenDefinition rows carry the same TargetUrl.
 *
 * Procedure contract, read out of the BLL, not guessed:
 *
 *   Season dates   USP_SeasonYearSchedule_GetAll  @OrganizationId, @CompanyId
 *                  (BLL.SystemUtilities.SeasonYearSchedule.GetAll :26-39)
 *                  -> SeasonStartDate, SeasonEndDate
 *
 *   The cards      USP_PurchaseAnalyticsA_Report  @OrganizationId, @CompanyId,
 *                  [@BranchId only when non-zero], @FromDate, @ToDate, @SeasonStart, @SeasonEnd
 *                  (BLL.Inventory.InvPurchaseInvoice.PurchaseAnalyticsA_Report :2755-2776)
 *                  -> IpcId, ParentCategory, SortNo, Descriptions, Qty, NetWeight, Rate,
 *                     Exp40Kg, AvgRate
 *
 *   Row drill-down USP_PurchaseAnalyticsAB_Report @OrganizationId, @CompanyId,
 *                  [@BranchId], @FromDate, @ToDate, @SeasonStart, @SeasonEnd,
 *                  [@ParentCategoryIdsForReports only when non-empty],
 *                  [@SortNo only when non-zero], [@ItemId only when non-zero], @Activity
 *                  (same class, :2778-2812) with @Activity = 'ProductAndPartyWise'
 *                  -> ItemId, ItemName, SortNo, Suppplier (sic), Descriptions, Qty, NetWeight,
 *                     Rate, Exp40Kg, AvgRate, AmountWithExp, AnalysisQty, Moisture,
 *                     "Empty Shell / Trash", "Dust/Stone", Broken, AGL, Damage
 *
 * The form never sets BranchesId, so @BranchId is omitted here exactly as the BLL omits it -
 * and omitting a parameter is not the same as passing NULL.
 *
 * Read-only: this screen has no save, update or delete, so none is invented for it.
 */
@Service
public class PurchaseAnalyticPeriodicService {

    private static final Logger LOG = LoggerFactory.getLogger(PurchaseAnalyticPeriodicService.class);

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    // ---------------------------------------------------------------- season schedule

    /**
     * GetSeasonScheduleDates(), form :85-113. No row means the form disables Show and says
     * "No Schedule found. Please Make a season year schedule first!" - reproduced verbatim,
     * and nothing is defaulted in its place.
     *
     * On a hit the desktop sets From = SeasonStartDate and To = DateTime.Now (:100-101).
     */
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

    // ---------------------------------------------------------------- the cards

    /**
     * btnshow_Click, form :170-252.
     *
     * The rows are grouped by IpcId in the order they arrive (Distinct() on a DataTable keeps
     * first-seen order), one card per group, captioned by that group's ParentCategory. Each row
     * becomes one grid line, and "This Season" is relabelled "Current Season" (:240-243).
     *
     * Card grid for RequestFor = "Product_Wise_Summ" (PurchaseAnalyticCard :210-225): Supplier,
     * Exp+Amount, Moisture, Broken and AnalysisQty are hidden, as are Empty Shell / Trash,
     * Dust / Stone, AGL and Damage (:161-164). What is left is SortNo, Descriptions, Qty,
     * Weight, ItemRate, Exp40Kg, AvgRate - which is what the web card renders.
     */
    public Map<String, Object> report(String fromDate, String toDate,
                                      String seasonStart, String seasonEnd) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> cards = new ArrayList<>();
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC USP_PurchaseAnalyticsA_Report @OrganizationId=?, @CompanyId=?, "
                  + "@FromDate=?, @ToDate=?, @SeasonStart=?, @SeasonEnd=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    date(fromDate), date(toDate), date(seasonStart), date(seasonEnd));

            Map<String, Map<String, Object>> byParent = new LinkedHashMap<>();
            for (Map<String, Object> r : rows) {
                String ipcId = String.valueOf(asInt(col(r, "IpcId")));
                Map<String, Object> card = byParent.get(ipcId);
                if (card == null) {
                    card = new LinkedHashMap<>();
                    card.put("id", asInt(col(r, "IpcId")));
                    card.put("caption", str(col(r, "ParentCategory")));
                    card.put("rows", new ArrayList<Map<String, Object>>());
                    byParent.put(ipcId, card);
                    cards.add(card);
                }

                /* :240 - the procedure's own wording is kept except for this one relabel. */
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

                @SuppressWarnings("unchecked")
                List<Map<String, Object>> lines = (List<Map<String, Object>>) card.get("rows");
                lines.add(line);
            }
            out.put("cards", cards);
        } catch (Exception e) {
            LOG.error("Purchase analytics (periodic) report failed", e);
            out.put("cards", Collections.emptyList());
            out.put("error", e.getMessage());
        }
        return out;
    }

    // ---------------------------------------------------------------- the row drill-down

    /**
     * UserControl_Click, form :254-300 - clicking a grid row opens
     * PurchaseAnalyticPeriodicPartyAndItemWise with this card's IpcId as Ids and the clicked
     * row's SortNo, which then runs USP_PurchaseAnalyticsAB_Report with
     * @Activity = 'ProductAndPartyWise' (:85-99) and groups by ItemId, captioning each card
     * with ItemName.
     *
     * Card grid for RequestFor = "ProductAndPartyWise" (PurchaseAnalyticCard :227-233):
     * Descriptions is hidden and a total row is switched on; Empty Shell / Trash, Dust / Stone,
     * AGL and Damage stay hidden (:161-164); and because ParentIds is empty on this card the
     * :202-210 test takes its else branch, which hides Broken and leaves Moisture visible.
     * Exp+Amount carries AggregateFunction 2 - Sum - so that is the total shown.
     *
     * Three of the procedure's column names are misspelled at the source and are read exactly as
     * they come back: "Suppplier", "Dust/Stone", "AmountWithExp".
     */
    public Map<String, Object> partyAndItemWise(String fromDate, String toDate,
                                                String seasonStart, String seasonEnd,
                                                String ids, int sortNo, int itemId) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> cards = new ArrayList<>();
        try {
            List<String> names = new ArrayList<>();
            List<Object> args = new ArrayList<>();
            names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
            names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
            names.add("@FromDate");       args.add(date(fromDate));
            names.add("@ToDate");         args.add(date(toDate));
            names.add("@SeasonStart");    args.add(date(seasonStart));
            names.add("@SeasonEnd");      args.add(date(seasonEnd));
            /* BLL :2793-2804 - each of these three is appended only when it carries a value. */
            if (ids != null && !ids.trim().isEmpty()) {
                names.add("@ParentCategoryIdsForReports"); args.add(ids.trim());
            }
            if (sortNo != 0) { names.add("@SortNo"); args.add(sortNo); }
            if (itemId != 0) { names.add("@ItemId"); args.add(itemId); }
            names.add("@Activity"); args.add("ProductAndPartyWise");

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
                Map<String, Object> line = new LinkedHashMap<>();
                line.put("SortNo", asInt(col(r, "SortNo")));
                line.put("Supplier", str(col(r, "Suppplier")));      // spelled with three p's
                line.put("Qty", num(col(r, "Qty")));
                line.put("Weight", num(col(r, "NetWeight")));
                line.put("ItemRate", num(col(r, "Rate")));
                line.put("Exp40Kg", num(col(r, "Exp40Kg")));
                line.put("AvgRate", num(col(r, "AvgRate")));
                line.put("ExpAmount", num(col(r, "AmountWithExp")));
                line.put("AnalysisQty", num(col(r, "AnalysisQty")));
                line.put("Moisture", num(col(r, "Moisture")));

                @SuppressWarnings("unchecked")
                List<Map<String, Object>> lines = (List<Map<String, Object>>) card.get("rows");
                lines.add(line);
            }
            out.put("cards", cards);
        } catch (Exception e) {
            LOG.error("Purchase analytics party/item drill-down failed", e);
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

    /** The date as the browser sends it, never reformatted through a time zone. */
    private static Object date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, 10))); }
        catch (Exception e) { return null; }
    }

    private static String ymd(Object o) {
        if (o == null) return "";
        String s = String.valueOf(o);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }
}
