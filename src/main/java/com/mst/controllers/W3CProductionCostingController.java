package com.mst.controllers;

import com.mst.services.W3CProductionCostingService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataAccessException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.sql.SQLException;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.W3CSupport.col;
import static com.mst.services.W3CSupport.rowList;

/**
 * 887 "Production Costing" - Architecture.WinApp.Production/frmCreateProductionCostingReport.cs
 * (DocumentTypeId 921, ScreenName "frmCreateProductionCostingReport"), with its History tab
 * (Architecture.WinApp.Production_Reports/frmProductionCostingReport.cs).
 * Page /production/production-costing, JSON under /api/production/production-costing.
 *
 * No parameter carries tenancy, branch, financial year or user: all are server-derived.
 * Errors: a desktop MessageBox / thrown message -> 400 {success:false,message}; anything else -> 500.
 */
@Controller
public class W3CProductionCostingController {


    @Autowired private W3CProductionCostingService service;

    @GetMapping("/production/production-costing")
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/w3c_production_costing";
    }

    /** InitializeComponentMethod. */
    @GetMapping("/api/production/production-costing/init")
    @ResponseBody
    public ResponseEntity<?> init() {
        try { return ResponseEntity.ok(service.init()); } catch (Exception e) { return error(e); }
    }

    /** btnRefresh_Click:1283. */
    @GetMapping("/api/production/production-costing/refresh")
    @ResponseBody
    public ResponseEntity<?> refresh() {
        try { return ResponseEntity.ok(service.refresh()); } catch (Exception e) { return error(e); }
    }

    /** btnShow_Click:554. */
    @GetMapping("/api/production/production-costing/show")
    @ResponseBody
    public ResponseEntity<?> show(@RequestParam(defaultValue = "0") int jobOrderId) {
        try { return ResponseEntity.ok(service.show(jobOrderId)); } catch (Exception e) { return error(e); }
    }

    /** CmbInvoiceNoDetail_Leave:455. */
    @GetMapping("/api/production/production-costing/invoice-defaults")
    @ResponseBody
    public ResponseEntity<?> invoiceDefaults(@RequestParam(defaultValue = "0") int jobOrderId,
                                             @RequestParam(defaultValue = "0") int invoiceId) {
        try {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("row", service.invoiceDefaults(jobOrderId, invoiceId));
            return ResponseEntity.ok(m);
        } catch (Exception e) { return error(e); }
    }

    /** BtnByProductSettlementPopUp_Click:1830 - the rows of the pop-up. */
    @GetMapping("/api/production/production-costing/by-product-item-wise")
    @ResponseBody
    public ResponseEntity<?> byProductItemWise(@RequestParam(defaultValue = "0") int jobOrderId) {
        try { return ResponseEntity.ok(service.byProductItemWise(jobOrderId)); } catch (Exception e) { return error(e); }
    }

    /** BtnByProductSettlementPopUp_Click:1858 - ProductionByProductRateUpdateByJobOrderId. */
    @PostMapping("/api/production/production-costing/by-product-item-wise")
    @ResponseBody
    public ResponseEntity<?> byProductRates(@RequestBody Map<String, Object> body) {
        try {
            service.byProductRates(intOf(body.get("jobOrderId")), rowList(body.get("rows")));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("result", true);
            return ResponseEntity.ok(m);
        } catch (Exception e) { return error(e); }
    }

    /** Insert():957 - {result:true, id, updated} | 400 {message}. */
    @PostMapping("/api/production/production-costing/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.save(body)); } catch (Exception e) { return error(e); }
    }

    /** frmProductionCostingReport.btnshow_Click. */
    @GetMapping("/api/production/production-costing/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate) {
        try { return ResponseEntity.ok(service.history(fromDate, toDate)); } catch (Exception e) { return error(e); }
    }

    /** OpenPrint:1538 - row count behind the 921 prints. */
    @GetMapping("/api/production/production-costing/print-rows")
    @ResponseBody
    public ResponseEntity<?> printRows(@RequestParam(defaultValue = "0") int jobOrderId) {
        try { return ResponseEntity.ok(service.printRows(jobOrderId)); } catch (Exception e) { return error(e); }
    }

    /* ------------------------------------------------------------------------------ helpers */

    private static ResponseEntity<?> error(Exception e) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        String msg = e.getMessage();
        HttpStatus status = HttpStatus.BAD_REQUEST;
        if (e instanceof DataAccessException) {
            Throwable t = e;
            while (t.getCause() != null && !(t instanceof SQLException)) t = t.getCause();
            msg = t.getMessage();
        } else if (!(e instanceof IllegalArgumentException) && !(e instanceof IllegalStateException)) {
            status = HttpStatus.INTERNAL_SERVER_ERROR;
        }
        m.put("message", msg == null ? e.getClass().getSimpleName() : msg);
        return ResponseEntity.status(status).body(m);
    }

    private static int intOf(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        try { return Integer.parseInt(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }
}
