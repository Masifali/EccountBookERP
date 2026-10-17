package com.mst.controllers;

import com.mst.services.W3BWorkOrderService;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Mfg Production hub (module 103) - screen 593 "Work Order" (Architecture.WinApp.Mfg.frmWorkOrder, DocumentTypeId 1614).
 * Page /production/mfg/work-order, JSON under /production/mfg/work-order/api. Notes: {@link W3BWorkOrderService}.
 */
@Controller
@RequestMapping("/production/mfg/work-order")
public class W3BWorkOrderController {

    private final W3BWorkOrderService service;
    public W3BWorkOrderController(W3BWorkOrderService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Work Order");
        return "production/mfg/w3b_work_order";
    }

    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return W3BApi.run(service::lookups, "Could not load the screen."); }

    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return W3BApi.run(service::refresh, "Refresh failed."); }

    @GetMapping("/api/doc-no") @ResponseBody
    public ResponseEntity<?> docNo() { return W3BApi.run(() -> Map.of("docNo", service.docNo()), "Could not generate the document number."); }

    @GetMapping("/api/item-info") @ResponseBody
    public ResponseEntity<?> itemInfo(@RequestParam int itemId) {
        return W3BApi.run(() -> {
            Map<String, Object> m = new java.util.LinkedHashMap<>(service.saleOrders(itemId));
            m.put("boms", service.bomNames(itemId));
            return m;
        }, "Could not load the item.");
    }

    @GetMapping("/api/bom-operations") @ResponseBody
    public ResponseEntity<?> bomOperations(@RequestParam int bomId) {
        return W3BApi.run(() -> service.bomOperations(bomId), "Could not load the BOM operations.");
    }

    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int workOrderId, @RequestParam(defaultValue = "0") int itemId) {
        return W3BApi.run(() -> service.history(fromDate, toDate, fromDocNo, toDocNo, workOrderId, itemId), "History failed.");
    }

    @GetMapping("/api/read/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> read(@PathVariable int id) { return W3BApi.run(() -> service.load(id), "Could not open that work order."); }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return W3BApi.run(() -> service.save(body), "Save failed."); }

    @PostMapping("/api/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam int id) { return W3BApi.run(() -> service.delete(id), "Delete failed."); }
}
