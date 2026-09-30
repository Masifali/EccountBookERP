package com.mst.controllers;

import com.mst.security.CurrentUserContext;
import com.mst.services.SaleReturnGrnService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.util.*;

/** SaleReturnGrn.cs (screen 866, DocumentTypeId 143). */
@RestController
@RequestMapping("/api/purchase/sale-return-grn")
public class SaleReturnGrnRestController {

    @Autowired
    private SaleReturnGrnService saleReturnGrnService;

    @Autowired
    private CurrentUserContext currentUserContext;

    /** GatePassRecordFill :2955 - USP_LoadGpDataForSaleReturnGRN. An empty body means the procedure returned no row. */
    @GetMapping("/gate-passes/{id}")
    public ResponseEntity<?> gatePass(@PathVariable int id) {
        Map<String,Object> row=saleReturnGrnService.gatePass(id);
        return ResponseEntity.ok(row==null?Map.of():row);
    }

    /** CmbSupplierName_Leave :1105 - USP_GetItemsFromSaleInvoiceAgainstPartyId (95,99). */
    @GetMapping("/items")
    public ResponseEntity<?> items(@RequestParam int supplierId) { return ResponseEntity.ok(lookups.returnItems(supplierId)); }

    @Autowired private com.mst.repositories.PurchaseGrnLookupRepository lookups;

    /** btnLoadGdn_Click :3969 - frmLoadGdnForGrnSaleReturn data. */
    @GetMapping("/pending-gdn")
    public ResponseEntity<?> pendingGdn(@RequestParam(required = false) String fromDate,
                                        @RequestParam(required = false) String toDate,
                                        @RequestParam(required = false) Integer customerId) {
        return ResponseEntity.ok(saleReturnGrnService.pendingGdn(fromDate,toDate,customerId));
    }

    /** reset() :1755 - GatepassGridFill(GatepassDataDbCall()). */
    @GetMapping("/pending-gate-passes")
    public ResponseEntity<?> pendingGatePasses() { return ResponseEntity.ok(saleReturnGrnService.pendingGatePasses()); }

    /** btnRefreshHistory_Click :3351 - history supplier combo. */
    @GetMapping("/history-suppliers")
    public ResponseEntity<?> historySuppliers() { return ResponseEntity.ok(saleReturnGrnService.historySuppliers()); }

    @GetMapping("/dropdowns")
    public ResponseEntity<?> getDropdowns() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        return ResponseEntity.ok(saleReturnGrnService.getDropdowns(orgId, compId));
    }

    @GetMapping("/next-code")
    public ResponseEntity<?> getNextCode() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int branchId = currentUserContext.currentBranchId();
        int yearId = currentUserContext.currentFinancialYearId();
        int nextNo = saleReturnGrnService.generateNextDocNo(orgId, compId, branchId, yearId);
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

        List<Map<String, Object>> history = saleReturnGrnService.getHistory(orgId, compId, branchId, yearId, fromDate, toDate, supplierId, fromDocNo, toDocNo, dateType);
        return ResponseEntity.ok(history);
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getById(@PathVariable("id") int id) {
        return ResponseEntity.ok(saleReturnGrnService.getById(id));
    }

    /** Tenancy is added from the session inside the shared persistence service, never from the request. */
    @PostMapping("/save")
    public ResponseEntity<?> save(@RequestBody Map<String, Object> payload) {
        return ResponseEntity.ok(saleReturnGrnService.saveSaleReturnGrn(payload));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> delete(@PathVariable("id") int id) {
        saleReturnGrnService.deleteSaleReturnGrn(id);
        Map<String, Object> res = new HashMap<>();
        res.put("success", true);
        res.put("message", "Delete Record Seccessfully"); // btnDelete_Click :1579 (desktop spelling)
        return ResponseEntity.ok(res);
    }

    /** Procedure RAISERROR text (error 50000) is the desktop's MessageBox text; other SQL faults stay in the log. */
    @ExceptionHandler(org.springframework.dao.DataAccessException.class)
    public ResponseEntity<?> databaseFailure(org.springframework.dao.DataAccessException failure) {
        Throwable cause = failure.getMostSpecificCause();
        if (cause instanceof java.sql.SQLException && (((java.sql.SQLException) cause).getErrorCode() == 50000 || ((java.sql.SQLException) cause).getErrorCode() == 50001))
            return ResponseEntity.badRequest().body(Map.of("success", false, "message", cause.getMessage()));
        org.slf4j.LoggerFactory.getLogger(SaleReturnGrnRestController.class).error("Sale Return GRN database operation failed", failure);
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(Map.of("success", false, "message", "The database request failed. No changes were saved."));
    }

    @ExceptionHandler(ResponseStatusException.class)
    public ResponseEntity<?> refused(ResponseStatusException error) {
        return ResponseEntity.status(error.getRawStatusCode()).body(Map.of("success", false, "message", error.getReason() == null ? "This action is unavailable" : error.getReason()));
    }
}
