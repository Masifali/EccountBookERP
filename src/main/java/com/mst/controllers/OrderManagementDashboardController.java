package com.mst.controllers;

import com.mst.services.OrderManagementDashboardService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * 1001 Order Management Dashboard -
 * Architecture.WinApp.Dashboard\OrderManagementDashboard.cs.
 *
 * The path's last segment normalises to "ordermanagementdashboard", which is the TargetUrl class
 * name, so the DashBoard card links here with no hand-written mapping.
 */
@Controller
public class OrderManagementDashboardController {

    @Autowired
    private OrderManagementDashboardService service;

    @GetMapping("/dashboard/order-management-dashboard")
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "1001 Order Management Dashboard");
        return "dashboard/order_management_dashboard";
    }

    /** GetDataAndGenerateCards(), form :65-128. */
    @GetMapping("/api/dashboard/order-management/cards")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> cards(
            @RequestParam(required = false) String toDate) {
        return ResponseEntity.ok(service.cards(toDate));
    }
}
