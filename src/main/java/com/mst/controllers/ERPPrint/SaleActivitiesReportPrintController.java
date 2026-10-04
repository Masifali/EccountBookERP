package com.mst.controllers.ERPPrint;

import com.mst.models.*;
import com.mst.repositories.ICompanyRepository;
import com.mst.services.SaleActivitiesReportService;
import org.springframework.web.bind.annotation.*;
import javax.servlet.http.HttpServletResponse;
import java.util.*;

/** frmEvaulationDetailSalesReports: the chosen activity determines its desktop print template. */
@RestController
@RequestMapping("/sale/reports/sale-invoice-report-with-activities/api/print")
public class SaleActivitiesReportPrintController extends ReportPrintSupport {
    private final SaleActivitiesReportService service;
    private final ICompanyRepository companies;
    public SaleActivitiesReportPrintController(SaleActivitiesReportService service,ICompanyRepository companies){this.service=service;this.companies=companies;}
    @PostMapping
    public void print(HttpServletResponse response,@RequestBody SaleActivitiesReportFilter filter) throws Exception {
        var rows=service.reportRows(filter);
        var company=companies.findById(currentUserContext.currentCompanyId()).orElse(null);
        var parameters=new LinkedHashMap<String,Object>();
        parameters.put("CompanyName",company==null?"":Objects.toString(company.getCompName(),""));
        parameters.put("CompanyAddress",company==null?"":Objects.toString(company.getCompAddress(),""));
        printReportData(response,SaleActivitiesReportColumns.printTemplate(filter.activity()),Map.of("rows",rows,"reportParameters",parameters));
    }
}
