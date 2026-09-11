package com.mst.controllers;

import com.mst.security.CurrentUserContext;

import com.mst.services.PaddyGrnLoaderService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/paddy-grn-loader")
public class PaddyGrnLoaderRestController {

    @Autowired
    private PaddyGrnLoaderService service;

    @Autowired
    private CurrentUserContext currentUserContext;

    @GetMapping("/suppliers")
    public List<Map<String, Object>> getSuppliers() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return service.getSuppliers(orgId, compId);
    }

    @GetMapping("/orders-by-supplier")
    public List<Map<String, Object>> getOrdersBySupplier(@RequestParam Integer supplierId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return service.getOrdersBySupplier(orgId, compId, supplierId);
    }

    @GetMapping("/pending")
    public List<Map<String, Object>> getPendingGrns(
            @RequestParam(defaultValue = "165") Integer docTypeId,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer orderId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int yearId = currentUserContext.currentFinancialYearId();
        return service.getPendingPaddyGrns(orgId, compId, docTypeId, yearId, fromDate, toDate, orderId);
    }

    @PostMapping("/validate-selection")
    public Map<String, Object> validateSelection(
            @RequestBody List<Map<String, Object>> selectedRows,
            @RequestParam(required = false) Integer orderId,
            @RequestParam(defaultValue = "165") Integer docTypeId) {
        return service.validateAndSelectGrns(selectedRows, orderId, docTypeId);
    }
}
