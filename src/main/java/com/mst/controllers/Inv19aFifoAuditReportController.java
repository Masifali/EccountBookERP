package com.mst.controllers;

import com.mst.services.Inv19aFifoAuditReportService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/** frmFiFoAuditReport - ScreenDefinition 290 "FIFO Audit Report" (Inventory hub, module 19). */
@Controller
@RequestMapping("/inventory/reports/fifo-audit-report")
public class Inv19aFifoAuditReportController {

    private final Inv19aFifoAuditReportService service;
    public Inv19aFifoAuditReportController(Inv19aFifoAuditReportService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "FIFO Audit Report");
        return "inventory/inv19a_fifo_audit_report";
    }

    @GetMapping("/api/show") @ResponseBody
    public ResponseEntity<?> show() {
        try { return ResponseEntity.ok(service.show()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    @GetMapping("/api/transaction-detail") @ResponseBody
    public ResponseEntity<?> transactionDetail(@RequestParam(name = "refDocumentTypeId") int refDocumentTypeId,
                                               @RequestParam(name = "refDocIdNo") int refDocIdNo,
                                               @RequestParam(name = "refDocSubIdNo") int refDocSubIdNo) {
        try { return ResponseEntity.ok(service.transactionDetail(refDocumentTypeId, refDocIdNo, refDocSubIdNo)); }
        catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    private static ResponseEntity<Map<String, Object>> fail(HttpStatus status, String message) {
        Map<String, Object> m = new LinkedHashMap<>(); m.put("success", false); m.put("message", message);
        return ResponseEntity.status(status).body(m);
    }
    private static String msg(Exception e) {
        Throwable t = e; while (t.getCause() != null && (t.getMessage() == null || t.getMessage().isBlank())) t = t.getCause();
        return t.getMessage() == null || t.getMessage().isBlank() ? "Request failed." : t.getMessage();
    }
}
