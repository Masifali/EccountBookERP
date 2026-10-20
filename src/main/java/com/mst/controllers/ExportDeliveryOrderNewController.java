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


    @Autowired private ExportDeliveryOrderNewService svc;

    @GetMapping("/export/delivery-order-new")
    public String page(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/delivery_order_new";
    }

    @GetMapping("/api/export/delivery-order-new/setup") @ResponseBody
    public ResponseEntity<?> setup() { return call(() -> svc.setup(), "Error occurred during database call."); }

    @GetMapping("/api/export/delivery-order-new/refresh") @ResponseBody
    public ResponseEntity<?> refresh(@RequestParam(value = "recId", defaultValue = "0") int recId) { return call(() -> svc.refresh(recId), "Load failed."); }

    @GetMapping("/api/export/delivery-order-new/reset") @ResponseBody
    public ResponseEntity<?> reset() { return call(() -> svc.reset(), "Load failed."); }

    @GetMapping("/api/export/delivery-order-new/invoices") @ResponseBody
    public ResponseEntity<?> invoices(@RequestParam(value = "recId", defaultValue = "0") int recId) { return call(() -> svc.invoices(recId), "Load failed."); }

    @GetMapping("/api/export/delivery-order-new/invoice") @ResponseBody
    public ResponseEntity<?> invoice(@RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId,
                                     @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return call(() -> svc.invoice(invoiceId, recId), "Load failed.");
    }

    @GetMapping("/api/export/delivery-order-new/invoice-weights") @ResponseBody
    public ResponseEntity<?> invoiceWeights(@RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId) {
        return call(() -> svc.invoiceWeights(invoiceId), "Load failed.");
    }

    @GetMapping("/api/export/delivery-order-new/contract-items") @ResponseBody
    public ResponseEntity<?> contractItems(@RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId,
                                           @RequestParam(value = "orderId", defaultValue = "0") int orderId) {
        return call(() -> svc.contractItems(invoiceId, orderId), "Load failed.");
    }

    @GetMapping("/api/export/delivery-order-new/item-detail") @ResponseBody
    public ResponseEntity<?> itemDetail(@RequestParam(value = "invoiceDetailId", defaultValue = "0") int invoiceDetailId) {
        return call(() -> svc.itemDetail(invoiceDetailId), "Load failed.");
    }

    @GetMapping("/api/export/delivery-order-new/containers") @ResponseBody
    public ResponseEntity<?> containers(@RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId,
                                        @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return call(() -> svc.containers(invoiceId, recId), "Load failed.");
    }

    @GetMapping("/api/export/delivery-order-new/by-id") @ResponseBody
    public ResponseEntity<?> byId(@RequestParam("id") int id) { return call(() -> svc.readById(id), "Load failed."); }

    @PostMapping("/api/export/delivery-order-new/history") @ResponseBody
    public ResponseEntity<?> history(@RequestBody Map<String, Object> body) { return call(() -> svc.history(body), "Load failed."); }

    @GetMapping("/api/export/delivery-order-new/history-detail") @ResponseBody
    public ResponseEntity<?> historyDetail(@RequestParam("id") int id) { return call(() -> svc.historyDetail(id), "Load failed."); }

    @GetMapping("/api/export/delivery-order-new/history-combos") @ResponseBody
    public ResponseEntity<?> historyCombos() { return call(() -> svc.historyCombosRefresh(), "Load failed."); }

    @PostMapping("/api/export/delivery-order-new/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return call(() -> svc.save(body), "Save failed."); }
}
