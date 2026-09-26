package com.mst.controllers;

import com.mst.services.ProductionOverheadService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataAccessException;
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

import java.time.LocalDate;
import java.util.LinkedHashMap;
import java.util.Map;
import java.util.function.Supplier;

/**
 * 280 Production Against (Job Order) - the Overhead tab, frmProductionOverhead.cs.
 *
 * Framed by the shell (ProductionAgainstJobOrderService.tabs) into its own iframe, exactly as the
 * desktop drops a separate form into PanelOtherForms. The page also works standalone.
 *
 * No parameter here carries tenancy, the financial year, the branch or a user id; all of them are
 * server-derived in the service.
 */
@Controller
public class ProductionOverheadController {

    private static final String API = "/api/production/production-against-job-order/overhead";

    @Autowired
    private ProductionOverheadService service;

    @GetMapping("/production/production-against-job-order/overhead")
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/p280_overhead";
    }

    /** frmFoodProduction_Load:216 - rights, doc no, and the four lists Load binds. */
    @GetMapping(API + "/state")
    @ResponseBody
    public ResponseEntity<?> state() { return run(service::state); }

    /** GenerateCodeofOverHead:536. */
    @GetMapping(API + "/next-code")
    @ResponseBody
    public ResponseEntity<?> nextCode() {
        return run(() -> {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("docNo", service.generateCode());
            return m;
        });
    }

    /** btnRefreshOverHead_Click:1011 - UOM, charges type, job orders, chart of accounts again. */
    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() {
        return run(() -> {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("rateUoms", service.rateUoms());
            m.put("chargesTypes", service.chargesTypes());
            m.put("jobOrders", service.jobOrders());
            m.put("chartOfAccounts", service.chartOfAccounts());
            return m;
        });
    }

    /** GetPlantFeeder:385. */
    @GetMapping(API + "/plants")
    @ResponseBody
    public ResponseEntity<?> plants(@RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> service.plants(jobOrderId));
    }

    /** GetBrandItemsForPackingAndOverHeads:281 - documentTypeId 80 (Input) or 112 (Output). */
    @GetMapping(API + "/brand-items")
    @ResponseBody
    public ResponseEntity<?> brandItems(@RequestParam(defaultValue = "0") int jobOrderId,
                                        @RequestParam(defaultValue = "0") int documentTypeId) {
        return run(() -> service.brandItems(jobOrderId, documentTypeId));
    }

    /** GetItemUomByItemIdFromFinishGoods:311. */
    @GetMapping(API + "/brand-uoms")
    @ResponseBody
    public ResponseEntity<?> brandUoms(@RequestParam(defaultValue = "0") int jobOrderId,
                                       @RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.brandUoms(jobOrderId, itemId));
    }

    /** GetTotlInPutOut:658. */
    @GetMapping(API + "/totals")
    @ResponseBody
    public ResponseEntity<?> totals(@RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> service.inputOutputTotals(jobOrderId));
    }

    /** GetOutPutQtyByJobOrderandItemIdOverHeads:736. */
    @GetMapping(API + "/output-qty")
    @ResponseBody
    public ResponseEntity<?> outputQty(@RequestParam(defaultValue = "0") int jobOrderId,
                                       @RequestParam(defaultValue = "0") int brandId,
                                       @RequestParam(defaultValue = "0") int brandUomId,
                                       @RequestParam(defaultValue = "0") int documentTypeId) {
        return run(() -> service.outputQty(jobOrderId, brandId, brandUomId, documentTypeId));
    }

    /** BtnLoadFohOverHeads_Click:1754 - the rows for the GrdPopUpToReturndt dialog. */
    @GetMapping(API + "/foh-expenses")
    @ResponseBody
    public ResponseEntity<?> fohExpenses(@RequestParam(defaultValue = "0") int jobOrderId,
                                         @RequestParam(defaultValue = "0") int plantId,
                                         @RequestParam String docDate) {
        return run(() -> service.fohExpenses(jobOrderId, plantId, LocalDate.parse(docDate)));
    }

    /** ReadByIdOverHeadJobOrderWise:944 - also what P280ReadById loads. */
    @GetMapping(API + "/by-job-order/{jobOrderId}")
    @ResponseBody
    public ResponseEntity<?> byJobOrder(@PathVariable int jobOrderId) {
        return run(() -> service.byJobOrder(jobOrderId));
    }

    /** BindGridHistoryOverHead:1326. */
    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history() { return run(service::history); }

    /** grdOverHeadHistory_SelectionChanged:1468. */
    @GetMapping(API + "/history-detail")
    @ResponseBody
    public ResponseEntity<?> historyDetail(@RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> service.historyDetail(jobOrderId));
    }

    /** btnPrintOverHead_Click:1583 - how many rows 606 would print. */
    @GetMapping(API + "/print-606-rows")
    @ResponseBody
    public ResponseEntity<?> print606Rows(@RequestParam(defaultValue = "0") int jobOrderId) {
        return run(() -> {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("rows", service.report606Rows(jobOrderId));
            return m;
        });
    }

    /** btnSaveOverHead_Click:919 / btnUpdateOverHead_Click:932 -> OverHeadInsert:779. */
    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) {
        Map<String, Object> r = new LinkedHashMap<>();
        try {
            r.put("success", true);
            r.put("message", service.save(body));
            return ResponseEntity.ok(r);
        } catch (ProductionOverheadService.OverheadValidationException e) {
            r.put("success", false);
            r.put("message", e.getMessage());
            return ResponseEntity.badRequest().body(r);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(e));
        }
    }

    // ------------------------------------------------------------------------------ helpers

    private static ResponseEntity<?> run(Supplier<Object> s) {
        try {
            return ResponseEntity.ok(s.get());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(e));
        }
    }

    /**
     * The desktop shows ex.Message. For a SQL error that is the RAISERROR text, which Spring wraps;
     * the most specific cause carries it unchanged.
     */
    private static Map<String, Object> fail(Exception e) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        String msg = e.getMessage();
        if (e instanceof DataAccessException && ((DataAccessException) e).getMostSpecificCause() != null) {
            msg = ((DataAccessException) e).getMostSpecificCause().getMessage();
        }
        m.put("message", msg == null ? e.getClass().getSimpleName() : msg);
        return m;
    }
}
