package com.mst.controllers;

import com.mst.models.InwardGatePass;
import com.mst.security.CurrentUserContext;
import com.mst.services.InwardGatePassService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/inward-gate-pass")
public class InwardGatePassRestController {

    @Autowired
    private InwardGatePassService service;

    @Autowired
    private CurrentUserContext currentUserContext;

    @GetMapping("/dropdowns")
    public Map<String, Object> getDropdowns() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return service.getDropdowns(orgId, compId);
    }

    @GetMapping("/generate-no")
    public Map<String, Object> generateNextNumbers(
            @RequestParam(defaultValue = "51") Integer docTypeId,
            @RequestParam(defaultValue = "Paddy") String gatepassType) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int yearId = currentUserContext.currentFinancialYearId();
        return service.generateNextNumbers(orgId, compId, branchId, yearId, docTypeId, gatepassType);
    }

    @GetMapping("/{id}")
    public Map<String, Object> getById(@PathVariable Integer id) {
        return service.getById(id);
    }

    @PostMapping("/save")
    public Map<String, Object> saveRecord(@RequestBody InwardGatePass obj) {
        if (obj.getOrganizationId() == null || obj.getOrganizationId() == 0) {
            obj.setOrganizationId(currentUserContext.currentOrganizationId());
        }
        if (obj.getCompanyId() == null || obj.getCompanyId() == 0) {
            obj.setCompanyId(currentUserContext.currentCompanyId());
        }
        if (obj.getBranchesId() == null || obj.getBranchesId() == 0) {
            obj.setBranchesId(currentUserContext.currentBranchId());
        }
        if (obj.getFinancialYearId() == null || obj.getFinancialYearId() == 0) {
            obj.setFinancialYearId(currentUserContext.currentFinancialYearId());
        }
        if (obj.getEntryUser() == null || obj.getEntryUser() == 0) {
            obj.setEntryUser(currentUserContext.currentUserId());
        }
        return service.saveRecord(obj);
    }

    @PostMapping("/delete/{id}")
    public Map<String, Object> deleteRecord(@PathVariable Integer id) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return service.deleteRecord(id, orgId, compId);
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

        Integer docTypeId = 51;
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
}
