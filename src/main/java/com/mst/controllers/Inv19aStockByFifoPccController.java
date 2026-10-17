package com.mst.controllers;

import com.mst.services.Inv19aStockByFifoPccService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/** StockReportByFifo (Pcc.Reports) - ScreenDefinition 842 "Stock By FiFo" (Inventory hub, module 19). */
@Controller
@RequestMapping("/inventory/reports/stock-by-fifo-pcc")
public class Inv19aStockByFifoPccController {

    private final Inv19aStockByFifoPccService service;
    public Inv19aStockByFifoPccController(Inv19aStockByFifoPccService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Stock By FiFo");
        return "inventory/inv19a_stock_by_fifo_pcc";
    }

    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() {
        try { return ResponseEntity.ok(service.init()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    @GetMapping("/api/combos") @ResponseBody
    public ResponseEntity<?> combos() {
        try { return ResponseEntity.ok(service.combos()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    @GetMapping("/api/varients") @ResponseBody
    public ResponseEntity<?> varients(@RequestParam(name = "itemId") int itemId) {
        try { return ResponseEntity.ok(service.varients(itemId)); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

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
