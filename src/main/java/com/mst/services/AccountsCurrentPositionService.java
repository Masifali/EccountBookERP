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
 * Accounts Current Postition (Dashboard).
 *
 * Ported from Architecture.WinApp.Account_Reports\AcFrmDashboard.cs (1,580 lines). Note the
 * project: this form lives in Account_Reports, NOT in the Dashboard project - which is why an
 * earlier search for it came up empty and the screen was wrongly reported as missing.
 *
 * It is one form behind TWO DashBoard entries: "Accounts Current Postition (Dashboard)" appears
 * under both Accounts Dashboard and Exective DashBoards.
 *
 * Procedure contracts, read from the BLL:
 *
 *   Account cards   BLL.SmartApp.Dashboard.ExectiveDashboard (:14+)
 *                   Sp_ExectiveDashboard
 *                       @OrganizationId, @CompanyId, @FinancialYearId   always
 *                       @CompanyIds        only when non-empty
 *                       @BranchesIds       only when non-empty
 *                       @VoucherDateF      only when set
 *                       @VoucherDateT      only when set
 *                       @ActivityId        always - the form sends 1 (:328)
 *                   -> SortingNo, AccountDescription, Opening, CurrDr, CurrCr,
 *                      DiffAmount, DiffStatus, Closing
 *
 *   FCY cards       BLL.Reports.Accounts.VoucherReports.FCYPayablesAndReceivables_Rpt
 *                   @OrganizationId, @CompanyId                 always
 *                       @CompanyIds / @BranchesIds              only when non-empty
 *                       @FromDate / @ToDate                     only when set
 *                       @AccountTypeId / @ReportTypeId          only when non-zero - unset here
 *                       @ResultTypId   <- ActionId, the form sends 1 (:216)
 *                       @TransTypeId   <- ActivityId, unset here
 *                       @ExchangeRateUSD, @ExchangeRateEURO     always
 *                   -> TypeId, TypeName, CurrencyCode, FcyAmount, LcyAmount, LastExRate
 *
 *   Companies       Sp_Company_GetAllMethod @OrgCompanyTypeId [, @Id], @Activity
 *
 * Two ERP features gate the branch picker: 17 (branches) and 18 (consolidated), read at :497-498.
 *
 * Read-only: the desktop form has no save, update or delete.
 */
@Service
public class AccountsCurrentPositionService {

    private static final Logger LOG = LoggerFactory.getLogger(AccountsCurrentPositionService.class);

    private static final int FEATURE_BRANCHES = 17;
    private static final int FEATURE_CONSOLIDATED = 18;

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    // ---------------------------------------------------------------- setup

    /** AcFrmDashboard_Load, :492-517 - the features, the parameter list, and the companies. */
    public Map<String, Object> setup() {
        Map<String, Object> out = new LinkedHashMap<>();
        boolean branches = featureOn(FEATURE_BRANCHES);
        out.put("branchFeature", branches);
        out.put("consolidated", featureOn(FEATURE_CONSOLIDATED));
        out.put("companyId", safeCompanyId());
        out.put("fromDate", LocalDate.now().toString());   // parameter 1, "This Day"
        out.put("toDate", LocalDate.now().toString());

        List<Map<String, Object>> companies = new ArrayList<>();
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    "EXEC Sp_Company_GetAllMethod @OrgCompanyTypeId=?, @Activity=?",
                    currentUserContext.currentOrganizationId(), "ReadAll")) {
                Map<String, Object> c = new LinkedHashMap<>();
                c.put("id", asInt(col(r, "Id")));
                c.put("name", str(col(r, "CompName")));
                companies.add(c);
            }
        } catch (Exception e) {
            LOG.error("Company list failed", e);
            out.put("companyError", e.getMessage());
        }
        out.put("companies", companies);

        List<Map<String, Object>> branchRows = new ArrayList<>();
        if (branches) {
            try {
                branchRows = jdbcTemplate.queryForList(
                        "EXEC [dbo].[USP_GetBranchsAllocatedToUser] @OrganizationId=?, @CompanyId=?, @UserId=?",
                        currentUserContext.currentOrganizationId(),
                        currentUserContext.currentCompanyId(),
                        currentUserContext.currentUserId());
            } catch (Exception e) {
                LOG.error("Branch list failed", e);
                out.put("branchError", e.getMessage());
            }
        }
        out.put("branches", branchRows);
        return out;
    }

    private boolean featureOn(int featureId) {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "SELECT TOP 1 IsActive FROM ERPFeatures WHERE Id = ?", featureId);
            if (!rows.isEmpty()) {
                Object v = rows.get(0).values().iterator().next();
                if (v instanceof Boolean) return (Boolean) v;
                if (v instanceof Number) return ((Number) v).intValue() != 0;
                String s = String.valueOf(v).trim();
                return "1".equals(s) || "true".equalsIgnoreCase(s);
            }
        } catch (Exception e) {
            LOG.error("ERP feature lookup failed for {}", featureId, e);
        }
        return false;
    }

    private int safeCompanyId() {
        try { return currentUserContext.currentCompanyId(); } catch (Exception e) { return 0; }
    }

    // ---------------------------------------------------------------- the cards

    /** GenerateCards(), :316-354. */
    public Map<String, Object> accountCards(String fromDate, String toDate,
                                            String companyIds, String branchIds) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> cards = new ArrayList<>();
        try {
            List<String> names = new ArrayList<>();
            List<Object> args = new ArrayList<>();
            names.add("@OrganizationId");  args.add(currentUserContext.currentOrganizationId());
            names.add("@CompanyId");       args.add(currentUserContext.currentCompanyId());
            names.add("@FinancialYearId"); args.add(currentUserContext.currentFinancialYearId());
            if (nz(companyIds)) { names.add("@CompanyIds");  args.add(companyIds.trim()); }
            if (nz(branchIds))  { names.add("@BranchesIds"); args.add(branchIds.trim()); }
            Object from = date(fromDate);
            if (from != null) { names.add("@VoucherDateF"); args.add(from); }
            Object to = date(toDate);
            if (to != null)   { names.add("@VoucherDateT"); args.add(to); }
            /* :328 - the form always sends ActivityId 1. */
            names.add("@ActivityId"); args.add(1);

            StringBuilder sql = new StringBuilder("EXEC Sp_ExectiveDashboard ");
            for (int i = 0; i < names.size(); i++) {
                if (i > 0) sql.append(", ");
                sql.append(names.get(i)).append("=?");
            }

            for (Map<String, Object> r : jdbcTemplate.queryForList(sql.toString(), args.toArray())) {
                Map<String, Object> c = new LinkedHashMap<>();
                c.put("id", asInt(col(r, "SortingNo")));
                c.put("title", str(col(r, "AccountDescription")));
                c.put("opening", num(col(r, "Opening")));
                c.put("currDr", num(col(r, "CurrDr")));
                c.put("currCr", num(col(r, "CurrCr")));
                c.put("diffAmount", num(col(r, "DiffAmount")));
                c.put("diffStatus", str(col(r, "DiffStatus")));
                c.put("closing", num(col(r, "Closing")));
                cards.add(c);
            }
            out.put("cards", cards);
        } catch (Exception e) {
            LOG.error("Executive accounts dashboard failed", e);
            out.put("cards", Collections.emptyList());
            out.put("error", e.getMessage());
        }
        return out;
    }

    /**
     * GenerateFcyPayablesReceivablesCards(), :194-265.
     *
     * Rows are grouped by TypeId, one card per group captioned with its TypeName, and each card
     * totals its own FcyAmount and LcyAmount. The "Fcy Receivables Payables" heading is hidden
     * entirely when nothing comes back (:258-262).
     *
     * @ExchangeRateUSD and @ExchangeRateEURO are always sent by the BLL. The form leaves them at
     * their default, so 0 is what the procedure receives - that is the contract as written, and
     * no rate is invented here.
     */
    public Map<String, Object> fcyCards(String fromDate, String toDate,
                                        String companyIds, String branchIds) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> cards = new ArrayList<>();
        try {
            List<String> names = new ArrayList<>();
            List<Object> args = new ArrayList<>();
            names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
            names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
            if (nz(companyIds)) { names.add("@CompanyIds");  args.add(companyIds.trim()); }
            if (nz(branchIds))  { names.add("@BranchesIds"); args.add(branchIds.trim()); }
            Object from = date(fromDate);
            if (from != null) { names.add("@FromDate"); args.add(from); }
            Object to = date(toDate);
            if (to != null)   { names.add("@ToDate");   args.add(to); }
            /* ActionId 1 (:216) binds to @ResultTypId. */
            names.add("@ResultTypId");     args.add(1);
            names.add("@ExchangeRateUSD");  args.add(0);
            names.add("@ExchangeRateEURO"); args.add(0);

            StringBuilder sql = new StringBuilder("EXEC SPU_Accounts_FCYPayablesAndReceivables_Rpt ");
            for (int i = 0; i < names.size(); i++) {
                if (i > 0) sql.append(", ");
                sql.append(names.get(i)).append("=?");
            }

            Map<String, Map<String, Object>> byType = new LinkedHashMap<>();
            for (Map<String, Object> r : jdbcTemplate.queryForList(sql.toString(), args.toArray())) {
                String key = String.valueOf(asInt(col(r, "TypeId")));
                Map<String, Object> card = byType.get(key);
                if (card == null) {
                    card = new LinkedHashMap<>();
                    card.put("typeId", asInt(col(r, "TypeId")));
                    card.put("typeName", str(col(r, "TypeName")));
                    card.put("lines", new ArrayList<Map<String, Object>>());
                    byType.put(key, card);
                    cards.add(card);
                }
                Map<String, Object> line = new LinkedHashMap<>();
                line.put("currencyCode", str(col(r, "CurrencyCode")));
                line.put("fcyAmount", num(col(r, "FcyAmount")));
                line.put("lcyAmount", num(col(r, "LcyAmount")));
                line.put("lastExRate", num(col(r, "LastExRate")));

                @SuppressWarnings("unchecked")
                List<Map<String, Object>> lines = (List<Map<String, Object>>) card.get("lines");
                lines.add(line);
            }
            for (Map<String, Object> card : cards) {
                @SuppressWarnings("unchecked")
                List<Map<String, Object>> lines = (List<Map<String, Object>>) card.get("lines");
                double fcy = 0, lcy = 0;
                for (Map<String, Object> l : lines) {
                    fcy += num(l.get("fcyAmount"));
                    lcy += num(l.get("lcyAmount"));
                }
                card.put("totalFcy", fcy);
                card.put("totalLcy", lcy);
            }
            out.put("cards", cards);
        } catch (Exception e) {
            LOG.error("FCY payables/receivables cards failed", e);
            out.put("cards", Collections.emptyList());
            out.put("error", e.getMessage());
        }
        return out;
    }

    /**
     * cmbperemeter_SelectedIndexChanged, :533-568. Note which choices touch the To date:
     *
     *   1 This Day        From = today          To untouched
     *   2 This Week       From = today - 7      To untouched
     *   3 This Month      From = 1st of month   To = today
     *   4 This Year       From = 1 January      To = today
     *   5 Financial Year  From = year's start   To untouched
     *
     * Only 3 and 4 set the To date; the other three leave it alone. Reproduced as written.
     * Choice 3 uses DateTime.UtcNow on the desktop where the others use local time - server
     * local is used here, as on the Tasks Done screen, and the difference is noted rather than
     * hidden.
     */
    public Map<String, Object> parameter(int id, String currentTo) {
        Map<String, Object> out = new LinkedHashMap<>();
        LocalDate today = LocalDate.now();
        String from, to = (currentTo == null || currentTo.trim().isEmpty())
                ? today.toString() : currentTo;
        switch (id) {
            case 1: from = today.toString(); break;
            case 2: from = today.minusDays(7).toString(); break;
            case 3: from = today.withDayOfMonth(1).toString();      to = today.toString(); break;
            case 4: from = LocalDate.of(today.getYear(), 1, 1).toString(); to = today.toString(); break;
            case 5: from = financialYearStart(); break;
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

    private static boolean nz(String s) { return s != null && !s.trim().isEmpty(); }

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
