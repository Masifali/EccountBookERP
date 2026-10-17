package com.mst.services;

import com.mst.repositories.W3CProductionFohRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.YearMonth;
import java.time.format.DateTimeFormatter;
import java.time.format.DateTimeFormatterBuilder;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

import static com.mst.services.W3CSupport.col;
import static com.mst.services.W3CSupport.plainRows;
import static com.mst.services.W3CSupport.rowList;
import static com.mst.services.W3CSupport.str;
import static com.mst.services.W3CSupport.toDouble;
import static com.mst.services.W3CSupport.toInt;

/**
 * 906 Production FOH - frmProductionFohReportCreate.cs (DocumentTypeId 922).
 *
 * Tab "FOH Allocation": BindGrid:339 (month hours grouped by job order + plant and by plant) and the 666_01 print.
 * Tab "FOH Expense": Run (BtnShowFohExpense_Click:586), InsertFohExpense:860, Delete:1011, ReadByIdFohExpense:1044,
 * History (HistoryFill:1188), Print-922. The grid arithmetic of Run runs in the browser as it does in the desktop grid;
 * every database step and every refusal InsertFohExpense makes before its write runs here again.
 *
 * Fences the desktop does not need (its form can only hold ids it read itself): the account must be one the month's
 * ledger returns (insert) with its net amount; new detail rows must be (job order, plant) groups of that month's
 * hours with the hours the procedure returns; an existing row must match the stored detail of the record being updated;
 * a record must belong to this organization / company.
 *
 * DEVIATION (flagged): the desktop turns the month combo text "MMM-yy" into a date with Convert.ToDateTime(text),
 * whose reading of "Sep-26" depends on the Windows culture (day 26 of the current year in en-US). This service reads
 * "MMM-yy" as month and year (first of the month), which is what the combo lists.
 */
@Service
public class W3CProductionFohService {

    private static final DateTimeFormatter MMM_YY = new DateTimeFormatterBuilder().parseCaseInsensitive()
            .appendPattern("MMM-yy").toFormatter(Locale.ENGLISH);

    private static final DateTimeFormatter MMM_YY_OUT = DateTimeFormatter.ofPattern("MMM-yy", Locale.ENGLISH);

    @Autowired private W3CProductionFohRepository repo;
    @Autowired private W3CSupport support;
    @Autowired private CurrentUserContext ctx;

    /* ======================================================================== InitializeComponentMethod */

    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("screenName", W3CProductionFohRepository.SCREEN);
        out.put("rights", support.rights(W3CProductionFohRepository.SCREEN));
        out.put("months", support.financialYearMonths());
        out.put("methodTypes", methodTypes());
        out.put("amountDecimals", support.amountDecimals());
        out.put("defaultHistoryDays", support.defaultHistoryDays());
        out.put("today", LocalDate.now().toString());
        return out;
    }

    /** InitializeComponentCustom:240 - dtAllocationType. */
    private static List<Map<String, Object>> methodTypes() {
        List<Map<String, Object>> l = new ArrayList<>();
        Map<String, Object> a = new LinkedHashMap<>(); a.put("id", 1); a.put("name", "MachineHours"); l.add(a);
        Map<String, Object> b = new LinkedHashMap<>(); b.put("id", 2); b.put("name", "UsageBased"); l.add(b);
        return l;
    }

    /** BtnRefreshFohExpense_Click:1099 / btnRefreshHistoryFohExpense_Click:1164 - month list and method list are rebound. */
    public Map<String, Object> refresh() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("months", support.financialYearMonths());
        out.put("methodTypes", methodTypes());
        out.put("defaultHistoryDays", support.defaultHistoryDays());
        return out;
    }

    /* ======================================================================== FOH Allocation: BindGrid:339 */

    /**
     * BindGrid: FormHistoryAndReport(OrganizationId, CompanyId, MonthDate). With rows: "Job Order And Plant Wise" (group by
     * JobOrderId + PlantId, first row's names, summed hours, % of the total hours) and "Plant Wise" (group by PlantId).
     */
    public Map<String, Object> allocation(String monthDate) {
        List<Map<String, Object>> rows = repo.monthlyHours(ctx.currentOrganizationId(), ctx.currentCompanyId(), sqlDate(monthDate));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows.size());
        List<Map<String, Object>> jobPlant = new ArrayList<>(), plant = new ArrayList<>();
        if (!rows.isEmpty()) {
            double total = 0;
            for (Map<String, Object> r : rows) total += toDouble(col(r, "ConsumedHours"));
            Map<String, Map<String, Object>> jp = new LinkedHashMap<>(), pl = new LinkedHashMap<>();
            for (Map<String, Object> r : rows) {
                double h = toDouble(col(r, "ConsumedHours"));
                String k1 = str(col(r, "JobOrderId")) + "|" + str(col(r, "PlantId"));
                Map<String, Object> g = jp.get(k1);
                if (g == null) {
                    g = new LinkedHashMap<>();
                    g.put("jobOrderId", toInt(col(r, "JobOrderId"))); g.put("jobOrderNo", str(col(r, "JobOrderNo")));
                    g.put("plantId", toInt(col(r, "PlantId"))); g.put("plantName", str(col(r, "PlantName")));
                    g.put("consumedHours", 0d);
                    jp.put(k1, g);
                }
                g.put("consumedHours", (Double) g.get("consumedHours") + h);
                String k2 = str(col(r, "PlantId"));
                Map<String, Object> p = pl.get(k2);
                if (p == null) {
                    p = new LinkedHashMap<>();
                    p.put("plantId", toInt(col(r, "PlantId"))); p.put("plantName", str(col(r, "PlantName")));
                    p.put("consumedHours", 0d);
                    pl.put(k2, p);
                }
                p.put("consumedHours", (Double) p.get("consumedHours") + h);
            }
            for (Map<String, Object> g : jp.values()) { g.put("percentOfTotalHours", (Double) g.get("consumedHours") / total * 100.0); jobPlant.add(g); }
            for (Map<String, Object> p : pl.values()) { p.put("percentOfTotalHours", (Double) p.get("consumedHours") / total * 100.0); plant.add(p); }
        }
        out.put("jobPlant", jobPlant);
        out.put("plant", plant);
        return out;
    }

    /** btnPrint_Click:480 - the row count behind 666_01_MonthlyPlantConsumedHours_Report. */
    public Map<String, Object> allocationPrintRows(String monthDate) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rows", repo.monthlyHours(ctx.currentOrganizationId(), ctx.currentCompanyId(), sqlDate(monthDate)).size());
        return m;
    }

    /* ======================================================================== FOH Expense: pickers */

    /** AccountsBindByMonthDateDbCall:516 - the month's expense accounts with their net amount. */
    public List<Map<String, Object>> accounts(String month) {
        return plainRows(repo.expenseAccounts(ctx.currentOrganizationId(), ctx.currentCompanyId(), Date.valueOf(parseMonth(month))));
    }

    /** FillForFohExpenseData:663 - the month's hours grouped by JobOrderId + PlantId (first row's names, summed hours). */
    public List<Map<String, Object>> expenseHours(String month) {
        return groupedHours(Date.valueOf(parseMonth(month)));
    }

    private List<Map<String, Object>> groupedHours(Date monthDate) {
        List<Map<String, Object>> rows = repo.monthlyHours(ctx.currentOrganizationId(), ctx.currentCompanyId(), monthDate);
        Map<String, Map<String, Object>> g = new LinkedHashMap<>();
        for (Map<String, Object> r : rows) {
            String k = toInt(col(r, "JobOrderId")) + "|" + toInt(col(r, "PlantId"));
            Map<String, Object> e = g.get(k);
            if (e == null) {
                e = new LinkedHashMap<>();
                e.put("jobOrderId", toInt(col(r, "JobOrderId"))); e.put("jobOrderNo", str(col(r, "JobOrderNo")));
                e.put("plantId", toInt(col(r, "PlantId"))); e.put("plantName", str(col(r, "PlantName")));
                e.put("consumedHours", 0d);
                g.put(k, e);
            }
            e.put("consumedHours", (Double) e.get("consumedHours") + toDouble(col(r, "ConsumedHours")));
        }
        return new ArrayList<>(g.values());
    }

    /* ======================================================================== ReadByIdFohExpense:1044 */

    /** GetByID + the detail list; percent per FillDetailFromListCommonForReadById:1078 (hours / the plant's hours * 100). */
    public Map<String, Object> read(int id, boolean withAccounts) {
        Map<String, Object> header = ownHeader(id);
        if (header == null) throw new IllegalArgumentException("Record not found");
        List<Map<String, Object>> det = repo.detailsByHeaderId(id);
        Map<Integer, Double> plantHours = new HashMap<>();
        for (Map<String, Object> d : det) plantHours.merge(toInt(col(d, "PlantId")), toDouble(col(d, "ConsumedHours")), Double::sum);
        String month = str(col(header, "Month"));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : det) {
            double h = toDouble(col(d, "ConsumedHours"));
            double pt = plantHours.getOrDefault(toInt(col(d, "PlantId")), 0d);
            Map<String, Object> r = new LinkedHashMap<>();
            r.put("id", toInt(col(d, "Id")));
            r.put("month", month);
            r.put("accountId", toInt(col(header, "AccountId")));
            r.put("jobOrderId", toInt(col(d, "JobOrderId")));
            r.put("jobOrderNo", str(col(d, "JobOrderNo")));
            r.put("plantId", toInt(col(d, "PlantId")));
            r.put("plantName", str(col(d, "PlantName")));
            r.put("consumedHours", h);
            r.put("percentOfTotalHours", pt > 0 ? h / pt * 100.0 : 0.0);
            r.put("consumedFohAmount", toDouble(col(d, "ConsumedFohAmount")));
            r.put("remarks", str(col(d, "Remarks")));
            rows.add(r);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("month", month);
        out.put("accountId", toInt(col(header, "AccountId")));
        out.put("netAmount", toDouble(col(header, "NetAmount")));
        out.put("methodTypeId", toInt(col(header, "MethodTypeId")));
        out.put("rows", rows);
        if (withAccounts) {
            List<Map<String, Object>> acc = new ArrayList<>();
            try { acc = accounts(month); } catch (RuntimeException e) { /* an unreadable stored month leaves the account list empty */ }
            out.put("accounts", acc);
        }
        return out;
    }

    /** ReadById carries no tenancy: a record of another organization / company is not this user's. */
    private Map<String, Object> ownHeader(int id) {
        if (id <= 0) return null;
        int org = ctx.currentOrganizationId(), comp = ctx.currentCompanyId();
        for (Map<String, Object> h : repo.headerById(id)) {
            if (toInt(col(h, "OrganizationId")) == org && toInt(col(h, "CompanyId")) == comp) return h;
        }
        return null;
    }

    /* ======================================================================== History: HistoryFill:1188 */

    /**
     * basis: "entry" | "modify" | "approved" (rdentrydate / rdmodifydate / rdapproveddate); fromDate / toDate are sent only
     * when their check box is on (a blank value means off); month is the "MMM-yy" text or blank.
     */
    public List<Map<String, Object>> history(String basis, String fromDate, String toDate, String month) {
        Map<String, Boolean> rights = support.rights(W3CProductionFohRepository.SCREEN);
        boolean canViewAll = Boolean.TRUE.equals(rights.get("canViewAllRecord"));
        Date from = sqlDateOrNull(fromDate), to = sqlDateOrNull(toDate);
        Date ef = null, et = null, mf = null, mt = null, af = null, at = null;
        if ("modify".equals(basis)) { mf = from; mt = to; }
        else if ("approved".equals(basis)) { af = from; at = to; }
        else { ef = from; et = to; }
        return plainRows(repo.history(ctx.currentOrganizationId(), ctx.currentCompanyId(), ctx.currentFinancialYearId(),
                canViewAll, ctx.currentUserId(), ef, et, mf, mt, af, at, month));
    }

    /* ======================================================================== Print-922 */

    /** ProdcutionFohAllocateToJobOrderPrint922(Id): SlipAndRegister rows; Id 0 prints the whole register (Id is left out). */
    public Map<String, Object> printRows922(int id) {
        if (id > 0 && ownHeader(id) == null) throw new IllegalArgumentException("Record not found");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rows", repo.slipRows(ctx.currentOrganizationId(), ctx.currentCompanyId(), ctx.currentFinancialYearId(),
                ctx.currentBranchId(), id).size());
        return m;
    }

    /* ======================================================================== BtnDeleteFohExpense_Click:1011 */

    @Transactional(rollbackFor = Exception.class)
    public Map<String, Object> delete(int id) {
        support.require(support.rights(W3CProductionFohRepository.SCREEN), "delete", "Delete");
        if (id <= 0) throw new IllegalArgumentException("RecordId Not Found.....");
        if (ownHeader(id) == null) throw new IllegalArgumentException("RecordId Not Found.....");
        repo.deleteById(id, ctx.currentUserId());
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("result", true);
        return m;
    }

    /* ======================================================================== InsertFohExpense:860 */

    @Transactional(rollbackFor = Exception.class)
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = toInt(body.get("recId"));
        Map<String, Boolean> rights = support.rights(W3CProductionFohRepository.SCREEN);
        if (recId > 0) support.require(rights, "update", "Update"); else support.require(rights, "save", "Save");

        List<Map<String, Object>> rows = rowList(body.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");

        String month = str(body.get("month"));
        int accountId = toInt(body.get("accountId"));
        int methodTypeId = toInt(body.get("methodTypeId"));
        double netAmount = toDouble(body.get("netAmount"));
        if (month.isEmpty()) throw new IllegalArgumentException("Month field is required");
        if (accountId == 0) throw new IllegalArgumentException("Account field is required");
        if (methodTypeId == 0) throw new IllegalArgumentException("Method Type field is required");

        double sumAmt = 0;
        for (Map<String, Object> r : rows) sumAmt += toDouble(r.get("consumedFohAmount"));
        /* Math.Round(double) is round-half-even == Math.rint */
        if (Math.rint(netAmount) != Math.rint(sumAmt)) throw new IllegalArgumentException("Grid Amount not equal To Net Amount");

        if (methodTypeId != 1 && methodTypeId != 2) throw new IllegalArgumentException("Method Type field is required");
        int org = ctx.currentOrganizationId(), comp = ctx.currentCompanyId(), branch = ctx.currentBranchId();
        int fy = ctx.currentFinancialYearId(), user = ctx.currentUserId();

        LocalDate first = parseMonth(month);
        month = first.format(MMM_YY_OUT);
        if (!support.financialYearMonths().contains(month))
            throw new IllegalArgumentException("Document Date(" + month + ") is Not Valid Against Active Financial Year!");
        LocalDate last = first.plusMonths(1).minusDays(1);

        /* ------------------------------------------------------------------ fences */
        String attachmentsValues = "", customAttachmentsValues = "";
        Map<Integer, Map<String, Object>> stored = new HashMap<>();
        if (recId > 0) {
            Map<String, Object> h = ownHeader(recId);
            if (h == null) throw new IllegalArgumentException("RecId not Found");
            attachmentsValues = str(col(h, "AttachmentsValues"));
            customAttachmentsValues = str(col(h, "CustomAttachmentsValues"));
            for (Map<String, Object> d : repo.detailsByHeaderId(recId)) stored.put(toInt(col(d, "Id")), d);
            if (toInt(col(h, "AccountId")) != accountId) requireAccount(first, accountId, netAmount);
        } else {
            requireAccount(first, accountId, netAmount);
        }
        Map<String, Double> monthHours = null;
        for (Map<String, Object> r : rows) {
            int id = recId != 0 ? toInt(r.get("id")) : 0;
            int jo = toInt(r.get("jobOrderId")), pl = toInt(r.get("plantId"));
            double hrs = toDouble(r.get("consumedHours"));
            if (id > 0) {
                Map<String, Object> s = stored.get(id);
                if (s == null) throw new IllegalArgumentException("RecId not Found");
                if (toInt(col(s, "JobOrderId")) != jo || toInt(col(s, "PlantId")) != pl
                        || Math.abs(toDouble(col(s, "ConsumedHours")) - hrs) > 1e-6)
                    throw new IllegalArgumentException("The detail row does not match the stored record");
            } else {
                if (monthHours == null) {
                    monthHours = new HashMap<>();
                    for (Map<String, Object> g : groupedHours(Date.valueOf(first))) {
                        monthHours.put(toInt(g.get("jobOrderId")) + "|" + toInt(g.get("plantId")), (Double) g.get("consumedHours"));
                    }
                }
                Double h = monthHours.get(jo + "|" + pl);
                if (h == null || Math.abs(h - hrs) > 1e-6)
                    throw new IllegalArgumentException("The job order and plant hours are not those of " + month);
            }
        }

        /* ------------------------------------------------------------------ detail list (FillDetailList..:1033) */
        List<Map<String, Object>> list = new ArrayList<>();
        String remarksFromDetail = "";
        Set<Integer> seen = new HashSet<>();
        int i = 0;
        for (Map<String, Object> r : rows) {
            int id = recId != 0 ? toInt(r.get("id")) : 0;
            if (id > 0 && !seen.add(id)) throw new IllegalArgumentException("RecId not Found");
            Map<String, Object> p = new LinkedHashMap<>();
            p.put("Id", id);
            p.put("ActionTypeId", id <= 0 ? 1 : 2);
            p.put("JobOrderId", toInt(r.get("jobOrderId")));
            p.put("PlantId", toInt(r.get("plantId")));
            p.put("ConsumedHours", toDouble(r.get("consumedHours")));
            p.put("ConsumedFohAmount", toDouble(r.get("consumedFohAmount")));
            p.put("Remarks", str(r.get("remarks")));
            p.put("SortNo", 0);
            validateField(toInt(r.get("jobOrderId")), "JobOrder", i);
            validateField(toInt(r.get("plantId")), "Plant", i);
            validateField(toDouble(r.get("consumedHours")), "ConsumedHours", i);
            validateField(toDouble(r.get("consumedFohAmount")), "ConsumedFohAmount", i);
            remarksFromDetail = str(r.get("remarks"));      /* RemarksHeader is "" so every row overwrites it */
            list.add(p);
            i++;
        }

        /* ------------------------------------------------------------------ ProdcutionFohAllocateToJobOrder.Save */
        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> hp = new LinkedHashMap<>();
        hp.put("Id", recId);
        hp.put("DocumentTypeId", W3CProductionFohRepository.DOC_TYPE);
        hp.put("FromDateMonth", Timestamp.valueOf(first.atStartOfDay()));
        hp.put("ToDateMonth", Timestamp.valueOf(last.atStartOfDay()));
        hp.put("Month", month);
        hp.put("AccountId", accountId);
        hp.put("NetAmount", netAmount);
        hp.put("MethodTypeId", methodTypeId);
        hp.put("StatusId", 1);
        hp.put("Status", "Complete");
        hp.put("RemarksHeader", remarksFromDetail);
        hp.put("AttachmentsValues", attachmentsValues);
        hp.put("CustomAttachmentsValues", customAttachmentsValues);
        hp.put("OrganizationId", org);
        hp.put("CompanyId", comp);
        hp.put("BranchesId", branch);
        hp.put("ProjectId", 0);
        hp.put("FinancialYearId", fy);
        hp.put("EntryUserId", user);
        hp.put("EntryDate", now);
        hp.put("ModifyUserId", user);
        hp.put("ModifyDate", now);
        hp.put("ApprovedUserId", 0);
        hp.put("ApprovedDate", now);
        hp.put("IsApproved", Boolean.FALSE);
        hp.put("ActionId", recId == 0 ? 1 : 2);
        hp.put("PendingForView", 0);
        int id = repo.saveHeader(hp);
        if (id <= 0) id = recId;                                   /* SetData: num > 0 ? obj.Id = num : num = obj.Id */
        for (Map<String, Object> p : list) { p.put("ProdcutionFohAllocateToJobOrderId", id); repo.saveDetail(p); }

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("result", true);
        out.put("id", id);
        out.put("updated", recId != 0);
        return out;
    }

    /** The account must be one the month's ledger returns, with its net amount (to the rounding of the amount format). */
    private void requireAccount(LocalDate first, int accountId, double netAmount) {
        double tol = 0.5 * Math.pow(10, -support.amountDecimals()) + 1e-6;
        for (Map<String, Object> a : repo.expenseAccounts(ctx.currentOrganizationId(), ctx.currentCompanyId(), Date.valueOf(first))) {
            if (toInt(col(a, "AccountId")) == accountId) {
                if (Math.abs(toDouble(col(a, "NetAmount")) - netAmount) > tol)
                    throw new IllegalArgumentException("Net Amount does not match the account's amount for the month");
                return;
            }
        }
        throw new IllegalArgumentException("Account field is required");
    }

    /* ============================================================================ helpers */

    /** FormHelper.ValidateField: null / 0 / non-positive double / blank text. */
    private static void validateField(Object value, String field, int rowIndex) {
        boolean bad = value == null
                || (value instanceof Integer && (Integer) value == 0)
                || (value instanceof Double && (Double) value <= 0.0)
                || (value instanceof String && ((String) value).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    /** "MMM-yy" (the month combo text) -> the first of that month. */
    static LocalDate parseMonth(String text) {
        String t = str(text);
        if (t.isEmpty()) throw new IllegalArgumentException("Month field is required");
        try {
            return YearMonth.parse(t, MMM_YY).atDay(1);
        } catch (DateTimeParseException e) {
            throw new IllegalArgumentException("Month field is required");
        }
    }

    private static Date sqlDate(String s) {
        Date d = sqlDateOrNull(s);
        if (d == null) throw new IllegalArgumentException("Invalid date");
        return d;
    }

    private static Date sqlDateOrNull(String s) {
        String t = str(s);
        if (t.isEmpty()) return null;
        try { return Date.valueOf(LocalDate.parse(t.length() > 10 ? t.substring(0, 10) : t)); }
        catch (RuntimeException e) { throw new IllegalArgumentException("Invalid date: " + t); }
    }
}
