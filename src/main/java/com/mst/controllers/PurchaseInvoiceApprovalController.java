package com.mst.controllers;

import com.mst.services.PurchaseInvoiceApprovalService;
import com.mst.services.PurchaseInvoiceApprovalService.Filter;
import com.mst.services.PurchaseInvoiceApprovalService.Approval;
import java.time.LocalDate;
import java.time.format.DateTimeParseException;
import java.util.Map;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.dao.DataAccessException;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

@Controller
public class PurchaseInvoiceApprovalController {
    private static final Logger LOG = LoggerFactory.getLogger(PurchaseInvoiceApprovalController.class);
    private final PurchaseInvoiceApprovalService service;
    public PurchaseInvoiceApprovalController(PurchaseInvoiceApprovalService service) { this.service = service; }
    @GetMapping("/dashboard/purchase-invoice-for-approval")
    public String page(@RequestParam(defaultValue="false") boolean unApprove, Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", unApprove ? "Undo Purchase Invoice Approval" : "Purchase Invoice Approval");
        return "dashboard/purchase_invoice_for_approval";
    }
    @GetMapping("/api/dashboard/purchase-invoice-approval/setup") @ResponseBody
    public Map<String,Object> setup(@RequestParam Map<String,String> p) { return service.setup(filter(p)); }
    @GetMapping("/api/dashboard/purchase-invoice-approval/history") @ResponseBody
    public Map<String,Object> history(@RequestParam Map<String,String> p) { return service.history(filter(p)); }
    @PostMapping("/api/dashboard/purchase-invoice-approval/approve") @ResponseBody
    public Map<String,Object> approve(@RequestBody Approval request) { return service.approve(request); }
    @PostMapping("/api/dashboard/purchase-invoice-approval/unapprove") @ResponseBody
    public Map<String,Object> unapprove(@RequestBody Approval request) { return service.unapprove(request); }
    @ExceptionHandler({IllegalArgumentException.class, AccessDeniedException.class, DateTimeParseException.class}) @ResponseBody
    public ResponseEntity<Map<String,String>> invalid(RuntimeException e) {
        return ResponseEntity.status(e instanceof AccessDeniedException ? 403 : 400).body(Map.of("message",
                e instanceof DateTimeParseException ? "Select a valid date" : e.getMessage()));
    }
    @ExceptionHandler(DataAccessException.class) @ResponseBody
    public ResponseEntity<Map<String,String>> database(DataAccessException e) {
        for (Throwable cause=e; cause!=null; cause=cause.getCause()) {
            if (cause instanceof java.sql.SQLException sql && sql.getErrorCode()==50000)
                return ResponseEntity.badRequest().body(Map.of("message", sql.getMessage()));
        }
        LOG.error("Purchase invoice approval database request failed", e);
        return ResponseEntity.status(503).body(Map.of("message", "The database could not complete this request. Refresh the list and try again."));
    }
    private static Filter filter(Map<String,String> p) {
        return new Filter(number(p,"documentTypeId",56), number(p,"branchId",0), date(p.get("fromDate")),
                p.getOrDefault("toDate", "").isBlank() ? LocalDate.now() : date(p.get("toDate")),
                number(p,"supplierId",0), number(p,"itemId",0), Boolean.parseBoolean(p.getOrDefault("unApprove","false")));
    }
    private static int number(Map<String,String> p, String key, int def) { return p.getOrDefault(key,"").isBlank() ? def : Integer.parseInt(p.get(key)); }
    private static LocalDate date(String s) { return s == null || s.isBlank() ? null : LocalDate.parse(s); }
}
