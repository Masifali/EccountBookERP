package com.mst.controllers;

import com.mst.services.SaleOrderForApprovalService;
import com.mst.services.SaleOrderForApprovalService.Filter;
import com.mst.services.SaleOrderForApprovalService.Update;
import java.time.LocalDate;
import java.util.Map;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

@Controller
public class SaleOrderForApprovalController {
    private final SaleOrderForApprovalService service;
    public SaleOrderForApprovalController(SaleOrderForApprovalService service) { this.service = service; }
    @GetMapping("/dashboard/sale-order-for-approval")
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Sales Order Approval");
        return "dashboard/sale_order_for_approval";
    }
    @GetMapping("/api/dashboard/sale-order-approval/setup") @ResponseBody
    public Map<String,Object> setup(@RequestParam Map<String,String> p) { return service.setup(filter(p)); }
    @GetMapping("/api/dashboard/sale-order-approval/history") @ResponseBody
    public Map<String,Object> history(@RequestParam Map<String,String> p) { return service.history(filter(p)); }
    @PostMapping("/api/dashboard/sale-order-approval/update") @ResponseBody
    public Map<String,Object> update(@RequestBody Update request) { return service.update(request); }
    @ExceptionHandler({IllegalArgumentException.class, AccessDeniedException.class}) @ResponseBody
    public ResponseEntity<Map<String,String>> invalid(RuntimeException e) {
        return ResponseEntity.status(e instanceof AccessDeniedException ? 403 : 400).body(Map.of("message", e.getMessage()));
    }
    private static Filter filter(Map<String,String> p) {
        return new Filter(number(p,"documentTypeId",81), number(p,"branchId",0), date(p.get("fromDate")),
                p.getOrDefault("toDate", "").isBlank() ? LocalDate.now() : date(p.get("toDate")),
                number(p,"customerId",0), number(p,"itemId",0), p.getOrDefault("approved","Not Approved"), p.getOrDefault("status","Open"));
    }
    private static int number(Map<String,String> p, String key, int def) { return p.getOrDefault(key,"").isBlank() ? def : Integer.parseInt(p.get(key)); }
    private static LocalDate date(String s) { return s == null || s.isBlank() ? null : LocalDate.parse(s); }
}
