package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

import static com.mst.services.AccountReportsHSupport.params;
import static com.mst.services.AccountReportsHSupport.toInt;

/**
 * Screen 64 "Profit &amp; Loss 01" (ScreenName/TargetUrl frmProfitLossHararical -
 * frmProfitLossHararical.cs).
 *
 *   GridBind (:124)          VoucherReports.HararicalTrialBalance (BLL 0141) -> SpCoahierarchy_TrialBalance_Rpt
 *                            @OrganizationId, @CompanyId, @FromDate, @ToDate, @AccountClass='4,5'.
 *                            The form computes BranchIds and passes them in, but GridBind never puts
 *                            them on the ReportsParameters, so @BranchesIds is never sent; SkipZero is 0.
 *   ProfitAndLossStatic      VoucherReports.ProftLoss (BLL 0141) -> SpAccounts_ProfitLoassFormatA_Report
 *                            @OrganizationId, @CompanyId, @UserId, @FromDate, @ToDate,
 *                            @BranchesIds when non-empty (AccountNoteId 0 - never sent).
 *   166-Profit &amp; Loss      btnProfitAndLoss_Click -> VoucherReports.Accounts_ProfitLoss_ForCrystal
 *                            -> SpAccounts_ProfitLoss_ForCrystal @OrganizationId, @CompanyId,
 *                            @ClassIds='4,5', @UserId, @FromDate, @ToDate. The form sets BranchesId,
 *                            which that BLL method never reads (it only sends BranchesIds) - so no
 *                            branch parameter is sent.
 */
@Service
public class ProfitLossHierarchicalReportService {

    @Autowired
    private AccountReportsHSupport h;

    /** frmTrialBalancesAllLevel_Load: features, BranchFill, txtFromDate = ActiveYr.Start_Period. */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>(h.branchContext());
        out.put("amountDecimals", h.amountDecimals());
        out.put("financialYearStart", h.financialYearStart());
        return out;
    }

    /** btnshow_Click: GetBranchesIdsByFeature, then GridBind + ProfitAndLossStatic. */
    public Map<String, Object> show(Map<String, Object> body, boolean withStatic) {
        String branchIds = h.branchIdsByFeature(str(body.get("branchText")), toInt(body.get("branchId")));
        java.sql.Date from = AccountReportsHSupport.date(body.get("fromDate"), "From Date");
        java.sql.Date to = AccountReportsHSupport.date(body.get("toDate"), "To Date");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("grid", h.rows("SpCoahierarchy_TrialBalance_Rpt", params(
                "OrganizationId", h.org(),
                "CompanyId", h.comp(),
                "FromDate", from,
                "ToDate", to,
                "AccountClass", "4,5")));
        if (withStatic) {
            out.put("pl", h.rows("SpAccounts_ProfitLoassFormatA_Report", params(
                    "OrganizationId", h.org(),
                    "CompanyId", h.comp(),
                    "UserId", h.user(),
                    "FromDate", from,
                    "ToDate", to,
                    "BranchesIds", branchIds.isEmpty() ? null : branchIds)));
        }
        return out;
    }

    /** btnProfitAndLoss_Click - the rows 166-ProfitLossNewReport.rpt is filled with. */
    public List<Map<String, Object>> print166(Map<String, Object> body) {
        List<Map<String, Object>> rows = h.rows("SpAccounts_ProfitLoss_ForCrystal", params(
                "OrganizationId", h.org(),
                "CompanyId", h.comp(),
                "ClassIds", "4,5",
                "UserId", h.user(),
                "FromDate", AccountReportsHSupport.date(body.get("fromDate"), "From Date"),
                "ToDate", AccountReportsHSupport.date(body.get("toDate"), "To Date")));
        if (rows.isEmpty()) throw new AccountReportsHSupport.Refusal("Not Record Found For Display");
        return rows;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o); }
}
