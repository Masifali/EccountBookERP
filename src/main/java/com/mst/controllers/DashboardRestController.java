package com.mst.controllers;

import com.mst.services.DashboardService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

/**
 * REST controller serving dynamic metric reports for executive business dashboards.
 */
@RestController
public class DashboardRestController {

    @Autowired
    private DashboardService dashboardService;

    @PostMapping("/reports/sales_index_report")
    public List<Map<String, Object>> salesIndexReport(@RequestBody(required = false) Map<String, Object> req) {
        return dashboardService.getSalesIndexReport(req != null ? req : Map.of());
    }

    @PostMapping("/reports/sales_order_index_report")
    public List<Map<String, Object>> salesOrderIndexReport(@RequestBody(required = false) Map<String, Object> req) {
        return dashboardService.getSalesOrderIndexReport(req != null ? req : Map.of());
    }

    @PostMapping({"/reports/index_chart_sales1", "/reports/index_chart_sales2", "/reports/index_chart_sales3"})
    public List<Map<String, Object>> salesChartData(@RequestBody(required = false) Map<String, Object> req) {
        return dashboardService.getSalesChartData(1);
    }

    @PostMapping({"/reports/index_chart_profitability1", "/stocks/inventory-profitability-month-chart"})
    public List<Map<String, Object>> profitabilityChartData(@RequestBody(required = false) Map<String, Object> req) {
        return dashboardService.getProfitabilityChartData(1);
    }

    @PostMapping({"/reports/top_product_list1", "/reports/top_product_list2", "/reports/top_product_list3"})
    public List<Map<String, Object>> topProductList(@RequestBody(required = false) Map<String, Object> req) {
        return dashboardService.getTopProducts(1);
    }

    @PostMapping({"/reports/top_customer_list1", "/reports/top_customer_list2", "/reports/top_customer_list3"})
    public List<Map<String, Object>> topCustomerList(@RequestBody(required = false) Map<String, Object> req) {
        return dashboardService.getTopCustomers(1);
    }
}
