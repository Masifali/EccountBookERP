package com.mst.controllers;

import com.mst.security.CurrentUserContext;

import com.mst.services.GrnLoaderService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/grn-loader")
public class GrnLoaderRestController {

    @Autowired
    private GrnLoaderService service;

    @Autowired
    private CurrentUserContext currentUserContext;

    @GetMapping("/branches")
    public List<Map<String, Object>> getUserBranches(@RequestParam(defaultValue = "46") Integer docTypeId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int userId = currentUserContext.currentUserId();
        return service.getUserBranches(orgId, compId, userId, docTypeId);
    }

    @GetMapping("/pending")
    public List<Map<String, Object>> getPendingGrns(
            @RequestParam(defaultValue = "46") Integer docTypeId,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) String branchIds,
            @RequestParam(required = false) Integer supplierCustomerId,
            @RequestParam(required = false) Integer orderId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int yearId = currentUserContext.currentFinancialYearId();
        return service.getPendingGrns(orgId, compId, docTypeId, yearId, fromDate, toDate, branchIds, supplierCustomerId, orderId);
    }

    @GetMapping("/pending-market")
    public List<Map<String, Object>> getPendingMarketGrns(
            @RequestParam(defaultValue = "46") Integer docTypeId,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer supplierCustomerId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return service.getPendingMarketGrns(orgId, compId, docTypeId, fromDate, toDate, supplierCustomerId);
    }

    @GetMapping("/details/{grnId}")
    public List<Map<String, Object>> getGrnDetails(@PathVariable Integer grnId) {
        return service.getGrnDetails(grnId);
    }

    @PostMapping("/validate-selection")
    public Map<String, Object> validateSelection(
            @RequestBody List<Map<String, Object>> selectedRows,
            @RequestParam(defaultValue = "46") Integer docTypeId,
            @RequestParam(defaultValue = "false") Boolean acceptAccessWtHold) {
        return service.validateAndSelectGrns(selectedRows, docTypeId, acceptAccessWtHold);
    }
}
