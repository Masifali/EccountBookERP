package com.mst.controllers;

import com.mst.services.Inv4bCustomerDiscountPolicyService;
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

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * CustomerDiscountPolicy — ScreenDefinition 96 "Customer Discount Policy" (Inventory hub, module 4).
 * Page /inventory/customer-discount-policy, JSON under /inventory/customer-discount-policy/api/...
 */
@Controller
@RequestMapping("/inventory/customer-discount-policy")
public class Inv4bCustomerDiscountPolicyController {

    private final Inv4bCustomerDiscountPolicyService service;
    public Inv4bCustomerDiscountPolicyController(Inv4bCustomerDiscountPolicyService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Customer Discount Policy");
        return "inventory/inv4b_customer_discount_policy";
    }

    /** InvfrmPriceSchedule_Load :436 — rights, GridFill, DiscountTypeFill, CustomerBind. */
    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() { return run(service::init); }

    /** BtnNew_Click :476 — DiscountTypeFill + CustomerBind. */
    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups); }

    /** cmbDiscountType_Leave :337 — FillDiscountItems. */
    @GetMapping("/api/discount-items") @ResponseBody
    public ResponseEntity<?> discountItems(@RequestParam(defaultValue = "0") int discountTypeId) {
        return run(() -> service.discountItems(discountTypeId));
    }

    /** GridFill :358. */
    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history() { return run(service::history); }

    /** ReadById :233. */
    @GetMapping("/api/{id}") @ResponseBody
    public ResponseEntity<?> read(@PathVariable int id) {
        try {
            Map<String, Object> m = service.read(id);
            return m == null ? fail(HttpStatus.NOT_FOUND, "Record Not Found") : ResponseEntity.ok(m);
        } catch (Exception e) { return error(e); }
    }

    /** FormValidation :113 — before the "Are you sure to Save/Update?" box. */
    @PostMapping("/api/validate") @ResponseBody
    public ResponseEntity<?> validate(@RequestBody Map<String, Object> body) {
        return run(() -> { service.validate(body); return Map.of("success", true); });
    }

    /** Insert :149. */
    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return run(() -> service.save(body)); }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c) {
        try { return ResponseEntity.ok(c.get()); } catch (Exception e) { return error(e); }
    }

    private static ResponseEntity<?> error(Exception e) {
        if (e instanceof IllegalArgumentException) return fail(HttpStatus.BAD_REQUEST, e.getMessage());
        if (e instanceof IllegalStateException) return fail(HttpStatus.FORBIDDEN, e.getMessage());
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage() == null || t.getMessage().trim().isEmpty() ? "Request failed." : t.getMessage();
        return fail(HttpStatus.INTERNAL_SERVER_ERROR, m);
    }

    private static ResponseEntity<Map<String, Object>> fail(HttpStatus status, String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return ResponseEntity.status(status).body(m);
    }
}
