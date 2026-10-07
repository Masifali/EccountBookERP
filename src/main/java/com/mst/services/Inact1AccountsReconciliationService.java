package com.mst.services;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;

/**
 * Screen 32 "Accounts Reconcilation" = Architecture.WinApp.Reconciliation.AccountsReconciliation (form text "Eccount Book").
 * Every method names the form / BLL method it reproduces (AccountsReconciliation.cs, BLL 0640 AccountsReconciliationHeader,
 * DAL 0571 SetDate, ChartofAccount.ReadAllAccountgroup).
 *
 * Desktop quirks reproduced on purpose (the page must behave as the form does):
 *  - GenerateCode / ReadById send only @Activity (no @OrganizationId / @CompanyId / @Id), so the procedure sees NULLs.
 *  - BLL Save returns 0 without any database call when Id != 0 (the form still says "Record Update Successfully").
 *  - EntryUser / ModifyUser of the header are written from UserAccount.CompanyId.
 */
@Service
public class Inact1AccountsReconciliationService {
    private static final String GETALL = "Sp_AccountsReconiliation_GetAllMethod";
    private static final String OUT_OF_RANGE = "Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index";

    @Autowired private JdbcTemplate jdbc;
    @Autowired private CurrentUserContext ctx;

    private static Object ci(Map<String, Object> r, String key) {
        if (r.containsKey(key)) return r.get(key);
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    private static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        try { return (int) Double.parseDouble(o.toString().trim()); } catch (Exception e) { return 0; }
    }

    private static double toDouble(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(o.toString().trim().replace(",", "")); } catch (Exception e) { return 0; }
    }

    /** Math.Round(double) - banker's rounding. */
    private static double round(double v) { return new BigDecimal(v).setScale(0, RoundingMode.HALF_EVEN).doubleValue(); }

    private static LocalDate date(Object o, String field) {
        if (o == null || o.toString().isBlank()) throw new IllegalArgumentException(field + " Field is Required");
        String s = o.toString().trim();
        return LocalDate.parse(s.length() > 10 ? s.substring(0, 10) : s);
    }

    /** DayBookVoucher_Load: ParentAccountTitleFill = ChartofAccount.ReadAllAccountgroup (Account_Level 3), value Id / text AccountTitle. */
    public Map<String, Object> load() {
        List<Map<String, Object>> parents = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_ChartofAccount_GetAllMethodFromCOA", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(), "Account_Level", 3, "CoaType", "ReadAllAccountGroup"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("AccountTitle", ci(r, "AccountTitle"));
            parents.add(o);
        }
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("parentAccounts", parents);
        m.put("docNo", generateCodeValue());
        return m;
    }

    /** GenerateCode: BLL sends only @Activity = 'GenerateCode'; 0 -> "Max Number not Found". */
    public int generateCode() {
        int v = generateCodeValue();
        if (v > 0) return v;
        throw new IllegalStateException("Max Number not Found");
    }

    private int generateCodeValue() {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, GETALL, DesktopProc.params("Activity", "GenerateCode"));
        return rows.isEmpty() ? 0 : toInt(ci(rows.get(0), "DocNo"));
    }

    /** DetailGridFill: AccountsReconciliationGetBalances (FromDate = ToDate = DocDate; GroupAccountId / RecHId sent only when non-zero). */
    public List<Map<String, Object>> balances(Object docDate, Object groupAccountId, Object recId) {
        LocalDate d = date(docDate, "DocDate");
        int group = toInt(groupAccountId), id = toInt(recId);
        Map<String, Object> p = DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(), "VoucherDateF", d.toString(), "VoucherDateT", d.toString());
        if (group != 0) p.put("GroupAccountId", group);
        if (id != 0) p.put("RecHId", id);
        List<Map<String, Object>> lst = DesktopProc.rows(jdbc, "Sp_AccountsReconciliationGetBalances", p);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : lst) {
            if (ci(r, "MannualBalance") == null && !r.containsKey("MannualBalance"))      // lst.Rows[i]["MannualBalance"] on a table without that column
                throw new IllegalStateException("Column 'MannualBalance' does not belong to table .");
            double cd = round(toDouble(ci(r, "ClDebit"))), cc = round(toDouble(ci(r, "ClCredit"))), mb = round(toDouble(ci(r, "MannualBalance")));
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("AccountId", ci(r, "AccountId"));
            o.put("AccountCode", ci(r, "AccountCode"));
            o.put("AccountTitle", ci(r, "AccountTitle"));
            o.put("PageNo", ci(r, "GLPageNo"));
            o.put("SystemDebit", cd);
            o.put("SystemCredit", cc);
            o.put("ManualBalance", mb);
            o.put("Difference", cd + cc - Math.abs(mb));
            out.add(o);
        }
        return out;
    }

    /** txtDocNo_Leave: GetIdByDocNo (@OrganizationId, @CompanyId, @DocNo, @Activity = 'ReadIdByDocNo'). */
    public int idByDocNo(Object docNo) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, GETALL, DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(), "DocNo", toInt(docNo), "Activity", "ReadIdByDocNo"));
        return rows.isEmpty() ? 0 : toInt(ci(rows.get(0), "Id"));
    }

    /** ReadById_Update: BLL ReadById sends only @Activity = 'ReadById' and takes row [0]; no row -> ArgumentOutOfRangeException text. */
    public Map<String, Object> readById(int id) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, GETALL, DesktopProc.params("Activity", "ReadById"));
        if (rows.isEmpty()) throw new IllegalStateException(OUT_OF_RANGE);
        Map<String, Object> r = rows.get(0);
        if (toInt(ci(r, "OrganizationId")) != ctx.currentOrganizationId() || toInt(ci(r, "CompanyId")) != ctx.currentCompanyId())
            throw new IllegalStateException(OUT_OF_RANGE);
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        for (String k : new String[]{"DocDate", "DocNo", "BalanceDate", "RemarksHeader", "ReconciliationType", "AccountId"}) o.put(k, ci(r, k));
        return o;
    }

    /** FormValidation, in the form's order. */
    private void validate(Map<String, Object> b) {
        String docNo = b.get("docNo") == null ? "" : b.get("docNo").toString().trim();
        if (docNo.isEmpty() || docNo.equals("0")) throw new IllegalArgumentException("Doc No Field is Required");
        if (toInt(b.get("accountId")) == 0 && (b.get("accountText") == null || b.get("accountText").toString().trim().isEmpty()))
            throw new IllegalArgumentException("Account Title Field is Required");
        String t = b.get("reconciliationType") == null ? "" : b.get("reconciliationType").toString().trim();
        if (t.isEmpty() || t.equals("0")) throw new IllegalArgumentException("Reconciliation Type Field is Required");
    }

    /**
     * Insert() (Save / Update buttons): AccountsReconciliationHeader.Save. With Id == 0 the DAL SetDate runs in one transaction:
     * Sp_AccountsReconciliationHeader_Insert, then Sp_AccountsReconciliationDetail_Insert per grid row. With Id != 0 the BLL returns 0
     * and writes nothing. Returns the message the form shows ("" when the grid was empty - the form does nothing then).
     */
    @Transactional
    @SuppressWarnings("unchecked")
    public String save(Map<String, Object> b) {
        validate(b);
        int id = toInt(b.get("id"));
        List<Map<String, Object>> rows = (List<Map<String, Object>>) b.get("rows");
        if (rows == null || rows.isEmpty()) return "";                       // if (grd.RowCount > 0) else nothing
        int docNo;
        try { docNo = Integer.parseInt(b.get("docNo").toString().trim()); }
        catch (NumberFormatException e) { throw new IllegalArgumentException("Input string was not in a correct format."); }
        int accountId = toInt(b.get("accountId"));
        LocalDate docDate = date(b.get("docDate"), "DocDate"), upto = date(b.get("uptoDate"), "Up to Date");
        if (id == 0) {
            /* tenancy: the grid accounts must be the ones the procedure returns for this company / group / date */
            Set<Integer> allowed = new HashSet<>();
            for (Map<String, Object> r : balances(docDate.toString(), accountId, 0)) allowed.add(toInt(r.get("AccountId")));
            for (Map<String, Object> r : rows)
                if (!allowed.contains(toInt(r.get("accountId")))) throw new IllegalArgumentException("Record does not belong to the current company");
            int companyId = ctx.currentCompanyId();
            Timestamp now = Timestamp.valueOf(LocalDateTime.now());
            int hid = DesktopProc.setProc(jdbc, "Sp_AccountsReconciliationHeader_Insert", DesktopProc.params(
                    "Id", 0, "DocDate", docDate.toString(), "DocNo", docNo,
                    "ReconciliationType", b.get("reconciliationType").toString(), "BalanceDate", upto.toString(),
                    "RemarksHeader", b.get("remarks") == null ? "" : b.get("remarks").toString().trim(),
                    "OrganizationId", ctx.currentOrganizationId(), "IsApproved", false,
                    "EntryDate", now, "EntryUser", companyId, "ModifyDate", now, "ModifyUser", companyId,
                    "ApprovedUser", 0, "CompanyId", companyId, "BranchesId", 0, "ProjectsId", 0, "AccountId", accountId));
            for (Map<String, Object> r : rows) {
                DesktopProc.setProc(jdbc, "Sp_AccountsReconciliationDetail_Insert", DesktopProc.params(
                        "Id", 0, "ChartOfAccountId", toInt(r.get("accountId")),
                        "ManualBalance", (double) (float) toDouble(r.get("manualBalance")), "ErpBalance", 0d,
                        "BalanceDiff", (double) (float) toDouble(r.get("difference")), "AccountsReconiliationId", hid));
            }
        }
        return (id > 0 ? "Record Update Successfully...[" : "Record Save Successfully...[") + docNo + "]";
    }
}
