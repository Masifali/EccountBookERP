package com.mst.controllers.sale;

import com.mst.models.SaleActivitiesReportFilter;
import com.mst.models.SaleReportApprovalRequest;
import com.mst.services.SaleActivitiesReportService;
import com.mst.services.SaleReportApprovalService;
import java.util.*;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/sale/reports/sale-invoice-report-with-activities/api")
public class SaleActivitiesReportController {
    private final SaleActivitiesReportService service;
    private final SaleReportApprovalService approvals;
    public SaleActivitiesReportController(SaleActivitiesReportService service, SaleReportApprovalService approvals) {
        this.service=service; this.approvals=approvals;
    }
    @GetMapping("/initial") public Map<String,Object> initial() { return service.initial(); }
    @GetMapping("/lookups") public List<Map<String,Object>> lookups(@RequestParam List<Integer> branchIds) { return service.lookups(branchIds); }
    @PostMapping("/rows") public Map<String,Object> rows(@RequestBody SaleActivitiesReportFilter filter) { return service.rows(filter); }
    @PostMapping("/approval-history") public Map<String,Object> approvalHistory(
            @RequestBody SaleReportApprovalRequest<SaleActivitiesReportFilter> request) {
        if (request == null || request.filter() == null || !"Sales Register".equals(request.filter().activity()))
            throw new IllegalArgumentException("Select a document from Sales Register");
        return approvals.history(service.rows(request.filter()), "rows", request.documentTypeId(), request.id());
    }
    @ExceptionHandler(IllegalArgumentException.class) public ResponseEntity<?> invalid(IllegalArgumentException e) { return ResponseEntity.badRequest().body(Map.of("message",e.getMessage())); }
    @ExceptionHandler(AccessDeniedException.class) public ResponseEntity<?> denied(AccessDeniedException e) { return ResponseEntity.status(403).body(Map.of("message",e.getMessage())); }
}
