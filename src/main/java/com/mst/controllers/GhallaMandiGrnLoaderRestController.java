package com.mst.controllers;

import com.mst.security.CurrentUserContext;

import com.mst.services.GhallaMandiGrnLoaderService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/ghalla-mandi-grn-loader")
public class GhallaMandiGrnLoaderRestController {

    @Autowired
    private GhallaMandiGrnLoaderService service;

    @Autowired
    private CurrentUserContext currentUserContext;

    @GetMapping("/dropdowns")
    public Map<String, Object> getDropdowns() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return service.getDropdowns(orgId, compId);
    }

    @GetMapping("/pending")
    public List<Map<String, Object>> getPendingGrns(
            @RequestParam(defaultValue = "167") Integer docTypeId,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer supplierCustomerId,
            @RequestParam(required = false) Integer ghallaMandiId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int yearId = currentUserContext.currentFinancialYearId();
        return service.getPendingGhallaMandiGrns(orgId, compId, docTypeId, yearId, fromDate, toDate, supplierCustomerId, ghallaMandiId);
    }

    @PostMapping("/validate-selection")
    public Map<String, Object> validateSelection(@RequestBody List<Map<String, Object>> selectedRows) {
        return service.validateAndSelectGrns(selectedRows);
    }
}
