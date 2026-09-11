package com.mst.controllers;

import com.mst.security.CurrentUserContext;

import com.mst.services.GrnDirectAgainstOrderService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/grn-direct-against-order")
public class GrnDirectAgainstOrderRestController {

    @Autowired
    private GrnDirectAgainstOrderService service;

    @Autowired
    private CurrentUserContext currentUserContext;

    @GetMapping("/generate-no")
    public Map<String, Object> generateNextNumbers(@RequestParam(defaultValue = "169") Integer docTypeId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int yearId = currentUserContext.currentFinancialYearId();
        return service.generateNextNumbers(orgId, compId, yearId, docTypeId);
    }

    @GetMapping("/transporters")
    public List<Map<String, Object>> getTransporters() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return service.getTransporters(orgId, compId);
    }

    @GetMapping("/orders-by-supplier")
    public List<Map<String, Object>> getOrdersBySupplier(@RequestParam Integer supplierId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return service.getOrdersBySupplier(orgId, compId, supplierId);
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
            @RequestParam(required = false) Double toDocNo,
            @RequestParam(required = false) Integer supplierId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int yearId = currentUserContext.currentFinancialYearId();

        Integer docTypeId = 169;
        if (payload != null) {
            if (payload.get("documentTypeId") != null) docTypeId = Integer.parseInt(payload.get("documentTypeId").toString());
            if (payload.get("fromDate") != null) fromDate = payload.get("fromDate").toString();
            if (payload.get("toDate") != null) toDate = payload.get("toDate").toString();
            if (payload.get("fromDocNo") != null && !payload.get("fromDocNo").toString().isEmpty()) fromDocNo = Double.parseDouble(payload.get("fromDocNo").toString());
            if (payload.get("toDocNo") != null && !payload.get("toDocNo").toString().isEmpty()) toDocNo = Double.parseDouble(payload.get("toDocNo").toString());
            if (payload.get("supplierId") != null && !payload.get("supplierId").toString().isEmpty()) supplierId = Integer.parseInt(payload.get("supplierId").toString());
        }

        return service.getHistory(orgId, compId, branchId, yearId, docTypeId, fromDate, toDate, fromDocNo, toDocNo, supplierId);
    }

    @PostMapping("/delete/{id}")
    public Map<String, Object> deleteRecord(@PathVariable Integer id) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return service.deleteRecord(id, orgId, compId);
    }
}
