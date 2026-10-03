package com.mst.controllers;

import com.mst.services.ExportPackingDetailService;
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
 * 193 "Packing Detail" - Architecture.WinApp.Export.PackingDetailForCommercialInvoice (Export, ModuleId 100).
 * Page /export/packing-detail, API /api/export/packing-detail/... . Tenancy and user come from the session
 * (ExportPackingDetailService), never from a request parameter.
 */
@Controller
public class ExportPackingDetailController {

    private static final String API = "/api/export/packing-detail";

    @Autowired private ExportPackingDetailService svc;

    @GetMapping("/export/packing-detail")
    public String page(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/packing_detail";
    }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return call(() -> svc.setup(), "Error occurred during database call."); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return call(() -> svc.refresh(), "Load failed."); }

    @GetMapping(API + "/invoice") @ResponseBody
    public ResponseEntity<?> invoice(@RequestParam("id") int id) { return call(() -> svc.invoiceData(id), "Load failed."); }

    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return call(() -> svc.readById(id), "Load failed."); }

    @GetMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(value = "noOfRecords", defaultValue = "0") int noOfRecords) {
        return call(() -> svc.history(noOfRecords), "Load failed.");
    }

    @GetMapping(API + "/history-detail") @ResponseBody
    public ResponseEntity<?> historyDetail(@RequestParam("id") int id) { return call(() -> svc.historyDetail(id), "Load failed."); }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return call(() -> svc.save(body), "Save failed."); }

    @PostMapping(API + "/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestBody Map<String, Object> body) {
        Object v = body.get("recId");
        int id;
        try { id = v == null ? 0 : (int) Double.parseDouble(String.valueOf(v)); } catch (NumberFormatException e) { id = 0; }
        final int recId = id;
        return call(() -> svc.delete(recId), "Delete failed.");
    }

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
