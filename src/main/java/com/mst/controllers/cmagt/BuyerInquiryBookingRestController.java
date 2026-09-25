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
        /* The form's own DocumentTypeId (frmBuyerInquiryBooking.cs:349); the request parameter is
           kept for URL compatibility but never trusted. */
        int docNo = service.generateNextDocNo(orgId, compId, branchId, yearId,
                BuyerInquiryBookingService.DOCUMENT_TYPE_ID);
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

    /** FilldtLastAnalysisByParentItem (:991). The literal path takes precedence over /{id}. */
    @GetMapping("/last-analysis")
    public List<Map<String, Object>> lastAnalysis(@RequestParam(defaultValue = "0") int parentCategoryId) {
        return service.lastAnalysisByParentCategory(parentCategoryId);
    }

    /**
     * HistoryFill() (:2081). Filters may come as query parameters (what the page sends) or as a
     * JSON body; tenancy, CanViewAllRecord and the user are always taken from the session.
     */
    @RequestMapping(value = "/history", method = {RequestMethod.GET, RequestMethod.POST})
    public List<Map<String, Object>> getHistory(
            @RequestBody(required = false) Map<String, Object> payload,
            @RequestParam Map<String, String> query) {
        Map<String, Object> filters = new HashMap<>();
        if (query != null) filters.putAll(query);
        if (payload != null) filters.putAll(payload);
        return service.getHistory(filters);
    }
}
