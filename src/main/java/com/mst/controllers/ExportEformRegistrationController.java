package com.mst.controllers;

import com.mst.services.ExportEformRegistrationService;
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

/**
 * Export (App 8, ModuleId 11):
 *   /export/eform-registration      217 ExpfrmEformRegistration    "EFrom Registration (Not Use)"
 *   /export/financial-instrument    218 ExpfrmFinancialInsturment  "Financial Insturment (Not Use)"
 * Both write ExImEFormRegistration through ExportEformRegistrationService. No parameter carries tenancy.
 */
@Controller
public class ExportEformRegistrationController {

    private static final String EF = "/api/export/eform-registration";
    private static final String FI = "/api/export/financial-instrument";

    @Autowired private ExportEformRegistrationService svc;

    @GetMapping("/export/eform-registration")
    public String eformPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/eform_registration"; }

    @GetMapping("/export/financial-instrument")
    public String fiPage(Model model) { model.addAttribute("activeMenu", "export"); return "export/financial_instrument"; }

    // ---------------------------------------------------------------- 217
    @GetMapping(EF + "/setup") @ResponseBody
    public ResponseEntity<?> efSetup() { return call(() -> svc.setup(false), "Error occurred during database call."); }
    @GetMapping(EF + "/combos") @ResponseBody
    public ResponseEntity<?> efCombos() { return call(() -> svc.combos(false), "Load failed."); }
    @GetMapping(EF + "/receipts-by-customer") @ResponseBody
    public ResponseEntity<?> efReceipts(@RequestParam(value = "supplierCustomerId", defaultValue = "0") int supplierCustomerId) {
        return call(() -> svc.receiptsByCustomer(false, supplierCustomerId), "Load failed.");
    }
    @GetMapping(EF + "/receipt-by-id") @ResponseBody
    public ResponseEntity<?> efReceipt(@RequestParam(value = "id", defaultValue = "0") int id) { return call(() -> svc.receiptById(false, id), "Load failed."); }
    @GetMapping(EF + "/by-id") @ResponseBody
    public ResponseEntity<?> efById(@RequestParam("id") int id) { return call(() -> svc.readById(false, id), "Load failed."); }
    @GetMapping(EF + "/history") @ResponseBody
    public ResponseEntity<?> efHistory() { return call(() -> svc.history(false), "Load failed."); }
    @PostMapping(EF + "/save") @ResponseBody
    public ResponseEntity<?> efSave(@RequestBody Map<String, Object> body) { return call(() -> svc.save(false, body), "Save failed."); }

    // ---------------------------------------------------------------- 218
    @GetMapping(FI + "/setup") @ResponseBody
    public ResponseEntity<?> fiSetup() { return call(() -> svc.setup(true), "Error occurred during database call."); }
    @GetMapping(FI + "/combos") @ResponseBody
    public ResponseEntity<?> fiCombos() { return call(() -> svc.combos(true), "Load failed."); }
    @GetMapping(FI + "/receipts-by-customer") @ResponseBody
    public ResponseEntity<?> fiReceipts(@RequestParam(value = "supplierCustomerId", defaultValue = "0") int supplierCustomerId) {
        return call(() -> svc.receiptsByCustomer(true, supplierCustomerId), "Load failed.");
    }
    @GetMapping(FI + "/receipt-by-id") @ResponseBody
    public ResponseEntity<?> fiReceipt(@RequestParam(value = "id", defaultValue = "0") int id) { return call(() -> svc.receiptById(true, id), "Load failed."); }
    @GetMapping(FI + "/by-id") @ResponseBody
    public ResponseEntity<?> fiById(@RequestParam("id") int id) { return call(() -> svc.readById(true, id), "Load failed."); }
    @GetMapping(FI + "/history") @ResponseBody
    public ResponseEntity<?> fiHistory() { return call(() -> svc.history(true), "Load failed."); }
    @PostMapping(FI + "/save") @ResponseBody
    public ResponseEntity<?> fiSave(@RequestBody Map<String, Object> body) { return call(() -> svc.save(true, body), "Save failed."); }

    // ---------------------------------------------------------------- plumbing (as ExportModuleController)
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
