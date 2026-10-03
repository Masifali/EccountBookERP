package com.mst.controllers;

import com.mst.services.ExportPerformaInvoiceService;
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
 * /export/performa-invoice - screen 200 "Export Performa Invoice" (Architecture.WinApp.Export.ExImProformaInvoice,
 * App 8 / Module 11, DocumentTypeId 200). Tenancy and user come from the session only.
 */
@Controller
public class ExportPerformaInvoiceController {

    private static final String API = "/api/export/performa-invoice";

    @Autowired private ExportPerformaInvoiceService svc;

    @GetMapping("/export/performa-invoice")
    public String page(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/performa_invoice";
    }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return call(() -> svc.setup(), "Error occurred during database call."); }

    @GetMapping(API + "/new-code") @ResponseBody
    public ResponseEntity<?> newCode() { return call(() -> svc.newCode(), "Load failed."); }

    @GetMapping(API + "/items") @ResponseBody
    public ResponseEntity<?> items() { return call(() -> svc.items(), "Load failed."); }

    @GetMapping(API + "/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam(value = "itemId", defaultValue = "0") int itemId) { return call(() -> svc.uoms(itemId), "Load failed."); }

    @GetMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history() { return call(() -> svc.history(), "Load failed."); }

    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return call(() -> svc.readById(id), "Load failed."); }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return call(() -> svc.save(body), "Save failed."); }

    // ------------------------------------------------------------------ plumbing (same contract as 881 / 882)

    interface Call { Object run() throws Exception; }

    static ResponseEntity<?> call(Call c, String fallback) {
        try { return ResponseEntity.ok(c.run()); }
        catch (IllegalArgumentException | IllegalStateException e) { return ResponseEntity.badRequest().body(fail(root(e, fallback))); }
        catch (AccessDeniedException e) { return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(root(e, "Access denied."))); }
        catch (Exception e) { return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback))); }
    }

    static String root(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
