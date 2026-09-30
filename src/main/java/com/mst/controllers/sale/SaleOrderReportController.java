package com.mst.controllers.sale;

import com.mst.models.SaleOrderReportFilter;
import com.mst.models.SaleOrderReportAction;
import com.mst.models.SaleReportApprovalRequest;
import com.mst.services.SaleOrderReportActionService;
import com.mst.services.SaleReportApprovalService;
import com.mst.services.SaleOrderReportService;
import java.util.*;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/sale/reports/sale-order-report/api")
public class SaleOrderReportController {
    private final SaleOrderReportService service;
    private final SaleOrderReportActionService actions;
    private final SaleReportApprovalService approvals;
    public SaleOrderReportController(SaleOrderReportService service, SaleOrderReportActionService actions,
            SaleReportApprovalService approvals) {
        this.service = service; this.actions = actions; this.approvals = approvals;
    }
    @GetMapping("/initial") public Map<String, Object> initial() { return service.initial(); }
    @GetMapping("/lookups") public List<Map<String, Object>> lookups(@RequestParam List<Integer> branchIds,
            @RequestParam(defaultValue = "0") int costCenterId) { return service.lookups(branchIds, costCenterId); }
    @PostMapping("/detail") public Map<String, Object> detail(@RequestBody SaleOrderReportFilter f) { return service.detail(f); }
    @PostMapping("/summary") public Map<String, Object> summary(@RequestBody SaleOrderReportFilter f) { return service.summary(f); }
    @PostMapping("/action") public Map<String, Object> action(@RequestBody SaleOrderReportAction request) { return actions.apply(request); }
    @PostMapping("/approval-history") public Map<String, Object> approvalHistory(
            @RequestBody SaleReportApprovalRequest<SaleOrderReportFilter> request) {
        if (request == null || request.filter() == null) throw new IllegalArgumentException("Select a report document");
        if (request.summary() && !"Order Register".equals(request.filter().activity()))
            throw new IllegalArgumentException("Select an order from Order Register");
        return approvals.history(request.summary() ? service.summary(request.filter()) : service.detail(request.filter()),
                request.summary() ? "rows" : "headers", request.documentTypeId(), request.id());
    }
    @ExceptionHandler(IllegalArgumentException.class) public ResponseEntity<?> invalid(IllegalArgumentException e) {
        return ResponseEntity.badRequest().body(Map.of("message", e.getMessage()));
    }
    @ExceptionHandler(AccessDeniedException.class) public ResponseEntity<?> denied(AccessDeniedException e) {
        return ResponseEntity.status(403).body(Map.of("message", e.getMessage()));
    }
    @ExceptionHandler(IllegalStateException.class) public ResponseEntity<?> failed(IllegalStateException e) {
        return ResponseEntity.status(500).body(Map.of("message", e.getMessage()));
    }
}
