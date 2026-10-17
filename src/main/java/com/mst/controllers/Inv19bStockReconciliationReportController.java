package com.mst.controllers;

import com.mst.services.Inv19bStockReconciliationReportService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/** StockReportReconcilation - ScreenDefinition 295 "Stock Report Reconcilation" (Inventory hub, module 19 Stock Reports). */
@Controller
@RequestMapping("/inventory/reports/stock-reconciliation-report")
public class Inv19bStockReconciliationReportController {

    private final Inv19bStockReconciliationReportService service;
    public Inv19bStockReconciliationReportController(Inv19bStockReconciliationReportService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Stock Reconcilation Report");
        return "inventory/stock_reconciliation_report";
    }

    /** StockReportReconcilation_Load :109 */
    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() {
        try { return ResponseEntity.ok(service.init()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    /** btnRefresh_Click :443 - StockComboFill again. */
    @GetMapping("/api/combos") @ResponseBody
    public ResponseEntity<?> combos() {
        try { return ResponseEntity.ok(service.combos()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    /** GridBind :285 */
    @PostMapping("/api/show") @ResponseBody
    public ResponseEntity<?> show(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.show(body)); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
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
