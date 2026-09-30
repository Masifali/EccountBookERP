package com.mst.services.fixedassets;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.repositories.fixedassets.FaRepository;
import com.mst.services.desktopvoucher.DesktopVoucherSupport;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
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

import static com.mst.services.desktopvoucher.DesktopVoucherSupport.isoDay;
import static com.mst.services.desktopvoucher.DesktopVoucherSupport.now;
import static com.mst.services.hrm.HrmSupport.*;

/**
 * Fixed Assets - 918 "Fixed Asset Purchase / Opening", Architecture.WinApp.Account_Definition.frmFixedAssetPurchase
 * (base.Name "frmFixedAssetPurchase", DocumentTypeId 130, ClientSize 1166 x 711). The form is a copy of
 * frmBillsPayables (its History caption still reads "Bills Payables History") plus an "Entry Type" combo
 * (CommonServices.StaticColumnsService("FixedAssetEntryType"): 1 Purchase, 2 Opening -> VoucherHead.FixedAssetEntryTypeId).
 *
 * Save: VoucherHead.Save (BLL 0654 - no document-type branch applies to 130) -> DAL 0586 SetData, written by
 * the shared {@link DesktopVoucherSupport#saveVoucher}: Sp_VoucherHead_Insert | _Update, Sp_VoucherDetail_Insert per
 * line, USP_VoucherBalanceCheck, Sp_PaymentDueSchedule_Insert per schedule row, Sp_VoucherHead_H_Insert,
 * Sp_VoucherDetail_H_Insert per line, [DAW].[USp_DocumentApprovalDetail_Insert] - one transaction. The form has
 * no cost-centre and no multi-currency detail lists, so those DAL steps never run.
 *
 * Lines built by Insert():2099 (reproduced exactly):
 *   per grid row  row account DEBIT (Against = credit account, the row's subsidiary) and the credit account
 *                 CREDIT (Against = row account, the header "Subsidiary A/C"), LineId 1, 2, 3 ... IsTaxable "False",
 *                 LocationTypeId, currency, rate, FCY on both; "Fcy Amount Not Found In Detail Grid at Row#n".
 *   sales tax     when Tax Amount > 0 and a Tax Type: tax account Dr / credit account Cr (header subsidiary),
 *                 IsTaxable "True", TaxPrcnt, TaxTypeId, comments "Tax % x" / "Tax%x", LineId 0.
 *   advance tax   when amount > 0 and an account: adv-tax account Dr / credit account Cr (header subsidiary),
 *                 comments "Advance Tax", IsTaxable not set (NULL -> not sent), LineId 0.
 *   other charges when amount != 0 and an account: signed amounts, the accounts swap on the sign, the header
 *                 subsidiary on the credit-account line.
 *   schedule      the Amount Distribution grid when its total > 0 (Due Days / Due Date / Amount required,
 *                 "Due date Should not greater than VoucherDate In Tax Grid", total must equal the grid total),
 *                 otherwise one row: 1 day, Doc Date, 100 %, the grid total.
 * Header: VoucherCode = Doc No box, AgainstAccountId = Sales Tax Account, ManualBillNo / DueDate = Bill No /
 * Bill Date, IncludeWHT = tax type and tax account set, BranchId = the user's branch, FinancialYearId = the
 * active year, EntryUser = ModifyUser = user, EntryDate = ModifyDate = now, CustomAccounts = the check box,
 * AttachmentsValues kept on update (no attachment list is sent - the Attachments button is not ported).
 *
 * Rights (SetRightsValueInRightsObject on the desktop): View on ScreenDefinition 918 for every call; Save /
 * Save As = Save, Update = Update, 102-Print = Print (page), history "SaveAs" = Update, "CanView AllRecord" for
 * the history. Delete: btnDelete is invisible and btnDelete_Click is empty - no delete.
 *
 * DEVIATIONS (web): tenancy from the session; ReadById refuses a voucher of another organization / company or of
 * another document type; the header subsidiary must be in the credit account's subsidiary list and each row's
 * subsidiary in its account's list (or be the account itself, type 4 - what the desktop adds when the account
 * has no subsidiaries). Attachments (Ctrl+F10), "Accounts Allocate To Custom" and the grid-layout bar are not ported.
 */
@Service
public class FaPurchaseService {

    public static final int SCREEN_ID = 918;
    public static final int DOC_TYPE = 130;

    @Autowired private FaRepository repo;
    @Autowired private DesktopVoucherSupport s;
    @Autowired private HrmSupport hrm;

    // ================================================================================ lookups

    /** InitializeComponentMethod():477 + DefaultConfigurations():584. */
    public Map<String, Object> setup() {
        UserAccount u = hrm.user(SCREEN_ID);
        Map<String, Object> m = new LinkedHashMap<>();
        Map<String, Object> rights = hrm.rights(u, SCREEN_ID);
        rights.put("canViewAllRecord", hrm.can(u, SCREEN_ID, "CanView AllRecord"));
        m.put("rights", rights);
        boolean sub = s.feature(4);
        m.put("subsidiaryFeature", sub);
        m.put("voucherCode", s.generateVoucherCode(DOC_TYPE));
        m.put("locationTypes", repo.locationTypes());
        m.put("costCenters", repo.costCenters(u));
        List<Map<String, Object>> all = allAccounts();
        m.put("headerAccounts", byTypes(all, "3"));                            // CreditAccountAccountBind
        m.put("taxAccounts", byTypes(all, "3,6,8,19"));                        // SalesTaxAccountBind
        m.put("advanceTaxAccounts", byTypes(all, "6,16,17"));                  // AdvanceTaxAccountBind
        m.put("referenceAccounts", byTypes(all, "3, 5, 6, 7, 8"));             // ReferenceAccountBind
        m.put("accountTypes", accountTypes(all));                              // AccountTypeBind
        m.put("allAccounts", all);                                             // CmbAccountType_Leave filters it
        m.put("currencies", s.currencies());
        m.put("customGroups", repo.customGroups(DOC_TYPE));
        m.put("jobLots", s.jobLots());
        m.put("subsidiaries", sub ? s.subsidiaries() : new ArrayList<>());
        m.put("taxTypes", repo.taxTypes(u));
        m.put("entryTypes", repo.staticColumns("FixedAssetEntryType"));
        m.put("historyAccounts", historyAccounts());
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

    /** CommonServices.Accounts_GetAccountTitleByAccountTypeIds() (AllAccountsWithTypeIdDbCall). */
    private List<Map<String, Object>> allAccounts() {
        List<Map<String, Object>> all = new ArrayList<>();
        for (Map<String, Object> r : s.accountsGetAccountTitleByAccountTypeIds(null, 0)) {
            all.add(map("Id", toInt(r.get("Id")), "AccountTitle", r.get("AccountTitle"), "AccountCode", r.get("AccountCode"),
                    "AccountTypeId", toInt(r.get("AccountTypeId")), "NoteTitle", r.get("NoteTitle"), "AccountType", r.get("AccountType")));
        }
        return all;
    }

    private static List<Map<String, Object>> byTypes(List<Map<String, Object>> all, String csv) {
        Set<Integer> set = new HashSet<>();
        for (String p : csv.split(",")) { try { set.add(Integer.parseInt(p.trim())); } catch (NumberFormatException ignored) { } }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> a : all) if (set.contains(toInt(a.get("AccountTypeId")))) out.add(a);
        return out;
    }

    /** AccountTypeBind():1279 - distinct (AccountTypeId, AccountType) for {3,5,6,8,16,17,18,19}, ordered by id. */
    private static List<Map<String, Object>> accountTypes(List<Map<String, Object>> all) {
        Set<Integer> want = new HashSet<>(Arrays.asList(3, 5, 6, 8, 16, 17, 18, 19));
        Map<Integer, String> types = new TreeMap<>();
        for (Map<String, Object> a : all) {
            int t = toInt(a.get("AccountTypeId"));
            if (want.contains(t) && !types.containsKey(t)) types.put(t, str(a.get("AccountType")));
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map.Entry<Integer, String> e : types.entrySet()) out.add(map("Id", e.getKey(), "AccountType", e.getValue()));
        return out;
    }

    /** AccountTitleHistoryDbCall():2883 - CompanyId = UserAccount.OrganizationId (as the form sends it), AppId, UserId. */
    public List<Map<String, Object>> historyAccounts() {
        hrm.user(SCREEN_ID);
        return s.historyAccountHeaders(s.orgId(), s.appId(), s.userId(), String.valueOf(DOC_TYPE));
    }

    /** CmbCustomGroup_Leave:1189 -> GetCustomAccountsByGroup(CompanyId, id). */
    public List<Map<String, Object>> customAccounts(int customGroupId) {
        UserAccount u = hrm.user(SCREEN_ID);
        if (!owns(repo.customGroups(DOC_TYPE), "Id", customGroupId)) return new ArrayList<>();
        return repo.customAccounts(u, customGroupId);
    }

    /** cmbCurrency_Leave:1319 -> GetLastExchangeRateAndCurrencyOfVoucher (DocumentTypeIds "130", Ids = currency). */
    public List<Map<String, Object>> lastRate(int currencyId) {
        UserAccount u = hrm.user(SCREEN_ID);
        return repo.lastExchangeRate(u, String.valueOf(DOC_TYPE), String.valueOf(currencyId));
    }

    /** salesTex():2749 -> TaxScheduleMain.ReadTaxSchedule(EffectedDate = Doc Date, TaxNameId). */
    public List<Map<String, Object>> taxSchedule(int taxTypeId, String date) {
        UserAccount u = hrm.user(SCREEN_ID);
        LocalDateTimeHolder h = new LocalDateTimeHolder(date);
        return repo.taxSchedule(u, h.value(), taxTypeId);
    }

    /** DetailAccountCurrentBalance / CreditAccountCurrentBalance -> ReadByCurrentBalanceByDateAndAccountId. */
    public Map<String, Object> balance(int accountId, String date) {
        hrm.user(SCREEN_ID);
        List<Map<String, Object>> r = s.currentBalance(isoDay(date), accountId);
        return map("found", !r.isEmpty(), "Balance", r.isEmpty() ? null : str(r.get(0).get("Balance")));
    }

    /** ResetForm() -> UpdateDocumentNoUI(DocumentNoDBCall()). */
    public Map<String, Object> nextCode() {
        hrm.user(SCREEN_ID);
        return map("voucherCode", s.generateVoucherCode(DOC_TYPE));
    }

    // ==================================================================================== save

    @Transactional(rollbackFor = Exception.class)
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = hrm.user(SCREEN_ID);
        String mode = str(b.get("mode"));
        int recId = toInt(b.get("recId"));
        if ("update".equals(mode)) {
            if (recId == 0) throw invalid("Record Not Update  " + recId);                      // btnUpdate_Click:2451
            hrm.require(u, SCREEN_ID, "Update");
            if (s.readById(recId, DOC_TYPE) == null) throw invalid("Record Not Found");
        } else {
            recId = 0;                                                                          // btnsave / btnSaveAs: RecId = 0
            hrm.require(u, SCREEN_ID, "Save");
        }
        List<Map<String, Object>> rows = list(b.get("rows"));
        if (rows.isEmpty()) throw invalid("Grid Record not found");                             // Insert():2125

        boolean subFeature = s.feature(4);
        int creditId = toInt(b.get("accountId"));
        int hdrSubId = toInt(b.get("headerSubsidiaryId"));
        int hdrSubType = 0;
        List<Map<String, Object>> subs = subFeature ? s.subsidiaries() : new ArrayList<>();
        int subsForCredit = 0;
        for (Map<String, Object> x : subs) {
            if (toInt(x.get("AccountId")) == creditId) {
                subsForCredit++;
                if (toInt(x.get("Id")) == hdrSubId && hdrSubType == 0) hdrSubType = toInt(x.get("SubsidiaryTypeId"));
            }
        }
        if (hdrSubId > 0 && hdrSubType == 0) hdrSubId = 0;                                      // not in the combo's list

        /* FormValidation():1468 */
        int locationTypeId = toInt(b.get("locationTypeId"));
        if (locationTypeId == 0 || !owns(repo.locationTypes(), "Id", locationTypeId)) throw invalid("Location Type Field is Required");
        int projectId = toInt(b.get("projectId"));
        if (projectId == 0 || !owns(repo.costCenters(u), "Id", projectId)) throw invalid("Cost Center  Field Required");
        String docNo = str(b.get("voucherCode")).trim();
        if (docNo.isEmpty() || "0".equals(docNo)) throw invalid("DocNo  Field Required");
        List<Map<String, Object>> all = allAccounts();
        if (creditId == 0 || !owns(all, "Id", creditId)) throw invalid("Account  Field Required");
        if (subFeature && subsForCredit > 0 && hdrSubId == 0) throw invalid("Subsidiary Account Title Field Required");
        int currencyId = toInt(b.get("currencyId"));
        if (currencyId == 0) throw invalid("Fcy Code Field is Required");
        double rate = toDouble(b.get("exchangeRate"));
        if (rate == 0d) throw invalid("Exchange Rate Field is Required");
        double fcTotal = 0d;
        for (Map<String, Object> r : rows) fcTotal += toDouble(r.get("fcyAmount"));
        double fcAmount = Math.round(fcTotal * 1000d) / 1000d;                                  // txtFcyAmount "#,##0.###"
        if (fcAmount == 0d) throw invalid("Fcy Amount Field is Required");

        int taxAccountId = toInt(b.get("taxAccountId"));
        int taxTypeId = toInt(b.get("taxTypeId"));
        int advAcc = toInt(b.get("advanceTaxAccountId"));
        double advAmt = toDouble(b.get("advanceTaxAmount"));
        int ocAcc = toInt(b.get("otherChargesAccountId"));
        double ocAmt = toDouble(b.get("otherChargesAmount"));
        if (taxAccountId != 0 && !owns(all, "Id", taxAccountId)) taxAccountId = 0;
        if (advAcc != 0 && !owns(all, "Id", advAcc)) advAcc = 0;

        String voucherDate = isoDay(b.get("voucherDate"));
        ContraVoucherDto.Head h = new ContraVoucherDto.Head();
        h.Id = recId;
        h.ProjectId = projectId;
        h.DocumentTypeId = DOC_TYPE;
        h.VoucherCode = toInt(docNo);
        h.VoucherDate = voucherDate;
        h.RefAccountId = creditId;
        h.AgainstAccountId = taxAccountId;
        h.ManualBillNo = str(b.get("invoiceNo"));
        h.DueDate = isoDay(b.get("invoiceDate"));
        h.Remarks = str(b.get("remarks"));
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.BranchId = u.getBranchesId() == null ? 0 : u.getBranchesId();
        h.FinancialYearId = hrm.financialYearId();
        h.EntryUser = u.getId();
        h.ModifyUser = u.getId();
        h.IncludeWHT = taxTypeId > 0 && taxAccountId > 0;
        h.AdvanceTaxAccountId = advAcc;
        h.AdvanceTaxAmount = advAmt;
        h.OtherChargesAccountId = ocAcc;
        h.OtherChargesAmount = ocAmt;
        h.FixedAssetEntryTypeId = toInt(b.get("entryTypeId"));
        h.MultiCurrencyId = currencyId;
        h.ExchangeCurrencyRate = rate;
        h.FcAmount = fcAmount;
        h.CustomAccounts = toBool(b.get("customAccounts"));
        String now = now();
        h.EntryDate = now;
        h.ModifyDate = now;
        if (recId > 0) {                                                                        // AttachmentsValues kept on update
            Map<String, Object> v = s.readById(recId, DOC_TYPE);
            @SuppressWarnings("unchecked") Map<String, Object> hd = v == null ? null : (Map<String, Object>) v.get("head");
            if (hd != null) {
                String av = hd.get("AttachmentsValues") == null ? "" : str(hd.get("AttachmentsValues"));
                h.AttachmentsValues = av.isEmpty() ? null : av;
                h.CustomAttachmentsValues = hd.get("CustomAttachmentsValues") == null ? null : str(hd.get("CustomAttachmentsValues"));
            }
        }

        List<ContraVoucherDto.Detail> det = new ArrayList<>();
        double voucherAmount = 0d, gridTotal = 0d;
        int lineId = 1;
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            int acc = toInt(r.get("accountId"));
            int rowSub = toInt(r.get("subsidiaryAccountId"));
            int rowSubType = toInt(r.get("subsidiaryAccountTypeId"));
            if (acc == 0) throw invalid("Account  Field Required");
            if (rowSub > 0) rowSubType = rowSubsidiaryType(subs, acc, rowSub, rowSubType);
            if (creditId == acc && !subFeature) throw invalid("Dr Account Title Cannot Be Equal To Cr Account Title...");
            if (hdrSubId > 0 && hdrSubId == rowSub && subFeature)
                throw invalid("Dr Subsidiary Account Cannot Be Equal To Cr Subsidiary Account...");
            double amount = toDouble(r.get("amount"));
            double fcy = toDouble(r.get("fcyAmount"));
            gridTotal += amount;

            ContraVoucherDto.Detail dr = new ContraVoucherDto.Detail();
            dr.LineId = lineId;
            dr.AccountId = acc;
            dr.AgainstAccountId = creditId;
            dr.SubsidiaryTypeId = rowSubType;
            if (rowSub > 0) applySub(dr, rowSub, rowSubType);
            fillLine(dr, r, amount, fcy, locationTypeId, currencyId, rate);
            dr.DebitAmount = amount;
            if (fcy == 0d) throw invalid("Fcy Amount Not Found In Detail Grid at Row#" + (i + 1));
            det.add(dr);
            lineId++;
            voucherAmount += amount;

            ContraVoucherDto.Detail cr = new ContraVoucherDto.Detail();
            cr.LineId = lineId;
            cr.AccountId = creditId;
            cr.AgainstAccountId = acc;
            cr.SubsidiaryTypeId = hdrSubId > 0 ? hdrSubType : 0;
            if (hdrSubId > 0) applySub(cr, hdrSubId, hdrSubType);
            fillLine(cr, r, amount, fcy, locationTypeId, currencyId, rate);
            cr.CreditAmount = amount;
            det.add(cr);
            lineId++;
        }

        double taxAmt = toDouble(b.get("taxAmount"));
        double taxPct = toDouble(b.get("taxPercent"));
        String pctText = str(b.get("taxPercentText"));
        if (taxAmt > 0 && taxTypeId > 0) {
            ContraVoucherDto.Detail t1 = new ContraVoucherDto.Detail();
            t1.AccountId = taxAccountId; t1.AgainstAccountId = creditId; t1.Comments = "Tax % " + pctText;
            t1.DebitAmount = taxAmt; t1.TaxesTotalAmount = taxAmt; t1.CreditAmount = 0d;
            ContraVoucherDto.Detail t2 = new ContraVoucherDto.Detail();
            t2.AccountId = creditId; t2.Comments = "Tax%" + pctText; t2.AgainstAccountId = taxAccountId;
            t2.CreditAmount = taxAmt; t2.SubsidiaryTypeId = hdrSubId > 0 ? hdrSubType : 0;
            if (hdrSubId > 0) applySub(t2, hdrSubId, hdrSubType);
            t2.TaxesTotalAmount = taxAmt; t2.DebitAmount = 0d;
            for (ContraVoucherDto.Detail t : Arrays.asList(t1, t2)) {
                t.IsTaxable = "True"; t.TaxPrcnt = taxPct; t.TaxTypeId = taxTypeId; t.LineId = 0;
                det.add(t);
            }
        }
        if (advAmt > 0 && advAcc > 0) {
            ContraVoucherDto.Detail a1 = new ContraVoucherDto.Detail();
            a1.AccountId = advAcc; a1.AgainstAccountId = creditId; a1.Comments = "Advance Tax"; a1.DebitAmount = advAmt; a1.CreditAmount = 0d;
            ContraVoucherDto.Detail a2 = new ContraVoucherDto.Detail();
            a2.AccountId = creditId; a2.AgainstAccountId = advAcc; a2.Comments = "Advance Tax"; a2.CreditAmount = advAmt;
            a2.SubsidiaryTypeId = hdrSubId > 0 ? hdrSubType : 0;
            if (hdrSubId > 0) applySub(a2, hdrSubId, hdrSubType);
            a2.DebitAmount = 0d;
            det.add(a1); det.add(a2);
        }
        if (ocAmt != 0 && ocAcc > 0) {
            boolean pos = ocAmt > 0;
            ContraVoucherDto.Detail o1 = new ContraVoucherDto.Detail();
            o1.AccountId = pos ? ocAcc : creditId; o1.AgainstAccountId = pos ? creditId : ocAcc;
            o1.Comments = "Other Charges"; o1.DebitAmount = ocAmt;
            if (!pos) { o1.SubsidiaryTypeId = hdrSubId > 0 ? hdrSubType : 0; if (hdrSubId > 0) applySub(o1, hdrSubId, hdrSubType); }
            ContraVoucherDto.Detail o2 = new ContraVoucherDto.Detail();
            o2.AccountId = pos ? creditId : ocAcc; o2.AgainstAccountId = pos ? ocAcc : creditId;
            o2.Comments = "Other Charges"; o2.CreditAmount = ocAmt;
            if (pos) { o2.SubsidiaryTypeId = hdrSubId > 0 ? hdrSubType : 0; if (hdrSubId > 0) applySub(o2, hdrSubId, hdrSubType); }
            det.add(o1); det.add(o2);
        }

        /* Amount Distribution (grdSchedule) -> PaymentDuesSchedules (:2350) */
        List<DesktopVoucherSupport.DueSchedule> sched = new ArrayList<>();
        List<Map<String, Object>> srows = list(b.get("schedules"));
        double schedTotal = 0d;
        for (Map<String, Object> r : srows) schedTotal += toDouble(r.get("amount"));
        if (schedTotal > 0) {
            double total = 0d;
            for (Map<String, Object> r : srows) {
                DesktopVoucherSupport.DueSchedule ds = new DesktopVoucherSupport.DueSchedule();
                if (toInt(r.get("dueDays")) == 0) throw invalid("Due Days Filed Required in Tax Grid");
                ds.DueDays = toInt(r.get("dueDays"));
                String dd = isoDay(r.get("dueDate"));
                if (dd == null) throw invalid("Due Days Filed Required in Tax Grid");
                ds.DueDate = dd;
                if (voucherDate != null && dd.compareTo(voucherDate) < 0) throw invalid("Due date Should not greater than VoucherDate In Tax Grid");
                ds.DuePercentage = toDouble(r.get("duePercent"));
                if (toDouble(r.get("amount")) == 0d) throw invalid("Amount Filed Required in Tax Grid");
                ds.Amount = toDouble(r.get("amount"));
                total += ds.Amount;
                ds.refDocumentTypeId = DOC_TYPE;
                sched.add(ds);
            }
            if (gridTotal != total) throw invalid("Amount Doesn't Equal To Detail Grid Amount in Tax Schedule Grid");
        } else {
            DesktopVoucherSupport.DueSchedule ds = new DesktopVoucherSupport.DueSchedule();
            ds.DueDays = 1;
            ds.DueDate = voucherDate;
            ds.DuePercentage = 100d;
            ds.Amount = gridTotal;
            ds.refDocumentTypeId = DOC_TYPE;
            sched.add(ds);
        }
        h.VoucherAmount = voucherAmount;

        int id = s.saveVoucher(h, det, sched);
        Map<String, Object> res = saved(id, (recId > 0 ? "Record Update Successfully" : "Record Save Successfully") + h.VoucherCode);
        res.put("nextVoucherCode", s.generateVoucherCode(DOC_TYPE));
        return res;
    }

    /** The row's subsidiary: its type from the account's subsidiary list, or 4 when it is the account itself. */
    private static int rowSubsidiaryType(List<Map<String, Object>> subs, int acc, int sub, int sentType) {
        for (Map<String, Object> x : subs) {
            if (toInt(x.get("AccountId")) == acc && toInt(x.get("Id")) == sub && toInt(x.get("SubsidiaryTypeId")) == sentType) return sentType;
        }
        for (Map<String, Object> x : subs) {
            if (toInt(x.get("AccountId")) == acc && toInt(x.get("Id")) == sub) return toInt(x.get("SubsidiaryTypeId"));
        }
        if (sub == acc && sentType == 4) return 4;
        throw invalid("Subsidiary Account Title Field Required");
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

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Object o) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (o instanceof List) for (Object x : (List<Object>) o) if (x instanceof Map) out.add((Map<String, Object>) x);
        return out;
    }

    // ==================================================================================== load

    /** ReadById(ID):2463. */
    @SuppressWarnings("unchecked")
    public Map<String, Object> load(int id) {
        hrm.user(SCREEN_ID);
        Map<String, Object> v = s.readById(id, DOC_TYPE);
        if (v == null) throw invalid("Record Not Found");
        Map<String, Object> hd = (Map<String, Object>) v.get("head");
        List<Map<String, Object>> det = (List<Map<String, Object>>) v.get("details");
        List<Map<String, Object>> sch = (List<Map<String, Object>>) v.get("schedules");
        boolean subFeature = s.feature(4);
        int ref = toInt(hd.get("RefAccountId"));
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("id", id);
        o.put("voucherDate", isoDay(hd.get("VoucherDate")));
        o.put("accountId", ref);
        o.put("projectId", toInt(hd.get("ProjectId")));
        o.put("remarks", str(hd.get("Remarks")));
        o.put("invoiceNo", str(hd.get("ManualBillNo")));
        o.put("voucherCode", str(hd.get("VoucherCode")));
        o.put("entryTypeId", toInt(hd.get("FixedAssetEntryTypeId")));
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

        int hdrSub = 0;
        boolean flag = false;
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : det) {
            boolean nonTax = "False".equals(str(d.get("IsTaxable")));
            double drAmt = toDouble(d.get("DebitAmount")), crAmt = toDouble(d.get("CreditAmount"));
            if (subFeature && nonTax && crAmt > 0 && !flag) { hdrSub = toInt(d.get("SubsidiaryAccountId")); flag = true; }
            if (!nonTax) continue;
            int acc = toInt(d.get("AccountId"));
            boolean add = hdrSub > 0
                    ? (acc != ref && !subFeature) || (toInt(d.get("SubsidiaryAccountId")) != hdrSub && subFeature && drAmt > 0)
                    : (acc != ref && !subFeature) || (subFeature && drAmt > 0);
            if (!add) continue;
            rows.add(map("accountCode", str(d.get("AccountCode")), "accountId", acc, "account", str(d.get("AccountTitle")),
                    "subsidiaryAccountId", toInt(d.get("SubsidiaryAccountId")), "subsidiaryAccount", str(d.get("SubsidiaryAccountTitle")),
                    "subsidiaryAccountTypeId", toInt(d.get("SubsidiaryTypeId")), "jobLotId", toInt(d.get("JobLotId")),
                    "jobLot", str(d.get("JobLotDescription")), "invoiceNo", str(d.get("RefInvoiceNo")), "qty", toDouble(d.get("QtyIn")),
                    "rate", toDouble(d.get("ItemRate")), "amount", drAmt, "fcyAmount", toDouble(d.get("DCurrencyAmount")),
                    "referenceAccountId", toInt(d.get("ReferenceAccountId")), "referenceAccount", str(d.get("ReferenceAccount")),
                    "remarks", str(d.get("Comments"))));
        }
        o.put("headerSubsidiaryId", hdrSub);
        o.put("rows", rows);
        Map<String, Object> tax = map("taxPercent", "0", "taxAmount", "0");
        for (Map<String, Object> d : det) {
            if ("True".equals(str(d.get("IsTaxable")))) {
                if (toDouble(d.get("DebitAmount")) > 0) {
                    tax.put("taxTypeId", toInt(d.get("TaxTypeId")));
                    tax.put("taxAccountId", toInt(d.get("AccountId")));
                    tax.put("taxPercent", netNum(d.get("TaxPrcnt")));
                    tax.put("taxAmount", netNum(d.get("TaxesTotalAmount")));
                    break;
                }
            } else { tax.put("taxPercent", "0"); tax.put("taxAmount", "0"); }
        }
        o.put("tax", tax);
        List<Map<String, Object>> sl = new ArrayList<>();
        for (Map<String, Object> x : sch) {
            sl.add(map("dueDays", toInt(x.get("DueDays")), "dueDate", isoDay(x.get("DueDate")),
                    "duePercent", toDouble(x.get("DuePercentage")), "amount", toDouble(x.get("Amount"))));
        }
        o.put("schedules", sl);
        return o;
    }

    private static String netNum(Object v) {
        double d = toDouble(v);
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return java.math.BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
    }

    // ================================================================================= history

    /** HistoryFill():3019 - VoucherFormHistory(Ids = "130", AppId = UserAccount.AppId, PostState = CanView AllRecord). */
    public Map<String, Object> history(Map<String, String> q) {
        UserAccount u = hrm.user(SCREEN_ID);
        boolean all = hrm.can(u, SCREEN_ID, "CanView AllRecord");
        String status = q.getOrDefault("status", "Not Apporved");
        List<Map<String, Object>> raw = s.voucherFormHistory(String.valueOf(DOC_TYPE), all, q.get("dateType"),
                q.get("fromDate"), q.get("toDate"), toInt(q.get("fromDocNo")), toInt(q.get("toDocNo")),
                toInt(q.get("accountId")), status, "Approved".equals(status), s.appId());
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            rows.add(map("Id", r.get("Id"), "AccountTypeId", r.get("AccountTypeId"), "VoucherCode", r.get("VoucherCode"),
                    "DocumentTypeId", r.get("DocumentTypeId"), "DocumentTypeCode", r.get("DocumentTypeCode"),
                    "VoucherDate", r.get("VoucherDate"), "ManualBillNo", r.get("ManualBillNo"), "AccountTitle", r.get("AccountTitle"),
                    "AgainstAccount", r.get("AgainstAccount"), "VoucherAmount", r.get("VoucherAmount"), "FcyCode", r.get("Currencycode"),
                    "ExchangeRate", r.get("ExchangeCurrencyRate"), "FcyAmount", r.get("FcAmount"), "Remarks", r.get("Remarks"),
                    "EntryUser", r.get("UserName"), "EntryDate", r.get("EntryDate"), "ModifyUser", r.get("ModifyUserName"),
                    "ModifyDate", r.get("ModifyDate"), "ApprovedUser", r.get("ApprovedUserName"), "ApprovedDate", r.get("PostDate"),
                    "CheqNo", r.get("ChequeNo"), "Attachment", r.get("NoOfAttachments")));
        }
        Map<String, Object> out = map("rows", rows);
        if (!raw.isEmpty()) {
            out.put("totalVouchers", toInt(raw.get(0).get("TotalVouchers")));
            out.put("approvedVouchers", toInt(raw.get(0).get("TotalApprovedVoucher")));
            out.put("unApprovedVouchers", toInt(raw.get(0).get("TotalUnApprovedVoucher")));
        }
        return out;
    }

    /** VoucherDetailByHeaderId():3301 - lines with a debit or a credit. */
    @SuppressWarnings("unchecked")
    public List<Map<String, Object>> historyDetail(int id) {
        hrm.user(SCREEN_ID);
        Map<String, Object> v = s.readById(id, DOC_TYPE);
        List<Map<String, Object>> out = new ArrayList<>();
        if (v == null) return out;
        for (Map<String, Object> d : (List<Map<String, Object>>) v.get("details")) {
            double dr = toDouble(d.get("DebitAmount")), cr = toDouble(d.get("CreditAmount"));
            if (dr > 0 || cr > 0) {
                out.add(map("AccountCode", d.get("AccountCode"), "AccountTitle", d.get("AccountTitle"),
                        "SubsidiaryAccount", d.get("SubsidiaryAccountTitle"), "Job/Lot", d.get("JobLotDescription"),
                        "InvoiceNo", d.get("RefInvoiceNo"), "Qty", toDouble(d.get("QtyIn")), "Rate", toDouble(d.get("ItemRate")),
                        "DebitAmount", dr, "CreditAmount", cr, "FcyAmount", toDouble(d.get("DCurrencyAmount")),
                        "Remarks", d.get("Comments"), "ReferenceAccount", d.get("ReferenceAccount")));
            }
        }
        return out;
    }

    /** A voucher of this screen and tenant (print buttons check it before the print request). */
    public Map<String, Object> printCheck(int id) {
        UserAccount u = hrm.user(SCREEN_ID);
        if (id <= 0 || s.readById(id, DOC_TYPE) == null) throw invalid("Record Not Found For Display");
        return map("id", id, "canPrint", hrm.can(u, SCREEN_ID, "Print"));
    }

    /** "yyyy-MM-dd" -> the value the procedure receives (a Timestamp at midnight), null when blank. */
    private static final class LocalDateTimeHolder {
        private final Object v;
        LocalDateTimeHolder(String s) { this.v = ts(toDay(s)); }
        Object value() { return v; }
    }
}
