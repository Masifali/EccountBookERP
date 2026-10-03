package com.mst.controllers;

import com.mst.services.ExportReturnGrnService;
import com.mst.services.ExportReturnInvoiceService;
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
 * Export return documents (Module 11 "Export"):
 *   /export/return-grn       190  ExportReturn_Grn     "Export Return GRN"      (+ popup frmLoadExportReurnInvoiceForGrn)
 *   /export/return-invoice   191  ExportReturnInvoice  "Export Return Invoice"  (+ popup frmLoadCommercialInvoiceForReturn)
 * No parameter carries tenancy or a user id; the services derive them from the session.
 */
@Controller
public class ExportReturnController {

    private static final String RI = "/api/export/return-invoice";
    private static final String GRN = "/api/export/return-grn";

    @Autowired private ExportReturnInvoiceService ri;
    @Autowired private ExportReturnGrnService grn;

    // ------------------------------------------------------------------ pages

    @GetMapping("/export/return-invoice")
    public String returnInvoicePage(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/return_invoice";
    }

    @GetMapping("/export/return-grn")
    public String returnGrnPage(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/return_grn";
    }

    // ------------------------------------------------------------------ 191 Export Return Invoice

    @GetMapping(RI + "/setup") @ResponseBody
    public ResponseEntity<?> riSetup() { return call(() -> ri.setup(), "Error occurred during database call."); }

    @GetMapping(RI + "/refresh") @ResponseBody
    public ResponseEntity<?> riRefresh() { return call(() -> ri.refresh(), "Load failed."); }

    @GetMapping(RI + "/reset") @ResponseBody
    public ResponseEntity<?> riReset() { return call(() -> ri.reset(), "Load failed."); }

    @GetMapping(RI + "/payment-terms") @ResponseBody
    public ResponseEntity<?> riPaymentTerms(@RequestParam("invoiceId") int invoiceId) { return call(() -> ri.invoicePaymentTerms(invoiceId), "Load failed."); }

    @GetMapping(RI + "/by-id") @ResponseBody
    public ResponseEntity<?> riById(@RequestParam("id") int id, @RequestParam(value = "edit", defaultValue = "false") boolean edit) {
        return call(() -> ri.byId(id, edit), "Load failed.");
    }

    @GetMapping(RI + "/history-combos") @ResponseBody
    public ResponseEntity<?> riHistoryCombos() { return call(() -> ri.historyCombos(), "Load failed."); }

    @PostMapping(RI + "/history") @ResponseBody
    public ResponseEntity<?> riHistory(@RequestBody Map<String, Object> body) { return call(() -> ri.history(body), "Load failed."); }

    @GetMapping(RI + "/voucher-head") @ResponseBody
    public ResponseEntity<?> riVoucherHead(@RequestParam("id") int id) { return call(() -> ri.voucherHead(id), "Load failed."); }

    @PostMapping(RI + "/save") @ResponseBody
    public ResponseEntity<?> riSave(@RequestBody Map<String, Object> body) { return call(() -> ri.save(body), "Save failed."); }

    @PostMapping(RI + "/delete") @ResponseBody
    public ResponseEntity<?> riDelete(@RequestBody Map<String, Object> body) { return call(() -> ri.delete(intOf(body.get("recId"))), "Delete failed."); }

    @GetMapping(RI + "/loader/setup") @ResponseBody
    public ResponseEntity<?> riLoaderSetup() { return call(() -> ri.loaderSetup(), "Error occurred during database call."); }

    @PostMapping(RI + "/loader/rows") @ResponseBody
    public ResponseEntity<?> riLoaderRows(@RequestBody Map<String, Object> body) { return call(() -> ri.loaderRows(body), "Load failed."); }

    // ------------------------------------------------------------------ 190 Export Return GRN

    @GetMapping(GRN + "/setup") @ResponseBody
    public ResponseEntity<?> grnSetup() { return call(() -> grn.setup(), "Error occurred during database call."); }

    @GetMapping(GRN + "/refresh") @ResponseBody
    public ResponseEntity<?> grnRefresh(@RequestParam(value = "recId", defaultValue = "0") int recId) { return call(() -> grn.refresh(recId), "Load failed."); }

    @GetMapping(GRN + "/reset") @ResponseBody
    public ResponseEntity<?> grnReset() { return call(() -> grn.reset(), "Load failed."); }

    @GetMapping(GRN + "/gate-pass") @ResponseBody
    public ResponseEntity<?> grnGatePass(@RequestParam("gpId") int gpId, @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return call(() -> grn.gatePass(gpId, recId), "Load failed.");
    }

    @GetMapping(GRN + "/forwarding") @ResponseBody
    public ResponseEntity<?> grnForwarding(@RequestParam("id") int id) { return call(() -> grn.forwarding(id), "Load failed."); }

    @GetMapping(GRN + "/item-uoms") @ResponseBody
    public ResponseEntity<?> grnItemUoms(@RequestParam("itemId") int itemId) { return call(() -> grn.itemUoms(itemId), "Load failed."); }

    @GetMapping(GRN + "/forwarding-data") @ResponseBody
    public ResponseEntity<?> grnForwardingData(@RequestParam("invoiceId") int invoiceId) { return call(() -> grn.forwardingData(invoiceId), "Load failed."); }

    @GetMapping(GRN + "/by-id") @ResponseBody
    public ResponseEntity<?> grnById(@RequestParam("id") int id) { return call(() -> grn.byId(id), "Load failed."); }

    @GetMapping(GRN + "/edit-data") @ResponseBody
    public ResponseEntity<?> grnEditData(@RequestParam("id") int id,
                                         @RequestParam(value = "forwardingId", defaultValue = "0") int forwardingId,
                                         @RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId) {
        return call(() -> grn.editData(id, forwardingId, invoiceId), "Load failed.");
    }

    @GetMapping(GRN + "/history-combos") @ResponseBody
    public ResponseEntity<?> grnHistoryCombos() { return call(() -> grn.historyCombos(), "Load failed."); }

    @PostMapping(GRN + "/history") @ResponseBody
    public ResponseEntity<?> grnHistory(@RequestBody Map<String, Object> body) { return call(() -> grn.history(body), "Load failed."); }

    @PostMapping(GRN + "/save") @ResponseBody
    public ResponseEntity<?> grnSave(@RequestBody Map<String, Object> body) { return call(() -> grn.save(body), "Save failed."); }

    @PostMapping(GRN + "/delete") @ResponseBody
    public ResponseEntity<?> grnDelete(@RequestBody Map<String, Object> body) { return call(() -> grn.delete(intOf(body.get("recId"))), "Delete failed."); }

    @GetMapping(GRN + "/loader/setup") @ResponseBody
    public ResponseEntity<?> grnLoaderSetup() { return call(() -> grn.loaderSetup(), "Error occurred during database call."); }

    @PostMapping(GRN + "/loader/rows") @ResponseBody
    public ResponseEntity<?> grnLoaderRows(@RequestBody Map<String, Object> body) { return call(() -> grn.loaderRows(body), "Load failed."); }

    // ------------------------------------------------------------------ plumbing

    interface Call { Object run() throws Exception; }

    private static int intOf(Object v) {
        if (v == null) return 0;
        try { return (int) Double.parseDouble(String.valueOf(v)); } catch (NumberFormatException e) { return 0; }
    }

    /** 400 for the form's own validation text, 403 for a missing right, 500 with the innermost message otherwise. */
    static ResponseEntity<?> call(Call c, String fallback) {
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
