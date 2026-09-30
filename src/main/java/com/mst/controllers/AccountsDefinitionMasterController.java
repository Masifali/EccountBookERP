package com.mst.controllers;

import com.mst.services.AccountsDefinitionMasterService;
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
 * AppModules 45 "Accounts Definition" (App 19 "Master Data Definition"):
 *
 *   427 /master-data/pdc-bank            PdcBank               430 /master-data/shipment-documents  ExImShipmentDocuments
 *   429 /master-data/document-group      DocumentGroup         414 /master-data/bs-pl-setting       BsPlSettingForm
 *   428 /master-data/tax-lookup          Lookups.TaxLookup     413 /master-data/bank                AcfrmDefineBank
 *   412 Account Custom Group = the desktop class of screen 1 -> /accounts/custom_group (DashboardModuleService).
 *
 * API: /api/master-data/{key}/{setup|list|by-id|save} plus the per-form extras named below.
 */
@Controller
public class AccountsDefinitionMasterController {

    private static final String API = "/api/master-data";

    @Autowired private AccountsDefinitionMasterService service;

    @GetMapping("/master-data/{page:pdc-bank|document-group|shipment-documents|tax-lookup|bs-pl-setting|bank}")
    public String page(@PathVariable("page") String page, Model model) {
        model.addAttribute("activeMenu", "apps");
        return "master_data/" + page.replace('-', '_');
    }

    @GetMapping(API + "/{key:pdc-bank|document-group|shipment-documents|tax-lookup|bank}/setup")
    @ResponseBody
    public ResponseEntity<?> setup(@PathVariable("key") String key) {
        try {
            switch (key) {
                case "pdc-bank":           return ResponseEntity.ok(service.pdcBanks());
                case "document-group":     return ResponseEntity.ok(service.docGroups());
                case "shipment-documents": return ResponseEntity.ok(service.shipmentDocs());
                case "tax-lookup":         return ResponseEntity.ok(service.taxLookupSetup());
                case "bank":               return ResponseEntity.ok(service.bankSetup());
                default: return ResponseEntity.notFound().build();
            }
        } catch (Exception e) { return error(e, "Load failed."); }
    }

    @GetMapping(API + "/{key:pdc-bank|document-group|shipment-documents|tax-lookup|bank}/list")
    @ResponseBody
    public ResponseEntity<?> list(@PathVariable("key") String key) {
        try {
            switch (key) {
                case "pdc-bank":           return ResponseEntity.ok(service.pdcBanks());
                case "document-group":     return ResponseEntity.ok(service.docGroups());
                case "shipment-documents": return ResponseEntity.ok(service.shipmentDocs());
                case "tax-lookup":         return ResponseEntity.ok(service.taxLookups());
                case "bank":               return ResponseEntity.ok(service.banks());
                default: return ResponseEntity.notFound().build();
            }
        } catch (Exception e) { return error(e, "Load failed."); }
    }

    @GetMapping(API + "/{key:pdc-bank|document-group|shipment-documents|tax-lookup|bank}/by-id")
    @ResponseBody
    public ResponseEntity<?> byId(@PathVariable("key") String key, @RequestParam("id") int id) {
        try {
            switch (key) {
                case "pdc-bank":           return ResponseEntity.ok(service.pdcBank(id));
                case "document-group":     return ResponseEntity.ok(service.docGroup(id));
                case "shipment-documents": return ResponseEntity.ok(service.shipmentDoc(id));
                case "tax-lookup":         return ResponseEntity.ok(service.taxLookup(id));
                case "bank":               return ResponseEntity.ok(service.bank(id));
                default: return ResponseEntity.notFound().build();
            }
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(root(e, "Record not found.")));
        } catch (Exception e) { return error(e, "Load failed."); }
    }

    @PostMapping(API + "/{key:pdc-bank|document-group|shipment-documents|tax-lookup|bank}/save")
    @ResponseBody
    public ResponseEntity<?> save(@PathVariable("key") String key, @RequestBody Map<String, Object> body) {
        try {
            switch (key) {
                case "pdc-bank":           return ResponseEntity.ok(service.savePdcBank(body));
                case "document-group":     return ResponseEntity.ok(service.saveDocGroup(body));
                case "shipment-documents": return ResponseEntity.ok(service.saveShipmentDoc(body));
                case "tax-lookup":         return ResponseEntity.ok(service.saveTaxLookup(body));
                case "bank":               return ResponseEntity.ok(service.saveBank(body));
                default: return ResponseEntity.notFound().build();
            }
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.badRequest().body(fail(root(e, "Save failed.")));
        } catch (Exception e) { return error(e, "Save failed."); }
    }

    /** TaxLookup.cmbProfileName_Leave - the generated code for a lookup type. */
    @GetMapping(API + "/tax-lookup/code")
    @ResponseBody
    public ResponseEntity<?> taxLookupCode(@RequestParam("typeId") int typeId) {
        try { return ResponseEntity.ok(service.taxLookupCode(typeId)); } catch (Exception e) { return error(e, "Code failed."); }
    }

    /** AcfrmDefineBank.btnRefresh_Click - the four combos. */
    @GetMapping(API + "/bank/combos")
    @ResponseBody
    public ResponseEntity<?> bankCombos() {
        try { return ResponseEntity.ok(service.bankCombos()); } catch (Exception e) { return error(e, "Load failed."); }
    }

    // ------------------------------------------------------------------ 414 BS & PL Setting

    /** GridLoad - req = PL | BS, accountClassId 2 Assets / 3 Liabilities for BS. */
    @GetMapping(API + "/bs-pl-setting/load")
    @ResponseBody
    public ResponseEntity<?> bsPlLoad(@RequestParam("req") String req,
                                      @RequestParam(value = "accountClassId", required = false, defaultValue = "0") int accountClassId) {
        try { return ResponseEntity.ok(service.bsPlLoad(req, accountClassId)); }
        catch (IllegalArgumentException e) { return ResponseEntity.badRequest().body(fail(root(e, "Load failed."))); }
        catch (Exception e) { return error(e, "Load failed."); }
    }

    /** btnUpdate_Click - the changed rows. */
    @PostMapping(API + "/bs-pl-setting/update")
    @ResponseBody
    public ResponseEntity<?> bsPlUpdate(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.bsPlUpdate(body)); }
        catch (IllegalArgumentException e) { return ResponseEntity.badRequest().body(fail(root(e, "Update failed."))); }
        catch (Exception e) { return error(e, "Update failed."); }
    }

    /** GridNoteTitleFill - note = "" | PL | BS. */
    @GetMapping(API + "/bs-pl-setting/notes")
    @ResponseBody
    public ResponseEntity<?> bsPlNotes(@RequestParam(value = "note", required = false, defaultValue = "") String note) {
        try { return ResponseEntity.ok(service.bsPlNotes(note)); } catch (Exception e) { return error(e, "Load failed."); }
    }

    /** btnUpdateNote_Click - the retitled rows. */
    @PostMapping(API + "/bs-pl-setting/update-notes")
    @ResponseBody
    public ResponseEntity<?> bsPlUpdateNotes(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.bsPlUpdateNotes(body)); } catch (Exception e) { return error(e, "Update failed."); }
    }

    // ------------------------------------------------------------------ plumbing

    private static String root(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static ResponseEntity<Map<String, Object>> error(Exception e, String fallback) {
        if (e instanceof AccessDeniedException) return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(root(e, "Access denied.")));
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback)));
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", false);
        r.put("message", message);
        return r;
    }
}
