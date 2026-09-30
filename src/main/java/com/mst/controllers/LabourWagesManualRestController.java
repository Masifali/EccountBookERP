package com.mst.controllers;

import com.mst.services.LabourWagesManualService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.concurrent.Callable;

/**
 * Labour Wages Manual (frmWagesBillManual, screen 187, DocumentTypeId 810).
 * See {@link LabourWagesManualService} for the desktop methods and procedures behind each call.
 */
@RestController
@RequestMapping("/api/contractor-wages/labour-wages-manual")
public class LabourWagesManualRestController {

    private static final Logger LOG = LoggerFactory.getLogger(LabourWagesManualRestController.class);

    @Autowired
    private LabourWagesManualService service;

    /** frmWagesBillManualLoad */
    @GetMapping("/load")
    public ResponseEntity<?> load() { return run(service::load, "The screen could not be loaded."); }

    /** btnRefresh_Click */
    @GetMapping("/refresh")
    public ResponseEntity<?> refresh() { return run(service::refresh, "Refresh failed."); }

    /** GenerateDocNo (FormRest) */
    @GetMapping("/doc-no")
    public ResponseEntity<?> docNo() {
        return run(() -> Map.of("docNo", service.generateDocNo()), "Doc no could not be generated.");
    }

    /** DocumentTypeFill (History Refresh) */
    @GetMapping("/document-types")
    public ResponseEntity<?> documentTypes() { return run(service::documentTypes, "Document types could not be read."); }

    /** JobOrderNoFill (Document Type 112 / 80) */
    @GetMapping("/job-orders")
    public ResponseEntity<?> jobOrders() { return run(service::jobOrders, "Job orders could not be read."); }

    /** CommonServices.CheckItemsFreeofcostforWages */
    @GetMapping("/free-of-cost")
    public ResponseEntity<?> freeOfCost(@RequestParam(required = false) String docDate,
                                        @RequestParam(defaultValue = "0") int refDocumentTypeId,
                                        @RequestParam(defaultValue = "0") int itemId,
                                        @RequestParam(defaultValue = "0") int wagesAccountId) {
        return run(() -> Map.of("freeOfCost", service.freeOfCost(docDate, refDocumentTypeId, itemId, wagesAccountId)),
                "Free-of-cost check failed.");
    }

    /** CommonServices.GetWagesRate */
    @GetMapping("/wages-rate")
    public ResponseEntity<?> wagesRate(@RequestParam(required = false) String docDate,
                                       @RequestParam(defaultValue = "0") double packSize,
                                       @RequestParam(defaultValue = "0") int wagesAccountId,
                                       @RequestParam(defaultValue = "0") int contractorId) {
        return run(() -> service.wagesRate(docDate, packSize, wagesAccountId, contractorId), "Wages rate lookup failed.");
    }

    /** ReadById / HistoryDetailGridBind */
    @GetMapping("/{id}")
    public ResponseEntity<?> getById(@PathVariable int id) { return run(() -> service.getById(id), "Record Not found"); }

    /** bindHistory */
    @GetMapping("/history")
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(required = false) String fromDocNo,
                                     @RequestParam(required = false) String toDocNo,
                                     @RequestParam(defaultValue = "0") int refDocumentTypeId) {
        return run(() -> service.history(fromDate, toDate, fromDocNo, toDocNo, refDocumentTypeId), "History could not be read.");
    }

    /** ContractorWagesBillManualSlip004 - checks before the Crystal viewer. */
    @GetMapping("/slip-check/{id}")
    public ResponseEntity<?> slipCheck(@PathVariable int id) { return run(() -> service.slipCheck(id), "No Record Found For Display"); }

    /** VoucherReport_118(VoucherHeadIdGet(Id, 810), 810) - checks before the Crystal viewer. */
    @GetMapping("/voucher-check/{id}")
    public ResponseEntity<?> voucherCheck(@PathVariable int id) { return run(() -> service.voucherCheck(id), "VoucherId Not Found"); }

    /** Insert (Save / Update - Id decides, as the BLL's Save does). */
    @PostMapping("/save")
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return run(() -> service.save(body), "Save failed."); }

    private static ResponseEntity<?> run(Callable<?> body, String fallback) {
        try {
            return ResponseEntity.ok(body.call());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException | AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            LOG.warn("Labour Wages Manual: {}", fallback, e);
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, fallback)));
        }
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }

    private static String msg(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        return (m == null || m.trim().isEmpty()) ? fallback : m;
    }
}
