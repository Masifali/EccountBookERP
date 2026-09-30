package com.mst.controllers;

import com.mst.services.InventoryStockReportService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/** frmStockReport - Stock Report (opened from the Sale Order / GDN / GRN "Stock Report" buttons and from the Inventory menu). */
@Controller
@RequestMapping("/inventory/stock-report")
public class InventoryStockReportController {

    private final InventoryStockReportService service;
    public InventoryStockReportController(InventoryStockReportService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Stock Report");
        return "inventory/stock_report";
    }

    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() {
        if (!service.canView()) return fail(HttpStatus.FORBIDDEN, "You don't have right");
        try { return ResponseEntity.ok(service.init()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }
    @GetMapping("/api/uoms") @ResponseBody
    public List<Map<String, Object>> uoms(@RequestParam("itemId") int itemId) { return service.uoms(itemId); }

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
