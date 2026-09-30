package com.mst.services.pbi;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.pbi.PbiVoucherSaveRequest;
import com.mst.repositories.pbi.PbiVoucherRepository;
import com.mst.services.desktopvoucher.DesktopVoucherSupport;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * BLL of screen 41 "Payment By Invoice Voucher New" -
 * Architecture.WinApp.Account_Definition.PaymentByInvoiceVoucherNew.cs and its popup LoadPendingInvoicesByPayment.cs.
 * Each method names the form method it reproduces. The save is BLL 0654 VoucherHead.Save (DocumentTypeId 1/2:
 * no BLL-side line generation) + DAL 0586 VoucherHead.SetData, run by {@link DesktopVoucherSupport#saveVoucher}
 * (Sp_VoucherHead_Insert|Update, Sp_VoucherDetail_Insert per line, USP_VoucherBalanceCheck, Sp_VoucherHead_H_Insert,
 * Sp_VoucherDetail_H_Insert, [DAW].[USp_DocumentApprovalDetail_Insert] - one transaction). The form sends no payment
 * schedule, cost centre or multi-currency lines; attachments (the Attachment dialog) are not ported.
 */
@Service
public class PbiVoucherService {

    public static final int SCREEN_ID = 41;
    /** base.Name of the form - the ScreenName of its DMS attachments. */
    public static final String SCREEN_NAME = "PaymentByInvoiceVoucherNew";

    private static final DateTimeFormatter ISO = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss");

    @Autowired private PbiVoucherRepository repo;
    @Autowired private HrmSupport hrm;
    @Autowired private DesktopVoucherSupport vouchers;

    // ================================================================== Load / Refresh

    /** AcfrmPaymentVoucher_Load / btnRefresh_Click: rights and every combo the Load fills. */
    public Map<String, Object> setup() {
        UserAccount u = hrm.user(SCREEN_ID);
        Map<String, Object> r = new LinkedHashMap<>();
        Map<String, Object> rights = hrm.rights(u, SCREEN_ID);
        r.put("rights", rights);
        r.putAll(combos(u));
        r.put("voucherCode", voucherCode(u, 1));                     // VoucherNofill: combvtype row 0 = CPV
        r.put("cheqBookEnabled", configTrue(u, "CheqBook Enabled"));
        r.put("startPeriod", startPeriod(u));
        r.put("today", LocalDate.now().toString());
        r.put("companyId", u.getCompanyId());
        return r;
    }

    /** btnRefresh_Click: CombCompanyFill .. BindTaxTypes (and VoucherNofill for the current type). */
    public Map<String, Object> refresh(int documentTypeId) {
        UserAccount u = hrm.user(SCREEN_ID);
        Map<String, Object> r = combos(u);
        r.put("voucherCode", voucherCode(u, documentTypeId == 2 ? 2 : 1));
        return r;
    }

    private Map<String, Object> combos(UserAccount u) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("companies", pick(repo.companies(u), "Id", "CompName"));
        r.put("branches", pick(repo.branches(u), "Id", "BranchName"));
        r.put("projects", pick(repo.projects(u), "Id", "ProjectName"));
        r.put("jobLots", pick(repo.jobLots(u), "Id", "JobLotDescription"));
        List<Map<String, Object>> coa = repo.coaAllocationSearch(u);
        r.put("againstAccounts", againstAccounts(coa));
        r.put("detailAccounts", detailAccounts(u, coa));
        r.put("taxTypes", pick(repo.taxTypes(u), "Id", "TaxName"));
        return r;
    }

    /** AccountsComboBind: CoaAllocationGetAllServiceBind rows with AccountTypeId not 2, 11, 15 -> CmbAgainstAc and CmbWithHoldingAc. */
    private static List<Map<String, Object>> againstAccounts(List<Map<String, Object>> coa) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> c : coa) {
            int t = toInt(c.get("AccountTypeId"));
            if (t != 2 && t != 11 && t != 15) out.add(map("Id", c.get("Id"), "AccountTitle", c.get("AccountTitle")));
        }
        return out;
    }

    /**
     * DetailAccountFill: configuration "ExpenseAccountAllowOnPaymentVoucher" true -> AccountTypeId not 2, 15;
     * false or not configured -> not 2, 11, 15. (bool.Parse of an unparsable key throws on the desktop - the
     * combo then stays empty; here it is treated as not configured.)
     */
    private List<Map<String, Object>> detailAccounts(UserAccount u, List<Map<String, Object>> coa) {
        boolean expenses = configTrue(u, "ExpenseAccountAllowOnPaymentVoucher");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> c : coa) {
            int t = toInt(c.get("AccountTypeId"));
            if (t != 2 && t != 15 && (expenses || t != 11)) out.add(map("Id", c.get("Id"), "AccountTitle", c.get("AccountTitle")));
        }
        return out;
    }

    private boolean configTrue(UserAccount u, String description) {
        String v = repo.config(u, description);
        return v != null && "true".equalsIgnoreCase(v.trim());
    }

    private String startPeriod(UserAccount u) {
        try {
            int id = hrm.financialYearId();
            Map<String, Object> row = null;
            List<Map<String, Object>> fy = repo.activeYears(u);
            for (Map<String, Object> r : fy) if (toInt(r.get("Id")) == id) { row = r; break; }
            if (row == null && !fy.isEmpty()) row = fy.get(0);
            LocalDateTime s = row == null ? null : toDate(row.get("Start_Period"));
            return s == null ? null : s.toLocalDate().toString();
        } catch (RuntimeException e) {
            return null;
        }
    }

    private int voucherCode(UserAccount u, int documentTypeId) {
        List<Map<String, Object>> rows = repo.generateVoucherCode(u, hrm.financialYearId(), documentTypeId);
        return rows.isEmpty() ? 0 : toInt(rows.get(0).get("VoucherCode"));
    }

    // ================================================================== header events

    /**
     * combvtype_Leave: AccountTitleFill (CPV -> DocumentTypeId 1, anything else 2) and, when Save is visible
     * and enabled, VoucherNofill. The page says whether its Save button is visible (new voucher) - the code is
     * only generated then, as on the form.
     */
    public Map<String, Object> voucherType(int documentTypeId, boolean newVoucher) {
        UserAccount u = hrm.user(SCREEN_ID);
        int dt = documentTypeId == 1 ? 1 : 2;
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("accounts", pick(repo.detailAccountsByDocumentType(u, dt), "Id", "AccountTitle"));
        if (newVoucher && hrm.can(u, SCREEN_ID, "Save")) r.put("voucherCode", voucherCode(u, dt));
        return r;
    }

    /**
     * combcreditac_Leave: AccountCurrentBalance (Balance -> txtbalance, Dr / Cr / Nill) and CheqNoFill
     * (BPV and configuration "CheqBook Enabled" only: OutstandingCheqNo(BankId = the account)). cheques is null
     * when CheqNoFill returns without binding (the combo keeps what it had).
     */
    public Map<String, Object> creditAccount(int accountId, String voucherDate, int documentTypeId) {
        UserAccount u = hrm.user(SCREEN_ID);
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("balance", balance(u, accountId, voucherDate));
        List<Map<String, Object>> cheques = null;
        if (documentTypeId == 2 && configTrue(u, "CheqBook Enabled")) {
            List<Map<String, Object>> rows = pick(repo.outstandingCheques(u, accountId), "Id", "CheqNo");
            if (!rows.isEmpty()) cheques = rows;           // "if (dt.Rows.Count > 0) BindDDL"
        }
        r.put("cheques", cheques);
        return r;
    }

    /** Balance of ReadByCurrentBalanceByDateAndAccountId, null when the procedure returns no row. */
    private Object balance(UserAccount u, int accountId, String voucherDate) {
        List<Map<String, Object>> rows = repo.currentBalance(u, hrm.financialYearId(), accountId, stamp(voucherDate, null));
        return rows.isEmpty() ? null : rows.get(0).get("Balance");
    }

    // ================================================================== detail events

    /**
     * combactitle_Leave: DetailAccountCurrentBalance, InvoiceNoFill, GetInvoiceBalanceAmount (for whatever
     * CmbInvoiceNo holds - the page passes it).
     */
    public Map<String, Object> detailAccount(int accountId, String voucherDate, int invoiceId) {
        UserAccount u = hrm.user(SCREEN_ID);
        int y = hrm.financialYearId();
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("balance", balance(u, accountId, voucherDate));
        List<Map<String, Object>> inv = new ArrayList<>();
        for (Map<String, Object> x : repo.invoiceNos(u, y, accountId)) {
            inv.add(map("Id", x.get("Id"), "VoucherCode", x.get("VoucherCode"), "DocumentTypeId", x.get("DocumentTypeId"),
                    "RefAccountId", x.get("RefAccountId"), "DocumentTypeCode", x.get("DocumentTypeCode"), "VoucherDate", x.get("VoucherDate"),
                    "DueDate", x.get("DueDate"), "ManualBillNo", x.get("ManualBillNo"), "BillAmount", x.get("BillAmount")));
        }
        r.put("invoices", inv);
        r.put("invoiceBalance", invoiceBalance(u, y, accountId, invoiceId));
        return r;
    }

    /** CmbInvoiceNo_ValueChanged / _Leave: GetInvoiceBalanceAmount. */
    public Map<String, Object> invoiceBalance(int accountId, int invoiceId) {
        UserAccount u = hrm.user(SCREEN_ID);
        return invoiceBalance(u, hrm.financialYearId(), accountId, invoiceId);
    }

    /** {InvoiceAmount, TotalPaidAmount, BalanceAmount} of the first row, or null (the form then shows "0" three times). */
    private Map<String, Object> invoiceBalance(UserAccount u, int y, int accountId, int invoiceId) {
        List<Map<String, Object>> rows = repo.invoiceBalance(u, y, accountId, invoiceId);
        if (rows.isEmpty()) return null;
        Map<String, Object> x = rows.get(0);
        return map("InvoiceAmount", toDouble(x.get("InvoiceAmount")), "TotalPaidAmount", toDouble(x.get("TotalPaidAmount")),
                "BalanceAmount", toDouble(x.get("BalanceAmount")));
    }

    /** checkBox1_CheckedChanged: TaxScheduleMain.ReadTaxSchedule(EffectedDate = voucher date, TaxNameId = CmbTaxType). */
    public Map<String, Object> taxSchedule(int taxTypeId, String voucherDate) {
        UserAccount u = hrm.user(SCREEN_ID);
        List<Map<String, Object>> rows = repo.taxSchedule(u, stamp(voucherDate, null), taxTypeId);
        if (rows.isEmpty()) return map("found", false);
        Map<String, Object> x = rows.get(0);
        return map("found", true, "TaxPercent", str(x.get("TaxPercent")), "TaxGLAccountId", x.get("TaxGLAccountId"),
                "AccountTitle", x.get("AccountTitle"));
    }

    // ================================================================== Load Invoices popup

    /** LoadPendingInvoicesByPayment Load: AccountTitleFill (GlAccountId / AccountTitle) and the Start_Period. */
    public Map<String, Object> loaderSetup() {
        UserAccount u = hrm.user(SCREEN_ID);
        return map("accounts", pick(repo.supplierGlAccounts(u), "GlAccountId", "AccountTitle"), "startPeriod", startPeriod(u));
    }

    /** PendingPurchaseInvoiceForLoad (ReqType 'Loader'): the popup grid. */
    public List<Map<String, Object>> loaderSearch(int accountId, String fromDate, String toDate) {
        UserAccount u = hrm.user(SCREEN_ID);
        return repo.invoicesForLoader(u, hrm.financialYearId(), accountId, stamp(fromDate, null), stamp(toDate, null), null, "Loader");
    }

    /**
     * LoadInGridDetail (ReqType 'LoadOnVoucher'): InvoicesIds is built as "," + id for every checked row (the
     * leading comma is sent as the desktop sends it) and AcId is the last checked row's RefAccountId. No dates.
     */
    public List<Map<String, Object>> loadOnVoucher(List<Integer> ids, int accountId) {
        UserAccount u = hrm.user(SCREEN_ID);
        StringBuilder s = new StringBuilder();
        if (ids != null) for (Integer id : ids) s.append(',').append(id == null ? 0 : id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> x : repo.invoicesForLoader(u, hrm.financialYearId(), accountId, null, null, s.toString(), "LoadOnVoucher")) {
            out.add(map("RefDocumentTypeId", x.get("RefDocumentTypeId"), "InvoiceId", x.get("InvoiceId"), "InvoiceNo", x.get("InvoiceNo"),
                    "AccountId", x.get("AccountId"), "JobLotId", x.get("JobLotId"), "Remarks", x.get("Remarks"), "WHT", toDouble(x.get("WHT")),
                    "InvoiceAmount", toDouble(x.get("InvoiceAmount")), "TotalPaidAmount", toDouble(x.get("TotalPaidAmount")),
                    "BalanceAmount", toDouble(x.get("BalanceAmount")), "Amount", toDouble(x.get("Amount"))));
        }
        return out;
    }

    // ================================================================== History tab

    /** HistoryFill(NoofRecords): 50 when the tab opens, 0 (all) from LoadAll. */
    public List<Map<String, Object>> history(int noOfRecords) {
        UserAccount u = hrm.user(SCREEN_ID);
        boolean all = hrm.can(u, SCREEN_ID, "CanView AllRecord");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> x : repo.history(u, hrm.financialYearId(), all, noOfRecords)) {
            out.add(map("Id", x.get("Id"), "DocumentTypeId", x.get("DocumentTypeId"), "VoucherCode", x.get("VoucherCode"),
                    "DocumentTypeCode", x.get("DocumentTypeCode"), "VoucherDate", x.get("VoucherDate"), "AccountTitle", x.get("AccountTitle"),
                    "Remarks", x.get("Remarks"), "VoucherAmount", toDouble(x.get("VoucherAmount")), "CheqNo", x.get("ChequeNo"),
                    "UserName", x.get("UserName"), "Attachment", x.get("NoOfAttachments")));
        }
        return out;
    }

    /** VoucherHead.GetByID for Edit (HistoryGridFill) and View (grdDetail) - only a CPV / BPV of this company. */
    public Map<String, Object> byId(int id) {
        UserAccount u = hrm.user(SCREEN_ID);
        Map<String, Object> head = ownHead(u, id);
        if (head == null) throw invalid("Not Record Found For Display");
        Map<String, Object> h = map("Id", head.get("Id"), "CompanyId", head.get("CompanyId"), "BranchId", head.get("BranchId"),
                "ProjectId", head.get("ProjectId"), "DocumentTypeId", head.get("DocumentTypeId"), "VoucherDate", day(head.get("VoucherDate")),
                "RefAccountId", head.get("RefAccountId"), "PayTitle", head.get("PayTitle"), "CheqId", head.get("CheqId"),
                "ChequeNo", head.get("ChequeNo"), "ChequeDate", day(head.get("ChequeDate")), "Remarks", head.get("Remarks"),
                "VoucherCode", head.get("VoucherCode"), "VoucherAmount", head.get("VoucherAmount"));
        List<Map<String, Object>> d = new ArrayList<>();
        for (Map<String, Object> x : repo.details(id)) {
            d.add(map("DocumentTypeIdRef", x.get("DocumentTypeIdRef"), "OrderNo", x.get("OrderNo"), "RefInvoiceNo", x.get("RefInvoiceNo"),
                    "AccountId", x.get("AccountId"), "AccountTitle", x.get("AccountTitle"), "JobLotId", x.get("JobLotId"),
                    "JobLotDescription", x.get("JobLotDescription"), "Comments", x.get("Comments"), "WhtHolding", toDouble(x.get("WhtHolding")),
                    "QtyIn", toDouble(x.get("QtyIn")), "QtyOut", toDouble(x.get("QtyOut")), "ItemAmount", toDouble(x.get("ItemAmount")),
                    "DebitAmount", toDouble(x.get("DebitAmount")), "CreditAmount", toDouble(x.get("CreditAmount")),
                    "IsTaxable", x.get("IsTaxable") == null ? null : str(x.get("IsTaxable")), "TaxTypeId", x.get("TaxTypeId"),
                    "TaxPrcnt", toDouble(x.get("TaxPrcnt")), "TaxesTotalAmount", toDouble(x.get("TaxesTotalAmount"))));
        }
        return map("head", h, "details", d);
    }

    /** DataGridHistory_LinkClicked: the voucher's DMS attachments (names only - files are not served by this page). */
    public List<Map<String, Object>> attachments(int id) {
        UserAccount u = hrm.user(SCREEN_ID);
        if (ownHead(u, id) == null) throw invalid("Not Record Found For Display");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> x : repo.attachments(id, SCREEN_NAME)) {
            out.add(map("Attachment", x.get("Attachment"), "UploadedFileCustomName", x.get("UploadedFileCustomName")));
        }
        return out;
    }

    /** GenerateReport / the history Print cell: Print right, then "Not Record Found For Display" when the slip has no rows. */
    public Map<String, Object> printCheck(int id) {
        UserAccount u = hrm.user(SCREEN_ID);
        hrm.require(u, SCREEN_ID, "Print");
        if (repo.slip(u, id).isEmpty()) throw invalid("Not Record Found For Display");
        return map("id", id);
    }

    private Map<String, Object> ownHead(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> head = first(repo.head(id));
        if (head == null) return null;
        if (toInt(head.get("OrganizationId")) != toInt(u.getOrganizationId()) || toInt(head.get("CompanyId")) != toInt(u.getCompanyId())) return null;
        int dt = toInt(head.get("DocumentTypeId"));
        return dt == 1 || dt == 2 ? head : null;
    }

    // ================================================================== Save / Update

    /**
     * Save_Click (Id 0) / Update_Click (Id > 0): the form's checks in the form's order and wording, then
     * VoucherHead.Save. The "Are you sure to ..." question is asked by the page before it posts.
     */
    public Map<String, Object> save(PbiVoucherSaveRequest q) {
        UserAccount u = hrm.user(SCREEN_ID);
        boolean update = q.Id != null && q.Id > 0;
        hrm.require(u, SCREEN_ID, update ? "Update" : "Save");
        List<PbiVoucherSaveRequest.Row> rows = q.rows == null ? new ArrayList<>() : q.rows;

        Map<String, Object> stored = null;
        if (update) {
            stored = ownHead(u, q.Id);
            if (stored == null) throw invalid("Record Not Found For Update  " + q.Id);
        }
        if (rows.isEmpty()) throw invalid("Grid Record not found");

        // FormValidation()
        int dt = toInt(q.DocumentTypeId);
        if (toInt(q.RefAccountId) == 0) throw invalid("Account Field is Required");
        if ((dt == 1 || dt == 2) && !has(repo.detailAccountsByDocumentType(u, dt), "Id", toInt(q.RefAccountId))) {
            throw invalid("Account Field is Required");             // not a row of combcreditac (LimitToList)
        }
        if (!has(repo.companies(u), "Id", toInt(q.CompanyId))) throw invalid("Company Name Field is Required");
        if (!has(repo.branches(u), "Id", toInt(q.BranchId))) throw invalid("Branch Name Field is Required");
        if (!has(repo.projects(u), "Id", toInt(q.ProjectId))) throw invalid("Project Name Field is Required");
        if (dt != 1 && dt != 2) throw invalid("Voucher Type Field is Required");

        List<Map<String, Object>> coa = repo.coaAllocationSearch(u);
        boolean wht = Boolean.TRUE.equals(q.IncludeWHT);
        String taxName = "";
        if (wht) {
            List<Map<String, Object>> against = againstAccounts(coa);
            if (!has(against, "Id", toInt(q.WhtAgainstAccountId))) throw invalid("Against Account Field is Required");
            if (!has(against, "Id", toInt(q.WhtAccountId))) throw invalid("WithHolding Account Field is Required");
            Map<String, Object> tax = find(repo.taxTypes(u), "Id", toInt(q.TaxTypeId));
            if (tax == null) throw invalid("TaxType Field is Required");
            taxName = str(tax.get("TaxName"));
        }
        Set<Integer> accounts = ids(coa, "Id");
        Set<Integer> lots = ids(repo.jobLots(u), "Id");
        for (PbiVoucherSaveRequest.Row r : rows) {
            double amount = nz(r.Amount);
            if (amount > 0.0) {
                if (amount > nz(r.InvoiceAmount)) throw invalid("DebitAmount not greater than InvoiceAmount!");
                if (toInt(r.AccountId) == 0 || !accounts.contains(toInt(r.AccountId))) throw invalid("Please Select Account Title First");
                if (toInt(r.JobLotId) != 0 && !lots.contains(toInt(r.JobLotId))) throw invalid("Job/Lot Field Required");
                continue;
            }
            throw invalid("Debit Amount Field Required");
        }

        // the tax figures the form shows (checkBox1_CheckedChanged / WHTTaxPropotion), recomputed from Tax %
        double pct = wht ? toDouble(q.TaxPercent) : 0d;
        double value = 0d;
        for (PbiVoucherSaveRequest.Row r : rows) value += nz(r.Amount);
        value = roundAway(value);                                  // txtValue.Text = Amount.ToString("0,0")
        double taxAmount = wht ? taxOf(value, pct) : 0d;

        ContraVoucherDto.Head h = new ContraVoucherDto.Head();
        if (update) h.Id = q.Id;
        h.DocumentTypeId = dt;
        h.DocumentTypeSrNo = dt;
        h.VoucherCode = update ? toInt(stored.get("VoucherCode")) : toInt(q.VoucherCode);
        h.VoucherDate = iso(stamp(q.VoucherDate, update ? stored.get("VoucherDate") : null));
        h.RefAccountId = toInt(q.RefAccountId);
        h.AgainstAccountId = toInt(q.WhtAccountId);                 // CmbWithHoldingAc.Value
        h.RefDocNoId = toInt(q.WhtAgainstAccountId);                // CmbAgainstAc.Value
        h.ChequeDate = iso(stamp(q.ChequeDate, update ? stored.get("ChequeDate") : null));
        h.Remarks = trim(q.Remarks);
        h.CheqId = toInt(q.CheqId);
        h.ChequeNo = q.ChequeNo == null ? "" : q.ChequeNo;
        h.PayTitle = trim(q.PayTitle);
        h.IncludeWHT = wht;
        h.BranchId = toInt(q.BranchId);
        h.ProjectId = toInt(q.ProjectId);
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.FinancialYearId = hrm.financialYearId();
        h.EntryUser = u.getId();
        h.ModifyUser = u.getId();
        String now = LocalDateTime.now().withNano(0).format(ISO);    // BLL 0654 Save: EntryDate / ModifyDate = DateTime.Now
        h.EntryDate = now;
        h.ModifyDate = now;

        List<ContraVoucherDto.Detail> details = new ArrayList<>();
        for (PbiVoucherSaveRequest.Row r : rows) {
            ContraVoucherDto.Detail d = new ContraVoucherDto.Detail();
            d.PaymentType = "Regular";
            d.DocumentTypeIdRef = toInt(r.RefDocumentTypeId);
            d.OrderNo = toInt(r.InvoiceId);
            d.RefInvoiceNo = r.InvoiceNo == null ? "" : r.InvoiceNo;
            d.AccountId = toInt(r.AccountId);
            d.AgainstAccountId = toInt(q.RefAccountId);
            d.JobLotId = toInt(r.JobLotId);
            d.Comments = r.Remarks == null ? "" : r.Remarks;
            d.DebitAmount = nz(r.Amount);
            d.QtyIn = nz(r.InvoiceAmount);
            d.QtyOut = nz(r.TotalPaidAmount);
            d.ItemAmount = nz(r.BalanceAmount);
            d.CreditAmount = 0d;
            d.WhtHolding = wht ? taxOf(nz(r.Amount), pct) : 0d;
            d.InvoiceNoRefId = toInt(q.CheqId);
            details.add(d);
        }
        if (wht) {
            ContraVoucherDto.Detail t = new ContraVoucherDto.Detail();
            t.AccountId = h.RefDocNoId;
            t.AgainstAccountId = h.AgainstAccountId;
            t.TaxTypeId = toInt(q.TaxTypeId);
            t.IsTaxable = "True";
            t.Comments = "Tax Name    " + taxName + "   Tax %   " + trim(q.TaxPercent);
            t.TaxPrcnt = pct;
            t.TaxesTotalAmount = taxAmount;
            t.DebitAmount = taxAmount;
            details.add(t);
        }

        int id = vouchers.saveVoucher(h, details, null);
        int code = h.VoucherCode;
        Map<String, Object> saved = first(repo.head(id));
        if (saved != null && toInt(saved.get("VoucherCode")) != 0) code = toInt(saved.get("VoucherCode"));
        Map<String, Object> r = saved(id, (update ? "Voucher Update Successfully...[" : "Voucher Save Successfully...[") + code + "]");
        r.put("voucherCode", code);
        return r;
    }

    // ================================================================== helpers

    /** Math.Round(Amount / (100 - Tax%) * 100 * Tax% / 100, 2) - MidpointRounding.ToEven. */
    static double taxOf(double amount, double pct) {
        double v = amount / (100.0 - pct) * 100.0 * pct / 100.0;
        if (Double.isNaN(v) || Double.isInfinite(v)) return 0d;
        return BigDecimal.valueOf(v).setScale(2, RoundingMode.HALF_EVEN).doubleValue();
    }

    /** double.ToString("0,0") read back by Conversion.ToDouble: a whole number, midpoints away from zero. */
    static double roundAway(double v) {
        return BigDecimal.valueOf(v).setScale(0, RoundingMode.HALF_UP).doubleValue();
    }

    private static Map<String, Object> first(List<Map<String, Object>> rows) { return rows == null || rows.isEmpty() ? null : rows.get(0); }

    private static double nz(Double d) { return d == null ? 0d : d; }

    /**
     * The DateTimePicker's Value: the chosen day with the current time of day (a new picker holds DateTime.Now);
     * a voucher loaded for Update keeps its stored value while its day is unchanged.
     */
    static Timestamp stamp(String day, Object stored) {
        LocalDate d;
        try { d = day == null || day.trim().isEmpty() ? LocalDate.now() : LocalDate.parse(day.trim().substring(0, 10)); }
        catch (RuntimeException e) { throw invalid("String was not recognized as a valid DateTime."); }
        LocalDateTime s = stored == null ? null : toDate(stored);
        if (s != null && s.toLocalDate().equals(d)) return Timestamp.valueOf(s);
        return Timestamp.valueOf(LocalDateTime.of(d, LocalTime.now().withNano(0)));
    }

    private static String iso(Timestamp t) { return t == null ? null : t.toLocalDateTime().withNano(0).format(ISO); }

    private static String day(Object v) {
        LocalDateTime d = toDate(v);
        return d == null ? null : d.toLocalDate().toString();
    }

    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String idKey, String textKey) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(map(idKey, r.get(idKey), textKey, r.get(textKey)));
        return out;
    }

    private static boolean has(List<Map<String, Object>> rows, String key, int id) { return id != 0 && find(rows, key, id) != null; }

    private static Map<String, Object> find(List<Map<String, Object>> rows, String key, int id) {
        if (id == 0) return null;
        for (Map<String, Object> r : rows) if (toInt(r.get(key)) == id) return r;
        return null;
    }

    private static Set<Integer> ids(List<Map<String, Object>> rows, String key) {
        Set<Integer> s = new HashSet<>();
        for (Map<String, Object> r : rows) s.add(toInt(r.get(key)));
        return s;
    }
}
