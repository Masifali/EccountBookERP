package com.mst.controllers;

import com.mst.services.Inv19cInventoryProfitLossService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/** frmInventoryProfitLoss - ScreenDefinition 289 "Inventory Stock Evaluation Profit & Loss (Not Use)" (Inventory hub, Stock Reports). */
@Controller
@RequestMapping("/inventory/reports/inventory-profit-loss")
public class Inv19cInventoryProfitLossController {

    private final Inv19cInventoryProfitLossService service;
    public Inv19cInventoryProfitLossController(Inv19cInventoryProfitLossService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Inventory Stock Evaluation Profit & Loss");
        return "inventory/inventory_profit_loss";
    }

    /** frmStockReport_Load :115 - combos, stock accounts, class groups, financial-year start. */
    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() {
        try { return ResponseEntity.ok(service.init()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    /** GridBind :289. */
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
