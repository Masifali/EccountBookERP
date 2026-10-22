package com.mst.controllers;

import com.mst.services.Prod284ProductionNewPostingService;
import com.mst.services.Prod284ProductionNewService;
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

import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Production (AppModules 18) - screen 284 "ProductionNew Against (Pre Costing Job Order)",
 * base.Name {@code FoodProductionWithValuesNew} (Architecture.WinApp.Production/FoodProductionWithValuesNew.cs),
 * at /production/production-new-against-job-order, JSON under /production/production-new-against-job-order/api.
 *
 * The seven desktop tabs (Input 80, OutPut 112, Packing Material 111, Over Head 110, Transaction
 * History, Summery, Settlement 142) are one page; the four "Load ... From Job Order" dialogs and
 * LoadOutPutPendingforRates are in-page modals; frmwagesBillHeader opens /production/wages-bill in an
 * overlay as the desktop's ShowDialog; "Define Job Order" links to the ProductionPreCostingAndJobOrder page.
 *
 * Errors: a desktop MessageBox validation -> 400 {success:false,message}; a missing right -> 403;
 * anything else -> 500 with the innermost (RAISERROR) text, as the desktop shows ex.Message.
 */
@Controller
@RequestMapping("/production/production-new-against-job-order")
public class Prod284ProductionNewAgainstJobOrderController {


    private final Prod284ProductionNewService service;
    private final Prod284ProductionNewPostingService posting;

    public Prod284ProductionNewAgainstJobOrderController(Prod284ProductionNewService service,
                                                         Prod284ProductionNewPostingService posting) {
        this.service = service;
        this.posting = posting;
    }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        model.addAttribute("moduleTitle", "ProductionNew Against (Pre Costing Job Order)");
        return "production/prod284_production_new_against_job_order";
    }

    // --------------------------------------------------------------------------- Load / lookups

    /** frmFoodProduction_Load:893 */
    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() { return run(service::init); }

    /** JobOrderNoFill:1180 (btnRefresh / btnRefreshOutPut / btnRefreshPackingMaterial / btnRefreshOverHead) */
    @GetMapping("/api/job-orders") @ResponseBody
    public ResponseEntity<?> jobOrders() { return run(service::jobOrders); }

    /** JobOrderNoFillForSummery:1223 (btnRefreshSummary / btnResetSettlement) */
    @GetMapping("/api/job-orders-all") @ResponseBody
    public ResponseEntity<?> jobOrdersAll() { return run(service::jobOrdersAll); }

    /** btnRefreshOutPut_Click:2385 re-reads the grid drop-down lists too */
    @GetMapping("/api/lists") @ResponseBody
    public ResponseEntity<?> lists() {
        return run(() -> {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("differenceAccounts", service.differenceAccounts());
            m.put("jobLots", Prod284ProductionNewService.project(service.jobLots(), "Id", "JobLotDescription"));
            m.put("warehouses", Prod284ProductionNewService.project(service.activeWarehouses(), "Id", "WareHouseName"));
            m.put("warehousesAll", service.allWarehouses());
            m.put("jobOrders", service.jobOrders());
            return m;
        });
    }

    /** gridsettingsOutPut:2622 / grdDetailPackingMaterialSettings:3859 - the grid's WareHouse list */
    @GetMapping("/api/warehouses-all") @ResponseBody
    public ResponseEntity<?> warehousesAll() { return run(service::allWarehouses); }

    /** GetPlantFeeder:1327 */
    @GetMapping("/api/plants") @ResponseBody
    public ResponseEntity<?> plants(@RequestParam(defaultValue = "0") int jobOrderId) { return run(() -> service.plants(jobOrderId)); }

    /** CmbJobOrderNo_Leave:1625 / CmbJobOrderNoOutput_Leave:2468 - WIP account, WIP item, FinishGoodRate */
    @GetMapping("/api/gl-accounts") @ResponseBody
    public ResponseEntity<?> glAccounts(@RequestParam(defaultValue = "0") int jobOrderId) { return run(() -> service.glAccounts(jobOrderId)); }

    /** GenerateDocNumberInput:1595 (80) / GenerateDocNumberOutPut:2438 (112) */
    @GetMapping("/api/serial") @ResponseBody
    public ResponseEntity<?> serial(@RequestParam(defaultValue = "80") int documentTypeId) {
        return run(() -> Collections.singletonMap("docNo", service.serial(documentTypeId)));
    }

    /** GenerateCodeOfPackingMaterial:3371 */
    @GetMapping("/api/pm/next-code") @ResponseBody
    public ResponseEntity<?> pmNextCode() { return run(() -> Collections.singletonMap("docNo", service.pmNextCode())); }

    /** GenerateCodeofOverHead:4170 */
    @GetMapping("/api/oh/next-code") @ResponseBody
    public ResponseEntity<?> ohNextCode() { return run(() -> Collections.singletonMap("docNo", service.ohNextCode())); }

    /** GetInPutTotalQtyandWeightByJobOrderId (:2482, :3424, :4220) */
    @GetMapping("/api/totals") @ResponseBody
    public ResponseEntity<?> totals(@RequestParam(defaultValue = "0") int jobOrderId) { return run(() -> service.totals(jobOrderId)); }

    /** GetVoucherHeadId:1369 */
    @GetMapping("/api/voucher-head") @ResponseBody
    public ResponseEntity<?> voucherHead(@RequestParam(defaultValue = "0") int id, @RequestParam(defaultValue = "0") int documentTypeId) {
        return run(() -> Collections.singletonMap("voucherHeadId", service.voucherHeadId(id, documentTypeId)));
    }

    /** GetUomScheduleByItemId (grdByProduct F1 :2764, settlement :6468) */
    @GetMapping("/api/uom-by-item") @ResponseBody
    public ResponseEntity<?> uomByItem(@RequestParam(defaultValue = "0") int itemId) { return run(() -> service.uomsForItem(itemId)); }

    // --------------------------------------------------------------------------- reads / histories

    /** InvFoodProduction.GetByID - ReadByIdInput:1881 / ReadByIdOutPut:2791 / the history detail grids */
    @GetMapping("/api/production/{id}") @ResponseBody
    public ResponseEntity<?> production(@PathVariable int id) { return run(() -> service.production(id)); }

    /** InvFoodProduction.GetAll - documentTypeId 80 (Input History), 112 (OutPut History), 0 (Transaction History) */
    @GetMapping("/api/history") @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "0") int documentTypeId) { return run(() -> service.history(documentTypeId)); }

    /** ReadByIdPackingMaterial:3677 */
    @GetMapping("/api/pm/{id}") @ResponseBody
    public ResponseEntity<?> pm(@PathVariable int id) { return run(() -> service.pm(id)); }

    /** BindHistoryPMGrid:3937 */
    @GetMapping("/api/pm/history") @ResponseBody
    public ResponseEntity<?> pmHistory(@RequestParam(defaultValue = "false") boolean pendingForRates) { return run(() -> service.pmHistory(pendingForRates)); }

    /** ReadByIdOverHeadJobOrderWise:4432 / grdOverHeadHistory_SelectionChanged:4856 */
    @GetMapping("/api/oh/by-job-order/{jobOrderId}") @ResponseBody
    public ResponseEntity<?> ohByJobOrder(@PathVariable int jobOrderId) { return run(() -> service.ohByJobOrder(jobOrderId)); }

    /** BindGridHistoryOverHead:4736 */
    @GetMapping("/api/oh/history") @ResponseBody
    public ResponseEntity<?> ohHistory() { return run(service::ohHistory); }

    // --------------------------------------------------------------------------- loader dialogs

    @GetMapping("/api/load/input") @ResponseBody
    public ResponseEntity<?> loadInput(@RequestParam int jobOrderId) { return run(() -> service.loadInput(jobOrderId)); }

    @GetMapping("/api/load/output") @ResponseBody
    public ResponseEntity<?> loadOutput(@RequestParam int jobOrderId) { return run(() -> service.loadOutput(jobOrderId)); }

    @GetMapping("/api/load/pm") @ResponseBody
    public ResponseEntity<?> loadPm(@RequestParam int jobOrderId) { return run(() -> service.loadPm(jobOrderId)); }

    @GetMapping("/api/load/oh") @ResponseBody
    public ResponseEntity<?> loadOh(@RequestParam int jobOrderId) { return run(() -> service.loadOh(jobOrderId)); }

    /** LoadOutPutPendingforRates (btnPenForRateEntries_Click:3255) */
    @GetMapping("/api/pending-rates/job-orders") @ResponseBody
    public ResponseEntity<?> pendingRateJobOrders() { return run(service::pendingRateJobOrders); }

    @GetMapping("/api/pending-rates") @ResponseBody
    public ResponseEntity<?> pendingRates(@RequestParam(defaultValue = "") String from, @RequestParam(defaultValue = "") String to,
                                          @RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> service.pendingRates(from, to, jobOrderId));
    }

    // --------------------------------------------------------------------------- Summery / Settlement

    /** GetSummeryValues (:5455, :5927, :6400, :6906) */
    @GetMapping("/api/summary-values") @ResponseBody
    public ResponseEntity<?> summaryValues(@RequestParam(defaultValue = "0") int jobOrderId, @RequestParam(defaultValue = "0") double netWeight) {
        return run(() -> service.summaryValues(jobOrderId, netWeight));
    }

    /** GrdSummaryReportFill:5367 */
    @GetMapping("/api/summary/report") @ResponseBody
    public ResponseEntity<?> summaryReport(@RequestParam(defaultValue = "0") int jobOrderId, @RequestParam(defaultValue = "0") int plantId) {
        return run(() -> service.summaryReport(jobOrderId, plantId, 0));
    }

    /** grdDocWiseSummeryFill:5510 */
    @GetMapping("/api/summary/doc-wise") @ResponseBody
    public ResponseEntity<?> docWise(@RequestParam(defaultValue = "0") int jobOrderId, @RequestParam(defaultValue = "0") int plantId) {
        return run(() -> service.docWiseSummary(jobOrderId, plantId));
    }

    /** btnGeneralSettlement_Click:5768 - activity InPutOutPut / OverHeads / PackingMaterial */
    @GetMapping("/api/settlement") @ResponseBody
    public ResponseEntity<?> settlement(@RequestParam(defaultValue = "0") int jobOrderId, @RequestParam(defaultValue = "InPutOutPut") String activity) {
        return run(() -> service.settlement(jobOrderId, activity));
    }

    @PostMapping("/api/settlement/financials") @ResponseBody
    public ResponseEntity<?> settlementFinancials(@RequestBody Map<String, Object> body) { return run(() -> posting.settlementFinancials(body)); }

    @PostMapping("/api/settlement/approve") @ResponseBody
    public ResponseEntity<?> approve(@RequestBody Map<String, Object> body) {
        return run(() -> posting.approveSettlement(com.mst.repositories.ProductionInputRepository.toInt(body.get("jobOrderId"))));
    }

    // --------------------------------------------------------------------------- saves

    /** btnsave_Click:1856 / btnUpdate_Click:1869 -> InsertInput */
    @PostMapping("/api/input/save") @ResponseBody
    public ResponseEntity<?> saveInput(@RequestBody Map<String, Object> body) { return run(() -> posting.saveInput(body)); }

    /** btnSaveOutPut_Click:2866 / btnUpdateOutPut_Click:2879 -> OutPutInsert */
    @PostMapping("/api/output/save") @ResponseBody
    public ResponseEntity<?> saveOutput(@RequestBody Map<String, Object> body) { return run(() -> posting.saveOutput(body)); }

    /** btnsavepackingmaterial_Click:3652 / btnUpdatepackingmaterial_Click:3665 -> PackingMaterialInsert */
    @PostMapping("/api/pm/save") @ResponseBody
    public ResponseEntity<?> savePm(@RequestBody Map<String, Object> body) { return run(() -> posting.savePm(body)); }

    /** btnSaveOverHead_Click:4407 / btnUpdateOverHead_Click:4420 -> OverHeadInsert */
    @PostMapping("/api/oh/save") @ResponseBody
    public ResponseEntity<?> saveOh(@RequestBody Map<String, Object> body) { return run(() -> posting.saveOh(body)); }

    // --------------------------------------------------------------------------- print data sources

    /** 601-InvFoodProductionSlip.rpt rows (Print_Click:2188 / btnPrintOutPut_Click:3205) */
    @GetMapping("/api/print/601") @ResponseBody
    public ResponseEntity<?> print601(@RequestParam(defaultValue = "0") int id, @RequestParam(defaultValue = "") String entryType,
                                      @RequestParam(defaultValue = "0") int jobOrderId) {
        return rows(() -> service.print601(id, entryType, jobOrderId), "Record Not Found For Display");
    }

    /** 606 rows (btnPackingMaterialSlip_Click:4095 "PackingMaterial" / btnPrintOverHead_Click:5006 "OverHead") */
    @GetMapping("/api/print/606") @ResponseBody
    public ResponseEntity<?> print606(@RequestParam(defaultValue = "0") int jobOrderId, @RequestParam(defaultValue = "PackingMaterial") String reportType) {
        return rows(() -> service.print606(jobOrderId, reportType), "Record Not Found For DisPlay");
    }

    /** 607 rows (btn607ProductionIssuanceGrnWiseReport_Click:5723) */
    @GetMapping("/api/print/607") @ResponseBody
    public ResponseEntity<?> print607(@RequestParam(defaultValue = "0") int jobOrderId) {
        return rows(() -> service.print607(jobOrderId), "Record Not Found For DisPlay");
    }

    /** 613 / 615 / 602A rows (btn613SummaryReport_Click:6697, btn615Report_Click:6730, btnRecoveryReport_Click:6763) */
    @GetMapping("/api/print/summary") @ResponseBody
    public ResponseEntity<?> printSummary(@RequestParam String rpt, @RequestParam(defaultValue = "0") int jobOrderId,
                                          @RequestParam(defaultValue = "false") boolean includeWages) {
        return rows(() -> service.printSummary(rpt, jobOrderId, includeWages ? 1 : 0), "Record Not Found For DisPlay");
    }

    // --------------------------------------------------------------------------- plumbing

    private interface Call { Object get() throws Exception; }
    private interface Rows { List<Map<String, Object>> get() throws Exception; }

    private static ResponseEntity<?> rows(Rows c, String emptyMessage) {
        try {
            List<Map<String, Object>> r = c.get();
            if (r == null || r.isEmpty()) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail(emptyMessage));
            return ResponseEntity.ok(r);
        } catch (Exception e) {
            return error(e);
        }
    }

    private static ResponseEntity<?> run(Call c) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (Exception e) {
            return error(e);
        }
    }

    private static ResponseEntity<?> error(Exception e) {
        if (e instanceof Prod284ProductionNewService.Prod284Message || e instanceof IllegalArgumentException) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        }
        if (e instanceof IllegalStateException) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        }
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(Prod284ProductionNewService.rootMessage(e)));
    }

    /** {success:false, message} - the text the desktop would show in its MessageBox. */
    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message == null || message.isEmpty() ? "Request failed" : message);
        return m;
    }
}
