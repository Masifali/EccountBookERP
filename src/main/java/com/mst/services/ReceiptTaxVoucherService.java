package com.mst.services;

import com.mst.models.ConfigrationsAllocation;
import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.ReceiptTaxVoucherDto;
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
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

/**
 * Screens 853 frmCashReceiptVoucherTax and 854 frmBankReceiptVoucherTax.
 *
 * Desktop: Architecture.WinApp.Account_Definition.VouchersWithTax.ReceiptsVoucherNew. The dashboard opens the
 * one class with form.Tag = ScreenName, and AcfrmPaymentVoucher_Load reads that Tag into DocumentTypeId
 * (frmCashReceiptVoucherTax = 3, frmBankReceiptVoucherTax = 4). "mode" below is that DocumentTypeId, so a
 * screen holds exactly one voucher type. The rights are read under the Tag.
 *
 * Persistence is the same BLL VoucherHead.Save chain every desktop voucher uses (DesktopVoucherWriter), not
 * the JPA saveVoucher. The ledger lines are derived here from the form's own Insert() (vd debit / vd2 credit per
 * grid row, WHT vd3/vd4, discount vd5/vd6), so the page never chooses a debit/credit side or a total.
 */
@Service
public class ReceiptTaxVoucherService {

    private static final Logger LOG = LoggerFactory.getLogger(ReceiptTaxVoucherService.class);

    private static final String SQL_USER_RIGHTS =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, @CompanyId=?, @Activity=?";

    @Autowired private DesktopVoucherWriter writer;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private VoucherDesktopConfigService config;
    @Autowired private DesktopVoucherScreenService screens;

    // ================================================================================ form identity

    /** The document type of the screen: 3 = Cash Receipt Voucher (853), 4 = Bank Receipt Voucher (854). */
    public static void requireMode(int mode) {
        if (mode != 3 && mode != 4) throw new IllegalArgumentException("Unknown screen " + mode);
    }

    /** base.Tag - the ScreenName the rights are read under. */
    public static String screenName(int mode) {
        return mode == 3 ? "frmCashReceiptVoucherTax" : "frmBankReceiptVoucherTax";
    }

    // ================================================================================ lookups

    /** Everything the form binds on Load, each from the desktop's own call. */
    public Map<String, Object> lookups(int mode) {
        requireMode(mode);
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> r = new LinkedHashMap<>();

        /* TypeFill: the one voucher type of this screen */
        List<Map<String, Object>> types = new ArrayList<>();
        types.add(typeRow(mode, mode == 3 ? "Cash Receipt Voucher" : "Bank Receipt Voucher"));
        r.put("voucherTypes", types);
        r.put("startDoc", mode);

        /* AccountTitleFill: COAAllocation.GetDetailAccountByDocumentTypeId -> CmbDebitAccount */
        List<Map<String, Object>> header = new ArrayList<>();
        for (Map<String, Object> row : jdbcTemplate.queryForList(
                "EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), mode, "GetDetailAccountsByDocumentTypeId")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(row, "Id"));
            o.put("AccountTitle", ci(row, "AccountTitle"));
            o.put("CurrencyId", ci(row, "CurrencyId"));
            o.put("CurrencyCode", ci(row, "CurrencyCode"));
            header.add(o);
        }
        r.put("debitAccounts", header);

        r.put("creditAccounts", creditAccounts());                                    // DetailAccountFill (combactitle)
        r.put("referenceAccounts", screens.globalAccounts(new int[] {3, 5, 6, 7, 8}, new int[0], new int[0]));
        r.put("discountAccounts", screens.globalAccounts(new int[] {11, 13, 20, 21, 22}, new int[0], new int[0]));
        r.put("whtAccounts", screens.globalAccounts(new int[0], new int[] {2, 11, 15}, new int[0]));   // AccountsComboBind
        r.put("locationTypes", screens.locationTypes());
        r.put("currencies", screens.currenciesWithRate());                            // CurrencyFill (header + Tcy detail)
        r.put("paymentTypes", jdbcTemplate.queryForList("EXEC [Account].[USP_lookUp_GetAllMethod] @Activity=?",
                "GetDataPaymentTypeForReceipts"));
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
        r.put("nextCode", nextCode(mode));
        return r;
    }

    private static Map<String, Object> typeRow(int id, String name) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("Name", name);
        return o;
    }

    /**
     * DetailAccountFill: InventoryRelatedAccountsShowInVouchers off -> exclude account types
     * {2,4,11,12,13,14,15,19,20,21} and PLNoteId 2; on -> exclude {2,15}. (No ExpenseAccountAllowOnPaymentVoucher
     * switch on a receipt.)
     */
    public List<Map<String, Object>> creditAccounts() {
        Map<String, ConfigrationsAllocation> m = config.configMap();
        boolean inventory = VoucherDesktopConfigService.toBool(VoucherDesktopConfigService.key(m, "InventoryRelatedAccountsShowInVouchers"));
        int[] excluded = !inventory ? new int[] {2, 4, 11, 12, 13, 14, 15, 19, 20, 21} : new int[] {2, 15};
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
     * HistoryFillCrv / HistoryFillBrv -> VoucherHead.VoucherFormHistory -> USP_VoucherFormHistory with
     * vh.Ids = "3" / "4" (each tab its own single document type), CanViewAllRecord = the screen's
     * "CanView AllRecord" right, EntryUser only when that right is off, AppId when != 0, IsApproved
     * unless "All". Failure is reported, never replaced by rows from another source.
     */
    public Map<String, Object> history(int mode, String dateType, String fromDate, String toDate,
                                       Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus) {
        requireMode(mode);
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean viewAll = hasRight(mode, "CanView AllRecord");
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_VoucherFormHistory @OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeName=?, @CanViewAllRecord=?");
        List<Object> a = new ArrayList<>();
        a.add(u.getOrganizationId()); a.add(u.getCompanyId()); a.add(u.getId()); a.add(String.valueOf(mode)); a.add(viewAll);
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
            if (mode == 4) o.put("chequeNo", ci(r, "ChequeNo"));
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

    /** ComboBindForCrvHistory / Brv: Sp_Vouchers_LedgerByJobLot_DropDownAndLists, rows with ActivityType "Account Header". */
    public List<Map<String, Object>> historyAccounts(int mode) {
        requireMode(mode);
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC dbo.Sp_Vouchers_LedgerByJobLot_DropDownAndLists @OrganizationId=?, @CompanyId=?, @AppId=?, @UserId=?, @DocumentTypeIds=?",
                u.getOrganizationId(), u.getCompanyId(), u.getAppId() == null ? 0 : u.getAppId(), u.getId(), String.valueOf(mode))) {
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

    /**
     * The head row when it is this tenant's receipt voucher of this screen's document type, otherwise null.
     * A non-tax receipt (BaseDocumentTypeId 0) belongs to the Receipts Voucher New screen: the desktop's
     * CommonServices routes it there by BaseDocumentTypeId, so here it is refused with that pointer.
     */
    private Map<String, Object> ownHead(int mode, int id) {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> heads = head(id);
        if (heads.isEmpty()) return null;
        Map<String, Object> h = heads.get(0);
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId() || toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "DocumentTypeId")) != mode) return null;
        Object base = ci(h, "BaseDocumentTypeId");       // absent from the row -> not checked (as the desktop's GetByID)
        if (base != null && toInt(base) != 2) {
            throw new IllegalArgumentException("This is a receipt voucher without tax. Open it from the Receipts Voucher screen.");
        }
        return h;
    }

    /**
     * ReadById(): VoucherHead.GetByID. Grid rows are the IsTaxable "False" lines whose account is not the
     * header (debit) account - the credit line of each pair, so Amount is the CreditAmount. The WHT block comes
     * from the first tax line (IsTaxable True, TaxesTotalAmount > 0, TaxTypeId > 0), the discount block from the
     * first line with DiscountAmount > 0 and a CreditAmount > 0.
     */
    public Map<String, Object> load(int mode, int id) {
        requireMode(mode);
        Map<String, Object> h = ownHead(mode, id);
        if (h == null) return null;
        List<Map<String, Object>> details = jdbcTemplate.queryForList(
                "EXEC dbo.Sp_Vouchers_GetMethods @Id=?, @Activity=?", id, "VoucherDetail_ReadByVoucherHeadID");
        int refAccountId = toInt(ci(h, "RefAccountId"));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", toInt(ci(h, "Id")));
        out.put("documentTypeId", mode);
        out.put("voucherCode", ci(h, "VoucherCode"));
        out.put("voucherDate", dateStr(ci(h, "VoucherDate")));
        out.put("refAccountId", refAccountId);
        out.put("projectId", ci(h, "ProjectId"));
        out.put("multiCurrencyId", ci(h, "MultiCurrencyId"));
        out.put("exchangeCurrencyRate", ci(h, "ExchangeCurrencyRate"));
        out.put("fcAmount", ci(h, "FcAmount"));
        out.put("voucherAmount", ci(h, "VoucherAmount"));
        out.put("isApproved", truthy(ci(h, "IsApproved")));
        out.put("customAccounts", truthy(ci(h, "CustomAccounts")));
        out.put("refDocNoId", ci(h, "RefDocNoId"));
        out.put("remarks", str(ci(h, "Remarks")));
        out.put("locationTypeId", details.isEmpty() ? 0 : toInt(ci(details.get(0), "LocationTypeId")));

        List<Map<String, Object>> rows = new ArrayList<>();
        Map<String, Object> wht = null, disc = null;
        for (Map<String, Object> d : details) {
            String taxable = str(ci(d, "IsTaxable"));
            int acc = toInt(ci(d, "AccountId"));
            double credit = toDouble(ci(d, "CreditAmount"));
            if ("False".equals(taxable) && acc != refAccountId) {
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
                r.put("remarks", ci(d, "Comments"));
                r.put("tcyCodeId", ci(d, "DMultiCurrencyId"));
                r.put("tcyCode", ci(d, "TcyCode"));
                r.put("tcyExchangeRate", ci(d, "DExchangeCurrencyRate"));
                r.put("fcyAmount", ci(d, "DCurrencyAmount"));
                r.put("amount", credit);
                r.put("referenceAccountId", ci(d, "ReferenceAccountId"));
                r.put("referenceAccount", ci(d, "ReferenceAccount"));
                r.put("chequeDate", dateStr(ci(d, "DCheqDate")));
                r.put("chequeNo", ci(d, "CheqNoDetail"));
                r.put("payeeTitle", ci(d, "PayeeTitle"));
                r.put("branchId", ci(d, "BranchesId"));
                r.put("branchName", ci(d, "BranchName"));
                r.put("costCenterId", ci(d, "CostCenterId"));
                rows.add(r);
            }
            if (wht == null && "True".equals(taxable) && toDouble(ci(d, "TaxesTotalAmount")) > 0 && toInt(ci(d, "TaxTypeId")) > 0) {
                wht = new LinkedHashMap<>();
                wht.put("againstAcId", ci(h, "RefDocNoId"));
                wht.put("withHoldingAcId", acc);
                wht.put("withHoldingAcTitle", ci(d, "AccountTitle"));
                wht.put("taxTypeId", ci(d, "TaxTypeId"));
                wht.put("taxPercent", ci(d, "TaxPrcnt"));
                wht.put("taxAmount", ci(d, "TaxesTotalAmount"));
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
        out.put("discount", disc);
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
            o.put("tcyCode", ci(d, "TcyCode"));
            o.put("tcyExchangeRate", ci(d, "DExchangeCurrencyRate"));
            o.put("debit", toDouble(ci(d, "DebitAmount")));
            o.put("credit", toDouble(ci(d, "CreditAmount")));
            o.put("fcy", toDouble(ci(d, "DCurrencyAmount")));
            o.put("referenceAccount", ci(d, "ReferenceAccount"));
            o.put("branchName", ci(d, "BranchName"));
            o.put("costCenter", ci(d, "CostCenterName"));
            o.put("isTaxable", str(ci(d, "IsTaxable")));
            o.put("chequeDate", dateStr(ci(d, "DCheqDate")));
            o.put("chequeNo", ci(d, "CheqNoDetail"));
            o.put("payeeTitle", ci(d, "PayeeTitle"));
            rows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", toInt(ci(h, "Id")));
        out.put("documentTypeId", toInt(ci(h, "DocumentTypeId")));
        out.put("rows", rows);
        return out;
    }

    /** cmbCurrency_Leave: Sp_Vouchers_GetMethods 'GetMultiCurrencyAndLastRate', LastExchRate; no row -> 0. */
    public double lastRate(int mode, int currencyId) {
        requireMode(mode);
        UserAccount u = currentUserContext.requireAccountingUser();
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @DocumentTypeIds=?");
        List<Object> a = new ArrayList<>();
        a.add(u.getOrganizationId()); a.add(u.getCompanyId()); a.add(String.valueOf(mode));
        if (currencyId != 0) { sql.append(", @DMultiCurrencyIds=?"); a.add(String.valueOf(currencyId)); }
        sql.append(", @Activity=?"); a.add("GetMultiCurrencyAndLastRate");
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(sql.toString(), a.toArray());
        return rows.isEmpty() ? 0d : toDouble(ci(rows.get(0), "LastExchRate"));
    }

    // ================================================================================ Total()

    /** Total() / checkBox1_CheckedChanged results: txtValue / txtTaxAmount / txtTotalAmount. */
    public static final class Totals {
        public double base, value, tax, total;
        /** txtValue.Text as the desktop shows it (clsGlobalVariables.stringFormatsingle). */
        public String valueText = "";
    }

    /**
     * Total(): Value = sum of the row Amounts (text formatted with the amount decimals), Total = Amount + Tax +
     * Discount. Tax (checkBox1_CheckedChanged, WHT ticked): Math.Round((Value + Discount) / (100 - rate) * 100 *
     * rate / 100, 2), rounded to even; 0 when WHT is off. Nothing is set while the grid amount is 0.
     */
    public static Totals total(ReceiptTaxVoucherDto dto, int decimals) {
        Totals t = new Totals();
        double base = 0d;
        if (dto.rows != null) for (ReceiptTaxVoucherDto.Row r : dto.rows) base += nzd(r.Amount);
        t.base = base;
        if (base <= 0d) return t;
        t.valueText = fmt(base, decimals);
        t.value = toDouble(t.valueText);
        double disc = nzd(dto.DiscountAmount);
        double rate = Boolean.TRUE.equals(dto.IncludeWHT) ? toDouble(dto.TaxPercent) : 0d;
        t.tax = (rate > 0d && rate < 100d) ? roundEven((t.value + disc) / (100d - rate) * 100d * rate / 100d, 2) : 0d;
        t.total = round(base + t.tax + disc, decimals);
        return t;
    }

    // ================================================================================ save

    @Transactional
    public Map<String, Object> save(int mode, ReceiptTaxVoucherDto dto) {
        requireMode(mode);
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean insert = dto.Id == null || dto.Id <= 0;
        if (insert && !hasRight(mode, "Save")) throw new SecurityException("You don't have Save right on this screen.");
        if (!insert && !hasRight(mode, "Update")) throw new SecurityException("You don't have Update right on this screen.");
        if (!insert && ownHead(mode, dto.Id) == null) {
            throw new IllegalArgumentException("Record Not Update  " + dto.Id);
        }
        int org = u.getOrganizationId(), comp = u.getCompanyId();
        int branchId = u.getBranchesId() == null ? 0 : u.getBranchesId();
        int year = currentUserContext.currentFinancialYearId();
        Map<String, Object> f = flags(mode);
        boolean branchFeature = Boolean.TRUE.equals(f.get("branchFeature"));
        boolean subsidiaryFeature = Boolean.TRUE.equals(f.get("subsidiaryFeature"));
        int decimals = toInt(f.get("amountDecimals"));
        Set<String> ack = new HashSet<>(dto.acknowledged == null ? new ArrayList<String>() : dto.acknowledged);

        Map<Integer, String> debitTitles = new LinkedHashMap<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Activity=?",
                org, comp, mode, "GetDetailAccountsByDocumentTypeId")) {
            debitTitles.put(toInt(ci(r, "Id")), str(ci(r, "AccountTitle")));
        }
        Map<Integer, String> creditTitles = titles(creditAccounts());
        Map<Integer, String> paymentTypes = new LinkedHashMap<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList("EXEC [Account].[USP_lookUp_GetAllMethod] @Activity=?",
                "GetDataPaymentTypeForReceipts")) {
            paymentTypes.put(toInt(ci(r, "Id")), str(ci(r, "PaymentType")));
        }
        Map<Integer, String> whtTitles = titles(screens.globalAccounts(new int[0], new int[] {2, 11, 15}, new int[0]));
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
        validate(dto, f, totals, debitTitles, creditTitles, paymentTypes, whtTitles, discTitles,
                taxNames, currencies, locations, branchFeature, subsidiaryFeature, ack);

        String voucherDate = isoDay(dto.VoucherDate);
        String now = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date());
        int voucherCode = nz(dto.VoucherCode);
        if (insert && voucherCode <= 0) voucherCode = nextCode(mode);

        boolean wht = Boolean.TRUE.equals(dto.IncludeWHT);
        double rate = nzd(dto.ExchangeCurrencyRate);

        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        vh.Id = insert ? 0 : dto.Id;
        vh.BaseDocumentTypeId = 2;
        vh.DocumentTypeId = mode;
        vh.ProjectId = nz(dto.ProjectId);
        vh.VoucherCode = voucherCode;
        vh.VoucherDate = voucherDate;
        vh.RefAccountId = nz(dto.RefAccountId);            // CmbDebitAccount
        vh.AgainstAccountId = nz(dto.WithHoldingAcId);     // CmbWithHoldingAc
        vh.RefDocNoId = nz(dto.AgainstAcId);               // CmbAgainstAc
        vh.Remarks = nzs(dto.Remarks).trim();              // txtremarksmain.Text.Trim()
        vh.RemarksOtherLingo = "";
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

        List<ContraVoucherDto.Detail> details = new ArrayList<>();
        List<ContraVoucherDto.CostCentre> costCentres = new ArrayList<>();
        List<ReceiptTaxVoucherDto.Row> rows = dto.rows;
        int count = rows.size();
        int locationId = nz(dto.LocationTypeId);
        int lineId = 1;
        double voucherAmt = 0d;

        for (int i = 0; i < count; i++) {
            ReceiptTaxVoucherDto.Row r = rows.get(i);
            double amount = nzd(r.Amount);
            String rowRemarks = nzs(r.Remarks);
            String chequeNo = nzs(r.ChequeNo);
            String payee = nzs(r.PayeeTitle);
            String chequeDate = isoDay(r.ChequeDate);
            String ptText = paymentTypes.getOrDefault(nz(r.PaymentTypeId), "");
            int branch = branchFeature ? nz(r.BranchId) : branchId;
            int subType = nz(r.SubsidiaryAccountTypeId);
            int subId = nz(r.SubsidiaryAccountId);

            ContraVoucherDto.Detail vd = new ContraVoucherDto.Detail();
            vd.SubsidiaryAgainstTypeId = subType;
            if (subId > 0) vd.SubsidiaryAgainstAccountId = subId;
            /* AutoRemarksForPaymentThroughBank is forced false on a bank receipt, so both document types take this branch. */
            vd.Comments = !vh.Remarks.isEmpty() ? vh.Remarks : rowRemarks;
            vd.LineId = lineId;
            vd.SortNo = i + 1;
            vd.PaymentTypeId = nz(r.PaymentTypeId);
            vd.PaymentType = ptText;
            vd.AccountId = nz(dto.RefAccountId);
            vd.AgainstAccountId = nz(r.AccountId);
            vd.JobLotId = nz(r.JobLotId);
            vd.DebitAmount = amount;
            vd.IsTaxable = "False";
            if (mode != 3) {
                vd.InvoiceNoRefId = nz(vh.CheqId);
                vd.DCheqDate = chequeDate;
                vd.CheqNoDetail = chequeNo;
                vd.PayeeTitle = payee;
                if (blank(vh.ChequeNo)) {
                    vh.ChequeDate = chequeDate;
                    vh.ChequeNo = chequeNo;
                    vh.PayTitle = payee;
                }
            }
            vd.BranchesId = branch;
            vd.CostCenterId = nz(r.CostCenterId);
            vd.ReferenceAccountId = nz(r.ReferenceAccountId);
            vd.DMultiCurrencyId = nz(r.TcyCodeId);
            vd.DExchangeCurrencyRate = nzd(r.TcyExchangeRate);
            vd.DCurrencyAmount = nzd(r.FcyAmount);
            vd.LocationTypeId = locationId;
            voucherAmt += vd.DebitAmount;
            if (count == 1 && vh.Remarks.isEmpty()) vh.Remarks = vd.Comments == null ? "" : vd.Comments;
            details.add(vd);
            lineId++;

            ContraVoucherDto.Detail vd2 = new ContraVoucherDto.Detail();
            vd2.SubsidiaryTypeId = subType;
            if (subId > 0) {
                vd2.SupplierCustomerId = subType == 1 ? subId : 0;
                vd2.EmployeeId = subType == 2 ? subId : 0;
                vd2.SubsidiaryAccountId = subId;
            }
            vd2.Comments = rowRemarks;
            vd2.LineId = lineId;
            vd2.SortNo = i + 1;
            vd2.PaymentTypeId = nz(r.PaymentTypeId);
            vd2.PaymentType = ptText;
            vd2.AccountId = nz(r.AccountId);
            vd2.AgainstAccountId = nz(dto.RefAccountId);
            vd2.JobLotId = nz(r.JobLotId);
            vd2.CreditAmount = amount;
            vd2.IsTaxable = "False";
            if (mode != 3) {
                vd2.InvoiceNoRefId = nz(vh.CheqId);
                vd2.DCheqDate = chequeDate;
                vd2.CheqNoDetail = chequeNo;
                vd2.PayeeTitle = payee;
                if (!blank(vh.ChequeNo)) {
                    vh.ChequeDate = chequeDate;
                    vh.ChequeNo = chequeNo;
                    vh.PayTitle = payee;
                }
            }
            vd2.DMultiCurrencyId = nz(r.TcyCodeId);
            vd2.DExchangeCurrencyRate = nzd(r.TcyExchangeRate);
            vd2.DCurrencyAmount = nzd(r.FcyAmount);
            vd2.BranchesId = branch;
            vd2.CostCenterId = nz(r.CostCenterId);
            vd2.ReferenceAccountId = nz(r.ReferenceAccountId);
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

        int debitAccount = details.get(0).AccountId;
        if (wht) {
            String pct = nzs(dto.TaxPercent).trim();
            double gross = totals.value + totals.tax;
            String comments = "Received after WHT deduction: Gross " + groupedInt(gross) + ", WHT " + num(roundEven(totals.tax, 2))
                    + ", Net " + num(totals.value) + " @ " + pct + "%";
            double fcy = rate > 0 ? roundEven(totals.tax / rate, decimals) : 0d;
            ContraVoucherDto.Detail vd3 = new ContraVoucherDto.Detail();
            vd3.LineId = lineId + 1;
            vd3.AccountId = vh.AgainstAccountId;
            vd3.AgainstAccountId = vh.RefDocNoId;
            vd3.TaxTypeId = nz(dto.TaxTypeId);
            vd3.IsTaxable = "True";
            vd3.Comments = comments;
            vd3.TaxPrcnt = toDouble(pct);
            vd3.TaxesTotalAmount = totals.tax;
            vd3.DebitAmount = totals.tax;
            vd3.DCurrencyAmount = fcy;
            details.add(vd3);
            ContraVoucherDto.Detail vd4 = new ContraVoucherDto.Detail();
            vd4.LineId = lineId + 1;
            vd4.AccountId = vh.RefDocNoId;
            vd4.AgainstAccountId = vh.AgainstAccountId;
            vd4.TaxTypeId = nz(dto.TaxTypeId);
            vd4.IsTaxable = "True";
            vd4.Comments = comments;
            vd4.TaxPrcnt = toDouble(pct);
            vd4.TaxesTotalAmount = totals.tax;
            vd4.CreditAmount = totals.tax;
            vd4.DCurrencyAmount = fcy;
            details.add(vd4);
            voucherAmt += vd3.DebitAmount;
        }
        double disc = nzd(dto.DiscountAmount);
        if (nz(dto.DiscountAccountId) > 0 && disc > 0) {
            String discText = dto.DiscountAmountText != null && !dto.DiscountAmountText.trim().isEmpty() ? dto.DiscountAmountText.trim() : num(disc);
            double original = totals.value + toDouble(discText);
            String comments = "Original receivable amount was " + num(original) + ". Discount of " + discText
                    + " given upon mutual agreement. Final amount received: " + totals.valueText + ".";
            double fcy = rate > 0 ? roundEven(disc / rate, decimals) : 0d;
            double pct = nzd(dto.DiscountPercent);
            ContraVoucherDto.Detail vd5 = new ContraVoucherDto.Detail();
            vd5.LineId = lineId + 1;
            vd5.AccountId = debitAccount;
            vd5.AgainstAccountId = nz(dto.DiscountAccountId);
            vd5.DiscountPercent = pct;
            vd5.DiscountAmount = disc;
            vd5.DebitAmount = disc;
            vd5.DCurrencyAmount = fcy;
            vd5.Comments = comments;
            details.add(vd5);
            ContraVoucherDto.Detail vd6 = new ContraVoucherDto.Detail();
            vd6.LineId = lineId + 1;
            vd6.AccountId = nz(dto.DiscountAccountId);
            vd6.AgainstAccountId = debitAccount;
            vd6.DiscountPercent = pct;
            vd6.DiscountAmount = disc;
            vd6.CreditAmount = disc;
            vd6.DCurrencyAmount = fcy;
            vd6.Comments = comments;
            details.add(vd6);
            voucherAmt += vd5.DebitAmount;
        }
        /* A receipt Insert() has no negative-balance, duplicate-voucher or GL-balance prompt. */
        vh.VoucherAmount = voucherAmt;

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

    private static boolean blank(String s) { return s == null || s.trim().isEmpty(); }

    // ================================================================================ validation (Insert() order)

    private void validate(ReceiptTaxVoucherDto dto, Map<String, Object> f, Totals totals,
                          Map<Integer, String> debitTitles, Map<Integer, String> creditTitles, Map<Integer, String> paymentTypes,
                          Map<Integer, String> whtTitles, Map<Integer, String> discTitles,
                          Map<Integer, String> taxNames, Map<Integer, String> currencies, Map<Integer, String> locations,
                          boolean branchFeature, boolean subsidiaryFeature, Set<String> ack) {
        if (dto.rows == null || dto.rows.isEmpty()) throw new IllegalArgumentException("Grid Record not found");
        if (isoDay(dto.VoucherDate) == null) throw new IllegalArgumentException("Voucher Date Field is Required");

        // FormValidation()
        if (nz(dto.ProjectId) == 0) throw new IllegalArgumentException("Cost Center Field is Required");
        if (nz(dto.LocationTypeId) == 0 || !locations.containsKey(nz(dto.LocationTypeId))) {
            throw new IllegalArgumentException("Location Type Field is Required");
        }
        if (nz(dto.RefAccountId) == 0 || !debitTitles.containsKey(nz(dto.RefAccountId))) {
            throw new IllegalArgumentException("Debit Account Field is Required");
        }
        /* MultiCurrencyFeatureVisibilty is hard-wired true on this form (:1410), so the Fcy trio is always required. */
        if (nz(dto.MultiCurrencyId) == 0 || !currencies.containsKey(nz(dto.MultiCurrencyId))) throw new IllegalArgumentException("Fcy Code Field is Required");
        if (nzd(dto.ExchangeCurrencyRate) == 0d) throw new IllegalArgumentException("Exchange Rate Field is Required");
        if (nzd(dto.FcAmount) == 0d) throw new IllegalArgumentException("Fcy Amount Field is Required");
        if (Boolean.TRUE.equals(f.get("multiCurrencyGrid"))) {
            throw new IllegalArgumentException("The Multi Currency grid (ERP feature 6) of this voucher is not available on the web yet. Please use the desktop for this company.");
        }

        // Row-add checks (Add_Click_1): a row without them was not built by the screen.
        for (ReceiptTaxVoucherDto.Row r : dto.rows) {
            if (!paymentTypes.containsKey(nz(r.PaymentTypeId))) throw new IllegalArgumentException("PaymentType Field Required");
            if (nz(r.AccountId) == 0 || !creditTitles.containsKey(nz(r.AccountId))) throw new IllegalArgumentException("Credit Account Field Required");
            if (nz(r.JobLotId) == 0) throw new IllegalArgumentException("Job/Lot Field Required");
            if (nz(r.TcyCodeId) == 0 || !currencies.containsKey(nz(r.TcyCodeId))) throw new IllegalArgumentException("Tcy Code Field is Required");
            if (nzd(r.TcyExchangeRate) == 0d) throw new IllegalArgumentException("Tcy Exchange Rate Field is Required");
            if (nzd(r.Amount) == 0d) throw new IllegalArgumentException("Amount Field Required");
            if (subsidiaryFeature && nz(r.SubsidiaryAccountId) == 0) throw new IllegalArgumentException("Subsidiary Account Title Field Is Required");
            if (branchFeature && nz(r.BranchId) == 0) throw new IllegalArgumentException("BranchName Field is Required");
        }

        // WHT checks inside Insert()
        boolean wht = Boolean.TRUE.equals(dto.IncludeWHT);
        if (wht) {
            if (nz(dto.TaxTypeId) == 0 || !taxNames.containsKey(nz(dto.TaxTypeId))) throw new IllegalArgumentException("TaxType Account Field is Required");
            if (toDouble(dto.TaxPercent) == 0d) throw new IllegalArgumentException("Tax Percent Field is Required");
            if (nz(dto.AgainstAcId) == 0 || !whtTitles.containsKey(nz(dto.AgainstAcId))) throw new IllegalArgumentException("Withholding Credit Account Field is Required");
            if (nz(dto.WithHoldingAcId) == 0 || !whtTitles.containsKey(nz(dto.WithHoldingAcId))) throw new IllegalArgumentException("Withholding Debit Account Field is Required");
            /* web-only guard: the desktop formula divides by (100 - rate). */
            if (toDouble(dto.TaxPercent) < 0d || toDouble(dto.TaxPercent) >= 100d) throw new IllegalArgumentException("Tax Percent must be less than 100");
        }
        double disc = nzd(dto.DiscountAmount);
        if (nz(dto.DiscountAccountId) == 0 && disc > 0) throw new IllegalArgumentException("Discount Account Field is Required");
        if (nz(dto.DiscountAccountId) != 0 && !discTitles.containsKey(nz(dto.DiscountAccountId)) && disc > 0) throw new IllegalArgumentException("Discount Account Field is Required");
        if (nzd(dto.DiscountPercent) == 0d && disc > 0) throw new IllegalArgumentException("Disc% Field is Required");
        for (ReceiptTaxVoucherDto.Row r : dto.rows) {
            if (nzd(r.Amount) > 0 && nz(r.AccountId) == 0) throw new IllegalArgumentException("Please Select Account Title First");
        }

        Set<Integer> accounts = new HashSet<>();
        for (ReceiptTaxVoucherDto.Row r : dto.rows) accounts.add(nz(r.AccountId));
        if (((wht && totals.tax > 0d) || (nz(dto.DiscountAccountId) > 0 && disc > 0)) && accounts.size() > 1) {
            List<String> applied = new ArrayList<>();
            if (wht) applied.add("Withholding Tax");
            if (nz(dto.DiscountAccountId) > 0) applied.add("Discount");
            throw new IllegalArgumentException("There are multiple accounts in detail, so you can't apply: "
                    + String.join(", ", applied) + ".\nYou can only apply these to a single account in detail.");
        }
        if (wht && nz(dto.rows.get(0).AccountId) != nz(dto.AgainstAcId) && !ack.contains("whtMismatch")) {
            throw new ConfirmationRequiredException("WHT A/C not match with detail Credit A/C. Are You Sure To Proceed?", "whtMismatch");
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

    /** C# double.ToString("#,#"): rounded away from zero to a whole number with thousands separators; 0 gives "". */
    private static String groupedInt(double v) {
        long n = BigDecimal.valueOf(v).setScale(0, RoundingMode.HALF_UP).longValue();
        if (n == 0L) return "";
        return new DecimalFormat("#,#", DecimalFormatSymbols.getInstance(Locale.US)).format(n);
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
