package com.mst.controllers;

import com.mst.services.AdmOrganizationService;
import com.mst.services.AdmPanelService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.function.Supplier;

/**
 * 402 Architecture.WinApp.Templates.DefineOrgnization  ->  /admin/define-organization  (Admin role only).
 * API under /api/adm-org.
 */
@Controller
public class AdmOrganizationController {

    private static final String API = "/api/adm-org";

    @Autowired private AdmOrganizationService service;
    @Autowired private AdmPanelService gate;

    @GetMapping("/admin/define-organization")
    public String page(Model model) {
        gate.admin();
        model.addAttribute("activeMenu", "apps");
        return "adm_panel/define_organization";
    }

    @GetMapping("/api/adm-org/templates") @ResponseBody
    public ResponseEntity<?> templates() { return run(service::templates, "Load failed."); }

    @GetMapping("/api/adm-org/history") @ResponseBody
    public ResponseEntity<?> history() { return run(service::history, "Load failed."); }

    @GetMapping("/api/adm-org/read") @ResponseBody
    public ResponseEntity<?> read(@RequestParam("id") int id) { return run(() -> service.read(id), "Load failed."); }

    @PostMapping("/api/adm-org/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return run(() -> service.save(body), "Save failed."); }

    private static ResponseEntity<?> run(Supplier<Object> call, String fallback) {
        try {
            return ResponseEntity.ok(call.get());
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.badRequest().body(fail(root(e, fallback)));
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(root(e, "Access denied.")));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback)));
        }
    }

    private static String root(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", false);
        r.put("message", message);
        return r;
    }
}
