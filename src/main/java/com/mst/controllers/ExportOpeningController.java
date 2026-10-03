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

    private static final String API = "/api/export/export-opening";

    @Autowired private ExportOpeningService svc;

    @GetMapping("/export/export-opening")
    public String page(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/export_opening";
    }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return call(() -> svc.setup(), "Error occurred during database call."); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return call(() -> svc.refresh(), "Load failed."); }

    @GetMapping(API + "/reset") @ResponseBody
    public ResponseEntity<?> reset() { return call(() -> svc.reset(), "Load failed."); }

    @GetMapping(API + "/customer") @ResponseBody
    public ResponseEntity<?> customer(@RequestParam("customerId") int customerId, @RequestParam(value = "itemId", defaultValue = "0") int itemId) {
        return call(() -> svc.customer(customerId, itemId), "Load failed.");
    }

    @GetMapping(API + "/item") @ResponseBody
    public ResponseEntity<?> item(@RequestParam("itemId") int itemId, @RequestParam(value = "customerId", defaultValue = "0") int customerId) {
        return call(() -> svc.item(itemId, customerId), "Load failed.");
    }

    @GetMapping(API + "/fi-balance") @ResponseBody
    public ResponseEntity<?> fiBalance(@RequestParam("documentTypeId") int documentTypeId, @RequestParam("id") int id) {
        return call(() -> svc.fiBalance(documentTypeId, id), "Load failed.");
    }

    @GetMapping(API + "/history-customers") @ResponseBody
    public ResponseEntity<?> historyCustomers() { return call(() -> svc.historyCustomers(), "Load failed."); }

    @PostMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return call(() -> svc.history(body), "Load failed."); }

    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return call(() -> svc.byId(id), "Load failed."); }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return call(() -> svc.save(body), "Save failed."); }

    @GetMapping(API + "/loader/setup") @ResponseBody
    public ResponseEntity<?> loaderSetup() { return call(() -> svc.loaderSetup(), "Load failed."); }

    @PostMapping(API + "/loader/rows") @ResponseBody
    public ResponseEntity<?> loaderRows(@RequestBody Map<String, Object> body) { return call(() -> svc.loaderRows(body), "Load failed."); }

    @GetMapping(API + "/loader/bind") @ResponseBody
    public ResponseEntity<?> loaderBind(@RequestParam("ids") String ids) { return call(() -> svc.loaderBind(ids), "Load failed."); }
}
