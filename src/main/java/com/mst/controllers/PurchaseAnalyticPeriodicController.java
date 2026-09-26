package com.mst.controllers;

import com.mst.services.PurchaseAnalyticPeriodicService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Purchase Analytics (Periodic) - Architecture.WinApp.AnalyticDashboard\PurchaseAnalyticPeriodic.cs.
 *
 * The page path is /dashboard/purchase-analytic-periodic on purpose: ScreenRouteIndex normalises a
 * route's last segment to letters and digits and matches it against the screen's name, so
 * "purchase-analytic-periodic" becomes "purchaseanalyticperiodic" and lines up with the
 * TargetUrl class PurchaseAnalyticPeriodic. Both DashBoard entries backed by that form - under
 * Exective DashBoards and under Purchase & Sales Dashboard - therefore start linking here with no
 * hand-written mapping.
 */
@Controller
public class PurchaseAnalyticPeriodicController {

    @Autowired
    private PurchaseAnalyticPeriodicService service;

    @GetMapping("/dashboard/purchase-analytic-periodic")
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Purchase Analytics (Periodic)");
        return "dashboard/purchase_analytic_periodic";
    }

    /** GetSeasonScheduleDates(), form :85-113. Drives the date bounds and whether Show is enabled. */
    @GetMapping("/api/analytics/purchase-periodic/season")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> season() {
        return ResponseEntity.ok(service.seasonSchedule());
    }

    /** btnshow_Click, form :170-252. */
    @GetMapping("/api/analytics/purchase-periodic/report")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> report(
            @RequestParam String fromDate,
            @RequestParam String toDate,
            @RequestParam String seasonStart,
            @RequestParam String seasonEnd) {
        return ResponseEntity.ok(service.report(fromDate, toDate, seasonStart, seasonEnd));
    }

    /** UserControl_Click, form :254-300 - the grid-row drill-down. */
    @GetMapping("/api/analytics/purchase-periodic/party-item")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> partyAndItemWise(
            @RequestParam String fromDate,
            @RequestParam String toDate,
            @RequestParam String seasonStart,
            @RequestParam String seasonEnd,
            @RequestParam(required = false) String ids,
            @RequestParam(defaultValue = "0") int sortNo,
            @RequestParam(defaultValue = "0") int itemId) {
        return ResponseEntity.ok(service.partyAndItemWise(
                fromDate, toDate, seasonStart, seasonEnd, ids, sortNo, itemId));
    }
}
