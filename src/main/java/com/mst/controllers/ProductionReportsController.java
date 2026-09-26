package com.mst.controllers;

import com.mst.services.ProductionReportsService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Production Reports. One route per report, so no two of them can resolve to the same page —
 * ScreenRouteIndex drops a normalised key that two routes both claim, and each report's path is
 * distinct.
 *
 * Reports 975 (Job Order Summary) and 309 (Production Summary) are built. The other three get
 * their own routes here as they are built rather than sharing one generic page, because they take
 * different filters and return different columns; forcing them through a single endpoint is
 * exactly the mistake this port has been asked not to make.
 *
 * No parameter carries an organization, company or user. They come from the signed-in user.
 */
@Controller
public class ProductionReportsController {

    @Autowired
    private ProductionReportsService service;

    // ------------------------------------------------- 975 Job Order Summary Report

    @GetMapping("/production/reports/job-order-summary")
    public String jobOrderSummaryPage(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/reports/job_order_summary";
    }

    /**
     * @param approvalFilter All | Approved | Unapproved — "All" omits &#64;IsApproved, as the
     *                       desktop's rdAll radio does.
     * @param fromDate       optional; the desktop's From picker is unticked by default.
     */
    @GetMapping("/api/production/reports/job-order-summary")
    @ResponseBody
    public ResponseEntity<?> jobOrderSummary(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false, defaultValue = "All") String approvalFilter) {
        try {
            return ResponseEntity.ok(service.jobOrderSummary(fromDate, toDate, approvalFilter));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Report failed." : e.getMessage()));
        }
    }

    /** From-date default and amount format - InitializeComponentMethod:122. */
    @GetMapping("/api/production/reports/job-order-summary/defaults")
    @ResponseBody
    public ResponseEntity<?> jobOrderSummaryDefaults() {
        try {
            return ResponseEntity.ok(service.jobOrderSummaryDefaults());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Defaults failed." : e.getMessage()));
        }
    }

    /** The drill-down behind a summary row. */
    @GetMapping("/api/production/reports/job-order-summary/detail")
    @ResponseBody
    public ResponseEntity<?> jobOrderSummaryDetail(@RequestParam int jobOrderId,
                                                   @RequestParam(required = false, defaultValue = "0") int actionId) {
        try {
            return ResponseEntity.ok(service.productionSettlementDetail(jobOrderId, actionId));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Detail failed." : e.getMessage()));
        }
    }

    // ------------------------------------------------- 309 Production Summary Report

    @GetMapping("/production/reports/production-summary")
    public String productionSummaryPage(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/reports/production_summary";
    }

    /** The branch multi-select, its default, and whether this user may see rate and amount. */
    @GetMapping("/api/production/reports/production-summary/lookups")
    @ResponseBody
    public ResponseEntity<?> productionSummaryLookups() {
        try {
            return ResponseEntity.ok(service.productionSummaryLookups());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Could not load filters." : e.getMessage()));
        }
    }

    /** Cascades off the branch tick-list, as cmbBranchName_Leave does. */
    @GetMapping("/api/production/reports/production-summary/job-orders")
    @ResponseBody
    public ResponseEntity<?> productionSummaryJobOrders(
            @RequestParam(required = false) String branchIds) {
        try {
            return ResponseEntity.ok(service.productionSummaryJobOrders(ids(branchIds)));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Could not load job orders." : e.getMessage()));
        }
    }

    /** Cascades off the chosen job order, as cmbsummeryJobOrderNo_TextChanged does. */
    @GetMapping("/api/production/reports/production-summary/plants")
    @ResponseBody
    public ResponseEntity<?> productionSummaryPlants(
            @RequestParam int jobOrderId,
            @RequestParam(required = false) String branchIds) {
        try {
            return ResponseEntity.ok(service.productionSummaryPlants(jobOrderId, ids(branchIds)));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Could not load plants." : e.getMessage()));
        }
    }

    /**
     * One call for all four grids, because btnShow_Click fills them together and they are only
     * consistent with each other when they were read for the same filter set.
     *
     * fromDate and toDate are each independently optional — the desktop's two pickers carry their
     * own checkboxes, so an absent end is a real query rather than a missing value.
     */
    @GetMapping("/api/production/reports/production-summary")
    @ResponseBody
    public ResponseEntity<?> productionSummary(
            @RequestParam int jobOrderId,
            @RequestParam(required = false, defaultValue = "0") int plantId,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) String branchIds) {
        try {
            return ResponseEntity.ok(service.productionSummary(
                    jobOrderId, plantId, fromDate, toDate, ids(branchIds)));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Report failed." : e.getMessage()));
        }
    }

    // ------------------------------------------------- 310 Production Register

    @GetMapping("/production/reports/production-register")
    public String productionRegisterPage(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/reports/production_register";
    }

    /** Six pickers from one procedure, plus the job orders, branches and the two fixed lists. */
    @GetMapping("/api/production/reports/production-register/lookups")
    @ResponseBody
    public ResponseEntity<?> productionRegisterLookups() {
        try {
            return ResponseEntity.ok(service.productionRegisterLookups());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Could not load filters." : e.getMessage()));
        }
    }

    /**
     * @param activity  which of the three shapes to return — it goes to the procedure's
     *                  &#64;Activity, so only the form's own three values are accepted.
     * @param dateMode  "doc" or "entry"; it selects which pair of date parameters is sent.
     */
    @GetMapping("/api/production/reports/production-register")
    @ResponseBody
    public ResponseEntity<?> productionRegister(
            @RequestParam String activity,
            @RequestParam(required = false, defaultValue = "doc") String dateMode,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false) String entryTypes,
            @RequestParam(required = false, defaultValue = "0") int jobOrderId,
            @RequestParam(required = false, defaultValue = "0") int parentCategoryId,
            @RequestParam(required = false, defaultValue = "0") int warehouseId,
            @RequestParam(required = false, defaultValue = "0") int itemId,
            @RequestParam(required = false, defaultValue = "0") int plantId,
            @RequestParam(required = false, defaultValue = "0") int wipAccountId,
            @RequestParam(required = false, defaultValue = "0") int stockAccountId,
            @RequestParam(required = false) String branchIds) {
        try {
            return ResponseEntity.ok(service.productionRegister(
                    activity, dateMode, fromDate, toDate, keys(entryTypes), jobOrderId,
                    parentCategoryId, warehouseId, itemId, plantId, wipAccountId,
                    stockAccountId, ids(branchIds)));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Report failed." : e.getMessage()));
        }
    }

    // ------------------------------------------------- 308 Production Comparison Report

    @GetMapping("/production/reports/production-comparison")
    public String productionComparisonPage(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/reports/production_comparison";
    }

    /** Branch tick-list, production types, the rate right and the financial year's start. */
    @GetMapping("/api/production/reports/production-comparison/lookups")
    @ResponseBody
    public ResponseEntity<?> comparisonFormLookups() {
        try {
            return ResponseEntity.ok(service.comparisonFormLookups());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Could not load filters." : e.getMessage()));
        }
    }

    /** The Comparison tab's two pickers; they cascade off its own branch tick-list. */
    @GetMapping("/api/production/reports/production-comparison/comparison-lookups")
    @ResponseBody
    public ResponseEntity<?> comparisonLookups(@RequestParam(required = false) String branchIds) {
        try {
            return ResponseEntity.ok(service.comparisonLookups(ids(branchIds)));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Could not load filters." : e.getMessage()));
        }
    }

    /** The Summary tab's pickers; it carries its own branch tick-list on the desktop too. */
    @GetMapping("/api/production/reports/production-comparison/summary-lookups")
    @ResponseBody
    public ResponseEntity<?> comparisonSummaryLookups(@RequestParam(required = false) String branchIds) {
        try {
            return ResponseEntity.ok(service.comparisonSummaryLookups(ids(branchIds)));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Could not load filters." : e.getMessage()));
        }
    }

    /** The pack UOM list, cascading off the Summary tab's item. */
    @GetMapping("/api/production/reports/production-comparison/pack-uoms")
    @ResponseBody
    public ResponseEntity<?> comparisonPackUoms(@RequestParam int itemId) {
        try {
            return ResponseEntity.ok(service.comparisonPackUoms(itemId));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Could not load pack UOMs." : e.getMessage()));
        }
    }

    @GetMapping("/api/production/reports/production-comparison")
    @ResponseBody
    public ResponseEntity<?> productionComparison(
            @RequestParam String productionType,
            @RequestParam(required = false, defaultValue = "0") int docNoFrom,
            @RequestParam(required = false, defaultValue = "0") int docNoTo,
            @RequestParam(required = false, defaultValue = "0") int plantId,
            @RequestParam(required = false, defaultValue = "0") int jobOrderId) {
        try {
            return ResponseEntity.ok(service.productionComparison(
                    productionType, docNoFrom, docNoTo, plantId, jobOrderId));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Report failed." : e.getMessage()));
        }
    }

    /** @param mode "detail" (the desktop's default radio) or "gainloss". */
    @GetMapping("/api/production/reports/production-comparison/summary")
    @ResponseBody
    public ResponseEntity<?> comparisonSummary(
            @RequestParam(required = false, defaultValue = "detail") String mode,
            @RequestParam(required = false, defaultValue = "0") int jobOrderId,
            @RequestParam(required = false, defaultValue = "0") int plantId,
            @RequestParam(required = false, defaultValue = "0") int itemId,
            @RequestParam(required = false, defaultValue = "0") int packUomId,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate) {
        try {
            return ResponseEntity.ok(service.comparisonSummary(
                    mode, jobOrderId, plantId, itemId, packUomId, fromDate, toDate));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Report failed." : e.getMessage()));
        }
    }

    // -------------------------------- 306 Production PackingMaterial Consumption Register

    @GetMapping("/production/reports/packing-material-consumption")
    public String packingMaterialPage(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/reports/packing_material_consumption";
    }

    @GetMapping("/api/production/reports/packing-material-consumption/lookups")
    @ResponseBody
    public ResponseEntity<?> packingMaterialLookups(@RequestParam(required = false) String branchIds) {
        try {
            return ResponseEntity.ok(service.packingMaterialLookups(ids(branchIds)));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Could not load filters." : e.getMessage()));
        }
    }

    @GetMapping("/api/production/reports/packing-material-consumption")
    @ResponseBody
    public ResponseEntity<?> packingMaterialConsumption(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false, defaultValue = "0") int itemId,
            @RequestParam(required = false, defaultValue = "0") int pmItemId,
            @RequestParam(required = false) String branchIds) {
        try {
            return ResponseEntity.ok(service.packingMaterialConsumption(
                    fromDate, toDate, itemId, pmItemId, ids(branchIds)));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Report failed." : e.getMessage()));
        }
    }

    // ------------------------------------------------- helpers

    /** "Issue,FinishGoods" -> ["Issue", "FinishGoods"]; the service drops anything unrecognised. */
    private static List<String> keys(String csv) {
        List<String> out = new ArrayList<>();
        if (csv == null || csv.trim().isEmpty()) return out;
        for (String part : csv.split(",")) {
            String t = part.trim();
            if (!t.isEmpty()) out.add(t);
        }
        return out;
    }

    /** "3,7,9" -> [3, 7, 9]. A value that is not a number is dropped rather than guessed at. */
    private static List<Integer> ids(String csv) {
        List<Integer> out = new ArrayList<>();
        if (csv == null || csv.trim().isEmpty()) return out;
        for (String part : csv.split(",")) {
            String t = part.trim();
            if (t.isEmpty()) continue;
            try { out.add(Integer.valueOf(t)); } catch (NumberFormatException ignored) { }
        }
        return out;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", false);
        r.put("message", message);
        return r;
    }
}
