package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import com.mst.services.desktopvoucher.DesktopVoucherSupport;
import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

/**
 * R3 account reports (new, additive - nothing existing is touched). Tenancy always comes from CurrentUserContext;
 * every stored procedure and parameter set is the desktop form's own call. A null argument is OMITTED from the EXEC
 * (DesktopProc), exactly as AddWithValue(null) is left out on the desktop.
 *
 *  60  FcyGeneralLedgerRpt (Account_Reports) - CommonServices.CoaAllocationAccountTitleByAccountTypeIds("22"),
 *        VoucherReports.FCY_LedgerAgainstAccountId (SPU_Accounts_FCYCustomerLedger_Rpt) for "From Exports" and
 *        VoucherReports.Accounts_FcyGeneralLedger_Rpt (Usp_Accounts_FcyGeneralLedger_Rpt) for "From Accounts".
 *  85  FcyBankCharges_Register - ExImFCBankReceipts.GetDataForDropDownFromFcyBankReceipts
 *        (USP_GetDataForDropDownFromFcyBankReceipts, Activity ReceiverAccount / FDBCNo / ChargesAccount) and
 *        ExImFCBankReceipts.FcyBankCharges_Register ([dbo].[usp_FcyBankCharges_Register]).
 *  86  ProfitLoss (Profit &amp; Loss 02) - VoucherReports.ProftLoss (SpAccounts_ProfitLoassFormatA_Report),
 *        VoucherReports.Accounts_ProfitLoss_ForCrystal (SpAccounts_ProfitLoss_ForCrystal), branch combo
 *        (USP_GetBranchesFromVouchersByAccountId) and InfragisticsHelper.GetBranchesIdsByFeature.
 *  67  CostomerWiseVoucherSlip - VoucherHead.GetCashAndBanksAccountByDate / GetPartiesFromVouchersByDate
 *        (Sp_Vouchers_GetMethods) and VoucherReports.VoucherSlipPartyWise (Usp_VouchersPaymentReceiptSlip_Report).
 */
@Service
public class AcRpt3ReportService {
    private static final Logger LOG = LoggerFactory.getLogger(AcRpt3ReportService.class);

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final DesktopVoucherSupport dv;
    private final AccountReportsHSupport h;

    public AcRpt3ReportService(JdbcTemplate jdbc, CurrentUserContext ctx, DesktopVoucherSupport dv, AccountReportsHSupport h) {
        this.jdbc = jdbc;
        this.ctx = ctx;
        this.dv = dv;
        this.h = h;
    }

    // ===================================================================================== shared helpers

    private UserAccount user() { return ctx.requireAccountingUser(); }

    private static LocalDate date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        return LocalDate.parse(s.trim().substring(0, 10));
    }

    private static java.sql.Date sqlDate(LocalDate d) { return d == null ? null : java.sql.Date.valueOf(d); }

    private static LocalDate requireDate(String s, String caption) {
        LocalDate d;
        try { d = date(s); } catch (Exception e) { d = null; }
        if (d == null) throw new IllegalArgumentException(caption + " is not a valid date.");
        return d;
    }

    /** DECIMAL as exact text, dates as local yyyy-MM-dd (a UTC instant would shift a day); binary cells (logos) are dropped. */
    private static List<Map<String, Object>> clean(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof byte[]) continue;
                o.put(e.getKey(), cell(v));
            }
            out.add(o);
        }
        return out;
    }

    private static Object cell(Object v) {
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        if (v instanceof Timestamp) {
            Timestamp t = (Timestamp) v;
            java.time.LocalDateTime l = t.toLocalDateTime();
            return l.toLocalTime().toSecondOfDay() == 0 && l.getNano() == 0 ? l.toLocalDate().toString() : l.toString().replace('T', ' ');
        }
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        return v;
    }

    private static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return (int) Math.round(Double.parseDouble(o.toString().trim())); } catch (NumberFormatException e) { return 0; }
    }

    /** GetDecimalConfiguration() switch: 1..4 -> that many zeros in the format, anything else -> none. */
    private int decimalPlaces(String description) {
        int p = toInt(dv.config(description));
        return p >= 1 && p <= 4 ? p : 0;
    }

    /** The three formats the FCY screens read: stringFormatbothForFcy, DecimalFCYRateFormate and stringFormatboth. */
    private Map<String, Object> decimals() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("fcyDecimals", decimalPlaces("DefaultNoOfDecimalPointsForFcyAmount"));
        m.put("fcyRateDecimals", decimalPlaces("DefaultNoOfDecimalPointsForFcyRate"));
        m.put("amountDecimals", decimalPlaces("Default NoofDecimal Points For Amount"));
        return m;
    }

    private static Map<String, Object> idName(Object id, String key, Object name) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", toInt(id));
        o.put(key, name == null ? "" : String.valueOf(name));
        return o;
    }

    // ===================================================================================== 60 FCY general ledger

    /** VoucherValidation_Load: AllowEditOnVoucherNoinReports, AccountTitleFill, ActiveYr.Start_Period. */
    public Map<String, Object> fcyGlLookups() {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>(decimals());
        /* AccountTitleFill(): CoaAllocationAccountTitleByAccountTypeIds("22", null, 0) -> BindDDL(Id, AccountTitle, "Account Title", ZeroIndex false) */
        List<Map<String, Object>> accounts = new ArrayList<>();
        for (Map<String, Object> r : dv.coaAccountTitleByAccountTypeIds("22", 0)) accounts.add(idName(r.get("Id"), "AccountTitle", r.get("AccountTitle")));
        out.put("accounts", accounts);
        /* VoucherNo is a link column when UserAccount.RoleName == "Admin" or AllowEditOnVoucherNoinReports is true */
        boolean admin = "Admin".equals(ctx.currentRoleName());
        out.put("allowVoucherLink", admin || DesktopVoucherSupport.toBool(dv.config("AllowEditOnVoucherNoinReports")));
        out.put("yearStart", h.financialYearStart());
        return out;
    }

    /**
     * btnshow_Click. mode "exports": FCY_LedgerAgainstAccountId -> SPU_Accounts_FCYCustomerLedger_Rpt (@OrganizationId, @CompanyId,
     * @FromDate, @ToDate, @AccountId; @AccountTypeId is 0 so never sent). mode "accounts": Accounts_FcyGeneralLedger_Rpt ->
     * Usp_Accounts_FcyGeneralLedger_Rpt (@FinancialYearId = 0 as the desktop's unset ReportsParameters sends it, @OrganizationId,
     * @CompanyId, @FromDate, @EndDate, @AccountId; ApprovedFilter "All" so @IsApproved is not sent; @AccountIds is a null string, omitted).
     */
    public List<Map<String, Object>> fcyGlData(String mode, int accountId, String from, String to) {
        UserAccount u = user();
        if (accountId == 0) throw new IllegalArgumentException("Account Title Required");
        LocalDate f = requireDate(from, "From Date"), t = requireDate(to, "To Date");
        if ("exports".equals(mode)) {
            return clean(DesktopProc.rows(jdbc, "SPU_Accounts_FCYCustomerLedger_Rpt", DesktopProc.params(
                    "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                    "FromDate", sqlDate(f), "ToDate", sqlDate(t), "AccountId", accountId)));
        }
        if ("accounts".equals(mode)) {
            return clean(DesktopProc.rows(jdbc, "Usp_Accounts_FcyGeneralLedger_Rpt", DesktopProc.params(
                    "FinancialYearId", 0, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                    "FromDate", sqlDate(f), "EndDate", sqlDate(t), "AccountId", accountId)));
        }
        throw new IllegalArgumentException("Select From Exports or From Accounts");
    }

    // ===================================================================================== 85 FCY bank charges register

    /** ReceiverAccountFill / FDBCNoFill / ChargesAccountFill: USP_GetDataForDropDownFromFcyBankReceipts by Activity, rows (Id, ReferenceName). */
    public Map<String, Object> bankChargesLookups() {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>(decimals());
        out.put("bankAccounts", dropDown(u, "ReceiverAccount"));
        out.put("fdbcNos", dropDown(u, "FDBCNo"));
        out.put("chargesAccounts", dropDown(u, "ChargesAccount"));
        return out;
    }

    private List<Map<String, Object>> dropDown(UserAccount u, String activity) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromFcyBankReceipts", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", activity))) {
            out.add(idName(r.get("Id"), "Name", r.get("ReferenceName")));
        }
        return out;
    }

    /** GridBind: @BankId, @ChargesAccountId and @FDBCNo (the combo's TEXT) are sent only when non-zero / non-empty. */
    public List<Map<String, Object>> bankChargesData(int bankId, int chargesAccountId, String fdbcNo) {
        UserAccount u = user();
        return clean(DesktopProc.rows(jdbc, "[dbo].[usp_FcyBankCharges_Register]", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BankId", bankId != 0 ? bankId : null,
                "ChargesAccountId", chargesAccountId != 0 ? chargesAccountId : null,
                "FDBCNo", fdbcNo == null || fdbcNo.isEmpty() ? null : fdbcNo)));
    }

    // ===================================================================================== 86 Profit & Loss 02

    /** VoucherValidation_Load: features 17 / 18, BranchFill (GetBranchesFromVouchersByAccountId), UserAccount.BranchName, ActiveYr.Start_Period, amount format. */
    public Map<String, Object> profitLossLookups() {
        user();
        Map<String, Object> out = new LinkedHashMap<>(h.branchContext());
        out.put("amountDecimals", h.amountDecimals());
        out.put("yearStart", h.financialYearStart());
        return out;
    }

    /**
     * GridBind: VoucherReports.ProftLoss -> SpAccounts_ProfitLoassFormatA_Report (@OrganizationId, @CompanyId, @UserId, @FromDate, @ToDate,
     * @BranchesIds only when GetBranchesIdsByFeature returned one; @AccountNoteId is 0 so never sent).
     */
    public List<Map<String, Object>> profitLossData(String from, String to, String branchText, Integer branchId) {
        UserAccount u = user();
        LocalDate f = requireDate(from, "From Date"), t = requireDate(to, "To Date");
        String ids = h.branchIdsByFeature(branchText, branchId);
        return clean(DesktopProc.rows(jdbc, "SpAccounts_ProfitLoassFormatA_Report", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "UserId", ctx.currentUserId(),
                "FromDate", sqlDate(f), "ToDate", sqlDate(t), "BranchesIds", ids.isEmpty() ? null : ids)));
    }

    /**
     * print_Click_1: VoucherReports.Accounts_ProfitLoss_ForCrystal -> SpAccounts_ProfitLoss_ForCrystal (@OrganizationId, @CompanyId,
     * @ClassIds "4,5", @UserId, @FromDate, @ToDate, @BranchesIds when non-empty).
     */
    public List<Map<String, Object>> profitLossPrintRows(String from, String to, String branchText, Integer branchId) {
        UserAccount u = user();
        LocalDate f = requireDate(from, "From Date"), t = requireDate(to, "To Date");
        String ids = h.branchIdsByFeature(branchText, branchId);
        return clean(DesktopProc.rows(jdbc, "SpAccounts_ProfitLoss_ForCrystal", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ClassIds", "4,5", "UserId", ctx.currentUserId(),
                "FromDate", sqlDate(f), "ToDate", sqlDate(t), "BranchesIds", ids.isEmpty() ? null : ids)));
    }

    // ===================================================================================== 67 Customer voucher report

    /** CashBankFill: VoucherHead.GetCashAndBanksAccountByDate -> Sp_Vouchers_GetMethods @ToDate, @Activity='GetCashAndBanksAccountByDate'. */
    public List<Map<String, Object>> cashBanks(String onDate) {
        return byDate(onDate, "GetCashAndBanksAccountByDate");
    }

    /** PartyAccountFill: VoucherHead.GetPartiesFromVouchersByDate -> Sp_Vouchers_GetMethods @ToDate, @Activity='GetPartiesFromVouchersByDate'. */
    public List<Map<String, Object>> parties(String onDate) {
        return byDate(onDate, "GetPartiesFromVouchersByDate");
    }

    private List<Map<String, Object>> byDate(String onDate, String activity) {
        UserAccount u = user();
        LocalDate d = requireDate(onDate, "Date");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ToDate", sqlDate(d), "Activity", activity))) {
            out.add(idName(r.get("Id"), "AccountTitle", r.get("AccountTitle")));
        }
        return out;
    }

    /** Date, decimals and the load-time context (VoucherValidation_Load). */
    public Map<String, Object> voucherSlipLookups() {
        user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("today", LocalDate.now().toString());
        return out;
    }

    /**
     * btnshow_Click: VoucherReports.VoucherSlipPartyWise -> Usp_VouchersPaymentReceiptSlip_Report: @OrganizationId, @CompanyId,
     * @FromDate = @ToDate = the date, @AccountId (party), @SlipFlag Payment / Receipt, @CashVouchers 'Cash', @BankVouchers 'Bank',
     * @AccountIds (",id,id") only when the Bank &amp; Cash combo has text; @DocumentTypeId is 0 so never sent.
     */
    public List<Map<String, Object>> voucherSlipData(String onDate, int partyId, String slipFlag, boolean cash, boolean bank, String accountIds) {
        UserAccount u = user();
        /* FormValidation() */
        if (partyId == 0) throw new IllegalArgumentException("Party Account Field is Required");
        if (!cash && !bank) throw new IllegalArgumentException("Selection Of Cash Or Bank Field is Required");
        LocalDate d = requireDate(onDate, "Date");
        return clean(DesktopProc.rows(jdbc, "Usp_VouchersPaymentReceiptSlip_Report", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FromDate", sqlDate(d), "ToDate", sqlDate(d), "AccountId", partyId != 0 ? partyId : null,
                "SlipFlag", slipFlag == null || slipFlag.isEmpty() ? null : slipFlag,
                "CashVouchers", cash ? "Cash" : null, "BankVouchers", bank ? "Bank" : null,
                "AccountIds", accountIds == null || accountIds.isEmpty() ? null : accountIds)));
    }
}
