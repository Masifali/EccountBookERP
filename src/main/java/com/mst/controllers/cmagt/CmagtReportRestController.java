package com.mst.controllers.cmagt;

import com.mst.services.cmagt.CmagtReportService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/commission/reports")
public class CmagtReportRestController {

    @Autowired
    private CmagtReportService service;

    @GetMapping("/sale-order")
    public ResponseEntity<List<Map<String, Object>>> getSaleOrderReport(
            @RequestParam(required = false, defaultValue = "1") Integer companyId,
            @RequestParam(required = false, defaultValue = "1") Integer organizationId,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer buyerId,
            @RequestParam(required = false) Integer agentId) {
        return ResponseEntity.ok(service.getSaleOrderReport(companyId, organizationId, fromDate, toDate, buyerId, agentId));
    }

    @GetMapping("/purchase-order")
    public ResponseEntity<List<Map<String, Object>>> getPurchaseOrderReport(
            @RequestParam(required = false, defaultValue = "1") Integer companyId,
            @RequestParam(required = false, defaultValue = "1") Integer organizationId,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer supplierId,
            @RequestParam(required = false) Integer agentId) {
        return ResponseEntity.ok(service.getPurchaseOrderReport(companyId, organizationId, fromDate, toDate, supplierId, agentId));
    }

    @GetMapping("/grn-supplier-loading")
    public ResponseEntity<List<Map<String, Object>>> getGrnSupplierLoadingReport(
            @RequestParam(required = false, defaultValue = "1") Integer companyId,
            @RequestParam(required = false, defaultValue = "1") Integer organizationId,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer supplierId) {
        return ResponseEntity.ok(service.getGrnSupplierLoadingReport(companyId, organizationId, fromDate, toDate, supplierId));
    }

    @GetMapping("/gdn-buyer-dispatch")
    public ResponseEntity<List<Map<String, Object>>> getGdnBuyerDispatchReport(
            @RequestParam(required = false, defaultValue = "1") Integer companyId,
            @RequestParam(required = false, defaultValue = "1") Integer organizationId,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer buyerId) {
        return ResponseEntity.ok(service.getGdnBuyerDispatchReport(companyId, organizationId, fromDate, toDate, buyerId));
    }

    @GetMapping("/agent-trade-bill-register")
    public ResponseEntity<List<Map<String, Object>>> getAgentTradeBillRegister(
            @RequestParam(required = false, defaultValue = "1") Integer companyId,
            @RequestParam(required = false, defaultValue = "1") Integer organizationId,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer agentId) {
        return ResponseEntity.ok(service.getAgentTradeBillRegister(companyId, organizationId, fromDate, toDate, agentId));
    }
}
