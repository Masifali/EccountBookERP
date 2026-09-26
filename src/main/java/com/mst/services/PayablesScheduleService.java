package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Payables And Payment Schedule.
 *
 * Ported from Architecture.WinApp.Account_Reports\PayablesAndPaymentSchedule.cs (1,864 lines).
 *
 * Report contract, read from BLL.Reports.Accounts.VoucherReports.PayablesAndPaymentSchedule
 * (0141_Architecture.BLL.Reports.Accounts.VoucherReports.cs:1610-1701):
 *
 *   usp_PayablesAndPaymentSchedule
 *       @OrganizationId, @CompanyId, @UserId, @ParentCategoryId     ALWAYS sent, even as 0
 *       @FromDate          only when datDateFrom is ticked
 *       @ToDate            only when datDateTo   is ticked
 *       @DueDateFrom       only when datDueFrom  is ticked
 *       @DueDateTo         only when datDateTo   is ticked   <-- the same quirk as receivables
 *       @PurchaseFromDate  only when datPurchaseFrom is ticked
 *       @PurchaseToDate    only when datPurchaseTo   is ticked
 *       @CustomGroupId     only when non-zero
 *       @PartyGroupId      only when non-zero
 *       @ControlAccountIds only when non-empty   (a list of ACCOUNT CODES - :465-469)
 *       @AgingDays         only when non-zero    (txtAgingDays, designer default "30")
 *       @BranchesIds       only when non-empty
 *
 *   @BalanceFrom, @BalanceTo, @ActionId and @TradeTypeId are never set by this form, so they
 *   are omitted entirely. Omitting is not the same as sending NULL.
 *
 * TWO RESULT SETS (:317-345): Tables[0] is the detail grid, Tables[1] is the aging summary whose
 * four bucket COLUMN HEADINGS are themselves data - IstIntervale, ScnInterval, TrdIntarval and
 * Above (the source's own spellings) carry the captions, and Value_1..Value_4 the figures. The
 * summary is read with a ConnectionCallback because queryForList would return only the first
 * set.
 *
 * QUIRK, reproduced (:309): Increase/Decrease on THIS form is
 *   Short/Excess &lt; 0 ? "Increase" : "Decrease"
 * There is no "Nill" case, and the sign is the OPPOSITE way round from the receivables form,
 * which reads Short/Excess &gt; 0 as "Increase" and has a "Nill" at exactly zero. Both are kept
 * as they are so each screen returns what its desktop form returns.
 *
 * Read-only: no save, update or delete, and no History button on the desktop form. PayToday is
 * an editable grid cell there (:365-369, EditType 1) but nothing writes it back to the database.
 */
@Service
public class PayablesScheduleService {

    private static final Logger LOG = LoggerFactory.getLogger(PayablesScheduleService.class);

    /** CommonServices.GetERPFeatureById(17) / (18) (:153-154). */
    private static final int FEATURE_BRANCH = 17;
    private static final int FEATURE_BRANCH_CONSOLIDATED = 18;

    /** Designer defaults: txtAgingDays "30" (:1466), txtIntervalDays "30" (:1473, :167). */
    private static final int DEFAULT_AGING_DAYS = 30;
    private static final int DEFAULT_INTERVAL_DAYS = 30;

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    // ------------------------------------------------------------------ setup

    /** PayablesAndPaymentSchedule_Load, :146-179. */
    public Map<String, Object> setup() {
        Map<String, Object> out = new LinkedHashMap<>();

        boolean branchFeature = featureOn(FEATURE_BRANCH);
        out.put("branchFeature", branchFeature);
        out.put("branchConsolidated", featureOn(FEATURE_BRANCH_CONSOLIDATED));

        /* :168-169 - Date From thirty days back, Purchase From txtIntervalDays back. */
        LocalDate today = LocalDate.now();
        out.put("fromDate", today.minusDays(30).toString());
        out.put("toDate", today.toString());
        out.put("dueFrom", today.toString());
        out.put("dueUpto", today.toString());
        out.put("purchaseFrom", today.minusDays(DEFAULT_INTERVAL_DAYS).toString());
        out.put("purchaseTo", today.toString());
        out.put("intervalDays", DEFAULT_INTERVAL_DAYS);
        out.put("agingDays", DEFAULT_AGING_DAYS);

        /* Only datDueFrom starts unticked (:1624); the others default to Checked = true. */
        out.put("chkFrom", true);
        out.put("chkTo", true);
        out.put("chkDueFrom", false);
        out.put("chkDueUpto", true);
        out.put("chkPurchaseFrom", true);
        out.put("chkPurchaseTo", true);

        out.put("decimalsAmount", decimalPointsForAmount());

        String myBranch = "";
        try { myBranch = String.valueOf(currentUserContext.currentBranchId()); } catch (Exception ignored) { }
        out.put("currentBranchId", myBranch);

        out.put("parentCategories", readOrError(out, "parentCategoryError", this::parentCategories));
        out.put("controlAccounts",  readOrError(out, "controlAccountError", this::controlAccounts));
        out.put("customGroups",     readOrError(out, "customGroupError",    this::customGroups));
        out.put("customerGroups",   readOrError(out, "customerGroupError",  this::customerGroups));
        out.put("branches", branchFeature
                ? readOrError(out, "branchError", this::branches)
                : new ArrayList<Map<String, Object>>());
        return out;
    }

    private interface Reader { List<Map<String, Object>> read(); }

    private List<Map<String, Object>> readOrError(Map<String, Object> out, String key, Reader r) {
        try {
            return r.read();
        } catch (Exception e) {
            LOG.error("Dropdown read failed for {}", key, e);
            out.put(key, e.getMessage());
            return new ArrayList<>();
        }
    }

    // -------------------------------------------------------------- dropdowns

    /** ParentCategoryFill, :588-622 - same source as the receivables form. */
    private List<Map<String, Object>> parentCategories() {
        List<Map<String, Object>> all = jdbcTemplate.queryForList(
                "EXEC [Sp_InvPurchaseInvoice_GetAllMethod] @OrganizationId=?, @CompanyId=?, @Activity=?",
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                "GetDataForPurchaseInvoiceRegisterDropdownBind");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : all) {
            if (!"ItemParentCategory".equals(str(col(r, "ActivityType")))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", str(col(r, "Id")));
            m.put("name", str(col(r, "Name")));
            out.add(m);
        }
        return out;
    }

    /**
     * ControlAccountFill3rdLevel, :438-486.
     *
     * The ONLY difference from the receivables form is @AccountClassId: 3 here (payables),
     * 2 there (receivables). The value bound is AccountCode, so @ControlAccountIds is a list of
     * account codes.
     */
    private List<Map<String, Object>> controlAccounts() {
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId=?, @CompanyId=?, "
                        + "@FinancialYearId=?, @Account_Level=?, @AccountTypeId=?, @AccountClassId=?, @CoaType=?",
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                currentUserContext.currentFinancialYearId(),
                3, 3, 3, "ReadAllAccountGroup");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", str(col(r, "AccountCode")));
            m.put("name", str(col(r, "AccountTitle")));
            out.add(m);
        }
        return out;
    }

    /** CustomGroupsFill, :488-520 - CustomeGroupsDefine(1) -> Sp_AcLookUps_GetAllMethod. */
    private List<Map<String, Object>> customGroups() {
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC Sp_AcLookUps_GetAllMethod @OrganizationId=?, @CompanyId=?, "
                        + "@AcLookUpTypesId=?, @Activity=?",
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                1, "ReadAll");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", str(col(r, "Id")));
            m.put("name", str(col(r, "AcLookUpsDescription")));
            out.add(m);
        }
        return out;
    }

    /** CustomerGroupFill, :522-554 - Sp_CustomerGroup_GetAllMethod @Activity='ReadAll'. */
    private List<Map<String, Object>> customerGroups() {
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC Sp_CustomerGroup_GetAllMethod @Activity=?, @OrganizationId=?, @CompanyId=?",
                "ReadAll",
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId());
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", str(col(r, "Id")));
            m.put("name", str(col(r, "Description")));
            out.add(m);
        }
        return out;
    }

    /** BranchesFill, :556-586 - USP_GetBranchesFromVouchersByAccountId. */
    private List<Map<String, Object>> branches() {
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC USP_GetBranchesFromVouchersByAccountId @OrganizationId=?, @CompanyId=?",
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId());
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", str(col(r, "Id")));
            m.put("name", str(col(r, "BranchName")));
            out.add(m);
        }
        return out;
    }

    // ----------------------------------------------------------------- report

    public static class Filters {
        public String fromDate, toDate, dueFrom, dueUpto, purchaseFrom, purchaseTo;
        public boolean chkFrom, chkTo, chkDueFrom, chkDueUpto, chkPurchaseFrom, chkPurchaseTo;
        public int parentCategoryId, customGroupId, customerGroupId, agingDays;
        public String controlAccountIds = "", branchIds = "";
    }

    /** gridHistory(), :199-355. */
    public Map<String, Object> report(Filters f) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();

        names.add("@OrganizationId");   args.add(currentUserContext.currentOrganizationId());
        names.add("@CompanyId");        args.add(currentUserContext.currentCompanyId());
        names.add("@UserId");           args.add(currentUserContext.currentUserId());
        names.add("@ParentCategoryId"); args.add(f.parentCategoryId);

        if (f.chkFrom)    addDate(names, args, "@FromDate", f.fromDate);
        if (f.chkTo)      addDate(names, args, "@ToDate",   f.toDate);
        if (f.chkDueFrom) addDate(names, args, "@DueDateFrom", f.dueFrom);
        /* :222-225 - value from datDueUpto, tick box tested is datDateTo. Desktop defect, kept. */
        if (f.chkTo)      addDate(names, args, "@DueDateTo", f.dueUpto);
        if (f.chkPurchaseFrom) addDate(names, args, "@PurchaseFromDate", f.purchaseFrom);
        if (f.chkPurchaseTo)   addDate(names, args, "@PurchaseToDate",   f.purchaseTo);

        if (f.customGroupId != 0)   { names.add("@CustomGroupId"); args.add(f.customGroupId); }
        if (f.customerGroupId != 0) { names.add("@PartyGroupId");  args.add(f.customerGroupId); }
        if (notBlank(f.controlAccountIds)) { names.add("@ControlAccountIds"); args.add(f.controlAccountIds.trim()); }
        /* :256 - AgingDays is read straight off txtAgingDays, and the BLL drops it when zero. */
        if (f.agingDays != 0) { names.add("@AgingDays"); args.add(f.agingDays); }
        if (notBlank(f.branchIds)) { names.add("@BranchesIds"); args.add(f.branchIds.trim()); }

        List<List<Map<String, Object>>> sets;
        try {
            sets = callMultiSet(exec("usp_PayablesAndPaymentSchedule", names), args);
        } catch (Exception e) {
            LOG.error("usp_PayablesAndPaymentSchedule failed", e);
            out.put("error", e.getMessage());
            out.put("rows", new ArrayList<>());
            out.put("summary", new ArrayList<>());
            return out;
        }

        List<Map<String, Object>> detail = sets.size() > 0 ? sets.get(0) : new ArrayList<>();
        List<Map<String, Object>> summaryRaw = sets.size() > 1 ? sets.get(1) : new ArrayList<>();

        /* :307-311 - the desktop rebuilds the detail table, renaming ClassName and deriving
           Increase/Decrease. Every other column keeps the procedure's own name. */
        List<Map<String, Object>> rows = new ArrayList<>(detail.size());
        for (Map<String, Object> r : detail) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("AccountId",         col(r, "AccountId"));
            m.put("AccountTypeId",     col(r, "AccountTypeId"));
            m.put("BranchesId",        col(r, "BranchesId"));
            m.put("BranchName",        col(r, "BranchName"));
            m.put("AccountClass",      col(r, "ClassName"));      // ClassName -> AccountClass
            m.put("ParentAccount",     col(r, "ParentAccount"));
            m.put("AccountCode",       col(r, "AccountCode"));
            m.put("AccountTitle",      col(r, "AccountTitle"));
            m.put("AccountType",       col(r, "AccountType"));
            m.put("Opening",           col(r, "Opening"));
            m.put("CurrDebit",         col(r, "CurrDebit"));
            m.put("CurrCredit",        col(r, "CurrCredit"));
            m.put("Closing",           col(r, "Closing"));
            m.put("DueBalance",        col(r, "DueBalance"));
            m.put("NotYetDue",         col(r, "NotYetDue"));
            m.put("PayToday",          col(r, "PayToday"));

            Object shortExcess = col(r, "Short/Excess");
            m.put("Short/Excess", shortExcess);
            /* :309 - no "Nill" here, and the sign is the opposite of the receivables form. */
            m.put("Increase/Decrease", dbl(shortExcess) < 0.0 ? "Increase" : "Decrease");

            m.put("LastBillDate",      col(r, "LastBillDate"));
            m.put("LastBillsAmount",   col(r, "LastBillsAmount"));
            m.put("BillDays",          col(r, "BillDays"));
            m.put("LastPaymentDate",   col(r, "LastPaymentDate"));
            m.put("LastPaymentAmount", col(r, "LastPaymentAmount"));
            m.put("PaymentDays",       col(r, "PaymentDays"));
            m.put("PurchaseQty",       col(r, "PurchaseQty"));
            m.put("PurchaseWeight",    col(r, "PurchaseWeight"));
            m.put("PurchaseAmount",    col(r, "PurchaseAmount"));
            m.put("OrderQty",          col(r, "OrderQty"));
            m.put("ReceivedQty",       col(r, "ReceivedQty"));
            m.put("BalQty",            col(r, "BalQty"));
            m.put("OrderWeight",       col(r, "OrderWeight"));
            m.put("ReceivedWeight",    col(r, "ReceivedWeight"));
            m.put("BalWeight",         col(r, "BalWeight"));
            m.put("FromDate",          col(r, "FromDate"));
            m.put("ToDate",            col(r, "ToDate"));
            m.put("DueDateFrom",       col(r, "DueDateFrom"));
            m.put("DueDateTo",         col(r, "DueDateTo"));
            m.put("PurchaseFromDate",  col(r, "PurchaseFromDate"));
            m.put("PurchaseToDate",    col(r, "PurchaseToDate"));
            rows.add(m);
        }
        out.put("rows", rows);

        /* :317-345 - the four bucket headings come out of the FIRST summary row, and every row
           then supplies Value_1..Value_4 under them. The source's own spellings are used. */
        List<Map<String, Object>> summary = new ArrayList<>();
        List<String> buckets = new ArrayList<>();
        if (!summaryRaw.isEmpty()) {
            Map<String, Object> first = summaryRaw.get(0);
            buckets.add(str(col(first, "IstIntervale")));
            buckets.add(str(col(first, "ScnInterval")));
            buckets.add(str(col(first, "TrdIntarval")));
            buckets.add(str(col(first, "Above")));
            for (Map<String, Object> r : summaryRaw) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("description", str(col(r, "Description")));
                m.put("amount",  col(r, "Amount"));
                m.put("value1",  col(r, "Value_1"));
                m.put("value2",  col(r, "Value_2"));
                m.put("value3",  col(r, "Value_3"));
                m.put("value4",  col(r, "Value_4"));
                summary.add(m);
            }
        }
        out.put("summaryBuckets", buckets);
        out.put("summary", summary);
        return out;
    }

    // ---------------------------------------------------------------- helpers

    private static void addDate(List<String> names, List<Object> args, String name, String value) {
        Object v = date(value);
        if (v != null) { names.add(name); args.add(v); }
    }

    /**
     * Reads every result set the procedure returns. queryForList stops after the first, which
     * would silently drop the aging summary.
     */
    private List<List<Map<String, Object>>> callMultiSet(final String sql, final List<Object> args) {
        return jdbcTemplate.execute((ConnectionCallback<List<List<Map<String, Object>>>>) (Connection con) -> {
            List<List<Map<String, Object>>> sets = new ArrayList<>();
            /* Plain PreparedStatement on the same "EXEC proc @Name=?" text the rest of this
               class uses - the {call ...} escape does not accept @Name=? bindings. */
            try (PreparedStatement ps = con.prepareStatement(sql)) {
                for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
                boolean hasResults = ps.execute();
                while (true) {
                    if (hasResults) {
                        try (ResultSet rs = ps.getResultSet()) {
                            sets.add(readRows(rs));
                        }
                    } else if (ps.getUpdateCount() == -1) {
                        break;
                    }
                    hasResults = ps.getMoreResults();
                    if (!hasResults && ps.getUpdateCount() == -1) break;
                }
            }
            return sets;
        });
    }

    private static List<Map<String, Object>> readRows(ResultSet rs) throws java.sql.SQLException {
        List<Map<String, Object>> rows = new ArrayList<>();
        ResultSetMetaData md = rs.getMetaData();
        int n = md.getColumnCount();
        while (rs.next()) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (int i = 1; i <= n; i++) {
                String label = md.getColumnLabel(i);
                if (label == null || label.isEmpty()) label = md.getColumnName(i);
                m.put(label, rs.getObject(i));
            }
            rows.add(m);
        }
        return rows;
    }

    /** "Default NoofDecimal Points For Amount" -> Sp_ConfigrationsAllocation_GetAllMethod. */
    private int decimalPointsForAmount() {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
                            + "@ConfigDescription=?, @Activity=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    "Default NoofDecimal Points For Amount",
                    "GetConfigurationByOrgCompandConfigDescription");
            if (!rows.isEmpty()) {
                int n = asInt(col(rows.get(0), "ConfigKey"));
                if (n >= 1 && n <= 4) return n;
            }
        } catch (Exception e) {
            LOG.error("Decimal-points configuration read failed", e);
        }
        return 2;
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

    private static String exec(String proc, List<String> names) {
        StringBuilder b = new StringBuilder("EXEC ").append(proc).append(' ');
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) b.append(", ");
            b.append(names.get(i)).append("=?");
        }
        return b.toString();
    }

    private static Object col(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    private static boolean notBlank(String s) { return s != null && !s.trim().isEmpty(); }

    private static int asInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    private static double dbl(Object o) {
        if (o == null) return 0.0;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0.0; }
    }

    /** Plain yyyy-MM-dd only - nothing shifts a day at UTC+5. */
    private static Object date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, 10))); }
        catch (Exception e) { return null; }
    }
}
