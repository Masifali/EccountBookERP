package com.mst.controllers;

import com.mst.services.SalePccRptService;
import java.util.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

/**
 * Group P pages and data endpoints for the Sales Pcc Reports (module 88) 451 / 561 / 562 (prints: SalePccRptPrintController).
 *   GET /sale/reports/pcc/stock-transfer-register       451 StockTranfserRegister  (Stock Transfer Register thal)
 *   GET /sale/reports/pcc/sales-evaluation-detail       561 Sales_EvaulationDetailReports  (Sale Invoice Register)
 *   GET /sale/reports/pcc/sales-wages-register          562 SalesWages_Register  (Sales Wages Register)
 *   GET /api/sale/salepcc/{report}/lookups              the form's Load lists
 *   GET /api/sale/salepcc/{report}/rows                 the Show button (query = the form's filters)
 */
@Controller
public class SalePccRptController {
    private final SalePccRptService service;

    public SalePccRptController(SalePccRptService service) { this.service = service; }

    @GetMapping("/sale/reports/pcc/stock-transfer-register")
    public String stockTransferRegister() { return "sale/reports/pcc_stock_transfer_register"; }

    @GetMapping("/sale/reports/pcc/sales-evaluation-detail")
    public String salesEvaluationDetail() { return "sale/reports/pcc_sales_evaluation_detail"; }

    @GetMapping("/sale/reports/pcc/sales-wages-register")
    public String salesWagesRegister() { return "sale/reports/pcc_sales_wages_register"; }

    @GetMapping("/api/sale/salepcc/{report}/lookups")
    @ResponseBody
    public Map<String, Object> lookups(@PathVariable("report") String report) { return service.lookups(report); }

    @GetMapping("/api/sale/salepcc/{report}/rows")
    @ResponseBody
    public List<Map<String, Object>> rows(@PathVariable("report") String report, @RequestParam Map<String, String> q) { return service.rows(report, q); }

    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class)
    @ResponseBody
    public org.springframework.http.ResponseEntity<String> denied(org.springframework.security.access.AccessDeniedException e) {
        return org.springframework.http.ResponseEntity.status(403).contentType(org.springframework.http.MediaType.TEXT_PLAIN).body(e.getMessage());
    }

    @ExceptionHandler({IllegalArgumentException.class, IllegalStateException.class, org.springframework.dao.DataAccessException.class})
    @ResponseBody
    public org.springframework.http.ResponseEntity<String> failed(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage() == null ? e.getMessage() : t.getMessage();
        return org.springframework.http.ResponseEntity.badRequest().contentType(org.springframework.http.MediaType.TEXT_PLAIN).body(m);
    }
}
