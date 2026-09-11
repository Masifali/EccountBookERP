package com.mst.controllers;

import com.mst.security.CurrentUserContext;
import com.mst.services.PendingGatePassLoaderService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/purchase/pending-gatepass-loader")
public class PendingGatePassLoaderRestController {

    @Autowired
    private PendingGatePassLoaderService pendingGatePassLoaderService;

    @Autowired
    private CurrentUserContext currentUserContext;

    @GetMapping("/list")
    public ResponseEntity<?> getPendingGatePasses() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int yearId = currentUserContext.currentFinancialYearId();
        List<Map<String, Object>> list = pendingGatePassLoaderService.getPendingGatePasses(orgId, compId, branchId, yearId);
        return ResponseEntity.ok(list);
    }
}
