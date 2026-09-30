package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

import static com.mst.services.AccountReportsHSupport.params;
import static com.mst.services.AccountReportsHSupport.toInt;

/**
 * Screen 62 "Balance Sheet" (ScreenName frmBalanceSheet, TargetUrl
 * Architecture.WinApp.Account_Reports.BalanceSheet - BalanceSheet.cs) and the breakup dialog it
 * opens, screen 84 "BS and PL Breakup" (BSandPLBreakup.cs).
 *
 *   Load / Show  BindDatainGrid (:261) -> VoucherReports.AccountsBalanceSheetStandardFormatII
 *                (BLL 0141:2456) -> SpAccounts_BalanceSheetFormatA_Report
 *                @OrganizationId, @CompanyId, @UserId, @ToDate (FromDate is never set, so never
 *                sent), @BranchesIds only when GetBranchesIdsByFeature returned a non-empty string.
 *   153-Print    toolStripButton1_Click (:517) -> AccountsBalanceSheetStandardForCrystal
 *                (BLL 0141:2569) -> SpAccounts_BalanceSheetFormatA_ForCrystal
 *                @OrganizationId, @CompanyId, @ClassIds='1,2,3', @UserId, @ToDate,
 *                @BranchesId only when BranchFeature and the combo value is non-zero.
 *   Breakup      BSandPLBreakup.AccountTitleFill -> AccountNotes.ReadByBSNote(0) (Status 0) /
 *                ReadByPLNote() (Status 1) -> SP_AccountNotes_ReadAllMethodBySPType
 *                @AccountNotesType = 'ReadAllMethodByBSNote' | 'ReadAllMethodByPLNote'
 *                (@AccountClassId only when non-zero - never, the form passes 0).
 */
@Service
public class BalanceSheetReportService {

    @Autowired
    private AccountReportsHSupport h;

    /** BalanceSheet_Load (:558): features 17/18, BranchFill, amount format. */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>(h.branchContext());
        out.put("amountDecimals", h.amountDecimals());
        out.put("financialYearStart", h.financialYearStart());
        return out;
    }

    /** BindDatainGrid (:261). */
    public List<Map<String, Object>> show(Map<String, Object> body) {
        String branchIds = h.branchIdsByFeature(str(body.get("branchText")), toInt(body.get("branchId")));
        return h.rows("SpAccounts_BalanceSheetFormatA_Report", params(
                "OrganizationId", h.org(),
                "CompanyId", h.comp(),
                "UserId", h.user(),
                "ToDate", AccountReportsHSupport.date(body.get("toDate"), "To Date"),
                "BranchesIds", branchIds.isEmpty() ? null : branchIds));
    }

    /** toolStripButton1_Click (:517) - the rows 153-BalanceSheetStatementRpt.rpt is filled with. */
    public List<Map<String, Object>> print153(Map<String, Object> body) {
        boolean f17 = h.erpFeature(AccountReportsHSupport.FEATURE_BRANCH);
        boolean f18 = h.erpFeature(AccountReportsHSupport.FEATURE_BRANCH_CONSOLIDATED);
        /* BranchesId = BranchFeature ? Conversion.ToInt(cmbbranch.Value) : 0. With the consolidated
           checked list the Value is the checked-items list, which Conversion.ToInt turns into 0. */
        int branchId = (f17 && !f18) ? toInt(body.get("branchId")) : 0;
        List<Map<String, Object>> rows = h.rows("SpAccounts_BalanceSheetFormatA_ForCrystal", params(
                "OrganizationId", h.org(),
                "CompanyId", h.comp(),
                "ClassIds", "1,2,3",
                "UserId", h.user(),
                "ToDate", AccountReportsHSupport.date(body.get("toDate"), "To Date"),
                "BranchesId", branchId != 0 ? branchId : null));
        if (rows.isEmpty()) throw new AccountReportsHSupport.Refusal("Not Record Found For Display");
        return rows;
    }

    /** BSandPLBreakup.AccountTitleFill: status 1 = PL notes, anything else = BS notes. */
    public List<Map<String, Object>> breakupNotes(int status) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : h.rows("SP_AccountNotes_ReadAllMethodBySPType",
                params("AccountNotesType", status == 1 ? "ReadAllMethodByPLNote" : "ReadAllMethodByBSNote"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", AccountReportsHSupport.col(r, "Id"));
            m.put("NoteTitle", AccountReportsHSupport.col(r, "NoteTitle"));
            out.add(m);
        }
        return out;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o); }
}
