package com.mst.controllers;

import com.mst.security.CurrentUserContext;

import com.mst.services.MarketGrnService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.*;

@RestController
@RequestMapping("/api/purchase/market-grn")
public class MarketGrnRestController {

    @Autowired
    private MarketGrnService marketGrnService;

    @Autowired
    private CurrentUserContext currentUserContext;

    @Autowired private com.mst.repositories.PurchaseGrnLookupRepository lookups;
    @Autowired private com.mst.services.PurchaseGrnSupplementService supplements;

    @Autowired private com.mst.repositories.PurchaseGrnFormRepository form;

    /* InvFrmGRN form-time lookups — see PurchaseGrnFormRepository for the desktop line of each. */
    @GetMapping("/form/config")
    public ResponseEntity<?> formConfig() { return ResponseEntity.ok(form.config()); }

    @GetMapping("/form/pending")
    public ResponseEntity<?> formPending() { return ResponseEntity.ok(form.pendingRows()); }

    @GetMapping("/form/load/{gpId}")
    public ResponseEntity<?> formLoad(@PathVariable int gpId) { return ResponseEntity.ok(form.load(gpId,form.configValues())); }

    @GetMapping("/form/items/{gpId}")
    public ResponseEntity<?> formItems(@PathVariable int gpId) { return ResponseEntity.ok(form.items(gpId)); }

    @GetMapping("/form/lab")
    public ResponseEntity<?> formLab(@RequestParam int gpId,@RequestParam int itemId) { return ResponseEntity.ok(form.lab(gpId,itemId)); }

    @GetMapping("/form/lab-parameters/{labId}")
    public ResponseEntity<?> formLabParameters(@PathVariable int labId) { return ResponseEntity.ok(form.labParameters(labId)); }

    @GetMapping("/form/pre-bills")
    public ResponseEntity<?> formPreBills(@RequestParam(defaultValue="0") int supplierId,@RequestParam(defaultValue="0") int grnId,
                                          @RequestParam(defaultValue="0") int gpId,@RequestParam(defaultValue="0") int orderId) {
        return ResponseEntity.ok(form.preBills(supplierId,grnId,gpId,orderId));
    }

    @GetMapping("/form/purchase-order/{orderId}")
    public ResponseEntity<?> formPurchaseOrder(@PathVariable int orderId) { return ResponseEntity.ok(form.purchaseOrder(orderId)); }

    @GetMapping("/form/received-weight/{gpId}")
    public ResponseEntity<?> formReceivedWeight(@PathVariable int gpId) { return ResponseEntity.ok(form.receivedWeight(gpId)); }

    @GetMapping("/form/previous-data/{gpId}")
    public ResponseEntity<?> formPreviousData(@PathVariable int gpId,@RequestParam(defaultValue="0") int recId) { return ResponseEntity.ok(form.previousData(gpId,recId)); }

    @GetMapping("/form/deduction-policy")
    public ResponseEntity<?> formDeductionPolicy(@RequestParam(required=false) String date,@RequestParam double difference) {
        return ResponseEntity.ok(form.deductionPolicy(date,difference));
    }

    @PostMapping("/calculate-breakups")
    public ResponseEntity<?> calculateBreakups(@RequestBody Map<String,Object> payload) { return ResponseEntity.ok(supplements.calculate(payload)); }

    @ExceptionHandler(IllegalArgumentException.class)
    public ResponseEntity<?> invalidInput(IllegalArgumentException failure) { return ResponseEntity.badRequest().body(Map.of("success",false,"message",failure.getMessage())); }

    @GetMapping("/gate-passes/{id}")
    public ResponseEntity<?> gatePass(@PathVariable int id) { return ResponseEntity.ok(lookups.gatePass(id,46)); }

    @GetMapping("/dropdowns")
    public ResponseEntity<?> getDropdowns() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return ResponseEntity.ok(marketGrnService.getDropdowns(orgId, compId));
    }

    @GetMapping("/next-code")
    public ResponseEntity<?> getNextCode() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int yearId = currentUserContext.currentFinancialYearId();
        int nextNo = marketGrnService.generateNextDocNo(orgId, compId, branchId, yearId);
        Map<String, Object> res = new HashMap<>();
        res.put("docNo", nextNo);
        return ResponseEntity.ok(res);
    }

    @RequestMapping(value = "/history", method = {RequestMethod.GET, RequestMethod.POST})
    public ResponseEntity<?> getHistory(@RequestBody(required = false) Map<String, Object> bodyParams,
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
            if (fromDate == null && bodyParams.get("fromDate") != null) fromDate = bodyParams.get("fromDate").toString();
            if (toDate == null && bodyParams.get("toDate") != null) toDate = bodyParams.get("toDate").toString();
            if (supplierId == null && bodyParams.get("supplierId") != null) supplierId = ((Number) bodyParams.get("supplierId")).intValue();
            if (fromDocNo == null && bodyParams.get("fromDocNo") != null) fromDocNo = ((Number) bodyParams.get("fromDocNo")).intValue();
            if (toDocNo == null && bodyParams.get("toDocNo") != null) toDocNo = ((Number) bodyParams.get("toDocNo")).intValue();
            if (dateType == null && bodyParams.get("dateType") != null) dateType = bodyParams.get("dateType").toString();
        }

        List<Map<String, Object>> history = marketGrnService.getHistory(orgId, compId, branchId, yearId, fromDate, toDate, supplierId, fromDocNo, toDocNo, dateType);
        return ResponseEntity.ok(history);
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getById(@PathVariable("id") int id) {
        Map<String, Object> data = marketGrnService.getById(id);
        if (data != null) return ResponseEntity.ok(data);
        return ResponseEntity.notFound().build();
    }

    @PostMapping("/save")
    public ResponseEntity<?> save(@RequestBody Map<String, Object> payload) {
        payload.put("organizationId", currentUserContext.currentOrganizationId());
        payload.put("companyId", currentUserContext.currentCompanyId());
        payload.put("branchesId", currentUserContext.currentBranchId());
        payload.put("financialYearId", currentUserContext.currentFinancialYearId());

        Map<String, Object> res = marketGrnService.saveMarketGrn(payload);
        if (Boolean.TRUE.equals(res.get("success"))) {
            return ResponseEntity.ok(res);
        } else {
            return ResponseEntity.status(400).body(res);
        }
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> delete(@PathVariable("id") int id) {
        boolean ok = marketGrnService.deleteMarketGrn(id);
        Map<String, Object> res = new HashMap<>();
        res.put("success", ok);
        res.put("message", ok ? "Deleted successfully" : "Failed to delete");
        return ResponseEntity.ok(res);
    }
    @ExceptionHandler(org.springframework.dao.DataAccessException.class)
    public ResponseEntity<?> databaseFailure(org.springframework.dao.DataAccessException failure) {
        return ResponseEntity.badRequest().body(Map.of("success",false,"message",failure.getMostSpecificCause().getMessage()));
    }
}
