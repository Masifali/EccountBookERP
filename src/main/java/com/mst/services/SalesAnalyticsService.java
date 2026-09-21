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
 * Sales Analytics (Sales Analytics Dashboard).
 *
 * Ported from Architecture.WinApp.Dashboard\SalesAnalytics.cs (523 lines).
 *
 * Procedure contract, from
 * BLL.OrganizationLevelDashboard.OrgGroup_WsRmSalesComparison (:274-330):
 *
 *   SpOrgGroup_WsRmSalesComparison
 *       @OrganizationId     always
 *       @CompanyId          only when non-zero
 *       @DateFrom, @DateTo  only when set
 *       @UserId             only when non-zero
 *       @Activity           only when non-empty
 *
 * The form builds its ReportsParameters with OrganizationId, FromDate, ToDate and Activity ONLY
 * (:140-147). CompanyId and EntryUser are left at zero, so @CompanyId and @UserId are omitted -
 * deliberately, because this is an ORGANISATION-level dashboard that compares locations across
 * companies. Sending the current company would silently change the answer, so it is not sent.
 *
 * Activity is "OrganizationTotalSaleComparisonByLocation".
 *
 * NOTE on the second method: the form also defines OrganizationTotalSaleComparisonByItem()
 * (:165-197, @Activity = 'OrganizationTotalSaleComparisonByItem'), but nothing calls it - no
 * button, no menu, no event. It is dead code on the desktop, so it is not exposed here either;
 * adding a control for it would be a feature the desktop does not have.
 *
 * Returned columns (by location): LocationName, LocationAmountTotal, LocationQtyTotal,
 * LocationWeightTotal, OrgLocationPrcnt, plus OrgTotalAmount and AvgRate which the grid hides
 * (:213-214).
 *
 * Read-only: no save, update or delete. Its Print handler calls ShowReport(), which is an empty
 * method body on the desktop (:75-77) - it prints nothing.
 */
@Service
public class SalesAnalyticsService {

    private static final Logger LOG = LoggerFactory.getLogger(SalesAnalyticsService.class);

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    /**
     * OrganizationTotalSaleComparisonByLocation(), form :130-163.
     */
    public Map<String, Object> byLocation(String fromDate, String toDate) {
        Map<String, Object> out = new LinkedHashMap<>();
        try {
            List<String> names = new ArrayList<>();
            List<Object> args = new ArrayList<>();
            names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());

            Object from = date(fromDate);
            if (from != null) { names.add("@DateFrom"); args.add(from); }
            Object to = date(toDate);
            if (to != null)   { names.add("@DateTo");   args.add(to); }

            names.add("@Activity"); args.add("OrganizationTotalSaleComparisonByLocation");

            StringBuilder sql = new StringBuilder("EXEC SpOrgGroup_WsRmSalesComparison ");
            for (int i = 0; i < names.size(); i++) {
                if (i > 0) sql.append(", ");
                sql.append(names.get(i)).append("=?");
            }

            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> r : jdbcTemplate.queryForList(sql.toString(), args.toArray())) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("LocationName", str(col(r, "LocationName")));
                m.put("LocationAmountTotal", num(col(r, "LocationAmountTotal")));
                m.put("LocationQtyTotal", num(col(r, "LocationQtyTotal")));
                m.put("LocationWeightTotal", num(col(r, "LocationWeightTotal")));
                m.put("OrgLocationPrcnt", num(col(r, "OrgLocationPrcnt")));
                rows.add(m);
            }
            out.put("rows", rows);
        } catch (Exception e) {
            LOG.error("Sales analytics by location failed", e);
            out.put("rows", Collections.emptyList());
            out.put("error", e.getMessage());
        }
        return out;
    }

    /**
     * VoucherValidation_Load, form :95-108 - From is the financial year's start, To is today,
     * and the form then runs the report straight away.
     */
    public Map<String, Object> defaults() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("fromDate", "");
        out.put("toDate", LocalDate.now().toString());
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "SELECT Start_Period FROM FinancialYear WHERE Id = ?",
                    currentUserContext.currentFinancialYearId());
            if (!rows.isEmpty()) out.put("fromDate", ymd(col(rows.get(0), "Start_Period")));
        } catch (Exception e) {
            LOG.error("Financial year start read failed", e);
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

    private static String ymd(Object o) {
        if (o == null) return "";
        String s = String.valueOf(o);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }
}
