package com.mst.controllers;

import com.mst.security.CurrentUserContext;

import com.mst.services.PurchaseInvoiceFullService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/purchase/purchase-invoice-full")
public class PurchaseInvoiceFullRestController {

    @Autowired
    private PurchaseInvoiceFullService purchaseInvoiceFullService;

    @Autowired
    private CurrentUserContext currentUserContext;

    @GetMapping("/dropdowns")
    public ResponseEntity<?> getDropdowns() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return ResponseEntity.ok(purchaseInvoiceFullService.getDropdowns(orgId, compId));
    }

    @GetMapping("/next-code")
    public ResponseEntity<?> getNextCode(@RequestParam(required = false, defaultValue = "56") int docTypeId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int yearId = currentUserContext.currentFinancialYearId();
        int nextNo = purchaseInvoiceFullService.generateNextDocNo(orgId, compId, branchId, yearId, docTypeId);
        Map<String, Object> res = new HashMap<>();
        res.put("docNo", nextNo);
        return ResponseEntity.ok(res);
    }

    @RequestMapping(value = "/history", method = {RequestMethod.GET, RequestMethod.POST})
    public ResponseEntity<?> getHistory(@RequestBody(required = false) Map<String, Object> bodyParams,
                                         @RequestParam(required = false, defaultValue = "56") int docTypeId,
                                         @RequestParam(required = false) String fromDate,
                                         @RequestParam(required = false) String toDate,
                                         @RequestParam(required = false) Integer supplierId,
                                         @RequestParam(required = false) Integer fromDocNo,
                                         @RequestParam(required = false) Integer toDocNo,
                                         @RequestParam(required = false) String dateType) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int yearId = currentUserContext.currentFinancialYearId();

        if (bodyParams != null) {
            if (bodyParams.get("docTypeId") != null) docTypeId = ((Number) bodyParams.get("docTypeId")).intValue();
            if (fromDate == null && bodyParams.get("fromDate") != null) fromDate = bodyParams.get("fromDate").toString();
            if (toDate == null && bodyParams.get("toDate") != null) toDate = bodyParams.get("toDate").toString();
            if (supplierId == null && bodyParams.get("supplierId") != null) supplierId = ((Number) bodyParams.get("supplierId")).intValue();
            if (fromDocNo == null && bodyParams.get("fromDocNo") != null) fromDocNo = ((Number) bodyParams.get("fromDocNo")).intValue();
            if (toDocNo == null && bodyParams.get("toDocNo") != null) toDocNo = ((Number) bodyParams.get("toDocNo")).intValue();
            if (dateType == null && bodyParams.get("dateType") != null) dateType = bodyParams.get("dateType").toString();
        }

        List<Map<String, Object>> history = purchaseInvoiceFullService.getHistory(orgId, compId, branchId, yearId, docTypeId, fromDate, toDate, supplierId, fromDocNo, toDocNo, dateType);
        return ResponseEntity.ok(history);
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getById(@PathVariable("id") int id) {
        Map<String, Object> data = purchaseInvoiceFullService.getById(id);
        if (data != null) return ResponseEntity.ok(data);
        return ResponseEntity.notFound().build();
    }

    @PostMapping("/save")
    public ResponseEntity<?> save(@RequestBody Map<String, Object> payload) {
        payload.put("organizationId", currentUserContext.currentOrganizationId());
        payload.put("companyId", currentUserContext.currentCompanyId());
        payload.put("branchesId", currentUserContext.currentBranchId());
        payload.put("financialYearId", currentUserContext.currentFinancialYearId());

        Map<String, Object> res = purchaseInvoiceFullService.savePurchaseInvoice(payload);
        if (Boolean.TRUE.equals(res.get("success"))) {
            return ResponseEntity.ok(res);
        } else {
            return ResponseEntity.status(400).body(res);
        }
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> delete(@PathVariable("id") int id) {
        boolean ok = purchaseInvoiceFullService.deletePurchaseInvoice(id);
        Map<String, Object> res = new HashMap<>();
        res.put("success", ok);
        res.put("message", ok ? "Deleted successfully" : "Failed to delete");
        return ResponseEntity.ok(res);
    }
}
