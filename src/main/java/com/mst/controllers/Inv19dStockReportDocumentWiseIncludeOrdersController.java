package com.mst.controllers;

import com.mst.services.Inv19dStockReportDocumentWiseIncludeOrdersService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/** StockReportWithDocumentWiseIncludeOrders - ScreenDefinition 843 "Stock Report With Document Wise Include Orders" (Inventory hub, module 19 Stock Reports). */
@Controller
@RequestMapping("/inventory/reports/stock-report-document-wise-include-orders")
public class Inv19dStockReportDocumentWiseIncludeOrdersController {

    private final Inv19dStockReportDocumentWiseIncludeOrdersService service;
    public Inv19dStockReportDocumentWiseIncludeOrdersController(Inv19dStockReportDocumentWiseIncludeOrdersService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Stock Report With Document Wise Include Orders");
        return "inventory/inv19d_stock_report_document_wise_include_orders";
    }

    /** InventoryStockEvaluation_GenrateStocks_Load :101 - StockComboFill + Datetypefill (+ FY start for date type 5). */
    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() {
        try { return ResponseEntity.ok(service.init()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    /** btnRefresh_Click :402 - StockComboFill only. */
    @GetMapping("/api/combos") @ResponseBody
    public ResponseEntity<?> combos() {
        try { return ResponseEntity.ok(service.combos()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    /** GridBind :246 -> StocksReport.StockReportWithDocumentWiseIncludeOrders -> [pcc].[USP_StockReportWithDocumentWiseIncludeOrders]. */
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
