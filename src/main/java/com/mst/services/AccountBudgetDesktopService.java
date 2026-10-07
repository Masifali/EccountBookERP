package com.mst.services;

import java.math.BigDecimal;
import java.math.MathContext;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.YearMonth;
import java.time.format.DateTimeFormatter;
import java.time.format.DateTimeFormatterBuilder;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 3 "Account Budget" - desktop form Architecture.WinApp.Account_Definition.AccountBudget
 * (BLL Accounts.AccountsBudgetHeader, DAL Accounts.AccountsBudgetHeader.SetData).
 *
 * <ul>
 * <li>ProjectFill: CommonServices.ProjectServiceBind -> Projects.GetAlldt -> Sp_Projects_GetAllMethod
 *     @OrganizationId, @CompanyId, @MethodType='GetAll'; BindDDLNew(Id, ProjectName).</li>
 * <li>COABind: CommonServices.CoaAllocationGetAllServiceBind -> Sp_COAAllocation_GetAllMethod
 *     @OrganizationId, @CompanyId, @UserId, @Activity='COAAllocationSearch'; only AccountTypeId = 11 rows
 *     (Id, AccountTitle) go to cmbExpenseAcc / CmbExpenseAccountHistory (BindDDL ZeroIndex = true).</li>
 * <li>MonthFill: ActiveYr.Start_Period .. End_Period stepped with AddMonths(1), "MMM yyyy".</li>
 * <li>GenerateCode: Sp_AccountBudget_GetAllMethod @OrganizationId, @CompanyId, @FinancialYearId, @Activity='GenerateCode'.</li>
 * <li>History: AccountsBudgetHeader.GetHisoty -> Sp_AccountBudget_GetAllMethod @Activity='GetAll' (zero / empty filters are not sent).</li>
 * <li>ReadById / history detail: GetByID -> @Activity='GetById' + 'GetDetailbyHeaderId'.</li>
 * <li>Save: BLL Save (Id 0 -> Sp_AccountsBudgetHeader_Insert else _Update) then Sp_AccountsBudgetDetail_Insert per row,
 *     attachments after them, all in one transaction (DAL SetData). Print: 11-AccountsBudgetSlip.rpt through the
 *     existing /reports/print/11-accounts-budget-slip contract (USP_AccountsBudgetSlipAndRegister).</li>
 * </ul>
 * The form has no delete.
 */
@Service
public class AccountBudgetDesktopService {

    public static final String SCREEN_NAME = "AccountBudget";
    private static final int DOC_TYPE = 11;
    private static final String BAD_DATE = "String was not recognized as a valid DateTime.";
    private static final String OUT_OF_RANGE = "Index was out of range. Must be non-negative and less than the size of the collection.";
    private static final DateTimeFormatter MMM_YYYY = new DateTimeFormatterBuilder().parseCaseInsensitive()
            .appendPattern("MMM yyyy").toFormatter(Locale.US);
    private static final DateTimeFormatter MMM_DASH_YYYY = new DateTimeFormatterBuilder().parseCaseInsensitive()
            .appendPattern("MMM-yyyy").toFormatter(Locale.US);
    private static final DateTimeFormatter DD_MMM_YYYY = DateTimeFormatter.ofPattern("dd-MMM-yyyy", Locale.US);
    private static final DateTimeFormatter SHORT_DATE = DateTimeFormatter.ofPattern("dd/MM/yyyy", Locale.US);

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;
    private final AccountBudgetAttachmentService attachments;

    public AccountBudgetDesktopService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights,
                                       AccountBudgetAttachmentService attachments) {
        this.jdbc = jdbc;
        this.ctx = ctx;
        this.rights = rights;
        this.attachments = attachments;
    }

    public Map<String, Boolean> rights() { return rights.of(SCREEN_NAME); }

    // ------------------------------------------------------------------------------------ Load / Refresh

    /** AccountBudget_Load: rights, ProjectFill, GenerateCode, COABind, MonthFill and the amount format. */
    public Map<String, Object> init() {
        Map<String, Object> m = lookups();
        m.put("rights", rights());
        m.put("docNo", generateCode());
        m.put("amountDecimals", amountDecimals());
        return m;
    }

    /** btnRefresh_Click: ProjectFill(); COABind(); MonthFill(). */
    public Map<String, Object> lookups() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("projects", projects());
        m.put("accounts", accounts());
        m.put("months", months());
        return m;
    }

    public List<Map<String, Object>> projects() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_Projects_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "MethodType", "GetAll"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id")));
            m.put("ProjectName", str(r.get("ProjectName")));
            out.add(m);
        }
        return out;
    }

    /** CoaAllocationGetAllServiceBind filtered to AccountTypeId == 11 (COABind). */
    public List<Map<String, Object>> accounts() {
        int user = ctx.currentUserId();
        List<Map<String, Object>> src = DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "UserId", user != 0 ? user : null,
                "Activity", "COAAllocationSearch"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            if (toInt(r.get("AccountTypeId")) != 11) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id")));
            m.put("AccountTitle", str(r.get("AccountTitle")));
            out.add(m);
        }
        return out;
    }

    /** MonthFill: every month from ActiveYr.Start_Period to End_Period (date.AddMonths(1)), "MMM yyyy". */
    public List<String> months() {
        Map<String, Object> year = activeYear();
        LocalDateTime start = toDateTime(year.get("Start_Period"));
        LocalDateTime end = toDateTime(year.get("End_Period"));
        List<String> out = new ArrayList<>();
        if (start == null || end == null) return out;
        LocalDateTime date = start;
        int guard = 0;
        while (!date.isAfter(end) && guard++ < 1200) {
            out.add(date.format(MMM_YYYY));
            date = date.plusMonths(1);
        }
        return out;
    }

    private Map<String, Object> activeYear() {
        int yearId = ctx.currentFinancialYearId();
        List<Map<String, Object>> years = DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId()));
        for (Map<String, Object> y : years) if (toInt(y.get("Id")) == yearId) return y;
        if (!years.isEmpty()) return years.get(0);
        throw new IllegalStateException("No active financial year");
    }

    /** CommonServices.GetDecimalConfiguration: "Default NoofDecimal Points For Amount" 1-4 places, else none. */
    public int amountDecimals() {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "ConfigDescription", "Default NoofDecimal Points For Amount",
                "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        int n = r.isEmpty() ? 0 : toInt(str(r.get(0).get("ConfigKey")).trim());
        return (n >= 1 && n <= 4) ? n : 0;
    }

    /** GenerateCode(): AccountsBudgetHeader.GenerateCode. */
    public int generateCode() {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_AccountBudget_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(), "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    // ------------------------------------------------------------------------------------ ReadById

    /** AccountsBudgetHeader.GetByID (header + GetDetailbyHeaderId). The procs have no tenancy filter, so it is checked here. */
    private Map<String, Object> header(int id) {
        List<Map<String, Object>> h = DesktopProc.rows(jdbc, "Sp_AccountBudget_GetAllMethod", DesktopProc.params(
                "Id", id, "Activity", "GetById"));
        if (h.isEmpty()) throw new IllegalArgumentException(OUT_OF_RANGE);
        Map<String, Object> row = h.get(0);
        if (toInt(row.get("OrganizationId")) != ctx.currentOrganizationId() || toInt(row.get("CompanyId")) != ctx.currentCompanyId())
            throw new IllegalArgumentException(OUT_OF_RANGE);
        return row;
    }

    private List<Map<String, Object>> detailRows(int id) {
        List<Map<String, Object>> d = DesktopProc.rows(jdbc, "Sp_AccountBudget_GetAllMethod", DesktopProc.params(
                "Id", id, "Activity", "GetDetailbyHeaderId"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : d) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id")));
            LocalDateTime bm = toDateTime(r.get("BudgetMonth"));
            m.put("Month", (bm == null ? LocalDate.of(1900, 1, 1).atStartOfDay() : bm).format(MMM_DASH_YYYY));
            m.put("Percent", dbl(r.get("BudgetPrcnt")));
            m.put("Amount", dbl(r.get("BudgetAmount")));
            out.add(m);
        }
        return out;
    }

    /** ReadById(Id): only loads when the header exists and has detail rows. */
    public Map<String, Object> record(int id) {
        Map<String, Object> hd = header(id);
        List<Map<String, Object>> rows = detailRows(id);
        Map<String, Object> m = new LinkedHashMap<>();
        if (rows.isEmpty()) { m.put("loaded", false); return m; }
        m.put("loaded", true);
        m.put("id", id);
        m.put("projectId", toInt(hd.get("ProjectId")));
        m.put("docNo", str(hd.get("DocNo")));
        LocalDateTime dd = toDateTime(hd.get("DocDate"));
        m.put("docDate", dd == null ? "" : dd.toLocalDate().toString());
        LocalDateTime f = toDateTime(hd.get("FnFromMonth")), t = toDateTime(hd.get("FnToMonth"));
        m.put("fromMonth", (f == null ? LocalDate.of(1900, 1, 1).atStartOfDay() : f).format(MMM_YYYY));
        m.put("toMonth", (t == null ? LocalDate.of(1900, 1, 1).atStartOfDay() : t).format(MMM_YYYY));
        Object amt = hd.get("BudgetTotalAmount");
        m.put("amount", amt instanceof BigDecimal ? ((BigDecimal) amt).toPlainString() : str(amt));   /* BudgetTotalAmount.ToString() */
        m.put("expenseAccountId", toInt(hd.get("ExpenseGLAcId")));
        m.put("rows", rows);
        m.put("attachments", attachments.list(id));
        return m;
    }

    /** grdHistoryDetailbind(Id). */
    public List<Map<String, Object>> historyDetail(int id) {
        header(id);
        return detailRows(id);
    }

    // ------------------------------------------------------------------------------------ History

    /** HistoryGridBind: ReportsParameters -> AccountsBudgetHeader.GetHisoty, then the desktop's dthistory table. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        Map<String, Boolean> rt = rights();
        boolean viewAll = Boolean.TRUE.equals(rt.get("viewAll"));
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", ctx.currentOrganizationId());
        p.put("CompanyId", ctx.currentCompanyId());
        p.put("FinancialYearId", ctx.currentFinancialYearId());
        p.put("DocumentTypeId", DOC_TYPE);
        java.sql.Date from = sqlDate(b.get("fromDate")), to = sqlDate(b.get("toDate"));
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        int fromNo = toInt(str(b.get("fromDocNo")).trim()), toNo = toInt(str(b.get("toDocNo")).trim());
        if (fromNo != 0) p.put("FromDocNo", fromNo);
        if (toNo != 0) p.put("ToDocNo", toNo);
        /* header.AccountId = Conversion.ToInt(CmbExpenseAccountHistory.Text): the desktop converts the combo TEXT. */
        int acc = toInt(str(b.get("accountText")).trim());
        if (acc != 0) p.put("AccountId", acc);
        p.put("CanViewAllRecord", viewAll);
        if (!viewAll) p.put("EntryUser", ctx.currentUserId());
        p.put("Activity", "GetAll");
        List<Map<String, Object>> src = DesktopProc.rows(jdbc, "Sp_AccountBudget_GetAllMethod", p);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id")));
            m.put("DocumentTypeId", toInt(r.get("DocumentTypeId")));
            m.put("DocNo", toInt(r.get("DocNo")));
            m.put("DocDate", dateOr1900(r.get("DocDate")).format(DD_MMM_YYYY));
            m.put("FromMonth", dateOr1900(r.get("FnFromMonth")).format(MMM_DASH_YYYY));
            m.put("ToMonth", dateOr1900(r.get("FnToMonth")).format(MMM_DASH_YYYY));
            m.put("ExpenseAccount", str(r.get("AccountTitle")));
            m.put("Amount", dbl(r.get("BudgetTotalAmount")));
            m.put("EntryUserName", str(r.get("EntryUserName")));
            m.put("EntryDate", dateOr1900(r.get("EntryDate")).format(SHORT_DATE));
            m.put("ModifyUserName", str(r.get("ModifyUserName")));
            m.put("ModifyDate", dateOr1900(r.get("ModifyDate")).format(SHORT_DATE));
            m.put("IsApproved", toBool(r.get("IsApproved")));
            m.put("ApprovedUserName", str(r.get("ApprovedUserName")));
            m.put("ApprovedDate", dateOr1900(r.get("ApprovedDate")).format(SHORT_DATE));
            m.put("NoOfAttachments", toInt(r.get("NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    // ------------------------------------------------------------------------------------ Save / Update

    /**
     * Insert(): the checks in the desktop's order (the Yes/No prompt is answered on the page first), then
     * AccountsBudgetHeader.Save -> DAL SetData in one transaction.
     */
    @Transactional(rollbackFor = Exception.class)
    public Map<String, Object> save(Map<String, Object> b) {
        int recId = toInt(b.get("id"));
        Map<String, Boolean> rt = rights();
        if (recId > 0) {
            if (!Boolean.TRUE.equals(rt.get("update"))) throw new IllegalArgumentException("You do not have the Update right for this screen.");
        } else if (!Boolean.TRUE.equals(rt.get("save"))) {
            throw new IllegalArgumentException("You do not have the Save right for this screen.");
        }
        List<Map<String, Object>> rows = rowList(b.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");

        String fromText = str(b.get("fromMonth")), toText = str(b.get("toMonth"));
        LocalDate fromMonth = parseMonth(fromText), toMonth = parseMonth(toText);
        long days = ChronoUnit.DAYS.between(fromMonth, toMonth);
        int expected = (int) Math.rint(days / 30.4);                 /* Conversion.ToInt(double) = Convert.ToInt32 (banker's rounding) */
        if (Math.abs(expected - rows.size()) > 1 || Math.abs(expected - rows.size()) < 1)
            throw new IllegalArgumentException("Month count does not match with Detail Record");

        int projectId = toInt(b.get("projectId"));
        if (projectId == 0) throw new IllegalArgumentException("Project  Field Required");
        String docNoText = str(b.get("docNo")).trim();
        if (docNoText.isEmpty() || docNoText.equals("0")) throw new IllegalArgumentException("Doc No   Field Required");
        int expenseId = toInt(b.get("expenseAccountId"));
        if (expenseId == 0) throw new IllegalArgumentException("Exp Account Field Required");
        String amountText = str(b.get("amount"));
        if (toDouble(amountText.trim()) == 0.0 || toDouble(amountText) == 0.0) throw new IllegalArgumentException("Amount  Field Required");

        double sumPct = 0, sumAmt = 0;
        for (Map<String, Object> r : rows) { sumPct += toDouble(r.get("percent")); sumAmt += toDouble(r.get("amount")); }
        if (Math.rint(sumPct) != 100.0) throw new IllegalArgumentException("Grid Total Percent Not Equal To 100");
        if (Math.rint(sumAmt) != toDouble(amountText.trim())) throw new IllegalArgumentException("Grid Total Amount and Header Amount Not Match ");

        /* The ids must belong to the signed-in company's own combos. */
        if (projects().stream().noneMatch(p -> toInt(p.get("Id")) == projectId)) throw new IllegalArgumentException("Project  Field Required");
        if (accounts().stream().noneMatch(a -> toInt(a.get("Id")) == expenseId)) throw new IllegalArgumentException("Exp Account Field Required");
        if (recId > 0) header(recId);

        LocalDate docDate = parseIso(str(b.get("docDate")));
        Timestamp now = new Timestamp(System.currentTimeMillis());
        int user = ctx.currentUserId();
        long docNo = toInt(docNoText);
        BigDecimal total = toDecimal(amountText);

        /* AccountsBudgetHeader model: every non-virtual property is a parameter (GenericProvider.SetProc); null = not sent. */
        Map<String, Object> hp = new LinkedHashMap<>();
        hp.put("IsApproved", false);
        hp.put("DocDate", java.sql.Date.valueOf(docDate));
        hp.put("EntryDate", now);
        hp.put("ModifyDate", now);
        hp.put("BudgetTotalAmount", total);
        hp.put("ApprovedUserId", 0);
        hp.put("BranchId", ctx.currentBranchId());
        hp.put("CompanyId", ctx.currentCompanyId());
        hp.put("EntryUserId", user);
        hp.put("ExpenseGLAcId", expenseId);
        hp.put("FnFromMonth", java.sql.Date.valueOf(fromMonth));
        hp.put("FnToMonth", java.sql.Date.valueOf(toMonth));
        hp.put("Id", recId);
        hp.put("ModifyUserId", user);
        hp.put("DocumentTypeId", DOC_TYPE);
        hp.put("FinancialYearId", ctx.currentFinancialYearId());
        hp.put("OrganizationId", ctx.currentOrganizationId());
        hp.put("ProjectId", projectId);
        hp.put("DocNo", docNo);

        Object atts = b.get("attachments");
        Object files = null, removeIds = null;
        if (atts instanceof Map) { files = ((Map<?, ?>) atts).get("files"); removeIds = ((Map<?, ?>) atts).get("removeIds"); }
        AccountBudgetAttachmentService.Prepared prepared = attachments.prepare(recId, files, removeIds);

        int num = DesktopProc.setProc(jdbc, recId == 0 ? "[dbo].[Sp_AccountsBudgetHeader_Insert]" : "[dbo].[Sp_AccountsBudgetHeader_Update]", hp);
        int headerId = recId;
        if (num > 0) headerId = num; else num = recId;
        for (Map<String, Object> r : rows) {
            Map<String, Object> dp = new LinkedHashMap<>();
            dp.put("BudgetAmount", toDecimal15(r.get("amount")));
            dp.put("BudgetPrcnt", toDecimal15(r.get("percent")));
            dp.put("AccountsBudgetHeaderId", headerId);
            dp.put("BudgetMonth", java.sql.Date.valueOf(parseDashMonth(str(r.get("month")))));
            dp.put("Id", toInt(r.get("id")));
            DesktopProc.setProc(jdbc, "Sp_AccountsBudgetDetail_Insert", dp);
        }
        attachments.persist(num, recId > 0, prepared);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", num);
        out.put("docNo", docNo);
        out.put("message", (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + docNo);
        return out;
    }

    // ------------------------------------------------------------------------------------ helpers

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> rowList(Object o) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (o instanceof List) for (Object x : (List<?>) o) if (x instanceof Map) out.add((Map<String, Object>) x);
        return out;
    }

    /** DateTime.ParseExact(text, "MMM yyyy", InvariantCulture). */
    private static LocalDate parseMonth(String text) {
        try { return YearMonth.parse(text.trim(), MMM_YYYY).atDay(1); }
        catch (Exception e) { throw new IllegalArgumentException(BAD_DATE); }
    }

    /** Conversion.ToDateTime("MMM-yyyy"): first of the month; unreadable text is 1900-01-01. */
    private static LocalDate parseDashMonth(String text) {
        try { return YearMonth.parse(text.trim(), MMM_DASH_YYYY).atDay(1); }
        catch (Exception e) { return LocalDate.of(1900, 1, 1); }
    }

    private static LocalDate parseIso(String s) {
        try { return LocalDate.parse(s.trim().substring(0, 10)); }
        catch (Exception e) { return LocalDate.now(); }
    }

    private static java.sql.Date sqlDate(Object o) {
        if (o == null) return null;
        try {
            LocalDate d = LocalDate.parse(String.valueOf(o).trim().substring(0, 10));
            if (d.getYear() <= 1900) return null;                    /* Conversion.CheckDateTimeNull */
            return java.sql.Date.valueOf(d);
        } catch (Exception e) { return null; }
    }

    private static LocalDateTime dateOr1900(Object o) {
        LocalDateTime d = toDateTime(o);
        return d == null ? LocalDate.of(1900, 1, 1).atStartOfDay() : d;
    }

    private static LocalDateTime toDateTime(Object o) {
        if (o == null) return null;
        if (o instanceof Timestamp) return ((Timestamp) o).toLocalDateTime();
        if (o instanceof java.sql.Date) return ((java.sql.Date) o).toLocalDate().atStartOfDay();
        if (o instanceof java.util.Date) return new Timestamp(((java.util.Date) o).getTime()).toLocalDateTime();
        if (o instanceof LocalDateTime) return (LocalDateTime) o;
        if (o instanceof LocalDate) return ((LocalDate) o).atStartOfDay();
        try { return LocalDate.parse(String.valueOf(o).substring(0, 10)).atStartOfDay(); } catch (Exception e) { return null; }
    }

    /** Convert.ToDecimal(double): 15 significant digits. */
    private static BigDecimal toDecimal15(Object o) {
        double d = toDouble(o);
        if (Double.isNaN(d) || Double.isInfinite(d)) return BigDecimal.ZERO;
        return new BigDecimal(d).round(new MathContext(15));
    }

    /** Conversion.ToDecimal(string). */
    private static BigDecimal toDecimal(String s) {
        try { return new BigDecimal(s.trim().replace(",", "")); } catch (Exception e) { return BigDecimal.ZERO; }
    }

    /** Conversion.ToDouble. */
    private static double toDouble(Object o) {
        if (o instanceof Number) { double d = ((Number) o).doubleValue(); return Double.isInfinite(d) || Double.isNaN(d) ? 0.0 : d; }
        if (o == null) return 0.0;
        try {
            double d = Double.parseDouble(String.valueOf(o).trim().replace(",", ""));
            return Double.isInfinite(d) || Double.isNaN(d) ? 0.0 : d;
        } catch (NumberFormatException e) { return 0.0; }
    }

    private static double dbl(Object o) { return o instanceof Number ? ((Number) o).doubleValue() : 0.0; }

    /** Conversion.ToString */
    private static String str(Object o) {
        if (o == null) return "";
        if (o instanceof Boolean) return ((Boolean) o) ? "True" : "False";
        return String.valueOf(o);
    }

    /** Conversion.ToInt */
    private static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return Integer.parseInt(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static boolean toBool(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        return o != null && Boolean.parseBoolean(String.valueOf(o).trim());
    }
}
