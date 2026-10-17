package com.mst.controllers;

import com.mst.services.Prod871FoodProductionSaver.FormMessage;
import com.mst.services.Prod871ProductionAgainstPreCostingJobOrderService;
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
 * 871 Production Against (Pre Costing Job Order) - Architecture.WinApp.Production/ProductionAgainstPreCostingJobOrder.cs
 * (Production hub, module 18). Page + every JSON endpoint of the eight tabs under {@code /api/...}.
 */
@Controller
@RequestMapping("/production/production-against-pre-costing-job-order")
public class Prod871ProductionAgainstPreCostingJobOrderController {

    private final Prod871ProductionAgainstPreCostingJobOrderService service;

    public Prod871ProductionAgainstPreCostingJobOrderController(Prod871ProductionAgainstPreCostingJobOrderService service) {
        this.service = service;
    }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Production Against (Pre Costing Job Order)");
        return "production/production_against_pre_costing_job_order";
    }

    // ------------------------------------------------------------------------------- load

    /** frmFoodProduction_Load:1227 - switches, rights, the shared combos, history pickers, doc numbers. */
    @GetMapping("/api/state") @ResponseBody
    public ResponseEntity<?> state() { return run(service::state); }

    /** JobOrderNoFill:1585 (Refresh buttons). */
    @GetMapping("/api/job-orders") @ResponseBody
    public ResponseEntity<?> jobOrders() { return run(service::jobOrders); }

    /** JobOrderNoFillForSummery:1669 - branchIds ",1,2" from the ticked branches. */
    @GetMapping("/api/summary-job-orders") @ResponseBody
    public ResponseEntity<?> summaryJobOrders(@RequestParam(defaultValue = "") String branchIds) {
        return run(() -> service.summaryJobOrders(branchIds));
    }

    /** JobOrderNoFillForSettlement(ActionId):1708 - 1 = pending, 0 = all ("All Job Orders"). */
    @GetMapping("/api/settlement-job-orders") @ResponseBody
    public ResponseEntity<?> settlementJobOrders(@RequestParam(defaultValue = "1") int actionId) {
        return run(() -> service.settlementJobOrders(actionId));
    }

    /** The history job-order pickers (documentTypeIds "80" / "112" / "181"). */
    @GetMapping("/api/history-job-orders") @ResponseBody
    public ResponseEntity<?> historyJobOrders(@RequestParam String documentTypeIds) {
        return run(() -> service.historyJobOrders(documentTypeIds));
    }

    /** jobOrderTransactionHistoryCombobind:4080. */
    @GetMapping("/api/transaction-history-combos") @ResponseBody
    public ResponseEntity<?> transactionHistoryCombos() { return run(service::transactionHistoryCombos); }

    /** GenerateDocNumberInput / OutPut / Consumption (documentTypeId 80 / 112 / 181). */
    @GetMapping("/api/serial") @ResponseBody
    public ResponseEntity<?> serial(@RequestParam int documentTypeId) {
        return run(() -> one("docNo", service.serial(documentTypeId)));
    }

    /** GenerateCodeOfPackingMaterial:4543. */
    @GetMapping("/api/pm/next-code") @ResponseBody
    public ResponseEntity<?> pmCode() { return run(() -> one("docNo", service.packingMaterialCode())); }

    /** GenerateCodeofOverHead:5331. */
    @GetMapping("/api/oh/next-code") @ResponseBody
    public ResponseEntity<?> ohCode() { return run(() -> one("docNo", service.overheadCode())); }

    // --------------------------------------------------------------------- job order cascade

    /** CmbJobOrderNo_Leave / cmbJobOrderConsumption_Leave / CmbJobOrderNoOutput_Leave - WIP account, WIP item, FinishGoodRate, plants. */
    @GetMapping("/api/job-order/{id}") @ResponseBody
    public ResponseEntity<?> jobOrder(@PathVariable int id) { return run(() -> service.jobOrderCascade(id)); }

    /** GetPlantFeeder:1755. */
    @GetMapping("/api/job-order/{id}/plants") @ResponseBody
    public ResponseEntity<?> plants(@PathVariable int id) { return run(() -> service.plants(id)); }

    /** OutPutPlantFillFromProductionInput:1831. */
    @GetMapping("/api/job-order/{id}/plants-from-input") @ResponseBody
    public ResponseEntity<?> plantsFromInput(@PathVariable int id) { return run(() -> service.plantsFromInput(id)); }

    /** cmbsummeryJobOrderNo_TextChanged:6521 - GetPlantByJobOrderId. */
    @GetMapping("/api/job-order/{id}/plants-summary") @ResponseBody
    public ResponseEntity<?> plantsSummary(@PathVariable int id) { return run(() -> service.plantsForSummary(id)); }

    /** GetInPutTotalQtyandWeightByJobOrderId rows (Output / PM / Overhead totals). */
    @GetMapping("/api/totals") @ResponseBody
    public ResponseEntity<?> totals(@RequestParam int jobOrderId, @RequestParam(defaultValue = "0") int plantId) {
        return run(() -> service.inputTotals(jobOrderId, plantId));
    }

    // ------------------------------------------------------------------------------ loaders

    @GetMapping("/api/loader/input") @ResponseBody
    public ResponseEntity<?> loadInput(@RequestParam int jobOrderId) { return run(() -> service.loadInput(jobOrderId)); }

    @GetMapping("/api/loader/output") @ResponseBody
    public ResponseEntity<?> loadOutput(@RequestParam int jobOrderId) { return run(() -> service.loadOutput(jobOrderId)); }

    @GetMapping("/api/loader/packing-material") @ResponseBody
    public ResponseEntity<?> loadPm(@RequestParam int jobOrderId) { return run(() -> service.loadPackingMaterial(jobOrderId)); }

    @GetMapping("/api/loader/overheads") @ResponseBody
    public ResponseEntity<?> loadOh(@RequestParam int jobOrderId) { return run(() -> service.loadOverheads(jobOrderId)); }

    /** LoadOutPutPendingforRates / LoadConsumptionPendingforRates - the job order picker. */
    @GetMapping("/api/pending-rates/job-orders") @ResponseBody
    public ResponseEntity<?> pendingRateJobOrders() { return run(service::pendingRateJobOrders); }

    /** LoadOutPutPendingforRates (entryType "Issue" for Input, "ByProduct" for Output). */
    @GetMapping("/api/pending-rates") @ResponseBody
    public ResponseEntity<?> pendingRates(@RequestParam(defaultValue = "") String from, @RequestParam(defaultValue = "") String to,
                                          @RequestParam(defaultValue = "0") int jobOrderId, @RequestParam(defaultValue = "") String entryType) {
        return run(() -> service.pendingForRates(from, to, jobOrderId, entryType));
    }

    /** LoadConsumptionPendingforRates. */
    @GetMapping("/api/consumption/pending-rates") @ResponseBody
    public ResponseEntity<?> consumptionPendingRates(@RequestParam(defaultValue = "") String from, @RequestParam(defaultValue = "") String to,
                                                     @RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> service.consumptionPendingForRates(from, to, jobOrderId));
    }

    /** LoadavailableTransactionsForIssuanceOnConsumption - the filter combos (one row set split by Activity). */
    @GetMapping("/api/consumption/issuance-loader/combos") @ResponseBody
    public ResponseEntity<?> issuanceCombos() { return run(service::issuanceDropDowns); }

    /** LoadavailableTransactionsForIssuanceOnConsumption - Search. */
    @GetMapping("/api/consumption/issuance-loader/search") @ResponseBody
    public ResponseEntity<?> issuanceSearch(@RequestParam(defaultValue = "") String from, @RequestParam(defaultValue = "") String to,
                                            @RequestParam(defaultValue = "0") int parentCategoryId, @RequestParam(defaultValue = "0") int itemCategoryId,
                                            @RequestParam(defaultValue = "0") int itemTypeId, @RequestParam(defaultValue = "0") int jobLotId,
                                            @RequestParam(defaultValue = "") String cropYear, @RequestParam(defaultValue = "0") int warehouseId,
                                            @RequestParam(defaultValue = "0") int refDocumentTypeId, @RequestParam(defaultValue = "0") int supplierCustomerId,
                                            @RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.issuanceSearch(from, to, parentCategoryId, itemCategoryId, itemTypeId, jobLotId, cropYear,
                warehouseId, refDocumentTypeId, supplierCustomerId, itemId));
    }

    // ----------------------------------------------------------------- consumption detail entry

    /** ConsumptionDetailComboBind:9225. */
    @GetMapping("/api/consumption/detail-combos") @ResponseBody
    public ResponseEntity<?> consumptionCombos() { return run(service::consumptionDetailCombos); }

    /** GetAvailableStockForConsumption:10493. */
    @GetMapping("/api/stock") @ResponseBody
    public ResponseEntity<?> stock(@RequestParam(defaultValue = "0") int warehouseId, @RequestParam(defaultValue = "0") int itemId,
                                   @RequestParam(defaultValue = "0") int jobLotId, @RequestParam(defaultValue = "") String cropYear,
                                   @RequestParam(defaultValue = "") String toDate) {
        return run(() -> service.stock(warehouseId, itemId, jobLotId, cropYear, toDate));
    }

    /** CommonServices.AvgRateOnlyForCGS. */
    @GetMapping("/api/avg-rate-cgs") @ResponseBody
    public ResponseEntity<?> avgRateCgs(@RequestParam int itemId, @RequestParam(defaultValue = "") String docDate,
                                        @RequestParam(defaultValue = "0") int documentTypeId, @RequestParam(defaultValue = "0") int recId,
                                        @RequestParam(defaultValue = "0") int jobLotId, @RequestParam(defaultValue = "0") int cropYearId,
                                        @RequestParam(required = false) String cropYear, @RequestParam(defaultValue = "0") int warehouseId) {
        return run(() -> one("rate", service.avgRateCgs(itemId, docDate, documentTypeId, recId, jobLotId, cropYearId, cropYear, warehouseId)));
    }

    /** CommonServices.GetAvgRateFromFIFOMethod. */
    @GetMapping("/api/fifo-rate") @ResponseBody
    public ResponseEntity<?> fifoRate(@RequestParam int itemId, @RequestParam(defaultValue = "0") int packUomId,
                                      @RequestParam(defaultValue = "") String docDate, @RequestParam(defaultValue = "0") int warehouseId,
                                      @RequestParam(defaultValue = "0") int jobLotId, @RequestParam(defaultValue = "0") int packingTypeId,
                                      @RequestParam(defaultValue = "") String cropYear) {
        return run(() -> one("rate", service.fifoRate(itemId, packUomId, docDate, warehouseId, jobLotId, packingTypeId, cropYear)));
    }

    /** CommonServices.GetUomScheduleByItemId (grdByProduct F1 on ItemUOM :3684). */
    @GetMapping("/api/uom-by-item") @ResponseBody
    public ResponseEntity<?> uomByItem(@RequestParam int itemId) { return run(() -> service.uomByItem(itemId)); }

    // ------------------------------------------------------------------------- read / history

    /** InvFoodProduction.GetByID (ReadByIdInput / ReadByIdOutPut / ReadByIdConsumption / history detail grids). */
    @GetMapping("/api/production/{id}") @ResponseBody
    public ResponseEntity<?> production(@PathVariable int id) { return run(() -> service.production(id)); }

    /** ReadByIdPackingMaterial:4841. */
    @GetMapping("/api/pm/{id}") @ResponseBody
    public ResponseEntity<?> packingMaterial(@PathVariable int id) { return run(() -> service.packingMaterial(id)); }

    /** ReadByIdOverHeadJobOrderWise:5589 / overhead history detail. */
    @GetMapping("/api/oh/by-job-order/{id}") @ResponseBody
    public ResponseEntity<?> overheadsByJobOrder(@PathVariable int id) { return run(() -> service.overheadsByJobOrder(id)); }

    /** CommonServices.VoucherHeadIdGet. */
    @GetMapping("/api/voucher-head") @ResponseBody
    public ResponseEntity<?> voucherHead(@RequestParam int id, @RequestParam int documentTypeId) {
        return run(() -> one("voucherHeadId", service.voucherHeadId(id, documentTypeId)));
    }

    /** ProductionFormHistory (documentTypeId 80 / 112 / 181 filters the rows; 0 = Transaction History, all). */
    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "") String from, @RequestParam(defaultValue = "") String to,
                                     @RequestParam(defaultValue = "0") int docNoFrom, @RequestParam(defaultValue = "0") int docNoTo,
                                     @RequestParam(defaultValue = "0") int jobOrderId, @RequestParam(defaultValue = "0") int plantId,
                                     @RequestParam(defaultValue = "0") int documentTypeId) {
        return run(() -> service.formHistory(from, to, docNoFrom, docNoTo, jobOrderId, plantId, documentTypeId));
    }

    /** BindHistoryPMGrid:4883. */
    @GetMapping("/api/pm/history") @ResponseBody
    public ResponseEntity<?> pmHistory(@RequestParam(defaultValue = "false") boolean pendingForRates) {
        return run(() -> service.packingMaterialHistory(pendingForRates));
    }

    /** BindGridHistoryOverHead:5877. */
    @GetMapping("/api/oh/history") @ResponseBody
    public ResponseEntity<?> ohHistory() { return run(service::overheadHistory); }

    // -------------------------------------------------------------------- summary / settlement

    /** btnShow_Click:6557. */
    @GetMapping("/api/summary") @ResponseBody
    public ResponseEntity<?> summary(@RequestParam(defaultValue = "0") int jobOrderId, @RequestParam(defaultValue = "0") int plantId,
                                     @RequestParam(defaultValue = "") String from, @RequestParam(defaultValue = "") String to,
                                     @RequestParam(defaultValue = "") String branchIds) {
        return run(() -> service.summary(jobOrderId, plantId, from, to, branchIds));
    }

    /** btnGeneralSettlement_Click:7079. */
    @GetMapping("/api/settlement/generate") @ResponseBody
    public ResponseEntity<?> settlementGenerate(@RequestParam int jobOrderId) { return run(() -> service.settlementGenerate(jobOrderId)); }

    /** txtFGWeight_TextChanged:8636. */
    @GetMapping("/api/summary-values") @ResponseBody
    public ResponseEntity<?> summaryValues(@RequestParam int jobOrderId, @RequestParam(defaultValue = "0") double fgWeight) {
        return run(() -> service.summaryValuesFg(jobOrderId, fgWeight));
    }

    /** btnUpdateSettlement_Click:8338. */
    @PostMapping("/api/settlement/update") @ResponseBody
    public ResponseEntity<?> settlementUpdate(@RequestBody Map<String, Object> body) { return run(() -> service.settlementUpdate(body)); }

    /** btnApproveSettlement_Click:8546. */
    @PostMapping("/api/settlement/approve") @ResponseBody
    public ResponseEntity<?> settlementApprove(@RequestBody Map<String, Object> body) {
        return run(() -> {
            service.approveSettlement(toInt(body.get("jobOrderId")));
            return one("message", "Record Approved Successfully");
        });
    }

    /** The row count a print button checks ("Record Not Found For DisPlay"). */
    @GetMapping("/api/report-rows") @ResponseBody
    public ResponseEntity<?> reportRows(@RequestParam String report, @RequestParam(defaultValue = "0") int jobOrderId,
                                        @RequestParam(defaultValue = "0") int actionId, @RequestParam(defaultValue = "0") int plantId,
                                        @RequestParam(defaultValue = "") String from, @RequestParam(defaultValue = "") String to,
                                        @RequestParam(defaultValue = "") String branchIds) {
        return run(() -> one("count", service.reportRowCount(report, jobOrderId, actionId, plantId, from, to, branchIds)));
    }

    // ---------------------------------------------------------------------------------- saves

    @PostMapping("/api/input/save") @ResponseBody
    public ResponseEntity<?> saveInput(@RequestBody Map<String, Object> body) { return run(() -> service.saveInput(body)); }

    @PostMapping("/api/output/save") @ResponseBody
    public ResponseEntity<?> saveOutput(@RequestBody Map<String, Object> body) { return run(() -> service.saveOutput(body)); }

    @PostMapping("/api/consumption/save") @ResponseBody
    public ResponseEntity<?> saveConsumption(@RequestBody Map<String, Object> body) { return run(() -> service.saveConsumption(body)); }

    @PostMapping("/api/pm/save") @ResponseBody
    public ResponseEntity<?> savePm(@RequestBody Map<String, Object> body) { return run(() -> service.savePackingMaterial(body)); }

    @PostMapping("/api/oh/save") @ResponseBody
    public ResponseEntity<?> saveOh(@RequestBody Map<String, Object> body) { return run(() -> service.saveOverheads(body)); }

    // -------------------------------------------------------------------------------- plumbing

    private static ResponseEntity<?> run(Callable<?> c) {
        try {
            return ResponseEntity.ok(c.call());
        } catch (FormMessage | IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(e));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(e));
        }
    }

    private static Map<String, Object> one(String key, Object value) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put(key, value);
        return m;
    }

    private static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        try { return Integer.parseInt(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    /** MessageBox.Show(ex.Message): the innermost cause, so a RAISERROR text reaches the operator unwrapped. */
    private static Map<String, Object> fail(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        String msg = t.getMessage();
        if (msg == null || msg.isEmpty()) msg = e.getMessage();
        m.put("message", msg == null ? t.getClass().getSimpleName() : msg);
        return m;
    }
}
