package com.mst.controllers;

import com.mst.services.Prod273PreCostingJobOrderService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Production hub (module 18) — screen 273 "Production Job Order (Pre Costing)"
 * (Architecture.WinApp.Production.ProductionPreCostingAndJobOrder, DocumentTypeId 401).
 * Page /production/pre-costing-job-order, JSON under /production/pre-costing-job-order/api.
 * Sibling screen 281 lives at /production/job-order (ProductionJobOrderMainController).
 *
 * Errors: validation → 400 {message}; missing right → 403 {message}; anything else → 500 {message}.
 */
@Controller
@RequestMapping("/production/pre-costing-job-order")
public class Prod273PreCostingJobOrderController {

    private final Prod273PreCostingJobOrderService service;
    public Prod273PreCostingJobOrderController(Prod273PreCostingJobOrderService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Production Job Order (Pre Costing)");
        return "production/prod273_pre_costing_job_order";
    }

    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    @GetMapping("/api/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return run(service::refresh, "Refresh failed."); }

    @GetMapping("/api/reset") @ResponseBody
    public ResponseEntity<?> reset() { return run(service::resetData, "Could not generate the Plan Code."); }

    @GetMapping("/api/doc-no-list") @ResponseBody
    public ResponseEntity<?> docNoList(@RequestParam(defaultValue = "0") int filterType) {
        return run(() -> service.docNoList(filterType), "Could not load the Doc No list.");
    }

    @GetMapping("/api/items-by-doc") @ResponseBody
    public ResponseEntity<?> itemsByDoc(@RequestParam(defaultValue = "0") int filterType, @RequestParam(defaultValue = "0") int docId) {
        return run(() -> service.itemsByDoc(filterType, docId), "Could not load the items.");
    }

    @GetMapping("/api/stock-weight") @ResponseBody
    public ResponseEntity<?> stockWeight(@RequestParam(defaultValue = "0") int itemId, @RequestParam(required = false) String planDate) {
        return run(() -> single("balWeight", service.stockWeight(itemId, planDate)), "Could not read the stock.");
    }

    @GetMapping("/api/avg-rate") @ResponseBody
    public ResponseEntity<?> avgRate(@RequestParam(defaultValue = "0") int itemId, @RequestParam(required = false) String planDate,
                                     @RequestParam(defaultValue = "0") int recId, @RequestParam(defaultValue = "0") int jobLotId,
                                     @RequestParam(defaultValue = "0") int cropYearId, @RequestParam(required = false) String cropYear) {
        return run(() -> single("rate", service.avgRate(itemId, planDate, recId, jobLotId, cropYearId, cropYear)), "Could not read the average rate.");
    }

    @GetMapping("/api/analysis-groups") @ResponseBody
    public ResponseEntity<?> analysisGroups(@RequestParam(defaultValue = "0") int brandItemId, @RequestParam(defaultValue = "0") int supplierCustomerId) {
        return run(() -> service.analysisGroups(brandItemId, supplierCustomerId), "Could not load the standards.");
    }

    @GetMapping("/api/analysis-group-rows") @ResponseBody
    public ResponseEntity<?> analysisGroupRows(@RequestParam(defaultValue = "0") int mainId) {
        return run(() -> service.analysisGroupRows(mainId), "Could not load the standard rows.");
    }

    @GetMapping("/api/referenced") @ResponseBody
    public ResponseEntity<?> referenced(@RequestParam(defaultValue = "0") int jobOrderId, @RequestParam(defaultValue = "0") int productionDocType) {
        return run(() -> service.referenced(jobOrderId, productionDocType), "Check failed.");
    }

    @GetMapping("/api/plant-deletable") @ResponseBody
    public ResponseEntity<?> plantDeletable(@RequestParam(defaultValue = "0") int jobOrderId, @RequestParam(defaultValue = "0") int plantId,
                                            @RequestParam(required = false, defaultValue = "") String plantName) {
        return run(() -> service.plantDeletable(jobOrderId, plantId, plantName), "Check failed.");
    }

    @GetMapping("/api/loader/lookups") @ResponseBody
    public ResponseEntity<?> loaderLookups() { return run(service::loaderLookups, "Could not load the filters."); }

    @GetMapping("/api/loader/search") @ResponseBody
    public ResponseEntity<?> loaderSearch(@RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                          @RequestParam(defaultValue = "0") int warehouseId, @RequestParam(defaultValue = "0") int itemId,
                                          @RequestParam(defaultValue = "0") int jobLotId, @RequestParam(required = false) String cropYear,
                                          @RequestParam(defaultValue = "0") int refDocumentTypeId, @RequestParam(defaultValue = "0") int supplierCustomerId,
                                          @RequestParam(defaultValue = "0") int parentCategory, @RequestParam(defaultValue = "0") int itemCategoryId,
                                          @RequestParam(defaultValue = "0") int itemTypeId) {
        return run(() -> service.loaderSearch(fromDate, toDate, warehouseId, itemId, jobLotId, cropYear, refDocumentTypeId,
                supplierCustomerId, parentCategory, itemCategoryId, itemTypeId), "Could not load the transactions.");
    }

    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "doc") String dateMode,
                                     @RequestParam(required = false) String fromDate, @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int docNoFrom, @RequestParam(defaultValue = "0") int docNoTo,
                                     @RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> service.history(dateMode, fromDate, toDate, docNoFrom, docNoTo, jobOrderId), "History failed.");
    }

    @GetMapping("/api/attachments") @ResponseBody
    public ResponseEntity<?> attachments(@RequestParam int id) { return run(() -> service.attachments(id), "Could not read the attachments."); }

    @GetMapping("/api/{id:[0-9]+}") @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record Not Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not open that job order.")));
        }
    }

    @PostMapping("/api/precheck") @ResponseBody
    public ResponseEntity<?> precheck(@RequestBody Map<String, Object> body) { return write(() -> service.precheck(body)); }

    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return write(() -> service.save(body)); }

    // ------------------------------------------------------------------------------ plumbing

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> write(Call c) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(r, "Save failed.")));
        }
    }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, fallback)));
        }
    }

    private static Map<String, Object> single(String key, Object value) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put(key, value);
        return m;
    }

    private static String msg(Throwable e, String fallback) {
        return (e.getMessage() == null || e.getMessage().trim().isEmpty()) ? fallback : e.getMessage();
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
