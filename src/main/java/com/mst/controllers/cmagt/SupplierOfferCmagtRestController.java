package com.mst.controllers.cmagt;

import com.mst.models.cmagt.dto.SupplierOfferCmagtDto;
import com.mst.security.CurrentUserContext;
import com.mst.services.cmagt.SupplierOfferCmagtService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/commission/supplier-offer")
public class SupplierOfferCmagtRestController {

    @Autowired
    private SupplierOfferCmagtService service;

    @Autowired
    private CurrentUserContext currentUserContext;

    @GetMapping("/generate-no")
    public Map<String, Object> generateNextNo(@RequestParam(defaultValue = "1051") Integer docTypeId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int yearId = currentUserContext.currentFinancialYearId();
        int docNo = service.generateNextDocNo(orgId, compId, branchId, yearId, docTypeId);
        Map<String, Object> res = new HashMap<>();
        res.put("docNo", docNo);
        return res;
    }

    @GetMapping("/{id}")
    public Map<String, Object> getById(@PathVariable Integer id) {
        return service.getById(id);
    }

    @PostMapping("/save")
    public Map<String, Object> saveRecord(@RequestBody SupplierOfferCmagtDto dto) {
        if (dto.getOrganizationId() == null || dto.getOrganizationId() == 0) {
            dto.setOrganizationId(currentUserContext.currentOrganizationId());
        }
        if (dto.getCompanyId() == null || dto.getCompanyId() == 0) {
            dto.setCompanyId(currentUserContext.currentCompanyId());
        }
        if (dto.getBranchId() == null || dto.getBranchId() == 0) {
            dto.setBranchId(currentUserContext.currentBranchId());
        }
        if (dto.getFinancialYearId() == null || dto.getFinancialYearId() == 0) {
            dto.setFinancialYearId(currentUserContext.currentFinancialYearId());
        }
        if (dto.getEntryUserId() == null || dto.getEntryUserId() == 0) {
            dto.setEntryUserId(currentUserContext.currentUserId());
        }
        return service.saveRecord(dto);
    }

    @PostMapping("/delete/{id}")
    public Map<String, Object> deleteRecord(@PathVariable Integer id) {
        int userId = currentUserContext.currentUserId();
        return service.deleteRecord(userId, id);
    }

    @RequestMapping(value = "/history", method = {RequestMethod.GET, RequestMethod.POST})
    public List<Map<String, Object>> getHistory(
            @RequestBody(required = false) Map<String, Object> payload,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) Integer fromDocNo,
            @RequestParam(required = false) Integer toDocNo,
            @RequestParam(required = false) Integer commissionAgentId,
            @RequestParam(required = false) Integer supplierId,
            @RequestParam(required = false) Integer itemId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int yearId = currentUserContext.currentFinancialYearId();
        int userId = currentUserContext.currentUserId();

        if (payload != null) {
            if (payload.get("fromDate") != null) fromDate = payload.get("fromDate").toString();
            if (payload.get("toDate") != null) toDate = payload.get("toDate").toString();
            if (payload.get("fromDocNo") != null && !payload.get("fromDocNo").toString().isEmpty()) {
                fromDocNo = Integer.parseInt(payload.get("fromDocNo").toString());
            }
            if (payload.get("toDocNo") != null && !payload.get("toDocNo").toString().isEmpty()) {
                toDocNo = Integer.parseInt(payload.get("toDocNo").toString());
            }
            if (payload.get("commissionAgentId") != null && !payload.get("commissionAgentId").toString().isEmpty()) {
                commissionAgentId = Integer.parseInt(payload.get("commissionAgentId").toString());
            }
            if (payload.get("supplierId") != null && !payload.get("supplierId").toString().isEmpty()) {
                supplierId = Integer.parseInt(payload.get("supplierId").toString());
            }
            if (payload.get("itemId") != null && !payload.get("itemId").toString().isEmpty()) {
                itemId = Integer.parseInt(payload.get("itemId").toString());
            }
        }

        return service.getHistory(orgId, compId, branchId, yearId, true, userId, fromDate, toDate, fromDocNo, toDocNo, null, commissionAgentId, supplierId, itemId);
    }
}
