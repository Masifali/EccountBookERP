package com.mst.controllers;

import com.mst.services.ExportDeliveryOrderNewService;
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
 * /export/delivery-order-new - screen 201 "Export Delivery Order" (Architecture.WinApp.Export.ExportDeliveryOrderNew,
 * App 8 / Module 100, DocumentTypeId 84, DeliveryOrderType "Export"). Tenancy and user from the session only.
 */
@Controller
public class ExportDeliveryOrderNewController {

    private static final String API = "/api/export/delivery-order-new";

    @Autowired private ExportDeliveryOrderNewService svc;

    @GetMapping("/export/delivery-order-new")
    public String page(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/delivery_order_new";
    }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return call(() -> svc.setup(), "Error occurred during database call."); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh(@RequestParam(value = "recId", defaultValue = "0") int recId) { return call(() -> svc.refresh(recId), "Load failed."); }

    @GetMapping(API + "/reset") @ResponseBody
    public ResponseEntity<?> reset() { return call(() -> svc.reset(), "Load failed."); }

    @GetMapping(API + "/invoices") @ResponseBody
    public ResponseEntity<?> invoices(@RequestParam(value = "recId", defaultValue = "0") int recId) { return call(() -> svc.invoices(recId), "Load failed."); }

    @GetMapping(API + "/invoice") @ResponseBody
    public ResponseEntity<?> invoice(@RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId,
                                     @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return call(() -> svc.invoice(invoiceId, recId), "Load failed.");
    }

    @GetMapping(API + "/invoice-weights") @ResponseBody
    public ResponseEntity<?> invoiceWeights(@RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId) {
        return call(() -> svc.invoiceWeights(invoiceId), "Load failed.");
    }

    @GetMapping(API + "/contract-items") @ResponseBody
    public ResponseEntity<?> contractItems(@RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId,
                                           @RequestParam(value = "orderId", defaultValue = "0") int orderId) {
        return call(() -> svc.contractItems(invoiceId, orderId), "Load failed.");
    }

    @GetMapping(API + "/item-detail") @ResponseBody
    public ResponseEntity<?> itemDetail(@RequestParam(value = "invoiceDetailId", defaultValue = "0") int invoiceDetailId) {
        return call(() -> svc.itemDetail(invoiceDetailId), "Load failed.");
    }

    @GetMapping(API + "/containers") @ResponseBody
    public ResponseEntity<?> containers(@RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId,
                                        @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return call(() -> svc.containers(invoiceId, recId), "Load failed.");
    }

    @GetMapping(API + "/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return call(() -> svc.readById(id), "Load failed."); }

    @PostMapping(API + "/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return call(() -> svc.history(body), "Load failed."); }

    @GetMapping(API + "/history-detail") @ResponseBody
    public ResponseEntity<?> historyDetail(@RequestParam("id") int id) { return call(() -> svc.historyDetail(id), "Load failed."); }

    @GetMapping(API + "/history-combos") @ResponseBody
    public ResponseEntity<?> historyCombos() { return call(() -> svc.historyCombosRefresh(), "Load failed."); }

    @PostMapping(API + "/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return call(() -> svc.save(body), "Save failed."); }
}
