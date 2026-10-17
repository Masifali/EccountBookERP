package com.mst.controllers;

import com.mst.services.Inv19dEvaluationTransactionsReportService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/** EvaluationStockTransactionsReport - ScreenDefinition 815 "Evaluation Transactions Report" (Inventory hub, module 19 Stock Reports). */
@Controller
@RequestMapping("/inventory/reports/evaluation-transactions-report")
public class Inv19dEvaluationTransactionsReportController {

    private final Inv19dEvaluationTransactionsReportService service;
    public Inv19dEvaluationTransactionsReportController(Inv19dEvaluationTransactionsReportService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Evaluation Transactions Report");
        return "inventory/inv19d_evaluation_transactions_report";
    }

    /** PurchaseOrderHistory_Load :141 - StockComboFill, ParameterFill, DocumentTypeFillFromInventoryTransactions, AsOnDate. */
    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() {
        try { return ResponseEntity.ok(service.init()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    /** btnRefresh_Click :735 - StockComboFill + DocumentTypeFillFromInventoryTransactions only. */
    @GetMapping("/api/lookups") @ResponseBody
    public ResponseEntity<?> lookups() {
        try { return ResponseEntity.ok(service.lookups()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    /** btnshow_Click :451 -> InventoryStockEvalautionDetail.EvaluationStockTransactionsReport -> [pcc].[USP-EvaluationStockTransactionsReport]. */
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
