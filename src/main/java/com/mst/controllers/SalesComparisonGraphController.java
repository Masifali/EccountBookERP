package com.mst.controllers;

import com.mst.services.SalesComparisonGraphService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Sales Comparison (Graph) Report -
 * Architecture.WinApp.Graph\SalesComparisonReportWithGraph.cs.
 *
 * The path's last segment normalises to "salescomparisonreportwithgraph", the TargetUrl class
 * name, so the DashBoard card links here on its own.
 */
@Controller
public class SalesComparisonGraphController {

    @Autowired
    private SalesComparisonGraphService service;

    @GetMapping("/dashboard/sales-comparison-report-with-graph")
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Sales Comparison (Graph) Report");
        return "dashboard/sales_comparison_graph";
    }

    /** Load(), :157-200 - dates, Top rows, and the three lists. */
    @GetMapping("/api/dashboard/sales-comparison/setup")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> setup() {
        return ResponseEntity.ok(service.setup());
    }

    /** ShowReport(), :280-540. */
    @GetMapping("/api/dashboard/sales-comparison/report")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> report(
            @RequestParam String fromDate,
            @RequestParam String toDate,
            @RequestParam(defaultValue = "10") int topRows,
            @RequestParam(required = false) String orderByFlag,
            @RequestParam(defaultValue = "0") int categoryId,
            @RequestParam(defaultValue = "0") int itemTypeId,
            @RequestParam(defaultValue = "0") int costCenterId) {
        return ResponseEntity.ok(service.report(fromDate, toDate, topRows, orderByFlag,
                                                categoryId, itemTypeId, costCenterId));
    }
}
