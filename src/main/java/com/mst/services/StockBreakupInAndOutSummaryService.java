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
 * Stock Breakup In &amp; Out Summary - the whole of the Audit Dashboard module.
 *
 * Ported from Architecture.WinApp.Audit_Dashboard\StockBreakupInAndOutSummary.cs (625 lines).
 * Its caption on the desktop is
 * "Stock Opening,Purchase,Production From Paddy Process and Sales Summary" (:479).
 *
 * Procedure contract, from
 * BLL.Inventory.InventoryStockEvalautionDetail.TotalStockBreakup_InAndOut_Summary (:3957-4012):
 *
 *   usp_TotalStockBreakup_InAndOut_Summary
 *       @OrganizationId, @CompanyId, @Activity            always
 *       @FromDate                                         ONLY when the From date is ticked
 *       @ToDate                                           always
 *       @ActionId                                         ONLY when non-zero
 *       @SortNo                                           ONLY when non-zero
 *
 * The form passes @Activity = "Summary" and @ActionId = 2 when the "Production From Paddy
 * (Custom Rate &amp; Amount)" radio is chosen, 0 for the System Rate one - and zero means the
 * parameter is left off entirely, which is not the same as passing NULL.
 *
 * The From date is a DateTimePicker with ShowCheckBox: Reset() sets Checked = false (:84), so on
 * a fresh load the report runs with NO @FromDate at all. That is reproduced with an explicit
 * tick-box on the web form rather than silently sending the financial year start.
 *
 * Returned columns: SortNo, TransType, Activity, ParentCategory, ClassGroup, ItemQty, Weight,
 * Amount, AvgRate, RecoveryPercent.
 *
 * Read-only: the desktop form has no save, update or delete. Its "Generate" button only
 * recalculates the shortage row in memory - see the JavaScript - and never writes to the
 * database, so nothing is written here either.
 */
@Service
public class StockBreakupInAndOutSummaryService {
    private static final Logger LOG =
            LoggerFactory.getLogger(StockBreakupInAndOutSummaryService.class);

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    /**
     * GridFill(), form :107-155.
     *
     * @param fromDate  null or empty when the From box is unticked - the parameter is then omitted
     * @param customRate true for the Custom Rate radio, which sends @ActionId = 2
     */
    public Map<String, Object> report(String fromDate, String toDate, boolean customRate) {
        Map<String, Object> out = new LinkedHashMap<>();
        try {
            List<String> names = new ArrayList<>();
            List<Object> args = new ArrayList<>();
            names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
            names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
            names.add("@Activity");       args.add("Summary");

            Object from = date(fromDate);
            if (from != null) { names.add("@FromDate"); args.add(from); }

            names.add("@ToDate"); args.add(date(toDate));

            /* RdCustomRateAmount.Checked ? 2 : 0, and the BLL omits it when it is 0 (:3990). */
            if (customRate) { names.add("@ActionId"); args.add(2); }

            StringBuilder sql = new StringBuilder("EXEC usp_TotalStockBreakup_InAndOut_Summary ");
            for (int i = 0; i < names.size(); i++) {
                if (i > 0) sql.append(", ");
                sql.append(names.get(i)).append("=?");
            }

            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> r : jdbcTemplate.queryForList(sql.toString(), args.toArray())) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("SortNo", asInt(col(r, "SortNo")));
                m.put("TransType", str(col(r, "TransType")));
                m.put("Activity", str(col(r, "Activity")));
                m.put("ParentCategory", str(col(r, "ParentCategory")));
                m.put("ClassGroup", str(col(r, "ClassGroup")));
                m.put("ItemQty", num(col(r, "ItemQty")));
                m.put("Weight", num(col(r, "Weight")));
                m.put("Amount", num(col(r, "Amount")));
                m.put("AvgRate", num(col(r, "AvgRate")));
                /* The grid column is captioned Recovery% but the procedure returns
                   RecoveryPercent - GridFill() maps one to the other at :139. */
                m.put("RecoveryPercent", num(col(r, "RecoveryPercent")));
                rows.add(m);
            }
            out.put("rows", rows);
        } catch (Exception e) {
            LOG.error("Stock breakup in/out summary failed", e);
            out.put("rows", Collections.emptyList());
            out.put("error", e.getMessage());
        }
        return out;
    }

    /**
     * The financial year's start, which Load() puts in the From box (:96). The box itself starts
     * unticked, so this is only the value shown, not a filter that is applied.
     */
    public Map<String, Object> defaults() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("fromDate", "");
        out.put("toDate", LocalDate.now().toString());
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "SELECT Start_Period, End_Period FROM FinancialYear WHERE Id = ?",
                    currentUserContext.currentFinancialYearId());
            if (!rows.isEmpty()) {
                out.put("fromDate", ymd(col(rows.get(0), "Start_Period")));
            }
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
