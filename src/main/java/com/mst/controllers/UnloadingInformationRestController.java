package com.mst.controllers;

import com.mst.security.CurrentUserContext;

import com.mst.services.UnloadingInformationService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/unloading-information")
public class UnloadingInformationRestController {

    @Autowired
    private UnloadingInformationService service;

    @Autowired
    private CurrentUserContext currentUserContext;

    @GetMapping("/items-for-unloading")
    public List<Map<String, Object>> getItemsForUnloading(
            @RequestParam(required = false) Integer gpId,
            @RequestParam(required = false) Integer labId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return service.getItemsForUnloading(orgId, compId, gpId, labId);
    }

    @GetMapping("/contractors")
    public List<Map<String, Object>> getContractors(@RequestParam(required = false) Integer branchId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return service.getContractors(orgId, compId, branchId != null ? branchId : currentUserContext.currentBranchId());
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

        if (payload != null) {
            if (payload.get("fromDate") != null) fromDate = payload.get("fromDate").toString();
            if (payload.get("toDate") != null) toDate = payload.get("toDate").toString();
            if (payload.get("fromDocNo") != null && !payload.get("fromDocNo").toString().isEmpty()) fromDocNo = Double.parseDouble(payload.get("fromDocNo").toString());
            if (payload.get("toDocNo") != null && !payload.get("toDocNo").toString().isEmpty()) toDocNo = Double.parseDouble(payload.get("toDocNo").toString());
        }

        return service.getHistory(orgId, compId, yearId, fromDate, toDate, fromDocNo, toDocNo);
    }

    @PostMapping("/delete/{id}")
    public Map<String, Object> deleteRecord(@PathVariable Integer id) {
        int userId = currentUserContext.currentUserId();
        return service.deleteRecord(id, userId);
    }
}
