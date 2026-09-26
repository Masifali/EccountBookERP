package com.mst.controllers;

import com.mst.services.SalesAnalyticsDashboardService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Sales Analytics Dashboard -
 * Architecture.WinApp.AnalyticDashboard\SalesAnalyticsDashBoard.cs.
 *
 * Distinct from /dashboard/sales-analytics, which serves
 * Architecture.WinApp.Dashboard\SalesAnalytics.cs (523 lines) - a different form with a different
 * procedure. Both exist on the desktop; both are now served.
 *
 * The last path segment normalises to "salesanalyticsdashboard", which is the TargetUrl type name,
 * so the DashBoard card resolves here through ScreenRouteIndex on its own.
 */
@Controller
public class SalesAnalyticsDashboardController {

    @Autowired
    private SalesAnalyticsDashboardService service;

    @GetMapping("/dashboard/sales-analytics-dashboard")
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Sales Analytics Dashboard");
        return "dashboard/sales_analytics_dashboard";
    }

    /** frmAnalyticsDashboard_Load, :160-192. */
    @GetMapping("/api/dashboard/sales-analytics/setup")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> setup() {
        return ResponseEntity.ok(service.setup());
    }

    /** AllComboBind, :256-320 - the cascade off Parent Category. */
    @GetMapping("/api/dashboard/sales-analytics/combos")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> combos(
            @RequestParam(required = false) String parentCategoryId) {
        return ResponseEntity.ok(service.combos(parentCategoryId));
    }

    /**
     * btnshow_Click -> GetComparisonData, :350-392.
     *
     * One call returns both summaries and both break-up sets, because the desktop reads all four
     * together and the break-up buttons then filter what is already in memory.
     */
    @GetMapping("/api/dashboard/sales-analytics/report")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> report(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(defaultValue = "0") int parentCategoryId,
            @RequestParam(defaultValue = "0") int itemCategoryId,
            @RequestParam(defaultValue = "0") int itemTypeId,
            @RequestParam(defaultValue = "0") int jobLotId,
            @RequestParam(defaultValue = "0") int partyId,
            @RequestParam(defaultValue = "0") int itemId,
            @RequestParam(required = false) String cropYear,
            @RequestParam(required = false) String branchIds) {

        SalesAnalyticsDashboardService.Filters f = new SalesAnalyticsDashboardService.Filters();
        f.fromDate = fromDate;
        f.toDate = toDate;
        f.parentCategoryId = parentCategoryId;
        f.itemCategoryId = itemCategoryId;
        f.itemTypeId = itemTypeId;
        f.jobLotId = jobLotId;
        f.partyId = partyId;
        f.itemId = itemId;
        f.cropYear = cropYear == null ? "" : cropYear;
        f.branchIds = branchIds == null ? "" : branchIds;

        return ResponseEntity.ok(service.report(f));
    }
}
