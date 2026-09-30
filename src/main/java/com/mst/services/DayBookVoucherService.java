package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.DayBookVoucherDto;
import com.mst.repositories.AccountsGroupDSupport;
import com.mst.repositories.DesktopVoucherWriter;
import com.mst.repositories.support.DesktopProc;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Date;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.AccountsGroupDSupport.toBool;
import static com.mst.repositories.AccountsGroupDSupport.toDouble;
import static com.mst.repositories.AccountsGroupDSupport.toInt;

/**
 * The two desktop Day Book entry screens, both written through the desktop's own voucher chain
 * ({@link DesktopVoucherWriter} = BLL 0654 VoucherHead.Save → DAL 0586 SetData):
 *
 * <pre>
 *  kind "cash"   screen 15 "Day Book"            DayBook.cs     ScreenName DayBook     DocumentTypeId 9
 *  kind "offset" screen 24 "Day Book (Off Set)"  frmDayBook.cs  ScreenName frmDayBook  DocumentTypeId 8
 * </pre>
 *
 * BLL 0654 Save assigns no VoucherType to document types 8 and 9, so neither the Payment/Receipt
 * re-pairing nor the VoucherAmount recomputation runs for them: the details go to the DAL exactly
 * as each form built them. DAL 0586 then runs Sp_VoucherHead_Insert|_Update, Sp_VoucherDetail_Insert
 * per line, USP_VoucherBalanceCheck, Sp_VoucherHead_H_Insert, Sp_VoucherDetail_H_Insert per line
 * and [DAW].[USp_DocumentApprovalDetail_Insert], in one transaction.
 *
 * Not reproduced: attachments (header and per-row, DMSAttachments) — the Attachment buttons say so
 * on the page. The 144 / 102 prints are the rows of the desktop's own procedures shown in the
 * browser, not the Crystal layouts.
 */
@Service
public class DayBookVoucherService {

    public static final int DOC_CASH = 9;     // DayBook.cs Insert(): Vh.DocumentTypeId = 9
    public static final int DOC_OFFSET = 8;   // frmDayBook.cs btnsave_Click: vh.DocumentTypeId = 8

    @Autowired private AccountsGroupDSupport s;
    @Autowired private DesktopVoucherWriter writer;

    /** A desktop refusal (MessageBox) in the desktop's own words. */
    public static class Refusal extends RuntimeException {
        public Refusal(String m) { super(m); }
    }

    /** A desktop Yes/No the operator must answer before the save continues. */
    public static class Confirm extends RuntimeException {
        public final String kind;
        public Confirm(String m, String kind) { super(m); this.kind = kind; }
    }

    public static int docType(String kind) {
        if ("cash".equals(kind)) return DOC_CASH;
        if ("offset".equals(kind)) return DOC_OFFSET;
        throw new Refusal("Unknown day book kind");
    }

    public static String screenName(String kind) {
        return docType(kind) == DOC_CASH ? "DayBook" : "frmDayBook";
    }

    // ================================================================================== load

    /**
     * DayBook_Load / frmDayBook_Load: rights, the configuration switches and ERP features the forms
     * read, the voucher code, and every dropdown's rows.
     */
    public Map<String, Object> init(String kind) {
        int doc = docType(kind);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", s.rights(screenName(kind)));
        boolean inventoryRelated = toBool(s.config("InventoryRelatedAccountsShowInVouchers"));
        boolean subsidiary = s.feature(4);                       // GetERPFeatureById(4)
        out.put("subsidiaryFeature", subsidiary);
        out.put("voucherCode", nextCode(doc));
        // CommonServices.GetDecimalConfiguration: stringFormatsingle / stringFormatboth / DecimalRateFormate
        out.put("amountDecimals", toInt(s.config("Default NoofDecimal Points For Amount")));
        out.put("rateDecimals", toInt(s.config("Default NoofDecimal Points For Rate")));
        // CashAccountFill: GetAccountsFromGlobalByTypeIds({2})
        out.put("cashAccounts", s.accountsFromGlobal(set(2), null, null));
        out.put("subsidiaryAccounts", subsidiary ? subsidiaryAccounts() : new ArrayList<>());
        out.put("voucherDates", voucherDates(doc));
        if (doc == DOC_CASH) {
            // DayBook.AccountsFill: exclude types {4,12} unless InventoryRelatedAccountsShowInVouchers
            // (then new int[1] = {0}), and PL note 2 unless the same switch is on.
            out.put("detailAccounts", s.accountsFromGlobal(null,
                    inventoryRelated ? set(0) : set(4, 12), inventoryRelated ? null : set(2)));
        } else {
            boolean multiCurrency = s.feature(6);                // MultiCurrencyFeature(): GetERPFeatureById(6)
            out.put("multiCurrencyFeature", multiCurrency);
            // frmDayBook.AccountsFill runs BEFORE MultiCurrencyFeature() on load, so the flag it
            // reads is still false there: type 22 is excluded on the first fill ({4,12,22} or {22}).
            // A Refresh re-runs it with the real flag; the page asks for that through /accounts.
            out.put("detailAccounts", offsetAccounts(inventoryRelated, false));
            out.put("temporaryAccountId", toInt(s.config("DayBookAcTemporary")));
            out.put("chequeBookEnabled", toBool(s.config("CheqBook Enabled")));
            out.put("currencies", currencies());
            // DefaultConfigurations(): "Base Currency" and "BaseCurrencyRate" (read through ToInt)
            out.put("baseCurrencyId", toInt(s.config("Base Currency")));
            out.put("baseCurrencyRate", toInt(s.config("BaseCurrencyRate")));
        }
        return out;
    }

    /** btnRefresh: the account lists again (frmDayBook with the real multi-currency flag). */
    public Map<String, Object> accounts(String kind, boolean multiCurrencyKnown) {
        int doc = docType(kind);
        Map<String, Object> out = new LinkedHashMap<>();
        boolean inventoryRelated = toBool(s.config("InventoryRelatedAccountsShowInVouchers"));
        out.put("cashAccounts", s.accountsFromGlobal(set(2), null, null));
        if (doc == DOC_CASH) {
            out.put("detailAccounts", s.accountsFromGlobal(null,
                    inventoryRelated ? set(0) : set(4, 12), inventoryRelated ? null : set(2)));
            if (s.feature(4)) out.put("subsidiaryAccounts", subsidiaryAccounts());
        } else {
            out.put("detailAccounts", offsetAccounts(inventoryRelated, multiCurrencyKnown && s.feature(6)));
            out.put("temporaryAccountId", toInt(s.config("DayBookAcTemporary")));
            out.put("currencies", currencies());
            out.put("baseCurrencyId", toInt(s.config("Base Currency")));
            out.put("baseCurrencyRate", toInt(s.config("BaseCurrencyRate")));
        }
        return out;
    }

    private List<Map<String, Object>> offsetAccounts(boolean inventoryRelated, boolean multiCurrency) {
        int t22 = multiCurrency ? 0 : 22;
        Set<Integer> without = inventoryRelated ? set(t22) : set(4, 12, t22);
        return s.accountsFromGlobal(null, without, inventoryRelated ? null : set(2));
    }

    /** VoucherHead.BindSubsidiaryAccount(CompanyId, 0, null) — USP_GetSubsidiaryAccountsByParentAccount @CompanyId. */
    public List<Map<String, Object>> subsidiaryAccounts() {
        return DesktopProc.rows(s.jdbc(), "USP_GetSubsidiaryAccountsByParentAccount",
                DesktopProc.params("CompanyId", s.user().getCompanyId()));
    }

    /** MultiCurrency.GetAll — Sp_MultiCurrency_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> currencies() {
        UserAccount u = s.user();
        return DesktopProc.rows(s.jdbc(), "Sp_MultiCurrency_GetAllMethod", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** GetVoucherDateForDaybookDropdown(docType) — usp_getvouchersdateforDaybookDropdown. */
    public List<Map<String, Object>> voucherDates(int doc) {
        UserAccount u = s.user();
        return DesktopProc.rows(s.jdbc(), "usp_getvouchersdateforDaybookDropdown", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", doc));
    }

    /** GenerateVoucherCodeByDocumentTypeId (both forms send the same five parameters). */
    public int nextCode(int doc) {
        UserAccount u = s.user();
        return writer.nextVoucherCode(u.getOrganizationId(), u.getCompanyId(), doc, s.yearId(), s.branchId());
    }

    public Map<String, Object> balance(int accountId, String date) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("balance", s.glBalance(accountId, date));
        return m;
    }

    /** frmDayBook.BindChequeNo → CheqBookHeader.OutstandingCheqNo (BLL 0647). */
    public List<Map<String, Object>> cheques(int bankId) {
        UserAccount u = s.user();
        return DesktopProc.rows(s.jdbc(), "SP_CheqBookHeader_GetAllMethod", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BankId", bankId, "MethodType", "OutstandingCheqNo"));
    }

    /** frmDayBook.cmbCurrency_Leave → GetLastExchangeRateAndCurrencyOfVoucher (DocumentTypeIds "8"). */
    public Map<String, Object> lastRate(int currencyId) {
        UserAccount u = s.user();
        List<Map<String, Object>> r = DesktopProc.rows(s.jdbc(), "Sp_Vouchers_GetMethods", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeIds", "8", "DMultiCurrencyIds", String.valueOf(currencyId),
                "Activity", "GetMultiCurrencyAndLastRate"));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("found", !r.isEmpty());
        m.put("lastExchRate", r.isEmpty() ? 0d : toDouble(r.get(0).get("LastExchRate")));
        return m;
    }

    // =============================================================================== history

    /**
     * HistoryFill → VoucherHead.DayBookHistory: @ChartOfAccountId only when a cash account is
     * chosen, @DocumentTypeId always (9 or 8 — the procedure itself reads IN (8,9)), and
     * @VoucherDate = Conversion.ToDateTime(combo text), which is 1900-01-01 when nothing is chosen.
     */
    public List<Map<String, Object>> history(String kind, String voucherDate, int cashAccountId) {
        UserAccount u = s.user();
        Map<String, Object> p = DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "FinancialYearId", s.yearId());
        if (cashAccountId != 0) p.put("ChartOfAccountId", cashAccountId);
        p.put("DocumentTypeId", docType(kind));
        p.put("VoucherDate", blank(voucherDate) ? "1900-01-01" : voucherDate);
        p.put("Activity", "DayBookHistory");
        return DesktopProc.rows(s.jdbc(), "Sp_Vouchers_GetMethods", p);
    }

    /** VoucherHead.GetByID: 'ReadByID' (first row, or the desktop's index error) + its details. */
    public Map<String, Object> read(int id) {
        List<Map<String, Object>> head = DesktopProc.rows(s.jdbc(), "Sp_Vouchers_GetMethods",
                DesktopProc.params("Id", id, "Activity", "ReadByID"));
        if (head.isEmpty()) {
            throw new Refusal("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", head.get(0));
        out.put("details", DesktopProc.rows(s.jdbc(), "Sp_Vouchers_GetMethods",
                DesktopProc.params("Id", id, "Activity", "VoucherDetail_ReadByVoucherHeadID")));
        return out;
    }

    // ================================================================================= prints

    /**
     * GeneralReprots.DayBookSlip (BLL 0140) → Sp_DayBookSlip. Each argument is sent only when the
     * desktop sends it: DocumentTypeId / ChartOfAccountId / Id when non-zero, SkipCashTrans when
     * the "without cash" box is ticked, VoucherDate when it is not the 1900/0001 "empty" date.
     * The history toolbar's 144-Print reads the date combo's Value (the row number), which
     * Conversion.ToDateTime turns into 1900-01-01 — so that print never sends a date.
     */
    public List<Map<String, Object>> daybookSlip(int documentTypeId, int id, int accountId, boolean skipCash) {
        UserAccount u = s.user();
        Map<String, Object> p = DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (accountId != 0) p.put("ChartOfAccountId", accountId);
        if (id != 0) p.put("Id", id);
        if (skipCash) p.put("SkipCashTrans", 1);
        return DesktopProc.rows(s.jdbc(), "Sp_DayBookSlip", p);
    }

    /** CommonServices.AcRptPaymentReceiptsVoucherSlip_102 → VoucherReports.VoucherReport (BLL 0141). */
    public List<Map<String, Object>> voucherSlip102(int voucherHeadId, int documentTypeId) {
        if (voucherHeadId == 0) throw new Refusal("VoucherId Not Found");
        UserAccount u = s.user();
        return DesktopProc.rows(s.jdbc(), "Sp_Vouchers_PaymentReceiptVoucherSlip_Rpt", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", voucherHeadId, "DocumentTypeId", documentTypeId));
    }

    // =============================================================================== save 15

    /** DayBook.cs btnsave_Click / btnUpdate_Click → Insert() (:1456). */
    @Transactional
    public Map<String, Object> saveCash(DayBookVoucherDto dto) {
        boolean update = dto.id != null && dto.id > 0;
        s.requireRight("DayBook", update ? "canUpdate" : "canSave",
                update ? "You do not have the Update right for this screen." : "You do not have the Save right for this screen.");
        // FormValidation()
        if (toInt(dto.voucherCode) == 0) throw new Refusal("Doc No Field is Required");
        if (nz(dto.cashAccountId) == 0) throw new Refusal("cash Account Field is Required");
        if (dto.receipts.isEmpty() && dto.payments.isEmpty()) {
            throw new Refusal("Add AtLeast One Row In Either Payment Side Or Receipt Side");
        }
        UserAccount u = s.user();
        int cash = nz(dto.cashAccountId);
        ContraVoucherDto.Head vh = newHead(u, update ? dto.id : 0, DOC_CASH, toInt(dto.voucherCode), dto.voucherDate);
        vh.Remarks = dto.remarks == null ? "" : dto.remarks;
        vh.RefAccountId = cash;
        vh.AgainstAccountId = cash;

        List<ContraVoucherDto.Detail> details = new ArrayList<>();
        double credit = 0d, debit = 0d;
        for (DayBookVoucherDto.Line r : dto.receipts) {
            ContraVoucherDto.Detail d = cashLine(r, cash);
            d.CreditAmount = nzd(r.amount);
            credit += d.CreditAmount;
            details.add(d);
        }
        for (DayBookVoucherDto.Line r : dto.payments) {
            ContraVoucherDto.Detail d = cashLine(r, cash);
            d.DebitAmount = nzd(r.amount);
            debit += d.DebitAmount;
            details.add(d);
        }
        // The balancing cash line (:1561-1584) — only when the two sides differ.
        double difference = debit - credit;
        if (Math.abs(difference) > 0d) {
            ContraVoucherDto.Detail c = new ContraVoucherDto.Detail();
            c.AccountId = cash;
            c.AgainstAccountId = cash;
            if (debit == 0d) {
                c.DebitAmount = credit;
                c.CreditAmount = 0d;
            } else if (credit == 0d) {
                c.CreditAmount = debit;
                c.DebitAmount = 0d;
            } else {
                c.CreditAmount = difference > 0d ? difference : 0d;
                c.DebitAmount = difference < 0d ? -difference : 0d;
            }
            c.Comments = difference > 0d ? "Cash Account Credit With Difference Of " + clr(difference)
                    : difference < 0d ? "Cash Account Debit With Difference Of " + clr(-difference) : "";
            details.add(c);
        }
        int id = writer.save(vh, details, new ArrayList<>());
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", id);
        res.put("message", (update ? "Record Update Successfully...[" : "Record Save Successfully...[") + vh.VoucherCode + "]");
        return res;
    }

    /** One ReceiptGrid / PaymentGrid row as Insert() copies it (:1518-1534). */
    private static ContraVoucherDto.Detail cashLine(DayBookVoucherDto.Line r, int cash) {
        ContraVoucherDto.Detail d = new ContraVoucherDto.Detail();
        d.AccountId = nz(r.accountId);
        d.SubsidiaryTypeId = nz(r.subsidiaryAccountTypeId);
        int sub = nz(r.subsidiaryAccountId);
        if (sub > 0) {
            d.SupplierCustomerId = d.SubsidiaryTypeId == 1 ? sub : 0;
            d.EmployeeId = d.SubsidiaryTypeId == 2 ? sub : 0;
            d.SubsidiaryAccountId = sub;
        }
        d.AgainstAccountId = cash;
        d.SortNo = nz(r.pageNo);
        d.DCheqDate = day(r.chequeDate);
        d.CheqNoDetail = r.chequeRef == null ? "" : r.chequeRef;
        d.Comments = r.remarks == null ? "" : r.remarks;
        d.LineId = nz(r.lineId);
        return d;
    }

    // =============================================================================== save 24

    /** frmDayBook.cs btnsave_Click (:926); btnUpdate_Click only checks RecId and calls it. */
    @Transactional
    public Map<String, Object> saveOffset(DayBookVoucherDto dto) {
        boolean update = dto.id != null && dto.id > 0;
        s.requireRight("frmDayBook", update ? "canUpdate" : "canSave",
                update ? "You do not have the Update right for this screen." : "You do not have the Save right for this screen.");
        boolean multiCurrency = s.feature(6);
        // FormValidation()
        if (dto.voucherCode == null || dto.voucherCode.isEmpty()) throw new Refusal("DocNo Required");
        if (nz(dto.cashAccountId) == 0) throw new Refusal("Cash Account Required");
        if (multiCurrency) {
            if (nz(dto.currencyId) == 0) throw new Refusal("Fcy Code Field is Required");
            if (toDouble(dto.exchangeRate) == 0d) throw new Refusal("Exchange Rate Field is Required");
            if (toDouble(dto.fcyAmount) == 0d) throw new Refusal("Fcy Amount Field is Required");
        } else {
            if (nz(dto.currencyId) == 0) throw new Refusal("Please Configure Your Base Currency In configurations");
            String rate = dto.exchangeRate == null ? "" : dto.exchangeRate.trim();
            if (rate.isEmpty() || "0".equals(rate)) throw new Refusal("Please Configure Your Base Currency Rate In configurations");
        }
        if (dto.rows.isEmpty()) throw new Refusal("Grid Fields Required");

        double tempDebit = 0d, tempCredit = 0d;
        for (DayBookVoucherDto.Row r : dto.rows) {
            if (nzd(r.amountDr) > 0d && nz(r.accountId) == 0) throw new Refusal("Please Select Debit Account Title First");
            if (nz(r.accountId) == nz(dto.temporaryAccountId)) {
                tempDebit += cInt(nzd(r.amountDr));      // Conversion.ToInt — rounded to a whole number
                tempCredit += cInt(nzd(r.amountCr));
            }
        }
        if (tempDebit != tempCredit) {
            throw new Refusal("Temporary Account Debit And Credit not equal.Debit is " + clr(tempDebit) + " and Credit is " + clr(tempCredit));
        }
        for (DayBookVoucherDto.Row r : dto.rows) {
            if (nzd(r.amountCr) > 0d && nz(r.accountId) == 0) throw new Refusal("Please Select Credit Account Title First");
        }

        UserAccount u = s.user();
        ContraVoucherDto.Head vh = newHead(u, update ? dto.id : 0, DOC_OFFSET, toInt(dto.voucherCode.trim()), dto.voucherDate);
        vh.DocumentTypeSrNo = 0;
        vh.RefAccountId = nz(dto.cashAccountId);
        vh.Remarks = dto.remarks == null ? "" : dto.remarks;
        vh.MultiCurrencyId = nz(dto.currencyId);
        vh.ExchangeCurrencyRate = toDouble(dto.exchangeRate);
        vh.FcAmount = toDouble(dto.fcyAmount);

        List<ContraVoucherDto.Detail> details = new ArrayList<>();
        double debit = 0d, credit = 0d;
        for (DayBookVoucherDto.Row r : dto.rows) {
            ContraVoucherDto.Detail d = new ContraVoucherDto.Detail();
            d.SubNo = nz(r.subCode);
            d.AccountId = nz(r.accountId);
            d.AgainstAccountId = nz(r.againstAccountId);
            d.SubsidiaryTypeId = nz(r.subsidiaryAccountTypeId);
            int sub = nz(r.subsidiaryAccountId);
            if (sub > 0) {
                d.SupplierCustomerId = d.SubsidiaryTypeId == 1 ? sub : 0;
                d.EmployeeId = d.SubsidiaryTypeId == 2 ? sub : 0;
                d.SubsidiaryAccountId = sub;
            }
            d.Comments = r.remarks == null ? "" : r.remarks;
            d.CheqNoDetail = r.cheqNo == null ? "" : r.cheqNo;
            d.InvoiceNoRefId = nz(r.cheqId);
            d.TotalDebitAmount = nzd(r.totalAmountDr);
            d.TotalCreditAmount = nzd(r.totalAmountCr);
            d.DebitAmount = nzd(r.amountDr);
            d.CreditAmount = nzd(r.amountCr);
            d.LineId = nz(r.lineId);
            d.DMultiCurrencyId = nz(dto.currencyId);
            d.DExchangeCurrencyRate = toDouble(dto.exchangeRate);
            d.DCurrencyAmount = d.DebitAmount > 0d ? nzd(r.debitFcyAmount) : nzd(r.creditFcyAmount);
            debit += d.DebitAmount;
            credit += d.CreditAmount;
            details.add(d);
        }
        if (credit != debit) throw new Refusal("Debit & Credit side Must Be equal");
        vh.VoucherAmount = debit;
        vh.BillAmount = credit;

        // VoucherHead.VoucherExistWithSameAmountInSameDate: one call per distinct AccountId among
        // the debit lines, first line of each; the detail's ActionId (still 0) is what is sent.
        if (!Boolean.TRUE.equals(dto.duplicateAcknowledged)) {
            Set<Integer> seen = new LinkedHashSet<>();
            for (ContraVoucherDto.Detail d : details) {
                if (d.DebitAmount <= 0d || !seen.add(d.AccountId)) continue;
                String title = writer.duplicateVoucherTitle(u.getOrganizationId(), u.getCompanyId(),
                        vh.VoucherDate, d.AccountId, d.DebitAmount, 0);
                if (title != null) {
                    throw new Confirm("Voucher against '" + title + "' with same Debit Amount already exists on this date. Do you want to continue?", "duplicate");
                }
            }
        }
        int id = writer.save(vh, details, new ArrayList<>());
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", id);
        res.put("message", (update ? "Record Update Successfully...[" : "Record Save Successfully...[") + vh.VoucherCode + "]");
        return res;
    }

    // ================================================================================ helpers

    private ContraVoucherDto.Head newHead(UserAccount u, int id, int doc, int code, String voucherDate) {
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        vh.Id = id;
        vh.DocumentTypeId = doc;
        vh.VoucherCode = code;
        vh.VoucherDate = dateTime(voucherDate);
        vh.OrganizationId = u.getOrganizationId();
        vh.CompanyId = u.getCompanyId();
        vh.BranchId = s.branchId();
        vh.FinancialYearId = s.yearId();
        vh.EntryUser = u.getId();
        vh.ModifyUser = u.getId();
        String now = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date());
        vh.EntryDate = now;           // BLL 0654 Save sets both on every save
        vh.ModifyDate = now;
        return vh;
    }

    /** DateTimePicker.Value carries the time of day the picker was created with. */
    private static String dateTime(String v) {
        if (blank(v)) return new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date());
        String t = v.trim().replace('T', ' ');
        if (t.length() == 10) t = t + " " + new SimpleDateFormat("HH:mm:ss").format(new Date());
        return t.length() > 19 ? t.substring(0, 19) : t;
    }

    private static String day(String v) {
        if (blank(v)) return null;
        String t = v.trim();
        return t.length() >= 10 ? t.substring(0, 10) : t;
    }

    /** C# double.ToString() for the amounts these messages carry: no trailing ".0". */
    static String clr(double d) {
        return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
    }

    /** Convert.ToInt32(double): rounds half to even. */
    private static int cInt(double d) { return (int) Math.rint(d); }

    private static boolean blank(String v) { return v == null || v.trim().isEmpty(); }
    private static int nz(Integer v) { return v == null ? 0 : v; }
    private static double nzd(Double v) { return v == null ? 0d : v; }
    private static Set<Integer> set(Integer... v) { return new HashSet<>(Arrays.asList(v)); }
}
