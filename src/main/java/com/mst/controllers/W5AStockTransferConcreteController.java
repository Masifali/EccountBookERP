package com.mst.controllers;

import com.mst.services.W5AStockTransferConcreteService;
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
 * Production (Pcc) - screen 596 "Stock Transfer Manual" (Architecture.WinApp.pcc.Production.frmStockTransferManualConcrete, DocumentTypeId 1860).
 * Page /production/pcc/stock-transfer-manual-concrete, JSON under /production/pcc/stock-transfer-manual-concrete/api. Notes: {@link W5AStockTransferConcreteService}.
 * Print reuses GET /reports/print/1860-stock-transfer-manual-slip?success=ID (ConcretePrintController).
 */
@Controller
@RequestMapping("/production/pcc/stock-transfer-manual-concrete")
public class W5AStockTransferConcreteController {

    private final W5AStockTransferConcreteService service;
    public W5AStockTransferConcreteController(W5AStockTransferConcreteService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Stock Transfer Manual");
        return "production/pcc/w5a_stock_transfer_concrete";
    }

    @GetMapping("/api/initial") @ResponseBody
    public ResponseEntity<?> initial() { return W3BApi.run(service::initial, "Could not load the screen."); }

    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() {
        return W3BApi.run(() -> { Map<String, Object> m = new java.util.LinkedHashMap<>(service.lists()); m.put("docNo", service.nextNo()); return m; }, "Refresh failed.");
    }

    @GetMapping("/api/doc-no") @ResponseBody
    public ResponseEntity<?> docNo() { return W3BApi.run(() -> Map.of("docNo", service.nextNo()), "Could not generate the document number."); }

    @GetMapping("/api/item-info") @ResponseBody
    public ResponseEntity<?> itemInfo(@RequestParam int itemId) { return W3BApi.run(() -> service.itemInfo(itemId), "Could not load the item."); }

    @GetMapping("/api/stock") @ResponseBody
    public ResponseEntity<?> stock(@RequestParam(defaultValue = "0") int itemId, @RequestParam(defaultValue = "0") int warehouseId,
                                   @RequestParam(defaultValue = "0") int jobLotId, @RequestParam(defaultValue = "0") int variantId,
                                   @RequestParam(defaultValue = "false") boolean withVariant, @RequestParam(defaultValue = "0") double qty,
                                   @RequestParam(defaultValue = "0") int recId, @RequestParam(required = false) String docDate) {
        return W3BApi.run(() -> service.stock(itemId, warehouseId, jobLotId, variantId, withVariant, qty, recId, docDate), "Could not read the stock.");
    }

    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int fromDocNo, @RequestParam(defaultValue = "0") int toDocNo,
                                     @RequestParam(required = false) String vehicleNo) {
        return W3BApi.run(() -> service.history(fromDate, toDate, fromDocNo, toDocNo, vehicleNo), "History failed.");
    }

    @GetMapping("/api/read/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> read(@PathVariable int id) { return W3BApi.run(() -> service.read(id), "Could not open that stock transfer."); }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return W3BApi.run(() -> service.save(body), "Save failed."); }

    @PostMapping("/api/delete") @ResponseBody
    public ResponseEntity<?> delete(@RequestParam int id) { return W3BApi.run(() -> service.delete(id), "Delete failed."); }
}
