package com.mst.controllers.sale;

import com.mst.services.SaleReportService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/sale/api/reports")
public class SaleReportRestController {

    @Autowired
    private SaleReportService saleReportService;

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

    @PostMapping("/receivables-aging")
    public ResponseEntity<?> getReceivablesAging(@RequestBody Map<String, Object> req) {
        try {
            String asOnDate = strOrNull(req.get("asOnDate"));
            Integer customerId = intOrNull(req.get("customerId"));

            List<Map<String, Object>> data = saleReportService.getCustomerReceivablesAgingReport(asOnDate, customerId);
            Map<String, Object> res = new HashMap<>();
            res.put("success", true);
            res.put("data", data);
            res.put("totalRecords", data.size());
            return ResponseEntity.ok(res);
        } catch (Exception e) {
            Map<String, Object> err = new HashMap<>();
            err.put("success", false);
            err.put("message", "Error loading Customer Receivables Aging Report: " + e.getMessage());
            return ResponseEntity.status(500).body(err);
        }
    }

    @PostMapping("/sale-invoices")
    public ResponseEntity<?> getSaleInvoiceRegister(@RequestBody Map<String, Object> req) {
        try {
            String fromDate = strOrNull(req.get("fromDate"));
            String toDate = strOrNull(req.get("toDate"));
            Integer customerId = intOrNull(req.get("customerId"));

            List<Map<String, Object>> data = saleReportService.getSaleInvoiceRegisterReport(fromDate, toDate, customerId);
            Map<String, Object> res = new HashMap<>();
            res.put("success", true);
            res.put("data", data);
            res.put("totalRecords", data.size());
            return ResponseEntity.ok(res);
        } catch (Exception e) {
            Map<String, Object> err = new HashMap<>();
            err.put("success", false);
            err.put("message", "Error loading Sale Invoice Register Report: " + e.getMessage());
            return ResponseEntity.status(500).body(err);
        }
    }
}
