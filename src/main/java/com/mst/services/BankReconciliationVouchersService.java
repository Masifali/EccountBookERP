package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.AccountsGroupDSupport;
import com.mst.repositories.support.DesktopProc;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

import static com.mst.repositories.AccountsGroupDSupport.toDouble;
import static com.mst.repositories.AccountsGroupDSupport.toInt;

/**
 * Screen 885 "Bank Reconciliation With Vouchers" —
 * Architecture.WinApp.Account_Definition.AdjustmentVouchers.frmBankReconciliationWithVouchers
 * (ScreenName frmBankReconciliationWithVouchers, DocumentTypeId 920), BLL 0627 BankReconciliation.
 *
 * The screen never inserts BankReconciliation rows (those come from screen 884, the Excel upload);
 * it only stamps uploaded bank-statement rows with the voucher they reconcile to, and un-stamps
 * them, through [dbo].[USP_BankReconciliation_UpdateStatusWithVoucher] in one transaction
 * (StatusId 1 = reconcile with @DiffAmount, 2 = undo).
 *
 * Kept as the desktop has it:
 *  - Edit (ReadByIdAccountIdAndVoucherHeadId) lists EVERY live row of the bank account — the
 *    procedure's VoucherHeadId filter is commented out — and takes the header from the first row
 *    of the chosen voucher.
 *  - Update re-reconciles the ticked rows exactly like Save (the desktop has no separate update).
 *  - History sends @EntryUser when the user lacks "CanView AllRecord"; the procedure declares
 *    @EntryUserId, not @EntryUser, so for such a user the desktop's history fails with SQL
 *    Server's own "has no parameter named" error, and so does this one.
 */
@Service
public class BankReconciliationVouchersService {

    public static final int DOCUMENT_TYPE_ID = 920;
    private static final String SCREEN = "frmBankReconciliationWithVouchers";

    @Autowired private AccountsGroupDSupport s;

    public static class Refusal extends RuntimeException { public Refusal(String m) { super(m); } }

    /** InitializeComponentMethod + GetConfigrationsFromGlobal + BankGlAccountBindFromGlobal + HistoryComboBind. */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", s.rights(SCREEN));
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(s.config("DefaultDaysToLessFromHistoryFromDate")));
        out.put("adjustmentMaxAmount", toDouble(s.config("BankReconcilationAdjustmentMaxAmount")));
        out.put("amountDecimals", toInt(s.config("Default NoofDecimal Points For Amount")));
        out.put("financialYearStart", s.yearStart());
        out.put("bankAccounts", bankAccounts());
        out.put("historyBanks", historyBanks());
        return out;
    }

    /** BankGlAccountBindFromGlobal: GetAccountsFromGlobalByTypeIds({15}). */
    public List<Map<String, Object>> bankAccounts() {
        return s.accountsFromGlobal(new HashSet<>(Arrays.asList(15)), null, null);
    }

    /** GetDataForDropDownFromBankReconciliation(org, comp, 0, "BankAccount") — @BankAccountId not sent. */
    public List<Map<String, Object>> historyBanks() {
        UserAccount u = s.user();
        return DesktopProc.rows(s.jdbc(), "[dbo].[USP_GetDataForDropDownFromBankReconciliation]", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "BankAccount"));
    }

    /** btnShowRecords_Click → FillGrdPendingVouchers + FillGrdReconcilation. */
    public Map<String, Object> show(int bankAccountId) {
        UserAccount u = s.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("pending", DesktopProc.rows(s.jdbc(), "[dbo].[USP_GetAccountLedgerForBankReconciliation]", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "AccountId", bankAccountId)));
        out.put("rows", DesktopProc.rows(s.jdbc(), "[dbo].[USP_BankReconciliation_GetDataForReconciliationWithvouchers]", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "FinancialYearId", s.yearId(),
                "BankAccountId", bankAccountId)));
        return out;
    }

    /** ReadById → ReadByIdAccountIdAndVoucherHeadId. */
    public List<Map<String, Object>> read(int bankAccountId, int voucherHeadId) {
        return DesktopProc.rows(s.jdbc(), "[dbo].[USP_BankReconciliation_GetAllMethod]", DesktopProc.params(
                "BankAccountId", bankAccountId, "VoucherHeadId", voucherHeadId, "Activity", "ReadByIdAccountIdAndVoucherHeadId"));
    }

    /** GenerateSlip → BankReconciliation_SlipAndRegister. */
    public List<Map<String, Object>> slip(int bankAccountId, int voucherHeadId) {
        UserAccount u = s.user();
        Map<String, Object> p = DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", s.branchId(), "FinancialYearId", s.yearId());
        if (bankAccountId != 0) p.put("BankAccountId", bankAccountId);
        if (voucherHeadId != 0) p.put("VoucherHeadId", voucherHeadId);
        return DesktopProc.rows(s.jdbc(), "[dbo].[USP_BankReconciliation_SlipAndRegister]", p);
    }

    /** FillHistory → BLL 0627 FormHistory, parameter for parameter. */
    public List<Map<String, Object>> history(String dateType, String from, String to, int bankAccountId) {
        UserAccount u = s.user();
        boolean viewAll = Boolean.TRUE.equals(s.rights(SCREEN).get("canViewAll"));
        Map<String, Object> p = DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID, "FinancialYearId", s.yearId());
        if (s.branchId() != 0) p.put("BranchesId", s.branchId());
        p.put("CanViewAllRecord", viewAll);
        if (!viewAll) p.put("EntryUser", u.getId());
        String fk, tk;
        if ("entry".equals(dateType)) { fk = "EntryFromDate"; tk = "EntryToDate"; }
        else if ("modify".equals(dateType)) { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
        else if ("approved".equals(dateType)) { fk = "ApprovedFromDate"; tk = "ApprovedToDate"; }
        else { fk = "FromDate"; tk = "ToDate"; }
        if (from != null && !from.trim().isEmpty()) p.put(fk, from.trim());
        if (to != null && !to.trim().isEmpty()) p.put(tk, to.trim());
        if (bankAccountId != 0) p.put("BankAccountId", bankAccountId);
        p.put("IsReconciled", 1);                     // SkipZero = 1
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(s.jdbc(), "[dbo].[USP_BankReconciliation_GetAllMethod]", p);
    }

    /** Payload of Save / Update / Delete. */
    public static class Request {
        public Integer recId = 0;
        public Integer bankAccountId = 0;
        public Integer voucherHeadId = 0;
        public Integer voucherDetailId = 0;
        public String voucherNo;
        public String voucherAmountDr;
        public String voucherAmountCr;
        public Integer gridRowCount = 0;
        /** the ticked rows: Id, Debit, Credit (Save/Update) — every grid row's Id (Delete) */
        public List<Map<String, Object>> rows = new ArrayList<>();
    }

    /** BtnSave_Click / btnUpdate_Click → Insert() (:701). */
    @Transactional
    public Map<String, Object> save(Request r) {
        int recId = r.recId == null ? 0 : r.recId;
        s.requireRight(SCREEN, recId > 0 ? "canUpdate" : "canSave",
                recId > 0 ? "You do not have the Update right for this screen." : "You do not have the Save right for this screen.");
        if (r.gridRowCount == null || r.gridRowCount == 0) throw new Refusal("Grid Record Not Found");
        if (r.rows == null || r.rows.isEmpty()) throw new Refusal("Please check Any Record");
        if (nz(r.bankAccountId) == 0) throw new Refusal("Bank Account field is required");
        if (nz(r.voucherHeadId) == 0) throw new Refusal("Voucher No field is required");
        double max = toDouble(s.config("BankReconcilationAdjustmentMaxAmount"));
        double dr = toDouble(r.voucherAmountDr), cr = toDouble(r.voucherAmountCr);
        double sumCredit = 0d, sumDebit = 0d;
        for (Map<String, Object> row : r.rows) { sumCredit += toDouble(row.get("Credit")); sumDebit += toDouble(row.get("Debit")); }
        double sumBalance = sumDebit - sumCredit;
        double diff = 0d;
        if (dr > 0d) {
            if (!(sumBalance < 0d)) {
                throw new Refusal("Reconciliation failed: Debit amount is " + clr(dr) + ". Please select credit-side entries totaling this amount. Currently selected amount is " + clr(sumBalance) + ".");
            }
            diff = dr - Math.abs(sumBalance);
            if (Math.abs(diff) > max) throw new Refusal("Difference Amount " + clr(Math.abs(diff)) + " exceeds the maximum allowed of " + clr(max) + ".");
        } else if (cr > 0d) {
            if (!(sumBalance > 0d)) {
                throw new Refusal("Reconciliation failed: Credit amount is " + clr(cr) + ". Please select debit-side entries totaling this amount. Currently selected amount is " + clr(sumBalance) + ".");
            }
            diff = cr - Math.abs(sumBalance);
            if (Math.abs(diff) > max) throw new Refusal("Difference Amount " + clr(Math.abs(diff)) + " exceeds the maximum allowed of " + clr(max) + ".");
        }
        update(r, diff, 1, r.rows);
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("message", (recId == 0 ? "Record Saved Successfully [" : "Record Update Successfully [") + (r.voucherNo == null ? "" : r.voucherNo) + "] ");
        return res;
    }

    /** btnDelete_Click (:828): StatusId 2, DiffAmount 0, every grid row's Id. */
    @Transactional
    public Map<String, Object> delete(Request r) {
        s.requireRight(SCREEN, "canDelete", "You do not have the Delete right for this screen.");
        if (r.recId == null || r.recId == 0) throw new Refusal("Record Id not found for deletion...");
        update(r, 0d, 2, r.rows);
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("message", "Deleted Successfully");
        return res;
    }

    /** BankReconciliation_UpdateStatusWithVoucher — all eleven parameters, as AddWithValue sends them. */
    private void update(Request r, double diff, int status, List<Map<String, Object>> rows) {
        UserAccount u = s.user();
        String ids = rows.stream().map(x -> String.valueOf(toInt(x.get("Id")))).collect(Collectors.joining(","));
        JdbcTemplate jdbc = s.jdbc();
        DesktopProc.rows(jdbc, "[dbo].[USP_BankReconciliation_UpdateStatusWithVoucher]", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "FinancialYearId", s.yearId(),
                "BranchesId", s.branchId(), "EntryUserId", u.getId(), "BankAccountId", nz(r.bankAccountId),
                "VoucherHeadId", nz(r.voucherHeadId), "VoucherDetailId", nz(r.voucherDetailId),
                "DiffAmount", diff, "ReconsilationStatusId", status, "Ids", ids));
    }

    /** C# double.ToString() in an interpolated message. */
    private static String clr(double d) { return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString(); }

    private static int nz(Integer v) { return v == null ? 0 : v; }
}
