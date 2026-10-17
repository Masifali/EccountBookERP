package com.mst.controllers;

import com.mst.services.SteelProductionJobOrderService;
import com.mst.services.SteelProductionSupport;
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
import java.util.concurrent.Callable;

/**
 * 600 Production Job Order (Production Steel, module 107) —
 * Architecture.WinApp.Steel.Production/ProductionPreCostingAndJobOrder_St.cs, DocumentTypeId 1511.
 *
 * Page: /production/steel/pre-costing-job-order ; JSON under /production/steel/pre-costing-job-order/api/...
 * Print 1563 is the registered steel report (/reports/print/1563-production-job-order-slip, SteelPrintController),
 * reached through print-rpt.js with the pre-check of CommonServices.ProductionJobOrderSlipSteel1563 done here.
 */
@Controller
@RequestMapping("/production/steel/pre-costing-job-order")
public class SteelProductionJobOrderController {

    private final SteelProductionJobOrderService service;
    private final SteelProductionSupport support;

    public SteelProductionJobOrderController(SteelProductionJobOrderService service, SteelProductionSupport support) {
        this.service = service;
        this.support = support;
    }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Production Job Order");
        return "production/steel_pre_costing_job_order";
    }

    /** frmProductionJobOrder_Load:647 */
    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups); }

    /** GeneratePlanCode:1137 */
    @GetMapping("/api/next-code") @ResponseBody
    public ResponseEntity<?> nextCode() {
        return run(() -> {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("planCode", service.nextCode(support.user()));
            return m;
        });
    }

    /** cmbFiltersType_Leave:855 */
    @GetMapping("/api/doc-nos") @ResponseBody
    public ResponseEntity<?> docNos(@RequestParam(defaultValue = "") String filterType) {
        return run(() -> service.docNos(filterType));
    }

    /** cmbDocNo_Leave:884 */
    @GetMapping("/api/filtered-items") @ResponseBody
    public ResponseEntity<?> filteredItems(@RequestParam(defaultValue = "") String filterType,
                                           @RequestParam(defaultValue = "0") int docNo,
                                           @RequestParam(defaultValue = "Output") String entryType) {
        return run(() -> service.filteredItems(filterType, docNo, entryType));
    }

    /** bindPackSizeInput:1713 / bindRateUomAndItemPackUom:4079 */
    @GetMapping("/api/uom-schedule") @ResponseBody
    public ResponseEntity<?> uomSchedule(@RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.uomSchedule(itemId));
    }

    /** GetAvgRate:2183 (1511) / BtnGenerateInPutRates_Click:5226 (401) */
    @GetMapping("/api/avg-rate") @ResponseBody
    public ResponseEntity<?> avgRate(@RequestParam(defaultValue = "0") int itemId,
                                     @RequestParam(defaultValue = "") String docDate,
                                     @RequestParam(defaultValue = "1511") int documentTypeId,
                                     @RequestParam(defaultValue = "0") int recId,
                                     @RequestParam(defaultValue = "0") int jobLotId,
                                     @RequestParam(defaultValue = "0") int warehouseId) {
        return run(() -> service.avgRate(itemId, docDate, documentTypeId, recId, jobLotId, warehouseId));
    }

    /** BindHistoryGrid:3545 - 50 on tab change, 0 = Load All */
    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "50") int noOfRecords) {
        return run(() -> service.history(noOfRecords));
    }

    /** grdInputDetail_DoubleClick:1950 (80) / grdPlannedOutput_DoubleClick:2488 (112) */
    @GetMapping("/api/ref-check") @ResponseBody
    public ResponseEntity<?> refCheck(@RequestParam int documentTypeId, @RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> service.refCheck(documentTypeId, jobOrderId));
    }

    /** grdPlantdetail_ColumnButtonClick:5128 */
    @GetMapping("/api/plant-deletable") @ResponseBody
    public ResponseEntity<?> plantDeletable(@RequestParam int jobOrderId, @RequestParam int plantId,
                                            @RequestParam(required = false) String plantName) {
        return run(() -> service.plantDeletable(jobOrderId, plantId, plantName));
    }

    /** ProductionJobOrderSlipSteel1563:16748 pre-check */
    @GetMapping("/api/print-check") @ResponseBody
    public ResponseEntity<?> printCheck(@RequestParam int id) { return run(() -> service.printCheck(id)); }

    /** DataGridHistory_LinkClicked:3733 */
    @GetMapping("/api/attachments") @ResponseBody
    public ResponseEntity<?> attachments(@RequestParam int id) { return run(() -> service.attachments(id)); }

    /** LoadavailableTransactionsForIssuance - StockComboFill:197 */
    @GetMapping("/api/loader/combos") @ResponseBody
    public ResponseEntity<?> loaderCombos() { return run(service::loaderCombos); }

    /** LoadavailableTransactionsForIssuance - PendingInventoryTransactionsForIssuanceLoad:319 */
    @GetMapping("/api/loader/rows") @ResponseBody
    public ResponseEntity<?> loaderRows(@RequestParam(defaultValue = "") String from,
                                        @RequestParam(defaultValue = "") String to,
                                        @RequestParam(defaultValue = "0") int parentCategoryId,
                                        @RequestParam(defaultValue = "0") int itemCategoryId,
                                        @RequestParam(defaultValue = "0") int itemTypeId,
                                        @RequestParam(defaultValue = "0") int jobLotId,
                                        @RequestParam(defaultValue = "") String cropYear,
                                        @RequestParam(defaultValue = "0") int warehouseId,
                                        @RequestParam(defaultValue = "0") int refDocumentTypeId,
                                        @RequestParam(defaultValue = "0") int supplierCustomerId,
                                        @RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.loaderRows(from, to, parentCategoryId, itemCategoryId, itemTypeId, jobLotId, cropYear,
                warehouseId, refDocumentTypeId, supplierCustomerId, itemId));
    }

    /** DataGridHistory_DoubleClick:3641 - GetByID */
    @GetMapping("/api/{id}") @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        return run(() -> {
            Map<String, Object> m = service.load(id);
            if (m == null) throw new IllegalArgumentException("Record Not Found");
            return m;
        });
    }

    /** Insert:1349 */
    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return run(() -> service.save(body)); }

    // ------------------------------------------------------------------------------------ helpers

    private static ResponseEntity<?> run(Callable<?> c) {
        try {
            return ResponseEntity.ok(c.call());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(e));
        }
    }

    /** The innermost cause, so a procedure's RAISERROR text reaches the operator as MessageBox(ex.Message) shows it. */
    private static Map<String, Object> fail(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        String msg = t.getMessage();
        if (msg == null) msg = e.getMessage();
        m.put("message", msg == null ? t.getClass().getSimpleName() : msg);
        return m;
    }
}
