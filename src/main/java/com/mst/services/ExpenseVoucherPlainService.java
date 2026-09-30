package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.DesktopVoucherDtos;
import com.mst.repositories.DesktopVoucherWriter;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * Screen 46 "Expense Voucher" - {@code Architecture.WinApp.Account_Definition.ExpenseVoucher}
 * (the plain form; ScreenDefinition 46's TargetUrl), DocumentTypeId 26, save and update.
 * A port of ExpenseVoucher.Insert() (:1893) onto the desktop's procedure chain
 * ({@link DesktopVoucherWriter}).
 *
 * DOUBLE ENTRY (:1973-2076): every grid row writes a PAIR - vd debits the row's account
 * (AgainstAccountId = the header Credit Account), vd2 credits the header Credit Account
 * (CmbRefAccountId; AgainstAccountId = the row's account) for the same amount; both carry the
 * row's LineId as SortNo. VoucherAmount is the sum of the credits.
 *
 * Budget control ("Apply On Actual Expenses" / "MonthlyBudget"), the negative-balance guard and
 * the same-amount warning are reproduced in the desktop's order. btnDelete_Click (:2443) is empty
 * on the desktop and the button is hidden - there is no delete here.
 */
@Service
public class ExpenseVoucherPlainService {

    private static final DesktopVoucherScreenService.Screen SCREEN = DesktopVoucherScreenService.Screen.EXPENSE;

    @Autowired private DesktopVoucherWriter writer;
    @Autowired private DesktopVoucherScreenService screens;
    @Autowired private CurrentUserContext ctx;
    @Autowired private JdbcTemplate jdbc;

    @Transactional
    public Map<String, Object> save(DesktopVoucherDtos.Expense dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.id == null ? 0 : dto.id;
        boolean insert = recId <= 0;
        if (insert && !screens.hasRight(SCREEN, "save")) throw new IllegalStateException("You don't have right");
        if (!insert && !screens.hasRight(SCREEN, "update")) throw new IllegalStateException("You don't have right");
        Map<String, Object> existing = insert ? null : screens.requireOwnHead(SCREEN, recId);

        boolean multiCurrency = screens.feature(6);
        boolean branchFeature = screens.feature(17);
        List<DesktopVoucherDtos.ExpenseRow> rows = dto.rows == null ? new ArrayList<>() : dto.rows;
        List<String> warnings = new ArrayList<>();

        // ---------------------------------------------------------------- Insert() :1909 + FormValidation :2447
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record not found");
        if (nz(dto.projectId) == 0) throw new IllegalArgumentException("Cost Center Field is Required");
        if (nz(dto.refAccountId) == 0) throw new IllegalArgumentException("Credit Account Field is Required");
        if (multiCurrency) {
            if (nz(dto.multiCurrencyId) == 0) throw new IllegalArgumentException("Fcy Code Field is Required");
            if (nzd(dto.exchangeCurrencyRate) == 0d) throw new IllegalArgumentException("Exchange Rate Field is Required");
            if (nzd(dto.fcAmount) == 0d) throw new IllegalArgumentException("Fcy Amount Field is Required");
        } else {
            if (nz(dto.multiCurrencyId) == 0) throw new IllegalArgumentException("Please Configure Your Base Currency In configurations");
            if (nzd(dto.exchangeCurrencyRate) == 0d) throw new IllegalArgumentException("Please Configure Your Base Currency Rate In configurations");
        }
        for (DesktopVoucherDtos.ExpenseRow r : rows) {
            if (nzd(r.amount) > 0d && nz(r.accountId) == 0) throw new IllegalArgumentException("Please Select Account Title First");
        }

        // ------------------------------------------------------------------------------ header :1942-1963
        ContraVoucherDto.Head head = new ContraVoucherDto.Head();
        head.Id = insert ? 0 : recId;
        head.DocumentTypeId = 26;
        head.VoucherCode = nz(dto.voucherCode);
        head.ProjectId = nz(dto.projectId);
        head.VoucherDate = day(dto.voucherDate);
        head.RefAccountId = nz(dto.refAccountId);
        head.AgainstAccountId = nz(dto.refAccountId);
        head.RefDocNoId = nz(dto.refAccountId);                 // :1949 - the credit account again
        head.ChequeDate = day(dto.chequeDate);
        head.Remarks = dto.remarks == null ? "" : dto.remarks.trim();
        head.CheqId = nz(dto.cheqId);
        head.ChequeNo = dto.chequeNo == null ? "" : dto.chequeNo; // CmbChequeNo.Text - never null
        head.PayTitle = dto.payTitle == null ? "" : dto.payTitle.trim();
        head.IncludeWHT = Boolean.FALSE;
        head.IsApproved = Boolean.FALSE;
        head.OrganizationId = u.getOrganizationId();
        head.CompanyId = u.getCompanyId();
        head.FinancialYearId = ctx.currentFinancialYearId();
        head.EntryUser = u.getId();
        head.ModifyUser = u.getId();
        String now = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date());
        head.EntryDate = now;
        head.ModifyDate = now;
        head.MultiCurrencyId = nz(dto.multiCurrencyId);
        head.ExchangeCurrencyRate = nzd(dto.exchangeCurrencyRate);
        head.FcAmount = nzd(dto.fcAmount);
        /* BranchId is never assigned by this Insert(): it stays 0. */
        head.BranchId = 0;
        head.AttachmentsValues = existing == null ? null : strOrNull(existing.get("AttachmentsValues"));
        head.CustomAttachmentsValues = existing == null ? "" : str(existing.get("CustomAttachmentsValues"));

        // ----------------------------------------------------------------------- budget control inputs
        /* "Apply On Actual Expenses" is read with bool.Parse (:1984) - only True/False parse. */
        boolean applyOnActual = parseBoolStrict(screens.config("Apply On Actual Expenses"));
        String monthlyBudget = screens.config("MonthlyBudget");
        /* DebitAmtGet (:2403) - on an update, ReadById summed the loaded debit lines whose account
           has a budget row; recomputed here from the stored voucher the same way. */
        double debitAmtGet = 0d;
        if (!insert && applyOnActual) {
            String oldDate = day(str(existing.get("VoucherDate")));
            for (Map<String, Object> d : screens.readDetails(recId)) {
                if (!budgetBalances(oldDate, DesktopVoucherScreenService.toInt(d.get("AccountId"))).isEmpty()) {
                    debitAmtGet += DesktopVoucherScreenService.toDouble(d.get("DebitAmount"));
                }
            }
        }

        // ------------------------------------------------------------------------------ detail :1965-2077
        List<ContraVoucherDto.Detail> details = new ArrayList<>();
        int userBranch = u.getBranchesId() == null ? 0 : u.getBranchesId();
        double debitAccum = 0d;                                  // DebitAmount, declared outside the loop
        double voucherAmount = 0d;
        int lineId = 1;
        for (int i = 0; i < rows.size(); i++) {
            DesktopVoucherDtos.ExpenseRow r = rows.get(i);
            ContraVoucherDto.Detail vd = new ContraVoucherDto.Detail();
            vd.SubsidiaryTypeId = nz(r.subsidiaryAccountTypeId);
            int sub = nz(r.subsidiaryAccountId);
            if (sub > 0) {
                vd.SupplierCustomerId = vd.SubsidiaryTypeId == 1 ? sub : 0;
                vd.EmployeeId = vd.SubsidiaryTypeId == 2 ? sub : 0;
                vd.SubsidiaryAccountId = sub;
            }
            vd.AccountId = nz(r.accountId);

            if (applyOnActual) {
                /* :1986-2028, reproduced as written: the budget row is read with OrganizationId /
                   CompanyId left at 0 (the ReportsParameters sets only DocDate and AccountId), and when
                   it matches, the amounts of EVERY grid row are added to the running total. */
                List<Map<String, Object>> dt = budgetBalances(head.VoucherDate, vd.AccountId);
                if (!dt.isEmpty()) {
                    int budgetAc = DesktopVoucherScreenService.toInt(dt.get(0).get("AccountId"));
                    for (DesktopVoucherDtos.ExpenseRow x : rows) {
                        if (budgetAc == vd.AccountId) debitAccum += nzd(x.amount);
                    }
                    if (budgetAc == vd.AccountId && monthlyBudget != null) {
                        double balance = DesktopVoucherScreenService.toDouble(dt.get(0).get("DebitAmount"));
                        double utilize = debitAccum;
                        if (!insert) balance += debitAmtGet;
                        double diff = Math.rint(balance) - Math.rint(utilize);
                        String msg = "Accumulated Monthly Budget For " + str(r.accountTitle) + " Up is"
                                + net(balance) + " It will exceed by " + net(diff);
                        if ("Stop".equals(monthlyBudget) && utilize > balance) throw new IllegalArgumentException(msg);
                        if ("Warn".equals(monthlyBudget) && utilize > balance) warnings.add(msg);
                    }
                }
            }

            vd.LineId = lineId;
            vd.SortNo = nz(r.lineId);
            vd.AgainstAccountId = nz(dto.refAccountId);
            vd.JobLotId = nz(r.jobLotId);
            vd.Comments = r.remarks == null ? "" : r.remarks;
            vd.DebitAmount = nzd(r.amount);
            vd.InvoiceNoRefId = head.CheqId;
            vd.CheqNoDetail = head.ChequeNo;
            vd.DCheqDate = head.ChequeDate;
            vd.IsTaxable = "False";
            vd.PayeeTitle = head.PayTitle;
            vd.DMultiCurrencyId = nz(dto.multiCurrencyId);
            vd.CostCenterId = nz(r.costCenterId);
            vd.DExchangeCurrencyRate = nzd(dto.exchangeCurrencyRate);
            vd.DCurrencyAmount = nzd(r.fcyAmount);
            vd.BranchesId = branchFeature ? nz(r.branchId) : userBranch;
            if (vd.DCurrencyAmount == 0d) throw new IllegalArgumentException("Fcy Amount Not Found In Detail Grid at Row#" + (i + 1));
            details.add(vd);
            lineId++;

            ContraVoucherDto.Detail vd2 = new ContraVoucherDto.Detail();
            vd2.LineId = lineId;
            vd2.SortNo = nz(r.lineId);
            vd2.AccountId = nz(dto.refAccountId);
            vd2.AgainstAccountId = nz(r.accountId);
            vd2.JobLotId = nz(r.jobLotId);
            vd2.Comments = r.remarks == null ? "" : r.remarks;
            vd2.CreditAmount = nzd(r.amount);
            vd2.InvoiceNoRefId = head.CheqId;
            vd2.CheqNoDetail = head.ChequeNo;
            vd2.DCheqDate = head.ChequeDate;
            vd2.PayeeTitle = head.PayTitle;
            vd2.IsTaxable = "False";
            vd2.DMultiCurrencyId = nz(dto.multiCurrencyId);
            vd2.DExchangeCurrencyRate = nzd(dto.exchangeCurrencyRate);
            vd2.CostCenterId = nz(r.costCenterId);
            vd2.DCurrencyAmount = nzd(r.fcyAmount);
            vd2.BranchesId = branchFeature ? nz(r.branchId) : userBranch;
            if (vd2.DCurrencyAmount == 0d) throw new IllegalArgumentException("Fcy Amount Not Found In Detail Grid at Row#" + (i + 1));
            voucherAmount += vd2.CreditAmount;
            details.add(vd2);
            lineId++;
        }
        head.VoucherAmount = voucherAmount;

        // ------------------------------------------------------------------- negative balance :2088-2104
        if (!screens.configBool("DisableBothNegativeBalanceRestrictions")) {
            double closing = Math.rint(screens.accountBalance(head.RefAccountId, head.VoucherDate));
            if (voucherAmount > closing) {
                String msg = "Debit Amount cannot be greater than Account Balance.\nAccount '" + str(dto.refAccountTitle)
                        + "' Balance is " + net(closing) + " and Total Debit Amount is " + net(voucherAmount) + ".";
                if (screens.configBool("is Minus balance Allowed")) throw new IllegalArgumentException(msg);
                if (screens.configBool("DisplayWarningforNegativeBalance") && !Boolean.TRUE.equals(dto.negativeBalanceAcknowledged)) {
                    throw new ContraVoucherService.ConfirmationRequiredException(msg, "negativeBalance");
                }
            }
        }

        // --------------------------------------------- VoucherExistWithSameAmountInSameDate :2106
        /* vd.ActionId is never set on this form, so the procedure receives @ActionId = 0 and, by its
           own IF branches, returns nothing - the desktop never shows this warning here. The call is
           still made, exactly as the desktop makes it. */
        if (!Boolean.TRUE.equals(dto.duplicateAcknowledged)) {
            Set<Integer> seen = new LinkedHashSet<>();
            for (ContraVoucherDto.Detail d : details) {
                if (nzd(d.DebitAmount) <= 0d || !seen.add(nz(d.AccountId))) continue;
                String title = writer.duplicateVoucherTitle(u.getOrganizationId(), u.getCompanyId(),
                        head.VoucherDate, nz(d.AccountId), nzd(d.DebitAmount), nz(d.ActionId));
                if (title != null) {
                    throw new ContraVoucherService.ConfirmationRequiredException(
                            "Voucher against '" + title + "' with same Debit Amount already exists on this date. Do you want to continue?",
                            "duplicate");
                }
            }
        }

        // ----------------------------------------------------------------------- cost centres :2110-2136
        List<ContraVoucherDto.CostCentre> costs = new ArrayList<>();
        if (dto.costCenters != null) {
            for (DesktopVoucherDtos.CostCenterBreakup b : dto.costCenters) {
                ContraVoucherDto.CostCentre c = new ContraVoucherDto.CostCentre();
                c.Id = nz(b.id);
                c.SortNo = nz(b.lineId);
                c.CostCenterId = nz(b.costCenterId);
                c.costPrcent = BigDecimal.valueOf(nzd(b.percent));
                c.costAmount = nzd(b.amount);
                costs.add(c);
            }
        }
        for (ContraVoucherDto.Detail line : details) {
            if (nzd(line.DebitAmount) <= 0d) continue;
            double amt = 0d;
            BigDecimal pct = BigDecimal.ZERO;
            boolean any = false;
            for (ContraVoucherDto.CostCentre c : costs) {
                if (c.SortNo != null && c.SortNo.intValue() == nz(line.SortNo)) {
                    any = true;
                    amt += c.costAmount;
                    pct = pct.add(c.costPrcent);
                }
            }
            if (!any) continue;
            if (pct.setScale(0, RoundingMode.HALF_EVEN).compareTo(new BigDecimal("100")) != 0) {
                throw new IllegalArgumentException("Cost Center BreakUp Percent of LineId" + line.LineId + " not equal to 100");
            }
            if (amt != nzd(line.DebitAmount)) {
                throw new IllegalArgumentException("Cost Center BreakUp Amount and Detail Amount of LineId" + line.LineId + " not Match");
            }
        }

        int id = writer.save(head, details, costs);

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", id);
        res.put("voucherCode", head.VoucherCode);
        res.put("warnings", warnings);
        res.put("message", (insert ? "Voucher Save Successfully...[" : "Voucher Update Successfully...[") + head.VoucherCode + "]");
        return res;
    }

    /** AccountsBudgetHeader.BudgetBalances (BLL 0625:244) with the form's own argument: a new
     *  ReportsParameters carrying only DocDate and AccountId, so @OrganizationId/@CompanyId are 0. */
    private List<Map<String, Object>> budgetBalances(String voucherDate, int accountId) {
        return DesktopProc.rows(jdbc, "Sp_AccountBudget_GetAllMethod",
                DesktopProc.params("OrganizationId", 0, "CompanyId", 0, "BudgetGlAcId", accountId,
                        "VoucherDate", voucherDate, "Activity", "BudgetBalances"));
    }

    /** bool.Parse: "True"/"False" in any case; anything else throws, as it does on the desktop
     *  (where the exception reaches Insert()'s catch and the save stops). A missing row is false. */
    private static boolean parseBoolStrict(String v) {
        if (v == null) return false;
        String s = v.trim();
        if ("true".equalsIgnoreCase(s)) return true;
        if ("false".equalsIgnoreCase(s)) return false;
        throw new IllegalArgumentException("String was not recognized as a valid Boolean.");
    }

    /** .NET's default double.ToString(): no trailing ".0". */
    static String net(double d) {
        if (d == Math.rint(d) && !Double.isInfinite(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
    }

    private static int nz(Integer v) { return v == null ? 0 : v; }
    private static double nzd(Double v) { return v == null ? 0d : v; }
    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }
    private static String strOrNull(Object v) { return v == null ? null : String.valueOf(v); }
    private static String day(String v) {
        if (v == null || v.trim().isEmpty()) return null;
        String s = v.trim();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }
}
