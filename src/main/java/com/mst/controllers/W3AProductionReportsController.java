package com.mst.controllers;

import com.mst.services.W3AProductionReportsService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * W3A - Production report screens (read-only), one shared page template / script:
 *   307 GrnsPendingOrUsedinProduction       /production/reports/grns-pending-or-used
 *   311 frmProductionPendingPurchaseInvoice /production/reports/pending-invoice-for-production
 *   820 frmConcreteProductionReport         /production/reports/concrete-production-report
 *   591 ProductionRegisterSalt              /production/reports/salt-production-register
 * JSON: /production/reports/w3a-api/{key}/init | combos | show   (key = grns | pendinginv | concrete | salt)
 */
@Controller
public class W3AProductionReportsController {

    private final W3AProductionReportsService service;
    public W3AProductionReportsController(W3AProductionReportsService service) { this.service = service; }

    private String page(Model model, String key, String title) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", title);
        model.addAttribute("w3aKey", key);
        model.addAttribute("w3aTitle", title);
        return "production/w3a_report";
    }

    @GetMapping("/production/reports/grns-pending-or-used")
    public String grns(Model m) { return page(m, "grns", "Grns Pending Or Used in Production"); }

    @GetMapping("/production/reports/pending-invoice-for-production")
    public String pendingInvoice(Model m) { return page(m, "pendinginv", "Pending Invoice For Production"); }

    @GetMapping("/production/reports/concrete-production-report")
    public String concrete(Model m) { return page(m, "concrete", "Production Report"); }

    @GetMapping("/production/reports/salt-production-register")
    public String salt(Model m) { return page(m, "salt", "Production Register Salt"); }

    @GetMapping("/production/reports/w3a-api/{key}/init") @ResponseBody
    public ResponseEntity<?> init(@PathVariable String key) { return run(() -> service.init(key)); }

    @PostMapping("/production/reports/w3a-api/{key}/combos") @ResponseBody
    public ResponseEntity<?> combos(@PathVariable String key, @RequestBody Map<String, Object> body) {
        return run(() -> {
            int screen = key.equals("grns") ? W3AProductionReportsService.S_GRNS : W3AProductionReportsService.S_PENDING_INVOICE;
            if (!key.equals("grns") && !key.equals("pendinginv")) throw new IllegalArgumentException("Unknown report.");
            return service.stockCombos(screen, body.get("branchIds"));
        });
    }

    @PostMapping("/production/reports/w3a-api/{key}/show") @ResponseBody
    public ResponseEntity<?> show(@PathVariable String key, @RequestBody Map<String, Object> body) {
        return run(() -> {
            switch (key) {
                case "grns": return service.grnsPendingOrUsed(body);
                case "pendinginv": return service.pendingInvoices(body);
                case "concrete": return service.concreteProduction(body);
                case "salt": return service.saltRegister(body);
                default: throw new IllegalArgumentException("Unknown report.");
            }
        });
    }

    private interface Call { Object get(); }
    private ResponseEntity<?> run(Call c) {
        try { return ResponseEntity.ok(c.get()); }
        catch (AccessDeniedException e) { return fail(HttpStatus.FORBIDDEN, msg(e)); }
        catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
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
