package com.mst.controllers;

import com.mst.services.ExportContractIIIService;
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
 * Screen 879 "Export Contract (III)" - frmExportContractIII.cs (Architecture.WinApp.Export), DocumentTypeId 223.
 * Page /export/contract-iii, API /api/export/contract-iii/... . Tenancy and the user come from the session;
 * the service checks the rights of ScreenDefinition 879.
 */
@Controller
public class ExportContractIIIController {

    private static final String API = "/api/export/contract-iii";

    @Autowired private ExportContractIIIService svc;

    @GetMapping("/export/contract-iii")
    public String page(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/contract_iii";
    }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return call(() -> svc.setup(), "Error occurred during database call."); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return call(() -> svc.refresh(), "Load failed."); }

    @GetMapping(API + "/contract-nos") @ResponseBody
    public ResponseEntity<?> contractNos(@RequestParam(value = "contractNo", required = false) String contractNo) {
        return call(() -> svc.contractNosApi(contractNo), "Load failed.");
    }

    @GetMapping(API + "/commodity-remarks") @ResponseBody
    public ResponseEntity<?> commodityRemarks(@RequestParam(value = "itemId", defaultValue = "0") int itemId) {
        return call(() -> svc.commodityRemarksApi(itemId), "Load failed.");
    }

    @GetMapping(API + "/last-saved") @ResponseBody
    public ResponseEntity<?> lastSaved(@RequestParam(value = "documentTypeId", defaultValue = "0") int documentTypeId) {
        return call(() -> svc.lastSavedApi(documentTypeId), "Load failed.");
    }

    @GetMapping(API + "/custom-groups") @ResponseBody
    public ResponseEntity<?> customGroups(@RequestParam(value = "customerId", defaultValue = "0") int customerId) {
        return call(() -> svc.customGroupsApi(customerId), "Load failed.");
    }

    @GetMapping(API + "/history-combos") @ResponseBody
    public ResponseEntity<?> historyCombos() { return call(() -> svc.historyCombos(), "Load failed."); }

    @GetMapping(API + "/uom-for-export") @ResponseBody
    public ResponseEntity<?> uomForExport(@RequestParam("itemId") int itemId) { return call(() -> svc.uomForExport(itemId), "Load failed."); }

    @GetMapping(API + "/global-uoms") @ResponseBody
    public ResponseEntity<?> globalUoms(@RequestParam("itemId") int itemId) { return call(() -> svc.globalUoms(itemId), "Load failed."); }

    @GetMapping(API + "/last-rate") @ResponseBody
    public ResponseEntity<?> lastRate(@RequestParam("itemId") int itemId) { return call(() -> svc.lastRate(itemId), "Load failed."); }

    @GetMapping(API + "/scheduling-policy") @ResponseBody
    public ResponseEntity<?> schedulingPolicy() { return call(() -> svc.schedulingPolicy(), "Load failed."); }

    @GetMapping(API + "/read") @ResponseBody
    public ResponseEntity<?> read(@RequestParam("id") int id) { return call(() -> svc.readById(id), "Load failed."); }

    @PostMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return call(() -> svc.history(body), "Load failed."); }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return call(() -> svc.save(body), "Save failed."); }

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
