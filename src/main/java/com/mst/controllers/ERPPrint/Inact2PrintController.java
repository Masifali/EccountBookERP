package com.mst.controllers.ERPPrint;

import com.mst.repositories.ICompanyRepository;
import com.mst.services.Inact2Service;
import java.time.LocalDate;
import java.util.*;
import javax.servlet.http.HttpServletResponse;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

/**
 * Inactive Account_Reports group K prints. The desktop hands a DataTable to the Crystal template
 * (Reporting.ShowReportWithDataTable(dt, "...rpt")) with @CompanyAddress / @CompanyName. The web re-runs the SAME BLL call
 * with the same arguments and renders the same template through the shared Jasper pipeline (ReportPrintSupport.printReportData).
 *   54 109-AcRptGeneralLedgerStatement.rpt        GeneralLedgerStatement (FromDate, ToDate, AccountId only)  (@CompanyAddress + @CompanyName)
 *   55 151-AcRptAccountsBalanceSheetStandard.rpt  AccountsBalanceSheetStandardRpt  (@CompanyAddress + @CompanyName)
 *   55 152-AcRptAccountsProfitLoss.rpt            AccountsProfitLoassStandard      (both)
 *   55 153-BalanceSheetStatementRpt.rpt           AccountsBalanceSheetStandardFormatII (both)
 *   55 154-Profit&amp;Loss.rpt                    ProfitandLoss                    (both)
 *   58 120-AcRptPayablesAging.rpt                 LedgerAging                      (both)
 *   59 111-AcRptReceivablesAging.rpt              ReceivableAging                  (@CompanyName only)
 */
@Controller
public class Inact2PrintController extends ReportPrintSupport {
    private final Inact2Service service;
    private final ICompanyRepository companies;

    public Inact2PrintController(Inact2Service service, ICompanyRepository companies) {
        this.service = service;
        this.companies = companies;
    }

    private String[] header() {
        String name = "", address = "";
        try {
            com.mst.models.Company c = companies.findById(currentUserContext.currentCompanyId()).orElse(null);
            if (c != null) { name = c.getCompName() == null ? "" : c.getCompName(); address = c.getCompAddress() == null ? "" : c.getCompAddress(); }
        } catch (Exception ignored) { /* a missing header is cosmetic */ }
        return new String[] { name, address };
    }

    private Map<String, Object> result(List<Map<String, Object>> rows, boolean withAddress) {
        String[] h = header();
        Map<String, Object> params = new LinkedHashMap<>();
        if (withAddress) params.put("@CompanyAddress", h[1]);
        params.put("@CompanyName", h[0]);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        out.put("reportParameters", params);
        return out;
    }

    @GetMapping("/accounts/reports/inact2/print/ledger-statement-109")
    public void ledgerStatement109(HttpServletResponse response, @RequestParam(defaultValue = "0") int accountId,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate) throws Exception {
        /* print_Click_1 passes only FromDate / ToDate / AccountId (no branch, no project) */
        printReportData(response, "109-AcRptGeneralLedgerStatement.rpt", result(service.ledgerStatement(accountId, fromDate, toDate, 0, 0), true));
    }

    @GetMapping("/accounts/reports/inact2/print/balance-sheet-151")
    public void balanceSheet151(HttpServletResponse response,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate) throws Exception {
        printReportData(response, "151-AcRptAccountsBalanceSheetStandard.rpt", result(service.balanceSheet(toDate), true));
    }

    @GetMapping("/accounts/reports/inact2/print/profit-loss-152")
    public void profitLoss152(HttpServletResponse response,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate) throws Exception {
        printReportData(response, "152-AcRptAccountsProfitLoss.rpt", result(service.profitLoss(fromDate, toDate), true));
    }

    @GetMapping("/accounts/reports/inact2/print/balance-sheet-153")
    public void balanceSheet153(HttpServletResponse response,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate) throws Exception {
        printReportData(response, "153-BalanceSheetStatementRpt.rpt", result(service.balanceSheetFormat2(fromDate, toDate), true));
    }

    @GetMapping("/accounts/reports/inact2/print/profit-loss-154")
    public void profitLoss154(HttpServletResponse response,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate) throws Exception {
        printReportData(response, "154-Profit&Loss.rpt", result(service.profitAndLoss(toDate), true));
    }

    @GetMapping("/accounts/reports/inact2/print/payables-aging-120")
    public void payablesAging120(HttpServletResponse response,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate endDate,
            @RequestParam(defaultValue = "0") int agingDays, @RequestParam(defaultValue = "0") int intervalDays,
            @RequestParam(defaultValue = "false") boolean skipZero) throws Exception {
        printReportData(response, "120-AcRptPayablesAging.rpt", result(service.payablesAging(endDate, agingDays, intervalDays, skipZero), true));
    }

    @GetMapping("/accounts/reports/inact2/print/receivable-aging-111")
    public void receivableAging111(HttpServletResponse response,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate endDate,
            @RequestParam(defaultValue = "0") int intervalDays) throws Exception {
        printReportData(response, "111-AcRptReceivablesAging.rpt", result(service.receivableAging(endDate, intervalDays), false));
    }
}
