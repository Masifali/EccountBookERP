package com.mst.controllers;

import com.mst.services.ProductionPackingMaterialService;
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
 * Screen 280 "Production Against (Job Order)", tab PackingMaterial -
 * Architecture.WinApp.Production.frmProductionPackingMaterial (src280/frmProductionPackingMaterial.cs).
 *
 * Hosted in the 280 shell's iframe (FoodProductionWithValues.tabControl1_SelectedIndexChanged:867
 * drops `new frmProductionPackingMaterial(UserAccount)` into PanelOtherForms) and usable standalone.
 * No parameter here carries tenancy, the financial year or a user id: all are server-derived.
 */
@Controller
public class ProductionPackingMaterialController {

    private static final String API = "/api/production/production-against-job-order/packing-material";

    @Autowired
    private ProductionPackingMaterialService service;

    @GetMapping("/production/production-against-job-order/packing-material")
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/p280_packing_material";
    }

    /** frmFoodProduction_Load:272. */
    @GetMapping(API + "/init")
    @ResponseBody
    public ResponseEntity<?> init() { return run(service::init); }

    /** btnRefreshPackingMaterial_Click:1000 (ScheduleNoDbCall uses RecpackingMaterial). */
    @GetMapping(API + "/refresh")
    @ResponseBody
    public ResponseEntity<?> refresh(@RequestParam(defaultValue = "0") int recId) {
        return run(() -> service.refresh(recId));
    }

    /** GenerateCodeOfPackingMaterial:666. */
    @GetMapping(API + "/next-code")
    @ResponseBody
    public ResponseEntity<?> nextCode() {
        return run(() -> one("docNo", service.nextCode()));
    }

    /** GetPlantFeeder:391. */
    @GetMapping(API + "/plants")
    @ResponseBody
    public ResponseEntity<?> plants(@RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> service.plants(jobOrderId));
    }

    /** GetTotalInPutOutOnPM:834. */
    @GetMapping(API + "/totals")
    @ResponseBody
    public ResponseEntity<?> totals(@RequestParam(defaultValue = "0") int jobOrderId,
                                    @RequestParam(defaultValue = "0") int plantId) {
        return run(() -> service.totals(jobOrderId, plantId));
    }

    /** USP_GetBrandItemsForPackMaterialAgainstPmItem - three callers (:572, :622, :931). */
    @GetMapping(API + "/brand-items")
    @ResponseBody
    public ResponseEntity<?> brandItems(@RequestParam(defaultValue = "0") int jobOrderId,
                                        @RequestParam(defaultValue = "0") int itemId,
                                        @RequestParam(defaultValue = "0") int pmDocId) {
        return run(() -> service.brandItems(jobOrderId, itemId, pmDocId));
    }

    /** racksWithWarehouseAndItems for one item (:715, :749, :799). */
    @GetMapping(API + "/racks")
    @ResponseBody
    public ResponseEntity<?> racks(@RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.racks(itemId));
    }

    /** GetAvgRateQtyAndStockInHand (:1865, :2022). */
    @GetMapping(API + "/avg-rate")
    @ResponseBody
    public ResponseEntity<?> avgRate(@RequestParam(defaultValue = "0") int itemId,
                                     @RequestParam(required = false) String docDate,
                                     @RequestParam(defaultValue = "0") int itemConditionId,
                                     @RequestParam(defaultValue = "0") int recId) {
        return run(() -> one("avgRate", service.avgRate(itemId, docDate, itemConditionId, recId)));
    }

    /** HistoryComboDBCall:2080 (btnRefreshHistory_Click:2139). */
    @GetMapping(API + "/history-job-orders")
    @ResponseBody
    public ResponseEntity<?> historyJobOrders() { return run(service::historyJobOrders); }

    /** BindHistoryPMGrid:1324. */
    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(defaultValue = "false") boolean pendingForRates,
                                     @RequestParam(defaultValue = "doc") String dateMode,
                                     @RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(required = false) String docNoFrom,
                                     @RequestParam(required = false) String docNoTo,
                                     @RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> service.history(pendingForRates, dateMode, fromDate, toDate, docNoFrom, docNoTo, jobOrderId));
    }

    /** btnPackingMaterialSlip_Click:1492 - row count before the viewer opens. */
    @GetMapping(API + "/print-606-rows")
    @ResponseBody
    public ResponseEntity<?> print606Rows(@RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> one("rows", service.report606Rows(jobOrderId)));
    }

    /** ReadByIdPackingMaterial:1253 (+ GetVoucherHeadId(Rec, 111)). */
    @GetMapping(API + "/{id}")
    @ResponseBody
    public ResponseEntity<?> readById(@PathVariable int id) {
        return run(() -> service.readById(id));
    }

    /** PackingMaterialInsert:1096 - Save and Update. */
    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody ProductionPackingMaterialService.SaveRequest req) {
        return run(() -> service.save(req));
    }

    // ------------------------------------------------------------------------------ helpers

    private static Map<String, Object> one(String k, Object v) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put(k, v);
        return m;
    }

    /** A failed lookup is reported with its message, never turned into an empty list. */
    private static ResponseEntity<?> run(Supplier<Object> s) {
        try {
            return ResponseEntity.ok(s.get());
        } catch (Exception e) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("success", false);
            m.put("message", ProductionPackingMaterialService.rootMessage(e));
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(m);
        }
    }
}
