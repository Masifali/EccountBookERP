package com.mst.controllers;

import com.mst.services.AcRpt4ReportService;
import java.util.LinkedHashMap;
import java.util.Map;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

/** R4 reports 56 / 76 / 87 / 932 / 933 - REST endpoints used by the five new pages (tenancy from CurrentUserContext). */
@RestController
@RequestMapping("/api/accounts/acrpt4")
public class AcRpt4ReportController {
    private final AcRpt4ReportService service;

    public AcRpt4ReportController(AcRpt4ReportService service) { this.service = service; }

    // 56 AuditDashboard
    @GetMapping("/audit/init")
    public ResponseEntity<?> auditInit() { return run(() -> service.auditInit()); }

    @GetMapping("/audit/data")
    public ResponseEntity<?> auditData(@RequestParam String fromDate, @RequestParam String toDate) {
        return run(() -> service.auditData(fromDate, toDate));
    }

    // 76 PdcInventoryReport
    @GetMapping("/pdc/init")
    public ResponseEntity<?> pdcInit() { return run(() -> service.pdcInit()); }

    @GetMapping("/pdc/data")
    public ResponseEntity<?> pdcData(@RequestParam(defaultValue = "0") int accountId,
                                     @RequestParam(required = false) String dateFrom, @RequestParam(required = false) String dateTo,
                                     @RequestParam(required = false) String chqFrom, @RequestParam(required = false) String chqTo,
                                     @RequestParam(defaultValue = "") String status) {
        return run(() -> service.pdcData(accountId, dateFrom, dateTo, chqFrom, chqTo, status));
    }

    // 87 frmPostDatedChequeReports
    @GetMapping("/pchq/init")
    public ResponseEntity<?> pchqInit() { return run(() -> service.pchqInit()); }

    @GetMapping("/pchq/data")
    public ResponseEntity<?> pchqData(@RequestParam String fromDate, @RequestParam(required = false) String toDate,
                                      @RequestParam(defaultValue = "0") int bankId, @RequestParam(defaultValue = "0") int accountId,
                                      @RequestParam(defaultValue = "0") int chqNoFrom, @RequestParam(defaultValue = "0") int chqNoTo,
                                      @RequestParam(defaultValue = "false") boolean approved) {
        return run(() -> service.pchqData(fromDate, toDate, bankId, accountId, chqNoFrom, chqNoTo, approved));
    }

    // 932 frmAuditByWeightReport
    @GetMapping("/weight/init")
    public ResponseEntity<?> weightInit() { return run(() -> service.weightInit()); }

    @GetMapping("/weight/data")
    public ResponseEntity<?> weightData(@RequestParam(defaultValue = "0") int invoiceId, @RequestParam(defaultValue = "0") int contractId,
                                        @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                        @RequestParam(defaultValue = "") String status) {
        return run(() -> service.weightData(invoiceId, contractId, fromDate, toDate, status));
    }

    // 933 frmGrnAudit_History
    @GetMapping("/grn/init")
    public ResponseEntity<?> grnInit() { return run(() -> service.grnInit()); }

    @GetMapping("/grn/data")
    public ResponseEntity<?> grnData(@RequestParam String fromDate, @RequestParam String toDate,
                                     @RequestParam(defaultValue = "0") int parentCategoryId, @RequestParam(defaultValue = "0") int purchaseTypeId,
                                     @RequestParam(defaultValue = "0") int orderId, @RequestParam(defaultValue = "0") int supplierId,
                                     @RequestParam(defaultValue = "0") int cityId,
                                     @RequestParam(defaultValue = "false") boolean onlyDiscounted,
                                     @RequestParam(defaultValue = "false") boolean freightByCity) {
        return run(() -> service.grnData(fromDate, toDate, parentCategoryId, purchaseTypeId, orderId, supplierId, cityId, onlyDiscounted, freightByCity));
    }

    @GetMapping("/grn/wb-slip")
    public ResponseEntity<?> grnWbSlip(@RequestParam int gpId) { return run(() -> service.grnWbSlip(gpId)); }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return fail(400, e);
        } catch (SecurityException e) {
            return fail(403, e);
        } catch (Exception e) {
            Throwable t = e;
            while (t.getCause() != null && t.getCause() != t) t = t.getCause();
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("message", t.getMessage() == null ? t.getClass().getSimpleName() : t.getMessage());
            return ResponseEntity.status(500).body(m);
        }
    }

    private static ResponseEntity<?> fail(int status, Exception e) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("message", e.getMessage() == null ? e.getClass().getSimpleName() : e.getMessage());
        return ResponseEntity.status(status).body(m);
    }
}
