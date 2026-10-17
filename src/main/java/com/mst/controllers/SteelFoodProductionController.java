package com.mst.controllers;

import com.mst.services.SteelFoodProductionService;
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
 * 601 Food Production With Values New (Production Steel, module 107) —
 * Architecture.WinApp.Steel.Production/FoodProductionWithValuesNew_St.cs.
 * Documents: Input 1512, OutPut 1513, Over Head 1514; Summery / Settlement read the job order (1511).
 *
 * Page: /production/steel/food-production-with-values ; JSON under .../api/...
 * Prints are the registered steel reports (SteelPrintController): 1512-FoodProductionSlip.rpt (Input 1512 /
 * OutPut 1513), 1514-FoodProductionOverHeadJobOrderWise_Slip.rpt, 1513-FoodProductionSummery_WithOutValue.rpt,
 * 1513A_ProductionSummeryWithValues.rpt; 118 voucher = CrystalPrint 'acc-118'.
 */
@Controller
@RequestMapping("/production/steel/food-production-with-values")
public class SteelFoodProductionController {

    private final SteelFoodProductionService service;

    public SteelFoodProductionController(SteelFoodProductionService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "Food Production With Values New");
        return "production/steel_food_production_with_values";
    }

    /** frmFoodProduction_Load:726 */
    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups); }

    /** GenerateDocNumberInput / OutPut / GenerateCodeofOverHead + the job-order pickers (btnRefresh*) */
    @GetMapping("/api/next-codes") @ResponseBody
    public ResponseEntity<?> nextCodes() { return run(service::nextCodes); }

    /** CmbJobOrderNo_Leave:1348 / CmbJobOrderNoOutput_Leave:2306 / cmbJobOrderOverHead_Leave:3267 */
    @GetMapping("/api/job-order-info") @ResponseBody
    public ResponseEntity<?> jobOrderInfo(@RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> service.jobOrderInfo(jobOrderId));
    }

    /** LoadInputDataByJobOrder (btnLoadInvoices_Click:1845) */
    @GetMapping("/api/load-input") @ResponseBody
    public ResponseEntity<?> loadInput(@RequestParam int jobOrderId) { return run(() -> service.loadInput(jobOrderId)); }

    /** LoadOutPutDataByJobOrder (btnLoadOutPutFromjobOrder_Click:3090) */
    @GetMapping("/api/load-output") @ResponseBody
    public ResponseEntity<?> loadOutput(@RequestParam int jobOrderId) { return run(() -> service.loadOutput(jobOrderId)); }

    /** LoadOverHeadsDataByJobOrder (btnLoadOverHeadsFromJobOrder_Click:4145) */
    @GetMapping("/api/load-overheads") @ResponseBody
    public ResponseEntity<?> loadOverheads(@RequestParam int jobOrderId) { return run(() -> service.loadOverheads(jobOrderId)); }

    /** InputGridHistory:1968 (1512) / OutputGridHistory:2909 (1513) / GridHistory:4203 (0 = both) */
    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "0") int documentTypeId,
                                     @RequestParam(defaultValue = "50") int noOfRecords) {
        return run(() -> service.history(documentTypeId, noOfRecords));
    }

    /** GetVoucherHeadId:1121 */
    @GetMapping("/api/voucher-head") @ResponseBody
    public ResponseEntity<?> voucherHead(@RequestParam int id, @RequestParam int documentTypeId) {
        return run(() -> {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("voucherHeadId", service.voucherHeadId(id, documentTypeId));
            return m;
        });
    }

    /** GenerateSlipInput_Output:1920 pre-check */
    @GetMapping("/api/slip-check") @ResponseBody
    public ResponseEntity<?> slipCheck(@RequestParam int id, @RequestParam int documentTypeId) {
        return run(() -> service.slipPrintCheck(id, documentTypeId));
    }

    /** ReadByIdInput:1574 / ReadByIdOutPut:2628 / DatagridHistoryDetail:4322 */
    @GetMapping("/api/{id}") @ResponseBody
    public ResponseEntity<?> read(@PathVariable int id) {
        return run(() -> {
            Map<String, Object> m = service.read(id);
            if (m == null) throw new IllegalArgumentException("Record Not Found");
            return m;
        });
    }

    /** InsertInput:1414 / OutPutInsert:2731 */
    @PostMapping("/api/save") @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) { return run(() -> service.save(body)); }

    // ------------------------------------------------------------------------ Over Head

    /** BindGridHistoryOverHead:3792 */
    @GetMapping("/api/overhead/history") @ResponseBody
    public ResponseEntity<?> overheadHistory(@RequestParam(defaultValue = "50") int noOfRecords) {
        return run(() -> service.overheadHistory(noOfRecords));
    }

    /** ReadByIdOverHeadJobOrderWise:3493 / grdOverHeadHistory_SelectionChanged:3924 */
    @GetMapping("/api/overhead/by-job-order") @ResponseBody
    public ResponseEntity<?> overheadsByJobOrder(@RequestParam int jobOrderId) {
        return run(() -> service.overheadsByJobOrder(jobOrderId));
    }

    /** OverHeadInsert:3344 */
    @PostMapping("/api/overhead/save") @ResponseBody
    public ResponseEntity<?> saveOverheads(@RequestBody Map<String, Object> body) { return run(() -> service.saveOverheads(body)); }

    // ------------------------------------------------------------------------ Summery

    /** btnShow_Click:4468 */
    @GetMapping("/api/summary") @ResponseBody
    public ResponseEntity<?> summary(@RequestParam int jobOrderId, @RequestParam(defaultValue = "0") int plantId) {
        return run(() -> service.summary(jobOrderId, plantId));
    }

    /** btnProductionRecoveryReport_Click:4736 (1513) / btnRecoveryReport_Click:5653 (1513A) / GenerateOverHeadPrint:4089 (1514) */
    @GetMapping("/api/summary-print-check") @ResponseBody
    public ResponseEntity<?> summaryPrintCheck(@RequestParam int jobOrderId, @RequestParam(defaultValue = "1513") String which) {
        return run(() -> service.summaryPrintCheck(jobOrderId, which));
    }

    // ------------------------------------------------------------------------ Settlement

    /** btnGeneralSettlement_Click:4776 */
    @GetMapping("/api/settlement") @ResponseBody
    public ResponseEntity<?> settlement(@RequestParam int jobOrderId) { return run(() -> service.settlement(jobOrderId)); }

    /** btnApplyAvgRateFG_Click:5247 / txtFGWeight_TextChanged:5801 - GetSummeryValues with the FG net weight */
    @GetMapping("/api/summary-values") @ResponseBody
    public ResponseEntity<?> summaryValues(@RequestParam int jobOrderId, @RequestParam(defaultValue = "0") double netWeight) {
        return run(() -> service.summaryValuesFor(jobOrderId, netWeight));
    }

    /** btnUpdateSettlement_Click:5638 */
    @PostMapping("/api/settlement/update") @ResponseBody
    public ResponseEntity<?> updateSettlement(@RequestBody Map<String, Object> body) { return run(() -> service.updateSettlement(body)); }

    /** btnApproveSettlement_Click:5693 */
    @PostMapping("/api/settlement/approve") @ResponseBody
    public ResponseEntity<?> approveSettlement(@RequestParam int jobOrderId) { return run(() -> service.approveSettlement(jobOrderId)); }

    // ------------------------------------------------------------------------ helpers

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
