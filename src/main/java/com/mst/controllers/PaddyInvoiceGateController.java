package com.mst.controllers;

import com.mst.services.PaddyInvoiceService;
import java.util.List;
import java.util.Map;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

/** Screen 466 frmPurchaseInvoiceForGate (Paddy Purchase, module 48; also Supplier Portal 892): page /purchase/purchase-invoice-for-gate, API /api/paddy-purchase/invoice-gate/*. */
@Controller
public class PaddyInvoiceGateController {
    @GetMapping("/purchase/purchase-invoice-for-gate")
    public String page(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Invoice For Gate");
        return "purchase/paddy_invoice_gate";
    }

    @RestController
    @RequestMapping("/api/paddy-purchase/invoice-gate")
    public static class Api implements PaddyPurchaseApi {
        @Autowired private PaddyInvoiceService service;
        private static final PaddyInvoiceService.Profile P = PaddyInvoiceService.GATE;

        @GetMapping("/init") public Map<String, Object> init() { return service.init(P); }
        @GetMapping("/lookups") public Map<String, Object> lookups() { return service.lookups(); }
        @GetMapping("/doc-no") public Map<String, Object> docNo() { return Map.of("docNo", service.docNo(P)); }
        @GetMapping("/popup/lookups") public Map<String, Object> popupLookups() { return service.popupLookups(); }
        @GetMapping("/popup/year-start") public Map<String, Object> yearStart() { return service.startYear(); }
        @GetMapping("/popup/orders") public List<Map<String, Object>> orders(@RequestParam int supplierId) { return service.popupOrders(supplierId); }
        @GetMapping("/popup/pending") public List<Map<String, Object>> pending(@RequestParam(defaultValue = "0") int orderId, @RequestParam(required = false) String from, @RequestParam(required = false) String to) { return service.pendingGrns(P, orderId, from, to); }
        @GetMapping("/load-grn") public List<Map<String, Object>> loadGrn(@RequestParam int grnId, @RequestParam(defaultValue = "0") int orderId) { return service.loadGrn(grnId, orderId); }
        @GetMapping("/load-extras") public Map<String, Object> loadExtras(@RequestParam String ids, @RequestParam(defaultValue = "0") int orderId) { return service.loadExtras(ids, orderId); }
        @GetMapping("/uoms") public List<Map<String, Object>> uoms(@RequestParam int itemId) { return service.uoms(itemId); }
        @GetMapping("/history") public List<Map<String, Object>> history(@RequestParam(defaultValue = "0") int noOfRecords) { return service.history(P, noOfRecords); }
        @GetMapping("/history-detail/{id:[0-9]+}") public List<Map<String, Object>> historyDetail(@PathVariable int id) { return service.historyDetail(P, id); }
        @GetMapping("/{id:[0-9]+}") public Map<String, Object> byId(@PathVariable int id) { return service.getById(P, id); }
        @GetMapping("/{id:[0-9]+}/attachments") public List<Map<String, Object>> attachments(@PathVariable int id) { return service.attachments(P, id); }
        @PostMapping("/save") public Map<String, Object> save(@RequestBody Map<String, Object> body) { return service.save(P, body); }
    }
}
