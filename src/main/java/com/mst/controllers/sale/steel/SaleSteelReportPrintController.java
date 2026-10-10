package com.mst.controllers.sale.steel;

import com.mst.controllers.ERPPrint.ReportPrintSupport;
import com.mst.repositories.ICompanyRepository;
import com.mst.services.sale.steel.SaleSteelReportService;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletResponse;
import java.io.File;
import java.util.*;
import java.util.regex.Pattern;

/**
 * Prints of module 87. The desktop hands the LAST SHOW's DataTable (or, for a row button, the slip procedure's rows) to the Crystal
 * template with CompanyName / CompanyAddress; the web re-runs the same procedure with the same arguments (the page passes them back)
 * and renders the same template through the shared Jasper pipeline (ReportPrintSupport.printReportData).
 *   557 flow tab drop-down  -> a file of the Sales_Steel folder            (mode=flow)
 *   557 direct tab drop-down-> a file of the SalesDirect_Steel folder      (mode=direct)
 *   557 row Print           -> 1514-InvRepSaleBillCustomer.rpt (+ sub report InvRptPurchaseBillSupplierOthers) / 1516-SaleInvoiceCustomerBill.rpt
 *   558 btnPrint            -> the activity's 15xx .rpt over dtGrid
 *   559 register / row      -> 1520-InvRptGatePassOutwardRegister.rpt / 1512-GatePassOutwardSlipAndRegisterSteel.rpt
 *   560 register / DocNo    -> 1509-SaleOrderSlipAndRegister.rpt / 1511-SaleOrderSlipAndRegister.rpt
 */
@Controller
@RequestMapping("/sale/reports/steel/print")
public class SaleSteelReportPrintController extends ReportPrintSupport {
    private static final Pattern DYNAMIC_NAME = Pattern.compile("^[^\\\\/:*?\"<>|]+$");
    /** PrintButtonManage: activity text -> button text = the .rpt name. */
    private static final Map<String, String> ACTIVITY_RPT;
    static {
        Map<String, String> m = new LinkedHashMap<>();
        m.put("Sales Register", "1522-SalesRegisterSummary");
        m.put("Sales Summary By Item", "1526-SalesRegisterSummaryByItemWithoutPacking");
        m.put("Sales Summary By Item & City", "1528-SalesSummaryByItem&City");
        m.put("Sales Summary By Item & Pack Size", "1524-SalesRegisterSummaryByItem");
        m.put("Sales Summary By Item & Warehouse", "1527-SalesRegisterSummaryByWarehouse");
        m.put("Sales Summary By Item,Pack Size & City", "1530-SalesSummaryByItemPackSize&City");
        m.put("Sales Summary By Customer", "1523-SalesRegisterSummaryByCustomer");
        m.put("Sales Summary By Customer & Item", "1525-SalesRegisterSummaryByCustomer&Item");
        m.put("Sales Summary By Customer & City", "1529-SalesSummaryByCustomer&City");
        m.put("Sales Summary By Customer,Item & City", "1531-SalesSummaryByCustomerItem&City");
        m.put("Sales Summary By Customer & Pack Size", "1532-SalesSummaryByCustomer&PackSize");
        m.put("Sales Summary By Parent Category", "1533-SalesSummaryByParentCategory");
        m.put("Sales Summary By Parent Category & Item", "1534-SalesSummaryByParentCategory&Item");
        m.put("Sales Summary By Parent Category & Customer", "1535-SalesSummaryByParentCategory&Customer");
        ACTIVITY_RPT = Collections.unmodifiableMap(m);
    }

    private final SaleSteelReportService service;
    private final ICompanyRepository companies;

    @Value("${reports.crystal.template-root:}")
    private String reportRoot;

    public SaleSteelReportPrintController(SaleSteelReportService service, ICompanyRepository companies) {
        this.service = service; this.companies = companies;
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

    /** CommonServices.DynamicReportsLoad(folder): the file must be one the folder lists (extension *rpt, name shown without it). */
    private String dynamicTemplate(String folder, String name) {
        if (name == null || name.trim().isEmpty() || !DYNAMIC_NAME.matcher(name).matches() || name.contains("..")) throw new IllegalArgumentException("Invalid report name.");
        if (reportRoot != null && !reportRoot.trim().isEmpty()) {
            File[] files = new File(reportRoot, folder).listFiles();
            if (files != null) {
                for (File f : files) {
                    String n = f.getName();
                    if (!f.isFile() || !n.toLowerCase(Locale.ROOT).endsWith("rpt")) continue;
                    int dot = n.lastIndexOf('.');
                    if ((dot > 0 ? n.substring(0, dot) : n).equals(name)) return name + ".rpt";
                }
            }
        }
        throw new IllegalArgumentException("Report " + name + " is not in the " + folder + " folder");
    }

    @GetMapping("/sale-invoice-register")
    public void invoiceRegister(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        boolean direct = "direct".equals(q.get("mode"));
        String template = dynamicTemplate(direct ? "SalesDirect_Steel" : "Sales_Steel", q.get("template"));
        printReportData(response, template, result(service.invoiceRaw(q)));
    }

    /** SaleInvoiceSteelSlip_1514 (mode flow) / SaleInvoiceDirectSteelSlip_1516 (mode direct) */
    @GetMapping("/sale-invoice-slip")
    public void invoiceSlip(HttpServletResponse response, @RequestParam int id, @RequestParam(defaultValue = "flow") String mode) throws Exception {
        boolean direct = "direct".equals(mode);
        Map<String, Object> r = result(service.invoiceSlip(id, direct));
        String[] h = header();
        Map<String, Object> params = new LinkedHashMap<>();
        params.put("CompanyAddress", h[1]);
        params.put("CompanyName", h[0]);
        r.put("reportParameters", params);
        Map<String, Object> sub = new LinkedHashMap<>();
        sub.put("template", direct ? "InvRptPurchaseBillSupplierOthers" : "InvRptPurchaseBillSupplierOthers.rpt");
        sub.put("rows", service.invoiceSlipSub(id));
        r.put("subReports", new ArrayList<>(List.of(sub)));
        printReportData(response, direct ? "1516-SaleInvoiceCustomerBill.rpt" : "1514-InvRepSaleBillCustomer.rpt", r);
    }

    @GetMapping("/sale-invoice-register-with-activities")
    public void activities(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        // btnPrint.Text (set by PrintButtonManage) names the .rpt; only the 14 known names are accepted
        String rpt = q.get("rpt") == null ? "" : q.get("rpt").trim();
        if (!ACTIVITY_RPT.containsValue(rpt)) throw new IllegalArgumentException("Record Not Found For Display");
        printReportData(response, rpt + ".rpt", result(service.activitiesRaw(q)));
    }

    @GetMapping("/gp-outward-register")
    public void gpRegister(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        printReportData(response, "1520-InvRptGatePassOutwardRegister.rpt", result(service.gpRaw(q)));
    }

    /** CommonServices.GatePassOutwardSlipandRegisterForSteel(Id): the procedure with Org, Company and Id only. */
    @GetMapping("/gp-outward-slip")
    public void gpSlip(HttpServletResponse response, @RequestParam int id) throws Exception {
        Map<String, String> q = new LinkedHashMap<>();
        q.put("id", String.valueOf(id));
        printReportData(response, "1512-GatePassOutwardSlipAndRegisterSteel.rpt", result(slipRows(q)));
    }

    private List<Map<String, Object>> slipRows(Map<String, String> q) {
        // the slip call sends no dates: gpRaw adds the dates only when the query has them
        return service.gpRaw(q);
    }

    @GetMapping("/sale-order-slip-register")
    public void orderRegister(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        printReportData(response, "1509-SaleOrderSlipAndRegister.rpt", result(service.orderRaw(q)));
    }

    @GetMapping("/sale-order-slip")
    public void orderSlip(HttpServletResponse response, @RequestParam int id) throws Exception {
        printReportData(response, "1511-SaleOrderSlipAndRegister.rpt", result(service.orderSlip(id)));
    }
}
