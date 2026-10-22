package com.mst.controllers;

import com.mst.services.ProductionInputService;
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
import java.util.function.Supplier;

/**
 * 280 Production Against (Job Order) - the INPUT tab: Architecture.WinApp.Production/frmProductionInput.cs
 * (DocumentTypeId 80), hosted by the shell in an iframe exactly as the desktop drops the form into
 * PanelOtherForms.
 *
 * No parameter here carries tenancy, branch, financial year or the user: all are derived in the
 * service from the signed-in user.
 */
@Controller
public class ProductionInputController {

    private static final Logger LOG = LoggerFactory.getLogger(ProductionInputController.class);

    @Autowired
    private ProductionInputService service;

    @GetMapping("/production/production-against-job-order/input")
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/p280_input";
    }

    /** frmFoodProduction_Load:351 - switches, rights, doc number and every picker. */
    @GetMapping("/api/production/production-against-job-order/input/state")
    @ResponseBody
    public ResponseEntity<?> state() { return run(service::state); }

    /** btnRefresh_Click:1735 */
    @GetMapping("/api/production/production-against-job-order/input/refresh")
    @ResponseBody
    public ResponseEntity<?> refresh() { return run(service::refresh); }

    /** GenerateDocNumberInput:1045 (called again by reset()). */
    @GetMapping("/api/production/production-against-job-order/input/serial")
    @ResponseBody
    public ResponseEntity<?> serial() {
        return run(() -> { Map<String, Object> m = new LinkedHashMap<>(); m.put("docNumber", service.serial()); return m; });
    }

    /** CmbJobOrderNo_Leave:1130 */
    @GetMapping("/api/production/production-against-job-order/input/job-order")
    @ResponseBody
    public ResponseEntity<?> jobOrder(@RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> service.jobOrder(jobOrderId));
    }

    /** cmbWareHouseInput_ValueChanged:1165 -> GetItemByWareHouseId */
    @GetMapping("/api/production/production-against-job-order/input/items")
    @ResponseBody
    public ResponseEntity<?> items(@RequestParam(defaultValue = "0") int warehouseId) {
        return run(() -> service.itemsByWarehouse(warehouseId));
    }

    /** GetAvailableStockForInput:899 */
    @GetMapping("/api/production/production-against-job-order/input/stock")
    @ResponseBody
    public ResponseEntity<?> stock(@RequestParam(defaultValue = "0") int warehouseId,
                                   @RequestParam(defaultValue = "0") int itemId,
                                   @RequestParam(defaultValue = "0") int jobLotId,
                                   @RequestParam(defaultValue = "") String cropYear,
                                   @RequestParam(required = false) String docDate) {
        return run(() -> service.stock(warehouseId, itemId, jobLotId, cropYear, docDate));
    }

    /** GetAvgRateByItemAndJoblot:1194 */
    @GetMapping("/api/production/production-against-job-order/input/avg-rate")
    @ResponseBody
    public ResponseEntity<?> avgRate(@RequestParam(defaultValue = "0") int itemId,
                                     @RequestParam(required = false) String docDate,
                                     @RequestParam(defaultValue = "0") int recId,
                                     @RequestParam(defaultValue = "0") int jobLotId,
                                     @RequestParam(defaultValue = "0") int cropYearId,
                                     @RequestParam(defaultValue = "") String cropYear,
                                     @RequestParam(defaultValue = "0") int warehouseId,
                                     @RequestParam(defaultValue = "0") int stockUom,
                                     @RequestParam(defaultValue = "0") int packingTypeId,
                                     @RequestParam(defaultValue = "0") double qty,
                                     @RequestParam(defaultValue = "0") double netWeight) {
        return run(() -> service.avgRate(itemId, docDate, recId, jobLotId, cropYearId, cropYear, warehouseId,
                stockUom, packingTypeId, qty, netWeight));
    }

    /** grdStockConversionProductionDetail_CellUpdated FIFO branch (:1810 / :1844) */
    @GetMapping("/api/production/production-against-job-order/input/fifo-rate")
    @ResponseBody
    public ResponseEntity<?> fifoRate(@RequestParam(required = false) String docDate,
                                      @RequestParam(defaultValue = "0") int warehouseId,
                                      @RequestParam(defaultValue = "0") int itemId,
                                      @RequestParam(defaultValue = "0") int jobLotId,
                                      @RequestParam(defaultValue = "0") int packingTypeId,
                                      @RequestParam(defaultValue = "") String cropYear,
                                      @RequestParam(defaultValue = "0") double qty,
                                      @RequestParam(defaultValue = "0") double netWeight) {
        return run(() -> service.fifoRateForRow(docDate, warehouseId, itemId, jobLotId, packingTypeId, cropYear, qty, netWeight));
    }

    /** BtnGenerateRates_Click -> AvgRateUpdateOnDocDateChange:2836 (one grid row per call) */
    @GetMapping("/api/production/production-against-job-order/input/generate-rate")
    @ResponseBody
    public ResponseEntity<?> generateRate(@RequestParam(required = false) String docDate,
                                          @RequestParam(defaultValue = "0") int recId,
                                          @RequestParam(defaultValue = "0") int itemId,
                                          @RequestParam(defaultValue = "0") int itemUomId,
                                          @RequestParam(defaultValue = "0") int packingTypeId,
                                          @RequestParam(defaultValue = "0") int warehouseId,
                                          @RequestParam(defaultValue = "") String cropYear,
                                          @RequestParam(defaultValue = "0") int jobLotId,
                                          @RequestParam(defaultValue = "0") double qty,
                                          @RequestParam(defaultValue = "0") double weight) {
        return run(() -> service.generateRate(docDate, recId, itemId, itemUomId, packingTypeId, warehouseId,
                cropYear, jobLotId, qty, weight));
    }

    /** ReadByIdInput:1575 / InputDetailByHeaderId:2609 */
    @GetMapping("/api/production/production-against-job-order/input/{id}")
    @ResponseBody
    public ResponseEntity<?> read(@PathVariable int id) {
        try {
            Map<String, Object> r = service.read(id);
            if (r == null) return ResponseEntity.notFound().build();
            return ResponseEntity.ok(r);
        } catch (Exception e) {
            return fail(e);
        }
    }

    /** InputGridHistory:2301 */
    @GetMapping("/api/production/production-against-job-order/input/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int docNoFrom,
                                     @RequestParam(defaultValue = "0") int docNoTo,
                                     @RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> service.history(fromDate, toDate, docNoFrom, docNoTo, jobOrderId));
    }

    /** BtnRefereshInputHistory_Click -> jobOrderInputHCombobind:2180 */
    @GetMapping("/api/production/production-against-job-order/input/history-job-orders")
    @ResponseBody
    public ResponseEntity<?> historyJobOrders() { return run(service::historyJobOrders); }

    /** GetVoucherHeadId(RecId, 80) - used by 118-Voucher. */
    @GetMapping("/api/production/production-against-job-order/input/voucher-head")
    @ResponseBody
    public ResponseEntity<?> voucherHead(@RequestParam(defaultValue = "0") int id) {
        return run(() -> { Map<String, Object> m = new LinkedHashMap<>(); m.put("voucherHeadId", service.voucherHeadId(id)); return m; });
    }

    /** btnsave_Click / btnUpdate_Click -> InsertInput:1385 */
    @PostMapping("/api/production/production-against-job-order/input/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) {
        try {
            return ResponseEntity.ok(service.save(body));
        } catch (ProductionInputService.InputMessage e) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("success", false);
            m.put("message", e.getMessage());
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            LOG.warn("Production Input save failed", e);
            return fail(e);
        }
    }

    // ------------------------------------------------------- LoadavailableTransactionsForIssuance

    @GetMapping("/api/production/production-against-job-order/input/loader/lookups")
    @ResponseBody
    public ResponseEntity<?> loaderLookups() { return run(() -> service.loaderLookups(null)); }

    @GetMapping("/api/production/production-against-job-order/input/loader/search")
    @ResponseBody
    public ResponseEntity<?> loaderSearch(@RequestParam(required = false) String fromDate,
                                          @RequestParam(required = false) String toDate,
                                          @RequestParam(defaultValue = "0") int parentCategory,
                                          @RequestParam(defaultValue = "0") int itemCategory,
                                          @RequestParam(defaultValue = "0") int itemType,
                                          @RequestParam(defaultValue = "0") int jobLotId,
                                          @RequestParam(defaultValue = "") String cropYear,
                                          @RequestParam(defaultValue = "0") int warehouseId,
                                          @RequestParam(defaultValue = "0") int refDocumentTypeId,
                                          @RequestParam(defaultValue = "0") int supplierId,
                                          @RequestParam(defaultValue = "0") int itemId,
                                          @RequestParam(defaultValue = "") String itemIds) {
        return run(() -> service.loaderSearch(fromDate, toDate, parentCategory, itemCategory, itemType, jobLotId,
                cropYear, warehouseId, refDocumentTypeId, supplierId, itemId, itemIds));
    }

    // ------------------------------------------------------------ LoadOutPutPendingforRates ("Issue")

    @GetMapping("/api/production/production-against-job-order/input/pending-rates/job-orders")
    @ResponseBody
    public ResponseEntity<?> pendingRateJobOrders() { return run(service::pendingRateJobOrders); }

    @GetMapping("/api/production/production-against-job-order/input/pending-rates")
    @ResponseBody
    public ResponseEntity<?> pendingRates(@RequestParam(required = false) String fromDate,
                                          @RequestParam(required = false) String toDate,
                                          @RequestParam(defaultValue = "0") int jobOrderId,
                                          @RequestParam(required = false) String entryType) {
        return run(() -> service.pendingRates(fromDate, toDate, jobOrderId, entryType));
    }

    // ------------------------------------------------------------ frmPendingMoveOrderDocuments

    @GetMapping("/api/production/production-against-job-order/input/move-orders")
    @ResponseBody
    public ResponseEntity<?> moveOrders(@RequestParam(required = false) String fromDate,
                                        @RequestParam(required = false) String toDate,
                                        @RequestParam(defaultValue = "0") int fromDocNo,
                                        @RequestParam(defaultValue = "0") int toDocNo,
                                        @RequestParam(defaultValue = "") String vehicleNo,
                                        @RequestParam(defaultValue = "") String biltyNo) {
        return run(() -> service.moveOrders(fromDate, toDate, fromDocNo, toDocNo, vehicleNo, biltyNo));
    }

    // ------------------------------------------------------------------------------ helpers

    private ResponseEntity<?> run(Supplier<?> s) {
        try {
            return ResponseEntity.ok(s.get());
        } catch (ProductionInputService.InputMessage e) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("success", false);
            m.put("message", e.getMessage());
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(m);
        } catch (Exception e) {
            LOG.warn("Production Input request failed", e);
            return fail(e);
        }
    }

    /** The desktop shows ex.Message; the root cause carries SQL Server's RAISERROR text. */
    private static ResponseEntity<Map<String, Object>> fail(Exception e) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", ProductionInputService.rootMessage(e));
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(m);
    }
}
