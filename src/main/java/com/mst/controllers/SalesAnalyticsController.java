package com.mst.controllers;

import com.mst.services.SalesAnalyticsService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Sales Analytics - Architecture.WinApp.Dashboard\SalesAnalytics.cs.
 *
 * The path's last segment normalises to "salesanalytics", which is the TargetUrl class name, so
 * the DashBoard card links here on its own.
 *
 * This route previously did NOT exist: the screen was pointed at /business-dashboard by a
 * hand-written entry in DashboardModuleService.WEB_ROUTES. That page renders index.html, a
 * generic KPI page written for this web application - not a port of this form. The entry was
 * removed; this is the real one.
 */
@Controller
public class SalesAnalyticsController {

    @Autowired
    private SalesAnalyticsService service;

    @GetMapping("/dashboard/sales-analytics")
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Sales Analytics Dashboard");
        return "dashboard/sales_analytics";
    }

    /** VoucherValidation_Load, form :95-108. */
    @GetMapping("/api/dashboard/sales-analytics/defaults")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> defaults() {
        return ResponseEntity.ok(service.defaults());
    }

    /** OrganizationTotalSaleComparisonByLocation(), form :130-163. */
    @GetMapping("/api/dashboard/sales-analytics/by-location")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> byLocation(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate) {
        return ResponseEntity.ok(service.byLocation(fromDate, toDate));
    }
}
