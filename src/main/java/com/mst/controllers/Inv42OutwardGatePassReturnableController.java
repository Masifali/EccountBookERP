package com.mst.controllers;

import com.mst.services.Inv42OutwardGatePassReturnableService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Inventory hub, module 42 — screen 928 "Outward GatePass" (OutwardGatePassReturnable, DocumentTypeId 1201).
 * Page /inventory/returnable/outward-gate-pass-returnable, JSON under .../api.
 */
@Controller
@RequestMapping("/inventory/returnable/outward-gate-pass-returnable")
public class Inv42OutwardGatePassReturnableController {

    private final Inv42OutwardGatePassReturnableService service;
    public Inv42OutwardGatePassReturnableController(Inv42OutwardGatePassReturnableService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Outward GatePass");
        return "inventory/inv42_outward_gate_pass_returnable";
    }

    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return run(service::refresh, "Refresh failed."); }

    @GetMapping("/api/gp-no") @ResponseBody
    public ResponseEntity<?> gpNo() { return run(() -> Collections.singletonMap("gpNo", service.gpNo()), "Could not generate the Gate Pass No."); }

    @GetMapping("/api/type-code") @ResponseBody
    public ResponseEntity<?> typeCode(@RequestParam String gatepassType) {
        return run(() -> Collections.singletonMap("gpTypeSrNo", service.typeCode(gatepassType)), "Could not generate the type number.");
    }

    @GetMapping("/api/delivery-order") @ResponseBody
    public ResponseEntity<?> deliveryOrder(@RequestParam int docNo) { return run(() -> service.deliveryOrder(docNo), "Delivery order lookup failed."); }

    @GetMapping("/api/open-gate-passes") @ResponseBody
    public ResponseEntity<?> open() { return run(service::openGatePasses, "Could not load the open gate passes."); }

    @GetMapping("/api/pending-delivery-orders") @ResponseBody
    public ResponseEntity<?> pending() { return run(service::pendingDeliveryOrders, "Could not load the delivery orders."); }

    @PostMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return run(() -> service.history(body), "History failed."); }

    @GetMapping("/api/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record Not Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not open that gate pass.")));
        }
    }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return write(() -> service.save(body)); }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> write(Call c) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(r, "Save failed.")));
        }
    }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
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
