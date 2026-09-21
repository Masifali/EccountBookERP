package com.mst.controllers;

import com.mst.services.StockBreakupInAndOutSummaryService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Stock Breakup In &amp; Out Summary - Architecture.WinApp.Audit_Dashboard\StockBreakupInAndOutSummary.cs.
 * The only screen in the Audit Dashboard module.
 *
 * The path's last segment normalises to "stockbreakupinandoutsummary", which is the TargetUrl
 * class name, so ScreenRouteIndex links the DashBoard card to it with no hand-written mapping.
 */
@Controller
public class StockBreakupInAndOutSummaryController {

    @Autowired
    private StockBreakupInAndOutSummaryService service;

    @GetMapping("/dashboard/stock-breakup-in-and-out-summary")
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Stock Breakup In & Out Summary");
        return "dashboard/stock_breakup_in_and_out_summary";
    }

    /** Load()/Reset() - the financial year's start and today (:78-105). */
    @GetMapping("/api/audit/stock-breakup/defaults")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> defaults() {
        return ResponseEntity.ok(service.defaults());
    }

    /**
     * GridFill(), form :107-155. fromDate is left blank when the From box is unticked, and the
     * parameter is then omitted entirely, exactly as the BLL omits it.
     */
    @GetMapping("/api/audit/stock-breakup/report")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> report(
            @RequestParam(required = false) String fromDate,
            @RequestParam String toDate,
            @RequestParam(defaultValue = "false") boolean customRate) {
        return ResponseEntity.ok(service.report(fromDate, toDate, customRate));
    }
}
