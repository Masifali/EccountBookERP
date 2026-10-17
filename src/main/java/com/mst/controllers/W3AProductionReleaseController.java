package com.mst.controllers;

import com.mst.services.W3AProductionReleaseService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/** W3A - 599 ProductionRelease (pcc.Production). Page /production/production-release, JSON under /production/production-release/api. */
@Controller
public class W3AProductionReleaseController {

    private final W3AProductionReleaseService service;
    public W3AProductionReleaseController(W3AProductionReleaseService service) { this.service = service; }

    @GetMapping("/production/production-release")
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Production Release");
        return "production/w3a_production_release";
    }

    @GetMapping("/production/production-release/api/init") @ResponseBody
    public ResponseEntity<?> init() { return run(service::init); }

    @PostMapping("/production/production-release/api/pending") @ResponseBody
    public ResponseEntity<?> pending(@RequestBody Map<String, Object> b) { return run(() -> service.pending(b)); }

    @GetMapping("/production/production-release/api/detail/{id}") @ResponseBody
    public ResponseEntity<?> detail(@PathVariable int id) { return run(() -> service.detail(id)); }

    @PostMapping("/production/production-release/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> b) { return run(() -> service.history(b)); }

    @PostMapping("/production/production-release/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> b) {
        return run(() -> { service.save(b); Map<String, Object> m = new LinkedHashMap<>(); m.put("success", true); m.put("message", "Record Save Successfully"); return m; });
    }

    @PostMapping("/production/production-release/api/delete/{id}") @ResponseBody
    public ResponseEntity<?> delete(@PathVariable int id) {
        return run(() -> { service.delete(id); Map<String, Object> m = new LinkedHashMap<>(); m.put("success", true); m.put("message", "Record Deleted Successfully."); return m; });
    }

    @GetMapping("/production/production-release/api/voucher-head/{id}") @ResponseBody
    public ResponseEntity<?> voucherHead(@PathVariable int id) {
        return run(() -> { Map<String, Object> m = new LinkedHashMap<>(); m.put("voucherHeadId", service.voucherHeadId(id)); return m; });
    }

    private interface Call { Object get(); }
    private ResponseEntity<?> run(Call c) {
        try { return ResponseEntity.ok(c.get()); }
        catch (AccessDeniedException e) { return fail(HttpStatus.FORBIDDEN, msg(e)); }
        catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }
    private static ResponseEntity<Map<String, Object>> fail(HttpStatus s, String message) {
        Map<String, Object> m = new LinkedHashMap<>(); m.put("success", false); m.put("message", message);
        return ResponseEntity.status(s).body(m);
    }
    private static String msg(Exception e) {
        Throwable t = e; while (t.getCause() != null && (t.getMessage() == null || t.getMessage().isBlank())) t = t.getCause();
        return t.getMessage() == null || t.getMessage().isBlank() ? "Request failed." : t.getMessage();
    }
}
