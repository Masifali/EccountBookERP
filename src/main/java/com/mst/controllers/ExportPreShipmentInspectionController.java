package com.mst.controllers;

import com.mst.services.ExportPreShipmentInspectionService;
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

/** 213 "Export Bill Of Lading" - EximBillOfLading. Page /export/bill-of-lading, API /api/export/pre-shipment-inspection/... */
@Controller
public class ExportPreShipmentInspectionController {

    private static final String API = "/api/export/pre-shipment-inspection";

    @Autowired private ExportPreShipmentInspectionService svc;

    @GetMapping("/export/pre-shipment-inspection")
    public String page(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/pre_shipment_inspection";
    }

    @GetMapping("/api/export/pre-shipment-inspection/setup") @ResponseBody
    public ResponseEntity<?> setup() { return call(() -> svc.setup(), "Error occurred during database call."); }

    @GetMapping("/api/export/pre-shipment-inspection/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return call(() -> svc.refresh(), "Load failed."); }

    @GetMapping("/api/export/pre-shipment-inspection/doc-no") @ResponseBody
    public ResponseEntity<?> docNo() { return call(() -> svc.docNo(), "Load failed."); }

    @GetMapping("/api/export/pre-shipment-inspection/history-items") @ResponseBody
    public ResponseEntity<?> historyItems() { return call(() -> svc.historyItems(), "Load failed."); }

    @GetMapping("/api/export/pre-shipment-inspection/contracts") @ResponseBody
    public ResponseEntity<?> contracts(@RequestParam("customerId") int customerId) { return call(() -> svc.contracts(customerId), "Load failed."); }

    @GetMapping("/api/export/pre-shipment-inspection/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return call(() -> svc.readById(id), "Load failed."); }

    @GetMapping("/api/export/pre-shipment-inspection/details") @ResponseBody
    public ResponseEntity<?> details(@RequestParam("id") int id) { return call(() -> svc.detailsOf(id), "Load failed."); }

    @PostMapping("/api/export/pre-shipment-inspection/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return call(() -> svc.history(body), "Load failed."); }

    @PostMapping("/api/export/pre-shipment-inspection/save") @ResponseBody
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
