package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Receivables And Receipt Schedule.
 *
 * Ported from Architecture.WinApp.Account_Reports\ReceivablesAndReceiptSchedule.cs (1,812 lines).
 * The file's own methods are still named PayablesAndPaymentSchedule_* - it was copied from that
 * form and never renamed. The captions and the procedure are the receivables ones.
 *
 * Report contract, read from BLL.Reports.Accounts.VoucherReports.ReceivablesAndReceiptsSchedule
 * (0141_Architecture.BLL.Reports.Accounts.VoucherReports.cs:1707-1782):
 *
 *   usp_ReceivablesAndReceiptsSchedule
 *       @OrganizationId, @CompanyId, @UserId, @ParentCategoryId     ALWAYS sent, even as 0
 *       @FromDate        only when datDateFrom is ticked
 *       @ToDate          only when datDateTo   is ticked
 *       @DueDateFrom     only when datDueFrom  is ticked
 *       @DueDateTo       only when datDateTo   is ticked   <-- see the note below
 *       @SaleFromDate    only when datSaleFrom is ticked
 *       @SaleToDate      only when datSaleTo   is ticked
 *       @CustomGroupId   only when non-zero
 *       @PartyGroupId    only when non-zero   (from CmbCustomerGroup)
 *       @ControlAccountIds only when non-empty
 *       @BranchesIds     only when non-empty
 *
 *   @BalanceFrom, @BalanceTo, @ActionId and @TradeTypeId exist in the BLL but this form never
 *   sets them, so they are omitted. Omitting is not the same as sending NULL, so they are not
 *   sent at all.
 *
 * SOURCE QUIRK, reproduced deliberately (:203-206): the Due Up To value comes from datDueUpto,
 * but the tick box the form tests is datDateTo, not datDueUpto. So Due Up To is sent whenever
 * To Date is ticked and is suppressed whenever To Date is unticked, whatever the Due Up To tick
 * box says. This is a defect in the desktop form; it is kept because both apps must return the
 * same rows for the same input.
 *
 * SECOND QUIRK: cmbAccount is bound with AccountCode in the "Id" column (:505-510), so
 * @ControlAccountIds carries a comma list of ACCOUNT CODES, not account ids.
 *
 * Read-only screen: no save, update or delete, and no History button on the desktop form.
 */
@Service
public class ReceivablesScheduleService {

    private static final Logger LOG = LoggerFactory.getLogger(ReceivablesScheduleService.class);

    /** CommonServices.GetERPFeatureById(17) / (18) - branches, and consolidated branches (:139-140). */
    private static final int FEATURE_BRANCH = 17;
    private static final int FEATURE_BRANCH_CONSOLIDATED = 18;

    /** txtIntervalDays designer default (:1430). */
    private static final int DEFAULT_INTERVAL_DAYS = 15;

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    // ------------------------------------------------------------------ setup

    /** PayablesAndPaymentSchedule_Load, :132-163. */
    public Map<String, Object> setup() {
        Map<String, Object> out = new LinkedHashMap<>();

        boolean branchFeature = featureOn(FEATURE_BRANCH);
        boolean branchConsolidated = featureOn(FEATURE_BRANCH_CONSOLIDATED);
        out.put("branchFeature", branchFeature);
        out.put("branchConsolidated", branchConsolidated);

        /* :150-152 - Date From is thirty days back, Sale From is txtIntervalDays back.
           Date To, Due From, Due Up To and Sale To keep the designer's "today". */
        LocalDate today = LocalDate.now();
        out.put("fromDate", today.minusDays(30).toString());
        out.put("toDate", today.toString());
        out.put("dueFrom", today.toString());
        out.put("dueUpto", today.toString());
        out.put("saleFrom", today.minusDays(DEFAULT_INTERVAL_DAYS).toString());
        out.put("saleTo", today.toString());
        out.put("intervalDays", DEFAULT_INTERVAL_DAYS);

        /* Designer tick states: only datDueFrom starts unticked (:1518); a DateTimePicker with
           ShowCheckBox defaults to Checked = true and nothing else sets it. */
        out.put("chkFrom", true);
        out.put("chkTo", true);
        out.put("chkDueFrom", false);
        out.put("chkDueUpto", true);
        out.put("chkSaleFrom", true);
        out.put("chkSaleTo", true);

        out.put("decimalsAmount", decimalPointsForAmount());

        String myBranch = "";
        try { myBranch = String.valueOf(currentUserContext.currentBranchId()); } catch (Exception ignored) { }
        out.put("currentBranchId", myBranch);

        out.put("parentCategories", readOrError(out, "parentCategoryError", this::parentCategories));
        out.put("controlAccounts",  readOrError(out, "controlAccountError", this::controlAccounts));
        out.put("customGroups",     readOrError(out, "customGroupError",    this::customGroups));
        out.put("customerGroups",   readOrError(out, "customerGroupError",  this::customerGroups));
        /* :141-148 - the branch picker is filled only when feature 17 is on; otherwise the
           desktop hides the label and the combo outright. */
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

    /**
     * ParentCategoryFill, :594-628.
     * InvPurchaseInvoice.GetDataForPurchaseInvoiceRegisterDropdownBind ->
     *   [Sp_InvPurchaseInvoice_GetAllMethod] @OrganizationId, @CompanyId,
     *                                        @Activity='GetDataForPurchaseInvoiceRegisterDropdownBind'
     * One result set carrying every dropdown for that register; the form keeps only the rows
     * whose ActivityType is "ItemParentCategory" (:617).
     */
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
     * ControlAccountFill3rdLevel, :478-527.
     * ChartofAccount.ReadAllAccountgroup ->
     *   Sp_ChartofAccount_GetAllMethodFromCOA
     *       @OrganizationId, @CompanyId, @FinancialYearId, @Account_Level=3,
     *       @AccountTypeId=3, @AccountClassId=2, @CoaType='ReadAllAccountGroup'
     * The desktop binds AccountCode into the "Id" column (:505-510), so the value posted back in
     * @ControlAccountIds is a list of account CODES.
     */
    private List<Map<String, Object>> controlAccounts() {
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId=?, @CompanyId=?, "
                        + "@FinancialYearId=?, @Account_Level=?, @AccountTypeId=?, @AccountClassId=?, @CoaType=?",
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                currentUserContext.currentFinancialYearId(),
                3, 3, 2, "ReadAllAccountGroup");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", str(col(r, "AccountCode")));
            m.put("name", str(col(r, "AccountTitle")));
            out.add(m);
        }
        return out;
    }

    /**
     * CustomGroupsFill, :444-476.
     * CommonServices.CustomeGroupsDefine(1) -> AcLookUps.GetAll ->
     *   Sp_AcLookUps_GetAllMethod @OrganizationId, @CompanyId, @AcLookUpTypesId=1, @Activity='ReadAll'
     */
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

    /**
     * CustomerGroupFill, :528-561.
     * CustomerGroup.GetAll -> Sp_CustomerGroup_GetAllMethod @Activity='ReadAll',
     *                                                       @OrganizationId, @CompanyId
     */
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

    /**
     * BranchesFill, :562-593.
     * VoucherHead.GetBranchesFromVouchersByAccountId(OrganizationId, CompanyId, "", 0) ->
     *   USP_GetBranchesFromVouchersByAccountId @OrganizationId, @CompanyId
     * @CompanyIds and @AccountId are omitted because the call passes "" and 0.
     */
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

    /** Every filter the desktop's gridHistory() reads off the form, in the same order. */
    public static class Filters {
        public String fromDate, toDate, dueFrom, dueUpto, saleFrom, saleTo;
        public boolean chkFrom, chkTo, chkDueFrom, chkDueUpto, chkSaleFrom, chkSaleTo;
        public int parentCategoryId, customGroupId, customerGroupId;
        public String controlAccountIds = "", branchIds = "";
    }

    /** gridHistory(), :183-293. */
    public Map<String, Object> report(Filters f) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();

        names.add("@OrganizationId");   args.add(currentUserContext.currentOrganizationId());
        names.add("@CompanyId");        args.add(currentUserContext.currentCompanyId());
        names.add("@UserId");           args.add(currentUserContext.currentUserId());
        /* :192 - @ParentCategoryId is added unconditionally by the BLL, so 0 is a real value here. */
        names.add("@ParentCategoryId"); args.add(f.parentCategoryId);

        if (f.chkFrom) {
            Object v = date(f.fromDate);
            if (v != null) { names.add("@FromDate"); args.add(v); }
        }
        if (f.chkTo) {
            Object v = date(f.toDate);
            if (v != null) { names.add("@ToDate"); args.add(v); }
        }
        if (f.chkDueFrom) {
            Object v = date(f.dueFrom);
            if (v != null) { names.add("@DueDateFrom"); args.add(v); }
        }
        /* :203-206 - the SOURCE QUIRK: value from datDueUpto, tick box tested is datDateTo. */
        if (f.chkTo) {
            Object v = date(f.dueUpto);
            if (v != null) { names.add("@DueDateTo"); args.add(v); }
        }
        if (f.chkSaleFrom) {
            Object v = date(f.saleFrom);
            if (v != null) { names.add("@SaleFromDate"); args.add(v); }
        }
        if (f.chkSaleTo) {
            Object v = date(f.saleTo);
            if (v != null) { names.add("@SaleToDate"); args.add(v); }
        }

        if (f.customGroupId != 0)   { names.add("@CustomGroupId"); args.add(f.customGroupId); }
        if (f.customerGroupId != 0) { names.add("@PartyGroupId");  args.add(f.customerGroupId); }
        if (notBlank(f.controlAccountIds)) { names.add("@ControlAccountIds"); args.add(f.controlAccountIds.trim()); }
        if (notBlank(f.branchIds))         { names.add("@BranchesIds");       args.add(f.branchIds.trim()); }

        List<Map<String, Object>> raw;
        try {
            raw = jdbcTemplate.queryForList(exec("usp_ReceivablesAndReceiptsSchedule", names), args.toArray());
        } catch (Exception e) {
            LOG.error("usp_ReceivablesAndReceiptsSchedule failed", e);
            out.put("error", e.getMessage());
            out.put("rows", new ArrayList<>());
            return out;
        }

        /* :211-283 - the desktop rebuilds the table, renaming seven columns and deriving one.
           The renames are the desktop's, so the grid must show these names, not the procedure's. */
        List<Map<String, Object>> rows = new ArrayList<>(raw.size());
        for (Map<String, Object> r : raw) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("AccountId",          col(r, "AccountId"));
            m.put("AccountTypeId",      col(r, "AccountTypeId"));
            m.put("BranchesId",         col(r, "BranchesId"));
            m.put("BranchName",         col(r, "BranchName"));
            m.put("AccountClass",       col(r, "ClassName"));          // ClassName   -> AccountClass
            m.put("ParentAccount",      col(r, "ParentAccount"));
            m.put("AccountCode",        col(r, "AccountCode"));
            m.put("AccountTitle",       col(r, "AccountTitle"));
            m.put("AccountType",        col(r, "AccountType"));
            m.put("Opening",            col(r, "Opening"));
            m.put("CurrDebit",          col(r, "CurrDebit"));
            m.put("CurrCredit",         col(r, "CurrCredit"));
            m.put("Closing",            col(r, "Closing"));

            Object shortExcess = col(r, "Short/Excess");
            m.put("Short/Excess", shortExcess);
            /* :264 - Nill at exactly zero, Increase above it, Decrease below it. */
            double se = dbl(shortExcess);
            m.put("Increase/Decrease", se == 0.0 ? "Nill" : (se > 0.0 ? "Increase" : "Decrease"));

            m.put("DueBalance",         col(r, "DueBalance"));
            m.put("NotYetDue",          col(r, "NotYetDue"));
            m.put("LastBillDate",       col(r, "LastBillDate"));
            m.put("LastBillsAmount",    col(r, "LastBillsAmount"));
            m.put("BillDays",           col(r, "BillDays"));
            m.put("LastReceivedDate",   col(r, "LastReceiptDate"));    // LastReceiptDate   -> LastReceivedDate
            m.put("LastReceivedAmount", col(r, "LastReceiptAmount"));  // LastReceiptAmount -> LastReceivedAmount
            m.put("ReceivedDays",       col(r, "ReceiptsDays"));       // ReceiptsDays      -> ReceivedDays
            m.put("SaleAmount",         col(r, "SaleAmount"));
            m.put("SaleQty",            col(r, "SaleQty"));
            m.put("SaleWeight",         col(r, "SaleWeight"));
            m.put("OrderQty",           col(r, "OrderQty"));
            m.put("DispatchedQty",      col(r, "DispatchQty"));        // DispatchQty     -> DispatchedQty
            m.put("BalQty",             col(r, "BalQty"));
            m.put("OrderWeight",        col(r, "OrderWeight"));
            m.put("DispatchedWeight",   col(r, "DispatchWeight"));     // DispatchWeight  -> DispatchedWeight
            m.put("BalWeight",          col(r, "BalWeight"));
            m.put("ReceiveToday",       col(r, "ReceiptsToday"));      // ReceiptsToday   -> ReceiveToday
            m.put("FromDate",           col(r, "FromDate"));
            m.put("ToDate",             col(r, "ToDate"));
            m.put("DueDateFrom",        col(r, "DueDateFrom"));
            m.put("DueDateTo",          col(r, "DueDateTo"));
            m.put("SaleFromDate",       col(r, "SaleFromDate"));
            m.put("SaleToDate",         col(r, "SaleToDate"));
            rows.add(m);
        }
        out.put("rows", rows);
        return out;
    }

    // ---------------------------------------------------------------- helpers

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
        return 2;   // clsGlobalVariables falls back to 2 when the configuration is missing
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

    /** Plain yyyy-MM-dd only - never a parsed instant, so nothing shifts a day at UTC+5. */
    private static Object date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, 10))); }
        catch (Exception e) { return null; }
    }
}
