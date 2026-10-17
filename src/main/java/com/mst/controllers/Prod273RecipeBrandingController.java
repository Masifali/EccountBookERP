package com.mst.controllers;

import com.mst.services.Prod273RecipeBrandingService;
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
 * Production hub (module 18) — screen 285 "Receipe (Branding)"
 * (Architecture.WinApp.Mfg.frmBillOfMaterial_Manufacturing, DocumentTypeId 1120).
 * Page /production/recipe-branding, JSON under /production/recipe-branding/api.
 *
 * Errors: validation → 400 {message}; missing right → 403 {message}; anything else → 500 {message}.
 */
@Controller
@RequestMapping("/production/recipe-branding")
public class Prod273RecipeBrandingController {

    private final Prod273RecipeBrandingService service;
    public Prod273RecipeBrandingController(Prod273RecipeBrandingService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Receipe (Branding)");
        return "production/prod273_recipe_branding";
    }

    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return run(service::refresh, "Refresh failed."); }

    @GetMapping("/api/doc-no") @ResponseBody
    public ResponseEntity<?> docNo() { return run(() -> Collections.singletonMap("docNo", service.docNo()), "Could not generate the Doc No."); }

    @GetMapping("/api/history-combos") @ResponseBody
    public ResponseEntity<?> historyCombos() { return run(service::historyCombos, "Could not load the history filters."); }

    @GetMapping("/api/avg-rate") @ResponseBody
    public ResponseEntity<?> avgRate(@RequestParam(defaultValue = "0") int itemId, @RequestParam(required = false) String docDate) {
        return run(() -> Collections.singletonMap("rate", service.avgRate(itemId, docDate)), "Could not read the average rate.");
    }

    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int bomId, @RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.history(fromDate, toDate, fromDocNo, toDocNo, bomId, itemId), "History failed.");
    }

    @GetMapping("/api/attachments") @ResponseBody
    public ResponseEntity<?> attachments(@RequestParam int id) { return run(() -> service.attachments(id), "Could not read the attachments."); }

    @GetMapping("/api/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record Not Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not open that recipe.")));
        }
    }

    @PostMapping("/api/precheck") @ResponseBody
    public ResponseEntity<?> precheck(@RequestBody Map<String, Object> body) { return write(() -> service.precheck(body)); }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return write(() -> service.save(body)); }

    @PostMapping("/api/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam int id) { return write(() -> service.delete(id)); }

    // ------------------------------------------------------------------------------ plumbing

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
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
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
