package com.mst.controllers;

import com.mst.services.Inv19cStockEvaluationCgsReportService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/** frmStockEvaluationCgsReport - ScreenDefinition 874 "Stock Evaluation Cgs Report" (Inventory hub, Stock Reports). */
@Controller
@RequestMapping("/inventory/reports/stock-evaluation-cgs-report")
public class Inv19cStockEvaluationCgsReportController {

    private final Inv19cStockEvaluationCgsReportService service;
    public Inv19cStockEvaluationCgsReportController(Inv19cStockEvaluationCgsReportService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Stock Evaluation Cgs Report");
        return "inventory/stock_evaluation_cgs_report";
    }

    /** InitializeComponentMethod :203 - combos, dynamic report list, financial-year start. */
    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() {
        try { return ResponseEntity.ok(service.init()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    /** btnRefresh_Click :907 - AllComboBind(ComboData()). */
    @GetMapping("/api/combos") @ResponseBody
    public ResponseEntity<?> combos() {
        try { return ResponseEntity.ok(service.combos()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    /** ReportsLoad :259 - tsDrowdown items. */
    @GetMapping("/api/dynamic-reports") @ResponseBody
    public ResponseEntity<?> dynamicReports() {
        try { return ResponseEntity.ok(service.dynamicReports()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    /** GridFill :483. */
    @PostMapping("/api/show") @ResponseBody
    public ResponseEntity<?> show(@RequestBody Map<String, Object> body) {
        try { return ResponseEntity.ok(service.show(body)); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    private static ResponseEntity<Map<String, Object>> fail(HttpStatus status, String message) {
        Map<String, Object> m = new LinkedHashMap<>(); m.put("success", false); m.put("message", message);
        return ResponseEntity.status(status).body(m);
    }
    private static String msg(Exception e) {
        Throwable t = e; while (t.getCause() != null && (t.getMessage() == null || t.getMessage().isBlank())) t = t.getCause();
        return t.getMessage() == null || t.getMessage().isBlank() ? "Request failed." : t.getMessage();
    }
}
