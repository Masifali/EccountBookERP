package com.mst.controllers.ERPPrint;

import com.mst.repositories.ICompanyRepository;
import com.mst.services.SaRpt1Service;
import java.io.File;
import java.util.*;
import java.util.regex.Pattern;
import javax.servlet.http.HttpServletResponse;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

/**
 * Group R1 prints. The desktop hands the LAST SHOW's DataTable to the Crystal template
 * (Reporting.ShowReportWithDataTable(dt, "...rpt")) with CompanyName / CompanyAddress. The web re-runs the SAME procedure with the
 * SAME arguments the last Show used (the page passes them back) and renders the same template through the shared Jasper pipeline
 * (ReportPrintSupport.printReportData), so the template's own columns are fed exactly like the desktop.
 *   832  kind=1855 -> 1855-GDNRegister.rpt, kind=1855A -> 1855A-GDNRegister.rpt            (the detail / main grid rows of the last Show)
 *   840  template=1861_NN-....rpt chosen by PrintButtonManage from the report type + Add City / Add Varient
 *   490  tab 1 / 2 -> 146-OutstandingOrderWithLedgerBalance.rpt, tab 3 -> 199-GetOutstandingOrdersWithLedgerBalance.rpt
 *   485  template = a file of the "Sales" folder (tsDropDown_DropDownItemClicked)
 *   487  kind=302 -> 302-InvSaleInvoice.rpt, template = a file of the "SaleDirectRegister" folder
 *   491  616-GetItemSalePriceListRegister.rpt over the discount table
 *   486  0228-InvPurchaseInvoice_SaleReturnRegister.rpt        488  56_08_PurchaseAndSaleDetailByJobLot.rpt
 *   489  263-InvDeliveryOrderForApproval.rpt
 */
@Controller
@RequestMapping("/sale/reports/sarpt1/print")
public class SaRpt1PrintController extends ReportPrintSupport {
    private static final Pattern TEMPLATE_1861 = Pattern.compile("^1861_[0-9]{2}-[A-Za-z_&]+$");
    private static final Pattern DYNAMIC_NAME = Pattern.compile("^[^\\\\/:*?\"<>|]+$");

    private final SaRpt1Service service;
    private final ICompanyRepository companies;

    @Value("${reports.crystal.template-root:}")
    private String reportRoot;

    public SaRpt1PrintController(SaRpt1Service service, ICompanyRepository companies) {
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

    private static Object ci(Map<String, Object> row, String key) {
        if (row.containsKey(key)) return row.get(key);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
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

    @GetMapping("/gdn-register")
    public void gdnRegister(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        String kind = q.getOrDefault("kind", "1855");
        if (!"1855".equals(kind) && !"1855A".equals(kind)) throw new IllegalArgumentException("Unknown register " + kind);
        List<Map<String, Object>> rows = service.rows("gdn-register", q);
        printReportData(response, "1855A".equals(kind) ? "1855A-GDNRegister.rpt" : "1855-GDNRegister.rpt", result(rows));
    }

    @GetMapping("/sale-invoice-register")
    public void saleInvoiceRegister(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        String template = q.get("template");
        if (template == null || !TEMPLATE_1861.matcher(template).matches()) throw new IllegalArgumentException("Invalid report name.");
        printReportData(response, template + ".rpt", result(service.rows("sale-invoice-register", q)));
    }

    /** Print-146 (tabs 1 and 2: the grid's table with its own column names) / Print-199 (tab 3: the procedure's rows). */
    @GetMapping("/orders-with-ledger-balance")
    public void ordersWithLedgerBalance(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        int tab = q.get("tab") == null ? 1 : Integer.parseInt(q.get("tab").trim());
        List<Map<String, Object>> src = service.rows("orders-with-ledger-balance", q);
        if (tab == 3) {
            printReportData(response, "199-GetOutstandingOrdersWithLedgerBalance.rpt", result(src));
            return;
        }
        List<Map<String, Object>> rows = new ArrayList<>(src.size());
        for (Map<String, Object> r : src) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("DocDate", ci(r, "DocDate"));
            m.put("DocNo", ci(r, "DocNo"));
            m.put("SuppCustId", ci(r, "OrderSupCustId"));
            m.put("CustomerName", ci(r, "CustomerName"));
            m.put("OrderQty", ci(r, "OrderQty"));
            m.put("DispatchQty", ci(r, "DispatchQty"));
            m.put("BalQty", ci(r, "BalQty"));
            m.put("OrderWeight", ci(r, "OrderWeight"));
            m.put("DispatchWeight", ci(r, "DispatchWeight"));
            m.put("BalWeight", ci(r, "BalWeight"));
            m.put("OrderAmount", ci(r, "OrderAmount"));
            m.put("DispatchAmount", ci(r, "DispatchAmount"));
            m.put("BalAmount", ci(r, "OrderBalAmount"));
            m.put("LedgerBalance", ci(r, "LedgerBalance"));
            m.put("RunningBalance", ci(r, "RunningBalance"));
            m.put("Remarks", ci(r, "RemarksHeader"));
            m.put("CustomerGlId", ci(r, "CustomerGlId"));
            rows.add(m);
        }
        printReportData(response, "146-OutstandingOrderWithLedgerBalance.rpt", result(rows));
    }

    @GetMapping("/sale-invoice-history")
    public void saleInvoiceHistory(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        String template = dynamicTemplate("Sales", q.get("template"));
        printReportData(response, template, result(service.rows("sale-invoice-history", q)));
    }

    @GetMapping("/sale-invoice-direct-register")
    public void saleInvoiceDirectRegister(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        String template = "302".equals(q.get("kind")) ? "302-InvSaleInvoice.rpt" : dynamicTemplate("SaleDirectRegister", q.get("template"));
        printReportData(response, template, result(service.rows("sale-invoice-direct-register", q)));
    }

    @GetMapping("/sale-price-list-with-discount")
    public void salePriceListWithDiscount(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        printReportData(response, "616-GetItemSalePriceListRegister.rpt", result(service.rows("sale-price-list-with-discount", q)));
    }

    @GetMapping("/sale-invoice-return-register")
    public void saleInvoiceReturnRegister(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        printReportData(response, "0228-InvPurchaseInvoice_SaleReturnRegister.rpt", result(service.rows("sale-invoice-return-register", q)));
    }

    @GetMapping("/purchase-and-sale-detail-by-joblot")
    public void purchaseAndSaleByJobLot(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        printReportData(response, "56_08_PurchaseAndSaleDetailByJobLot.rpt", result(service.rows("purchase-and-sale-detail-by-joblot", q)));
    }

    @GetMapping("/delivery-order-history")
    public void deliveryOrderHistory(HttpServletResponse response, @RequestParam Map<String, String> q) throws Exception {
        printReportData(response, "263-InvDeliveryOrderForApproval.rpt", result(service.rows("delivery-order-history", q)));
    }
}
