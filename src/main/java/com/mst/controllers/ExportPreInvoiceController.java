package com.mst.controllers;

import com.mst.services.ExportPreInvoiceService;
import com.mst.services.ExportPreInvoiceService.Variant;
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

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * 192 "Pre Invoice" (PreCommiercialInvoice, DocumentTypeId 209) and 194 "Export Opening Balance"
 * (CommiercialInvoiceForOpeningBalance, DocumentTypeId 212) - Export application, ModuleId 100.
 *
 *   page /export/pre-invoice       API /api/export/pre-invoice/...
 *   page /export/opening-balance   API /api/export/opening-balance/...
 *
 * The {form} path segment picks the screen (and with it the ScreenDefinition.Id the rights are checked against);
 * nothing else in a request carries tenancy or a user id - ExportPreInvoiceService takes them from the session.
 */
@Controller
public class ExportPreInvoiceController {

    @Autowired private ExportPreInvoiceService svc;

    @GetMapping("/export/pre-invoice")
    public String preInvoicePage(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/pre_invoice";
    }

    @GetMapping("/export/opening-balance")
    public String openingBalancePage(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/opening_balance";
    }

    private static Variant variant(String form) {
        if ("pre-invoice".equals(form)) return Variant.PRE;
        if ("opening-balance".equals(form)) return Variant.OPENING;
        throw new IllegalArgumentException("Unknown form");
    }

    @GetMapping("/api/export/{form:pre-invoice|opening-balance}/setup") @ResponseBody
    public ResponseEntity<?> setup(@PathVariable("form") String form) {
        return call(() -> svc.setup(variant(form)), "Error occurred during database call.");
    }

    @GetMapping("/api/export/{form:pre-invoice|opening-balance}/new-codes") @ResponseBody
    public ResponseEntity<?> newCodes(@PathVariable("form") String form) {
        return call(() -> svc.newCodes(variant(form)), "Load failed.");
    }

    @GetMapping("/api/export/{form:pre-invoice|opening-balance}/refresh") @ResponseBody
    public ResponseEntity<?> refresh(@PathVariable("form") String form) {
        return call(() -> svc.refresh(variant(form)), "Load failed.");
    }

    @GetMapping("/api/export/{form:pre-invoice|opening-balance}/history-combos") @ResponseBody
    public ResponseEntity<?> historyCombos(@PathVariable("form") String form) {
        return call(() -> svc.historyCombos(variant(form)), "Load failed.");
    }

    @GetMapping("/api/export/{form:pre-invoice|opening-balance}/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@PathVariable("form") String form, @RequestParam(value = "itemId", defaultValue = "0") int itemId) {
        return call(() -> svc.uoms(variant(form), itemId), "Load failed.");
    }

    @GetMapping("/api/export/{form:pre-invoice|opening-balance}/financial-instruments") @ResponseBody
    public ResponseEntity<?> fis(@PathVariable("form") String form, @RequestParam(value = "customerId", defaultValue = "0") int customerId) {
        return call(() -> svc.financialInstruments(variant(form), customerId), "Load failed.");
    }

    @GetMapping("/api/export/{form:pre-invoice|opening-balance}/fi-balance") @ResponseBody
    public ResponseEntity<?> fiBalance(@PathVariable("form") String form,
                                       @RequestParam(value = "documentTypeId", defaultValue = "0") int documentTypeId,
                                       @RequestParam(value = "id", defaultValue = "0") int id) {
        return call(() -> svc.fiBalance(variant(form), documentTypeId, id), "Load failed.");
    }

    @GetMapping("/api/export/{form:pre-invoice|opening-balance}/loader/setup") @ResponseBody
    public ResponseEntity<?> loaderSetup(@PathVariable("form") String form) {
        return call(() -> svc.loaderSetup(variant(form)), "Load failed.");
    }

    @GetMapping("/api/export/{form:pre-invoice|opening-balance}/loader/rows") @ResponseBody
    public ResponseEntity<?> loaderRows(@PathVariable("form") String form,
                                        @RequestParam(value = "partyId", defaultValue = "0") int partyId,
                                        @RequestParam(value = "itemId", defaultValue = "0") int itemId,
                                        @RequestParam(value = "currencyId", defaultValue = "0") int currencyId) {
        return call(() -> svc.loaderRows(variant(form), partyId, itemId, currencyId), "Load failed.");
    }

    @PostMapping("/api/export/{form:pre-invoice|opening-balance}/loader/load") @ResponseBody
    public ResponseEntity<?> loaderLoad(@PathVariable("form") String form, @RequestBody Map<String, Object> body) {
        Object ids = body.get("ids");
        List<Object> list = ids instanceof List ? castList(ids) : new ArrayList<>();
        return call(() -> svc.loadProformas(variant(form), list), "Load failed.");
    }

    @GetMapping("/api/export/{form:pre-invoice|opening-balance}/by-id") @ResponseBody
    public ResponseEntity<?> byId(@PathVariable("form") String form, @RequestParam("id") int id) {
        return call(() -> svc.readById(variant(form), id), "Load failed.");
    }

    @GetMapping("/api/export/{form:pre-invoice|opening-balance}/history-detail") @ResponseBody
    public ResponseEntity<?> historyDetail(@PathVariable("form") String form, @RequestParam("id") int id) {
        return call(() -> svc.historyDetail(variant(form), id), "Load failed.");
    }

    @PostMapping("/api/export/{form:pre-invoice|opening-balance}/history") @ResponseBody
    public ResponseEntity<?> history(@PathVariable("form") String form, @RequestBody Map<String, Object> body) {
        return call(() -> svc.history(variant(form), body), "Load failed.");
    }

    @PostMapping("/api/export/{form:pre-invoice|opening-balance}/save") @ResponseBody
    public ResponseEntity<?> save(@PathVariable("form") String form, @RequestBody Map<String, Object> body) {
        return call(() -> svc.save(variant(form), body), "Save failed.");
    }

    @SuppressWarnings("unchecked")
    private static List<Object> castList(Object o) { return (List<Object>) o; }

    // ------------------------------------------------------------------ plumbing (as ExportModuleController)

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
