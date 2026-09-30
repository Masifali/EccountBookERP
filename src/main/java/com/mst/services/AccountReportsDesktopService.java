package com.mst.services;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.text.SimpleDateFormat;
import java.util.*;

/**
 * Account Reports (module 3) - desktop-exact backend for the screens rechecked on 2026-09-30
 * (group F): 79 GeneralLedger, 49 CustomerLedger (info box only), 51 TrialBalance,
 * 81 SelectedTrialBalance, 82 SelectedTrialBalanceAllLevel (frmTrialBalancesAllLevel),
 * 53 VoucherReport, 52 ChartOfAccount (CoaTitleChange), 69 ActicitySummery, 73 BankBalances,
 * 74 CashBalances (frmCashBalance).
 *
 * Every call below is the desktop's own procedure with the desktop BLL's own parameter list
 * (read from 0141_Architecture.BLL.Reports.Accounts.VoucherReports.cs and the lookup BLLs).
 * A parameter the BLL only adds "when != 0 / not empty" is passed as null here, and
 * DesktopProc omits nulls from the EXEC exactly as ADO.NET does, so the procedure default
 * applies. Tenancy (organization, company, user, financial year, app) always comes from
 * CurrentUserContext, never from the request. There are no raw-SQL fallbacks: a failing
 * procedure surfaces its error, as the desktop's MessageBox does.
 */
@Service
public class AccountReportsDesktopService {

    @Autowired
    private JdbcTemplate jdbc;

    @Autowired
    private CurrentUserContext ctx;

    // ------------------------------------------------------------------ helpers

    private List<Map<String, Object>> rows(String proc, Map<String, Object> p) {
        return DesktopProc.rows(jdbc, proc, p);
    }

    private static Map<String, Object> p(Object... kv) {
        return DesktopProc.params(kv);
    }

    /** yyyy-MM-dd (or an ISO date-time) to java.sql.Date; blank = null (omitted). */
    public static java.sql.Date date(String s) {
        if (s == null || s.isBlank()) return null;
        String t = s.trim();
        if (t.length() > 10) t = t.substring(0, 10);
        return java.sql.Date.valueOf(t);
    }

    /** ReportsParameters int semantics: 0 means "not set" and the BLL leaves the parameter out. */
    private static Integer nz(Integer v) {
        return v == null || v == 0 ? null : v;
    }

    private static String ns(String s) {
        return s == null || s.isEmpty() ? null : s;
    }

    private static double dbl(Object o) {
        if (o == null) return 0.0;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(o.toString().trim()); } catch (Exception e) { return 0.0; }
    }

    private static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        try { return (int) Double.parseDouble(o.toString().trim()); } catch (Exception e) { return 0; }
    }

    private static String str(Object o) {
        return o == null ? "" : o.toString();
    }

    private static String shortDate(Object o) {
        if (o == null) return "";
        if (o instanceof java.util.Date) return new SimpleDateFormat("yyyy-MM-dd").format((java.util.Date) o);
        String s = o.toString();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    /** Conversion.CheckDateTimeNull: null or the CLR MinValue/1900 sentinel counts as "no date". */
    private static boolean nullDate(Object o) {
        if (o == null) return true;
        String s = shortDate(o);
        return s.isEmpty() || s.startsWith("0001-") || s.startsWith("1900-01-01");
    }

    private int org() { return ctx.currentOrganizationId(); }
    private int comp() { return ctx.currentCompanyId(); }
    private int user() { return ctx.currentUserId(); }
    private int finYear() { return ctx.currentFinancialYearId(); }

    // ------------------------------------------------------------------ globals / features

    /** clsGlobalVariables.ActiveYr: Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId, the row whose Id is the active year. */
    public Map<String, Object> activeYear() {
        Map<String, Object> out = new LinkedHashMap<>();
        int fy = finYear();
        List<Map<String, Object>> years = rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                p("OrganizationId", org(), "CompanyId", comp()));
        Map<String, Object> row = null;
        for (Map<String, Object> r : years) if (toInt(r.get("Id")) == fy) { row = r; break; }
        if (row == null && !years.isEmpty()) row = years.get(0);
        out.put("id", fy);
        out.put("startPeriod", row == null || nullDate(row.get("Start_Period")) ? null : shortDate(row.get("Start_Period")));
        out.put("endPeriod", row == null || nullDate(row.get("End_Period")) ? null : shortDate(row.get("End_Period")));
        return out;
    }

    /** CommonServices.GetERPFeatureById: membership in USP_GetERPFeaturesByCompanyId. */
    public Set<Integer> erpFeatures() {
        Set<Integer> ids = new TreeSet<>();
        for (Map<String, Object> r : rows("USP_GetERPFeaturesByCompanyId", p("OrganizationId", org(), "CompanyId", comp())))
            ids.add(toInt(r.get("Id")));
        return ids;
    }

    /** Values the forms read from UserAccount / clsGlobalVariables on load. */
    public Map<String, Object> formContext() {
        Map<String, Object> out = new LinkedHashMap<>();
        Set<Integer> f = erpFeatures();
        out.put("activeYear", activeYear());
        out.put("subsidiaryFeature", f.contains(4));      // GetERPFeatureById(4)  SubsidiaryAccountAllownOnVouchers
        out.put("branchFeature", f.contains(17));         // GetERPFeatureById(17) BranchFeature
        out.put("branchConsolidated", f.contains(18));    // GetERPFeatureById(18) BranchFeatureConsolidated
        int branchId = 0, appId = 0;
        try { branchId = ctx.currentBranchId(); } catch (RuntimeException e) { /* user without a branch: no default branch */ }
        try { appId = ctx.currentAppId(); } catch (RuntimeException e) { /* no application chosen: AppId 0 */ }
        out.put("userBranchId", branchId);                // CmbBranches.Text = UserAccount.BranchName
        out.put("appId", appId);
        out.put("roleName", ctx.currentRoleName());
        // CommonServices.GetDecimalConfiguration: stringFormatsingle/stringFormatboth use this many decimals.
        out.put("amountDecimals", toInt(config("Default NoofDecimal Points For Amount")));
        out.put("allowEditOnVoucherNoinReports", configBool("AllowEditOnVoucherNoinReports"));
        return out;
    }

    /**
     * ConfigrationsAllocation.GetConfigurationByOrgCompandConfigDescription -> Sp_ConfigrationsAllocation_GetAllMethod
     * @Activity='GetConfigurationByOrgCompandConfigDescription'; first row's ConfigKey, "" when none.
     */
    public String config(String description) {
        List<Map<String, Object>> r = rows("Sp_ConfigrationsAllocation_GetAllMethod", p("OrganizationId", org(),
                "CompanyId", comp(), "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        return r.isEmpty() || r.get(0).get("ConfigKey") == null ? "" : r.get(0).get("ConfigKey").toString().trim();
    }

    /** Conversion.ToBool: "1" or "true" (any case) is true. */
    public boolean configBool(String description) {
        String v = config(description);
        return "1".equals(v) || "true".equalsIgnoreCase(v);
    }

    /** GeneralLedger_Load's GetMultipleConfigurationsByConfigDescriptions keys, read one by one. */
    public Map<String, Object> generalLedgerConfig() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("defaulLedgerDetail", configBool("DefaulLedgerDetail"));
        out.put("defaulLedgerSummaryOne", configBool("DefaulLedgerSummaryOne"));
        out.put("defaulLedgerSummaryTwo", configBool("DefaulLedgerSummaryTwo"));
        out.put("filterRecordsByDocType", configBool("FilterRecordsByDocumentTypeOnGeneralLedger"));
        out.put("isStockReservedPerParty", configBool("IsStockReservedPerParty"));
        return out;
    }

    // ------------------------------------------------------------------ lookups (DDL sources)

    /** CommonServices.MultiLanguagesGetAll -> Sp_MultiLanguages_GetAll @MethodType='ReadAll'. Id / LanguageDescription. */
    public List<Map<String, Object>> languages() {
        return rows("Sp_MultiLanguages_GetAll", p("MethodType", "ReadAll", "OrganizationId", org(), "CompanyId", comp()));
    }

    /** Projects.GetSubCostCenters(org, comp, user, AppId, 0) -> usp_getCostCenters. Id / CostCenterName. */
    public List<Map<String, Object>> costCenters() {
        return rows("usp_getCostCenters", p("OrganizationId", org(), "CompanyId", comp(), "UserId", user(),
                "AppId", nz(ctx.currentAppId())));
    }

    /** CommonServices.GetAllDetailAccount(CostCenterId) -> ChartofAccount.DetailAccount -> Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='DetailAccount'. */
    public List<Map<String, Object>> detailAccounts(Integer costCenterId) {
        return rows("Sp_ChartofAccount_GetAllMethodFromCOA", p("OrganizationId", org(), "CompanyId", comp(),
                "UsersId", user(), "AppId", ctx.currentAppId(), "CostCenterId", nz(costCenterId), "CoaType", "DetailAccount"));
    }

    /** VoucherHead.GetSubsidiaryAccountFromVouchers -> USP_GetSubsidiaryAccountFromVouchers. SubsidiaryAccountId / SubsidaryAccount / SubsidiaryTypeId. */
    public List<Map<String, Object>> subsidiaryAccounts(Integer costCenterId, Integer accountId) {
        return rows("USP_GetSubsidiaryAccountFromVouchers", p("OrganizationId", org(), "CompanyId", comp(),
                "UserId", user(), "AppId", ctx.currentAppId(), "AccountId", nz(accountId), "CostCenterId", nz(costCenterId)));
    }

    /** VoucherHead.GetBranchesFromVouchersByAccountId(org, comp, "", 0) -> USP_GetBranchesFromVouchersByAccountId. Id / BranchName. */
    public List<Map<String, Object>> branches() {
        return rows("USP_GetBranchesFromVouchersByAccountId", p("OrganizationId", org(), "CompanyId", nz(comp())));
    }

    /** VoucherHead.GetDocumentTypesFromVouchers -> Sp_Vouchers_GetMethods @Activity='GetDocumentTypesFromVouchers'. Id / DocumentTypeDescription. */
    public List<Map<String, Object>> documentTypes() {
        return rows("Sp_Vouchers_GetMethods", p("OrganizationId", org(), "CompanyId", comp(), "Activity", "GetDocumentTypesFromVouchers"));
    }

    /** CommonServices.CustomeGroupsDefine(1) -> AcLookUps.GetAll -> Sp_AcLookUps_GetAllMethod @Activity='ReadAll'. Id / AcLookUpsDescription. */
    public List<Map<String, Object>> customGroups() {
        return rows("Sp_AcLookUps_GetAllMethod", p("OrganizationId", org(), "CompanyId", comp(), "AcLookUpTypesId", 1, "Activity", "ReadAll"));
    }

    /** ChartofAccount.ReadAllAccountgroup(org, comp, ActiveYr.Id) -> Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='ReadAllAccountGroup'. Id / AccountTitle / Account_Level. */
    public List<Map<String, Object>> accountGroups() {
        return rows("Sp_ChartofAccount_GetAllMethodFromCOA", p("OrganizationId", org(), "CompanyId", comp(),
                "FinancialYearId", finYear(), "CoaType", "ReadAllAccountGroup"));
    }

    /** CommonServices.CityGetAllService -> City.GetAll -> SP_City_GetAllMethod @MethodType='GetAll'. Id / CityName. */
    public List<Map<String, Object>> cities() {
        return rows("SP_City_GetAllMethod", p("OrganizationId", org(), "CompanyId", comp(), "MethodType", "GetAll"));
    }

    /** CommonServices.SupplierCustomerGetforComboServiceBind -> SupplierCustomer.GetforComboBinding -> Sp_SupplierCustomer_GetAllMethod @Activity='ReadByOrganizationIdCompanyIdForBinding'. Id / CompanyName / GlAccountId. */
    public List<Map<String, Object>> supplierCustomers() {
        return rows("Sp_SupplierCustomer_GetAllMethod", p("OrganizationId", org(), "CompanyId", comp(),
                "Activity", "ReadByOrganizationIdCompanyIdForBinding"));
    }

    /** CommonServices.CoaAllocationGetAllServiceBind -> COAAllocation.GetAll -> Sp_COAAllocation_GetAllMethod @Activity='COAAllocationSearch'. Id / AccountTitle. */
    public List<Map<String, Object>> coaAllocation() {
        return rows("Sp_COAAllocation_GetAllMethod", p("OrganizationId", org(), "CompanyId", comp(), "UserId", nz(user()),
                "Activity", "COAAllocationSearch"));
    }

    /** CommonServices.Vouchers_GetLastRecord(AccountTypeId) -> USP_Vouchers_GetLastRecord; first row's voucherdate. */
    public String lastRecordDate(int accountTypeId) {
        List<Map<String, Object>> r = rows("[dbo].[USP_Vouchers_GetLastRecord]", p("OrganizationId", org(), "CompanyId", comp(),
                "FinancialYearId", finYear(), "AccountTypeId", accountTypeId));
        if (r.isEmpty() || nullDate(r.get(0).get("voucherdate"))) return null;
        return shortDate(r.get(0).get("voucherdate"));
    }

    // ------------------------------------------------------------------ account info box (GL + Party Ledger)

    /**
     * LedgerDetailByAccountId (GeneralLedger.cs / CustomerLedger.cs):
     * ChartofAccount.GetCoaDetailByAccountId -> Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='GetCoaDetailByAccountId' (@Id),
     * then CommonServices.GetAccountsBalancesOpeningCurrentAndClosing(AccountId, From, To, CostCenterId) ->
     * Sp_Vouchers_GetMethods @Activity='GetAccountsBalancesOpeningCurrentAndClosing' (@FromDate/@ToDate always, @CostCenterId when != 0).
     */
    public Map<String, Object> accountInfo(Integer accountId, String fromDate, String toDate, Integer costCenterId) {
        Map<String, Object> out = new LinkedHashMap<>();
        int id = accountId == null ? 0 : accountId;
        List<Map<String, Object>> coa = rows("Sp_ChartofAccount_GetAllMethodFromCOA", p("OrganizationId", org(), "CompanyId", comp(),
                "Id", id, "CoaType", "GetCoaDetailByAccountId"));
        out.put("coa", coa.isEmpty() ? null : coa.get(0));
        List<Map<String, Object>> bal = rows("Sp_Vouchers_GetMethods", p("OrganizationId", org(), "CompanyId", comp(),
                "AccountId", id, "FromDate", date(fromDate), "ToDate", date(toDate), "CostCenterId", nz(costCenterId),
                "Activity", "GetAccountsBalancesOpeningCurrentAndClosing"));
        out.put("balances", bal.isEmpty() ? null : bal.get(0));
        return out;
    }

    // ------------------------------------------------------------------ 79 General Ledger (single account tab)

    /**
     * GeneralLedger.btnshow_Click. mode "detail" -> VoucherReports.GeneralLedgerWithOffsetAccount
     * (Sp_Accounts_GeneralLedger_Rpt); "summary1" -> GeneralLedgerSummery (Sp_VouchersAccountsGeneralLedgerSummery);
     * "summary2" -> GeneralLedger2Format_Rpt (SpAccounts_GeneralLedger2Format_Rpt).
     * includeUnposted = ChkBoxPost: unchecked -> IsApproved=true is sent; checked -> ApprovedFilter "All", @IsApproved left out.
     */
    public Map<String, Object> generalLedger(String mode, Integer accountId, String fromDate, String toDate,
            boolean includeUnposted, Integer languageId, Integer subsidiaryAccountId, Integer subsidiaryTypeId,
            Integer branchId, Integer costCenterId) {
        Map<String, Object> ctxForm = formContext();
        boolean branchFeature = Boolean.TRUE.equals(ctxForm.get("branchFeature"));
        if (accountId == null || accountId == 0) throw new IllegalArgumentException("Account Title Field is Required");
        int appId = toInt(ctxForm.get("appId"));
        if (appId == 5 && (costCenterId == null || costCenterId == 0)) throw new IllegalArgumentException("Cost Center Not Found");
        Integer branchesId = branchFeature ? nz(branchId) : null;
        Integer subId = nz(subsidiaryAccountId);
        Integer subType = subId == null ? null : nz(subsidiaryTypeId);
        Boolean isApproved = includeUnposted ? null : Boolean.TRUE;

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("mode", mode);
        List<Map<String, Object>> raw;
        if ("summary1".equals(mode)) {
            raw = rows("Sp_VouchersAccountsGeneralLedgerSummery", p("FinancialYearId", finYear(), "OrganizationId", org(),
                    "CompanyId", comp(), "BranchesId", branchesId, "VoucherDateF", date(fromDate), "VoucherDateT", date(toDate),
                    "AccountId", accountId, "IsApproved", isApproved, "SubsidiaryAccountId", subId, "SubsidiaryTypeId", subType,
                    "LanguageId", nz(languageId), "CostCenterId", nz(costCenterId)));
            List<Map<String, Object>> grid = new ArrayList<>();
            for (Map<String, Object> r : raw) {
                Map<String, Object> g = new LinkedHashMap<>();
                g.put("BranchName", r.get("BranchName"));
                g.put("VoucherDate", shortDate(r.get("VoucherDate")));
                g.put("DocType", r.get("DocumentTypeDescription"));
                g.put("VoucherCode", r.get("VoucherCode"));
                g.put("ManualBillNo", r.get("ManualBillNo"));
                g.put("AccountId", r.get("AccountId"));
                g.put("AccountTitle", r.get("AccountTitle"));
                g.put("Remarks", r.get("Remarks"));
                g.put("DebitAmount", dbl(r.get("DebitAmount")));
                g.put("CreditAmount", dbl(r.get("CreditAmount")));
                g.put("RunningBalance", dbl(r.get("RunningBalance")));
                g.put("DueDate", nullDate(r.get("DueDate")) ? "" : shortDate(r.get("DueDate")));
                g.put("DueDays", r.get("DueDays"));
                g.put("IsApproved", r.get("IsApproved"));
                g.put("CurrentDR/CrDiff", dbl(r.get("DebitAmount")) - dbl(r.get("CreditAmount")));
                g.put("VehicleNo", r.get("VehicleNo"));
                g.put("VoucherHeadId", r.get("Id"));
                g.put("DocumentTypeId", r.get("DocumentTypeId"));
                g.put("BaseDocumentTypeId", r.get("BaseDocumentTypeId"));
                g.put("DocumentTypeSrNo", r.get("DocumentTypeSrNo"));
                grid.add(g);
            }
            out.put("rows", grid);
        } else if ("summary2".equals(mode)) {
            raw = rows("SpAccounts_GeneralLedger2Format_Rpt", p("OrganizationId", org(), "CompanyId", comp(),
                    "BranchesId", branchesId, "FromDate", date(fromDate), "ToDate", date(toDate), "AccountId", accountId,
                    "LanguageId", nz(languageId), "IsApproved", isApproved, "SubsidiaryAccountId", subId,
                    "SubsidiaryTypeId", subType, "CostCenterId", nz(costCenterId)));
            List<Map<String, Object>> grid = new ArrayList<>();
            double runningOpening = 0.0;
            for (int i = 0; i < raw.size(); i++) {
                Map<String, Object> r = raw.get(i);
                if (i == 0) runningOpening = dbl(r.get("RunningBalance"));
                Map<String, Object> g = new LinkedHashMap<>();
                g.put("BranchName", r.get("BranchName"));
                g.put("VoucherDate", shortDate(r.get("VoucherDate")));
                g.put("VoucherType", r.get("VoucherType"));
                g.put("VoucherNo", r.get("VoucherNo"));
                g.put("AccountTitle", r.get("AccountTitle"));
                g.put("OffSetAccountTitle", r.get("OffSetAccountTitle"));
                g.put("Remarks", r.get("Remarks"));
                g.put("DebitAmount", dbl(r.get("DebitAmount")));
                g.put("CreditAmount", dbl(r.get("CreditAmount")));
                g.put("Balance", r.get("RunningBalance"));
                // desktop: RunningBalance column = RunningOpening + Debit - Credit, with RunningOpening seeded from row 0.
                g.put("RunningBalance", runningOpening + dbl(r.get("DebitAmount")) - dbl(r.get("CreditAmount")));
                g.put("ChequeDate", nullDate(r.get("ChequeDate")) ? "" : shortDate(r.get("ChequeDate")));
                g.put("ChequeNo", r.get("ChequeNo"));
                g.put("PayeeTitle", r.get("PayeeTitle"));
                g.put("DueDate", nullDate(r.get("DueDate")) ? "" : shortDate(r.get("DueDate")));
                g.put("DueDays", r.get("DueDays"));
                g.put("VoucherHeadId", r.get("VoucherHeadId"));
                g.put("DocumentTypeId", r.get("DocumentTypeId"));
                g.put("BaseDocumentTypeId", r.get("BaseDocumentTypeId"));
                g.put("DocumentTypeSrNo", r.get("DocumentTypeSrNo"));
                g.put("NoOfAttachments", r.get("NoOfAttachments"));
                g.put("AccountId", r.get("AccountId"));
                grid.add(g);
                if (i != 0) runningOpening = runningOpening + dbl(r.get("DebitAmount")) - dbl(r.get("CreditAmount"));
            }
            out.put("rows", grid);
        } else {
            raw = rows("Sp_Accounts_GeneralLedger_Rpt", p("FinancialYearId", finYear(), "OrganizationId", org(),
                    "CompanyId", comp(), "BranchesId", branchesId, "FromDate", date(fromDate), "EndDate", date(toDate),
                    "AccountId", accountId, "IsApproved", isApproved, "EntryUser", nz(user()), "LanguageId", nz(languageId),
                    "SubsidiaryAccountId", subId, "SubsidiaryTypeId", subType, "CostCenterId", nz(costCenterId)));
            if (raw.isEmpty()) throw new IllegalStateException("Record Not Found For Display");
            List<Map<String, Object>> grid = new ArrayList<>();
            for (Map<String, Object> r : raw) {
                double bal = dbl(r.get("Balance"));
                Map<String, Object> g = new LinkedHashMap<>();
                g.put("DetailId", r.get("DetailId"));
                g.put("Checked", toInt(r.get("BookmarkStatus")) == 1);
                g.put("BookmarkRemarks", r.get("BookmarkRemarks"));
                g.put("BranchName", r.get("BranchName"));
                g.put("VoucherDate", shortDate(r.get("VoucherDate")));
                g.put("DocType", r.get("DocTypeCode"));
                g.put("VoucherNo", r.get("VoucherCode"));
                g.put("AccountCode", r.get("AccountCode"));
                g.put("OffSetTitle", r.get("OffsetAccountTitle"));
                g.put("SubsidiaryAccount", r.get("SubsidiaryAccount"));
                g.put("Debit", dbl(r.get("DebitAmount")));
                g.put("Credit", dbl(r.get("CreditAmount")));
                g.put("Balance", bal);
                g.put("BalType", bal > 0 ? "Dr" : (bal < 0 ? "Cr" : "Nill"));
                g.put("BillAmount", r.get("BillAmount"));
                g.put("DueDays", r.get("DueDays"));
                g.put("DueDate", nullDate(r.get("DueDate")) ? "" : shortDate(r.get("DueDate")));
                g.put("Comments", r.get("Comments"));
                g.put("ChequeNo", r.get("CheqNoDetail"));
                g.put("ChequeDate", nullDate(r.get("CheqDate")) ? "" : shortDate(r.get("CheqDate")));
                g.put("PayeeTitle", r.get("PayeeTitle"));
                g.put("RefParty", r.get("RefParty"));
                g.put("JobLot", r.get("JobLot"));
                g.put("MannualNo", r.get("MannualNo"));
                g.put("RefNo", r.get("RefInvoiceNo"));
                g.put("StockStatus", r.get("StockReservedStatus"));
                g.put("QtyIn", r.get("QtyIn"));
                g.put("QtyOut", r.get("QtyOut"));
                g.put("ItemRate", r.get("ItemRate"));
                g.put("VehicleNo", r.get("VehcileNo"));
                g.put("NoOfAttachments", r.get("NoOfAttachments"));
                g.put("VoucherHeadId", r.get("VoucherHeadId"));
                g.put("DocumentTypeId", r.get("DocumentTypeId"));
                g.put("DocumentTypeSrNo", r.get("DocumentTypeSrNo"));
                g.put("IsApproved", r.get("IsApproved"));
                g.put("BaseDocumentTypeId", r.get("BaseDocumentTypeId"));
                g.put("AccountId", r.get("AccountId"));
                grid.add(g);
            }
            out.put("rows", grid);
        }
        if (!raw.isEmpty()) {
            out.put("postDatedCheqAmount", dbl(raw.get(0).get("PostDatedCheqAmount")));
            out.put("postDatedNoOfCheqs", dbl(raw.get(0).get("PostDatedNoOfCheqs")));
        }
        // ChartofAccount.getGLBalanceBranchesWiseByAccountId, only when BranchFeature (all params always sent).
        if (branchFeature) {
            out.put("branchBalances", rows("usp_getGLBalanceBranchesWiseByAccountId", p("OrganizationId", org(),
                    "CompanyId", comp(), "AccountId", accountId, "FromDate", date(fromDate), "ToDate", date(toDate))));
        }
        return out;
    }

    /**
     * btnSaveCheckedRows_Click -> VoucherHead.InsertBookmarkStatus: USP_InsertBookmarkStatus
     * (@BookmarkStatus, @BookmarkRemarks, @Id) once per grid row that has a DetailId, checked or not.
     * Only DetailIds that this user's own last ledger query returned are accepted (tenancy guard).
     */
    public int saveBookmarks(List<Map<String, Object>> items, Set<Integer> allowedDetailIds) {
        if (items == null || items.isEmpty()) throw new IllegalArgumentException("No Rows Checked");
        boolean anyChecked = false;
        for (Map<String, Object> it : items) if (Boolean.parseBoolean(String.valueOf(it.get("checked")))) anyChecked = true;
        if (!anyChecked) throw new IllegalArgumentException("No Rows Checked");
        int n = 0;
        for (Map<String, Object> it : items) {
            int id = toInt(it.get("detailId"));
            if (id <= 0) continue;
            if (allowedDetailIds == null || !allowedDetailIds.contains(id))
                throw new SecurityException("Row " + id + " is not part of the ledger that was shown.");
            boolean checked = Boolean.parseBoolean(String.valueOf(it.get("checked")));
            Object remarks = it.get("bookmarkRemarks");
            DesktopProc.rows(jdbc, "USP_InsertBookmarkStatus", p("BookmarkStatus", checked,
                    "BookmarkRemarks", remarks == null ? "" : remarks.toString(), "Id", id));
            n++;
        }
        return n;
    }

    // ------------------------------------------------------------------ 51 Trial Balance

    /** TrialBalance.btnshow_Click -> VoucherReports.TrialBalanceReport -> Sp_TrialBalance_Rpt. */
    public List<Map<String, Object>> trialBalance(String fromDate, String toDate, boolean skipZero, double clDebit,
            double clCredit, boolean includeUnposted, Integer languageId, String documentTypeIds, String branchesIds,
            String accountTypeIds, Integer customGroupId, Integer groupAccountId, boolean skipCgs) {
        Map<String, Object> fc = formContext();
        String branchIds = branchIdsByFeature(fc, branchesIds, false);
        return rows("Sp_TrialBalance_Rpt", p("FinancialYearId", finYear(), "OrganizationId", org(), "CompanyId", comp(),
                "LanguageId", languageId == null ? 0 : languageId,            // always sent (CmbLanguage 0 by default)
                "ZeroBalanceType", skipZero ? 1 : 0,                          // always sent
                "UserId", nz(user()), "VoucherDateF", date(fromDate), "VoucherDateT", date(toDate),
                "IsApproved", includeUnposted ? null : Boolean.TRUE,
                "DocumentTypeIds", plainIds(documentTypeIds), "BranchesIds", ns(branchIds),
                "ClDebit", clDebit != 0.0 ? clDebit : null, "ClCredit", clCredit != 0.0 ? clCredit : null,
                "AcTypeIds", plainIds(accountTypeIds), "GroupAccountId", nz(groupAccountId),
                "CustomeGroupId", nz(customGroupId), "SkipCGS", skipCgs ? 1 : null));
    }

    /**
     * InfragisticsHelper.GetBranchesIdsByFeature: consolidated -> the checked ids as ",id,id";
     * branch feature only -> the single selected id, required ("Please Select Branch first!");
     * no branch feature -> "".
     */
    private String branchIdsByFeature(Map<String, Object> fc, String requested) {
        return branchIdsByFeature(fc, requested, true);
    }

    /** leadingComma=false is TrialBalance.btnshow_Click's own GetSelectedIdsFromMultiSelectionCombo ("id,id"). */
    private String branchIdsByFeature(Map<String, Object> fc, String requested, boolean leadingComma) {
        boolean bf = Boolean.TRUE.equals(fc.get("branchFeature"));
        boolean cons = Boolean.TRUE.equals(fc.get("branchConsolidated"));
        String req = requested == null ? "" : requested.trim();
        if (bf && cons) {
            if (req.isEmpty()) return "";
            if (!leadingComma) { String plain = plainIds(req); return plain == null ? "" : plain; }
            StringBuilder sb = new StringBuilder();
            for (String s : req.split(",")) { s = s.trim(); if (!s.isEmpty()) sb.append(',').append(Integer.parseInt(s)); }
            return sb.toString();
        } else if (bf) {
            String first = req.isEmpty() ? "" : req.split(",")[0].trim().replace(",", "");
            if (first.isEmpty() || Integer.parseInt(first) == 0) throw new IllegalArgumentException("Please Select Branch first!");
            return String.valueOf(Integer.parseInt(first));
        }
        return "";
    }

    /** "id,id" with every element parsed as an int (no leading comma). */
    private static String plainIds(String csv) {
        if (csv == null || csv.isBlank()) return null;
        List<String> ids = new ArrayList<>();
        for (String s : csv.split(",")) { s = s.trim(); if (!s.isEmpty()) ids.add(String.valueOf(Integer.parseInt(s))); }
        return ids.isEmpty() ? null : String.join(",", ids);
    }

    /** Desktop multi-select id lists are built as ",id,id" (leading comma), reproduced as sent. */
    private static String leadingCommaIds(String csv) {
        if (csv == null || csv.isBlank()) return null;
        StringBuilder sb = new StringBuilder();
        for (String s : csv.split(",")) { s = s.trim(); if (!s.isEmpty()) sb.append(',').append(Integer.parseInt(s)); }
        return sb.length() == 0 ? null : sb.toString();
    }

    // ------------------------------------------------------------------ 81 Trial Balance Selected

    /** SelectedTrialBalance.btnShowSelectedTrial_Click -> VoucherReports.SelectedTrialBalanceNew -> SpAccounts_TrialBalanceSelectedNew_Report (Tables[0]). */
    public List<Map<String, Object>> selectedTrialBalance(String fromDate, String toDate, double clDebit, double clCredit,
            Integer groupAccountId, Integer cityId, boolean includeUnposted, Integer languageId, Integer customGroupId,
            boolean skipZero, boolean skipCgs, String documentTypeIds, String branchesIds) {
        Map<String, Object> fc = formContext();
        String branchIds = branchIdsByFeature(fc, branchesIds);
        List<Map<String, Object>> raw = rows("SpAccounts_TrialBalanceSelectedNew_Report", p("OrganizationId", org(),
                "CompanyId", comp(), "LanguageId", languageId == null ? 0 : languageId, "UserId", nz(user()),
                "FromDate", date(fromDate), "ToDate", date(toDate), "CityId", nz(cityId), "GroupAccountId", nz(groupAccountId),
                "CustomeGroupId", nz(customGroupId), "ZeroBalanceType", skipZero ? 1 : null, "SkipCgsAccounts", skipCgs ? 1 : null,
                "IsApproved", includeUnposted ? null : Boolean.TRUE, "DocumentTypeIds", leadingCommaIds(documentTypeIds),
                "BranchesIds", ns(branchIds), "ClDebit", clDebit != 0.0 ? clDebit : null, "ClCredit", clCredit != 0.0 ? clCredit : null));
        List<Map<String, Object>> grid = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> g = new LinkedHashMap<>();
            for (String k : new String[]{"AccountId", "BranchesId", "BranchName", "ClassName", "AcLevel"}) g.put(k, r.get(k));
            g.put("ParentTitle", r.get("ParentAccountTitle"));
            for (String k : new String[]{"AccountCode", "AccountTitle", "AccountType"}) g.put(k, r.get(k));
            for (String k : new String[]{"Opening", "OpeningDr", "OpeningCr", "CurrDebit", "CurrCredit", "Closing", "ClosingDr", "ClosingCr"})
                g.put(k, dbl(r.get(k)));
            grid.add(g);
        }
        return grid;
    }

    // ------------------------------------------------------------------ 82 Trial Balance All Levels

    /** frmTrialBalancesAllLevel.GridBind -> VoucherReports.HararicalTrialBalance -> SpCoahierarchy_TrialBalance_Rpt. */
    public List<Map<String, Object>> trialBalanceAllLevels(String fromDate, String toDate, boolean skipZero) {
        return rows("SpCoahierarchy_TrialBalance_Rpt", p("OrganizationId", org(), "CompanyId", comp(),
                "FromDate", date(fromDate), "ToDate", date(toDate), "SkipZero", skipZero ? 1 : null));
    }

    // ------------------------------------------------------------------ 53 Voucher Report

    /**
     * VoucherReport.btnshow_Click -> VoucherReports.VoucherValidationReport -> Sp_Accounts_VouchersValidation_Rpt.
     * dateFilter: doc | entry | modify | approved (rdDocFromToDate / rdEntryFromToDate / rdmodifydate / rdApproveddate).
     * approval: approved | unapproved | all.
     */
    public List<Map<String, Object>> voucherReport(String dateFilter, String fromDate, String toDate, Integer fromDocNo,
            Integer toDocNo, Integer accountId, Integer customGroupId, String manualBillNo, String approval, boolean skipCgs,
            Integer languageId, String documentTypeIds) {
        Map<String, Object> pr = p("OrganizationId", org(), "CompanyId", comp(), "FinancialYearId", nz(finYear()), "UserId", nz(user()),
                "VoucherCodeF", nz(fromDocNo), "VoucherCodeT", nz(toDocNo));
        String f = dateFilter == null ? "doc" : dateFilter;
        if ("entry".equals(f)) { pr.put("EntryDateFrom", date(fromDate)); pr.put("EntryDateTo", date(toDate)); }
        else if ("modify".equals(f)) { pr.put("ModifyFromDate", date(fromDate)); pr.put("ModifyToDate", date(toDate)); }
        else if ("approved".equals(f)) { pr.put("ApprovedFromDate", date(fromDate)); pr.put("ApprovedToDate", date(toDate)); }
        else { pr.put("VoucherDateF", date(fromDate)); pr.put("VoucherDateT", date(toDate)); }
        pr.put("CustomGroupId", nz(customGroupId));
        pr.put("AccountId", nz(accountId));
        pr.put("DocumentTypeIds", leadingCommaIds(documentTypeIds));
        if ("approved".equals(approval)) pr.put("IsApproved", Boolean.TRUE);
        else if ("unapproved".equals(approval)) pr.put("IsApproved", Boolean.FALSE);
        pr.put("ManualBillNo", ns(manualBillNo));
        pr.put("IsCGS", skipCgs ? 1 : null);
        pr.put("LanguageId", nz(languageId));
        List<Map<String, Object>> raw = rows("Sp_Accounts_VouchersValidation_Rpt", pr);
        List<Map<String, Object>> grid = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> g = new LinkedHashMap<>();
            g.put("Id", toInt(r.get("Id")));
            g.put("AccountId", toInt(r.get("AccountId")));
            g.put("DocumentType", str(r.get("DocumentTypeDescription")));
            g.put("DocumentTypeId", toInt(r.get("DocumentTypeId")));
            g.put("DocumentTypeSrNo", toInt(r.get("DocumentTypeSrNo")));
            g.put("VoucherCode", toInt(r.get("VoucherCode")));
            g.put("VoucherDate", shortDate(r.get("VoucherDate")));
            g.put("AccountCode", str(r.get("AccountCode")));
            g.put("AccountTitle", str(r.get("AccountTitle")));
            g.put("AgainstAccountTitle", str(r.get("AccountTitleCoag")));
            g.put("DebitAmount", dbl(r.get("DebitAmount")));
            g.put("CreditAmount", dbl(r.get("CreditAmount")));
            g.put("ChequeNo", str(r.get("CheqNoDetail")));
            g.put("ChequePartyName", str(r.get("ChequePartyName")));
            g.put("ManualBillNo", str(r.get("ManualBillNo")));
            g.put("Comments", str(r.get("Comments")));
            g.put("EntryUser", str(r.get("EntryUserName")));
            g.put("EntryDate", shortDate(r.get("EntryDate")));
            g.put("ModifyUser", str(r.get("ModifyUserName")));
            g.put("ModifyDate", nullDate(r.get("ModifyDate")) ? "" : shortDate(r.get("ModifyDate")));
            g.put("ApprovedUser", str(r.get("ApprovedUserName")));
            g.put("ApprovedDate", nullDate(r.get("PostDate")) ? "" : shortDate(r.get("PostDate")));
            g.put("NoOfAttachments", toInt(r.get("NoOfAttachments")));
            grid.add(g);
        }
        return grid;
    }

    // ------------------------------------------------------------------ 52 Chart of Account (CoaTitleChange)

    /** CoaTitleChange.btnshow_Click -> VoucherReports.ChartofAccount -> Sp_ChartOfAccounts_Rpt. status: all | active | inactive; levels "1,2,3,4". */
    public List<Map<String, Object>> chartOfAccounts(Integer accountId, String customGroupIds, String status, String accountLevels) {
        if (accountLevels == null || accountLevels.isBlank()) throw new IllegalArgumentException("Please Check the Ac Levels First");
        Boolean isActive = null;                       // RadAll -> ApprovedFilter "All" -> @IsActive left out
        if ("active".equals(status)) isActive = Boolean.TRUE;           // !RadInActive && RadActiveOnly
        else if ("inactive".equals(status)) isActive = Boolean.FALSE;
        List<Map<String, Object>> raw = rows("Sp_ChartOfAccounts_Rpt", p("OrganizationId", org(), "CompanyId", comp(),
                "AccountId", nz(accountId), "IsActive", isActive, "AccountLevels", accountLevels.trim(),
                "CustomGroupIds", leadingCommaIds(customGroupIds)));
        List<Map<String, Object>> grid = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> g = new LinkedHashMap<>();
            g.put("Id", toInt(r.get("Id")));
            int parent = toInt(r.get("ParentCodeId"));
            g.put("ParentAccountId", parent == 0 ? null : parent);
            g.put("AccountTitle", str(r.get("AccountTitle")));
            g.put("AccountCode", str(r.get("AccountCode")));
            g.put("AccountGroup", str(r.get("AccountGroup")));
            g.put("Account_Level", toInt(r.get("Account_Level")));
            g.put("Status", str(r.get("IsActive")));
            g.put("AccountClass", str(r.get("AccountClass")));
            g.put("AccountType", str(r.get("AccountType")));
            g.put("NoteTitle", str(r.get("NoteTitle")));
            g.put("LanguageId", toInt(r.get("LanguageId")));
            g.put("LanguageName", str(r.get("LanguageName")));
            g.put("AccountTitleOtherLingo", str(r.get("AccountTitleOtherLingo")));
            grid.add(g);
        }
        return grid;
    }

    /**
     * CoaTitleChange.button1_Click ("Update") -> ChartOfAccountMultiLingo.SaveList -> DAL SetDataList:
     * one transaction, GenericProvider.SetProc per item on Sp_ChartOfAccountMultiLingo_Insert with the
     * model's non-virtual properties (@Id=0, @ChartOfAccountId, @MultiLanguagesId, @AccountTitle).
     * Same three validations and messages as the form. The account must be one of this user's
     * COA-allocation rows (the combo the form picks it from).
     */
    @org.springframework.transaction.annotation.Transactional
    public void saveOtherLanguageTitle(Integer chartOfAccountId, Integer languageId, String title) {
        if (chartOfAccountId == null || chartOfAccountId <= 0) throw new IllegalArgumentException("AccountTitle Field Required");
        if (languageId == null || languageId <= 0) throw new IllegalArgumentException("Language Field Required");
        if (title == null || title.isEmpty() || "0".equals(title)) throw new IllegalArgumentException("Other Language AccountTitle Field Required");
        boolean own = false;
        for (Map<String, Object> r : coaAllocation()) if (toInt(r.get("Id")) == chartOfAccountId) { own = true; break; }
        if (!own) throw new SecurityException("That account does not belong to this company.");
        boolean langOk = false;
        for (Map<String, Object> r : languages()) if (toInt(r.get("Id")) == languageId) { langOk = true; break; }
        if (!langOk) throw new IllegalArgumentException("Language Field Required");
        DesktopProc.scalar(jdbc, "Sp_ChartOfAccountMultiLingo_Insert", p("Id", 0, "ChartOfAccountId", chartOfAccountId,
                "MultiLanguagesId", languageId, "AccountTitle", title));
    }

    // ------------------------------------------------------------------ 69 Accounts Activity Summary

    /** ActicitySummery.HistoryFill -> VoucherReports.ActivitySummaryReport -> Sp_ActivityReportSummery_Rpt. */
    public List<Map<String, Object>> activitySummary(String dateFilter, String fromDate, String toDate, boolean includeUnposted,
            Integer reportTypeId, String documentTypeIds, String branchesIds, Integer languageId) {
        Map<String, Object> fc = formContext();
        String branchIds = branchIdsByFeature(fc, branchesIds);
        boolean subsidiary = Boolean.TRUE.equals(fc.get("subsidiaryFeature"));
        Map<String, Object> pr = p("FinancialYearId", finYear(), "OrganizationId", org(), "CompanyId", comp());
        if ("entry".equals(dateFilter)) { pr.put("EntryDateFrom", date(fromDate)); pr.put("EntryDateTo", date(toDate)); }
        else { pr.put("VoucherDateF", date(fromDate)); pr.put("VoucherDateT", date(toDate)); }
        pr.put("UserId", nz(user()));
        pr.put("DocumentTypeIds", leadingCommaIds(documentTypeIds));
        pr.put("BranchesIds", ns(branchIds));
        pr.put("IsApproved", includeUnposted ? null : Boolean.TRUE);
        pr.put("ReportTypeId", subsidiary ? nz(reportTypeId) : null);
        pr.put("LanguageId", nz(languageId));
        List<Map<String, Object>> raw = rows("Sp_ActivityReportSummery_Rpt", pr);
        List<Map<String, Object>> grid = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> g = new LinkedHashMap<>();
            g.put("AccountId", toInt(r.get("AccountId")));
            g.put("BranchesId", toInt(r.get("BranchesId")));
            for (String k : new String[]{"BranchName", "ParentAccount", "ParentAccountTitle", "AccountCode", "AccountTitle", "SubsidiaryAccountTitle"})
                g.put(k, str(r.get(k)));
            for (String k : new String[]{"OpeningDr", "OpeningCr", "Opening", "Debit", "Credit", "Closing", "ClosingDr", "ClosingCr"})
                g.put(k, dbl(r.get(k)));
            g.put("SubsidiaryGroupAccount", str(r.get("SubsidiaryGroupAccount")));
            grid.add(g);
        }
        return grid;
    }

    // ------------------------------------------------------------------ 73 Bank Balances / 74 Cash Balances

    /** VoucherReports.CashandBankBalancesSummery -> Sp_Accounts_CashBankBalancesSummery_Rpt (Tables[0]); ApprovedFilter "All" so @IsApproved is left out. */
    private List<Map<String, Object>> cashBankSummary(int accountTypeId, String fromDate, String toDate, String branchIds, Integer languageId) {
        return rows("Sp_Accounts_CashBankBalancesSummery_Rpt", p("FinancialYearId", finYear(), "OrganizationId", org(),
                "CompanyId", comp(), "UserId", user(), "FromDate", date(fromDate), "ToDate", date(toDate),
                "AccountTypeId", accountTypeId, "BranchesIds", ns(branchIds), "LanguageId", nz(languageId)));
    }

    /**
     * BankBalances.btnshow_Click: ReciptandPaymentGrid (VoucherReports.BankBalances -> Sp_Accounts_BankBalances_Rpt,
     * no @AccountTypeId) + SummeryGrd (CashandBankBalancesSummery, AccountTypeId 15). withDetail=false is the form load,
     * which fills only the summary.
     */
    public Map<String, Object> bankBalances(String fromDate, String toDate, String branchesIds, Integer languageId,
            boolean excludeZeroFromSummary, boolean withDetail) {
        Map<String, Object> fc = formContext();
        String branchIds = branchIdsByFeature(fc, branchesIds);
        Map<String, Object> out = new LinkedHashMap<>();
        if (withDetail) {
            List<Map<String, Object>> raw = rows("Sp_Accounts_BankBalances_Rpt", p("FinancialYearId", finYear(),
                    "OrganizationId", org(), "CompanyId", comp(), "UserId", user(), "FromDate", date(fromDate),
                    "ToDate", date(toDate), "BranchesIds", ns(branchIds), "LanguageId", nz(languageId)));
            List<Map<String, Object>> rec = new ArrayList<>(), pay = new ArrayList<>();
            for (Map<String, Object> r : raw) {
                String tran = str(r.get("TranType"));
                Map<String, Object> g = new LinkedHashMap<>();
                g.put("BranchesId", toInt(r.get("BranchesId")));
                g.put("BranchName", str(r.get("BranchName")));
                g.put("V.Date", shortDate(r.get("voucherdate")));
                g.put("Id", r.get("Id"));
                g.put("DocumentTypeId", r.get("DocumentTypeId"));
                g.put("DocumentTypeSrNo", r.get("DocumentTypeSrNo"));
                g.put("V.Type", str(r.get("DocumentTypeCode")));
                g.put("V.No", toInt(r.get("Vouchercode")));
                g.put("BankName", str(r.get("AccountTitle")));
                g.put("AgainstAccountId", r.get("AgainstAccountId"));
                g.put("Account", str(r.get("OffsetAccountTitle")));
                g.put("SubsidiaryAccountId", r.get("SubsidiaryAccountId"));
                g.put("Amount", dbl(r.get("DebitAmount")));
                g.put("ChequeNo", str(r.get("ChequeNo")));
                if ("Receipts".equals(tran)) rec.add(g); else if ("Payments".equals(tran)) pay.add(g);
            }
            out.put("receipts", rec);
            out.put("payments", pay);
        }
        List<Map<String, Object>> sum = cashBankSummary(15, fromDate, toDate, branchIds, languageId);
        List<Map<String, Object>> grid = new ArrayList<>();
        for (Map<String, Object> r : sum) {
            if (excludeZeroFromSummary && !(dbl(r.get("CurrDebit")) > 0.0 || dbl(r.get("CurrCredit")) > 0.0)) continue;
            Map<String, Object> g = new LinkedHashMap<>();
            g.put("AccountId", r.get("AccountId"));
            g.put("BranchesId", toInt(r.get("BranchesId")));
            g.put("BranchName", str(r.get("BranchName")));
            g.put("CustomGroup", str(r.get("CustomGroup")));
            g.put("AccountCode", str(r.get("AccountCode")));
            g.put("AccountTitle", str(r.get("AccountTitle")));
            g.put("Opening", dbl(r.get("Opening")));
            g.put("Debit", dbl(r.get("CurrDebit")));
            g.put("Credit", dbl(r.get("CurrCredit")));
            g.put("Closing", dbl(r.get("Closing")));
            g.put("PendingPrematureReceipts", dbl(r.get("PendingPrematureReceipts")));
            g.put("PendingPrematurePayments", dbl(r.get("PendingPrematureDebit")));
            g.put("BalanceAfterPrematureReceiptsClearance", dbl(r.get("BalanceAfterPrematureReceiptsClearance")));
            g.put("PostDateCheqsReceipts", dbl(r.get("PdcReceipts")));
            g.put("PostDateCheqsPayments", dbl(r.get("PdcPayments")));
            g.put("BalanceAfterCheqClearance", dbl(r.get("BalanceAfterCheqClearance")));
            g.put("PostDatedCheqAmount", dbl(r.get("PostDatedCheqAmount")));
            Object bt = r.get("BalanceTime");
            g.put("BalanceTime", bt == null ? "" : (bt instanceof java.util.Date ? new SimpleDateFormat("hh:mm").format((java.util.Date) bt) : bt.toString()));
            g.put("ManualBalance", dbl(r.get("ManualBankBalance")));
            g.put("Source", str(r.get("SourceBy")));
            g.put("ConfirmBy", str(r.get("ConfirmedBy")));
            grid.add(g);
        }
        out.put("summary", grid);
        return out;
    }

    /**
     * frmCashBalance.btnSearch_Click: reciptandpayment (VoucherReports.CashBalances -> Sp_Accounts_CashBalances_Rpt,
     * no @FinancialYearId, @AccountId when > 0) + cashSummery (CashandBankBalancesSummery, AccountTypeId 2).
     */
    public Map<String, Object> cashBalances(String fromDate, String toDate, Integer accountId, String branchesIds,
            Integer languageId, boolean withDetail) {
        Map<String, Object> fc = formContext();
        String branchIds = branchIdsByFeature(fc, branchesIds);
        Map<String, Object> out = new LinkedHashMap<>();
        if (withDetail) {
            List<Map<String, Object>> raw = rows("Sp_Accounts_CashBalances_Rpt", p("OrganizationId", org(), "CompanyId", comp(),
                    "UserId", user(), "FromDate", date(fromDate), "ToDate", date(toDate),
                    "AccountId", accountId != null && accountId > 0 ? accountId : null,
                    "BranchesIds", ns(branchIds), "LanguageId", nz(languageId)));
            List<Map<String, Object>> rec = new ArrayList<>(), pay = new ArrayList<>();
            for (Map<String, Object> r : raw) {
                String tran = str(r.get("TranType"));
                Map<String, Object> g = new LinkedHashMap<>();
                g.put("BranchesId", toInt(r.get("BranchesId")));
                g.put("BranchName", str(r.get("BranchName")));
                g.put("TranType", tran);
                g.put("V.Type", str(r.get("DocumentTypeCode")));
                g.put("V.No", str(r.get("VoucherCode")));
                g.put("Id", r.get("Id"));
                g.put("DocumentTypeId", r.get("DocumentTypeId"));
                g.put("DocumentTypeSrNo", r.get("DocumentTypeSrNo"));
                g.put("V.Date", shortDate(r.get("voucherdate")));
                g.put("CashAccount", str(r.get("AccountTitle")));
                g.put("Account", str(r.get("OffsetAccountTitle")));
                g.put("Amount", dbl(r.get("DebitAmount")));
                g.put("NoOfAttachments", str(r.get("NoOfAttachments")));
                if ("Receipts".equals(tran)) rec.add(g); else if ("Payments".equals(tran)) pay.add(g);
            }
            out.put("receipts", rec);
            out.put("payments", pay);
        }
        List<Map<String, Object>> sum = cashBankSummary(2, fromDate, toDate, branchIds, languageId);
        List<Map<String, Object>> cards = new ArrayList<>();
        for (Map<String, Object> r : sum) {
            Map<String, Object> c = new LinkedHashMap<>();
            c.put("AccountId", toInt(r.get("AccountId")));
            c.put("AccountTitle", str(r.get("AccountTitle")));
            c.put("Opening", dbl(r.get("Opening")));
            c.put("CurrDebit", dbl(r.get("CurrDebit")));
            c.put("CurrCredit", dbl(r.get("CurrCredit")));
            c.put("Closing", dbl(r.get("Closing")));
            cards.add(c);
        }
        out.put("summary", cards);
        return out;
    }
}
