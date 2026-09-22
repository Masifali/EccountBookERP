package com.mst.controllers.cmagt;

import com.mst.models.cmagt.dto.BuyerInquiryBookingDto;
import com.mst.security.CurrentUserContext;
import com.mst.services.cmagt.BuyerInquiryBookingService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/commission/buyer-inquiry-booking")
public class BuyerInquiryBookingRestController {

    @Autowired
    private BuyerInquiryBookingService service;

    @Autowired
    private CurrentUserContext currentUserContext;

    @GetMapping("/generate-no")
    public Map<String, Object> generateNextNo(@RequestParam(defaultValue = "1050") Integer docTypeId) {
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

    /**
     * Tenancy and authorship are set UNCONDITIONALLY from the authenticated session.
     *
     * They used to be applied only when the posted value was null or zero, which meant a caller
     * could write into another organization, company or branch, or attribute the document to
     * another user, simply by putting an id in the body. The desktop reads UserAccount and
     * clsGlobalVariables.ActiveYr and offers no override; this is that, enforced.
     *
     * DocumentTypeId is likewise fixed at the form's own 1050 (frmBuyerInquiryBooking.cs:349)
     * and never taken from the request.
     */
    @PostMapping("/save")
    public Map<String, Object> saveRecord(@RequestBody BuyerInquiryBookingDto dto) {
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
            @RequestParam(required = false) Integer buyerId,
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
            if (payload.get("buyerId") != null && !payload.get("buyerId").toString().isEmpty()) {
                buyerId = Integer.parseInt(payload.get("buyerId").toString());
            }
            if (payload.get("itemId") != null && !payload.get("itemId").toString().isEmpty()) {
                itemId = Integer.parseInt(payload.get("itemId").toString());
            }
        }

        return service.getHistory(orgId, compId, branchId, yearId, true, userId, fromDate, toDate, fromDocNo, toDocNo, null, commissionAgentId, buyerId, itemId);
    }
}
