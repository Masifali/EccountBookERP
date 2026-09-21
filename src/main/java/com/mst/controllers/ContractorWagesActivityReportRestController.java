package com.mst.controllers;

import com.mst.services.ContractorWagesActivityReportService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.Map;

/**
 * "Wages Report (With Activities)" - Architecture.WinApp.Pcc.Reports\WagesReportWithActivities.cs.
 *
 * Kept on its own path because the page previously posted to /accounts/api/reports/wages-report,
 * which is the PLAIN Wages Report's endpoint and a different procedure entirely
 * (InventoryStockEvalautionDetail.WagesRegister vs [pcc].[USP_WagesReportWithActivities]).
 */
@RestController
@RequestMapping("/accounts/api/reports/wages-report-activities")
public class ContractorWagesActivityReportRestController {

    @Autowired
    private ContractorWagesActivityReportService reportService;

    /** Everything the filter bar needs, in one call (form :285-440). */
    @GetMapping("/lookups")
    public ResponseEntity<Map<String, Object>> getLookups() {
        return ResponseEntity.ok(reportService.getFilterLookups());
    }

    /** GridBind(), form :490-520. */
    @PostMapping
    public ResponseEntity<Map<String, Object>> run(@RequestBody Map<String, Object> req) {
        return ResponseEntity.ok(reportService.runReport(
                asString(req.get("reportType")),
                asString(req.get("fromDate")),
                asString(req.get("toDate")),
                asInt(req.get("fromDocNo")),
                asInt(req.get("toDocNo")),
                asInt(req.get("plantId")),
                asInt(req.get("itemId")),
                asInt(req.get("actionId")),
                asInt(req.get("contractorId")),
                asInt(req.get("serviceActivityId"))));
    }

    private static String asString(Object o) { return o == null ? null : String.valueOf(o); }

    private static Integer asInt(Object o) {
        if (o == null) return null;
        String s = String.valueOf(o).trim();
        if (s.isEmpty()) return null;
        try { return (int) Double.parseDouble(s); } catch (Exception e) { return null; }
    }
}
