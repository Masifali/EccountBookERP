package com.mst.services.desktopvoucher;

import com.mst.models.dto.ContraVoucherDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.desktopvoucher.DesktopVoucherSupport.*;

/**
 * Screens 16 "Party Payment" (frmPartyPaymentVoucher, DocumentTypeId 35) and
 * 17 "Party Receipts" (frmPartyReceiptVoucher, DocumentTypeId 34).
 *
 * The two forms are the same form with the ledger direction mirrored:
 *
 *   PPV Insert():1422  per grid row  vd1 = header party (cmbAccount)  DEBIT,
 *                                    detail = row account (bank/cash) CREDIT
 *   PRV Insert()       per grid row  detail = row account (bank/cash) DEBIT,
 *                                    vd1 = header party (cmbAccount)  CREDIT
 *
 * both through VoucherHead.Save (BLL 0654 -> DAL 0586 SetData). The previous web port wrote PPV
 * with raw INSERTs into VoucherHead/VoucherDetail (IsApproved hard-coded 1, no balance check,
 * no history mirror, no approval row, no update path) and PRV through the JPA VoucherService.
 *
 * Desktop behaviours reproduced deliberately (quirks, not corrections):
 *  - VoucherAmount = float.Parse(total) - the grid total goes through a single-precision float.
 *  - Remarks = txtRemarks.Trim()=="" ? AllRemarks : txtRemarks.Trim()+" / "+AllRemarks, where
 *    AllRemarks is every row's comment followed by " , "; RemarksOtherLingo = txtRemarks.
 *  - Header BranchId is never set (0); each line's BranchesId = row branch when the Branch
 *    feature (17) is on, else the user's branch.
 *  - AgainstAccountId = cmbSaleTaxAccount, which sits on the "Sales Tax" tab the form removes at
 *    load (tabControl2.TabPages.Remove(tabPage4)), so it is always 0; IncludeWHT false.
 *  - The header "Subsidiary Account" combo (CmbSubsidiaryAccountDr/Cr) is never bound
 *    (BindSubsidiaryAccountCr is never called), so the party line never carries a subsidiary.
 *  - PPV cheque fields go on the lines only when a cheque was picked (ChequeId > 0), and then on
 *    BOTH lines; PRV writes the typed ChequeNo on both lines, without a date.
 *  - Delete: btnDelete_Click is empty on both forms, so there is no delete route.
 */
@Service
public class PartyVoucherService {

    public static final int PPV = 35;
    public static final int PRV = 34;

    @Autowired private DesktopVoucherSupport s;

    public static String screenName(int docType) {
        return docType == PPV ? "frmPartyPaymentVoucher" : "frmPartyReceiptVoucher";
    }

    private static void checkDocType(int docType) {
        if (docType != PPV && docType != PRV) throw new IllegalArgumentException("Unknown document type");
    }

    // ================================================================================= lookups

    /** Everything frmBillsPayables_Load (the forms' Load handler) binds, in one call. */
    public Map<String, Object> lookups(int docType) {
        checkDocType(docType);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("documentTypeId", docType);
        m.put("rights", s.rights(screenName(docType)));
        Map<String, Object> f = new LinkedHashMap<>();
        f.put("subsidiary", s.feature(4));      // SubsidiaryAccountAllownOnVouchers
        f.put("branch", s.feature(17));         // BranchFeature
        f.put("multiLanguage", s.feature(7));   // HasMultiLanguageFeature
        f.put("multiCurrency", s.feature(6));   // MultiCurrencyFeature()
        m.put("features", f);

        /* BindGLAccount -> AccountsBind(): one COAAllocationSearch list, split by AccountTypeId. */
        List<Map<String, Object>> header = new ArrayList<>();
        List<Map<String, Object>> detail = new ArrayList<>();
        for (Map<String, Object> r : s.coaAllocationSearch()) {
            int t = toInt(r.get("AccountTypeId"));
            Map<String, Object> a = new LinkedHashMap<>();
            a.put("Id", r.get("Id"));
            a.put("AccountTitle", r.get("AccountTitle"));
            a.put("AccountCode", r.get("AccountCode"));
            a.put("AccountTypeId", t);
            if (t == 3 || t == 6 || t == 8) header.add(a);
            if (t == 2 || t == 15 || t == 3 || t == 6 || t == 8) detail.add(a);
        }
        m.put("headerAccounts", header);
        m.put("detailAccounts", detail);
        m.put("jobLots", s.jobLots());
        m.put("currencies", s.currencies());
        m.put("languages", s.languages());
        m.put("subsidiaries", s.subsidiaries());
        m.put("branches", s.branchesAllocatedToUser());
        m.put("userBranchId", s.branchId());

        Map<String, Object> d = new LinkedHashMap<>();
        d.put("jobLotId", toInt(s.config("Job/Lot")));
        d.put("currencyId", toInt(s.config("Base Currency")));
        d.put("exchangeRate", toDouble(s.config("BaseCurrencyRate")));
        m.put("defaults", d);
        m.put("voucherCode", s.generateVoucherCode(docType));
        m.put("historyAccounts", historyAccounts(docType));
        m.put("decimals", s.decimals());
        return m;
    }

    /** ComboBindForHistory(): CompanyId = UserAccount.OrganizationId, AppId/UserId unset (0). */
    public List<Map<String, Object>> historyAccounts(int docType) {
        checkDocType(docType);
        return s.historyAccountHeaders(s.orgId(), 0, 0, String.valueOf(docType));
    }

    public int nextCode(int docType) {
        checkDocType(docType);
        return s.generateVoucherCode(docType);
    }

    // ==================================================================================== save

    @Transactional
    public Map<String, Object> save(int docType, Map<String, Object> body) {
        checkDocType(docType);
        String mode = str(body.get("mode"));
        int recId = toInt(body.get("recId"));
        Map<String, Boolean> rights = s.rights(screenName(docType));
        if ("update".equals(mode)) {
            /* btnUpdate_Click */
            if (recId == 0) throw new IllegalArgumentException("Record Not Update  " + recId);
            if (!Boolean.TRUE.equals(rights.get("update"))) throw new SecurityException("You do not have the Update right for this screen.");
        } else {
            /* btnsave_Click / btnSaveAs_Click: RecId = 0; Insert(). */
            recId = 0;
            if (!Boolean.TRUE.equals(rights.get("save"))) throw new SecurityException("You do not have the Save right for this screen.");
        }

        List<Map<String, Object>> rows = rows(body.get("rows"));
        /* Insert():1432 */
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record not found");
        /* FormValidation():1108 */
        int voucherCode = toInt(body.get("voucherCode"));
        if (voucherCode == 0) throw new IllegalArgumentException("DocNo  Field Required");
        int partyId = toInt(body.get("accountId"));
        if (partyId == 0) throw new IllegalArgumentException("Account  Field Required");

        boolean branchFeature = s.feature(17);
        /* FormDetailValidation() runs at row-add time on the desktop; a row that reaches the
           server without these was not built by the screen. */
        for (Map<String, Object> r : rows) {
            if (toInt(r.get("accountId")) == 0) throw new IllegalArgumentException("Account  Field Required");
            if (toDouble(r.get("amount")) == 0d) throw new IllegalArgumentException("Amount  Field Required");
            if (str(r.get("remarks")).trim().isEmpty()) throw new IllegalArgumentException("Remarks  Field Required");
            if (docType == PPV && branchFeature && toInt(r.get("branchId")) == 0)
                throw new IllegalArgumentException("Branch Name Field is Required");
        }

        int userBranch = s.branchId();
        int currencyId = toInt(body.get("currencyId"));
        double rate = toDouble(body.get("exchangeRate"));

        ContraVoucherDto.Head h = new ContraVoucherDto.Head();
        h.Id = recId;
        h.DocumentTypeId = docType;
        h.VoucherCode = voucherCode;
        h.VoucherDate = isoDay(body.get("voucherDate"));
        h.RefAccountId = partyId;
        h.AgainstAccountId = 0;                                   // cmbSaleTaxAccount (removed tab)
        String txtRemarks = body.get("remarks") == null ? "" : String.valueOf(body.get("remarks"));
        h.RemarksOtherLingo = txtRemarks;
        h.OrganizationId = s.orgId();
        h.CompanyId = s.companyId();
        h.FinancialYearId = s.yearId();
        h.EntryUser = s.userId();
        h.ModifyUser = s.userId();
        h.IncludeWHT = Boolean.FALSE;                             // chkCalculateSaleTax (removed tab)
        h.MultiCurrencyId = currencyId;
        h.ExchangeCurrencyRate = rate;
        String now = now();
        h.EntryDate = now;                                        // BLL 0654:23
        h.ModifyDate = now;

        List<ContraVoucherDto.Detail> details = new ArrayList<>();
        StringBuilder allRemarks = new StringBuilder();
        double total = 0d, totalFcy = 0d;
        int lineId = 1;
        for (Map<String, Object> r : rows) {
            int acc = toInt(r.get("accountId"));
            double amount = toDouble(r.get("amount"));
            /* btnAdd_Click / txtExchangeRate_TextChanged: FcyAmount = Amount / ExchangeRate. */
            double fcy = (rate > 0 && amount > 0) ? amount / rate : 0d;
            String comments = str(r.get("remarks"));
            int jobLot = toInt(r.get("jobLotId"));
            int rowBranch = branchFeature ? toInt(r.get("branchId")) : userBranch;
            int subTypeId = toInt(r.get("subsidiaryAccountTypeId"));
            int subId = toInt(r.get("subsidiaryAccountId"));
            total += amount;
            totalFcy += fcy;

            ContraVoucherDto.Detail party = new ContraVoucherDto.Detail();   // vd1
            party.AccountId = partyId;
            party.AgainstAccountId = acc;
            party.SubsidiaryTypeId = 0;                                        // header subsidiary never bound
            party.JobLotId = jobLot;
            party.Comments = comments;
            party.DMultiCurrencyId = currencyId;
            party.DExchangeCurrencyRate = rate;
            party.DCurrencyAmount = fcy;
            party.IsTaxable = "False";
            party.BranchesId = rowBranch;

            ContraVoucherDto.Detail line = new ContraVoucherDto.Detail();    // detail
            line.AccountId = acc;
            line.AgainstAccountId = partyId;
            line.SubsidiaryTypeId = subTypeId;
            if (subId > 0) {
                line.SupplierCustomerId = subTypeId == 1 ? subId : 0;
                line.EmployeeId = subTypeId == 2 ? subId : 0;
                line.SubsidiaryAccountId = subId;
            }
            line.JobLotId = jobLot;
            line.Comments = comments;
            line.DMultiCurrencyId = currencyId;
            line.DExchangeCurrencyRate = rate;
            line.DCurrencyAmount = fcy;
            line.IsTaxable = "False";
            line.BranchesId = rowBranch;

            if (docType == PPV) {
                int chequeId = toInt(r.get("chequeId"));
                if (chequeId > 0) {
                    String chqNo = str(r.get("chequeNo"));
                    String chqDate = isoDay(r.get("chequeDate"));
                    party.InvoiceNoRefId = chequeId; party.CheqNoDetail = chqNo; party.DCheqDate = chqDate;
                    line.InvoiceNoRefId = chequeId;  line.CheqNoDetail = chqNo;  line.DCheqDate = chqDate;
                }
                party.DebitAmount = amount;                                  // party Dr
                line.CreditAmount = amount;                                  // bank/cash Cr
                party.LineId = lineId++;
                details.add(party);
                line.LineId = lineId++;
                details.add(line);
            } else {
                String chqNo = str(r.get("chequeNo"));
                line.CheqNoDetail = chqNo;
                party.CheqNoDetail = chqNo;
                line.DebitAmount = amount;                                   // bank/cash Dr
                party.CreditAmount = amount;                                 // party Cr
                line.LineId = lineId++;
                details.add(line);
                party.LineId = lineId++;
                details.add(party);
            }
            allRemarks.append(comments).append(" , ");
        }
        /* voucher.VoucherAmount = float.Parse(total.ToString()) */
        h.VoucherAmount = (double) (float) total;
        /* txtFcyAmount.Text = TotalFcyAmount.ToString("#,##0.###") */
        h.FcAmount = Math.round(totalFcy * 1000d) / 1000d;
        h.Remarks = txtRemarks.trim().isEmpty() ? allRemarks.toString()
                                                : txtRemarks.trim() + " / " + allRemarks;

        int id = s.saveVoucher(h, details, new ArrayList<>());

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", id);
        res.put("voucherCode", voucherCode);
        res.put("message", (recId > 0 ? "Record Update Successfully" : "Record Save Successfully") + voucherCode);
        res.put("nextVoucherCode", s.generateVoucherCode(docType));
        return res;
    }

    // ==================================================================================== load

    /** ReadById(ID) - the header and the grid rows exactly as the form rebuilds them. */
    public Map<String, Object> load(int docType, int id) {
        checkDocType(docType);
        Map<String, Object> v = s.readById(id, docType);
        if (v == null) return null;
        @SuppressWarnings("unchecked") Map<String, Object> head = (Map<String, Object>) v.get("head");
        @SuppressWarnings("unchecked") List<Map<String, Object>> det = (List<Map<String, Object>>) v.get("details");
        Map<String, Object> out = new LinkedHashMap<>();
        int ref = toInt(head.get("RefAccountId"));
        out.put("id", id);
        out.put("voucherCode", head.get("VoucherCode"));
        out.put("voucherDate", isoDay(head.get("VoucherDate")));
        out.put("accountId", ref);
        out.put("remarks", head.get("RemarksOtherLingo"));
        out.put("currencyId", toInt(head.get("MultiCurrencyId")));
        out.put("exchangeRate", toDouble(head.get("ExchangeCurrencyRate")));
        out.put("fcAmount", toDouble(head.get("FcAmount")));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : det) {
            if (!"False".equals(str(d.get("IsTaxable")))) continue;
            if (toInt(d.get("AccountId")) == ref) continue;
            int t = toInt(d.get("SubsidiaryTypeId"));
            /* ReadById puts SupplierCustomerId / EmployeeId / SubsidiaryAccountId into the
               grid's SubsidiaryAccountTypeId column - reproduced as the form does it. */
            int typeCol = t == 1 ? toInt(d.get("SupplierCustomerId"))
                        : t == 2 ? toInt(d.get("EmployeeId"))
                        : (t == 3 || t == 4) ? toInt(d.get("SubsidiaryAccountId")) : 0;
            Map<String, Object> r = new LinkedHashMap<>();
            r.put("accountCode", d.get("AccountCode"));
            r.put("accountId", toInt(d.get("AccountId")));
            r.put("accountTitle", d.get("AccountTitle"));
            r.put("subsidiaryAccountId", toInt(d.get("SubsidiaryAccountId")));
            r.put("subsidiaryAccount", d.get("SubsidiaryAccountTitle"));
            r.put("subsidiaryAccountTypeId", typeCol);
            r.put("jobLotId", toInt(d.get("JobLotId")));
            r.put("jobLot", d.get("JobLotDescription"));
            if (docType == PPV) {
                r.put("chequeDate", isoDay(d.get("DCheqDate")));
                r.put("chequeId", toInt(d.get("InvoiceNoRefId")));
                r.put("amount", toDouble(d.get("CreditAmount")));
            } else {
                r.put("amount", toDouble(d.get("DebitAmount")));
            }
            r.put("chequeNo", d.get("CheqNoDetail"));
            r.put("fcyAmount", toDouble(d.get("DCurrencyAmount")));
            r.put("remarks", d.get("Comments"));
            r.put("branchId", toInt(d.get("BranchesId")));
            r.put("branchName", d.get("BranchName"));
            rows.add(r);
        }
        out.put("rows", rows);
        return out;
    }

    // ================================================================================= history

    /** HistoryGridFill() - VoucherFormHistory with Ids = "35" / "34". */
    public Map<String, Object> history(int docType, Map<String, String> q) {
        checkDocType(docType);
        boolean all = Boolean.TRUE.equals(s.rights(screenName(docType)).get("canViewAllRecord"));
        String status = q.getOrDefault("status", "Not Approved");
        List<Map<String, Object>> raw = s.voucherFormHistory(String.valueOf(docType), all,
                q.get("dateType"), q.get("fromDate"), q.get("toDate"),
                toInt(q.get("fromDocNo")), toInt(q.get("toDocNo")), toInt(q.get("accountId")),
                status, "Approved".equals(status), 0);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", r.get("Id"));
            m.put("DocumentTypeId", r.get("DocumentTypeId"));
            m.put("VoucherDate", isoDay(r.get("VoucherDate")));
            m.put("VoucherCode", r.get("VoucherCode"));
            m.put("DocumentType", r.get("DocumentTypeCode"));
            m.put("AccountTitle", r.get("AccountTitle"));
            m.put("VoucherAmount", r.get("VoucherAmount"));
            m.put("Remarks", r.get("Remarks"));
            m.put("EntryUser", r.get("UserName"));
            m.put("Attachment", toInt(r.get("NoOfAttachments")));
            rows.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        if (!raw.isEmpty()) {
            out.put("totalVouchers", toInt(raw.get(0).get("TotalVouchers")));
            out.put("approvedVouchers", toInt(raw.get(0).get("TotalApprovedVoucher")));
            out.put("unApprovedVouchers", toInt(raw.get(0).get("TotalUnApprovedVoucher")));
        }
        return out;
    }

    /** DataGridHistory_SelectionChanged - the lines of the selected voucher with a Dr or Cr. */
    public List<Map<String, Object>> historyDetail(int docType, int id) {
        checkDocType(docType);
        Map<String, Object> v = s.readById(id, docType);
        List<Map<String, Object>> out = new ArrayList<>();
        if (v == null) return out;
        @SuppressWarnings("unchecked") List<Map<String, Object>> det = (List<Map<String, Object>>) v.get("details");
        for (Map<String, Object> d : det) {
            double dr = toDouble(d.get("DebitAmount")), cr = toDouble(d.get("CreditAmount"));
            if (dr > 0 || cr > 0) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("AccountCode", d.get("AccountCode"));
                m.put("AccountTitle", d.get("AccountTitle"));
                if (docType == PPV) m.put("ChequeDate", isoDay(d.get("DCheqDate")));
                m.put("ChequeNo", d.get("CheqNoDetail"));
                m.put("DebitAmount", dr);
                m.put("CreditAmount", cr);
                m.put("Remarks", d.get("Comments"));
                out.add(m);
            }
        }
        return out;
    }

    @SuppressWarnings("unchecked")
    static List<Map<String, Object>> rows(Object o) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (o instanceof List) for (Object x : (List<Object>) o) if (x instanceof Map) out.add((Map<String, Object>) x);
        return out;
    }
}
