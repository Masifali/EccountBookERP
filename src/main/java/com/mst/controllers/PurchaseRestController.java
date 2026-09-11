package com.mst.controllers;

import com.mst.models.dto.PurchaseTransactionDto;
import com.mst.services.PurchaseService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.Map;

@RestController
@RequestMapping("/api/purchase")
public class PurchaseRestController {

    @Autowired
    private PurchaseService purchaseService;

    @GetMapping("/next-code")
    public ResponseEntity<?> getNextDocCode(@RequestParam(value = "docType", defaultValue = "16") int docType) {
        Map<String, Object> res = new HashMap<>();
        int code = purchaseService.generateNextDocNo(docType);
        res.put("docNo", code);
        res.put("docNoDisplay", String.format("DOC-%04d", code));
        return ResponseEntity.ok(res);
    }

    @GetMapping("/suppliers")
    public ResponseEntity<?> searchSuppliers(@RequestParam(value = "q", required = false) String query) {
        return ResponseEntity.ok(purchaseService.getSuppliers(query));
    }

    @GetMapping("/items")
    public ResponseEntity<?> searchItems(@RequestParam(value = "q", required = false) String query) {
        return ResponseEntity.ok(purchaseService.getItems(query));
    }

    @GetMapping("/warehouses")
    public ResponseEntity<?> getWarehouses() {
        return ResponseEntity.ok(purchaseService.getWarehouses());
    }

    @GetMapping("/job-lots")
    public ResponseEntity<?> getJobLots() {
        return ResponseEntity.ok(purchaseService.getJobLots());
    }

    @GetMapping("/list")
    public ResponseEntity<?> getTransactionsByDocType(@RequestParam(value = "docType", defaultValue = "16") int docType) {
        return ResponseEntity.ok(purchaseService.getPurchaseTransactionsByDocType(docType));
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getTransactionById(@PathVariable("id") Integer id) {
        Map<String, Object> transaction = purchaseService.getPurchaseTransactionById(id);
        if (transaction != null) {
            return ResponseEntity.ok(transaction);
        } else {
            return ResponseEntity.notFound().build();
        }
    }

    @PostMapping("/save")
    public ResponseEntity<?> saveTransaction(@RequestBody PurchaseTransactionDto dto) {
        Map<String, Object> res = purchaseService.savePurchaseTransaction(dto);
        if (Boolean.TRUE.equals(res.get("success"))) {
            return ResponseEntity.ok(res);
        } else {
            return ResponseEntity.status(400).body(res);
        }
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteTransaction(@PathVariable("id") Integer id) {
        boolean deleted = purchaseService.deletePurchaseTransaction(id);
        Map<String, Object> res = new HashMap<>();
        if (deleted) {
            res.put("success", true);
            res.put("message", "Purchase transaction deleted successfully.");
            return ResponseEntity.ok(res);
        } else {
            res.put("success", false);
            res.put("message", "Failed to delete transaction.");
            return ResponseEntity.status(400).body(res);
        }
    }
}
