package com.mst.controllers;

import com.mst.services.ExportPartyDefinesService;
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
 * 206 "Export Parties Define" - Architecture.WinApp.Export.ExportPartyDefines.
 * Page /export/parties-define, API /api/export/parties-define/... No parameter carries tenancy or a user id.
 */
@Controller
public class ExportPartyDefinesController {

    private static final String API = "/api/export/parties-define";

    @Autowired private ExportPartyDefinesService svc;

    @GetMapping("/export/parties-define")
    public String page(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/parties_define";
    }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return call(() -> svc.setup(), "Error occurred during database call."); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return call(() -> svc.refresh(), "Load failed."); }

    @GetMapping(API + "/gl-accounts") @ResponseBody
    public ResponseEntity<?> glAccounts(@RequestParam(value = "recId", defaultValue = "0") int recId) {
        return call(() -> svc.glAccounts(recId), "Load failed.");
    }

    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return call(() -> svc.readById(id), "Load failed."); }

    @GetMapping(API + "/gl-leave") @ResponseBody
    public ResponseEntity<?> glLeave(@RequestParam("glAccountId") int glAccountId) {
        return call(() -> svc.glAccountLeave(glAccountId), "Id Not Found");
    }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return call(() -> svc.saveParty(body), "Save failed."); }

    @PostMapping(API + "/update-row") @ResponseBody
    public ResponseEntity<?> updateRow(@RequestBody Map<String, Object> body) { return call(() -> svc.updatePartyRow(body), "Update failed."); }

    @PostMapping(API + "/update-rows") @ResponseBody
    public ResponseEntity<?> updateRows(@RequestBody Map<String, Object> body) { return call(() -> svc.updatePartyRows(body), "Update failed."); }

    @GetMapping(API + "/consignee/by-id") @ResponseBody
    public ResponseEntity<?> consigneeById(@RequestParam("id") int id,
                                           @RequestParam(value = "mainRecId", defaultValue = "0") int mainRecId) {
        return call(() -> svc.readConsigneeById(id, mainRecId), "Load failed.");
    }

    @PostMapping(API + "/consignee/save") @ResponseBody
    public ResponseEntity<?> consigneeSave(@RequestBody Map<String, Object> body) { return call(() -> svc.saveConsignee(body), "Save failed."); }

    @PostMapping(API + "/consignee/update-row") @ResponseBody
    public ResponseEntity<?> consigneeUpdateRow(@RequestBody Map<String, Object> body) { return call(() -> svc.updateConsigneeRow(body), "Update failed."); }

    @PostMapping(API + "/consignee/update-rows") @ResponseBody
    public ResponseEntity<?> consigneeUpdateRows(@RequestBody Map<String, Object> body) { return call(() -> svc.updateConsigneeRows(body), "Update failed."); }

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
