package com.mst.controllers;

import com.mst.services.Inv42StockInTrustService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Inventory hub, module 137 "Stock in Trust" — screen 963 "Stock In Trust" (frmGatePassForStockInTrust).
 * Page /inventory/stock-in-trust, JSON under /inventory/stock-in-trust/api.
 */
@Controller
@RequestMapping("/inventory/stock-in-trust")
public class Inv42StockInTrustController {

    private final Inv42StockInTrustService service;
    public Inv42StockInTrustController(Inv42StockInTrustService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Stock In Trust");
        return "inventory/inv42_stock_in_trust";
    }

    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() { return run(service::init, "Could not load the screen."); }

    @GetMapping("/api/dropdowns") @ResponseBody
    public ResponseEntity<?> dropDowns(@RequestParam(required = false) String branchIds) { return run(() -> service.dropDowns(branchIds), "Could not load the filters."); }

    @PostMapping("/api/show") @ResponseBody
    public ResponseEntity<?> show(@RequestBody Map<String, Object> body) { return run(() -> service.show(body), "Show failed."); }

    @PostMapping("/api/status") @ResponseBody
    public ResponseEntity<?> status(@RequestBody Map<String, Object> body) {
        int id = body.get("id") == null ? 0 : Integer.parseInt(String.valueOf(body.get("id")));
        String status = body.get("status") == null ? "" : String.valueOf(body.get("status"));
        return run(() -> service.updateStatus(id, status), "Update failed.");
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (AccessDeniedException | IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(r, fallback)));
        }
    }

    private static String msg(Throwable e, String fallback) {
        return (e.getMessage() == null || e.getMessage().trim().isEmpty()) ? fallback : e.getMessage();
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
