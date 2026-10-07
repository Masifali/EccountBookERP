package com.mst.services;

import com.mst.models.ConfigrationsAllocation;
import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.ExpenseTaxVoucherDto;
import com.mst.repositories.DesktopVoucherWriter;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * Screen 862 ExpenseVoucherNew - "Expense Voucher New".
 *
 * Desktop: Architecture.WinApp.Account_Definition.VouchersWithTax.ExpenseVoucherNew (DocumentTypeId 26,
 * base.Name "ExpenseVoucherNew" - the ScreenName the rights are read under). One grid row = one debit line
 * (detail account) and the form adds the matching credit line against the single Credit Account of the header.
 *
 * Persistence is the BLL VoucherHead.Save chain (DesktopVoucherWriter), exactly the call the desktop's Insert()
 * makes. The ledger line pairs, the cost-centre rows and VoucherAmount are derived here from the posted rows.
 */
@Service
public class ExpenseTaxVoucherService {

    private static final Logger LOG = LoggerFactory.getLogger(ExpenseTaxVoucherService.class);
    public static final int DOCUMENT_TYPE_ID = 26;
    public static final String SCREEN_NAME = "ExpenseVoucherNew";

    private static final String SQL_USER_RIGHTS =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, @CompanyId=?, @Activity=?";

    @Autowired private DesktopVoucherWriter writer;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private VoucherDesktopConfigService config;
    @Autowired private DesktopVoucherScreenService screens;

    // ================================================================================ lookups

    /** Everything ExpenseVoucher_Load / Refresh binds, each from the desktop's own call. */
    public Map<String, Object> lookups() {
        Map<String, Object> r = new LinkedHashMap<>();
        Map<String, Object> flags = flags();
        r.put("creditAccounts", creditAccounts());              // AccountTitleFill - Accounts_GetAccountTitleByAccountTypeIds("2,15")
        r.put("accounts", detailAccounts());                    // DetailAccountFill - clsGlobalVariables.AllAccountsWithCustomGroupId
        r.put("refAccounts", screens.referenceAccounts());      // BindReferenceAccounts
        r.put("projects", screens.projects());                  // CombProjectFill
        r.put("locationTypes", screens.locationTypes());        // LocationType
        r.put("currencies", screens.currenciesWithRate());      // CurrencyFill
        r.put("jobLots", screens.jobLots());                    // combojoblotfill
        r.put("branches", Boolean.TRUE.equals(flags.get("branchFeature")) ? screens.branches() : new ArrayList<>());
        r.put("costCenters", subCostCenters());                 // dtAllSubCostCenters
        r.put("historyAccounts", historyAccounts());            // CmbAccountTitleHistory
        r.put("flags", flags);
        r.put("nextCode", nextCode());
        return r;
    }

    /** CommonServices.Accounts_GetAccountTitleByAccountTypeIds("2,15"): Id, AccountTitle, AccountCode, AccountTypeId, ParentAccountTitle, AccountClass, CurrencyId, CurrencyCode. */
    public List<Map<String, Object>> creditAccounts() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> row : screens.contraAccounts()) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(row, "Id"));
            o.put("AccountTitle", ci(row, "AccountTitle"));
            o.put("AccountCode", ci(row, "AccountCode"));
            o.put("AccountTypeId", ci(row, "AccountTypeId"));
            o.put("ParentAccountTitle", ci(row, "ParentAccountTitle"));
            o.put("AccountClass", ci(row, "AccountClass"));
            o.put("CurrencyId", ci(row, "CurrencyId"));
            o.put("CurrencyCode", ci(row, "CurrencyCode"));
            out.add(o);
        }
        return out;
    }

    /**
     * DetailAccountFill (:578): clsGlobalVariables.AllAccountsWithCustomGroupId (USP_GETAllAccountsFromCustomGroups)
     * kept to AccountTypeId {11,13,14,20,21,22,23} (12 added when InventoryRelatedAccountsShowInVouchers), the first
     * row per ChartOfAccountId, with the account's CurrencyId / CurrencyCode.
     */
    public List<Map<String, Object>> detailAccounts() {
        boolean inventory = screens.configBool("InventoryRelatedAccountsShowInVouchers");
        Set<Integer> include = new HashSet<>();
        for (int t : inventory ? new int[] {11, 12, 13, 14, 20, 21, 22, 23} : new int[] {11, 13, 14, 20, 21, 22, 23}) include.add(t);
        List<Map<String, Object>> rows = DesktopProc.rows(jdbcTemplate, "[dbo].[USP_GETAllAccountsFromCustomGroups]",
                DesktopProc.params("OrganizationId", currentUserContext.currentOrganizationId(),
                        "CompanyId", currentUserContext.currentCompanyId()));
        Set<Integer> seen = new HashSet<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (!include.contains(toInt(ci(r, "AccountTypeId")))) continue;
            int id = toInt(ci(r, "ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("AccountTitle", ci(r, "AccountTitle"));
            o.put("AccountCode", ci(r, "AccountCode"));
            o.put("AccountTypeId", ci(r, "AccountTypeId"));
            o.put("ParentAccountTitle", ci(r, "ParentAccountTitle"));
            o.put("AccountClass", ci(r, "AccountClassName"));
            o.put("CurrencyId", ci(r, "CurrencyId"));
            o.put("CurrencyCode", ci(r, "CurrencyCode"));
            out.add(o);
        }
        return out;
    }

    /** Projects.GetSubCostCenters -> Id, CostCenterName, ParentCostCenterId, parentCostCenterName. */
    public List<Map<String, Object>> subCostCenters() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : screens.costCenters()) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("CostCenterName", ci(r, "CostCenterName"));
            o.put("ParentCostCenterId", ci(r, "ParentCostCenterId"));
            o.put("ParentCostCenter", ci(r, "parentCostCenterName"));
            out.add(o);
        }
        return out;
    }

    /** CmbAccountTitleHistory: Sp_Vouchers_LedgerByJobLot_DropDownAndLists with DocumentTypeIds "26" (CompanyId = OrganizationId, as the BLL sends it). */
    public List<Map<String, Object>> historyAccounts() {
        int user = currentUserContext.currentUserId();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbcTemplate, "Sp_Vouchers_LedgerByJobLot_DropDownAndLists",
                DesktopProc.params("OrganizationId", currentUserContext.currentOrganizationId(),
                        "CompanyId", currentUserContext.currentOrganizationId(), "AppId", screens.appId(), "UserId", user,
                        "DocumentTypeIds", String.valueOf(DOCUMENT_TYPE_ID)));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (!"Account Header".equals(str(ci(r, "ActivityType")))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("AccountTitle", ci(r, "name"));
            out.add(m);
        }
        return out;
    }

    /** BindSubsidiaryAccount(CompanyId, AccountId, "3") for the detail account picked. */
    public List<Map<String, Object>> subsidiaries(int accountId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> row : screens.subsidiaryAccounts(accountId, "3")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(row, "Id"));
            o.put("SubsidiaryAccount", ci(row, "SubsidiaryAccount"));
            Object type = ci(row, "SubsidiaryTypeId");
            o.put("SubsidiaryTypeId", type != null ? type : pos(row, 2));
            out.add(o);
        }
        return out;
    }

    /** CheqNoFill: SP_CheqBookHeader OutstandingCheqNo for the bank, only when "CheqBook Enabled" is on. */
    public List<Map<String, Object>> cheques(int bankId, int recId) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (!screens.configBool("CheqBook Enabled")) return out;
        for (Map<String, Object> r : screens.outstandingCheques(bankId, recId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("CheqNo", ci(r, "CheqNo"));
            out.add(o);
        }
        return out;
    }

    /** The Load-time switches plus the defaults DefaultConfigurations() applies. */
    public Map<String, Object> flags() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> f = new LinkedHashMap<>();
        Map<String, ConfigrationsAllocation> m = config.configMap();
        f.put("defaultJobLotId", toInt(VoucherDesktopConfigService.key(m, "Job/Lot")));
        f.put("baseCurrencyId", toInt(VoucherDesktopConfigService.key(m, "Base Currency")));
        String rate = VoucherDesktopConfigService.key(m, "BaseCurrencyRate");
        f.put("baseCurrencyRate", rate == null ? null : VoucherDesktopConfigService.toDouble(rate));
        f.put("amountDecimals", toInt(VoucherDesktopConfigService.key(m, "Default NoofDecimal Points For Amount")));
        f.put("rateDecimals", toInt(VoucherDesktopConfigService.key(m, "Default NoofDecimal Points For Rate")));
        f.put("defaultBranchId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        f.put("multiCurrencyFeature", config.erpFeature(6));      // MultiCurrencyFeatureVisibilty (MultiCurrencyGridWorking)
        f.put("subsidiaryFeature", config.erpFeature(4));         // SubsidiaryAccountAllowOnVouchers
        f.put("branchFeature", config.erpFeature(17));            // BranchFeature
        f.put("isBookingOffice", screens.isBookingOffice());      // AppId 5 - the Cost Center combo gets its first row activated
        f.put("chequeBookEnabled", screens.configBool("CheqBook Enabled"));
        f.put("amountLimit", toDouble(VoucherDesktopConfigService.key(m, "AmountLimitForExpenseVoucher")));
        f.put("screenName", SCREEN_NAME);
        f.put("documentTypeId", DOCUMENT_TYPE_ID);
        f.put("rights", rights());
        return f;
    }

    /** CommonServices.GenerateVoucherCode(26). */
    public int nextCode() {
        UserAccount u = currentUserContext.requireAccountingUser();
        return writer.nextVoucherCode(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID,
                currentUserContext.currentFinancialYearId(), u.getBranchesId() == null ? 0 : u.getBranchesId());
    }

    /** AccountCurrentBalance / DetailAccountCurrentBalance: ReadByCurrentBalanceByDateAndAccountId, Balance column. */
    public double balance(int accountId, String date) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return writer.accountBalance(u.getOrganizationId(), u.getCompanyId(),
                currentUserContext.currentFinancialYearId(), isoDay(date), accountId);
    }

    /** CmbSubsidiaryAccount_ValueChanged: GetSubsiadiaryBalance. */
    public double subsidiaryBalance(int glAccountId, int subsidiaryId, String date) {
        return screens.subsidiaryBalance(glAccountId, subsidiaryId, isoDay(date));
    }

    /** cmbCurrency_Leave: GetLastExchangeRateAndCurrencyOfVoucher with DocumentTypeIds "26"; no row -> 0. */
    public double lastRate(int currencyId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbcTemplate, "Sp_Vouchers_GetMethods",
                DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                        "DocumentTypeIds", String.valueOf(DOCUMENT_TYPE_ID),
                        "DMultiCurrencyIds", currencyId != 0 ? String.valueOf(currencyId) : null,
                        "Activity", "GetMultiCurrencyAndLastRate"));
        return rows.isEmpty() ? 0d : toDouble(ci(rows.get(0), "LastExchRate"));
    }

    // ================================================================================ history

    /**
     * HistoryFill -> VoucherHead.VoucherFormHistory -> USP_VoucherFormHistory with DocumentTypeName "26",
     * CanViewAllRecord = the screen's right, EntryUser only when that right is off, AppId, AccountId when picked,
     * IsApproved unless "All".
     */
    public Map<String, Object> history(String dateType, String fromDate, String toDate,
                                       Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus) {
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean viewAll = hasRight("CanView AllRecord");
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_VoucherFormHistory @OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeName=?, @CanViewAllRecord=?");
        List<Object> a = new ArrayList<>();
        a.add(u.getOrganizationId()); a.add(u.getCompanyId()); a.add(u.getId()); a.add(String.valueOf(DOCUMENT_TYPE_ID)); a.add(viewAll);
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
        int app = screens.appId();
        if (app != 0) { sql.append(", @AppId=?"); a.add(app); }

        List<Map<String, Object>> raw = jdbcTemplate.queryForList(sql.toString(), a.toArray());
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("id", ci(r, "Id"));
            o.put("accountTypeId", ci(r, "AccountTypeId"));
            o.put("documentTypeId", ci(r, "DocumentTypeId"));
            o.put("voucherDate", dateStr(ci(r, "VoucherDate")));
            o.put("voucherCode", ci(r, "VoucherCode"));
            o.put("documentType", ci(r, "DocumentTypeCode"));
            o.put("accountTitle", ci(r, "AccountTitle"));
            o.put("remarks", ci(r, "Remarks"));
            o.put("voucherAmount", ci(r, "VoucherAmount"));
            o.put("fcyAmount", ci(r, "FcAmount"));
            o.put("entryUser", ci(r, "UserName"));
            o.put("entryDate", dateTimeStr(ci(r, "EntryDate")));
            o.put("modifyUser", ci(r, "ModifyUserName"));
            o.put("modifyDate", dateTimeStr(ci(r, "ModifyDate")));
            o.put("approvedUser", ci(r, "ApprovedUserName"));
            o.put("approvedDate", dateTimeStr(ci(r, "PostDate")));
            o.put("cheqNo", ci(r, "ChequeNo"));
            o.put("attachment", ci(r, "NoOfAttachments"));
            rows.add(o);
        }
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("rows", rows);
        res.put("totalVouchers", raw.isEmpty() ? 0 : toInt(ci(raw.get(0), "TotalVouchers")));
        res.put("totalApprovedVoucher", raw.isEmpty() ? 0 : toInt(ci(raw.get(0), "TotalApprovedVoucher")));
        res.put("totalUnApprovedVoucher", raw.isEmpty() ? 0 : toInt(ci(raw.get(0), "TotalUnApprovedVoucher")));
        return res;
    }

    // ================================================================================ load

    /** The head row when it is this tenant's document type 26 voucher, otherwise null. */
    private Map<String, Object> ownHead(int id) {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> h = screens.readHead(id);
        if (h == null) return null;
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId() || toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) return null;
        return h;
    }

    /**
     * ReadById(): the head fields, one grid row per stored debit line (IsTaxable "False" and not the credit
     * account) and the cost-centre breakup rows.
     */
    public Map<String, Object> load(int id) {
        Map<String, Object> h = ownHead(id);
        if (h == null) return null;
        boolean branchFeature = config.erpFeature(17);
        int refAccount = toInt(ci(h, "RefAccountId"));
        List<Map<String, Object>> details = screens.readDetails(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", toInt(ci(h, "Id")));
        out.put("documentTypeId", DOCUMENT_TYPE_ID);
        out.put("voucherCode", ci(h, "VoucherCode"));
        out.put("voucherDate", dateStr(ci(h, "VoucherDate")));
        out.put("projectId", ci(h, "ProjectId"));
        out.put("refAccountId", refAccount);
        out.put("payTitle", str(ci(h, "PayTitle")));
        out.put("chequeNo", str(ci(h, "ChequeNo")));
        out.put("chequeDate", dateStr(ci(h, "ChequeDate")));
        out.put("remarks", str(ci(h, "Remarks")));
        out.put("multiCurrencyId", ci(h, "MultiCurrencyId"));
        out.put("exchangeCurrencyRate", ci(h, "ExchangeCurrencyRate"));
        out.put("fcAmount", ci(h, "FcAmount"));
        out.put("voucherAmount", ci(h, "VoucherAmount"));
        out.put("isApproved", truthy(ci(h, "IsApproved")));
        out.put("customAccounts", truthy(ci(h, "CustomAccounts")));
        out.put("locationTypeId", details.isEmpty() ? 0 : toInt(ci(details.get(0), "LocationTypeId")));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : details) {
            if (!"False".equals(str(ci(d, "IsTaxable"))) || toInt(ci(d, "AccountId")) == refAccount) continue;
            int type = toInt(ci(d, "SubsidiaryTypeId"));
            Object sub = type == 1 ? ci(d, "SupplierCustomerId") : type == 2 ? ci(d, "EmployeeId")
                    : (type == 3 || type == 4) ? ci(d, "SubsidiaryAccountId") : 0;
            Map<String, Object> r = new LinkedHashMap<>();
            r.put("accountCode", ci(d, "AccountCode"));
            r.put("accountId", ci(d, "AccountId"));
            r.put("accountTitle", ci(d, "AccountTitle"));
            r.put("glCurrencyId", ci(d, "ThirdCurrencyId"));
            r.put("glCurrency", ci(d, "GlcyCode"));
            r.put("subsidiaryAccountId", sub);
            r.put("subsidiaryAccount", ci(d, "SubsidiaryAccountTitle"));
            r.put("subsidiaryAccountTypeId", type);
            r.put("jobLotId", ci(d, "JobLotId"));
            r.put("remarks", ci(d, "Comments"));
            r.put("tcyCodeId", ci(d, "DMultiCurrencyId"));
            r.put("tcyCode", ci(d, "TcyCode"));
            r.put("tcyExchangeRate", ci(d, "DExchangeCurrencyRate"));
            r.put("fcyAmount", ci(d, "DCurrencyAmount"));
            r.put("amount", ci(d, "DebitAmount"));
            r.put("referenceAccountId", ci(d, "ReferenceAccountId"));
            r.put("referenceAccount", ci(d, "ReferenceAccount"));
            r.put("lineId", ci(d, "SortNo"));
            r.put("costCenterId", ci(d, "CostCenterId"));
            r.put("costCenterName", ci(d, "CostCenterName"));
            r.put("branchId", branchFeature ? ci(d, "BranchesId") : 0);
            r.put("branchName", branchFeature ? ci(d, "BranchName") : "");
            r.put("chequeId", ci(d, "InvoiceNoRefId"));
            r.put("chequeDate", dateStr(ci(d, "DCheqDate")));
            r.put("chequeNo", ci(d, "CheqNoDetail"));
            r.put("payTitle", ci(d, "PayeeTitle"));
            rows.add(r);
        }
        out.put("rows", rows);
        List<Map<String, Object>> costs = new ArrayList<>();
        for (Map<String, Object> c : DesktopProc.rows(jdbcTemplate, "Sp_Vouchers_GetMethods",
                DesktopProc.params("Id", id, "Activity", "voucherCostCenterDetailByHeaderId"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("id", ci(c, "Id"));
            o.put("lineId", ci(c, "SortNo"));
            o.put("parentId", ci(c, "ParentCostCenterId"));
            o.put("debitAccount", ci(c, "AccountTitle"));
            o.put("costCenterId", ci(c, "CostCenterId"));
            o.put("costCenter", ci(c, "CostCenterName"));
            o.put("percent", ci(c, "costPrcent"));
            o.put("amount", ci(c, "costAmount"));
            costs.add(o);
        }
        out.put("costCenters", costs);
        return out;
    }

    /** VoucherDetailByHeaderId: the grid under the history grid (lines that carry a debit or a credit). */
    public Map<String, Object> lines(int id) {
        Map<String, Object> h = ownHead(id);
        if (h == null) return null;
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : screens.readDetails(id)) {
            double dr = toDouble(ci(d, "DebitAmount")), cr = toDouble(ci(d, "CreditAmount"));
            if (!(dr > 0d || cr > 0d)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("accountCode", ci(d, "AccountCode"));
            o.put("accountTitle", ci(d, "AccountTitle"));
            o.put("glCurrency", ci(d, "GlcyCode"));
            o.put("subsidiaryAccount", ci(d, "SubsidiaryAccountTitle"));
            o.put("jobLot", ci(d, "JobLotDescription"));
            o.put("remarks", ci(d, "Comments"));
            o.put("tcyCode", ci(d, "TcyCode"));
            o.put("tcyExchangeRate", ci(d, "DExchangeCurrencyRate"));
            o.put("tcyDebit", dr > 0 ? toDouble(ci(d, "DCurrencyAmount")) : 0d);
            o.put("debitAmount", dr);
            o.put("tcyCredit", cr > 0 ? toDouble(ci(d, "DCurrencyAmount")) : 0d);
            o.put("creditAmount", cr);
            o.put("referenceAccount", ci(d, "ReferenceAccount"));
            o.put("branchName", ci(d, "BranchName"));
            o.put("chequeDate", dateStr(ci(d, "DCheqDate")));
            o.put("chequeNo", ci(d, "CheqNoDetail"));
            o.put("payTitle", ci(d, "PayeeTitle"));
            rows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", toInt(ci(h, "Id")));
        out.put("rows", rows);
        return out;
    }

    // ================================================================================ save

    @Transactional
    public Map<String, Object> save(ExpenseTaxVoucherDto dto) {
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean insert = dto.Id == null || dto.Id <= 0;
        if (insert && !hasRight("Save")) throw new SecurityException("You don't have Save right on this screen.");
        if (!insert && !hasRight("Update")) throw new SecurityException("You don't have Update right on this screen.");
        Map<String, Object> existing = null;
        if (!insert) {
            existing = ownHead(dto.Id);
            if (existing == null) throw new IllegalArgumentException("Record Not Update  " + dto.Id);
        }
        boolean multiCurrency = config.erpFeature(6);
        boolean subsidiaryFeature = config.erpFeature(4);
        boolean branchFeature = config.erpFeature(17);
        List<ExpenseTaxVoucherDto.Row> rows = dto.rows == null ? new ArrayList<>() : dto.rows;
        List<String> warnings = new ArrayList<>();

        // ---- Insert() preamble + FormValidation (:2447)
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record not found");
        if (nz(dto.LocationTypeId) == 0) throw new IllegalArgumentException("Location Type Field is Required");
        if (nz(dto.ProjectId) == 0) throw new IllegalArgumentException("Cost Center Field is Required");
        if (nz(dto.RefAccountId) == 0) throw new IllegalArgumentException("Credit Account Field is Required");
        Map<String, Object> credit = null;
        for (Map<String, Object> a : creditAccounts()) {
            if (toInt(ci(a, "Id")) == nz(dto.RefAccountId)) { credit = a; break; }
        }
        if (credit == null) throw new IllegalArgumentException("Credit Account Field is Required");
        if (multiCurrency && toInt(ci(credit, "CurrencyId")) == 0) throw new IllegalArgumentException("Credit Account Currency not set Please Check");
        if (nz(dto.MultiCurrencyId) == 0) throw new IllegalArgumentException("Fcy Code Field is Required");
        if (nzd(dto.ExchangeCurrencyRate) == 0d) throw new IllegalArgumentException("Exchange Rate Field is Required");
        if (nzd(dto.FcAmount) == 0d) throw new IllegalArgumentException("Fcy Amount Field is Required");
        if (isoDay(dto.VoucherDate) == null) throw new IllegalArgumentException("Voucher Date Field is Required");
        if (multiCurrency) {
            // The grdMultiCurrency per-GL-currency rate table (ThirdCurrency*, BaseFcy*) is not carried by this page.
            throw new IllegalArgumentException("Multi Currency Grid is not available on the web yet. This voucher cannot be saved while the Multi Currency feature is on.");
        }
        Map<Integer, String> titles = new HashMap<>();
        for (Map<String, Object> a : detailAccounts()) titles.put(toInt(ci(a, "Id")), str(ci(a, "AccountTitle")));
        validateRows(rows, titles, subsidiaryFeature, branchFeature);
        for (ExpenseTaxVoucherDto.Row r : rows) {
            if (nzd(r.Amount) > 0d && nz(r.AccountId) == 0) throw new IllegalArgumentException("Please Select Account Title First");
        }
        boolean chequeAccount = toInt(ci(credit, "AccountTypeId")) == 15;

        // ---- header (:2000-2027)
        ContraVoucherDto.Head head = new ContraVoucherDto.Head();
        head.Id = insert ? 0 : dto.Id;
        head.DocumentTypeId = DOCUMENT_TYPE_ID;
        head.VoucherCode = nz(dto.VoucherCode);
        if (insert && head.VoucherCode <= 0) head.VoucherCode = nextCode();
        head.ProjectId = nz(dto.ProjectId);
        head.VoucherDate = isoDay(dto.VoucherDate);
        head.RefAccountId = nz(dto.RefAccountId);
        head.AgainstAccountId = nz(dto.RefAccountId);
        head.RefDocNoId = nz(dto.RefAccountId);                  // the credit account again
        head.Remarks = nzs(dto.Remarks).trim();
        /* The header's CheqId / ChequeDate / ChequeNo / PayTitle are NOT assigned by Insert(): they stay empty and, for a
           type-15 credit account, are taken from the first grid row that carries a cheque number (see the loop below). */
        head.CheqId = 0;
        head.IncludeWHT = Boolean.FALSE;
        head.IsApproved = Boolean.FALSE;
        head.OrganizationId = u.getOrganizationId();
        head.CompanyId = u.getCompanyId();
        head.FinancialYearId = currentUserContext.currentFinancialYearId();
        head.EntryUser = u.getId();
        head.ModifyUser = u.getId();
        String now = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date());
        head.EntryDate = now;
        head.ModifyDate = now;
        head.MultiCurrencyId = nz(dto.MultiCurrencyId);
        head.ExchangeCurrencyRate = nzd(dto.ExchangeCurrencyRate);
        head.FcAmount = nzd(dto.FcAmount);
        head.CustomAccounts = Boolean.TRUE.equals(dto.CustomAccounts);
        head.BranchId = 0;                                       // never assigned by this Insert()
        if (existing != null) {
            Object av = ci(existing, "AttachmentsValues");
            head.AttachmentsValues = av == null ? null : String.valueOf(av);
            head.CustomAttachmentsValues = str(ci(existing, "CustomAttachmentsValues"));
        } else {
            head.CustomAttachmentsValues = "";
        }

        // ---- budget control inputs
        boolean applyOnActual = parseBoolStrict(screens.config("Apply On Actual Expenses"));
        String monthlyBudget = screens.config("MonthlyBudget");
        double debitAmtGet = 0d;                                  // DebitAmtGet - ReadById summed the loaded lines that have a budget row
        if (!insert && applyOnActual) {
            String oldDate = isoDay(dateStr(ci(existing, "VoucherDate")));
            for (Map<String, Object> d : screens.readDetails(dto.Id)) {
                if (!budgetBalances(oldDate, toInt(ci(d, "AccountId"))).isEmpty()) debitAmtGet += toDouble(ci(d, "DebitAmount"));
            }
        }

        // ---- detail pairs (:2028-2200)
        List<ContraVoucherDto.Detail> details = new ArrayList<>();
        int userBranch = u.getBranchesId() == null ? 0 : u.getBranchesId();
        double debitAccum = 0d;                                   // DebitAmount, declared outside the loop
        double voucherAmount = 0d;
        int lineId = 1;
        for (int i = 0; i < rows.size(); i++) {
            ExpenseTaxVoucherDto.Row r = rows.get(i);
            ContraVoucherDto.Detail vd = new ContraVoucherDto.Detail();
            vd.SubsidiaryTypeId = nz(r.SubsidiaryAccountTypeId);
            int sub = nz(r.SubsidiaryAccountId);
            if (sub > 0) {
                vd.SupplierCustomerId = vd.SubsidiaryTypeId == 1 ? sub : 0;
                vd.EmployeeId = vd.SubsidiaryTypeId == 2 ? sub : 0;
                vd.SubsidiaryAccountId = sub;
            }
            vd.AccountId = nz(r.AccountId);

            if (applyOnActual) {
                List<Map<String, Object>> dt = budgetBalances(head.VoucherDate, vd.AccountId);
                if (!dt.isEmpty()) {
                    int budgetAc = toInt(ci(dt.get(0), "AccountId"));
                    for (ExpenseTaxVoucherDto.Row x : rows) {
                        if (budgetAc == vd.AccountId) debitAccum += nzd(x.Amount);
                    }
                    if (budgetAc == vd.AccountId && monthlyBudget != null) {
                        double balance = toDouble(ci(dt.get(0), "DebitAmount"));
                        double utilize = debitAccum;
                        if (!insert) balance += debitAmtGet;
                        double diff = Math.rint(balance) - Math.rint(utilize);
                        String msg = "Accumulated Monthly Budget For " + titles.getOrDefault(vd.AccountId, "") + " Up is"
                                + net(balance) + " It will exceed by " + net(diff);
                        if ("Stop".equals(monthlyBudget) && utilize > balance) throw new IllegalArgumentException(msg);
                        if ("Warn".equals(monthlyBudget) && utilize > balance) warnings.add(msg);
                    }
                }
            }

            vd.LineId = lineId;
            vd.SortNo = nz(r.LineId);
            vd.AgainstAccountId = nz(dto.RefAccountId);
            vd.JobLotId = nz(r.JobLotId);
            vd.Comments = nzs(r.Remarks);
            vd.DebitAmount = nzd(r.Amount);
            if (chequeAccount) {
                vd.InvoiceNoRefId = nz(r.ChequeId);
                vd.DCheqDate = isoDay(r.ChequeDate);
                vd.CheqNoDetail = nzs(r.ChequeNo);
                vd.PayeeTitle = nzs(r.PayTitle);
                if (head.ChequeNo == null || head.ChequeNo.trim().isEmpty()) {
                    head.CheqId = nz(r.ChequeId);
                    head.ChequeDate = isoDay(r.ChequeDate);
                    head.ChequeNo = nzs(r.ChequeNo);
                    head.PayTitle = nzs(r.PayTitle);
                }
            }
            vd.IsTaxable = "False";
            vd.CostCenterId = nz(r.CostCenterId);
            vd.ReferenceAccountId = nz(r.ReferenceAccountId);
            vd.DMultiCurrencyId = nz(r.TcyCodeId);
            vd.DExchangeCurrencyRate = nzd(r.TcyExchangeRate);
            vd.DCurrencyAmount = nzd(r.FcyAmount);
            vd.BranchesId = branchFeature ? nz(r.BranchId) : userBranch;
            if (vd.DCurrencyAmount == 0d) throw new IllegalArgumentException("Fcy Amount Not Found In Detail Grid at Row#" + (i + 1));
            vd.LocationTypeId = nz(dto.LocationTypeId);
            details.add(vd);
            lineId++;

            ContraVoucherDto.Detail vd2 = new ContraVoucherDto.Detail();
            vd2.LineId = lineId;
            vd2.SortNo = nz(r.LineId);
            vd2.AccountId = nz(dto.RefAccountId);
            vd2.AgainstAccountId = nz(r.AccountId);
            vd2.JobLotId = nz(r.JobLotId);
            vd2.Comments = nzs(r.Remarks);
            vd2.CreditAmount = nzd(r.Amount);
            vd2.InvoiceNoRefId = head.CheqId;
            vd2.CheqNoDetail = head.ChequeNo;
            vd2.DCheqDate = head.ChequeDate;
            vd2.PayeeTitle = head.PayTitle;
            vd2.IsTaxable = "False";
            vd2.CostCenterId = nz(r.CostCenterId);
            vd2.ReferenceAccountId = nz(r.ReferenceAccountId);
            vd2.BranchesId = branchFeature ? nz(r.BranchId) : userBranch;
            vd2.DMultiCurrencyId = nz(r.TcyCodeId);
            vd2.DExchangeCurrencyRate = nzd(r.TcyExchangeRate);
            vd2.DCurrencyAmount = nzd(r.FcyAmount);
            if (vd2.DCurrencyAmount == 0d) throw new IllegalArgumentException("Fcy Amount Not Found In Detail Grid at Row#" + (i + 1));
            if (chequeAccount) {
                vd2.InvoiceNoRefId = nz(r.ChequeId);
                vd2.DCheqDate = isoDay(r.ChequeDate);
                vd2.CheqNoDetail = nzs(r.ChequeNo);
                vd2.PayeeTitle = nzs(r.PayTitle);
            }
            voucherAmount += vd2.CreditAmount;
            vd2.LocationTypeId = nz(dto.LocationTypeId);
            details.add(vd2);
            lineId++;
        }
        head.VoucherAmount = voucherAmount;

        // ---- AmountLimitForExpenseVoucher (:2214)
        double limit = toDouble(screens.config("AmountLimitForExpenseVoucher"));
        if (limit > 0d && voucherAmount > limit) {
            throw new IllegalArgumentException("Total VoucherAmount " + net(voucherAmount) + " can not be grater than Amount Limit " + net(limit));
        }

        // ---- negative balance
        if (!screens.configBool("DisableBothNegativeBalanceRestrictions")) {
            double closing = Math.rint(screens.accountBalance(head.RefAccountId, head.VoucherDate));
            if (voucherAmount > closing) {
                String msg = "Debit Amount cannot be greater than Account Balance.\nAccount '" + str(ci(credit, "AccountTitle"))
                        + "' Balance is " + net(closing) + " and Total Debit Amount is " + net(voucherAmount) + ".";
                if (screens.configBool("is Minus balance Allowed")) throw new IllegalArgumentException(msg);
                if (screens.configBool("DisplayWarningforNegativeBalance") && !Boolean.TRUE.equals(dto.NegativeBalanceAcknowledged)) {
                    throw new ContraVoucherService.ConfirmationRequiredException(msg, "negativeBalance");
                }
            }
        }

        // ---- VoucherExistWithSameAmountInSameDate: vd.ActionId is never set on this form, so @ActionId = 0 and nothing comes back
        if (!Boolean.TRUE.equals(dto.DuplicateAcknowledged)) {
            Set<Integer> seen = new LinkedHashSet<>();
            for (ContraVoucherDto.Detail d : details) {
                if (nzd(d.DebitAmount) <= 0d || !seen.add(nz(d.AccountId))) continue;
                String title = writer.duplicateVoucherTitle(u.getOrganizationId(), u.getCompanyId(),
                        head.VoucherDate, nz(d.AccountId), nzd(d.DebitAmount), nz(d.ActionId));
                if (title != null) {
                    throw new ContraVoucherService.ConfirmationRequiredException("Voucher against '" + title
                            + "' with same Debit Amount already exists on this date. Do you want to continue?", "duplicate");
                }
            }
        }

        // ---- cost centres: the grdCostCenterDetail rows, then the per-line checks
        Set<Integer> lineIds = new HashSet<>();
        for (ExpenseTaxVoucherDto.Row r : rows) lineIds.add(nz(r.LineId));
        List<ContraVoucherDto.CostCentre> costs = new ArrayList<>();
        if (dto.costCenters != null) {
            for (ExpenseTaxVoucherDto.Break b : dto.costCenters) {
                if (!lineIds.contains(nz(b.LineId))) continue;
                ContraVoucherDto.CostCentre c = new ContraVoucherDto.CostCentre();
                c.Id = nz(b.Id);
                c.SortNo = nz(b.LineId);
                c.CostCenterId = nz(b.CostCenterId);
                c.costPrcent = BigDecimal.valueOf(nzd(b.Percent));
                c.costAmount = nzd(b.Amount);
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
        res.put("voucherAmount", head.VoucherAmount);
        res.put("warnings", warnings);
        res.put("message", (insert ? "Voucher Save Successfully...[" : "Voucher Update Successfully...[") + head.VoucherCode + "]");
        return res;
    }

    /**
     * btnplus_Click / btnUpdateDetail_Click FormValidation for every posted row: a row that skips it was not built
     * by the screen. (Update says "Account Title Field Required"; this runs the Add wording.)
     */
    private void validateRows(List<ExpenseTaxVoucherDto.Row> rows, Map<Integer, String> accounts,
                              boolean subsidiaryFeature, boolean branchFeature) {
        Map<Integer, Boolean> hasSub = new HashMap<>();
        for (ExpenseTaxVoucherDto.Row r : rows) {
            if (nz(r.AccountId) == 0 || !accounts.containsKey(nz(r.AccountId))) throw new IllegalArgumentException("Debit Account Title Field Required");
            if (subsidiaryFeature) {
                Boolean has = hasSub.get(nz(r.AccountId));
                if (has == null) { has = !subsidiaries(nz(r.AccountId)).isEmpty(); hasSub.put(nz(r.AccountId), has); }
                if (has && nz(r.SubsidiaryAccountId) == 0) throw new IllegalArgumentException("Subsidiary Account Title Field Required");
            }
            if (nz(r.JobLotId) == 0) throw new IllegalArgumentException("Job/Lot Field Required");
            if (nz(r.TcyCodeId) == 0) throw new IllegalArgumentException("Tcy Code Field is Required");
            if (nzd(r.TcyExchangeRate) == 0d) throw new IllegalArgumentException("Tcy Exchange Rate Field is Required");
            if (nzd(r.Amount) == 0d) throw new IllegalArgumentException("Amount Field Required");
            if (branchFeature && nz(r.BranchId) == 0) throw new IllegalArgumentException("BranchName Field is Required");
        }
    }

    /** AccountsBudgetHeader.BudgetBalances with the form's own argument: a new ReportsParameters carrying only DocDate and AccountId (Org/Company 0). */
    private List<Map<String, Object>> budgetBalances(String voucherDate, int accountId) {
        return DesktopProc.rows(jdbcTemplate, "Sp_AccountBudget_GetAllMethod",
                DesktopProc.params("OrganizationId", 0, "CompanyId", 0, "BudgetGlAcId", accountId,
                        "VoucherDate", voucherDate, "Activity", "BudgetBalances"));
    }

    /** bool.Parse: only True/False parse; a missing row is false. */
    private static boolean parseBoolStrict(String v) {
        if (v == null) return false;
        String s = v.trim();
        if ("true".equalsIgnoreCase(s)) return true;
        if ("false".equalsIgnoreCase(s)) return false;
        throw new IllegalArgumentException("String was not recognized as a valid Boolean.");
    }

    /** .NET's default double.ToString(): no trailing ".0". */
    private static String net(double d) {
        if (d == Math.rint(d) && !Double.isInfinite(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
    }

    // ================================================================================ rights

    /** CommonServices.SetRightsValueInRightsObject("ExpenseVoucherNew") with its Admin short-circuit. */
    public Map<String, Boolean> rights() {
        Map<String, Boolean> r = new LinkedHashMap<>();
        for (String n : new String[] {"Save", "Update", "Print", "Delete"}) r.put(n.toLowerCase(), hasRight(n));
        return r;
    }

    public boolean hasRight(String rightName) {
        String role = currentUserContext.currentRoleName();
        if ("Admin".equals(role)) return true;
        try {
            for (Map<String, Object> row : jdbcTemplate.queryForList(SQL_USER_RIGHTS, currentUserContext.currentUserId(),
                    SCREEN_NAME, role == null ? "" : role, currentUserContext.currentCompanyId(), "GetByUserId")) {
                Object n = ci(row, "RightName");
                if (n != null && rightName.equals(String.valueOf(n).trim())) return truthy(ci(row, "Value"));
            }
        } catch (Exception e) {
            LOG.warn("Could not read the '{}' right for {}; denying", rightName, SCREEN_NAME, e);
        }
        return false;
    }

    // ================================================================================ helpers

    private static int nz(Integer v) { return v == null ? 0 : v; }
    private static double nzd(Double v) { return v == null ? 0d : v; }
    private static String nzs(String v) { return v == null ? "" : v; }
    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    private static Object pos(Map<String, Object> row, int index) {
        int i = 0;
        for (Object v : row.values()) { if (i++ == index) return v; }
        return null;
    }

    private static String dateTimeStr(Object v) {
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().withNano(0).toString().replace('T', ' ');
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
