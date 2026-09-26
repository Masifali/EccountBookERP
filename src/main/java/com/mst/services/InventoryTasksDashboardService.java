package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Purchase Sale Done Work (Dashboard) - "Tasks Done".
 *
 * Ported from Architecture.WinApp.Dashboard\InventoryDashboardTasksInformation.cs (858 lines),
 * with its three card controls InventoryTaskInfoCard1, InventoryTaskInfoCard2ForWages and
 * InventoryTaskInfoCard3ForWagesValues.
 *
 * Procedure contract, from BLL.Dashboard.TaskDoneStatus_Dashboard:
 *
 *   USP_TaskDoneStatus_Dashboard @OrganizationId, @CompanyId, @FromDate, @ToDate
 *
 * All four are always sent. It returns a DataSet of TWO result sets, and the form uses both
 * (:170-183), so this reads both rather than only the first - which is what JdbcTemplate's
 * queryForList would have given.
 *
 *   Table 0 - tasks:  TransId (1 Purchase / 2 Sale), ParentId, TransTypeKey, TransType,
 *                     VehicleNos, Qty, Weight
 *   Table 1 - wages:  TransId, CardNo, RefDocumentTypeId, TransType, Activity,
 *                     Qty, Weight, AvgRate, Amount
 *
 * Each table is split by TransId into a Purchase and a Sale panel, and the wages rows are then
 * grouped by CardNo, one card per group with its activity lines inside (:236-278).
 *
 * Read-only: no save, update or delete on the desktop form.
 */
@Service
public class InventoryTasksDashboardService {
    private static final Logger LOG = LoggerFactory.getLogger(InventoryTasksDashboardService.class);

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    /**
     * GenerateCards(), form :142-336.
     */
    public Map<String, Object> cards(String fromDate, String toDate) {
        Map<String, Object> out = new LinkedHashMap<>();
        try {
            final Object from = date(fromDate);
            final Object to = date(toDate);
            final int orgId = currentUserContext.currentOrganizationId();
            final int compId = currentUserContext.currentCompanyId();

            List<List<Map<String, Object>>> sets = jdbcTemplate.execute(
                (ConnectionCallback<List<List<Map<String, Object>>>>) con -> {
                    List<List<Map<String, Object>>> all = new ArrayList<>();
                    try (PreparedStatement ps = con.prepareStatement(
                            "EXEC USP_TaskDoneStatus_Dashboard @OrganizationId=?, @CompanyId=?, "
                          + "@FromDate=?, @ToDate=?")) {
                        ps.setInt(1, orgId);
                        ps.setInt(2, compId);
                        ps.setObject(3, from);
                        ps.setObject(4, to);
                        boolean hasResult = ps.execute();
                        while (true) {
                            if (hasResult) {
                                try (ResultSet rs = ps.getResultSet()) {
                                    all.add(readAll(rs));
                                }
                            } else if (ps.getUpdateCount() == -1) {
                                break;
                            }
                            hasResult = ps.getMoreResults();
                            if (!hasResult && ps.getUpdateCount() == -1) break;
                        }
                    }
                    return all;
                });

            List<Map<String, Object>> tasks = sets != null && sets.size() > 0
                    ? sets.get(0) : Collections.emptyList();
            List<Map<String, Object>> wages = sets != null && sets.size() > 1
                    ? sets.get(1) : Collections.emptyList();

            out.put("purchaseTasks", taskCards(tasks, 1));
            out.put("saleTasks",     taskCards(tasks, 2));
            out.put("purchaseWages", wagesCards(wages, 1));
            out.put("saleWages",     wagesCards(wages, 2));
        } catch (Exception e) {
            LOG.error("Task done status dashboard failed", e);
            out.put("purchaseTasks", Collections.emptyList());
            out.put("saleTasks", Collections.emptyList());
            out.put("purchaseWages", Collections.emptyList());
            out.put("saleWages", Collections.emptyList());
            out.put("error", e.getMessage());
        }
        return out;
    }

    /** InventoryTaskInfoCard1 - Title, Vehicles, Quantity, Weight (:182-206). */
    private List<Map<String, Object>> taskCards(List<Map<String, Object>> rows, int transId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (asInt(col(r, "TransId")) != transId) continue;
            Map<String, Object> c = new LinkedHashMap<>();
            c.put("transId", asInt(col(r, "TransId")));
            c.put("parentId", asInt(col(r, "ParentId")));
            c.put("key", str(col(r, "TransTypeKey")));
            c.put("title", str(col(r, "TransType")));
            c.put("vehicleNos", num(col(r, "VehicleNos")));
            c.put("qty", num(col(r, "Qty")));
            c.put("weight", num(col(r, "Weight")));
            out.add(c);
        }
        return out;
    }

    /**
     * InventoryTaskInfoCard2ForWages - grouped by CardNo, one card per group, with an
     * InventoryTaskInfoCard3ForWagesValues line per activity (:236-278).
     *
     * The card's four totals are the desktop's own:
     *   QtyTotal     = sum of Qty
     *   WeightTotal  = sum of Weight
     *   AmountTotal  = sum of Amount
     *   AvgRate      = sum of AvgRate / number of lines   (:275 - a mean OF THE RATES, not
     *                  Amount / Weight; reproduced as written)
     */
    private List<Map<String, Object>> wagesCards(List<Map<String, Object>> rows, int transId) {
        Map<String, Map<String, Object>> byCard = new LinkedHashMap<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (asInt(col(r, "TransId")) != transId) continue;
            String key = String.valueOf(asInt(col(r, "CardNo")));
            Map<String, Object> card = byCard.get(key);
            if (card == null) {
                card = new LinkedHashMap<>();
                card.put("cardNo", asInt(col(r, "CardNo")));
                card.put("transId", asInt(col(r, "TransId")));
                card.put("documentTypeId", asInt(col(r, "RefDocumentTypeId")));
                card.put("title", str(col(r, "TransType")));
                card.put("lines", new ArrayList<Map<String, Object>>());
                byCard.put(key, card);
                out.add(card);
            }
            Map<String, Object> line = new LinkedHashMap<>();
            line.put("activity", str(col(r, "Activity")));
            line.put("qty", num(col(r, "Qty")));
            line.put("weight", num(col(r, "Weight")));
            line.put("rate", num(col(r, "AvgRate")));
            line.put("amount", num(col(r, "Amount")));

            @SuppressWarnings("unchecked")
            List<Map<String, Object>> lines = (List<Map<String, Object>>) card.get("lines");
            lines.add(line);
        }
        for (Map<String, Object> card : out) {
            @SuppressWarnings("unchecked")
            List<Map<String, Object>> lines = (List<Map<String, Object>>) card.get("lines");
            double q = 0, w = 0, a = 0, rateSum = 0;
            for (Map<String, Object> l : lines) {
                q += num(l.get("qty"));
                w += num(l.get("weight"));
                a += num(l.get("amount"));
                rateSum += num(l.get("rate"));
            }
            card.put("qtyTotal", q);
            card.put("weightTotal", w);
            card.put("amountTotal", a);
            card.put("avgRate", lines.isEmpty() ? 0 : rateSum / lines.size());
        }
        return out;
    }

    private static List<Map<String, Object>> readAll(ResultSet rs) throws java.sql.SQLException {
        List<Map<String, Object>> out = new ArrayList<>();
        ResultSetMetaData md = rs.getMetaData();
        int n = md.getColumnCount();
        while (rs.next()) {
            Map<String, Object> row = new LinkedHashMap<>();
            for (int i = 1; i <= n; i++) {
                row.put(md.getColumnLabel(i), rs.getObject(i));
            }
            out.add(row);
        }
        return out;
    }

    /**
     * CmbDateType_ValueChanged, form :441-478. The five choices and the dates they set:
     *
     *   1 This Day        From = today,                 To = today
     *   2 This Week       From = today - 7,             To = today
     *   3 This Month      From = the 1st of this month, To = today
     *   4 This Year       From = 1 January,             To = today
     *   5 Financial Year  From = the year's start       - and To is LEFT ALONE (:472-475)
     *
     * Option 3 uses DateTime.UtcNow on the desktop where the others use local time; on the last
     * day of a month at UTC+5 that can name the previous month. Server-local is used here, which
     * is the only reading that is right in every case - noted so the difference is visible
     * rather than silent.
     */
    public Map<String, Object> dateType(int id, String currentTo) {
        Map<String, Object> out = new LinkedHashMap<>();
        LocalDate today = LocalDate.now();
        String from = "", to = today.toString();
        switch (id) {
            case 1: from = today.toString(); break;
            case 2: from = today.minusDays(7).toString(); break;
            case 3: from = today.withDayOfMonth(1).toString(); break;
            case 4: from = LocalDate.of(today.getYear(), 1, 1).toString(); break;
            case 5:
                from = financialYearStart();
                /* :472-475 - only From is set for this one. */
                to = (currentTo == null || currentTo.trim().isEmpty()) ? today.toString() : currentTo;
                break;
            default: from = today.toString(); break;
        }
        out.put("fromDate", from);
        out.put("toDate", to);
        return out;
    }

    private String financialYearStart() {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "SELECT Start_Period FROM FinancialYear WHERE Id = ?",
                    currentUserContext.currentFinancialYearId());
            if (!rows.isEmpty()) return ymd(col(rows.get(0), "Start_Period"));
        } catch (Exception e) {
            LOG.error("Financial year start read failed", e);
        }
        return LocalDate.now().toString();
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
