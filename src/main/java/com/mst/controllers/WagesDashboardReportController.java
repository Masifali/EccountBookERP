package com.mst.controllers;

import com.mst.services.WagesDashboardReportService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Wages Dashboard Report -
 * Architecture.WinApp.Inventory_Reports\frmWagesDashboardReport.cs.
 *
 * The last path segment normalises to "wagesdashboardreport"; the TargetUrl class name
 * normalises to "frmwagesdashboardreport", and ScreenRouteIndex strips a leading "frm" when it
 * matches, so the DashBoard card resolves here. The alternate path is registered as well, so a
 * card that carries the "frm" spelling verbatim still lands on this page rather than on
 * "not built yet".
 */
@Controller
public class WagesDashboardReportController {

    @Autowired
    private WagesDashboardReportService service;

    @GetMapping({ "/dashboard/wages-dashboard-report", "/dashboard/frm-wages-dashboard-report" })
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Wages Dashboard Report");
        return "dashboard/wages_dashboard_report";
    }

    /** frmEvaulationDetailSalesReports_Load, :191-271 - the three dropdowns and the dates. */
    @GetMapping("/api/dashboard/wages-report/setup")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> setup() {
        return ResponseEntity.ok(service.setup());
    }

    /** btnShow_Click -> AllGridFill(), :345-523 - five result sets feeding seven grids. */
    @GetMapping("/api/dashboard/wages-report/report")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> report(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(defaultValue = "0") int contractorId,
            @RequestParam(defaultValue = "0") int wagesAccountId,
            @RequestParam(defaultValue = "0") int refDocumentTypeId) {
        return ResponseEntity.ok(
                service.report(fromDate, toDate, contractorId, wagesAccountId, refDocumentTypeId));
    }
}
