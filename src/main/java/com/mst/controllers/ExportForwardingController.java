package com.mst.controllers;

import com.mst.services.ExportForwardingNewService;
import com.mst.services.ExportForwardingService;
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
 * Export forwarding screens (App 8 "Export"):
 *   /export/forwarding       212  EximForwarding     "Export Forwarding"        (module 11, DocumentTypeId 205)
 *   /export/forwarding-new   793  frmForwardingNew   "Export Forwarding (New)"  (module 100, DocumentTypeId 210)
 * No parameter carries tenancy or a user id; the services take them from the session.
 */
@Controller
public class ExportForwardingController {

    private static final String FW = "/api/export/forwarding";
    private static final String FN = "/api/export/forwarding-new";

    @Autowired private ExportForwardingService fw;
    @Autowired private ExportForwardingNewService fn;

    @GetMapping("/export/forwarding")
    public String forwardingPage(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/forwarding";
    }

    @GetMapping("/export/forwarding-new")
    public String forwardingNewPage(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/forwarding_new";
    }

    // ------------------------------------------------------------------ 212

    @GetMapping(FW + "/setup") @ResponseBody
    public ResponseEntity<?> fwSetup() { return call(() -> fw.setup(), "Error occurred during database call."); }

    @GetMapping(FW + "/refresh") @ResponseBody
    public ResponseEntity<?> fwRefresh() { return call(() -> fw.refresh(), "Load failed."); }

    @GetMapping(FW + "/reset") @ResponseBody
    public ResponseEntity<?> fwReset() { return call(() -> fw.reset(), "Load failed."); }

    @GetMapping(FW + "/gate-pass") @ResponseBody
    public ResponseEntity<?> fwGatePass(@RequestParam("gpId") int gpId, @RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId) {
        return call(() -> fw.gatePass(gpId, invoiceId), "Load failed.");
    }

    @GetMapping(FW + "/invoice") @ResponseBody
    public ResponseEntity<?> fwInvoice(@RequestParam("gpId") int gpId, @RequestParam("invoiceId") int invoiceId) {
        return call(() -> fw.invoice(gpId, invoiceId), "Load failed.");
    }

    @GetMapping(FW + "/uoms") @ResponseBody
    public ResponseEntity<?> fwUoms(@RequestParam("itemId") int itemId) { return call(() -> fw.uoms(itemId), "Load failed."); }

    @GetMapping(FW + "/by-id") @ResponseBody
    public ResponseEntity<?> fwById(@RequestParam("id") int id) { return call(() -> fw.readById(id), "Load failed."); }

    @PostMapping(FW + "/history") @ResponseBody
    public ResponseEntity<?> fwHistory(@RequestBody Map<String, Object> body) { return call(() -> fw.history(body), "Load failed."); }

    @GetMapping(FW + "/history-detail") @ResponseBody
    public ResponseEntity<?> fwHistoryDetail(@RequestParam("id") int id) { return call(() -> fw.historyDetail(id), "Load failed."); }

    @GetMapping(FW + "/history-combos") @ResponseBody
    public ResponseEntity<?> fwHistoryCombos() { return call(() -> fw.historyCombosRefresh(), "Load failed."); }

    @PostMapping(FW + "/save") @ResponseBody
    public ResponseEntity<?> fwSave(@RequestBody Map<String, Object> body) { return call(() -> fw.save(body), "Save failed."); }

    @PostMapping(FW + "/delete") @ResponseBody
    public ResponseEntity<?> fwDelete(@RequestBody Map<String, Object> body) { return call(() -> fw.delete(body), "Delete failed."); }

    @GetMapping(FW + "/auto-update-ids") @ResponseBody
    public ResponseEntity<?> fwAutoIds() { return call(() -> fw.autoUpdateIds(), "Load failed."); }

    @PostMapping(FW + "/available-stock") @ResponseBody
    public ResponseEntity<?> fwStock(@RequestBody Map<String, Object> body) { return call(() -> fw.availableStock(body), "Load failed."); }

    // ------------------------------------------------------------------ 793

    @GetMapping(FN + "/setup") @ResponseBody
    public ResponseEntity<?> fnSetup() { return call(() -> fn.setup(), "Error occurred during database call."); }

    @GetMapping(FN + "/refresh") @ResponseBody
    public ResponseEntity<?> fnRefresh() { return call(() -> fn.refresh(), "Load failed."); }

    @GetMapping(FN + "/reset") @ResponseBody
    public ResponseEntity<?> fnReset() { return call(() -> fn.reset(), "Load failed."); }

    @GetMapping(FN + "/gate-pass") @ResponseBody
    public ResponseEntity<?> fnGatePass(@RequestParam("gpId") int gpId) { return call(() -> fn.gatePass(gpId), "Load failed."); }

    @GetMapping(FN + "/invoice") @ResponseBody
    public ResponseEntity<?> fnInvoice(@RequestParam("gpId") int gpId, @RequestParam("invoiceId") int invoiceId) {
        return call(() -> fn.invoice(gpId, invoiceId), "Load failed.");
    }

    @GetMapping(FN + "/uoms") @ResponseBody
    public ResponseEntity<?> fnUoms(@RequestParam("itemId") int itemId) { return call(() -> fn.uoms(itemId), "Load failed."); }

    @GetMapping(FN + "/by-id") @ResponseBody
    public ResponseEntity<?> fnById(@RequestParam("id") int id) { return call(() -> fn.readById(id), "Load failed."); }

    @PostMapping(FN + "/history") @ResponseBody
    public ResponseEntity<?> fnHistory(@RequestBody Map<String, Object> body) { return call(() -> fn.history(body), "Load failed."); }

    @GetMapping(FN + "/history-detail") @ResponseBody
    public ResponseEntity<?> fnHistoryDetail(@RequestParam("id") int id) { return call(() -> fn.historyDetail(id), "Load failed."); }

    @GetMapping(FN + "/history-combos") @ResponseBody
    public ResponseEntity<?> fnHistoryCombos() { return call(() -> fn.historyCombosRefresh(), "Load failed."); }

    @PostMapping(FN + "/save") @ResponseBody
    public ResponseEntity<?> fnSave(@RequestBody Map<String, Object> body) { return call(() -> fn.save(body), "Save failed."); }

    @PostMapping(FN + "/delete") @ResponseBody
    public ResponseEntity<?> fnDelete(@RequestBody Map<String, Object> body) { return call(() -> fn.delete(body), "Delete failed."); }

    @GetMapping(FN + "/auto-update-ids") @ResponseBody
    public ResponseEntity<?> fnAutoIds() { return call(() -> fn.autoUpdateIds(), "Load failed."); }

    // ------------------------------------------------------------------ plumbing

    interface Call { Object run() throws Exception; }

    static ResponseEntity<?> call(Call c, String fallback) {
        try { return ResponseEntity.ok(c.run()); }
        catch (IllegalArgumentException | IllegalStateException e) { return ResponseEntity.badRequest().body(fail(root(e, fallback))); }
        catch (AccessDeniedException e) { return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(root(e, "Access denied."))); }
        catch (Exception e) { return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback))); }
    }

    static String root(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
