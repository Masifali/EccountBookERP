package com.mst.controllers;

import com.mst.services.InventoryTasksDashboardService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Purchase Sale Done Work (Dashboard) -
 * Architecture.WinApp.Dashboard\InventoryDashboardTasksInformation.cs.
 *
 * The path's last segment normalises to "inventorydashboardtasksinformation", the TargetUrl
 * class name, so the DashBoard card links here on its own.
 */
@Controller
public class InventoryTasksDashboardController {

    @Autowired
    private InventoryTasksDashboardService service;

    @GetMapping("/dashboard/inventory-dashboard-tasks-information")
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Tasks Done");
        return "dashboard/inventory_dashboard_tasks";
    }

    /** CmbDateType_ValueChanged, form :441-478. */
    @GetMapping("/api/dashboard/inventory-tasks/date-type")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> dateType(
            @RequestParam(defaultValue = "1") int id,
            @RequestParam(required = false) String toDate) {
        return ResponseEntity.ok(service.dateType(id, toDate));
    }

    /** GenerateCards(), form :142-336. */
    @GetMapping("/api/dashboard/inventory-tasks/cards")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> cards(
            @RequestParam String fromDate,
            @RequestParam String toDate) {
        return ResponseEntity.ok(service.cards(fromDate, toDate));
    }
}
