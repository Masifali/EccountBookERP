package com.mst.controllers.ERPPrint;

import com.mst.models.SaleOrderReportFilter;
import com.mst.models.SaleOrderReportColumns;
import com.mst.repositories.ICompanyRepository;
import com.mst.services.SaleOrderReportService;
import org.springframework.web.bind.annotation.*;
import javax.servlet.http.HttpServletResponse;
import java.util.*;

/** frmSaleOrderHistory prints its loaded register, with the same branch/customer filters as Show. */
@RestController
@RequestMapping("/sale/reports/sale-order-report/api/print")
public class SaleOrderReportPrintController extends ReportPrintSupport {
    private final SaleOrderReportService service;
    private final ICompanyRepository companies;
    public SaleOrderReportPrintController(SaleOrderReportService service, ICompanyRepository companies) {
        this.service = service; this.companies = companies;
    }

    @PostMapping("/{variant}")
    public void print(HttpServletResponse response, @PathVariable("variant") String variant,
            @RequestBody SaleOrderReportFilter filter) throws Exception {
        String template = switch (variant) {
            case "summary" -> SaleOrderReportColumns.summaryPrintTemplate(filter.activity());
            case "270" -> "270-InvRptSaleOderRegister.rpt";
            case "271" -> "271-InvRptSalesOrderRegister.rpt";
            case "271A" -> "271A-InvRptSalesOrderRegister.rpt";
            default -> throw new IllegalArgumentException("Select a valid Sale Order print");
        };
        // Never accept company/user IDs, raw rows, or an arbitrary template from the browser.
        var rows = "summary".equals(variant) ? service.summaryRows(filter) : service.detailRows(filter);
        var company = companies.findById(currentUserContext.currentCompanyId()).orElse(null);
        Map<String, Object> parameters = new LinkedHashMap<>();
        parameters.put("CompanyName", company == null ? "" : Objects.toString(company.getCompName(), ""));
        parameters.put("CompanyAddress", company == null ? "" : Objects.toString(company.getCompAddress(), ""));
        printReportData(response, template, Map.of("rows", rows, "reportParameters", parameters));
    }
}
