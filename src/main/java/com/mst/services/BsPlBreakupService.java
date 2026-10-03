package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

import static com.mst.services.AccountReportsHSupport.col;
import static com.mst.services.AccountReportsHSupport.params;
import static com.mst.services.AccountReportsHSupport.toInt;

/**
 * Screen 84 "BS and PL Breakup" (TargetUrl Architecture.WinApp.Account_Reports.BSandPLBreakup) as a standalone page
 * (/accounts/reports/bs-pl-breakup, the hub card).
 *
 * The desktop form only has the constructor (UserAccount, DataTable): BalanceSheet.GetBsBreakup and
 * frmProfitLossHararical.GetBsBreakup hand it their own data table (dtvoucher) and it never runs a report procedure.
 * Opened on its own there is no parent table, so this service returns exactly the table that parent builds, in the
 * parent's default state (branch combo Text = UserAccount.BranchName, as both parents' BranchFill leave it):
 *   status 0 (BS notes)  BalanceSheet.BindDatainGrid -> VoucherReports.AccountsBalanceSheetStandardFormatII (BLL 0141:2456)
 *                        -> SpAccounts_BalanceSheetFormatA_Report @OrganizationId, @CompanyId, @UserId, @ToDate,
 *                           @BranchesIds only when GetBranchesIdsByFeature returned one (FromDate is never set there).
 *   status 1 (PL notes)  frmProfitLossHararical.ProfitAndLossStatic -> VoucherReports.ProftLoss (BLL 0141:2833)
 *                        -> SpAccounts_ProfitLoassFormatA_Report @OrganizationId, @CompanyId, @UserId, @FromDate, @ToDate,
 *                           @BranchesIds only when non-empty.
 * The Account Notes combo (AccountTitleFill) is BalanceSheetReportService.breakupNotes (SP_AccountNotes_ReadAllMethodBySPType).
 * Tenancy only from CurrentUserContext.
 */
@Service
public class BsPlBreakupService {

    @Autowired
    private AccountReportsHSupport h;

    /** VoucherValidation_Load needs ActiveYr.Start_Period; the grid needs the amount format. */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("amountDecimals", h.amountDecimals());
        out.put("financialYearStart", h.financialYearStart());
        return out;
    }

    /** dtvoucher - the parent's data table (see the class comment). */
    public List<Map<String, Object>> data(Map<String, Object> body) {
        int status = toInt(body.get("status"));
        String branchIds = defaultBranchIds();
        java.sql.Date to = AccountReportsHSupport.date(body.get("toDate"), "To Date");
        if (status == 1) {
            return h.rows("SpAccounts_ProfitLoassFormatA_Report", params(
                    "OrganizationId", h.org(),
                    "CompanyId", h.comp(),
                    "UserId", h.user(),
                    "FromDate", AccountReportsHSupport.date(body.get("fromDate"), "From Date"),
                    "ToDate", to,
                    "BranchesIds", branchIds.isEmpty() ? null : branchIds));
        }
        return h.rows("SpAccounts_BalanceSheetFormatA_Report", params(
                "OrganizationId", h.org(),
                "CompanyId", h.comp(),
                "UserId", h.user(),
                "ToDate", to,
                "BranchesIds", branchIds.isEmpty() ? null : branchIds));
    }

    /**
     * InfragisticsHelper.GetBranchesIdsByFeature over the parent's untouched branch combo: features 17+18 -> the checked
     * list whose Text is UserAccount.BranchName; 17 only -> the row whose BranchName is UserAccount.BranchName (none ->
     * "Please Select Branch first!"); otherwise "".
     */
    private String defaultBranchIds() {
        boolean f17 = h.erpFeature(AccountReportsHSupport.FEATURE_BRANCH);
        if (!f17) return "";
        String name = h.userBranchName();
        boolean f18 = h.erpFeature(AccountReportsHSupport.FEATURE_BRANCH_CONSOLIDATED);
        if (f18) return h.branchIdsByFeature(name, 0);
        int id = 0;
        for (Map<String, Object> b : h.branchesFromVouchers()) {
            if (String.valueOf(col(b, "BranchName")).equals(name)) { id = toInt(col(b, "Id")); break; }
        }
        return h.branchIdsByFeature(name, id);
    }
}
