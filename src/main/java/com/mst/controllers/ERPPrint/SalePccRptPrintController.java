package com.mst.controllers.ERPPrint;

import com.mst.repositories.ICompanyRepository;
import com.mst.services.SalePccRptService;
import java.util.*;
import javax.servlet.http.HttpServletResponse;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

/**
 * Group P prints. The desktop hands the LAST SHOW's DataTable to the Crystal template (Reporting.ShowReportWithDataTable(dt, "...rpt")) with
 * CompanyName / CompanyAddress. The web re-runs the SAME procedure with the SAME arguments the last Show used (the page passes them back) and renders
 * the same template through the shared Jasper pipeline (ReportPrintSupport.printReportData).
 *   451  1860-StockTransferManual_Register.rpt
 *   561  the 1880..1899 templates by activity (PrintButtonManage)
 *   562  Detail 1865-SalesWages_Register.rpt, Summary 1865_01_SalesWages_SummaryRegister.rpt
 */
@Controller
@RequestMapping("/sale/reports/pcc/print")
public class SalePccRptPrintController extends ReportPrintSupport {
    /** 561 PrintButtonManage: the Print button text (= the template name) of each activity. */
    private static final Map<String, String> ACTIVITY_TEMPLATE;
    static {
        Map<String, String> m = new LinkedHashMap<>();
        m.put("Sales Register", "1880-SalesRegisterSummary");
        m.put("Sales Summary By Item", "1889-SalesRegisterSummaryByItem");
        m.put("Sales Summary By Item & City", "1890-SalesSummaryByItem&City");
        m.put("Sales Summary By Item & Varient", "1891-SalesRegisterSummaryByItem&Varient");
        m.put("Sales Summary By Item & Warehouse", "1892-SalesRegisterSummaryByItem&Warehouse");
        m.put("Sales Summary By Item,Varient & City", "1893-SalesSummaryByItemPackSize&City");
        m.put("Sales Summary By Customer", "1894-SalesRegisterSummaryByCustomer");
        m.put("Sales Summary By Customer & Item", "1895-SalesSummaryByCustomer&Item");
        m.put("Sales Summary By Customer & City", "1896-SalesRegisterSummaryByCustomer&City");
        m.put("Sales Summary By Customer,Item & City", "1898-SalesSummaryByCustomerItem&City");
        m.put("Sales Summary By Customer & Pack Size", "1897-SalesSummaryByCustomer&Varient");
        m.put("Sales Summary By City", "1899-SalesSummaryByCity");
        m.put("Sales Summary By Parent Category", "1881-SalesSummaryByParentCategory");
        m.put("Sales Summary By Parent Category & Item", "1882-SalesSummaryByParentCategory&Item");
        m.put("Sales Summary By Parent Category & Customer", "1883-SalesSummaryByParentCategory&Customter");
        m.put("Sales Summary By Vehicles", "1884-SalesSummaryByVehicles");
        m.put("Sales Summary By Customer,Item & Varient", "1885-SalesSummaryByCustomerItemandVareient");
        ACTIVITY_TEMPLATE = Collections.unmodifiableMap(m);
    }

    private final SalePccRptService service;
    private final ICompanyRepository companies;

    public SalePccRptPrintController(SalePccRptService service, ICompanyRepository companies) {
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

    private Map<String, Object> result(List<Map<String, Object>> rows) {
        String[] h = header();
        Map<String, Object> params = new LinkedHashMap<>();
        params.put("@CompanyName", h[0]);
        params.put("@CompanyAddress", h[1]);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        out.put("reportParameters", params);
        return out;
    }

    /** toolStripButton1_Click: "Not Record Found For Display" when the last Show gave no rows. */
    @GetMapping("/stock-transfer-register")
    public void stockTransferRegister(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        List<Map<String, Object>> rows = service.rows("stock-transfer-register", q);
        if (rows.isEmpty()) throw new IllegalArgumentException("Not Record Found For Display");
        printReportData(response, "1860-StockTransferManual_Register.rpt", result(rows));
    }

    /**
     * btnPrint_Click: the template named by the Print button text at the time of the click (PrintButtonManage keeps the previous text for an activity
     * without a mapping, so the template and the activity of the last Show can differ); the table printed is the last Show's.
     */
    @GetMapping("/sales-evaluation-detail")
    public void salesEvaluationDetail(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        String template = q.get("template");
        if (template == null || !(ACTIVITY_TEMPLATE.containsValue(template) || "1897-SalesSummaryByCustomer&Varient".equals(template))) throw new IllegalArgumentException("Invalid report name.");
        List<Map<String, Object>> rows = service.rows("sales-evaluation-detail", q);
        if (rows.isEmpty()) throw new IllegalArgumentException("Record Not Found For Display");
        printReportData(response, template + ".rpt", result(rows));
    }

    /** print_Click_1 (Detail, 1865-Print) / btnPrint1865_01_Click (Summary, 1865_01-Print). */
    @GetMapping("/sales-wages-register")
    public void salesWagesRegister(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        boolean summary = q.get("kind") != null ? "1865_01".equals(q.get("kind")) : "2".equals(q.get("activity"));   // the button (kind) decides the template, as print_Click_1 / btnPrint1865_01_Click
        List<Map<String, Object>> rows = service.rows("sales-wages-register", q);
        if (rows.isEmpty()) throw new IllegalArgumentException("Not Record Found For Display");
        printReportData(response, summary ? "1865_01_SalesWages_SummaryRegister.rpt" : "1865-SalesWages_Register.rpt", result(rows));
    }
}
