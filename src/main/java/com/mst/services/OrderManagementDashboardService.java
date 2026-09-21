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
 * 1001 Order Management Dashboard.
 *
 * Ported from Architecture.WinApp.Dashboard\OrderManagementDashboard.cs (283 lines) with the
 * card controls Architecture.WinApp.DynamicCards\OrderManageMentCardHeader.cs and
 * OrderManagementInfoCard.cs.
 *
 * Procedure contract, from BLL.Dashboard.SPU_Invetnory_OrderManagementDashBoards (:1069-1095):
 *
 *   SPU_Invetnory_OrderManagementDashBoards @OrganizationId, @CompanyId, @ToDate
 *
 * All three are always sent; the form passes DateTime.Now for @ToDate (:78).
 *
 * Returned columns: TranType ("Purchase" / "Sales"), CardCount, ActivityDescription, ItemId,
 * ItemName, OrderBalanceQty, OrderBalalanceWeight (misspelled at source), StockBalanceQty,
 * StockBalanceWeight, NoOfParties, OrderCount.
 *
 * The form splits the rows by TranType into two panels, and within each panel groups them by
 * CardCount - one card per group, titled with that group's ActivityDescription (:88-124 and
 * OrderManageMentCardHeader :74-99). That grouping is done here so the page renders the same
 * shape.
 *
 * Read-only: no save, update or delete on the desktop form.
 */
@Service
public class OrderManagementDashboardService {

    private static final Logger LOG = LoggerFactory.getLogger(OrderManagementDashboardService.class);

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    /** GetDataAndGenerateCards(), form :65-128. */
    public Map<String, Object> cards(String toDate) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> purchase = new ArrayList<>();
        List<Map<String, Object>> sales = new ArrayList<>();
        try {
            Object to = date(toDate);
            if (to == null) to = java.sql.Date.valueOf(LocalDate.now());

            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC SPU_Invetnory_OrderManagementDashBoards @OrganizationId=?, @CompanyId=?, @ToDate=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    to);

            Map<String, Map<String, Object>> groups = new LinkedHashMap<>();
            for (Map<String, Object> r : rows) {
                String tranType = str(col(r, "TranType"));
                int cardCount = asInt(col(r, "CardCount"));
                String key = tranType + "#" + cardCount;

                Map<String, Object> card = groups.get(key);
                if (card == null) {
                    card = new LinkedHashMap<>();
                    card.put("tranType", tranType);
                    card.put("cardCount", cardCount);
                    /* OrderManageMentCardHeader :76 - the title is the first row's own. */
                    card.put("title", str(col(r, "ActivityDescription")));
                    card.put("items", new ArrayList<Map<String, Object>>());
                    groups.put(key, card);
                    if ("Sales".equals(tranType)) sales.add(card);
                    else if ("Purchase".equals(tranType)) purchase.add(card);
                }

                Map<String, Object> item = new LinkedHashMap<>();
                item.put("itemId", asInt(col(r, "ItemId")));
                item.put("itemName", str(col(r, "ItemName")));
                item.put("orderQty", num(col(r, "OrderBalanceQty")));
                /* "OrderBalalanceWeight" - spelled that way by the procedure, read as it comes. */
                item.put("orderWeight", num(col(r, "OrderBalalanceWeight")));
                item.put("stockQty", num(col(r, "StockBalanceQty")));
                item.put("stockWeight", num(col(r, "StockBalanceWeight")));
                item.put("noOfParties", num(col(r, "NoOfParties")));
                item.put("noOfOrders", num(col(r, "OrderCount")));

                @SuppressWarnings("unchecked")
                List<Map<String, Object>> items = (List<Map<String, Object>>) card.get("items");
                items.add(item);
            }
        } catch (Exception e) {
            LOG.error("Order management dashboard failed", e);
            out.put("error", e.getMessage());
        }
        out.put("purchase", purchase);
        out.put("sales", sales);
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
        catch (Exception e) { return null; }
    }
}
