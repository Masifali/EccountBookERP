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

    /* Company and organization are NEVER taken from the request. They used to arrive as
       @RequestParam(defaultValue = "1"), which meant two things at once: a caller could read
       another company's data by appending ?companyId=, and a caller that omitted it silently
       queried company 1 - which in this database does not exist, so the screen showed nothing and
       said nothing. The desktop reads UserAccount.OrganizationId / .CompanyId and offers no
       override; this is that, enforced server-side. */
    @org.springframework.beans.factory.annotation.Autowired
    private com.mst.security.CurrentUserContext currentUserContext;

    @GetMapping("/sale-order")
    public ResponseEntity<List<Map<String, Object>>> getSaleOrderReport(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer buyerId,
            @RequestParam(required = false) Integer agentId) {
        return ResponseEntity.ok(service.getSaleOrderReport(currentUserContext.currentCompanyId(), currentUserContext.currentOrganizationId(), fromDate, toDate, buyerId, agentId));
    }

    @GetMapping("/purchase-order")
    public ResponseEntity<List<Map<String, Object>>> getPurchaseOrderReport(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer supplierId,
            @RequestParam(required = false) Integer agentId) {
        return ResponseEntity.ok(service.getPurchaseOrderReport(currentUserContext.currentCompanyId(), currentUserContext.currentOrganizationId(), fromDate, toDate, supplierId, agentId));
    }

    @GetMapping("/grn-supplier-loading")
    public ResponseEntity<List<Map<String, Object>>> getGrnSupplierLoadingReport(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer supplierId) {
        return ResponseEntity.ok(service.getGrnSupplierLoadingReport(currentUserContext.currentCompanyId(), currentUserContext.currentOrganizationId(), fromDate, toDate, supplierId));
    }

    @GetMapping("/gdn-buyer-dispatch")
    public ResponseEntity<List<Map<String, Object>>> getGdnBuyerDispatchReport(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer buyerId) {
        return ResponseEntity.ok(service.getGdnBuyerDispatchReport(currentUserContext.currentCompanyId(), currentUserContext.currentOrganizationId(), fromDate, toDate, buyerId));
    }

    @GetMapping("/agent-trade-bill-register")
    public ResponseEntity<List<Map<String, Object>>> getAgentTradeBillRegister(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer agentId) {
        return ResponseEntity.ok(service.getAgentTradeBillRegister(currentUserContext.currentCompanyId(), currentUserContext.currentOrganizationId(), fromDate, toDate, agentId));
    }
}
