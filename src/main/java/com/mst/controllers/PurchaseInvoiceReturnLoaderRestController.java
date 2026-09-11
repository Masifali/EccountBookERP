package com.mst.controllers;

import com.mst.security.CurrentUserContext;

import com.mst.services.PurchaseInvoiceReturnLoaderService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/purchase-invoice-return-loader")
public class PurchaseInvoiceReturnLoaderRestController {

    @Autowired
    private PurchaseInvoiceReturnLoaderService service;

    @Autowired
    private CurrentUserContext currentUserContext;

    @GetMapping("/branches")
    public List<Map<String, Object>> getUserBranches() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int userId = currentUserContext.currentUserId();
        return service.getUserBranches(orgId, compId, userId);
    }

    @GetMapping("/pending")
    public List<Map<String, Object>> getPendingInvoices(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) String branchIds) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int yearId = currentUserContext.currentFinancialYearId();
        return service.getPendingInvoicesForReturn(orgId, compId, yearId, fromDate, toDate, branchIds);
    }

    @PostMapping("/validate-selection")
    public Map<String, Object> validateSelection(
            @RequestBody List<Map<String, Object>> selectedRows,
            @RequestParam(defaultValue = "false") Boolean branchImplemented) {
        return service.validateAndSelectInvoices(selectedRows, branchImplemented);
    }
}
