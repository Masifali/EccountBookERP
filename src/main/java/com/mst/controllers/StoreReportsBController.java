package com.mst.controllers;

import com.mst.services.StoreReportsBService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.Callable;

/**
 * Store Management Reports (module 46), group B:
 * <ul>
 *   <li>454 "Stock Adjustment Report" - StockAdjustmentRegister.cs - /store/reports/stock-adjustment-register</li>
 *   <li>455 "Department Request History" - DepartmentRequestHistory.cs - /store/reports/department-request-history</li>
 *   <li>456 "Stock Transfer Report" - frmStockTranfserRegister.cs - /store/reports/stock-transfer-register</li>
 * </ul>
 * Read-only. No parameter carries an organization, company, user or year - they come from the
 * session. See {@link StoreReportsBService} for the desktop-behaviour notes and deviations.
 */
@Controller
public class StoreReportsBController {

    private static final Logger LOG = LoggerFactory.getLogger(StoreReportsBController.class);

    private static final String SA = "/api/store/reports/stock-adjustment-register";
    private static final String DR = "/api/store/reports/department-request-history";
    private static final String ST = "/api/store/reports/stock-transfer-register";

    private final StoreReportsBService service;
    public StoreReportsBController(StoreReportsBService service) { this.service = service; }

    // ------------------------------------------------------------------ 454 Stock Adjustment Report

    @GetMapping("/store/reports/stock-adjustment-register")
    public String saPage(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/reports/stock_adjustment_register";
    }

    @GetMapping(SA + "/lookups")
    @ResponseBody
    public ResponseEntity<?> saLookups() { return run(service::saLookups, "Could not load the filters."); }

    @GetMapping(SA + "/refresh")
    @ResponseBody
    public ResponseEntity<?> saRefresh() { return run(service::saRefresh, "Could not refresh the filters."); }

    @GetMapping(SA)
    @ResponseBody
    public ResponseEntity<?> saShow(@RequestParam(required = false) String fromDate,
                                    @RequestParam(required = false) String toDate,
                                    @RequestParam(defaultValue = "0") int itemId,
                                    @RequestParam(defaultValue = "0") int cropYearId,
                                    @RequestParam(defaultValue = "0") int jobLotId,
                                    @RequestParam(defaultValue = "0") int warehouseId,
                                    @RequestParam(defaultValue = "0") int entryTypeId) {
        return run(() -> service.saShow(fromDate, toDate, itemId, cropYearId, jobLotId, warehouseId, entryTypeId),
                "Report failed.");
    }

    @GetMapping(SA + "/{id}/slip")
    @ResponseBody
    public ResponseEntity<?> saSlip(@PathVariable int id) { return run(() -> service.saSlip(id), "Print failed."); }

    // ------------------------------------------------------------------ 455 Department Request History

    @GetMapping("/store/reports/department-request-history")
    public String drPage(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/reports/department_request_history";
    }

    @GetMapping(DR + "/lookups")
    @ResponseBody
    public ResponseEntity<?> drLookups() { return run(service::drLookups, "Could not load the filters."); }

    @GetMapping(DR + "/refresh")
    @ResponseBody
    public ResponseEntity<?> drRefresh() { return run(service::drRefresh, "Could not refresh the filters."); }

    @GetMapping(DR)
    @ResponseBody
    public ResponseEntity<?> drShow(@RequestParam(required = false) String fromDate,
                                    @RequestParam(required = false) String toDate,
                                    @RequestParam(defaultValue = "0") int departmentId,
                                    @RequestParam(defaultValue = "0") int itemId,
                                    @RequestParam(defaultValue = "0") int assetId,
                                    @RequestParam(required = false) String approved) {
        return run(() -> service.drShow(fromDate, toDate, departmentId, itemId, assetId, approved), "Report failed.");
    }

    @GetMapping(DR + "/{id}/slip")
    @ResponseBody
    public ResponseEntity<?> drSlip(@PathVariable int id) { return run(() -> service.drSlip(id), "Print failed."); }

    // ------------------------------------------------------------------ 456 Stock Transfer Report

    @GetMapping("/store/reports/stock-transfer-register")
    public String stPage(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/reports/stock_transfer_register";
    }

    @GetMapping(ST + "/lookups")
    @ResponseBody
    public ResponseEntity<?> stLookups() { return run(service::stLookups, "Could not load the filters."); }

    @GetMapping(ST + "/combos")
    @ResponseBody
    public ResponseEntity<?> stCombos(@RequestParam(required = false) String branchIds) {
        return run(() -> service.stCombos(ids(branchIds)), "Could not load the filters.");
    }

    @GetMapping(ST + "/document-types")
    @ResponseBody
    public ResponseEntity<?> stDocumentTypes() { return run(service::stDocumentTypes, "Could not load the document types."); }

    @GetMapping(ST)
    @ResponseBody
    public ResponseEntity<?> stShow(@RequestParam(required = false) String branchIds,
                                    @RequestParam(required = false) String fromDate,
                                    @RequestParam(required = false) String toDate,
                                    @RequestParam(defaultValue = "0") int itemId,
                                    @RequestParam(defaultValue = "0") int fromWarehouseId,
                                    @RequestParam(defaultValue = "0") int toWarehouseId,
                                    @RequestParam(defaultValue = "0") int jobLotId,
                                    @RequestParam(defaultValue = "0") int toJobLotId,
                                    @RequestParam(required = false) String documentType) {
        return run(() -> service.stShow(ids(branchIds), fromDate, toDate, itemId, fromWarehouseId, toWarehouseId,
                jobLotId, toJobLotId, documentType), "Report failed.");
    }

    // ------------------------------------------------------------------ plumbing

    private static List<Integer> ids(String csv) {
        List<Integer> out = new ArrayList<>();
        if (csv == null) return out;
        for (String s : csv.split(",")) {
            s = s.trim();
            if (s.isEmpty()) continue;
            try { out.add(Integer.valueOf(s)); } catch (NumberFormatException ignored) { /* not a branch id */ }
        }
        return out;
    }

    private static ResponseEntity<?> run(Callable<?> action, String fallback) {
        try {
            return ResponseEntity.ok(action.call());
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage() == null ? fallback : e.getMessage()));
        } catch (org.springframework.security.access.AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            LOG.warn("Store report B request failed", e);
            return ResponseEntity.badRequest().body(fail(e.getMessage() == null ? fallback : e.getMessage()));
        }
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
