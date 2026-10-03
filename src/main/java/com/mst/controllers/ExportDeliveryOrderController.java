package com.mst.controllers;

import com.mst.services.ExportDeliveryOrderService;
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

import static com.mst.controllers.ExportForwardingController.call;

/**
 * /export/delivery-order - screen 208 "Export Delivery Order" (ExportDeliveryOrderB, App 8 / Module 11,
 * DocumentTypeId 84). Tenancy and user from the session only.
 */
@Controller
public class ExportDeliveryOrderController {

    private static final String API = "/api/export/delivery-order";

    @Autowired private ExportDeliveryOrderService svc;

    @GetMapping("/export/delivery-order")
    public String page(Model model) {
        model.addAttribute("activeMenu", "export");
        return "export/delivery_order";
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
    public ResponseEntity<?> invoice(@RequestParam("invoiceId") int invoiceId,
                                     @RequestParam(value = "recId", defaultValue = "0") int recId,
                                     @RequestParam(value = "itemId", defaultValue = "0") int itemId) {
        return call(() -> svc.invoice(invoiceId, recId, itemId), "Load failed.");
    }

    @GetMapping(API + "/invoice-weights") @ResponseBody
    public ResponseEntity<?> invoiceWeights(@RequestParam("invoiceId") int invoiceId) { return call(() -> svc.invoiceWeights(invoiceId), "Load failed."); }

    @GetMapping(API + "/containers") @ResponseBody
    public ResponseEntity<?> containers(@RequestParam("invoiceId") int invoiceId, @RequestParam(value = "recId", defaultValue = "0") int recId) {
        return call(() -> svc.containers(invoiceId, recId), "Load failed.");
    }

    @GetMapping(API + "/contract-items") @ResponseBody
    public ResponseEntity<?> contractItems(@RequestParam("invoiceId") int invoiceId, @RequestParam("orderId") int orderId) {
        return call(() -> svc.contractItems(invoiceId, orderId), "Load failed.");
    }

    @GetMapping(API + "/item") @ResponseBody
    public ResponseEntity<?> item(@RequestParam("invoiceDetailId") int invoiceDetailId, @RequestParam("itemId") int itemId,
                                  @RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId) {
        return call(() -> svc.item(invoiceDetailId, itemId, invoiceId), "Load failed.");
    }

    @GetMapping(API + "/sub-lots") @ResponseBody
    public ResponseEntity<?> subLots(@RequestParam("analysisId") int analysisId, @RequestParam(value = "invoiceId", defaultValue = "0") int invoiceId) {
        return call(() -> svc.subLots(analysisId, invoiceId), "Load failed.");
    }

    @GetMapping(API + "/item-uoms") @ResponseBody
    public ResponseEntity<?> itemUoms(@RequestParam("itemId") int itemId) { return call(() -> svc.itemUoms(itemId), "Load failed."); }

    @GetMapping(API + "/cro-id") @ResponseBody
    public ResponseEntity<?> croId(@RequestParam("invoiceId") int invoiceId) { return call(() -> svc.croId(invoiceId), "Load failed."); }

    @GetMapping(API + "/loader/setup") @ResponseBody
    public ResponseEntity<?> loaderSetup() { return call(() -> svc.loaderSetup(), "Error occurred during database call."); }

    @PostMapping(API + "/loader/search") @ResponseBody
    public ResponseEntity<?> loaderSearch(@RequestBody Map<String, Object> body) { return call(() -> svc.loaderData(body), "Load failed."); }

    @GetMapping(API + "/loader/other-items") @ResponseBody
    public ResponseEntity<?> loaderOther(@RequestParam("ids") String ids) { return call(() -> svc.loaderOtherItems(ids), "Load failed."); }

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
