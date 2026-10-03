package com.mst.controllers;

import com.mst.services.ExportCommercialInvoiceIIIService;
import com.mst.services.ExportCommercialInvoiceService;
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
 * The two DocumentTypeId 204 commercial-invoice screens of the Export application (App 8, module 11):
 *
 *   /export/commercial-invoice       211  ExImCommercialInvoice   "Export Commercial Invoice"
 *   /export/commercial-invoice-iii   880  frmCommercialInvoiceIII "Commercial Invoice III"
 *
 * No parameter carries tenancy or a user id; the services derive them from the session.
 */
@Controller
public class ExportCommercialInvoiceController {

    private static final String CI = "/api/export/commercial-invoice";
    private static final String C3 = "/api/export/commercial-invoice-iii";

    @Autowired private ExportCommercialInvoiceService ci;
    @Autowired private ExportCommercialInvoiceIIIService c3;

    // ------------------------------------------------------------------ pages

    @GetMapping("/export/commercial-invoice")
    public String commercialInvoicePage(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/commercial_invoice";
    }

    @GetMapping("/export/commercial-invoice-iii")
    public String commercialInvoiceIIIPage(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/commercial_invoice_iii";
    }

    // ------------------------------------------------------------------ 211 Export Commercial Invoice

    @GetMapping(CI + "/setup") @ResponseBody
    public ResponseEntity<?> ciSetup() { return call(() -> ci.setup(), "Error occurred during database call."); }

    @GetMapping(CI + "/refresh") @ResponseBody
    public ResponseEntity<?> ciRefresh() { return call(() -> ci.refresh(), "Load failed."); }

    @GetMapping(CI + "/new") @ResponseBody
    public ResponseEntity<?> ciNew() { return call(() -> ci.newForm(), "Load failed."); }

    @GetMapping(CI + "/customer-leave") @ResponseBody
    public ResponseEntity<?> ciCustomer(@RequestParam(value = "customerId", defaultValue = "0") int customerId,
                                        @RequestParam(value = "itemId", defaultValue = "0") int itemId) {
        return call(() -> ci.customerLeave(customerId, itemId), "Load failed.");
    }

    @GetMapping(CI + "/item-leave") @ResponseBody
    public ResponseEntity<?> ciItem(@RequestParam(value = "itemId", defaultValue = "0") int itemId,
                                    @RequestParam(value = "customerId", defaultValue = "0") int customerId) {
        return call(() -> ci.itemLeave(itemId, customerId), "Load failed.");
    }

    @GetMapping(CI + "/contract-schedules") @ResponseBody
    public ResponseEntity<?> ciSchedules(@RequestParam(value = "contractId", defaultValue = "0") int contractId,
                                         @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return call(() -> ci.contractSchedules(contractId, recId), "Load failed.");
    }

    @GetMapping(CI + "/fi-balance") @ResponseBody
    public ResponseEntity<?> ciFiBalance(@RequestParam("id") int id, @RequestParam(value = "documentTypeId", defaultValue = "0") int documentTypeId) {
        return call(() -> ci.fiBalance(id, documentTypeId), "Load failed.");
    }

    @GetMapping(CI + "/history-combos") @ResponseBody
    public ResponseEntity<?> ciHistoryCombos() { return call(() -> ci.historyCombos(), "Load failed."); }

    @PostMapping(CI + "/history") @ResponseBody
    public ResponseEntity<?> ciHistory(@RequestBody Map<String, Object> body) { return call(() -> ci.history(body), "Load failed."); }

    @GetMapping(CI + "/history-detail") @ResponseBody
    public ResponseEntity<?> ciHistoryDetail(@RequestParam("id") int id) { return call(() -> ci.historyDetail(id), "Load failed."); }

    @GetMapping(CI + "/by-id") @ResponseBody
    public ResponseEntity<?> ciById(@RequestParam("id") int id) { return call(() -> ci.readById(id), "Load failed."); }

    @GetMapping(CI + "/loader/setup") @ResponseBody
    public ResponseEntity<?> ciLoaderSetup() { return call(() -> ci.loaderSetup(), "Load failed."); }

    @PostMapping(CI + "/loader/search") @ResponseBody
    public ResponseEntity<?> ciLoader(@RequestBody Map<String, Object> body) { return call(() -> ci.loader(body), "Load failed."); }

    @GetMapping(CI + "/loader/apply") @ResponseBody
    public ResponseEntity<?> ciLoaderApply(@RequestParam("ids") String ids) { return call(() -> ci.loaderApply(ids), "Load failed."); }

    @PostMapping(CI + "/save") @ResponseBody
    public ResponseEntity<?> ciSave(@RequestBody Map<String, Object> body) { return call(() -> ci.save(body), "Save failed."); }

    // ------------------------------------------------------------------ 880 Commercial Invoice III

    @GetMapping(C3 + "/setup") @ResponseBody
    public ResponseEntity<?> c3Setup() { return call(() -> c3.setup(), "Error occurred during database call."); }

    @GetMapping(C3 + "/refresh") @ResponseBody
    public ResponseEntity<?> c3Refresh(@RequestParam(value = "recId", defaultValue = "0") int recId) { return call(() -> c3.refresh(recId), "Load failed."); }

    @GetMapping(C3 + "/new") @ResponseBody
    public ResponseEntity<?> c3New() { return call(() -> c3.newForm(), "Load failed."); }

    @GetMapping(C3 + "/customer-leave") @ResponseBody
    public ResponseEntity<?> c3Customer(@RequestParam(value = "customerId", defaultValue = "0") int customerId) {
        return call(() -> c3.customerLeave(customerId), "Load failed.");
    }

    @GetMapping(C3 + "/fi-balance") @ResponseBody
    public ResponseEntity<?> c3FiBalance(@RequestParam("id") int id, @RequestParam(value = "documentTypeId", defaultValue = "0") int documentTypeId) {
        return call(() -> c3.fiBalance(id, documentTypeId), "Load failed.");
    }

    @GetMapping(C3 + "/uoms") @ResponseBody
    public ResponseEntity<?> c3Uoms(@RequestParam("itemId") int itemId) { return call(() -> c3.uoms(itemId), "Load failed."); }

    @GetMapping(C3 + "/commodity") @ResponseBody
    public ResponseEntity<?> c3Commodity(@RequestParam("itemId") int itemId) { return call(() -> c3.commodity(itemId), "Load failed."); }

    @GetMapping(C3 + "/third-party") @ResponseBody
    public ResponseEntity<?> c3ThirdParty(@RequestParam("itemId") int itemId) { return call(() -> c3.thirdParty(itemId), "Load failed."); }

    @GetMapping(C3 + "/history-combos") @ResponseBody
    public ResponseEntity<?> c3HistoryCombos() { return call(() -> c3.historyCombos(), "Load failed."); }

    @PostMapping(C3 + "/history") @ResponseBody
    public ResponseEntity<?> c3History(@RequestBody Map<String, Object> body) { return call(() -> c3.history(body), "Load failed."); }

    @GetMapping(C3 + "/history-detail") @ResponseBody
    public ResponseEntity<?> c3HistoryDetail(@RequestParam("id") int id) { return call(() -> c3.historyDetail(id), "Load failed."); }

    @GetMapping(C3 + "/by-id") @ResponseBody
    public ResponseEntity<?> c3ById(@RequestParam("id") int id) { return call(() -> c3.readById(id), "Load failed."); }

    @GetMapping(C3 + "/loader/setup") @ResponseBody
    public ResponseEntity<?> c3LoaderSetup() { return call(() -> c3.loaderSetup(), "Load failed."); }

    @PostMapping(C3 + "/loader/search") @ResponseBody
    public ResponseEntity<?> c3Loader(@RequestBody Map<String, Object> body) { return call(() -> c3.loader(body), "Load failed."); }

    @GetMapping(C3 + "/loader/apply") @ResponseBody
    public ResponseEntity<?> c3LoaderApply(@RequestParam("ids") String ids) { return call(() -> c3.loaderApply(ids), "Load failed."); }

    @PostMapping(C3 + "/save") @ResponseBody
    public ResponseEntity<?> c3Save(@RequestBody Map<String, Object> body) { return call(() -> c3.save(body), "Save failed."); }

    // ------------------------------------------------------------------ plumbing

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
