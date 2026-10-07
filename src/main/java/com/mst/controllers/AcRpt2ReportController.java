package com.mst.controllers;

import com.mst.services.AcRpt2ReportService;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

/** R2 account reports 72 / 75 / 77 / 870 - REST endpoints used by the four new pages (tenancy from CurrentUserContext). */
@RestController
@RequestMapping("/api/accounts/acrpt2")
public class AcRpt2ReportController {
    private final AcRpt2ReportService service;

    public AcRpt2ReportController(AcRpt2ReportService service) { this.service = service; }

    // 72 FCYPayablesAndReceivablesRpt
    @GetMapping("/fcy/lookups")
    public ResponseEntity<?> fcyLookups() { return run(() -> service.fcyLookups()); }

    @GetMapping("/fcy/data")
    public ResponseEntity<?> fcyData(@RequestParam(required = false) String fromDate, @RequestParam String toDate,
                                     @RequestParam(defaultValue = "0") int currencyId) {
        return run(() -> service.fcyData(fromDate, toDate, currencyId));
    }

    @GetMapping("/fcy/print-rows")
    public ResponseEntity<?> fcyPrintRows(@RequestParam String fromDate, @RequestParam String toDate,
                                          @RequestParam(defaultValue = "1") int reportTypeId) {
        return run(() -> service.fcyPrintRows(fromDate, toDate, reportTypeId));
    }

    // 75 DueDateAnalysisPayablesAndReceivablesForcast
    @GetMapping("/forecast/lookups")
    public ResponseEntity<?> forecastLookups() { return run(() -> service.forecastLookups()); }

    @GetMapping("/forecast/data")
    public ResponseEntity<?> forecast(@RequestParam String fromDate, @RequestParam(required = false) String toDate,
                                      @RequestParam(defaultValue = "0") int intervalDays,
                                      @RequestParam(defaultValue = "0") int sortNo) {
        return run(() -> service.forecast(fromDate, toDate, intervalDays, sortNo));
    }

    // 77 PayablesandReceivablesAging
    @GetMapping("/aging/lookups")
    public ResponseEntity<?> agingLookups() { return run(() -> service.agingLookups()); }

    @GetMapping("/aging/data")
    public ResponseEntity<?> aging(@RequestParam String toDate, @RequestParam(defaultValue = "0") int intervalDays,
                                   @RequestParam(defaultValue = "0") int notEqualTo,
                                   @RequestParam(defaultValue = "0") int classId,
                                   @RequestParam(defaultValue = "0") int typeId) {
        return run(() -> service.aging(toDate, intervalDays, notEqualTo, classId, typeId));
    }

    // 870 frmPayablesAndReceivablesWithPaymentAndReceipts
    @GetMapping("/pr/lookups")
    public ResponseEntity<?> prLookups() { return run(() -> service.prLookups()); }

    @GetMapping("/pr/data")
    public ResponseEntity<?> prData(@RequestParam String fromDate, @RequestParam String toDate,
                                    @RequestParam(defaultValue = "") String branchIds,
                                    @RequestParam(defaultValue = "0") int classId,
                                    @RequestParam(defaultValue = "0") int customGroupId,
                                    @RequestParam(defaultValue = "0") int parentId,
                                    @RequestParam(defaultValue = "") String city) {
        return run(() -> service.prData(fromDate, toDate, branchIds, classId, customGroupId, parentId, city));
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c) {
        try {
            Object body = c.get();
            return ResponseEntity.ok(body);
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
