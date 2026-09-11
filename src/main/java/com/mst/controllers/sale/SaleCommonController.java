package com.mst.controllers.sale;

import com.mst.services.sale.SaleCommonService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

/**
 * Controller providing REST API endpoints for Sale Module master lookups,
 * customer search, item search, warehouse search, and document code generation.
 */
@RestController
@RequestMapping("/api/sale/common")
public class SaleCommonController {

    @Autowired
    private SaleCommonService saleCommonService;

    @GetMapping("/customers")
    public ResponseEntity<List<Map<String, Object>>> getCustomers(@RequestParam(value = "query", required = false) String query) {
        return ResponseEntity.ok(saleCommonService.getCustomers(query));
    }

    @GetMapping("/items")
    public ResponseEntity<List<Map<String, Object>>> getSaleItems(@RequestParam(value = "query", required = false) String query) {
        return ResponseEntity.ok(saleCommonService.getSaleItems(query));
    }

    @GetMapping("/warehouses")
    public ResponseEntity<List<Map<String, Object>>> getWarehouses() {
        return ResponseEntity.ok(saleCommonService.getWarehouses());
    }

    @GetMapping("/job-lots")
    public ResponseEntity<List<Map<String, Object>>> getJobLots() {
        return ResponseEntity.ok(saleCommonService.getJobLots());
    }

    @GetMapping("/packings")
    public ResponseEntity<List<Map<String, Object>>> getItemPackings() {
        return ResponseEntity.ok(saleCommonService.getItemPackings());
    }

    @GetMapping("/uoms")
    public ResponseEntity<List<Map<String, Object>>> getItemUoms() {
        return ResponseEntity.ok(saleCommonService.getItemUoms());
    }

    @GetMapping("/next-code/{documentTypeId}")
    public ResponseEntity<Map<String, Object>> getNextCode(@PathVariable("documentTypeId") int documentTypeId) {
        int nextCode = saleCommonService.generateDocumentCode(documentTypeId);
        return ResponseEntity.ok(Map.of("documentTypeId", documentTypeId, "nextCode", nextCode));
    }
}
