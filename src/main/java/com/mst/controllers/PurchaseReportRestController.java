package com.mst.controllers;

import com.mst.services.PurchaseReportService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/purchase/api/reports")
public class PurchaseReportRestController {

    @Autowired
    private PurchaseReportService purchaseReportService;

    private static Integer intOrNull(Object v) {
        if (v == null) return null;
        String s = v.toString().trim();
        if (s.isEmpty()) return null;
        try { return Integer.parseInt(s); } catch (NumberFormatException e) { return null; }
    }

    private static String strOrNull(Object v) {
        if (v == null) return null;
        String s = v.toString().trim();
        return s.isEmpty() ? null : s;
    }

    @PostMapping("/supplier-aging")
    public ResponseEntity<?> getSupplierAging(@RequestBody Map<String, Object> req) {
        try {
            String asOnDate = strOrNull(req.get("asOnDate"));
            Integer agingDays = intOrNull(req.get("agingDays"));
            Integer accountId = intOrNull(req.get("accountId"));
            Integer customGroupId = intOrNull(req.get("customGroupId"));
            Integer customerGroupId = intOrNull(req.get("customerGroupId"));

            List<Map<String, Object>> data = purchaseReportService.getSupplierAgingDocumentWiseReport(
                    asOnDate, agingDays, accountId, customGroupId, customerGroupId);
            Map<String, Object> res = new HashMap<>();
            res.put("success", true);
            res.put("data", data);
            res.put("totalRecords", data.size());
            return ResponseEntity.ok(res);
        } catch (Exception e) {
            Map<String, Object> err = new HashMap<>();
            err.put("success", false);
            err.put("message", "Error loading Supplier Aging Report: " + e.getMessage());
            return ResponseEntity.status(500).body(err);
        }
    }

    @PostMapping("/purchase-orders")
    public ResponseEntity<?> getPurchaseOrderRegister(@RequestBody Map<String, Object> req) {
        try {
            String fromDate = strOrNull(req.get("fromDate"));
            String toDate = strOrNull(req.get("toDate"));
            Integer supplierId = intOrNull(req.get("supplierId"));

            List<Map<String, Object>> data = purchaseReportService.getPurchaseOrderRegisterReport(fromDate, toDate, supplierId);
            Map<String, Object> res = new HashMap<>();
            res.put("success", true);
            res.put("data", data);
            res.put("totalRecords", data.size());
            return ResponseEntity.ok(res);
        } catch (Exception e) {
            Map<String, Object> err = new HashMap<>();
            err.put("success", false);
            err.put("message", "Error loading Purchase Orders Register: " + e.getMessage());
            return ResponseEntity.status(500).body(err);
        }
    }

    @PostMapping("/grn-register")
    public ResponseEntity<?> getGrnRegister(@RequestBody Map<String, Object> req) {
        try {
            String fromDate = strOrNull(req.get("fromDate"));
            String toDate = strOrNull(req.get("toDate"));
            Integer supplierId = intOrNull(req.get("supplierId"));

            List<Map<String, Object>> data = purchaseReportService.getGrnRegisterReport(fromDate, toDate, supplierId);
            Map<String, Object> res = new HashMap<>();
            res.put("success", true);
            res.put("data", data);
            res.put("totalRecords", data.size());
            return ResponseEntity.ok(res);
        } catch (Exception e) {
            Map<String, Object> err = new HashMap<>();
            err.put("success", false);
            err.put("message", "Error loading GRN Register: " + e.getMessage());
            return ResponseEntity.status(500).body(err);
        }
    }
}
