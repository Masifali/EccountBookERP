package com.mst.services;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 12 "Debit Credit Hold" - desktop form
 * Architecture.WinApp.Account_Definition.AccountsHoldForDebitOrCredit (BLL/DAL Accounts.AccountsHoldForDebitOrCredit).
 *
 * <ul>
 * <li>AccountTitleCombo: CommonServices.CoaAllocationGetAllServiceBind -> COAAllocation.GetAll ->
 *     Sp_COAAllocation_GetAllMethod @OrganizationId, @CompanyId, @UserId (only when != 0), @Activity='COAAllocationSearch';
 *     BindDDLNew(dt, cmb, "Id", "AccountTitle", "Account Title", false).</li>
 * <li>GridHistoryfill: AccountsHoldForDebitOrCredit.Formhistory -> USP_AccountsHoldForDebitOrCredit_GetAllMethod
 *     @OrganizationId, @CompanyId, @BranchesId, @Activity='FormHistory'.</li>
 * <li>ReadById / grdHistory_DoubleClick: GetById -> same proc @Id, @Activity='ReadById'.</li>
 * <li>Insert(): BLL Save -> Id == 0 USP_AccountsHoldForDebitOrCredit_Insert else _Update, one transaction (DAL SetData),
 *     with all 13 model properties as parameters.</li>
 * </ul>
 * The form has no delete and no print.
 */
@Service
public class AccountsHoldDebitCreditDesktopService {

    public static final String SCREEN_NAME = "AccountsHoldForDebitOrCredit";
    private static final DateTimeFormatter SHORT_DATE = DateTimeFormatter.ofPattern("dd/MM/yyyy");

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;

    public AccountsHoldDebitCreditDesktopService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights) {
        this.jdbc = jdbc;
        this.ctx = ctx;
        this.rights = rights;
    }

    public Map<String, Boolean> rights() {
        return rights.of(SCREEN_NAME);
    }

    /** CommonServices.CoaAllocationGetAllServiceBind (COAAllocation.GetAll). */
    public List<Map<String, Object>> accountTitles() {
        int user = ctx.currentUserId();
        List<Map<String, Object>> src = DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "UserId", user != 0 ? user : null,
                "Activity", "COAAllocationSearch"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {            /* BindDDLNew keeps only Id / AccountTitle */
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", r.get("Id"));
            m.put("AccountTitle", r.get("AccountTitle"));
            out.add(m);
        }
        return out;
    }

    /** GridHistoryfill: the dt the desktop builds from Formhistory (strings for status/reason/user, short dates). */
    public List<Map<String, Object>> history() {
        List<Map<String, Object>> lst = DesktopProc.rows(jdbc, "USP_AccountsHoldForDebitOrCredit_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "BranchesId", ctx.currentBranchId(),
                "Activity", "FormHistory"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : lst) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id")));
            m.put("CoaAccountId", toInt(r.get("CoaAccountId")));
            m.put("AccountTitle", str(r.get("AccountTitle")));
            m.put("CreditHoldStatus", str(r.get("CreditHoldStatus")));
            m.put("CreditHoldReason", str(r.get("CreditHoldReason")));
            m.put("DebitHoldStatus", str(r.get("DebitHoldStatus")));
            m.put("DebitHoldReason", str(r.get("DebitHoldReason")));
            m.put("EntryUser", str(r.get("EntryUser")));
            m.put("EntryDate", shortDate(r.get("CreatedDate")));
            m.put("ModifyUser", str(r.get("ModifyUser")));
            m.put("ModifyDate", shortDate(r.get("ModifyDate")));
            out.add(m);
        }
        return out;
    }

    /** ReadById: GetById(RecId). The proc itself has no tenancy filter, so the row is checked against the session. */
    public Map<String, Object> readById(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "USP_AccountsHoldForDebitOrCredit_GetAllMethod", DesktopProc.params(
                "Id", id, "Activity", "ReadById"));
        if (r.isEmpty()) throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.");
        Map<String, Object> row = r.get(0);
        if (toInt(row.get("OrganizationId")) != ctx.currentOrganizationId()
                || toInt(row.get("CompanyId")) != ctx.currentCompanyId()
                || toInt(row.get("BranchesId")) != ctx.currentBranchId()) {
            throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.");
        }
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", toInt(row.get("Id")));
        m.put("CoaAccountId", toInt(row.get("CoaAccountId")));
        m.put("CreditHoldStatus", toBool(row.get("CreditHoldStatus")));
        m.put("DebitHoldStatus", toBool(row.get("DebitHoldStatus")));
        m.put("CreditHoldReason", str(row.get("CreditHoldReason")));
        m.put("DebitHoldReason", str(row.get("DebitHoldReason")));
        return m;
    }

    /** Formvalidation + Insert(). The Yes/No confirmations are asked on the page before posting. */
    @Transactional
    public String save(int id, int coaAccountId, boolean debitHold, boolean creditHold, String debitReason, String creditReason) {
        Map<String, Boolean> rt = rights.of(SCREEN_NAME);
        if (id > 0) {
            if (!Boolean.TRUE.equals(rt.get("update"))) throw new IllegalArgumentException("You do not have the Update right for this screen.");
        } else if (!Boolean.TRUE.equals(rt.get("save"))) {
            throw new IllegalArgumentException("You do not have the Save right for this screen.");
        }
        String dr = debitReason == null ? "" : debitReason;
        String cr = creditReason == null ? "" : creditReason;
        if (coaAccountId <= 0) throw new IllegalArgumentException("Please enter a value for 'Account Title'.");
        if (dr.isEmpty() && debitHold) throw new IllegalArgumentException("Remarks / Reason Required For Holding Debit Entry.");
        if (cr.isEmpty() && creditHold) throw new IllegalArgumentException("Remarks / Reason Required For Holding Credit Entry.");

        LocalDateTime now = LocalDateTime.now();
        Map<String, Object> p = DesktopProc.params(
                "Id", id,
                "CoaAccountId", coaAccountId,
                "DebitHoldStatus", debitHold,
                "CreditHoldStatus", creditHold,
                "DebitHoldReason", dr,
                "CreditHoldReason", cr,
                "CreatedDate", now,
                "CreatedById", ctx.currentUserId(),
                "ModifyDate", now,
                "ModifyUserId", ctx.currentUserId(),
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "BranchesId", ctx.currentBranchId());
        if (id > 0) DesktopProc.setProc(jdbc, "[dbo].[USP_AccountsHoldForDebitOrCredit_Update]", p);
        else DesktopProc.setProc(jdbc, "[dbo].[USP_AccountsHoldForDebitOrCredit_Insert]", p);
        return id > 0 ? "Record Updated Successfully!" : "Record Saved Successfully!";
    }

    // ------------------------------------------------------------------------------ helpers

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

    /** Conversion.ToBool */
    private static boolean toBool(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        return o != null && Boolean.parseBoolean(String.valueOf(o).trim());
    }

    /** Conversion.ToDateTime(x).ToShortDateString(): empty/null becomes 1900-01-01. */
    private static String shortDate(Object o) {
        LocalDateTime d = null;
        if (o instanceof java.sql.Timestamp) d = ((java.sql.Timestamp) o).toLocalDateTime();
        else if (o instanceof java.util.Date) d = new java.sql.Timestamp(((java.util.Date) o).getTime()).toLocalDateTime();
        else if (o instanceof LocalDateTime) d = (LocalDateTime) o;
        if (d == null) return LocalDateTime.of(1900, 1, 1, 0, 0).format(SHORT_DATE);
        return d.format(SHORT_DATE);
    }
}
