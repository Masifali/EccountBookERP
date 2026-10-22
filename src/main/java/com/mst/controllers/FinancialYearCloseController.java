package com.mst.controllers;

import com.mst.services.AdmPanelService;
import com.mst.services.FinancialYearCloseService;
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
 * Admin Panel hub, module 43 "System Admin": 399 Architecture.WinApp.Account_Definition.frmFinancialYearClose
 * "Financial Year Close" at /admin/financial-year-close. Admin role only (AdmPanelService.admin()).
 */
@Controller
public class FinancialYearCloseController {


    @Autowired private FinancialYearCloseService service;
    @Autowired private AdmPanelService adm;

    @GetMapping("/admin/financial-year-close")
    public String page(Model model) {
        adm.admin();
        model.addAttribute("activeMenu", "apps");
        return "adm_panel/financial_year_close";
    }

    @GetMapping("/api/adm-panel/fyc/init") @ResponseBody
    public ResponseEntity<?> init() { return run(service::init, "Error occurred during database call."); }

    @PostMapping("/api/adm-panel/fyc/show") @ResponseBody
    public ResponseEntity<?> show(@RequestBody Map<String, Object> body) { return run(() -> service.show(body), "Show failed."); }

    @PostMapping("/api/adm-panel/fyc/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return run(() -> service.save(body), "Save failed."); }

    @GetMapping("/api/adm-panel/fyc/history") @ResponseBody
    public ResponseEntity<?> history() { return run(service::history, "History failed."); }

    @GetMapping("/api/adm-panel/fyc/get") @ResponseBody
    public ResponseEntity<?> get(@RequestParam("id") int id) { return run(() -> service.byId(id), "No Record Found"); }

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

    /** The desktop shows ex.Message - for a RAISERROR that is the procedure's own text. */
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
