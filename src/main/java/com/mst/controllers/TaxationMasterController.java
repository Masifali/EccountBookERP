package com.mst.controllers;

import com.mst.services.TaxationMasterService;
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
 * The four Taxation master-data definitions (dbo.ScreenDefinition ModuleId 9, App "Taxation"):
 *
 *   /taxation/add-tax-type                      178  InvfrmAddTaxType.cs                     "Add Tax Type"
 *   /taxation/add-tax-schedule                  177  InvfrmAddTaxSchedule.cs                 "Add Tax Schedule"
 *   /taxation/tax-notes-and-gl-maping           172  frmTaxNotesAndGLMaping.cs               "Tax Notes and GL Maping"
 *   /taxation/supplier-customer-tax-exemption   173  frmSupplierCustomerExemptionSchedule.cs "Supplier Customer Tax Exemption"
 *
 * The hub (/taxation, AppMenuController) links each screen row to its page through
 * DashboardModuleService.WEB_ROUTES_BY_SCREEN_ID - by the row's own Id, since none of these desktop class
 * names normalises to a route segment.
 *
 * No parameter carries tenancy or a user id; the service derives them from the session.
 */
@Controller
public class TaxationMasterController {

    private static final String API = "/api/taxation";

    @Autowired private TaxationMasterService service;

    // ------------------------------------------------------------------ pages

    @GetMapping("/taxation/add-tax-type")
    public String addTaxTypePage(Model model) {
        model.addAttribute("activeMenu", "taxation");
        return "taxation/add_tax_type";
    }

    @GetMapping("/taxation/add-tax-schedule")
    public String addTaxSchedulePage(Model model) {
        model.addAttribute("activeMenu", "taxation");
        return "taxation/add_tax_schedule";
    }

    @GetMapping("/taxation/tax-notes-and-gl-maping")
    public String taxNotesPage(Model model) {
        model.addAttribute("activeMenu", "taxation");
        return "taxation/tax_notes_and_gl_maping";
    }

    @GetMapping("/taxation/supplier-customer-tax-exemption")
    public String exemptionPage(Model model) {
        model.addAttribute("activeMenu", "taxation");
        return "taxation/supplier_customer_tax_exemption";
    }

    // ------------------------------------------------------------------ 178 Add Tax Type

    @GetMapping(API + "/tax-type/setup") @ResponseBody
    public ResponseEntity<?> taxTypeSetup() { return call(() -> service.taxTypeSetup(), "Load failed."); }

    @GetMapping(API + "/tax-type/list") @ResponseBody
    public ResponseEntity<?> taxTypes() { return call(() -> service.taxTypes(), "Load failed."); }

    @GetMapping(API + "/tax-type/by-id") @ResponseBody
    public ResponseEntity<?> taxType(@RequestParam("id") int id) { return call(() -> service.taxType(id), "Load failed."); }

    @PostMapping(API + "/tax-type/save") @ResponseBody
    public ResponseEntity<?> saveTaxType(@RequestBody Map<String, Object> body) { return call(() -> service.saveTaxType(body), "Save failed."); }

    // ------------------------------------------------------------------ 177 Add Tax Schedule

    @GetMapping(API + "/tax-schedule/setup") @ResponseBody
    public ResponseEntity<?> taxScheduleSetup() { return call(() -> service.taxScheduleSetup(), "Load failed."); }

    @GetMapping(API + "/tax-schedule/list") @ResponseBody
    public ResponseEntity<?> taxSchedules() { return call(() -> service.taxSchedules(), "Load failed."); }

    @GetMapping(API + "/tax-schedule/by-id") @ResponseBody
    public ResponseEntity<?> taxSchedule(@RequestParam("id") int id) { return call(() -> service.taxSchedule(id), "Load failed."); }

    @PostMapping(API + "/tax-schedule/save") @ResponseBody
    public ResponseEntity<?> saveTaxSchedule(@RequestBody Map<String, Object> body) { return call(() -> service.saveTaxSchedule(body), "Save failed."); }

    // ------------------------------------------------------------------ 172 Tax Notes and GL Maping

    @GetMapping(API + "/gl-maping/setup") @ResponseBody
    public ResponseEntity<?> glMapingSetup() { return call(() -> service.glMapingSetup(), "Load failed."); }

    @GetMapping(API + "/gl-maping/list") @ResponseBody
    public ResponseEntity<?> glMapings() { return call(() -> service.glMapings(), "Load failed."); }

    @GetMapping(API + "/gl-maping/by-id") @ResponseBody
    public ResponseEntity<?> glMaping(@RequestParam("id") int id) { return call(() -> service.glMaping(id), "Load failed."); }

    @PostMapping(API + "/gl-maping/save") @ResponseBody
    public ResponseEntity<?> saveGlMaping(@RequestBody Map<String, Object> body) { return call(() -> service.saveGlMaping(body), "Save failed."); }

    // ------------------------------------------------------------------ 173 Supplier Customer Tax Exemption

    @GetMapping(API + "/exemption/setup") @ResponseBody
    public ResponseEntity<?> exemptionSetup() { return call(() -> service.exemptionSetup(), "Load failed."); }

    @GetMapping(API + "/exemption/list") @ResponseBody
    public ResponseEntity<?> exemptions() { return call(() -> service.exemptions(), "Load failed."); }

    @GetMapping(API + "/exemption/by-id") @ResponseBody
    public ResponseEntity<?> exemption(@RequestParam("id") int id) { return call(() -> service.exemption(id), "Load failed."); }

    @PostMapping(API + "/exemption/save") @ResponseBody
    public ResponseEntity<?> saveExemption(@RequestBody Map<String, Object> body) { return call(() -> service.saveExemption(body), "Save failed."); }

    // ------------------------------------------------------------------ plumbing

    private interface Call { Object run() throws Exception; }

    /**
     * 400 for the form's own validation text, 403 for a missing right, 500 with the innermost message
     * otherwise - for an SQL RAISERROR that is the procedure's own text, as MessageBox.Show(ex.Message).
     */
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
