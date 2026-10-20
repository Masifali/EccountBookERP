package com.mst.controllers;

import com.mst.services.ExportReportsBService;
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
 * Module 17 "Export Reports" - group B. One page and one API prefix per desktop report form:
 *
 *   /export/shipment-tracking-follow-up   254  ExportPendingShipmentsFollowupReport        api /api/export/shipment-tracking-follow-up
 *   /export/shipment-weight-audit         261  frmAuditByWeightReport                      api /api/export/shipment-weight-audit
 *   /export/loading-sheet                 262  ExportLoadingSheet                          api /api/export/loading-sheet
 *   /export/shipment-cro-booking-report   266  frmExportShipingLineBookingRpt              api /api/export/shipment-cro-booking-report
 *   /export/forwarding-report             269  ExImForwardingHistory                       api /api/export/forwarding-report
 *   /export/sale-report                   270  ExportSalesReport                           api /api/export/sale-report
 *   /export/sale-comparisons-report       759  ExportCustomerWiseComparisonsSummeryReport  api /api/export/sale-comparisons-report
 *   /export/service-bill-register         912  frmServiceBillHistory                       api /api/export/service-bill-register
 *   /export/forwarding-costing-report     913  ExImForwardingCostingReport                 api /api/export/forwarding-costing-report
 *   /export/shipment-costing-summary      919  frmExImShipmentCostingSummary               api /api/export/shipment-costing-summary
 *   /export/fi-utilization-report         935  frmFinancialInstrumentAdvanceBalanceSummary api /api/export/fi-utilization-report
 *
 * Every "show" is a POST with the page's filter values (ids, ISO dates, radio codes); no parameter carries
 * tenancy or a user id, the service derives them from the session. Prints go through
 * /api/reports/{key}/print.pdf (ReportRegistry) from the page.
 */
@Controller
public class ExportReportsBController {


    @Autowired private ExportReportsBService svc;

    // ------------------------------------------------------------------ pages

    @GetMapping("/export/shipment-tracking-follow-up") public String page254(Model m) { m.addAttribute("activeMenu", "export"); return "export/shipment_tracking_follow_up"; }
    @GetMapping("/export/shipment-weight-audit")       public String page261(Model m) { m.addAttribute("activeMenu", "export"); return "export/shipment_weight_audit"; }
    @GetMapping("/export/loading-sheet")               public String page262(Model m) { m.addAttribute("activeMenu", "export"); return "export/loading_sheet"; }
    @GetMapping("/export/shipment-cro-booking-report") public String page266(Model m) { m.addAttribute("activeMenu", "export"); return "export/shipment_cro_booking_report"; }
    @GetMapping("/export/forwarding-report")           public String page269(Model m) { m.addAttribute("activeMenu", "export"); return "export/forwarding_report"; }
    @GetMapping("/export/sale-report")                 public String page270(Model m) { m.addAttribute("activeMenu", "export"); return "export/sale_report"; }
    @GetMapping("/export/sale-comparisons-report")     public String page759(Model m) { m.addAttribute("activeMenu", "export"); return "export/sale_comparisons_report"; }
    @GetMapping("/export/service-bill-register")       public String page912(Model m) { m.addAttribute("activeMenu", "export"); return "export/service_bill_register"; }
    @GetMapping("/export/forwarding-costing-report")   public String page913(Model m) { m.addAttribute("activeMenu", "export"); return "export/forwarding_costing_report"; }
    @GetMapping("/export/shipment-costing-summary")    public String page919(Model m) { m.addAttribute("activeMenu", "export"); return "export/shipment_costing_summary"; }
    @GetMapping("/export/fi-utilization-report")       public String page935(Model m) { m.addAttribute("activeMenu", "export"); return "export/fi_utilization_report"; }

    // ------------------------------------------------------------------ 254

    @GetMapping("/api/export/shipment-tracking-follow-up/setup") @ResponseBody public ResponseEntity<?> trkSetup() { return call(() -> svc.setup254(), "Error occurred during database call."); }
    @GetMapping("/api/export/shipment-tracking-follow-up/refresh") @ResponseBody public ResponseEntity<?> trkRefresh() { return call(() -> svc.refresh254(), "Load failed."); }
    @PostMapping("/api/export/shipment-tracking-follow-up/show") @ResponseBody public ResponseEntity<?> trkShow(@RequestBody Map<String, Object> f) { return call(() -> svc.show254(f), "Load failed."); }
    @GetMapping("/api/export/shipment-tracking-follow-up/cro-exists") @ResponseBody
    public ResponseEntity<?> trkCro(@RequestParam("invoiceId") int invoiceId) { return call(() -> svc.croBookingExists(invoiceId, ExportReportsBService.SCREEN_254), "Load failed."); }

    // ------------------------------------------------------------------ 261

    @GetMapping("/api/export/shipment-weight-audit/setup") @ResponseBody public ResponseEntity<?> wgtSetup() { return call(() -> svc.setup261(), "Error occurred during database call."); }
    @PostMapping("/api/export/shipment-weight-audit/show") @ResponseBody public ResponseEntity<?> wgtShow(@RequestBody Map<String, Object> f) { return call(() -> svc.show261(f), "Load failed."); }
    @GetMapping("/api/export/shipment-weight-audit/invoice-exists") @ResponseBody
    public ResponseEntity<?> wgtInvoice(@RequestParam("invoiceId") int invoiceId) { return call(() -> svc.auditByInvoiceExists(invoiceId), "Load failed."); }

    // ------------------------------------------------------------------ 262

    @GetMapping("/api/export/loading-sheet/setup") @ResponseBody public ResponseEntity<?> ldsSetup() { return call(() -> svc.setup262(), "Error occurred during database call."); }
    @PostMapping("/api/export/loading-sheet/show") @ResponseBody public ResponseEntity<?> ldsShow(@RequestBody Map<String, Object> f) { return call(() -> svc.show262(f), "Load failed."); }

    // ------------------------------------------------------------------ 266

    @GetMapping("/api/export/shipment-cro-booking-report/setup") @ResponseBody public ResponseEntity<?> croSetup() { return call(() -> svc.setup266(), "Error occurred during database call."); }
    @PostMapping("/api/export/shipment-cro-booking-report/show") @ResponseBody public ResponseEntity<?> croShow(@RequestBody Map<String, Object> f) { return call(() -> svc.show266(f), "Load failed."); }
    @GetMapping("/api/export/shipment-cro-booking-report/cro-exists") @ResponseBody
    public ResponseEntity<?> croExists(@RequestParam("id") int id) { return call(() -> svc.croBookingByIdExists(id), "Load failed."); }

    // ------------------------------------------------------------------ 269 / 913

    @GetMapping("/api/export/forwarding-report/setup") @ResponseBody public ResponseEntity<?> fwdSetup() { return call(() -> svc.setupForwarding(ExportReportsBService.SCREEN_269), "Error occurred during database call."); }
    @GetMapping("/api/export/forwarding-report/refresh") @ResponseBody public ResponseEntity<?> fwdRefresh() { return call(() -> svc.refreshForwarding(ExportReportsBService.SCREEN_269), "Load failed."); }
    @GetMapping("/api/export/forwarding-report/invoices") @ResponseBody
    public ResponseEntity<?> fwdInvoices(@RequestParam(value = "supplierCustomerId", defaultValue = "0") int supplierCustomerId) {
        return call(() -> svc.forwardingInvoices(ExportReportsBService.SCREEN_269, supplierCustomerId), "Load failed.");
    }
    @PostMapping("/api/export/forwarding-report/show") @ResponseBody public ResponseEntity<?> fwdShow(@RequestBody Map<String, Object> f) { return call(() -> svc.showForwarding(ExportReportsBService.SCREEN_269, f), "Load failed."); }

    @GetMapping("/api/export/forwarding-costing-report/setup") @ResponseBody public ResponseEntity<?> fwcSetup() { return call(() -> svc.setupForwarding(ExportReportsBService.SCREEN_913), "Error occurred during database call."); }
    @GetMapping("/api/export/forwarding-costing-report/refresh") @ResponseBody public ResponseEntity<?> fwcRefresh() { return call(() -> svc.refreshForwarding(ExportReportsBService.SCREEN_913), "Load failed."); }
    @GetMapping("/api/export/forwarding-costing-report/invoices") @ResponseBody
    public ResponseEntity<?> fwcInvoices(@RequestParam(value = "supplierCustomerId", defaultValue = "0") int supplierCustomerId) {
        return call(() -> svc.forwardingInvoices(ExportReportsBService.SCREEN_913, supplierCustomerId), "Load failed.");
    }
    @PostMapping("/api/export/forwarding-costing-report/show") @ResponseBody public ResponseEntity<?> fwcShow(@RequestBody Map<String, Object> f) { return call(() -> svc.showForwarding(ExportReportsBService.SCREEN_913, f), "Load failed."); }

    // ------------------------------------------------------------------ 270

    @GetMapping("/api/export/sale-report/setup") @ResponseBody public ResponseEntity<?> salSetup() { return call(() -> svc.setup270(), "Error occurred during database call."); }
    @PostMapping("/api/export/sale-report/show") @ResponseBody public ResponseEntity<?> salShow(@RequestBody Map<String, Object> f) { return call(() -> svc.show270(f), "Load failed."); }

    // ------------------------------------------------------------------ 759

    @GetMapping("/api/export/sale-comparisons-report/setup") @ResponseBody public ResponseEntity<?> cmpSetup() { return call(() -> svc.setup759(), "Error occurred during database call."); }
    @PostMapping("/api/export/sale-comparisons-report/show") @ResponseBody public ResponseEntity<?> cmpShow(@RequestBody Map<String, Object> f) { return call(() -> svc.show759(f), "Load failed."); }
    @GetMapping("/api/export/sale-comparisons-report/cro-exists") @ResponseBody
    public ResponseEntity<?> cmpCro(@RequestParam("invoiceId") int invoiceId) { return call(() -> svc.croBookingExists(invoiceId, ExportReportsBService.SCREEN_759), "Load failed."); }

    // ------------------------------------------------------------------ 912

    @GetMapping("/api/export/service-bill-register/setup") @ResponseBody public ResponseEntity<?> svcSetup() { return call(() -> svc.setup912(), "Error occurred during database call."); }
    @PostMapping("/api/export/service-bill-register/show") @ResponseBody public ResponseEntity<?> svcShow(@RequestBody Map<String, Object> f) { return call(() -> svc.show912(f), "Load failed."); }

    // ------------------------------------------------------------------ 919

    @GetMapping("/api/export/shipment-costing-summary/setup") @ResponseBody public ResponseEntity<?> cssSetup() { return call(() -> svc.setup919(), "Error occurred during database call."); }
    @PostMapping("/api/export/shipment-costing-summary/show") @ResponseBody public ResponseEntity<?> cssShow(@RequestBody Map<String, Object> f) { return call(() -> svc.show919(f), "Load failed."); }

    // ------------------------------------------------------------------ 935

    @GetMapping("/api/export/fi-utilization-report/setup") @ResponseBody public ResponseEntity<?> fiuSetup() { return call(() -> svc.setup935(), "Error occurred during database call."); }
    @PostMapping("/api/export/fi-utilization-report/show") @ResponseBody public ResponseEntity<?> fiuShow(@RequestBody Map<String, Object> f) { return call(() -> svc.show935(f), "Load failed."); }
    @GetMapping("/api/export/fi-utilization-report/utilize-detail") @ResponseBody
    public ResponseEntity<?> fiuDetail(@RequestParam("fiId") int fiId, @RequestParam(value = "documentTypeId", defaultValue = "0") int documentTypeId) {
        return call(() -> svc.fiUtilizeDetail(fiId, documentTypeId), "Load failed.");
    }

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
