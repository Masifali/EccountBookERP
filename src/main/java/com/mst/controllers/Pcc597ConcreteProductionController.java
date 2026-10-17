package com.mst.controllers;

import com.mst.services.Pcc597ConcreteProductionService;
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
 * Production hub screen 597 "Concrete Production" (Architecture.WinApp.Pcc.frmConcreteProduction, DocumentTypeId 1850).
 * Page /production/pcc/concrete-production, JSON under /production/pcc/concrete-production/api. Notes: {@link Pcc597ConcreteProductionService}.
 */
@Controller
@RequestMapping("/production/pcc/concrete-production")
public class Pcc597ConcreteProductionController {

    private final Pcc597ConcreteProductionService service;
    public Pcc597ConcreteProductionController(Pcc597ConcreteProductionService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Concrete Production");
        return "production/pcc597_concrete_production";
    }

    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return W3BApi.run(service::lookups, "Could not load the screen."); }

    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return W3BApi.run(service::refresh, "Refresh failed."); }

    @GetMapping("/api/doc-no") @ResponseBody
    public ResponseEntity<?> docNo() { return W3BApi.run(() -> Map.of("docNo", service.docNo()), "Could not generate the document number."); }

    @GetMapping("/api/production-nos") @ResponseBody
    public ResponseEntity<?> productionNos() { return W3BApi.run(service::productionNos, "Could not load the production numbers."); }

    @GetMapping("/api/bom-items") @ResponseBody
    public ResponseEntity<?> bomItems(@RequestParam(defaultValue = "0") int typeId) { return W3BApi.run(() -> service.bomItems(typeId), "Could not load the BOM items."); }

    @GetMapping("/api/bom-nos") @ResponseBody
    public ResponseEntity<?> bomNos() { return W3BApi.run(service::bomNos, "Could not load the BOM list."); }

    @GetMapping("/api/bom-load") @ResponseBody
    public ResponseEntity<?> bomLoad(@RequestParam int bomId, @RequestParam(defaultValue = "0") int plantId, @RequestParam(defaultValue = "0") int outItemId,
                                     @RequestParam(required = false) String outItemName) {
        return W3BApi.run(() -> service.bomLoad(bomId, plantId, outItemId, outItemName), "Could not load the Bill of Material.");
    }

    @GetMapping("/api/attributes") @ResponseBody
    public ResponseEntity<?> attributes(@RequestParam int itemId) { return W3BApi.run(() -> service.attributes(itemId), "Could not load the item variants."); }

    @GetMapping("/api/plant-warehouses") @ResponseBody
    public ResponseEntity<?> plantWarehouses(@RequestParam int plantId) { return W3BApi.run(() -> service.plantWarehouses(plantId), "Could not load the plant warehouses."); }

    @GetMapping("/api/sale-orders") @ResponseBody
    public ResponseEntity<?> saleOrders(@RequestParam(defaultValue = "0") int customerId, @RequestParam(defaultValue = "0") int itemId) {
        return W3BApi.run(() -> service.saleOrders(customerId, itemId), "Could not load the sale orders.");
    }

    @GetMapping("/api/alternative-items") @ResponseBody
    public ResponseEntity<?> alternativeItems(@RequestParam int itemId, @RequestParam int bomHeaderId) {
        return W3BApi.run(() -> service.alternativeItems(itemId, bomHeaderId), "Could not load the alternative items.");
    }

    @GetMapping("/api/work-orders") @ResponseBody
    public ResponseEntity<?> workOrders(@RequestParam(defaultValue = "0") int recId, @RequestParam(required = false) String detailIds) {
        return W3BApi.run(() -> service.workOrders(recId, detailIds), "Could not load the work orders.");
    }

    @GetMapping("/api/stock") @ResponseBody
    public ResponseEntity<?> stock(@RequestParam int itemId, @RequestParam(defaultValue = "0") int warehouseId, @RequestParam(defaultValue = "0") int jobLotId,
                                   @RequestParam(defaultValue = "0") double itemQty, @RequestParam(defaultValue = "0") double netWeight,
                                   @RequestParam(required = false) String docDate, @RequestParam(defaultValue = "0") int recId) {
        return W3BApi.run(() -> service.stock(itemId, warehouseId, jobLotId, itemQty, netWeight, docDate, recId), "Could not read the stock.");
    }

    @GetMapping("/api/service-activities") @ResponseBody
    public ResponseEntity<?> serviceActivities(@RequestParam int itemId, @RequestParam(required = false) String docDate,
                                               @RequestParam(defaultValue = "0") int activityId, @RequestParam(defaultValue = "0") int contractorId) {
        return W3BApi.run(() -> service.serviceActivities(itemId, docDate, activityId, contractorId), "Could not load the service activities.");
    }

    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int productionId, @RequestParam(defaultValue = "0") int referred) {
        return W3BApi.run(() -> service.history(fromDate, toDate, fromDocNo, toDocNo, productionId, referred), "History failed.");
    }

    @GetMapping("/api/read/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> read(@PathVariable int id) { return W3BApi.run(() -> service.read(id), "Could not open that production."); }

    @GetMapping("/api/voucher-head") @ResponseBody
    public ResponseEntity<?> voucherHead(@RequestParam int id) { return W3BApi.run(() -> service.voucherHead(id), "Could not find the voucher."); }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return W3BApi.run(() -> service.save(body), "Save failed."); }

    @PostMapping("/api/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam int id) { return W3BApi.run(() -> service.delete(id), "Delete failed."); }
}
