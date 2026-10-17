package com.mst.controllers;

import com.mst.services.EngrStageMoveService;
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
 * Production hub screens 807 and 846 "Production Move (Stage Shift)" (Architecture.WinApp.Mfg.frmStockTransferManual_Engr, DocumentTypeId 1663).
 * Page /production/engr-stage-move, JSON under /production/engr-stage-move/api. Notes: {@link EngrStageMoveService}.
 * The slip print is the existing GET /reports/print/1663-stock-transfer-slip?id= (ManufacturingPrintController).
 */
@Controller
@RequestMapping("/production/engr-stage-move")
public class EngrStageMoveController {

    private final EngrStageMoveService service;
    public EngrStageMoveController(EngrStageMoveService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Production Move (Stage Shift)");
        return "production/engr_stage_move";
    }

    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return W3BApi.run(service::lookups, "Could not load the screen."); }

    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return W3BApi.run(service::refresh, "Refresh failed."); }

    @GetMapping("/api/doc-no") @ResponseBody
    public ResponseEntity<?> docNo() { return W3BApi.run(() -> Map.of("docNo", service.docNo()), "Could not generate the document number."); }

    @GetMapping("/api/transfer-type") @ResponseBody
    public ResponseEntity<?> transferType(@RequestParam int id) { return W3BApi.run(() -> service.transferType(id), "Could not read the transfer type."); }

    @GetMapping("/api/available-stock") @ResponseBody
    public ResponseEntity<?> availableStock(@RequestParam(defaultValue = "0") int warehouseId, @RequestParam(defaultValue = "0") int stageId,
                                            @RequestParam(required = false) String docDate) {
        return W3BApi.run(() -> service.availableStock(warehouseId, stageId, docDate), "Could not read the available stock.");
    }

    @GetMapping("/api/attributes") @ResponseBody
    public ResponseEntity<?> attributes(@RequestParam int itemId) { return W3BApi.run(() -> service.attributes(itemId), "Could not load the item variants."); }

    @GetMapping("/api/avg-rate") @ResponseBody
    public ResponseEntity<?> avgRate(@RequestParam int itemId, @RequestParam(defaultValue = "0") int warehouseId,
                                     @RequestParam(required = false) String docDate, @RequestParam(defaultValue = "0") int recId) {
        return W3BApi.run(() -> service.avgRate(itemId, warehouseId, docDate, recId), "Could not read the average rate.");
    }

    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo) {
        return W3BApi.run(() -> service.history(fromDate, toDate, fromDocNo, toDocNo), "History failed.");
    }

    @GetMapping("/api/read/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> read(@PathVariable int id) { return W3BApi.run(() -> service.read(id), "Could not open that record."); }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return W3BApi.run(() -> service.save(body), "Save failed."); }

    @PostMapping("/api/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam int id) { return W3BApi.run(() -> service.delete(id), "Delete failed."); }
}
