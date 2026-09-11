package com.mst.controllers.sale;

import com.mst.models.dto.PurchaseTransactionDto;
import com.mst.services.SaleService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.Map;

@RestController
@RequestMapping("/api/sale")
public class SaleRestController {

    @Autowired
    private SaleService saleService;

    @GetMapping("/next-code")
    public ResponseEntity<?> getNextDocCode(@RequestParam(value = "docType", defaultValue = "17") int docType) {
        Map<String, Object> res = new HashMap<>();
        int code = saleService.generateNextDocNo(docType);
        res.put("docNo", code);
        res.put("docNoDisplay", String.format("DOC-%04d", code));
        return ResponseEntity.ok(res);
    }

    @GetMapping("/customers")
    public ResponseEntity<?> searchCustomers(@RequestParam(value = "q", required = false) String query) {
        return ResponseEntity.ok(saleService.getCustomers(query));
    }

    @GetMapping("/items")
    public ResponseEntity<?> searchItems(@RequestParam(value = "q", required = false) String query) {
        return ResponseEntity.ok(saleService.getItems(query));
    }

    @GetMapping("/warehouses")
    public ResponseEntity<?> getWarehouses() {
        return ResponseEntity.ok(saleService.getWarehouses());
    }

    @GetMapping("/job-lots")
    public ResponseEntity<?> getJobLots() {
        return ResponseEntity.ok(saleService.getJobLots());
    }

    @GetMapping("/drivers")
    public ResponseEntity<?> getDrivers() {
        return ResponseEntity.ok(saleService.getDrivers());
    }

    @GetMapping("/transporters")
    public ResponseEntity<?> getTransporters() {
        return ResponseEntity.ok(saleService.getTransporters());
    }

    @GetMapping("/salesmen")
    public ResponseEntity<?> getSalesmen() {
        return ResponseEntity.ok(saleService.getSalesmen());
    }

    @GetMapping("/payment-terms")
    public ResponseEntity<?> getPaymentTerms() {
        return ResponseEntity.ok(saleService.getPaymentTerms());
    }

    @GetMapping("/delivery-terms")
    public ResponseEntity<?> getDeliveryTerms() {
        return ResponseEntity.ok(saleService.getDeliveryTerms());
    }

    @GetMapping("/currencies")
    public ResponseEntity<?> getCurrencies() {
        return ResponseEntity.ok(saleService.getCurrencies());
    }

    @GetMapping("/delivery-orders")
    public ResponseEntity<?> getDeliveryOrders(@RequestParam(value = "customerId", required = false) Integer customerId) {
        return ResponseEntity.ok(saleService.getDeliveryOrders(customerId));
    }

    @GetMapping("/outward-gate-passes")
    public ResponseEntity<?> getOutwardGatePasses(@RequestParam(value = "customerId", required = false) Integer customerId) {
        return ResponseEntity.ok(saleService.getOutwardGatePasses(customerId));
    }

    @GetMapping("/gdn-list")
    public ResponseEntity<?> getGoodsDispatchNotes(@RequestParam(value = "customerId", required = false) Integer customerId) {
        return ResponseEntity.ok(saleService.getGoodsDispatchNotes(customerId));
    }

    @GetMapping("/list")
    public ResponseEntity<?> getTransactionsByDocType(@RequestParam(value = "docType", defaultValue = "17") int docType) {
        return ResponseEntity.ok(saleService.getSaleTransactionsByDocType(docType));
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getTransactionById(@PathVariable("id") Integer id) {
        Map<String, Object> transaction = saleService.getSaleTransactionById(id);
        if (transaction != null) {
            return ResponseEntity.ok(transaction);
        } else {
            return ResponseEntity.notFound().build();
        }
    }

    @PostMapping("/save")
    public ResponseEntity<?> saveTransaction(@RequestBody PurchaseTransactionDto dto) {
        Map<String, Object> res = saleService.saveSaleTransaction(dto);
        if (Boolean.TRUE.equals(res.get("success"))) {
            return ResponseEntity.ok(res);
        } else {
            return ResponseEntity.status(400).body(res);
        }
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteTransaction(@PathVariable("id") Integer id) {
        boolean deleted = saleService.deleteSaleTransaction(id);
        Map<String, Object> res = new HashMap<>();
        if (deleted) {
            res.put("success", true);
            res.put("message", "Sale transaction deleted successfully.");
            return ResponseEntity.ok(res);
        } else {
            res.put("success", false);
            res.put("message", "Failed to delete transaction.");
            return ResponseEntity.status(400).body(res);
        }
    }
}
