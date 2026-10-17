package com.mst.controllers;

import com.mst.services.Inv42GdnReturnableService;
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
 * Inventory hub, module 42 — screen 927 "Issuance Gdn" (GdnReturnable, DocumentTypeId 1202).
 * Page /inventory/returnable/gdn-returnable, JSON under /inventory/returnable/gdn-returnable/api.
 */
@Controller
@RequestMapping("/inventory/returnable/gdn-returnable")
public class Inv42GdnReturnableController {

    private final Inv42GdnReturnableService service;
    public Inv42GdnReturnableController(Inv42GdnReturnableService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Issuance Gdn");
        return "inventory/inv42_gdn_returnable";
    }

    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return run(service::refresh, "Refresh failed."); }

    @GetMapping("/api/doc-no") @ResponseBody
    public ResponseEntity<?> docNo() { return run(() -> Collections.singletonMap("docNo", service.docNo()), "Could not generate the Doc No."); }

    @GetMapping("/api/pending-gate-passes") @ResponseBody
    public ResponseEntity<?> pendingGatePasses() { return run(service::pendingGatePasses, "Could not load the pending gate passes."); }

    @GetMapping("/api/history-combos") @ResponseBody
    public ResponseEntity<?> historyCombos() { return run(service::historyCombos, "Could not load the history filters."); }

    @GetMapping("/api/pending-delivery-order-rows") @ResponseBody
    public ResponseEntity<?> pendingRows(@RequestParam(defaultValue = "0") int customerId,
                                         @RequestParam(defaultValue = "0") int deliveryTypeId,
                                         @RequestParam(defaultValue = "0") int deliveryOrderId,
                                         @RequestParam(defaultValue = "false") boolean fromGatePass) {
        return run(() -> service.pendingDeliveryOrderRows(customerId, deliveryTypeId, deliveryOrderId, fromGatePass), "Could not load the delivery order.");
    }

    @PostMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return run(() -> service.history(body), "History failed."); }

    @GetMapping("/api/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record Not Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not open that document.")));
        }
    }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return write(() -> service.save(body)); }

    @PostMapping("/api/{id:[0-9]+}/delete") @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) { return write(() -> service.delete(id)); }

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
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, fallback)));
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
