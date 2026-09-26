package com.mst.controllers;

import com.mst.services.ProductionOutputAllocationService;
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

import javax.servlet.http.HttpSession;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * "Production Output Allocation With Export Invoice" -
 * Architecture.WinApp.Production/ProductionOutputAllocationWithExportInvoice.cs.
 *
 * Desktop entry: frmProductionSettlement.btnAllocateExportInvoiceToJobOrder_Click:2206
 * `new ProductionOutputAllocationWithExportInvoice(UserAccount).Show()` - its own window, no
 * context passed; the Settlement tab opens this page in a new window. It also works on its own.
 *
 * No parameter carries tenancy, branch, financial year or user: all are server-derived.
 */
@Controller
public class ProductionOutputAllocationController {

    private static final String API = "/api/production/output-allocation-export-invoice";

    @Autowired private ProductionOutputAllocationService service;

    @GetMapping("/production/output-allocation-export-invoice")
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/output_allocation_export_invoice";
    }

    /** InitializeComponentMethod:158. */
    @GetMapping(API + "/load")
    @ResponseBody
    public ResponseEntity<?> load() {
        try { return ResponseEntity.ok(service.load()); } catch (Exception e) { return error(e); }
    }

    /** btnRefreshHistory_Click:993 - bindHistoryCombo(GetHistoryComboData()). */
    @GetMapping(API + "/history-job-orders")
    @ResponseBody
    public ResponseEntity<?> historyJobOrders() {
        try { return ResponseEntity.ok(service.historyJobOrders()); } catch (Exception e) { return error(e); }
    }

    /** CmbJobOrder_Leave:622 -> GetDataAgainstJobOrderFromProduction. */
    @GetMapping(API + "/output-items")
    @ResponseBody
    public ResponseEntity<?> outputItems(@RequestParam(defaultValue = "0") int jobOrderId) {
        try { return ResponseEntity.ok(service.outputItems(jobOrderId)); } catch (Exception e) { return error(e); }
    }

    /** ReadById:948 (history Edit button / Ctrl+Enter). */
    @GetMapping(API + "/read-by-id")
    @ResponseBody
    public ResponseEntity<?> readById(@RequestParam(defaultValue = "0") int jobOrderId, HttpSession session) {
        try { return ResponseEntity.ok(service.readById(jobOrderId, session)); } catch (Exception e) { return error(e); }
    }

    /** Insert():304 - {result:true, updated} | 400 {message}. */
    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody Map<String, Object> body, HttpSession session) {
        try {
            boolean updated = service.save(intOf(body.get("recId")), intOf(body.get("jobOrderId")),
                    rowsOf(body.get("rows")), rowsOf(body.get("removed")), session);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("result", true);
            m.put("updated", updated);
            return ResponseEntity.ok(m);
        } catch (Exception e) { return error(e); }
    }

    /** GetHistoryData:768. */
    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(defaultValue = "0") int jobOrderId) {
        try { return ResponseEntity.ok(service.history(fromDate, toDate, jobOrderId)); } catch (Exception e) { return error(e); }
    }

    /** Slip:1018 - row count behind Print-627. */
    @GetMapping(API + "/print-rows")
    @ResponseBody
    public ResponseEntity<?> printRows(@RequestParam(defaultValue = "0") int jobOrderId) {
        try {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("rows", service.slipRowCount(jobOrderId));
            return ResponseEntity.ok(m);
        } catch (Exception e) { return error(e); }
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

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> rowsOf(Object o) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (o instanceof List) {
            for (Object r : (List<?>) o) if (r instanceof Map) out.add((Map<String, Object>) r);
        }
        return out;
    }
}
