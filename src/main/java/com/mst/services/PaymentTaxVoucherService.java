package com.mst.services;

import com.mst.models.ConfigrationsAllocation;
import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.PaymentTaxVoucherDto;
import com.mst.repositories.DesktopVoucherWriter;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.text.DecimalFormat;
import java.text.DecimalFormatSymbols;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

/**
 * Screens 850 PaymentVoucherWithTax, 851 frmBankPaymentVoucherTax, 852 frmCashPaymentVoucherTax.
 *
 * Desktop: Architecture.WinApp.Account_Definition.VouchersWithTax.PaymentVoucherNew. The dashboard opens
 * the one class with form.Tag = ScreenName, and the form reads that Tag into DocumentTypeId
 * (frmCashPaymentVoucherTax = 1, frmBankPaymentVoucherTax = 2, PaymentVoucherWithTax = 0 -> both, Cash first).
 * "mode" below is that Tag: 0 = 850, 1 = 852 (cash), 2 = 851 (bank). The rights are read under the Tag.
 *
 * Persistence is the same BLL VoucherHead.Save chain every desktop voucher uses (DesktopVoucherWriter), not
 * the JPA saveVoucher. The ledger lines are derived here from the form's own Insert() (vd, vd2, WHT vd3/vd4,
 * SRB vd5/vd6, discount vd7/vd8), so the page never chooses a debit/credit side or a total.
 */
@Service
public class PaymentTaxVoucherService {

    private static final Logger LOG = LoggerFactory.getLogger(PaymentTaxVoucherService.class);

    private static final String SQL_USER_RIGHTS =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, @CompanyId=?, @Activity=?";

    @Autowired private DesktopVoucherWriter writer;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private VoucherDesktopConfigService config;
    @Autowired private DesktopVoucherScreenService screens;

    // ================================================================================ form identity

    public static void requireMode(int mode) {
        if (mode < 0 || mode > 2) throw new IllegalArgumentException("Unknown screen " + mode);
    }

    /** Valid document types of a screen: 850 both, 852 cash only, 851 bank only. */
    public static void requireDoc(int mode, int doc) {
        requireMode(mode);
        if (doc != 1 && doc != 2) throw new IllegalArgumentException("Voucher Type Field is Required");
        if (mode != 0 && doc != mode) throw new IllegalArgumentException("Unknown voucher type " + doc + " for this screen");
    }

    /** base.Tag - the ScreenName the rights are read under. */
    public static String screenName(int mode) {
        switch (mode) {
            case 1: return "frmCashPaymentVoucherTax";
            case 2: return "frmBankPaymentVoucherTax";
            default: return "PaymentVoucherWithTax";
        }
    }

    /** DocumentTypeFill(): Rows[0].Activate() - Cash on 850, the only row otherwise. */
    public static int startDoc(int mode) { return mode == 2 ? 2 : 1; }

    // ================================================================================ lookups

    /** Everything the form binds on Load, each from the desktop's own call. */
    public Map<String, Object> lookups(int mode) {
        requireMode(mode);
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> r = new LinkedHashMap<>();

        /* DocumentTypeFill */
        List<Map<String, Object>> types = new ArrayList<>();
        if (mode != 2) types.add(typeRow(1, "Cash Payment Voucher"));
        if (mode != 1) types.add(typeRow(2, "Bank Payment Voucher"));
        r.put("voucherTypes", types);
        r.put("startDoc", startDoc(mode));

        /* AccountTitleFill: COAAllocation.GetDetailAccountByDocumentTypeId per document type */
        Map<String, Object> header = new LinkedHashMap<>();
        for (Map<String, Object> t : types) {
            int doc = toInt(t.get("Id"));
            List<Map<String, Object>> list = new ArrayList<>();
            for (Map<String, Object> row : jdbcTemplate.queryForList(
                    "EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Activity=?",
                    u.getOrganizationId(), u.getCompanyId(), doc, "GetDetailAccountsByDocumentTypeId")) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", ci(row, "Id"));
                o.put("AccountTitle", ci(row, "AccountTitle"));
                o.put("CurrencyId", ci(row, "CurrencyId"));
                o.put("CurrencyCode", ci(row, "CurrencyCode"));
                list.add(o);
            }
            header.put(String.valueOf(doc), list);
        }
        r.put("headerAccounts", header);

        r.put("detailAccounts", detailAccounts());                                    // DetailAccountFill
        r.put("referenceAccounts", screens.globalAccounts(new int[] {3, 5, 6, 7, 8}, new int[0], new int[0]));
        r.put("srbAccounts", screens.globalAccounts(new int[] {6, 8}, new int[0], new int[0]));
        r.put("discountAccounts", screens.globalAccounts(new int[] {11, 13, 20, 21, 22}, new int[0], new int[0]));
        r.put("whtAccounts", screens.globalAccounts(new int[0], new int[] {2, 11, 15}, new int[0]));   // AccountsComboBind
        r.put("locationTypes", screens.locationTypes());
        r.put("currencies", screens.currenciesWithRate());                            // CurrencyFill (header + Tcy detail)
        r.put("paymentTypes", jdbcTemplate.queryForList("EXEC [Account].[USP_lookUp_GetAllMethod] @Activity=?",
                "GetDataPaymentTypeForPayments"));
        r.put("jobLots", screens.jobLots());
        r.put("projects", screens.projects());
        r.put("taxTypes", jdbcTemplate.queryForList(
                "EXEC dbo.Sp_TaxesTypes_GetAllMethod @OrganizationId=?, @CompanyId=?, @Type=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), 1, "ReadByCombo"));
        Map<String, Object> flags = flags(mode);
        r.put("flags", flags);
        r.put("branches", Boolean.TRUE.equals(flags.get("branchFeature")) ? screens.branches() : new ArrayList<>());
        r.put("costCenters", Boolean.TRUE.equals(flags.get("isBookingOffice")) ? screens.costCenters() : new ArrayList<>());
        r.put("subsidiaries", Boolean.TRUE.equals(flags.get("subsidiaryFeature")) ? subsidiaries() : new ArrayList<>());
        r.put("instrumentTypes", jdbcTemplate.queryForList("EXEC dbo.usp_getInstrumentType"));
        List<Map<String, Object>> ct = new ArrayList<>();
        for (Map<String, Object> row : jdbcTemplate.queryForList("EXEC [dbo].[sp_ChequeType]")) {   // image column not sent
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("id", ci(row, "id"));
            o.put("CheqType", ci(row, "CheqType"));
            ct.add(o);
        }
        r.put("chequeTypes", ct);
        r.put("nextCode", nextCode(startDoc(mode)));
        return r;
    }

    private static Map<String, Object> typeRow(int id, String name) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("Name", name);
        return o;
    }

    /**
     * DetailAccountFill (:2493): ExpenseAccountAllowOnPaymentVoucher on  -> exclude {2,15} (+{4,12} when
     * InventoryRelatedAccountsShowInVouchers is off); off -> exclude {2,11,12,13,14,15,20,21} (+{4} when the
     * inventory switch is off). PLNoteId 2 is dropped when the inventory switch is off.
     */
    public List<Map<String, Object>> detailAccounts() {
        Map<String, ConfigrationsAllocation> m = config.configMap();
        String exp = VoucherDesktopConfigService.key(m, "ExpenseAccountAllowOnPaymentVoucher");
        boolean expense = exp != null && "true".equals(exp.toLowerCase());
        boolean inventory = VoucherDesktopConfigService.toBool(VoucherDesktopConfigService.key(m, "InventoryRelatedAccountsShowInVouchers"));
        int[] excluded;
        if (!expense) excluded = !inventory ? new int[] {2, 4, 11, 12, 13, 14, 15, 20, 21} : new int[] {2, 11, 12, 13, 14, 15, 20, 21};
        else excluded = !inventory ? new int[] {2, 4, 12, 15} : new int[] {2, 15};
        return screens.globalAccounts(new int[0], excluded, !inventory ? new int[] {2} : new int[0]);
    }

    /** GetSubsidoryAccounts: VoucherHead.BindSubsidiaryAccount(CompanyId, 0, null) - every row, filtered per account on the page. */
    public List<Map<String, Object>> subsidiaries() {
        return screens.subsidiaryAccounts(0, null);
    }

    /** The Load-time switches of the form plus the defaults DefaultConfigurations() applies. */
    public Map<String, Object> flags(int mode) {
        requireMode(mode);
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> f = new LinkedHashMap<>(config.voucherFlags());
        Map<String, ConfigrationsAllocation> m = config.configMap();
        f.put("preventNegativeBalanceEntry", VoucherDesktopConfigService.toBool(VoucherDesktopConfigService.key(m, "is Minus balance Allowed")));
        f.put("displayWarningforNegativeBalance", VoucherDesktopConfigService.toBool(VoucherDesktopConfigService.key(m, "DisplayWarningforNegativeBalance")));
        f.put("disableBothNegativeBalanceRestrictions", VoucherDesktopConfigService.toBool(VoucherDesktopConfigService.key(m, "DisableBothNegativeBalanceRestrictions")));
        f.put("defaultJobLotId", toInt(VoucherDesktopConfigService.key(m, "Job/Lot")));
        f.put("baseCurrencyId", toInt(VoucherDesktopConfigService.key(m, "Base Currency")));
        String rate = VoucherDesktopConfigService.key(m, "BaseCurrencyRate");
        f.put("baseCurrencyRate", rate == null ? null : VoucherDesktopConfigService.toDouble(rate));
        f.put("amountDecimals", toInt(VoucherDesktopConfigService.key(m, "Default NoofDecimal Points For Amount")));
        f.put("rateDecimals", toInt(VoucherDesktopConfigService.key(m, "Default NoofDecimal Points For Rate")));
        f.put("fcyDecimals", toInt(VoucherDesktopConfigService.key(m, "DefaultNoOfDecimalPointsForFcyAmount")));
        f.put("defaultBranchId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        f.put("subsidiaryFeature", config.erpFeature(4));          // SubsidiaryAccountAllownOnVouchers
        f.put("multiCurrencyGrid", config.erpFeature(6));          // MultiCurrencyGridWorking - not ported (save refused)
        f.put("defaultSrbAccountId", toInt(VoucherDesktopConfigService.key(m, "DefaultRevenueBoardAccount")));
        f.put("defaultDiscountAccountId", toInt(VoucherDesktopConfigService.key(m, "DefaultDiscountAccount")));
        f.put("isBookingOffice", screens.isBookingOffice());
        f.put("mode", mode);
        f.put("screenName", screenName(mode));
        f.put("rights", rights(mode));
        return f;
    }

    /** CommonServices.GenerateVoucherCode(doc). */
    public int nextCode(int doc) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return writer.nextVoucherCode(u.getOrganizationId(), u.getCompanyId(), doc,
                currentUserContext.currentFinancialYearId(), u.getBranchesId() == null ? 0 : u.getBranchesId());
    }

    /** GetAccountBalance: ReadByCurrentBalanceByDateAndAccountId, Balance column. */
    public double balance(int accountId, String date) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return writer.accountBalance(u.getOrganizationId(), u.getCompanyId(),
                currentUserContext.currentFinancialYearId(), isoDay(date), accountId);
    }

    public double subsidiaryBalance(int glAccountId, int subsidiaryId, String date) {
        return screens.subsidiaryBalance(glAccountId, subsidiaryId, isoDay(date));
    }

    /** Sp_TaxSchedule_GetAllMehtod 'GetTaxPercentInTaxSchedule' - first row or null. */
    public Map<String, Object> taxSchedule(int taxTypeId, String date) {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC dbo.Sp_TaxSchedule_GetAllMehtod @OrganizationId=?, @CompanyId=?, @EffectedDate=?, @TaxNameId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), isoDay(date), taxTypeId, "GetTaxPercentInTaxSchedule");
        if (rows.isEmpty()) return null;
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("taxPercent", ci(rows.get(0), "TaxPercent"));
        o.put("taxGLAccountId", ci(rows.get(0), "TaxGLAccountId"));
        o.put("accountTitle", ci(rows.get(0), "AccountTitle"));
        return o;
    }

    // ================================================================================ history

    /**
     * HistoryFillCpv / HistoryFillBpv -> VoucherHead.VoucherFormHistory -> USP_VoucherFormHistory with
     * vh.Ids = "1" / "2" (each tab its own single document type), CanViewAllRecord = the screen's
     * "CanView AllRecord" right, EntryUser only when that right is off, AppId when != 0, IsApproved
     * unless "All". Failure is reported, never replaced by rows from another source.
     */
    public Map<String, Object> history(int mode, int doc, String dateType, String fromDate, String toDate,
                                       Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus) {
        requireDoc(mode, doc);
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean viewAll = hasRight(mode, "CanView AllRecord");
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_VoucherFormHistory @OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeName=?, @CanViewAllRecord=?");
        List<Object> a = new ArrayList<>();
        a.add(u.getOrganizationId()); a.add(u.getCompanyId()); a.add(u.getId()); a.add(String.valueOf(doc)); a.add(viewAll);
        int year = currentUserContext.currentFinancialYearId();
        if (year != 0) { sql.append(", @FinancialYearId=?"); a.add(year); }
        String from = isoDay(fromDate), to = isoDay(toDate);
        String fp, tp;
        if ("entrydate".equalsIgnoreCase(dateType)) { fp = "@EntryFromDate"; tp = "@EntryToDate"; }
        else if ("modifydate".equalsIgnoreCase(dateType)) { fp = "@ModifyFromDate"; tp = "@ModifyToDate"; }
        else if ("approveddate".equalsIgnoreCase(dateType)) { fp = "@ApprovedFromDate"; tp = "@ApprovedToDate"; }
        else { fp = "@FromDate"; tp = "@ToDate"; }
        if (from != null) { sql.append(", ").append(fp).append("=?"); a.add(from); }
        if (to != null) { sql.append(", ").append(tp).append("=?"); a.add(to); }
        if (nz(fromDocNo) != 0) { sql.append(", @DocNoFrom=?"); a.add(fromDocNo); }
        if (nz(toDocNo) != 0) { sql.append(", @DocNoTo=?"); a.add(toDocNo); }
        if (nz(accountId) != 0) { sql.append(", @AccountId=?"); a.add(accountId); }
        if (!viewAll) { sql.append(", @EntryUser=?"); a.add(u.getId()); }
        if (!"all".equalsIgnoreCase(approvedStatus)) { sql.append(", @IsApproved=?"); a.add("approved".equalsIgnoreCase(approvedStatus)); }
        int appId = u.getAppId() == null ? 0 : u.getAppId();
        if (appId != 0) { sql.append(", @AppId=?"); a.add(appId); }

        List<Map<String, Object>> raw = jdbcTemplate.queryForList(sql.toString(), a.toArray());
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("id", ci(r, "Id"));
            o.put("documentTypeId", ci(r, "DocumentTypeId"));
            o.put("voucherDate", dateStr(ci(r, "VoucherDate")));
            o.put("voucherCode", ci(r, "VoucherCode"));
            o.put("documentType", ci(r, "DocumentTypeCode"));
            if (doc == 2) o.put("chequeNo", ci(r, "ChequeNo"));
            o.put("accountId", ci(r, "RefAccountId"));
            o.put("accountTitle", ci(r, "AccountTitle"));
            o.put("voucherAmount", ci(r, "VoucherAmount"));
            o.put("fcyCode", ci(r, "Currencycode"));
            o.put("exchangeRate", ci(r, "ExchangeCurrencyRate"));
            o.put("fcyAmount", ci(r, "FcAmount"));
            o.put("remarks", ci(r, "Remarks"));
            o.put("entryUser", ci(r, "UserName"));
            o.put("entryDate", dateTimeStr(ci(r, "EntryDate")));
            o.put("modifyUser", ci(r, "ModifyUserName"));
            o.put("modifyDate", dateTimeStr(ci(r, "ModifyDate")));
            o.put("approvedUser", ci(r, "ApprovedUserName"));
            o.put("approvedDate", dateTimeStr(ci(r, "PostDate")));
            o.put("attachment", ci(r, "NoOfAttachments"));
            rows.add(o);
        }
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("rows", rows);
        res.put("totalVouchers", raw.isEmpty() ? 0 : toInt(ci(raw.get(0), "TotalVouchers")));
        res.put("totalApprovedVoucher", raw.isEmpty() ? 0 : toInt(ci(raw.get(0), "TotalApprovedVoucher")));
        res.put("totalUnApprovedVoucher", raw.isEmpty() ? 0 : toInt(ci(raw.get(0), "TotalUnApprovedVoucher")));
        res.put("multiCurrencyFeature", config.erpFeature(6));
        return res;
    }

    /** ComboBindForCpvHistory / Bpv: Sp_Vouchers_LedgerByJobLot_DropDownAndLists, rows with ActivityType "Account Header". */
    public List<Map<String, Object>> historyAccounts(int mode, int doc) {
        requireDoc(mode, doc);
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC dbo.Sp_Vouchers_LedgerByJobLot_DropDownAndLists @OrganizationId=?, @CompanyId=?, @AppId=?, @UserId=?, @DocumentTypeIds=?",
                u.getOrganizationId(), u.getCompanyId(), u.getAppId() == null ? 0 : u.getAppId(), u.getId(), String.valueOf(doc))) {
            if (!"Account Header".equals(str(ci(r, "ActivityType")))) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("name", ci(r, "name"));
            out.add(o);
        }
        return out;
    }

    // ================================================================================ load

    private List<Map<String, Object>> head(int id) {
        return jdbcTemplate.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @Id=?, @Activity=?", id, "ReadByID");
    }

    /** The head row when it is this tenant's and a payment voucher this screen may show, otherwise null. */
    private Map<String, Object> ownHead(int mode, int id) {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> heads = head(id);
        if (heads.isEmpty()) return null;
        Map<String, Object> h = heads.get(0);
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId() || toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        int doc = toInt(ci(h, "DocumentTypeId"));
        if (doc != 1 && doc != 2) return null;
        if (mode != 0 && doc != mode) return null;
        Object base = ci(h, "BaseDocumentTypeId");
        if (base != null && toInt(base) != 1) return null;     // a non-tax voucher belongs to the non-tax screen
        return h;
    }

    /**
     * ReadById(): VoucherHead.GetByID. Grid rows are the IsTaxable "False" lines whose account is not the
     * header account (the mirror of each pair drops out) - the discount debit line, whose IsTaxable is never
     * set by Insert(), is left out by its DiscountAmount. WHT / SRB / Discount controls come from the tax lines.
     */
    public Map<String, Object> load(int mode, int id) {
        requireMode(mode);
        Map<String, Object> h = ownHead(mode, id);
        if (h == null) return null;
        List<Map<String, Object>> details = jdbcTemplate.queryForList(
                "EXEC dbo.Sp_Vouchers_GetMethods @Id=?, @Activity=?", id, "VoucherDetail_ReadByVoucherHeadID");
        Map<String, Object> flags = config.voucherFlags();
        boolean autoPay = Boolean.TRUE.equals(flags.get("autoRemarksForPaymentThroughBank"));
        int hDoc = toInt(ci(h, "DocumentTypeId"));
        int refAccountId = toInt(ci(h, "RefAccountId"));
        boolean inclusive = truthy(ci(h, "InclusiveTax"));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", toInt(ci(h, "Id")));
        out.put("documentTypeId", hDoc);
        out.put("voucherCode", ci(h, "VoucherCode"));
        out.put("voucherDate", dateStr(ci(h, "VoucherDate")));
        out.put("refAccountId", refAccountId);
        out.put("projectId", ci(h, "ProjectId"));
        out.put("multiCurrencyId", ci(h, "MultiCurrencyId"));
        out.put("exchangeCurrencyRate", ci(h, "ExchangeCurrencyRate"));
        out.put("fcAmount", ci(h, "FcAmount"));
        out.put("voucherAmount", ci(h, "VoucherAmount"));
        out.put("isApproved", truthy(ci(h, "IsApproved")));
        out.put("inclusiveTax", inclusive);
        out.put("customAccounts", truthy(ci(h, "CustomAccounts")));
        out.put("refDocNoId", ci(h, "RefDocNoId"));
        String hRemarks = str(ci(h, "Remarks")), hOther = (String) ci(h, "RemarksOtherLingo");
        out.put("remarks", hDoc != 2 ? hRemarks : (autoPay ? hOther : (hOther != null ? hOther : hRemarks)));
        out.put("locationTypeId", details.isEmpty() ? 0 : toInt(ci(details.get(0), "LocationTypeId")));

        List<Map<String, Object>> rows = new ArrayList<>();
        Map<String, Object> wht = null, srb = null, disc = null;
        boolean lock = false;
        for (Map<String, Object> d : details) {
            String taxable = str(ci(d, "IsTaxable"));
            int acc = toInt(ci(d, "AccountId"));
            double debit = toDouble(ci(d, "DebitAmount")), credit = toDouble(ci(d, "CreditAmount"));
            double tax = toDouble(ci(d, "TaxAmount"));
            if (toInt(ci(d, "InvoiceNoRefId")) > 0) lock = true;
            boolean taxLine = toDouble(ci(d, "TaxesTotalAmount")) > 0 || toDouble(ci(d, "SBRTaxAmount")) > 0 || toDouble(ci(d, "DiscountAmount")) > 0;
            if (!"True".equals(taxable) && !taxLine && acc != refAccountId) {
                Map<String, Object> r = new LinkedHashMap<>();
                int st = toInt(ci(d, "SubsidiaryTypeId"));
                r.put("paymentTypeId", ci(d, "PaymentTypeId"));
                r.put("paymentType", ci(d, "PaymentType"));
                r.put("accountId", acc);
                r.put("accountCode", ci(d, "AccountCode"));
                r.put("accountTitle", ci(d, "AccountTitle"));
                r.put("subsidiaryAccountId", st == 1 ? ci(d, "SupplierCustomerId") : st == 2 ? ci(d, "EmployeeId") : (st == 3 || st == 4) ? ci(d, "SubsidiaryAccountId") : 0);
                r.put("subsidiaryAccountTitle", ci(d, "SubsidiaryAccountTitle"));
                r.put("subsidiaryAccountTypeId", st);
                r.put("jobLotId", ci(d, "JobLotId"));
                String c = (String) ci(d, "Comments"), co = (String) ci(d, "CommentsOtherLingo");
                r.put("remarks", hDoc != 2 ? c : (autoPay ? co : (co != null ? co : c)));
                r.put("tcyCodeId", ci(d, "DMultiCurrencyId"));
                r.put("tcyCode", ci(d, "TcyCode"));
                r.put("tcyExchangeRate", ci(d, "DExchangeCurrencyRate"));
                r.put("fcyAmount", ci(d, "DCurrencyAmount"));
                r.put("amount", (inclusive && debit > 0) ? debit + tax : debit);
                r.put("referenceAccountId", ci(d, "ReferenceAccountId"));
                r.put("referenceAccount", ci(d, "ReferenceAccount"));
                r.put("financialInstrumentId", ci(d, "InstrumentTypeId"));
                r.put("financialInstrument", ci(d, "InstrumentType"));
                r.put("chequeDate", dateStr(ci(d, "DCheqDate")));
                r.put("chequeId", ci(d, "InvoiceNoRefId"));
                r.put("chequeNo", ci(d, "CheqNoDetail"));
                r.put("payeeTitle", ci(d, "PayeeTitle"));
                r.put("chequeTypeId", ci(d, "ChequeTypeId"));
                r.put("branchId", ci(d, "BranchesId"));
                r.put("branchName", ci(d, "BranchName"));
                r.put("costCenterId", ci(d, "CostCenterId"));
                r.put("taxAmount", tax);
                rows.add(r);
            }
            /* WHT: the credit-side tax line (IsTaxable True, TaxesTotalAmount > 0, TaxTypeId > 0, credit > 0). */
            if (wht == null && "True".equals(taxable) && toDouble(ci(d, "TaxesTotalAmount")) > 0 && toInt(ci(d, "TaxTypeId")) > 0 && credit > 0) {
                wht = new LinkedHashMap<>();
                wht.put("againstAcId", ci(h, "RefDocNoId"));
                wht.put("withHoldingAcId", acc);
                wht.put("withHoldingAcTitle", ci(d, "AccountTitle"));
                wht.put("taxTypeId", ci(d, "TaxTypeId"));
                wht.put("taxPercent", ci(d, "TaxPrcnt"));
                wht.put("taxAmount", ci(d, "TaxesTotalAmount"));
            }
            if (srb == null && toDouble(ci(d, "SBRTaxAmount")) > 0 && credit > 0) {
                srb = new LinkedHashMap<>();
                srb.put("accountId", acc);
                srb.put("amount", ci(d, "SBRTaxAmount"));
            }
            if (disc == null && toDouble(ci(d, "DiscountAmount")) > 0 && credit > 0) {
                disc = new LinkedHashMap<>();
                disc.put("accountId", acc);
                disc.put("percent", ci(d, "DiscountPercent"));
                disc.put("amount", ci(d, "DiscountAmount"));
            }
        }
        out.put("rows", rows);
        out.put("includeWHT", wht != null);
        out.put("wht", wht);
        out.put("srb", srb);
        out.put("discount", disc);
        out.put("lockHeader", lock);        // any line with InvoiceNoRefId > 0 disables type + credit account
        return out;
    }

    /** History detail grid: every stored line of the voucher. */
    public Map<String, Object> lines(int mode, int id) {
        requireMode(mode);
        Map<String, Object> h = ownHead(mode, id);
        if (h == null) return null;
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : jdbcTemplate.queryForList(
                "EXEC dbo.Sp_Vouchers_GetMethods @Id=?, @Activity=?", id, "VoucherDetail_ReadByVoucherHeadID")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("id", toInt(ci(d, "Id")));
            o.put("accountCode", ci(d, "AccountCode"));
            o.put("accountTitle", ci(d, "AccountTitle"));
            o.put("subsidiaryAccount", ci(d, "SubsidiaryAccountTitle"));
            o.put("jobLot", ci(d, "JobLotDescription"));
            o.put("remarks", ci(d, "Comments"));
            o.put("debit", toDouble(ci(d, "DebitAmount")));
            o.put("credit", toDouble(ci(d, "CreditAmount")));
            o.put("fcy", toDouble(ci(d, "DCurrencyAmount")));
            o.put("referenceAccount", ci(d, "ReferenceAccount"));
            o.put("branchName", ci(d, "BranchName"));
            o.put("costCenter", ci(d, "CostCenterName"));
            o.put("taxAmount", toDouble(ci(d, "TaxAmount")));
            o.put("isTaxable", str(ci(d, "IsTaxable")));
            o.put("chequeDate", dateStr(ci(d, "DCheqDate")));
            o.put("chequeNo", ci(d, "CheqNoDetail"));
            o.put("payeeTitle", ci(d, "PayeeTitle"));
            rows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", toInt(ci(h, "Id")));
        out.put("documentTypeId", toInt(ci(h, "DocumentTypeId")));
        out.put("inclusiveTax", truthy(ci(h, "InclusiveTax")));
        out.put("rows", rows);
        return out;
    }

    /** cmbCurrency_Leave: Sp_Vouchers_GetMethods 'GetMultiCurrencyAndLastRate', LastExchRate; no row -> 0. */
    public double lastRate(int mode, int doc, int currencyId) {
        requireDoc(mode, doc);
        UserAccount u = currentUserContext.requireAccountingUser();
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @DocumentTypeIds=?");
        List<Object> a = new ArrayList<>();
        a.add(u.getOrganizationId()); a.add(u.getCompanyId()); a.add(String.valueOf(doc));
        if (currencyId != 0) { sql.append(", @DMultiCurrencyIds=?"); a.add(String.valueOf(currencyId)); }
        sql.append(", @Activity=?"); a.add("GetMultiCurrencyAndLastRate");
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(sql.toString(), a.toArray());
        return rows.isEmpty() ? 0d : toDouble(ci(rows.get(0), "LastExchRate"));
    }

    // ================================================================================ Total()

    /** Total() results: txtValue / txtTaxAmount / txtTotalAmount (rounded to the amount decimals) and the per-row TaxAmount. */
    public static final class Totals {
        public double base, value, tax, total;
        public double[] rowTax;
    }

    /**
     * Total() + TaxAmountProportion() (:3278 / :5962). Excluded: tax = (base+srb+disc) / (100-rate) * rate,
     * total = base+tax+srb+disc, value = base. Included: n = base+srb+disc, value = n/100*(100-rate),
     * total = n, tax = n-value. The texts are formatted with the amount decimals; the per-row tax exists
     * only in Included mode (rounded away from zero).
     */
    public static Totals total(PaymentTaxVoucherDto dto, int decimals) {
        Totals t = new Totals();
        int n = dto.rows == null ? 0 : dto.rows.size();
        t.rowTax = new double[n];
        double rate = Boolean.TRUE.equals(dto.IncludeWHT) ? toDouble(dto.TaxPercent) : 0d;
        double srb = nzd(dto.SrbAmount), disc = nzd(dto.DiscountAmount);
        double base = 0d;
        for (int i = 0; i < n; i++) base += nzd(dto.rows.get(i).Amount);
        t.base = base;
        if (base <= 0d) return t;
        double tax, tot, val = base;
        if (!Boolean.TRUE.equals(dto.InclusiveTax)) {
            tax = (base + srb + disc) / (100d - rate) * rate;
            tot = base + tax + srb + disc;
        } else {
            double nn = base + srb + disc;
            val = nn / 100d * (100d - rate);
            tot = nn;
            tax = tot - val;
        }
        t.value = round(val, decimals);
        t.tax = round(tax, decimals);
        t.total = round(tot, decimals);
        if (Boolean.TRUE.equals(dto.InclusiveTax) && t.tax > 0d) {
            for (int i = 0; i < n; i++) t.rowTax[i] = round(t.tax / base * nzd(dto.rows.get(i).Amount), decimals);
        }
        return t;
    }

    // ================================================================================ save

    @Transactional
    public Map<String, Object> save(int mode, PaymentTaxVoucherDto dto) {
        requireMode(mode);
        int doc = nz(dto.DocumentTypeId);
        requireDoc(mode, doc);
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean insert = dto.Id == null || dto.Id <= 0;
        if (insert && !hasRight(mode, "Save")) throw new SecurityException("You don't have Save right on this screen.");
        if (!insert && !hasRight(mode, "Update")) throw new SecurityException("You don't have Update right on this screen.");
        if (!insert) {
            Map<String, Object> existing = ownHead(mode, dto.Id);
            if (existing == null || toInt(ci(existing, "DocumentTypeId")) != doc) {
                throw new IllegalArgumentException("Record Not Update  " + dto.Id);
            }
        }
        int org = u.getOrganizationId(), comp = u.getCompanyId();
        int branchId = u.getBranchesId() == null ? 0 : u.getBranchesId();
        int year = currentUserContext.currentFinancialYearId();
        Map<String, Object> f = flags(mode);
        if (Boolean.TRUE.equals(f.get("multiCurrencyGrid"))) {
            throw new IllegalArgumentException("The Multi Currency grid (ERP feature 6) of this voucher is not available on the web yet. Please use the desktop for this company.");
        }
        boolean branchFeature = Boolean.TRUE.equals(f.get("branchFeature"));
        boolean subsidiaryFeature = Boolean.TRUE.equals(f.get("subsidiaryFeature"));
        int decimals = toInt(f.get("amountDecimals"));
        Set<String> ack = new HashSet<>(dto.acknowledged == null ? new ArrayList<String>() : dto.acknowledged);

        Map<Integer, String> headerTitles = new LinkedHashMap<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Activity=?",
                org, comp, doc, "GetDetailAccountsByDocumentTypeId")) {
            headerTitles.put(toInt(ci(r, "Id")), str(ci(r, "AccountTitle")));
        }
        Map<Integer, String> detailTitles = titles(detailAccounts());
        Map<Integer, String> paymentTypes = new LinkedHashMap<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList("EXEC [Account].[USP_lookUp_GetAllMethod] @Activity=?",
                "GetDataPaymentTypeForPayments")) {
            paymentTypes.put(toInt(ci(r, "Id")), str(ci(r, "PaymentType")));
        }
        Map<Integer, String> whtTitles = titles(screens.globalAccounts(new int[0], new int[] {2, 11, 15}, new int[0]));
        Map<Integer, String> srbTitles = titles(screens.globalAccounts(new int[] {6, 8}, new int[0], new int[0]));
        Map<Integer, String> discTitles = titles(screens.globalAccounts(new int[] {11, 13, 20, 21, 22}, new int[0], new int[0]));
        Map<Integer, String> taxNames = new LinkedHashMap<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC dbo.Sp_TaxesTypes_GetAllMethod @OrganizationId=?, @CompanyId=?, @Type=?, @Activity=?", org, comp, 1, "ReadByCombo")) {
            taxNames.put(toInt(ci(r, "Id")), str(ci(r, "TaxName")));
        }
        Map<Integer, String> currencies = new LinkedHashMap<>();
        for (Map<String, Object> r : screens.currenciesWithRate()) currencies.put(toInt(ci(r, "Id")), str(ci(r, "CurrencyCode")));
        Map<Integer, String> locations = new LinkedHashMap<>();
        for (Map<String, Object> r : screens.locationTypes()) locations.put(toInt(ci(r, "Id")), str(ci(r, "Location")));

        Totals totals = total(dto, decimals);
        validate(dto, f, doc, totals, headerTitles, detailTitles, paymentTypes, whtTitles, srbTitles, discTitles,
                taxNames, currencies, locations, branchFeature, subsidiaryFeature, ack);

        String voucherDate = isoDay(dto.VoucherDate);
        String now = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date());
        int voucherCode = nz(dto.VoucherCode);
        if (insert && voucherCode <= 0) voucherCode = nextCode(doc);

        boolean inclusive = Boolean.TRUE.equals(dto.InclusiveTax);
        boolean wht = Boolean.TRUE.equals(dto.IncludeWHT);
        boolean autoRemarks = Boolean.TRUE.equals(f.get("autoRemarksForPaymentThroughBank"));
        double rate = nzd(dto.ExchangeCurrencyRate);

        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        vh.Id = insert ? 0 : dto.Id;
        vh.BaseDocumentTypeId = 1;
        vh.DocumentTypeId = doc;
        vh.ProjectId = nz(dto.ProjectId);
        vh.VoucherCode = voucherCode;
        vh.VoucherDate = voucherDate;
        vh.RefAccountId = nz(dto.RefAccountId);
        vh.AgainstAccountId = nz(dto.WithHoldingAcId);     // CmbWithHoldingAc
        vh.RefDocNoId = nz(dto.AgainstAcId);               // CmbAgainstAc
        vh.Remarks = dto.Remarks == null ? "" : dto.Remarks;   // txtremarksmain.Text (untrimmed)
        vh.PayTitle = "";                                    // txtPayTitle is cleared after every Add
        vh.IncludeWHT = wht;
        vh.BranchId = branchId;
        vh.OrganizationId = org;
        vh.CompanyId = comp;
        vh.FinancialYearId = year;
        vh.EntryUser = u.getId();
        vh.ModifyUser = u.getId();
        vh.EntryDate = now;
        vh.ModifyDate = now;
        vh.MultiCurrencyId = nz(dto.MultiCurrencyId);
        vh.ExchangeCurrencyRate = rate;
        vh.FcAmount = nzd(dto.FcAmount);
        vh.CustomAccounts = Boolean.TRUE.equals(dto.CustomAccounts);
        vh.InclusiveTax = inclusive;

        List<ContraVoucherDto.Detail> details = new ArrayList<>();
        List<ContraVoucherDto.CostCentre> costCentres = new ArrayList<>();
        List<PaymentTaxVoucherDto.Row> rows = dto.rows;
        int count = rows.size();
        String headerTitle = headerTitles.getOrDefault(nz(dto.RefAccountId), "").trim();
        int locationId = nz(dto.LocationTypeId);
        int lineId = 1;
        double voucherAmt = 0d;

        for (int i = 0; i < count; i++) {
            PaymentTaxVoucherDto.Row r = rows.get(i);
            double amount = nzd(r.Amount);
            String rowRemarks = r.Remarks == null ? "" : r.Remarks;
            String chequeNo = r.ChequeNo == null ? "" : r.ChequeNo;
            String payee = r.PayeeTitle == null ? "" : r.PayeeTitle;
            String chequeDate = isoDay(r.ChequeDate);
            String rowTitle = detailTitles.getOrDefault(nz(r.AccountId), "");
            String ptText = paymentTypes.getOrDefault(nz(r.PaymentTypeId), "");
            int branch = branchFeature ? nz(r.BranchId) : branchId;
            String remarksAuto = "";

            ContraVoucherDto.Detail vd = new ContraVoucherDto.Detail();
            int subType = nz(r.SubsidiaryAccountTypeId);
            int subId = nz(r.SubsidiaryAccountId);
            vd.SubsidiaryTypeId = subType;
            if (subId > 0) {
                vd.SupplierCustomerId = subType == 1 ? subId : 0;
                vd.EmployeeId = subType == 2 ? subId : 0;
                vd.SubsidiaryAccountId = subId;
            }
            if (doc == 2) {
                if (count == 1) {
                    vh.CheqId = nz(r.ChequeId);
                    vh.ChequeNo = chequeNo;
                    vh.ChequeDate = chequeDate;
                    vh.PayTitle = payee;
                }
                if (autoRemarks) {
                    remarksAuto = autoRemarkText(f, r, chequeNo, chequeDate, payee, headerTitle, rowTitle);
                    String h0 = vh.Remarks;
                    boolean containsSub = h0.contains("CHEQUE DATE") || h0.contains("CHEQUE NO") || h0.contains("PayTitle") || h0.contains("Credit Account");
                    boolean incHeader = Boolean.TRUE.equals(f.get("autoRemarksIncludeHeaderRemarks"));
                    boolean incDetail = Boolean.TRUE.equals(f.get("autoRemarksIncludeDetailRemarks"));
                    if (incHeader && incDetail) remarksAuto = (containsSub ? "" : h0) + " " + remarksAuto + " " + rowRemarks;
                    else if (incHeader) remarksAuto = (containsSub ? "" : h0) + " " + remarksAuto;
                    else if (incDetail) remarksAuto = remarksAuto + " " + rowRemarks;
                    vd.Comments = remarksAuto;
                    vd.CommentsOtherLingo = rowRemarks;
                    if (count == 1) {
                        vh.RemarksOtherLingo = !vh.Remarks.isEmpty() ? vh.Remarks : "";
                        vh.Remarks = remarksAuto;
                    }
                } else if (!chequeNo.isEmpty()) {
                    vd.Comments = "Cheq/Ref#: " + chequeNo + ", " + rowRemarks;
                    vd.CommentsOtherLingo = rowRemarks;
                } else {
                    vd.Comments = rowRemarks;
                    vd.CommentsOtherLingo = rowRemarks;
                }
            } else {
                vd.Comments = rowRemarks;
            }
            if (nz(r.PaymentTypeId) == 1) vd.AdvanceAmount = amount;
            vd.LineId = lineId;
            vd.SortNo = i + 1;
            vd.PaymentTypeId = nz(r.PaymentTypeId);
            vd.PaymentType = ptText;
            vd.AccountId = nz(r.AccountId);
            vd.AgainstAccountId = nz(dto.RefAccountId);
            vd.JobLotId = nz(r.JobLotId);
            vd.ReferenceAccountId = nz(r.ReferenceAccountId);
            vd.TaxAmount = totals.rowTax[i];
            vd.DebitAmount = inclusive ? amount - vd.TaxAmount : amount;
            voucherAmt += vd.DebitAmount;
            vd.IsTaxable = "False";
            vd.InvoiceNoRefId = nz(r.ChequeId);
            vd.InstrumentTypeId = nz(r.FinancialInstrumentId);
            vd.CheqNoDetail = chequeNo;
            vd.DCheqDate = chequeDate;
            vd.PayeeTitle = payee;
            vd.ChequeTypeId = nz(r.ChequeTypeId);
            vd.BranchesId = branch;
            vd.DMultiCurrencyId = nz(r.TcyCodeId);
            vd.DExchangeCurrencyRate = nzd(r.TcyExchangeRate);
            vd.DCurrencyAmount = nzd(r.FcyAmount);
            vd.CostCenterId = nz(r.CostCenterId);
            if (count == 1 && vh.Remarks.trim().isEmpty()) {
                vh.RemarksOtherLingo = null;
                vh.Remarks = vd.Comments == null ? "" : vd.Comments;
            }
            if (!insert) vd.ActionId = 2;
            vd.LocationTypeId = locationId;
            details.add(vd);
            lineId++;

            ContraVoucherDto.Detail vd2 = new ContraVoucherDto.Detail();
            String hTrim = vh.Remarks.trim();
            if (doc == 2) {
                if (autoRemarks) {
                    vd2.Comments = remarksAuto;
                    vd2.CommentsOtherLingo = rowRemarks;
                } else if (!hTrim.isEmpty()) {
                    vd2.Comments = hTrim;
                    vd2.CommentsOtherLingo = vd.CommentsOtherLingo;
                } else {
                    vd2.Comments = vd.Comments;
                    vd2.CommentsOtherLingo = rowRemarks;
                }
            } else if (!hTrim.isEmpty()) {
                vd2.Comments = hTrim;
            } else {
                vd2.Comments = rowRemarks;
            }
            if (nz(r.PaymentTypeId) == 1) vd2.AdvanceAmount = amount;
            vd2.SubsidiaryAgainstTypeId = subType;
            if (subId > 0) vd2.SubsidiaryAgainstAccountId = subId;
            vd2.LineId = lineId;
            vd2.SortNo = i + 1;
            vd2.PaymentTypeId = nz(r.PaymentTypeId);
            vd2.PaymentType = ptText;
            vd2.AccountId = nz(dto.RefAccountId);
            vd2.AgainstAccountId = nz(r.AccountId);
            vd2.JobLotId = nz(r.JobLotId);
            vd2.TaxAmount = totals.rowTax[i];
            vd2.CreditAmount = inclusive ? amount - vd2.TaxAmount : amount;
            vd2.DebitAmount = 0d;
            vd2.ReferenceAccountId = nz(r.ReferenceAccountId);
            vd2.IsTaxable = "False";
            vd2.InvoiceNoRefId = nz(r.ChequeId);
            vd2.InstrumentTypeId = nz(r.FinancialInstrumentId);
            vd2.ChequeTypeId = nz(r.ChequeTypeId);
            vd2.BranchesId = branch;
            vd2.CheqNoDetail = chequeNo;
            vd2.DCheqDate = chequeDate;
            vd2.PayeeTitle = payee;
            vd2.DMultiCurrencyId = nz(r.TcyCodeId);
            vd2.DExchangeCurrencyRate = nzd(r.TcyExchangeRate);
            vd2.DCurrencyAmount = nzd(r.FcyAmount);
            vd2.CostCenterId = nz(r.CostCenterId);
            vd2.LocationTypeId = locationId;
            details.add(vd2);
            lineId++;

            if (nz(r.CostCenterId) > 0) {
                ContraVoucherDto.CostCentre c = new ContraVoucherDto.CostCentre();
                c.SortNo = i + 1;
                c.CostCenterId = nz(r.CostCenterId);
                c.costPrcent = new BigDecimal("100");
                c.costAmount = amount;                    // the full row Amount, as the desktop sends it
                costCentres.add(c);
            }
        }

        int firstAccount = details.get(0).AccountId;
        if (wht) {
            String pct = nzs(dto.TaxPercent).trim();
            String comments = "Withholding Tax deducted under " + taxNames.getOrDefault(nz(dto.TaxTypeId), "") + " at " + pct + "% rate.";
            double fcy = rate > 0 ? roundEven(totals.tax / rate, decimals) : 0d;
            ContraVoucherDto.Detail vd3 = new ContraVoucherDto.Detail();
            vd3.LineId = lineId + 1;
            vd3.AccountId = vh.RefDocNoId;
            vd3.AgainstAccountId = vh.AgainstAccountId;
            vd3.TaxTypeId = nz(dto.TaxTypeId);
            vd3.IsTaxable = "True";
            vd3.Comments = comments;
            vd3.TaxPrcnt = toDouble(pct);
            vd3.TaxesTotalAmount = totals.tax;
            vd3.WhtHolding = totals.tax;
            vd3.DebitAmount = totals.tax;
            vd3.DCurrencyAmount = fcy;
            details.add(vd3);
            ContraVoucherDto.Detail vd4 = new ContraVoucherDto.Detail();
            vd4.LineId = lineId + 1;
            vd4.AccountId = vh.AgainstAccountId;
            vd4.AgainstAccountId = vh.RefDocNoId;
            vd4.TaxTypeId = nz(dto.TaxTypeId);
            vd4.IsTaxable = "True";
            vd4.Comments = comments;
            vd4.TaxPrcnt = toDouble(pct);
            vd4.TaxesTotalAmount = totals.tax;
            vd4.WhtHolding = totals.tax;
            vd4.CreditAmount = totals.tax;
            vd4.DCurrencyAmount = fcy;
            details.add(vd4);
            voucherAmt += vd4.CreditAmount;
        }
        double srb = nzd(dto.SrbAmount);
        if (nz(dto.SrbAccountId) > 0 && srb > 0) {
            String comments = "SBR Tax deducted under " + srbTitles.getOrDefault(nz(dto.SrbAccountId), "");
            double fcy = rate > 0 ? roundEven(srb / rate, decimals) : 0d;
            ContraVoucherDto.Detail vd5 = new ContraVoucherDto.Detail();
            vd5.LineId = lineId + 1;
            vd5.AccountId = firstAccount;
            vd5.AgainstAccountId = nz(dto.SrbAccountId);
            vd5.TaxesTotalAmount = srb;
            vd5.SBRTaxAmount = srb;
            vd5.IsTaxable = "True";
            vd5.Comments = comments;
            vd5.DebitAmount = srb;
            vd5.DCurrencyAmount = fcy;
            details.add(vd5);
            ContraVoucherDto.Detail vd6 = new ContraVoucherDto.Detail();
            vd6.LineId = lineId + 1;
            vd6.AccountId = nz(dto.SrbAccountId);
            vd6.AgainstAccountId = firstAccount;
            vd6.TaxesTotalAmount = srb;
            vd6.SBRTaxAmount = srb;
            vd6.IsTaxable = "True";
            vd6.Comments = comments;
            vd6.CreditAmount = srb;
            vd6.DCurrencyAmount = fcy;
            details.add(vd6);
            voucherAmt += vd6.CreditAmount;
        }
        double disc = nzd(dto.DiscountAmount);
        if (nz(dto.DiscountAccountId) > 0 && disc > 0) {
            String discText = dto.DiscountAmountText != null && !dto.DiscountAmountText.trim().isEmpty() ? dto.DiscountAmountText.trim() : num(disc);
            double original = totals.value + disc;
            String comments = "Original payable amount was " + num(original) + ".Discount of " + discText
                    + " agreed upon mutual discussion. Final payment made: " + fmt(totals.value, decimals);
            double fcy = rate > 0 ? roundEven(disc / rate, decimals) : 0d;
            double pct = nzd(dto.DiscountPercent);
            ContraVoucherDto.Detail vd7 = new ContraVoucherDto.Detail();
            vd7.LineId = lineId + 1;
            vd7.AccountId = firstAccount;
            vd7.AgainstAccountId = nz(dto.DiscountAccountId);
            vd7.DiscountPercent = pct;
            vd7.DiscountAmount = disc;
            vd7.DebitAmount = disc;
            vd7.Comments = comments;
            vd7.DCurrencyAmount = fcy;
            details.add(vd7);
            ContraVoucherDto.Detail vd8 = new ContraVoucherDto.Detail();
            vd8.LineId = lineId + 1;
            vd8.AccountId = nz(dto.DiscountAccountId);
            vd8.AgainstAccountId = firstAccount;
            vd8.DiscountPercent = pct;
            vd8.DiscountAmount = disc;
            vd8.CreditAmount = disc;
            vd8.DCurrencyAmount = fcy;
            vd8.Comments = comments;
            details.add(vd8);
            voucherAmt += vd8.CreditAmount;
        }

        /* Negative balance (Acc = "Bank" for the document type 2, else "Cash"). */
        if (!Boolean.TRUE.equals(f.get("disableBothNegativeBalanceRestrictions"))) {
            double closing = Math.round(writer.accountBalance(org, comp, year, voucherDate, nz(dto.RefAccountId)));
            String acc = doc == 2 ? "Bank" : "Cash";
            if (voucherAmt > closing) {
                String msg = "Debit Amount cannot be greater than " + acc + " Balance.\n" + acc + " Balance is "
                        + num(closing) + " and Total Debit Amount is " + num(voucherAmt) + ".";
                if (Boolean.TRUE.equals(f.get("preventNegativeBalanceEntry"))) throw new IllegalArgumentException(msg);
                if (Boolean.TRUE.equals(f.get("displayWarningforNegativeBalance")) && !ack.contains("negativeBalance")) {
                    throw new ConfirmationRequiredException(msg, "negativeBalance");
                }
            }
        }
        duplicateGuard(details, ack, org, comp, voucherDate);
        vh.VoucherAmount = totals.total;                    // txtTotalAmount
        /* GetGLBalanceByAccountTypeIdClassIdAndAccountId per first line of each account with a plain debit. */
        Set<Integer> unique = new HashSet<>();
        for (ContraVoucherDto.Detail d : details) {
            if (unique.add(nz(d.AccountId)) && nzd(d.DebitAmount) > 0 && nzd(d.TaxesTotalAmount) == 0
                    && nzd(d.SBRTaxAmount) == 0 && nzd(d.DiscountAmount) == 0
                    && !ack.contains("glBalance:" + nz(d.AccountId))) {
                List<Map<String, Object>> g = jdbcTemplate.queryForList(
                        "EXEC [dbo].[USP_GetGLBalanceByAccountTypeIdClassIdAndAccountId] @OrganizationId=?, @CompanyId=?, @AccountId=?, @ToDate=?, @AccountTypeId=?, @AccountClass=?",
                        org, comp, nz(d.AccountId), voucherDate, 3, 3);
                double gl = g.isEmpty() || g.get(0).isEmpty() ? 0d : toDouble(g.get(0).values().iterator().next());
                if (gl > 0) {
                    String title = detailTitles.getOrDefault(nz(d.AccountId), "");
                    throw new ConfirmationRequiredException("The account '" + title
                            + "' already has a debit ledger balance with an amount of " + num(gl)
                            + ". Do you want to add the payment again?", "glBalance:" + nz(d.AccountId));
                }
            }
        }

        int newId = writer.save(vh, details, costCentres);

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", newId);
        res.put("voucherCode", vh.VoucherCode);
        res.put("voucherAmount", vh.VoucherAmount);
        res.put("detailLines", details.size());
        res.put("costCentreLines", costCentres.size());
        res.put("message", (insert ? "Voucher Save Successfully...[" : "Voucher Update Successfully...[") + vh.VoucherCode + "]");
        return res;
    }

    /** The RemarksAuto ternary of Insert() (:1218), before the header / detail remarks are added. */
    private static String autoRemarkText(Map<String, Object> f, PaymentTaxVoucherDto.Row r, String chequeNo,
                                         String chequeDate, String payee, String creditTitle, String rowTitle) {
        boolean dn = Boolean.TRUE.equals(f.get("autoRemarksIncludeChequeDateAndNumber"));
        boolean num = Boolean.TRUE.equals(f.get("autoRemarksIncludeChequeNumber"));
        boolean pay = Boolean.TRUE.equals(f.get("autoRemarksIncludePayeeTitle"));
        String date = shortDate(chequeDate);
        String tail = " Credit Account: " + creditTitle + " PAID TO: " + rowTitle;
        if (dn && pay) return " CHEQUE DATE: " + date + " CHEQUE NO: " + chequeNo + " PayTitle: " + payee + tail;
        if (dn) return " CHEQUE DATE: " + date + " CHEQUE NO: " + chequeNo + tail;
        if (num && pay) return " CHEQUE NO: " + chequeNo + " PayTitle: " + payee + tail;
        if (!pay) return tail;
        return " PayTitle: " + payee + tail;
    }

    /** DateTime.ToShortDateString(), en-US: M/d/yyyy. */
    private static String shortDate(String iso) {
        if (iso == null || iso.length() < 10) return "";
        try {
            return Integer.parseInt(iso.substring(5, 7)) + "/" + Integer.parseInt(iso.substring(8, 10)) + "/" + iso.substring(0, 4);
        } catch (NumberFormatException e) {
            return iso;
        }
    }

    /** BLL VoucherExistWithSameAmountInSameDate: first line of each AccountId among lines with a debit. */
    private void duplicateGuard(List<ContraVoucherDto.Detail> details, Set<String> ack, int org, int comp, String date) {
        if (ack.contains("duplicate")) return;
        Set<Integer> seen = new LinkedHashSet<>();
        for (ContraVoucherDto.Detail d : details) {
            if (nzd(d.DebitAmount) <= 0) continue;
            if (!seen.add(nz(d.AccountId))) continue;
            if (ack.contains("duplicate:" + nz(d.AccountId))) continue;
            String title = writer.duplicateVoucherTitle(org, comp, date, nz(d.AccountId), nzd(d.DebitAmount), nz(d.ActionId));
            if (title != null) {
                throw new ConfirmationRequiredException("Voucher against '" + title
                        + "' with same Debit Amount already exists on this date. Do you want to continue?", "duplicate:" + nz(d.AccountId));
            }
        }
    }

    // ================================================================================ validation (Insert() order)

    private void validate(PaymentTaxVoucherDto dto, Map<String, Object> f, int doc, Totals totals,
                          Map<Integer, String> headerTitles, Map<Integer, String> detailTitles, Map<Integer, String> paymentTypes,
                          Map<Integer, String> whtTitles, Map<Integer, String> srbTitles, Map<Integer, String> discTitles,
                          Map<Integer, String> taxNames, Map<Integer, String> currencies, Map<Integer, String> locations,
                          boolean branchFeature, boolean subsidiaryFeature, Set<String> ack) {
        if (dto.rows == null || dto.rows.isEmpty()) throw new IllegalArgumentException("Grid Record not found");
        if (isoDay(dto.VoucherDate) == null) throw new IllegalArgumentException("Voucher Date Field is Required");

        // FormValidation()
        if (nz(dto.ProjectId) == 0) throw new IllegalArgumentException("Cost Center Field is Required");
        if (nz(dto.LocationTypeId) == 0 || !locations.containsKey(nz(dto.LocationTypeId))) {
            throw new IllegalArgumentException("Location Type Field is Required");
        }
        if (nz(dto.RefAccountId) == 0 || !headerTitles.containsKey(nz(dto.RefAccountId))) {
            throw new IllegalArgumentException("Credit Account Field is Required");
        }
        /* MultiCurrencyFeatureVisibilty is hard-wired true on this form (MultiCurrencyFeature()), so the Fcy trio is always required. */
        if (nz(dto.MultiCurrencyId) == 0 || !currencies.containsKey(nz(dto.MultiCurrencyId))) throw new IllegalArgumentException("Fcy Code Field is Required");
        if (nzd(dto.ExchangeCurrencyRate) == 0d) throw new IllegalArgumentException("Exchange Rate Field is Required");
        if (nzd(dto.FcAmount) == 0d) throw new IllegalArgumentException("Fcy Amount Field is Required");

        // Row-add checks (FormValidationDetail): a row without them was not built by the screen.
        boolean compulsory = Boolean.TRUE.equals(f.get("chequeNoCompulsoryOnBpv"));
        for (PaymentTaxVoucherDto.Row r : dto.rows) {
            if (!paymentTypes.containsKey(nz(r.PaymentTypeId))) throw new IllegalArgumentException("PaymentType Field is Required");
            if (nz(r.AccountId) == 0 || !detailTitles.containsKey(nz(r.AccountId))) throw new IllegalArgumentException("Debit Account Field is Required");
            if (subsidiaryFeature && nz(r.SubsidiaryAccountId) == 0) throw new IllegalArgumentException("Subsidiary Account Field is Required");
            if (nz(r.JobLotId) == 0) throw new IllegalArgumentException("Job/Lot Field is Required");
            if (doc == 2) {
                if (nz(r.FinancialInstrumentId) == 0) throw new IllegalArgumentException("Financial Instrument Field is Required");
                if (nz(r.FinancialInstrumentId) == 1 && compulsory && nz(r.ChequeId) == 0) throw new IllegalArgumentException("Cheque_number Field is Required");
            }
            if (nz(r.TcyCodeId) == 0 || !currencies.containsKey(nz(r.TcyCodeId))) throw new IllegalArgumentException("Tcy Code Field is Required");
            if (nzd(r.TcyExchangeRate) == 0d) throw new IllegalArgumentException("Tcy Exchange Rate Field is Required");
            if (nzd(r.Amount) == 0d) throw new IllegalArgumentException("Amount Field is Required");
            if (doc == 2 && nz(r.FinancialInstrumentId) == 1 && nz(r.ChequeTypeId) == 0) throw new IllegalArgumentException("Cheque Type Field is Required");
            if (branchFeature && nz(r.BranchId) == 0) throw new IllegalArgumentException("BranchName Field is Required");
        }

        // WHT checks inside Insert()
        boolean wht = Boolean.TRUE.equals(dto.IncludeWHT);
        if (wht) {
            if (nz(dto.TaxTypeId) == 0 || !taxNames.containsKey(nz(dto.TaxTypeId))) throw new IllegalArgumentException("TaxType Field is Required");
            if (toDouble(dto.TaxPercent) == 0d) throw new IllegalArgumentException("Tax Percent Field is Required");
            if (toDouble(dto.TaxPercent) >= 100d) throw new IllegalArgumentException("Tax Percent must be less than 100");
            if (nz(dto.AgainstAcId) == 0 || !whtTitles.containsKey(nz(dto.AgainstAcId))) throw new IllegalArgumentException("Withholding Debit Account Field is Required");
            if (nz(dto.WithHoldingAcId) == 0 || !whtTitles.containsKey(nz(dto.WithHoldingAcId))) throw new IllegalArgumentException("Withholding Credit Account Field is Required");
        }
        double srb = nzd(dto.SrbAmount), disc = nzd(dto.DiscountAmount);
        if (nz(dto.SrbAccountId) == 0 && srb > 0) throw new IllegalArgumentException("SRB Account Field is Required");
        if (nz(dto.SrbAccountId) != 0 && !srbTitles.containsKey(nz(dto.SrbAccountId)) && srb > 0) throw new IllegalArgumentException("SRB Account Field is Required");
        if (nz(dto.DiscountAccountId) == 0 && disc > 0) throw new IllegalArgumentException("Discount Account Field is Required");
        if (nz(dto.DiscountAccountId) != 0 && !discTitles.containsKey(nz(dto.DiscountAccountId)) && disc > 0) throw new IllegalArgumentException("Discount Account Field is Required");
        if (nzd(dto.DiscountPercent) == 0d && disc > 0) throw new IllegalArgumentException("Disc% Field is Required");
        for (PaymentTaxVoucherDto.Row r : dto.rows) {
            if (nzd(r.Amount) > 0 && nz(r.AccountId) == 0) throw new IllegalArgumentException("Please Select Account Title First");
        }

        Set<Integer> accounts = new HashSet<>();
        for (PaymentTaxVoucherDto.Row r : dto.rows) accounts.add(nz(r.AccountId));
        if (((wht && totals.tax > 0d) || (nz(dto.DiscountAccountId) > 0 && disc > 0)) && accounts.size() > 1) {
            List<String> applied = new ArrayList<>();
            if (wht) applied.add("Withholding Tax");
            if (nz(dto.SrbAccountId) > 0) applied.add("SRB Tax");
            if (nz(dto.DiscountAccountId) > 0) applied.add("Discount");
            throw new IllegalArgumentException("There are multiple accounts in detail, so you can't apply: "
                    + String.join(", ", applied) + ".\nYou can only apply these to a single account in detail.");
        }
        if (wht && nz(dto.rows.get(0).AccountId) != nz(dto.AgainstAcId) && !ack.contains("whtMismatch")) {
            throw new ConfirmationRequiredException("WHT Debit A/C not match with detail Debit A/C. Are You Sure To Proceed?", "whtMismatch");
        }
    }

    // ================================================================================ rights

    /** CommonServices.SetRightsValueInRightsObject(ScreenName) with its Admin short-circuit. */
    public Map<String, Boolean> rights(int mode) {
        Map<String, Boolean> r = new LinkedHashMap<>();
        for (String n : new String[] {"Save", "Update", "Print", "Delete"}) r.put(n.toLowerCase(), hasRight(mode, n));
        return r;
    }

    private boolean hasRight(int mode, String rightName) {
        String role = currentUserContext.currentRoleName();
        if ("Admin".equals(role)) return true;
        try {
            for (Map<String, Object> row : jdbcTemplate.queryForList(SQL_USER_RIGHTS, currentUserContext.currentUserId(),
                    screenName(mode), role == null ? "" : role, currentUserContext.currentCompanyId(), "GetByUserId")) {
                Object n = ci(row, "RightName");
                if (n != null && rightName.equals(String.valueOf(n).trim())) return truthy(ci(row, "Value"));
            }
        } catch (Exception e) {
            LOG.warn("Could not read the '{}' right for {}; denying", rightName, screenName(mode), e);
        }
        return false;
    }

    // ================================================================================ small helpers

    /** A desktop Yes/No the operator has to answer before the save can proceed. */
    public static class ConfirmationRequiredException extends RuntimeException {
        public final String kind;
        public ConfirmationRequiredException(String message, String kind) { super(message); this.kind = kind; }
    }

    private static Map<Integer, String> titles(List<Map<String, Object>> list) {
        Map<Integer, String> m = new LinkedHashMap<>();
        for (Map<String, Object> r : list) m.put(toInt(ci(r, "Id")), str(ci(r, "AccountTitle")));
        return m;
    }

    private static double round(double v, int decimals) {
        return BigDecimal.valueOf(v).setScale(Math.max(decimals, 0), RoundingMode.HALF_UP).doubleValue();
    }

    /** Math.Round(double, digits): to even. */
    private static double roundEven(double v, int decimals) {
        return BigDecimal.valueOf(v).setScale(Math.max(decimals, 0), RoundingMode.HALF_EVEN).doubleValue();
    }

    /** clsGlobalVariables.stringFormatsingle = "#,##0." + decimals zeros. */
    private static String fmt(double v, int decimals) {
        int d = Math.max(decimals, 0);
        StringBuilder p = new StringBuilder("#,##0");
        if (d > 0) { p.append('.'); for (int i = 0; i < d; i++) p.append('0'); }
        DecimalFormat df = new DecimalFormat(p.toString(), DecimalFormatSymbols.getInstance(Locale.US));
        df.setRoundingMode(RoundingMode.HALF_UP);
        return df.format(v);
    }

    /** C# default double.ToString(): no trailing zeros. */
    private static String num(double d) {
        if (d == 0d) return "0";
        return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
    }

    private static int nz(Integer v) { return v == null ? 0 : v; }
    private static double nzd(Double v) { return v == null ? 0d : v; }
    private static String nzs(String v) { return v == null ? "" : v; }
    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    private static String dateTimeStr(Object v) {
        if (v instanceof java.sql.Timestamp) {
            return ((java.sql.Timestamp) v).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        }
        return v == null ? null : String.valueOf(v);
    }

    private static String dateStr(Object v) {
        if (v == null) return null;
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.time.LocalDateTime) return ((java.time.LocalDateTime) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new SimpleDateFormat("yyyy-MM-dd").format((java.util.Date) v);
        return isoDay(v);
    }

    private static String isoDay(Object v) {
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    private static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return (int) Double.parseDouble(v.toString().trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static double toDouble(Object v) {
        if (v instanceof Number) return ((Number) v).doubleValue();
        if (v == null) return 0d;
        try { return Double.parseDouble(v.toString().replace(",", "").trim()); } catch (NumberFormatException e) { return 0d; }
    }

    private static boolean truthy(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        if (v == null) return false;
        String s = v.toString().trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }
}
