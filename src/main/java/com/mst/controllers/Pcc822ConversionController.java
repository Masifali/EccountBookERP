package com.mst.controllers;

import com.mst.services.Pcc822ConversionService;
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
 * Production hub screen 822 "Stock Conversion" (Architecture.WinApp.Pcc.frmConversion, DocumentTypeId 1867).
 * Page /production/pcc-conversion, JSON under /production/pcc-conversion/api. Notes: {@link Pcc822ConversionService}.
 */
@Controller
@RequestMapping("/production/pcc-conversion")
public class Pcc822ConversionController {

    private final Pcc822ConversionService service;
    public Pcc822ConversionController(Pcc822ConversionService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Stock Conversion");
        return "production/pcc822_conversion";
    }

    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return W3BApi.run(service::lookups, "Could not load the screen."); }

    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return W3BApi.run(service::refresh, "Refresh failed."); }

    @GetMapping("/api/doc-no") @ResponseBody
    public ResponseEntity<?> docNo() { return W3BApi.run(() -> Map.of("docNo", service.docNo()), "Could not generate the document number."); }

    @GetMapping("/api/items") @ResponseBody
    public ResponseEntity<?> items(@RequestParam String typeIds) { return W3BApi.run(() -> service.items(typeIds), "Could not load the items."); }

    @GetMapping("/api/attributes") @ResponseBody
    public ResponseEntity<?> attributes(@RequestParam int itemId) { return W3BApi.run(() -> service.attributes(itemId), "Could not load the item variants."); }

    @GetMapping("/api/plant-warehouses") @ResponseBody
    public ResponseEntity<?> plantWarehouses(@RequestParam int plantId) { return W3BApi.run(() -> service.plantWarehouses(plantId), "Could not load the plant warehouses."); }

    @GetMapping("/api/warehouse-allocated") @ResponseBody
    public ResponseEntity<?> warehouseAllocated(@RequestParam int warehouseId, @RequestParam int plantId) {
        return W3BApi.run(() -> service.warehouseAllocated(warehouseId, plantId), "Could not check the warehouse.");
    }

    @GetMapping("/api/stock") @ResponseBody
    public ResponseEntity<?> stock(@RequestParam int itemId, @RequestParam(defaultValue = "0") int warehouseId, @RequestParam(defaultValue = "0") int varientId,
                                   @RequestParam(defaultValue = "0") double itemQty, @RequestParam(required = false) String docDate,
                                   @RequestParam(defaultValue = "0") int recId) {
        return W3BApi.run(() -> service.stock(itemId, warehouseId, varientId, itemQty, docDate, recId), "Could not read the stock.");
    }

    @GetMapping("/api/service-activities") @ResponseBody
    public ResponseEntity<?> serviceActivities(@RequestParam int itemId, @RequestParam(required = false) String docDate,
                                               @RequestParam(defaultValue = "0") int activityId, @RequestParam(defaultValue = "0") int contractorId) {
        return W3BApi.run(() -> service.serviceActivities(itemId, docDate, activityId, contractorId), "Could not load the service activities.");
    }

    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(defaultValue = "0") int conversionId) {
        return W3BApi.run(() -> service.history(fromDate, toDate, fromDocNo, toDocNo, conversionId), "History failed.");
    }

    @GetMapping("/api/read/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> read(@PathVariable int id) { return W3BApi.run(() -> service.read(id), "Could not open that conversion."); }

    @GetMapping("/api/voucher-head") @ResponseBody
    public ResponseEntity<?> voucherHead(@RequestParam int id) { return W3BApi.run(() -> service.voucherHead(id), "Could not find the voucher."); }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return W3BApi.run(() -> service.save(body), "Save failed."); }

    @PostMapping("/api/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam int id) { return W3BApi.run(() -> service.delete(id), "Delete failed."); }
}
