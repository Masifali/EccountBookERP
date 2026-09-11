package com.mst.controllers;

import com.mst.security.CurrentUserContext;

import com.mst.services.SupplierDispatchService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/supplier-dispatch")
public class SupplierDispatchRestController {

    @Autowired
    private SupplierDispatchService service;

    @Autowired
    private CurrentUserContext currentUserContext;

    @GetMapping("/generate-no")
    public Map<String, Object> generateNextNumbers(@RequestParam(defaultValue = "246") Integer docTypeId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int yearId = currentUserContext.currentFinancialYearId();
        return service.generateNextNumbers(orgId, compId, branchId, yearId, docTypeId);
    }

    @GetMapping("/cities")
    public List<Map<String, Object>> getCities() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return service.getCities(orgId, compId);
    }

    @GetMapping("/{id}")
    public Map<String, Object> getById(@PathVariable Integer id) {
        return service.getById(id);
    }

    @RequestMapping(value = "/history", method = {RequestMethod.GET, RequestMethod.POST})
    public List<Map<String, Object>> getHistory(
            @RequestBody(required = false) Map<String, Object> payload,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Double fromDocNo,
            @RequestParam(required = false) Double toDocNo) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int yearId = currentUserContext.currentFinancialYearId();

        Integer docTypeId = 246;
        if (payload != null) {
            if (payload.get("documentTypeId") != null) docTypeId = Integer.parseInt(payload.get("documentTypeId").toString());
            if (payload.get("fromDate") != null) fromDate = payload.get("fromDate").toString();
            if (payload.get("toDate") != null) toDate = payload.get("toDate").toString();
            if (payload.get("fromDocNo") != null && !payload.get("fromDocNo").toString().isEmpty()) fromDocNo = Double.parseDouble(payload.get("fromDocNo").toString());
            if (payload.get("toDocNo") != null && !payload.get("toDocNo").toString().isEmpty()) toDocNo = Double.parseDouble(payload.get("toDocNo").toString());
        }

        return service.getHistory(orgId, compId, docTypeId, yearId, fromDate, toDate, fromDocNo, toDocNo);
    }

    @PostMapping("/delete/{id}")
    public Map<String, Object> deleteRecord(@PathVariable Integer id) {
        int userId = currentUserContext.currentUserId();
        return service.deleteRecord(id, userId);
    }
}
