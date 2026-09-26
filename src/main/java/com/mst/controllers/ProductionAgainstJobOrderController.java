package com.mst.controllers;

import com.mst.services.ProductionAgainstJobOrderService;
import com.mst.services.ProductionConsumptionService;
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
import java.util.concurrent.Callable;

/**
 * 280 Production Against (Job Order) — Architecture.WinApp.Production/FoodProductionWithValues.cs
 *
 * The screen id is in this comment on purpose. It is the second of the two independent records
 * that let DashboardModuleService.WEB_ROUTES_BY_SCREEN_ID link a screen to a page without
 * guessing from a tile's wording — the first being the live rights dump. Any new screen page
 * should carry its id the same way.
 *
 * This controller serves the SHELL and the two tab pages FoodProductionWithValues implements
 * itself - Consumption (Form + History, DocumentTypeId 181) and Transaction History - with the two
 * loader forms the Consumption page opens. The six hosted forms are separate pages, framed.
 */
@Controller
public class ProductionAgainstJobOrderController {

    @Autowired
    private ProductionAgainstJobOrderService service;

    @Autowired
    private ProductionConsumptionService consumption;

    private static final String C = "/api/production/production-against-job-order/consumption";
    private static final String TH = "/api/production/production-against-job-order/transaction-history";

    // ------------------------------------------------- 280 Production Against (Job Order)

    @GetMapping("/production/production-against-job-order")
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/production_against_job_order";
    }

    /**
     * The seven configuration/rights switches and the tab strip they produce.
     *
     * The page cannot decide its own shape: whether the manual-entry panel exists, whether the
     * Rate column exists, and whether the Settlement and Overhead tabs exist at all are database
     * answers, exactly as frmFoodProduction_Load:452-516 reads them.
     */
    @GetMapping("/api/production/production-against-job-order/shell")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> shell() {
        try {
            return ResponseEntity.ok(service.shellState());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(e));
        }
    }

    /** Production Department — GetActiveWareHouseByWareHouseType(2). */
    @GetMapping("/api/production/production-against-job-order/departments")
    @ResponseBody
    public ResponseEntity<?> departments() {
        try {
            return ResponseEntity.ok(service.productionDepartments());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(e));
        }
    }

    /** Job Order picker — usp_getJobOrdersForProduction. */
    @GetMapping("/api/production/production-against-job-order/job-orders")
    @ResponseBody
    public ResponseEntity<?> jobOrders() {
        try {
            return ResponseEntity.ok(service.jobOrders());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(e));
        }
    }

    /** Plant picker — scoped to the chosen job order, as the desktop scopes it. */
    @GetMapping("/api/production/production-against-job-order/plants")
    @ResponseBody
    public ResponseEntity<?> plants(@RequestParam(defaultValue = "0") int jobOrderId) {
        try {
            return ResponseEntity.ok(service.plants(jobOrderId));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(e));
        }
    }

    /** WIP Account / WIP Item of a job order - cmbJobOrderConsumption_Leave:1433. */
    @GetMapping("/api/production/production-against-job-order/gl-accounts")
    @ResponseBody
    public ResponseEntity<?> glAccounts(@RequestParam(defaultValue = "0") int jobOrderId) {
        try {
            return ResponseEntity.ok(service.glAccounts(jobOrderId));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(e));
        }
    }

    /** UOMSchedule.Getall — the shared list every tab reads. */
    @GetMapping("/api/production/production-against-job-order/uom-schedules")
    @ResponseBody
    public ResponseEntity<?> uomSchedules() {
        try {
            return ResponseEntity.ok(service.uomSchedules());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(e));
        }
    }

    // ============================================================ Consumption tab page (DocType 181)

    /** Decimal formats (GetDecimalConfiguration) and ActiveYr.Start_Period. */
    @GetMapping(C + "/init")
    @ResponseBody
    public ResponseEntity<?> consumptionInit() { return run(() -> consumption.init()); }

    /** GenerateDocConsumption:1300. */
    @GetMapping(C + "/serial")
    @ResponseBody
    public ResponseEntity<?> consumptionSerial() { return run(() -> consumption.serial()); }

    /** ConsumptionDetailComboBind:1330 - USP_DropDownForConsumptionForm. */
    @GetMapping(C + "/detail-combos")
    @ResponseBody
    public ResponseEntity<?> consumptionDetailCombos() { return run(() -> consumption.detailCombos()); }

    /** GetAvailableStockForConsumption:2546. */
    @GetMapping(C + "/stock")
    @ResponseBody
    public ResponseEntity<?> consumptionStock(@RequestParam(defaultValue = "0") int warehouseId,
                                              @RequestParam(defaultValue = "0") int itemId,
                                              @RequestParam(defaultValue = "0") int jobLotId,
                                              @RequestParam(defaultValue = "") String cropYear,
                                              @RequestParam(defaultValue = "") String toDate) {
        return run(() -> consumption.stock(warehouseId, itemId, jobLotId, cropYear, toDate));
    }

    /** CommonServices.AvgRateOnlyForCGS. */
    @GetMapping(C + "/cgs-rate")
    @ResponseBody
    public ResponseEntity<?> consumptionCgsRate(@RequestParam(defaultValue = "0") int itemId,
                                                @RequestParam(defaultValue = "") String docDate,
                                                @RequestParam(defaultValue = "0") int documentTypeId,
                                                @RequestParam(defaultValue = "0") int recId,
                                                @RequestParam(defaultValue = "0") int jobLotId,
                                                @RequestParam(defaultValue = "0") int cropYearId,
                                                @RequestParam(required = false) String cropYear,
                                                @RequestParam(defaultValue = "0") int warehouseId) {
        return run(() -> consumption.cgsRate(itemId, docDate, documentTypeId, recId, jobLotId, cropYearId,
                cropYear, warehouseId));
    }

    /** CommonServices.GetAvgRateFromFIFOMethod. */
    @GetMapping(C + "/fifo-rate")
    @ResponseBody
    public ResponseEntity<?> consumptionFifoRate(@RequestParam(defaultValue = "") String docDate,
                                                 @RequestParam(defaultValue = "0") int itemId,
                                                 @RequestParam(defaultValue = "0") int stockUom,
                                                 @RequestParam(defaultValue = "0") int warehouseId,
                                                 @RequestParam(defaultValue = "0") int jobLotId,
                                                 @RequestParam(defaultValue = "0") int packingTypeId,
                                                 @RequestParam(required = false) String cropYear,
                                                 @RequestParam(defaultValue = "0") double itemQty,
                                                 @RequestParam(defaultValue = "0") double netWeight,
                                                 @RequestParam(defaultValue = "0") int documentTypeId,
                                                 @RequestParam(defaultValue = "0") int id) {
        return run(() -> consumption.fifoRate(docDate, itemId, stockUom, warehouseId, jobLotId, packingTypeId,
                cropYear, itemQty, netWeight, documentTypeId, id));
    }

    /** ReadByIdConsumption:2341 / DatagridHistoryDetail:1130 - InvFoodProduction.GetByID. */
    @GetMapping(C + "/{id}")
    @ResponseBody
    public ResponseEntity<?> consumptionRead(@PathVariable int id) { return run(() -> consumption.readById(id)); }

    /** InsertConsumption:2174 - InvFoodProduction.Save. */
    @PostMapping(C + "/save")
    @ResponseBody
    public ResponseEntity<?> consumptionSave(@RequestBody Map<String, Object> body) {
        return run(() -> consumption.save(body));
    }

    /** btnPrintVoucherConsumption_Click:2426 - CommonServices.VoucherHeadIdGet(Id, 181). */
    @GetMapping(C + "/voucher-head")
    @ResponseBody
    public ResponseEntity<?> consumptionVoucherHead(@RequestParam(defaultValue = "0") int id) {
        return run(() -> consumption.voucherHeadId(id));
    }

    /** jobOrderConsumptionHCombobind:2858. */
    @GetMapping(C + "/history/job-orders")
    @ResponseBody
    public ResponseEntity<?> consumptionHistoryJobOrders() { return run(() -> consumption.historyJobOrders()); }

    /** GrdHistoryConsumptionMain:2928. */
    @GetMapping(C + "/history")
    @ResponseBody
    public ResponseEntity<?> consumptionHistory(@RequestParam(defaultValue = "") String from,
                                                @RequestParam(defaultValue = "") String to,
                                                @RequestParam(defaultValue = "") String docFrom,
                                                @RequestParam(defaultValue = "") String docTo,
                                                @RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> consumption.history(from, to, docFrom, docTo, jobOrderId));
    }

    /** LoadConsumptionPendingforRates - JobOrderFill:113. */
    @GetMapping(C + "/pending-rates/job-orders")
    @ResponseBody
    public ResponseEntity<?> pendingJobOrders() { return run(() -> consumption.pendingJobOrders()); }

    /** LoadConsumptionPendingforRates - OutputGridHistory:189. */
    @GetMapping(C + "/pending-rates")
    @ResponseBody
    public ResponseEntity<?> pendingForRates(@RequestParam(defaultValue = "") String from,
                                             @RequestParam(defaultValue = "") String to,
                                             @RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> consumption.pendingForRates(from, to, jobOrderId));
    }

    /** LoadavailableTransactionsForIssuanceOnConsumption_Load:520. */
    @GetMapping(C + "/issuance-loader/init")
    @ResponseBody
    public ResponseEntity<?> issuanceLoaderInit() { return run(() -> consumption.loaderInit()); }

    /** The loader's Refresh (ConsumptionDetailComboBind). */
    @GetMapping(C + "/issuance-loader/combos")
    @ResponseBody
    public ResponseEntity<?> issuanceLoaderCombos() { return run(() -> consumption.loaderCombos()); }

    /** PendingInventoryTransactionsForIssuanceLoad:289. */
    @GetMapping(C + "/issuance-loader/search")
    @ResponseBody
    public ResponseEntity<?> issuanceLoaderSearch(@RequestParam(defaultValue = "") String from,
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
        return run(() -> consumption.loaderSearch(from, to, parentCategoryId, itemCategoryId, itemTypeId, jobLotId,
                cropYear, warehouseId, refDocumentTypeId, supplierCustomerId, itemId));
    }

    // ===================================================================== Transaction History tab

    /** jobOrderTransactionHistoryCombobind:931. */
    @GetMapping(TH + "/combos")
    @ResponseBody
    public ResponseEntity<?> transactionHistoryCombos() { return run(() -> consumption.transactionHistoryCombos()); }

    /** GridHistory:998. */
    @GetMapping(TH)
    @ResponseBody
    public ResponseEntity<?> transactionHistory(@RequestParam(defaultValue = "") String from,
                                                @RequestParam(defaultValue = "") String to,
                                                @RequestParam(defaultValue = "") String docFrom,
                                                @RequestParam(defaultValue = "") String docTo,
                                                @RequestParam(defaultValue = "0") int jobOrderId,
                                                @RequestParam(defaultValue = "0") int plantId) {
        return run(() -> consumption.transactionHistory(from, to, docFrom, docTo, jobOrderId, plantId));
    }

    private static ResponseEntity<?> run(Callable<?> c) {
        try {
            return ResponseEntity.ok(plain(c.call()));
        } catch (IllegalArgumentException | IllegalStateException | NullPointerException e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(e));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(e));
        }
    }

    /**
     * Dates leave as local "yyyy-MM-ddTHH:mm:ss" text. A java.sql.Timestamp handed to Jackson is
     * written in UTC, which moves a midnight document date to the previous day east of Greenwich;
     * the desktop shows the value the database holds, so the page must receive that value.
     */
    @SuppressWarnings("unchecked")
    private static Object plain(Object o) {
        if (o instanceof java.sql.Timestamp) return ((java.sql.Timestamp) o).toLocalDateTime().withNano(0).toString();
        if (o instanceof java.sql.Date) return ((java.sql.Date) o).toLocalDate().toString();
        if (o instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) o).getTime()).toLocalDateTime().withNano(0).toString();
        if (o instanceof java.time.temporal.TemporalAccessor) return o.toString();
        if (o instanceof Map) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : ((Map<String, Object>) o).entrySet()) m.put(e.getKey(), plain(e.getValue()));
            return m;
        }
        if (o instanceof List) {
            List<Object> l = new java.util.ArrayList<>();
            for (Object x : (List<Object>) o) l.add(plain(x));
            return l;
        }
        return o;
    }

    /**
     * A failed lookup is reported, never swallowed into an empty list: an empty picker and a
     * broken query look identical on screen otherwise, and the operator has no way to tell which
     * they are looking at. The innermost cause is used, so a procedure's RAISERROR text reaches the
     * operator as the desktop's MessageBox(ex.Message) shows it, without the JDBC wrapping.
     */
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
