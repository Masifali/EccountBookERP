package com.mst.controllers;

import com.mst.services.ExportOpeningService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

import static com.mst.controllers.ExportReturnController.call;

/**
 * /export/export-opening - screen 189 "Export Opening Balance" (Architecture.WinApp.Export.ExportOpening, the
 * "Commercial Invoice Opening" form, DocumentTypeId 212) with the popup LoadSalesContractForExportOpening.
 * Tenancy and user from the session only.
 */
@Controller
public class ExportOpeningController {


    @Autowired private ExportOpeningService svc;

    @GetMapping("/export/export-opening")
    public String page(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/export_opening";
    }

    @GetMapping("/api/export/export-opening/setup") @ResponseBody
    public ResponseEntity<?> setup() { return call(() -> svc.setup(), "Error occurred during database call."); }

    @GetMapping("/api/export/export-opening/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return call(() -> svc.refresh(), "Load failed."); }

    @GetMapping("/api/export/export-opening/reset") @ResponseBody
    public ResponseEntity<?> reset() { return call(() -> svc.reset(), "Load failed."); }

    @GetMapping("/api/export/export-opening/customer") @ResponseBody
    public ResponseEntity<?> customer(@RequestParam("customerId") int customerId, @RequestParam(value = "itemId", defaultValue = "0") int itemId) {
        return call(() -> svc.customer(customerId, itemId), "Load failed.");
    }

    @GetMapping("/api/export/export-opening/item") @ResponseBody
    public ResponseEntity<?> item(@RequestParam("itemId") int itemId, @RequestParam(value = "customerId", defaultValue = "0") int customerId) {
        return call(() -> svc.item(itemId, customerId), "Load failed.");
    }

    @GetMapping("/api/export/export-opening/fi-balance") @ResponseBody
    public ResponseEntity<?> fiBalance(@RequestParam("documentTypeId") int documentTypeId, @RequestParam("id") int id) {
        return call(() -> svc.fiBalance(documentTypeId, id), "Load failed.");
    }

    @GetMapping("/api/export/export-opening/history-customers") @ResponseBody
    public ResponseEntity<?> historyCustomers() { return call(() -> svc.historyCustomers(), "Load failed."); }

    @PostMapping("/api/export/export-opening/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return call(() -> svc.history(body), "Load failed."); }

    @GetMapping("/api/export/export-opening/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return call(() -> svc.byId(id), "Load failed."); }

    @PostMapping("/api/export/export-opening/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return call(() -> svc.save(body), "Save failed."); }

    @GetMapping("/api/export/export-opening/loader/setup") @ResponseBody
    public ResponseEntity<?> loaderSetup() { return call(() -> svc.loaderSetup(), "Load failed."); }

    @PostMapping("/api/export/export-opening/loader/rows") @ResponseBody
    public ResponseEntity<?> loaderRows(@RequestBody Map<String, Object> body) { return call(() -> svc.loaderRows(body), "Load failed."); }

    @GetMapping("/api/export/export-opening/loader/bind") @ResponseBody
    public ResponseEntity<?> loaderBind(@RequestParam("ids") String ids) { return call(() -> svc.loaderBind(ids), "Load failed."); }
}
