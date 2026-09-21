package com.mst.controllers;

import com.mst.models.dto.ProductionJobOrderMainDto;
import com.mst.services.ProductionJobOrderMainService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 281 "Production Job Order" — {@code frmProductionJobOrderMain.cs}, DocumentTypeId 403.
 *
 * ---------------------------------------------------------------------------------------------
 * WHY THIS ONE TAKES /production/job-order
 * ---------------------------------------------------------------------------------------------
 * dbo.ScreenDefinition is explicit about which form each screen opens:
 *
 *     281  frmProductionJobOrderMain   "Production Job Order"
 *     282  frmProductionJobOrder       "Production Job Order (Not Use)"
 *
 * SidebarScreenCatalog maps the tile "Production Job Order" to screen 281, so
 * {@code /production/job-order} must serve 281's form. It previously served the form behind 282 —
 * the one the ERP's own alias marks as not in use. ProductionJobOrderController keeps that page,
 * and its endpoints, on a path of its own; nothing is deleted.
 *
 * The API paths here are {@code /api/production/job-order-main/...} precisely so the two screens
 * can never answer each other's calls.
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT IS DELIBERATELY NOT A PARAMETER
 * ---------------------------------------------------------------------------------------------
 * No documentTypeId, organizationId, companyId, branchId, financialYearId, entryUser or
 * canViewAllRecord parameter appears on any route below. Every one of them is fixed by the form or
 * read from the signed-in user inside the service. Accepting any would let a caller write another
 * screen's documents, forge authorship, or read other users' job orders.
 */
@Controller
public class ProductionJobOrderMainController {

    @Autowired
    private ProductionJobOrderMainService service;

    private static final String API = "/api/production/job-order-main";

    // ------------------------------------------------------------------ page

    @GetMapping("/production/job-order")
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/production_job_order_main";
    }

    // ------------------------------------------------------------------ load

    /** frmProductionJobOrder_Load:367 — rights, both configuration flags and every picker. */
    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() {
        try {
            return ResponseEntity.ok(service.lookups());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(msg(e, "Could not load the screen.")));
        }
    }

    /** ReadById(ID):1905 — header plus the four grids. */
    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> jo = service.load(id);
            if (jo == null) return ResponseEntity.notFound().build();
            return ResponseEntity.ok(jo);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(msg(e, "Could not open that job order.")));
        }
    }

    // ------------------------------------------------------------------ save

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> save(@RequestBody ProductionJobOrderMainDto dto) {
        try {
            return ResponseEntity.ok(service.save(dto));
        } catch (IllegalArgumentException e) {
            /* The desktop's own validation text reaches the operator unchanged. */
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(msg(e, "Save failed.")));
        }
    }

    // ------------------------------------------------------------------ history

    /**
     * BindHistoryGrid():2022. dateMode is the desktop's four radio buttons — doc | entry | modify
     * | approved — and decides WHICH pair of date parameters the procedure receives.
     */
    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> history(
            @RequestParam(required = false, defaultValue = "doc") String dateMode,
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false, defaultValue = "0") int docNoFrom,
            @RequestParam(required = false, defaultValue = "0") int docNoTo,
            @RequestParam(required = false, defaultValue = "0") int jobOrderId) {
        try {
            return ResponseEntity.ok(
                    service.history(dateMode, fromDate, toDate, docNoFrom, docNoTo, jobOrderId));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(msg(e, "History failed.")));
        }
    }

    // ------------------------------------------------------------------ code generators

    @GetMapping(API + "/next-code")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> nextCode() {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("planCode", service.nextCode());
        return ResponseEntity.ok(r);
    }

    /** cmbPlanType_Leave:768 — the serial beside Plan Type, per plan type and financial year. */
    @GetMapping(API + "/next-plan-type-code")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> nextPlanTypeCode(@RequestParam String planType) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("planTypeSrNo", service.nextPlanTypeCode(planType));
        return ResponseEntity.ok(r);
    }

    // ------------------------------------------------------- Export Schedule loader dialog

    @GetMapping(API + "/schedule-lookups")
    @ResponseBody
    public ResponseEntity<?> scheduleLookups() {
        try {
            return ResponseEntity.ok(service.schedulePickerLookups());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(msg(e, "Could not load the schedule picker.")));
        }
    }

    @GetMapping(API + "/schedule")
    @ResponseBody
    public ResponseEntity<?> schedule(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(required = false, defaultValue = "0") int itemId,
            @RequestParam(required = false, defaultValue = "0") int contractId,
            @RequestParam(required = false, defaultValue = "0") int supplierCustomerId,
            @RequestParam(required = false, defaultValue = "0") int thirdPartyId) {
        try {
            List<Map<String, Object>> rows = service.schedule(
                    fromDate, toDate, itemId, contractId, supplierCustomerId, thirdPartyId);
            return ResponseEntity.ok(rows);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(msg(e, "Could not load the export schedule.")));
        }
    }

    // ------------------------------------------------------- per-row delete guards

    /** GridDetail_ColumnButtonClick:1016 — may this saved plant row be removed? */
    @GetMapping(API + "/plant-deletable")
    @ResponseBody
    public ResponseEntity<?> plantDeletable(@RequestParam int jobOrderId,
                                            @RequestParam int plantId,
                                            @RequestParam(required = false) String plantName) {
        try {
            return ResponseEntity.ok(service.plantDeletable(jobOrderId, plantId, plantName));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(msg(e, "Check failed.")));
        }
    }

    /**
     * grdByProduct / grdFinishGoods delete in update mode (:1226 / :1428). POST, not GET: it
     * removes a row, and a GET that deletes can be fired by a stray link or a prefetch.
     */
    @PostMapping(API + "/delete-rate-schedule-row")
    @ResponseBody
    public ResponseEntity<?> deleteRateScheduleRow(@RequestParam int jobOrderId,
                                                   @RequestParam int detailId) {
        try {
            return ResponseEntity.ok(service.deleteRateScheduleRow(jobOrderId, detailId));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body(fail(msg(e, "Delete failed.")));
        }
    }

    // ------------------------------------------------------------------ helpers

    private static String msg(Exception e, String fallback) {
        return (e.getMessage() == null || e.getMessage().trim().isEmpty()) ? fallback : e.getMessage();
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
