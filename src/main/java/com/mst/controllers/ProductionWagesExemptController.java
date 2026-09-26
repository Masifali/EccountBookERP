package com.mst.controllers;

import com.mst.services.ProductionWagesExemptService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Wages Exempt Item Schedule - WagesExemptItemSchedule.cs (Architecture.WinApp.Lookups). A standalone
 * page; Stock Conversion's "Wages Exempt" button opens it in a new window, as the desktop .Show()s it.
 * No parameter carries tenancy or a user id; they are server-derived in the service.
 */
@Controller
public class ProductionWagesExemptController {

    @Autowired
    private ProductionWagesExemptService service;

    @GetMapping("/production/wages-exempt-item-schedule")
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/wages_exempt_item_schedule";
    }

    @GetMapping("/api/production/wages-exempt-item-schedule/setup")
    @ResponseBody
    public ResponseEntity<?> setup() {
        try { return ResponseEntity.ok(service.setup()); }
        catch (Exception e) { return error(e, "Setup failed."); }
    }

    @GetMapping("/api/production/wages-exempt-item-schedule/history")
    @ResponseBody
    public ResponseEntity<?> history() {
        try { return ResponseEntity.ok(service.history()); }
        catch (Exception e) { return error(e, "History failed."); }
    }

    @PostMapping("/api/production/wages-exempt-item-schedule/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) {
        try {
            return ResponseEntity.ok(service.save(body));
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.badRequest().body(fail(root(e, "Save failed.")));
        } catch (Exception e) {
            return error(e, "Save failed.");
        }
    }

    private static String root(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static ResponseEntity<Map<String, Object>> error(Exception e, String fallback) {
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback)));
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", false);
        r.put("message", message);
        return r;
    }
}
