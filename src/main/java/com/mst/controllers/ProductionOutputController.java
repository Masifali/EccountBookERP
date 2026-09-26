package com.mst.controllers;

import com.mst.services.ProductionOutputService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
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
import java.util.Map;
import java.util.concurrent.Callable;

/**
 * 280 Production Against (Job Order) - the "Output" tab, hosted form
 * Architecture.WinApp.Production/frmProductionOutput.cs (DocumentTypeId 112), with its dialogs
 * LoadOutPutPendingforRates and frmPendingMoveOrderDocuments.
 *
 * The shell frames this page into its Output tab (tabControl1_SelectedIndexChanged ->
 * FormHelper.OpenOrGetFormInPanel("frmProductionOutput", ...)).
 */
@Controller
public class ProductionOutputController {

    private static final Logger LOG = LoggerFactory.getLogger(ProductionOutputController.class);
    private static final String API = "/api/production/production-against-job-order/output";

    @Autowired private ProductionOutputService service;

    @GetMapping("/production/production-against-job-order/output")
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/p280_output";
    }

    // ---------------------------------------------------------------------------- Load / lookups

    @GetMapping(API + "/load")
    @ResponseBody public ResponseEntity<?> load() { return run(service::load); }

    @GetMapping(API + "/switches")
    @ResponseBody public ResponseEntity<?> switches() { return run(service::switches); }

    @GetMapping(API + "/doc-no")
    @ResponseBody public ResponseEntity<?> docNo() { return run(() -> one("docNo", service.docNo())); }

    @GetMapping(API + "/job-orders")
    @ResponseBody public ResponseEntity<?> jobOrders() { return run(service::jobOrders); }

    @GetMapping(API + "/job-lots")
    @ResponseBody public ResponseEntity<?> jobLots() { return run(service::jobLots); }

    @GetMapping(API + "/crop-years")
    @ResponseBody public ResponseEntity<?> cropYears() { return run(service::cropYears); }

    @GetMapping(API + "/uoms")
    @ResponseBody public ResponseEntity<?> uoms() { return run(service::uoms); }

    @GetMapping(API + "/packing-types")
    @ResponseBody public ResponseEntity<?> packingTypes() { return run(service::packingTypes); }

    @GetMapping(API + "/warehouses")
    @ResponseBody public ResponseEntity<?> warehouses() { return run(service::warehouses); }

    @GetMapping(API + "/items")
    @ResponseBody public ResponseEntity<?> allItems() { return run(service::allItems); }

    @GetMapping(API + "/items-by-job-order")
    @ResponseBody public ResponseEntity<?> itemsByJobOrder(@RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> service.itemsByJobOrder(jobOrderId));
    }

    @GetMapping(API + "/plants")
    @ResponseBody public ResponseEntity<?> plants(@RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> service.plants(jobOrderId));
    }

    @GetMapping(API + "/totals")
    @ResponseBody public ResponseEntity<?> totals(@RequestParam(defaultValue = "0") int jobOrderId,
                                                 @RequestParam(defaultValue = "0") int plantId) {
        return run(() -> service.totals(jobOrderId, plantId));
    }

    @GetMapping(API + "/return-to-godown")
    @ResponseBody public ResponseEntity<?> returnToGodown(@RequestParam(defaultValue = "0") int jobOrderId,
                                                         @RequestParam(defaultValue = "0") int plantId) {
        return run(() -> service.returnToGodown(jobOrderId, plantId));
    }

    @GetMapping(API + "/export-contracts")
    @ResponseBody public ResponseEntity<?> exportContracts(@RequestParam(defaultValue = "0") int jobOrderId,
                                                          @RequestParam(defaultValue = "0") int recId) {
        return run(() -> service.exportContracts(jobOrderId, recId));
    }

    @GetMapping(API + "/last-item-rate")
    @ResponseBody public ResponseEntity<?> lastItemRate(@RequestParam(defaultValue = "0") int jobOrderId,
                                                       @RequestParam(defaultValue = "0") int typeId,
                                                       @RequestParam(defaultValue = "0") int itemId,
                                                       @RequestParam(required = false) String docDate) {
        return run(() -> service.lastItemRate(jobOrderId, typeId, itemId, docDate));
    }

    @GetMapping(API + "/avg-rate-return-to-godown")
    @ResponseBody public ResponseEntity<?> avgRate(@RequestParam(defaultValue = "0") int jobOrderId,
                                                  @RequestParam(defaultValue = "0") int plantId,
                                                  @RequestParam(defaultValue = "0") int itemId,
                                                  @RequestParam(defaultValue = "") String cropYear) {
        return run(() -> service.avgRateReturnToGodown(jobOrderId, plantId, itemId, cropYear));
    }

    @GetMapping(API + "/summary-values")
    @ResponseBody public ResponseEntity<?> summaryValues(@RequestParam(defaultValue = "0") int jobOrderId,
                                                        @RequestParam(defaultValue = "0") double fgWeight) {
        return run(() -> service.summaryValues(jobOrderId, fgWeight));
    }

    // ---------------------------------------------------------------------------- record

    @GetMapping(API + "/{id:\\d+}")
    @ResponseBody public ResponseEntity<?> read(@PathVariable int id) { return run(() -> service.read(id)); }

    @PostMapping(API + "/save")
    @ResponseBody public ResponseEntity<?> save(@RequestBody Map<String, Object> body) {
        return run(() -> service.save(body));
    }

    // ---------------------------------------------------------------------------- History tab

    @GetMapping(API + "/history-job-orders")
    @ResponseBody public ResponseEntity<?> historyJobOrders() { return run(service::historyJobOrders); }

    @GetMapping(API + "/history")
    @ResponseBody public ResponseEntity<?> history(@RequestParam(required = false) String fromDate,
                                                  @RequestParam(required = false) String toDate,
                                                  @RequestParam(required = false) String docFrom,
                                                  @RequestParam(required = false) String docTo,
                                                  @RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> service.history(fromDate, toDate, docFrom, docTo, jobOrderId));
    }

    // ---------------------------------------------------------------------------- dialogs

    @GetMapping(API + "/pending-rates/job-orders")
    @ResponseBody public ResponseEntity<?> pendingRatesJobOrders() { return run(service::pendingRatesJobOrders); }

    @GetMapping(API + "/pending-rates")
    @ResponseBody public ResponseEntity<?> pendingRates(@RequestParam(required = false) String fromDate,
                                                       @RequestParam(required = false) String toDate,
                                                       @RequestParam(defaultValue = "0") int jobOrderId,
                                                       @RequestParam(required = false) String entryType) {
        return run(() -> service.pendingRates(fromDate, toDate, jobOrderId, entryType));
    }

    @GetMapping(API + "/move-orders")
    @ResponseBody public ResponseEntity<?> moveOrders(@RequestParam(required = false) String fromDate,
                                                     @RequestParam(required = false) String toDate,
                                                     @RequestParam(required = false) String docFrom,
                                                     @RequestParam(required = false) String docTo,
                                                     @RequestParam(required = false) String vehicleNo,
                                                     @RequestParam(required = false) String biltyNo) {
        return run(() -> service.moveOrders(fromDate, toDate, docFrom, docTo, vehicleNo, biltyNo));
    }

    // ---------------------------------------------------------------------------- plumbing

    private static Map<String, Object> one(String k, Object v) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put(k, v);
        return m;
    }

    /**
     * The desktop shows ex.Message in a box; a refusal or a RAISERROR text is returned the same
     * way ({success:false, message}) so the page can show exactly that text.
     */
    private static ResponseEntity<?> run(Callable<?> c) {
        try {
            return ResponseEntity.ok(c.call());
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(e));
        } catch (Exception e) {
            LOG.warn("p280 output", e);
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(e));
        }
    }

    private static Map<String, Object> fail(Throwable e) {
        Throwable t = e;
        /* A RAISERROR travels as the root SQLException's message - the text the desktop shows. */
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String msg = t.getMessage() == null ? e.getMessage() : t.getMessage();
        if (msg == null) msg = e.getClass().getSimpleName();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", msg);
        m.put("error", msg);
        return m;
    }
}
