package com.mst.controllers;

import com.mst.services.ExportReportsOService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Export Reports - group O. One page and one API prefix per desktop report form (none has a ScreenDefinition row):
 *
 *   /export/commercial-invoice-history    ExImCommercialInvoiceHistory     api /api/export/commercial-invoice-history
 *   /export/shipment-schedule-history     ExImShipmentScheduleHistory      api /api/export/shipment-schedule-history
 *   /export/report-summaries              ExportReportSummaries            api /api/export/report-summaries
 *   /export/performa-invoice-register     PerformaInvoiceRegister          api /api/export/performa-invoice-register
 *   /export/invoice-summary-by-month      frmExportInvoiceSummaryByMonth   api /api/export/invoice-summary-by-month
 *   /export/stock-reserved-register       frmStockReservedRegister         api /api/export/stock-reserved-register
 *   /export/contract-reports              ExportContractReports            api /api/export/contract-reports
 *
 * No parameter carries tenancy or a user id; the service derives them from the session. Prints go through
 * CrystalPrint -> /api/reports/{key}/print.pdf with the keys registered by ExportReportsOReportSupport.
 */
@Controller
public class ExportReportsOController {

    private static final String CIH = "/api/export/commercial-invoice-history";
    private static final String SSH = "/api/export/shipment-schedule-history";
    private static final String SUM = "/api/export/report-summaries";
    private static final String PIR = "/api/export/performa-invoice-register";
    private static final String ISM = "/api/export/invoice-summary-by-month";
    private static final String SRR = "/api/export/stock-reserved-register";
    private static final String ECR = "/api/export/contract-reports";

    @Autowired private ExportReportsOService svc;

    // ------------------------------------------------------------------ pages

    @GetMapping("/export/commercial-invoice-history")
    public String invoiceHistoryPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/commercial_invoice_history"; }

    @GetMapping("/export/shipment-schedule-history")
    public String shipmentPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/shipment_schedule_history"; }

    @GetMapping("/export/report-summaries")
    public String summariesPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/report_summaries"; }

    @GetMapping("/export/performa-invoice-register")
    public String performaPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/performa_invoice_register"; }

    @GetMapping("/export/invoice-summary-by-month")
    public String monthPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/invoice_summary_by_month"; }

    @GetMapping("/export/stock-reserved-register")
    public String stockPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/stock_reserved_register"; }

    @GetMapping("/export/contract-reports")
    public String contractReportsPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/contract_reports"; }

    // ------------------------------------------------------------------ ExImCommercialInvoiceHistory

    @GetMapping(CIH + "/setup") @ResponseBody
    public ResponseEntity<?> cihSetup() { return call(() -> svc.invoiceHistorySetup(), "Load failed."); }

    @GetMapping(CIH + "/combos") @ResponseBody
    public ResponseEntity<?> cihCombos(@RequestParam(value = "reportTypes", defaultValue = "false") boolean reportTypes) {
        return call(() -> svc.invoiceHistoryCombos(reportTypes), "Load failed.");
    }

    @PostMapping(CIH + "/invoices") @ResponseBody
    public ResponseEntity<?> cihInvoices(@RequestBody Map<String, Object> body) { return call(() -> svc.invoiceHistoryInvoices(body), "Load failed."); }

    @PostMapping(CIH + "/export-history") @ResponseBody
    public ResponseEntity<?> cihExport(@RequestBody Map<String, Object> body) { return call(() -> svc.invoiceHistoryExport(body), "Load failed."); }

    // ------------------------------------------------------------------ ExImShipmentScheduleHistory

    @GetMapping(SSH + "/setup") @ResponseBody
    public ResponseEntity<?> sshSetup() { return call(() -> svc.shipmentSetup(), "Load failed."); }

    @GetMapping(SSH + "/combos") @ResponseBody
    public ResponseEntity<?> sshCombos() { return call(() -> svc.shipmentCombos(), "Load failed."); }

    @PostMapping(SSH + "/show") @ResponseBody
    public ResponseEntity<?> sshShow(@RequestBody Map<String, Object> body) { return call(() -> svc.shipmentShow(body), "Load failed."); }

    // ------------------------------------------------------------------ ExportReportSummaries

    @GetMapping(SUM + "/setup") @ResponseBody
    public ResponseEntity<?> sumSetup() { return call(() -> svc.summariesSetup(), "Load failed."); }

    @PostMapping(SUM + "/show") @ResponseBody
    public ResponseEntity<?> sumShow(@RequestBody Map<String, Object> body) { return call(() -> svc.summariesShow(body), "Load failed."); }

    // ------------------------------------------------------------------ PerformaInvoiceRegister

    @GetMapping(PIR + "/setup") @ResponseBody
    public ResponseEntity<?> pirSetup() { return call(() -> svc.performaSetup(), "Load failed."); }

    @GetMapping(PIR + "/combos") @ResponseBody
    public ResponseEntity<?> pirCombos() { return call(() -> svc.performaCombos(), "Load failed."); }

    @PostMapping(PIR + "/show") @ResponseBody
    public ResponseEntity<?> pirShow(@RequestBody Map<String, Object> body) { return call(() -> svc.performaShow(body), "Load failed."); }

    @GetMapping(PIR + "/attachments") @ResponseBody
    public ResponseEntity<?> pirAttachments(@RequestParam("id") int id) { return call(() -> svc.performaAttachments(id), "Load failed."); }

    // ------------------------------------------------------------------ frmExportInvoiceSummaryByMonth

    @GetMapping(ISM + "/setup") @ResponseBody
    public ResponseEntity<?> ismSetup() { return call(() -> svc.monthSetup(), "Load failed."); }

    @PostMapping(ISM + "/show") @ResponseBody
    public ResponseEntity<?> ismShow(@RequestBody Map<String, Object> body) { return call(() -> svc.monthShow(body), "Load failed."); }

    // ------------------------------------------------------------------ frmStockReservedRegister

    @GetMapping(SRR + "/setup") @ResponseBody
    public ResponseEntity<?> srrSetup() { return call(() -> svc.stockSetup(), "Error occurred during database call."); }

    @GetMapping(SRR + "/combos") @ResponseBody
    public ResponseEntity<?> srrCombos() { return call(() -> svc.stockCombos(), "Load failed."); }

    @PostMapping(SRR + "/show") @ResponseBody
    public ResponseEntity<?> srrShow(@RequestBody Map<String, Object> body) { return call(() -> svc.stockShow(body), "Load failed."); }

    // ------------------------------------------------------------------ ExportContractReports

    @GetMapping(ECR + "/tabs") @ResponseBody
    public ResponseEntity<?> ecrTabs() { return call(() -> svc.contractReportTabs(), "Load failed."); }

    // ------------------------------------------------------------------ plumbing

    private interface Call { Object run() throws Exception; }

    /** 400 for the form's own validation text, 403 for a missing right, 500 with the innermost message otherwise. */
    private static ResponseEntity<?> call(Call c, String fallback) {
        try { return ResponseEntity.ok(c.run()); }
        catch (IllegalArgumentException | IllegalStateException e) { return ResponseEntity.badRequest().body(fail(root(e, fallback))); }
        catch (AccessDeniedException e) { return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(root(e, "Access denied."))); }
        catch (Exception e) { return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback))); }
    }

    private static String root(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
