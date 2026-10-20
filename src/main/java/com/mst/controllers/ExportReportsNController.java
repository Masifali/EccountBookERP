package com.mst.controllers;

import com.mst.services.ExportReportsNService;
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
 * Module 17 "Export Reports" - group N. One page and one API prefix per desktop report form:
 *
 *   /export/contract-schedule-periodic-a                 237  frmExportContractSchedulePeriodic
 *   /export/comparison-summary                           248  ExportComparisonSummaryReport
 *   /export/invoice-against-forwarding-pre-invoices      259  CommercialInvoiceAgainstForwardingPreInvoices
 *   /export/bill-of-lading-report                        264  frmBillofLadingSlipandRegister
 *   /export/container-list                               268  ExImContainerList
 *   /export/pre-invoice-register                         271  PreInvoiceRegister
 *   /export/bank-gd-summary                              272  BankGdSummary   (+ frmGDBreakUp popup: /api/export/bank-gd-summary/gd-break-up)
 *
 * The API of each page is /api/export/<same kebab>/...; no parameter carries tenancy or a user id.
 */
@Controller
public class ExportReportsNController {


    @Autowired private ExportReportsNService svc;

    // ------------------------------------------------------------------ pages

    @GetMapping("/export/contract-schedule-periodic-a")
    public String scheduleAPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/contract_schedule_periodic_a"; }

    @GetMapping("/export/comparison-summary")
    public String comparisonPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/comparison_summary"; }

    @GetMapping("/export/invoice-against-forwarding-pre-invoices")
    public String forwardingPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/invoice_against_forwarding_pre_invoices"; }

    @GetMapping("/export/bill-of-lading-report")
    public String bolPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/bill_of_lading_report"; }

    @GetMapping("/export/container-list")
    public String containerPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/container_list"; }

    @GetMapping("/export/pre-invoice-register")
    public String preInvoicePage(Model model) { model.addAttribute("activeMenu", "export"); return "export/pre_invoice_register"; }

    @GetMapping("/export/bank-gd-summary")
    public String bankGdPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/bank_gd_summary"; }

    // ------------------------------------------------------------------ 237

    @GetMapping("/api/export/contract-schedule-periodic-a/setup") @ResponseBody
    public ResponseEntity<?> scaSetup() { return call(() -> svc.scheduleASetup(), "Error occurred during database call."); }

    @PostMapping("/api/export/contract-schedule-periodic-a/show") @ResponseBody
    public ResponseEntity<?> scaShow(@RequestBody Map<String, Object> body) { return call(() -> svc.scheduleAShow(body), "Load failed."); }

    // ------------------------------------------------------------------ 248

    @GetMapping("/api/export/comparison-summary/setup") @ResponseBody
    public ResponseEntity<?> cmpSetup() { return call(() -> svc.comparisonSetup(), "Error occurred during database call."); }

    @GetMapping("/api/export/comparison-summary/combos") @ResponseBody
    public ResponseEntity<?> cmpCombos() { return call(() -> svc.comparisonRefresh(), "Load failed."); }

    @PostMapping("/api/export/comparison-summary/show") @ResponseBody
    public ResponseEntity<?> cmpShow(@RequestBody Map<String, Object> body) { return call(() -> svc.comparisonShow(body), "Load failed."); }

    // ------------------------------------------------------------------ 259

    @GetMapping("/api/export/invoice-against-forwarding-pre-invoices/setup") @ResponseBody
    public ResponseEntity<?> fwdSetup() { return call(() -> svc.forwardingSetup(), "Error occurred during database call."); }

    @PostMapping("/api/export/invoice-against-forwarding-pre-invoices/show") @ResponseBody
    public ResponseEntity<?> fwdShow(@RequestBody Map<String, Object> body) { return call(() -> svc.forwardingShow(body), "Load failed."); }

    // ------------------------------------------------------------------ 264

    @GetMapping("/api/export/bill-of-lading-report/setup") @ResponseBody
    public ResponseEntity<?> bolSetup() { return call(() -> svc.bolSetup(), "Error occurred during database call."); }

    @GetMapping("/api/export/bill-of-lading-report/combos") @ResponseBody
    public ResponseEntity<?> bolCombos() { return call(() -> svc.bolRefresh(), "Load failed."); }

    @PostMapping("/api/export/bill-of-lading-report/show") @ResponseBody
    public ResponseEntity<?> bolShow(@RequestBody Map<String, Object> body) { return call(() -> svc.bolShow(body), "Load failed."); }

    // ------------------------------------------------------------------ 268

    @GetMapping("/api/export/container-list/setup") @ResponseBody
    public ResponseEntity<?> cntSetup() { return call(() -> svc.containerSetup(), "Error occurred during database call."); }

    @PostMapping("/api/export/container-list/show") @ResponseBody
    public ResponseEntity<?> cntShow(@RequestBody Map<String, Object> body) { return call(() -> svc.containerShow(body), "Load failed."); }

    // ------------------------------------------------------------------ 271

    @GetMapping("/api/export/pre-invoice-register/setup") @ResponseBody
    public ResponseEntity<?> pirSetup() { return call(() -> svc.preInvoiceSetup(), "Error occurred during database call."); }

    @PostMapping("/api/export/pre-invoice-register/show") @ResponseBody
    public ResponseEntity<?> pirShow(@RequestBody Map<String, Object> body) { return call(() -> svc.preInvoiceShow(body), "Load failed."); }

    @GetMapping("/api/export/pre-invoice-register/contract-check") @ResponseBody
    public ResponseEntity<?> pirContractCheck(@RequestParam(value = "contractId", defaultValue = "0") int contractId) {
        return call(() -> svc.preInvoiceContractCheck(contractId), "Load failed.");
    }

    // ------------------------------------------------------------------ 272

    @GetMapping("/api/export/bank-gd-summary/setup") @ResponseBody
    public ResponseEntity<?> bgdSetup() { return call(() -> svc.bankGdSetup(), "Error occurred during database call."); }

    @PostMapping("/api/export/bank-gd-summary/show") @ResponseBody
    public ResponseEntity<?> bgdShow(@RequestBody Map<String, Object> body) { return call(() -> svc.bankGdShow(body), "Load failed."); }

    @GetMapping("/api/export/bank-gd-summary/gd-break-up") @ResponseBody
    public ResponseEntity<?> bgdBreakUp(@RequestParam(value = "gdId", defaultValue = "0") int gdId,
                                        @RequestParam(value = "documentTypeId", defaultValue = "0") int documentTypeId) {
        return call(() -> svc.gdBreakUp(gdId, documentTypeId), "Load failed.");
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
