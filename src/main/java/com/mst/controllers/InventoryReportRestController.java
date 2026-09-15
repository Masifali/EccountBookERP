package com.mst.controllers;

import com.mst.services.InventoryReportService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/inventory/api/reports")
public class InventoryReportRestController {

    @Autowired
    private InventoryReportService inventoryReportService;

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

    @PostMapping("/stock-evaluation")
    public ResponseEntity<?> getStockEvaluation(@RequestBody Map<String, Object> req) {
        try {
            String asOnDate = strOrNull(req.get("asOnDate"));
            Integer warehouseId = intOrNull(req.get("warehouseId"));
            Integer itemCategoryId = intOrNull(req.get("itemCategoryId"));

            List<Map<String, Object>> data = inventoryReportService.getStockEvaluationReport(asOnDate, warehouseId, itemCategoryId);
            Map<String, Object> res = new HashMap<>();
            res.put("success", true);
            res.put("data", data);
            res.put("totalRecords", data.size());
            return ResponseEntity.ok(res);
        } catch (Exception e) {
            Map<String, Object> err = new HashMap<>();
            err.put("success", false);
            err.put("message", "Error loading Stock Evaluation Report: " + e.getMessage());
            return ResponseEntity.status(500).body(err);
        }
    }

    @PostMapping("/inward-gate-pass")
    public ResponseEntity<?> getInwardGatePassReport(@RequestBody Map<String, Object> req) {
        try {
            String fromDate = strOrNull(req.get("fromDate"));
            String toDate = strOrNull(req.get("toDate"));
            Integer supplierId = intOrNull(req.get("supplierId"));

            List<Map<String, Object>> data = inventoryReportService.getInwardGatePassReport(fromDate, toDate, supplierId);
            Map<String, Object> res = new HashMap<>();
            res.put("success", true);
            res.put("data", data);
            res.put("totalRecords", data.size());
            return ResponseEntity.ok(res);
        } catch (Exception e) {
            Map<String, Object> err = new HashMap<>();
            err.put("success", false);
            err.put("message", "Error loading Inward Gate Pass Report: " + e.getMessage());
            return ResponseEntity.status(500).body(err);
        }
    }

    @PostMapping({"/inventory-profitability", "/inventory-profitability-report"})
    public ResponseEntity<?> getInventoryProfitabilityReport(@RequestBody Map<String, Object> req) {
        try {
            String fromDate = strOrNull(req.get("fromDate"));
            String toDate = strOrNull(req.get("toDate"));
            Integer itemCategoryId = intOrNull(req.get("itemCategoryId"));
            Integer itemDefId = intOrNull(req.get("itemDefId"));

            List<Map<String, Object>> data = inventoryReportService.getInventoryProfitabilityReport(fromDate, toDate, itemCategoryId, itemDefId);
            return ResponseEntity.ok(data);
        } catch (Exception e) {
            return ResponseEntity.status(500).body(new HashMap<>());
        }
    }
}
