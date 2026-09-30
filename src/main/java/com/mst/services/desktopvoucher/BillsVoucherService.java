package com.mst.services.desktopvoucher;

import com.mst.models.dto.ContraVoucherDto;
import com.mst.repositories.support.DesktopProc;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.TreeMap;

import static com.mst.services.desktopvoucher.DesktopVoucherSupport.*;

/**
 * Screens 38 "Bills Payable" (frmBillsPayables, DocumentTypeId 6) and 39 "Bills Receivables"
 * (frmBillsReceivables, DocumentTypeId 7). There was no web page for either (the only "bills"
 * route was the Bills Payables REPORT).
 *
 * Both save through VoucherHead.Save (BLL 0654 -> DAL 0586 SetData) with a PaymentDueSchedule
 * list, so {@link DesktopVoucherSupport#saveVoucher} writes them. The two forms mirror each other:
 *
 *   per grid row   Payables: row account DEBIT  / header party (Cr) CREDIT
 *                  Receivables: row account CREDIT / header party (Dr) DEBIT
 *   sales tax      Payables: tax account Dr / party Cr (party line carries the header subsidiary)
 *                  Receivables: party Dr (header subsidiary) / tax account Cr
 *   advance tax    Payables: adv-tax account Dr / party Cr (header subsidiary)
 *                  Receivables: party Dr / adv-tax account Cr, the party line taking the DETAIL
 *                  subsidiary box's value (CmbSubsidiaryAccountCr is the detail box on this form)
 *   other charges  both: Dr line = the signed amount, Cr line = the signed amount, accounts swap
 *                  on the sign; the subsidiary sits on the party line (Payables: header box;
 *                  Receivables: the detail box, as above)
 *
 * Desktop behaviours reproduced deliberately: the tax / advance-tax / other-charges lines have no
 * LineId (0); IsTaxable is "True" on the two sales-tax lines and unset (NULL - not sent) on the
 * advance-tax and other-charges lines; a negative Other Charges amount is written as negative
 * Debit/Credit amounts; header BranchId = the user's branch; Remarks = txtRemarks as typed.
 * Delete: btnDelete_Click is empty and the button is invisible - no delete route.
 */
@Service
public class BillsVoucherService {

    public static final int PAYABLES = 6;
    public static final int RECEIVABLES = 7;

    @Autowired private DesktopVoucherSupport s;
    @Autowired private JdbcTemplate jdbc;

    public static String screenName(int docType) {
        return docType == PAYABLES ? "frmBillsPayables" : "frmBillsReceivables";
    }

    private static void checkDocType(int d) {
        if (d != PAYABLES && d != RECEIVABLES) throw new IllegalArgumentException("Unknown document type");
    }

    // ================================================================================= lookups

    /** InitializeComponentMethod(): everything the form loads, in one call. */
    public Map<String, Object> lookups(int docType) {
        checkDocType(docType);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("documentTypeId", docType);
        m.put("rights", s.rights(screenName(docType)));
        boolean sub = s.feature(4);
        m.put("subsidiaryFeature", sub);
        m.put("voucherCode", s.generateVoucherCode(docType));
        m.put("locationTypes", DesktopProc.rows(jdbc, "usp_getLocationType", new LinkedHashMap<>()));
        /* CommonServices.ProjectServiceBind -> Projects.GetAlldt */
        m.put("costCenters", DesktopProc.rows(jdbc, "Sp_Projects_GetAllMethod",
                DesktopProc.params("OrganizationId", s.orgId(), "CompanyId", s.companyId(), "MethodType", "GetAll")));
        /* AllAccountsWithTypeIdDbCall -> CommonServices.Accounts_GetAccountTitleByAccountTypeIds() */
        List<Map<String, Object>> all = new ArrayList<>();
        for (Map<String, Object> r : s.accountsGetAccountTitleByAccountTypeIds(null, 0)) {
            Map<String, Object> a = new LinkedHashMap<>();
            a.put("Id", r.get("Id"));
            a.put("AccountTitle", r.get("AccountTitle"));
            a.put("AccountCode", r.get("AccountCode"));
            a.put("AccountTypeId", toInt(r.get("AccountTypeId")));
            a.put("NoteTitle", r.get("NoteTitle"));
            a.put("AccountType", r.get("AccountType"));
            all.add(a);
        }
        m.put("headerAccounts", byTypes(all, "3"));                         // Credit / Debit Account
        m.put("taxAccounts", byTypes(all, "3,6,8,19"));                     // SalesTaxAccountBind
        m.put("advanceTaxAccounts", byTypes(all, "6,16,17"));               // AdvanceTaxAccountBind
        m.put("referenceAccounts", byTypes(all, "3, 5, 6, 7, 8"));          // ReferenceAccountBind
        if (docType == RECEIVABLES) m.put("otherChargesAccounts", byTypes(all, "10,11,12,13,14,20,21"));
        /* AccountTypeBind(): distinct (AccountTypeId, AccountType) in the form's list, ordered */
        int[] ids = docType == PAYABLES ? new int[] { 3, 5, 6, 8, 16, 17, 18, 19 }
                                        : new int[] { 3, 5, 6, 8, 11, 13, 14, 16, 17, 18, 19, 20, 21 };
        Set<Integer> want = new HashSet<>();
        for (int i : ids) want.add(i);
        Map<Integer, String> types = new TreeMap<>();
        for (Map<String, Object> a : all) {
            int t = toInt(a.get("AccountTypeId"));
            if (want.contains(t) && !types.containsKey(t)) types.put(t, str(a.get("AccountType")));
        }
        List<Map<String, Object>> tl = new ArrayList<>();
        for (Map.Entry<Integer, String> e : types.entrySet()) {
            Map<String, Object> x = new LinkedHashMap<>(); x.put("Id", e.getKey()); x.put("AccountType", e.getValue()); tl.add(x);
        }
        m.put("accountTypes", tl);
        m.put("allAccounts", all);
        m.put("currencies", s.currencies());
        m.put("customGroups", DesktopProc.rows(jdbc, "[dbo].[usp_getCustomAccountGroupsByDocumentType]",
                DesktopProc.params("DocumentTypeId", docType)));
        m.put("jobLots", s.jobLots());
        m.put("subsidiaries", sub ? s.subsidiaries() : new ArrayList<>());
        /* TaxTypesDbCall -> TaxesTypes.GetForComboBind(Type = 2) */
        m.put("taxTypes", DesktopProc.rows(jdbc, "Sp_TaxesTypes_GetAllMethod",
                DesktopProc.params("OrganizationId", s.orgId(), "CompanyId", s.companyId(), "Type", 2, "Activity", "ReadByCombo")));
        m.put("historyAccounts", historyAccounts(docType));
        m.put("decimals", s.decimals());
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("jobLotId", toInt(s.config("Job/Lot")));
        d.put("currencyId", toInt(s.config("Base Currency")));
        d.put("exchangeRate", toDouble(s.config("BaseCurrencyRate")));
        int dd = toInt(s.config("DefaultDaysToLessFromHistoryFromDate"));
        d.put("historyFromDaysBack", dd > 0 ? dd : 3);
        m.put("defaults", d);
        return m;
    }

    private static List<Map<String, Object>> byTypes(List<Map<String, Object>> all, String csv) {
        Set<Integer> set = new HashSet<>();
        for (String p : csv.split(",")) { try { set.add(Integer.parseInt(p.trim())); } catch (NumberFormatException ignored) { } }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> a : all) if (set.contains(toInt(a.get("AccountTypeId")))) out.add(a);
        return out;
    }

    /** AccountTitleHistoryDbCall(): CompanyId = UserAccount.OrganizationId; AppId, UserId set. */
    public List<Map<String, Object>> historyAccounts(int docType) {
        checkDocType(docType);
        return s.historyAccountHeaders(s.orgId(), s.appId(), s.userId(), String.valueOf(docType));
    }

    /** CmbCustomGroup_Leave -> GetCustomAccountsByGroup(CompanyId, CustomGroupId). */
    public List<Map<String, Object>> customAccounts(int customGroupId) {
        return DesktopProc.rows(jdbc, "[dbo].[usp_getCustomAccountsByGroupId]",
                DesktopProc.params("CompanyId", s.companyId(), "CustomGroupId", customGroupId));
    }

    /** cmbCurrency_Leave -> VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher. */
    public List<Map<String, Object>> lastExchangeRate(int docType, int currencyId) {
        checkDocType(docType);
        return DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods",
                DesktopProc.params("OrganizationId", s.orgId(), "CompanyId", s.companyId(),
                        "DocumentTypeIds", String.valueOf(docType), "DMultiCurrencyIds", String.valueOf(currencyId),
                        "Activity", "GetMultiCurrencyAndLastRate"));
    }

    /** salesTex() -> TaxScheduleMain.ReadTaxSchedule. */
    public List<Map<String, Object>> taxSchedule(int taxTypeId, String date) {
        return DesktopProc.rows(jdbc, "Sp_TaxSchedule_GetAllMehtod",
                DesktopProc.params("OrganizationId", s.orgId(), "CompanyId", s.companyId(),
                        "EffectedDate", date, "TaxNameId", taxTypeId, "Activity", "GetTaxPercentInTaxSchedule"));
    }

    public int nextCode(int docType) { checkDocType(docType); return s.generateVoucherCode(docType); }

    // ==================================================================================== save

    @Transactional
    public Map<String, Object> save(int docType, Map<String, Object> b) {
        checkDocType(docType);
        boolean pay = docType == PAYABLES;
        String mode = str(b.get("mode"));
        int recId = toInt(b.get("recId"));
        Map<String, Boolean> rights = s.rights(screenName(docType));
        if ("update".equals(mode)) {
            if (recId == 0) throw new IllegalArgumentException("Record Not Update  " + recId);
            if (!Boolean.TRUE.equals(rights.get("update"))) throw new SecurityException("You do not have the Update right for this screen.");
        } else {
            recId = 0;
            if (!Boolean.TRUE.equals(rights.get("save"))) throw new SecurityException("You do not have the Save right for this screen.");
        }
        List<Map<String, Object>> rows = PartyVoucherService.rows(b.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record not found");

        boolean subFeature = s.feature(4);
        int partyId = toInt(b.get("accountId"));
        int hdrSubId = toInt(b.get("headerSubsidiaryId"));
        int hdrSubType = 0;
        List<Map<String, Object>> subs = subFeature ? s.subsidiaries() : new ArrayList<>();
        int subsForParty = 0;
        for (Map<String, Object> x : subs) {
            if (toInt(x.get("AccountId")) == partyId) {
                subsForParty++;
                if (toInt(x.get("Id")) == hdrSubId) hdrSubType = toInt(x.get("SubsidiaryTypeId"));
            }
        }
        if (hdrSubId > 0 && hdrSubType == 0) hdrSubId = 0;       // not in the party's list: the combo would be empty
        /* FormValidation():1350 */
        if (toInt(b.get("locationTypeId")) == 0) throw new IllegalArgumentException("Location Type Field is Required");
        if (toInt(b.get("projectId")) == 0) throw new IllegalArgumentException("Cost Center  Field Required");
        int voucherCode = toInt(b.get("voucherCode"));
        if (voucherCode == 0) throw new IllegalArgumentException("DocNo  Field Required");
        if (partyId == 0) throw new IllegalArgumentException("Account  Field Required");
        if (subFeature && subsForParty > 0 && hdrSubId == 0) throw new IllegalArgumentException("Subsidiary Account Title Field Required");
        int currencyId = toInt(b.get("currencyId"));
        if (currencyId == 0) throw new IllegalArgumentException("Fcy Code Field is Required");
        double rate = toDouble(b.get("exchangeRate"));
        if (rate == 0d) throw new IllegalArgumentException("Exchange Rate Field is Required");
        double fcTotal = 0d;
        for (Map<String, Object> r : rows) fcTotal += toDouble(r.get("fcyAmount"));
        double fcAmount = Math.round(fcTotal * 1000d) / 1000d;    // txtFcyAmount "#,##0.###"
        if (fcAmount == 0d) throw new IllegalArgumentException("Fcy Amount Field is Required");

        int taxAccountId = toInt(b.get("taxAccountId"));
        int taxTypeId = toInt(b.get("taxTypeId"));
        int advAcc = toInt(b.get("advanceTaxAccountId"));
        double advAmt = toDouble(b.get("advanceTaxAmount"));
        int ocAcc = toInt(b.get("otherChargesAccountId"));
        double ocAmt = toDouble(b.get("otherChargesAmount"));
        int locationTypeId = toInt(b.get("locationTypeId"));

        ContraVoucherDto.Head h = new ContraVoucherDto.Head();
        h.Id = recId;
        h.ProjectId = toInt(b.get("projectId"));
        h.DocumentTypeId = docType;
        h.VoucherCode = voucherCode;
        h.VoucherDate = isoDay(b.get("voucherDate"));
        h.RefAccountId = partyId;
        h.AgainstAccountId = taxAccountId;
        h.ManualBillNo = str(b.get("invoiceNo"));
        h.DueDate = isoDay(b.get("invoiceDate"));
        h.Remarks = str(b.get("remarks"));
        h.OrganizationId = s.orgId();
        h.CompanyId = s.companyId();
        h.BranchId = s.branchId();
        h.FinancialYearId = s.yearId();
        h.EntryUser = s.userId();
        h.ModifyUser = s.userId();
        h.IncludeWHT = taxTypeId > 0 && taxAccountId > 0;
        h.AdvanceTaxAccountId = advAcc;
        h.AdvanceTaxAmount = advAmt;
        h.OtherChargesAccountId = ocAcc;
        h.OtherChargesAmount = ocAmt;
        h.MultiCurrencyId = currencyId;
        h.ExchangeCurrencyRate = rate;
        h.FcAmount = fcAmount;
        h.CustomAccounts = Boolean.TRUE.equals(b.get("customAccounts"));
        String now = now();
        h.EntryDate = now;
        h.ModifyDate = now;

        List<ContraVoucherDto.Detail> det = new ArrayList<>();
        double voucherAmount = 0d, gridTotal = 0d;
        int lineId = 1;
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            int acc = toInt(r.get("accountId"));
            int rowSub = toInt(r.get("subsidiaryAccountId"));
            if (partyId == acc && !subFeature)
                throw new IllegalArgumentException("Dr Account Title Cannot Be Equal To Cr Account Title...");
            if (hdrSubId > 0 && hdrSubId == rowSub && subFeature)
                throw new IllegalArgumentException("Dr Subsidiary Account Cannot Be Equal To Cr Subsidiary Account...");
            double amount = toDouble(r.get("amount"));
            double fcy = toDouble(r.get("fcyAmount"));
            gridTotal += amount;

            ContraVoucherDto.Detail row = new ContraVoucherDto.Detail();
            row.LineId = lineId;
            row.AccountId = acc;
            row.AgainstAccountId = partyId;
            row.SubsidiaryTypeId = toInt(r.get("subsidiaryAccountTypeId"));
            if (rowSub > 0) {
                row.SupplierCustomerId = row.SubsidiaryTypeId == 1 ? rowSub : 0;
                row.EmployeeId = row.SubsidiaryTypeId == 2 ? rowSub : 0;
                row.SubsidiaryAccountId = rowSub;
            }
            fillLine(row, r, amount, fcy, locationTypeId, currencyId, rate);
            if (pay) row.DebitAmount = amount; else row.CreditAmount = amount;
            if (fcy == 0d) throw new IllegalArgumentException("Fcy Amount Not Found In Detail Grid at Row#" + (i + 1));
            det.add(row);
            lineId++;
            voucherAmount += amount;

            ContraVoucherDto.Detail party = new ContraVoucherDto.Detail();
            party.LineId = lineId;
            party.AccountId = partyId;
            party.AgainstAccountId = acc;
            party.SubsidiaryTypeId = hdrSubId > 0 ? hdrSubType : 0;
            if (hdrSubId > 0) applySub(party, hdrSubId, hdrSubType);
            fillLine(party, r, amount, fcy, locationTypeId, currencyId, rate);
            if (pay) party.CreditAmount = amount; else party.DebitAmount = amount;
            det.add(party);
            lineId++;
        }

        double taxAmt = toDouble(b.get("taxAmount"));
        double taxPct = toDouble(b.get("taxPercent"));
        String pctText = str(b.get("taxPercentText")).isEmpty() ? str(b.get("taxPercent")) : str(b.get("taxPercentText"));
        if (taxAmt > 0 && taxTypeId > 0) {
            ContraVoucherDto.Detail t1 = new ContraVoucherDto.Detail();
            ContraVoucherDto.Detail t2 = new ContraVoucherDto.Detail();
            if (pay) {
                t1.AccountId = taxAccountId; t1.AgainstAccountId = partyId; t1.Comments = "Tax % " + pctText;
                t1.DebitAmount = taxAmt; t1.TaxesTotalAmount = taxAmt; t1.CreditAmount = 0d;
                t2.AccountId = partyId; t2.Comments = "Tax%" + pctText; t2.AgainstAccountId = taxAccountId;
                t2.CreditAmount = taxAmt; t2.SubsidiaryTypeId = hdrSubId > 0 ? hdrSubType : 0;
                if (hdrSubId > 0) applySub(t2, hdrSubId, hdrSubType);
                t2.TaxesTotalAmount = taxAmt; t2.DebitAmount = 0d;
            } else {
                t1.AccountId = partyId; t1.AgainstAccountId = taxAccountId; t1.Comments = "Tax % " + pctText;
                t1.DebitAmount = taxAmt; t1.TaxesTotalAmount = taxAmt; t1.CreditAmount = 0d;
                t1.SubsidiaryTypeId = hdrSubId > 0 ? hdrSubType : 0;
                if (hdrSubId > 0) applySub(t1, hdrSubId, hdrSubType);
                t2.AccountId = taxAccountId; t2.AgainstAccountId = partyId; t2.Comments = "Tax%" + pctText;
                t2.CreditAmount = taxAmt; t2.TaxesTotalAmount = taxAmt;
            }
            for (ContraVoucherDto.Detail t : Arrays.asList(t1, t2)) {
                t.IsTaxable = "True"; t.TaxPrcnt = taxPct; t.TaxTypeId = taxTypeId; t.LineId = 0;
                det.add(t);
            }
        }

        /* The subsidiary on the party side of the advance-tax / other-charges lines:
           Payables reads its header box (CmbSubsidiaryAccountCr); Receivables reads
           CmbSubsidiaryAccountCr too, which on that form is the DETAIL box. */
        int xSubId = pay ? hdrSubId : toInt(b.get("detailSubsidiaryId"));
        int xSubType = pay ? hdrSubType : toInt(b.get("detailSubsidiaryTypeId"));

        if (advAmt > 0 && advAcc > 0) {
            ContraVoucherDto.Detail a1 = new ContraVoucherDto.Detail();
            ContraVoucherDto.Detail a2 = new ContraVoucherDto.Detail();
            if (pay) {
                a1.AccountId = advAcc; a1.AgainstAccountId = partyId; a1.Comments = "Advance Tax"; a1.DebitAmount = advAmt; a1.CreditAmount = 0d;
                a2.AccountId = partyId; a2.AgainstAccountId = advAcc; a2.Comments = "Advance Tax"; a2.CreditAmount = advAmt;
                a2.SubsidiaryTypeId = xSubId > 0 ? xSubType : 0; if (xSubId > 0) applySub(a2, xSubId, xSubType); a2.DebitAmount = 0d;
            } else {
                a1.AccountId = partyId; a1.AgainstAccountId = advAcc; a1.Comments = "Advance Tax"; a1.DebitAmount = advAmt;
                a1.SubsidiaryTypeId = xSubId > 0 ? xSubType : 0; if (xSubId > 0) applySub(a1, xSubId, xSubType);
                a2.AccountId = advAcc; a2.AgainstAccountId = partyId; a2.Comments = "Advance Tax"; a2.CreditAmount = advAmt;
            }
            det.add(a1); det.add(a2);
        }

        if (ocAmt != 0 && ocAcc > 0) {
            boolean pos = ocAmt > 0;
            ContraVoucherDto.Detail o1 = new ContraVoucherDto.Detail();
            ContraVoucherDto.Detail o2 = new ContraVoucherDto.Detail();
            if (pay) {
                o1.AccountId = pos ? ocAcc : partyId; o1.AgainstAccountId = pos ? partyId : ocAcc;
                o1.Comments = "Other Charges"; o1.DebitAmount = ocAmt;
                if (!pos) { o1.SubsidiaryTypeId = xSubId > 0 ? xSubType : 0; if (xSubId > 0) applySub(o1, xSubId, xSubType); }
                o2.AccountId = pos ? partyId : ocAcc; o2.AgainstAccountId = pos ? ocAcc : partyId;
                o2.Comments = "Other Charges"; o2.CreditAmount = ocAmt;
                if (pos) { o2.SubsidiaryTypeId = xSubId > 0 ? xSubType : 0; if (xSubId > 0) applySub(o2, xSubId, xSubType); }
            } else {
                o1.AccountId = pos ? partyId : ocAcc; o1.AgainstAccountId = pos ? ocAcc : partyId;
                o1.Comments = "Other Charges"; o1.DebitAmount = ocAmt;
                if (pos) { o1.SubsidiaryTypeId = xSubId > 0 ? xSubType : 0; if (xSubId > 0) applySub(o1, xSubId, xSubType); }
                o2.AccountId = pos ? ocAcc : partyId; o2.AgainstAccountId = pos ? partyId : ocAcc;
                o2.Comments = "Other Charges"; o2.CreditAmount = ocAmt;
                if (!pos) { o2.SubsidiaryTypeId = xSubId > 0 ? xSubType : 0; if (xSubId > 0) applySub(o2, xSubId, xSubType); }
            }
            det.add(o1); det.add(o2);
        }

        /* Amount Distribution (grdSchedule) -> PaymentDuesSchedules */
        List<DesktopVoucherSupport.DueSchedule> sched = new ArrayList<>();
        List<Map<String, Object>> srows = PartyVoucherService.rows(b.get("schedules"));
        double schedTotal = 0d;
        for (Map<String, Object> r : srows) schedTotal += toDouble(r.get("amount"));
        if (schedTotal > 0) {
            double total = 0d, percent = 0d;
            for (Map<String, Object> r : srows) {
                DesktopVoucherSupport.DueSchedule ds = new DesktopVoucherSupport.DueSchedule();
                if (toInt(r.get("dueDays")) == 0)
                    throw new IllegalArgumentException(pay ? "Due Days Filed Required in Tax Grid" : "DueDays Filed Required in Grid");
                ds.DueDays = toInt(r.get("dueDays"));
                String dd = isoDay(r.get("dueDate"));
                if (dd == null) throw new IllegalArgumentException(pay ? "Due Days Filed Required in Tax Grid" : "Due Days Filed Required in Grid");
                ds.DueDate = dd;
                if (h.VoucherDate != null && dd.compareTo(h.VoucherDate) < 0)
                    throw new IllegalArgumentException(pay ? "Due date Should not greater than VoucherDate In Tax Grid" : "Due date Should not greater than VoucherDate");
                ds.DuePercentage = toDouble(r.get("duePercent"));
                percent += ds.DuePercentage;
                if (toDouble(r.get("amount")) == 0d)
                    throw new IllegalArgumentException(pay ? "Amount Filed Required in Tax Grid" : "Amount Filed Required in Grid");
                ds.Amount = toDouble(r.get("amount"));
                total += ds.Amount;
                ds.refDocumentTypeId = docType;
                sched.add(ds);
            }
            if (gridTotal != total) throw new IllegalArgumentException("Amount Doesn't Equal To Detail Grid Amount in Tax Schedule Grid");
            if (!pay && percent != 100d) throw new IllegalArgumentException("Percentage not grater than 100 in Tax Schedule Grid");
        } else {
            DesktopVoucherSupport.DueSchedule ds = new DesktopVoucherSupport.DueSchedule();
            ds.DueDays = 1;
            ds.DueDate = h.VoucherDate;
            ds.DuePercentage = 100d;
            ds.Amount = gridTotal;
            ds.refDocumentTypeId = docType;
            sched.add(ds);
        }
        h.VoucherAmount = voucherAmount;

        int id = s.saveVoucher(h, det, sched);
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", id);
        res.put("message", (recId > 0 ? "Record Update Successfully" : "Record Save Successfully") + voucherCode);
        res.put("nextVoucherCode", s.generateVoucherCode(docType));
        return res;
    }

    private static void fillLine(ContraVoucherDto.Detail d, Map<String, Object> r, double amount, double fcy,
                                 int locationTypeId, int currencyId, double rate) {
        d.JobLotId = toInt(r.get("jobLotId"));
        d.RefInvoiceNo = str(r.get("invoiceNo"));
        d.QtyIn = toDouble(r.get("qty"));
        d.ItemRate = toDouble(r.get("rate"));
        d.ItemAmount = amount;
        d.Comments = str(r.get("remarks"));
        d.IsTaxable = "False";
        d.LocationTypeId = locationTypeId;
        d.DMultiCurrencyId = currencyId;
        d.ReferenceAccountId = toInt(r.get("referenceAccountId"));
        d.DExchangeCurrencyRate = rate;
        d.DCurrencyAmount = fcy;
    }

    private static void applySub(ContraVoucherDto.Detail d, int subId, int subType) {
        d.SupplierCustomerId = subType == 1 ? subId : 0;
        d.EmployeeId = subType == 2 ? subId : 0;
        d.SubsidiaryAccountId = subId;
    }

    // ==================================================================================== load

    /** ReadById(ID). */
    public Map<String, Object> load(int docType, int id) {
        checkDocType(docType);
        boolean pay = docType == PAYABLES;
        Map<String, Object> v = s.readById(id, docType);
        if (v == null) return null;
        @SuppressWarnings("unchecked") Map<String, Object> hd = (Map<String, Object>) v.get("head");
        @SuppressWarnings("unchecked") List<Map<String, Object>> det = (List<Map<String, Object>>) v.get("details");
        @SuppressWarnings("unchecked") List<Map<String, Object>> sch = (List<Map<String, Object>>) v.get("schedules");
        boolean subFeature = s.feature(4);
        Map<String, Object> o = new LinkedHashMap<>();
        int ref = toInt(hd.get("RefAccountId"));
        o.put("id", id);
        o.put("voucherDate", isoDay(hd.get("VoucherDate")));
        o.put("accountId", ref);
        o.put("projectId", toInt(hd.get("ProjectId")));
        o.put("remarks", hd.get("Remarks"));
        o.put("invoiceNo", hd.get("ManualBillNo"));
        o.put("voucherCode", hd.get("VoucherCode"));
        o.put("currencyId", toInt(hd.get("MultiCurrencyId")));
        o.put("customAccounts", toBool(hd.get("CustomAccounts")));
        o.put("exchangeRate", toDouble(hd.get("ExchangeCurrencyRate")));
        o.put("fcAmount", toDouble(hd.get("FcAmount")));
        o.put("advanceTaxAccountId", toInt(hd.get("AdvanceTaxAccountId")));
        o.put("advanceTaxAmount", toDouble(hd.get("AdvanceTaxAmount")));
        o.put("otherChargesAccountId", toInt(hd.get("OtherChargesAccountId")));
        o.put("otherChargesAmount", toDouble(hd.get("OtherChargesAmount")));
        int firstLoc = 0;
        for (Map<String, Object> d : det) { if (toInt(d.get("LocationTypeId")) > 0) { firstLoc = toInt(d.get("LocationTypeId")); break; } }
        o.put("firstLocationTypeId", firstLoc);

        int hdrSub = 0; boolean flag = false;
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : det) {
            boolean nonTax = "False".equals(str(d.get("IsTaxable")));
            double dr = toDouble(d.get("DebitAmount")), cr = toDouble(d.get("CreditAmount"));
            if (subFeature && nonTax && (pay ? cr : dr) > 0 && !flag) {
                hdrSub = toInt(d.get("SubsidiaryAccountId"));
                flag = true;
            }
            if (!nonTax) continue;
            int acc = toInt(d.get("AccountId"));
            double sideAmt = pay ? dr : cr;
            boolean add;
            if (hdrSub > 0) {
                add = (acc != ref && !subFeature) || (toInt(d.get("SubsidiaryAccountId")) != hdrSub && subFeature && sideAmt > 0);
            } else {
                add = (acc != ref && !subFeature) || (subFeature && sideAmt > 0);
            }
            if (!add) continue;
            Map<String, Object> r = new LinkedHashMap<>();
            if (!pay) r.put("srNo", toInt(d.get("LineId")));
            r.put("accountCode", d.get("AccountCode"));
            r.put("accountId", acc);
            r.put("account", d.get("AccountTitle"));
            r.put("subsidiaryAccountId", toInt(d.get("SubsidiaryAccountId")));
            r.put("subsidiaryAccount", d.get("SubsidiaryAccountTitle"));
            r.put("subsidiaryAccountTypeId", toInt(d.get("SubsidiaryTypeId")));
            r.put("jobLotId", toInt(d.get("JobLotId")));
            r.put("jobLot", d.get("JobLotDescription"));
            r.put("invoiceNo", d.get("RefInvoiceNo"));
            r.put("qty", toDouble(d.get("QtyIn")));
            r.put("rate", toDouble(d.get("ItemRate")));
            r.put("amount", sideAmt);
            r.put("fcyAmount", toDouble(d.get("DCurrencyAmount")));
            r.put("referenceAccountId", toInt(d.get("ReferenceAccountId")));
            r.put("referenceAccount", d.get("ReferenceAccount"));
            r.put("remarks", d.get("Comments"));
            rows.add(r);
        }
        o.put("headerSubsidiaryId", hdrSub);
        o.put("rows", rows);
        /* tax: the first IsTaxable "True" line on the tax-account side */
        Map<String, Object> tax = new LinkedHashMap<>();
        tax.put("taxPercent", 0); tax.put("taxAmount", 0);
        for (Map<String, Object> d : det) {
            if ("True".equals(str(d.get("IsTaxable")))) {
                if ((pay ? toDouble(d.get("DebitAmount")) : toDouble(d.get("CreditAmount"))) > 0) {
                    tax.put("taxTypeId", toInt(d.get("TaxTypeId")));
                    tax.put("taxAccountId", toInt(d.get("AccountId")));
                    tax.put("taxPercent", toDouble(d.get("TaxPrcnt")));
                    tax.put("taxAmount", toDouble(d.get("TaxesTotalAmount")));
                    break;
                }
            } else { tax.put("taxPercent", 0); tax.put("taxAmount", 0); }
        }
        o.put("tax", tax);
        List<Map<String, Object>> sl = new ArrayList<>();
        for (Map<String, Object> x : sch) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("dueDays", toInt(x.get("DueDays")));
            m.put("dueDate", isoDay(x.get("DueDate")));
            m.put("duePercent", toDouble(x.get("DuePercentage")));
            m.put("amount", toDouble(x.get("Amount")));
            sl.add(m);
        }
        o.put("schedules", sl);
        return o;
    }

    // ================================================================================= history

    /** HistoryFill() -> VoucherFormHistory with Ids = DocumentTypeId and AppId = UserAccount.AppId. */
    public Map<String, Object> history(int docType, Map<String, String> q) {
        checkDocType(docType);
        boolean all = Boolean.TRUE.equals(s.rights(screenName(docType)).get("canViewAllRecord"));
        String status = q.getOrDefault("status", "Not Apporved");
        List<Map<String, Object>> raw = s.voucherFormHistory(String.valueOf(docType), all, q.get("dateType"),
                q.get("fromDate"), q.get("toDate"), toInt(q.get("fromDocNo")), toInt(q.get("toDocNo")),
                toInt(q.get("accountId")), status, "Approved".equals(status), s.appId());
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", r.get("Id"));
            m.put("AccountTypeId", r.get("AccountTypeId"));
            m.put("VoucherCode", r.get("VoucherCode"));
            m.put("DocumentTypeId", r.get("DocumentTypeId"));
            m.put("DocumentTypeCode", r.get("DocumentTypeCode"));
            m.put("VoucherDate", isoDay(r.get("VoucherDate")));
            m.put("ManualBillNo", r.get("ManualBillNo"));
            m.put("AccountTitle", r.get("AccountTitle"));
            m.put("AgainstAccount", r.get("AgainstAccount"));
            m.put("VoucherAmount", r.get("VoucherAmount"));
            m.put("FcyCode", r.get("Currencycode"));
            m.put("ExchangeRate", r.get("ExchangeCurrencyRate"));
            m.put("FcyAmount", r.get("FcAmount"));
            m.put("Remarks", r.get("Remarks"));
            m.put("EntryUser", r.get("UserName"));
            m.put("EntryDate", str(r.get("EntryDate")));
            m.put("ModifyUser", r.get("ModifyUserName"));
            m.put("ModifyDate", str(r.get("ModifyDate")));
            m.put("ApprovedUser", r.get("ApprovedUserName"));
            m.put("ApprovedDate", str(r.get("PostDate")));
            m.put("CheqNo", r.get("ChequeNo"));
            m.put("Attachment", r.get("NoOfAttachments"));
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

    /** VoucherDetailByHeaderId(). */
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
                m.put("SubsidiaryAccount", d.get("SubsidiaryAccountTitle"));
                m.put("Job/Lot", d.get("JobLotDescription"));
                m.put("InvoiceNo", d.get("RefInvoiceNo"));
                m.put("Qty", toDouble(d.get("QtyIn")));
                m.put("Rate", toDouble(d.get("ItemRate")));
                m.put("DebitAmount", dr);
                m.put("CreditAmount", cr);
                m.put("FcyAmount", toDouble(d.get("DCurrencyAmount")));
                m.put("Remarks", d.get("Comments"));
                m.put("ReferenceAccount", d.get("ReferenceAccount"));
                out.add(m);
            }
        }
        return out;
    }
}
