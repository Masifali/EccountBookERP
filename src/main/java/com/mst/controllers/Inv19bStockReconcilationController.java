package com.mst.controllers;

import com.mst.services.Inv19bStockReconcilationService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/** StockReconcilation - ScreenDefinition 303 "Stock Reconcilation (Not Use)" (Inventory hub, module 19 Stock Reports). */
@Controller
@RequestMapping("/inventory/reports/stock-reconcilation")
public class Inv19bStockReconcilationController {

    private final Inv19bStockReconcilationService service;
    public Inv19bStockReconcilationController(Inv19bStockReconcilationService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Stock Reconcilation");
        return "inventory/stock_reconcilation";
    }

    /** frmGatePassReport_Load :114 */
    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() {
        try { return ResponseEntity.ok(service.init()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    /** GridFill :181 */
    @PostMapping("/api/show") @ResponseBody
    public ResponseEntity<?> show(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.show(body)); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    /** toolStripButton1_Click :287 "Reconcilation" */
    @PostMapping("/api/reconcile") @ResponseBody
    public ResponseEntity<?> reconcile(@RequestBody Map<String, Object> body) {
        try {
            service.reconcile(body);
            Map<String, Object> m = new LinkedHashMap<>(); m.put("success", true); m.put("message", "Record Update");
            return ResponseEntity.ok(m);
        } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    private static ResponseEntity<Map<String, Object>> fail(HttpStatus status, String message) {
        Map<String, Object> m = new LinkedHashMap<>(); m.put("success", false); m.put("message", message);
        return ResponseEntity.status(status).body(m);
    }
    private static String msg(Exception e) {
        Throwable t = e; while (t.getCause() != null && (t.getMessage() == null || t.getMessage().isBlank())) t = t.getCause();
        return t.getMessage() == null || t.getMessage().isBlank() ? "Request failed." : t.getMessage();
    }
}
