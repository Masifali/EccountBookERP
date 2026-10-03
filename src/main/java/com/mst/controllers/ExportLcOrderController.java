package com.mst.controllers;

import com.mst.services.ExportLcOrderService;
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

/** /export/lc-order - 210 LcOrder "Export Lc Order (Not Use)", DocumentTypeId 201. */
@Controller
public class ExportLcOrderController {

    private static final String API = "/api/export/lc-order";

    @Autowired private ExportLcOrderService svc;

    @GetMapping("/export/lc-order")
    public String page(Model model) { model.addAttribute("activeMenu", "export"); return "export/lc_order"; }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return call(svc::setup, "Error occurred during database call."); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return call(svc::refresh, "Load failed."); }

    @GetMapping(API + "/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam(value = "itemId", defaultValue = "0") int itemId) { return call(() -> svc.uoms(itemId), "Load failed."); }

    @GetMapping(API + "/proforma-invoices") @ResponseBody
    public ResponseEntity<?> proforma(@RequestParam(value = "customerId", defaultValue = "0") int customerId) {
        return call(() -> svc.proformaInvoices(customerId), "Load failed.");
    }

    @GetMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history() { return call(svc::history, "Load failed."); }

    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return call(() -> svc.readById(id), "Load failed."); }

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
