package com.mst.controllers;

import com.mst.services.Engr795ProductionService;
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
 * Production hub screen 795 "Production (Casting)" (Architecture.WinApp.Mfg.frmProductionEngr, DocumentTypeId 1670).
 * Page /production/engr-production, JSON under /production/engr-production/api. Notes: {@link Engr795ProductionService}.
 * The slip print is the existing GET /reports/print/1670-production-engr-slip?branchesId=&id= (ManufacturingPrintController).
 */
@Controller
@RequestMapping("/production/engr-production")
public class Engr795ProductionController {

    private final Engr795ProductionService service;
    public Engr795ProductionController(Engr795ProductionService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Production (Casting)");
        return "production/engr_production";
    }

    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return W3BApi.run(service::lookups, "Could not load the screen."); }

    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return W3BApi.run(service::refresh, "Refresh failed."); }

    @GetMapping("/api/doc-no") @ResponseBody
    public ResponseEntity<?> docNo() { return W3BApi.run(() -> Map.of("docNo", service.docNo()), "Could not generate the document number."); }

    @GetMapping("/api/boms") @ResponseBody
    public ResponseEntity<?> boms(@RequestParam(defaultValue = "0") int recId) { return W3BApi.run(() -> service.boms(recId), "Could not load the bills of material."); }

    @GetMapping("/api/bom-inputs") @ResponseBody
    public ResponseEntity<?> bomInputs(@RequestParam int id) { return W3BApi.run(() -> service.bomInputs(id), "Could not load the BOM inputs."); }

    @GetMapping("/api/work-orders") @ResponseBody
    public ResponseEntity<?> workOrders(@RequestParam(defaultValue = "0") int castingTypeId, @RequestParam(defaultValue = "0") int recId) {
        return W3BApi.run(() -> service.workOrders(castingTypeId, recId), "Could not load the work orders.");
    }

    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo) {
        return W3BApi.run(() -> service.history(fromDate, toDate, fromDocNo, toDocNo), "History failed.");
    }

    @GetMapping("/api/read/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> read(@PathVariable int id) { return W3BApi.run(() -> service.read(id), "Could not open that record."); }

    @GetMapping("/api/voucher-head") @ResponseBody
    public ResponseEntity<?> voucherHead(@RequestParam int id) { return W3BApi.run(() -> service.voucherHead(id), "Could not find the voucher."); }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return W3BApi.run(() -> service.save(body), "Save failed."); }

    @PostMapping("/api/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam int id) { return W3BApi.run(() -> service.delete(id), "Delete failed."); }
}
