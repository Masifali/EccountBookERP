package com.mst.controllers;

import com.mst.services.ExportForwardingDirectService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Export (App 8, ModuleId 11):
 *   /export/forwarding-direct                 195 EximForwardingDirect             "Forwarding Direct (Not Use)"
 *   /export/forwarding-without-weighbridge    196 EximForwardingWithoutWeighBridge "Forwarding Without WeighBridge (Not Use)"
 * APIs /api/export/forwarding-direct/... and /api/export/forwarding-without-weighbridge/... (same handlers, the
 * path picks the form). No parameter carries tenancy.
 */
@Controller
public class ExportForwardingDirectController {

    @Autowired private ExportForwardingDirectService svc;

    @GetMapping("/export/forwarding-direct")
    public String directPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/forwarding_direct"; }

    @GetMapping("/export/forwarding-without-weighbridge")
    public String withoutWbPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/forwarding_without_weighbridge"; }

    private static boolean direct(String form) {
        if ("forwarding-direct".equals(form)) return true;
        if ("forwarding-without-weighbridge".equals(form)) return false;
        throw new IllegalArgumentException("Unknown form");
    }

    @GetMapping("/api/export/{form:forwarding-direct|forwarding-without-weighbridge}/setup") @ResponseBody
    public ResponseEntity<?> setup(@PathVariable("form") String form) { return call(() -> svc.setup(direct(form)), "Error occurred during database call."); }

    @GetMapping("/api/export/{form:forwarding-direct|forwarding-without-weighbridge}/refresh") @ResponseBody
    public ResponseEntity<?> refresh(@PathVariable("form") String form) { return call(() -> svc.refresh(direct(form)), "Load failed."); }

    @GetMapping("/api/export/{form:forwarding-direct|forwarding-without-weighbridge}/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@PathVariable("form") String form, @RequestParam(value = "itemId", defaultValue = "0") int itemId) {
        return call(() -> svc.uoms(direct(form), itemId), "Load failed.");
    }

    @GetMapping("/api/export/forwarding-without-weighbridge/invoice") @ResponseBody
    public ResponseEntity<?> invoice(@RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId) {
        return call(() -> svc.invoiceLeave(invoiceId), "Load failed.");
    }

    @GetMapping("/api/export/{form:forwarding-direct|forwarding-without-weighbridge}/history") @ResponseBody
    public ResponseEntity<?> history(@PathVariable("form") String form, @RequestParam(value = "noOfRecords", defaultValue = "0") int noOfRecords) {
        return call(() -> svc.history(direct(form), noOfRecords), "Load failed.");
    }

    @GetMapping("/api/export/forwarding-direct/reference-history") @ResponseBody
    public ResponseEntity<?> referenceHistory(@RequestParam(value = "referenceNo", defaultValue = "") String referenceNo) {
        return call(() -> svc.referenceHistory(referenceNo), "Load failed.");
    }

    @PostMapping("/api/export/forwarding-direct/register") @ResponseBody
    public ResponseEntity<?> register(@RequestBody Map<String, Object> body) { return call(() -> svc.register(body), "Load failed."); }

    @GetMapping("/api/export/{form:forwarding-direct|forwarding-without-weighbridge}/by-id") @ResponseBody
    public ResponseEntity<?> byId(@PathVariable("form") String form, @RequestParam("id") int id) {
        return call(() -> svc.readById(direct(form), id), "Load failed.");
    }

    @PostMapping("/api/export/{form:forwarding-direct|forwarding-without-weighbridge}/validate") @ResponseBody
    public ResponseEntity<?> validate(@PathVariable("form") String form, @RequestBody Map<String, Object> body) {
        return call(() -> svc.validate(direct(form), body), "Save failed.");
    }

    @PostMapping("/api/export/{form:forwarding-direct|forwarding-without-weighbridge}/save") @ResponseBody
    public ResponseEntity<?> save(@PathVariable("form") String form, @RequestBody Map<String, Object> body) {
        return call(() -> svc.save(direct(form), body), "Save failed.");
    }

    private interface Call { Object run() throws Exception; }

    private static ResponseEntity<?> call(Call c, String fallback) {
        try { return ResponseEntity.ok(c.run()); }
        catch (IllegalArgumentException | IllegalStateException e) { return ResponseEntity.badRequest().body(fail(root(e, fallback))); }
        catch (AccessDeniedException e) { return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(root(e, "Access denied."))); }
        catch (Exception e) { return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback))); }
    }

    private static String root(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
