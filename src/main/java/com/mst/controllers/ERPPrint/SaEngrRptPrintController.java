package com.mst.controllers.ERPPrint;

import com.mst.repositories.ICompanyRepository;
import com.mst.services.SaEngrRptService;
import java.util.*;
import javax.servlet.http.HttpServletResponse;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

/**
 * Group E2 prints. The desktop hands the LAST SHOW's DataTable to the Crystal template (Reporting.ShowReportWithDataTable(dt, "...rpt")) with
 * CompanyName / CompanyAddress. The web re-runs the SAME procedure with the SAME arguments the last Show used (the page passes them back) and renders
 * the same template through the shared Jasper pipeline (ReportPrintSupport.printReportData).
 *   555  1612-GdnRegister_Eng.rpt
 *   554  detail tab 299-SalesOrderRegistery.rpt; summary tab one of the eleven 1605_nn_*.rpt (named by the Print button text of the last activity)
 *   556  the 360..374 / 366A templates by activity; 366B-SalesSummaryByCustomer&Invoice.rpt for the Print-366B button
 */
@Controller
@RequestMapping("/sale/reports/saengr/print")
public class SaEngrRptPrintController extends ReportPrintSupport {
    private static final Set<String> ORDER_SUMMARY_TEMPLATES = new HashSet<>(Arrays.asList(
            "1605_01_OrderRegister", "1605_02_OrderSummaryByItem&PackSize", "1605_03_OrderSummaryByItem,PackSize&City",
            "1605_04_OrderSummaryByCustomer&PackSize", "1605_05_OrderSummaryByItem", "1605_06_OrderSummaryByItem&City",
            "1605_07_OrderSummaryByCustomer", "1605_08_OrderSummaryByCustomer&City", "1605_09_OrderSummaryByCustomer&Item",
            "1605_10_OrderSummaryByCustomer,Item&City", "1605_11_OrderSummaryByCustomer&Order"));
    /** PrintButtonManage: the Print button text (= the template name) of each activity. */
    private static final Map<String, String> ACTIVITY_TEMPLATE;
    static {
        Map<String, String> m = new LinkedHashMap<>();
        m.put("Sales Register", "360-SalesRegisterSummary");
        m.put("Sales Summary By Item", "361-SalesSummaryByItem");
        m.put("Sales Summary By Item & City", "362-SalesSummaryByItem&City");
        m.put("Sales Summary By Item & Pack Size", "363-SalesSummaryByItem&PackSize");
        m.put("Sales Summary By Item & Warehouse", "364-SalesSummaryByItem&Warehouse");
        m.put("Sales Summary By Item,Pack Size & City", "365-SalesSummaryByItemPackSize&City");
        m.put("Sales Summary By Customer", "366-SalesSummaryByCustomer");
        m.put("Sales Summary By Customer & Item", "367-SalesSummaryByCustomer&Item");
        m.put("Sales Summary By Customer & City", "368-SalesSummaryByCustomer&City");
        m.put("Sales Summary By Customer,Item & City", "369-SalesSummaryByCustomerItem&City");
        m.put("Sales Summary By Customer & Pack Size", "370-SalesSummaryByCustomer&PackSize");
        m.put("Sales Summary By City", "371-SalesSummaryByCity");
        m.put("Sales Summary By Parent Category", "372-SalesSummaryByParentCategory");
        m.put("Sales Summary By Parent Category & Item", "373-SalesSummaryByParentCategory&Item");
        m.put("Sales Summary By Parent Category & Customer", "374-SalesSummaryByParentCategory&Customer");
        m.put("Sales Summary By Customer & Invoice", "366ASalesSummaryByCustomer&Invoice");
        ACTIVITY_TEMPLATE = Collections.unmodifiableMap(m);
    }

    /** 848 PrintButtonManage: the Print button text is the report name (btnPrintSummary_Click appends ".rpt"). */
    private static final Set<String> MFG_ORDER_SUMMARY_TEMPLATES = new HashSet<>(Arrays.asList(
            "1656_01_OrderRegister", "1656_02_OrderSummaryByItem&PackSize", "1656_03_OrderSummaryByItem,PackSize&City",
            "1656_04_OrderSummaryByCustomer&PackSize", "1656_05_OrderSummaryByItem", "1656_06_OrderSummaryByItem&City",
            "1656_07_OrderSummaryByCustomer", "1656_08_OrderSummaryByCustomer&City", "1656_09_OrderSummaryByCustomer&Item",
            "1656_10_OrderSummaryByCustomer,Item&City", "1656_11_OrderSummaryByCustomer&Order"));

    private final SaEngrRptService service;
    private final ICompanyRepository companies;

    public SaEngrRptPrintController(SaEngrRptService service, ICompanyRepository companies) {
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

    @GetMapping("/gdn-history")
    public void gdnHistory(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        printReportData(response, "1612-GdnRegister_Eng.rpt", result(service.rows("gdn-history", q)));
    }

    /** kind=detail: print_Click (299-SalesOrderRegistery.rpt over the detail rows); kind=summary: btnPrintSummary_Click (template = the Print button text). */
    @GetMapping("/sale-order-history")
    public void saleOrderHistory(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        if ("summary".equals(q.get("kind"))) {
            String t = q.get("template");
            if (t == null || !ORDER_SUMMARY_TEMPLATES.contains(t)) throw new IllegalArgumentException("Invalid report name.");
            printReportData(response, t + ".rpt", result(service.orderSummaryRows(q)));
            return;
        }
        printReportData(response, "299-SalesOrderRegistery.rpt", result(service.rows("order-history", q)));
    }

    /** 848: kind=detail print_Click (1656_SaleOrderDetailRegister.rpt over the detail rows); kind=summary btnPrintSummary_Click (template = the Print button text). */
    @GetMapping("/mfg-sale-order-history")
    public void mfgSaleOrderHistory(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        if ("summary".equals(q.get("kind"))) {
            String t = q.get("template");
            if (t == null || !MFG_ORDER_SUMMARY_TEMPLATES.contains(t)) throw new IllegalArgumentException("Invalid report name.");
            printReportData(response, t + ".rpt", result(service.mfgOrderSummaryRows(q)));
            return;
        }
        printReportData(response, "1656_SaleOrderDetailRegister.rpt", result(service.rows("mfg-order-history", q)));
    }

    /** kind=366B: btnPrint366B_Click; otherwise btnPrint_Click (the template of the activity). */
    @GetMapping("/sale-report-with-activities")
    public void saleReportWithActivities(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        String template;
        if ("366B".equals(q.get("kind"))) template = "366B-SalesSummaryByCustomer&Invoice";
        else {
            template = ACTIVITY_TEMPLATE.get(q.get("activity"));
            if (template == null) throw new IllegalArgumentException("Invalid report name.");
        }
        printReportData(response, template + ".rpt", result(service.rows("sales-activities", q)));
    }
}
