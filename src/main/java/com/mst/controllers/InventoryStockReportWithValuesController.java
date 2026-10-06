package com.mst.controllers;

import com.mst.services.InventoryStockReportWithValuesService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/** frmStockReportWithValues - ScreenDefinition 299 "Stock Report With Values" (Inventory menu). */
@Controller
@RequestMapping("/inventory/stock-report-with-values")
public class InventoryStockReportWithValuesController {

    private final InventoryStockReportWithValuesService service;
    public InventoryStockReportWithValuesController(InventoryStockReportWithValuesService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Stock Report With Values");
        return "inventory/stock_report_with_values";
    }

    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() {
        try { return ResponseEntity.ok(service.init()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    @GetMapping("/api/as-on-date") @ResponseBody
    public ResponseEntity<?> asOnDate() {
        try { Map<String, Object> m = new LinkedHashMap<>(); m.put("asOnDate", service.asOnDate()); return ResponseEntity.ok(m); }
        catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    @PostMapping("/api/show") @ResponseBody
    public ResponseEntity<?> show(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.show(body)); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    @PostMapping("/api/update") @ResponseBody
    public ResponseEntity<?> update(@RequestBody Map<String, Object> body) {
        try {
            service.update(body);
            Map<String, Object> m = new LinkedHashMap<>(); m.put("success", true); m.put("message", "Data Updated Successfully.... ");
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
