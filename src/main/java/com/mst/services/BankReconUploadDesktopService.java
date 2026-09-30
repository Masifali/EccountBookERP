package com.mst.services;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 884 "Bank Reconciliation Upload Excel Sheet" - desktop form
 * Architecture.WinApp.Account_Definition.frmBankReconciliationUploadExcelSheet (DocumentTypeId 920),
 * BLL/DAL Accounts.BankReconciliation.
 *
 * <ul>
 * <li>Bank account: DatatableHelper.GetAccountsFromGlobalByTypeIds({15}) over the login list
 *     USP_GETAllAccountsFromCustomGroups @OrganizationId, @CompanyId (type 15, first row per account).</li>
 * <li>History bank combo: USP_GetDataForDropDownFromBankReconciliation @Activity='BankAccount'.</li>
 * <li>Show / Edit / History: BankReconciliation.FormHistory -> USP_BankReconciliation_GetAllMethod
 *     @Activity='FormHistory' with @CanViewAllRecord = true.</li>
 * <li>Save / Update: BankReconciliation.SaveList -> USP_BankReconciliation_InsertAndUpdate per row
 *     (ActionId 1 new, 2 existing, 3 for rows removed in Update mode), one transaction.</li>
 * </ul>
 * The form checks no rights.
 */
@Service
public class BankReconUploadDesktopService {

    public static final int DOCUMENT_TYPE_ID = 920;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;

    public BankReconUploadDesktopService(JdbcTemplate jdbc, CurrentUserContext ctx) {
        this.jdbc = jdbc;
        this.ctx = ctx;
    }

    public Map<String, Object> lookups() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("bankAccounts", bankAccounts());
        out.put("historyBanks", DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromBankReconciliation]", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "Activity", "BankAccount")));
        out.put("defaultDaysToLess", configValue("DefaultDaysToLessFromHistoryFromDate"));
        out.put("financialYearStart", financialYearStart());
        return out;
    }

    public List<Map<String, Object>> bankAccounts() {
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[USP_GETAllAccountsFromCustomGroups]", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId()))) {
            if (toInt(r.get("AccountTypeId")) != 15) continue;
            int id = toInt(r.get("ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", id);
            m.put("AccountTitle", r.get("AccountTitle"));
            m.put("AccountCode", r.get("AccountCode"));
            m.put("ParentAccountTitle", r.get("ParentAccountTitle"));
            m.put("AccountClass", r.get("AccountClassName"));
            out.add(m);
        }
        return out;
    }

    /**
     * FormHistory. dateType "entry" / "modify" picks the Entry or Modify date pair (only the ends the
     * user ticked are sent); bankAccountId 0 is not sent.
     */
    public List<Map<String, Object>> formHistory(int bankAccountId, String dateType, String from, String to) {
        int branch = ctx.currentBranchId();
        LocalDateTime f = parse(from), t = parse(to);
        boolean entry = !"modify".equals(dateType);
        return DesktopProc.rows(jdbc, "[dbo].[USP_BankReconciliation_GetAllMethod]", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID,
                "FinancialYearId", ctx.currentFinancialYearId(),
                "BranchesId", branch != 0 ? branch : null,
                "CanViewAllRecord", true,
                "EntryFromDate", entry ? f : null,
                "EntryToDate", entry ? t : null,
                "ModifyFromDate", entry ? null : f,
                "ModifyToDate", entry ? null : t,
                "BankAccountId", bankAccountId != 0 ? bankAccountId : null,
                "Activity", "FormHistory"));
    }

    /** Insert(): rows in grid order; removed = rows deleted from the grid (sent only in Update mode). */
    @Transactional
    public String saveList(int bankAccountId, String bankAccountText, boolean updateMode,
                           List<Map<String, Object>> rows, List<Map<String, Object>> removed) {
        try {
            if (bankAccountText == null || bankAccountText.trim().isEmpty()) throw new IllegalArgumentException("Please select an account title.");
            if (rows == null || rows.isEmpty()) return null;
            int org = ctx.currentOrganizationId(), company = ctx.currentCompanyId(), user = ctx.currentUserId();
            int branch = ctx.currentBranchId(), year = ctx.currentFinancialYearId();
            LocalDateTime now = LocalDateTime.now();
            List<Map<String, Object>> list = new ArrayList<>();
            for (int i = 0; i < rows.size(); i++) {
                Map<String, Object> r = rows.get(i);
                int id = toInt(r.get("id"));
                String particulars = r.get("particulars") == null ? "" : String.valueOf(r.get("particulars"));
                BigDecimal debit = dec(r.get("debit")), credit = dec(r.get("credit"));
                if (particulars.trim().isEmpty()) throw new IllegalArgumentException("Particulars is required in Excel Data at row No: " + (i + 1));
                boolean dz = debit.signum() <= 0, cz = credit.signum() <= 0;
                if ((dz && cz) || (!dz && !cz))
                    throw new IllegalArgumentException("Either Debit or Credit must be greater than zero but not both In Row No " + (i + 1) + " in Excel Data");
                list.add(DesktopProc.params(
                        "IsApproved", false, "IsReconciled", false, "ApprovedDate", now, "EntryDate", now, "ModifyDate", now,
                        "TransactionDate", date(r.get("transactionDate"), now), "Credit", credit, "Debit", debit,
                        "ActionId", id == 0 ? 1 : 2, "ApprovedUserId", user, "BankAccountId", bankAccountId,
                        "BranchesId", branch, "CompanyId", company, "DocumentTypeId", DOCUMENT_TYPE_ID, "EntryUserId", user,
                        "FinancialYearId", year, "Id", id, "ModifyUserId", user, "OrganizationId", org, "ProjectsId", branch,
                        "VoucherHeadId", 0, "VoucherDetailId", 0,
                        "ChequeNo", r.get("chequeNo") == null ? "" : String.valueOf(r.get("chequeNo")),
                        "Particulars", particulars));
            }
            if (updateMode && removed != null) {
                for (Map<String, Object> r : removed) {
                    list.add(DesktopProc.params(
                            "IsApproved", false, "IsReconciled", false, "ApprovedDate", now, "EntryDate", now, "ModifyDate", now,
                            "TransactionDate", date(r.get("transactionDate"), now), "Credit", dec(r.get("credit")), "Debit", dec(r.get("debit")),
                            "ActionId", 3, "ApprovedUserId", user, "BankAccountId", bankAccountId, "BranchesId", 0,
                            "CompanyId", company, "DocumentTypeId", 0, "EntryUserId", user, "FinancialYearId", 0,
                            "Id", toInt(r.get("id")), "ModifyUserId", user, "OrganizationId", org, "ProjectsId", 0,
                            "VoucherHeadId", 0, "VoucherDetailId", 0,
                            "ChequeNo", r.get("chequeNo") == null ? "" : String.valueOf(r.get("chequeNo")),
                            "Particulars", r.get("particulars") == null ? "" : String.valueOf(r.get("particulars"))));
                }
            }
            int result = 0;
            for (Map<String, Object> p : list) result = DesktopProc.setProc(jdbc, "[dbo].[USP_BankReconciliation_InsertAndUpdate]", p);
            return result > 0 ? "Record's Uploaded Successfully" : null;
        } catch (RuntimeException ex) {
            Throwable t = ex;
            while (t.getCause() != null && t.getCause() != t) t = t.getCause();
            throw new IllegalStateException("Exception From Insert Method:" + (t.getMessage() != null ? t.getMessage() : ex.getMessage()), ex);
        }
    }

    private String financialYearStart() {
        try {
            for (Map<String, Object> y : DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", DesktopProc.params(
                    "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId()))) {
                if (toInt(y.get("Id")) == ctx.currentFinancialYearId()) {
                    Object s = y.get("Start_Period");
                    return s == null ? "" : String.valueOf(s);
                }
            }
        } catch (RuntimeException ignored) {
            /* the date-type shortcut simply has no year start */
        }
        return "";
    }

    private String configValue(String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    private static LocalDateTime parse(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return LocalDate.parse(s.trim().substring(0, 10)).atTime(LocalTime.now()); } catch (Exception e) { return null; }
    }

    private static LocalDateTime date(Object o, LocalDateTime fallback) {
        if (o == null || String.valueOf(o).trim().isEmpty()) return fallback;
        String s = String.valueOf(o).trim();
        try {
            if (s.length() > 10) return LocalDateTime.parse(s.replace(' ', 'T').substring(0, 19));
            return LocalDate.parse(s).atStartOfDay();
        } catch (Exception e) {
            return fallback;
        }
    }

    /** Conversion.ToDecimal: anything unparsable is 0. */
    private static BigDecimal dec(Object o) {
        if (o instanceof Number) return new BigDecimal(String.valueOf(o));
        if (o == null) return BigDecimal.ZERO;
        try { return new BigDecimal(String.valueOf(o).trim().replace(",", "")); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
