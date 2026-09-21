package com.mst.controllers;

import com.mst.models.dto.ProductionJobOrderDto;
import com.mst.services.ProductionJobOrderService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Production Job Order - frmProductionJobOrder.cs, DocumentTypeId 401.
 *
 * ---------------------------------------------------------------------------------------------
 * THE ROUTE IS THE WIRING
 * ---------------------------------------------------------------------------------------------
 * ScreenRouteIndex resolves a desktop screen to a page by reading the routes Spring has actually
 * registered and matching on exact normalised equality - lower-case, letters and digits only:
 *
 *     ScreenName "ProductionJobOrder"  ->  "productionjoborder"
 *     GET /production/job-order        ->  "productionjoborder"      match
 *
 * So this screen's tile starts working because this route exists, not because anything was added
 * to a list. The path is deliberately distinct from every other registered route: the index drops
 * a normalised key that two routes both claim rather than resolving it arbitrarily, which is what
 * keeps different URLs from opening the same or an unrelated page.
 */
@Controller
public class ProductionJobOrderController {

    @Autowired
    private ProductionJobOrderService service;

    // ------------------------------------------------------------------ page

    /**
     * dbo.ScreenDefinition 282 names this form "Production Job Order (Not Use)". The tile
     * "Production Job Order" in SidebarScreenCatalog is screen 281, which opens
     * frmProductionJobOrderMain, so /production/job-order now belongs to
     * ProductionJobOrderMainController. This page keeps a path of its own rather than being
     * deleted: the work is intact, and nothing on the Production module page links to it.
     */
    @GetMapping("/production/job-order-not-use")
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/production_job_order";
    }

    // ------------------------------------------------------------------ api

    @GetMapping("/api/production/job-order/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        Map<String, Object> jo = service.load(id);
        if (jo == null) return ResponseEntity.notFound().build();
        return ResponseEntity.ok(jo);
    }

    /**
     * There is deliberately NO documentTypeId, organizationId, companyId, financialYearId or
     * entryUser parameter here. All of them are fixed by the form or read from the signed-in user
     * inside the service; accepting any of them would let a caller write another screen's
     * documents or forge authorship.
     */
    @PostMapping("/api/production/job-order/save")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> save(@RequestBody ProductionJobOrderDto dto) {
        try {
            return ResponseEntity.ok(service.save(dto));
        } catch (IllegalArgumentException | IllegalStateException e) {
            /* The desktop's own validation text reaches the operator unchanged. */
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Save failed." : e.getMessage()));
        }
    }

    /**
     * History. The only filter the desktop offers is "how many": the tab opens at 50 and the
     * Load All button asks for everything. No date or document-number parameters are exposed
     * because BindHistoryGrid never sends any.
     */
    @GetMapping("/api/production/job-order/history")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> history(
            @RequestParam(required = false, defaultValue = "50") Integer noOfRecords) {
        try {
            return ResponseEntity.ok(service.history(noOfRecords));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "History failed." : e.getMessage()));
        }
    }

    /** Double-clicking a saved grid row - may it be edited, or is it already used in Production? */
    @GetMapping("/api/production/job-order/row-editable")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> rowEditable(@RequestParam String grid,
                                                           @RequestParam int jobOrderId) {
        try {
            return ResponseEntity.ok(service.rowEditable(grid, jobOrderId));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Check failed." : e.getMessage()));
        }
    }

    @GetMapping("/api/production/job-order/next-code")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> nextCode() {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("planCode", service.nextCode());
        return ResponseEntity.ok(r);
    }

    @GetMapping("/api/production/job-order/next-plan-type-code")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> nextPlanTypeCode(@RequestParam String planType) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("planTypeSrNo", service.nextPlanTypeCode(planType));
        return ResponseEntity.ok(r);
    }


    // ------------------------------------------------------------------ lookups

    /** Every list the screen binds at load. One call, so the page does not open half-bound. */
    @GetMapping("/api/production/job-order/lookups")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> lookups() {
        try {
            return ResponseEntity.ok(service.lookups());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Lookups failed." : e.getMessage()));
        }
    }

    /** The per-item UOM schedule behind Inner UOM, Outer UOM and Pack Size. */
    @GetMapping("/api/production/job-order/uom-schedule")
    @ResponseBody
    public ResponseEntity<?> uomSchedule(@RequestParam int itemId) {
        try {
            return ResponseEntity.ok(service.uomSchedule(itemId));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "UOM schedule failed." : e.getMessage()));
        }
    }

    /** cmbFiltersType -> cmbDocNo. filterType is the caption the desktop compares on. */
    @GetMapping("/api/production/job-order/doc-no")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> docNo(@RequestParam(required = false) String filterType) {
        try {
            return ResponseEntity.ok(service.docNoList(filterType));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Document list failed." : e.getMessage()));
        }
    }

    /** cmbDocNo -> the Input or Output item picker. */
    @GetMapping("/api/production/job-order/filtered-items")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> filteredItems(
            @RequestParam(required = false) String entryType,
            @RequestParam(required = false) String filterType,
            @RequestParam(required = false) Integer docNoId) {
        try {
            return ResponseEntity.ok(service.filteredItems(entryType, filterType, docNoId));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(e.getMessage() == null ? "Item list failed." : e.getMessage()));
        }
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", false);
        r.put("message", message);
        return r;
    }
}
