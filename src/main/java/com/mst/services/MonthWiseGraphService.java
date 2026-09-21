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
 * Monthly Purchase (Graph) Report and Monthly Sale (Graph) Report.
 *
 * Ported from Architecture.WinApp.Graph\MonthWisePurchaseReport.cs (1,156 lines) and
 * MonthWiseSaleReport.cs (1,161 lines). The two forms are the same screen against two different
 * procedures, so they share this service and differ only by the constants below - but they keep
 * separate routes and separate pages, because they are separate DashBoard entries.
 *
 * Procedure contracts, read from the BLL:
 *
 *   Purchase  BLL.Inventory.InvPurchaseInvoice.InventoryPurchsaeComparisonsMonthWise
 *             SPU_InventoryPurchsaeComparisonsMonthWise
 *                 @Organizationid, @companyId          - note the casing, exactly as declared
 *                 @FromDate, @ToDate                   - only when set
 *                 @ByMonth                             - only when > 0
 *                 @InventoryParentCategoriesId         - only when > 0
 *                 @ItemClassGroupId                    - only when > 0
 *
 *   Sale      BLL.Reports.Inventory.InvSaleInvoiceReports.SalesComparisonsQuarter
 *             SPU_InventorySalesComparisons
 *                 @OrganizationId, @CompanyId, @FromDate, @ToDate   - always
 *                 @ByMonth / @InventoryParentCategoriesId / @ItemClassGroupId - only when non-zero
 *
 * Both return a DataSet and both forms read Tables[1] (quarters) and Tables[2] (months), so all
 * result sets are read here rather than only the first.
 *
 * The column names differ between the two, which is why they are constants rather than shared:
 *
 *                     month label      month amount          quarter amount
 *   Purchase          PurchaseMonth    CurrPurchaseAmount    CurrAmount
 *   Sale              SalesMonth       CurrSaleAmount        CurrSaleAmount
 *
 * The quarter amount really is named differently on the two sides - MonthWisePurchaseReport :268
 * reads "CurrAmount" while MonthWiseSaleReport :274 reads "CurrSaleAmount".
 *
 * Dates are always the financial year's start and end (:243-244 / :249-250); neither form gives
 * the user a date box.
 *
 * Combos:
 *   Purchase  [dbo].[Usp_AllComboAgainstPurchaseInvoice] @OrganizationId, @CompanyId
 *   Sale      [dbo].[Usp_AllComboAgainstSaleInvoice]     @OrganizationId, @CompanyId,
 *                                                        @AppId, @UserId
 * Both return one flat table that the form splits by an Activity column into
 * "ItemParentCategory" and "ItemClassGroup", taking Id and ReferenceName (:173-190).
 *
 * Read-only: neither form has a save, update or delete.
 */
@Service
public class MonthWiseGraphService {

    private static final Logger LOG = LoggerFactory.getLogger(MonthWiseGraphService.class);

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    /** The twelve months, hard-coded on both desktop forms (:128-152) - not a database list. */
    public static final String[] MONTHS = {
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    };

    // ---------------------------------------------------------------- combos

    /** CombosAgainstPurchaseInvoice() / its sale twin, :156-213. */
    public Map<String, Object> combos(boolean sale) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> categories = new ArrayList<>();
        List<Map<String, Object>> classGroups = new ArrayList<>();
        try {
            List<Map<String, Object>> rows;
            if (sale) {
                rows = jdbcTemplate.queryForList(
                        "EXEC [dbo].[Usp_AllComboAgainstSaleInvoice] @OrganizationId=?, @CompanyId=?, "
                      + "@AppId=?, @UserId=?",
                        currentUserContext.currentOrganizationId(),
                        currentUserContext.currentCompanyId(),
                        currentUserContext.currentAppId(),
                        currentUserContext.currentUserId());
            } else {
                rows = jdbcTemplate.queryForList(
                        "EXEC [dbo].[Usp_AllComboAgainstPurchaseInvoice] @OrganizationId=?, @CompanyId=?",
                        currentUserContext.currentOrganizationId(),
                        currentUserContext.currentCompanyId());
            }
            for (Map<String, Object> r : rows) {
                String activity = str(col(r, "Activity"));
                Map<String, Object> item = new LinkedHashMap<>();
                item.put("id", asInt(col(r, "Id")));
                item.put("name", str(col(r, "ReferenceName")));
                if ("ItemParentCategory".equals(activity))   categories.add(item);
                else if ("ItemClassGroup".equals(activity))  classGroups.add(item);
            }
        } catch (Exception e) {
            LOG.error("Combos read failed (sale={})", sale, e);
            out.put("error", e.getMessage());
        }
        out.put("categories", categories);
        out.put("classGroups", classGroups);
        out.put("months", MONTHS);
        return out;
    }

    // ---------------------------------------------------------------- the report

    /** ShowReport(), :227-330 on both forms. */
    public Map<String, Object> report(boolean sale, int month, int categoryId, int classGroupId) {
        Map<String, Object> out = new LinkedHashMap<>();
        try {
            Map<String, String> fy = financialYear();
            final Object from = date(fy.get("start"));
            final Object to = date(fy.get("end"));
            final int orgId = currentUserContext.currentOrganizationId();
            final int compId = currentUserContext.currentCompanyId();

            List<String> names = new ArrayList<>();
            List<Object> args = new ArrayList<>();
            if (sale) {
                names.add("@OrganizationId"); args.add(orgId);
                names.add("@CompanyId");      args.add(compId);
            } else {
                /* Declared as @Organizationid / @companyId on the purchase side. */
                names.add("@Organizationid"); args.add(orgId);
                names.add("@companyId");      args.add(compId);
            }
            names.add("@FromDate"); args.add(from);
            names.add("@ToDate");   args.add(to);
            if (month > 0)        { names.add("@ByMonth");                     args.add(month); }
            if (categoryId > 0)   { names.add("@InventoryParentCategoriesId"); args.add(categoryId); }
            if (classGroupId > 0) { names.add("@ItemClassGroupId");            args.add(classGroupId); }

            StringBuilder sb = new StringBuilder("EXEC ")
                    .append(sale ? "SPU_InventorySalesComparisons"
                                 : "SPU_InventoryPurchsaeComparisonsMonthWise").append(' ');
            for (int i = 0; i < names.size(); i++) {
                if (i > 0) sb.append(", ");
                sb.append(names.get(i)).append("=?");
            }
            final String sql = sb.toString();
            final Object[] params = args.toArray();

            List<List<Map<String, Object>>> sets = jdbcTemplate.execute(
                (ConnectionCallback<List<List<Map<String, Object>>>>) con -> {
                    List<List<Map<String, Object>>> all = new ArrayList<>();
                    try (PreparedStatement ps = con.prepareStatement(sql)) {
                        for (int i = 0; i < params.length; i++) ps.setObject(i + 1, params[i]);
                        boolean hasResult = ps.execute();
                        while (true) {
                            if (hasResult) {
                                try (ResultSet rs = ps.getResultSet()) { all.add(readAll(rs)); }
                            } else if (ps.getUpdateCount() == -1) {
                                break;
                            }
                            hasResult = ps.getMoreResults();
                            if (!hasResult && ps.getUpdateCount() == -1) break;
                        }
                    }
                    return all;
                });

            /* Tables[2] is the monthly set and Tables[1] the quarterly one - the forms index
               them that way, so the same positions are used rather than guessing by shape. */
            List<Map<String, Object>> quarterSet = sets != null && sets.size() > 1
                    ? sets.get(1) : Collections.emptyList();
            List<Map<String, Object>> monthSet = sets != null && sets.size() > 2
                    ? sets.get(2) : Collections.emptyList();

            String monthLabelCol  = sale ? "SalesMonth"     : "PurchaseMonth";
            String monthAmountCol = sale ? "CurrSaleAmount" : "CurrPurchaseAmount";
            String quarterAmtCol  = sale ? "CurrSaleAmount" : "CurrAmount";

            List<Map<String, Object>> months = new ArrayList<>();
            double monthTotal = 0;
            for (Map<String, Object> r : monthSet) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("month", str(col(r, monthLabelCol)));
                double amt = num(col(r, monthAmountCol));
                m.put("amount", amt);
                monthTotal += amt;
                months.add(m);
            }

            List<Map<String, Object>> quarters = new ArrayList<>();
            double quarterTotal = 0;
            for (Map<String, Object> r : quarterSet) {
                Map<String, Object> q = new LinkedHashMap<>();
                q.put("startDate", ymd(col(r, "QuarterStartDate")));
                q.put("endDate", ymd(col(r, "QuarterEndDate")));
                q.put("year", str(col(r, "YearNo")));
                double amt = num(col(r, quarterAmtCol));
                q.put("amount", amt);
                quarterTotal += amt;
                quarters.add(q);
            }

            out.put("months", months);
            out.put("quarters", quarters);
            out.put("monthTotal", monthTotal);
            out.put("quarterTotal", quarterTotal);
            out.put("fromDate", fy.get("start"));
            out.put("toDate", fy.get("end"));
        } catch (Exception e) {
            LOG.error("Month-wise graph report failed (sale={})", sale, e);
            out.put("months", Collections.emptyList());
            out.put("quarters", Collections.emptyList());
            out.put("error", e.getMessage());
        }
        return out;
    }

    /** The active financial year's period - what both forms send as From and To. */
    private Map<String, String> financialYear() {
        Map<String, String> out = new LinkedHashMap<>();
        out.put("start", "");
        out.put("end", "");
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "SELECT Start_Period, End_Period FROM FinancialYear WHERE Id = ?",
                    currentUserContext.currentFinancialYearId());
            if (!rows.isEmpty()) {
                out.put("start", ymd(col(rows.get(0), "Start_Period")));
                out.put("end", ymd(col(rows.get(0), "End_Period")));
            }
        } catch (Exception e) {
            LOG.error("Financial year read failed", e);
        }
        return out;
    }

    private static List<Map<String, Object>> readAll(ResultSet rs) throws java.sql.SQLException {
        List<Map<String, Object>> out = new ArrayList<>();
        ResultSetMetaData md = rs.getMetaData();
        int n = md.getColumnCount();
        while (rs.next()) {
            Map<String, Object> row = new LinkedHashMap<>();
            for (int i = 1; i <= n; i++) row.put(md.getColumnLabel(i), rs.getObject(i));
            out.add(row);
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
        catch (Exception e) { return null; }
    }

    private static String ymd(Object o) {
        if (o == null) return "";
        String s = String.valueOf(o);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }
}
