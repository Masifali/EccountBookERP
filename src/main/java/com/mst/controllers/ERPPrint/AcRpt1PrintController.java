package com.mst.controllers.ERPPrint;

import com.mst.models.UserAccount;
import com.mst.repositories.ICompanyRepository;
import com.mst.services.AcRpt1Service;
import java.time.LocalDate;
import java.util.*;
import javax.servlet.http.HttpServletResponse;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

/**
 * Group R1 prints. The desktop hands the LAST SHOW's DataTable to the Crystal template
 * (Reporting.ShowReportWithDataTable(dtrpt, "...rpt")) with @CompanyName / @CompanyAddress. The web re-runs the SAME
 * procedure with the SAME arguments the last Show used and renders the same template through the shared Jasper pipeline
 * (ReportPrintSupport.printReportData), so the template's own columns are fed exactly like the desktop.
 *   48  121-Accounts_Payables_Rpt.rpt            @CompanyName only
 *   63  142-DueByDatePayablesAndReceivables.rpt  @CompanyAddress + @CompanyName
 *   70  380-DueByDateReceivables.rpt             @CompanyAddress + @CompanyName
 *   68  121_01-ReceivablesByDueDates.rpt         @CompanyAddress + @CompanyName
 */
@Controller
public class AcRpt1PrintController extends ReportPrintSupport {
    private final AcRpt1Service service;
    private final ICompanyRepository companies;

    public AcRpt1PrintController(AcRpt1Service service, ICompanyRepository companies) {
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
        params.put("@CompanyName", h[0]);
        if (withAddress) params.put("@CompanyAddress", h[1]);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        out.put("reportParameters", params);
        return out;
    }

    @GetMapping("/accounts/reports/acrpt1/print/payables")
    public void payables(HttpServletResponse response,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate,
            @RequestParam(defaultValue = "0") int actionId, @RequestParam(defaultValue = "0") int cityId,
            @RequestParam(defaultValue = "") String branchesIds, @RequestParam(defaultValue = "") String controlAccountIds,
            @RequestParam(defaultValue = "0") int customGroupId, @RequestParam(defaultValue = "") String customerGroupIds,
            @RequestParam(defaultValue = "0") double balanceFrom, @RequestParam(defaultValue = "0") double balanceTo,
            @RequestParam(defaultValue = "0") int showAssetLiability) throws Exception {
        List<Map<String, Object>> rows = service.payables(fromDate, toDate, actionId, cityId, branchesIds, controlAccountIds, customGroupId,
                customerGroupIds, balanceFrom, balanceTo, showAssetLiability);
        printReportData(response, "121-Accounts_Payables_Rpt.rpt", result(rows, false));
    }

    @GetMapping("/accounts/reports/acrpt1/print/payables-due")
    public void payablesDue(HttpServletResponse response,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate dueDateTo,
            @RequestParam(defaultValue = "") String ids, @RequestParam(defaultValue = "0") int fromDocNo,
            @RequestParam(defaultValue = "0") int toDocNo, @RequestParam(defaultValue = "0") int languageId) throws Exception {
        printReportData(response, "142-DueByDatePayablesAndReceivables.rpt", result(service.payablesDue(dueDateTo, ids, fromDocNo, toDocNo, languageId), true));
    }

    @GetMapping("/accounts/reports/acrpt1/print/receivables-due")
    public void receivablesDue(HttpServletResponse response,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate dueDateTo,
            @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo,
            @RequestParam(defaultValue = "0") int languageId, @RequestParam(defaultValue = "0") int customGroupId,
            @RequestParam(defaultValue = "") String ids, @RequestParam(defaultValue = "") String parentAccountCode) throws Exception {
        printReportData(response, "380-DueByDateReceivables.rpt",
                result(service.receivablesDue(dueDateTo, fromDocNo, toDocNo, languageId, customGroupId, ids, parentAccountCode), true));
    }

    @GetMapping("/accounts/reports/acrpt1/print/receivables-new")
    public void receivablesNew(HttpServletResponse response,
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate,
            @RequestParam(defaultValue = "0") int customGroupId, @RequestParam(defaultValue = "0") int parentId) throws Exception {
        printReportData(response, "121_01-ReceivablesByDueDates.rpt", result(service.receivablesNew(fromDate, toDate, customGroupId, parentId), true));
    }
}
