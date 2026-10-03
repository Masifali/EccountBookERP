package com.mst.controllers;

import com.mst.services.ExportCommercialInvoiceTransferService;
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

import static com.mst.controllers.ExportPerformaInvoiceController.call;

/**
 * /export/commercial-invoice-transfer - screen 202 "Commercial Invoice" (Architecture.WinApp.Export
 * .CommiercialInvoiceAgainstPreInvoiceTransfer, App 8 / Module 100, ExImInvoice DocumentTypeId 211) with its loader
 * popup LoadForwardingForCommercialInvoice. Tenancy, user, branch and financial year from the session only.
 */
@Controller
public class ExportCommercialInvoiceTransferController {

    private static final String API = "/api/export/commercial-invoice-transfer";

    @Autowired private ExportCommercialInvoiceTransferService svc;

    @GetMapping("/export/commercial-invoice-transfer")
    public String page(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/commercial_invoice_transfer";
    }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return call(() -> svc.setup(), "Error occurred during database call."); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return call(() -> svc.refresh(), "Load failed."); }

    @GetMapping(API + "/new-code") @ResponseBody
    public ResponseEntity<?> newCode() { return call(() -> svc.newCode(), "Load failed."); }

    @GetMapping(API + "/invoice-nos") @ResponseBody
    public ResponseEntity<?> invoiceNos(@RequestParam(value = "invoiceNo", required = false) String invoiceNo) {
        return call(() -> svc.finalInvoiceNos(invoiceNo), "Load failed.");
    }

    @GetMapping(API + "/customer-leave") @ResponseBody
    public ResponseEntity<?> customerLeave(@RequestParam(value = "customerId", defaultValue = "0") int customerId,
                                           @RequestParam(value = "preInvoiceId", defaultValue = "0") int preInvoiceId) {
        return call(() -> svc.customerLeave(customerId, preInvoiceId), "Load failed.");
    }

    @GetMapping(API + "/fis") @ResponseBody
    public ResponseEntity<?> fis() { return call(() -> svc.fis(), "Load failed."); }

    @GetMapping(API + "/uoms") @ResponseBody
    public ResponseEntity<?> uoms(@RequestParam(value = "itemId", defaultValue = "0") int itemId) { return call(() -> svc.uoms(itemId), "Load failed."); }

    @GetMapping(API + "/loader/setup") @ResponseBody
    public ResponseEntity<?> loaderSetup() { return call(() -> svc.loaderSetup(), "Error occurred during database call."); }

    @PostMapping(API + "/loader/search") @ResponseBody
    public ResponseEntity<?> loaderSearch(@RequestBody Map<String, Object> body) { return call(() -> svc.loaderSearch(body), "Load failed."); }

    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return call(() -> svc.readById(id), "Load failed."); }

    @PostMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return call(() -> svc.history(body), "Load failed."); }

    @GetMapping(API + "/history-detail") @ResponseBody
    public ResponseEntity<?> historyDetail(@RequestParam("id") int id) { return call(() -> svc.historyDetail(id), "Load failed."); }

    @GetMapping(API + "/history-combos") @ResponseBody
    public ResponseEntity<?> historyCombos() { return call(() -> svc.historyCombos(), "Load failed."); }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return call(() -> svc.save(body), "Save failed."); }
}
