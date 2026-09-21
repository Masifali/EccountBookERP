package com.mst.controllers;

import com.mst.services.StockDashboardService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Stock DashBoard - Architecture.WinApp.Dashboard\frmStockDashboard.cs.
 *
 * The last path segment normalises to "stockdashboard"; the TargetUrl "frmStockDashboard"
 * normalises the same way once ScreenRouteIndex strips the leading "frm", so the DashBoard card
 * resolves here on its own. The "frm" spelling is registered too, so a card carrying it verbatim
 * still lands on this page rather than on "not built yet".
 */
@Controller
public class StockDashboardController {

    @Autowired
    private StockDashboardService service;

    @GetMapping({ "/dashboard/stock-dashboard", "/dashboard/frm-stock-dashboard" })
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Stock DashBoard");
        return "dashboard/stock_dashboard";
    }

    /** frmPendingWorksRpt_Load, :379-393 - the branch feature, the branches and the dates. */
    @GetMapping("/api/dashboard/stock/setup")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> setup() {
        return ResponseEntity.ok(service.setup());
    }

    /** btnsearch_Click -> GridBind(), :422-708 - one read, six totals panels. */
    @GetMapping("/api/dashboard/stock/report")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> report(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(defaultValue = "false") boolean saleValue,
            @RequestParam(required = false) String branchIds) {
        return ResponseEntity.ok(service.report(fromDate, toDate, saleValue, branchIds));
    }
}
