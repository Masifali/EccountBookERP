package com.mst.controllers;

import com.mst.services.ExportFcyReceiptsService;
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
 * 794 "Fcy Receipts" - Architecture.WinApp.Account_Definition.Acfrmfcbankreceipt (module 100), DocumentTypeId 203.
 * Page /export/fcy-receipts, API /api/export/fcy-receipts/... No parameter carries tenancy or a user id; the
 * service derives them from the session.
 */
@Controller
public class ExportFcyReceiptsController {

    private static final String API = "/api/export/fcy-receipts";

    @Autowired private ExportFcyReceiptsService svc;

    @GetMapping("/export/fcy-receipts")
    public String page(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/fcy_receipts";
    }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return call(() -> svc.setup(), "Error occurred during database call."); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return call(() -> svc.refresh(), "Load failed."); }

    @GetMapping(API + "/document-no") @ResponseBody
    public ResponseEntity<?> documentNo() { return call(() -> svc.newDocumentNo(), "Load failed."); }

    @GetMapping(API + "/customers") @ResponseBody
    public ResponseEntity<?> customers(@RequestParam(value = "term", defaultValue = "") String term,
                                       @RequestParam(value = "termId", defaultValue = "0") int termId) {
        return call(() -> svc.customers(term, termId), "Load failed.");
    }

    @GetMapping(API + "/invoices") @ResponseBody
    public ResponseEntity<?> invoices(@RequestParam(value = "customerId", defaultValue = "0") int customerId) {
        return call(() -> svc.invoices(customerId), "Load failed.");
    }

    @GetMapping(API + "/lc-orders") @ResponseBody
    public ResponseEntity<?> lcOrders(@RequestParam(value = "customerId", defaultValue = "0") int customerId) {
        return call(() -> svc.lcOrders(customerId), "Load failed.");
    }

    @GetMapping(API + "/breakup-invoices") @ResponseBody
    public ResponseEntity<?> breakupInvoices() { return call(() -> svc.breakupInvoices(), "Load failed."); }

    @GetMapping(API + "/invoice-info") @ResponseBody
    public ResponseEntity<?> invoiceInfo(@RequestParam(value = "breakup", defaultValue = "false") boolean breakup,
                                         @RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId,
                                         @RequestParam(value = "previousBalance", defaultValue = "0") double previousBalance) {
        return call(() -> svc.invoiceInfo(breakup, invoiceId, previousBalance), "Load failed.");
    }

    @GetMapping(API + "/payment-terms") @ResponseBody
    public ResponseEntity<?> paymentTerms(@RequestParam("invoiceId") int invoiceId,
                                          @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return call(() -> svc.paymentTerms(invoiceId, recId), "Load failed.");
    }

    @GetMapping(API + "/contract-amount") @ResponseBody
    public ResponseEntity<?> contractAmount(@RequestParam(value = "contractId", defaultValue = "0") int contractId) {
        return call(() -> svc.contractAmount(contractId), "Load failed.");
    }

    @GetMapping(API + "/gds") @ResponseBody
    public ResponseEntity<?> gds(@RequestParam("invoiceId") int invoiceId,
                                 @RequestParam(value = "gdBreakUpId", defaultValue = "0") int gdBreakUpId,
                                 @RequestParam(value = "refDocumentTypeId", defaultValue = "0") int refDocumentTypeId) {
        return call(() -> svc.gds(invoiceId, gdBreakUpId, refDocumentTypeId), "Load failed.");
    }

    @GetMapping(API + "/voucher-head") @ResponseBody
    public ResponseEntity<?> voucherHead(@RequestParam("id") int id) { return call(() -> svc.voucherHead(id), "Load failed."); }

    @GetMapping(API + "/gain-loss") @ResponseBody
    public ResponseEntity<?> gainLoss(@RequestParam("id") int id) { return call(() -> svc.gainLossBreakup(id), "Load failed."); }

    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return call(() -> svc.readById(id), "Load failed."); }

    @GetMapping(API + "/history-combos") @ResponseBody
    public ResponseEntity<?> historyCombos() { return call(() -> svc.historyComboRefresh(), "Load failed."); }

    @GetMapping(API + "/print-check") @ResponseBody
    public ResponseEntity<?> printCheck(@RequestParam(value = "id", defaultValue = "0") int id) { return call(() -> svc.printCheck(id), "Print failed."); }

    @PostMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return call(() -> svc.history(body), "Load failed."); }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return call(() -> svc.save(body), "Save failed."); }

    // ------------------------------------------------------------------ plumbing

    private interface Call { Object run() throws Exception; }

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
