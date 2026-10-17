package com.mst.controllers;

import com.mst.services.W4OrderManagementGridsService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Three Order Management dashboards that show the SPU_Invetnory_OrderManagementDashBoards result as grids.
 *
 *   363  /dashboard/order-management                          frmOrderManagement (Purchase / Sale tabs)
 *   366  /dashboard/order-management-dashboard-for-purchase   OrderManagementDashboardForPurchase
 *   367  /dashboard/order-management-dashboard-for-sale       OrderManagementDashboardForSale
 *
 * The last segment of the two dashboard paths normalises to the desktop class name, so the DashBoard
 * module's ScreenRouteIndex can also resolve them without a hand-written entry.
 */
@Controller
public class W4OrderManagementGridsController {

    @Autowired
    private W4OrderManagementGridsService service;

    /** 363 frmOrderManagement - two tabs, "Purchase" and "Sale". */
    @GetMapping("/dashboard/order-management")
    public String frmOrderManagement(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("w4Mode", "tabs");
        model.addAttribute("w4Title", "1003 Order Management (Dashboards)");
        model.addAttribute("w4Heading", "Order Management");
        return "dashboard/w4_order_management_grids";
    }

    /** 366 OrderManagementDashboardForPurchase - label1 "Purchase Order Management Dashboard". */
    @GetMapping("/dashboard/order-management-dashboard-for-purchase")
    public String forPurchase(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("w4Mode", "purchase");
        model.addAttribute("w4Title", "1002 Purchase Order Management Dashboard");
        model.addAttribute("w4Heading", "Purchase Order Management Dashboard");
        return "dashboard/w4_order_management_grids";
    }

    /** 367 OrderManagementDashboardForSale - label1 "Sale Order Management Dashboard". */
    @GetMapping("/dashboard/order-management-dashboard-for-sale")
    public String forSale(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("w4Mode", "sales");
        model.addAttribute("w4Title", "1004 Sale Order Management Dashboard");
        model.addAttribute("w4Heading", "Sale Order Management Dashboard");
        return "dashboard/w4_order_management_grids";
    }

    /** frmOrderManagement_Load / GetDataAndGenerateCards / BtnRefresh_Click. */
    @GetMapping("/api/dashboard/w4-order-management/data")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> data() {
        return ResponseEntity.ok(service.data());
    }
}
