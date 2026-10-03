package com.mst.controllers;

import com.mst.services.ExportAdvanceUtilizeInvoiceService;
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

/** 213 "Export Bill Of Lading" - EximBillOfLading. Page /export/bill-of-lading, API /api/export/advance-utilize-against-invoice/... */
@Controller
public class ExportAdvanceUtilizeInvoiceController {

    private static final String API = "/api/export/advance-utilize-against-invoice";

    @Autowired private ExportAdvanceUtilizeInvoiceService svc;

    @GetMapping("/export/advance-utilize-against-invoice")
    public String page(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/advance_utilize_against_invoice";
    }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return call(() -> svc.setup(), "Error occurred during database call."); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh(@RequestParam(value = "recId", defaultValue = "0") int recId) { return call(() -> svc.refresh(recId), "Load failed."); }

    @GetMapping(API + "/history-combos") @ResponseBody
    public ResponseEntity<?> historyCombos() { return call(() -> svc.historyCombos(), "Load failed."); }

    @GetMapping(API + "/invoice-leave") @ResponseBody
    public ResponseEntity<?> invoiceLeave(@RequestParam("invoiceId") int invoiceId,
                                          @RequestParam(value = "supplierCustomerId", defaultValue = "0") int supplierCustomerId) {
        return call(() -> svc.invoiceLeave(invoiceId, supplierCustomerId), "Load failed.");
    }

    @GetMapping(API + "/fi-balance") @ResponseBody
    public ResponseEntity<?> fiBalance(@RequestParam("documentTypeId") int documentTypeId, @RequestParam("id") int id) {
        return call(() -> svc.fiBalance(documentTypeId, id), "Load failed.");
    }

    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return call(() -> svc.readById(id), "Load failed."); }

    @PostMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return call(() -> svc.history(body), "Load failed."); }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return call(() -> svc.save(body), "Save failed."); }

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
