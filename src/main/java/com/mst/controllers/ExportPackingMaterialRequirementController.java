package com.mst.controllers;

import com.mst.services.ExportPackingMaterialRequirementService;
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

/** /export/packing-material-requirement - 219 PackingMaterialRequirement "Packing Material Requirement (Not Use)". */
@Controller
public class ExportPackingMaterialRequirementController {

    private static final String API = "/api/export/packing-material-requirement";

    @Autowired private ExportPackingMaterialRequirementService svc;

    @GetMapping("/export/packing-material-requirement")
    public String page(Model model) { model.addAttribute("activeMenu", "export"); return "export/packing_material_requirement"; }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return call(svc::setup, "Error occurred during database call."); }

    @GetMapping(API + "/sales-contracts") @ResponseBody
    public ResponseEntity<?> contracts(@RequestParam(value = "supplierCustomerId", defaultValue = "0") int supplierCustomerId) {
        return call(() -> svc.salesContracts(supplierCustomerId), "Load failed.");
    }

    @GetMapping(API + "/contract-items") @ResponseBody
    public ResponseEntity<?> items(@RequestParam(value = "contractId", defaultValue = "0") int contractId) {
        return call(() -> svc.contractItems(contractId), "Load failed.");
    }

    @GetMapping(API + "/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam(value = "itemId", defaultValue = "0") int itemId) { return call(() -> svc.uoms(itemId), "Load failed."); }

    @GetMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history() { return call(svc::history, "Load failed."); }

    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return call(() -> svc.readById(id), "Load failed."); }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return call(() -> svc.save(body), "Save failed."); }

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
