package com.mst.controllers;

import com.mst.services.Inv19dStockReportWithValuesEngrService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;

/** frmStockReportWithValues_Engr - ScreenDefinition 805 "Stock Report With Values (Engr)" (Inventory hub, module 19 Stock Reports). */
@Controller
@RequestMapping("/inventory/reports/stock-report-with-values-engr")
public class Inv19dStockReportWithValuesEngrController {

    private final Inv19dStockReportWithValuesEngrService service;
    public Inv19dStockReportWithValuesEngrController(Inv19dStockReportWithValuesEngrService service) { this.service = service; }

    @GetMapping
    public String page(Model model) {
        model.addAttribute("activeMenu", "inventory");
        model.addAttribute("moduleTitle", "Stock Report With Values (Engr)");
        return "inventory/inv19d_stock_report_with_values_engr";
    }

    /** frmStockReport_Load :174 - branches, parent-category combos for the user's branch, financial-year start. */
    @GetMapping("/api/init") @ResponseBody
    public ResponseEntity<?> init() {
        try { return ResponseEntity.ok(service.init()); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    /** ParentCategoryComboFill :291 / cmbBranchName_Leave :518 - the combo rows for the checked branches. */
    @GetMapping("/api/combos") @ResponseBody
    public ResponseEntity<?> combos(@RequestParam(value = "branchesIds", required = false) String branchesIds) {
        try { return ResponseEntity.ok(service.combos(branchesIds)); } catch (RuntimeException e) { return fail(HttpStatus.BAD_REQUEST, msg(e)); }
    }

    /** GridBind :662 -> StocksReport.StockReportWithValues_Engr -> [Mfg].[usp_ItemStockReportWithValues]. */
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
